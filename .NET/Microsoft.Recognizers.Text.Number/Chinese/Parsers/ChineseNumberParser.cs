using System.Text;

namespace Microsoft.Recognizers.Text.Number.Chinese
{
    public class ChineseNumberParser : BaseNumberParser
    {
        protected new readonly ChineseNumberParserConfiguration Config;

        public ChineseNumberParser(ChineseNumberParserConfiguration config) : base(config)
        {
            this.Config = config;
        }

        public override ParseResult Parse(ExtractResult extResult)
        {
            string extra = null;
            ParseResult ret = null;
            extra = extResult.Data as string;

            var simplifiedExtResult = new ExtractResult()
            {
                Start = extResult.Start,
                Length = extResult.Length,
                Data = extResult.Data,
                Text = ReplaceTraWithSim(extResult.Text),
                Type = extResult.Type
            };

            if (extra == null) {
                return null;
            }

            if (extra.Contains("Per"))
            {
                ret = PerParseChs(simplifiedExtResult);
            }
            else if (extra.Contains("Num"))
            {
                simplifiedExtResult.Text = PatternUtil.NormalizeCharWidth(simplifiedExtResult.Text, Culture.Chinese);
                ret = DigitNumberParse(simplifiedExtResult);
                if (Config.NegativeNumberSignRegex.IsMatch(simplifiedExtResult.Text) && (double)ret.Value > 0)
                {
                    ret.Value = -(double)ret.Value;
                }
                ret.ResolutionStr = ret.Value.ToString();
            }
            else if (extra.Contains("Pow"))
            {
                simplifiedExtResult.Text = PatternUtil.NormalizeCharWidth(simplifiedExtResult.Text, Culture.Chinese);
                ret = PowerNumberParse(simplifiedExtResult);
                ret.ResolutionStr = ret.Value.ToString();
            }
            else if (extra.Contains("Frac"))
            {
                ret = FracParseChs(simplifiedExtResult);
            }
            else if (extra.Contains("Dou"))
            {
                ret = DouParseChs(simplifiedExtResult);
            }
            else if (extra.Contains("Integer"))
            {
                ret = IntParseChs(simplifiedExtResult);
            }
            else if (extra.Contains("Ordinal"))
            {
                ret = OrdParseChs(simplifiedExtResult);
            }

            if (ret != null) {
                ret.Text = extResult.Text;
            }

            return ret;
        }

        private string ReplaceTraWithSim(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return text;
            }

            var builder = new StringBuilder();
            foreach (char c in text) {
                builder.Append(Config.TratoSimMapChs.ContainsKey(c) ? Config.TratoSimMapChs[c] : c);
            }
            return builder.ToString();
        }

        protected ParseResult FracParseChs(ExtractResult extResult)
        {
            var result = new ParseResult
            {
                Start = extResult.Start,
                Length = extResult.Length,
                Text = extResult.Text,
                Type = extResult.Type
            };

            var resultText = extResult.Text;
            var splitResult = Config.FracSplitRegex.Split(resultText);
            string intPart = "", demoPart = "", numPart = "";
            if (splitResult.Length == 3)
            {
                intPart = splitResult[0];
                demoPart = splitResult[1];
                numPart = splitResult[2];
            }
            else
            {
                intPart = "零";
                demoPart = splitResult[0];
                numPart = splitResult[1];
            }

            var intValue = Config.DigitNumRegex.IsMatch(intPart)
                ? GetDigitValueChs(intPart, 1.0)
                : PatternUtil.GetIntValue(intPart, Culture.Chinese);

            var numValue = Config.DigitNumRegex.IsMatch(numPart)
                ? GetDigitValueChs(numPart, 1.0)
                : PatternUtil.GetIntValue(numPart, Culture.Chinese);

            var demoValue = Config.DigitNumRegex.IsMatch(demoPart)
                ? GetDigitValueChs(demoPart, 1.0)
                : PatternUtil.GetIntValue(demoPart, Culture.Chinese);

            if (Config.NegativeNumberSignRegex.IsMatch(intPart))
            {
                result.Value = intValue - numValue / demoValue;
            }
            else
            {
                result.Value = intValue + numValue / demoValue;
            }

            result.ResolutionStr = result.Value.ToString();
            return result;
        }

        protected ParseResult DouParseChs(ExtractResult extResult)
        {
            var result = new ParseResult
            {
                Start = extResult.Start,
                Length = extResult.Length,
                Text = extResult.Text,
                Type = extResult.Type
            };

            var resultText = extResult.Text;

            if (Config.DoubleAndRoundChsRegex.IsMatch(resultText))
            {
                resultText = PatternUtil.ReplaceUnit(resultText, Culture.Chinese);
                result.Value = GetDigitValueChs(resultText.Substring(0, resultText.Length - 1),
                    Config.RoundNumberMapChs[resultText[resultText.Length - 1]]);
            }
            else
            {
                resultText = PatternUtil.ReplaceUnit(resultText, Culture.Chinese);
                var splitResult = Config.PointRegexChs.Split(resultText);

                if (splitResult[0] == "")
                {
                    splitResult[0] = "零";
                }

                if (Config.NegativeNumberSignRegex.IsMatch(splitResult[0]))
                {
                    result.Value = PatternUtil.GetIntValue(splitResult[0], Culture.Chinese) - PatternUtil.GetPointValue(splitResult[1], Culture.Chinese);
                }
                else
                {
                    result.Value = PatternUtil.GetIntValue(splitResult[0], Culture.Chinese) + PatternUtil.GetPointValue(splitResult[1], Culture.Chinese);
                }
            }

            result.ResolutionStr = result.Value.ToString();
            return result;
        }

