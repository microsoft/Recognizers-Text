// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Recognizers.Text.DataTypes.TimexExpression.Tests
{
    [TestClass]
    public class TestTimexDateHelpers
    {
        [TestMethod]
        public void DataTypes_DateHelpers_tomorrow()
        {
            Assert.AreEqual(new DateTime(2017, 1, 1), new DateTime(2016, 12, 31).Tomorrow());
            Assert.AreEqual(new DateTime(2017, 1, 2), new DateTime(2017, 1, 1).Tomorrow());
            Assert.AreEqual(new DateTime(2017, 3, 1), new DateTime(2017, 2, 28).Tomorrow());
            Assert.AreEqual(new DateTime(2016, 2, 29), new DateTime(2016, 2, 28).Tomorrow());
        }

        [TestMethod]
        public void DataTypes_DateHelpers_yesterday()
        {
            Assert.AreEqual(new DateTime(2016, 12, 31), new DateTime(2017, 1, 1).Yesterday());
            Assert.AreEqual(new DateTime(2017, 1, 1), new DateTime(2017, 1, 2).Yesterday());
            Assert.AreEqual(new DateTime(2017, 2, 28), new DateTime(2017, 3, 1).Yesterday());
            Assert.AreEqual(new DateTime(2016, 2, 28), new DateTime(2016, 2, 29).Yesterday());
        }

        [TestMethod]
        public void DataTypes_DateHelpers_datePartEquals()
        {
            Assert.IsTrue(new DateTime(2017, 5, 29).IsSameDate(new DateTime(2017, 5, 29)));
            Assert.IsTrue(new DateTime(2017, 5, 29, 19, 30, 0).IsSameDate(new DateTime(2017, 5, 29)));
            Assert.IsFalse(new DateTime(2017, 5, 29).IsSameDate(new DateTime(2017, 11, 15)));
        }

        [TestMethod]
        public void DataTypes_DateHelpers_isNextWeek()
        {
            var today = new DateTime(2017, 9, 25);
            Assert.IsTrue(new DateTime(2017, 10, 4).IsNextWeek(today));
            Assert.IsFalse(new DateTime(2017, 9, 27).IsNextWeek(today));
            Assert.IsFalse(today.IsNextWeek(today));
        }

        [TestMethod]
        public void DataTypes_DateHelpers_isLastWeek()
        {
            var today = new DateTime(2017, 9, 25);
            Assert.IsTrue(new DateTime(2017, 9, 20).IsLastWeek(today));
            Assert.IsFalse(new DateTime(2017, 9, 4).IsLastWeek(today));
            Assert.IsFalse(today.IsLastWeek(today));
        }

        [TestMethod]
        public void DataTypes_DateHelpers_weekOfyear()
        {
            Assert.AreEqual(52, TimexDateHelpers.WeekOfYear(new DateTime(2017, 1, 1)));
            Assert.AreEqual(1, TimexDateHelpers.WeekOfYear(new DateTime(2017, 1, 2)));
            Assert.AreEqual(8, TimexDateHelpers.WeekOfYear(new DateTime(2017, 2, 23)));
            Assert.AreEqual(11, TimexDateHelpers.WeekOfYear(new DateTime(2017, 3, 15)));
            Assert.AreEqual(39, TimexDateHelpers.WeekOfYear(new DateTime(2017, 9, 25)));
            Assert.AreEqual(52, TimexDateHelpers.WeekOfYear(new DateTime(2017, 12, 31)));
            Assert.AreEqual(1, TimexDateHelpers.WeekOfYear(new DateTime(2018, 1, 1)));
            Assert.AreEqual(1, TimexDateHelpers.WeekOfYear(new DateTime(2018, 1, 2)));
            Assert.AreEqual(1, TimexDateHelpers.WeekOfYear(new DateTime(2018, 1, 7)));
            Assert.AreEqual(2, TimexDateHelpers.WeekOfYear(new DateTime(2018, 1, 8)));
        }

        [TestMethod]
        public void DataTypes_DateHelpers_invariance()
        {
            var d = new DateTime(2017, 8, 25);
            var before = d;
            d.Tomorrow();
            d.Yesterday();
            new DateTime().IsSameDate(d);
            d.IsSameDate(new DateTime());
            d.IsNextWeek(new DateTime());
            new DateTime().IsNextWeek(d);
            new DateTime().IsLastWeek(d);
            TimexDateHelpers.WeekOfYear(d);
            var after = d;
            Assert.AreEqual(after, before);
        }

        [TestMethod]
        public void DataTypes_DateHelpers_dateOfLastDay_Friday_last_week()
        {
            var day = DayOfWeek.Friday;
            var date = new DateTime(2017, 9, 28);
            Assert.AreEqual(new DateTime(2017, 9, 22), date.DateOfLastDay(day));
        }

        [TestMethod]
        public void DataTypes_DateHelpers_dateOfLastDay_Wednesday_same_week()
        {
            var day = DayOfWeek.Wednesday;
            var date = new DateTime(2022, 3, 17);
            Assert.AreEqual(date.AddDays(-1), date.DateOfLastDay(day));
        }

        [TestMethod]
        public void DataTypes_DateHelpers_dateOfNextDay_Wednesday_next_week()
        {
            var day = DayOfWeek.Wednesday;
            var date = new DateTime(2017, 9, 28);
            Assert.IsTrue(TimexDateHelpers.DateOfNextDay(date, day).IsSameDate(new DateTime(2017, 10, 4)));
        }

        [TestMethod]
        public void DataTypes_DateHelpers_dateOfNextDay_today()
        {
            var day = DayOfWeek.Thursday;
            var date = new DateTime(2017, 9, 28);
            Assert.IsFalse(TimexDateHelpers.DateOfNextDay(date, day).IsSameDate(date));
        }

        [TestMethod]
        public void DataTypes_DateHelpers_datesMatchingDay()
        {
            var day = DayOfWeek.Thursday;
            var start = new DateTime(2017, 3, 1);
            var end = new DateTime(2017, 4, 1);
            var result = TimexDateHelpers.DatesMatchingDay(day, start, end);
            Assert.AreEqual(5, result.Count);
            Assert.IsTrue(result[0].IsSameDate(new DateTime(2017, 3, 2)));
            Assert.IsTrue(result[1].IsSameDate(new DateTime(2017, 3, 9)));
            Assert.IsTrue(result[2].IsSameDate(new DateTime(2017, 3, 16)));
            Assert.IsTrue(result[3].IsSameDate(new DateTime(2017, 3, 23)));
            Assert.IsTrue(result[4].IsSameDate(new DateTime(2017, 3, 30)));
        }
    }
}
