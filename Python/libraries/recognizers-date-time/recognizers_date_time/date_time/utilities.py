from abc import ABC, abstractmethod
from typing import List, Dict, Pattern, Union
from datetime import datetime
import regex

from recognizers_text.extractor import ExtractResult
from recognizers_text.utilities import RegExpUtility

class Token:
    def __init__(self, start: int, end: int):
        self.start: int = start
        self.end: int = end

    @property
    def length(self) -> int:
        return self.end - self.start

def merge_all_tokens(tokens: List[Token], source: str, extractor_name: str) -> List[ExtractResult]:
    merged_tokens: List[Token] = list()
    tokens_ = sorted(filter(None, tokens), key=lambda x: x.start, reverse=True)
    for token in tokens_:
        add = True
        for index, m_token in enumerate(merged_tokens):
            if not add:
                break
            if token.start >= m_token.start and token.end <= m_token.end:
                add = False
            if token.start > m_token.start and token.start < m_token.end:
                add = False
            if token.start <= m_token.start and token.end >= m_token.end:
                add = False
                merged_tokens[index] = token
        if add:
            merged_tokens.append(token)
    result: List[ExtractResult] = list(map(lambda x: __token_to_result(x, source, extractor_name), merged_tokens))
    return result

def __token_to_result(token: Token, source: str, name: str) -> ExtractResult:
    result: ExtractResult = ExtractResult()
    result.start = token.start
    result.length = token.length
    result.text = source[token.start:token.end]
    result.type = name
    return result

class DateTimeResolutionResult:
    def __init__(self):
        self.success: bool = False
        self.timex: str
        self.is_lunar: bool
        self.mod: str
        self.comment: str
        self.future_resolution: Dict[str, str] = dict()
        self.past_resolution: Dict[str, str] = dict()
        self.future_value: object
        self.past_value: object
        self.sub_date_time_entities: List[object] = list()

class DateTimeUtilityConfiguration(ABC):
    @property
    @abstractmethod
    def ago_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def later_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def in_connector_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def range_unit_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def am_desc_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def pm_desc__regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def am_pm_desc_regex(self) -> Pattern:
        raise NotImplementedError

class FormatUtil:
    HourTimeRegex = RegExpUtility.get_safe_reg_exp(r'(?<!P)T\d{2}')

    @staticmethod
    def to_str(num: Union[int, float], size: int) -> str:
        format_ = f'{{0:0{size}d}}'
        return str.format(format_, num)

    @staticmethod
    def luis_date(year: int, month: int, day: int) -> str:
        if year == -1:
            if month == -1:
                return f'XXXX-XX-{day:02d}'
            return f'XXXX-{month:02d}-{day:02d}'
        return f'{year:04d}-{month:02d}-{day:02d}'

    @staticmethod
    def luis_date_from_datetime(date: datetime) -> str:
        return FormatUtil.luis_date(date.year, date.month, date.day)

    @staticmethod
    def luis_time(hour: int, min: int, second: int) -> str:
        return f'{hour:02d}:{min:02d}:{second:02d}'

    @staticmethod
    def luis_time_from_datetime(time: datetime) -> str:
        return FormatUtil.luis_time(time.hour, time.minute, time.second)

    @staticmethod
    def format_date(date: datetime) -> str:
        return f'{date.year:04d}-{date.month:02d}-{date.day:02d}'

    @staticmethod
    def format_time(time: datetime) -> str:
        return f'{time.hour:02d}:{time.minute:02d}-{time.second:02d}'

    @staticmethod
    def format_date_time(date_time: datetime) -> str:
        return FormatUtil.format_date(date_time) + FormatUtil.format_time(date_time)

    @staticmethod
    def all_str_to_pm(source: str) -> str:
        matches = list(regex.finditer(FormatUtil.HourTimeRegex, source))
        split: List[str] = list()
        last_position = 0
        for match in matches:
            if last_position == match.start():
                split.append(source[last_position:match.start()])
            split.append(source[match.start():match.end()])
            last_position = match.end()
        if source[:last_position]:
            split.append(source[:last_position])
        for index, value in enumerate(split):
            if regex.search(FormatUtil.HourTimeRegex, value):
                split[index] = FormatUtil.to_pm(value)
        return ''.join(split)

    @staticmethod
    def to_pm(source: str) -> str:
        result = ''
        if source.startwith('T'):
            result = 'T'
            source = source[1:]
        split = source.split(':')
        hour = int(split[0])
        hour = 0 if hour == 12 else hour + 12
        split[0] = f'{hour:02d}'
        return result + ':'.join(split)
