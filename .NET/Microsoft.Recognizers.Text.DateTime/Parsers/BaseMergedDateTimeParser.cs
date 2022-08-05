// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

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

        public BaseMergedDateTimeParser(IMergedParserConfiguration configuration)
        {
            Config = configuration;
        }

        protected IMergedParserConfiguration Config { get; private set; }

        public List<DateTimeParseResult> FilterResults(string query, List<DateTimeParseResult> candidateResults)
        {
            return candidateResults;
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
                else if ((er.Type.Equals(Constants.SYS_DATETIME_DATEPERIOD, StringComparison.Ordinal) && Config.YearRegex.Match(er.Text).Success) ||
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

                val.Mod = MergedParserUtil.CombineMod(val.Mod, !hasInclusiveModifier ? Constants.BEFORE_MOD : Constants.UNTIL_MOD);

                if (hasAround)
                {
                    val.Mod = MergedParserUtil.CombineMod(Constants.APPROX_MOD, val.Mod);
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

                val.Mod = MergedParserUtil.CombineMod(val.Mod, !hasInclusiveModifier ? Constants.AFTER_MOD : Constants.SINCE_MOD);

                if (hasAround)
                {
                    val.Mod = MergedParserUtil.CombineMod(Constants.APPROX_MOD, val.Mod);
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
                val.Mod = MergedParserUtil.CombineMod(val.Mod, Constants.SINCE_MOD);

                if (hasAround)
                {
                    val.Mod = MergedParserUtil.CombineMod(Constants.APPROX_MOD, val.Mod);
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
                val.Mod = MergedParserUtil.CombineMod(val.Mod, Constants.APPROX_MOD);
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
                pr.Text += modStr;
                var val = (DateTimeResolutionResult)pr.Value;
                val.Mod = MergedParserUtil.CombineMod(val.Mod, Constants.SINCE_MOD);
                pr.Value = val;
                hasSince = true;
            }

            // For cases like "3 pm or later on monday"
            if (pr.Value != null && Config.SuffixAfter.Match(pr.Text)?.Index != 0 &&
                pr.Type.Equals(Constants.SYS_DATETIME_DATETIME, StringComparison.Ordinal) && !this.Config.CheckBothBeforeAfter)
            {
                var val = (DateTimeResolutionResult)pr.Value;
                val.Mod = MergedParserUtil.CombineMod(val.Mod, Constants.SINCE_MOD);
                pr.Value = val;
                hasSince = true;
            }

            if ((Config.Options & DateTimeOptions.SplitDateAndTime) != 0 &&
                ((DateTimeResolutionResult)pr?.Value)?.SubDateTimeEntities != null)
            {
                pr.Value = MergedParserUtil.DateTimeResolutionForSplit(pr, this.Config);
            }
            else
            {
                var hasRangeChangingMod = hasBefore || hasAfter || hasSince;
                if (pr.Value != null)
                {
                    ((DateTimeResolutionResult)pr.Value).HasRangeChangingMod = hasRangeChangingMod;
                }

                pr = MergedParserUtil.SetParseResult(pr, hasRangeChangingMod, this.Config);
            }

            // In this version, ExperimentalMode only cope with the "IncludePeriodEnd" case
            if ((this.Config.Options & DateTimeOptions.ExperimentalMode) != 0)
            {
                if (pr?.Metadata != null && pr.Metadata.PossiblyIncludePeriodEnd)
                {
                    pr = MergedParserUtil.SetInclusivePeriodEnd(pr);
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

            /* modification of "past datetime" references
               Input : 22 april at 5 pm. (reference time is 22/04/2022 T17:30:00)
               output under taskmode.
                                    Past resolution value: 22/04/2022T17,
                                    Future resolution value: 22/04/2023T17
               output under default mode.
                                     Past resolution value: 22/04/2021T17,
                                    Future resolution value: 22/04/2022T17

             */
            if ((this.Config.Options & DateTimeOptions.TasksMode) != 0)
            {
                // filter decade regexes
                if (pr != null)
                {
                    pr = TasksModeProcessing.TasksModeModification(pr, referenceTime);
                    pr.Text = originText;
                }
            }

            return pr;
        }

        // @TODO move to MergedParserUtil (if possible)
        private DateTimeParseResult ParseResult(ExtractResult extractResult, DateObject referenceTime)
        {
            DateTimeParseResult parseResult = null;
            switch (extractResult.Type)
            {
                case Constants.SYS_DATETIME_DATE:
                    if (extractResult.Metadata != null && extractResult.Metadata.IsHoliday && !extractResult.Metadata.IsHolidayRange)
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
                    if (extractResult.Metadata != null && extractResult.Metadata.IsHolidayRange)
                    {
                        parseResult = this.Config.HolidayParser.Parse(extractResult, referenceTime);
                    }
                    else
                    {
                        parseResult = this.Config.DatePeriodParser.Parse(extractResult, referenceTime);
                    }

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