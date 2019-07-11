import copy

from datatypes_timex_expression import Timex, Constants, TimexHelpers, TimeRange, Time, DateRange, TimexDateHelpers
from datatypes_timex_expression.timex_constraints_helper import TimexConstraintsHelper


class TimexRangeResolver:
    @staticmethod
    def evaluate(candidates: [str], constraints: [str]):
        timex_constraints = list(map(lambda tc: Timex(tc), constraints))
        candidates_with_duration_resolved = TimexRangeResolver.resolve_durations(
            candidates, timex_constraints)
        candidates_according_to_date = TimexRangeResolver.resolve_by_date_range_constraints(
            candidates_with_duration_resolved, timex_constraints)
        candidates_with_added_time = TimexRangeResolver.resolve_by_time_constraints(
            candidates_according_to_date, timex_constraints)
        candidates_filtered_by_time = TimexRangeResolver.resolve_by_timerange_constraints(
            candidates_with_added_time, timex_constraints)

        timex_results = list(
            map(lambda tc: Timex(tc), candidates_filtered_by_time))

        return timex_results

    @staticmethod
    def resolve_durations(candidates: [str], constraints: [Timex]):
        results = []
        for candidate in candidates:
            timex = Timex(candidate)

            if Constants.TIMEX_TYPES_DURATION in timex.types:
                resolved_duration = TimexRangeResolver.resolve_duration(timex, constraints)

                for resolved in resolved_duration:
                    results.append(resolved.timex_value())
            else:
                results.append(candidate)
        return results

    @staticmethod
    def resolve_duration(candidate: Timex, constraints: [Timex]):
        results = []

        for constraint in constraints:
            if Constants.TIMEX_TYPES_DATETIME in constraint.types:
                results.append(TimexHelpers.timex_datetime_add(
                    constraint, candidate))
            elif Constants.TIMEX_TYPES_TIME in constraint.types:
                results.append(TimexHelpers.timex_time_add(
                    constraint, candidate))

        return results

    @staticmethod
    def resolve_by_date_range_constraints(candidates: [str], timex_constraints: [Timex]):
        date_range_constraints = list(map(lambda t: TimexHelpers.daterange_from_timex(t),
                                          filter(lambda t: Constants.TIMEX_TYPES_DATERANGE in t.types, timex_constraints)))

        collapsed_date_ranges = TimexConstraintsHelper.collapse(
            TimexConstraintsHelper(), date_range_constraints)

        if not any(collapsed_date_ranges):
            return candidates

        resolution = []

        for timex in candidates:
            resolve_date = TimexRangeResolver.resolve_date(
                Timex(timex), collapsed_date_ranges)
            resolution.extend(resolve_date)

        return TimexRangeResolver.remove_duplicates(resolution)

    @staticmethod
    def resolve_date(timex: Timex, constraints):
        result = []

        for constraint in constraints:
            result.extend(
                TimexRangeResolver.resolve_date_against_constraint(timex, constraint))

        return result

    @staticmethod
    def resolve_by_timerange_constraints(candidates, timex_constraints):
        timerange_contraints = list(map(lambda ti: TimexHelpers.timerange_from_timex(ti),
                                        filter(lambda t: Constants.TIMEX_TYPES_TIMERANGE in t.types, timex_constraints)))
        collapsed_time_ranges = TimexConstraintsHelper.collapse(
            TimexConstraintsHelper(), timerange_contraints)

        if not any(collapsed_time_ranges):
            return candidates

        resolution = []

        for timex in candidates:
            t = Timex(timex)

            if Constants.TIMEX_TYPES_TIMERANGE in t.types:
                r = TimexRangeResolver.resolve_timerage(t, collapsed_time_ranges)
                resolution.extend(r)
            elif Constants.TIMEX_TYPES_TIME in t.types:
                r = TimexRangeResolver.resolve_time(t, collapsed_time_ranges)
                resolution.extend(r)

        return TimexRangeResolver.remove_duplicates(resolution)

    @staticmethod
    def resolve_timerage(timex: Timex, constraints: [TimeRange]):
        candidate = TimexHelpers.timerange_from_timex(timex)
        result = []

        for contraint in constraints:
            if candidate.is_overlapping(contraint):
                start = max(candidate.start.get_time(),
                            contraint.start.get_time())
                time = Time.from_seconds(start)

                resolved = copy.copy(timex)

                resolved.part_of_day = None
                resolved.seconds = None
                resolved.minutes = None
                resolved.hours = None
                resolved.second = time.second
                resolved.minute = time.minute
                resolved.hour = time.hour

                result.append(resolved.timex_value())

        return result

    @staticmethod
    def resolve_time(timex: Timex, constraints: [TimeRange]):
        result = []

        for constraint in constraints:
            result.extend(
                TimexRangeResolver.resolve_time_against_constraint(timex, constraint))

        return result

    @staticmethod
    def remove_duplicates(original):
        a = list(dict.fromkeys(original))
        return a

    @staticmethod
    def resolve_definite_against_constraint(timex, constraint):
        timex_date = TimexHelpers.date_from_timex(timex)
        if constraint.start <= timex_date < constraint.end:
            return [timex.timex_value()]
        return ['']

    @staticmethod
    def resolve_date_against_constraint(timex: Timex, constraint: DateRange):
        if (timex.month and timex.day_of_month) is not None:
            result = []
            year = constraint.start.year
            while year-1 != constraint.end.year:
                t = copy.copy(timex)
                t.year = year
                result.extend(
                    TimexRangeResolver.resolve_definite_against_constraint(t, constraint))
                year = year + 1
            return filter(lambda x: x != '', result)

        if timex.day_of_week is not None:
            day = timex.day_of_week - 1
            dates = TimexDateHelpers.dates_matching_day(
                day, constraint.start, constraint.end)
            result = []

            for date in dates:
                t = copy.copy(timex)
                t.day_of_week = None
                t.year = date.year
                t.month = date.month
                t.day_of_month = date.day
                result.append(t.timex_value())
            return result

        return ['']

    @staticmethod
    def resolve_by_time_constraints(candidates, timex_constraints):
        times = list((map(lambda ti: TimexHelpers.time_from_timex(ti),
                          filter(lambda t: Constants.TIMEX_TYPES_TIME in t.types, timex_constraints))))
        if not any(times):
            return candidates

        resolutions = []

        for timex in list(map(lambda t: Timex(t), candidates)):
            if Constants.TIMEX_TYPES_DATE in timex.types and not (Constants.TIMEX_TYPES_TIME in timex.types):
                for time in times:
                    timex.hour = time.hour
                    timex.minute = time.minute
                    timex.second = time.second
                    resolutions.append(timex.timex_value())
            else:
                resolutions.append(timex.timex_value())

        return TimexRangeResolver.remove_duplicates(resolutions)

    @staticmethod
    def resolve_time_against_constraint(timex, constraint):
        t = Time(timex.hour, timex.minute, timex.second)

        if constraint.start.get_time() <= t.get_time() < constraint.end.get_time():
            return [timex.timex_value()]
        return []
