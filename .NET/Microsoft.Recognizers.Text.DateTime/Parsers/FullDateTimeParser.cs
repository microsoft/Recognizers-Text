using System.Collections.Generic;
using System.Linq;

using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime
{
    public class FullDateTimeParser : IDateTimeParser
    {
        private readonly IFullDateTimeParserConfiguration config;

        public const string ParserTypeName = "datetimeV2";

        public FullDateTimeParser(IFullDateTimeParserConfiguration configuration)
        {
            config = configuration;
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
            var beforeMatch = config.BeforeRegex.Match(er.Text);
            var afterMatch = config.AfterRegex.Match(er.Text);
            var untilMatch = config.UntilRegex.Match(er.Text);
            var sinceMatchPrefix = config.SincePrefixRegex.Match(er.Text);
            var sinceMatchSuffix = config.SinceSuffixRegex.Match(er.Text);

            if (beforeMatch.Success && er.Text.EndsWith(beforeMatch.Value))
            {
                hasBefore = true;
                er.Length -= beforeMatch.Length;
                er.Text = er.Text.Substring(0, er.Length ?? 0);
                modStr = beforeMatch.Value;
            }
            else if (afterMatch.Success && er.Text.EndsWith(afterMatch.Value))
            {
                hasAfter = true;
                er.Length -= afterMatch.Length;
                er.Text = er.Text.Substring(0, er.Length ?? 0);
                modStr = afterMatch.Value;
            }
            else if (untilMatch.Success && untilMatch.Index == 0)
            {
                hasUntil = true;
                er.Start += untilMatch.Length;
                er.Length -= untilMatch.Length;
                er.Text = er.Text.Substring(untilMatch.Length);
                modStr = untilMatch.Value;
            }
            else
            {
                if (sinceMatchPrefix.Success && sinceMatchPrefix.Index == 0)
                {
                    hasSince = true;
                    er.Start += sinceMatchPrefix.Length;
                    er.Length -= sinceMatchPrefix.Length;
                    er.Text = er.Text.Substring(sinceMatchPrefix.Length);
                    modStrPrefix = sinceMatchPrefix.Value;
                }

                if (sinceMatchSuffix.Success && er.Text.EndsWith(sinceMatchSuffix.Value))
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
                val.Mod = TimeTypeConstants.BEFORE_MOD;
                pr.Value = val;
            }

            if (hasAfter)
            {
                pr.Length += modStr.Length;
                pr.Text = pr.Text + modStr;
                var val = (DateTimeResolutionResult)pr.Value;
                val.Mod = TimeTypeConstants.AFTER_MOD;
                pr.Value = val;
            }

            if (hasUntil)
            {
                pr.Length += modStr.Length;
                pr.Start -= modStr.Length;
                pr.Text = modStr + pr.Text;
                var val = (DateTimeResolutionResult)pr.Value;
                val.Mod = TimeTypeConstants.BEFORE_MOD;
                pr.Value = val;
                hasBefore = true;
            }

            if (hasSince)
            {
                pr.Length += modStrPrefix.Length + modStrSuffix.Length;
                pr.Start -= modStrPrefix.Length;
                pr.Text = modStrPrefix + pr.Text + modStrSuffix;
                var val = (DateTimeResolutionResult)pr.Value;
                val.Mod = TimeTypeConstants.SINCE_MOD;
                pr.Value = val;
            }

            pr.Value = DateTimeResolution(pr, hasBefore, hasAfter, hasSince);

            //change the type at last for the after or before mode
            pr.Type = $"{ParserTypeName}.{DetermineDateTimeType(er.Type, hasBefore, hasAfter, hasSince)}";

            return pr;
        }

        public string DetermineDateTimeType(string type, bool hasBefore, bool hasAfter, bool hasSince)
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
                res.Add(DateTimeResolutionKey.TimexKey, timex);
            }

            if (!string.IsNullOrEmpty(comment))
            {
                res.Add(DateTimeResolutionKey.CommentKey, comment);
            }

            if (!string.IsNullOrEmpty(mod))
            {
                res.Add(DateTimeResolutionKey.ModKey, mod);
            }

            if (!string.IsNullOrEmpty(type))
            {
                res.Add(ResolutionKey.TypeKey, typeOutput);
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
                    res.Add(DateTimeResolutionKey.ResolveKey, resolutionPast);
                }
            }
            else
            {
                if (resolutionPast.Count > 0)
                {
                    res.Add(DateTimeResolutionKey.ResolveToPastKey, resolutionPast);
                }

                if (resolutionFuture.Count > 0)
                {
                    res.Add(DateTimeResolutionKey.ResolveToFutureKey, resolutionFuture);
                }
            }

            // if ampm, double our resolution accordingly
            if (!string.IsNullOrEmpty(comment) && comment.Equals(Constants.Comment_AmPm))
            {
                if (res.ContainsKey(DateTimeResolutionKey.ResolveKey))
                {
                    ResolveAmpm(res, DateTimeResolutionKey.ResolveKey);
                }
                else
                {
                    ResolveAmpm(res, DateTimeResolutionKey.ResolveToPastKey);
                    ResolveAmpm(res, DateTimeResolutionKey.ResolveToFutureKey);
                }
            }

            if (isLunar)
            {
                res.Add(DateTimeResolutionKey.IsLunarKey, isLunar);
            }

            foreach (var p in res)
            {
                if (p.Value is Dictionary<string, string>)
                {
                    var value = new Dictionary<string, string>();

                    if (!string.IsNullOrEmpty(timex))
                    {
                        value.Add(DateTimeResolutionKey.TimexKey, timex);
                    }

                    if (!string.IsNullOrEmpty(mod))
                    {
                        value.Add(DateTimeResolutionKey.ModKey, mod);
                    }

                    if (!string.IsNullOrEmpty(type))
                    {
                        value.Add(ResolutionKey.TypeKey, typeOutput);
                    }

                    foreach (var q in (Dictionary<string, string>)p.Value)
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
                var notResolved = new Dictionary<string, string> {
                    {
                        DateTimeResolutionKey.TimexKey, timex
                    }, {
                        ResolutionKey.TypeKey, typeOutput
                    }, {
                        ResolutionKey.ValueKey, "not resolved"
                    }
                };

                resolutions.Add(notResolved);
            }

            return new SortedDictionary<string, object> { { ResolutionKey.ValueListKey, resolutions } };
        }

        internal void ResolveAmpm(Dictionary<string, object> resolutionDic,
            string keyName)
        {
            if (resolutionDic.ContainsKey(keyName))
            {
                var resolution = (Dictionary<string, string>)resolutionDic[keyName];
                if (!resolutionDic.ContainsKey(DateTimeResolutionKey.TimexKey))
                {
                    return;
                }

                var timex = (string)resolutionDic[DateTimeResolutionKey.TimexKey];
                resolutionDic.Remove(keyName);

                resolutionDic.Add(keyName + "Am", resolution);

                var resolutionPm = new Dictionary<string, string>();
                switch ((string)resolutionDic[ResolutionKey.TypeKey])
                {
                    case Constants.SYS_DATETIME_TIME:
                        resolutionPm[ResolutionKey.ValueKey] = FormatUtil.ToPm(resolution[ResolutionKey.ValueKey]);
                        resolutionPm[DateTimeResolutionKey.TimexKey] = FormatUtil.ToPm(timex);
                        break;
                    case Constants.SYS_DATETIME_DATETIME:
                        var splited = resolution[ResolutionKey.ValueKey].Split(' ');
                        resolutionPm[ResolutionKey.ValueKey] = splited[0] + " " + FormatUtil.ToPm(splited[1]);
                        resolutionPm[DateTimeResolutionKey.TimexKey] = FormatUtil.AllStringToPm(timex);
                        break;
                    case Constants.SYS_DATETIME_TIMEPERIOD:
                        if (resolution.ContainsKey(TimeTypeConstants.RESOLVE_START))
                        {
                            resolutionPm[TimeTypeConstants.RESOLVE_START] = FormatUtil.ToPm(resolution[TimeTypeConstants.RESOLVE_START]);
                        }

                        if (resolution.ContainsKey(TimeTypeConstants.RESOLVE_END))
                        {
                            resolutionPm[TimeTypeConstants.RESOLVE_END] = FormatUtil.ToPm(resolution[TimeTypeConstants.RESOLVE_END]);
                        }

                        resolutionPm[DateTimeResolutionKey.TimexKey] = FormatUtil.AllStringToPm(timex);
                        break;
                    case Constants.SYS_DATETIME_DATETIMEPERIOD:
                        splited = resolution[TimeTypeConstants.RESOLVE_START].Split(' ');
                        if (resolution.ContainsKey(TimeTypeConstants.RESOLVE_START))
                        {
                            resolutionPm[TimeTypeConstants.RESOLVE_START] = splited[0] + " " + FormatUtil.ToPm(splited[1]);
                        }

                        splited = resolution[TimeTypeConstants.RESOLVE_END].Split(' ');

                        if (resolution.ContainsKey(TimeTypeConstants.RESOLVE_END))
                        {
                            resolutionPm[TimeTypeConstants.RESOLVE_END] = splited[0] + " " + FormatUtil.ToPm(splited[1]);
                        }

                        resolutionPm[DateTimeResolutionKey.TimexKey] = FormatUtil.AllStringToPm(timex);
                        break;
                }

                resolutionDic.Add(keyName + "Pm", resolutionPm);
            }
        }

        internal Dictionary<string, string> GenerateResolution(string type, Dictionary<string, string> resolutionDic, string mod)
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
                    res.Add(ResolutionKey.ValueKey, resolutionDic[TimeTypeConstants.DURATION]);
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

        public void AddSingleDateTimeToResolution(Dictionary<string, string> resolutionDic,
            string type,
            string mod,
            Dictionary<string, string> res)
        {
            if (resolutionDic.ContainsKey(type))
            {
                if (!string.IsNullOrEmpty(mod))
                {
                    if (mod.Equals(TimeTypeConstants.BEFORE_MOD))
                    {
                        res.Add(TimeTypeConstants.RESOLVE_END, resolutionDic[type]);
                        return;
                    }

                    if (mod.Equals(TimeTypeConstants.AFTER_MOD))
                    {
                        res.Add(TimeTypeConstants.RESOLVE_START, resolutionDic[type]);
                        return;
                    }
                }

                res.Add(ResolutionKey.ValueKey, resolutionDic[type]);
            }
        }

        public void AddPeriodToResolution(Dictionary<string, string> resolutionDic, string startType, string endType, string mod,
            Dictionary<string, string> res)
        {
            var start = "";
            var end = "";

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
                //For before mode, the start of the period should be the end the new period, no start 
                if (mod.Equals(TimeTypeConstants.BEFORE_MOD))
                {
                    res.Add(TimeTypeConstants.RESOLVE_END, start);
                    return;
                }

                //For after mode, the end of the period should be the start the new period, no end 
                if (mod.Equals(TimeTypeConstants.AFTER_MOD))
                {
                    res.Add(TimeTypeConstants.RESOLVE_START, end);
                    return;
                }

                //For since mode, the start of the period should be the start the new period, no end 
                if (mod.Equals(TimeTypeConstants.SINCE_MOD))
                {
                    res.Add(TimeTypeConstants.RESOLVE_START, start);
                    return;
                }
            }

            if (!string.IsNullOrEmpty(start) && !string.IsNullOrEmpty(end))
            {
                res.Add(TimeTypeConstants.RESOLVE_START, start);
                res.Add(TimeTypeConstants.RESOLVE_END, end);
            }
        }
    }
}