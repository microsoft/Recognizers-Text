// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Globalization;
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
        public void DataTypes_Resolver_Date_6th()
        {
            var today = new System.DateTime(2019, 4, 23, 15, 30, 0);
            var resolution = TimexResolver.Resolve(new[] { "XXXX-XX-06" }, today);
            Assert.AreEqual(2, resolution.Values.Count);

            Assert.AreEqual("XXXX-XX-06", resolution.Values[0].Timex);
            Assert.AreEqual("date", resolution.Values[0].Type);
            Assert.AreEqual("2019-04-06", resolution.Values[0].Value);
            Assert.IsNull(resolution.Values[0].Start);
            Assert.IsNull(resolution.Values[0].End);

            Assert.AreEqual("XXXX-XX-06", resolution.Values[1].Timex);
            Assert.AreEqual("date", resolution.Values[1].Type);
            Assert.AreEqual("2019-05-06", resolution.Values[1].Value);
            Assert.IsNull(resolution.Values[1].Start);
            Assert.IsNull(resolution.Values[1].End);
        }

        [TestMethod]
        public void DataTypes_Resolver_Date_Feb_2nd()
        {
            var today = new System.DateTime(2020, 10, 20);
            var resolution = TimexResolver.Resolve(new[] { "XXXX-02-02 " }, today);
            Assert.AreEqual(2, resolution.Values.Count);

            Assert.AreEqual("XXXX-02-02", resolution.Values[0].Timex);
            Assert.AreEqual("date", resolution.Values[0].Type);
            Assert.AreEqual("2020-02-02", resolution.Values[0].Value);
            Assert.IsNull(resolution.Values[0].Start);
            Assert.IsNull(resolution.Values[0].End);

            Assert.AreEqual("XXXX-02-02", resolution.Values[1].Timex);
            Assert.AreEqual("date", resolution.Values[1].Type);
            Assert.AreEqual("2021-02-02", resolution.Values[1].Value);
            Assert.IsNull(resolution.Values[1].Start);
            Assert.IsNull(resolution.Values[1].End);
        }

        [TestMethod]
        public void DataTypes_Resolver_DateTimeRange_Oct_25th_Afternoon()
        {
            var today = new System.DateTime(2020, 10, 20);
            var resolution = TimexResolver.Resolve(new[] { "XXXX-10-25TAF" }, today);
            Assert.AreEqual(2, resolution.Values.Count);

            Assert.AreEqual("XXXX-10-25TAF", resolution.Values[0].Timex);
            Assert.AreEqual("datetimerange", resolution.Values[0].Type);
            Assert.AreEqual("2019-10-25 12:00:00", resolution.Values[0].Start);
            Assert.AreEqual("2019-10-25 16:00:00", resolution.Values[0].End);
            Assert.IsNull(resolution.Values[0].Value);

            Assert.AreEqual("XXXX-10-25TAF", resolution.Values[1].Timex);
            Assert.AreEqual("datetimerange", resolution.Values[1].Type);
            Assert.AreEqual("2020-10-25 12:00:00", resolution.Values[1].Start);
            Assert.AreEqual("2020-10-25 16:00:00", resolution.Values[1].End);
            Assert.IsNull(resolution.Values[1].Value);
        }

        [TestMethod]
        public void DataTypes_Resolver_DateTimeRange_Week11_Monday()
        {
            var today = new System.DateTime(2020, 10, 20);
            var resolution = TimexResolver.Resolve(new[] { "XXXX-W11-1" }, today);
            Assert.AreEqual(2, resolution.Values.Count);

            Assert.AreEqual("XXXX-W11-1", resolution.Values[0].Timex);
            Assert.AreEqual("date", resolution.Values[0].Type);
            Assert.IsNull(resolution.Values[0].Start);
            Assert.IsNull(resolution.Values[0].End);
            Assert.AreEqual("2020-03-09", resolution.Values[0].Value);

            Assert.AreEqual("XXXX-W11-1", resolution.Values[1].Timex);
            Assert.AreEqual("date", resolution.Values[1].Type);
            Assert.IsNull(resolution.Values[1].Start);
            Assert.IsNull(resolution.Values[1].End);
            Assert.AreEqual("2021-03-15", resolution.Values[1].Value);
        }

        [TestMethod]
        public void DataTypes_Resolver_DateTimeRange_Thanksgiving()
        {
            // XXXX-11-WXX-4-4 -> 4th Thursday (4th ISO weekday) in unspecified week in November in unspecified year
            var today = new System.DateTime(2020, 10, 20);
            var resolution = TimexResolver.Resolve(new[] { "XXXX-11-WXX-4-4" }, today);
            Assert.AreEqual(2, resolution.Values.Count);

            Assert.AreEqual("XXXX-11-WXX-4-4", resolution.Values[0].Timex);
            Assert.AreEqual("date", resolution.Values[0].Type);
            Assert.IsNull(resolution.Values[0].Start);
            Assert.IsNull(resolution.Values[0].End);
            Assert.AreEqual("2019-11-28", resolution.Values[0].Value);

            Assert.AreEqual("XXXX-11-WXX-4-4", resolution.Values[1].Timex);
            Assert.AreEqual("date", resolution.Values[1].Type);
            Assert.IsNull(resolution.Values[1].Start);
            Assert.IsNull(resolution.Values[1].End);
            Assert.AreEqual("2020-11-26", resolution.Values[1].Value);
        }

        [TestMethod]
        public void DataTypes_Resolver_DateTimeRange_Monday_Morning()
        {
            var today = new System.DateTime(2021, 1, 22, 15, 30, 0);
            var resolution = TimexResolver.Resolve(new[] { "XXXX-WXX-1TMO" }, today);
            Assert.AreEqual(2, resolution.Values.Count);

            Assert.AreEqual("XXXX-WXX-1TMO", resolution.Values[0].Timex);
            Assert.AreEqual("datetimerange", resolution.Values[0].Type);
            Assert.AreEqual("2021-01-18 08:00:00", resolution.Values[0].Start);
            Assert.AreEqual("2021-01-18 12:00:00", resolution.Values[0].End);
            Assert.IsNull(resolution.Values[0].Value);

            Assert.AreEqual("XXXX-WXX-1TMO", resolution.Values[1].Timex);
            Assert.AreEqual("datetimerange", resolution.Values[1].Type);
            Assert.AreEqual("2021-01-25 08:00:00", resolution.Values[1].Start);
            Assert.AreEqual("2021-01-25 12:00:00", resolution.Values[1].End);
            Assert.IsNull(resolution.Values[1].Value);
        }

        [TestMethod]
        public void DataTypes_Resolver_DateTimeRange_April_5th_from_10am_to_11am()
        {
            var today = new System.DateTime(2021, 1, 22, 15, 30, 0);
            var resolution = TimexResolver.Resolve(new[] { "(XXXX-04-05T10,XXXX-04-05T11,PT1H)" }, today);
            Assert.AreEqual(2, resolution.Values.Count);

            Assert.AreEqual("(XXXX-04-05T10,XXXX-04-05T11,PT1H)", resolution.Values[0].Timex);
            Assert.AreEqual("datetimerange", resolution.Values[0].Type);
            Assert.AreEqual("2020-04-05 10:00:00", resolution.Values[0].Start);
            Assert.AreEqual("2020-04-05 11:00:00", resolution.Values[0].End);
            Assert.IsNull(resolution.Values[0].Value);

            Assert.AreEqual("(XXXX-04-05T10,XXXX-04-05T11,PT1H)", resolution.Values[1].Timex);
            Assert.AreEqual("datetimerange", resolution.Values[1].Type);
            Assert.AreEqual("2021-04-05 10:00:00", resolution.Values[1].Start);
            Assert.AreEqual("2021-04-05 11:00:00", resolution.Values[1].End);
            Assert.IsNull(resolution.Values[1].Value);
        }

        [TestMethod]
        public void DataTypes_Resolver_DateRange_first_week_of_April_2019()
        {
            var today = new System.DateTime(2021, 1, 22, 15, 30, 0);
            var resolution = TimexResolver.Resolve(new[] { "2019-04-W01" }, today);
            Assert.AreEqual(1, resolution.Values.Count);

            Assert.AreEqual("2019-04-W01", resolution.Values[0].Timex);
            Assert.AreEqual("daterange", resolution.Values[0].Type);
            Assert.AreEqual("2019-04-01", resolution.Values[0].Start);
            Assert.AreEqual("2019-04-08", resolution.Values[0].End);
        }

        [TestMethod]
        public void DataTypes_Resolver_DateRange_first_week_of_April()
        {
            var today = new System.DateTime(2021, 1, 22);
            var resolution = TimexResolver.Resolve(new[] { "XXXX-04-W01" }, today);
            Assert.AreEqual(2, resolution.Values.Count);

            Assert.AreEqual("XXXX-04-W01", resolution.Values[0].Timex);
            Assert.AreEqual("daterange", resolution.Values[0].Type);
            Assert.AreEqual("2020-03-30", resolution.Values[0].Start);
            Assert.AreEqual("2020-04-06", resolution.Values[0].End);

            Assert.AreEqual("XXXX-04-W01", resolution.Values[1].Timex);
            Assert.AreEqual("daterange", resolution.Values[1].Type);
            Assert.AreEqual("2021-03-29", resolution.Values[1].Start);
            Assert.AreEqual("2021-04-05", resolution.Values[1].End);
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
        public void DataTypes_Resolver_Duration_1hour30minutes()
        {
            var resolution = TimexResolver.Resolve(new[] { "PT1H30M" });
            Assert.AreEqual(1, resolution.Values.Count);

            Assert.AreEqual("PT1H30M", resolution.Values[0].Timex);
            Assert.AreEqual("duration", resolution.Values[0].Type);
            Assert.AreEqual("5400", resolution.Values[0].Value);
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
        public void DataTypes_Resolver_DateRange_Decimal_Period_PT()
        {
            var sourceLanguage = CultureInfo.CurrentCulture;
            var testLanguage = new CultureInfo("pt-PT", false);
            CultureInfo.CurrentCulture = testLanguage;
            var today = new System.DateTime(2019, 4, 30);
            var resolution = TimexResolver.Resolve(new[] { "(2019-04-05,XXXX-04-11,P5.54701493625231D)" }, today);
            Assert.AreEqual(1, resolution.Values.Count);
            Assert.AreEqual("(2019-04-05,2019-04-10,P5.54701493625231D)", resolution.Values[0].Timex);
            Assert.AreEqual("daterange", resolution.Values[0].Type);
            Assert.AreEqual("2019-04-05", resolution.Values[0].Start);
            Assert.AreEqual("2019-04-10", resolution.Values[0].End);
            CultureInfo.CurrentCulture = sourceLanguage;
        }

        [TestMethod]
        public void DataTypes_Resolver_DateRange_Demaical_Period_EN()
        {
            var sourceLanguage = CultureInfo.CurrentCulture;
            var testLanguage = new CultureInfo("en-En", false);
            CultureInfo.CurrentCulture = testLanguage;
            var today = new System.DateTime(2019, 4, 30);
            var resolution = TimexResolver.Resolve(new[] { "(2019-04-05,XXXX-04-11,P5.54701493625231D)" }, today);
            Assert.AreEqual(1, resolution.Values.Count);
            Assert.AreEqual("(2019-04-05,2019-04-10,P5.54701493625231D)", resolution.Values[0].Timex);
            Assert.AreEqual("daterange", resolution.Values[0].Type);
            Assert.AreEqual("2019-04-05", resolution.Values[0].Start);
            Assert.AreEqual("2019-04-10", resolution.Values[0].End);
            CultureInfo.CurrentCulture = sourceLanguage;
        }

        [TestMethod]
        public void DataTypes_Resolver_TimeRange_11_30_to_12_00()
        {
            var today = System.DateTime.Now;
            var resolution = TimexResolver.Resolve(new[] { "(T11:30,T12:00,PT30M)" }, today);
            Assert.AreEqual(1, resolution.Values.Count);

            Assert.AreEqual("(T11:30,T12,PT30M)", resolution.Values[0].Timex);
            Assert.AreEqual("timerange", resolution.Values[0].Type);
            Assert.AreEqual("11:30:00", resolution.Values[0].Start);
            Assert.AreEqual("12:00:00", resolution.Values[0].End);
            Assert.IsNull(resolution.Values[0].Value);
        }

        [TestMethod]
        public void DataTypes_Resolver_TimeRange_11_30_to_12()
        {
            var today = System.DateTime.Now;
            var resolution = TimexResolver.Resolve(new[] { "(T11:30,T12,PT30M)" }, today);
            Assert.AreEqual(1, resolution.Values.Count);

            Assert.AreEqual("(T11:30,T12,PT30M)", resolution.Values[0].Timex);
            Assert.AreEqual("timerange", resolution.Values[0].Type);
            Assert.AreEqual("11:30:00", resolution.Values[0].Start);
            Assert.AreEqual("12:00:00", resolution.Values[0].End);
            Assert.IsNull(resolution.Values[0].Value);
        }

        [TestMethod]
        public void DataTypes_Resolver_TimeRange_11_to_11_30()
        {
            var today = System.DateTime.Now;
            var resolution = TimexResolver.Resolve(new[] { "(T11:00,T11:30,PT30M)" }, today);
            Assert.AreEqual(1, resolution.Values.Count);

            Assert.AreEqual("(T11,T11:30,PT30M)", resolution.Values[0].Timex);
            Assert.AreEqual("timerange", resolution.Values[0].Type);
            Assert.AreEqual("11:00:00", resolution.Values[0].Start);
            Assert.AreEqual("11:30:00", resolution.Values[0].End);
            Assert.IsNull(resolution.Values[0].Value);
        }

        [TestMethod]
        public void DataTypes_Resolver_TimeRange_23_45_to_00_30()
        {
            var today = System.DateTime.Now;
            var resolution = TimexResolver.Resolve(new[] { "(T23:45,T00:30,PT45M)" }, today);
            Assert.AreEqual(1, resolution.Values.Count);

            Assert.AreEqual("(T23:45,T00:30,PT45M)", resolution.Values[0].Timex);
            Assert.AreEqual("timerange", resolution.Values[0].Type);
            Assert.AreEqual("23:45:00", resolution.Values[0].Start);
            Assert.AreEqual("00:30:00", resolution.Values[0].End);
            Assert.IsNull(resolution.Values[0].Value);
        }

        [TestMethod]
        public void DataTypes_Resolver_DateTimeRange_20190401_09_30_to_20190401_11()
        {
            var today = System.DateTime.Now;
            var resolution = TimexResolver.Resolve(new[] { "(2019-04-01T09:30,2019-04-01T11,PT1H30M)" }, today);
            Assert.AreEqual(1, resolution.Values.Count);

            Assert.AreEqual("(2019-04-01T09:30,2019-04-01T11,PT1H30M)", resolution.Values[0].Timex);
            Assert.AreEqual("datetimerange", resolution.Values[0].Type);
            Assert.AreEqual("2019-04-01 09:30:00", resolution.Values[0].Start);
            Assert.AreEqual("2019-04-01 11:00:00", resolution.Values[0].End);
            Assert.IsNull(resolution.Values[0].Value);
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
        public void DataTypes_Resolver_TimeRange_23_45_to_01_20()
        {
            var today = System.DateTime.Now;
            var resolution = TimexResolver.Resolve(new[] { "(T23:45,T01:20,PT1H35M)" }, today);
            Assert.AreEqual(1, resolution.Values.Count);

            Assert.AreEqual("(T23:45,T01:20,PT1H35M)", resolution.Values[0].Timex);
            Assert.AreEqual("timerange", resolution.Values[0].Type);
            Assert.AreEqual("23:45:00", resolution.Values[0].Start);
            Assert.AreEqual("01:20:00", resolution.Values[0].End);
            Assert.IsNull(resolution.Values[0].Value);
        }

        [TestMethod]
        public void DataTypes_Resolver_TimeRange_15_15_to_16_20()
        {
            var today = System.DateTime.Now;
            var resolution = TimexResolver.Resolve(new[] { "(T15:15,T16:20,PT1H5M)" }, today);
            Assert.AreEqual(1, resolution.Values.Count);

            Assert.AreEqual("(T15:15,T16:20,PT1H5M)", resolution.Values[0].Timex);
            Assert.AreEqual("timerange", resolution.Values[0].Type);
            Assert.AreEqual("15:15:00", resolution.Values[0].Start);
            Assert.AreEqual("16:20:00", resolution.Values[0].End);
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
        public void DataTypes_Resolver_DateTimeRange_20200604_15_00_to_20200604_17_30()
        {
            var today = System.DateTime.Now;
            var resolution = TimexResolver.Resolve(new[] { "(2020-06-04T15,2020-06-04T17:30,PT2H30M)" }, today);
            Assert.AreEqual(1, resolution.Values.Count);

            Assert.AreEqual("(2020-06-04T15,2020-06-04T17:30,PT2H30M)", resolution.Values[0].Timex);
            Assert.AreEqual("datetimerange", resolution.Values[0].Type);
            Assert.AreEqual("2020-06-04 15:00:00", resolution.Values[0].Start);
            Assert.AreEqual("2020-06-04 17:30:00", resolution.Values[0].End);
            Assert.IsNull(resolution.Values[0].Value);
        }

        [TestMethod]
        public void DataTypes_Resolver_DateTimeRange_20190325_10_to_20190325_11()
        {
            var today = System.DateTime.Now;
            var resolution = TimexResolver.Resolve(new[] { "(2019-03-25T10,2019-03-25T11,PT1H)" }, today);
            Assert.AreEqual(1, resolution.Values.Count);

            Assert.AreEqual("(2019-03-25T10,2019-03-25T11,PT1H)", resolution.Values[0].Timex);
            Assert.AreEqual("datetimerange", resolution.Values[0].Type);
            Assert.AreEqual("2019-03-25 10:00:00", resolution.Values[0].Start);
            Assert.AreEqual("2019-03-25 11:00:00", resolution.Values[0].End);
            Assert.IsNull(resolution.Values[0].Value);
        }

        [TestMethod]
        public void DataTypes_Resolver_DateRange_20190427_20190511_2weeks()
        {
            var today = System.DateTime.Now;
            var resolution = TimexResolver.Resolve(new[] { "(2019-04-27,2019-05-11,P2W)" }, today);
            Assert.AreEqual(1, resolution.Values.Count);

            Assert.AreEqual("(2019-04-27,2019-05-11,P2W)", resolution.Values[0].Timex);
            Assert.AreEqual("daterange", resolution.Values[0].Type);
            Assert.AreEqual("2019-04-27", resolution.Values[0].Start);
            Assert.AreEqual("2019-05-11", resolution.Values[0].End);
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

        [TestMethod]
        public void DataTypes_Resolver_Date_SecondWeekInAugust()
        {
            var today = new System.DateTime(2019, 11, 06);
            var resolution = TimexResolver.Resolve(new[] { "XXXX-08-W02" }, today);
            Assert.AreEqual(2, resolution.Values.Count);

            Assert.AreEqual("XXXX-08-W02", resolution.Values[0].Timex);
            Assert.AreEqual("daterange", resolution.Values[0].Type);
            Assert.AreEqual("2018-08-06", resolution.Values[0].Start);
            Assert.AreEqual("2018-08-13", resolution.Values[0].End);
            Assert.IsNull(resolution.Values[0].Value);

            Assert.AreEqual("XXXX-08-W02", resolution.Values[1].Timex);
            Assert.AreEqual("daterange", resolution.Values[1].Type);
            Assert.AreEqual("2019-08-05", resolution.Values[1].Start);
            Assert.AreEqual("2019-08-12", resolution.Values[1].End);
            Assert.IsNull(resolution.Values[1].Value);
        }

        [TestMethod]
        public void DataTypes_Resolver_DateTime_Nov_6_at_11_45_25()
        {
            var today = new System.DateTime(2017, 9, 28, 15, 30, 0);
            var resolution = TimexResolver.Resolve(new[] { "2019-11-06T11:45:25" }, today);
            Assert.AreEqual(1, resolution.Values.Count);

            Assert.AreEqual("2019-11-06T11:45:25", resolution.Values[0].Timex);
            Assert.AreEqual("datetime", resolution.Values[0].Type);
            Assert.AreEqual("2019-11-06 11:45:25", resolution.Values[0].Value);
            Assert.IsNull(resolution.Values[0].Start);
            Assert.IsNull(resolution.Values[0].End);
        }

        [TestMethod]
        public void DataTypes_Resolver_DateTime_Nov_6_at_11_45_25_UTC()
        {
            var today = new System.DateTime(2017, 9, 28, 15, 30, 0);
            var resolution = TimexResolver.Resolve(new[] { "2019-11-06T11:45:25Z" }, today);
            Assert.AreEqual(1, resolution.Values.Count);

            Assert.AreEqual("2019-11-06T11:45:25", resolution.Values[0].Timex);
            Assert.AreEqual("datetime", resolution.Values[0].Type);
            Assert.AreEqual("2019-11-06 11:45:25", resolution.Values[0].Value);
            Assert.IsNull(resolution.Values[0].Start);
            Assert.IsNull(resolution.Values[0].End);
        }

        [TestMethod]
        public void DataTypes_Resolver_DateTime_TuesAt12PM()
        {
            var today = new System.DateTime(2019, 12, 05);
            var resolution = TimexResolver.Resolve(new[] { "XXXX-WXX-2T12" }, today);
            Assert.AreEqual(2, resolution.Values.Count);

            Assert.AreEqual("XXXX-WXX-2T12", resolution.Values[0].Timex);
            Assert.AreEqual("datetime", resolution.Values[0].Type);
            Assert.AreEqual("2019-12-03 12:00:00", resolution.Values[0].Value);
            Assert.IsNull(resolution.Values[0].Start);
            Assert.IsNull(resolution.Values[0].End);

            Assert.AreEqual("XXXX-WXX-2T12", resolution.Values[1].Timex);
            Assert.AreEqual("datetime", resolution.Values[1].Type);
            Assert.AreEqual("2019-12-10 12:00:00", resolution.Values[1].Value);
            Assert.IsNull(resolution.Values[1].Start);
            Assert.IsNull(resolution.Values[1].End);
        }

        [TestMethod]
        public void DataTypes_Resolver_DateTime_TuesAt12PM_UtcInput()
        {
            var today = new System.DateTime(2019, 12, 05);
            var resolution = TimexResolver.Resolve(new[] { "XXXX-WXX-2T12" }, today.ToUniversalTime());
            Assert.AreEqual(2, resolution.Values.Count);

            var previousWeekLocal = new System.DateTime(2019, 12, 03, 12, 0, 0, System.DateTimeKind.Local);
            var previousWeekUtc = previousWeekLocal.ToUniversalTime();

            Assert.AreEqual("XXXX-WXX-2T12", resolution.Values[0].Timex);
            Assert.AreEqual("datetime", resolution.Values[0].Type);
            Assert.AreEqual(previousWeekUtc.ToString("yyyy-MM-dd HH:mm:ss"), resolution.Values[0].Value);
            Assert.IsNull(resolution.Values[0].Start);
            Assert.IsNull(resolution.Values[0].End);

            var nextWeekLocal = new System.DateTime(2019, 12, 10, 12, 0, 0, System.DateTimeKind.Local);
            var nextWeekUtc = nextWeekLocal.ToUniversalTime();

            Assert.AreEqual("XXXX-WXX-2T12", resolution.Values[1].Timex);
            Assert.AreEqual("datetime", resolution.Values[1].Type);
            Assert.AreEqual(nextWeekUtc.ToString("yyyy-MM-dd HH:mm:ss"), resolution.Values[1].Value);
            Assert.IsNull(resolution.Values[1].Start);
            Assert.IsNull(resolution.Values[1].End);
        }

        [TestMethod]
        public void DataTypes_Resolver_DateTime_2021W01() // first day of the year is a Friday - week 1
        {
            var today = new System.DateTime(2021, 01, 05);
            var resolution = TimexResolver.Resolve(new[] { "2021-W01" }, today.ToUniversalTime());
            Assert.AreEqual(1, resolution.Values.Count);

            Assert.AreEqual("2021-W01", resolution.Values[0].Timex);
            Assert.AreEqual("daterange", resolution.Values[0].Type);
            Assert.AreEqual("2021-01-04", resolution.Values[0].Start);
            Assert.AreEqual("2021-01-11", resolution.Values[0].End);
        }

        [TestMethod]
        public void DataTypes_Resolver_DateTime_2021W02() // first day of the year is a Friday - week 2
        {
            var today = new System.DateTime(2021, 01, 05);
            var resolution = TimexResolver.Resolve(new[] { "2021-W02" }, today.ToUniversalTime());
            Assert.AreEqual(1, resolution.Values.Count);

            Assert.AreEqual("2021-W02", resolution.Values[0].Timex);
            Assert.AreEqual("daterange", resolution.Values[0].Type);
            Assert.AreEqual("2021-01-11", resolution.Values[0].Start);
            Assert.AreEqual("2021-01-18", resolution.Values[0].End);
        }

        [TestMethod]
        public void DataTypes_Resolver_DateTime_2020W53() // has a 53-week year
        {
            var today = new System.DateTime(2020, 12, 30);
            var resolution = TimexResolver.Resolve(new[] { "2020-W53" }, today.ToUniversalTime());
            Assert.AreEqual(1, resolution.Values.Count);

            Assert.AreEqual("2020-W53", resolution.Values[0].Timex);
            Assert.AreEqual("daterange", resolution.Values[0].Type);
            Assert.AreEqual("2020-12-28", resolution.Values[0].Start);
            Assert.AreEqual("2021-01-04", resolution.Values[0].End);
        }

        [TestMethod]
        public void DataTypes_Resolver_DateTime_2024W01() // first day of the year is a Monday
        {
            var today = new System.DateTime(2024, 01, 01);
            var resolution = TimexResolver.Resolve(new[] { "2024-W01" }, today.ToUniversalTime());
            Assert.AreEqual(1, resolution.Values.Count);

            Assert.AreEqual("2024-W01", resolution.Values[0].Timex);
            Assert.AreEqual("daterange", resolution.Values[0].Type);
            Assert.AreEqual("2024-01-01", resolution.Values[0].Start);
            Assert.AreEqual("2024-01-08", resolution.Values[0].End);
        }

        [TestMethod]
        public void DataTypes_Resolver_DateTime_Weekend()
        {
            var today = new System.DateTime(2020, 1, 7);
            var resolution = TimexResolver.Resolve(new[] { "2020-W02-WE" }, today);
            Assert.AreEqual(1, resolution.Values.Count);

            Assert.AreEqual("2020-W02-WE", resolution.Values[0].Timex);
            Assert.AreEqual("daterange", resolution.Values[0].Type);
            Assert.AreEqual("2020-01-11", resolution.Values[0].Start);
            Assert.AreEqual("2020-01-13", resolution.Values[0].End);
            Assert.IsNull(resolution.Values[0].Value);
        }

        [TestMethod]
        public void DataTypes_Resolver_MonthRange_December()
        {
            var today = new System.DateTime(2020, 3, 25);
            var resolution = TimexResolver.Resolve(new[] { "XXXX-12" }, today);
            Assert.AreEqual(2, resolution.Values.Count);

            Assert.AreEqual("XXXX-12", resolution.Values[0].Timex);
            Assert.AreEqual("daterange", resolution.Values[0].Type);
            Assert.AreEqual("2019-12-01", resolution.Values[0].Start);
            Assert.AreEqual("2020-01-01", resolution.Values[0].End);
            Assert.IsNull(resolution.Values[0].Value);

            Assert.AreEqual("XXXX-12", resolution.Values[1].Timex);
            Assert.AreEqual("daterange", resolution.Values[1].Type);
            Assert.AreEqual("2020-12-01", resolution.Values[1].Start);
            Assert.AreEqual("2021-01-01", resolution.Values[1].End);
            Assert.IsNull(resolution.Values[1].Value);
        }
    }
}
