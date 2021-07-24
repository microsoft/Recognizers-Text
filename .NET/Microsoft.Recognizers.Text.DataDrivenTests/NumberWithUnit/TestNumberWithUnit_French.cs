﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Recognizers.Text.DataDrivenTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Tests
{
    [TestClass]
    public class TestNumberWithUnit_French : TestBase
    {
        [NetCoreTestDataSource]
        [TestMethod]
        public void AgeModel(TestModel testSpec)
        {
            TestNumberWithUnit(testSpec);
        }

        [NetCoreTestDataSource]
        [TestMethod]
        public void CurrencyModel(TestModel testSpec)
        {
            TestNumberWithUnit(testSpec);
        }

        [NetCoreTestDataSource]
        [TestMethod]
        public void DimensionModel(TestModel testSpec)
        {
            TestNumberWithUnit(testSpec);
        }

        [NetCoreTestDataSource]
        [TestMethod]
        public void TemperatureModel(TestModel testSpec)
        {
            TestNumberWithUnit(testSpec);
        }
    }
}
