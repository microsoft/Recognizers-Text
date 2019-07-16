class TimexConstraintsHelper:

    def collapse(self, ranges: []):
        while self.inner_collapse(ranges):
            True
        return self.sort_range(ranges)

    def sort_range(self, range: []):
        if range:
            from datatypes_timex_expression import TimeRange
            if isinstance(range[0], TimeRange):
                return TimeRange.sort_range(range)
            from datatypes_timex_expression import DateRange
            if isinstance(range[0], DateRange):
                return DateRange.sort_range(range)
        else:
            return []

    @staticmethod
    def is_overlapping(range1, range2):
        return range1.is_overlapping(range2)

    @staticmethod
    def collapse_overlapping(range1, range2):
        return range1.collapse_overlapping(range2)

    def inner_collapse(self, ranges: []):
        if len(ranges) == 1:
            return False

        for i in range(0, len(ranges), 1):
            r1 = ranges[i]

            for j in range(i+1, len(ranges), 1):
                r2 = ranges[j]
                if self.is_overlapping(r1, r2):
                    del ranges[i:1]
                    del ranges[j-1:1]
                    ranges.append(self.collapse_overlapping(r1, r2))
                    return True

        return False
