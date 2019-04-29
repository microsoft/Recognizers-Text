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
            new Regex(@"\d+", RegexOptions.Singleline);

        private static readonly Regex MultiDecimalSeparatorCultureRegex =
            new Regex(@"^(en|es|fr|bg)(-)?\b", RegexOptions.Singleline | RegexOptions.Compiled);

        private static readonly List<string> CompoundNumberLanguages = new List<string> { "de-DE", "nl-NL", "sv-SE" };

        private readonly bool isMultiDecimalSeparatorCulture = false;

        public BaseNumberParser(INumberParserConfiguration config)
        {
            this.Config = config;

            this.isMultiDecimalSeparatorCulture = MultiDecimalSeparatorCultureRegex.IsMatch(config.CultureInfo.Name);

            var singleIntFrac = $"{this.Config.WordSeparatorToken}| -|" +
                                GetKeyRegex(this.Config.CardinalNumberMap.Keys) + "|" +
                                GetKeyRegex(this.Config.OrdinalNumberMap.Keys);

            string textNumberPattern;

            // Checks for languages that use "compound numbers". I.e. written number parts are not separated by whitespaces or special characters (e.g., dreihundert in German).
            if (CompoundNumberLanguages.Contains(config.CultureInfo.Name))
            {
                textNumberPattern = @"(" + singleIntFrac + @")";
            }
            else
            {
                // Default case, like in English.
                textNumberPattern = @"(?<=\b)(" + singleIntFrac + @")(?=\b)";
            }

            TextNumberRegex = new Regex(textNumberPattern, RegexOptions.Singleline | RegexOptions.Compiled);

            RoundNumberSet = new HashSet<string>();
            foreach (var roundNumber in this.Config.RoundNumberMap.Keys)
            {
                RoundNumberSet.Add(roundNumber);
            }
        }

        internal IEnumerable<string> SupportedTypes { get; set; }

        protected static Regex LongFormatRegex => LongFormRegex;

        protected INumberParserConfiguration Config { get; private set; }

        protected Regex TextNumberRegex { get; }

        protected HashSet<string> RoundNumberSet { get; }

        public virtual ParseResult Parse(ExtractResult extResult)
        {
            // Check if the parser is configured to support specific types
            if (SupportedTypes != null && !SupportedTypes.Any(t => extResult.Type.Equals(t)))
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
                extResult.Text = extResult.Text.Substring(matchNegative.Groups[1].Length);
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
            else if (extra.Contains($"{Constants.FRACTION_PREFIX}{Config.LangMarker}"))
            {
                // Such fractions are special cases, parse via another method
                ret = FracLikeNumberParse(extResult);
            }
            else if (extra.Contains(Config.LangMarker))
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
                    ret.Text = matchNegative.Groups[1].Value + extResult.Text;
                    ret.Value = -(double)ret.Value;
                }

                ret.ResolutionStr = GetResolutionStr(ret.Value);
            }

            // Add "offset" and "relativeTo" for ordinal
            if (!string.IsNullOrEmpty(ret.Type) && ret.Type.Contains(Constants.MODEL_ORDINAL))
            {
                if (Config.RelativeReferenceOffsetMap.ContainsKey(extResult.Text) &&
                    Config.RelativeReferenceRelativeToMap.ContainsKey(extResult.Text))
                {
                    ret.Metadata.Offset = Config.RelativeReferenceOffsetMap[extResult.Text];
                    ret.Metadata.RelativeTo = Config.RelativeReferenceRelativeToMap[extResult.Text];
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

        protected static string GetKeyRegex(IEnumerable<string> keyCollection)
        {
            var sortKeys = keyCollection.OrderByDescending(key => key.Length);
            return string.Join("|", sortKeys);
        }

        protected ParseResult PowerNumberParse(ExtractResult extResult)
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
            var dot = false;
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
                    dot = false;
                    isNegative = false;
                }
                else if (ch >= '0' && ch <= '9')
                {
                    if (dot)
                    {
                        tmp = tmp + (scale * (ch - '0'));
                        scale *= 0.1;
                    }
                    else
                    {
                        tmp = (tmp * scale) + (ch - '0');
                    }
                }
                else if (ch == Config.DecimalSeparatorChar)
                {
                    dot = true;
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
            result.ResolutionStr = ret.ToString(CultureInfo.InvariantCulture);

            return result;
        }

        protected ParseResult TextNumberParse(ExtractResult extResult)
        {
            var result = new ParseResult
            {
                Start = extResult.Start,
                Length = extResult.Length,
                Text = extResult.Text,
                Type = extResult.Type,
                Metadata = extResult.Metadata,
            };

            var handle = extResult.Text.ToLower();

            handle = Config.HalfADozenRegex.Replace(handle, Config.HalfADozenText);

            // Handling cases like "last", "next one", "previous one"
            if (Config.RelativeReferenceOffsetMap.ContainsKey(extResult.Text) &&
                Config.RelativeReferenceRelativeToMap.ContainsKey(extResult.Text))
            {
                return result;
            }

            var numGroup = handle.Split(Config.WrittenDecimalSeparatorTexts.ToArray(), StringSplitOptions.RemoveEmptyEntries);

            var intPart = numGroup[0];
            var stringMatch = TextNumberRegex.Match(intPart);

            // Store all match str.
            var matchStrs = new List<string>();

            while (stringMatch.Success)
            {
                var matchStr = stringMatch.Groups[0].Value.ToLower();
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
                    var matchStr = stringMatch.Groups[0].Value.ToLower();
                    matchStrs.Add(matchStr);
                    stringMatch = stringMatch.NextMatch();
                }

                pointPartRet += GetPointValue(matchStrs);
            }

            result.Value = intPartRet + pointPartRet;

            return result;
        }

        protected ParseResult FracLikeNumberParse(ExtractResult extResult)
        {
            var result = new ParseResult
            {
                Start = extResult.Start,
                Length = extResult.Length,
                Text = extResult.Text,
                Type = extResult.Type,
            };

            var resultText = extResult.Text.ToLower();
            if (Config.FractionPrepositionRegex.IsMatch(resultText))
            {
                var match = Config.FractionPrepositionRegex.Match(resultText);
                var numerator = match.Groups["numerator"].Value;
                var denominator = match.Groups["denominator"].Value;

                var smallValue = char.IsDigit(numerator[0]) ?
                    GetDigitalValue(numerator, 1) :
                    GetIntValue(GetMatches(numerator));

                var bigValue = char.IsDigit(denominator[0]) ?
                    GetDigitalValue(denominator, 1) :
                    GetIntValue(GetMatches(denominator));

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
                   result.Value = 1 / GetIntValue(fracWords);
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
                        numerValue = GetIntValue(GetMatches(numerStr));
                        mixedIndex = i + 1;
                        break;
                    }
                }

                var intStr = string.Join(" ", fracWords.GetRange(0, mixedIndex));
                intValue = GetIntValue(GetMatches(intStr));

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

        /// <summary>
        /// Precondition: ExtResult must have arabic numerals.
        /// </summary>
        /// <param name="extResult">input arabic number.</param>
        /// <returns>parsed result.</returns>
        protected ParseResult DigitNumberParse(ExtractResult extResult)
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
            var extText = extResult.Text.ToLower();
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
            result.Value = GetDigitalValue(extText, power);

            return result;
        }

        protected double GetDigitalValue(string digitsStr, double power)
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
            }

            calStack.Push(temp);

            // If the number is a fraction.
            double calResult = 0;
            if (isFrac)
            {
                var deno = calStack.Pop();
                var mole = calStack.Pop();
                calResult += mole / deno;
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

        private static string DetermineType(ExtractResult er)
        {
            var data = er.Data as string;
            var subType = string.Empty;

            if (!string.IsNullOrEmpty(data))
            {
                if (data.StartsWith(Constants.FRACTION_PREFIX))
                {
                    subType = Constants.FRACTION;
                }
                else if (data.Contains(Constants.POWER_SUFFIX))
                {
                    subType = Constants.POWER;
                }
                else if (data.StartsWith(Constants.INTEGER_PREFIX))
                {
                    subType = Constants.INTEGER;
                }
                else if (data.StartsWith(Constants.DOUBLE_PREFIX))
                {
                    subType = Constants.DECIMAL;
                }
            }

            return subType;
        }

        private static bool IsMergeable(double former, double later)
        {
            // The former number is an order of magnitude larger than the later number, and they must be integers
            return Math.Abs(former % 1) < double.Epsilon && Math.Abs(later % 1) < double.Epsilon &&
                   former > later && former.ToString(CultureInfo.InvariantCulture).Length > later.ToString(CultureInfo.InvariantCulture).Length && later > 0;
        }

        // Test if big and combine with small.
        // e.g. "hundred" can combine with "thirty" but "twenty" can't combine with "thirty".
        private static bool IsComposable(long big, long small)
        {
            var baseNumber = small > 10 ? 100 : 10;

            return big % baseNumber == 0 && big / baseNumber >= 1;
        }

        private string GetResolutionStr(object value)
        {
            var resolutionStr = value.ToString();

            if (Config.CultureInfo != null && value is double)
            {
                resolutionStr = ((double)value).ToString(Config.CultureInfo);
            }

            return resolutionStr;
        }

        // Special cases for multi-language countries where decimal separators can be used interchangeably. Mostly informally.
        // Ex: South Africa, Namibia; Puerto Rico in ES; or in Canada for EN and FR.
        // "me pidio $5.00 prestados" and "me pidio $5,00 prestados" -> currency $5
        private bool SkipNonDecimalSeparator(char ch, int distance)
        {
            var decimalLength = 3;

            return ch == Config.NonDecimalSeparatorChar && !(distance <= decimalLength && isMultiDecimalSeparatorCulture);
        }

        private List<string> GetMatches(string input)
        {
            var successMatch = TextNumberRegex.Match(input);
            var matchStrs = new List<string>();

            // Store all match str.
            while (successMatch.Success)
            {
                var matchStr = successMatch.Groups[0].Value.ToLower();
                matchStrs.Add(matchStr);
                successMatch = successMatch.NextMatch();
            }

            return matchStrs;
        }

        private double GetIntValue(List<string> matchStrs)
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
                            if (oldSym.Equals("-"))
                            {
                                var sum = tempStack.Pop() + matchValue;
                                tempStack.Push(sum);
                            }
                            else if (oldSym.Equals(Config.WrittenIntegerSeparatorTexts.First()) || tempStack.Count() < 2)
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

        private double GetPointValue(List<string> matchStrs)
        {
            double ret = 0;
            var firstMatch = matchStrs.First();

            if (Config.CardinalNumberMap.ContainsKey(firstMatch) && Config.CardinalNumberMap[firstMatch] >= 10)
            {
                var prefix = "0.";
                var tempInt = GetIntValue(matchStrs);
                var all = prefix + tempInt;
                ret = double.Parse(all);
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
    }
}