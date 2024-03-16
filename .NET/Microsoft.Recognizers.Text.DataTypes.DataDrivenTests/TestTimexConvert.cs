// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Recognizers.Text.DataTypes.TimexExpression.Tests
{
    [TestClass]
    public class TestTimexConvert
    {
        [TestMethod]
        public void DataTypes_Convert_CompleteDate()
        {
            Assert.AreEqual("29th May 2017", TimexConvert.ConvertTimexToString(new TimexProperty("2017-05-29")));
        }

        [TestMethod]
        [DataRow("XXXX-01-05", "5th January")]
        [DataRow("XXXX-02-05", "5th February")]
        [DataRow("XXXX-03-05", "5th March")]
        [DataRow("XXXX-04-05", "5th April")]
        [DataRow("XXXX-05-05", "5th May")]
        [DataRow("XXXX-06-05", "5th June")]
        [DataRow("XXXX-07-05", "5th July")]
        [DataRow("XXXX-08-05", "5th August")]
        [DataRow("XXXX-09-05", "5th September")]
        [DataRow("XXXX-10-05", "5th October")]
        [DataRow("XXXX-11-05", "5th November")]
        [DataRow("XXXX-12-05", "5th December")]
        public void DataTypes_Convert_MonthAndDayOfMonth(string timex, string expected)
        {
            Assert.AreEqual(expected, TimexConvert.ConvertTimexToString(new TimexProperty(timex)));
        }

        [TestMethod]
        [DataRow("XXXX-06-01", "1st June")]
        [DataRow("XXXX-06-02", "2nd June")]
        [DataRow("XXXX-06-03", "3rd June")]
        [DataRow("XXXX-06-04", "4th June")]
        public void DataTypes_Convert_MonthAndDayOfMonthWithCorrectAbbreviation(string timex, string expected)
        {
            Assert.AreEqual(expected, TimexConvert.ConvertTimexToString(new TimexProperty(timex)));
        }

        [TestMethod]
        [DataRow("XXXX-WXX-1", "Monday")]
        [DataRow("XXXX-WXX-2", "Tuesday")]
        [DataRow("XXXX-WXX-3", "Wednesday")]
        [DataRow("XXXX-WXX-4", "Thursday")]
        [DataRow("XXXX-WXX-5", "Friday")]
        [DataRow("XXXX-WXX-6", "Saturday")]
        [DataRow("XXXX-WXX-7", "Sunday")]
        public void DataTypes_Convert_DayOfWeek(string timex, string expected)
        {
            Assert.AreEqual(expected, TimexConvert.ConvertTimexToString(new TimexProperty(timex)));
        }

        [TestMethod]
        [DataRow("T17:30:05", "5:30:05PM")]
        [DataRow("T02:30:30", "2:30:30AM")]
        [DataRow("T00:30:30", "12:30:30AM")]
        [DataRow("T12:30:30", "12:30:30PM")]
        public void DataTypes_Convert_Time(string timex, string expected)
        {
            Assert.AreEqual(expected, TimexConvert.ConvertTimexToString(new TimexProperty(timex)));
        }

        [TestMethod]
        [DataRow("T17:30", "5:30PM")]
        [DataRow("T17:00", "5PM")]
        [DataRow("T01:30", "1:30AM")]
        [DataRow("T01:00", "1AM")]
        public void DataTypes_Convert_HourAndMinute(string timex, string expected)
        {
            Assert.AreEqual(expected, TimexConvert.ConvertTimexToString(new TimexProperty(timex)));
        }

        [TestMethod]
        [DataRow("T00", "midnight")]
        [DataRow("T01", "1AM")]
        [DataRow("T02", "2AM")]
        [DataRow("T03", "3AM")]
        [DataRow("T04", "4AM")]
        [DataRow("T12", "midday")]
        [DataRow("T13", "1PM")]
        [DataRow("T14", "2PM")]
        [DataRow("T23", "11PM")]
        public void DataTypes_Convert_Hour(string timex, string expected)
        {
            Assert.AreEqual(expected, TimexConvert.ConvertTimexToString(new TimexProperty(timex)));
        }

        [TestMethod]
        [DataRow("PRESENT_REF", "now")]
        public void DataTypes_Convert_Now(string timex, string expected)
        {
            Assert.AreEqual(expected, TimexConvert.ConvertTimexToString(new TimexProperty(timex)));
        }

        [TestMethod]
        [DataRow("1984-01-03T18:30:45", "6:30:45PM 3rd January 1984")]
        [DataRow("2000-01-01T00", "midnight 1st January 2000")]
        [DataRow("1967-05-29T19:30:00", "7:30PM 29th May 1967")]
        public void DataTypes_Convert_FullDatetime(string timex, string expected)
        {
            Assert.AreEqual(expected, TimexConvert.ConvertTimexToString(new TimexProperty(timex)));
        }

        [TestMethod]
        [DataRow("XXXX-WXX-3T16", "4PM Wednesday")]
        [DataRow("XXXX-WXX-5T18:30", "6:30PM Friday")]
        public void DataTypes_Convert_ParticularTimeOnParticularDayOfWeek(string timex, string expected)
        {
            Assert.AreEqual(expected, TimexConvert.ConvertTimexToString(new TimexProperty(timex)));
        }

        [TestMethod]
        [DataRow("2016", "2016")]
        public void DataTypes_Convert_Year(string timex, string expected)
        {
            Assert.AreEqual(expected, TimexConvert.ConvertTimexToString(new TimexProperty(timex)));
        }

        [TestMethod]
        [DataRow("1999-SU", "summer 1999")]
        public void DataTypes_Convert_YearSeason(string timex, string expected)
        {
            Assert.AreEqual(expected, TimexConvert.ConvertTimexToString(new TimexProperty(timex)));
        }

        [TestMethod]
        [DataRow("SU", "summer")]
        [DataRow("WI", "winter")]
        public void DataTypes_Convert_Season(string timex, string expected)
        {
            Assert.AreEqual(expected, TimexConvert.ConvertTimexToString(new TimexProperty(timex)));
        }

        [TestMethod]
        [DataRow("XXXX-01", "January")]
        [DataRow("XXXX-05", "May")]
        [DataRow("XXXX-12", "December")]
        public void DataTypes_Convert_Month(string timex, string expected)
        {
            Assert.AreEqual(expected, TimexConvert.ConvertTimexToString(new TimexProperty(timex)));
        }

        [TestMethod]
        [DataRow("2018-05", "May 2018")]
        public void DataTypes_Convert_MonthAndYear(string timex, string expected)
        {
            Assert.AreEqual(expected, TimexConvert.ConvertTimexToString(new TimexProperty(timex)));
        }

        [TestMethod]
        [DataRow("XXXX-01-W01", "first week of January")]
        [DataRow("XXXX-08-W03", "third week of August")]
        public void DataTypes_Convert_WeekOfMonth(string timex, string expected)
        {
            Assert.AreEqual(expected, TimexConvert.ConvertTimexToString(new TimexProperty(timex)));
        }

        [TestMethod]
        [DataRow("TDT", "daytime")]
        [DataRow("TNI", "night")]
        [DataRow("TMO", "morning")]
        [DataRow("TAF", "afternoon")]
        [DataRow("TEV", "evening")]
        public void DataTypes_Convert_PartOfTheDay(string timex, string expected)
        {
            Assert.AreEqual(expected, TimexConvert.ConvertTimexToString(new TimexProperty(timex)));
        }

        [TestMethod]
        [DataRow("(XXXX-XX-XX,XXXX-XX-XX,P1D)", "1 day")]
        public void DataTypes_Convert_Period(string timex, string expected)
        {
            Assert.AreEqual(expected, TimexConvert.ConvertTimexToString(new TimexProperty(timex)));
        }

        [TestMethod]
        [DataRow("XXXX-WXX-5TEV", "Friday evening")]
        public void DataTypes_Convert_FridayEvening(string timex, string expected)
        {
            Assert.AreEqual(expected, TimexConvert.ConvertTimexToString(new TimexProperty(timex)));
        }

        [TestMethod]
        [DataRow("2017-09-07TNI", "7th September 2017 night")]
        public void DataTypes_Convert_DateAndPartOfDay(string timex, string expected)
        {
            Assert.AreEqual(expected, TimexConvert.ConvertTimexToString(new TimexProperty(timex)));
        }

        [TestMethod]
        [DataRow("(2017-09-08T21:19:29,2017-09-08T21:24:29,PT5M)", "8th September 2017 9:19:29PM to 9:24:29PM")]
        public void DataTypes_Convert_Last5Minutes(string timex, string expected)
        {
            Assert.AreEqual(expected, TimexConvert.ConvertTimexToString(new TimexProperty(timex)));
        }

        [TestMethod]
        [DataRow("(XXXX-WXX-3,XXXX-WXX-6,P3D)", "Wednesday to Saturday")]
        public void DataTypes_Convert_DateRange(string timex, string expected)
        {
            Assert.AreEqual(expected, TimexConvert.ConvertTimexToString(new TimexProperty(timex)));
        }

        [TestMethod]
        [DataRow("P2Y", "2 years")]
        [DataRow("P1Y", "1 year")]
        public void DataTypes_Convert_Years(string timex, string expected)
        {
            Assert.AreEqual(expected, TimexConvert.ConvertTimexToString(new TimexProperty(timex)));
        }

        [TestMethod]
        [DataRow("P4M", "4 months")]
        [DataRow("P1M", "1 month")]
        [DataRow("P0M", "0 months")]
        public void DataTypes_Convert_Months(string timex, string expected)
        {
            Assert.AreEqual(expected, TimexConvert.ConvertTimexToString(new TimexProperty(timex)));
        }

        [TestMethod]
        [DataRow("P6W", "6 weeks")]
        [DataRow("P9.5W", "9.5 weeks")]
        public void DataTypes_Convert_Weeks(string timex, string expected)
        {
            Assert.AreEqual(expected, TimexConvert.ConvertTimexToString(new TimexProperty(timex)));
        }

        [TestMethod]
        [DataRow("P5D", "5 days")]
        [DataRow("P1D", "1 day")]
        public void DataTypes_Convert_Days(string timex, string expected)
        {
            Assert.AreEqual(expected, TimexConvert.ConvertTimexToString(new TimexProperty(timex)));
        }

        [TestMethod]
        [DataRow("PT5H", "5 hours")]
        [DataRow("PT1H", "1 hour")]
        public void DataTypes_Convert_Hours(string timex, string expected)
        {
            Assert.AreEqual(expected, TimexConvert.ConvertTimexToString(new TimexProperty(timex)));
        }

        [TestMethod]
        [DataRow("PT30M", "30 minutes")]
        [DataRow("PT1M", "1 minute")]
        public void DataTypes_Convert_Minutes(string timex, string expected)
        {
            Assert.AreEqual(expected, TimexConvert.ConvertTimexToString(new TimexProperty(timex)));
        }

        [TestMethod]
        [DataRow("PT45S", "45 seconds")]
        public void DataTypes_Convert_Seconds(string timex, string expected)
        {
            Assert.AreEqual(expected, TimexConvert.ConvertTimexToString(new TimexProperty(timex)));
        }

        [TestMethod]
        [DataRow("P2D", "every 2 days")]
        public void DataTypes_Convert_Every2Days(string timex, string expected)
        {
            Assert.AreEqual(expected, TimexConvert.ConvertTimexSetToString(new TimexSet(timex)));
        }

        [TestMethod]
        [DataRow("P1W", "every week")]
        public void DataTypes_Convert_EveryWeek(string timex, string expected)
        {
            Assert.AreEqual(expected, TimexConvert.ConvertTimexSetToString(new TimexSet(timex)));
        }

        [TestMethod]
        [DataRow("XXXX-10", "every October")]
        public void DataTypes_Convert_EveryOctober(string timex, string expected)
        {
            Assert.AreEqual(expected, TimexConvert.ConvertTimexSetToString(new TimexSet(timex)));
        }

        [TestMethod]
        [DataRow("XXXX-WXX-7", "every Sunday")]
        public void DataTypes_Convert_EverySunday(string timex, string expected)
        {
            Assert.AreEqual(expected, TimexConvert.ConvertTimexSetToString(new TimexSet(timex)));
        }

        [TestMethod]
        [DataRow("P1D", "every day")]
        public void DataTypes_Convert_EveryDay(string timex, string expected)
        {
            Assert.AreEqual(expected, TimexConvert.ConvertTimexSetToString(new TimexSet(timex)));
        }

        [TestMethod]
        [DataRow("P1Y", "every year")]
        public void DataTypes_Convert_EveryYear(string timex, string expected)
        {
            Assert.AreEqual(expected, TimexConvert.ConvertTimexSetToString(new TimexSet(timex)));
        }

        [TestMethod]
        [DataRow("SP", "every spring")]
        public void DataTypes_Convert_EverySpring(string timex, string expected)
        {
            Assert.AreEqual(expected, TimexConvert.ConvertTimexSetToString(new TimexSet(timex)));
        }

        [TestMethod]
        [DataRow("WI", "every winter")]
        public void DataTypes_Convert_EveryWinter(string timex, string expected)
        {
            Assert.AreEqual(expected, TimexConvert.ConvertTimexSetToString(new TimexSet(timex)));
        }

        [TestMethod]
        [DataRow("TEV", "every evening")]
        public void DataTypes_Convert_EveryEvening(string timex, string expected)
        {
            Assert.AreEqual(expected, TimexConvert.ConvertTimexSetToString(new TimexSet(timex)));
        }
    }
}
