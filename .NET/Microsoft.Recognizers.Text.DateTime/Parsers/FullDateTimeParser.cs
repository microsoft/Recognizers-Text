using System.Collections.Generic;
using System.Linq;

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

        public static void AddSingleDateTimeToResolution(Dictionary<string, string> resolutionDic, string type, string mod, Dictionary<string, string> res)
        {
            if (resolutionDic.ContainsKey(type))
            {
                if (!string.IsNullOrEmpty(mod))
                {
                    if (mod.Equals(Constants.BEFORE_MOD))
                    {
                        res.Add(DateTimeResolutionKey.END, resolutionDic[type]);
                        return;
                    }

                    if (mod.Equals(Constants.AFTER_MOD))
                    {
                        res.Add(DateTimeResolutionKey.START, resolutionDic[type]);
                        return;
                    }
                }

                res.Add(ResolutionKey.Value, resolutionDic[type]);
            }
        }

        public static void AddPeriodToResolution(Dictionary<string, string> resolutionDic, string startType, string endType, string mod, Dictionary<string, string> res)
        {
            var start = string.Empty;
            var end = string.Empty;

            if (resolutionDic.ContainsKey(startType))
            {
                start = resolutionDic[startType];
            }

            if (resolutionDic.ContainsKey(endType))
            {
                end = resolutionDic[endType];
            }

            if (!string.IsNullOrEmpty(mod))
            {
                // For before mode, the start of the period should be the end the new period, no start
                if (mod.Equals(Constants.BEFORE_MOD))
                {
                    res.Add(DateTimeResolutionKey.END, start);
                    return;
                }

                // For after mode, the end of the period should be the start the new period, no end
                if (mod.Equals(Constants.AFTER_MOD))
                {
                    res.Add(DateTimeResolutionKey.START, end);
                    return;
                }

                // For since mode, the start of the period should be the start the new period, no end
                if (mod.Equals(Constants.SINCE_MOD))
                {
                    res.Add(DateTimeResolutionKey.START, start);
                    return;
                }

                // For until mode, the end of the period should be the end the new period, no start
                if (mod.Equals(Constants.UNTIL_MOD))
                {
                    res.Add(DateTimeResolutionKey.END, start);
                    return;
                }
            }

            if (!string.IsNullOrEmpty(start) && !string.IsNullOrEmpty(end))
            {
                res.Add(DateTimeResolutionKey.START, start);
                res.Add(DateTimeResolutionKey.END, end);
            }
        }

        public static string DetermineDateTimeType(string type, bool hasBefore, bool hasAfter, bool hasSince)
        {
            if (hasBefore || hasAfter || hasSince)
            {
                if (type.Equals(Constants.SYS_DATETIME_DATE))
                {
                    return Constants.SYS_DATETIME_DATEPERIOD;
                }

                if (type.Equals(Constants.SYS_DATETIME_TIME))
                {
                    return Constants.SYS_DATETIME_TIMEPERIOD;
                }

                if (type.Equals(Constants.SYS_DATETIME_DATETIME))
                {
                    return Constants.SYS_DATETIME_DATETIMEPERIOD;
                }
            }

            return type;
        }

        public ParseResult Parse(ExtractResult extResult)
        {
            return Parse(extResult, DateObject.Now);
        }

        public DateTimeParseResult Parse(ExtractResult er, DateObject referenceTime)
        {
            DateTimeParseResult pr = null;

            // push, save teh MOD string
            bool hasBefore = false, hasAfter = false, hasUntil = false, hasSince = false;
            string modStr = string.Empty, modStrPrefix = string.Empty, modStrSuffix = string.Empty;
            var beforeMatch = config.BeforeRegex.MatchEnd(er.Text, trim: true);
            var afterMatch = config.AfterRegex.MatchEnd(er.Text, trim: true);
            var untilMatch = config.UntilRegex.MatchBegin(er.Text, trim: true);
            var sinceMatchPrefix = config.SincePrefixRegex.MatchBegin(er.Text, trim: true);
            var sinceMatchSuffix = config.SinceSuffixRegex.MatchEnd(er.Text, trim: true);

            if (beforeMatch.Success && !IsDurationWithBeforeAndAfter(er))
            {
                hasBefore = true;
                er.Length -= beforeMatch.Length;
                er.Text = er.Text.Substring(0, er.Length ?? 0);
                modStr = beforeMatch.Value;
            }
            else if (afterMatch.Success && !IsDurationWithBeforeAndAfter(er))
            {
                hasAfter = true;
                er.Length -= afterMatch.Length;
                er.Text = er.Text.Substring(0, er.Length ?? 0);
                modStr = afterMatch.Value;
            }
            else if (untilMatch.Success)
            {
                hasUntil = true;
                er.Start += untilMatch.Length;
                er.Length -= untilMatch.Length;
                er.Text = er.Text.Substring(untilMatch.Length);
                modStr = untilMatch.Value;
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

            if (er.Type.Equals(Constants.SYS_DATETIME_DATE))
            {
                pr = config.DateParser.Parse(er, referenceTime);
                if (pr.Value == null)
                {
                    pr = config.HolidayParser.Parse(er, referenceTime);
                }
            }
            else if (er.Type.Equals(Constants.SYS_DATETIME_TIME))
            {
                pr = config.TimeParser.Parse(er, referenceTime);
            }
            else if (er.Type.Equals(Constants.SYS_DATETIME_DATETIME))
            {
                pr = config.DateTimeParser.Parse(er, referenceTime);
            }
            else if (er.Type.Equals(Constants.SYS_DATETIME_DATEPERIOD))
            {
                pr = config.DatePeriodParser.Parse(er, referenceTime);
            }
            else if (er.Type.Equals(Constants.SYS_DATETIME_TIMEPERIOD))
            {
                pr = config.TimePeriodParser.Parse(er, referenceTime);
            }
            else if (er.Type.Equals(Constants.SYS_DATETIME_DATETIMEPERIOD))
            {
                pr = config.DateTimePeriodParser.Parse(er, referenceTime);
            }
            else if (er.Type.Equals(Constants.SYS_DATETIME_DURATION))
            {
                pr = config.DurationParser.Parse(er, referenceTime);
            }
            else if (er.Type.Equals(Constants.SYS_DATETIME_SET))
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
                val.Mod = Constants.BEFORE_MOD;
                pr.Value = val;
            }

            if (hasAfter)
            {
                pr.Length += modStr.Length;
                pr.Text = pr.Text + modStr;
                var val = (DateTimeResolutionResult)pr.Value;
                val.Mod = Constants.AFTER_MOD;
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

            pr.Value = DateTimeResolution(pr, hasBefore, hasAfter, hasSince);

            // change the type at last for the after or before mode
            pr.Type = $"{ParserTypeName}.{DetermineDateTimeType(er.Type, hasBefore, hasAfter, hasSince)}";

            return pr;
        }

        public SortedDictionary<string, object> DateTimeResolution(DateTimeParseResult slot, bool hasBefore, bool hasAfter, bool hasSince)
        {
            var resolutions = new List<Dictionary<string, string>>();
            var res = new Dictionary<string, object>();

            var type = slot.Type;
            var typeOutput = DetermineDateTimeType(slot.Type, hasBefore, hasAfter, hasSince);
            var timex = slot.TimexStr;

            var val = (DateTimeResolutionResult)slot.Value;
            if (val == null)
            {
                return null;
            }

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
            if (!string.IsNullOrEmpty(comment) && comment.Equals(Constants.Comment_AmPm))
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

                    foreach (var q in dictionary)
                    {
                        if (value.ContainsKey(q.Key))
                        {
                            value[q.Key] = q.Value;
                        }
                        else
                        {
                            value.Add(q.Key, q.Value);
                        }
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
                        if (resolution.ContainsKey(DateTimeResolutionKey.START))
                        {
                            resolutionPm[DateTimeResolutionKey.START] = DateTimeFormatUtil.ToPm(resolution[DateTimeResolutionKey.START]);
                        }

                        if (resolution.ContainsKey(DateTimeResolutionKey.END))
                        {
                            resolutionPm[DateTimeResolutionKey.END] = DateTimeFormatUtil.ToPm(resolution[DateTimeResolutionKey.END]);
                        }

                        resolutionPm[DateTimeResolutionKey.Timex] = DateTimeFormatUtil.AllStringToPm(timex);
                        break;
                    case Constants.SYS_DATETIME_DATETIMEPERIOD:
                        splited = resolution[DateTimeResolutionKey.START].Split(' ');
                        if (resolution.ContainsKey(DateTimeResolutionKey.START))
                        {
                            resolutionPm[DateTimeResolutionKey.START] = splited[0] + " " + DateTimeFormatUtil.ToPm(splited[1]);
                        }

                        splited = resolution[DateTimeResolutionKey.END].Split(' ');

                        if (resolution.ContainsKey(DateTimeResolutionKey.END))
                        {
                            resolutionPm[DateTimeResolutionKey.END] = splited[0] + " " + DateTimeFormatUtil.ToPm(splited[1]);
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

            if (type.Equals(Constants.SYS_DATETIME_DATETIME))
            {
                AddSingleDateTimeToResolution(resolutionDic, TimeTypeConstants.DATETIME, mod, res);
            }
            else if (type.Equals(Constants.SYS_DATETIME_TIME))
            {
                AddSingleDateTimeToResolution(resolutionDic, TimeTypeConstants.TIME, mod, res);
            }
            else if (type.Equals(Constants.SYS_DATETIME_DATE))
            {
                AddSingleDateTimeToResolution(resolutionDic, TimeTypeConstants.DATE, mod, res);
            }
            else if (type.Equals(Constants.SYS_DATETIME_DURATION))
            {
                if (resolutionDic.ContainsKey(TimeTypeConstants.DURATION))
                {
                    res.Add(ResolutionKey.Value, resolutionDic[TimeTypeConstants.DURATION]);
                }
            }
            else if (type.Equals(Constants.SYS_DATETIME_TIMEPERIOD))
            {
                AddPeriodToResolution(resolutionDic, TimeTypeConstants.START_TIME, TimeTypeConstants.END_TIME, mod, res);
            }
            else if (type.Equals(Constants.SYS_DATETIME_DATEPERIOD))
            {
                AddPeriodToResolution(resolutionDic, TimeTypeConstants.START_DATE, TimeTypeConstants.END_DATE, mod, res);
            }
            else if (type.Equals(Constants.SYS_DATETIME_DATETIMEPERIOD))
            {
                AddPeriodToResolution(resolutionDic, TimeTypeConstants.START_DATETIME, TimeTypeConstants.END_DATETIME, mod, res);
            }

            return res;
        }

        private bool IsDurationWithBeforeAndAfter(ExtractResult er)
        {
            return er.Metadata != null && er.Metadata.IsDurationWithBeforeAndAfter;
        }
    }
}