// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Recognizers.Text.DataTypes.TimexExpression
{
    public class TimexRangeResolver
    {
        public static List<TimexProperty> Evaluate(IEnumerable<string> candidates, IEnumerable<string> constraints)
        {
            var timexConstraints = constraints.Select((x) => { return new TimexProperty(x); });
            var candidatesWithDurationsResolved = ResolveDurations(candidates, timexConstraints);
            var candidatesAccordingToDate = ResolveByDateRangeConstraints(candidatesWithDurationsResolved, timexConstraints);
            var candidatesWithAddedTime = ResolveByTimeConstraints(candidatesAccordingToDate, timexConstraints);
            var candidatesFilteredByTime = ResolveByTimeRangeConstraints(candidatesWithAddedTime, timexConstraints);

            var timexResults = candidatesFilteredByTime
                .Select((x) => { return new TimexProperty(x); })
                .ToList();

            return timexResults;
        }
        private static List<string> ResolveDurations(IEnumerable<string> candidates, IEnumerable<TimexProperty> constraints)
        {
            var results = new List<string>();
            foreach (var candidate in candidates)
            {
                var timex = new TimexProperty(candidate);
                if (timex.Types.Contains(Constants.TimexTypes.Duration))
                {
                    var r = ResolveDuration(timex, constraints);
                    foreach (var resolved in r)
                    {
                        results.Add(resolved.TimexValue);
                    }
                }
                else
                {
                    results.Add(candidate);
                }
            }

            return results;
        }

        private static List<TimexProperty> ResolveDuration(TimexProperty candidate, IEnumerable<TimexProperty> constraints)
        {
            var results = new List<TimexProperty>();
            foreach (var constraint in constraints)
            {
                if (constraint.Types.Contains(Constants.TimexTypes.DateTime))
                {
                    results.Add(TimexHelpers.TimexDateTimeAdd(constraint, candidate));
                }
                else if (constraint.Types.Contains(Constants.TimexTypes.Time))
                {
                    results.Add(TimexHelpers.TimexTimeAdd(constraint, candidate));
                }
            }

            return results;
        }

        private static IEnumerable<string> ResolveByDateRangeConstraints(IEnumerable<string> candidates, IEnumerable<TimexProperty> timexConstraints)
        {
            var dateRangeconstraints = timexConstraints
                .Where((timex) => {
                    return timex.Types.Contains(Constants.TimexTypes.DateRange);
                })
                .Select((timex) => {
                    return TimexHelpers.DateRangeFromTimex(timex);
                });

            var collapsedDateRanges = TimexConstraintsHelper.Collapse(dateRangeconstraints);

            if (!collapsedDateRanges.Any())
            {
                return candidates;
            }

            var resolution = new List<string>();
            foreach (var timex in candidates)
            {
                var r = ResolveDate(new TimexProperty(timex), collapsedDateRanges);
                resolution.AddRange(r);        
            }

            return RemoveDuplicates(resolution);
        }

        private static IEnumerable<string> ResolveDate(TimexProperty timex, IEnumerable<DateRange> constraints)
        {
            var result = new List<string>();
            foreach (var constraint in constraints)
            {
                result.AddRange(ResolveDateAgainstConstraint(timex, constraint));
            }

            return result;
        }

        private static IEnumerable<string> ResolveByTimeRangeConstraints(IEnumerable<string> candidates, IEnumerable<TimexProperty> timexConstraints)
        {

            var timeRangeConstraints = timexConstraints
                .Where((timex) => {
                    return timex.Types.Contains(Constants.TimexTypes.TimeRange);
                })
                .Select((timex) => {
                    return TimexHelpers.TimeRangeFromTimex(timex);
                });

            var collapsedTimeRanges = TimexConstraintsHelper.Collapse(timeRangeConstraints);

            if (!collapsedTimeRanges.Any())
            {
                return candidates;
            }

            var resolution = new List<string>();
            foreach (var timex in candidates)
            {
                var t = new TimexProperty(timex);
                if (t.Types.Contains(Constants.TimexTypes.TimeRange))
                {
                    var r = ResolveTimeRange(t, collapsedTimeRanges);
                    resolution.AddRange(r);
                }
                else if (t.Types.Contains(Constants.TimexTypes.Time))
                {
                    var r = ResolveTime(t, collapsedTimeRanges);
                    resolution.AddRange(r);
                }
            }

            return RemoveDuplicates(resolution);
        }
        
        private static IEnumerable<string> ResolveTimeRange(TimexProperty timex, IEnumerable<TimeRange> constraints)
        {
            var candidate = TimexHelpers.TimeRangeFromTimex(timex);

            var result = new List<string>();
            foreach (var constraint in constraints)
            {
                if (TimexConstraintsHelper.IsOverlapping(candidate, constraint))
                {
                    var start = Math.Max(candidate.Start.GetTime(), constraint.Start.GetTime());
                    var time = new Time(start);

                    // TODO: consider a method on TimexProperty to do this clone/overwrite pattern
                    var resolved = timex.Clone();
                    resolved.PartOfDay = null;
                    resolved.Seconds = null;
                    resolved.Minutes = null;
                    resolved.Hours = null;
                    resolved.Second = time.Second;
                    resolved.Minute = time.Minute;
                    resolved.Hour = time.Hour;

                    result.Add(resolved.TimexValue);
                }
            }

            return result;
        }

        private static IEnumerable<string> ResolveTime(TimexProperty timex, IEnumerable<TimeRange> constraints)
        {
            var result = new List<string>();
            foreach (var constraint in constraints)
            {
                result.AddRange(ResolveTimeAgainstConstraint(timex, constraint));
            }

            return result;
        }

        private static IEnumerable<string> ResolveTimeAgainstConstraint(TimexProperty timex, TimeRange constraint)
        {
            var t = new Time(timex.Hour.Value, timex.Minute.Value, timex.Second.Value);
            if (t.GetTime() >= constraint.Start.GetTime() && t.GetTime() < constraint.End.GetTime())
            {
                return new [] { timex.TimexValue };
            }

            return Enumerable.Empty<string>();
        }

        private static IEnumerable<string> RemoveDuplicates(IEnumerable<string> original)
        {
            return new HashSet<string>(original);
        }

        private static IEnumerable<string> ResolveDefiniteAgainstConstraint(TimexProperty timex, DateRange constraint)
        {
            var timexDate = TimexHelpers.DateFromTimex(timex);
            if (timexDate >= constraint.Start && timexDate < constraint.End)
            {
                return new[] { timex.TimexValue };
            }

            return Enumerable.Empty<string>();
        }

        private static IEnumerable<string> ResolveDateAgainstConstraint(TimexProperty timex, DateRange constraint)
        {
            if (timex.Month != null && timex.DayOfMonth != null)
            {
                var result = new List<string>();
                for (var year = constraint.Start.Year; year <= constraint.End.Year; year++)
                {
                    var t = timex.Clone();
                    t.Year = year;
                    result.AddRange(ResolveDefiniteAgainstConstraint(t, constraint));
                }

                return result;
            }

            if (timex.DayOfWeek != null)
            {
                // convert between ISO day of week and .NET day of week
                var day = timex.DayOfWeek == 7 ? DayOfWeek.Sunday : (DayOfWeek)timex.DayOfWeek;
                var dates = TimexDateHelpers.DatesMatchingDay(day, constraint.Start, constraint.End);
                var result = new List<string>();

                foreach (var d in dates)
                {
                    var t = timex.Clone();
                    t.DayOfWeek = null;
                    t.Year = d.Year;
                    t.Month = d.Month;
                    t.DayOfMonth = d.Day;
                    result.Add(t.TimexValue);
                }

                return result;
            }

            return Enumerable.Empty<string>();
        }

        private static IEnumerable<string> ResolveByTimeConstraints(IEnumerable<string> candidates, IEnumerable<TimexProperty> timexConstraints)
        {
            var times = timexConstraints
                .Where((timex) => {
                    return timex.Types.Contains(Constants.TimexTypes.Time);
                })
                .Select((timex) => {
                    return TimexHelpers.TimeFromTimex(timex);
                });

            if (!times.Any()) {
                return candidates;
            }

            var resolution = new List<string>();
            foreach (var timex in candidates.Select(t => new TimexProperty(t))) 
            {
                if (timex.Types.Contains(Constants.TimexTypes.Date) && !timex.Types.Contains(Constants.TimexTypes.Time))
                {
                    foreach (var time in times)
                    {
                        timex.Hour = time.Hour;
                        timex.Minute = time.Minute;
                        timex.Second = time.Second;
                        resolution.Add(timex.TimexValue);
                    }
                }
                else
                {
                    resolution.Add(timex.TimexValue);
                }
            }

            return RemoveDuplicates(resolution);
        }
    }
}
