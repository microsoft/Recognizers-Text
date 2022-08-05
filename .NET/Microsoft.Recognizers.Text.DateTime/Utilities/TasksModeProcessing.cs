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

        /*
        TasksModeModification modifies past datetime references under tasksmode.
        Eg if input text is 22 june at 9 pm and current time is 22 june 2022, 8 am then
        under default mode pastdateime value will be 22 june 2022 9 pm, but since time has not been passed
        under tasksmode it's value will get mapped to 22 june 2021 9 pm.
        TasksModeModification function will modify datetime value according to it's type and w.r.t
        refrence time.

        Under TasksMode
        For Input: 22 april at 5 pm. (reference time is 22/04/2022 T17:30:00, output type is datetime)
        Expected output : {Past resolution value: 22/04/2022T17,
        Future resolution value: 22/04/2023T17
        },

        Under Default Mode
        For Input: 22 april at 5 pm. (reference time is 22/04/2022 T17:30:00)
        Expected output : {Past resolution value: 22/04/2021T17,
        Future resolution value: 22/04/2022T17
        },
         */
        public static DateTimeParseResult TasksModeModification(DateTimeParseResult slot, DateObject referenceTime)
        {
            switch (slot.Type.Substring(ParserTypeName.Length + 1))
            {
                case TasksModeConstants.SYS_DATETIME_DATE:
                    slot = TasksModeModifyDateValue(slot, referenceTime);
                case Constants.SYS_DATETIME_DATE:
                    slot = TasksModeProcessing.TasksModeModifyDateValue(slot, referenceTime);
                    break;

                case Constants.SYS_DATETIME_DATEPERIOD:
                    slot = TasksModeProcessing.TasksModeModifyDatePeriodValue(slot, referenceTime);
                    break;

                case Constants.SYS_DATETIME_TIME:
                    slot = TasksModeProcessing.TasksModeModifyTimeValue(slot, referenceTime);
                    break;

                case Constants.SYS_DATETIME_TIMEPERIOD:
                    slot = TasksModeProcessing.TasksModeTimePeriodValue(slot, referenceTime);
                    break;

                case Constants.SYS_DATETIME_DATETIME:
                    slot = TasksModeProcessing.TasksModeModifyDateTimeValue(slot, referenceTime);
                    break;

                case Constants.SYS_DATETIME_DATETIMEPERIOD:
                    slot = TasksModeProcessing.TasksModeModifyDateTimePeriodValue(slot, referenceTime);
                    break;
            }

            return slot;
        }

        /*Under TasksMode If you input today's date, future date should get mapped to current date insted of next year.
         ex if input is meet on 7 july and refrence time is 7 july 2022,
        expected future value --> 7 july 2022 &&
        past value--> 7 july 2021
        */
        private static DateTimeParseResult TasksModeModifyDateValue(DateTimeParseResult slot, DateObject referenceTime)
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
                        var inputTime = DateObject.Parse(values[DateTimeResolutionKey.Value], CultureInfo.InvariantCulture);
                        var inputDay = inputTime.Day;
                        var inputMonth = inputTime.Month;

                        if (inputDay == referenceTime.Day && inputMonth == referenceTime.Month)
                        {
                            // ignore for input text like monday, tue etc
                            if (!slot.TimexStr.Contains("XXXX-WXX"))
                            {
                                var newDate = inputTime.Date.AddYears(-1);
                                var dateTimeToSet = DateObject.MinValue.SafeCreateFromValue(newDate.Year, newDate.Month, newDate.Day);

                                values[DateTimeResolutionKey.Value] = DateTimeFormatUtil.FormatDate(dateTimeToSet);
                            }
                        }

                    }
                }
            }

            slot.Value = value;

            return slot;
        }

        /*TasksMode specific change.
         Under TasksMode If  input is today's dateperiod, future value should get mapped to current dateperiod insted of next year.
         ex if input is "meet after 7 july" and refrence time is 7 july 2022,
        expected future start value --> 7 july 2022 &&
        past start value--> 7 july 2021
        */
        private static DateTimeParseResult TasksModeModifyDatePeriodValue(DateTimeParseResult slot, DateObject referenceTime)
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
                    for (int i = 0; i < valueSet.Count - 1; i = i + 2)
                    {
                        var pastvalue = valueSet.ElementAt(i);
                        var futurevalue = valueSet.ElementAt(i + 1);

                        DateObject pastdate;
                        DateObject futuredate;

                        bool maptonew = false;

                        if (pastvalue.ContainsKey("start"))
                        {
                            futuredate = DateObject.Parse(futurevalue[DateTimeResolutionKey.Start], CultureInfo.InvariantCulture);

                            if ((futuredate.Day == referenceTime.Day) && (futuredate.Month == referenceTime.Month)
                               && (futuredate.Year != referenceTime.Year) && (!slot.TimexStr.Contains("XXXX-WXX")))
                            {
                                maptonew = true;
                            }

                        }

                        if (pastvalue.ContainsKey("end"))
                        {

                            futuredate = DateObject.Parse(futurevalue[DateTimeResolutionKey.End], CultureInfo.InvariantCulture);

                            if ((futuredate.Day == referenceTime.Day) && (futuredate.Month == referenceTime.Month)
                                && (futuredate.Year != referenceTime.Year) && (!slot.TimexStr.Contains("XXXX-WXX")))
                            {
                                maptonew = true;
                            }

                        }

                        if (maptonew)
                        {
                            {
                                if (pastvalue.ContainsKey("start"))
                                {
                                    pastdate = DateObject.Parse(pastvalue[DateTimeResolutionKey.Start],
                                                                           CultureInfo.InvariantCulture);

                                    futuredate = DateObject.Parse(futurevalue[DateTimeResolutionKey.Start],
                                                                           CultureInfo.InvariantCulture);

                                    futurevalue[DateTimeResolutionKey.Start] = DateTimeFormatUtil.FormatDate(futuredate.AddYears(-1));

                                    pastvalue[DateTimeResolutionKey.Start] = DateTimeFormatUtil.FormatDate(pastdate.AddYears(-1));
                                }

                                if (pastvalue.ContainsKey("end"))
                                {
                                    pastdate = DateObject.Parse(pastvalue[DateTimeResolutionKey.End],
                                                                           CultureInfo.InvariantCulture);

                                    futuredate = DateObject.Parse(futurevalue[DateTimeResolutionKey.End],
                                                                           CultureInfo.InvariantCulture);

                                    futurevalue[DateTimeResolutionKey.End] = DateTimeFormatUtil.FormatDate(futuredate.AddYears(-1));

                                    pastvalue[DateTimeResolutionKey.End] = DateTimeFormatUtil.FormatDate(pastdate.AddYears(-1));
                                }

                            }

                        }

                    }
                }
            }

            slot.Value = value;

            return slot;
        }

        /* TasksMode specific change.
        If input datetimeperiod string precedes the referenceTime.
        ex if input is "meet on 7 july morning" and refrence time is 7 july 2022 10pm,
        expected future value should get mapped to 7 july 2023, morning &&
        past value get mapped to 7 july 2022, morning.

        ex if input is "meet on  thursday morning" and refrence time is 7 july 2022 (thursday) 10pm,
        expected future value should get mapped to 14 july 2022, morning &&
        past value get mapped to 7 july 2022, morning.
       */
        private static DateTimeParseResult TasksModeModifyDateTimePeriodValue(DateTimeParseResult slot, DateObject referenceTime)
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
                    for (int i = 0; i < valueSet.Count - 1; i = i + 2)
                    {
                        var pastvalue = valueSet.ElementAt(i);
                        var futurevalue = valueSet.ElementAt(i + 1);

                        DateObject pastdatetimeperiod;
                        DateObject futuredatetimeperiod;

                        bool maptonew = false;

                        if (pastvalue.ContainsKey("start"))
                        {
                            pastdatetimeperiod = DateObject.Parse(pastvalue[DateTimeResolutionKey.Start], CultureInfo.InvariantCulture);
                            futuredatetimeperiod = DateObject.Parse(futurevalue[DateTimeResolutionKey.Start], CultureInfo.InvariantCulture);

                            if ((pastdatetimeperiod > referenceTime) && !slot.TimexStr.Contains("XXXX-WXX"))
                            {
                                maptonew = true;
                            }

                            if ((futuredatetimeperiod < referenceTime) && slot.TimexStr.Contains("XXXX-WXX"))
                            {
                                maptonew = true;
                            }

                        }
                        else
                        {
                            if (pastvalue.ContainsKey("end"))
                            {
                                pastdatetimeperiod = DateObject.Parse(pastvalue[DateTimeResolutionKey.End], CultureInfo.InvariantCulture);
                                futuredatetimeperiod = DateObject.Parse(futurevalue[DateTimeResolutionKey.End], CultureInfo.InvariantCulture);

                                if ((pastdatetimeperiod > referenceTime) && !slot.TimexStr.Contains("XXXX-WXX"))
                                {
                                    maptonew = true;
                                }

                                if ((futuredatetimeperiod < referenceTime) && slot.TimexStr.Contains("XXXX-WXX"))
                                {
                                    maptonew = true;
                                }
                            }
                        }

                        if (maptonew)
                        {
                            if (slot.TimexStr.Contains("XXXX-WXX"))
                            {
                                if (pastvalue.ContainsKey("start"))
                                {
                                    futuredatetimeperiod = DateObject.Parse(futurevalue[DateTimeResolutionKey.Start],
                                                                            CultureInfo.InvariantCulture);
                                    pastvalue[DateTimeResolutionKey.Start] = futurevalue[DateTimeResolutionKey.Start];
                                    var tempdate = futuredatetimeperiod.AddDays(7);
                                    var dateTimeToSet = DateObject.MinValue.SafeCreateFromValue(tempdate.Year, tempdate.Month, tempdate.Day, tempdate.Hour,
                                                                                                 tempdate.Minute, tempdate.Second);
                                    futurevalue[DateTimeResolutionKey.Start] = DateTimeFormatUtil.FormatDateTime(dateTimeToSet);
                                }

                                if (pastvalue.ContainsKey("end"))
                                {
                                    futuredatetimeperiod = DateObject.Parse(futurevalue[DateTimeResolutionKey.End],
                                                                             CultureInfo.InvariantCulture);
                                    pastvalue[DateTimeResolutionKey.End] = futurevalue[DateTimeResolutionKey.End];
                                    var tempdate = futuredatetimeperiod.AddDays(7);
                                    var dateTimeToSet = DateObject.MinValue.SafeCreateFromValue(tempdate.Year, tempdate.Month, tempdate.Day, tempdate.Hour,
                                                                                                 tempdate.Minute, tempdate.Second);
                                    futurevalue[DateTimeResolutionKey.End] = DateTimeFormatUtil.FormatDateTime(dateTimeToSet);
                                }

                            }
                            else
                            {
                                if (pastvalue.ContainsKey("start"))
                                {
                                    pastdatetimeperiod = DateObject.Parse(pastvalue[DateTimeResolutionKey.Start],
                                                                           CultureInfo.InvariantCulture);

                                    futurevalue[DateTimeResolutionKey.Start] = pastvalue[DateTimeResolutionKey.Start];
                                    var tempdate = pastdatetimeperiod.AddYears(-1);
                                    var dateTimeToSet = DateObject.MinValue.SafeCreateFromValue(tempdate.Year, tempdate.Month, tempdate.Day, tempdate.Hour,
                                                                                                 tempdate.Minute, tempdate.Second);
                                    pastvalue[DateTimeResolutionKey.Start] = DateTimeFormatUtil.FormatDateTime(dateTimeToSet);
                                }

                                if (pastvalue.ContainsKey("end"))
                                {
                                    pastdatetimeperiod = DateObject.Parse(pastvalue[DateTimeResolutionKey.End],
                                                                           CultureInfo.InvariantCulture);
                                    futurevalue[DateTimeResolutionKey.End] = pastvalue[DateTimeResolutionKey.End];
                                    var tempdate = pastdatetimeperiod.AddYears(-1);
                                    var dateTimeToSet = DateObject.MinValue.SafeCreateFromValue(tempdate.Year, tempdate.Month, tempdate.Day, tempdate.Hour,
                                                                                                 tempdate.Minute, tempdate.Second);
                                    pastvalue[DateTimeResolutionKey.End] = DateTimeFormatUtil.FormatDateTime(dateTimeToSet);
                                }

                            }

                        }

                    }

                    if ((valueSet.Count == 1) && slot.TimexStr.Contains("XXXX-WXX"))
                    {
                        var currvalue = valueSet.ElementAt(0);
                        bool maptonew = false;

                        if (currvalue.ContainsKey("start"))
                        {
                            var datetimeperiod = DateObject.Parse(currvalue[DateTimeResolutionKey.Start], CultureInfo.InvariantCulture);

                            if (datetimeperiod < referenceTime)
                            {
                                maptonew = true;
                            }

                        }
                        else
                        {
                            if (currvalue.ContainsKey("end"))
                            {
                                var datetimeperiod = DateObject.Parse(currvalue[DateTimeResolutionKey.End], CultureInfo.InvariantCulture);

                                if (datetimeperiod < referenceTime)
                                {
                                    maptonew = true;
                                }

                            }
                        }

                        if (maptonew)
                        {
                            if (slot.TimexStr.Contains("XXXX-WXX"))
                            {
                                if (currvalue.ContainsKey("start"))
                                {
                                    var datetimeperiod = DateObject.Parse(currvalue[DateTimeResolutionKey.Start],
                                                                            CultureInfo.InvariantCulture);
                                    var tempdate = datetimeperiod.AddDays(7);
                                    var dateTimeToSet = DateObject.MinValue.SafeCreateFromValue(tempdate.Year, tempdate.Month, tempdate.Day, tempdate.Hour,
                                                                                                 tempdate.Minute, tempdate.Second);
                                    currvalue[DateTimeResolutionKey.Start] = DateTimeFormatUtil.FormatDateTime(dateTimeToSet);
                                }

                                if (currvalue.ContainsKey("end"))
                                {
                                    var datetimeperiod = DateObject.Parse(currvalue[DateTimeResolutionKey.End],
                                                                            CultureInfo.InvariantCulture);
                                    var tempdate = datetimeperiod.AddDays(7);
                                    var dateTimeToSet = DateObject.MinValue.SafeCreateFromValue(tempdate.Year, tempdate.Month, tempdate.Day, tempdate.Hour,
                                                                                                 tempdate.Minute, tempdate.Second);
                                    currvalue[DateTimeResolutionKey.End] = DateTimeFormatUtil.FormatDateTime(dateTimeToSet);
                                }

                            }
                        }
                    }
                }
            }

            slot.Value = value;
            return slot;
        }

        /*
        If input datetime string precedes the referenceTime.
        ex if input is "meet after 7 july at 9pm" and refrence time is 7 july 2022 10pm,
        expected future value should get mapped to 7 july 2023,9pm &&
        past value get mapped to 7 july 2022,9pm.

        ex if input is "meet on  thursday at 6pm" and refrence time is 7 july 2022 (thursday) 10pm,
        expected future value should get mapped to 14 july 2022, 6pm &&
        past value get mapped to 7 july 2022, 6pm.
       */
        private static DateTimeParseResult TasksModeModifyDateTimeValue(DateTimeParseResult slot, DateObject referenceTime)
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
                    int i;
                    for (i = 0; i < valueSet.Count - 1; i = i + 2)
                    {
                        var pastvalue = valueSet.ElementAt(i);
                        var futurevalue = valueSet.ElementAt(i + 1);

                        var pastdatetime = DateObject.Parse(pastvalue[DateTimeResolutionKey.Value], CultureInfo.InvariantCulture);
                        var futuredatetime = DateObject.Parse(futurevalue[DateTimeResolutionKey.Value], CultureInfo.InvariantCulture);

                        if (futuredatetime < referenceTime)
                        {
                            if (slot.TimexStr.Contains("XXXX-WXX"))
                            {
                                pastvalue[DateTimeResolutionKey.Value] = futurevalue[DateTimeResolutionKey.Value];
                                var tempdate = futuredatetime.Date.AddDays(7);
                                var dateTimeToSet = DateObject.MinValue.SafeCreateFromValue(tempdate.Year, tempdate.Month, tempdate.Day, futuredatetime.Hour,
                                                                                             futuredatetime.Minute, futuredatetime.Second);
                                futurevalue[DateTimeResolutionKey.Value] = DateTimeFormatUtil.FormatDateTime(dateTimeToSet);
                            }
                            else
                            {
                                pastvalue[DateTimeResolutionKey.Value] = futurevalue[DateTimeResolutionKey.Value];
                                var tempdate = futuredatetime.AddYears(1);
                                var dateTimeToSet = DateObject.MinValue.SafeCreateFromValue(tempdate.Year, tempdate.Month, tempdate.Day, futuredatetime.Hour,
                                                                                             futuredatetime.Minute, futuredatetime.Second);
                                futurevalue[DateTimeResolutionKey.Value] = DateTimeFormatUtil.FormatDateTime(dateTimeToSet);
                            }

                        }

                    }
                }

            }

            slot.Value = value;
            return slot;
        }

        /*Tasksmode specific change.
       If input time string precedes the referenceTime, then the date should be set to the next day,
       and instead of returning time only, both date and time should be returned.Example:
       "Do this at 9 AM" issued when the current time is past 9 AM, e.g., 10 AM.When AM/PM is not explicitly mentioned,
       then this has to be done for both AM and PM but depending on the date, e.g., if we say "Do this at 9" but current time is
       8 PM, then we mean 9 PM on the same day or 9 AM the next day.
       */
        private static DateTimeParseResult TasksModeModifyTimeValue(DateTimeParseResult slot, DateObject referenceTime)
        {
            var value = (SortedDictionary<string, object>)slot.Value;
            var newType = $"{ParserTypeName}.{Constants.SYS_DATETIME_TIME}";
            if (value != null && value.ContainsKey(ResolutionKey.ValueSet))
            {
                if (value[ResolutionKey.ValueSet] is IList<Dictionary<string, string>> valueSet && valueSet.Any())
                {
                    foreach (var values in valueSet)
                    {
                        var inputTime = DateObject.Parse(values[DateTimeResolutionKey.Value], CultureInfo.InvariantCulture);
                        int inputHour = inputTime.Hour;
                        int inputMinute = inputTime.Minute;
                        if ((inputHour < referenceTime.Hour) || (inputHour == referenceTime.Hour && inputMinute < referenceTime.Minute))
                        {
                            var tomorrowsDate = referenceTime.Date.AddDays(1);
                            var dateTimeToSet = DateObject.MinValue.SafeCreateFromValue(tomorrowsDate.Year, tomorrowsDate.Month, tomorrowsDate.Day, inputHour, inputMinute, inputTime.Second);
                            var timeStr = inputHour.ToString("D2", CultureInfo.InvariantCulture);
                            if (inputMinute > 0)
                            {
                                timeStr += ":" + inputMinute.ToString("D2", CultureInfo.InvariantCulture);
                            }

                            values[DateTimeResolutionKey.Timex] = $"{DateTimeFormatUtil.LuisDate(dateTimeToSet)}T{timeStr}";
                            values[DateTimeResolutionKey.Value] = DateTimeFormatUtil.FormatDateTime(dateTimeToSet);
                            values[ResolutionKey.Type] = $"{Constants.SYS_DATETIME_DATETIME}";
                            if (newType == $"{ParserTypeName}.{Constants.SYS_DATETIME_TIME}")
                            {
                                newType = $"{ParserTypeName}.{Constants.SYS_DATETIME_DATETIME}";
                            }
                        }
                        else
                        {
                            if (newType == $"{ParserTypeName}.{Constants.SYS_DATETIME_DATETIME}")
                            {
                                var dateTimeToSet = DateObject.MinValue.SafeCreateFromValue(referenceTime.Year, referenceTime.Month, referenceTime.Day, inputHour, inputMinute, inputTime.Second);
                                var timeStr = inputHour.ToString("D2", CultureInfo.InvariantCulture);
                                if (inputMinute > 0)
                                {
                                    timeStr += ":" + inputMinute.ToString("D2", CultureInfo.InvariantCulture);
                                }

                                values[DateTimeResolutionKey.Timex] = $"{DateTimeFormatUtil.LuisDate(dateTimeToSet)}T{timeStr}";
                                values[DateTimeResolutionKey.Value] = DateTimeFormatUtil.FormatDateTime(dateTimeToSet);
                                values[ResolutionKey.Type] = $"{Constants.SYS_DATETIME_DATETIME}";

                            }
                        }
                    }
                }
            }

            slot.Value = value;
            slot.Type = newType;

            return slot;
        }

        /*Tasksmode specific change.
        If input timeperiod string precedes the referenceTime, then the date should be set to the next day,
        and instead of returning time only, both date and time should be returned.Example:
        "Do this in morning" issued when the current time is past 9 pm.
        */
        private static DateTimeParseResult TasksModeTimePeriodValue(DateTimeParseResult slot, DateObject referenceTime)
        {
            var value = (SortedDictionary<string, object>)slot.Value;
            var newType = $"{ParserTypeName}.{Constants.SYS_DATETIME_TIMEPERIOD}";
            if (value != null && value.ContainsKey(ResolutionKey.ValueSet))
            {
                if (value[ResolutionKey.ValueSet] is IList<Dictionary<string, string>> valueSet && valueSet.Any())
                {
                    foreach (var values in valueSet)
                    {
                        var tempDate = referenceTime.Date;
                        var timexComponents = slot.TimexStr.Split(Constants.DatePeriodTimexSplitter, StringSplitOptions.RemoveEmptyEntries);
                        var maptonextday = false;
                        var tempstr = string.Empty;
                        var timestr = "(";
                        if (values.ContainsKey("start"))
                        {
                            var startinputTime = DateObject.Parse(values[DateTimeResolutionKey.Start], CultureInfo.InvariantCulture);
                            int startinputHour = startinputTime.Hour;
                            int startinputMinute = startinputTime.Minute;
                            if ((startinputHour < referenceTime.Hour) || (startinputHour == referenceTime.Hour && startinputMinute < referenceTime.Minute))
                            {
                                maptonextday = true;
                            }
                        }
                        else
                        {
                            if (values.ContainsKey("end"))
                            {
                                var endinputTime = DateObject.Parse(values[DateTimeResolutionKey.End], CultureInfo.InvariantCulture);
                                int endinputHour = endinputTime.Hour;
                                int endtinputMinute = endinputTime.Minute;
                                if ((endinputHour < referenceTime.Hour) || (endinputHour == referenceTime.Hour && endinputHour < referenceTime.Minute))
                                {
                                    maptonextday = true;
                                }
                            }
                        }

                        if (maptonextday)
                        {
                            tempDate = referenceTime.Date.AddDays(1);
                            if (values.ContainsKey("start"))
                            {
                                var startinputTime = DateObject.Parse(values[DateTimeResolutionKey.Start], CultureInfo.InvariantCulture);
                                int startinputHour = startinputTime.Hour;
                                int startinputMinute = startinputTime.Minute;
                                var startDateTimeToSet = DateObject.MinValue.SafeCreateFromValue(tempDate.Year, tempDate.Month, tempDate.Day, startinputHour, startinputMinute, 0);
                                values[DateTimeResolutionKey.Start] = $"{DateTimeFormatUtil.FormatDateTime(startDateTimeToSet)}";
                                tempstr = startinputHour.ToString("D2", CultureInfo.InvariantCulture);
                                if (startinputMinute > 0)
                                {
                                    tempstr += ":" + startinputMinute.ToString("D2", CultureInfo.InvariantCulture);
                                }

                                timestr = "(" + $"{DateTimeFormatUtil.LuisDate(startDateTimeToSet)}T{tempstr}";

                            }

                            if (values.ContainsKey("end"))
                            {
                                var endinputTime = DateObject.Parse(values[DateTimeResolutionKey.End], CultureInfo.InvariantCulture);
                                int endinputHour = endinputTime.Hour;
                                int endinputMinute = endinputTime.Minute;
                                var endDateTimeToSet = DateObject.MinValue.SafeCreateFromValue(tempDate.Year, tempDate.Month, tempDate.Day, endinputHour, endinputMinute, 0);
                                values[DateTimeResolutionKey.End] = $"{DateTimeFormatUtil.FormatDateTime(endDateTimeToSet)}";

                                tempstr = endinputHour.ToString("D2", CultureInfo.InvariantCulture);
                                if (endinputMinute > 0)
                                {
                                    tempstr += ":" + endinputMinute.ToString("D2", CultureInfo.InvariantCulture);
                                }

                                if (timestr == "(")
                                {
                                    timestr = timestr + $"{DateTimeFormatUtil.LuisDate(endDateTimeToSet)}T{tempstr}";
                                }
                                else
                                {
                                    timestr = timestr + "," + $"{DateTimeFormatUtil.LuisDate(endDateTimeToSet)}T{tempstr}";
                                }
                            }

                            newType = $"{ParserTypeName}.{Constants.SYS_DATETIME_DATETIMEPERIOD}";
                            if (timexComponents.Length == 3)
                            {
                                timestr = timestr + "," + timexComponents[2] + ")";
                            }

                            // handling cases : afternoon, morning, night
                            else
                            {
                                timestr = $"{DateTimeFormatUtil.LuisDate(tempDate)}{values[DateTimeResolutionKey.Timex]}";

                            }

                            values[DateTimeResolutionKey.Timex] = timestr;
                            values[ResolutionKey.Type] = $"{Constants.SYS_DATETIME_DATETIMEPERIOD}";

                        }
                    }
                }
            }

            slot.Value = value;
            slot.Type = newType;

            return slot;

        }
    }
}