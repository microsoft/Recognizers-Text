using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.Recognizers.Text.Utilities;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime
{
    public static class TasksModeProcessing
    {
        public const string ParserTypeName = "datetimeV2";

        public static readonly string DateMinString = DateTimeFormatUtil.FormatDate(DateObject.MinValue);

        /*Under TasksMode If you input today's date, future date should get mapped to current date insted of next year.
         ex if input is meet on 7 july and refrence time is 7 july 2022,
        expected future value --> 7 july 2022 &&
        past value--> 7 july 2021
        */
        public static DateTimeParseResult TasksModeModifyDateValue(DateTimeParseResult slot, DateObject referenceTime)
        {
            if (!slot.TimexStr.Contains("XXXX"))
            {
                return slot;
            }

            var value = (SortedDictionary<string, object>)slot.Value;
            if (value != null && value.ContainsKey(ResolutionKey.ValueSet))
            {
                if (value[ResolutionKey.ValueSet] is IList<Dictionary<string, string>> valueSet && valueSet.Any())
                {
                    foreach (var values in valueSet)
                    {
                        if (slot.Text.Contains("next week"))
                        {
                            var tempdate = referenceTime.Upcoming(DayOfWeek.Monday).Date;
                            var dateTimeToSet = DateObject.MinValue.SafeCreateFromValue(tempdate.Year, tempdate.Month, tempdate.Day);
                            values[DateTimeResolutionKey.Value] = DateTimeFormatUtil.FormatDate(dateTimeToSet);
                            values[DateTimeResolutionKey.Timex] = $"{DateTimeFormatUtil.LuisDate(dateTimeToSet)}";
                        }

                    }
                }
            }

            slot.Value = value;

            return slot;
        }
    }
}