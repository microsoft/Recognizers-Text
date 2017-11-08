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

        public override ParseResult Parse(ExtractResult extResultChs)
        {
            string extra = null;
            ParseResult ret = null;
            extra = extResultChs.Data as string;

            var simplifiedExtResultChs = new ExtractResult()
            {
                Start = extResultChs.Start,
                Length = extResultChs.Length,
                Data = extResultChs.Data,
                Text = ReplaceTraWithSim(extResultChs.Text),
                Type = extResultChs.Type
            };

            if (extra == null) {
                return null;
            }

            if (extra.Contains("Per"))
            {
                ret = PerParseChs(simplifiedExtResultChs);
            }
            else if (extra.Contains("Num"))
            {
                simplifiedExtResultChs.Text = ReplaceFullWithHalf(simplifiedExtResultChs.Text);
                ret = DigitNumberParse(simplifiedExtResultChs);
                ret.ResolutionStr = ret.Value.ToString();
            }
            else if (extra.Contains("Pow"))
            {
                simplifiedExtResultChs.Text = ReplaceFullWithHalf(simplifiedExtResultChs.Text);
                ret = PowerNumberParse(simplifiedExtResultChs);
                ret.ResolutionStr = ret.Value.ToString();
            }
            else if (extra.Contains("Frac"))
            {
                ret = FracParseChs(simplifiedExtResultChs);
            }
            else if (extra.Contains("Dou"))
            {
                ret = DouParseChs(simplifiedExtResultChs);
            }
            else if (extra.Contains("Integer"))
            {
                ret = IntParseChs(simplifiedExtResultChs);
            }
            else if (extra.Contains("Ordinal"))
            {
                ret = OrdParseChs(simplifiedExtResultChs);
            }

            if (ret != null) {
                ret.Text = extResultChs.Text;
            }

            return ret;
        }

        private string ReplaceFullWithHalf(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return text;
            }

            var builder = new StringBuilder();
            foreach (char c in text) {
                builder.Append(Config.FullToHalfMapChs.ContainsKey(c) ? Config.FullToHalfMapChs[c] : c);
            }
            return builder.ToString();
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

        protected ParseResult FracParseChs(ExtractResult extResultChs)
        {
            var result = new ParseResult
            {
                Start = extResultChs.Start,
                Length = extResultChs.Length,
                Text = extResultChs.Text,
                Type = extResultChs.Type
            };

            var resultText = extResultChs.Text;
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
                : GetIntValueChs(intPart);

            var numValue = Config.DigitNumRegex.IsMatch(numPart)
                ? GetDigitValueChs(numPart, 1.0)
                : GetIntValueChs(numPart);

            var demoValue = Config.DigitNumRegex.IsMatch(demoPart)
                ? GetDigitValueChs(demoPart, 1.0)
                : GetIntValueChs(demoPart);

            if (Config.SymbolRegex.IsMatch(intPart))
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

        protected ParseResult DouParseChs(ExtractResult extResultChs)
        {
            var result = new ParseResult
            {
                Start = extResultChs.Start,
                Length = extResultChs.Length,
                Text = extResultChs.Text,
                Type = extResultChs.Type
            };

            var resultText = extResultChs.Text;

            if (Config.DoubleAndRoundChsRegex.IsMatch(resultText))
            {
                resultText = ReplaceUnit(resultText);
                result.Value = GetDigitValueChs(resultText.Substring(0, resultText.Length - 1),
                    Config.RoundNumberMapChs[resultText[resultText.Length - 1]]);
            }
            else
            {
                resultText = ReplaceUnit(resultText);
                var splitResult = Config.PointRegexChs.Split(resultText);

                if (splitResult[0] == "")
                {
                    splitResult[0] = "零";
                }

                if (Config.SymbolRegex.IsMatch(splitResult[0]))
                {
                    result.Value = GetIntValueChs(splitResult[0]) - GetPointValue(splitResult[1]);
                }
                else
                {
                    result.Value = GetIntValueChs(splitResult[0]) + GetPointValue(splitResult[1]);
                }
            }

            result.ResolutionStr = result.Value.ToString();
            return result;
        }

        private string ReplaceUnit(string resultText)
        {
            foreach (var unit in Config.UnitMapChs.Keys)
            {
                resultText = resultText.Replace(unit, Config.UnitMapChs[unit]);
            }
            return resultText;
        }

        protected ParseResult IntParseChs(ExtractResult extResultChs)
        {
            var result = new ParseResult
            {
                Start = extResultChs.Start,
                Length = extResultChs.Length,
                Text = extResultChs.Text,
                Type = extResultChs.Type,
                Value = GetIntValueChs(extResultChs.Text)
            };

            result.ResolutionStr = result.Value.ToString();
            return result;
        }

        protected ParseResult PerParseChs(ExtractResult extResultChs)
        {
            var result = new ParseResult
            {
                Start = extResultChs.Start,
                Length = extResultChs.Length,
                Text = extResultChs.Text,
                Type = extResultChs.Type
            };

            var resultText = extResultChs.Text;
            long power = 1;

            if (extResultChs.Data.ToString().Contains("Spe"))
            {
                resultText = ReplaceFullWithHalf(resultText);
                resultText = ReplaceUnit(resultText);

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
            else if (extResultChs.Data.ToString().Contains("Num"))
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
                doubleText = ReplaceUnit(doubleText);

                var splitResult = Config.PointRegexChs.Split(doubleText);
                if (splitResult[0] == "")
                {
                    splitResult[0] = "零";
                }

                var doubleValue = GetIntValueChs(splitResult[0]);
                if (splitResult.Length == 2)
                {
                    if (Config.SymbolRegex.IsMatch(splitResult[0]))
                    {
                        doubleValue -= GetPointValue(splitResult[1]);
                    }
                    else
                    {
                        doubleValue += GetPointValue(splitResult[1]);
                    }
                }

                result.Value = doubleValue;
            }

            result.ResolutionStr = result.Value + @"%";
            return result;
        }

        protected ParseResult OrdParseChs(ExtractResult extResultChs)
        {
            var result = new ParseResult
            {
                Start = extResultChs.Start,
                Length = extResultChs.Length,
                Text = extResultChs.Text,
                Type = extResultChs.Type
            };

            var resultText = extResultChs.Text;
            resultText = resultText.Substring(1);

            result.Value = Config.DigitNumRegex.IsMatch(resultText)
                ? GetDigitValueChs(resultText, 1)
                : GetIntValueChs(resultText);

            result.ResolutionStr = result.Value.ToString();
            return result;
        }

        private double GetDigitValueChs(string intStr, double power)
        {
            var isLessZero = false;
            if (Config.SymbolRegex.IsMatch(intStr))
            {
                isLessZero = true;
                intStr = intStr.Substring(1);
            }

            intStr = ReplaceFullWithHalf(intStr);
            var intValue = GetDigitalValue(intStr, power);
            if (isLessZero)
            {
                intValue = -intValue;
            }

            return intValue;
        }

        private double GetIntValueChs(string intStr)
        {
            var isDozen = false;
            var isPair = false;
            if (Config.DozenRegex.IsMatch(intStr))
            {
                isDozen = true;
                intStr = intStr.Substring(0, intStr.Length - 1);
            }
            else if (Config.PairRegex.IsMatch(intStr))
            {
                isPair = true;
                intStr = intStr.Substring(0, intStr.Length - 1);
            }

            intStr = ReplaceUnit(intStr);
            double intValue = 0, partValue = 0, beforeValue = 1;
            var isRoundBefore = false;
            long roundBefore = -1, roundDefault = 1;
            var isLessZero = false;

            if (Config.SymbolRegex.IsMatch(intStr))
            {
                isLessZero = true;
                intStr = intStr.Substring(1);
            }

            for (var i = 0; i < intStr.Length; i++)
            {
                if (Config.RoundNumberMapChs.ContainsKey(intStr[i]))
                {

                    var roundRecent = Config.RoundNumberMapChs[intStr[i]];
                    if (roundBefore != -1 && roundRecent > roundBefore)
                    {
                        if (isRoundBefore)
                        {
                            intValue += partValue * roundRecent;
                            isRoundBefore = false;
                        }
                        else
                        {
                            partValue += beforeValue * roundDefault;
                            intValue += partValue * roundRecent;
                        }
                        roundBefore = -1;
                        partValue = 0;
                    }
                    else
                    {
                        isRoundBefore = true;
                        partValue += beforeValue * roundRecent;
                        roundBefore = roundRecent;

                        if (i == intStr.Length - 1 || Config.RoundDirectListChs.Contains(intStr[i]))
                        {
                            intValue += partValue;
                            partValue = 0;
                        }
                    }

                    roundDefault = roundRecent / 10;
                }
                else if (Config.ZeroToNineMapChs.ContainsKey(intStr[i]))
                {
                    if (i != intStr.Length - 1)
                    {
                        if (intStr[i] == '零' && !Config.RoundNumberMapChs.ContainsKey(intStr[i + 1]))
                        {
                            beforeValue = 1;
                            roundDefault = 1;
                        }
                        else
                        {
                            beforeValue = Config.ZeroToNineMapChs[intStr[i]];
                            isRoundBefore = false;
                        }
                    }
                    else
                    {
                        partValue += Config.ZeroToNineMapChs[intStr[i]] * roundDefault;
                        intValue += partValue;
                        partValue = 0;
                    }
                }
            }

            if (isLessZero)
            {
                intValue = -intValue;
            }

            if (isDozen)
            {
                intValue = intValue * 12;
            }
            else if (isPair)
            {
                intValue = intValue * 2;
            }

            return intValue;
        }

        private double GetPointValue(string pointStr)
        {
            double pointValue = 0;
            var scale = 0.1;
            foreach (char c in pointStr) {
                pointValue += scale * Config.ZeroToNineMapChs[c];
                scale *= 0.1;
            }
            return pointValue;
        }
    }
}