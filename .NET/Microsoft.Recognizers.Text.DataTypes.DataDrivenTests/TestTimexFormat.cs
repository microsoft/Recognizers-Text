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
            Assert.AreEqual("2017-09-27", new TimexProperty { Year = 2017, Month = 9, DayOfMonth = 27 }.TimexValue);
            Assert.AreEqual("XXXX-WXX-3", new TimexProperty { DayOfWeek = 3 }.TimexValue);
            Assert.AreEqual("XXXX-12-05", new TimexProperty { Month = 12, DayOfMonth = 5 }.TimexValue);
        }

        [TestMethod]
        public void DataTypes_Format_Time()
        {
            Assert.AreEqual("T17:30:45", new TimexProperty { Hour = 17, Minute = 30, Second = 45 }.TimexValue);
            Assert.AreEqual("T05:06:07", new TimexProperty { Hour = 5, Minute = 6, Second = 7 }.TimexValue);
            Assert.AreEqual("T17:30", new TimexProperty { Hour = 17, Minute = 30, Second = 0 }.TimexValue);
            Assert.AreEqual("T23", new TimexProperty { Hour = 23, Minute = 0, Second = 0 }.TimexValue);
        }

        [TestMethod]
        public void DataTypes_Format_Duration()
        {
            Assert.AreEqual("P50Y", new TimexProperty { Years = 50 }.TimexValue);
            Assert.AreEqual("P6M", new TimexProperty { Months = 6 }.TimexValue);
            Assert.AreEqual("P3W", new TimexProperty { Weeks = 3 }.TimexValue);
            Assert.AreEqual("P5D", new TimexProperty { Days = 5 }.TimexValue);
            Assert.AreEqual("PT16H", new TimexProperty { Hours = 16 }.TimexValue);
            Assert.AreEqual("PT32M", new TimexProperty { Minutes = 32 }.TimexValue);
            Assert.AreEqual("PT20S", new TimexProperty { Seconds = 20 }.TimexValue);
        }

        [TestMethod]
        public void DataTypes_Format_Present()
        {
            Assert.AreEqual("PRESENT_REF", new TimexProperty { Now = true }.TimexValue);
        }

        [TestMethod]
        public void DataTypes_Format_DateTime()
        {
            Assert.AreEqual("XXXX-WXX-3T04", new TimexProperty { DayOfWeek = 3, Hour = 4, Minute = 0, Second = 0 }.TimexValue);
            Assert.AreEqual("2017-09-27T11:41:30", new TimexProperty { Year = 2017, Month = 9, DayOfMonth = 27, Hour = 11, Minute = 41, Second = 30 }.TimexValue);
        }

        [TestMethod]
        public void DataTypes_Format_DateRange()
        {
            Assert.AreEqual("2017", new TimexProperty { Year = 2017 }.TimexValue);
            Assert.AreEqual("SU", new TimexProperty { Season = "SU" }.TimexValue);
            Assert.AreEqual("2017-WI", new TimexProperty { Year = 2017, Season = "WI" }.TimexValue);
            Assert.AreEqual("2017-09", new TimexProperty { Year = 2017, Month = 9 }.TimexValue);
            Assert.AreEqual("2017-W37", new TimexProperty { Year = 2017, WeekOfYear = 37 }.TimexValue);
            Assert.AreEqual("2017-W37-WE", new TimexProperty { Year = 2017, WeekOfYear = 37, Weekend = true }.TimexValue);
            Assert.AreEqual("XXXX-05", new TimexProperty { Month = 5 }.TimexValue);
        }

        [TestMethod]
        public void DataTypes_Format_TimeRange()
        {
            Assert.AreEqual("TEV", new TimexProperty { PartOfDay = "EV" }.TimexValue);
        }

        [TestMethod]
        public void DataTypes_Format_DateTimeRange()
        {
            Assert.AreEqual("2017-09-27TEV", new TimexProperty { Year = 2017, Month = 9, DayOfMonth = 27, PartOfDay = "EV" }.TimexValue);
        }
    }
}
