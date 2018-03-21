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
            Assert.AreEqual(new System.DateTime(2017, 1, 1), TimexDateHelpers.Tomorrow(new System.DateTime(2016, 12, 31)));
            Assert.AreEqual(new System.DateTime(2017, 1, 2), TimexDateHelpers.Tomorrow(new System.DateTime(2017, 1, 1)));
            Assert.AreEqual(new System.DateTime(2017, 3, 1), TimexDateHelpers.Tomorrow(new System.DateTime(2017, 2, 28)));
            Assert.AreEqual(new System.DateTime(2016, 2, 29), TimexDateHelpers.Tomorrow(new System.DateTime(2016, 2, 28)));
        }

        [TestMethod]
        public void DataTypes_DateHelpers_yesterday()
        {
            Assert.AreEqual(new System.DateTime(2016, 12, 31), TimexDateHelpers.Yesterday(new System.DateTime(2017, 1, 1)));
            Assert.AreEqual(new System.DateTime(2017, 1, 1), TimexDateHelpers.Yesterday(new System.DateTime(2017, 1, 2)));
            Assert.AreEqual(new System.DateTime(2017, 2, 28), TimexDateHelpers.Yesterday(new System.DateTime(2017, 3, 1)));
            Assert.AreEqual(new System.DateTime(2016, 2, 28), TimexDateHelpers.Yesterday(new System.DateTime(2016, 2, 29)));
        }

        [TestMethod]
        public void DataTypes_DateHelpers_datePartEquals()
        {
            Assert.IsTrue(TimexDateHelpers.DatePartEquals(new System.DateTime(2017, 5, 29), new System.DateTime(2017, 5, 29)));
            Assert.IsTrue(TimexDateHelpers.DatePartEquals(new System.DateTime(2017, 5, 29, 19, 30, 0), new System.DateTime(2017, 5, 29)));
            Assert.IsFalse(TimexDateHelpers.DatePartEquals(new System.DateTime(2017, 5, 29), new System.DateTime(2017, 11, 15)));
        }

        [TestMethod]
        public void DataTypes_DateHelpers_isNextWeek()
        {
            var today = new System.DateTime(2017, 9, 25);
            Assert.IsTrue(TimexDateHelpers.IsNextWeek(new System.DateTime(2017, 10, 4), today));
            Assert.IsFalse(TimexDateHelpers.IsNextWeek(new System.DateTime(2017, 9, 27), today));
            Assert.IsFalse(TimexDateHelpers.IsNextWeek(today, today));
        }

        [TestMethod]
        public void DataTypes_DateHelpers_isLastWeek()
        {
            var today = new System.DateTime(2017, 9, 25);
            Assert.IsTrue(TimexDateHelpers.IsLastWeek(new System.DateTime(2017, 9, 20), today));
            Assert.IsFalse(TimexDateHelpers.IsLastWeek(new System.DateTime(2017, 9, 4), today));
            Assert.IsFalse(TimexDateHelpers.IsLastWeek(today, today));
        }

        [TestMethod]
        public void DataTypes_DateHelpers_weekOfyear()
        {
            Assert.AreEqual(1, TimexDateHelpers.WeekOfYear(new System.DateTime(2017, 1, 1)));
            Assert.AreEqual(2, TimexDateHelpers.WeekOfYear(new System.DateTime(2017, 1, 2)));
            Assert.AreEqual(9, TimexDateHelpers.WeekOfYear(new System.DateTime(2017, 2, 23)));
            Assert.AreEqual(12, TimexDateHelpers.WeekOfYear(new System.DateTime(2017, 3, 15)));
            Assert.AreEqual(40, TimexDateHelpers.WeekOfYear(new System.DateTime(2017, 9, 25)));
            Assert.AreEqual(53, TimexDateHelpers.WeekOfYear(new System.DateTime(2017, 12, 31)));
            Assert.AreEqual(1, TimexDateHelpers.WeekOfYear(new System.DateTime(2018, 1, 1)));
            Assert.AreEqual(1, TimexDateHelpers.WeekOfYear(new System.DateTime(2018, 1, 2)));
            Assert.AreEqual(1, TimexDateHelpers.WeekOfYear(new System.DateTime(2018, 1, 7)));
            Assert.AreEqual(2, TimexDateHelpers.WeekOfYear(new System.DateTime(2018, 1, 8)));
        }

        [TestMethod]
        public void DataTypes_DateHelpers_invariance()
        {
            var d = new System.DateTime(2017, 8, 25);
            var before = d;
            TimexDateHelpers.Tomorrow(d);
            TimexDateHelpers.Yesterday(d);
            TimexDateHelpers.DatePartEquals(new System.DateTime(), d);
            TimexDateHelpers.DatePartEquals(d, new System.DateTime());
            TimexDateHelpers.IsNextWeek(d, new System.DateTime());
            TimexDateHelpers.IsNextWeek(new System.DateTime(), d);
            TimexDateHelpers.IsLastWeek(new System.DateTime(), d);
            TimexDateHelpers.WeekOfYear(d);
            var after = d;
            Assert.AreEqual(after, before);
        }

        [TestMethod]
        public void DataTypes_DateHelpers_dateOfLastDay_Friday_last_week()
        {
            var day = DayOfWeek.Friday;
            var date = new System.DateTime(2017, 9, 28);
            Assert.IsTrue(TimexDateHelpers.DatePartEquals(TimexDateHelpers.DateOfLastDay(day, date), new System.DateTime(2017, 9, 22)));
        }

        [TestMethod]
        public void DataTypes_DateHelpers_dateOfNextDay_Wednesday_next_week()
        {
            var day = DayOfWeek.Wednesday;
            var date = new System.DateTime(2017, 9, 28);
            Assert.IsTrue(TimexDateHelpers.DatePartEquals(TimexDateHelpers.DateOfNextDay(day, date), new System.DateTime(2017, 10, 4)));
        }

        [TestMethod]
        public void DataTypes_DateHelpers_dateOfNextDay_today()
        {
            var day = DayOfWeek.Thursday;
            var date = new System.DateTime(2017, 9, 28);
            Assert.IsFalse(TimexDateHelpers.DatePartEquals(TimexDateHelpers.DateOfNextDay(day, date), date));
        }

        [TestMethod]
        public void DataTypes_DateHelpers_datesMatchingDay()
        {
            var day = DayOfWeek.Thursday;
            var start = new System.DateTime(2017, 3, 1);
            var end = new System.DateTime(2017, 4, 1);
            var result = TimexDateHelpers.DatesMatchingDay(day, start, end);
            Assert.AreEqual(5, result.Count);
            Assert.IsTrue(TimexDateHelpers.DatePartEquals(result[0], new System.DateTime(2017, 3, 2)));
            Assert.IsTrue(TimexDateHelpers.DatePartEquals(result[1], new System.DateTime(2017, 3, 9)));
            Assert.IsTrue(TimexDateHelpers.DatePartEquals(result[2], new System.DateTime(2017, 3, 16)));
            Assert.IsTrue(TimexDateHelpers.DatePartEquals(result[3], new System.DateTime(2017, 3, 23)));
            Assert.IsTrue(TimexDateHelpers.DatePartEquals(result[4], new System.DateTime(2017, 3, 30)));
        }
    }
}