        protected ParseResult IntParseChs(ExtractResult extResult)
        {
            var result = new ParseResult
            {
                Start = extResult.Start,
                Length = extResult.Length,
                Text = extResult.Text,
                Type = extResult.Type,
                Value = PatternUtil.GetIntValue(extResult.Text, Culture.Chinese)
            };

            result.ResolutionStr = result.Value.ToString();
            return result;
        }

        protected ParseResult PerParseChs(ExtractResult extResult)
        {
            var result = new ParseResult
            {
                Start = extResult.Start,
                Length = extResult.Length,
                Text = extResult.Text,
                Type = extResult.Type
            };

            var resultText = extResult.Text;
            long power = 1;

            if (extResult.Data.ToString().Contains("Spe"))
            {
                resultText = PatternUtil.NormalizeCharWidth(resultText, Culture.Chinese);
                resultText = PatternUtil.ReplaceUnit(resultText, Culture.Chinese);

                if (resultText == "半折")
                {
                    result.Value = 50;
                }
                else if (resultText == "10成")
                {
                    result.Value = 100;
                }
                else
                {
                    var matches = Config.SpeGetNumberRegex.Matches(resultText);
                    double intNumber;

                    if (matches.Count == 2)
                    {
                        var intNumberChar = matches[0].Value[0];

                        if (intNumberChar == '对')
                        {
                            intNumber = 5;
                        }
                        else if (intNumberChar == '十' || intNumberChar == '拾')
                        {
                            intNumber = 10;
                        }
                        else
                        {
                            intNumber = Config.ZeroToNineMapChs[intNumberChar];
                        }

                        var pointNumberChar = matches[1].Value[0];
                        double pointNumber;
                        if (pointNumberChar == '半')
                        {
                            pointNumber = 0.5;
                        }
                        else
                        {
                            pointNumber = Config.ZeroToNineMapChs[pointNumberChar] * 0.1;
                        }

                        result.Value = (intNumber + pointNumber) * 10;
                    }
                    else
                    {
                        var intNumberChar = matches[0].Value[0];

                        if (intNumberChar == '对')
                        {
                            intNumber = 5;
                        }
                        else if (intNumberChar == '十' || intNumberChar == '拾')
                        {
                            intNumber = 10;
                        }
                        else
                        {
                            intNumber = Config.ZeroToNineMapChs[intNumberChar];
                        }

                        result.Value = intNumber * 10;
                    }
                }
            }
            else if (extResult.Data.ToString().Contains("Num"))
            {
                var doubleText = Config.PercentageRegex.Match(resultText).Value;

                if (doubleText.Contains("k") || doubleText.Contains("K") || doubleText.Contains("ｋ") ||
                    doubleText.Contains("Ｋ"))
                {
                    power = 1000;
                }

                if (doubleText.Contains("M") || doubleText.Contains("Ｍ"))
                {
                    power = 1000000;
                }

                if (doubleText.Contains("G") || doubleText.Contains("Ｇ"))
                {
                    power = 1000000000;
                }

                if (doubleText.Contains("T") || doubleText.Contains("Ｔ"))
                {
                    power = 1000000000000;
                }

                result.Value = GetDigitValueChs(resultText, power);
            }
            else
            {
                var doubleText = Config.PercentageRegex.Match(resultText).Value;
                doubleText = PatternUtil.ReplaceUnit(doubleText, Culture.Chinese);

                var splitResult = Config.PointRegexChs.Split(doubleText);
                if (splitResult[0] == "")
                {
                    splitResult[0] = "零";
                }

                var doubleValue = PatternUtil.GetIntValue(splitResult[0], Culture.Chinese);
                if (splitResult.Length == 2)
                {
                    if (Config.NegativeNumberSignRegex.IsMatch(splitResult[0]))
                    {
                        doubleValue -= PatternUtil.GetPointValue(splitResult[1], Culture.Chinese);
                    }
                    else
                    {
                        doubleValue += PatternUtil.GetPointValue(splitResult[1], Culture.Chinese);
                    }
                }

                result.Value = doubleValue;
            }

            result.ResolutionStr = result.Value + @"%";
            return result;
        }

        protected ParseResult OrdParseChs(ExtractResult extResult)
        {
            var result = new ParseResult
            {
                Start = extResult.Start,
                Length = extResult.Length,
                Text = extResult.Text,
                Type = extResult.Type
            };

            var resultText = extResult.Text;
            resultText = resultText.Substring(1);

            result.Value = Config.DigitNumRegex.IsMatch(resultText)
                ? GetDigitValueChs(resultText, 1)
                : PatternUtil.GetIntValue(resultText, Culture.Chinese);

            result.ResolutionStr = result.Value.ToString();
            return result;
        }

        private double GetDigitValueChs(string intStr, double power)
        {
            var isNegative = false;
            if (Config.NegativeNumberSignRegex.IsMatch(intStr))
            {
                isNegative = true;
                intStr = intStr.Substring(1);
            }

            intStr = PatternUtil.NormalizeCharWidth(intStr, Culture.Chinese);
            var intValue = GetDigitalValue(intStr, power);
            if (isNegative)
            {
                intValue = -intValue;
            }

            return intValue;
        }
    }
}