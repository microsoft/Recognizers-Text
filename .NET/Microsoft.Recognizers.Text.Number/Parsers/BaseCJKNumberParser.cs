﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.Number
{
    public class BaseCJKNumberParser : BaseNumberParser
    {
        public BaseCJKNumberParser(INumberParserConfiguration config)
            : base(config)
        {
            this.Config = config as ICJKNumberParserConfiguration;
        }

        protected new ICJKNumberParserConfiguration Config { get; private set; }

        public override ParseResult Parse(ExtractResult extResult)
        {
            // Check if the parser is configured to support specific types
            if (SupportedTypes != null && !SupportedTypes.Any(t => extResult.Type.Equals(t, StringComparison.Ordinal)))
            {
                return null;
            }

            string extra = null;
            ParseResult ret = null;
            extra = extResult.Data as string;

            var getExtResult = new ExtractResult()
            {
                Start = extResult.Start,
                Length = extResult.Length,
                Data = extResult.Data,
                Text = extResult.Text,
                Type = extResult.Type,
                Metadata = extResult.Metadata,
            };

            if (Config.CultureInfo.Name == "zh-CN")
            {
                getExtResult.Text = ReplaceTraWithSim(getExtResult.Text);
            }

            if (extra == null)
            {
                return null;
            }

            if (extra.Contains("Per"))
            {
                ret = ParsePercentage(getExtResult);
            }
            else if (extra.Contains("Num"))
            {
                getExtResult.Text = NormalizeCharWidth(getExtResult.Text);
                ret = DigitNumberParse(getExtResult);
                if (Config.NegativeNumberSignRegex.IsMatch(getExtResult.Text) && (double)ret.Value > 0)
                {
                    ret.Value = -(double)ret.Value;
                }

                ret.ResolutionStr = ((double)ret.Value).ToString("G15", CultureInfo.InvariantCulture);
            }
            else if (extra.Contains("Pow"))
            {
                getExtResult.Text = NormalizeCharWidth(getExtResult.Text);
                ret = PowerNumberParse(getExtResult);
                ret.ResolutionStr = ((double)ret.Value).ToString("G15", CultureInfo.InvariantCulture);
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

            if (ret != null)
            {
                ret.Text = extResult.Text.ToLowerInvariant();
            }

            // Add "offset" and "relativeTo" for ordinal
            if (!string.IsNullOrEmpty(ret.Type) && ret.Type.Contains(Constants.MODEL_ORDINAL))
            {
                if (Config.RelativeReferenceOffsetMap.ContainsKey(extResult.Text) &&
                    Config.RelativeReferenceRelativeToMap.ContainsKey(extResult.Text))
                {
                    ret.Metadata.Offset = Config.RelativeReferenceOffsetMap[extResult.Text];
                    ret.Metadata.RelativeTo = Config.RelativeReferenceRelativeToMap[extResult.Text];
                    ret.Type = Constants.MODEL_ORDINAL_RELATIVE;
                }
                else
                {
                    ret.Metadata.Offset = ret.ResolutionStr;

                    // Every ordinal number is relative to the start
                    ret.Metadata.RelativeTo = Constants.RELATIVE_START;
                    ret.Type = Constants.MODEL_ORDINAL;
                }
            }

            // TODO: @Refactor this check to determine the subtype for JA and KO
            if ((Config.CultureInfo.Name == "ja-JP" || Config.CultureInfo.Name == "ko-KR") && ret != null)
            {
                ret.Type = DetermineType(extResult, ret);
                ret.Text = ret.Text.ToLowerInvariant();
            }

            return ret;
        }

        // Parse Fraction phrase.
        protected ParseResult ParseFraction(ExtractResult extResult)
        {
            var result = new ParseResult
            {
                Start = extResult.Start,
                Length = extResult.Length,
                Text = extResult.Text,
                Type = extResult.Type,
            };

            var resultText = extResult.Text;
            var splitResult = Config.FracSplitRegex.Split(resultText);
            string intPart = string.Empty, demoPart = string.Empty, numPart = string.Empty;

            if (splitResult.Length == 3)
            {
                intPart = splitResult[0];
                demoPart = splitResult[1];
                numPart = splitResult[2];
            }
            else if (splitResult.Length == 1)
            {
                // Needed to support "half" (eg. KO: 반, JA: 半)
                intPart = Config.ZeroChar.ToString(CultureInfo.InvariantCulture);
                demoPart = "2";
                numPart = "1";
            }
            else
            {
                intPart = Config.ZeroChar.ToString(CultureInfo.InvariantCulture);
                demoPart = splitResult[0];
                numPart = splitResult[1];
            }

            var intValue = Config.DigitNumRegex.IsMatch(intPart)
                ? GetDigitValue(intPart, 1.0)
                : GetIntValue(intPart);

            var numValue = Config.DigitNumRegex.IsMatch(numPart)
                ? GetDigitValue(numPart, 1.0)
                : (Config.PointRegex.IsMatch(numPart)
                ? GetIntValue(Config.PointRegex.Split(numPart)[0]) + GetPointValue(Config.PointRegex.Split(numPart)[1])
                : GetIntValue(numPart));

            var demoValue = Config.DigitNumRegex.IsMatch(demoPart)
                ? GetDigitValue(demoPart, 1.0)
                : GetIntValue(demoPart);

            if (Config.NegativeNumberSignRegex.IsMatch(intPart))
            {
                result.Value = intValue - (numValue / demoValue);
            }
            else
            {
                result.Value = intValue + (numValue / demoValue);
            }

            result.ResolutionStr = ((double)result.Value).ToString("G15", CultureInfo.InvariantCulture);

            return result;
        }

        // Parse percentage phrase.
        protected ParseResult ParsePercentage(ExtractResult extResult)
        {
            var result = new ParseResult
            {
                Start = extResult.Start,
                Length = extResult.Length,
                Text = extResult.Text,
                Type = extResult.Type,
            };

            var resultText = extResult.Text;
            long power = 1;

            if (extResult.Data.ToString().Contains("Spe"))
            {
                resultText = NormalizeCharWidth(resultText);
                resultText = ReplaceUnit(resultText);

                if (resultText == "半額" || resultText == "半値" || resultText == "半折")
                {
                    result.Value = 50d;
                }
                else if (resultText == "10成" || resultText == "10割" || resultText == "十割")
                {
                    result.Value = 100d;
                }
                else
                {
                    var matches = Config.SpeGetNumberRegex.Matches(resultText);
                    double intNumber;

                    if (matches.Count == 2)
                    {
                        var intNumberChar = matches[0].Value[0];

                        if (intNumberChar == Config.PairChar)
                        {
                            intNumber = 5;
                        }
                        else if (Config.TenChars.Contains(intNumberChar))
                        {
                            intNumber = 10;
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
                    else if (matches.Count == 5)
                    {
                        // Deal the Japanese percentage case like "xxx割xxx分xxx厘", get the integer value and convert into result.
                        var intNumberChar = matches[0].Value[0];
                        var pointNumberChar = matches[1].Value[0];
                        var dotNumberChar = matches[3].Value[0];

                        double pointNumber = Config.ZeroToNineMap[pointNumberChar] * 0.1;
                        double dotNumber = Config.ZeroToNineMap[dotNumberChar] * 0.01;

                        intNumber = Config.ZeroToNineMap[intNumberChar];

                        result.Value = (intNumber + pointNumber + dotNumber) * 10;
                    }
                    else
                    {
                        var intNumberChar = matches[0].Value[0];

                        if (intNumberChar == Config.PairChar)
                        {
                            intNumber = 5;
                        }
                        else if (Config.TenChars.Contains(intNumberChar))
                        {
                            intNumber = 10;
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
                doubleText = ReplaceUnit(doubleText);

                var splitResult = Config.PointRegex.Split(doubleText);
                if (string.IsNullOrEmpty(splitResult[0]))
                {
                    splitResult[0] = Config.ZeroChar.ToString(CultureInfo.InvariantCulture);
                }

                var doubleValue = GetIntValue(splitResult[0]);
                if (splitResult.Length == 2)
                {
                    if (Config.NegativeNumberSignRegex.IsMatch(splitResult[0]))
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

            if (Config.PercentageNumRegex != null)
            {
                var percentageNumSearch = Config.PercentageNumRegex.Match(resultText);
                if (percentageNumSearch.Length != 0)
                {
                    string demoPart = percentageNumSearch.Value;
                    var splitResult = Config.FracSplitRegex.Split(demoPart);
                    demoPart = splitResult[0];
                    var demoValue = Config.DigitNumRegex.IsMatch(demoPart)
                        ? GetDigitValue(demoPart, 1.0)
                        : GetIntValue(demoPart);

                    if (demoValue < 100 && demoValue > 0)
                    {
                        result.Value = (double)result.Value * (100 / demoValue);
                    }
                    else if (demoValue > 100)
                    {
                        result.Value = (double)result.Value / (demoValue / 100);
                    }
                }
            }

            result.ResolutionStr = ((double)result.Value).ToString("G15", CultureInfo.InvariantCulture) + @"%";
            return result;
        }

        // Parse ordinal phrase.
        protected ParseResult ParseOrdinal(ExtractResult extResult)
        {
            var result = new ParseResult
            {
                Start = extResult.Start,
                Length = extResult.Length,
                Text = extResult.Text,
                Type = extResult.Type,
                Metadata = extResult.Metadata,
            };

            var resultText = extResult.Text;
            resultText = resultText.Substring(1);

            result.Value = (Config.DigitNumRegex.IsMatch(resultText) && !Config.RoundNumberIntegerRegex.IsMatch(resultText))
                ? GetDigitValue(resultText, 1)
                : GetIntValue(resultText);

            result.ResolutionStr = ((double)result.Value).ToString("G15", CultureInfo.InvariantCulture);

            return result;
        }

        // Parse double phrase
        protected ParseResult ParseDouble(ExtractResult extResult)
        {
            var result = new ParseResult
            {
                Start = extResult.Start,
                Length = extResult.Length,
                Text = extResult.Text,
                Type = extResult.Type,
            };

            var resultText = extResult.Text;

            if (Config.DoubleAndRoundRegex.IsMatch(resultText))
            {
                resultText = ReplaceUnit(resultText);
                result.Value = GetDigitValue(
                    resultText.Substring(0, resultText.Length - 1),
                    Config.RoundNumberMapChar[resultText[resultText.Length - 1]]);
            }
            else
            {
                resultText = ReplaceUnit(resultText);
                var splitResult = Config.PointRegex.Split(resultText);

                if (string.IsNullOrEmpty(splitResult[0]))
                {
                    splitResult[0] = Config.ZeroChar.ToString(CultureInfo.InvariantCulture);
                }

                if (Config.NegativeNumberSignRegex.IsMatch(splitResult[0]))
                {
                    result.Value = GetIntValue(splitResult[0]) - GetPointValue(splitResult[1]);
                }
                else
                {
                    result.Value = GetIntValue(splitResult[0]) + GetPointValue(splitResult[1]);
                }
            }

            result.ResolutionStr = ((double)result.Value).ToString("G15", CultureInfo.InvariantCulture);
            return result;
        }

        // Parse integer phrase
        protected ParseResult ParseInteger(ExtractResult extResult)
        {
            var result = new ParseResult
            {
                Start = extResult.Start,
                Length = extResult.Length,
                Text = extResult.Text,
                Type = extResult.Type,
                Value = GetIntValue(extResult.Text),
            };

            result.ResolutionStr = result.Value.ToString();
            return result;
        }

        // Replace traditional Chinese characters with simplified Chinese ones.
        private string ReplaceTraWithSim(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return text;
            }

            var builder = new StringBuilder();
            foreach (char c in text)
            {
                builder.Append(Config.TratoSimMap.ContainsKey(c) ? Config.TratoSimMap[c] : c);
            }

            return builder.ToString();
        }

        private double GetDigitValue(string intStr, double power)
        {
            var isNegative = false;

            if (Config.NegativeNumberSignRegex.IsMatch(intStr))
            {
                isNegative = true;
                intStr = intStr.Substring(1);
            }

            intStr = NormalizeCharWidth(intStr);
            var intValue = GetDigitalValue(intStr, power);
            if (isNegative)
            {
                intValue = -intValue;
            }

            return intValue;
        }

        // Replace full digit numbers with half digit numbers. "４" and "4" are both legal in Japanese, replace "４" with "4", then deal with "4"
        private string NormalizeCharWidth(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return text;
            }

            var builder = new StringBuilder();
            foreach (char c in text)
            {
                builder.Append(Config.FullToHalfMap.ContainsKey(c) ? Config.FullToHalfMap[c] : c);
            }

            return builder.ToString();
        }

        // Parse unit phrase. "万", "億",...
        private string ReplaceUnit(string resultText)
        {
            foreach (var unit in Config.UnitMap.Keys)
            {
                resultText = resultText.Replace(unit, Config.UnitMap[unit]);
            }

            return resultText;
        }

        private double GetIntValue(string intStr)
        {
            intStr = ReplaceUnit(intStr);
            double intValue = 0, partValue = 0, beforeValue = 1;
            var isRoundBefore = false;
            long roundBefore = -1, roundDefault = 1;
            var isNegative = false;
            var hasPreviousDigits = false;

            var isDozen = false;
            var isPair = false;

            if (Config.DozenRegex.IsMatch(intStr))
            {
                isDozen = true;
                if (Config.CultureInfo.Name == "zh-CN")
                {
                    intStr = intStr.Substring(0, intStr.Length - 1);
                }
                else if (Config.CultureInfo.Name == "ja-JP")
                {
                    intStr = intStr.Substring(0, intStr.Length - 3);
                }
            }
            else if (Config.PairRegex.IsMatch(intStr))
            {
                isPair = true;
                intStr = intStr.Substring(0, intStr.Length - 1);
            }

            if (Config.NegativeNumberSignRegex.IsMatch(intStr))
            {
                isNegative = true;
                if (Config.CultureInfo.Name == "ko-KR")
                {
                    intStr = Regex.Replace(intStr, Config.NegativeNumberSignRegex.ToString(), string.Empty);
                }
                else
                {
                    intStr = intStr.Substring(1);
                }
            }

            for (var i = 0; i < intStr.Length; i++)
            {
                if (Config.RoundNumberMapChar.ContainsKey(intStr[i]))
                {
                    var roundRecent = Config.RoundNumberMapChar[intStr[i]];
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

                        if (i == intStr.Length - 1 || Config.RoundDirectList.Contains(intStr[i]))
                        {
                            intValue += partValue;
                            partValue = 0;
                        }
                    }

                    roundDefault = roundRecent / 10;
                }
                else if (Config.ZeroToNineMap.ContainsKey(intStr[i]))
                {
                    if (i != intStr.Length - 1)
                    {
                        var isNotRoundNext = Config.TenChars.Contains(intStr[i + 1]) || !Config.RoundNumberMapChar.ContainsKey(intStr[i + 1]);
                        if (intStr[i] == Config.ZeroChar && isNotRoundNext)
                        {
                            beforeValue = 1;
                            roundDefault = 1;
                        }
                        else
                        {
                            double currentDigit = Config.ZeroToNineMap[intStr[i]];
                            if (hasPreviousDigits)
                            {
                                beforeValue = (beforeValue * 10) + currentDigit;
                            }
                            else
                            {
                                beforeValue = currentDigit;
                            }

                            isRoundBefore = false;
                        }
                    }
                    else
                    {
                        // In colloquial Chinese, 百 may be omitted from the end of a number, similarly to how 一 can be dropped
                        // from the beginning. Japanese doesn't have such behaviour.
                        if ((Config.CultureInfo.Name == "ja-JP" || Config.CultureInfo.Name == "ko-KR") || char.IsDigit(intStr[i]))
                        {
                            roundDefault = 1;
                        }

                        double currentDigit = Config.ZeroToNineMap[intStr[i]];
                        if (hasPreviousDigits)
                        {
                            beforeValue = (beforeValue * 10) + currentDigit;
                        }
                        else
                        {
                            beforeValue = currentDigit;
                        }

                        partValue += beforeValue * roundDefault;
                        intValue += partValue;
                        partValue = 0;
                    }
                }

                hasPreviousDigits = char.IsDigit(intStr[i]);
            }

            if (isNegative)
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
            foreach (char c in pointStr)
            {
                pointValue += scale * Config.ZeroToNineMap[c];
                scale *= 0.1;
            }

            return pointValue;
        }
    }
}
