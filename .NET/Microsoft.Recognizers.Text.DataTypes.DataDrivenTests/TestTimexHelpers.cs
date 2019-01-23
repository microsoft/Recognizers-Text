// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Recognizers.Text.DataTypes.TimexExpression.Tests
{
    [TestClass]
    public class TestTimexHelpers
    {
        [TestMethod]
        public void DataTypes_Helpers_ExpandDateTimeRange_Short()
        {
            var timex = new TimexProperty("(2017-09-27,2017-09-29,P2D)");
            var range = TimexHelpers.ExpandDateTimeRange(timex);
            Assert.AreEqual("2017-09-27", range.Start.TimexValue);
            Assert.AreEqual("2017-09-29", range.End.TimexValue);
        }

        [TestMethod]
        public void DataTypes_Helpers_ExpandDateTimeRange_Long()
        {
            var timex = new TimexProperty("(2006-01-01,2008-06-01,P882D)");
            var range = TimexHelpers.ExpandDateTimeRange(timex);
            Assert.AreEqual("2006-01-01", range.Start.TimexValue);
            Assert.AreEqual("2008-06-01", range.End.TimexValue);
        }

        [TestMethod]
        public void DataTypes_Helpers_ExpandDateTimeRange_Include_Time()
        {
            var timex = new TimexProperty("(2017-10-10T16:02:04,2017-10-10T16:07:04,PT5M)");
            var range = TimexHelpers.ExpandDateTimeRange(timex);
            Assert.AreEqual("2017-10-10T16:02:04", range.Start.TimexValue);
            Assert.AreEqual("2017-10-10T16:07:04", range.End.TimexValue);
        }

        [TestMethod]
        public void DataTypes_Helpers_ExpandDateTimeRange_Month()
        {
            var timex = new TimexProperty("2017-05");
            var range = TimexHelpers.ExpandDateTimeRange(timex);
            Assert.AreEqual("2017-05-01", range.Start.TimexValue);
            Assert.AreEqual("2017-06-01", range.End.TimexValue);
        }

        [TestMethod]
        public void DataTypes_Helpers_ExpandDateTimeRange_Year()
        {
            var timex = new TimexProperty("1999");
            var range = TimexHelpers.ExpandDateTimeRange(timex);
            Assert.AreEqual("1999-01-01", range.Start.TimexValue);
            Assert.AreEqual("2000-01-01", range.End.TimexValue);
        }

        [TestMethod]
        public void DataTypes_Helpers_ExpandTimeRange()
        {
            var timex = new TimexProperty("(T14,T16,PT2H)");
            var range = TimexHelpers.ExpandTimeRange(timex);
            Assert.AreEqual("T14", range.Start.TimexValue);
            Assert.AreEqual("T16", range.End.TimexValue);
        }

        [TestMethod]
        public void DataTypes_Helpers_DateRangeFromTimex()
        {
            var timex = new TimexProperty("(2017-09-27,2017-09-29,P2D)");
            var range = TimexHelpers.DateRangeFromTimex(timex);
            Assert.AreEqual(new System.DateTime(2017, 9, 27), range.Start);
            Assert.AreEqual(new System.DateTime(2017, 9, 29), range.End);
        }

        [TestMethod]
        public void DataTypes_Helpers_TimeRangeFromTimex()
        {
            var timex = new TimexProperty("(T14,T16,PT2H)");
            var range = TimexHelpers.TimeRangeFromTimex(timex);
            Assert.AreEqual(new Time(14, 0, 0).GetTime(), range.Start.GetTime());
            Assert.AreEqual(new Time(16, 0, 0).GetTime(), range.End.GetTime());
        }

        [TestMethod]
        public void DataTypes_Helpers_DateFromTimex()
        {
            var timex = new TimexProperty("2017-09-27");
            var date = TimexHelpers.DateFromTimex(timex);
            Assert.AreEqual(new System.DateTime(2017, 9, 27), date);
        }

        [TestMethod]
        public void DataTypes_Helpers_TimeFromTimex()
        {
            var timex = new TimexProperty("T00:05:00");
            var time = TimexHelpers.TimeFromTimex(timex);
            Assert.AreEqual(new Time(0, 5, 0).GetTime(), time.GetTime());
        }
    }
}
