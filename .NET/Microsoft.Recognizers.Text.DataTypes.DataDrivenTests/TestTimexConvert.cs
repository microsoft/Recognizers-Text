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
            Assert.AreEqual("29th May 2017", TimexConvert.ConvertTimexToString(new TimexProperties("2017-05-29")));
        }

        [TestMethod]
        public void DataTypes_Convert_MonthAndDayOfMonth()
        {
            Assert.AreEqual("5th January", TimexConvert.ConvertTimexToString(new TimexProperties("XXXX-01-05")));
            Assert.AreEqual("5th February", TimexConvert.ConvertTimexToString(new TimexProperties("XXXX-02-05")));
            Assert.AreEqual("5th March", TimexConvert.ConvertTimexToString(new TimexProperties("XXXX-03-05")));
            Assert.AreEqual("5th April", TimexConvert.ConvertTimexToString(new TimexProperties("XXXX-04-05")));
            Assert.AreEqual("5th May", TimexConvert.ConvertTimexToString(new TimexProperties("XXXX-05-05")));
            Assert.AreEqual("5th June", TimexConvert.ConvertTimexToString(new TimexProperties("XXXX-06-05")));
            Assert.AreEqual("5th July", TimexConvert.ConvertTimexToString(new TimexProperties("XXXX-07-05")));
            Assert.AreEqual("5th August", TimexConvert.ConvertTimexToString(new TimexProperties("XXXX-08-05")));
            Assert.AreEqual("5th September", TimexConvert.ConvertTimexToString(new TimexProperties("XXXX-09-05")));
            Assert.AreEqual("5th October", TimexConvert.ConvertTimexToString(new TimexProperties("XXXX-10-05")));
            Assert.AreEqual("5th November", TimexConvert.ConvertTimexToString(new TimexProperties("XXXX-11-05")));
            Assert.AreEqual("5th December", TimexConvert.ConvertTimexToString(new TimexProperties("XXXX-12-05")));
        }

        [TestMethod]
        public void DataTypes_Convert_MonthAndDayOfMonthWithCorrectAbbreviation()
        {
            Assert.AreEqual("1st June", TimexConvert.ConvertTimexToString(new TimexProperties("XXXX-06-01")));
            Assert.AreEqual("2nd June", TimexConvert.ConvertTimexToString(new TimexProperties("XXXX-06-02")));
            Assert.AreEqual("3rd June", TimexConvert.ConvertTimexToString(new TimexProperties("XXXX-06-03")));
            Assert.AreEqual("4th June", TimexConvert.ConvertTimexToString(new TimexProperties("XXXX-06-04")));
        }

        [TestMethod]
        public void DataTypes_Convert_DayOfWeek()
        {
            Assert.AreEqual("Monday", TimexConvert.ConvertTimexToString(new TimexProperties("XXXX-WXX-1")));
            Assert.AreEqual("Tuesday", TimexConvert.ConvertTimexToString(new TimexProperties("XXXX-WXX-2")));
            Assert.AreEqual("Wednesday", TimexConvert.ConvertTimexToString(new TimexProperties("XXXX-WXX-3")));
            Assert.AreEqual("Thursday", TimexConvert.ConvertTimexToString(new TimexProperties("XXXX-WXX-4")));
            Assert.AreEqual("Friday", TimexConvert.ConvertTimexToString(new TimexProperties("XXXX-WXX-5")));
            Assert.AreEqual("Saturday", TimexConvert.ConvertTimexToString(new TimexProperties("XXXX-WXX-6")));
            Assert.AreEqual("Sunday", TimexConvert.ConvertTimexToString(new TimexProperties("XXXX-WXX-7")));
        }

        [TestMethod]
        public void DataTypes_Convert_Time()
        {
            Assert.AreEqual("5:30:05PM", TimexConvert.ConvertTimexToString(new TimexProperties("T17:30:05")));
            Assert.AreEqual("2:30:30AM", TimexConvert.ConvertTimexToString(new TimexProperties("T02:30:30")));
            Assert.AreEqual("12:30:30AM", TimexConvert.ConvertTimexToString(new TimexProperties("T00:30:30")));
            Assert.AreEqual("12:30:30PM", TimexConvert.ConvertTimexToString(new TimexProperties("T12:30:30")));
        }

        [TestMethod]
        public void DataTypes_Convert_HourAndMinute()
        {
            Assert.AreEqual("5:30PM", TimexConvert.ConvertTimexToString(new TimexProperties("T17:30")));
            Assert.AreEqual("5PM", TimexConvert.ConvertTimexToString(new TimexProperties("T17:00")));
            Assert.AreEqual("1:30AM", TimexConvert.ConvertTimexToString(new TimexProperties("T01:30")));
            Assert.AreEqual("1AM", TimexConvert.ConvertTimexToString(new TimexProperties("T01:00")));
        }

        [TestMethod]
        public void DataTypes_Convert_Hour()
        {
            Assert.AreEqual("midnight", TimexConvert.ConvertTimexToString(new TimexProperties("T00")));
            Assert.AreEqual("1AM", TimexConvert.ConvertTimexToString(new TimexProperties("T01")));
            Assert.AreEqual("2AM", TimexConvert.ConvertTimexToString(new TimexProperties("T02")));
            Assert.AreEqual("3AM", TimexConvert.ConvertTimexToString(new TimexProperties("T03")));
            Assert.AreEqual("4AM", TimexConvert.ConvertTimexToString(new TimexProperties("T04")));
            Assert.AreEqual("midday", TimexConvert.ConvertTimexToString(new TimexProperties("T12")));
            Assert.AreEqual("1PM", TimexConvert.ConvertTimexToString(new TimexProperties("T13")));
            Assert.AreEqual("2PM", TimexConvert.ConvertTimexToString(new TimexProperties("T14")));
            Assert.AreEqual("11PM", TimexConvert.ConvertTimexToString(new TimexProperties("T23")));
        }

        [TestMethod]
        public void DataTypes_Convert_Now()
        {
            Assert.AreEqual("now", TimexConvert.ConvertTimexToString(new TimexProperties("PRESENT_REF")));
        }

        [TestMethod]
        public void DataTypes_Convert_FullDatetime()
        {
            Assert.AreEqual("6:30:45PM 3rd January 1984", TimexConvert.ConvertTimexToString(new TimexProperties("1984-01-03T18:30:45")));
            Assert.AreEqual("midnight 1st January 2000", TimexConvert.ConvertTimexToString(new TimexProperties("2000-01-01T00")));
            Assert.AreEqual("7:30PM 29th May 1967", TimexConvert.ConvertTimexToString(new TimexProperties("1967-05-29T19:30:00")));
        }

        [TestMethod]
        public void DataTypes_Convert_ParicularTimeOnParticularDayOfWeek()
        {
            Assert.AreEqual("4PM Wednesday", TimexConvert.ConvertTimexToString(new TimexProperties("XXXX-WXX-3T16")));
            Assert.AreEqual("6:30PM Friday", TimexConvert.ConvertTimexToString(new TimexProperties("XXXX-WXX-5T18:30")));
        }

        [TestMethod]
        public void DataTypes_Convert_Year()
        {
            Assert.AreEqual("2016", TimexConvert.ConvertTimexToString(new TimexProperties("2016")));
        }

        [TestMethod]
        public void DataTypes_Convert_YearSeason()
        {
            Assert.AreEqual("summer 1999", TimexConvert.ConvertTimexToString(new TimexProperties("1999-SU")));
        }
        public void DataTypes_Convert_Season()
        {
            Assert.AreEqual("summer", TimexConvert.ConvertTimexToString(new TimexProperties("SU")));
            Assert.AreEqual("winter", TimexConvert.ConvertTimexToString(new TimexProperties("WI")));
        }

        [TestMethod]
        public void DataTypes_Convert_Month()
        {
            Assert.AreEqual("January", TimexConvert.ConvertTimexToString(new TimexProperties("XXXX-01")));
            Assert.AreEqual("May", TimexConvert.ConvertTimexToString(new TimexProperties("XXXX-05")));
            Assert.AreEqual("December", TimexConvert.ConvertTimexToString(new TimexProperties("XXXX-12")));
        }

        [TestMethod]
        public void DataTypes_Convert_MonthAndYear()
        {
            Assert.AreEqual("May 2018", TimexConvert.ConvertTimexToString(new TimexProperties("2018-05")));
        }

        [TestMethod]
        public void DataTypes_Convert_WeekOfMonth()
        {
            Assert.AreEqual("first week of January", TimexConvert.ConvertTimexToString(new TimexProperties("XXXX-01-W01")));
            Assert.AreEqual("third week of August", TimexConvert.ConvertTimexToString(new TimexProperties("XXXX-08-W03")));
        }

        [TestMethod]
        public void DataTypes_Convert_PartOfTheDay()
        {
            Assert.AreEqual("daytime", TimexConvert.ConvertTimexToString(new TimexProperties("TDT")));
            Assert.AreEqual("night", TimexConvert.ConvertTimexToString(new TimexProperties("TNI")));
            Assert.AreEqual("morning", TimexConvert.ConvertTimexToString(new TimexProperties("TMO")));
            Assert.AreEqual("afternoon", TimexConvert.ConvertTimexToString(new TimexProperties("TAF")));
            Assert.AreEqual("evening", TimexConvert.ConvertTimexToString(new TimexProperties("TEV")));
        }

        [TestMethod]
        public void DataTypes_Convert_FridayEvening()
        {
            Assert.AreEqual("Friday evening", TimexConvert.ConvertTimexToString(new TimexProperties("XXXX-WXX-5TEV")));
        }

        [TestMethod]
        public void DataTypes_Convert_DateAndPartOfDay()
        {
            Assert.AreEqual("7th September 2017 night", TimexConvert.ConvertTimexToString(new TimexProperties("2017-09-07TNI")));
        }

        [TestMethod]
        public void DataTypes_Convert_Last5Minutes()
        {
            // date + time + duration
            var timex = new TimexProperties("(2017-09-08T21:19:29,2017-09-08T21:24:29,PT5M)");
            // TODO 
        }

        [TestMethod]
        public void DataTypes_Convert_WednesdayToSaturday()
        {
            // date + duration
            var timex = new TimexProperties("(XXXX-WXX-3,XXXX-WXX-6,P3D)");
            // TODO
        }

        [TestMethod]
        public void DataTypes_Convert_Years()
        {
            Assert.AreEqual("2 years", TimexConvert.ConvertTimexToString(new TimexProperties("P2Y")));
            Assert.AreEqual("1 year", TimexConvert.ConvertTimexToString(new TimexProperties("P1Y")));
        }

        [TestMethod]
        public void DataTypes_Convert_Months()
        {
            Assert.AreEqual("4 months", TimexConvert.ConvertTimexToString(new TimexProperties("P4M")));
            Assert.AreEqual("1 month", TimexConvert.ConvertTimexToString(new TimexProperties("P1M")));
            Assert.AreEqual("0 months", TimexConvert.ConvertTimexToString(new TimexProperties("P0M")));
        }

        [TestMethod]
        public void DataTypes_Convert_Weeks()
        {
            Assert.AreEqual("6 weeks", TimexConvert.ConvertTimexToString(new TimexProperties("P6W")));
            Assert.AreEqual("9.5 weeks", TimexConvert.ConvertTimexToString(new TimexProperties("P9.5W")));
        }

        [TestMethod]
        public void DataTypes_Convert_Days()
        {
            Assert.AreEqual("5 days", TimexConvert.ConvertTimexToString(new TimexProperties("P5D")));
            Assert.AreEqual("1 day", TimexConvert.ConvertTimexToString(new TimexProperties("P1D")));
        }

        [TestMethod]
        public void DataTypes_Convert_Hours()
        {
            Assert.AreEqual("5 hours", TimexConvert.ConvertTimexToString(new TimexProperties("PT5H")));
            Assert.AreEqual("1 hour", TimexConvert.ConvertTimexToString(new TimexProperties("PT1H")));
        }

        [TestMethod]
        public void DataTypes_Convert_Minutes()
        {
            Assert.AreEqual("30 minutes", TimexConvert.ConvertTimexToString(new TimexProperties("PT30M")));
            Assert.AreEqual("1 minute", TimexConvert.ConvertTimexToString(new TimexProperties("PT1M")));
        }

        [TestMethod]
        public void DataTypes_Convert_Seconds()
        {
            Assert.AreEqual("45 seconds", TimexConvert.ConvertTimexToString(new TimexProperties("PT45S")));
        }

        [TestMethod]
        public void DataTypes_Convert_Every2Days()
        {
            Assert.AreEqual("every 2 days", TimexConvert.ConvertTimexSetToString(new TimexSet("P2D")));
        }

        [TestMethod]
        public void DataTypes_Convert_EveryWeek()
        {
            Assert.AreEqual("every week", TimexConvert.ConvertTimexSetToString(new TimexSet("P1W")));
        }

        [TestMethod]
        public void DataTypes_Convert_EveryOctober()
        {
            Assert.AreEqual("every October", TimexConvert.ConvertTimexSetToString(new TimexSet("XXXX-10")));
        }

        [TestMethod]
        public void DataTypes_Convert_EverySunday()
        {
            Assert.AreEqual("every Sunday", TimexConvert.ConvertTimexSetToString(new TimexSet("XXXX-WXX-7")));
        }

        [TestMethod]
        public void DataTypes_Convert_EveryDay()
        {
            Assert.AreEqual("every day", TimexConvert.ConvertTimexSetToString(new TimexSet("P1D")));
        }

        [TestMethod]
        public void DataTypes_Convert_EveryYear()
        {
            Assert.AreEqual("every year", TimexConvert.ConvertTimexSetToString(new TimexSet("P1Y")));
        }

        [TestMethod]
        public void DataTypes_Convert_EverySpring()
        {
            Assert.AreEqual("every spring", TimexConvert.ConvertTimexSetToString(new TimexSet("SP")));
        }

        [TestMethod]
        public void DataTypes_Convert_EveryWinter()
        {
            Assert.AreEqual("every winter", TimexConvert.ConvertTimexSetToString(new TimexSet("WI")));
        }

        [TestMethod]
        public void DataTypes_Convert_EveryEvening()
        {
            Assert.AreEqual("every evening", TimexConvert.ConvertTimexSetToString(new TimexSet("TEV")));
        }
    }
}
