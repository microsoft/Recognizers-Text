// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Recognizers.Text.DataTypes.TimexExpression.Tests
{
    [TestClass]
    public class TestTimexParsing
    {
        [TestMethod]
        public void DataTypes_Parsing_CompleteDate()
        {
            var timex = new TimexProperty("2017-05-29");
            CollectionAssert.AreEquivalent(new[] { Constants.TimexTypes.Definite, Constants.TimexTypes.Date }, timex.Types.ToList());

            Assert.AreEqual(2017, timex.Year);
            Assert.AreEqual(5, timex.Month);
            Assert.AreEqual(29, timex.DayOfMonth);
            Assert.IsNull(timex.DayOfWeek);
            Assert.IsNull(timex.WeekOfYear);
            Assert.IsNull(timex.WeekOfMonth);
            Assert.IsNull(timex.Season);
            Assert.IsNull(timex.Hour);
            Assert.IsNull(timex.Minute);
            Assert.IsNull(timex.Second);
            Assert.IsNull(timex.Weekend);
            Assert.IsNull(timex.PartOfDay);
            Assert.IsNull(timex.Years);
            Assert.IsNull(timex.Months);
            Assert.IsNull(timex.Weeks);
            Assert.IsNull(timex.Days);
            Assert.IsNull(timex.Hours);
            Assert.IsNull(timex.Minutes);
            Assert.IsNull(timex.Seconds);
            Assert.IsNull(timex.Now);
        }

        [TestMethod]
        public void DataTypes_Parsing_MonthAndDayOfMonth()
        {
            var timex = new TimexProperty("XXXX-12-05");
            CollectionAssert.AreEquivalent(new[] { Constants.TimexTypes.Date }, timex.Types.ToList());

            Assert.IsNull(timex.Year);
            Assert.AreEqual(12, timex.Month);
            Assert.AreEqual(5, timex.DayOfMonth);
            Assert.IsNull(timex.DayOfWeek);
            Assert.IsNull(timex.WeekOfYear);
            Assert.IsNull(timex.WeekOfMonth);
            Assert.IsNull(timex.Season);
            Assert.IsNull(timex.Hour);
            Assert.IsNull(timex.Minute);
            Assert.IsNull(timex.Second);
            Assert.IsNull(timex.Weekend);
            Assert.IsNull(timex.PartOfDay);
            Assert.IsNull(timex.Years);
            Assert.IsNull(timex.Months);
            Assert.IsNull(timex.Weeks);
            Assert.IsNull(timex.Days);
            Assert.IsNull(timex.Hours);
            Assert.IsNull(timex.Minutes);
            Assert.IsNull(timex.Seconds);
            Assert.IsNull(timex.Now);
        }

        [TestMethod]
        public void DataTypes_Parsing_DayOfWeek()
        {
            var timex = new TimexProperty("XXXX-WXX-3");
            CollectionAssert.AreEquivalent(new[] { Constants.TimexTypes.Date }, timex.Types.ToList());

            Assert.IsNull(timex.Year);
            Assert.IsNull(timex.Month);
            Assert.IsNull(timex.DayOfMonth);
            Assert.AreEqual(timex.DayOfWeek, 3);
            Assert.IsNull(timex.WeekOfYear);
            Assert.IsNull(timex.WeekOfMonth);
            Assert.IsNull(timex.Season);
            Assert.IsNull(timex.Hour);
            Assert.IsNull(timex.Minute);
            Assert.IsNull(timex.Second);
            Assert.IsNull(timex.Weekend);
            Assert.IsNull(timex.PartOfDay);
            Assert.IsNull(timex.Years);
            Assert.IsNull(timex.Months);
            Assert.IsNull(timex.Weeks);
            Assert.IsNull(timex.Days);
            Assert.IsNull(timex.Hours);
            Assert.IsNull(timex.Minutes);
            Assert.IsNull(timex.Seconds);
            Assert.IsNull(timex.Now);
        }

        [TestMethod]
        public void DataTypes_Parsing_HoursMinutesAndSeconds()
        {
            var timex = new TimexProperty("T17:30:05");
            CollectionAssert.AreEquivalent(new[] { Constants.TimexTypes.Time }, timex.Types.ToList());

            Assert.IsNull(timex.Year);
            Assert.IsNull(timex.Month);
            Assert.IsNull(timex.DayOfMonth);
            Assert.IsNull(timex.DayOfWeek);
            Assert.IsNull(timex.WeekOfYear);
            Assert.IsNull(timex.WeekOfMonth);
            Assert.IsNull(timex.Season);
            Assert.AreEqual(17, timex.Hour);
            Assert.AreEqual(30, timex.Minute);
            Assert.AreEqual(5, timex.Second);
            Assert.IsNull(timex.Weekend);
            Assert.IsNull(timex.PartOfDay);
            Assert.IsNull(timex.Years);
            Assert.IsNull(timex.Months);
            Assert.IsNull(timex.Weeks);
            Assert.IsNull(timex.Days);
            Assert.IsNull(timex.Hours);
            Assert.IsNull(timex.Minutes);
            Assert.IsNull(timex.Seconds);
            Assert.IsNull(timex.Now);
        }

        [TestMethod]
        public void DataTypes_Parsing_HoursAndMinutes()
        {
            var timex = new TimexProperty("T17:30");
            CollectionAssert.AreEquivalent(new[] { Constants.TimexTypes.Time }, timex.Types.ToList());

            Assert.IsNull(timex.Year);
            Assert.IsNull(timex.Month);
            Assert.IsNull(timex.DayOfMonth);
            Assert.IsNull(timex.DayOfWeek);
            Assert.IsNull(timex.WeekOfYear);
            Assert.IsNull(timex.WeekOfMonth);
            Assert.IsNull(timex.Season);
            Assert.AreEqual(17, timex.Hour);
            Assert.AreEqual(30, timex.Minute);
            Assert.AreEqual(0, timex.Second);
            Assert.IsNull(timex.Weekend);
            Assert.IsNull(timex.PartOfDay);
            Assert.IsNull(timex.Years);
            Assert.IsNull(timex.Months);
            Assert.IsNull(timex.Weeks);
            Assert.IsNull(timex.Days);
            Assert.IsNull(timex.Hours);
            Assert.IsNull(timex.Minutes);
            Assert.IsNull(timex.Seconds);
            Assert.IsNull(timex.Now);
        }

        [TestMethod]
        public void DataTypes_Parsing_Hours()
        {
            var timex = new TimexProperty("T17");
            CollectionAssert.AreEquivalent(new[] { Constants.TimexTypes.Time }, timex.Types.ToList());

            Assert.IsNull(timex.Year);
            Assert.IsNull(timex.Month);
            Assert.IsNull(timex.DayOfMonth);
            Assert.IsNull(timex.DayOfWeek);
            Assert.IsNull(timex.WeekOfYear);
            Assert.IsNull(timex.WeekOfMonth);
            Assert.IsNull(timex.Season);
            Assert.AreEqual(17, timex.Hour);
            Assert.AreEqual(0, timex.Minute);
            Assert.AreEqual(0, timex.Second);
            Assert.IsNull(timex.Weekend);
            Assert.IsNull(timex.PartOfDay);
            Assert.IsNull(timex.Years);
            Assert.IsNull(timex.Months);
            Assert.IsNull(timex.Weeks);
            Assert.IsNull(timex.Days);
            Assert.IsNull(timex.Hours);
            Assert.IsNull(timex.Minutes);
            Assert.IsNull(timex.Seconds);
            Assert.IsNull(timex.Now);
        }

        [TestMethod]
        public void DataTypes_Parsing_Now()
        {
            var timex = new TimexProperty("PRESENT_REF");
            CollectionAssert.AreEquivalent(
                new[]
                {
                Constants.TimexTypes.Present,
                Constants.TimexTypes.Date,
                Constants.TimexTypes.Time,
                Constants.TimexTypes.DateTime,
                }, timex.Types.ToList());

            Assert.IsNull(timex.Year);
            Assert.IsNull(timex.Month);
            Assert.IsNull(timex.DayOfMonth);
            Assert.IsNull(timex.DayOfWeek);
            Assert.IsNull(timex.WeekOfYear);
            Assert.IsNull(timex.WeekOfMonth);
            Assert.IsNull(timex.Season);
            Assert.IsNull(timex.Hour);
            Assert.IsNull(timex.Minute);
            Assert.IsNull(timex.Second);
            Assert.IsNull(timex.Weekend);
            Assert.IsNull(timex.PartOfDay);
            Assert.IsNull(timex.Years);
            Assert.IsNull(timex.Months);
            Assert.IsNull(timex.Weeks);
            Assert.IsNull(timex.Days);
            Assert.IsNull(timex.Hours);
            Assert.IsNull(timex.Minutes);
            Assert.IsNull(timex.Seconds);
            Assert.AreEqual(true, timex.Now);
        }

        [TestMethod]
        public void DataTypes_Parsing_FullDatetime()
        {
            var timex = new TimexProperty("1984-01-03T18:30:45");
            CollectionAssert.AreEquivalent(
                new[]
                {
                Constants.TimexTypes.Definite,
                Constants.TimexTypes.Date,
                Constants.TimexTypes.Time,
                Constants.TimexTypes.DateTime,
                }, timex.Types.ToList());

            Assert.AreEqual(1984, timex.Year);
            Assert.AreEqual(1, timex.Month);
            Assert.AreEqual(3, timex.DayOfMonth);
            Assert.IsNull(timex.DayOfWeek);
            Assert.IsNull(timex.WeekOfYear);
            Assert.IsNull(timex.WeekOfMonth);
            Assert.IsNull(timex.Season);
            Assert.AreEqual(18, timex.Hour);
            Assert.AreEqual(30, timex.Minute);
            Assert.AreEqual(45, timex.Second);
            Assert.IsNull(timex.Weekend);
            Assert.IsNull(timex.PartOfDay);
            Assert.IsNull(timex.Years);
            Assert.IsNull(timex.Months);
            Assert.IsNull(timex.Weeks);
            Assert.IsNull(timex.Days);
            Assert.IsNull(timex.Hours);
            Assert.IsNull(timex.Minutes);
            Assert.IsNull(timex.Seconds);
            Assert.IsNull(timex.Now);
        }

        [TestMethod]
        public void DataTypes_Parsing_ParicularTimeOnParticularDayOfWeek()
        {
            var timex = new TimexProperty("XXXX-WXX-3T16");
            CollectionAssert.AreEquivalent(
                new[] { Constants.TimexTypes.Time, Constants.TimexTypes.Date, Constants.TimexTypes.DateTime }, timex.Types.ToList());

            Assert.IsNull(timex.Year);
            Assert.IsNull(timex.Month);
            Assert.IsNull(timex.DayOfMonth);
            Assert.AreEqual(3, timex.DayOfWeek);
            Assert.IsNull(timex.WeekOfYear);
            Assert.IsNull(timex.WeekOfMonth);
            Assert.IsNull(timex.Season);
            Assert.AreEqual(16, timex.Hour);
            Assert.AreEqual(0, timex.Minute);
            Assert.AreEqual(0, timex.Second);
            Assert.IsNull(timex.Weekend);
            Assert.IsNull(timex.PartOfDay);
            Assert.IsNull(timex.Years);
            Assert.IsNull(timex.Months);
            Assert.IsNull(timex.Weeks);
            Assert.IsNull(timex.Days);
            Assert.IsNull(timex.Hours);
            Assert.IsNull(timex.Minutes);
            Assert.IsNull(timex.Seconds);
            Assert.IsNull(timex.Now);
        }

        [TestMethod]
        public void DataTypes_Parsing_Year()
        {
            var timex = new TimexProperty("2016");
            CollectionAssert.AreEquivalent(new[] { Constants.TimexTypes.DateRange }, timex.Types.ToList());

            Assert.AreEqual(2016, timex.Year);
            Assert.IsNull(timex.Month);
            Assert.IsNull(timex.DayOfMonth);
            Assert.IsNull(timex.DayOfWeek);
            Assert.IsNull(timex.WeekOfYear);
            Assert.IsNull(timex.WeekOfMonth);
            Assert.IsNull(timex.Season);
            Assert.IsNull(timex.Hour);
            Assert.IsNull(timex.Minute);
            Assert.IsNull(timex.Second);
            Assert.IsNull(timex.Weekend);
            Assert.IsNull(timex.PartOfDay);
            Assert.IsNull(timex.Years);
            Assert.IsNull(timex.Months);
            Assert.IsNull(timex.Weeks);
            Assert.IsNull(timex.Days);
            Assert.IsNull(timex.Hours);
            Assert.IsNull(timex.Minutes);
            Assert.IsNull(timex.Seconds);
            Assert.IsNull(timex.Now);
        }

        [TestMethod]
        public void DataTypes_Parsing_SummerOf1999()
        {
            var timex = new TimexProperty("1999-SU");
            CollectionAssert.AreEquivalent(new[] { Constants.TimexTypes.DateRange }, timex.Types.ToList());

            Assert.AreEqual(1999, timex.Year);
            Assert.IsNull(timex.Month);
            Assert.IsNull(timex.DayOfMonth);
            Assert.IsNull(timex.DayOfWeek);
            Assert.IsNull(timex.WeekOfYear);
            Assert.IsNull(timex.WeekOfMonth);
            Assert.AreEqual("SU", timex.Season);
            Assert.IsNull(timex.Hour);
            Assert.IsNull(timex.Minute);
            Assert.IsNull(timex.Second);
            Assert.IsNull(timex.Weekend);
            Assert.IsNull(timex.PartOfDay);
            Assert.IsNull(timex.Years);
            Assert.IsNull(timex.Months);
            Assert.IsNull(timex.Weeks);
            Assert.IsNull(timex.Days);
            Assert.IsNull(timex.Hours);
            Assert.IsNull(timex.Minutes);
            Assert.IsNull(timex.Seconds);
            Assert.IsNull(timex.Now);
        }

        [TestMethod]
        public void DataTypes_Parsing_YearAndWeek()
        {
            var timex = new TimexProperty("2017-W37");
            CollectionAssert.AreEquivalent(new[] { Constants.TimexTypes.DateRange }, timex.Types.ToList());

            Assert.AreEqual(2017, timex.Year);
            Assert.IsNull(timex.Month);
            Assert.IsNull(timex.DayOfMonth);
            Assert.IsNull(timex.DayOfWeek);
            Assert.AreEqual(37, timex.WeekOfYear);
            Assert.IsNull(timex.WeekOfMonth);
            Assert.IsNull(timex.Season);
            Assert.IsNull(timex.Hour);
            Assert.IsNull(timex.Minute);
            Assert.IsNull(timex.Second);
            Assert.IsNull(timex.Weekend);
            Assert.IsNull(timex.PartOfDay);
            Assert.IsNull(timex.Years);
            Assert.IsNull(timex.Months);
            Assert.IsNull(timex.Weeks);
            Assert.IsNull(timex.Days);
            Assert.IsNull(timex.Hours);
            Assert.IsNull(timex.Minutes);
            Assert.IsNull(timex.Seconds);
            Assert.IsNull(timex.Now);
        }

        [TestMethod]
        public void DataTypes_Parsing_SeasonSummer()
        {
            var timex = new TimexProperty("SU");
            CollectionAssert.AreEquivalent(new[] { Constants.TimexTypes.DateRange }, timex.Types.ToList());

            Assert.IsNull(timex.Year);
            Assert.IsNull(timex.Month);
            Assert.IsNull(timex.DayOfMonth);
            Assert.IsNull(timex.DayOfWeek);
            Assert.IsNull(timex.WeekOfYear);
            Assert.IsNull(timex.WeekOfMonth);
            Assert.AreEqual("SU", timex.Season);
            Assert.IsNull(timex.Hour);
            Assert.IsNull(timex.Minute);
            Assert.IsNull(timex.Second);
            Assert.IsNull(timex.Weekend);
            Assert.IsNull(timex.PartOfDay);
            Assert.IsNull(timex.Years);
            Assert.IsNull(timex.Months);
            Assert.IsNull(timex.Weeks);
            Assert.IsNull(timex.Days);
            Assert.IsNull(timex.Hours);
            Assert.IsNull(timex.Minutes);
            Assert.IsNull(timex.Seconds);
            Assert.IsNull(timex.Now);
        }

        [TestMethod]
        public void DataTypes_Parsing_SeasonWinter()
        {
            var timex = new TimexProperty("WI");
            CollectionAssert.AreEquivalent(new[] { Constants.TimexTypes.DateRange }, timex.Types.ToList());

            Assert.IsNull(timex.Year);
            Assert.IsNull(timex.Month);
            Assert.IsNull(timex.DayOfMonth);
            Assert.IsNull(timex.DayOfWeek);
            Assert.IsNull(timex.WeekOfYear);
            Assert.IsNull(timex.WeekOfMonth);
            Assert.AreEqual("WI", timex.Season);
            Assert.IsNull(timex.Hour);
            Assert.IsNull(timex.Minute);
            Assert.IsNull(timex.Second);
            Assert.IsNull(timex.Weekend);
            Assert.IsNull(timex.PartOfDay);
            Assert.IsNull(timex.Years);
            Assert.IsNull(timex.Months);
            Assert.IsNull(timex.Weeks);
            Assert.IsNull(timex.Days);
            Assert.IsNull(timex.Hours);
            Assert.IsNull(timex.Minutes);
            Assert.IsNull(timex.Seconds);
            Assert.IsNull(timex.Now);
        }

        [TestMethod]
        public void DataTypes_Parsing_YearAndWeekend()
        {
            var timex = new TimexProperty("2017-W37-WE");
            CollectionAssert.AreEquivalent(new[] { Constants.TimexTypes.DateRange }, timex.Types.ToList());

            Assert.AreEqual(2017, timex.Year);
            Assert.IsNull(timex.Month);
            Assert.IsNull(timex.DayOfMonth);
            Assert.IsNull(timex.DayOfWeek);
            Assert.AreEqual(37, timex.WeekOfYear);
            Assert.IsNull(timex.WeekOfMonth);
            Assert.IsNull(timex.Season);
            Assert.IsNull(timex.Hour);
            Assert.IsNull(timex.Minute);
            Assert.IsNull(timex.Second);
            Assert.AreEqual(true, timex.Weekend);
            Assert.IsNull(timex.PartOfDay);
            Assert.IsNull(timex.Years);
            Assert.IsNull(timex.Months);
            Assert.IsNull(timex.Weeks);
            Assert.IsNull(timex.Days);
            Assert.IsNull(timex.Hours);
            Assert.IsNull(timex.Minutes);
            Assert.IsNull(timex.Seconds);
            Assert.IsNull(timex.Now);
        }

        [TestMethod]
        public void DataTypes_Parsing_May()
        {
            var timex = new TimexProperty("XXXX-05");
            CollectionAssert.AreEquivalent(new[] { Constants.TimexTypes.DateRange }, timex.Types.ToList());

            Assert.IsNull(timex.Year);
            Assert.AreEqual(5, timex.Month);
            Assert.IsNull(timex.DayOfMonth);
            Assert.IsNull(timex.DayOfWeek);
            Assert.IsNull(timex.WeekOfYear);
            Assert.IsNull(timex.WeekOfMonth);
            Assert.IsNull(timex.Season);
            Assert.IsNull(timex.Hour);
            Assert.IsNull(timex.Minute);
            Assert.IsNull(timex.Second);
            Assert.IsNull(timex.Weekend);
            Assert.IsNull(timex.PartOfDay);
            Assert.IsNull(timex.Years);
            Assert.IsNull(timex.Months);
            Assert.IsNull(timex.Weeks);
            Assert.IsNull(timex.Days);
            Assert.IsNull(timex.Hours);
            Assert.IsNull(timex.Minutes);
            Assert.IsNull(timex.Seconds);
            Assert.IsNull(timex.Now);
        }

        [TestMethod]
        public void DataTypes_Parsing_July2020()
        {
            var timex = new TimexProperty("2020-07");
            CollectionAssert.AreEquivalent(new[] { Constants.TimexTypes.DateRange }, timex.Types.ToList());

            Assert.AreEqual(2020, timex.Year);
            Assert.AreEqual(7, timex.Month);
            Assert.IsNull(timex.DayOfMonth);
            Assert.IsNull(timex.DayOfWeek);
            Assert.IsNull(timex.WeekOfYear);
            Assert.IsNull(timex.WeekOfMonth);
            Assert.IsNull(timex.Season);
            Assert.IsNull(timex.Hour);
            Assert.IsNull(timex.Minute);
            Assert.IsNull(timex.Second);
            Assert.IsNull(timex.Weekend);
            Assert.IsNull(timex.PartOfDay);
            Assert.IsNull(timex.Years);
            Assert.IsNull(timex.Months);
            Assert.IsNull(timex.Weeks);
            Assert.IsNull(timex.Days);
            Assert.IsNull(timex.Hours);
            Assert.IsNull(timex.Minutes);
            Assert.IsNull(timex.Seconds);
            Assert.IsNull(timex.Now);
        }

        [TestMethod]
        public void DataTypes_Parsing_WeekOfMonth()
        {
            var timex = new TimexProperty("XXXX-01-W01");
            CollectionAssert.AreEquivalent(new[] { Constants.TimexTypes.DateRange }, timex.Types.ToList());

            Assert.IsNull(timex.Year);
            Assert.AreEqual(1, timex.Month);
            Assert.IsNull(timex.DayOfMonth);
            Assert.IsNull(timex.DayOfWeek);
            Assert.IsNull(timex.WeekOfYear);
            Assert.AreEqual(1, timex.WeekOfMonth);
            Assert.IsNull(timex.Season);
            Assert.IsNull(timex.Hour);
            Assert.IsNull(timex.Minute);
            Assert.IsNull(timex.Second);
            Assert.IsNull(timex.Weekend);
            Assert.IsNull(timex.PartOfDay);
            Assert.IsNull(timex.Years);
            Assert.IsNull(timex.Months);
            Assert.IsNull(timex.Weeks);
            Assert.IsNull(timex.Days);
            Assert.IsNull(timex.Hours);
            Assert.IsNull(timex.Minutes);
            Assert.IsNull(timex.Seconds);
            Assert.IsNull(timex.Now);
        }

        [TestMethod]
        public void DataTypes_Parsing_WednesdayToSaturday()
        {
            var timex = new TimexProperty("(XXXX-WXX-3,XXXX-WXX-6,P3D)");
            CollectionAssert.AreEquivalent(
                new[]
                {
                Constants.TimexTypes.Date,
                Constants.TimexTypes.Duration,
                Constants.TimexTypes.DateRange,
                }, timex.Types.ToList());

            Assert.IsNull(timex.Year);
            Assert.IsNull(timex.Month);
            Assert.IsNull(timex.DayOfMonth);
            Assert.AreEqual(3, timex.DayOfWeek);
            Assert.IsNull(timex.WeekOfYear);
            Assert.IsNull(timex.WeekOfMonth);
            Assert.IsNull(timex.Season);
            Assert.IsNull(timex.Hour);
            Assert.IsNull(timex.Minute);
            Assert.IsNull(timex.Second);
            Assert.IsNull(timex.Weekend);
            Assert.IsNull(timex.PartOfDay);
            Assert.IsNull(timex.Years);
            Assert.IsNull(timex.Months);
            Assert.IsNull(timex.Weeks);
            Assert.AreEqual(3, timex.Days);
            Assert.IsNull(timex.Hours);
            Assert.IsNull(timex.Minutes);
            Assert.IsNull(timex.Seconds);
            Assert.IsNull(timex.Now);
        }

        [TestMethod]
        public void DataTypes_Parsing_Jan1ToAug5()
        {
            var timex = new TimexProperty("(XXXX-01-01,XXXX-08-05,P216D)");
            CollectionAssert.AreEquivalent(
                new[]
                {
                Constants.TimexTypes.Date,
                Constants.TimexTypes.Duration,
                Constants.TimexTypes.DateRange,
                }, timex.Types.ToList());

            Assert.IsNull(timex.Year);
            Assert.AreEqual(1, timex.Month);
            Assert.AreEqual(1, timex.DayOfMonth);
            Assert.IsNull(timex.DayOfWeek);
            Assert.IsNull(timex.WeekOfYear);
            Assert.IsNull(timex.WeekOfMonth);
            Assert.IsNull(timex.Season);
            Assert.IsNull(timex.Hour);
            Assert.IsNull(timex.Minute);
            Assert.IsNull(timex.Second);
            Assert.IsNull(timex.Weekend);
            Assert.IsNull(timex.PartOfDay);
            Assert.IsNull(timex.Years);
            Assert.IsNull(timex.Months);
            Assert.IsNull(timex.Weeks);
            Assert.AreEqual(216, timex.Days);
            Assert.IsNull(timex.Hours);
            Assert.IsNull(timex.Minutes);
            Assert.IsNull(timex.Seconds);
            Assert.IsNull(timex.Now);
        }

        [TestMethod]
        public void DataTypes_Parsing_Jan1ToAug5Year2015()
        {
            var timex = new TimexProperty("(2015-01-01,2015-08-05,P216D)");
            CollectionAssert.AreEquivalent(
                new[]
                {
                Constants.TimexTypes.Definite,
                Constants.TimexTypes.Date,
                Constants.TimexTypes.Duration,
                Constants.TimexTypes.DateRange,
                }, timex.Types.ToList());

            Assert.AreEqual(2015, timex.Year);
            Assert.AreEqual(1, timex.Month);
            Assert.AreEqual(1, timex.DayOfMonth);
            Assert.IsNull(timex.DayOfWeek);
            Assert.IsNull(timex.WeekOfYear);
            Assert.IsNull(timex.WeekOfMonth);
            Assert.IsNull(timex.Season);
            Assert.IsNull(timex.Hour);
            Assert.IsNull(timex.Minute);
            Assert.IsNull(timex.Second);
            Assert.IsNull(timex.Weekend);
            Assert.IsNull(timex.PartOfDay);
            Assert.IsNull(timex.Years);
            Assert.IsNull(timex.Months);
            Assert.IsNull(timex.Weeks);
            Assert.AreEqual(216, timex.Days);
            Assert.IsNull(timex.Hours);
            Assert.IsNull(timex.Minutes);
            Assert.IsNull(timex.Seconds);
            Assert.IsNull(timex.Now);
        }

        [TestMethod]
        public void DataTypes_Parsing_DayTime()
        {
            var timex = new TimexProperty("TDT");
            CollectionAssert.AreEquivalent(new[] { Constants.TimexTypes.TimeRange }, timex.Types.ToList());

            Assert.IsNull(timex.Year);
            Assert.IsNull(timex.Month);
            Assert.IsNull(timex.DayOfMonth);
            Assert.IsNull(timex.DayOfWeek);
            Assert.IsNull(timex.WeekOfYear);
            Assert.IsNull(timex.WeekOfMonth);
            Assert.IsNull(timex.Season);
            Assert.IsNull(timex.Hour);
            Assert.IsNull(timex.Minute);
            Assert.IsNull(timex.Second);
            Assert.IsNull(timex.Weekend);
            Assert.AreEqual("DT", timex.PartOfDay);
            Assert.IsNull(timex.Years);
            Assert.IsNull(timex.Months);
            Assert.IsNull(timex.Weeks);
            Assert.IsNull(timex.Days);
            Assert.IsNull(timex.Hours);
            Assert.IsNull(timex.Minutes);
            Assert.IsNull(timex.Seconds);
            Assert.IsNull(timex.Now);
        }

        [TestMethod]
        public void DataTypes_Parsing_NightTime()
        {
            var timex = new TimexProperty("TNI");
            CollectionAssert.AreEquivalent(new[] { Constants.TimexTypes.TimeRange }, timex.Types.ToList());

            Assert.IsNull(timex.Year);
            Assert.IsNull(timex.Month);
            Assert.IsNull(timex.DayOfMonth);
            Assert.IsNull(timex.DayOfWeek);
            Assert.IsNull(timex.WeekOfYear);
            Assert.IsNull(timex.WeekOfMonth);
            Assert.IsNull(timex.Season);
            Assert.IsNull(timex.Hour);
            Assert.IsNull(timex.Minute);
            Assert.IsNull(timex.Second);
            Assert.IsNull(timex.Weekend);
            Assert.AreEqual("NI", timex.PartOfDay);
            Assert.IsNull(timex.Years);
            Assert.IsNull(timex.Months);
            Assert.IsNull(timex.Weeks);
            Assert.IsNull(timex.Days);
            Assert.IsNull(timex.Hours);
            Assert.IsNull(timex.Minutes);
            Assert.IsNull(timex.Seconds);
            Assert.IsNull(timex.Now);
        }

        [TestMethod]
        public void DataTypes_Parsing_Morning()
        {
            var timex = new TimexProperty("TMO");
            CollectionAssert.AreEquivalent(new[] { Constants.TimexTypes.TimeRange }, timex.Types.ToList());

            Assert.IsNull(timex.Year);
            Assert.IsNull(timex.Month);
            Assert.IsNull(timex.DayOfMonth);
            Assert.IsNull(timex.DayOfWeek);
            Assert.IsNull(timex.WeekOfYear);
            Assert.IsNull(timex.WeekOfMonth);
            Assert.IsNull(timex.Season);
            Assert.IsNull(timex.Hour);
            Assert.IsNull(timex.Minute);
            Assert.IsNull(timex.Second);
            Assert.IsNull(timex.Weekend);
            Assert.AreEqual("MO", timex.PartOfDay);
            Assert.IsNull(timex.Years);
            Assert.IsNull(timex.Months);
            Assert.IsNull(timex.Weeks);
            Assert.IsNull(timex.Days);
            Assert.IsNull(timex.Hours);
            Assert.IsNull(timex.Minutes);
            Assert.IsNull(timex.Seconds);
            Assert.IsNull(timex.Now);
        }

        [TestMethod]
        public void DataTypes_Parsing_Afternoon()
        {
            var timex = new TimexProperty("TAF");
            CollectionAssert.AreEquivalent(new[] { Constants.TimexTypes.TimeRange }, timex.Types.ToList());

            Assert.IsNull(timex.Year);
            Assert.IsNull(timex.Month);
            Assert.IsNull(timex.DayOfMonth);
            Assert.IsNull(timex.DayOfWeek);
            Assert.IsNull(timex.WeekOfYear);
            Assert.IsNull(timex.WeekOfMonth);
            Assert.IsNull(timex.Season);
            Assert.IsNull(timex.Hour);
            Assert.IsNull(timex.Minute);
            Assert.IsNull(timex.Second);
            Assert.IsNull(timex.Weekend);
            Assert.AreEqual("AF", timex.PartOfDay);
            Assert.IsNull(timex.Years);
            Assert.IsNull(timex.Months);
            Assert.IsNull(timex.Weeks);
            Assert.IsNull(timex.Days);
            Assert.IsNull(timex.Hours);
            Assert.IsNull(timex.Minutes);
            Assert.IsNull(timex.Seconds);
            Assert.IsNull(timex.Now);
        }

        [TestMethod]
        public void DataTypes_Parsing_Evening()
        {
            var timex = new TimexProperty("TEV");
            CollectionAssert.AreEquivalent(new[] { Constants.TimexTypes.TimeRange }, timex.Types.ToList());

            Assert.IsNull(timex.Year);
            Assert.IsNull(timex.Month);
            Assert.IsNull(timex.DayOfMonth);
            Assert.IsNull(timex.DayOfWeek);
            Assert.IsNull(timex.WeekOfYear);
            Assert.IsNull(timex.WeekOfMonth);
            Assert.IsNull(timex.Season);
            Assert.IsNull(timex.Hour);
            Assert.IsNull(timex.Minute);
            Assert.IsNull(timex.Second);
            Assert.IsNull(timex.Weekend);
            Assert.AreEqual("EV", timex.PartOfDay);
            Assert.IsNull(timex.Years);
            Assert.IsNull(timex.Months);
            Assert.IsNull(timex.Weeks);
            Assert.IsNull(timex.Days);
            Assert.IsNull(timex.Hours);
            Assert.IsNull(timex.Minutes);
            Assert.IsNull(timex.Seconds);
            Assert.IsNull(timex.Now);
        }

        [TestMethod]
        public void DataTypes_Parsing_Timerange430pmTo445pm()
        {
            var timex = new TimexProperty("(T16:30,T16:45,PT15M)");
            CollectionAssert.AreEquivalent(
                new[]
                {
                Constants.TimexTypes.Time,
                Constants.TimexTypes.Duration,
                Constants.TimexTypes.TimeRange,
                }, timex.Types.ToList());

            Assert.IsNull(timex.Year);
            Assert.IsNull(timex.Month);
            Assert.IsNull(timex.DayOfMonth);
            Assert.IsNull(timex.DayOfWeek);
            Assert.IsNull(timex.WeekOfYear);
            Assert.IsNull(timex.WeekOfMonth);
            Assert.IsNull(timex.Season);
            Assert.AreEqual(16, timex.Hour);
            Assert.AreEqual(30, timex.Minute);
            Assert.AreEqual(0, timex.Second);
            Assert.IsNull(timex.Weekend);
            Assert.IsNull(timex.PartOfDay);
            Assert.IsNull(timex.Years);
            Assert.IsNull(timex.Months);
            Assert.IsNull(timex.Weeks);
            Assert.IsNull(timex.Days);
            Assert.IsNull(timex.Hours);
            Assert.AreEqual(15, timex.Minutes);
            Assert.IsNull(timex.Seconds);
            Assert.IsNull(timex.Now);
        }

        [TestMethod]
        public void DataTypes_Parsing_DateTimeRange()
        {
            var timex = new TimexProperty("XXXX-WXX-5TEV");
            CollectionAssert.AreEquivalent(
                new[]
                {
                Constants.TimexTypes.Date,
                Constants.TimexTypes.TimeRange,
                Constants.TimexTypes.DateTimeRange,
                }, timex.Types.ToList());

            Assert.IsNull(timex.Year);
            Assert.IsNull(timex.Month);
            Assert.IsNull(timex.DayOfMonth);
            Assert.AreEqual(5, timex.DayOfWeek);
            Assert.IsNull(timex.WeekOfYear);
            Assert.IsNull(timex.WeekOfMonth);
            Assert.IsNull(timex.Season);
            Assert.IsNull(timex.Hour);
            Assert.IsNull(timex.Minute);
            Assert.IsNull(timex.Second);
            Assert.IsNull(timex.Weekend);
            Assert.AreEqual("EV", timex.PartOfDay);
            Assert.IsNull(timex.Years);
            Assert.IsNull(timex.Months);
            Assert.IsNull(timex.Weeks);
            Assert.IsNull(timex.Days);
            Assert.IsNull(timex.Hours);
            Assert.IsNull(timex.Minutes);
            Assert.IsNull(timex.Seconds);
            Assert.IsNull(timex.Now);
        }

        [TestMethod]
        public void DataTypes_Parsing_LastNight()
        {
            var timex = new TimexProperty("2017-09-07TNI");
            CollectionAssert.AreEquivalent(
                new[]
                {
                Constants.TimexTypes.Definite,
                Constants.TimexTypes.Date,
                Constants.TimexTypes.TimeRange,
                Constants.TimexTypes.DateTimeRange,
                }, timex.Types.ToList());

            Assert.AreEqual(2017, timex.Year);
            Assert.AreEqual(9, timex.Month);
            Assert.AreEqual(7, timex.DayOfMonth);
            Assert.IsNull(timex.DayOfWeek);
            Assert.IsNull(timex.WeekOfYear);
            Assert.IsNull(timex.WeekOfMonth);
            Assert.IsNull(timex.Season);
            Assert.IsNull(timex.Hour);
            Assert.IsNull(timex.Minute);
            Assert.IsNull(timex.Second);
            Assert.IsNull(timex.Weekend);
            Assert.AreEqual("NI", timex.PartOfDay);
            Assert.IsNull(timex.Years);
            Assert.IsNull(timex.Months);
            Assert.IsNull(timex.Weeks);
            Assert.IsNull(timex.Days);
            Assert.IsNull(timex.Hours);
            Assert.IsNull(timex.Minutes);
            Assert.IsNull(timex.Seconds);
            Assert.IsNull(timex.Now);
        }

        [TestMethod]
        public void DataTypes_Parsing_Last5Minutes()
        {
            var timex = new TimexProperty("(2017-09-08T21:19:29,2017-09-08T21:24:29,PT5M)");
            CollectionAssert.AreEquivalent(
                new[]
                {
                Constants.TimexTypes.Date,
                Constants.TimexTypes.TimeRange,
                Constants.TimexTypes.DateTimeRange,
                Constants.TimexTypes.Time,
                Constants.TimexTypes.DateTime,
                Constants.TimexTypes.Duration,
                Constants.TimexTypes.DateRange,
                Constants.TimexTypes.Definite,
            }, timex.Types.ToList());

            Assert.AreEqual(2017, timex.Year);
            Assert.AreEqual(9, timex.Month);
            Assert.AreEqual(8, timex.DayOfMonth);
            Assert.IsNull(timex.DayOfWeek);
            Assert.IsNull(timex.WeekOfYear);
            Assert.IsNull(timex.WeekOfMonth);
            Assert.IsNull(timex.Season);
            Assert.AreEqual(21, timex.Hour);
            Assert.AreEqual(19, timex.Minute);
            Assert.AreEqual(29, timex.Second);
            Assert.IsNull(timex.Weekend);
            Assert.IsNull(timex.PartOfDay);
            Assert.IsNull(timex.Years);
            Assert.IsNull(timex.Months);
            Assert.IsNull(timex.Weeks);
            Assert.IsNull(timex.Days);
            Assert.IsNull(timex.Hours);
            Assert.AreEqual(5, timex.Minutes);
            Assert.IsNull(timex.Seconds);
            Assert.IsNull(timex.Now);
        }

        [TestMethod]
        public void DataTypes_Parsing_Wed4PMToSat3PM()
        {
            var timex = new TimexProperty("(XXXX-WXX-3T16,XXXX-WXX-6T15,PT71H)");
            CollectionAssert.AreEquivalent(
                new[]
                {
                Constants.TimexTypes.Date,
                Constants.TimexTypes.TimeRange,
                Constants.TimexTypes.DateTimeRange,
                Constants.TimexTypes.Time,
                Constants.TimexTypes.DateTime,
                Constants.TimexTypes.Duration,
                Constants.TimexTypes.DateRange,
            }, timex.Types.ToList());

            Assert.IsNull(timex.Year);
            Assert.IsNull(timex.Month);
            Assert.IsNull(timex.DayOfMonth);
            Assert.AreEqual(3, timex.DayOfWeek);
            Assert.IsNull(timex.WeekOfYear);
            Assert.IsNull(timex.WeekOfMonth);
            Assert.IsNull(timex.Season);
            Assert.AreEqual(16, timex.Hour);
            Assert.AreEqual(0, timex.Minute);
            Assert.AreEqual(0, timex.Second);
            Assert.IsNull(timex.Weekend);
            Assert.IsNull(timex.PartOfDay);
            Assert.IsNull(timex.Years);
            Assert.IsNull(timex.Months);
            Assert.IsNull(timex.Weeks);
            Assert.IsNull(timex.Days);
            Assert.AreEqual(71, timex.Hours);
            Assert.IsNull(timex.Minutes);
            Assert.IsNull(timex.Seconds);
            Assert.IsNull(timex.Now);
        }

        [TestMethod]
        public void DataTypes_Parsing_DurationYears()
        {
            var timex = new TimexProperty("P2Y");
            CollectionAssert.AreEquivalent(new[] { Constants.TimexTypes.Duration }, timex.Types.ToList());

            Assert.IsNull(timex.Year);
            Assert.IsNull(timex.Month);
            Assert.IsNull(timex.DayOfMonth);
            Assert.IsNull(timex.DayOfWeek);
            Assert.IsNull(timex.WeekOfYear);
            Assert.IsNull(timex.WeekOfMonth);
            Assert.IsNull(timex.Season);
            Assert.IsNull(timex.Hour);
            Assert.IsNull(timex.Minute);
            Assert.IsNull(timex.Second);
            Assert.IsNull(timex.Weekend);
            Assert.IsNull(timex.PartOfDay);
            Assert.AreEqual(2, timex.Years);
            Assert.IsNull(timex.Months);
            Assert.IsNull(timex.Weeks);
            Assert.IsNull(timex.Days);
            Assert.IsNull(timex.Hours);
            Assert.IsNull(timex.Minutes);
            Assert.IsNull(timex.Seconds);
            Assert.IsNull(timex.Now);
        }

        [TestMethod]
        public void DataTypes_Parsing_DurationMonths()
        {
            var timex = new TimexProperty("P4M");
            CollectionAssert.AreEquivalent(new[] { Constants.TimexTypes.Duration }, timex.Types.ToList());

            Assert.IsNull(timex.Year);
            Assert.IsNull(timex.Month);
            Assert.IsNull(timex.DayOfMonth);
            Assert.IsNull(timex.DayOfWeek);
            Assert.IsNull(timex.WeekOfYear);
            Assert.IsNull(timex.WeekOfMonth);
            Assert.IsNull(timex.Season);
            Assert.IsNull(timex.Hour);
            Assert.IsNull(timex.Minute);
            Assert.IsNull(timex.Second);
            Assert.IsNull(timex.Weekend);
            Assert.IsNull(timex.PartOfDay);
            Assert.IsNull(timex.Years);
            Assert.AreEqual(4, timex.Months);
            Assert.IsNull(timex.Weeks);
            Assert.IsNull(timex.Days);
            Assert.IsNull(timex.Hours);
            Assert.IsNull(timex.Minutes);
            Assert.IsNull(timex.Seconds);
            Assert.IsNull(timex.Now);
        }

        [TestMethod]
        public void DataTypes_Parsing_DurationWeeks()
        {
            var timex = new TimexProperty("P6W");
            CollectionAssert.AreEquivalent(new[] { Constants.TimexTypes.Duration }, timex.Types.ToList());

            Assert.IsNull(timex.Year);
            Assert.IsNull(timex.Month);
            Assert.IsNull(timex.DayOfMonth);
            Assert.IsNull(timex.DayOfWeek);
            Assert.IsNull(timex.WeekOfYear);
            Assert.IsNull(timex.WeekOfMonth);
            Assert.IsNull(timex.Season);
            Assert.IsNull(timex.Hour);
            Assert.IsNull(timex.Minute);
            Assert.IsNull(timex.Second);
            Assert.IsNull(timex.Weekend);
            Assert.IsNull(timex.PartOfDay);
            Assert.IsNull(timex.Years);
            Assert.IsNull(timex.Months);
            Assert.AreEqual(6, timex.Weeks);
            Assert.IsNull(timex.Days);
            Assert.IsNull(timex.Hours);
            Assert.IsNull(timex.Minutes);
            Assert.IsNull(timex.Seconds);
            Assert.IsNull(timex.Now);
        }

        [TestMethod]
        public void DataTypes_Parsing_DurationWeeksFloatingPoint()
        {
            var timex = new TimexProperty("P2.5W");
            CollectionAssert.AreEquivalent(new[] { Constants.TimexTypes.Duration }, timex.Types.ToList());

            Assert.IsNull(timex.Year);
            Assert.IsNull(timex.Month);
            Assert.IsNull(timex.DayOfMonth);
            Assert.IsNull(timex.DayOfWeek);
            Assert.IsNull(timex.WeekOfYear);
            Assert.IsNull(timex.WeekOfMonth);
            Assert.IsNull(timex.Season);
            Assert.IsNull(timex.Hour);
            Assert.IsNull(timex.Minute);
            Assert.IsNull(timex.Second);
            Assert.IsNull(timex.Weekend);
            Assert.IsNull(timex.PartOfDay);
            Assert.IsNull(timex.Years);
            Assert.IsNull(timex.Months);
            Assert.AreEqual(2.5m, timex.Weeks);
            Assert.IsNull(timex.Days);
            Assert.IsNull(timex.Hours);
            Assert.IsNull(timex.Minutes);
            Assert.IsNull(timex.Seconds);
            Assert.IsNull(timex.Now);
        }

        [TestMethod]
        public void DataTypes_Parsing_DurationDays()
        {
            var timex = new TimexProperty("P1D");
            CollectionAssert.AreEquivalent(new[] { Constants.TimexTypes.Duration }, timex.Types.ToList());

            Assert.IsNull(timex.Year);
            Assert.IsNull(timex.Month);
            Assert.IsNull(timex.DayOfMonth);
            Assert.IsNull(timex.DayOfWeek);
            Assert.IsNull(timex.WeekOfYear);
            Assert.IsNull(timex.WeekOfMonth);
            Assert.IsNull(timex.Season);
            Assert.IsNull(timex.Hour);
            Assert.IsNull(timex.Minute);
            Assert.IsNull(timex.Second);
            Assert.IsNull(timex.Weekend);
            Assert.IsNull(timex.PartOfDay);
            Assert.IsNull(timex.Years);
            Assert.IsNull(timex.Months);
            Assert.IsNull(timex.Weeks);
            Assert.AreEqual(1, timex.Days);
            Assert.IsNull(timex.Hours);
            Assert.IsNull(timex.Minutes);
            Assert.IsNull(timex.Seconds);
            Assert.IsNull(timex.Now);
        }

        [TestMethod]
        public void DataTypes_Parsing_DurationHours()
        {
            var timex = new TimexProperty("PT5H");
            CollectionAssert.AreEquivalent(new[] { Constants.TimexTypes.Duration }, timex.Types.ToList());

            Assert.IsNull(timex.Year);
            Assert.IsNull(timex.Month);
            Assert.IsNull(timex.DayOfMonth);
            Assert.IsNull(timex.DayOfWeek);
            Assert.IsNull(timex.WeekOfYear);
            Assert.IsNull(timex.WeekOfMonth);
            Assert.IsNull(timex.Season);
            Assert.IsNull(timex.Hour);
            Assert.IsNull(timex.Minute);
            Assert.IsNull(timex.Second);
            Assert.IsNull(timex.Weekend);
            Assert.IsNull(timex.PartOfDay);
            Assert.IsNull(timex.Years);
            Assert.IsNull(timex.Months);
            Assert.IsNull(timex.Weeks);
            Assert.IsNull(timex.Days);
            Assert.AreEqual(5, timex.Hours);
            Assert.IsNull(timex.Minutes);
            Assert.IsNull(timex.Seconds);
            Assert.IsNull(timex.Now);
        }

        [TestMethod]
        public void DataTypes_Parsing_DurationMinutes()
        {
            var timex = new TimexProperty("PT30M");
            CollectionAssert.AreEquivalent(new[] { Constants.TimexTypes.Duration }, timex.Types.ToList());

            Assert.IsNull(timex.Year);
            Assert.IsNull(timex.Month);
            Assert.IsNull(timex.DayOfMonth);
            Assert.IsNull(timex.DayOfWeek);
            Assert.IsNull(timex.WeekOfYear);
            Assert.IsNull(timex.WeekOfMonth);
            Assert.IsNull(timex.Season);
            Assert.IsNull(timex.Hour);
            Assert.IsNull(timex.Minute);
            Assert.IsNull(timex.Second);
            Assert.IsNull(timex.Weekend);
            Assert.IsNull(timex.PartOfDay);
            Assert.IsNull(timex.Years);
            Assert.IsNull(timex.Months);
            Assert.IsNull(timex.Weeks);
            Assert.IsNull(timex.Days);
            Assert.IsNull(timex.Hours);
            Assert.AreEqual(30, timex.Minutes);
            Assert.IsNull(timex.Seconds);
            Assert.IsNull(timex.Now);
        }

        [TestMethod]
        public void DataTypes_Parsing_DurationSeconds()
        {
            var timex = new TimexProperty("PT45S");
            CollectionAssert.AreEquivalent(new[] { Constants.TimexTypes.Duration }, timex.Types.ToList());

            Assert.IsNull(timex.Year);
            Assert.IsNull(timex.Month);
            Assert.IsNull(timex.DayOfMonth);
            Assert.IsNull(timex.DayOfWeek);
            Assert.IsNull(timex.WeekOfYear);
            Assert.IsNull(timex.WeekOfMonth);
            Assert.IsNull(timex.Season);
            Assert.IsNull(timex.Hour);
            Assert.IsNull(timex.Minute);
            Assert.IsNull(timex.Second);
            Assert.IsNull(timex.Weekend);
            Assert.IsNull(timex.PartOfDay);
            Assert.IsNull(timex.Years);
            Assert.IsNull(timex.Months);
            Assert.IsNull(timex.Weeks);
            Assert.IsNull(timex.Days);
            Assert.IsNull(timex.Hours);
            Assert.IsNull(timex.Minutes);
            Assert.AreEqual(45, timex.Seconds);
            Assert.IsNull(timex.Now);
        }
    }
}
