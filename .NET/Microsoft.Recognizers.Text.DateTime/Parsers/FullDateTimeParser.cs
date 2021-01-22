using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Recognizers.Text.Utilities;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime
{
    public class FullDateTimeParser : IDateTimeParser
    {
        public const string ParserTypeName = "datetimeV2";

        private readonly IFullDateTimeParserConfiguration config;

        public FullDateTimeParser(IFullDateTimeParserConfiguration configuration)
        {
            config = configuration;
        }

        public static void AddSingleDateTimeToResolution(Dictionary<string, string> resolutionDic, string type,
                                                         string mod, Dictionary<string, string> res)
        {
            if (resolutionDic.ContainsKey(type) &&
                !resolutionDic[type].Equals(Constants.InvalidDateString, StringComparison.Ordinal))
            {
                if (!string.IsNullOrEmpty(mod))
                {
                    if (mod.Equals(Constants.BEFORE_MOD, StringComparison.Ordinal))
                    {
                        res.Add(DateTimeResolutionKey.End, resolutionDic[type]);
                        return;
                    }

                    if (mod.Equals(Constants.AFTER_MOD, StringComparison.Ordinal))
                    {
                        res.Add(DateTimeResolutionKey.Start, resolutionDic[type]);
                        return;
                    }
                }

                res.Add(ResolutionKey.Value, resolutionDic[type]);
            }
        }

        public static void AddPeriodToResolution(Dictionary<string, string> resolutionDic, string startType, string endType,
                                                 string mod, Dictionary<string, string> res)
        {
            var start = string.Empty;
            var end = string.Empty;

            if (resolutionDic.ContainsKey(startType))
            {
                start = resolutionDic[startType];
                if (start.Equals(Constants.InvalidDateString, StringComparison.Ordinal))
                {
                    return;
                }
            }

            if (resolutionDic.ContainsKey(endType))
            {
                end = resolutionDic[endType];
                if (end.Equals(Constants.InvalidDateString, StringComparison.Ordinal))
                {
                    return;
                }
            }

            if (!string.IsNullOrEmpty(mod))
            {
                // For before mode, the start of the period should be the end the new period, no start
                if (mod.Equals(Constants.BEFORE_MOD, StringComparison.Ordinal))
                {
                    res.Add(DateTimeResolutionKey.End, start);
                    return;
                }

                // For after mode, the end of the period should be the start the new period, no end
                if (mod.Equals(Constants.AFTER_MOD, StringComparison.Ordinal))
                {
                    res.Add(DateTimeResolutionKey.Start, end);
                    return;
                }

                // For since mode, the start of the period should be the start the new period, no end
                if (mod.Equals(Constants.SINCE_MOD, StringComparison.Ordinal))
                {
                    res.Add(DateTimeResolutionKey.Start, start);
                    return;
                }

                // For until mode, the end of the period should be the end the new period, no start
                if (mod.Equals(Constants.UNTIL_MOD, StringComparison.Ordinal))
                {
                    res.Add(DateTimeResolutionKey.End, start);
                    return;
                }
            }

            if (!string.IsNullOrEmpty(start) && !string.IsNullOrEmpty(end))
            {
                res.Add(DateTimeResolutionKey.Start, start);
                res.Add(DateTimeResolutionKey.End, end);
            }
        }

        public static string DetermineDateTimeType(string type, bool hasRangeChangingMod)
        {
            if (hasRangeChangingMod)
            {
                if (type.Equals(Constants.SYS_DATETIME_DATE, StringComparison.Ordinal))
                {
                    return Constants.SYS_DATETIME_DATEPERIOD;
                }

                if (type.Equals(Constants.SYS_DATETIME_TIME, StringComparison.Ordinal))
                {
                    return Constants.SYS_DATETIME_TIMEPERIOD;
                }

                if (type.Equals(Constants.SYS_DATETIME_DATETIME, StringComparison.Ordinal))
                {
                    return Constants.SYS_DATETIME_DATETIMEPERIOD;
                }
            }

            return type;
        }

        public static string DetermineSourceEntityType(string sourceType, string newType, bool hasMod)
        {
            if (!hasMod)
            {
                return null;
            }

            if (!newType.Equals(sourceType, StringComparison.Ordinal))
            {
                return Constants.SYS_DATETIME_DATETIMEPOINT;
            }

            if (newType.Equals(Constants.SYS_DATETIME_DATEPERIOD, StringComparison.Ordinal))
            {
                return Constants.SYS_DATETIME_DATETIMEPERIOD;
            }

            return null;
        }

        public ParseResult Parse(ExtractResult extResult)
        {
            return Parse(extResult, DateObject.Now);
        }

        public DateTimeParseResult Parse(ExtractResult er, DateObject referenceTime)
        {
            DateTimeParseResult pr = null;

            // push, save teh MOD string
            var hasInclusiveModifier = false;
            bool hasBefore = false, hasAfter = false, hasUntil = false, hasSince = false, hasEqual = false;
            string modStr = string.Empty, modStrPrefix = string.Empty, modStrSuffix = string.Empty;
            if (er.Metadata != null)
            {
                var beforeMatch = config.BeforeRegex.MatchEnd(er.Text, trim: true);
                var afterMatch = config.AfterRegex.MatchEnd(er.Text, trim: true);
                var untilMatch = config.UntilRegex.MatchBegin(er.Text, trim: true);
                var sinceMatchPrefix = config.SincePrefixRegex.MatchBegin(er.Text, trim: true);
                var sinceMatchSuffix = config.SinceSuffixRegex.MatchEnd(er.Text, trim: true);
                var equalMatch = config.EqualRegex.MatchBegin(er.Text, trim: true);

                if (beforeMatch.Success && !IsDurationWithAgoAndLater(er))
                {
                    hasBefore = true;
                    er.Length -= beforeMatch.Length;
                    er.Text = er.Text.Substring(0, er.Length ?? 0);
                    modStr = beforeMatch.Value;

                    if (!string.IsNullOrEmpty(beforeMatch.Groups[Constants.IncludeGroupName].Value))
                    {
                        hasInclusiveModifier = true;
                    }
                }
                else if (afterMatch.Success && !IsDurationWithAgoAndLater(er))
                {
                    hasAfter = true;
                    er.Length -= afterMatch.Length;
                    er.Text = er.Text.Substring(0, er.Length ?? 0);
                    modStr = afterMatch.Value;

                    if (!string.IsNullOrEmpty(afterMatch.Groups[Constants.IncludeGroupName].Value))
                    {
                        hasInclusiveModifier = true;
                    }
                }
                else if (untilMatch.Success)
                {
                    hasUntil = true;
                    er.Start += untilMatch.Length;
                    er.Length -= untilMatch.Length;
                    er.Text = er.Text.Substring(untilMatch.Length);
                    modStr = untilMatch.Value;
                }
                else if (equalMatch.Success)
                {
                    hasEqual = true;
                    er.Start += equalMatch.Length;
                    er.Length -= equalMatch.Length;
                    er.Text = er.Text.Substring(equalMatch.Length);
                    modStr = equalMatch.Value;
                }
                else
                {
                    if (sinceMatchPrefix.Success)
                    {
                        hasSince = true;
                        er.Start += sinceMatchPrefix.Length;
                        er.Length -= sinceMatchPrefix.Length;
                        er.Text = er.Text.Substring(sinceMatchPrefix.Length);
                        modStrPrefix = sinceMatchPrefix.Value;
                    }

                    if (sinceMatchSuffix.Success)
                    {
                        hasSince = true;
                        er.Length -= sinceMatchSuffix.Length;
                        er.Text = er.Text.Substring(0, er.Length ?? 0);
                        modStrSuffix = sinceMatchSuffix.Value;
                    }
                }
            }

            if (er.Type.Equals(Constants.SYS_DATETIME_DATE, StringComparison.Ordinal))
            {
                pr = config.DateParser.Parse(er, referenceTime);
                if (pr.Value == null)
                {
                    pr = config.HolidayParser.Parse(er, referenceTime);
                }
            }
            else if (er.Type.Equals(Constants.SYS_DATETIME_TIME, StringComparison.Ordinal))
            {
                pr = config.TimeParser.Parse(er, referenceTime);
            }
            else if (er.Type.Equals(Constants.SYS_DATETIME_DATETIME, StringComparison.Ordinal))
            {
                pr = config.DateTimeParser.Parse(er, referenceTime);
            }
            else if (er.Type.Equals(Constants.SYS_DATETIME_DATEPERIOD, StringComparison.Ordinal))
            {
                pr = config.DatePeriodParser.Parse(er, referenceTime);
            }
            else if (er.Type.Equals(Constants.SYS_DATETIME_TIMEPERIOD, StringComparison.Ordinal))
            {
                pr = config.TimePeriodParser.Parse(er, referenceTime);
            }
            else if (er.Type.Equals(Constants.SYS_DATETIME_DATETIMEPERIOD, StringComparison.Ordinal))
            {
                pr = config.DateTimePeriodParser.Parse(er, referenceTime);
            }
            else if (er.Type.Equals(Constants.SYS_DATETIME_DURATION, StringComparison.Ordinal))
            {
                pr = config.DurationParser.Parse(er, referenceTime);
            }
            else if (er.Type.Equals(Constants.SYS_DATETIME_SET, StringComparison.Ordinal))
            {
                pr = config.GetParser.Parse(er, referenceTime);
            }
            else
            {
                return null;
            }

            // pop, restore the MOD string
            if (hasBefore)
            {
                pr.Length += modStr.Length;
                pr.Text = pr.Text + modStr;
                var val = (DateTimeResolutionResult)pr.Value;

                val.Mod = CombineMod(val.Mod, !hasInclusiveModifier ? Constants.BEFORE_MOD : Constants.UNTIL_MOD);

                pr.Value = val;
            }

            if (hasAfter)
            {
                pr.Length += modStr.Length;
                pr.Text = pr.Text + modStr;
                var val = (DateTimeResolutionResult)pr.Value;

                val.Mod = CombineMod(val.Mod, !hasInclusiveModifier ? Constants.AFTER_MOD : Constants.SINCE_MOD);

                pr.Value = val;
            }

            if (hasUntil)
            {
                pr.Length += modStr.Length;
                pr.Start -= modStr.Length;
                pr.Text = modStr + pr.Text;
                var val = (DateTimeResolutionResult)pr.Value;
                val.Mod = Constants.BEFORE_MOD;
                pr.Value = val;
                hasBefore = true;
            }

            if (hasSince)
            {
                pr.Length += modStrPrefix.Length + modStrSuffix.Length;
                pr.Start -= modStrPrefix.Length;
                pr.Text = modStrPrefix + pr.Text + modStrSuffix;
                var val = (DateTimeResolutionResult)pr.Value;
                val.Mod = Constants.SINCE_MOD;
                pr.Value = val;
            }

            if (hasEqual)
            {
                pr.Length += modStr.Length;
                pr.Start -= modStr.Length;
                pr.Text = modStr + pr.Text;
            }

            var hasRangeChangingMod = hasBefore || hasAfter || hasSince;
            if (pr.Value != null)
            {
                ((DateTimeResolutionResult)pr.Value).HasRangeChangingMod = hasRangeChangingMod;
            }

            pr.Value = DateTimeResolution(pr, hasRangeChangingMod);

            // change the type at last for the after or before mode
            pr.Type = $"{ParserTypeName}.{DetermineDateTimeType(er.Type, hasRangeChangingMod)}";

            return pr;
        }

        public SortedDictionary<string, object> DateTimeResolution(DateTimeParseResult slot, bool hasRangeChangingMod)
        {
            var resolutions = new List<Dictionary<string, string>>();
            var res = new Dictionary<string, object>();

            var val = (DateTimeResolutionResult)slot.Value;
            if (val == null)
            {
                return null;
            }

            var type = slot.Type;
            var typeOutput = DetermineDateTimeType(slot.Type, hasRangeChangingMod);
            var sourceEntity = DetermineSourceEntityType(slot.Type, typeOutput, val.HasRangeChangingMod);
            var timex = slot.TimexStr;

            var isLunar = val.IsLunar;
            var mod = val.Mod;
            var comment = val.Comment;

            if (!string.IsNullOrEmpty(timex))
            {
                res.Add(DateTimeResolutionKey.Timex, timex);
            }

            if (!string.IsNullOrEmpty(comment))
            {
                res.Add(Constants.Comment, comment);
            }

            if (!string.IsNullOrEmpty(mod))
            {
                res.Add(DateTimeResolutionKey.Mod, mod);
            }

            if (!string.IsNullOrEmpty(type))
            {
                res.Add(ResolutionKey.Type, typeOutput);
            }

            var pastResolutionStr = ((DateTimeResolutionResult)slot.Value).PastResolution;
            var futureResolutionStr = ((DateTimeResolutionResult)slot.Value).FutureResolution;

            var resolutionPast = GenerateResolution(type, pastResolutionStr, mod);
            var resolutionFuture = GenerateResolution(type, futureResolutionStr, mod);

            // if past and future are same, keep only one
            if (resolutionFuture.OrderBy(t => t.Key).Select(t => t.Value).SequenceEqual(resolutionPast.OrderBy(t => t.Key).Select(t => t.Value)))
            {
                if (resolutionPast.Count > 0)
                {
                    res.Add(Constants.Resolve, resolutionPast);
                }
            }
            else
            {
                if (resolutionPast.Count > 0)
                {
                    res.Add(Constants.ResolveToPast, resolutionPast);
                }

                if (resolutionFuture.Count > 0)
                {
                    res.Add(Constants.ResolveToFuture, resolutionFuture);
                }
            }

            // if ampm, double our resolution accordingly
            if (!string.IsNullOrEmpty(comment) && comment.Equals(Constants.Comment_AmPm, StringComparison.Ordinal))
            {
                if (res.ContainsKey(Constants.Resolve))
                {
                    ResolveAmpm(res, Constants.Resolve);
                }
                else
                {
                    ResolveAmpm(res, Constants.ResolveToPast);
                    ResolveAmpm(res, Constants.ResolveToFuture);
                }
            }

            if (!string.IsNullOrEmpty(comment) && TimexUtility.HasDoubleTimex(comment))
            {
                ProcessDoubleTimex(res, Constants.ResolveToFuture, Constants.ResolveToPast, timex);
            }

            if (isLunar)
            {
                res.Add(DateTimeResolutionKey.IsLunar, isLunar);
            }

            foreach (var p in res)
            {
                if (p.Value is Dictionary<string, string> dictionary)
                {
                    var value = new Dictionary<string, string>();

                    if (!string.IsNullOrEmpty(timex))
                    {
                        value.Add(DateTimeResolutionKey.Timex, timex);
                    }

                    if (!string.IsNullOrEmpty(mod))
                    {
                        value.Add(DateTimeResolutionKey.Mod, mod);
                    }

                    if (!string.IsNullOrEmpty(type))
                    {
                        value.Add(ResolutionKey.Type, typeOutput);
                    }

                    if (!string.IsNullOrEmpty(sourceEntity))
                    {
                        value.Add(DateTimeResolutionKey.SourceEntity, sourceEntity);
                    }

                    foreach (var q in dictionary)
                    {
                        value[q.Key] = q.Value;
                    }

                    resolutions.Add(value);
                }
            }

            if (resolutionPast.Count == 0 && resolutionFuture.Count == 0)
            {
                var notResolved = new Dictionary<string, string>
                {
                    {
                        DateTimeResolutionKey.Timex, timex
                    },
                    {
                        ResolutionKey.Type, typeOutput
                    },
                    {
                        ResolutionKey.Value, "not resolved"
                    },
                };

                resolutions.Add(notResolved);
            }

            return new SortedDictionary<string, object> { { ResolutionKey.ValueSet, resolutions } };
        }

        public List<DateTimeParseResult> FilterResults(string query, List<DateTimeParseResult> candidateResults)
        {
            return candidateResults;
        }

        internal static void ProcessDoubleTimex(Dictionary<string, object> resolutionDic, string futureKey, string pastKey, string originTimex)
        {
            string[] timexes = originTimex.Split(Constants.CompositeTimexDelimiter);

            if (!resolutionDic.ContainsKey(futureKey) || !resolutionDic.ContainsKey(pastKey) || timexes.Length != 2)
            {
                return;
            }

            var futureResolution = (Dictionary<string, string>)resolutionDic[futureKey];
            var pastResolution = (Dictionary<string, string>)resolutionDic[pastKey];
            futureResolution[DateTimeResolutionKey.Timex] = timexes[0];
            pastResolution[DateTimeResolutionKey.Timex] = timexes[1];
        }

        internal static void ResolveAmpm(Dictionary<string, object> resolutionDic, string keyName)
        {
            if (resolutionDic.ContainsKey(keyName))
            {
                var resolution = (Dictionary<string, string>)resolutionDic[keyName];
                if (!resolutionDic.ContainsKey(DateTimeResolutionKey.Timex))
                {
                    return;
                }

                var timex = (string)resolutionDic[DateTimeResolutionKey.Timex];
                resolutionDic.Remove(keyName);

                resolutionDic.Add(keyName + "Am", resolution);

                var resolutionPm = new Dictionary<string, string>();
                switch ((string)resolutionDic[ResolutionKey.Type])
                {
                    case Constants.SYS_DATETIME_TIME:
                        resolutionPm[ResolutionKey.Value] = DateTimeFormatUtil.ToPm(resolution[ResolutionKey.Value]);
                        resolutionPm[DateTimeResolutionKey.Timex] = DateTimeFormatUtil.ToPm(timex);
                        break;
                    case Constants.SYS_DATETIME_DATETIME:
                        var splited = resolution[ResolutionKey.Value].Split(' ');
                        resolutionPm[ResolutionKey.Value] = splited[0] + " " + DateTimeFormatUtil.ToPm(splited[1]);
                        resolutionPm[DateTimeResolutionKey.Timex] = DateTimeFormatUtil.AllStringToPm(timex);
                        break;
                    case Constants.SYS_DATETIME_TIMEPERIOD:
                        if (resolution.ContainsKey(DateTimeResolutionKey.Start))
                        {
                            resolutionPm[DateTimeResolutionKey.Start] = DateTimeFormatUtil.ToPm(resolution[DateTimeResolutionKey.Start]);
                        }

                        if (resolution.ContainsKey(DateTimeResolutionKey.End))
                        {
                            resolutionPm[DateTimeResolutionKey.End] = DateTimeFormatUtil.ToPm(resolution[DateTimeResolutionKey.End]);
                        }

                        resolutionPm[DateTimeResolutionKey.Timex] = DateTimeFormatUtil.AllStringToPm(timex);
                        break;
                    case Constants.SYS_DATETIME_DATETIMEPERIOD:
                        if (resolution.ContainsKey(DateTimeResolutionKey.Start))
                        {
                            splited = resolution[DateTimeResolutionKey.Start].Split(' ');
                            resolutionPm[DateTimeResolutionKey.Start] = splited[0] + " " + DateTimeFormatUtil.ToPm(splited[1]);
                        }

                        if (resolution.ContainsKey(DateTimeResolutionKey.End))
                        {
                            splited = resolution[DateTimeResolutionKey.End].Split(' ');
                            resolutionPm[DateTimeResolutionKey.End] = splited[0] + " " + DateTimeFormatUtil.ToPm(splited[1]);
                        }

                        resolutionPm[DateTimeResolutionKey.Timex] = DateTimeFormatUtil.AllStringToPm(timex);
                        break;
                }

                resolutionDic.Add(keyName + "Pm", resolutionPm);
            }
        }

        internal static Dictionary<string, string> GenerateResolution(string type, Dictionary<string, string> resolutionDic, string mod)
        {
            var res = new Dictionary<string, string>();

            if (type.Equals(Constants.SYS_DATETIME_DATETIME, StringComparison.Ordinal))
            {
                AddSingleDateTimeToResolution(resolutionDic, TimeTypeConstants.DATETIME, mod, res);
            }
            else if (type.Equals(Constants.SYS_DATETIME_TIME, StringComparison.Ordinal))
            {
                AddSingleDateTimeToResolution(resolutionDic, TimeTypeConstants.TIME, mod, res);
            }
            else if (type.Equals(Constants.SYS_DATETIME_DATE, StringComparison.Ordinal))
            {
                AddSingleDateTimeToResolution(resolutionDic, TimeTypeConstants.DATE, mod, res);
            }
            else if (type.Equals(Constants.SYS_DATETIME_DURATION, StringComparison.Ordinal))
            {
                if (resolutionDic.ContainsKey(TimeTypeConstants.DURATION))
                {
                    res.Add(ResolutionKey.Value, resolutionDic[TimeTypeConstants.DURATION]);
                }
            }
            else if (type.Equals(Constants.SYS_DATETIME_TIMEPERIOD, StringComparison.Ordinal))
            {
                AddPeriodToResolution(resolutionDic, TimeTypeConstants.START_TIME, TimeTypeConstants.END_TIME, mod, res);
            }
            else if (type.Equals(Constants.SYS_DATETIME_DATEPERIOD, StringComparison.Ordinal))
            {
                AddPeriodToResolution(resolutionDic, TimeTypeConstants.START_DATE, TimeTypeConstants.END_DATE, mod, res);
            }
            else if (type.Equals(Constants.SYS_DATETIME_DATETIMEPERIOD, StringComparison.Ordinal))
            {
                AddPeriodToResolution(resolutionDic, TimeTypeConstants.START_DATETIME, TimeTypeConstants.END_DATETIME, mod, res);
            }

            return res;
        }

        private static string CombineMod(string originalMod, string newMod)
        {
            var combinedMod = newMod;

            if (!string.IsNullOrEmpty(originalMod))
            {
                combinedMod = $"{newMod}-{originalMod}";
            }

            return combinedMod;
        }

        private bool IsDurationWithAgoAndLater(ExtractResult er)
        {
            return er.Metadata != null && er.Metadata.IsDurationWithAgoAndLater;
        }
    }
}