using System.Collections.Generic;

using Microsoft.Recognizers.Text.Utilities;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime
{
    public class BaseCJKDurationExtractor : IDateTimeExtractor
    {
        public static readonly string ExtractorName = Constants.SYS_DATETIME_DURATION;

        private readonly ICJKDurationExtractorConfiguration config;

        private readonly bool merge;

        public BaseCJKDurationExtractor(ICJKDurationExtractorConfiguration config, bool merge = true)
        {
            this.config = config;
            this.merge = merge;
        }

        public List<ExtractResult> Extract(string text)
        {
            return Extract(text, DateObject.Now);
        }

        public List<ExtractResult> Extract(string source, DateObject referenceTime)
        {
            // Use Unit to extract
            var retList = this.config.InternalExtractor.Extract(source);
            var res = new List<ExtractResult>();
            foreach (var ret in retList)
            {
                // filter
                var match = this.config.YearRegex.Match(ret.Text);
                if (match.Success)
                {
                    continue;
                }

                res.Add(ret);
            }

            // handle "all day", "more days", "few days"
            res.AddRange(ImplicitDuration(source));

            res = ExtractResultExtension.MergeAllResults(res);

            if (this.merge)
            {
                res = MergeMultipleDuration(source, res);
            }

            return res;
        }

        private List<ExtractResult> MergeMultipleDuration(string text, List<ExtractResult> extractorResults)
        {
            if (extractorResults.Count <= 1)
            {
                return extractorResults;
            }

            var unitMap = this.config.UnitMap;
            var unitValueMap = this.config.UnitValueMap;
            var unitRegex = this.config.DurationUnitRegex;
            List<ExtractResult> ret = new List<ExtractResult>();

            var firstExtractionIndex = 0;
            var timeUnit = 0;
            var totalUnit = 0;
            while (firstExtractionIndex < extractorResults.Count)
            {
                string curUnit = null;
                var unitMatch = unitRegex.Match(extractorResults[firstExtractionIndex].Text);

                if (unitMatch.Success && unitMap.ContainsKey(unitMatch.Groups["unit"].ToString()))
                {
                    curUnit = unitMatch.Groups["unit"].ToString();
                    totalUnit++;
                    if (DurationParsingUtil.IsTimeDurationUnit(unitMap[curUnit]))
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
                    if (midStrBegin > midStrEnd)
                    {
                        return extractorResults;
                    }

                    var midStr = text.Substring(midStrBegin, midStrEnd - midStrBegin);
                    var match = this.config.DurationConnectorRegex.Match(midStr);
                    if (match.Success)
                    {
                        unitMatch = unitRegex.Match(extractorResults[secondExtractionIndex].Text);
                        if (unitMatch.Success && unitMap.ContainsKey(unitMatch.Groups["unit"].ToString()))
                        {
                            var nextUnitStr = unitMatch.Groups["unit"].ToString();
                            if (unitValueMap[unitMap[nextUnitStr]] != unitValueMap[unitMap[curUnit]])
                            {
                                valid = true;
                                if (unitValueMap[unitMap[nextUnitStr]] < unitValueMap[unitMap[curUnit]])
                                {
                                    curUnit = nextUnitStr;
                                }
                            }

                            totalUnit++;
                            if (DurationParsingUtil.IsTimeDurationUnit(unitMap[nextUnitStr]))
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
                    node.Text = text.Substring(node.Start ?? 0, node.Length ?? 0);
                    node.Type = extractorResults[firstExtractionIndex].Type;

                    // Add multiple duration type to extract result
                    string type = Constants.MultipleDuration_DateTime; // Default type
                    if (timeUnit == totalUnit)
                    {
                        type = Constants.MultipleDuration_Time;
                    }
                    else if (timeUnit == 0)
                    {
                        type = Constants.MultipleDuration_Date;
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

        private List<ExtractResult> ImplicitDuration(string text)
        {
            var ret = new List<Token>();

            // handle "all day", "all year"
            ret.AddRange(Token.GetTokenFromRegex(config.AllRegex, text));

            // handle "half day", "half year"
            ret.AddRange(Token.GetTokenFromRegex(config.HalfRegex, text));

            // handle "next day", "last year"
            ret.AddRange(Token.GetTokenFromRegex(config.RelativeDurationUnitRegex, text));

            // handle "more day", "more year"
            ret.AddRange(Token.GetTokenFromRegex(config.MoreOrLessRegex, text));

            // handle "few days", "few months"
            ret.AddRange(Token.GetTokenFromRegex(config.SomeRegex, text));

            // handle "during/for the day/week/month/year"
            if ((config.Options & DateTimeOptions.CalendarMode) != 0)
            {
                ret.AddRange(Token.GetTokenFromRegex(config.DuringRegex, text));
            }

            var result = new List<ExtractResult>();
            foreach (var e in ret)
            {
                var node = new ExtractResult();
                node.Start = e.Start;
                node.Length = e.Length;
                node.Text = text.Substring(node.Start ?? 0, node.Length ?? 0);
                node.Type = ExtractorName;

                result.Add(node);
            }

            return result;
        }
    }
}
