// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Recognizers.Text.DataTypes.TimexExpression.Tests
{
    [TestClass]
    public class TestTimexToString
    {
        [TestMethod]
        public void DataTypes_Timex_ToNaturalLanguage()
        {
            var today = new System.DateTime(2017, 10, 16);
            Assert.AreEqual("tomorrow", new TimexProperty("2017-10-17").ToNaturalLanguage(today));
        }

        [TestMethod]
        public void DataTypes_Timex_FromTime()
        {
            Assert.AreEqual("T23:59:30", TimexProperty.FromTime(new Time(23, 59, 30)).TimexValue);
        }

        [TestMethod]
        [DataRow("XXXX-05-05", "5th May")]
        public void DataTypes_Timex_ToString(string timex, string expected)
        {
            Assert.AreEqual(expected, new TimexProperty(timex).ToString());
        }

        [TestMethod]
        [DataRow("2022-03-11", "11th March 2022")]
        [DataRow("2022-03-12", "12th March 2022")]
        [DataRow("2022-03-13", "13th March 2022")]
        public void DataTypes_Timex_FromDateTime_ToString(string timex, string expected)
        {
            Assert.AreEqual(expected, new TimexProperty(timex).ToString());
        }

        [TestMethod]
        [DataRow("(2022-03-15T16,2022-03-15T18,PT2H)", "15th March 2022 4PM to 6PM")]

        [DataRow("(2022-03-15T16,2022-03-17T16,PT50H)", "15th March 2022 4PM to 17th March 2022 6PM")]

        [DataRow("(2022-03-15T16,2022-03-17T16,PT50H)", "15th March 2022 4PM to 17th March 2022 6PM")]
        public void DataTypes_Timex_FromDateTimeRange_ToString(string timex, string expected)
        {
            Assert.AreEqual(expected, new TimexProperty(timex).ToString());
        }

        [TestMethod]
        [DataRow("(2022-SU,2022-SU,PT3H40M)", "summer 2022 3 hours 40 minutes")]
        [DataRow("(2022-W02-6,2022-W03-6,P7D)", "Saturday week 2 2022 to Saturday week 3 2022")]
        [DataRow("(2022-W02-WE,2022-W03-WE,P7D)", "weekend of week 2 2022 to weekend of week 3 2022")]
        public void DataTypes_Timex_FromDateRange_ToString(string timex, string expected)
        {
            Assert.AreEqual(expected, new TimexProperty(timex).ToString());
        }

        [TestMethod]
        [DataRow("PT2H50M", "2 hours 50 minutes")]
        public void DataTypes_Timex_FromDuration_ToString(string timex, string expected)
        {
            Assert.AreEqual(expected, new TimexProperty(timex).ToString());
        }

        [TestMethod]
        [DataRow("PT2H50M", "every 2 hours 50 minutes")]
        [DataRow("P2D", "every 2 days")]
        [DataRow("(2022-SU,2022-SU,PT3H40M)", "every 3 hours 40 minutes")]
        public void DataTypes_TimexSet_FromDuration_ToString(string timex, string expected)
        {
            Assert.AreEqual(expected, new TimexSet(timex).ToString());
        }
    }
}