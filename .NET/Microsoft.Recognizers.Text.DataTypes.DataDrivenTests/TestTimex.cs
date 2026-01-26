// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Recognizers.Text.DataTypes.TimexExpression.Tests
{
    [TestClass]
    public class TestTimex
    {
        [TestMethod]
        public void DataTypes_Timex_FromDate()
        {
            Assert.AreEqual("2017-12-05", TimexProperty.FromDate(new System.DateTime(2017, 12, 5)).TimexValue);
        }

        [TestMethod]
        public void DataTypes_Timex_FromDateTime()
        {
            Assert.AreEqual("2017-12-05T23:57:35",
                TimexProperty.FromDateTime(new System.DateTime(2017, 12, 5, 23, 57, 35)).TimexValue);
        }

        [TestMethod]
        [DataRow("2017-09-27")]
        [DataRow("XXXX-WXX-3")]
        [DataRow("XXXX-12-05")]
        public void DataTypes_Timex_Roundtrip_Date(string timex)
        {
            Roundtrip(timex);
        }

        [TestMethod]
        [DataRow("T17:30:45")]
        [DataRow("T05:06:07")]
        [DataRow("T17:30")]
        [DataRow("T23")]
        public void DataTypes_Timex_Roundtrip_Time(string timex)
        {
            Roundtrip(timex);
        }

        [TestMethod]
        [DataRow("P50Y")]
        [DataRow("P6M")]
        [DataRow("P3W")]
        [DataRow("P5D")]
        [DataRow("PT16H")]
        [DataRow("PT32M")]
        [DataRow("PT20S")]
        public void DataTypes_Timex_Roundtrip_Duration(string timex)
        {
            Roundtrip(timex);
        }

        [TestMethod]
        [DataRow("PRESENT_REF")]
        public void DataTypes_Timex_Roundtrip_Now(string timex)
        {
            Roundtrip(timex);
        }

        [TestMethod]
        [DataRow("XXXX-WXX-3T04")]
        [DataRow("2017-09-27T11:41:30")]
        public void DataTypes_Timex_Roundtrip_DateTime(string timex)
        {
            Roundtrip(timex);
        }

        [TestMethod]
        [DataRow("2017")]
        [DataRow("SU")]
        [DataRow("2017-WI")]
        [DataRow("2017-09")]
        [DataRow("2017-W37")]
        [DataRow("2017-W37-WE")]
        [DataRow("XXXX-05")]
        public void DataTypes_Timex_Roundtrip_DateRange(string timex)
        {
            Roundtrip(timex);
        }

        [TestMethod]
        [DataRow("(XXXX-WXX-3,XXXX-WXX-6,P3D)")]
        [DataRow("(XXXX-01-01,XXXX-08-05,P216D)")]
        [DataRow("(2017-01-01,2017-08-05,P216D)")]
        [DataRow("(2016-01-01,2016-08-05,P217D)")]
        public void DataTypes_Timex_Roundtrip_DateRange_Start_End_Duration(string timex)
        {
            Roundtrip(timex);
        }

        [TestMethod]
        [DataRow("TEV")]
        public void DataTypes_Timex_Roundtrip_TimeRange(string timex)
        {
            Roundtrip(timex);
        }

        [TestMethod]
        [DataRow("(T16,T19,PT3H)")]
        public void DataTypes_Timex_Roundtrip_TimeRange_Start_End_Duration(string timex)
        {
            Roundtrip(timex);
        }

        [TestMethod]
        [DataRow("2017-09-27TEV")]
        public void DataTypes_Timex_Roundtrip_DateTimeRange(string timex)
        {
            Roundtrip(timex);
        }

        [TestMethod]
        [DataRow("(2017-09-08T21:19:29,2017-09-08T21:24:29,PT5M)")]
        [DataRow("(XXXX-WXX-3T16,XXXX-WXX-6T15,PT71H)")]
        public void DataTypes_Timex_Roundtrip_DateTimeRange_Start_End_Duration(string timex)
        {
            Roundtrip(timex);
        }

        private static void Roundtrip(string timex)
        {
            Assert.AreEqual(timex, new TimexProperty(timex).TimexValue);
        }
    }
}