from typing import Optional, Dict, List
from datetime import datetime

from recognizers_text import ExtractResult

from ..utilities import DateTimeOptions, DateTimeResolutionResult
from ..constants import Constants, TimeTypeConstants
from ..parsers import DateTimeParseResult
from ..base_merged import BaseMergedParser
from .merged_parser_config import ChineseMergedParserConfiguration

class ChineseMergedParser(BaseMergedParser):
    def __init__(self):
        super().__init__(ChineseMergedParserConfiguration(), DateTimeOptions.NONE)

    def parse(self, source: ExtractResult, reference: datetime = None) -> Optional[DateTimeParseResult]:
        if not reference:
            reference = datetime.now()

        result = DateTimeParseResult()

        # push, save teh MOD string
        before_match = self.config.before_regex.match(source.text)
        after_match = self.config.after_regex.match(source.text)
        mod_str = ''
        has_before = False
        has_after = False

        if before_match:
            has_before = True
            result.start += before_match.start()
            result.length -= len(before_match.group())
            result.text = result.text[before_match.start():]
            mod_str = before_match.group()
        elif after_match:
            has_after = True
            result.start += after_match.start()
            result.length -= len(after_match.group())
            result.text = result.text[after_match.start():]
            mod_str = after_match.group()

        if source.type == Constants.SYS_DATETIME_DATE:
            result = self.config.date_parser.parse(source, reference)
            if not result.value:
                result = self.config.holiday_parser.parse(source, reference)
        elif source.type == Constants.SYS_DATETIME_TIME:
            result = self.config.time_parser.parse(source, reference)
        elif source.type == Constants.SYS_DATETIME_DATETIME:
            result = self.config.date_time_parser.parse(source, reference)
        elif source.type == Constants.SYS_DATETIME_DATEPERIOD:
            result = self.config.date_period_parser.parse(source, reference)
        elif source.type == Constants.SYS_DATETIME_TIMEPERIOD:
            result = self.config.time_period_parser.parse(source, reference)
        elif source.type == Constants.SYS_DATETIME_DATETIMEPERIOD:
            result = self.config.date_time_period_parser.parse(source, reference)
        elif source.type == Constants.SYS_DATETIME_DURATION:
            result = self.config.duration_parser.parse(source, reference)
        elif source.type == Constants.SYS_DATETIME_SET:
            result = self.config.set_parser.parse(source, reference)
        else:
            return None

        # pop, restore the MOD string
        if has_before and result.value:
            result.length += len(mod_str)
            result.start -= len(mod_str)
            result.text = mod_str + result.text
            value: DateTimeResolutionResult = result.value
            value.mod = TimeTypeConstants.BEFORE_MOD
            result.value = value

        if has_after and result.value:
            result.length += len(mod_str)
            result.start -= len(mod_str)
            result.text = mod_str + result.text
            value: DateTimeResolutionResult = result.value
            value.mod = TimeTypeConstants.AFTER_MOD
            result.value = value

        result.value = self._date_time_resolution(result, has_before, has_after)

        result.type = self.parser_type_name + '.' + self._determine_date_time_types(result.type, has_before, has_after)

        return result

    def _date_time_resolution(self, slot: DateTimeParseResult, has_before: bool = False, has_after: bool = False, has_since: bool = False) -> Dict[str, List[Dict[str, str]]]:
        if not slot:
            return None

        result: Dict[str, any] = dict()
        resolutions: List[Dict[str, str]] = list()

        d_type = slot.type
        output_type = self._determine_date_time_types(d_type, has_before, has_after)
        timex = slot.timex_str

        value: DateTimeResolutionResult = slot.value
        if not value:
            return None

        is_lunar = value.is_lunar
        mod = value.mod
        comment = value.comment

        self._add_resolution_fields_any(result, Constants.TimexKey, timex)
        self._add_resolution_fields_any(result, Constants.CommentKey, comment)
        self._add_resolution_fields_any(result, Constants.ModKey, mod)
        self._add_resolution_fields_any(result, Constants.TypeKey, output_type)
        # self._add_resolution_fields_any(result, Constants.IsLunarKey, str(is_lunar).lower() if is_lunar else '')

        future_resolution = value.future_resolution
        past_resolution = value.past_resolution

        future = self._generate_from_resolution(d_type, future_resolution, mod)
        past = self._generate_from_resolution(d_type, past_resolution, mod)

        future_values = sorted(future.values())
        past_values = sorted(past.values())
        intersect_values = [i for i, j in zip(future_values, past_values) if i == j]

        if len(intersect_values) == len(past_values) and len(intersect_values) == len(future_values):
            if past_values:
                self._add_resolution_fields_any(result, Constants.ResolveKey, past)
        else:
            if past_values:
                self._add_resolution_fields_any(result, Constants.ResolveToPastKey, past)
            if future_resolution:
                self._add_resolution_fields_any(result, Constants.ResolveToFutureKey, future)

        if comment == 'ampm':
            if 'resolve' in result:
                self._resolve_ampm(result, 'resolve')
            else:
                self._resolve_ampm(result, 'resolveToPast')
                self._resolve_ampm(result, 'resolveToFuture')

        if is_lunar:
            self._add_resolution_fields_any(result, Constants.IsLunarKey, is_lunar)

        for value in result.values():
            if isinstance(value, dict):
                new_values = {}
                self._add_resolution_fields(new_values, Constants.TimexKey, timex)
                self._add_resolution_fields(new_values, Constants.ModKey, mod)
                self._add_resolution_fields(new_values, Constants.TypeKey, output_type)

                for inner_key in value:
                    new_values[inner_key] = value[inner_key]

                resolutions.append(new_values)

        if not past and not future:
            dummy = {}
            dummy['timex'] = timex
            dummy['type'] = output_type
            dummy['value'] = 'not resolved'
            resolutions.append(dummy)

        return {'values': resolutions}

    def _determine_date_time_types(self, d_type: str, before: bool = False, after: bool = False, since: bool = False) -> str:
        if before or after or since:
            if d_type == Constants.SYS_DATETIME_DATE:
                return Constants.SYS_DATETIME_DATEPERIOD
            if d_type == Constants.SYS_DATETIME_TIME:
                return Constants.SYS_DATETIME_TIMEPERIOD
            if d_type == Constants.SYS_DATETIME_DATETIME:
                return Constants.SYS_DATETIME_DATETIMEPERIOD

        return d_type
