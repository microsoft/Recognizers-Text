﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Globalization;

using Microsoft.Recognizers.Text.Utilities;

using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime.French
{
    public class TimeParser : BaseTimeParser
    {
        public TimeParser(ITimeParserConfiguration configuration)
            : base(configuration)
        {
        }

        protected override DateTimeResolutionResult InternalParse(string text, DateObject referenceTime)
        {
            var innerResult = base.InternalParse(text, referenceTime);
            if (!innerResult.Success)
            {
                innerResult = ParseIsh(text, referenceTime);
            }

            return innerResult;
        }

        // parse "noonish", "11-ish"
        // fr - peu pres midi ("approximately noon")- IshRegex for 'peu pres'
        private DateTimeResolutionResult ParseIsh(string text, DateObject referenceTime)
        {
            var ret = new DateTimeResolutionResult();

            var match = FrenchTimeExtractorConfiguration.IshRegex.MatchExact(text, trim: true);

            if (match.Success)
            {
                var hourStr = match.Groups[Constants.HourGroupName].Value;
                var hour = Constants.HalfDayHourCount;
                if (!string.IsNullOrEmpty(hourStr))
                {
                    hour = int.Parse(hourStr, CultureInfo.InvariantCulture);
                }

                ret.Timex = "T" + hour.ToString("D2", CultureInfo.InvariantCulture);
                ret.FutureValue =
                    ret.PastValue =
                        DateObject.MinValue.SafeCreateFromValue(referenceTime.Year, referenceTime.Month, referenceTime.Day, hour, 0, 0);
                ret.Success = true;
                return ret;
            }

            return ret;
        }
    }
}
