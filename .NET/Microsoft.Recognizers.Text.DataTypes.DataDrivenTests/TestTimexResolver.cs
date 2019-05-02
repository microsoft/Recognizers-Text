// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Recognizers.Text.DataTypes.TimexExpression.Tests
{
    [TestClass]
    public class TestTimexResolver
    {
        [TestMethod]
        public void DataTypes_Resolver_Date_Definite()
        {
            var today = new System.DateTime(2017, 9, 26, 15, 30, 0);
            var resolution = TimexResolver.Resolve(new[] { "2017-09-28" }, today);
            Assert.AreEqual(1, resolution.Values.Count);

            Assert.AreEqual("2017-09-28", resolution.Values[0].Timex);
            Assert.AreEqual("date", resolution.Values[0].Type);
            Assert.AreEqual("2017-09-28", resolution.Values[0].Value);
            Assert.IsNull(resolution.Values[0].Start);
            Assert.IsNull(resolution.Values[0].End);
        }

        [TestMethod]
        public void DataTypes_Resolver_Date_Saturday()
        {
            var today = new System.DateTime(2017, 9, 26, 15, 30, 0);
            var resolution = TimexResolver.Resolve(new[] { "XXXX-WXX-6" }, today);
            Assert.AreEqual(2, resolution.Values.Count);

            Assert.AreEqual("XXXX-WXX-6", resolution.Values[0].Timex);
            Assert.AreEqual("date", resolution.Values[0].Type);
            Assert.AreEqual("2017-09-23", resolution.Values[0].Value);
            Assert.IsNull(resolution.Values[0].Start);
            Assert.IsNull(resolution.Values[0].End);

            Assert.AreEqual("XXXX-WXX-6", resolution.Values[1].Timex);
            Assert.AreEqual("date", resolution.Values[1].Type);
            Assert.AreEqual("2017-09-30", resolution.Values[1].Value);
            Assert.IsNull(resolution.Values[1].Start);
            Assert.IsNull(resolution.Values[1].End);
        }

        [TestMethod]
        public void DataTypes_Resolver_Date_Sunday()
        {
            var today = new System.DateTime(2019, 4, 23, 15, 30, 0);
            var resolution = TimexResolver.Resolve(new[] { "XXXX-WXX-7" }, today);
            Assert.AreEqual(2, resolution.Values.Count);

            Assert.AreEqual("XXXX-WXX-7", resolution.Values[0].Timex);
            Assert.AreEqual("date", resolution.Values[0].Type);
            Assert.AreEqual("2019-04-21", resolution.Values[0].Value);
            Assert.IsNull(resolution.Values[0].Start);
            Assert.IsNull(resolution.Values[0].End);

            Assert.AreEqual("XXXX-WXX-7", resolution.Values[1].Timex);
            Assert.AreEqual("date", resolution.Values[1].Type);
            Assert.AreEqual("2019-04-28", resolution.Values[1].Value);
            Assert.IsNull(resolution.Values[1].Start);
            Assert.IsNull(resolution.Values[1].End);
        }

        [TestMethod]
        public void DataTypes_Resolver_DateTime_Wednesday_4()
        {
            var today = new System.DateTime(2017, 9, 28, 15, 30, 0);
            var resolution = TimexResolver.Resolve(new[] { "XXXX-WXX-3T04", "XXXX-WXX-3T16" }, today);
            Assert.AreEqual(4, resolution.Values.Count);

            Assert.AreEqual("XXXX-WXX-3T04", resolution.Values[0].Timex);
            Assert.AreEqual("datetime", resolution.Values[0].Type);
            Assert.AreEqual("2017-09-27 04:00:00", resolution.Values[0].Value);
            Assert.IsNull(resolution.Values[0].Start);
            Assert.IsNull(resolution.Values[0].End);

            Assert.AreEqual("XXXX-WXX-3T04", resolution.Values[1].Timex);
            Assert.AreEqual("datetime", resolution.Values[1].Type);
            Assert.AreEqual("2017-10-04 04:00:00", resolution.Values[1].Value);
            Assert.IsNull(resolution.Values[1].Start);
            Assert.IsNull(resolution.Values[1].End);

            Assert.AreEqual("XXXX-WXX-3T16", resolution.Values[2].Timex);
            Assert.AreEqual("datetime", resolution.Values[2].Type);
            Assert.AreEqual("2017-09-27 16:00:00", resolution.Values[2].Value);
            Assert.IsNull(resolution.Values[2].Start);
            Assert.IsNull(resolution.Values[2].End);

            Assert.AreEqual("XXXX-WXX-3T16", resolution.Values[3].Timex);
            Assert.AreEqual("datetime", resolution.Values[3].Type);
            Assert.AreEqual("2017-10-04 16:00:00", resolution.Values[3].Value);
            Assert.IsNull(resolution.Values[3].Start);
            Assert.IsNull(resolution.Values[3].End);
        }

        [TestMethod]
        public void DataTypes_Resolver_DateTime_Wednesday_4_am()
        {
            var today = new System.DateTime(2017, 9, 28, 15, 30, 0);
            var resolution = TimexResolver.Resolve(new[] { "XXXX-WXX-3T04" }, today);
            Assert.AreEqual(2, resolution.Values.Count);

            Assert.AreEqual("XXXX-WXX-3T04", resolution.Values[0].Timex);
            Assert.AreEqual("datetime", resolution.Values[0].Type);
            Assert.AreEqual("2017-09-27 04:00:00", resolution.Values[0].Value);
            Assert.IsNull(resolution.Values[0].Start);
            Assert.IsNull(resolution.Values[0].End);

            Assert.AreEqual("XXXX-WXX-3T04", resolution.Values[1].Timex);
            Assert.AreEqual("datetime", resolution.Values[1].Type);
            Assert.AreEqual("2017-10-04 04:00:00", resolution.Values[1].Value);
            Assert.IsNull(resolution.Values[1].Start);
            Assert.IsNull(resolution.Values[1].End);
        }

        [TestMethod]
        public void DataTypes_Resolver_DateTime_Next_Wednesday_4_am()
        {
            var today = new System.DateTime(2017, 9, 7);
            var resolution = TimexResolver.Resolve(new[] { "2017-10-11T04" }, today);
            Assert.AreEqual(1, resolution.Values.Count);

            Assert.AreEqual("2017-10-11T04", resolution.Values[0].Timex);
            Assert.AreEqual("datetime", resolution.Values[0].Type);
            Assert.AreEqual("2017-10-11 04:00:00", resolution.Values[0].Value);
            Assert.IsNull(resolution.Values[0].Start);
            Assert.IsNull(resolution.Values[0].End);
        }

        [TestMethod]
        public void DataTypes_Resolver_Duration_2years()
        {
            var resolution = TimexResolver.Resolve(new[] { "P2Y" });
            Assert.AreEqual(1, resolution.Values.Count);

            Assert.AreEqual("P2Y", resolution.Values[0].Timex);
            Assert.AreEqual("duration", resolution.Values[0].Type);
            Assert.AreEqual("63072000", resolution.Values[0].Value);
            Assert.IsNull(resolution.Values[0].Start);
            Assert.IsNull(resolution.Values[0].End);
        }

        [TestMethod]
        public void DataTypes_Resolver_Duration_6months()
        {
            var resolution = TimexResolver.Resolve(new[] { "P6M" });
            Assert.AreEqual(1, resolution.Values.Count);

            Assert.AreEqual("P6M", resolution.Values[0].Timex);
            Assert.AreEqual("duration", resolution.Values[0].Type);
            Assert.AreEqual("15552000", resolution.Values[0].Value);
            Assert.IsNull(resolution.Values[0].Start);
            Assert.IsNull(resolution.Values[0].End);
        }

        [TestMethod]
        public void DataTypes_Resolver_Duration_3weeks()
        {
            var resolution = TimexResolver.Resolve(new[] { "P3W" });
            Assert.AreEqual(1, resolution.Values.Count);

            Assert.AreEqual("P3W", resolution.Values[0].Timex);
            Assert.AreEqual("duration", resolution.Values[0].Type);
            Assert.AreEqual("1814400", resolution.Values[0].Value);
            Assert.IsNull(resolution.Values[0].Start);
            Assert.IsNull(resolution.Values[0].End);
        }

        [TestMethod]
        public void DataTypes_Resolver_Duration_5days()
        {
            var resolution = TimexResolver.Resolve(new[] { "P5D" });
            Assert.AreEqual(1, resolution.Values.Count);

            Assert.AreEqual("P5D", resolution.Values[0].Timex);
            Assert.AreEqual("duration", resolution.Values[0].Type);
            Assert.AreEqual("432000", resolution.Values[0].Value);
            Assert.IsNull(resolution.Values[0].Start);
            Assert.IsNull(resolution.Values[0].End);
        }

        [TestMethod]
        public void DataTypes_Resolver_Duration_8hours()
        {
            var resolution = TimexResolver.Resolve(new[] { "PT8H" });
            Assert.AreEqual(1, resolution.Values.Count);

            Assert.AreEqual("PT8H", resolution.Values[0].Timex);
            Assert.AreEqual("duration", resolution.Values[0].Type);
            Assert.AreEqual("28800", resolution.Values[0].Value);
            Assert.IsNull(resolution.Values[0].Start);
            Assert.IsNull(resolution.Values[0].End);
        }

        [TestMethod]
        public void DataTypes_Resolver_Duration_15minutes()
        {
            var resolution = TimexResolver.Resolve(new[] { "PT15M" });
            Assert.AreEqual(1, resolution.Values.Count);

            Assert.AreEqual("PT15M", resolution.Values[0].Timex);
            Assert.AreEqual("duration", resolution.Values[0].Type);
            Assert.AreEqual("900", resolution.Values[0].Value);
            Assert.IsNull(resolution.Values[0].Start);
            Assert.IsNull(resolution.Values[0].End);
        }

        [TestMethod]
        public void DataTypes_Resolver_Duration_10seconds()
        {
            var resolution = TimexResolver.Resolve(new[] { "PT10S" });
            Assert.AreEqual(1, resolution.Values.Count);

            Assert.AreEqual("PT10S", resolution.Values[0].Timex);
            Assert.AreEqual("duration", resolution.Values[0].Type);
            Assert.AreEqual("10", resolution.Values[0].Value);
            Assert.IsNull(resolution.Values[0].Start);
            Assert.IsNull(resolution.Values[0].End);
        }

        [TestMethod]
        public void DataTypes_Resolver_DateRange_September()
        {
            var today = new System.DateTime(2017, 9, 28);
            var resolution = TimexResolver.Resolve(new[] { "XXXX-09" }, today);
            Assert.AreEqual(2, resolution.Values.Count);

            Assert.AreEqual("XXXX-09", resolution.Values[0].Timex);
            Assert.AreEqual("daterange", resolution.Values[0].Type);
            Assert.AreEqual("2016-09-01", resolution.Values[0].Start);
            Assert.AreEqual("2016-10-01", resolution.Values[0].End);
            Assert.IsNull(resolution.Values[0].Value);

            Assert.AreEqual("XXXX-09", resolution.Values[1].Timex);
            Assert.AreEqual("daterange", resolution.Values[1].Type);
            Assert.AreEqual("2017-09-01", resolution.Values[1].Start);
            Assert.AreEqual("2017-10-01", resolution.Values[1].End);
            Assert.IsNull(resolution.Values[1].Value);
        }

        [TestMethod]
        public void DataTypes_Resolver_DateRange_Winter()
        {
            var resolution = TimexResolver.Resolve(new[] { "WI" });
            Assert.AreEqual(1, resolution.Values.Count);

            Assert.AreEqual("WI", resolution.Values[0].Timex);
            Assert.AreEqual("daterange", resolution.Values[0].Type);
            Assert.AreEqual("not resolved", resolution.Values[0].Value);
            Assert.IsNull(resolution.Values[0].Start);
            Assert.IsNull(resolution.Values[0].End);
        }

        [TestMethod]
        public void DataTypes_Resolver_DateRange_Last_Week()
        {
            var today = new System.DateTime(2019, 4, 30);
            var resolution = TimexResolver.Resolve(new[] { "2019-W17" }, today);
            Assert.AreEqual(1, resolution.Values.Count);

            Assert.AreEqual("2019-W17", resolution.Values[0].Timex);
            Assert.AreEqual("daterange", resolution.Values[0].Type);
            Assert.AreEqual("2019-04-22", resolution.Values[0].Start);
            Assert.AreEqual("2019-04-29", resolution.Values[0].End);
        }

        [TestMethod]
        public void DataTypes_Resolver_DateRange_Last_Month()
        {
            var today = new System.DateTime(2019, 4, 30);
            var resolution = TimexResolver.Resolve(new[] { "2019-03" }, today);
            Assert.AreEqual(1, resolution.Values.Count);

            Assert.AreEqual("2019-03", resolution.Values[0].Timex);
            Assert.AreEqual("daterange", resolution.Values[0].Type);
            Assert.AreEqual("2019-03-01", resolution.Values[0].Start);
            Assert.AreEqual("2019-04-01", resolution.Values[0].End);
        }

        [TestMethod]
        public void DataTypes_Resolver_DateRange_Last_Year()
        {
            var today = new System.DateTime(2019, 4, 30);
            var resolution = TimexResolver.Resolve(new[] { "2018" }, today);
            Assert.AreEqual(1, resolution.Values.Count);

            Assert.AreEqual("2018", resolution.Values[0].Timex);
            Assert.AreEqual("daterange", resolution.Values[0].Type);
            Assert.AreEqual("2018-01-01", resolution.Values[0].Start);
            Assert.AreEqual("2019-01-01", resolution.Values[0].End);
        }

        [TestMethod]
        public void DataTypes_Resolver_DateRange_Last_Three_Weeks()
        {
            var today = new System.DateTime(2019, 4, 30);
            var resolution = TimexResolver.Resolve(new[] { "(2019-04-10,2019-05-01,P3W)" }, today);
            Assert.AreEqual(1, resolution.Values.Count);

            Assert.AreEqual("(2019-04-10,2019-05-01,P3W)", resolution.Values[0].Timex);
            Assert.AreEqual("daterange", resolution.Values[0].Type);
            Assert.AreEqual("2019-04-10", resolution.Values[0].Start);
            Assert.AreEqual("2019-05-01", resolution.Values[0].End);
        }

        [TestMethod]
        public void DataTypes_Resolver_TimeRange_4am_to_8pm()
        {
            var today = System.DateTime.Now;
            var resolution = TimexResolver.Resolve(new[] { "(T04,T20,PT16H)" }, today);
            Assert.AreEqual(1, resolution.Values.Count);

            Assert.AreEqual("(T04,T20,PT16H)", resolution.Values[0].Timex);
            Assert.AreEqual("timerange", resolution.Values[0].Type);
            Assert.AreEqual("04:00:00", resolution.Values[0].Start);
            Assert.AreEqual("20:00:00", resolution.Values[0].End);
            Assert.IsNull(resolution.Values[0].Value);
        }

        [TestMethod]
        public void DataTypes_Resolver_TimeRange_Morning()
        {
            var today = System.DateTime.Now;
            var resolution = TimexResolver.Resolve(new[] { "TMO" }, today);
            Assert.AreEqual(1, resolution.Values.Count);

            Assert.AreEqual("TMO", resolution.Values[0].Timex);
            Assert.AreEqual("timerange", resolution.Values[0].Type);
            Assert.AreEqual("08:00:00", resolution.Values[0].Start);
            Assert.AreEqual("12:00:00", resolution.Values[0].End);
            Assert.IsNull(resolution.Values[0].Value);
        }

        [TestMethod]
        public void DataTypes_Resolver_TimeRange_Afternoon()
        {
            var today = System.DateTime.Now;
            var resolution = TimexResolver.Resolve(new[] { "TAF" }, today);
            Assert.AreEqual(1, resolution.Values.Count);

            Assert.AreEqual("TAF", resolution.Values[0].Timex);
            Assert.AreEqual("timerange", resolution.Values[0].Type);
            Assert.AreEqual("12:00:00", resolution.Values[0].Start);
            Assert.AreEqual("16:00:00", resolution.Values[0].End);
            Assert.IsNull(resolution.Values[0].Value);
        }

        [TestMethod]
        public void DataTypes_Resolver_TimeRange_Evening()
        {
            var today = System.DateTime.Now;
            var resolution = TimexResolver.Resolve(new[] { "TEV" }, today);
            Assert.AreEqual(1, resolution.Values.Count);

            Assert.AreEqual("TEV", resolution.Values[0].Timex);
            Assert.AreEqual("timerange", resolution.Values[0].Type);
            Assert.AreEqual("16:00:00", resolution.Values[0].Start);
            Assert.AreEqual("20:00:00", resolution.Values[0].End);
            Assert.IsNull(resolution.Values[0].Value);
        }

        [TestMethod]
        public void DataTypes_Resolver_DateTimeRange_This_Morning()
        {
            var resolution = TimexResolver.Resolve(new[] { "2017-10-07TMO" });
            Assert.AreEqual(1, resolution.Values.Count);

            Assert.AreEqual("2017-10-07TMO", resolution.Values[0].Timex);
            Assert.AreEqual("datetimerange", resolution.Values[0].Type);
            Assert.AreEqual("2017-10-07 08:00:00", resolution.Values[0].Start);
            Assert.AreEqual("2017-10-07 12:00:00", resolution.Values[0].End);
            Assert.IsNull(resolution.Values[0].Value);
        }

        [TestMethod]
        public void DataTypes_Resolver_DateTimeRange_Tonight()
        {
            var resolution = TimexResolver.Resolve(new[] { "2018-03-18TNI" });
            Assert.AreEqual(1, resolution.Values.Count);

            Assert.AreEqual("2018-03-18TNI", resolution.Values[0].Timex);
            Assert.AreEqual("datetimerange", resolution.Values[0].Type);
            Assert.AreEqual("2018-03-18 20:00:00", resolution.Values[0].Start);
            Assert.AreEqual("2018-03-18 24:00:00", resolution.Values[0].End);
            Assert.IsNull(resolution.Values[0].Value);
        }

        [TestMethod]
        public void DataTypes_Resolver_DateTimeRange_next_monday_4am_to_next_thursday_3pm()
        {
            var today = System.DateTime.Now;
            var resolution = TimexResolver.Resolve(new[] { "(2017-10-09T04,2017-10-12T15,PT83H)" }, today);
            Assert.AreEqual(1, resolution.Values.Count);

            Assert.AreEqual("(2017-10-09T04,2017-10-12T15,PT83H)", resolution.Values[0].Timex);
            Assert.AreEqual("datetimerange", resolution.Values[0].Type);
            Assert.AreEqual("2017-10-09 04:00:00", resolution.Values[0].Start);
            Assert.AreEqual("2017-10-12 15:00:00", resolution.Values[0].End);
            Assert.IsNull(resolution.Values[0].Value);
        }

        [TestMethod]
        public void DataTypes_Resolver_Time_4am()
        {
            var resolution = TimexResolver.Resolve(new[] { "T04" });
            Assert.AreEqual(1, resolution.Values.Count);

            Assert.AreEqual("T04", resolution.Values[0].Timex);
            Assert.AreEqual("time", resolution.Values[0].Type);
            Assert.AreEqual("04:00:00", resolution.Values[0].Value);
            Assert.IsNull(resolution.Values[0].Start);
            Assert.IsNull(resolution.Values[0].End);
        }

        [TestMethod]
        public void DataTypes_Resolver_Time_4_oclock()
        {
            var resolution = TimexResolver.Resolve(new[] { "T04", "T16" });
            Assert.AreEqual(2, resolution.Values.Count);

            Assert.AreEqual("T04", resolution.Values[0].Timex);
            Assert.AreEqual("time", resolution.Values[0].Type);
            Assert.AreEqual("04:00:00", resolution.Values[0].Value);
            Assert.IsNull(resolution.Values[0].Start);
            Assert.IsNull(resolution.Values[0].End);

            Assert.AreEqual("T16", resolution.Values[1].Timex);
            Assert.AreEqual("time", resolution.Values[1].Type);
            Assert.AreEqual("16:00:00", resolution.Values[1].Value);
            Assert.IsNull(resolution.Values[1].Start);
            Assert.IsNull(resolution.Values[1].End);
        }
    }
}
