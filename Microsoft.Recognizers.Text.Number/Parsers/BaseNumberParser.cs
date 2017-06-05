using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.Number
{
    public class BaseNumberParser : IParser
    {
        protected readonly INumberParserConfiguration config;

        protected Regex TextNumberRegex { get; }

        protected Regex ArabicNumberRegex { get; }

        protected HashSet<string> RoundNumberSet { get; }

        internal IEnumerable<string> SupportedTypes { get; set; }

        public BaseNumberParser(INumberParserConfiguration config)
        {
            this.config = config;

            var singleIntFrac = $"{this.config.WordSeparatorToken}| -|"
                                + GetKeyRegex(this.config.CardinalNumberMap.Keys) + "|"
                                + GetKeyRegex(this.config.OrdinalNumberMap.Keys);
            TextNumberRegex = new Regex(@"(?<=\b)(" + singleIntFrac + @")(?=\b)",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);
            ArabicNumberRegex = new Regex(@"\d+",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);
            RoundNumberSet = new HashSet<string>();
            foreach (var roundNumber in this.config.RoundNumberMap.Keys)
            {
                RoundNumberSet.Add(roundNumber);
            }
        }

        public virtual ParseResult Parse(ExtractResult extResult)
        {
            // check if the parser is configured to support specific types
            if (SupportedTypes != null && !SupportedTypes.Any(t => extResult.Type.Equals(t)))
            {
                return null;
            }

            string extra = null;
            ParseResult ret = null;
            if ((extra = extResult.Data as string) == null)
            {
                if (ArabicNumberRegex.Match(extResult.Text).Success)
                {
                    extra = "Num";
                }
                else
                {
                    extra = config.LangMarker;
                }
            }
            if (extra.Contains("Num"))
            {
                ret = DigitNumberParse(extResult);
            }
            //Frac is a special number, parse via another method
            else if (extra.Contains($"Frac{config.LangMarker}"))
            {
                ret = FracLikeNumberParse(extResult);
            }
            else if (extra.Contains(config.LangMarker))
            {
                ret = TextNumberParse(extResult);
            }
            else if (extra.Contains("Pow"))
            {
                ret = PowerNumberParse(extResult);
            }
            if (ret?.Value != null)
            {
                ret.ResolutionStr = config.CultureInfo != null
                    ? ((double)ret.Value).ToString(config.CultureInfo)
                    : ret.Value.ToString();
            }

            return ret;
        }

        protected ParseResult PowerNumberParse(ExtractResult extResult)
        {
            var result = new ParseResult
            {
                Start = extResult.Start,
                Length = extResult.Length,
                Text = extResult.Text,
                Type = extResult.Type
            };

            var handle = extResult.Text.ToUpperInvariant();
            var isE = !extResult.Text.Contains("^");

            //[1] 1e10
            //[2] 1.1^-23
            var calStack = new Queue<double>();

            double scale = 10;
            var dot = false;
            var isLessZero = false;
            double tmp = 0;
            for (var i = 0; i < handle.Length; i++)
            {
                var ch = handle[i];
                if (ch == '^' || ch == 'E')
                {
                    if (isLessZero)
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
                    isLessZero = false;
                }
                else if (ch >= '0' && ch <= '9')
                {
                    if (dot)
                    {
                        tmp = tmp + scale * (ch - '0');
                        scale *= 0.1;
                    }
                    else
                    {
                        tmp = tmp * scale + (ch - '0');
                    }
                }
                else if (ch == config.DecimalSeparatorChar)
                {
                    dot = true;
                    scale = 0.1;
                }
                else if (ch == '-')
                {
                    isLessZero = !isLessZero;
                }
                else if (ch == '+')
                {
                    continue;
                }
                if (i == handle.Length - 1)
                {
                    if (isLessZero)
                    {
                        calStack.Enqueue(-tmp);
                    }
                    else
                    {
                        calStack.Enqueue(tmp);
                    }
                }
            }

            double ret = 0;
            if (isE)
            {
                ret = calStack.Dequeue() * Math.Pow(10, calStack.Dequeue());
            }
            else
            {
                ret = Math.Pow(calStack.Dequeue(), calStack.Dequeue());
            }

            result.Value = ret;
            result.ResolutionStr = ret.ToString();

            return result;
        }

        protected ParseResult TextNumberParse(ExtractResult extResult)
        {
            var result = new ParseResult
            {
                Start = extResult.Start,
                Length = extResult.Length,
                Text = extResult.Text,
                Type = extResult.Type
            };
            var handle = extResult.Text.ToLower();

            #region dozen special

            handle = config.HalfADozenRegex.Replace(handle, config.HalfADozenText);

            #endregion

            var numGroup = handle.Split(config.WrittenDecimalSeparatorTexts.ToArray(),
                StringSplitOptions.RemoveEmptyEntries);

            #region intPart

            var intPart = numGroup[0];
            var sMatch = TextNumberRegex.Match(intPart);
            var matchStrs = new List<string>();
            //Store all match str.
            while (sMatch.Success)
            {
                var matchStr = sMatch.Groups[0].Value.ToLower();
                matchStrs.Add(matchStr);
                sMatch = sMatch.NextMatch();
            }

            //Get the value recursively
            var intPartRet = GetIntValue(matchStrs);

            #endregion

            #region PointPart

            double pointPartRet = 0;
            if (numGroup.Length == 2)
            {
                var pointPart = numGroup[1];
                sMatch = TextNumberRegex.Match(pointPart);
                matchStrs.Clear();
                while (sMatch.Success)
                {
                    var matchStr = sMatch.Groups[0].Value.ToLower();
                    matchStrs.Add(matchStr);
                    sMatch = sMatch.NextMatch();
                }
                pointPartRet += GetPointValue(matchStrs);
            }

            #endregion

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
                Type = extResult.Type
            };

            var resultText = extResult.Text.ToLower();
            if (resultText.Contains($@"{config.FractionMarkerToken}"))
            {
                var overIndex = resultText.IndexOf(config.FractionMarkerToken, StringComparison.Ordinal);
                var smallPart = resultText.Substring(0, overIndex).Trim();
                var bigPart = resultText.Substring(overIndex + config.FractionMarkerToken.Length,
                    resultText.Length - overIndex - config.FractionMarkerToken.Length).Trim();

                var smallValue = char.IsDigit(smallPart[0])
                    ? GetDigitalValue(smallPart, 1)
                    : GetIntValue(GetMatches(smallPart));

                var bigValue = char.IsDigit(bigPart[0])
                    ? GetDigitalValue(bigPart, 1)
                    : GetIntValue(GetMatches(bigPart));

                result.Value = smallValue / bigValue;
            }
            else
            {
                var fracWords = config.NormalizeTokenSet(resultText.Split(null), result).ToList();

                // Split fraction with integer
                var splitIndex = fracWords.Count - 1;
                var currentValue = config.ResolveCompositeNumber(fracWords[splitIndex]);
                long roundValue = 1;

                for (splitIndex = fracWords.Count - 2; splitIndex >= 0; splitIndex--)
                {
                    if (config.WrittenFractionSeparatorTexts.Contains(fracWords[splitIndex]))
                    {
                        continue;
                    }
                    var previousValue = currentValue;
                    currentValue = config.ResolveCompositeNumber(fracWords[splitIndex]);
                    // previous : hundred
                    // current : one
                    if ((previousValue >= 100 && previousValue > currentValue)
                        || (previousValue < 100 && IsComposable(currentValue, previousValue)))
                    {
                        if (previousValue < 100 && currentValue >= roundValue)
                        {
                            roundValue = currentValue;
                        }
                        else if (previousValue < 100 && currentValue < roundValue)
                        {
                            splitIndex++;
                            break;
                        }
                        // current is the first word
                        if (splitIndex == 0)
                        {
                            // scan, skip the first word
                            splitIndex = 1;
                            while (splitIndex <= fracWords.Count - 2)
                            {
                                // e.g. one hundred thousand 
                                // frac[i+1] % 100 && frac[i] % 100 = 0
                                if (config.ResolveCompositeNumber(fracWords[splitIndex]) >= 100
                                    && !config.WrittenFractionSeparatorTexts.Contains(fracWords[splitIndex + 1])
                                    && config.ResolveCompositeNumber(fracWords[splitIndex + 1]) < 100)
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

                // denomi = denominator
                var denomiValue = GetIntValue(fracPart);
                // Split mixed number with fraction
                double numerValue = 0;
                double intValue = 0;

                var mixedIndex = fracWords.Count;
                for (var i = fracWords.Count - 1; i >= 0; i--)
                {
                    if (i < fracWords.Count - 1 && config.WrittenFractionSeparatorTexts.Contains(fracWords[i]))
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
                if (mixedIndex != fracWords.Count && numerValue < denomiValue)
                {
                    result.Value = intValue + numerValue / denomiValue;
                }
                else
                {
                    result.Value = (intValue + numerValue) / denomiValue;
                }
            }

            return result;
        }

        protected string GetKeyRegex(IEnumerable<string> keyCollection)
        {
            var sortKeys = keyCollection.OrderByDescending(key => key.Length);
            return string.Join("|", sortKeys);
        }

        private List<string> GetMatches(string input)
        {
            var sMatch = TextNumberRegex.Match(input);
            var matchStrs = new List<string>();

            //Store all match str.
            while (sMatch.Success)
            {
                var matchStr = sMatch.Groups[0].Value.ToLower();
                matchStrs.Add(matchStr);
                sMatch = sMatch.NextMatch();
            }

            return matchStrs;
        }

        //Test if big and combine with small.
        //e.g. "hundred" can combine with "thirty" but "twenty" can't combine with "thirty".
        private bool IsComposable(long big, long small)
        {
            var baseNumber = small > 10 ? 100 : 10;

            if (big % baseNumber == 0 && big / baseNumber >= 1)
            {
                return true;
            }

            return false;
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

            //Scan from end to start, find the end word
            for (var i = matchStrs.Count - 1; i >= 0; i--)
            {
                if (RoundNumberSet.Contains(matchStrs[i]))
                {
                    //if false,then continue
                    //You will meet hundred first, then thousand.
                    if (endFlag > config.RoundNumberMap[matchStrs[i]])
                    {
                        continue;
                    }
                    isEnd[i] = true;
                    endFlag = config.RoundNumberMap[matchStrs[i]];
                }
            }

            if (endFlag == 1)
            {
                var tempStack = new Stack<double>();
                var oldSym = "";
                foreach (var matchStr in matchStrs)
                {
                    var isCardinal = config.CardinalNumberMap.ContainsKey(matchStr);
                    var isOrdinal = config.OrdinalNumberMap.ContainsKey(matchStr);
                    if (isCardinal || isOrdinal)
                    {
                        var matchValue = isCardinal
                            ? config.CardinalNumberMap[matchStr]
                            : config.OrdinalNumberMap[matchStr];
                        //This is just for ordinal now. Not for fraction ever.
                        if (isOrdinal)
                        {
                            double fracPart = config.OrdinalNumberMap[matchStr];
                            if (tempStack.Any())
                            {
                                var intPart = tempStack.Pop();
                                // if intPart >= fracPart, it means it is an ordinal number
                                // it begins with an integer, ends with an ordinal
                                // e.g. ninety-ninth
                                if (intPart >= fracPart)
                                {
                                    tempStack.Push(intPart + fracPart);
                                }
                                // another case of the type is ordinal
                                // e.g. three hundredth
                                else
                                {
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
                        else if (config.CardinalNumberMap.ContainsKey(matchStr))
                        {
                            if (oldSym.Equals("-"))
                            {
                                var sum = tempStack.Pop() + matchValue;
                                tempStack.Push(sum);
                            }
                            else if (oldSym.Equals(config.WrittenIntegerSeparatorTexts.First()) || tempStack.Count() < 2)
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
                        var complexValue = config.ResolveCompositeNumber(matchStr);
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
                        mulValue = config.RoundNumberMap[matchStrs[i]];
                        partValue = 1;

                        if (i != 0)
                        {
                            partValue = GetIntValue(matchStrs.GetRange(lastIndex, i - lastIndex));
                        }

                        tempValue += mulValue * partValue;
                        lastIndex = i + 1;
                    }
                }

                //Calculate the part like "thirty-one"
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

            if (config.CardinalNumberMap.ContainsKey(firstMatch) && config.CardinalNumberMap[firstMatch] >= 10)
            {
                var prefix = "0.";
                var tempInt = GetIntValue(matchStrs);
                var all = prefix + tempInt;
                ret = double.Parse(all);
            }
            else
            {
                var scale = 0.1;
                for (var i = 0; i < matchStrs.Count; i++)
                {
                    ret += config.CardinalNumberMap[matchStrs[i]] * scale;
                    scale *= 0.1;
                }
            }

            return ret;
        }

        /// <summary>
        /// Precondition: ExtResult must have arabian digitals.
        /// </summary>
        /// <param name="extResult">input arabian number</param>
        /// <returns></returns>
        protected ParseResult DigitNumberParse(ExtractResult extResult)
        {
            var result = new ParseResult
            {
                Start = extResult.Start,
                Length = extResult.Length,
                Text = extResult.Text,
                Type = extResult.Type
            };

            //[1] 24
            //[2] 12 32/33
            //[3] 1,000,000
            //[4] 234.567
            //[5] 44/55
            //[6] 2 hundred
            //dot occured.
            double power = 1;
            var handle = extResult.Text.ToLower();
            var match = config.DigitalNumberRegex.Match(handle);
            int tmpIndex = -1, startIndex = 0;
            while (match.Success)
            {
                double rep = config.RoundNumberMap[match.Value];
                // \\s+ for filter the spaces.
                power *= rep;
                while ((tmpIndex = handle.IndexOf(match.Value, startIndex)) >= 0)
                {
                    var front = handle.Substring(0, tmpIndex).TrimEnd();
                    startIndex = front.Length;
                    handle = front + handle.Substring(tmpIndex + match.Value.Length);
                }
                match = match.NextMatch();
            }

            //scale used in the calculate of double
            result.Value = GetDigitalValue(handle, power);

            return result;
        }

        protected double GetDigitalValue(string digitStr, double power)
        {
            double temp = 0;
            double scale = 10;
            var dot = false;
            var isLessZero = false;
            var isFrac = false;

            var calStack = new Stack<double>();

            for (var i = 0; i < digitStr.Count(); i++)
            {
                var ch = digitStr[i];
                if (ch == '/')
                {
                    isFrac = true;
                }

                if (ch == ' ' || ch == '/')
                {
                    calStack.Push(temp);
                    temp = 0;
                }
                else if (ch >= '0' && ch <= '9')
                {
                    if (dot)
                    {
                        temp = temp + scale * (ch - '0');
                        scale *= 0.1;
                    }
                    else
                    {
                        temp = temp * scale + (ch - '0');
                    }
                }
                else if (ch == config.DecimalSeparatorChar)
                {
                    dot = true;
                    scale = 0.1;
                }
                else if (ch == '-')
                {
                    isLessZero = true;
                }
            }
            calStack.Push(temp);

            // is the number is a fraction.
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

            if (isLessZero)
            {
                return -calResult;
            }

            return calResult;
        }
    }
}