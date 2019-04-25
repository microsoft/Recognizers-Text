using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime
{
    public class BaseMergedDateTimeParser : IDateTimeParser
    {
        public const string ParserTypeName = "datetimeV2";

        public static readonly string DateMinString = DateTimeFormatUtil.FormatDate(DateObject.MinValue);
        public static readonly string DateTimeMinString = DateTimeFormatUtil.FormatDateTime(DateObject.MinValue);
        private static readonly Calendar Cal = DateTimeFormatInfo.InvariantInfo.Calendar;

        public BaseMergedDateTimeParser(IMergedParserConfiguration configuration)
        {
            Config = configuration;
        }

        protected IMergedParserConfiguration Config { get; private set; }

        public static void AddAltSingleDateTimeToResolution(Dictionary<string, string> resolutionDic, string type, string mod, Dictionary<string, string> res)
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

        public static void AddSingleDateTimeToResolution(Dictionary<string, string> resolutionDic, string type, string mod, Dictionary<string, string> res)
        {
            if (resolutionDic.ContainsKey(type) &&
                !resolutionDic[type].Equals(DateMinString) && !resolutionDic[type].Equals(DateTimeMinString))
            {
                if (!string.IsNullOrEmpty(mod))
                {
                    if (mod.StartsWith(Constants.BEFORE_MOD))
                    {
                        res.Add(DateTimeResolutionKey.END, resolutionDic[type]);
                        return;
                    }

                    if (mod.StartsWith(Constants.AFTER_MOD))
                    {
                        res.Add(DateTimeResolutionKey.START, resolutionDic[type]);
                        return;
                    }

                    if (mod.StartsWith(Constants.SINCE_MOD))
                    {
                        res.Add(DateTimeResolutionKey.START, resolutionDic[type]);
                        return;
                    }

                    if (mod.StartsWith(Constants.UNTIL_MOD))
                    {
                        res.Add(DateTimeResolutionKey.END, resolutionDic[type]);
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
                // For the 'before' mod
                // 1. Cases like "Before December", the start of the period should be the end of the new period, not the start
                // 2. Cases like "More than 3 days before today", the date point should be the end of the new period
                if (mod.StartsWith(Constants.BEFORE_MOD))
                {
                    if (!string.IsNullOrEmpty(start) && !string.IsNullOrEmpty(end))
                    {
                        res.Add(DateTimeResolutionKey.END, start);
                    }
                    else
                    {
                        res.Add(DateTimeResolutionKey.END, end);
                    }

                    return;
                }

                // For the 'after' mod
                // 1. Cases like "After January", the end of the period should be the start of the new period, not the end
                // 2. Cases like "More than 3 days after today", the date point should be the start of the new period
                if (mod.StartsWith(Constants.AFTER_MOD))
                {
                    if (!string.IsNullOrEmpty(start) && !string.IsNullOrEmpty(end))
                    {
                        res.Add(DateTimeResolutionKey.START, end);
                    }
                    else
                    {
                        res.Add(DateTimeResolutionKey.START, start);
                    }

                    return;
                }

                // For the 'since' mod, the start of the period should be the start of the new period, not the end
                if (mod.StartsWith(Constants.SINCE_MOD))
                {
                    res.Add(DateTimeResolutionKey.START, start);
                    return;
                }

                // For the 'until' mod, the end of the period should be the end of the new period, not the start
                if (mod.StartsWith(Constants.UNTIL_MOD))
                {
                    res.Add(DateTimeResolutionKey.END, end);
                    return;
                }
            }

            if (!string.IsNullOrEmpty(start) && !string.IsNullOrEmpty(end))
            {
                res.Add(DateTimeResolutionKey.START, start);
                res.Add(DateTimeResolutionKey.END, end);
            }
        }

        public static string GenerateEndInclusiveTimex(string originalTimex, DatePeriodTimexType datePeriodTimexType, DateObject startDate, DateObject endDate)
        {
            var timexEndInclusive = TimexUtility.GenerateDatePeriodTimex(startDate, endDate, datePeriodTimexType);

            // Sometimes the original timex contains fuzzy part like "XXXX-05-31"
            // The fuzzy part needs to stay the same in the new end-inclusive timex
            if (originalTimex.Contains(Constants.TimexFuzzy) && originalTimex.Length == timexEndInclusive.Length)
            {
                var timexCharSet = new char[timexEndInclusive.Length];

                for (int i = 0; i < originalTimex.Length; i++)
                {
                    if (originalTimex[i] != Constants.TimexFuzzy)
                    {
                        timexCharSet[i] = timexEndInclusive[i];
                    }
                    else
                    {
                        timexCharSet[i] = Constants.TimexFuzzy;
                    }
                }

                timexEndInclusive = new string(timexCharSet);
            }

            return timexEndInclusive;
        }

        public static DateTimeParseResult SetInclusivePeriodEnd(DateTimeParseResult slot)
        {
            if (slot.Type == $"{ParserTypeName}.{Constants.SYS_DATETIME_DATEPERIOD}")
            {
                var timexComponents = slot.TimexStr.Split(Constants.DatePeriodTimexSplitter, StringSplitOptions.RemoveEmptyEntries);

                // Only handle DatePeriod like "(StartDate,EndDate,Duration)"
                if (timexComponents.Length == 3)
                {
                    var value = (SortedDictionary<string, object>)slot.Value;
                    var altTimex = string.Empty;

                    if (value != null && value.ContainsKey(ResolutionKey.ValueSet))
                    {
                        if (value[ResolutionKey.ValueSet] is IList<Dictionary<string, string>> valueSet && valueSet.Any())
                        {
                            foreach (var values in valueSet)
                            {
                                // This is only a sanity check, as here we only handle DatePeriod like "(StartDate,EndDate,Duration)"
                                if (values.ContainsKey(DateTimeResolutionKey.START) && values.ContainsKey(DateTimeResolutionKey.END) &&
                                    values.ContainsKey(DateTimeResolutionKey.Timex))
                                {
                                    var startDate = DateObject.Parse(values[DateTimeResolutionKey.START]);
                                    var endDate = DateObject.Parse(values[DateTimeResolutionKey.END]);
                                    var durationStr = timexComponents[2];
                                    var datePeriodTimexType = TimexUtility.GetDatePeriodTimexType(durationStr);
                                    endDate = TimexUtility.OffsetDateObject(endDate, offset: 1, timexType: datePeriodTimexType);
                                    values[DateTimeResolutionKey.END] = DateTimeFormatUtil.LuisDate(endDate);
                                    values[DateTimeResolutionKey.Timex] =
                                        GenerateEndInclusiveTimex(slot.TimexStr, datePeriodTimexType, startDate, endDate);

                                    if (string.IsNullOrEmpty(altTimex))
                                    {
                                        altTimex = values[DateTimeResolutionKey.Timex];
                                    }
                                }
                            }
                        }
                    }

                    slot.Value = value;
                    slot.TimexStr = altTimex;
                }
            }

            return slot;
        }

        public static void AddAltPeriodToResolution(Dictionary<string, string> resolutionDic, string mod, Dictionary<string, string> res)
        {
            if (resolutionDic.ContainsKey(TimeTypeConstants.START_DATETIME) || resolutionDic.ContainsKey(TimeTypeConstants.END_DATETIME))
            {
                AddPeriodToResolution(resolutionDic, TimeTypeConstants.START_DATETIME, TimeTypeConstants.END_DATETIME, mod, res);
            }
            else if (resolutionDic.ContainsKey(TimeTypeConstants.START_DATE) || resolutionDic.ContainsKey(TimeTypeConstants.END_DATE))
            {
                AddPeriodToResolution(resolutionDic, TimeTypeConstants.START_DATE, TimeTypeConstants.END_DATE, mod, res);
            }
            else if (resolutionDic.ContainsKey(TimeTypeConstants.START_TIME) || resolutionDic.ContainsKey(TimeTypeConstants.END_TIME))
            {
                AddPeriodToResolution(resolutionDic, TimeTypeConstants.START_TIME, TimeTypeConstants.END_TIME, mod, res);
            }
        }

        public ParseResult Parse(ExtractResult er)
        {
            return Parse(er, DateObject.Now);
        }

        public DateTimeParseResult Parse(ExtractResult er, DateObject refTime)
        {
            var referenceTime = refTime;
            DateTimeParseResult pr = null;

            var originText = er.Text;
            if ((this.Config.Options & DateTimeOptions.EnablePreview) != 0)
            {
                er.Text = MatchingUtil.PreProcessTextRemoveSuperfluousWords(er.Text, Config.SuperfluousWordMatcher, out var _);
                er.Length += er.Text.Length - originText.Length;
            }

            // Push, save the MOD string
            bool hasBefore = false, hasAfter = false, hasSince = false, hasAround = false, hasDateAfter = false;

            // "InclusiveModifier" means MOD should include the start/end time
            // For example, cases like "on or later than", "earlier than or in" have inclusive modifier
            bool hasInclusiveModifier = false;
            var modStr = string.Empty;
            var beforeMatch = Config.BeforeRegex.MatchBegin(er.Text, trim: true);
            var afterMatch = Config.AfterRegex.MatchBegin(er.Text, trim: true);
            var sinceMatch = Config.SinceRegex.MatchBegin(er.Text, trim: true);
            var aroundMatch = Config.AroundRegex.MatchBegin(er.Text, trim: true);

            if (beforeMatch.Success)
            {
                hasBefore = true;
                er.Start += beforeMatch.Length;
                er.Length -= beforeMatch.Length;
                er.Text = er.Text.Substring(beforeMatch.Length);
                modStr = beforeMatch.Value;

                if (!string.IsNullOrEmpty(beforeMatch.Groups["include"].Value))
                {
                    hasInclusiveModifier = true;
                }
            }
            else if (afterMatch.Success)
            {
                hasAfter = true;
                er.Start += afterMatch.Length;
                er.Length -= afterMatch.Length;
                er.Text = er.Text.Substring(afterMatch.Length);
                modStr = afterMatch.Value;

                if (!string.IsNullOrEmpty(afterMatch.Groups["include"].Value))
                {
                    hasInclusiveModifier = true;
                }
            }
            else if (sinceMatch.Success)
            {
                hasSince = true;
                er.Start += sinceMatch.Length;
                er.Length -= sinceMatch.Length;
                er.Text = er.Text.Substring(sinceMatch.Length);
                modStr = sinceMatch.Value;
            }
            else if (aroundMatch.Success)
            {
                hasAround = true;
                er.Start += aroundMatch.Length;
                er.Length -= aroundMatch.Length;
                er.Text = er.Text.Substring(aroundMatch.Length);
                modStr = aroundMatch.Value;
            }
            else if ((er.Type.Equals(Constants.SYS_DATETIME_DATEPERIOD) && Config.YearRegex.Match(er.Text).Success) || er.Type.Equals(Constants.SYS_DATETIME_DATE) || er.Type.Equals(Constants.SYS_DATETIME_TIME))
            {
                // This has to be put at the end of the if, or cases like "before 2012" and "after 2012" would fall into this
                // 2012 or after/above
                // 3 pm or later
                var match = Config.SuffixAfter.MatchEnd(er.Text, trim: true);
                if (match.Success)
                {
                    hasDateAfter = true;
                    er.Length -= match.Length;
                    er.Text = er.Text.Substring(0, er.Length ?? 0);
                    modStr = match.Value;
                }
            }

            pr = ParseResult(er, referenceTime);
            if (pr == null)
            {
                return null;
            }

            // Pop, restore the MOD string
            if (hasBefore && (pr != null && pr.Value != null))
            {
                pr.Length += modStr.Length;
                pr.Start -= modStr.Length;
                pr.Text = modStr + pr.Text;
                var val = (DateTimeResolutionResult)pr.Value;

                if (!hasInclusiveModifier)
                {
                    val.Mod = CombineMod(val.Mod, Constants.BEFORE_MOD);
                }
                else
                {
                    val.Mod = CombineMod(val.Mod, Constants.UNTIL_MOD);
                }

                pr.Value = val;
            }

            if (hasAfter && (pr != null && pr.Value != null))
            {
                pr.Length += modStr.Length;
                pr.Start -= modStr.Length;
                pr.Text = modStr + pr.Text;
                var val = (DateTimeResolutionResult)pr.Value;

                if (!hasInclusiveModifier)
                {
                    val.Mod = CombineMod(val.Mod, Constants.AFTER_MOD);
                }
                else
                {
                    val.Mod = CombineMod(val.Mod, Constants.SINCE_MOD);
                }

                pr.Value = val;
            }

            if (hasSince && (pr != null && pr.Value != null))
            {
                pr.Length += modStr.Length;
                pr.Start -= modStr.Length;
                pr.Text = modStr + pr.Text;
                var val = (DateTimeResolutionResult)pr.Value;
                val.Mod = CombineMod(val.Mod, Constants.SINCE_MOD);
                pr.Value = val;
            }

            if (hasAround && (pr != null && pr.Value != null))
            {
                pr.Length += modStr.Length;
                pr.Start -= modStr.Length;
                pr.Text = modStr + pr.Text;
                var val = (DateTimeResolutionResult)pr.Value;
                val.Mod = CombineMod(val.Mod, Constants.APPROX_MOD);
                pr.Value = val;
            }

            if (hasDateAfter && (pr != null && pr.Value != null))
            {
                pr.Length += modStr.Length;
                pr.Text = pr.Text + modStr;
                var val = (DateTimeResolutionResult)pr.Value;
                val.Mod = CombineMod(val.Mod, Constants.SINCE_MOD);
                pr.Value = val;
                hasSince = true;
            }

            // For cases like "3 pm or later on monday"
            if ((pr != null && pr.Value != null) && Config.SuffixAfter.Match(pr.Text)?.Index != 0 &&
                pr.Type.Equals(Constants.SYS_DATETIME_DATETIME))
            {
                var val = (DateTimeResolutionResult)pr.Value;
                val.Mod = CombineMod(val.Mod, Constants.SINCE_MOD);
                pr.Value = val;
                hasSince = true;
            }

            if ((Config.Options & DateTimeOptions.SplitDateAndTime) != 0 &&
                ((DateTimeResolutionResult)pr?.Value)?.SubDateTimeEntities != null)
            {
                if (pr != null)
                {
                    pr.Value = DateTimeResolutionForSplit(pr);
                }
            }
            else
            {
                var hasModifier = hasBefore || hasAfter || hasSince;
                pr = SetParseResult(pr, hasModifier);
            }

            // In this version, ExperimentalMode only cope with the "IncludePeriodEnd" case
            if ((this.Config.Options & DateTimeOptions.ExperimentalMode) != 0)
            {
                if (pr?.Metadata != null && pr.Metadata.PossiblyIncludePeriodEnd)
                {
                    pr = SetInclusivePeriodEnd(pr);
                }
            }

            if ((this.Config.Options & DateTimeOptions.EnablePreview) != 0)
            {
                pr.Length += originText.Length - pr.Text.Length;
                pr.Text = originText;
            }

            return pr;
        }

        public List<DateTimeParseResult> FilterResults(string query, List<DateTimeParseResult> candidateResults)
        {
            return candidateResults;
        }

        public DateTimeParseResult SetParseResult(DateTimeParseResult slot, bool hasMod)
        {
            slot.Value = DateTimeResolution(slot);

            // Change the type at last for the after or before modes
            slot.Type = $"{ParserTypeName}.{DetermineDateTimeType(slot.Type, hasMod)}";
            return slot;
        }

        public string DetermineDateTimeType(string type, bool hasMod)
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
                if (hasMod)
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
            if (((DateTimeResolutionResult)slot.Value).SubDateTimeEntities != null)
            {
                var subEntities = ((DateTimeResolutionResult)slot.Value).SubDateTimeEntities;
                foreach (var subEntity in subEntities)
                {
                    var result = (DateTimeParseResult)subEntity;
                    result.Start += slot.Start;
                    results.AddRange(DateTimeResolutionForSplit(result));
                }
            }
            else
            {
                slot.Value = DateTimeResolution(slot);
                slot.Type = $"{ParserTypeName}.{DetermineDateTimeType(slot.Type, hasMod: false)}";
                results.Add(slot);
            }

            return results;
        }

        public SortedDictionary<string, object> DateTimeResolution(DateTimeParseResult slot)
        {
            if (slot == null)
            {
                return null;
            }

            var resolutions = new List<Dictionary<string, string>>();
            var res = new Dictionary<string, object>();

            var type = slot.Type;
            var timex = slot.TimexStr;

            var val = (DateTimeResolutionResult)slot.Value;
            if (val == null)
            {
                return null;
            }

            var isLunar = val.IsLunar;
            var mod = val.Mod;
            string list = null;

            // Resolve dates list for date periods
            if (slot.Type.Equals(Constants.SYS_DATETIME_DATEPERIOD) && val.List != null)
            {
                list = string.Join(",", val.List.Select(o => DateTimeFormatUtil.LuisDate((DateObject)o)).ToArray());
            }

            // With modifier, output Type might not be the same with type in resolution result
            // For example, if the resolution type is "date", with modifier the output type should be "daterange"
            var typeOutput = DetermineDateTimeType(slot.Type, hasMod: !string.IsNullOrEmpty(mod));
            var comment = val.Comment;

            // The following should be added to res first, since ResolveAmPm requires these fields.
            AddResolutionFields(res, DateTimeResolutionKey.Timex, timex);
            AddResolutionFields(res, Constants.Comment, comment);
            AddResolutionFields(res, DateTimeResolutionKey.Mod, mod);
            AddResolutionFields(res, ResolutionKey.Type, typeOutput);
            AddResolutionFields(res, DateTimeResolutionKey.IsLunar, isLunar ? isLunar.ToString() : string.Empty);

            var hasTimeZone = false;

            // For standalone timezone entity recognition, we generate TimeZoneResolution for each entity we extracted.
            // We also merge time entity with timezone entity and add the information in TimeZoneResolution to every DateTime resolutions.
            if (val.TimeZoneResolution != null)
            {
                if (slot.Type.Equals(Constants.SYS_DATETIME_TIMEZONE))
                {
                    // single timezone
                    AddResolutionFields(res, Constants.ResolveTimeZone, new Dictionary<string, string>
                    {
                        { ResolutionKey.Value, val.TimeZoneResolution.Value },
                        { Constants.UtcOffsetMinsKey, val.TimeZoneResolution.UtcOffsetMins.ToString() },
                    });
                }
                else
                {
                    // timezone as clarification of datetime
                    hasTimeZone = true;
                    AddResolutionFields(res, Constants.TimeZone, val.TimeZoneResolution.Value);
                    AddResolutionFields(res, Constants.TimeZoneText, val.TimeZoneResolution.TimeZoneText);
                    AddResolutionFields(res, Constants.UtcOffsetMinsKey, val.TimeZoneResolution.UtcOffsetMins.ToString());
                }
            }

            var pastResolutionStr = ((DateTimeResolutionResult)slot.Value).PastResolution;
            var futureResolutionStr = ((DateTimeResolutionResult)slot.Value).FutureResolution;

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

            foreach (var p in res)
            {
                if (p.Value is Dictionary<string, string> dictionary)
                {
                    var value = new Dictionary<string, string>();

                    AddResolutionFields(value, DateTimeResolutionKey.Timex, timex);
                    AddResolutionFields(value, DateTimeResolutionKey.Mod, mod);
                    AddResolutionFields(value, ResolutionKey.Type, typeOutput);
                    AddResolutionFields(value, DateTimeResolutionKey.IsLunar, isLunar ? isLunar.ToString() : string.Empty);
                    AddResolutionFields(value, DateTimeResolutionKey.List, list);

                    if (hasTimeZone)
                    {
                        AddResolutionFields(value, Constants.TimeZone, val.TimeZoneResolution.Value);
                        AddResolutionFields(value, Constants.TimeZoneText, val.TimeZoneResolution.TimeZoneText);
                        AddResolutionFields(value, Constants.UtcOffsetMinsKey, val.TimeZoneResolution.UtcOffsetMins.ToString());
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

            if (resolutionPast.Count == 0 && resolutionFuture.Count == 0 && val.TimeZoneResolution == null)
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

        internal static void AddResolutionFields(Dictionary<string, string> dic, string key, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                dic.Add(key, value);
            }
        }

        internal static void AddResolutionFields(Dictionary<string, object> dic, string key, object value)
        {
            if (value != null)
            {
                dic.Add(key, value);
            }
        }

        internal static void ResolveAmpm(Dictionary<string, object> resolutionDic, string keyName)
        {
            if (resolutionDic.ContainsKey(keyName))
            {
                var resolution = (Dictionary<string, string>)resolutionDic[keyName];
                var resolutionPm = new Dictionary<string, string>();

                if (!resolutionDic.ContainsKey(DateTimeResolutionKey.Timex))
                {
                    return;
                }

                var timex = (string)resolutionDic[DateTimeResolutionKey.Timex];

                resolutionDic.Remove(keyName);
                resolutionDic.Add(keyName + "Am", resolution);

                switch ((string)resolutionDic[ResolutionKey.Type])
                {
                    case Constants.SYS_DATETIME_TIME:
                        resolutionPm[ResolutionKey.Value] = DateTimeFormatUtil.ToPm(resolution[ResolutionKey.Value]);
                        resolutionPm[DateTimeResolutionKey.Timex] = DateTimeFormatUtil.ToPm(timex);
                        break;

                    case Constants.SYS_DATETIME_DATETIME:
                        var split = resolution[ResolutionKey.Value].Split(' ');
                        resolutionPm[ResolutionKey.Value] = split[0] + " " + DateTimeFormatUtil.ToPm(split[1]);
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
                        if (resolution.ContainsKey(DateTimeResolutionKey.START))
                        {
                            var start = Convert.ToDateTime(resolution[DateTimeResolutionKey.START]);
                            start = start.Hour == Constants.HalfDayHourCount ? start.AddHours(-Constants.HalfDayHourCount) : start.AddHours(Constants.HalfDayHourCount);

                            resolutionPm[DateTimeResolutionKey.START] = DateTimeFormatUtil.FormatDateTime(start);
                        }

                        if (resolution.ContainsKey(DateTimeResolutionKey.END))
                        {
                            var end = Convert.ToDateTime(resolution[DateTimeResolutionKey.END]);
                            end = end.Hour == Constants.HalfDayHourCount ? end.AddHours(-Constants.HalfDayHourCount) : end.AddHours(Constants.HalfDayHourCount);

                            resolutionPm[DateTimeResolutionKey.END] = DateTimeFormatUtil.FormatDateTime(end);
                        }

                        resolutionPm[DateTimeResolutionKey.Timex] = DateTimeFormatUtil.AllStringToPm(timex);
                        break;
                }

                resolutionDic.Add(keyName + "Pm", resolutionPm);
            }
        }

        internal static void ResolveWeekOf(Dictionary<string, object> resolutionDic, string keyName)
        {
            if (resolutionDic.ContainsKey(keyName))
            {
                var resolution = (Dictionary<string, string>)resolutionDic[keyName];

                var monday = DateObject.Parse(resolution[DateTimeResolutionKey.START]);
                resolution[DateTimeResolutionKey.Timex] = DateTimeFormatUtil.ToIsoWeekTimex(monday);

                resolutionDic.Remove(keyName);
                resolutionDic.Add(keyName, resolution);
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
            else if (type.Equals(Constants.SYS_DATETIME_DATETIMEALT))
            {
                // for a period
                if (resolutionDic.Count > 2 || !string.IsNullOrEmpty(mod))
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

        private static string CombineMod(string originalMod, string newMod)
        {
            var combinedMod = newMod;

            if (!string.IsNullOrEmpty(originalMod))
            {
                combinedMod = $"{newMod}-{originalMod}";
            }

            return combinedMod;
        }

        private static string DetermineResolutionDateTimeType(Dictionary<string, string> pastResolutionStr)
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

        private DateTimeParseResult ParseResult(ExtractResult extractResult, DateObject referenceTime)
        {
            DateTimeParseResult parseResult = null;
            switch (extractResult.Type)
            {
                case Constants.SYS_DATETIME_DATE:
                    parseResult = this.Config.DateParser.Parse(extractResult, referenceTime);
                    if (parseResult.Value == null)
                    {
                        parseResult = Config.HolidayParser.Parse(extractResult, referenceTime);
                    }

                    break;
                case Constants.SYS_DATETIME_TIME:
                    parseResult = this.Config.TimeParser.Parse(extractResult, referenceTime);

                    break;
                case Constants.SYS_DATETIME_DATETIME:
                    parseResult = this.Config.DateTimeParser.Parse(extractResult, referenceTime);

                    break;
                case Constants.SYS_DATETIME_DATEPERIOD:
                    parseResult = this.Config.DatePeriodParser.Parse(extractResult, referenceTime);

                    break;
                case Constants.SYS_DATETIME_TIMEPERIOD:
                    parseResult = this.Config.TimePeriodParser.Parse(extractResult, referenceTime);

                    break;
                case Constants.SYS_DATETIME_DATETIMEPERIOD:
                    parseResult = this.Config.DateTimePeriodParser.Parse(extractResult, referenceTime);

                    break;
                case Constants.SYS_DATETIME_DURATION:
                    parseResult = this.Config.DurationParser.Parse(extractResult, referenceTime);

                    break;
                case Constants.SYS_DATETIME_SET:
                    parseResult = this.Config.SetParser.Parse(extractResult, referenceTime);

                    break;
                case Constants.SYS_DATETIME_DATETIMEALT:
                    parseResult = this.Config.DateTimeAltParser.Parse(extractResult, referenceTime);

                    break;
                case Constants.SYS_DATETIME_TIMEZONE:
                    if ((Config.Options & DateTimeOptions.EnablePreview) != 0)
                    {
                        parseResult = this.Config.TimeZoneParser.Parse(extractResult, referenceTime);
                    }

                    break;
                default:
                    return null;
            }

            return parseResult;
        }
    }
}