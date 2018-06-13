// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Recognizers.Text.DataTypes.TimexExpression.Tests
{
    [TestClass]
    public class TestTimexRangeResolve
    {
        [TestMethod]
        public void DataTypes_RangeResolve_daterange_definite()
        {
            var candidates = new[] { "2017-09-28" };
            var constraints = new[] { (new TimexProperty { Year = 2017, Month = 9, DayOfMonth = 27, Days = 2 }).TimexValue };

            var result = TimexRangeResolver.Evaluate(candidates, constraints);

            var r = new HashSet<string>(result.Select(t => t.TimexValue));
            Assert.IsTrue(r.Contains("2017-09-28"));
            Assert.AreEqual(1, r.Count);
        }

        [TestMethod]
        public void DataTypes_RangeResolve_daterange_definite_constrainst_as_timex()
        {
            var candidates = new[] { "2017-09-28" };
            var constraints = new[] { "(2017-09-27,2017-09-29,P2D)" };

            var result = TimexRangeResolver.Evaluate(candidates, constraints);

            var r = new HashSet<string>(result.Select(t => t.TimexValue));
            Assert.IsTrue(r.Contains("2017-09-28"));
            Assert.AreEqual(1, r.Count);
        }

        [TestMethod]
        public void DataTypes_RangeResolve_daterange_month_and_date()
        {
            var candidates = new[] { "XXXX-05-29" };
            var constraints = new[] { (new TimexProperty { Year = 2006, Month = 1, DayOfMonth = 1, Years = 2 }).TimexValue };

            var result = TimexRangeResolver.Evaluate(candidates, constraints);

            var r = new HashSet<string>(result.Select(t => t.TimexValue));
            Assert.IsTrue(r.Contains("2006-05-29"));
            Assert.IsTrue(r.Contains("2007-05-29"));
            Assert.AreEqual(2, r.Count);
        }

        [TestMethod]
        public void DataTypes_RangeResolve_daterange_month_and_date_conditional()
        {
            var candidates = new[] { "XXXX-05-29" };
            var constraints = new[] { "(2006-01-01,2008-06-01,P882D)" };

            var result = TimexRangeResolver.Evaluate(candidates, constraints);

            var r = new HashSet<string>(result.Select(t => t.TimexValue));
            Assert.IsTrue(r.Contains("2006-05-29"));
            Assert.IsTrue(r.Contains("2007-05-29"));
            Assert.IsTrue(r.Contains("2008-05-29"));
            Assert.AreEqual(3, r.Count);
        }

        [TestMethod]
        public void DataTypes_RangeResolve_daterange_Saturdays_in_September()
        {
            var candidates = new[] { "XXXX-WXX-6" };
            var constraints = new[] { "2017-09" };

            var result = TimexRangeResolver.Evaluate(candidates, constraints);

            var r = new HashSet<string>(result.Select(t => t.TimexValue));
            Assert.IsTrue(r.Contains("2017-09-02"));
            Assert.IsTrue(r.Contains("2017-09-09"));
            Assert.IsTrue(r.Contains("2017-09-16"));
            Assert.IsTrue(r.Contains("2017-09-23"));
            Assert.IsTrue(r.Contains("2017-09-30"));
            Assert.AreEqual(5, r.Count);
        }

        [TestMethod]
        public void DataTypes_RangeResolve_daterange_Saturdays_in_September_expressed_as_range()
        {
            var candidates = new[] { "XXXX-WXX-6" };
            var constraints = new[] { "(2017-09-01,2017-10-01,P30D)" };

            var result = TimexRangeResolver.Evaluate(candidates, constraints);

            var r = new HashSet<string>(result.Select(t => t.TimexValue));
            Assert.IsTrue(r.Contains("2017-09-02"));
            Assert.IsTrue(r.Contains("2017-09-09"));
            Assert.IsTrue(r.Contains("2017-09-16"));
            Assert.IsTrue(r.Contains("2017-09-23"));
            Assert.IsTrue(r.Contains("2017-09-30"));
            Assert.AreEqual(5, r.Count);
        }

        [TestMethod]
        public void DataTypes_RangeResolve_daterange_year()
        {
            var candidates = new[] { "XXXX-05-29" };
            var constraints = new[] { "2018" };

            var result = TimexRangeResolver.Evaluate(candidates, constraints);

            var r = new HashSet<string>(result.Select(t => t.TimexValue));
            Assert.IsTrue(r.Contains("2018-05-29"));
            Assert.AreEqual(1, r.Count);
        }

        [TestMethod]
        public void DataTypes_RangeResolve_daterange_expressed_as_range()
        {
            var candidates = new[] { "XXXX-05-29" };
            var constraints = new[] { "(2018-01-01,2019-01-01,P365D)" };

            var result = TimexRangeResolver.Evaluate(candidates, constraints);

            var r = new HashSet<string>(result.Select(t => t.TimexValue));
            Assert.IsTrue(r.Contains("2018-05-29"));
            Assert.AreEqual(1, r.Count);
        }

        [TestMethod]
        public void DataTypes_RangeResolve_daterange_multiple_constraints()
        {
            var candidates = new[] { "XXXX-WXX-3" };
            var constraints = new[] { "(2017-09-01,2017-09-08,P7D)", "(2017-10-01,2017-10-08,P7D)" };

            var result = TimexRangeResolver.Evaluate(candidates, constraints);

            var r = new HashSet<string>(result.Select(t => t.TimexValue));
            Assert.IsTrue(r.Contains("2017-09-06"));
            Assert.IsTrue(r.Contains("2017-10-04"));
            Assert.AreEqual(2, r.Count);
        }

        [TestMethod]
        public void DataTypes_RangeResolve_daterange_multiple_candidates_with_multiple_constraints()
        {
            var candidates = new[]
            {
                "XXXX-WXX-2",
                "XXXX-WXX-4"
            };
            var constraints = new[]
            {
                "(2017-09-01,2017-09-08,P7D)",
                "(2017-10-01,2017-10-08,P7D)"
            };

            var result = TimexRangeResolver.Evaluate(candidates, constraints);

            var r = new HashSet<string>(result.Select(t => t.TimexValue));
            Assert.IsTrue(r.Contains("2017-09-05"));
            Assert.IsTrue(r.Contains("2017-09-07"));
            Assert.IsTrue(r.Contains("2017-10-03"));
            Assert.IsTrue(r.Contains("2017-10-05"));
            Assert.AreEqual(4, r.Count);
        }

        [TestMethod]
        public void DataTypes_RangeResolve_daterange_multiple_overlapping_constraints()
        {
            var candidates = new[] { "XXXX-WXX-3" };
            var constraints = new[] { "(2017-09-03,2017-09-07,P4D)", "(2017-09-01,2017-09-08,P7D)", "(2017-09-01,2017-09-16,P15D)" };

            var result = TimexRangeResolver.Evaluate(candidates, constraints);

            var r = new HashSet<string>(result.Select(t => t.TimexValue));
            Assert.IsTrue(r.Contains("2017-09-06"));
            Assert.AreEqual(1, r.Count);
        }

        [TestMethod]
        public void DataTypes_RangeResolve_timerange_time_within_range()
        {
            var candidates = new[] { "T16" };
            var constraints = new[] { (new TimexProperty { Hour = 14, Hours = 4 }).TimexValue };

            var result = TimexRangeResolver.Evaluate(candidates, constraints);

            var r = new HashSet<string>(result.Select(t => t.TimexValue));
            Assert.IsTrue(r.Contains("T16"));
            Assert.AreEqual(1, r.Count);
        }

        [TestMethod]
        public void DataTypes_RangeResolve_timerange_multiple_times_within_range()
        {
            var candidates = new[] { "T12", "T16", "T16:30", "T17", "T18" };
            var constraints = new[] { (new TimexProperty { Hour = 14, Hours = 4 }).TimexValue };

            var result = TimexRangeResolver.Evaluate(candidates, constraints);

            var r = new HashSet<string>(result.Select(t => t.TimexValue));
            Assert.IsTrue(r.Contains("T16"));
            Assert.IsTrue(r.Contains("T16:30"));
            Assert.IsTrue(r.Contains("T17"));
            Assert.AreEqual(3, r.Count);
        }

        [TestMethod]
        public void DataTypes_RangeResolve_timerange_time_with_overlapping_ranges()
        {
            var constraints = new List<string> { (new TimexProperty { Hour = 16, Hours = 4 }).TimexValue };

            var result1 = TimexRangeResolver.Evaluate(new[] { "T19" }, constraints);

            var r1 = new HashSet<string>(result1.Select(t => t.TimexValue));
            Assert.IsTrue(r1.Contains("T19"));
            Assert.AreEqual(1, r1.Count);

            constraints.Add((new TimexProperty { Hour = 14, Hours = 4 }).TimexValue);

            var result2 = TimexRangeResolver.Evaluate(new[] { "T19" }, constraints);

            var r2 = new HashSet<string>(result2.Select(t => t.TimexValue));
            Assert.IsFalse(r2.Any());

            var result3 = TimexRangeResolver.Evaluate(new[] { "T17" }, constraints);

            var r3 = new HashSet<string>(result3.Select(t => t.TimexValue));
            Assert.IsTrue(r3.Contains("T17"));
            Assert.AreEqual(1, r3.Count);
        }

        [TestMethod]
        public void DataTypes_RangeResolve_multiple_times_with_overlapping_ranges()
        {
            var constraints = new List<string> { (new TimexProperty { Hour = 16, Hours = 4 }).TimexValue };

            var result1 = TimexRangeResolver.Evaluate(new[] { "T19", "T19:30" }, constraints);

            var r1 = new HashSet<string>(result1.Select(t => t.TimexValue));
            Assert.IsTrue(r1.Contains("T19"));
            Assert.IsTrue(r1.Contains("T19:30"));
            Assert.AreEqual(2, r1.Count);

            constraints.Add((new TimexProperty { Hour = 14, Hours = 4 }).TimexValue);

            var result2 = TimexRangeResolver.Evaluate(new[] { "T19", "T19:30" }, constraints);

            var r2 = new HashSet<string>(result2.Select(t => t.TimexValue));
            Assert.IsFalse(r2.Any());

            var result3 = TimexRangeResolver.Evaluate(new[] { "T17", "T17:30", "T19" }, constraints);

            var r3 = new HashSet<string>(result3.Select(t => t.TimexValue));
            Assert.IsTrue(r3.Contains("T17"));
            Assert.IsTrue(r3.Contains("T17:30"));
            Assert.AreEqual(2, r3.Count);
        }

        [TestMethod]
        public void DataTypes_RangeResolve_filter_duplicate()
        {
            var constraints = new List<string> { (new TimexProperty { Hour = 16, Hours = 4 }).TimexValue };

            var result = TimexRangeResolver.Evaluate(new[] { "T16", "T16", "T16" }, constraints);

            var r = new HashSet<string>(result.Select(t => t.TimexValue));
            Assert.IsTrue(r.Contains("T16"));
            Assert.AreEqual(1, r.Count);
        }

        [TestMethod]
        public void DataTypes_RangeResolve_carry_through_time_definite()
        {
            var constraints = new List<string>
            {
                (new TimexProperty { Year = 2017, Month = 9, DayOfMonth = 27, Days = 2 }).TimexValue
            };

            var result = TimexRangeResolver.Evaluate(new[] { "2017-09-28T18:30:01" }, constraints);

            var r = new HashSet<string>(result.Select(t => t.TimexValue));
            Assert.IsTrue(r.Contains("2017-09-28T18:30:01"));
            Assert.AreEqual(1, r.Count);
        }

        [TestMethod]
        public void DataTypes_RangeResolve_carry_through_time_definite_constrainst_expressed_as_timex()
        {
            var constraints = new List<string> { "(2017-09-27,2017-09-29,P2D)" };

            var result = TimexRangeResolver.Evaluate(new[] { "2017-09-28T18:30:01" }, constraints);

            var r = new HashSet<string>(result.Select(t => t.TimexValue));
            Assert.IsTrue(r.Contains("2017-09-28T18:30:01"));
            Assert.AreEqual(1, r.Count);
        }

        [TestMethod]
        public void DataTypes_RangeResolve_carry_through_time_month_and_date()
        {
            var constraints = new List<string>
            {
                (new TimexProperty { Year = 2006, Month = 1, DayOfMonth = 1, Years = 2 }).TimexValue
            };

            var result = TimexRangeResolver.Evaluate(new[] { "XXXX-05-29T19:30" }, constraints);

            var r = new HashSet<string>(result.Select(t => t.TimexValue));
            Assert.IsTrue(r.Contains("2006-05-29T19:30"));
            Assert.IsTrue(r.Contains("2007-05-29T19:30"));
            Assert.AreEqual(2, r.Count);
        }

        [TestMethod]
        public void DataTypes_RangeResolve_carry_through_time_month_and_date_conditional()
        {
            var constraints = new List<string> { "(2006-01-01,2008-06-01,P882D)" };

            var result = TimexRangeResolver.Evaluate(new[] { "XXXX-05-29T19:30" }, constraints);

            var r = new HashSet<string>(result.Select(t => t.TimexValue));
            Assert.IsTrue(r.Contains("2006-05-29T19:30"));
            Assert.IsTrue(r.Contains("2007-05-29T19:30"));
            Assert.IsTrue(r.Contains("2008-05-29T19:30"));
            Assert.AreEqual(3, r.Count);
        }

        [TestMethod]
        public void DataTypes_RangeResolve_carry_through_time_Saturdays_in_September()
        {
            var constraints = new List<string> { "(2017-09-01,2017-10-01,P30D)" };

            var result = TimexRangeResolver.Evaluate(new[] { "XXXX-WXX-6T01:00:00" }, constraints);

            var r = new HashSet<string>(result.Select(t => t.TimexValue));
            Assert.IsTrue(r.Contains("2017-09-02T01"));
            Assert.IsTrue(r.Contains("2017-09-09T01"));
            Assert.IsTrue(r.Contains("2017-09-16T01"));
            Assert.IsTrue(r.Contains("2017-09-23T01"));
            Assert.IsTrue(r.Contains("2017-09-30T01"));
            Assert.AreEqual(5, r.Count);
        }

        [TestMethod]
        public void DataTypes_RangeResolve_carry_through_time_multiple_constraints()
        {
            var constraints = new List<string>
            {
                "(2017-09-01,2017-09-08,P7D)",
                "(2017-10-01,2017-10-08,P7D)"
            };

            var result = TimexRangeResolver.Evaluate(new[] { "XXXX-WXX-3T01:02" }, constraints);

            var r = new HashSet<string>(result.Select(t => t.TimexValue));
            Assert.IsTrue(r.Contains("2017-09-06T01:02"));
            Assert.IsTrue(r.Contains("2017-10-04T01:02"));
            Assert.AreEqual(2, r.Count);
        }

        [TestMethod]
        public void DataTypes_RangeResolve_combined_daterange_and_timerange_next_week_and_any_time()
        {
            var constraints = new List<string>
            {
                (new TimexProperty { Year = 2017, Month = 10, DayOfMonth = 5, Days = 7 }).TimexValue,
                (new TimexProperty { Hour = 0, Minute = 0, Second = 0, Hours = 24 }).TimexValue
            };

            var result = TimexRangeResolver.Evaluate(new[] { "XXXX-WXX-3T04", "XXXX-WXX-3T16" }, constraints);

            var r = new HashSet<string>(result.Select(t => t.TimexValue));
            Assert.IsTrue(r.Contains("2017-10-11T04"));
            Assert.IsTrue(r.Contains("2017-10-11T16"));
            Assert.AreEqual(2, r.Count);
        }

        [TestMethod]
        public void DataTypes_RangeResolve_combined_daterange_and_timerange_next_week_and_business_hours()
        {
            var constraints = new List<string>
            {
                (new TimexProperty { Year = 2017, Month = 10, DayOfMonth = 5, Days = 7 }).TimexValue,
                (new TimexProperty { Hour = 12, Minute = 0, Second = 0, Hours = 8 }).TimexValue
            };

            var result = TimexRangeResolver.Evaluate(new[] { "XXXX-WXX-3T04", "XXXX-WXX-3T16" }, constraints);

            var r = new HashSet<string>(result.Select(t => t.TimexValue));
            Assert.IsTrue(r.Contains("2017-10-11T16"));
            Assert.AreEqual(1, r.Count);
        }

        [TestMethod]
        public void DataTypes_RangeResolve_adding_times_add_specific_time_to_date()
        {
            var constraints = new List<string>
            {
                "2017",
                "T19:30:00",
            };

            var result = TimexRangeResolver.Evaluate(new[] { "XXXX-05-29" }, constraints);

            var r = new HashSet<string>(result.Select(t => t.TimexValue));
            Assert.IsTrue(r.Contains("2017-05-29T19:30"));
            Assert.AreEqual(1, r.Count);
        }

        [TestMethod]
        public void DataTypes_RangeResolve_adding_times_add_specific_time_to_date_2()
        {
            var constraints = new List<string>
            {
                "2017",
                "T19:30:00",
                "T20:01:01"
            };

            var result = TimexRangeResolver.Evaluate(new[] { "XXXX-05-29" }, constraints);

            var r = new HashSet<string>(result.Select(t => t.TimexValue));
            Assert.IsTrue(r.Contains("2017-05-29T19:30"));
            Assert.IsTrue(r.Contains("2017-05-29T20:01:01"));
            Assert.AreEqual(2, r.Count);
        }

        [TestMethod]
        public void DataTypes_RangeResolve_duration_specific_datetime()
        {
            var constraints = new List<string>
            {
                "2017-12-05T19:30:00"
            };

            var result = TimexRangeResolver.Evaluate(new[] { "PT5M" }, constraints);

            var r = new HashSet<string>(result.Select(t => t.TimexValue));
            Assert.IsTrue(r.Contains("2017-12-05T19:35"));
            Assert.AreEqual(1, r.Count);
        }

        [TestMethod]
        public void DataTypes_RangeResolve_duration_specific_time()
        {
            var constraints = new List<string>
            {
                "T19:30:00"
            };

            var result = TimexRangeResolver.Evaluate(new[] { "PT5M" }, constraints);

            var r = new HashSet<string>(result.Select(t => t.TimexValue));
            Assert.IsTrue(r.Contains("T19:35"));
            Assert.AreEqual(1, r.Count);
        }

        [TestMethod]
        public void DataTypes_RangeResolve_duration_no_constraints()
        {
            var constraints = new List<string>
            {
                // empty
            };

            var result = TimexRangeResolver.Evaluate(new[] { "PT5M" }, constraints);

            var r = new HashSet<string>(result.Select(t => t.TimexValue));
            Assert.IsFalse(r.Any());
        }

        [TestMethod]
        public void DataTypes_RangeResolve_duration_no_time_component()
        {
            var constraints = new List<string>
            {
                (new TimexProperty { Year = 2017, Month = 10, DayOfMonth = 5, Days = 7 }).TimexValue
            };

            var result = TimexRangeResolver.Evaluate(new[] { "PT5M" }, constraints);

            var r = new HashSet<string>(result.Select(t => t.TimexValue));
            Assert.IsFalse(r.Any());
        }

        [TestMethod]
        public void DataTypes_RangeResolve_dateranges()
        {
            var candidates = new[] { "XXXX-WXX-7" };
            var constraints = new[]
            {
                "(2018-06-04,2018-06-11,P7D)",  // e.g. this week
                "(2018-06-11,2018-06-18,P7D)",  // e.g. next week
                TimexCreator.Evening
            };

            var result = TimexRangeResolver.Evaluate(candidates, constraints);

            var r = new HashSet<string>(result.Select(t => t.TimexValue));
            Assert.IsTrue(r.Contains("2018-06-10T16"));
            Assert.IsTrue(r.Contains("2018-06-17T16"));
            Assert.AreEqual(2, r.Count);
        }

        [TestMethod]
        public void DataTypes_RangeResolve_dateranges_no_time_constraint()
        {
            var candidates = new[] { "XXXX-WXX-7TEV" };
            var constraints = new[]
            {
                "(2018-06-04,2018-06-11,P7D)",  // e.g. this week
                "(2018-06-11,2018-06-18,P7D)"   // e.g. next week
            };

            var result = TimexRangeResolver.Evaluate(candidates, constraints);

            var r = new HashSet<string>(result.Select(t => t.TimexValue));
            Assert.IsTrue(r.Contains("2018-06-10TEV"));
            Assert.IsTrue(r.Contains("2018-06-17TEV"));
            Assert.AreEqual(2, r.Count);
        }

        [TestMethod]
        public void DataTypes_RangeResolve_dateranges_overlapping_constraint_1()
        {
            var candidates = new[] { "XXXX-WXX-7TEV" };
            var constraints = new[]
            {
                "(2018-06-04,2018-06-11,P7D)",  // e.g. this week
                "(2018-06-11,2018-06-18,P7D)",  // e.g. next week
                "(T18,T22,PT4H)"
            };

            var result = TimexRangeResolver.Evaluate(candidates, constraints);

            var r = new HashSet<string>(result.Select(t => t.TimexValue));
            Assert.IsTrue(r.Contains("2018-06-10T18"));
            Assert.IsTrue(r.Contains("2018-06-17T18"));
            Assert.AreEqual(2, r.Count);
        }

        [TestMethod]
        public void DataTypes_RangeResolve_dateranges_overlapping_constraint_2()
        {
            var candidates = new[] { "XXXX-WXX-7TEV" };
            var constraints = new[]
            {
                "(2018-06-04,2018-06-11,P7D)",  // e.g. this week
                "(2018-06-11,2018-06-18,P7D)",  // e.g. next week
                "(T15,T19,PT4H)"
            };

            var result = TimexRangeResolver.Evaluate(candidates, constraints);

            var r = new HashSet<string>(result.Select(t => t.TimexValue));
            Assert.IsTrue(r.Contains("2018-06-10T16"));
            Assert.IsTrue(r.Contains("2018-06-17T16"));
            Assert.AreEqual(2, r.Count);
        }

        [TestMethod]
        public void DataTypes_RangeResolve_dateranges_non_overlapping_constraint()
        {
            var candidates = new[] { "XXXX-WXX-7TEV" };
            var constraints = new[]
            {
                "(2018-06-04,2018-06-11,P7D)",  // e.g. this week
                "(2018-06-11,2018-06-18,P7D)",  // e.g. next week
                TimexCreator.Morning
            };

            var result = TimexRangeResolver.Evaluate(candidates, constraints);

            Assert.IsFalse(result.Any());
        }

        [TestMethod]
        public void DataTypes_RangeResolve_dateranges_Sunday_Evening()
        {
            var candidates = new[] { "XXXX-WXX-7TEV" };
            var constraints = new[]
            {
                "(2018-06-04,2018-06-11,P7D)",  // e.g. this week
                "(2018-06-11,2018-06-18,P7D)",  // e.g. next week
                TimexCreator.Evening
            };

            var result = TimexRangeResolver.Evaluate(candidates, constraints);

            var r = new HashSet<string>(result.Select(t => t.TimexValue));
            Assert.IsTrue(r.Contains("2018-06-10T16"));
            Assert.IsTrue(r.Contains("2018-06-17T16"));
            Assert.AreEqual(2, r.Count);
        }
    }
}
