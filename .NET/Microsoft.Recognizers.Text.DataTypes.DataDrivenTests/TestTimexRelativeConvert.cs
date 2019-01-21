// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Recognizers.Text.DataTypes.TimexExpression.Tests
{
    [TestClass]
    public class TestTimexRelativeConvert
    {
        [TestMethod]
        public void DataTypes_RelativeConvert_Date_Today()
        {
            var timex = new TimexProperty("2017-09-25");
            var today = new System.DateTime(2017, 9, 25);
            Assert.AreEqual("today", TimexRelativeConvert.ConvertTimexToStringRelative(timex, today));
        }

        [TestMethod]
        public void DataTypes_RelativeConvert_Date_Tomorrow()
        {
            var timex = new TimexProperty("2017-09-23");
            var today = new System.DateTime(2017, 9, 22);
            Assert.AreEqual("tomorrow", TimexRelativeConvert.ConvertTimexToStringRelative(timex, today));
        }

        [TestMethod]
        public void DataTypes_RelativeConvert_Date_tomorrow_cross_year_month_boundary()
        {
            var timex = new TimexProperty("2018-01-01");
            var today = new System.DateTime(2017, 12, 31);
            Assert.AreEqual("tomorrow", TimexRelativeConvert.ConvertTimexToStringRelative(timex, today));
        }

        [TestMethod]
        public void DataTypes_RelativeConvert_Date_yesterday()
        {
            var timex = new TimexProperty("2017-09-21");
            var today = new System.DateTime(2017, 9, 22);
            Assert.AreEqual("yesterday", TimexRelativeConvert.ConvertTimexToStringRelative(timex, today));
        }

        [TestMethod]
        public void DataTypes_RelativeConvert_Date_yesterday_cross_year_month_boundary()
        {
            var timex = new TimexProperty("2017-12-31");
            var today = new System.DateTime(2018, 1, 1);
            Assert.AreEqual("yesterday", TimexRelativeConvert.ConvertTimexToStringRelative(timex, today));
        }

        [TestMethod]
        public void DataTypes_RelativeConvert_Date_this_week()
        {
            var timex = new TimexProperty("2017-10-18");
            var today = new System.DateTime(2017, 10, 16);
            Assert.AreEqual("this Wednesday", TimexRelativeConvert.ConvertTimexToStringRelative(timex, today));
        }

        [TestMethod]
        public void DataTypes_RelativeConvert_Date_this_week_cross_year_month_boundary()
        {
            var timex = new TimexProperty("2017-11-03");
            var today = new System.DateTime(2017, 10, 31);
            Assert.AreEqual("this Friday", TimexRelativeConvert.ConvertTimexToStringRelative(timex, today));
        }

        [TestMethod]
        public void DataTypes_RelativeConvert_Date_next_week()
        {
            var timex = new TimexProperty("2017-09-27");
            var today = new System.DateTime(2017, 9, 22);
            Assert.AreEqual("next Wednesday", TimexRelativeConvert.ConvertTimexToStringRelative(timex, today));
        }

        [TestMethod]
        public void DataTypes_RelativeConvert_Date_next_week_cross_year_month_boundary()
        {
            var timex = new TimexProperty("2018-01-05");
            var today = new System.DateTime(2017, 12, 28);
            Assert.AreEqual("next Friday", TimexRelativeConvert.ConvertTimexToStringRelative(timex, today));
        }

        [TestMethod]
        public void DataTypes_RelativeConvert_Date_last_week()
        {
            var timex = new TimexProperty("2017-09-14");
            var today = new System.DateTime(2017, 9, 22);
            Assert.AreEqual("last Thursday", TimexRelativeConvert.ConvertTimexToStringRelative(timex, today));
        }

        [TestMethod]
        public void DataTypes_RelativeConvert_Date_last_week_cross_year_month_boundary()
        {
            var timex = new TimexProperty("2017-12-25");
            var today = new System.DateTime(2018, 1, 4);
            Assert.AreEqual("last Monday", TimexRelativeConvert.ConvertTimexToStringRelative(timex, today));
        }

        [TestMethod]
        public void DataTypes_RelativeConvert_Date_this_week_2()
        {
            var timex = new TimexProperty("2017-10-25");
            var today = new System.DateTime(2017, 9, 9);
            Assert.AreEqual("25th October 2017", TimexRelativeConvert.ConvertTimexToStringRelative(timex, today));
        }

        [TestMethod]
        public void DataTypes_RelativeConvert_Date_next_week_2()
        {
            var timex = new TimexProperty("2017-10-04");
            var today = new System.DateTime(2017, 9, 22);
            Assert.AreEqual("4th October 2017", TimexRelativeConvert.ConvertTimexToStringRelative(timex, today));
        }

        [TestMethod]
        public void DataTypes_RelativeConvert_Date_last_week_2()
        {
            var timex = new TimexProperty("2017-09-07");
            var today = new System.DateTime(2017, 9, 22);
            Assert.AreEqual("7th September 2017", TimexRelativeConvert.ConvertTimexToStringRelative(timex, today));
        }

        [TestMethod]
        public void DataTypes_RelativeConvert_DateTime_today()
        {
            var timex = new TimexProperty("2017-09-25T16:00:00");
            var today = new System.DateTime(2017, 9, 25);
            Assert.AreEqual("today 4PM", TimexRelativeConvert.ConvertTimexToStringRelative(timex, today));
        }

        [TestMethod]
        public void DataTypes_RelativeConvert_DateTime_tomorrow()
        {
            var timex = new TimexProperty("2017-09-23T16:00:00");
            var today = new System.DateTime(2017, 9, 22);
            Assert.AreEqual("tomorrow 4PM", TimexRelativeConvert.ConvertTimexToStringRelative(timex, today));
        }

        [TestMethod]
        public void DataTypes_RelativeConvert_DateTime_yesterday()
        {
            var timex = new TimexProperty("2017-09-21T16:00:00");
            var today = new System.DateTime(2017, 9, 22);
            Assert.AreEqual("yesterday 4PM", TimexRelativeConvert.ConvertTimexToStringRelative(timex, today));
        }

        [TestMethod]
        public void DataTypes_RelativeConvert_DateRange_this_week()
        {
            var timex = new TimexProperty("2017-W40");
            var today = new System.DateTime(2017, 9, 25);
            Assert.AreEqual("this week", TimexRelativeConvert.ConvertTimexToStringRelative(timex, today));
        }

        [TestMethod]
        public void DataTypes_RelativeConvert_DateRange_next_week()
        {
            var timex = new TimexProperty("2017-W41");
            var today = new System.DateTime(2017, 9, 25);
            Assert.AreEqual("next week", TimexRelativeConvert.ConvertTimexToStringRelative(timex, today));
        }

        [TestMethod]
        public void DataTypes_RelativeConvert_DateRange_last_week()
        {
            var timex = new TimexProperty("2017-W39");
            var today = new System.DateTime(2017, 9, 25);
            Assert.AreEqual("last week", TimexRelativeConvert.ConvertTimexToStringRelative(timex, today));
        }

        [TestMethod]
        public void DataTypes_RelativeConvert_DateRange_this_week_2()
        {
            var timex = new TimexProperty("2017-W41");
            var today = new System.DateTime(2017, 10, 4);
            Assert.AreEqual("this week", TimexRelativeConvert.ConvertTimexToStringRelative(timex, today));
        }

        [TestMethod]
        public void DataTypes_RelativeConvert_DateRange_next_week_2()
        {
            var timex = new TimexProperty("2017-W42");
            var today = new System.DateTime(2017, 10, 4);
            Assert.AreEqual("next week", TimexRelativeConvert.ConvertTimexToStringRelative(timex, today));
        }

        [TestMethod]
        public void DataTypes_RelativeConvert_DateRange_last_week_2()
        {
            var timex = new TimexProperty("2017-W40");
            var today = new System.DateTime(2017, 10, 4);
            Assert.AreEqual("last week", TimexRelativeConvert.ConvertTimexToStringRelative(timex, today));
        }

        [TestMethod]
        public void DataTypes_RelativeConvert_Weekend_this_weekend()
        {
            var timex = new TimexProperty("2017-W40-WE");
            var today = new System.DateTime(2017, 9, 25);
            Assert.AreEqual("this weekend", TimexRelativeConvert.ConvertTimexToStringRelative(timex, today));
        }

        [TestMethod]
        public void DataTypes_RelativeConvert_Weekend_next_weekend()
        {
            var timex = new TimexProperty("2017-W41-WE");
            var today = new System.DateTime(2017, 9, 25);
            Assert.AreEqual("next weekend", TimexRelativeConvert.ConvertTimexToStringRelative(timex, today));
        }

        [TestMethod]
        public void DataTypes_RelativeConvert_Weekend_last_weekend()
        {
            var timex = new TimexProperty("2017-W39-WE");
            var today = new System.DateTime(2017, 9, 25);
            Assert.AreEqual("last weekend", TimexRelativeConvert.ConvertTimexToStringRelative(timex, today));
        }

        [TestMethod]
        public void DataTypes_RelativeConvert_Month_this_month()
        {
            var timex = new TimexProperty("2017-09");
            var today = new System.DateTime(2017, 9, 25);
            Assert.AreEqual("this month", TimexRelativeConvert.ConvertTimexToStringRelative(timex, today));
        }

        [TestMethod]
        public void DataTypes_RelativeConvert_Month_next_month()
        {
            var timex = new TimexProperty("2017-10");
            var today = new System.DateTime(2017, 9, 25);
            Assert.AreEqual("next month", TimexRelativeConvert.ConvertTimexToStringRelative(timex, today));
        }

        [TestMethod]
        public void DataTypes_RelativeConvert_Month_last_month()
        {
            var timex = new TimexProperty("2017-08");
            var today = new System.DateTime(2017, 9, 25);
            Assert.AreEqual("last month", TimexRelativeConvert.ConvertTimexToStringRelative(timex, today));
        }

        [TestMethod]
        public void DataTypes_RelativeConvert_Year_this_year()
        {
            var timex = new TimexProperty("2017");
            var today = new System.DateTime(2017, 9, 25);
            Assert.AreEqual("this year", TimexRelativeConvert.ConvertTimexToStringRelative(timex, today));
        }

        [TestMethod]
        public void DataTypes_RelativeConvert_Year_next_year()
        {
            var timex = new TimexProperty("2018");
            var today = new System.DateTime(2017, 9, 25);
            Assert.AreEqual("next year", TimexRelativeConvert.ConvertTimexToStringRelative(timex, today));
        }

        [TestMethod]
        public void DataTypes_RelativeConvert_Year_last_year()
        {
            var timex = new TimexProperty("2016");
            var today = new System.DateTime(2017, 9, 25);
            Assert.AreEqual("last year", TimexRelativeConvert.ConvertTimexToStringRelative(timex, today));
        }

        [TestMethod]
        public void DataTypes_RelativeConvert_Season_this_summer()
        {
            var timex = new TimexProperty("2017-SU");
            var today = new System.DateTime(2017, 9, 25);
            Assert.AreEqual("this summer", TimexRelativeConvert.ConvertTimexToStringRelative(timex, today));
        }

        [TestMethod]
        public void DataTypes_RelativeConvert_Season_next_summer()
        {
            var timex = new TimexProperty("2018-SU");
            var today = new System.DateTime(2017, 9, 25);
            Assert.AreEqual("next summer", TimexRelativeConvert.ConvertTimexToStringRelative(timex, today));
        }

        [TestMethod]
        public void DataTypes_RelativeConvert_Season_last_summer()
        {
            var timex = new TimexProperty("2016-SU");
            var today = new System.DateTime(2017, 9, 25);
            Assert.AreEqual("last summer", TimexRelativeConvert.ConvertTimexToStringRelative(timex, today));
        }

        [TestMethod]
        public void DataTypes_RelativeConvert_PartOfDay_this_evening()
        {
            var timex = new TimexProperty("2017-09-25TEV");
            var today = new System.DateTime(2017, 9, 25);
            Assert.AreEqual("this evening", TimexRelativeConvert.ConvertTimexToStringRelative(timex, today));
        }

        [TestMethod]
        public void DataTypes_RelativeConvert_PartOfDay_tonight()
        {
            var timex = new TimexProperty("2017-09-25TNI");
            var today = new System.DateTime(2017, 9, 25);
            Assert.AreEqual("tonight", TimexRelativeConvert.ConvertTimexToStringRelative(timex, today));
        }

        [TestMethod]
        public void DataTypes_RelativeConvert_PartOfDay_tomorrow_morning()
        {
            var timex = new TimexProperty("2017-09-26TMO");
            var today = new System.DateTime(2017, 9, 25);
            Assert.AreEqual("tomorrow morning", TimexRelativeConvert.ConvertTimexToStringRelative(timex, today));
        }

        [TestMethod]
        public void DataTypes_RelativeConvert_PartOfDay_yesterday_afternoon()
        {
            var timex = new TimexProperty("2017-09-24TAF");
            var today = new System.DateTime(2017, 9, 25);
            Assert.AreEqual("yesterday afternoon", TimexRelativeConvert.ConvertTimexToStringRelative(timex, today));
        }

        [TestMethod]
        public void DataTypes_RelativeConvert_PartOfDay_next_wednesday_evening()
        {
            var timex = new TimexProperty("2017-10-04TEV");
            var today = new System.DateTime(2017, 9, 25);
            Assert.AreEqual("next Wednesday evening", TimexRelativeConvert.ConvertTimexToStringRelative(timex, today));
        }
    }
}
