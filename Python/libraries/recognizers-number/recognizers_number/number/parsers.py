from abc import ABC, abstractproperty
from typing import Dict, Pattern, Optional, List

from recognizers_text.extractor import ExtractResult
from recognizers_text.parser import Parser, ParseResult

class NumberParserConfiguration(ABC):
    @abstractproperty
    def cardinal_number_map(self) -> Dict[str, int]: pass
    @abstractproperty
    def ordinal_number_map(self) -> Dict[str, int]: pass
    
class BaseNumberParser(Parser):
    @abstractproperty
    def supported_types(self) -> List[str]: pass

    def __init__(self, config: NumberParserConfiguration):
        self.config: NumberParserConfiguration=config

    def parse(self, source: ExtractResult) -> Optional[ParseResult]:
        # check if the parser is configured to support specific types
        if source.type not in self.supported_types:
            return None
        return ParseResult(source)