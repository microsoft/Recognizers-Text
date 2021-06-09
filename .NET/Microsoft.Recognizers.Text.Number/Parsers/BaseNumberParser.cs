using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.Number
{
    public class BaseNumberParser : IParser
    {
        private static readonly Regex LongFormRegex =
            new Regex(@"\d+", RegexOptions.Singleline | RegexOptions.Compiled);

        private readonly bool isMultiDecimalSeparatorCulture = false;

        private readonly bool isNonStandardSeparatorVariant = false;

        private readonly bool isCompoundNumberLanguage = false;

        public BaseNumberParser(INumberParserConfiguration config)
        {
            this.Config = config;

            this.isMultiDecimalSeparatorCulture = config.IsMultiDecimalSeparatorCulture;
            this.isCompoundNumberLanguage = config.IsCompoundNumberLanguage;

            TextNumberRegex = BuildTextNumberRegex();

            RoundNumberSet = new HashSet<string>();
            foreach (var roundNumber in this.Config.RoundNumberMap.Keys)
            {
                RoundNumberSet.Add(roundNumber);
            }

            isNonStandardSeparatorVariant = Config.NonStandardSeparatorVariants.Contains(Config.CultureInfo.Name.ToLowerInvariant());
        }

        internal IEnumerable<string> SupportedTypes { get; set; }

        protected static Regex LongFormatRegex => LongFormRegex;

        protected INumberParserConfiguration Config { get; private set; }

        protected Regex TextNumberRegex { get; }

        protected HashSet<string> RoundNumberSet { get; }

        public virtual ParseResult Parse(ExtractResult extResult)
        {
            // Check if the parser is configured to support specific types
            if (SupportedTypes != null && !SupportedTypes.Any(t => extResult.Type.Equals(t, StringComparison.Ordinal)))
            {
                return null;
            }

            ParseResult ret = null;

            if (!(extResult.Data is string extra))
            {
                extra = LongFormatRegex.Match(extResult.Text).Success ? Constants.NUMBER_SUFFIX : Config.LanguageMarker;
            }

            // Resolve symbol prefix
            var isNegative = false;
            var matchNegative = Config.NegativeNumberSignRegex.Match(extResult.Text);

            if (matchNegative.Success)
            {
                isNegative = true;
                extResult.Text = extResult.Text.Substring(matchNegative.Groups["negTerm"].Length);
            }

            // Assign resolution value
            if (extResult.Data is List<ExtractResult> ers)
            {
                var innerPrs = ers.Select(Parse).ToList();
                var mergedPrs = new List<ParseResult>();

                double val = 0;
                var count = 0;

                for (var idx = 0; idx < innerPrs.Count; idx++)
                {
                    val += (double)innerPrs[idx].Value;

                    if (idx + 1 >= innerPrs.Count || !IsMergeable((double)innerPrs[idx].Value, (double)innerPrs[idx + 1].Value))
                    {
                        var start = (int)ers[idx - count].Start;
                        var length = (int)(ers[idx].Start + ers[idx].Length - start);
                        mergedPrs.Add(new ParseResult
                        {
                            Start = start,
                            Length = length,
                            Text = extResult.Text.Substring((int)(start - extResult.Start), length),
                            Type = extResult.Type,
                            Value = val,
                            Data = null,
                        });

                        val = 0;
                        count = 0;
                    }
                    else
                    {
                        count++;
                    }
                }

                ret = new ParseResult(extResult) { Value = val, Data = mergedPrs };
            }
            else if (extra.Contains(Constants.NUMBER_SUFFIX))
            {
                ret = DigitNumberParse(extResult);
            }
            else if (extra.Contains($"{Constants.FRACTION_PREFIX}{Config.LanguageMarker}"))
            {
                // Such fractions are special cases, parse via another method
                ret = FracLikeNumberParse(extResult);
            }
            else if (extra.Contains(Config.LanguageMarker))
            {
                ret = TextNumberParse(extResult);
            }
            else if (extra.Contains(Constants.POWER_SUFFIX))
            {
                ret = PowerNumberParse(extResult);
            }

            if (ret?.Data is List<ParseResult> prs)
            {
                foreach (var parseResult in prs)
                {
                    parseResult.ResolutionStr = GetResolutionStr(parseResult.Value);
                }
            }
            else if (ret?.Value != null)
            {

                if (isNegative)
                {
                    // Recover the original extracted Text
                    ret.Text = matchNegative.Groups["negTerm"].Value + extResult.Text;
                    ret.Value = -(double)ret.Value;
                }

                ret.ResolutionStr = GetResolutionStr(ret.Value);
            }

            // Add "offset" and "relativeTo" for ordinal
            if (!string.IsNullOrEmpty(ret.Type) && ret.Type.Contains(Constants.MODEL_ORDINAL))
            {
                if ((this.Config.Config.Options & NumberOptions.SuppressExtendedTypes) == 0 && ret.Metadata.IsOrdinalRelative)
                {
                    var offset = Config.RelativeReferenceOffsetMap[extResult.Text];
                    var relativeTo = Config.RelativeReferenceRelativeToMap[extResult.Text];

                    ret.Metadata.Offset = offset;
                    ret.Metadata.RelativeTo = relativeTo;

                    // Add value for ordinal.relative
                    string sign = offset[0].Equals('-') ? string.Empty : "+";
                    ret.Value = string.Concat(relativeTo, sign, offset);
                    ret.ResolutionStr = GetResolutionStr(ret.Value);
                }
                else
                {
                    ret.Metadata.Offset = ret.ResolutionStr;

                    // Every ordinal number is relative to the start
                    ret.Metadata.RelativeTo = Constants.RELATIVE_START;
                }
            }

            if (ret != null)
            {
                ret.Type = DetermineType(extResult, ret);
                ret.Text = ret.Text.ToLowerInvariant();
            }

            return ret;
        }

        public virtual ParseResult PowerNumberParse(ExtractResult extResult)
        {
            var result = new ParseResult
            {
                Start = extResult.Start,
                Length = extResult.Length,
                Text = extResult.Text,
                Type = extResult.Type,
            };

            var handle = extResult.Text.ToUpperInvariant();
            var isE = !extResult.Text.Contains("^");

            // [1] 1e10
            // [2] 1.1^-23
            var calStack = new Queue<double>();

            double scale = 10;
            var decimalSeparatorFound = false;
            var isNegative = false;
            double tmp = 0;
            for (var i = 0; i < handle.Length; i++)
            {
                var ch = handle[i];
                if (ch == '^' || ch == 'E')
                {
                    if (isNegative)
                    {
                        calStack.Enqueue(-tmp);
                    }
                    else
                    {
                        calStack.Enqueue(tmp);
                    }

                    tmp = 0;
                    scale = 10;
                    decimalSeparatorFound = false;
                    isNegative = false;
                }
                else if (ch >= '0' && ch <= '9')
                {
                    if (decimalSeparatorFound)
                    {
                        tmp += scale * (ch - '0');
                        scale *= 0.1;
                    }
                    else
                    {
                        tmp = (tmp * scale) + (ch - '0');
                    }
                }
                else if (ch == Config.DecimalSeparatorChar)
                {
                    decimalSeparatorFound = true;
                    scale = 0.1;
                }
                else if (ch == '-')
                {
                    isNegative = !isNegative;
                }
                else if (ch == '+')
                {
                    continue;
                }

                if (i == handle.Length - 1)
                {
                    if (isNegative)
                    {
                        calStack.Enqueue(-tmp);
                    }
                    else
                    {
                        calStack.Enqueue(tmp);
                    }
                }
            }

            double ret;
            if (isE)
            {
                ret = calStack.Dequeue() * Math.Pow(10, calStack.Dequeue());
            }
            else
            {
                ret = Math.Pow(calStack.Dequeue(), calStack.Dequeue());
            }

            result.Value = ret;
            result.ResolutionStr = ret.ToString("G15", CultureInfo.InvariantCulture);

            return result;
        }

        public virtual ParseResult TextNumberParse(ExtractResult extResult)
        {
            var result = new ParseResult
            {
                Start = extResult.Start,
                Length = extResult.Length,
                Text = extResult.Text,
                Type = extResult.Type,
                Metadata = extResult.Metadata,
            };

            var handle = extResult.Text;

            handle = Config.HalfADozenRegex.Replace(handle, Config.HalfADozenText);

            // Handling cases like "last", "next one", "previous one"
            if ((this.Config.Config.Options & NumberOptions.SuppressExtendedTypes) == 0)
            {
                if (extResult.Metadata != null && extResult.Metadata.IsOrdinalRelative)
                {
                    return result;
                }
            }

            var numGroup = handle.Split(Config.WrittenDecimalSeparatorTexts.ToArray(), StringSplitOptions.RemoveEmptyEntries);

            var intPart = numGroup[0];
            var stringMatch = TextNumberRegex.Match(intPart);

            // Store all match str.
            var matchStrs = new List<string>();

            while (stringMatch.Success)
            {
                var matchStr = stringMatch.Groups[0].Value;
                matchStrs.Add(matchStr);
                stringMatch = stringMatch.NextMatch();
            }

            // Get the value recursively
            var intPartRet = GetIntValue(matchStrs);

            double pointPartRet = 0;
            if (numGroup.Length == 2)
            {
                var pointPart = numGroup[1];
                stringMatch = TextNumberRegex.Match(pointPart);
                matchStrs.Clear();

                while (stringMatch.Success)
                {
                    var matchStr = stringMatch.Groups[0].Value;
                    matchStrs.Add(matchStr);
                    stringMatch = stringMatch.NextMatch();
                }

                pointPartRet += GetPointValue(matchStrs);
            }

            result.Value = intPartRet + pointPartRet;

            return result;
        }

        public virtual ParseResult FracLikeNumberParse(ExtractResult extResult)
        {
            var result = new ParseResult
            {
                Start = extResult.Start,
                Length = extResult.Length,
                Text = extResult.Text,
                Type = extResult.Type,
            };

            var resultText = extResult.Text;
            if (Config.FractionPrepositionRegex.IsMatch(resultText))
            {
                var match = Config.FractionPrepositionRegex.Match(resultText);
                var numerator = match.Groups["numerator"].Value;
                var denominator = match.Groups["denominator"].Value;

                var smallValue = char.IsDigit(numerator[0]) ?
                                 GetDigitalValue(numerator, 1) :
                                 GetIntValue(Utilities.RegExpUtility.GetMatches(this.TextNumberRegex, numerator));

                var bigValue = char.IsDigit(denominator[0]) ?
                               GetDigitalValue(denominator, 1) :
                               GetIntValue(Utilities.RegExpUtility.GetMatches(this.TextNumberRegex, denominator));

                result.Value = smallValue / bigValue;
            }
            else
            {
                long multiplier = 1;
                if (Config.RoundMultiplierRegex != null)
                {
                    var match = Config.RoundMultiplierRegex.Match(resultText);
                    if (match.Success)
                    {
                        resultText = resultText.Replace(match.Value, string.Empty);
                        multiplier = Config.RoundNumberMap[match.Groups["multiplier"].Value];
                    }
                }

                var fracWords = Config.NormalizeTokenSet(resultText.Split(null), result).ToList();

                // Split fraction with integer
                var splitIndex = fracWords.Count - 1;
                var currentValue = Config.ResolveCompositeNumber(fracWords[splitIndex]);
                long roundValue = 1;

                // For case like "half"
                if (fracWords.Count == 1)
                {
                   result.Value = (1 / GetIntValue(fracWords)) * multiplier;
                   return result;
                }

                for (splitIndex = fracWords.Count - 2; splitIndex >= 0; splitIndex--)
                {
                    if (Config.WrittenFractionSeparatorTexts.Contains(fracWords[splitIndex]) ||
                        Config.WrittenIntegerSeparatorTexts.Contains(fracWords[splitIndex]))
                    {
                        continue;
                    }

                    var previousValue = currentValue;
                    currentValue = Config.ResolveCompositeNumber(fracWords[splitIndex]);

                    var hundredsSM = 100;

                    // Previous : hundred
                    // Current : one
                    if ((previousValue >= hundredsSM && previousValue > currentValue) ||
                        (previousValue < hundredsSM && IsComposable(currentValue, previousValue)))
                    {
                        if (previousValue < hundredsSM && currentValue >= roundValue)
                        {
                            roundValue = currentValue;
                        }
                        else if (previousValue < hundredsSM && currentValue < roundValue)
                        {
                            splitIndex++;
                            break;
                        }

                        // Current is the first word
                        if (splitIndex == 0)
                        {
                            // Scan, skip the first word
                            splitIndex = 1;
                            while (splitIndex <= fracWords.Count - 2)
                            {
                                // e.g. one hundred thousand
                                // frac[i+1] % 100 && frac[i] % 100 = 0
                                if (Config.ResolveCompositeNumber(fracWords[splitIndex]) >= hundredsSM &&
                                    !Config.WrittenFractionSeparatorTexts.Contains(fracWords[splitIndex + 1]) &&
                                    Config.ResolveCompositeNumber(fracWords[splitIndex + 1]) < hundredsSM)
                                {
                                    splitIndex++;
                                    break;
                                }

                                splitIndex++;
                            }

                            break;
                        }

                        continue;
                    }

                    splitIndex++;
                    break;
                }

                if (splitIndex < 0)
                {
                    splitIndex = 0;
                }

                var fracPart = new List<string>();
                for (var i = splitIndex; i < fracWords.Count; i++)
                {
                    if (fracWords[i].Contains("-"))
                    {
                        var split = fracWords[i].Split('-');
                        fracPart.Add(split[0]);
                        fracPart.Add("-");
                        fracPart.Add(split[1]);
                    }
                    else
                    {
                        fracPart.Add(fracWords[i]);
                    }
                }

                fracWords.RemoveRange(splitIndex, fracWords.Count - splitIndex);

                // Split mixed number with fraction
                var denominator = GetIntValue(fracPart);
                double numerValue = 0;
                double intValue = 0;

                var mixedIndex = fracWords.Count;
                for (var i = fracWords.Count - 1; i >= 0; i--)
                {
                    if (i < fracWords.Count - 1 && Config.WrittenFractionSeparatorTexts.Contains(fracWords[i]))
                    {
                        var numerStr = string.Join(" ", fracWords.GetRange(i + 1, fracWords.Count - 1 - i));
                        numerValue = GetIntValue(Utilities.RegExpUtility.GetMatches(this.TextNumberRegex, numerStr));
                        mixedIndex = i + 1;
                        break;
                    }
                }

                var intStr = string.Join(" ", fracWords.GetRange(0, mixedIndex));
                intValue = GetIntValue(Utilities.RegExpUtility.GetMatches(this.TextNumberRegex, intStr));

                // Find mixed number
                if (mixedIndex != fracWords.Count && numerValue < denominator)
                {
                    result.Value = intValue + (multiplier * numerValue / denominator);
                }
                else
                {
                    result.Value = multiplier * (intValue + numerValue) / denominator;
                }
            }

            return result;
        }

        /// <summary>
        /// Precondition: ExtResult must have arabic numerals.
        /// </summary>
        /// <param name="extResult">input arabic number.</param>
        /// <returns>parsed result.</returns>
        public virtual ParseResult DigitNumberParse(ExtractResult extResult)
        {
            var result = new ParseResult
            {
                Start = extResult.Start,
                Length = extResult.Length,
                Text = extResult.Text,
                Type = extResult.Type,
                Metadata = extResult.Metadata != null ? extResult.Metadata : new Metadata(),
            };

            // [1] 24
            // [2] 12 32/33
            // [3] 1,000,000
            // [4] 234.567
            // [5] 44/55
            // [6] 2 hundred
            // dot occured.
            double power = 1;
            var extText = extResult.Text.ToLowerInvariant();
            int startIndex = 0;

            var match = Config.DigitalNumberRegex.Match(extText);

            while (match.Success)
            {
                var tmpIndex = -1;
                double rep = Config.RoundNumberMap[match.Value];

                // \\s+ to filter whitespaces.
                power *= rep;

                while ((tmpIndex = extText.IndexOf(match.Value, startIndex, StringComparison.Ordinal)) >= 0)
                {
                    var front = extText.Substring(0, tmpIndex).TrimEnd();
                    startIndex = front.Length;
                    extText = front + extText.Substring(tmpIndex + match.Value.Length);
                }

                match = match.NextMatch();
            }

            // Scale used in calculating double
            var value = GetDigitalValue(extText, power);
            result.Value = value;
            result.Metadata.TreatAsInteger = (value % 1) == 0;

            return result;
        }

        public virtual double GetDigitalValue(string digitsStr, double power)
        {
            double temp = 0;
            double scale = 10;
            var hasDecimalSeparator = false;
            var isNegative = false;

            var strLength = digitsStr.Length;
            var isFrac = digitsStr.Contains('/');

            // As some languages use different separators depending on variant, some pre-processing is required to allow for unified processing.

            // Default separators from general language config
            var decimalSeparator = Config.DecimalSeparatorChar;
            var nonDecimalSeparator = Config.NonDecimalSeparatorChar;

            var lastDecimalSeparator = -1;
            var lastNonDecimalSeparator = -1;
            var firstNonDecimalSeparator = int.MaxValue;
            var hasSingleSeparator = false;

            if (Config.IsMultiDecimalSeparatorCulture)
            {

                if (isNonStandardSeparatorVariant)
                {
                    // Reverse separators
                    decimalSeparator = Config.NonDecimalSeparatorChar;
                    nonDecimalSeparator = Config.DecimalSeparatorChar;
                }

                for (int i = 0; i < strLength; i++)
                {
                    var ch = digitsStr[i];
                    if (ch == decimalSeparator)
                    {
                        lastDecimalSeparator = i;
                    }
                    else if (ch == nonDecimalSeparator)
                    {
                        lastNonDecimalSeparator = i;
                        if (firstNonDecimalSeparator == int.MaxValue)
                        {
                            firstNonDecimalSeparator = i;
                        }
                    }
                }

                if (((lastDecimalSeparator < 0 && lastNonDecimalSeparator >= 0) || (lastNonDecimalSeparator < 0 && lastDecimalSeparator >= 0)) &&
                    firstNonDecimalSeparator == lastNonDecimalSeparator)
                {
                    hasSingleSeparator = true;
                }
                else if ((lastDecimalSeparator < lastNonDecimalSeparator) && !(lastDecimalSeparator == -1 || lastNonDecimalSeparator == -1))
                {
                    // Switch separators
                    var aux = decimalSeparator;
                    decimalSeparator = nonDecimalSeparator;
                    nonDecimalSeparator = aux;
                }

            }

            // Try to parse vulgar fraction chars
            if (!isFrac && strLength == 1 && !char.IsDigit(digitsStr[0]))
            {
                double fracResult = char.GetNumericValue(digitsStr, 0);

                if (fracResult != -1.0)
                {
                    return fracResult;
                }
            }

            var calStack = new Stack<double>();

            for (var i = 0; i < strLength; i++)
            {
                var ch = digitsStr[i];
                var prevCh = (i > 0) ? digitsStr[i - 1] : '\0';

                var skippableNonDecimal = SkipNonDecimalSeparator(ch, strLength - i, i, hasSingleSeparator, prevCh, nonDecimalSeparator);

                if (!isFrac && (ch == ' ' || ch == Constants.NO_BREAK_SPACE || skippableNonDecimal))
                {
                    continue;
                }

                if (ch == ' ' || ch == '/')
                {
                    calStack.Push(temp);
                    temp = 0;
                }
                else if (ch >= '0' && ch <= '9')
                {
                    if (hasDecimalSeparator)
                    {
                        temp += scale * (ch - '0');
                        scale *= 0.1;
                    }
                    else
                    {
                        temp = (temp * scale) + (ch - '0');
                    }
                }
                else if (ch == decimalSeparator || (!skippableNonDecimal && ch == nonDecimalSeparator))
                {
                    hasDecimalSeparator = true;
                    scale = 0.1;
                }
                else if (ch == '-')
                {
                    isNegative = true;
                }
            }

            calStack.Push(temp);

            // If the number is a fraction.
            double calResult = 0;
            if (isFrac)
            {
                var denominator = calStack.Pop();
                var mole = calStack.Pop();
                calResult += mole / denominator;
            }

            while (calStack.Any())
            {
                calResult += calStack.Pop();
            }

            calResult *= power;

            if (isNegative)
            {
                return -calResult;
            }

            return calResult;
        }

        public virtual double GetIntValue(List<string> matchStrs)
        {
            var specialCase = Config.GetLangSpecificIntValue(matchStrs);

            if (specialCase.isRelevant)
            {
                return specialCase.value;
            }

            var isEnd = new bool[matchStrs.Count];
            for (var i = 0; i < isEnd.Length; i++)
            {
                isEnd[i] = false;
            }

            double tempValue = 0;
            long endFlag = 1;

            // Scan from end to start, find the end word
            for (var i = matchStrs.Count - 1; i >= 0; i--)
            {
                var matchI = matchStrs[i].ToLowerInvariant();

                if (RoundNumberSet.Contains(matchI))
                {
                    var mappedValue = Config.RoundNumberMap[matchI];

                    // If false, then continue. Will meet hundred first, then thousand.
                    if (endFlag > mappedValue)
                    {
                        continue;
                    }

                    isEnd[i] = true;
                    endFlag = mappedValue;
                }
            }

            // If no multiplier found
            if (endFlag == 1)
            {
                var tempStack = new Stack<double>();
                var oldSym = string.Empty;

                foreach (var matchStr in matchStrs)
                {
                    var isCardinal = Config.CardinalNumberMap.ContainsKey(matchStr);
                    var isOrdinal = Config.OrdinalNumberMap.ContainsKey(matchStr);

                    if (isCardinal || isOrdinal)
                    {
                        var matchValue = isCardinal ?
                            Config.CardinalNumberMap[matchStr] :
                            Config.OrdinalNumberMap[matchStr];

                        // This is just for ordinal now. Not for fraction ever.
                        if (isOrdinal)
                        {
                            double fracPart = Config.OrdinalNumberMap[matchStr];

                            if (tempStack.Any())
                            {
                                var intPart = tempStack.Pop();

                                // If intPart >= fracPart, it means it is an ordinal number
                                // it begins with an integer, ends with an ordinal
                                // e.g. ninety-ninth
                                if (intPart >= fracPart)
                                {
                                    tempStack.Push(intPart + fracPart);
                                }
                                else
                                {
                                    // Another case where the type is ordinal
                                    // e.g. three hundredth
                                    while (tempStack.Any())
                                    {
                                        intPart += tempStack.Pop();
                                    }

                                    tempStack.Push(intPart * fracPart);
                                }
                            }
                            else
                            {
                                tempStack.Push(fracPart);
                            }
                        }
                        else if (Config.CardinalNumberMap.ContainsKey(matchStr))
                        {
                            if (oldSym.Equals("-", StringComparison.Ordinal))
                            {
                                var sum = tempStack.Pop() + matchValue;
                                tempStack.Push(sum);
                            }
                            else if (oldSym.Equals(Config.WrittenIntegerSeparatorTexts.First(), StringComparison.Ordinal) || tempStack.Count < 2)
                            {
                                tempStack.Push(matchValue);
                            }
                            else if (tempStack.Count >= 2)
                            {
                                var sum = tempStack.Pop() + matchValue;
                                sum += tempStack.Pop();
                                tempStack.Push(sum);
                            }
                        }
                    }
                    else
                    {
                        var complexValue = Config.ResolveCompositeNumber(matchStr);
                        if (complexValue != 0)
                        {
                            tempStack.Push(complexValue);
                        }
                    }

                    oldSym = matchStr;
                }

                foreach (var stackValue in tempStack)
                {
                    tempValue += stackValue;
                }
            }
            else
            {
                var lastIndex = 0;
                double mulValue = 1;
                double partValue = 1;

                for (var i = 0; i < isEnd.Length; i++)
                {
                    if (isEnd[i])
                    {
                        mulValue = Config.RoundNumberMap[matchStrs[i]];
                        partValue = 1;

                        if (i != 0)
                        {
                            partValue = GetIntValue(matchStrs.GetRange(lastIndex, i - lastIndex));
                        }

                        tempValue += mulValue * partValue;
                        lastIndex = i + 1;
                    }
                }

                // Calculate the part like "thirty-one"
                mulValue = 1;

                if (lastIndex != isEnd.Length)
                {
                    partValue = GetIntValue(matchStrs.GetRange(lastIndex, isEnd.Length - lastIndex));
                    tempValue += mulValue * partValue;
                }
            }

            return tempValue;
        }

        protected static string GetKeyRegex(IEnumerable<string> keyCollection)
        {
            var sortKeys = keyCollection.OrderByDescending(key => key.Length);
            return string.Join("|", sortKeys);
        }

        protected static string DetermineType(ExtractResult er, ParseResult pr)
        {
            if (!string.IsNullOrEmpty(er.Type) && er.Type.Contains(Constants.MODEL_ORDINAL))
            {
                return er.Metadata.IsOrdinalRelative ? Constants.MODEL_ORDINAL_RELATIVE : Constants.MODEL_ORDINAL;
            }

            var data = er.Data as string;
            var subType = string.Empty;

            if (!string.IsNullOrEmpty(data))
            {
                if (data.StartsWith(Constants.FRACTION_PREFIX, StringComparison.Ordinal))
                {
                    subType = Constants.FRACTION;
                }
                else if (data.Contains(Constants.POWER_SUFFIX))
                {
                    subType = Constants.POWER;
                }
                else if (data.StartsWith(Constants.INTEGER_PREFIX, StringComparison.Ordinal))
                {
                    subType = (pr.Metadata == null || pr.Metadata.TreatAsInteger) ? Constants.INTEGER : Constants.DECIMAL;
                }
                else if (data.StartsWith(Constants.DOUBLE_PREFIX, StringComparison.Ordinal))
                {
                    subType = (pr.Metadata == null || !pr.Metadata.TreatAsInteger) ? Constants.DECIMAL : Constants.INTEGER;
                }
            }

            return subType;
        }

        protected static bool IsMergeable(double former, double later)
        {
            // The former number is an order of magnitude larger than the later number, and they must be integers
            return Math.Abs(former % 1) < double.Epsilon && Math.Abs(later % 1) < double.Epsilon && former > later &&
                   former.ToString("G15", CultureInfo.InvariantCulture).Length > later.ToString("G15", CultureInfo.InvariantCulture).Length &&
                   later > 0;
        }

        // Test if big and combine with small.
        // e.g. "hundred" can combine with "thirty" but "twenty" can't be combined with "thirty".
        protected static bool IsComposable(long big, long small)
        {
            var baseNumber = small > 10 ? 100 : 10;

            return big % baseNumber == 0 && big / baseNumber >= 1;
        }

        protected string GetResolutionStr(object value)
        {
            var resolutionStr = value.ToString();

            if (Config.CultureInfo != null && value is double)
            {
                resolutionStr = ((double)value).ToString("G15", Config.CultureInfo);
            }

            return resolutionStr;
        }

        // Special cases for multi-language countries where decimal separators can be used interchangeably. Mostly informally.
        // Ex: South Africa, Namibia; Puerto Rico in ES; or in Canada for EN and FR.
        // "me pidio $5.00 prestados" and "me pidio $5,00 prestados" -> currency $5
        // "1.000" can be ambiguous and should return "1000" by default
        // If only one separator and not three digits to the right, interpret as decimal separator
        // "100.00" = "100,00" -> "100"
        protected bool SkipNonDecimalSeparator(char ch, int distanceEnd, int distanceStart, bool hasSingleSeparator, char prevCh, char nonDecimalSeparator)
        {
            bool result = false;

            const int decimalLength = 1 + 3;

            if (ch == nonDecimalSeparator)
            {
                result = true;

                if (isMultiDecimalSeparatorCulture && hasSingleSeparator &&
                    (distanceEnd != decimalLength || (prevCh == '0' && distanceStart == 1) || distanceStart > 3))
                {
                    result = false;
                }
            }

            return result;
        }

        protected double GetPointValue(List<string> matchStrs)
        {
            double ret = 0;
            var firstMatch = matchStrs.First();

            if (Config.CardinalNumberMap.ContainsKey(firstMatch) && Config.CardinalNumberMap[firstMatch] >= 10)
            {
                var tempInt = GetIntValue(matchStrs);
                var all = $"0.{tempInt}";
                ret = double.Parse(all, CultureInfo.InvariantCulture);
            }
            else
            {
                var scale = 0.1;
                foreach (string matchStr in matchStrs)
                {
                    ret += Config.CardinalNumberMap[matchStr] * scale;
                    scale *= 0.1;
                }
            }

            return ret;
        }

        private Regex BuildTextNumberRegex()
        {
            var singleIntFrac = $"{this.Config.WordSeparatorToken}| -|" +
                                GetKeyRegex(this.Config.CardinalNumberMap.Keys) + "|" +
                                GetKeyRegex(this.Config.OrdinalNumberMap.Keys);

            // @TODO consider remodeling the creation of this regex
            // For Italian, we invert the order of Cardinal and Ordinal in singleIntFrac in order to correctly extract
            // ordinals that contain cardinals such as 'tredicesimo' (thirteenth) which starts with 'tre' (three).
            // With the standard order, the parser fails to return '13' since only the cardinal 'tre' (3) is extracted
            if (this.Config.CultureInfo.Name == "it-IT")
            {
                singleIntFrac = $"{this.Config.WordSeparatorToken}| -|" +
                                    GetKeyRegex(this.Config.OrdinalNumberMap.Keys) + "|" +
                                    GetKeyRegex(this.Config.CardinalNumberMap.Keys);
            }

            string textNumberPattern;

            // Checks for languages that use "compound numbers". I.e. written number parts are not separated by whitespaces or
            // special characters (e.g., dreihundert in German).
            if (isCompoundNumberLanguage)
            {
                textNumberPattern = @"(" + singleIntFrac + @")";
            }
            else
            {
                // Default case, like in English.
                textNumberPattern = @"(?<=\b)(" + singleIntFrac + @")(?=\b)";
            }

            return new Regex(textNumberPattern, RegexOptions.Singleline | RegexOptions.Compiled);
        }
    }
}