// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Recognizers.Text.DataTypes.TimexExpression
{
    public static class TimexConstraintsHelper
    {
        public static IEnumerable<TimeRange> Collapse(IEnumerable<TimeRange> ranges)
        {
            var r = ranges.ToList();

            while (InnerCollapse(r)) { }

            r.Sort((a, b) => a.Start.GetTime() - b.Start.GetTime());
            return r;
        }

        public static IEnumerable<DateRange> Collapse(IEnumerable<DateRange> ranges)
        {
            var r = ranges.ToList();

            while (InnerCollapse(r)) { }

            r.Sort((a, b) => System.DateTime.Compare(a.Start, b.Start));
            return r;
        }

        public static bool IsOverlapping(TimeRange r1, TimeRange r2)
        {
            return r1.End.GetTime() > r2.Start.GetTime() && r1.Start.GetTime() <= r2.Start.GetTime() || 
                   r1.Start.GetTime() < r2.End.GetTime() && r1.Start.GetTime() >= r2.Start.GetTime();
        }

        private static TimeRange CollapseOverlapping(TimeRange r1, TimeRange r2) 
        {
            return new TimeRange
            {
                Start = new Time(Math.Max(r1.Start.GetTime(), r2.Start.GetTime())),
                End = new Time(Math.Min(r1.End.GetTime(), r2.End.GetTime()))
            };
        }

        private static bool InnerCollapse(List<TimeRange> ranges)
        {
            if (ranges.Count == 1)
            {
                return false;
            }

            for (int i = 0; i < ranges.Count; i++)
            {
                var r1 = ranges[i];
                for (int j = i + 1; j < ranges.Count; j++)
                {
                    var r2 = ranges[j];
                    if (IsOverlapping(r1, r2))
                    {
                        ranges.RemoveRange(i, 1);
                        ranges.RemoveRange(j - 1, 1);
                        ranges.Add(CollapseOverlapping(r1, r2));
                        return true;
                    }
                }
            }

            return false;
        }
        private static bool IsOverlapping(DateRange r1, DateRange r2)
        {
            return r1.End > r2.Start && r1.Start <= r2.Start || 
                   r1.Start < r2.End && r1.Start >= r2.Start;
        }

        private static DateRange CollapseOverlapping(DateRange r1, DateRange r2)
        {
            return new DateRange
            {
                Start = r1.Start > r2.Start ? r1.Start : r2.Start,
                End = r1.End < r2.End ? r1.End : r2.End
            };
        }

        private static bool InnerCollapse(List<DateRange> ranges)
        {
            if (ranges.Count == 1)
            {
                return false;
            }

            for (int i = 0; i < ranges.Count; i++)
            {
                var r1 = ranges[i];
                for (int j = i + 1; j < ranges.Count; j++)
                {
                    var r2 = ranges[j];
                    if (IsOverlapping(r1, r2))
                    {
                        ranges.RemoveRange(i, 1);
                        ranges.RemoveRange(j - 1, 1);
                        ranges.Add(CollapseOverlapping(r1, r2));
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
