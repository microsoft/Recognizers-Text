from abc import ABC, abstractmethod
from typing import Dict, Pattern, Optional, List
from decimal import Decimal, getcontext
import regex
from recognizers_text import ExtractResult
from recognizers_text.parser import Parser, ParseResult


class SequenceParser(Parser):
    def parse(self, source: ExtractResult) -> Optional[ParseResult]:
        res = ParseResult(source)
        res.resolution_str = res.text
        return res
