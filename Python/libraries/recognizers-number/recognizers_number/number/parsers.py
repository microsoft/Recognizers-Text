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
    def culture_Info(self) -> str: pass
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
    def normalize_token_set(tokens: List[str], context: ParseResult) -> List[str]: pass
    @abstractproperty
    def resolve_composite_number(number_str: str) -> int: pass
    
class BaseNumberParser(Parser):
    @abstractproperty
    def supported_types(self) -> List[str]: pass

    def __init__(self, config: NumberParserConfiguration):
        self.config: NumberParserConfiguration = config

        singleIntFrac = f"{self.config.word_separator_token}| -|{self.get_key_regex(self.config.cardinal_number_map.keys())}|{self.get_key_regex(self.config.ordinal_number_map.keys())}"

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

    def get_key_regex(self, keys: List[str]) -> str:
        return str.join('|', sorted(keys, key = len, reverse=True))