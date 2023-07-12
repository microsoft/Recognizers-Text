import datetime
from typing import Dict, Optional, List
from recognizers_text import ExtractResult, ResolutionKey
from recognizers_date_time.date_time.parsers import DateTimeParseResult
from recognizers_date_time.date_time import Constants
from recognizers_date_time.date_time.utilities import DateTimeFormatUtil, DateTimeOptionsConfiguration, \
    DateTimeResolutionResult, TimexUtil, DateTimeResolutionKey, DateTimeOptions, TimeTypeConstants


class MergedParserUtil:
    parser_type_name = "datetimeV2"
    @staticmethod
    def is_duration_with_ago_and_later(er: ExtractResult) -> bool:
        return er.meta_data and er.meta_data.is_duration_with_ago_and_later

    @staticmethod
    def combined_mod(original_mod: str, new_mod: str) -> str:
        combined_mod = new_mod
        if original_mod and original_mod == new_mod:
            combined_mod = f'{new_mod}-{original_mod}'
        return combined_mod

    @staticmethod
    def set_parse_result(slot: DateTimeParseResult, has_mod: bool, config: DateTimeOptionsConfiguration):
        slot.value = MergedParserUtil.date_time_resolution(slot, config)

        # Change the type at last for the after or before modes
        slot.type = f"{MergedParserUtil.parser_type_name}." \
                    f"{MergedParserUtil.determine_datetime_type(slot.type, has_mod, config)}"
        return slot

    @staticmethod
    def date_time_resolution(slot: DateTimeParseResult, config: DateTimeOptionsConfiguration) -> \
            Optional[Dict[str, object]]:
        if not slot:
            return None

        resolutions: List[Dict[str, str]] = list()
        res: Dict[str, object] = dict()

        slot_type = slot.type
        timex = slot.timex_str
        val: DateTimeResolutionResult = slot.value

        val_success = getattr(val, 'success', None)

        if not val or not val_success:
            return None

        is_lunar = val.is_lunar
        mod = val.mod
        lst = []

        # Resolve dates list for date periods
        if slot.type == Constants.SYS_DATETIME_DATEPERIOD and val.list:
            for o in val.list:
                lst.append(DateTimeFormatUtil.luis_date_from_datetime(o))
            lst = ",".join(lst)

        #  With modifier, output Type might not be the same with type in resolution result
        # For example, if the resolution type is "date", with modifier the output type should be "daterange"
        type_output = MergedParserUtil.determine_datetime_type(slot_type, True if mod else False, config)
        source_entity = MergedParserUtil.determine_source_entity(slot_type, type_output, val.has_range_changing_mod)
        comment = val.comment

        # The following should be added to res first, since ResolveAmPm requires these fields.
        res[DateTimeResolutionKey.timex] = timex
        res[Constants.COMMENT_KEY] = comment
        res[DateTimeResolutionKey.mod] = mod
        res[ResolutionKey.type] = type_output
        res[DateTimeResolutionKey.is_lunar] = is_lunar

        has_time_zone = False
        if hasattr(val, 'timezone_resolution'):
            if slot_type == Constants.SYS_DATETIME_TIMEZONE:
                #  single timezone
                res[Constants.RESOLVE_TIMEZONE] = {
                    ResolutionKey.value: val.timezone_resolution.value,
                    Constants.UTC_OFFSET_MINS_KEY: str(val.timezone_resolution.utc_offset_mins)
                }
            else:
                # timezone as clarification of datetime
                has_time_zone = True
                res[Constants.SYS_DATETIME_TIMEZONE] = val.timezone_resolution.value
                res[Constants.TIMEZONE_TEXT] = val.timezone_resolution.timezone_text
                res[Constants.UTC_OFFSET_MINS_KEY] = str(val.timezone_resolution.utc_offset_mins)

        past_resolution_str = slot.value.past_resolution
        future_resolution_str = slot.value.future_resolution

        if type_output == Constants.SYS_DATETIME_DATETIMEALT and len(past_resolution_str) > 0:
            type_output = MergedParserUtil.determine_resolution_datetime_type(past_resolution_str)

        resolution_past = MergedParserUtil.generate_resolution(slot_type, past_resolution_str, mod)
        resolution_future = MergedParserUtil.generate_resolution(slot_type, future_resolution_str, mod)

        # If past and future are same, keep only one
        if resolution_future == resolution_past:
            if len(resolution_past) > 0:
                res[Constants.RESOLVE_KEY] = resolution_past
        else:
            if len(resolution_past) > 0:
                res[Constants.RESOLVE_TO_PAST_KEY] = resolution_past
            if len(resolution_future) > 0:
                res[Constants.RESOLVE_TO_FUTURE_KEY] = resolution_future

        # If 'ampm', double our resolution accordingly
        if comment and comment == Constants.COMMENT_AMPM:
            if res[Constants.RESOLVE_KEY]:
                res = MergedParserUtil.resolve_ampm(res, Constants.RESOLVE_KEY)
            else:
                res = MergedParserUtil.resolve_ampm(res, Constants.RESOLVE_TO_PAST_KEY)
                res = MergedParserUtil.resolve_ampm(res, Constants.RESOLVE_TO_FUTURE_KEY)

        #  If WeekOf and in CalendarMode, modify the past part of our resolution
        if (config.options and DateTimeOptions.CALENDAR) != 0 and comment and comment == Constants.COMMENT_WEEK_OF:
            res = MergedParserUtil.resolve_week_of(res, Constants.RESOLVE_TO_PAST_KEY)

        if comment and TimexUtil.has_double_timex(comment):
            res = TimexUtil.process_double_timex(res, Constants.RESOLVE_TO_FUTURE_KEY,
                                                 Constants.RESOLVE_TO_PAST_KEY, timex)

        for p in res.values():
            if isinstance(p, dict):
                value = {}
                value[DateTimeResolutionKey.timex] = timex
                value[DateTimeResolutionKey.mod] = mod
                value[ResolutionKey.type] = type_output
                value[DateTimeResolutionKey.is_lunar] = is_lunar
                value[DateTimeResolutionKey.list] = lst
                value[DateTimeResolutionKey.source_entity] = source_entity

                if has_time_zone:
                    value[Constants.SYS_DATETIME_TIMEZONE] = val.timezone_resolution.value
                    value[Constants.TIMEZONE_TEXT] = val.timezone_resolution.timezone_text
                    value[Constants.UTC_OFFSET_MINS_KEY] = str(val.timezone_resolutions.utc_offset_mins)

                value.update(p)
                resolutions.append(value)

        if resolution_past and resolution_future and len(resolution_past) == 0 and len(resolution_future) == 0 \
                and not val.timezone_resolution:
            not_resolved = {
                DateTimeResolutionKey.timex: timex,
                ResolutionKey.type: type_output,
                ResolutionKey.value: "not resolved"
            }
            resolutions.append(not_resolved)
        return {ResolutionKey.value_set: resolutions}

    @staticmethod
    def determine_datetime_type(dt_type: str, has_mod: bool, config: DateTimeOptionsConfiguration) -> str:
        if (config.options and DateTimeOptions.SPLIT_DATE_AND_TIME) != 0:
            if dt_type == Constants.SYS_DATETIME_DATETIME:
                return Constants.SYS_DATETIME_TIME
        else:
            if has_mod:
                if dt_type == Constants.SYS_DATETIME_DATE:
                    return Constants.SYS_DATETIME_DATEPERIOD
                if dt_type == Constants.SYS_DATETIME_TIME:
                    return Constants.SYS_DATETIME_TIMEPERIOD
                if dt_type == Constants.SYS_DATETIME_DATETIME:
                    return Constants.SYS_DATETIME_DATETIMEPERIOD
        return dt_type

    @staticmethod
    def determine_source_entity(source_type: str, new_type: str, has_mod: bool) -> Optional[str]:
        if not has_mod:
            return None
        if new_type != source_type:
            return Constants.SYS_DATETIME_DATETIMEPOINT
        if new_type == Constants.SYS_DATETIME_DATEPERIOD:
            return Constants.SYS_DATETIME_DATEPERIOD
        return None

    @staticmethod
    def determine_resolution_datetime_type(past_resolutions: Dict[str, str]) -> str:
        if list(past_resolutions.keys())[0] == TimeTypeConstants.START_DATE:
            return Constants.SYS_DATETIME_DATEPERIOD
        elif list(past_resolutions.keys())[0] == TimeTypeConstants.START_DATETIME:
            return Constants.SYS_DATETIME_DATETIMEPERIOD
        elif list(past_resolutions.keys())[0] == TimeTypeConstants.START_TIME:
            return Constants.SYS_DATETIME_TIMEPERIOD
        else:
            return list(past_resolutions.keys())[0].lower()

    @staticmethod
    def generate_resolution(dt_type: str, resolution_dict: Dict[str, str], mod: str) -> Dict[str, str]:
        res: Dict[str, str] = {}
        if dt_type == Constants.SYS_DATETIME_DATETIME:
            res = MergedParserUtil.add_single_datetime_to_resolution(resolution_dict,
                                                                     TimeTypeConstants.DATETIME, mod, res)
        elif dt_type == Constants.SYS_DATETIME_TIME:
            res = MergedParserUtil.add_single_datetime_to_resolution(resolution_dict, TimeTypeConstants.TIME, mod, res)
        elif dt_type == Constants.SYS_DATETIME_DATE:
            res = MergedParserUtil.add_single_datetime_to_resolution(resolution_dict, TimeTypeConstants.DATE, mod, res)
        elif dt_type == Constants.SYS_DATETIME_DURATION:
            if TimeTypeConstants.DURATION in resolution_dict:
                res[ResolutionKey.value] = resolution_dict[TimeTypeConstants.DURATION]
        elif dt_type == Constants.SYS_DATETIME_TIMEPERIOD:
            res = MergedParserUtil.add_period_to_resolution(resolution_dict, TimeTypeConstants.START_TIME,
                                                            TimeTypeConstants.END_TIME, mod, res)
        elif dt_type == Constants.SYS_DATETIME_DATEPERIOD:
            res = MergedParserUtil.add_period_to_resolution(resolution_dict, TimeTypeConstants.START_DATE,
                                                            TimeTypeConstants.END_DATE, mod, res)
        elif dt_type == Constants.SYS_DATETIME_DATETIMEPERIOD:
            res = MergedParserUtil.add_period_to_resolution(resolution_dict, TimeTypeConstants.START_TIME,
                                                            TimeTypeConstants.END_TIME, mod, res)
        elif dt_type == Constants.SYS_DATETIME_DATETIMEALT:
            # for a period
            if len(resolution_dict) > 2 or mod:
                res = MergedParserUtil.add_alt_period_to_resolution(resolution_dict, mod, res)
            else:
                # Fot a datetime point
                res = MergedParserUtil.add_alt_single_datetime_to_resolution(resolution_dict, mod, res)
        return res

    @staticmethod
    def add_single_datetime_to_resolution(resolution_dict: Dict[str, str], dt_type: str, mod: str,
                                          res: Dict[str, str]) -> Dict[str, str]:
        # If an "invalid" Date or DateTime is extracted, it should not have an assigned resolution.
        # Only valid entities should pass this condition.
        if dt_type in resolution_dict:
            if mod:
                if mod.startswith(Constants.BEFORE_MOD):
                    res[DateTimeResolutionKey.end] = resolution_dict[dt_type]
                    return res
                if mod.startswith(Constants.AFTER_MOD):
                    res[DateTimeResolutionKey.start] = resolution_dict[dt_type]
                    return res
                if mod.startswith(Constants.SINCE_MOD):
                    res[DateTimeResolutionKey.start] = resolution_dict[dt_type]
                    return res
                if mod.startswith(Constants.UNTIL_MOD):
                    res[DateTimeResolutionKey.end] = resolution_dict[dt_type]
                    return res
            res[ResolutionKey.value] = resolution_dict[dt_type]
        return res

    @staticmethod
    def add_period_to_resolution(resolution_dict: Dict[str, str], start_type: str, end_type: str, mod: str,
                                 res: Dict[str, str]):
        start = ""
        end = ""
        if start_type in resolution_dict:
            start = resolution_dict[start_type]
            if start == Constants.INVALID_DATE_STRING:
                return res
        if end_type in resolution_dict:
            end = resolution_dict[end_type]
            if end == Constants.INVALID_DATE_STRING:
                return res
        if mod:
            # For the 'before' mod
            # 1. Cases like "Before December", the start of the period should be the end of the new period, not the start
            # (but not for cases like "Before the end of December")
            # 2. Cases like "More than 3 days before today", the date point should be the end of the new period
            if mod.startswith(Constants.BEFORE_MOD):
                if start and end and mod.endswith(TimeTypeConstants.LATE_MOD):
                    res[DateTimeResolutionKey.end] = start
                else:
                    res[DateTimeResolutionKey.end] = end
                return res
            # For the 'after' mod
            # 1. Cases like "After January", the end of the period should be the start of the new period, not the end
            # (but not for cases like "After the beginning of January")
            # 2. Cases like "More than 3 days after today", the date point should be the start of the new period
            if mod.startswith(Constants.AFTER_MOD):
                if start and end and mod.endswith(TimeTypeConstants.EARLY_MOD):
                    res[DateTimeResolutionKey.start] = end
                else:
                    res[DateTimeResolutionKey.start] = start
                return res
            # For the 'since' mod, the start of the period should be the start of the new period, not the end
            if mod.startswith(Constants.SINCE_MOD):
                res[DateTimeResolutionKey.start] = start
                return res
            # For the 'until' mod, the end of the period should be the end of the new period, not the start
            if mod.startswith(Constants.UNTIL_MOD):
                res[DateTimeResolutionKey.end] = end
                return res

            return res

        if not MergedParserUtil.are_unresolved_dates(start, end):
            res[DateTimeResolutionKey.start] = start
            res[DateTimeResolutionKey.end] = end
            # Preserving any present timex values. Useful for Holiday weekend where the timex is known during parsing.
            if DateTimeResolutionKey.timex in resolution_dict:
                res[DateTimeResolutionKey.timex] = resolution_dict[DateTimeResolutionKey.timex]

            return res

    @staticmethod
    def are_unresolved_dates(start_date: str, end_date: str) -> bool:
        if not start_date or not end_date:
            return True
        return False

    @staticmethod
    def add_alt_period_to_resolution(resolution_dict: Dict[str, str], mod: str, res: Dict[str, str]) -> Dict[str, str]:
        if TimeTypeConstants.START_DATETIME in resolution_dict or TimeTypeConstants.END_DATETIME in resolution_dict:
            return MergedParserUtil.add_period_to_resolution(resolution_dict, TimeTypeConstants.START_DATETIME,
                                                             TimeTypeConstants.END_DATETIME, mod, res)
        elif TimeTypeConstants.START_DATE in resolution_dict or TimeTypeConstants.END_DATE in resolution_dict:
            return MergedParserUtil.add_period_to_resolution(resolution_dict, TimeTypeConstants.START_DATE,
                                                             TimeTypeConstants.END_DATE, mod, res)
        elif TimeTypeConstants.START_TIME in resolution_dict or TimeTypeConstants.END_TIME in resolution_dict:
            return MergedParserUtil.add_period_to_resolution(resolution_dict, TimeTypeConstants.START_TIME,
                                                             TimeTypeConstants.END_TIME, mod, res)
        return res

    @staticmethod
    def add_alt_single_datetime_to_resolution(resolution_dict: Dict[str, str], mod: str,
                                              res: Dict[str, str]) -> Dict[str, str]:
        if TimeTypeConstants.DATE in resolution_dict:
            return MergedParserUtil.add_single_datetime_to_resolution(resolution_dict, TimeTypeConstants.DATE, mod, res)
        elif TimeTypeConstants.DATETIME in resolution_dict:
            return MergedParserUtil.add_single_datetime_to_resolution(resolution_dict, TimeTypeConstants.DATETIME, mod, res)
        elif TimeTypeConstants.TIME in resolution_dict:
            return MergedParserUtil.add_single_datetime_to_resolution(resolution_dict, TimeTypeConstants.TIME, mod, res)
        return res

    @staticmethod
    def resolve_ampm(resolution_dict: Dict, key_name: str) -> Dict:
        if key_name in resolution_dict:
            resolution: Dict[str, str] = resolution_dict[key_name]
            resolution_pm: Dict[str, str] = dict()

            if DateTimeResolutionKey.timex not in resolution_dict:
                return resolution_dict
            timex = resolution_dict[DateTimeResolutionKey.timex]
            resolution_dict.pop(key_name)
            resolution_dict[key_name + 'Am'] = resolution

            if resolution_dict[ResolutionKey.type] == Constants.SYS_DATETIME_TIME:

                resolution_pm[ResolutionKey.value] = DateTimeFormatUtil.to_pm(resolution[ResolutionKey.value])
                resolution_pm[DateTimeResolutionKey.timex] = DateTimeFormatUtil.to_pm(timex)

            elif resolution_dict[ResolutionKey.type] == Constants.SYS_DATETIME_DATETIME:
                split = resolution[ResolutionKey.value].split(' ')
                resolution_pm[ResolutionKey.value] = split[0] + ' ' + DateTimeFormatUtil.to_pm((split[1]))
                resolution_pm[DateTimeResolutionKey.timex] = DateTimeFormatUtil.all_str_to_pm(timex)

            elif resolution_dict[ResolutionKey.type] == Constants.SYS_DATETIME_TIMEPERIOD:
                if DateTimeResolutionKey.start in resolution:
                    resolution_pm[DateTimeResolutionKey.start] = DateTimeFormatUtil.to_pm(
                        resolution[DateTimeResolutionKey.start])
                if DateTimeResolutionKey.end in resolution:
                    resolution_pm[DateTimeResolutionKey.end] = DateTimeFormatUtil.to_pm(
                        resolution[DateTimeResolutionKey.end])
                if DateTimeResolutionKey.value in resolution:
                    resolution_pm[ResolutionKey.value] = DateTimeFormatUtil.to_pm(resolution[ResolutionKey.value])
                resolution_pm[DateTimeResolutionKey.timex] = DateTimeFormatUtil.all_str_to_pm(timex)

            elif resolution_dict[ResolutionKey.type] == Constants.SYS_DATETIME_DATETIMEPERIOD:
                if DateTimeResolutionKey.start in resolution:
                    start = datetime.datetime.strptime(resolution[DateTimeResolutionKey.start], '%H')
                    if start.hour == Constants.HALF_DAY_HOUR_COUNT:
                        start -= datetime.timedelta(hours=Constants.HALF_DAY_HOUR_COUNT)
                    else:
                        start += datetime.timedelta(hours=Constants.HALF_DAY_HOUR_COUNT)
                if DateTimeResolutionKey.end in resolution:
                    end = datetime.datetime.strptime(resolution[DateTimeResolutionKey.end], '%H')
                    if end.hour == Constants.HALF_DAY_HOUR_COUNT:
                        end -= datetime.timedelta(hours=Constants.HALF_DAY_HOUR_COUNT)
                    else:
                        end += datetime.timedelta(hours=Constants.HALF_DAY_HOUR_COUNT)

            resolution_dict[key_name + "Pm"] = resolution_pm
        return resolution_dict

    @staticmethod
    def resolve_week_of(resolution_dict: Dict, key_name: str) -> Dict:
        if key_name in resolution_dict:
            resolution: Dict[str, str] = resolution_dict[key_name]

            monday = datetime.datetime.strptime(resolution[DateTimeResolutionKey.start], '%A')
            resolution[DateTimeResolutionKey.timex] = DateTimeFormatUtil.to_iso_week_timex(monday)

            resolution_dict.pop(key_name)
            resolution_dict[key_name] = resolution
        return resolution_dict






