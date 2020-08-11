from datatypes_timex_expression import Timex, TimexDateHelpers


class TimexValue:

    @staticmethod
    def date_value(timex_property: Timex):
        if (timex_property.year and timex_property.month and timex_property.day_of_month) is not None:
            return f'{TimexDateHelpers.fixed_format_number(timex_property.year, 4)}-{TimexDateHelpers.fixed_format_number(timex_property.month,2)}-{TimexDateHelpers.fixed_format_number(timex_property.day_of_month,2)}'
        return ''

    @staticmethod
    def time_value(timex_property: Timex):
        if (timex_property.hour and timex_property.minute and timex_property.second) is not None:
            return f'{TimexDateHelpers.fixed_format_number(timex_property.hour, 2)}:{TimexDateHelpers.fixed_format_number(timex_property.minute, 2)}:{TimexDateHelpers.fixed_format_number(timex_property.second, 2)}'

    def datetime_value(self, timex_property: Timex):
        return f'{self.date_value(timex_property)} {self.time_value(timex_property)}'

    @staticmethod
    def duration_value(timex_property: Timex):
        if timex_property.years is not None:
            return str(31536000 * timex_property.years)

        if timex_property.months is not None:
            return str(2592000 * timex_property.months)

        if timex_property.weeks is not None:
            return str(604800 * timex_property.weeks)

        if timex_property.days is not None:
            return str(86400 * timex_property.days)

        if timex_property.hours is not None:
            return str(3600 * timex_property.hours)

        if timex_property.minutes is not None:
            return str(60 * timex_property.minutes)

        if timex_property.seconds is not None:
            return str(timex_property.seconds)

        return ''
