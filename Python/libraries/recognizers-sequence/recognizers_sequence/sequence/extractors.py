from abc import ABC, abstractmethod
from typing import List, Dict, Set, Pattern, Match
from copy import deepcopy
from collections import namedtuple
from itertools import chain
import regex as re

from .constants import *
from recognizers_text.utilities import RegExpUtility
from recognizers_text.extractor import Extractor, ExtractResult
from recognizers_number.culture import CultureInfo
from recognizers_sequence.resources import BasePhoneNumbers
from recognizers_sequence.resources import BaseEmail

ReVal = namedtuple('ReVal', ['re', 'val'])
MatchesVal = namedtuple('MatchesVal', ['matches', 'val'])


class SequenceExtractor(Extractor):
    @property
    @abstractmethod
    def regexes(self) -> List[ReVal]:
        raise NotImplementedError

    @property
    @abstractmethod
    def _extract_type(self) -> str:
        raise NotImplementedError

    def extract(self, source: str) -> List[ExtractResult]:
        result: List[ExtractResult] = list()

        if not self._pre_check_str(source):
            return result

        matched: List[bool] = [False] * len(source)
        match_source: Dict[Match, str] = dict()

        matches_list = list(
            map(lambda x: MatchesVal(matches=list(re.finditer(x.re, source)), val=x.val), self.regexes))
        matches_list = list(filter(lambda x: len(x.matches) > 0, matches_list))

        for ml in matches_list:
            for m in ml.matches:
                for j in range(len(m.group())):
                    matched[m.start() + j] = True
                # Keep Source Data for extra information
                match_source[m] = ml.val

        last = -1
        for i in range(len(source)):
            if not matched[i]:
                last = i
            else:
                if i + 1 == len(source) or not matched[i + 1]:
                    start = last + 1
                    length = i - last
                    substring = source[start:start + length].strip()
                    src_match = next(
                        (x for x in iter(match_source) if (x.start() == start and (x.end() - x.start()) == length)),
                        None)

                    if src_match is not None:
                        value = ExtractResult()
                        value.start = start
                        value.length = length
                        value.text = substring
                        value.type = self._extract_type
                        value.data = match_source.get(src_match, None)
                        result.append(value)
        return result

    @staticmethod
    def _pre_check_str(source: str) -> bool:
        return len(source) != 0


class BasePhoneNumberExtractor(SequenceExtractor):
    @property
    def _extract_type(self) -> str:
        return 'phonenumber'

    @property
    def regexes(self) -> List[ReVal]:
        return self._regexes

    def extract(self, source: str):
        extract_results = super().extract(source)
        ret = []
        format_indicator_regex = re.compile(BasePhoneNumbers.FormatIndicatorRegex, re.IGNORECASE | re.DOTALL)
        for er in extract_results:
            ch = source[er.start - 1]
            if er.start == 0 or ch not in BasePhoneNumbers.BoundaryMarkers:
                ret.append(er)
            elif ch in BasePhoneNumbers.SpecialBoundaryMarkers and \
                    format_indicator_regex.search(er.text) and \
                    er.start >= 2:
                ch_gap = source[er.start - 2]
                if not ch_gap.isdigit():
                    ret.append(er)
                front = source[0:er.start - 1]
                international_dialing_prefix_regex = re.compile(BasePhoneNumbers.InternationDialingPrefixRegex)
                match = international_dialing_prefix_regex.search(front)
                if match is not None:
                    er.start = match.start()
                    er.length = er.length + match.end() - match.start() + 1
                    er.text = source[er.start:er.start + er.length].strip()
                    ret.append(er)
        return ret

    def __init__(self):
        self._regexes = [
            ReVal(RegExpUtility.get_safe_reg_exp(BasePhoneNumbers.BRPhoneNumberRegex), Constants.PHONE_NUMBER_REGEX_BR),
            ReVal(RegExpUtility.get_safe_reg_exp(BasePhoneNumbers.GeneralPhoneNumberRegex),
                  Constants.PHONE_NUMBER_REGEX_GENERAL),
            ReVal(RegExpUtility.get_safe_reg_exp(BasePhoneNumbers.UKPhoneNumberRegex), Constants.PHONE_NUMBER_REGEX_UK),
            ReVal(RegExpUtility.get_safe_reg_exp(BasePhoneNumbers.DEPhoneNumberRegex), Constants.PHONE_NUMBER_REGEX_DE),
            ReVal(RegExpUtility.get_safe_reg_exp(BasePhoneNumbers.USPhoneNumberRegex), Constants.PHONE_NUMBER_REGEX_US),
            ReVal(RegExpUtility.get_safe_reg_exp(BasePhoneNumbers.CNPhoneNumberRegex), Constants.PHONE_NUMBER_REGEX_CN),
            ReVal(RegExpUtility.get_safe_reg_exp(BasePhoneNumbers.DKPhoneNumberRegex), Constants.PHONE_NUMBER_REGEX_DK),
            ReVal(RegExpUtility.get_safe_reg_exp(BasePhoneNumbers.ITPhoneNumberRegex), Constants.PHONE_NUMBER_REGEX_IT),
            ReVal(RegExpUtility.get_safe_reg_exp(BasePhoneNumbers.NLPhoneNumberRegex), Constants.PHONE_NUMBER_REGEX_NL),
            ReVal(RegExpUtility.get_safe_reg_exp(BasePhoneNumbers.SpecialPhoneNumberRegex),
                  Constants.PHONE_NUMBER_REGEX_SPECIAL),
        ]

class BaseEmailExtractor(SequenceExtractor):
    @property
    def _extract_type(self) -> str:
        return 'email'

    @property
    def regexes(self) -> List[ReVal]:
        return self._regexes

    def __init__(self):
        self._regexes = [
            ReVal(RegExpUtility.get_safe_reg_exp(BaseEmail.EmailRegex), Constants.EMAIL_REGEX),
            #EmailRegex2 will break the code as it's not supported in Python, comment out for now
            #ReVal(RegExpUtility.get_safe_reg_exp(BaseEmail.EmailRegex2), Constants.EMAIL_REGEX),
        ]