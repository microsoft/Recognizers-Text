using Microsoft.Recognizers.Text.Number.Chinese;
using Microsoft.Recognizers.Text.Number.Japanese;
using System.Text;

namespace Microsoft.Recognizers.Text.Number
{
    public class PatternUtil
    {
        public static JapaneseNumberParserConfiguration ConfigJpn = new JapaneseNumberParserConfiguration();
        public static ChineseNumberParserConfiguration ConfigChs = new ChineseNumberParserConfiguration();
        public static BaseNumberParser baseNumberParser;

        // Replace full digtal numbers with half digtal numbers. "４" and "4" are both legal in Japanese, replace "４" with "4", then deal with "4"
        public static string NormalizeCharWidth(string text, string cultureCode)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return text;
            }

            var builder = new StringBuilder();
            foreach (char c in text)
            {

                if (cultureCode.Equals(Culture.Japanese))
                {
                    builder.Append(ConfigJpn.FullToHalfMap.ContainsKey(c) ? ConfigJpn.FullToHalfMap[c] : c);
                }
                else if (cultureCode.Equals(Culture.Chinese))
                {
                    builder.Append(ConfigChs.FullToHalfMapChs.ContainsKey(c) ? ConfigChs.FullToHalfMapChs[c] : c);
                }
            }

            return builder.ToString();
        }

        // Parse unit phrase. "万", "億",...
        public static string ReplaceUnit(string resultText, string cultureCode)
        {
            if (cultureCode.Equals(Culture.Japanese))
            {
                foreach (var unit in ConfigJpn.UnitMap.Keys)
                {
                    resultText = resultText.Replace(unit, ConfigJpn.UnitMap[unit]);
                }
            }
            else if (cultureCode.Equals(Culture.Chinese))
            {
                foreach (var unit in ConfigChs.UnitMapChs.Keys)
                {
                    resultText = resultText.Replace(unit, ConfigChs.UnitMapChs[unit]);
                }
            }

            return resultText;
        }

        public static double GetIntValue(string intStr, string cultureCode)
        {
            intStr = ReplaceUnit(intStr, cultureCode);
            double intValue = 0, partValue = 0, beforeValue = 1;
            var isRoundBefore = false;
            long roundBefore = -1, roundDefault = 1;
            var isNegative = false;

            if (cultureCode.Equals(Culture.Japanese))
            {
                var isDozen = false;
                var isPair = false;

                if (ConfigJpn.DozenRegex.IsMatch(intStr))
                {
                    isDozen = true;
                    intStr = intStr.Substring(0, intStr.Length - 3);
                }
                else if (ConfigJpn.PairRegex.IsMatch(intStr))
                {
                    isPair = true;
                    intStr = intStr.Substring(0, intStr.Length - 1);
                }

                if (ConfigJpn.NegativeNumberSignRegex.IsMatch(intStr))
                {
                    isNegative = true;
                    intStr = intStr.Substring(1);
                }

                for (var i = 0; i < intStr.Length; i++)
                {
                    if (ConfigJpn.RoundNumberMapChar.ContainsKey(intStr[i]))
                    {

                        var roundRecent = ConfigJpn.RoundNumberMapChar[intStr[i]];
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

                            if (i == intStr.Length - 1 || ConfigJpn.RoundDirectList.Contains(intStr[i]))
                            {
                                intValue += partValue;
                                partValue = 0;
                            }
                        }

                        roundDefault = roundRecent / 10;
                    }
                    else if (ConfigJpn.ZeroToNineMap.ContainsKey(intStr[i]))
                    {
                        if (i != intStr.Length - 1)
                        {
                            if (intStr[i] == '零' && !ConfigJpn.RoundNumberMapChar.ContainsKey(intStr[i + 1]))
                            {
                                beforeValue = 1;
                                roundDefault = 1;
                            }
                            else
                            {
                                beforeValue = ConfigJpn.ZeroToNineMap[intStr[i]];
                                isRoundBefore = false;
                            }
                        }
                        else
                        {
                            partValue += ConfigJpn.ZeroToNineMap[intStr[i]] * roundDefault;
                            intValue += partValue;
                            partValue = 0;
                        }
                    }
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
            }
            else if (cultureCode.Equals(Culture.Chinese))
            {
                var isDozen = false;
                var isPair = false;

                if (ConfigChs.DozenRegex.IsMatch(intStr))
                {
                    isDozen = true;
                    intStr = intStr.Substring(0, intStr.Length - 1);
                }
                else if (ConfigChs.PairRegex.IsMatch(intStr))
                {
                    isPair = true;
                    intStr = intStr.Substring(0, intStr.Length - 1);
                }

                if (ConfigChs.NegativeNumberSignRegex.IsMatch(intStr))
                {
                    isNegative = true;
                    intStr = intStr.Substring(1);
                }

                for (var i = 0; i < intStr.Length; i++)
                {
                    if (ConfigChs.RoundNumberMapChs.ContainsKey(intStr[i]))
                    {

                        var roundRecent = ConfigChs.RoundNumberMapChs[intStr[i]];
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

                            if (i == intStr.Length - 1 || ConfigChs.RoundDirectListChs.Contains(intStr[i]))
                            {
                                intValue += partValue;
                                partValue = 0;
                            }
                        }

                        roundDefault = roundRecent / 10;
                    }
                    else if (ConfigChs.ZeroToNineMapChs.ContainsKey(intStr[i]))
                    {
                        if (i != intStr.Length - 1)
                        {
                            if (intStr[i] == '零' && !ConfigChs.RoundNumberMapChs.ContainsKey(intStr[i + 1]))
                            {
                                beforeValue = 1;
                                roundDefault = 1;
                            }
                            else
                            {
                                beforeValue = ConfigChs.ZeroToNineMapChs[intStr[i]];
                                isRoundBefore = false;
                            }
                        }
                        else
                        {
                            partValue += ConfigChs.ZeroToNineMapChs[intStr[i]] * roundDefault;
                            intValue += partValue;
                            partValue = 0;
                        }
                    }
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
            }

            return intValue;
        }

        public static double GetPointValue(string pointStr, string cultureCode)
        {
            double pointValue = 0;
            var scale = 0.1;
            foreach (char c in pointStr)
            {
                if (cultureCode.Equals(Culture.Japanese))
                {
                    pointValue += scale * ConfigJpn.ZeroToNineMap[c];
                }
                else if (cultureCode.Equals(Culture.Chinese))
                {
                    pointValue += scale * ConfigChs.ZeroToNineMapChs[c];
                }

                scale *= 0.1;
            }

            return pointValue;
        }
    }

}
