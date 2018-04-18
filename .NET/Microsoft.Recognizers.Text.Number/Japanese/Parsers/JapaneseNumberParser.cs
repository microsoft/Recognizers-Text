using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Recognizers.Text.Number.Japanese
{
    public class JapaneseNumberParser : BaseNumberParser
    {
        protected new readonly JapaneseNumberParserConfiguration Config;

        public JapaneseNumberParser(JapaneseNumberParserConfiguration config) : base(config)
        {
            this.Config = config;
        }

        public override ParseResult Parse(ExtractResult extResult)
        {
            string extra = null;
            ParseResult ret = null;
            extra = extResult.Data as string;

            var getExtResult = new ExtractResult()
            {
                Start = extResult.Start,
                Length = extResult.Length,
                Data = extResult.Data,
                Text = extResult.Text,
                Type = extResult.Type
            };

            if (extra == null)
            {
                return null;
            }
            Console.WriteLine(getExtResult.Type);
            if (extra.Contains("Per"))
            {
                ret = ParsePercentage(getExtResult);
            }
            else if (extra.Contains("Num"))
            {
                getExtResult.Text = PatternUtil.NormalizeCharWidth(getExtResult.Text, Culture.Japanese);
                ret = DigitNumberParse(getExtResult);
                if (Config.NegativeNumberSignRegex.IsMatch(getExtResult.Text) && (double)ret.Value > 0)
                {
                    ret.Value = -(double)ret.Value;
                }
                ret.ResolutionStr = ret.Value.ToString();
            }
            else if (extra.Contains("Pow"))
            {
                getExtResult.Text = PatternUtil.NormalizeCharWidth(getExtResult.Text, Culture.Japanese);
                ret = PowerNumberParse(getExtResult);
                ret.ResolutionStr = ret.Value.ToString();
            }
            else if (extra.Contains("Frac"))
            {
                ret = ParseFraction(getExtResult);
            }
            else if (extra.Contains("Dou"))
            {
                ret = ParseDouble(getExtResult);
            }
            else if (extra.Contains("Integer"))
            {
                ret = ParseInteger(getExtResult);
            }
            else if (extra.Contains("Ordinal"))
            {
                ret = ParseOrdinal(getExtResult);
            }
            else


            if (ret != null)
            {
                ret.Text = extResult.Text;
            }

            return ret;
        }

        // Parse Japanese Fraction phrase. 
        protected ParseResult ParseFraction(ExtractResult extResult)
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
                ? GetDigitValue(intPart, 1.0)
                : PatternUtil.GetIntValue(intPart, Culture.Japanese);

            var numValue = Config.DigitNumRegex.IsMatch(numPart)
                ? GetDigitValue(numPart, 1.0)
                : PatternUtil.GetIntValue(numPart, Culture.Japanese);

            var demoValue = Config.DigitNumRegex.IsMatch(demoPart)
                ? GetDigitValue(demoPart, 1.0)
                : PatternUtil.GetIntValue(demoPart, Culture.Japanese);

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

        // Parse Japanese percentage phrase. 
        protected ParseResult ParsePercentage(ExtractResult extResult)
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
                resultText = PatternUtil.NormalizeCharWidth(resultText, Culture.Japanese);
                resultText = PatternUtil.ReplaceUnit(resultText, Culture.Japanese);

                if (resultText == "半額" || resultText == "半値")
                {
                    result.Value = 50;
                }
                else
                {
                    var matches = Config.SpeGetNumberRegex.Matches(resultText);
                    double intNumber;

                    if (matches.Count == 2)
                    {
                        var intNumberChar = matches[0].Value[0];

                        if (intNumberChar == '対')
                        {
                            intNumber = 5;
                        }
                        else
                        {
                            intNumber = Config.ZeroToNineMap[intNumberChar];
                        }

                        var pointNumberChar = matches[1].Value[0];
                        double pointNumber;
                        if (pointNumberChar == '半')
                        {
                            pointNumber = 0.5;
                        }
                        else
                        {
                            pointNumber = Config.ZeroToNineMap[pointNumberChar] * 0.1;
                        }

                        result.Value = (intNumber + pointNumber) * 10;
                    }
                    else
                    {
                        var intNumberChar = matches[0].Value[0];

                        if (intNumberChar == '対')
                        {
                            intNumber = 5;
                        }
                        else
                        {
                            intNumber = Config.ZeroToNineMap[intNumberChar];
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

                result.Value = GetDigitValue(resultText, power);
            }
            else
            {
                var doubleText = Config.PercentageRegex.Match(resultText).Value;
                doubleText = PatternUtil.ReplaceUnit(doubleText, Culture.Japanese);

                var splitResult = Config.PointRegex.Split(doubleText);
                if (splitResult[0] == "")
                {
                    splitResult[0] = "零";
                }

                var doubleValue = PatternUtil.GetIntValue(splitResult[0], Culture.Japanese);
                if (splitResult.Length == 2)
                {
                    if (Config.NegativeNumberSignRegex.IsMatch(splitResult[0]))
                    {
                        doubleValue -= PatternUtil.GetPointValue(splitResult[1], Culture.Japanese);
                    }
                    else
                    {
                        doubleValue += PatternUtil.GetPointValue(splitResult[1], Culture.Japanese);
                    }
                }

                result.Value = doubleValue;
            }

            result.ResolutionStr = result.Value + @"%";
            return result;
        }

        // Parse Japanese ordinal phrase. 
        protected ParseResult ParseOrdinal(ExtractResult extResult)
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
                ? GetDigitValue(resultText, 1)
                : PatternUtil.GetIntValue(resultText, Culture.Japanese);
            result.ResolutionStr = result.Value.ToString();

            return result;
        }

        // Parse Japanese double phrase
        protected ParseResult ParseDouble(ExtractResult extResult)
        {
            var result = new ParseResult
            {
                Start = extResult.Start,
                Length = extResult.Length,
                Text = extResult.Text,
                Type = extResult.Type
            };

            var resultText = extResult.Text;

            if (Config.DoubleAndRoundRegex.IsMatch(resultText))
            {
                resultText = PatternUtil.ReplaceUnit(resultText, Culture.Japanese);
                result.Value = GetDigitValue(resultText.Substring(0, resultText.Length - 1),
                    Config.RoundNumberMapChar[resultText[resultText.Length - 1]]);
            }
            else
            {
                resultText = PatternUtil.ReplaceUnit(resultText, Culture.Japanese);
                var splitResult = Config.PointRegex.Split(resultText);

                if (splitResult[0] == "")
                {
                    splitResult[0] = "零";
                }

                if (Config.NegativeNumberSignRegex.IsMatch(splitResult[0]))
                {
                    result.Value = PatternUtil.GetIntValue(splitResult[0], Culture.Japanese) - PatternUtil.GetPointValue(splitResult[1], Culture.Japanese);
                }
                else
                {
                    result.Value = PatternUtil.GetIntValue(splitResult[0], Culture.Japanese) + PatternUtil.GetPointValue(splitResult[1], Culture.Japanese);
                }
            }

            result.ResolutionStr = result.Value.ToString();
            return result;
        }

        // Parse Japanese integer phrase
        protected ParseResult ParseInteger(ExtractResult extResult)
        {
            var result = new ParseResult
            {
                Start = extResult.Start,
                Length = extResult.Length,
                Text = extResult.Text,
                Type = extResult.Type,
                Value = PatternUtil.GetIntValue(extResult.Text, Culture.Japanese)
            };

            result.ResolutionStr = result.Value.ToString();
            return result;
        }

        private double GetDigitValue(string intStr, double power)
        {
            var isNegative = false;

            if (Config.NegativeNumberSignRegex.IsMatch(intStr))
            {
                isNegative = true;
                intStr = intStr.Substring(1);
            }

            intStr = PatternUtil.NormalizeCharWidth(intStr, Culture.Japanese);
            var intValue = GetDigitalValue(intStr, power);
            if (isNegative)
            {
                intValue = -intValue;
            }

            return intValue;
        }
    }
}