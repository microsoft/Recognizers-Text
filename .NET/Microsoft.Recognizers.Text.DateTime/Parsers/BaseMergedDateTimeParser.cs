using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Text.RegularExpressions;

using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime
{
    public class BaseMergedDateTimeParser : IDateTimeParser
    {
        public const string ParserTypeName = "datetimeV2";

        protected readonly IMergedParserConfiguration Config;

        public static readonly string DateMinString = FormatUtil.FormatDate(DateObject.MinValue);
        public static readonly string DateTimeMinString = FormatUtil.FormatDateTime(DateObject.MinValue);
        private static readonly Calendar Cal = DateTimeFormatInfo.InvariantInfo.Calendar;

        public BaseMergedDateTimeParser(IMergedParserConfiguration configuration)
        {
            Config = configuration;
        }

        public ParseResult Parse(ExtractResult er)
        {
            return Parse(er, DateObject.Now);
        }

        public DateTimeParseResult Parse(ExtractResult er, DateObject refTime)
        {
            var referenceTime = refTime;
            DateTimeParseResult pr = null;

            // Push, save the MOD string
            bool hasBefore = false, hasAfter = false, hasSince = false, hasYearAfter = false;
            var modStr = string.Empty;
            var beforeMatch = Config.BeforeRegex.Match(er.Text);
            var afterMatch = Config.AfterRegex.Match(er.Text);
            var sinceMatch = Config.SinceRegex.Match(er.Text);
            
            if (beforeMatch.Success && beforeMatch.Index==0)
            {
                hasBefore = true;
                er.Start += beforeMatch.Length;
                er.Length -= beforeMatch.Length;
                er.Text = er.Text.Substring(beforeMatch.Length);
                modStr = beforeMatch.Value;
            }
            else if (afterMatch.Success && afterMatch.Index == 0)
            {
                hasAfter = true;
                er.Start += afterMatch.Length;
                er.Length -= afterMatch.Length;
                er.Text = er.Text.Substring(afterMatch.Length);
                modStr = afterMatch.Value;
            }
            else if (sinceMatch.Success && sinceMatch.Index == 0)
            {
                hasSince = true;
                er.Start += sinceMatch.Length;
                er.Length -= sinceMatch.Length;
                er.Text = er.Text.Substring(sinceMatch.Length);
                modStr = sinceMatch.Value;
            }
            else if (er.Type.Equals(Constants.SYS_DATETIME_DATEPERIOD) && Config.YearRegex.Match(er.Text).Success)
            {
                // This has to be put at the end of the if, or cases like "before 2012" and "after 2012" would fall into this
                // 2012 or after/above
                var match = Config.YearAfterRegex.Match(er.Text);
                if (match.Success && er.Text.EndsWith(match.Value))
                {
                    hasYearAfter = true;
                    er.Length -= match.Length;
                    er.Text = er.Text.Substring(0, er.Length ?? 0);
                    modStr = match.Value;
                }
            }

            if (er.Type.Equals(Constants.SYS_DATETIME_DATE))
            {
                pr = this.Config.DateParser.Parse(er, referenceTime);
                if (pr.Value == null)
                {
                    pr = Config.HolidayParser.Parse(er, referenceTime);
                }
            }
            else if (er.Type.Equals(Constants.SYS_DATETIME_TIME))
            {
                pr = this.Config.TimeParser.Parse(er, referenceTime);
            }
            else if (er.Type.Equals(Constants.SYS_DATETIME_DATETIME))
            {
                pr = this.Config.DateTimeParser.Parse(er, referenceTime);
            }
            else if (er.Type.Equals(Constants.SYS_DATETIME_DATEPERIOD))
            {
                pr = this.Config.DatePeriodParser.Parse(er, referenceTime);
            }
            else if (er.Type.Equals(Constants.SYS_DATETIME_TIMEPERIOD))
            {
                pr = this.Config.TimePeriodParser.Parse(er, referenceTime);
            }
            else if (er.Type.Equals(Constants.SYS_DATETIME_DATETIMEPERIOD))
            {
                pr = this.Config.DateTimePeriodParser.Parse(er, referenceTime);
            }
            else if (er.Type.Equals(Constants.SYS_DATETIME_DURATION))
            {
                pr = this.Config.DurationParser.Parse(er, referenceTime);
            }
            else if (er.Type.Equals(Constants.SYS_DATETIME_SET))
            {
                pr = this.Config.GetParser.Parse(er, referenceTime);
            }
            else if (er.Type.Equals(Constants.SYS_DATETIME_DATETIMEALT))
            {
                pr = this.Config.DateTimeAltParser.Parse(er, referenceTime);
            }
            else if (er.Type.Equals(Constants.SYS_DATETIME_TIMEZONE))
            {
                if ((Config.Options & DateTimeOptions.EnablePreview) != 0)
                {
                    pr = this.Config.TimeZoneParser.Parse(er, referenceTime);
                }
            }
            else
            {
                return null;
            }

            // Pop, restore the MOD string
            if (hasBefore && pr.Value != null)
            {
                pr.Length += modStr.Length;
                pr.Start -= modStr.Length;
                pr.Text = modStr + pr.Text;
                var val = (DateTimeResolutionResult) pr.Value;
                val.Mod = Constants.BEFORE_MOD;
                pr.Value = val;
            }

            if (hasAfter && pr.Value != null)
            {
                pr.Length += modStr.Length;
                pr.Start -= modStr.Length;
                pr.Text = modStr + pr.Text;
                var val = (DateTimeResolutionResult) pr.Value;
                val.Mod = Constants.AFTER_MOD;
                pr.Value = val;
            }

            if (hasSince && pr.Value != null)
            {
                pr.Length += modStr.Length;
                pr.Start -= modStr.Length;
                pr.Text = modStr + pr.Text;
                var val = (DateTimeResolutionResult)pr.Value;
                val.Mod = Constants.SINCE_MOD;
                pr.Value = val;
            }

            if (hasYearAfter && pr.Value != null)
            {
                pr.Length += modStr.Length;
                pr.Text = pr.Text + modStr;
                var val = (DateTimeResolutionResult)pr.Value;
                val.Mod = Constants.SINCE_MOD;
                pr.Value = val;
                hasSince = true;
            }

            if ((Config.Options & DateTimeOptions.SplitDateAndTime) != 0 && 
                ((DateTimeResolutionResult)pr.Value)?.SubDateTimeEntities != null)
            {
                pr.Value = DateTimeResolutionForSplit(pr);
            }
            else
            {
                pr = SetParseResult(pr, hasBefore, hasAfter, hasSince);
            }

            return pr;
        }

        public List<DateTimeParseResult> FilterResults(string query, List<DateTimeParseResult> candidateResults)
        {

            if (candidateResults != null && candidateResults.Any())
            { 

                var matches = Config.AmbiguousMonthP0Regex.Matches(query);

                foreach (Match match in matches)
                {

                    // Check for intersections/overlaps
                    candidateResults = candidateResults.Where(c => !( match.Index < c.Start + c.Length &&
                                                                      c.Start < match.Index + match.Length )).ToList();
                }

            }

            return candidateResults;
        }

        public DateTimeParseResult SetParseResult(DateTimeParseResult slot, bool hasBefore, bool hasAfter, bool hasSince)
        {
            slot.Value = DateTimeResolution(slot, hasBefore, hasAfter, hasSince);

            // Change the type at last for the after or before modes
            slot.Type = $"{ParserTypeName}.{DetermineDateTimeType(slot.Type, hasBefore, hasAfter, hasSince)}";
            return slot;
        }

        public string DetermineDateTimeType(string type, bool hasBefore, bool hasAfter, bool hasSince)
        {
            if ((Config.Options & DateTimeOptions.SplitDateAndTime) != 0)
            {
                if (type.Equals(Constants.SYS_DATETIME_DATETIME))
                {
                    return Constants.SYS_DATETIME_TIME;
                }
            }
            else
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
            }

            return type;
        }

        public List<DateTimeParseResult> DateTimeResolutionForSplit(DateTimeParseResult slot)
        {
            var results = new List<DateTimeParseResult>();
            if (((DateTimeResolutionResult) slot.Value).SubDateTimeEntities != null)
            {
                var subEntities = ((DateTimeResolutionResult) slot.Value).SubDateTimeEntities;
                foreach (var subEntity in subEntities)
                {
                    var result = (DateTimeParseResult) subEntity;
                    results.AddRange(DateTimeResolutionForSplit(result));
                }
            }
            else
            {
                slot.Value = DateTimeResolution(slot, false, false, false);
                slot.Type = $"{ParserTypeName}.{DetermineDateTimeType(slot.Type, false, false, false)}";
                results.Add(slot);
            }

            return results;
        }

        public SortedDictionary<string, object> DateTimeResolution(DateTimeParseResult slot, bool hasBefore, bool hasAfter, bool hasSince)
        {
            if (slot == null)
            {
                return null;
            }
            var resolutions = new List<Dictionary<string, string>>();
            var res = new Dictionary<string, object>();

            var type = slot.Type;
            var typeOutput = DetermineDateTimeType(slot.Type, hasBefore, hasAfter, hasSince);
            var timex = slot.TimexStr;

            var val = (DateTimeResolutionResult) slot.Value;
            if (val == null)
            {
                return null;
            }

            var islunar = val.IsLunar;
            var mod = val.Mod;
            var comment = val.Comment;

            // The following should be added to res first, since ResolveAmPm requires these fields.
            AddResolutionFields(res, DateTimeResolutionKey.Timex, timex);
            AddResolutionFields(res, Constants.Comment, comment);
            AddResolutionFields(res, DateTimeResolutionKey.Mod, mod);
            AddResolutionFields(res, ResolutionKey.Type, typeOutput);
            AddResolutionFields(res, DateTimeResolutionKey.IsLunar, islunar? islunar.ToString():string.Empty);

            var pastResolutionStr = ((DateTimeResolutionResult) slot.Value).PastResolution;
            var futureResolutionStr = ((DateTimeResolutionResult) slot.Value).FutureResolution;

            if (typeOutput == Constants.SYS_DATETIME_DATETIMEALT && pastResolutionStr.Count > 0)
            {
                typeOutput = DetermineResolutionDateTimeType(pastResolutionStr);
            }

            var resolutionPast = GenerateResolution(type, pastResolutionStr, mod);
            var resolutionFuture = GenerateResolution(type, futureResolutionStr, mod);

            // If past and future are same, keep only one
            if (resolutionFuture.OrderBy(t => t.Key).Select(t => t.Value)
                .SequenceEqual(resolutionPast.OrderBy(t => t.Key).Select(t => t.Value)))
            {
                if (resolutionPast.Count > 0)
                {
                    AddResolutionFields(res, Constants.Resolve, resolutionPast);
                }
            }
            else
            {
                if (resolutionPast.Count > 0)
                {
                    AddResolutionFields(res, Constants.ResolveToPast, resolutionPast);
                }

                if (resolutionFuture.Count > 0)
                {
                    AddResolutionFields(res, Constants.ResolveToFuture, resolutionFuture);
                }
            }

            // If 'ampm', double our resolution accordingly
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

            // If WeekOf and in CalendarMode, modify the past part of our resolution
            if ((Config.Options & DateTimeOptions.CalendarMode) != 0 && 
                !string.IsNullOrEmpty(comment) && comment.Equals(Constants.Comment_WeekOf))
            {
                ResolveWeekOf(res, Constants.ResolveToPast); 
            }

            if (val.TimeZoneResolution != null)
            {
                var timeZoneResolution = new Dictionary<string, string>();
                timeZoneResolution.Add(ResolutionKey.Value, val.TimeZoneResolution.Value);
                timeZoneResolution.Add(Constants.UtcOffsetMinsKey, val.TimeZoneResolution.UtcOffsetMins.ToString());

                AddResolutionFields(res, Constants.ResolveTimeZone, timeZoneResolution);
            }

            foreach (var p in res)
            {
                if (p.Value is Dictionary<string, string>)
                {
                    var value = new Dictionary<string, string>();

                    AddResolutionFields(value, DateTimeResolutionKey.Timex, timex);
                    AddResolutionFields(value, DateTimeResolutionKey.Mod, mod);
                    AddResolutionFields(value, ResolutionKey.Type, typeOutput);
                    AddResolutionFields(value, DateTimeResolutionKey.IsLunar, islunar ? islunar.ToString() : string.Empty);

                    foreach (var q in (Dictionary<string, string>) p.Value)
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

            if (resolutionPast.Count == 0 && resolutionFuture.Count == 0 && val.TimeZoneResolution == null)
            {
                var notResolved = new Dictionary<string, string> {
                    {
                        DateTimeResolutionKey.Timex, timex
                    },
                    {
                        ResolutionKey.Type, typeOutput
                    },
                    {
                        ResolutionKey.Value, "not resolved"
                    }
                };

                resolutions.Add(notResolved);
            }

            return new SortedDictionary<string, object> { { ResolutionKey.ValueSet, resolutions } };
        }

        private string DetermineResolutionDateTimeType(Dictionary<string, string> pastResolutionStr)
        {
            switch (pastResolutionStr.Keys.First())
            {
                case TimeTypeConstants.START_DATE:
                    return Constants.SYS_DATETIME_DATEPERIOD;

                case TimeTypeConstants.START_DATETIME:
                    return Constants.SYS_DATETIME_DATETIMEPERIOD;

                case TimeTypeConstants.START_TIME:
                    return Constants.SYS_DATETIME_TIMEPERIOD;

                default:
                    return pastResolutionStr.Keys.First().ToLower();
            }
        }

        internal void AddResolutionFields(Dictionary<string, object> dic, string key, object value)
        {
            if (value is string)
            {
                if (!string.IsNullOrEmpty((string)value))
                {
                    dic.Add(key, value);
                }
            }
            else
            {
                dic.Add(key, value);
            }
        }

        internal void AddResolutionFields(Dictionary<string, string> dic, string key, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                dic.Add(key, value);
            }
        }

        internal void ResolveAmpm(Dictionary<string, object> resolutionDic, string keyName)
        {
            if (resolutionDic.ContainsKey(keyName))
            {
                var resolution = (Dictionary<string, string>) resolutionDic[keyName];
                if (!resolutionDic.ContainsKey(DateTimeResolutionKey.Timex))
                {
                    return;
                }

                var timex = (string) resolutionDic[DateTimeResolutionKey.Timex];
                resolutionDic.Remove(keyName);

                resolutionDic.Add(keyName + "Am", resolution);

                var resolutionPm = new Dictionary<string, string>();

                switch ((string) resolutionDic[ResolutionKey.Type])
                {
                    case Constants.SYS_DATETIME_TIME:
                        resolutionPm[ResolutionKey.Value] = FormatUtil.ToPm(resolution[ResolutionKey.Value]);
                        resolutionPm[DateTimeResolutionKey.Timex] = FormatUtil.ToPm(timex);
                        break;

                    case Constants.SYS_DATETIME_DATETIME:
                        var splited = resolution[ResolutionKey.Value].Split(' ');
                        resolutionPm[ResolutionKey.Value] = splited[0] + " " + FormatUtil.ToPm(splited[1]);
                        resolutionPm[DateTimeResolutionKey.Timex] = FormatUtil.AllStringToPm(timex);
                        break;

                    case Constants.SYS_DATETIME_TIMEPERIOD:
                        if (resolution.ContainsKey(DateTimeResolutionKey.START))
                        {
                            resolutionPm[DateTimeResolutionKey.START] = FormatUtil.ToPm(resolution[DateTimeResolutionKey.START]);
                        }

                        if (resolution.ContainsKey(DateTimeResolutionKey.END))
                        {
                            resolutionPm[DateTimeResolutionKey.END] = FormatUtil.ToPm(resolution[DateTimeResolutionKey.END]);
                        }

                        resolutionPm[DateTimeResolutionKey.Timex] = FormatUtil.AllStringToPm(timex);
                        break;

                    case Constants.SYS_DATETIME_DATETIMEPERIOD:
                        if (resolution.ContainsKey(DateTimeResolutionKey.START))
                        {
                            splited = resolution[DateTimeResolutionKey.START].Split(' ');
                            if (resolution.ContainsKey(DateTimeResolutionKey.START))
                            {
                                resolutionPm[DateTimeResolutionKey.START] = splited[0] + " " + FormatUtil.ToPm(splited[1]);
                            }
                        }

                        if (resolution.ContainsKey(DateTimeResolutionKey.END))
                        {
                            splited = resolution[DateTimeResolutionKey.END].Split(' ');

                            if (resolution.ContainsKey(DateTimeResolutionKey.END))
                            {
                                resolutionPm[DateTimeResolutionKey.END] = splited[0] + " " + FormatUtil.ToPm(splited[1]);
                            }
                        }

                        resolutionPm[DateTimeResolutionKey.Timex] = FormatUtil.AllStringToPm(timex);
                        break;
                }

                resolutionDic.Add(keyName + "Pm", resolutionPm);
            }
        }

        internal void ResolveWeekOf(Dictionary<string, object> resolutionDic, string keyName)
        {
            if (resolutionDic.ContainsKey(keyName))
            {
                var resolution = (Dictionary<string, string>)resolutionDic[keyName];

                var monday = DateObject.Parse(resolution[DateTimeResolutionKey.START]);
                resolution[DateTimeResolutionKey.Timex] = FormatUtil.ToIsoWeekTimex(monday);

                resolutionDic.Remove(keyName);
                resolutionDic.Add(keyName, resolution);
            }
        }

        internal Dictionary<string, string> GenerateResolution(string type, Dictionary<string, string> resolutionDic, string mod)
        {
            var res = new Dictionary<string, string>();

            if (type.Equals(Constants.SYS_DATETIME_DATETIME))
            {
                AddSingleDateTimeToResolution(resolutionDic, TimeTypeConstants.DATETIME,mod, res);
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
            else if (type.Equals(Constants.SYS_DATETIME_DATETIMEALT))
            {
                // for a period
                if (resolutionDic.Count > 2)
                {
                    AddAltPeriodToResolution(resolutionDic, mod, res);
                }
                else
                {
                    // for a datetime point
                    AddAltSingleDateTimeToResolution(resolutionDic, TimeTypeConstants.DATETIMEALT, mod, res);
                }
            }

            return res;
        }

        public void AddAltPeriodToResolution(Dictionary<string, string> resolutionDic, string mod, Dictionary<string, string> res)
        {
            if (resolutionDic.ContainsKey(TimeTypeConstants.START_DATETIME) && resolutionDic.ContainsKey(TimeTypeConstants.END_DATETIME))
            {
                AddPeriodToResolution(resolutionDic, TimeTypeConstants.START_DATETIME, TimeTypeConstants.END_DATETIME, mod, res);
            }
            else if (resolutionDic.ContainsKey(TimeTypeConstants.START_DATE) && resolutionDic.ContainsKey(TimeTypeConstants.END_DATE))
            {
                AddPeriodToResolution(resolutionDic, TimeTypeConstants.START_DATE, TimeTypeConstants.END_DATE, mod, res);
            }
            else if (resolutionDic.ContainsKey(TimeTypeConstants.START_TIME) && resolutionDic.ContainsKey(TimeTypeConstants.END_TIME))
            {
                AddPeriodToResolution(resolutionDic, TimeTypeConstants.START_TIME, TimeTypeConstants.END_TIME, mod, res);
            }

        }

        public void AddAltSingleDateTimeToResolution(Dictionary<string, string> resolutionDic, string type, string mod,
            Dictionary<string, string> res)
        {
            if (resolutionDic.ContainsKey(TimeTypeConstants.DATE))
            {
                AddSingleDateTimeToResolution(resolutionDic, TimeTypeConstants.DATE, mod, res);
            }
            else if (resolutionDic.ContainsKey(TimeTypeConstants.DATETIME))
            {
                AddSingleDateTimeToResolution(resolutionDic, TimeTypeConstants.DATETIME, mod, res);
            }
            else if (resolutionDic.ContainsKey(TimeTypeConstants.TIME))
            {
                AddSingleDateTimeToResolution(resolutionDic, TimeTypeConstants.TIME, mod, res);
            }

        }

        public void AddSingleDateTimeToResolution(Dictionary<string, string> resolutionDic, string type, string mod, 
            Dictionary<string, string> res)
        {
            if (resolutionDic.ContainsKey(type) && 
                !resolutionDic[type].Equals(DateMinString) && !resolutionDic[type].Equals(DateTimeMinString))
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

                    if (mod.Equals(Constants.SINCE_MOD))
                    {
                        res.Add(DateTimeResolutionKey.START, resolutionDic[type]);
                        return;
                    }
                }

                res.Add(ResolutionKey.Value, resolutionDic[type]);
            }
        }

        public void AddPeriodToResolution(Dictionary<string, string> resolutionDic, string startType, string endType,
            string mod, Dictionary<string, string> res)
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
                // For the 'before' mod, the start of the period should be the end the new period, not the start 
                if (mod.Equals(Constants.BEFORE_MOD))
                {
                    res.Add(DateTimeResolutionKey.END, start);
                    return;
                }

                // For the 'after' mod, the end of the period should be the start the new period, not the end 
                if (mod.Equals(Constants.AFTER_MOD))
                {
                    res.Add(DateTimeResolutionKey.START, end);
                    return;
                }

                // For the 'since' mod, the start of the period should be the start the new period, not the end 
                if (mod.Equals(Constants.SINCE_MOD))
                {
                    res.Add(DateTimeResolutionKey.START, start);
                    return;
                }
            }

            if (!string.IsNullOrEmpty(start) && !string.IsNullOrEmpty(end))
            {
                res.Add(DateTimeResolutionKey.START, start);
                res.Add(DateTimeResolutionKey.END, end);
            }
        }
    }
}