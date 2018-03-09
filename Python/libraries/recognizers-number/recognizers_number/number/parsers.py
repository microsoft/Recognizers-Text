from abc import ABC, abstractproperty
from typing import Dict, Pattern, Optional, List
import regex

from recognizers_text.extractor import ExtractResult
from recognizers_text.parser import Parser, ParseResult

class NumberParserConfiguration(ABC):
    @abstractproperty
    def cardinal_number_map(self) -> Dict[str, int]: pass
    @abstractproperty
    def ordinal_number_map(self) -> Dict[str, int]: pass
    @abstractproperty
    def round_number_map(self) -> Dict[str, int]: pass
    @abstractproperty
    def culture_info(self) -> str: pass
    @abstractproperty
    def digital_number_regex(self) -> Pattern: pass
    @abstractproperty
    def fraction_marker_token(self) -> str: pass
    @abstractproperty
    def negative_number_sign_regex(self) -> Pattern: pass
    @abstractproperty
    def half_a_dozen_regex(self) -> Pattern: pass
    @abstractproperty
    def half_a_dozen_text(self) -> str: pass
    @abstractproperty
    def lang_marker(self) -> str: pass
    @abstractproperty
    def non_decimal_separator_char(self) -> str: pass
    @abstractproperty
    def decimal_separator_char(self) -> str: pass
    @abstractproperty
    def word_separator_token(self) -> str: pass
    @abstractproperty
    def written_decimal_separator_texts(self) -> List[str]: pass
    @abstractproperty
    def written_group_separator_texts(self) -> List[str]: pass
    @abstractproperty
    def written_integer_separator_texts(self) -> List[str]: pass
    @abstractproperty
    def written_fraction_separator_texts(self) -> List[str]: pass
    @abstractproperty
    def normalize_token_set(self, tokens: List[str], context: ParseResult) -> List[str]: pass
    @abstractproperty
    def resolve_composite_number(self, number_str: str) -> int: pass
    
class BaseNumberParser(Parser):
    def __init__(self, config: NumberParserConfiguration):
        self.config: NumberParserConfiguration = config
        self.supported_types: List[str] = list()

        singleIntFrac = f"{self.config.word_separator_token}| -|{self._get_key_regex(self.config.cardinal_number_map.keys())}|{self._get_key_regex(self.config.ordinal_number_map.keys())}"

        #self.text_number_regex: Pattern = RegExpUtility.getSafeRegExp(String.raw`(?=\b)(${singleIntFrac})(?=\b)`, "gis")
        #self.arabic_number_regex: Pattern = RegExpUtility.getSafeRegExp(String.raw`\d+`, "is")
        #self.roundN_number_set: Pattern = new Set<string>()
        #self.config.round_number_map.forEach((value, key) =>
        #    self.roundNumberSet.add(key)
        #);

    def parse(self, source: ExtractResult) -> Optional[ParseResult]:
        # check if the parser is configured to support specific types
        if source.type not in self.supported_types:
            return None
        return ParseResult(source)

    def _get_key_regex(self, keys: List[str]) -> str:
        return str.join('|', sorted(keys, key = len, reverse=True))

    def _digit_number_parse(self, ext_result: ExtractResult) -> ParseResult:
        pass
    
    def _is_digit(self, c: str) -> bool:
        pass
    
    def _frac_like_number_parse(self, ext_result: ExtractResult) -> ParseResult:
        pass
    
    def _text_number_parse(self, ext_result: ExtractResult) -> ParseResult:
        pass

    def _power_number_parse(self, ext_result: ExtractResult) -> ParseResult:
        pass

    def __split_multi(self, source: str, tokens: List[str]) -> List[str]:
        pass

    def __get_matches(self, source: str) -> List[str]:
        pass

    def __is_composable(self, big: int, small: int) -> bool:
        pass

    def __get_int_value(self, matches: List[str]) -> int:
        pass

    def __get_point_value(self, matches: List[str]) -> float:
        pass

    def __get_digital_value(self, digitstr: str, power: int) -> float:
        tmp: float = 0
        scale: float = 10
        dot: bool = False
        negative: bool = False
        fraction: bool = False

        call_stack: List[int] = list()

        for c in digitstr:
            if c == '/':
                fraction = True

            if c == ' ' or c == '/':
                call_stack.append(tmp)
            elif c.isdigit():
                if dot:
                    tmp = tmp + scale * int(c)
                    scale *= 0.1
                else:
                    tmp = tmp * scale + int(c)
            elif c == self.config.decimal_separator_char:
                dot = True
                scale = 0.1
            elif c == '-':
                negative = True

        call_stack.append(tmp)

        # is the number is a fraction.
        cal_result = 0
        if fraction:
            deno = call_stack.pop()
            mole = call_stack.pop()
            cal_result += mole / deno
        
        for n in call_stack:
            cal_result += n

        cal_result *= power

        return cal_result if not negative else -cal_result

class BasePercentageParser(BaseNumberParser):
    def parse(self, source: ExtractResult) -> Optional[ParseResult]:
        original = source.text

        # do replace text & data from extended info
        if type(source.data) is list:
            source.text = source.data[0]
            source.data = source.data[1].data
        
        result: ParseResult() = super().parse(source)

        if len(result.resolution_str) > 0:
            if not result.resolution_str.strip().endswith('%'):
                result.resolution_str = result.resolution_str.strip() + '%'

        result.data = source.text
        result.text = original

        return result