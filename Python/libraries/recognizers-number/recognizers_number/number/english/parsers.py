from typing import Dict, Pattern, List

from recognizers_text.parser import ParseResult
from recognizers_number.number.parsers import NumberParserConfiguration, BaseNumberParser
from recognizers_number.resources.english_numeric import EnglishNumeric

class EnglishNumberParserConfiguration(NumberParserConfiguration):
    @property
    def cardinal_number_map(self) -> Dict[str, int]: pass
    @property
    def ordinal_number_map(self) -> Dict[str, int]: pass
    @property
    def round_number_map(self) -> Dict[str, int]: pass
    @property
    def culture_info(self): return self._culture_info
    @property
    def digital_number_regex(self) -> Pattern: pass
    @property
    def fraction_marker_token(self) -> str: pass
    @property
    def negative_number_sign_regex(self) -> Pattern: pass
    @property
    def half_a_dozen_regex(self) -> Pattern: pass
    @property
    def half_a_dozen_text(self) -> str: pass
    @property
    def lang_marker(self) -> str: return self._lang_marker
    @property
    def non_decimal_separator_char(self) -> str: pass
    @property
    def decimal_separator_char(self) -> str: pass
    @property
    def word_separator_token(self) -> str: pass
    @property
    def written_decimal_separator_texts(self) -> List[str]: pass
    @property
    def written_group_separator_texts(self) -> List[str]: pass
    @property
    def written_integer_separator_texts(self) -> List[str]: pass
    @property
    def written_fraction_separator_texts(self) -> List[str]: pass
    @property
    def normalize_token_set(self, tokens: List[str], context: ParseResult) -> List[str]: pass
    @property
    def resolve_composite_number(self, number_str: str) -> int: pass

    def __init__(self, culture_info=None):
        if culture_info is None:
            culture_info = 'English'

        self._culture_info = culture_info
        self._lang_marker = EnglishNumeric.LangMarker