from typing import Pattern
import regex

class RegExpUtility:
    @staticmethod
    def get_safe_reg_exp(source: str, flags: int = regex.I | regex.S) -> Pattern:
        return regex.compile(source, flags=flags)
