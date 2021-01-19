using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.Recognizers.Text.Utilities;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime
{
    public class BaseMergedDateTimeParser : IDateTimeParser
    {
        public const string ParserTypeName = "datetimeV2";

        public static readonly string DateMinString = DateTimeFormatUtil.FormatDate(DateObject.MinValue);
        public static readonly string DateTimeMinString = DateTimeFormatUtil.FormatDateTime(DateObject.MinValue);

        public BaseMergedDateTimeParser(IMergedParserConfiguration configuration)
        {
            Config = configuration;
        }

        protected IMergedParserConfiguration Config { get; private set; }

        public static void AddAltSingleDateTimeToResolution(Dictionary<string, string> resolutionDic, string type, string mod,
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

        public static void AddSingleDateTimeToResolution(Dictionary<string, string> resolutionDic, string type, string mod,
                                                         Dictionary<string, string> res)
        {

            // If an "invalid" Date or DateTime is extracted, it should not have an assigned resolution.
            // Only valid entities should pass this condition.
            if (resolutionDic.ContainsKey(type) &&
                !resolutionDic[type].StartsWith(DateMinString, StringComparison.Ordinal))
            {
                if (!string.IsNullOrEmpty(mod))
                {
                    if (mod.StartsWith(Constants.BEFORE_MOD, StringComparison.Ordinal))
                    {
                        res.Add(DateTimeResolutionKey.End, resolutionDic[type]);
                        return;
                    }

                    if (mod.StartsWith(Constants.AFTER_MOD, StringComparison.Ordinal))
                    {
                        res.Add(DateTimeResolutionKey.Start, resolutionDic[type]);
                        return;
                    }

                    if (mod.StartsWith(Constants.SINCE_MOD, StringComparison.Ordinal))
                    {
                        res.Add(DateTimeResolutionKey.Start, resolutionDic[type]);
                        return;
                    }

                    if (mod.StartsWith(Constants.UNTIL_MOD, StringComparison.Ordinal))
                    {
                        res.Add(DateTimeResolutionKey.End, resolutionDic[type]);
                        return;
                    }
                }

                res.Add(ResolutionKey.Value, resolutionDic[type]);
            }
        }

        public static void AddPeriodToResolution(Dictionary<string, string> resolutionDic, string startType, string endType, string mod,
                                                 Dictionary<string, string> res)
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
                // For the 'before' mod
                // 1. Cases like "Before December", the start of the period should be the end of the new period, not the start
                // (but not for cases like "Before the end of December")
                // 2. Cases like "More than 3 days before today", the date point should be the end of the new period
                if (mod.StartsWith(Constants.BEFORE_MOD, StringComparison.Ordinal))
                {
                    if (!string.IsNullOrEmpty(start) && !string.IsNullOrEmpty(end) && !mod.EndsWith(Constants.LATE_MOD, StringComparison.Ordinal))
                    {
                        res.Add(DateTimeResolutionKey.End, start);
                    }
                    else
                    {
                        res.Add(DateTimeResolutionKey.End, end);
                    }

                    return;
                }

                // For the 'after' mod
                // 1. Cases like "After January", the end of the period should be the start of the new period, not the end
                // (but not for cases like "After the beginning of January")
                // 2. Cases like "More than 3 days after today", the date point should be the start of the new period
                if (mod.StartsWith(Constants.AFTER_MOD, StringComparison.Ordinal))
                {
                    if (!string.IsNullOrEmpty(start) && !string.IsNullOrEmpty(end) && !mod.EndsWith(Constants.EARLY_MOD, StringComparison.Ordinal))
                    {
                        res.Add(DateTimeResolutionKey.Start, end);
                    }
                    else
                    {
                        res.Add(DateTimeResolutionKey.Start, start);
                    }

                    return;
                }

                // For the 'since' mod, the start of the period should be the start of the new period, not the end
                if (mod.StartsWith(Constants.SINCE_MOD, StringComparison.Ordinal))
                {
                    res.Add(DateTimeResolutionKey.Start, start);
                    return;
                }

                // For the 'until' mod, the end of the period should be the end of the new period, not the start
                if (mod.StartsWith(Constants.UNTIL_MOD, StringComparison.Ordinal))
                {
                    res.Add(DateTimeResolutionKey.End, end);
                    return;
                }
            }

            if (!AreUnresolvedDates(start, end))
            {
                res.Add(DateTimeResolutionKey.Start, start);
                res.Add(DateTimeResolutionKey.End, end);
            }
        }

        public static string GenerateEndInclusiveTimex(string originalTimex, DatePeriodTimexType datePeriodTimexType,
                                                       DateObject startDate, DateObject endDate)
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
                                if (values.ContainsKey(DateTimeResolutionKey.Start) && values.ContainsKey(DateTimeResolutionKey.End) &&
                                    values.ContainsKey(DateTimeResolutionKey.Timex))
                                {
                                    var startDate = DateObject.Parse(values[DateTimeResolutionKey.Start], CultureInfo.InvariantCulture);
                                    var endDate = DateObject.Parse(values[DateTimeResolutionKey.End], CultureInfo.InvariantCulture);
                                    var durationStr = timexComponents[2];
                                    var datePeriodTimexType = TimexUtility.GetDatePeriodTimexType(durationStr);

                                    endDate = TimexUtility.OffsetDateObject(endDate, offset: 1, timexType: datePeriodTimexType);
                                    values[DateTimeResolutionKey.End] = DateTimeFormatUtil.LuisDate(endDate);
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

        public static bool AreUnresolvedDates(string startDate, string endDate)
        {
            return string.IsNullOrEmpty(startDate) || string.IsNullOrEmpty(endDate) ||
                   startDate.StartsWith(DateMinString, StringComparison.Ordinal) ||
                   endDate.StartsWith(DateMinString, StringComparison.Ordinal);
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

            bool hasBefore = false, hasAfter = false, hasSince = false, hasAround = false, hasEqual = false, hasDateAfter = false;

            // "InclusiveModifier" means MOD should include the start/end time
            // For example, cases like "on or later than", "earlier than or in" have inclusive modifier
            var hasInclusiveModifier = false;
            var matchIsAfter = false;
            var modStr = string.Empty;

            // Analyze and process modifiers
            // Push, save the MOD string
            if (er.Metadata != null && er.Metadata.HasMod)
            {
                var beforeMatch = Config.BeforeRegex.MatchBegin(er.Text, trim: true);
                var afterMatch = Config.AfterRegex.MatchBegin(er.Text, trim: true);
                var sinceMatch = Config.SinceRegex.MatchBegin(er.Text, trim: true);
                var preLength = 0;
                if (beforeMatch.Success)
                {
                    preLength = beforeMatch.Index + beforeMatch.Length;
                }
                else if (afterMatch.Success)
                {
                    preLength = afterMatch.Index + afterMatch.Length;
                }
                else if (sinceMatch.Success)
                {
                    preLength = sinceMatch.Index + sinceMatch.Length;
                }

                var aroundText = er.Text.Substring(preLength);
                var aroundMatch = Config.AroundRegex.MatchBegin(aroundText, trim: true);
                var equalMatch = Config.EqualRegex.MatchBegin(er.Text, trim: true);

                // check also after match
                if (this.Config.CheckBothBeforeAfter && er.Data != null && er.Data.Equals(Constants.HAS_MOD))
                {
                    if (!beforeMatch.Success)
                    {
                        beforeMatch = Config.BeforeRegex.MatchEnd(er.Text, trim: true);
                        matchIsAfter = matchIsAfter || beforeMatch.Success;
                    }

                    if (!afterMatch.Success)
                    {
                        afterMatch = Config.AfterRegex.MatchEnd(er.Text, trim: true);
                        matchIsAfter = matchIsAfter || afterMatch.Success;
                    }

                    if (!sinceMatch.Success)
                    {
                        sinceMatch = Config.SinceRegex.MatchEnd(er.Text, trim: true);
                        matchIsAfter = matchIsAfter || sinceMatch.Success;
                    }

                    if (!aroundMatch.Success)
                    {
                        aroundMatch = Config.AroundRegex.MatchEnd(er.Text, trim: true);
                        matchIsAfter = matchIsAfter || aroundMatch.Success;
                    }

                    if (!equalMatch.Success)
                    {
                        equalMatch = Config.EqualRegex.MatchEnd(er.Text, trim: true);
                        matchIsAfter = matchIsAfter || equalMatch.Success;
                    }
                }

                if (aroundMatch.Success)
                {
                    hasAround = true;
                    er.Start += matchIsAfter ? 0 : preLength + aroundMatch.Index + aroundMatch.Length;
                    er.Length -= matchIsAfter ? aroundMatch.Length : preLength + aroundMatch.Index + aroundMatch.Length;
                    er.Text = matchIsAfter ? er.Text.Substring(0, (int)er.Length) : er.Text.Substring(preLength + aroundMatch.Index + aroundMatch.Length);
                    modStr = matchIsAfter ? aroundMatch.Value : aroundText.Substring(0, aroundMatch.Index + aroundMatch.Length);
                }

                if (beforeMatch.Success)
                {
                    hasBefore = true;
                    if (!hasAround)
                    {
                        er.Start += matchIsAfter ? 0 : beforeMatch.Length;
                        er.Length -= beforeMatch.Length;
                        er.Text = matchIsAfter ? er.Text.Substring(0, (int)er.Length) : er.Text.Substring(beforeMatch.Length);
                    }

                    modStr = beforeMatch.Value + modStr;

                    if (!string.IsNullOrEmpty(beforeMatch.Groups[Constants.IncludeGroupName].Value))
                    {
                        hasInclusiveModifier = true;
                    }
                }
                else if (afterMatch.Success)
                {
                    hasAfter = true;
                    if (!hasAround)
                    {
                        er.Start += matchIsAfter ? 0 : afterMatch.Length;
                        er.Length -= afterMatch.Length;
                        er.Text = matchIsAfter ? er.Text.Substring(0, (int)er.Length) : er.Text.Substring(afterMatch.Length);
                    }

                    modStr = afterMatch.Value + modStr;

                    if (!string.IsNullOrEmpty(afterMatch.Groups[Constants.IncludeGroupName].Value))
                    {
                        hasInclusiveModifier = true;
                    }
                }
                else if (sinceMatch.Success)
                {
                    hasSince = true;
                    if (!hasAround)
                    {
                        er.Start += matchIsAfter ? 0 : sinceMatch.Length;
                        er.Length -= sinceMatch.Length;
                        er.Text = matchIsAfter ? er.Text.Substring(0, (int)er.Length) : er.Text.Substring(sinceMatch.Length);
                    }

                    modStr = sinceMatch.Value + modStr;
                }
                else if (equalMatch.Success)
                {
                    hasEqual = true;
                    er.Start += matchIsAfter ? 0 : equalMatch.Length;
                    er.Length -= equalMatch.Length;
                    er.Text = matchIsAfter ? er.Text.Substring(0, (int)er.Length) : er.Text.Substring(equalMatch.Length);
                    modStr = equalMatch.Value;
                }
                else if ((er.Type.Equals(Constants.SYS_DATETIME_DATEPERIOD, StringComparison.Ordinal) &&
                          Config.YearRegex.Match(er.Text).Success) ||
                         er.Type.Equals(Constants.SYS_DATETIME_DATE, StringComparison.Ordinal) ||
                         er.Type.Equals(Constants.SYS_DATETIME_TIME, StringComparison.Ordinal))
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
            }

            // Parse extracted datetime mention
            pr = ParseResult(er, referenceTime);
            if (pr == null)
            {
                return null;
            }

            // Apply processed modifiers
            // Pop, restore the MOD string
            if (hasBefore && pr.Value != null)
            {
                pr.Length += modStr.Length;
                pr.Start -= matchIsAfter ? 0 : modStr.Length;
                pr.Text = matchIsAfter ? pr.Text + modStr : modStr + pr.Text;
                var val = (DateTimeResolutionResult)pr.Value;

                val.Mod = CombineMod(val.Mod, !hasInclusiveModifier ? Constants.BEFORE_MOD : Constants.UNTIL_MOD);

                if (hasAround)
                {
                    val.Mod = CombineMod(Constants.APPROX_MOD, val.Mod);
                    hasAround = false;
                }

                pr.Value = val;
            }

            if (hasAfter && pr.Value != null)
            {
                pr.Length += modStr.Length;
                pr.Start -= matchIsAfter ? 0 : modStr.Length;
                pr.Text = matchIsAfter ? pr.Text + modStr : modStr + pr.Text;
                var val = (DateTimeResolutionResult)pr.Value;

                val.Mod = CombineMod(val.Mod, !hasInclusiveModifier ? Constants.AFTER_MOD : Constants.SINCE_MOD);

                if (hasAround)
                {
                    val.Mod = CombineMod(Constants.APPROX_MOD, val.Mod);
                    hasAround = false;
                }

                pr.Value = val;
            }

            if (hasSince && pr.Value != null)
            {
                pr.Length += modStr.Length;
                pr.Start -= matchIsAfter ? 0 : modStr.Length;
                pr.Text = matchIsAfter ? pr.Text + modStr : modStr + pr.Text;
                var val = (DateTimeResolutionResult)pr.Value;
                val.Mod = CombineMod(val.Mod, Constants.SINCE_MOD);

                if (hasAround)
                {
                    val.Mod = CombineMod(Constants.APPROX_MOD, val.Mod);
                    hasAround = false;
                }

                pr.Value = val;
            }

            if (hasAround && pr.Value != null)
            {
                pr.Length += modStr.Length;
                pr.Start -= matchIsAfter ? 0 : modStr.Length;
                pr.Text = matchIsAfter ? pr.Text + modStr : modStr + pr.Text;
                var val = (DateTimeResolutionResult)pr.Value;
                val.Mod = CombineMod(val.Mod, Constants.APPROX_MOD);
                pr.Value = val;
            }

            if (hasEqual && pr.Value != null)
            {
                pr.Length += modStr.Length;
                pr.Start -= matchIsAfter ? 0 : modStr.Length;
                pr.Text = matchIsAfter ? pr.Text + modStr : modStr + pr.Text;
            }

            if (hasDateAfter && pr.Value != null)
            {
                pr.Length += modStr.Length;
                pr.Text = pr.Text + modStr;
                var val = (DateTimeResolutionResult)pr.Value;
                val.Mod = CombineMod(val.Mod, Constants.SINCE_MOD);
                pr.Value = val;
                hasSince = true;
            }

            // For cases like "3 pm or later on monday"
            if (pr.Value != null && Config.SuffixAfter.Match(pr.Text)?.Index != 0 &&
                pr.Type.Equals(Constants.SYS_DATETIME_DATETIME, StringComparison.Ordinal) && !this.Config.CheckBothBeforeAfter)
            {
                var val = (DateTimeResolutionResult)pr.Value;
                val.Mod = CombineMod(val.Mod, Constants.SINCE_MOD);
                pr.Value = val;
                hasSince = true;
            }

            if ((Config.Options & DateTimeOptions.SplitDateAndTime) != 0 &&
                ((DateTimeResolutionResult)pr?.Value)?.SubDateTimeEntities != null)
            {
                pr.Value = DateTimeResolutionForSplit(pr);
            }
            else
            {
                var hasRangeChangingMod = hasBefore || hasAfter || hasSince;
                if (pr.Value != null)
                {
                    ((DateTimeResolutionResult)pr.Value).HasRangeChangingMod = hasRangeChangingMod;
                }

                pr = SetParseResult(pr, hasRangeChangingMod);
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
                if (pr != null)
                {
                    pr.Length += originText.Length - pr.Text.Length;
                    pr.Text = originText;
                }
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
                if (type.Equals(Constants.SYS_DATETIME_DATETIME, StringComparison.Ordinal))
                {
                    return Constants.SYS_DATETIME_TIME;
                }
            }
            else
            {
                if (hasMod)
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
            if (slot.Type.Equals(Constants.SYS_DATETIME_DATEPERIOD, StringComparison.Ordinal) && val.List != null)
            {
                list = string.Join(",", val.List.Select(o => DateTimeFormatUtil.LuisDate((DateObject)o)).ToArray());
            }

            // With modifier, output Type might not be the same with type in resolution result
            // For example, if the resolution type is "date", with modifier the output type should be "daterange"
            var typeOutput = DetermineDateTimeType(slot.Type, hasMod: !string.IsNullOrEmpty(mod));

            var sourceEntity = DetermineSourceEntityType(slot.Type, typeOutput, val.HasRangeChangingMod);

            var comment = val.Comment;

            // The following should be added to res first, since ResolveAmPm requires these fields.
            AddResolutionFields(res, DateTimeResolutionKey.Timex, timex);
            AddResolutionFields(res, Constants.Comment, comment);
            AddResolutionFields(res, DateTimeResolutionKey.Mod, mod);
            AddResolutionFields(res, ResolutionKey.Type, typeOutput);
            AddResolutionFields(res, DateTimeResolutionKey.IsLunar, isLunar ? isLunar.ToString(CultureInfo.InvariantCulture) : string.Empty);

            var hasTimeZone = false;

            // For standalone timezone entity recognition, we generate TimeZoneResolution for each entity we extracted.
            // We also merge time entity with timezone entity and add the information in TimeZoneResolution to every DateTime resolutions.
            if (val.TimeZoneResolution != null)
            {
                if (slot.Type.Equals(Constants.SYS_DATETIME_TIMEZONE, StringComparison.Ordinal))
                {
                    // single timezone
                    AddResolutionFields(res, Constants.ResolveTimeZone, new Dictionary<string, string>
                    {
                        { ResolutionKey.Value, val.TimeZoneResolution.Value },
                        { Constants.UtcOffsetMinsKey, val.TimeZoneResolution.UtcOffsetMins.ToString(CultureInfo.InvariantCulture) },
                    });
                }
                else
                {
                    // timezone as clarification of datetime
                    hasTimeZone = true;
                    AddResolutionFields(res, Constants.TimeZone, val.TimeZoneResolution.Value);
                    AddResolutionFields(res, Constants.TimeZoneText, val.TimeZoneResolution.TimeZoneText);
                    AddResolutionFields(res, Constants.UtcOffsetMinsKey,
                                        val.TimeZoneResolution.UtcOffsetMins.ToString(CultureInfo.InvariantCulture));
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

            // If WeekOf and in CalendarMode, modify the past part of our resolution
            if ((Config.Options & DateTimeOptions.CalendarMode) != 0 &&
                !string.IsNullOrEmpty(comment) && comment.Equals(Constants.Comment_WeekOf, StringComparison.Ordinal))
            {
                ResolveWeekOf(res, Constants.ResolveToPast);
            }

            if (!string.IsNullOrEmpty(comment) && TimexUtility.HasDoubleTimex(comment))
            {
                TimexUtility.ProcessDoubleTimex(res, Constants.ResolveToFuture, Constants.ResolveToPast, timex);
            }

            foreach (var p in res)
            {
                if (p.Value is Dictionary<string, string> dictionary)
                {
                    var value = new Dictionary<string, string>();

                    AddResolutionFields(value, DateTimeResolutionKey.Timex, timex);
                    AddResolutionFields(value, DateTimeResolutionKey.Mod, mod);
                    AddResolutionFields(value, ResolutionKey.Type, typeOutput);
                    AddResolutionFields(value, DateTimeResolutionKey.IsLunar,
                                        isLunar ? isLunar.ToString(CultureInfo.InvariantCulture) : string.Empty);
                    AddResolutionFields(value, DateTimeResolutionKey.List, list);
                    AddResolutionFields(value, DateTimeResolutionKey.SourceEntity, sourceEntity);

                    if (hasTimeZone)
                    {
                        AddResolutionFields(value, Constants.TimeZone, val.TimeZoneResolution.Value);
                        AddResolutionFields(value, Constants.TimeZoneText, val.TimeZoneResolution.TimeZoneText);
                        AddResolutionFields(value, Constants.UtcOffsetMinsKey,
                                            val.TimeZoneResolution.UtcOffsetMins.ToString(CultureInfo.InvariantCulture));
                    }

                    foreach (var q in dictionary)
                    {
                        value[q.Key] = q.Value;
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
                        if (resolution.ContainsKey(DateTimeResolutionKey.Start))
                        {
                            resolutionPm[DateTimeResolutionKey.Start] = DateTimeFormatUtil.ToPm(resolution[DateTimeResolutionKey.Start]);
                        }

                        if (resolution.ContainsKey(DateTimeResolutionKey.End))
                        {
                            resolutionPm[DateTimeResolutionKey.End] = DateTimeFormatUtil.ToPm(resolution[DateTimeResolutionKey.End]);
                        }

                        if (resolution.ContainsKey(DateTimeResolutionKey.Value))
                        {
                            resolutionPm[ResolutionKey.Value] = DateTimeFormatUtil.ToPm(resolution[ResolutionKey.Value]);
                        }

                        resolutionPm[DateTimeResolutionKey.Timex] = DateTimeFormatUtil.AllStringToPm(timex);
                        break;

                    case Constants.SYS_DATETIME_DATETIMEPERIOD:
                        if (resolution.ContainsKey(DateTimeResolutionKey.Start))
                        {
                            var start = Convert.ToDateTime(resolution[DateTimeResolutionKey.Start], CultureInfo.InvariantCulture);
                            start = start.Hour == Constants.HalfDayHourCount ?
                                start.AddHours(-Constants.HalfDayHourCount) : start.AddHours(Constants.HalfDayHourCount);

                            resolutionPm[DateTimeResolutionKey.Start] = DateTimeFormatUtil.FormatDateTime(start);
                        }

                        if (resolution.ContainsKey(DateTimeResolutionKey.End))
                        {
                            var end = Convert.ToDateTime(resolution[DateTimeResolutionKey.End], CultureInfo.InvariantCulture);
                            end = end.Hour == Constants.HalfDayHourCount ?
                                end.AddHours(-Constants.HalfDayHourCount) : end.AddHours(Constants.HalfDayHourCount);

                            resolutionPm[DateTimeResolutionKey.End] = DateTimeFormatUtil.FormatDateTime(end);
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

                var monday = DateObject.Parse(resolution[DateTimeResolutionKey.Start], CultureInfo.InvariantCulture);
                resolution[DateTimeResolutionKey.Timex] = DateTimeFormatUtil.ToIsoWeekTimex(monday);

                resolutionDic.Remove(keyName);
                resolutionDic.Add(keyName, resolution);
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
            else if (type.Equals(Constants.SYS_DATETIME_DATETIMEALT, StringComparison.Ordinal))
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
                    // ToLowerInvariant needed for legacy reasons with subtype code.
                    // @TODO remove in future refactoring of test code and double-check there's no impact in output schema.
                    return pastResolutionStr.Keys.First().ToLowerInvariant();
            }
        }

        private DateTimeParseResult ParseResult(ExtractResult extractResult, DateObject referenceTime)
        {
            DateTimeParseResult parseResult = null;
            switch (extractResult.Type)
            {
                case Constants.SYS_DATETIME_DATE:
                    if (extractResult.Metadata != null && extractResult.Metadata.IsHoliday)
                    {
                        parseResult = Config.HolidayParser.Parse(extractResult, referenceTime);
                    }
                    else
                    {
                        parseResult = this.Config.DateParser.Parse(extractResult, referenceTime);
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