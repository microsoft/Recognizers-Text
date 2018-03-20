// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Recognizers.Text.DataTypes.TimexExpression.Tests
{
    [TestClass]
    public class TestTimexFormat
    {
        [TestMethod]
        public void DataTypes_Format_Date()
        {
            Assert.AreEqual("2017-09-27", (new TimexProperties { Year = 2017, Month = 9, DayOfMonth = 27 }).TimexValue);
            Assert.AreEqual("XXXX-WXX-3", (new TimexProperties { DayOfWeek = 3 }).TimexValue);
            Assert.AreEqual("XXXX-12-05", (new TimexProperties { Month = 12, DayOfMonth = 5 }).TimexValue);
        }

        [TestMethod]
        public void DataTypes_Format_Time()
        {
            Assert.AreEqual("T17:30:45", (new TimexProperties { Hour = 17, Minute = 30, Second = 45 }).TimexValue);
            Assert.AreEqual("T05:06:07", (new TimexProperties { Hour = 5, Minute = 6, Second = 7 }).TimexValue);
            Assert.AreEqual("T17:30", (new TimexProperties { Hour = 17, Minute = 30, Second = 0 }).TimexValue);
            Assert.AreEqual("T23", (new TimexProperties { Hour = 23, Minute = 0, Second = 0 }).TimexValue);
        }

        [TestMethod]
        public void DataTypes_Format_Duration()
        {
            Assert.AreEqual("P50Y", (new TimexProperties { Years = 50 }).TimexValue);
            Assert.AreEqual("P6M", (new TimexProperties { Months = 6 }).TimexValue);
            Assert.AreEqual("P3W", (new TimexProperties { Weeks = 3 }).TimexValue);
            Assert.AreEqual("P5D", (new TimexProperties { Days = 5 }).TimexValue);
            Assert.AreEqual("PT16H", (new TimexProperties { Hours = 16 }).TimexValue);
            Assert.AreEqual("PT32M", (new TimexProperties { Minutes = 32 }).TimexValue);
            Assert.AreEqual("PT20S", (new TimexProperties { Seconds = 20 }).TimexValue);
        }

        [TestMethod]
        public void DataTypes_Format_Present()
        {
            Assert.AreEqual("PRESENT_REF", (new TimexProperties { Now = true }).TimexValue);
        }

        [TestMethod]
        public void DataTypes_Format_DateTime()
        {
            Assert.AreEqual("XXXX-WXX-3T04", (new TimexProperties { DayOfWeek = 3, Hour = 4, Minute = 0, Second = 0 }).TimexValue);
            Assert.AreEqual("2017-09-27T11:41:30", (new TimexProperties { Year = 2017, Month = 9, DayOfMonth = 27, Hour = 11, Minute = 41, Second = 30 }).TimexValue);
        }

        [TestMethod]
        public void DataTypes_Format_DateRange()
        {
            Assert.AreEqual("2017", (new TimexProperties { Year = 2017 }).TimexValue);
            Assert.AreEqual("SU", (new TimexProperties { Season = "SU" }).TimexValue);
            Assert.AreEqual("2017-WI", (new TimexProperties { Year = 2017, Season = "WI" }).TimexValue);
            Assert.AreEqual("2017-09", (new TimexProperties { Year = 2017, Month = 9 }).TimexValue);
            Assert.AreEqual("2017-W37", (new TimexProperties { Year = 2017, WeekOfYear = 37 }).TimexValue);
            Assert.AreEqual("2017-W37-WE", (new TimexProperties { Year = 2017, WeekOfYear = 37, Weekend = true }).TimexValue);
            Assert.AreEqual("XXXX-05", (new TimexProperties { Month = 5 }).TimexValue);
        }

        [TestMethod]
        public void DataTypes_Format_TimeRange()
        {
            Assert.AreEqual("TEV", (new TimexProperties { PartOfDay = "EV" }).TimexValue);
        }

        [TestMethod]
        public void DataTypes_Format_DateTimeRange()
        {
            Assert.AreEqual("2017-09-27TEV", (new TimexProperties { Year = 2017, Month = 9, DayOfMonth = 27, PartOfDay = "EV" }).TimexValue);
        }
    }
}
