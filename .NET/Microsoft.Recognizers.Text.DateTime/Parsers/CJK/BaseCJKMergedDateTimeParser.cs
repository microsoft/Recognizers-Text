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
    public class BaseCJKMergedDateTimeParser : IDateTimeParser
    {
        private readonly ICJKMergedParserConfiguration config;

        public BaseCJKMergedDateTimeParser(ICJKMergedParserConfiguration configuration)
        {
            config = configuration;
        }

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
            DateTimeParseResult pr;

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

                if (beforeMatch.Success && !MergedParserUtil.IsDurationWithAgoAndLater(er))
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
                else if (afterMatch.Success && !MergedParserUtil.IsDurationWithAgoAndLater(er))
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

            // Parse extracted datetime mention
            pr = ParseResult(er, referenceTime);
            if (pr == null)
            {
                return null;
            }

            // pop, restore the MOD string
            if (hasBefore)
            {
                pr.Length += modStr.Length;
                pr.Text = pr.Text + modStr;
                var val = (DateTimeResolutionResult)pr.Value;

                val.Mod = MergedParserUtil.CombineMod(val.Mod, !hasInclusiveModifier ? Constants.BEFORE_MOD : Constants.UNTIL_MOD);

                pr.Value = val;
            }

            if (hasAfter)
            {
                pr.Length += modStr.Length;
                pr.Text = pr.Text + modStr;
                var val = (DateTimeResolutionResult)pr.Value;

                val.Mod = MergedParserUtil.CombineMod(val.Mod, !hasInclusiveModifier ? Constants.AFTER_MOD : Constants.SINCE_MOD);

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

            pr = MergedParserUtil.SetParseResult(pr, hasRangeChangingMod, this.config);

            return pr;
        }

        // @TODO move to MergedParserUtil (if possible)
        private DateTimeParseResult ParseResult(ExtractResult extractResult, DateObject referenceTime)
        {
            DateTimeParseResult parseResult = null;
            switch (extractResult.Type)
            {
                case Constants.SYS_DATETIME_DATE:
                    if (extractResult.Metadata != null && extractResult.Metadata.IsHoliday)
                    {
                        parseResult = config.HolidayParser.Parse(extractResult, referenceTime);
                    }
                    else
                    {
                        parseResult = this.config.DateParser.Parse(extractResult, referenceTime);
                    }

                    break;
                case Constants.SYS_DATETIME_TIME:
                    parseResult = this.config.TimeParser.Parse(extractResult, referenceTime);

                    break;
                case Constants.SYS_DATETIME_DATETIME:
                    parseResult = this.config.DateTimeParser.Parse(extractResult, referenceTime);

                    break;
                case Constants.SYS_DATETIME_DATEPERIOD:
                    parseResult = this.config.DatePeriodParser.Parse(extractResult, referenceTime);

                    break;
                case Constants.SYS_DATETIME_TIMEPERIOD:
                    parseResult = this.config.TimePeriodParser.Parse(extractResult, referenceTime);

                    break;
                case Constants.SYS_DATETIME_DATETIMEPERIOD:
                    parseResult = this.config.DateTimePeriodParser.Parse(extractResult, referenceTime);

                    break;
                case Constants.SYS_DATETIME_DURATION:
                    parseResult = this.config.DurationParser.Parse(extractResult, referenceTime);

                    break;
                case Constants.SYS_DATETIME_SET:
                    parseResult = this.config.SetParser.Parse(extractResult, referenceTime);

                    break;
                case Constants.SYS_DATETIME_DATETIMEALT:
                    parseResult = this.config.DateTimeAltParser.Parse(extractResult, referenceTime);

                    break;
                case Constants.SYS_DATETIME_TIMEZONE:
                    if ((config.Options & DateTimeOptions.EnablePreview) != 0)
                    {
                        parseResult = this.config.TimeZoneParser.Parse(extractResult, referenceTime);
                    }

                    break;
                default:
                    return null;
            }

            return parseResult;
        }
    }
}