using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.Number
{
    public class BaseIndianNumberParser : BaseNumberParser
    {
        private readonly bool isMultiDecimalSeparatorCulture = false;

        private readonly bool isCompoundNumberLanguage = false;

        public BaseIndianNumberParser(INumberParserConfiguration config)
            : base(config)
        {
            this.Config = config as IIndianNumberParserConfiguration;

            this.isMultiDecimalSeparatorCulture = config.IsMultiDecimalSeparatorCulture;
            this.isCompoundNumberLanguage = config.IsCompoundNumberLanguage;

            TextNumberRegex = BuildTextNumberRegex();
        }

        protected new IIndianNumberParserConfiguration Config { get; private set; }

        protected new Regex TextNumberRegex { get; }

        // Overrides Parse from BaseNumberParser to account for the new functions used for peculaiar handeling of Indo-Aryan Lanauges
        // Uses the same flow as the BaseNumberParser Parse
        public override ParseResult Parse(ExtractResult extResult)
        {
            // Check if the parser is configured to support specific types
            if (SupportedTypes != null && !SupportedTypes.Any(t => extResult.Type.Equals(t, StringComparison.Ordinal)))
            {
                return null;
            }

            ParseResult ret = null;

            if (!(extResult.Data is string extra))
            {
                extra = LongFormatRegex.Match(extResult.Text).Success ? Constants.NUMBER_SUFFIX : Config.LangMarker;
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
                // Overriden behavior with special handeling for Indo - Arayn lanuages renamed from DigitNumberParse
                ret = ParseDigitNumber(extResult);
            }
            else if (extra.Contains($"{Constants.FRACTION_PREFIX}{Config.LangMarker}"))
            {
                // Overriden behavior with special handeling for Indo - Arayn lanuages renamed from FracLikeNumberParse
                ret = ParseFracLikeNumber(extResult);
            }
            else if (extra.Contains(Config.LangMarker))
            {
                // Overriden behavior with special handeling for Indo - Arayn lanuages renamed from TextNumberParse
                ret = ParseTextNumber(extResult);
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
                ret.Type = DetermineType(extResult);
                ret.Text = ret.Text.ToLowerInvariant();
            }

            return ret;
        }

        // Renamed from TextNumberParse, same behavior but accounts for GetTheIntValue
        protected ParseResult ParseTextNumber(ExtractResult extResult)
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
            var intPartRet = GetTheIntValue(matchStrs);

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

        // Renamed from FracLikeNumberParse, same behavior but accounts peculiarities in Indian languages Fractions
        protected ParseResult ParseFracLikeNumber(ExtractResult extResult)
        {
            var result = new ParseResult
            {
                Start = extResult.Start,
                Length = extResult.Length,
                Text = extResult.Text,
                Type = extResult.Type,
            };

            var resultText = extResult.Text;
            if (Config.FractionPrepositionRegex.IsMatch(resultText) && !Config.AdditionTermsRegex.IsMatch(resultText))
            {
                // condition inncludes AdditionTermsRegex in combination with FractionPrepositionRegex
                // to account for Behaviour changes of और - In fraction cases where और is used to connect two words and may not be used as addition of two words from its left and right.
                // solve cases like: "तीन और एक का पाँचवाँ भाग"
                var match = Config.FractionPrepositionRegex.Match(resultText);
                var numerator = match.Groups["numerator"].Value;
                var denominator = match.Groups["denominator"].Value;

                var smallValue = char.IsDigit(numerator[0]) ?
                    GetTheDigitalValue(numerator, 1) :
                    GetTheIntValue(Utilities.RegExpUtility.GetMatches(this.TextNumberRegex, numerator));

                var bigValue = char.IsDigit(denominator[0]) ?
                    GetTheDigitalValue(denominator, 1) :
                    GetTheIntValue(Utilities.RegExpUtility.GetMatches(this.TextNumberRegex, denominator));

                result.Value = smallValue / bigValue;
            }
            else if (Config.FractionPrepositionInverseRegex.IsMatch(resultText))
            {
                // condition  to use FractionPrepositionInverseRegex where denominator and nominator are switched to account for
                // में से (out of) - These type of cases are very common in Hindi. It belongs to fraction unit type. Here any word/char
                // at left of में से acts as denominator and right of it acts as numerator
                var match = Config.FractionPrepositionInverseRegex.Match(resultText);
                var numerator = match.Groups["numerator"].Value;
                var denominator = match.Groups["denominator"].Value;

                var smallValue = char.IsDigit(numerator[0]) ?
                    GetTheDigitalValue(numerator, 1) :
                    GetTheIntValue(Utilities.RegExpUtility.GetMatches(this.TextNumberRegex, numerator));

                var bigValue = char.IsDigit(denominator[0]) ?
                    GetTheDigitalValue(denominator, 1) :
                    GetTheIntValue(Utilities.RegExpUtility.GetMatches(this.TextNumberRegex, denominator));

                result.Value = smallValue / bigValue;
            }
            else
            {
                var fracWords = Config.NormalizeTokenSet(resultText.Split(null), result).ToList();

                // Split fraction with integer
                var splitIndex = fracWords.Count - 1;
                var currentValue = Config.ResolveCompositeNumber(fracWords[splitIndex]);
                long roundValue = 1;

                // For case like "half"
                if (fracWords.Count == 1)
                {
                    result.Value = 1 / GetTheIntValue(fracWords);
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
                var denominator = GetTheIntValue(fracPart);
                double numerValue = 0;
                double intValue = 0;

                var mixedIndex = fracWords.Count;
                for (var i = fracWords.Count - 1; i >= 0; i--)
                {
                    if (i < fracWords.Count - 1 && Config.WrittenFractionSeparatorTexts.Contains(fracWords[i]))
                    {
                        var numerStr = string.Join(" ", fracWords.GetRange(i + 1, fracWords.Count - 1 - i));
                        numerValue = GetTheIntValue(Utilities.RegExpUtility.GetMatches(this.TextNumberRegex, numerStr));
                        mixedIndex = i + 1;
                        break;
                    }
                }

                var intStr = string.Join(" ", fracWords.GetRange(0, mixedIndex));
                intValue = GetTheIntValue(Utilities.RegExpUtility.GetMatches(this.TextNumberRegex, intStr));

                // Find mixed number
                if (mixedIndex != fracWords.Count && numerValue < denominator)
                {
                    result.Value = intValue + (numerValue / denominator);
                }
                else
                {
                    result.Value = (intValue + numerValue) / denominator;
                }
            }

            return result;
        }

        // Renamed from DigitNumberNumberParse, same behavior but accounts for GetTheDigitalValue
        protected ParseResult ParseDigitNumber(ExtractResult extResult)
        {
            var result = new ParseResult
            {
                Start = extResult.Start,
                Length = extResult.Length,
                Text = extResult.Text,
                Type = extResult.Type,
                Metadata = extResult.Metadata,
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
            result.Value = GetTheDigitalValue(extText, power);

            return result;
        }

        // Renamed from GetDigitalValue, same behavior but accounts for Devenagari Numerals in parsing
        protected double GetTheDigitalValue(string digitsStr, double power)
        {
            double temp = 0;
            double scale = 10;
            var decimalSeparator = false;
            var strLength = digitsStr.Length;
            var isNegative = false;
            var isFrac = digitsStr.Contains('/');

            var calStack = new Stack<double>();

            for (var i = 0; i < digitsStr.Length; i++)
            {
                var ch = digitsStr[i];
                var skippableNonDecimal = SkipNonDecimalSeparator(ch, strLength - i);
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
                    if (decimalSeparator)
                    {
                        temp = temp + (scale * (ch - '0'));
                        scale *= 0.1;
                    }
                    else
                    {
                        temp = (temp * scale) + (ch - '0');
                    }
                }
                else if (ch == Config.DecimalSeparatorChar || (!skippableNonDecimal && ch == Config.NonDecimalSeparatorChar))
                {
                    decimalSeparator = true;
                    scale = 0.1;
                }
                else if (ch == '-')
                {
                    isNegative = true;
                }
                else if (Config.ZeroToNineMap.Any(x => x.Key == ch))
                {
                    // handle Devanagari numerals defined in ZeroToNineMap
                    if (char.IsDigit(ch))
                    {
                        if (decimalSeparator)
                        {
                            temp = temp + (Config.ZeroToNineMap[ch] * scale);
                            scale *= 0.1;
                        }
                        else
                        {
                            temp = (temp * scale) + Config.ZeroToNineMap[ch];
                        }
                    }
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

        // Renamed from GetIntValue, same behavior but accounts for regional Hindi cases like डेढ/सवा/ढाई
        private double GetTheIntValue(List<string> matchStrs)
        {
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
                                        intPart = intPart + tempStack.Pop();
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
                            else if (oldSym.Equals(Config.WrittenIntegerSeparatorTexts.First(), StringComparison.Ordinal) || tempStack.Count() < 2)
                            {
                                tempStack.Push(matchValue);
                            }
                            else if (tempStack.Count() >= 2)
                            {
                                var sum = tempStack.Pop() + matchValue;
                                sum = tempStack.Pop() + sum;
                                tempStack.Push(sum);
                            }
                        }
                    }
                    else
                    {
                        // Used to parse regional Hindi cases like डेढ/सवा/ढाई
                        // They are Indian Language specific cases and holds various meaning when prefixed with Number unit.
                        var complexVal = Config.ResolveUnitCompositeNumber(matchStr);
                        if (complexVal != 0)
                        {
                            tempStack.Push(complexVal);
                        }

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
                            partValue = GetTheIntValue(matchStrs.GetRange(lastIndex, i - lastIndex));
                        }

                        tempValue += mulValue * partValue;
                        lastIndex = i + 1;
                    }
                }

                // Calculate the part like "thirty-one"
                mulValue = 1;

                if (lastIndex != isEnd.Length)
                {
                    partValue = GetTheIntValue(matchStrs.GetRange(lastIndex, isEnd.Length - lastIndex));
                    tempValue += mulValue * partValue;
                }
            }

            return tempValue;
        }

        private Regex BuildTextNumberRegex()
        {
            // For Hindi, there is a need for another NumberMap of the type double to handle values like 1.5.
            // As this cannot be included in either Cardinal or Ordinal NumberMap as they are of the type long,
            // DecimalUnitsList (type double) takes care of these entries and it needs to be added to the singleIntFrac
            // for extraction
            var singleIntFrac = $"{this.Config.WordSeparatorToken}| -|" +
                                GetKeyRegex(this.Config.CardinalNumberMap.Keys) + "|" +
                                GetKeyRegex(this.Config.OrdinalNumberMap.Keys) + "|" +
                                GetKeyRegex(this.Config.DecimalUnitsMap.Keys);

            string textNumberPattern;

            // Checks for languages that use "compound numbers". I.e. written number parts are not separated by whitespaces or special characters (e.g., dreihundert in German).
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