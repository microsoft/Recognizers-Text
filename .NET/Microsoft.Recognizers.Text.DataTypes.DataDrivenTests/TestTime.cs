// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Recognizers.Text.DataTypes.TimexExpression.Tests
{
    [TestClass]
    public class TestTime
    {
        [TestMethod]
        public void DataTypes_Time_Constructor()
        {
            var t = new Time(23, 45, 32);
            Assert.AreEqual(23, t.Hour);
            Assert.AreEqual(45, t.Minute);
            Assert.AreEqual(32, t.Second);
        }

        [TestMethod]
        public void DataTypes_Time_GetTime()
        {
            var t = new Time(23, 45, 32);
            Assert.AreEqual(85532000, t.GetTime());
        }
    }
}
