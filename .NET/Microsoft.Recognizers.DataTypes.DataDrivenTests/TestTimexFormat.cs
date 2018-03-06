// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Recognizers.DataTypes.DateTime.Tests
{
    [TestClass]
    public class TestTimexFormat
    {
        [TestMethod]
        public void Format_Date()
        {
            Assert.AreEqual("2017-09-27", (new Timex { Year = 2017, Month = 9, DayOfMonth = 27 }).TimexValue);
            Assert.AreEqual("XXXX-WXX-3", (new Timex { DayOfWeek = 3 }).TimexValue);
            Assert.AreEqual("XXXX-12-05", (new Timex { Month = 12, DayOfMonth = 5 }).TimexValue);
        }

        [TestMethod]
        public void Format_Time()
        {
            Assert.AreEqual("T17:30:45", (new Timex { Hour = 17, Minute = 30, Second = 45 }).TimexValue);
            Assert.AreEqual("T05:06:07", (new Timex { Hour = 5, Minute = 6, Second = 7 }).TimexValue);
            Assert.AreEqual("T17:30", (new Timex { Hour = 17, Minute = 30, Second = 0 }).TimexValue);
            Assert.AreEqual("T23", (new Timex { Hour = 23, Minute = 0, Second = 0 }).TimexValue);
        }

        [TestMethod]
        public void Format_Duration()
        {
            Assert.AreEqual("P50Y", (new Timex { Years = 50 }).TimexValue);
            Assert.AreEqual("P6M", (new Timex { Months = 6 }).TimexValue);
            Assert.AreEqual("P3W", (new Timex { Weeks = 3 }).TimexValue);
            Assert.AreEqual("P5D", (new Timex { Days = 5 }).TimexValue);
            Assert.AreEqual("PT16H", (new Timex { Hours = 16 }).TimexValue);
            Assert.AreEqual("PT32M", (new Timex { Minutes = 32 }).TimexValue);
            Assert.AreEqual("PT20S", (new Timex { Seconds = 20 }).TimexValue);
        }

        [TestMethod]
        public void Format_Present()
        {
            Assert.AreEqual("PRESENT_REF", (new Timex { Now = true }).TimexValue);
        }

        [TestMethod]
        public void Format_DateTime()
        {
            Assert.AreEqual("XXXX-WXX-3T04", (new Timex { DayOfWeek = 3, Hour = 4, Minute = 0, Second = 0 }).TimexValue);
            Assert.AreEqual("2017-09-27T11:41:30", (new Timex { Year = 2017, Month = 9, DayOfMonth = 27, Hour = 11, Minute = 41, Second = 30 }).TimexValue);
        }

        [TestMethod]
        public void Format_DateRange()
        {
            Assert.AreEqual("2017", (new Timex { Year = 2017 }).TimexValue);
            Assert.AreEqual("SU", (new Timex { Season = "SU" }).TimexValue);
            Assert.AreEqual("2017-WI", (new Timex { Year = 2017, Season = "WI" }).TimexValue);
            Assert.AreEqual("2017-09", (new Timex { Year = 2017, Month = 9 }).TimexValue);
            Assert.AreEqual("2017-W37", (new Timex { Year = 2017, WeekOfYear = 37 }).TimexValue);
            Assert.AreEqual("2017-W37-WE", (new Timex { Year = 2017, WeekOfYear = 37, Weekend = true }).TimexValue);
            Assert.AreEqual("XXXX-05", (new Timex { Month = 5 }).TimexValue);
        }

        [TestMethod]
        public void Format_TimeRange()
        {
            Assert.AreEqual("TEV", (new Timex { PartOfDay = "EV" }).TimexValue);
        }

        [TestMethod]
        public void Format_DateTimeRange()
        {
            Assert.AreEqual("2017-09-27TEV", (new Timex { Year = 2017, Month = 9, DayOfMonth = 27, PartOfDay = "EV" }).TimexValue);
        }
    }
}
