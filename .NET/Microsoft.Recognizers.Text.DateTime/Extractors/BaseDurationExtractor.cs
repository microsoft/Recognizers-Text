using System.Collections.Generic;
using System.Text.RegularExpressions;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime
{
    public class BaseDurationExtractor : IDateTimeExtractor
    {
        public static readonly string ExtractorName = Constants.SYS_DATETIME_DURATION;

        private readonly IDurationExtractorConfiguration config;

        private readonly bool merge;

        public BaseDurationExtractor(IDurationExtractorConfiguration config, bool merge = true)
        {
            this.config = config;
            this.merge = merge;
        }

        public List<ExtractResult> Extract(string text)
        {
            return Extract(text, DateObject.Now);
        }

        public List<ExtractResult> Extract(string text, DateObject reference)
        {
            var tokens = new List<Token>();
            var numberWithUnitTokens = NumberWithUnit(text);

            tokens.AddRange(numberWithUnitTokens);
            tokens.AddRange(NumberWithUnitAndSuffix(text, numberWithUnitTokens));
            tokens.AddRange(ImplicitDuration(text));

            var rets = Token.MergeAllTokens(tokens, text, ExtractorName);

            // First MergeMultipleDuration then ResolveMoreThanOrLessThanPrefix so cases like "more than 4 days and less than 1 week" will not be merged into one "multipleDuration"
            if (this.merge)
            {
                rets = MergeMultipleDuration(text, rets);
            }

            rets = TagInequalityPrefix(text, rets);

            return rets;
        }

        // handle cases look like: {more than | less than} {duration}?
        private List<ExtractResult> TagInequalityPrefix(string text, List<ExtractResult> ers)
        {
            foreach (var er in ers)
            {
                var beforeString = text.Substring(0, (int)er.Start);
                bool isInequalityPrefixMatched = false;

                var match = config.MoreThanRegex.Match(beforeString);

                // The second condition is necessary so for "1 week" in "more than 4 days and less than 1 week", it will not be tagged incorrectly as "more than"
                if (match.Success && match.Index + match.Length == beforeString.Trim().Length)
                {
                    er.Data = Constants.MORE_THAN_MOD;
                    isInequalityPrefixMatched = true;
                }

                if (!isInequalityPrefixMatched)
                {
                    match = config.LessThanRegex.Match(beforeString);
                    if (match.Success && match.Index + match.Length == beforeString.Trim().Length)
                    {
                        er.Data = Constants.LESS_THAN_MOD;
                        isInequalityPrefixMatched = true;
                    }
                }

                if (isInequalityPrefixMatched)
                {
                    er.Length += er.Start - match.Index;
                    er.Start = match.Index;
                    er.Text = text.Substring((int)er.Start, (int)er.Length);
                }
            }

            return ers;
        }
        
        // handle cases look like: {number} {unit}? and {an|a} {half|quarter} {unit}?
        // define the part "and {an|a} {half|quarter}" as Suffix
        private List<Token> NumberWithUnitAndSuffix(string text, List<Token> ers)
        {
            var ret = new List<Token>();
            foreach (var er in ers)
            {
                var afterStr = text.Substring(er.Start + er.Length);
                var match = this.config.SuffixAndRegex.Match(afterStr);
                if (match.Success && match.Index == 0)
                {
                    ret.Add(new Token(er.Start, (er.Start + er.Length) + match.Length));
                }
            }
            return ret;
        }

        // simple cases made by a number followed an unit
        private List<Token> NumberWithUnit(string text)
        {
            var ret = new List<Token>();
            var ers = this.config.CardinalExtractor.Extract(text);
            foreach (var er in ers)
            {
                var afterStr = text.Substring(er.Start + er.Length ?? 0);
                var match = this.config.FollowedUnit.Match(afterStr);
                if (match.Success && match.Index == 0)
                {
                    ret.Add(new Token(er.Start ?? 0, (er.Start + er.Length ?? 0) + match.Length));
                }
            }

            // handle "3hrs"
            ret.AddRange(GetTokenFromRegex(config.NumberCombinedWithUnit, text));

            // handle "an hour"
            ret.AddRange(GetTokenFromRegex(config.AnUnitRegex, text));

            // handle "few" related cases
            ret.AddRange(GetTokenFromRegex(config.InexactNumberUnitRegex, text));

            return ret;
        }

        // handle cases that don't contain nubmer
        private List<Token> ImplicitDuration(string text)
        {
            var ret = new List<Token>();
            // handle "all day", "all year"
            ret.AddRange(GetTokenFromRegex(config.AllRegex, text));

            // handle "half day", "half year"
            ret.AddRange(GetTokenFromRegex(config.HalfRegex, text));

            // handle "next day", "last year"
            ret.AddRange(GetTokenFromRegex(config.RelativeDurationUnitRegex, text));

            // handle "during/for the day/week/month/year"
            if ((config.Options & DateTimeOptions.CalendarMode) != 0)
            {
                ret.AddRange(GetTokenFromRegex(config.DuringRegex, text));
            }

            return ret;
        }

        private static List<Token> GetTokenFromRegex(Regex regex, string text)
        {
            var ret = new List<Token>();
            var matches = regex.Matches(text);
            foreach (Match match in matches)
            {
                ret.Add(new Token(match.Index, match.Index + match.Length));
            }
            return ret;
        }

        private List<ExtractResult> MergeMultipleDuration(string text, List<ExtractResult> extractorResults)
        {
            if (extractorResults.Count <= 1)
            {
                return extractorResults;
            }

            var UnitMap = this.config.UnitMap;
            var UnitValueMap = this.config.UnitValueMap;
            var unitRegex = this.config.DurationUnitRegex;
            List<ExtractResult> ret = new List<ExtractResult>();

            var firstExtractionIndex = 0;
            var timeUnit = 0;
            var totalUnit = 0;
            while (firstExtractionIndex < extractorResults.Count)
            {
                string curUnit = null;
                var unitMatch = unitRegex.Match(extractorResults[firstExtractionIndex].Text);
                
                if (unitMatch.Success && UnitMap.ContainsKey(unitMatch.Groups["unit"].ToString()))
                {
                    curUnit = unitMatch.Groups["unit"].ToString();
                    totalUnit++;
                    if (DurationParsingUtil.IsTimeDurationUnit(UnitMap[curUnit]))
                    {
                        timeUnit++;
                    }
                }

                if (string.IsNullOrEmpty(curUnit))
                {
                    firstExtractionIndex++;
                    continue;
                }

                var secondExtractionIndex = firstExtractionIndex + 1;
                while (secondExtractionIndex < extractorResults.Count)
                {
                    var valid = false;
                    var midStrBegin = extractorResults[secondExtractionIndex - 1].Start + extractorResults[secondExtractionIndex - 1].Length ?? 0;
                    var midStrEnd = extractorResults[secondExtractionIndex].Start ?? 0;
                    var midStr = text.Substring(midStrBegin, midStrEnd - midStrBegin);
                    var match = this.config.DurationConnectorRegex.Match(midStr);
                    if (match.Success)
                    {
                        unitMatch = unitRegex.Match(extractorResults[secondExtractionIndex].Text);
                        if (unitMatch.Success && UnitMap.ContainsKey(unitMatch.Groups["unit"].ToString()))
                        {
                            var nextUnitStr = unitMatch.Groups["unit"].ToString();
                            if (UnitValueMap[nextUnitStr] != UnitValueMap[curUnit])
                            {
                                valid = true;
                                if (UnitValueMap[nextUnitStr] < UnitValueMap[curUnit])
                                {
                                    curUnit = nextUnitStr;
                                }
                            }

                            totalUnit++;
                            if (DurationParsingUtil.IsTimeDurationUnit(UnitMap[nextUnitStr]))
                            {
                                timeUnit++;
                            }
                        }
                    }

                    if (!valid)
                    {
                        break;
                    }

                    secondExtractionIndex++;
                }

                if (secondExtractionIndex - 1 > firstExtractionIndex)
                {
                    var node = new ExtractResult();
                    node.Start = extractorResults[firstExtractionIndex].Start;
                    node.Length = extractorResults[secondExtractionIndex - 1].Start + extractorResults[secondExtractionIndex - 1].Length - node.Start;
                    node.Text = text.Substring(node.Start?? 0, node.Length?? 0);
                    node.Type = extractorResults[firstExtractionIndex].Type;

                    // add multiple duration type to extract result
                    string type = null;
                    if (timeUnit == totalUnit)
                    {
                        type = Constants.MultipleDuration_Time;
                    }
                    else if (timeUnit == 0)
                    {
                        type = Constants.MultipleDuration_Date;
                    }
                    else
                    {
                        type = Constants.MultipleDuration_DateTime;
                    }
                    node.Data = type;

                    ret.Add(node);

                    timeUnit = 0;
                    totalUnit = 0;
                }
                else
                {
                    ret.Add(extractorResults[firstExtractionIndex]);
                }

                firstExtractionIndex = secondExtractionIndex;
            }

            return ret;
        }
    }
}
