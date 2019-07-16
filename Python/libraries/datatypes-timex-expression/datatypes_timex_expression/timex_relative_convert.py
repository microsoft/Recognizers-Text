
class TimexRelativeConvert:

    @staticmethod
    def convert_timex_to_string_relative(timex, reference_date):
        from .english import english_convert_timex_to_string_relative
        return english_convert_timex_to_string_relative(timex=timex, date=reference_date)
