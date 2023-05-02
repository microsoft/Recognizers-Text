from recognizers_date_time.date_time.constants import Constants


class DurationParsingUtil:

    @staticmethod
    def is_time_duration_unit(uni_str: str):

        if uni_str == Constants.UNIT_H:
            result = True
        elif uni_str == Constants.UNIT_M:
            result = True
        elif uni_str == Constants.UNIT_S:
            result = True
        else:
            result = False

        return result
