from abc import ABC, abstractmethod
from typing import List, Dict, Set, Pattern, Match
from collections import namedtuple
import regex as re
from recognizers_sequence.sequence.config.url_configuration import URLConfiguration
from recognizers_text.matcher.string_matcher import StringMatcher

from .constants import *
from recognizers_text.utilities import RegExpUtility
from recognizers_text.extractor import Extractor, ExtractResult
from recognizers_number.culture import CultureInfo
from recognizers_sequence.resources import *
from urllib.parse import urlparse
from os.path import splitext
from recognizers_text.matcher.simple_tokenizer import SimpleTokenizer

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
                if self._is_valid_match(m):
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
                        (x for x in iter(match_source) if (x.start() ==
                                                           start and (x.end() - x.start()) == length)),
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

    def _is_valid_match(self, source: str) -> bool:
        return True


def count_digits(candidate_string: str):
    count = 0
    for t in candidate_string:
        if t.isdigit():
            count += 1
    return count


class BasePhoneNumberExtractor(SequenceExtractor):

    def __init__(self, config):
        self.config = config
        word_boundaries_regex = config.word_boundaries_regex
        non_word_boundaries_regex = config.non_word_boundaries_regex
        end_word_boundaries_regex = config.end_word_boundaries_regex

        self._regexes = [
            ReVal(RegExpUtility.get_safe_reg_exp(BasePhoneNumbers.GeneralPhoneNumberRegex(
                word_boundaries_regex, end_word_boundaries_regex)), Constants.PHONE_NUMBER_REGEX_GENERAL),
            ReVal(RegExpUtility.get_safe_reg_exp(BasePhoneNumbers.BRPhoneNumberRegex(
                word_boundaries_regex, non_word_boundaries_regex, end_word_boundaries_regex)),
                Constants.PHONE_NUMBER_REGEX_BR),
            ReVal(RegExpUtility.get_safe_reg_exp(BasePhoneNumbers.UKPhoneNumberRegex(
                word_boundaries_regex, non_word_boundaries_regex, end_word_boundaries_regex)),
                Constants.PHONE_NUMBER_REGEX_UK),
            ReVal(RegExpUtility.get_safe_reg_exp(BasePhoneNumbers.DEPhoneNumberRegex(
                word_boundaries_regex, end_word_boundaries_regex)), Constants.PHONE_NUMBER_REGEX_DE),
            ReVal(RegExpUtility.get_safe_reg_exp(BasePhoneNumbers.USPhoneNumberRegex(
                word_boundaries_regex, non_word_boundaries_regex, end_word_boundaries_regex)),
                Constants.PHONE_NUMBER_REGEX_US),
            ReVal(RegExpUtility.get_safe_reg_exp(BasePhoneNumbers.CNPhoneNumberRegex(
                word_boundaries_regex, end_word_boundaries_regex)), Constants.PHONE_NUMBER_REGEX_CN),
            ReVal(RegExpUtility.get_safe_reg_exp(BasePhoneNumbers.DKPhoneNumberRegex(
                word_boundaries_regex, end_word_boundaries_regex)), Constants.PHONE_NUMBER_REGEX_DK),
            ReVal(RegExpUtility.get_safe_reg_exp(BasePhoneNumbers.ITPhoneNumberRegex(
                word_boundaries_regex, end_word_boundaries_regex)), Constants.PHONE_NUMBER_REGEX_IT),
            ReVal(RegExpUtility.get_safe_reg_exp(BasePhoneNumbers.NLPhoneNumberRegex(
                word_boundaries_regex, end_word_boundaries_regex)), Constants.PHONE_NUMBER_REGEX_NL),
            ReVal(RegExpUtility.get_safe_reg_exp(BasePhoneNumbers.SpecialPhoneNumberRegex(
                word_boundaries_regex, end_word_boundaries_regex)), Constants.PHONE_NUMBER_REGEX_SPECIAL),
        ]

    @property
    def regexes(self) -> List[ReVal]:
        return self._regexes

    @property
    def _extract_type(self) -> str:
        return Constants.SYS_PHONE_NUMBER

    def extract(self, source: str):
        ret = []
        pre_check_phone_number_regex = re.compile(BasePhoneNumbers.PreCheckPhoneNumberRegex)
        ssn_filter_regex = re.compile(BasePhoneNumbers.SSNFilterRegex)
        colon_prefix_check_regex = re.compile(self.config.colon_prefix_check_regex)

        if (pre_check_phone_number_regex.search(source) is None):
            return ret
        extract_results = super().extract(source)
        format_indicator_regex = re.compile(
            BasePhoneNumbers.FormatIndicatorRegex, re.IGNORECASE | re.DOTALL)
        for er in extract_results:
            if (count_digits(er.text) < 7 and er.data != "ITPhoneNumber") or \
                    ssn_filter_regex.search(er.text):
                continue
            if er.start + er.length < len(source):
                ch = source[er.start + er.length]
                if ch in BasePhoneNumbers.ForbiddenSuffixMarkers:
                    continue

            ch = source[er.start - 1]
            front = source[0: er.start - 1]
            if self.config.false_positive_prefix_regex and re.compile(self.config.false_positive_prefix_regex).search(front):
                continue

            if er.start != 0:
                if ch in BasePhoneNumbers.BoundaryMarkers:
                    # Handle cases like "-1234567" and "-1234+5678"
                    if ch in BasePhoneNumbers.SpecialBoundaryMarkers and \
                            format_indicator_regex.search(er.text) and \
                            er.start >= 2:
                        ch_gap = source[er.start - 2]
                        if ch_gap.isdigit():
                            international_dialing_prefix_regex = re.compile(
                                BasePhoneNumbers.InternationDialingPrefixRegex)
                            match = international_dialing_prefix_regex.search(front)
                            if match is not None:
                                er.start = match.start()
                                er.length = er.length + match.end() - match.start() + 1
                                er.text = source[er.start:er.start + er.length].strip()
                                ret.append(er)
                        # Handle cases like "91a-677-0060".
                        elif ch_gap.islower():
                            continue
                        else:
                            ret.append(er)
                    continue
                elif ch in self.config.forbidden_prefix_markers:
                    # Handle "tel:123456".
                    if ch in BasePhoneNumbers.ColonMarkers:
                        # If the char before ':' is not letter, ignore it.
                        if colon_prefix_check_regex.search(front) is None:
                            continue
                    else:
                        continue
            ret.append(er)

        # filter hexadecimal address like 00 10 00 31 46 D9 E9 11
        for m in re.finditer(BasePhoneNumbers.PhoneNumberMaskRegex, source):
            ret = [er for er in ret if er.start <
                   m.start() or er.end > m.end()]

        return ret


class BaseEmailExtractor(SequenceExtractor):
    @property
    def _extract_type(self) -> str:
        return Constants.SYS_EMAIL

    @property
    def regexes(self) -> List[ReVal]:
        return self._regexes

    def __init__(self):
        self._regexes = [
            ReVal(RegExpUtility.get_safe_reg_exp(
                BaseEmail.EmailRegex), Constants.EMAIL_REGEX),
            # EmailRegex2 will break the code as it's not supported in Python, comment out for now
            # ReVal(RegExpUtility.get_safe_reg_exp(BaseEmail.EmailRegex2, remove_question=True), Constants.EMAIL_REGEX),
        ]


class BaseHashTagExtractor(SequenceExtractor):
    @property
    def _extract_type(self) -> str:
        return Constants.SYS_HASHTAG

    @property
    def regexes(self) -> List[ReVal]:
        return self._regexes

    def __init__(self):
        self._regexes = [ReVal(RegExpUtility.get_safe_reg_exp(BaseHashtag.HashtagRegex), Constants.HASHTAG_REGEX)]


class BaseGUIDExtractor(SequenceExtractor):
    @property
    def _extract_type(self) -> str:
        return Constants.SYS_GUID

    @property
    def regexes(self) -> List[ReVal]:
        return self._regexes

    def __init__(self):
        self._regexes = [ReVal(RegExpUtility.get_safe_reg_exp(BaseGUID.GUIDRegex), Constants.GUID_REGEX)]


class BaseIpExtractor(SequenceExtractor):
    @property
    def _extract_type(self) -> str:
        return Constants.SYS_IP

    @property
    def regexes(self) -> List[ReVal]:
        return self._regexes

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
                    simple_tokenizer = SimpleTokenizer()
                    if substring.startswith(Constants.IPV6_ELLIPSIS) and (
                            start > 0 and (str.isdigit(source[start - 1]) or
                                           (str.isalpha(source[start - 1]) and
                                            not simple_tokenizer.is_cjk(c=list(source)[start - 1])))):
                        continue

                    elif substring.endswith(Constants.IPV6_ELLIPSIS) and (
                            i + 1 < len(source) and (str.isdigit(source[i + 1]) or
                                                     (str.isalpha(source[i + 1]) and
                                                      not simple_tokenizer.is_cjk(c=list(source)[start - 1])))):
                        continue

                    src_match = next(
                        (x for x in iter(match_source) if (x.start() ==
                                                           start and (x.end() - x.start()) == length)),
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

    def __init__(self, config):
        self.config = config
        self._regexes = [
            ReVal(self.config.ipv4_regex, Constants.IP_REGEX_IPV4),
            ReVal(self.config.ipv6_regex, Constants.IP_REGEX_IPV6)
        ]


class BaseMentionExtractor(SequenceExtractor):
    @property
    def _extract_type(self) -> str:
        return Constants.SYS_MENTION

    @property
    def regexes(self) -> List[ReVal]:
        return self._regexes

    def __init__(self):
        self._regexes = [ReVal(RegExpUtility.get_safe_reg_exp(BaseMention.MentionRegex), Constants.MENTION_REGEX)]


class BaseURLExtractor(SequenceExtractor):
    @property
    def _extract_type(self) -> str:
        return Constants.SYS_URL

    @property
    def ambiguous_time_term(self) -> ReVal:
        return self._ambiguous_time_term

    @property
    def config(self) -> URLConfiguration:
        return self._config

    @config.setter
    def config(self, config):
        self._config = config

    def tld_matcher(self) -> StringMatcher:
        return self._tld_matcher

    def _is_valid_match(self, match: Match) -> bool:
        is_valid_tld = False
        is_ip_url = RegExpUtility.get_group(match, 'IPurl')

        if not is_ip_url:
            tld_string = RegExpUtility.get_group(match, 'Tld')
            tld_matches = self.tld_matcher().find(tld_string)
            if any(o.start == 0 and o.end == len(tld_string) for o in tld_matches):
                is_valid_tld = True

        # For cases like "7.am" or "8.pm" which are more likely time terms.
        if re.match(self.ambiguous_time_term.re, match.group(0)) is not None:
            return False
        return is_valid_tld or is_ip_url

    @property
    def regexes(self) -> List[ReVal]:
        return self._regexes

    @ambiguous_time_term.setter
    def ambiguous_time_term(self, value):
        self._ambiguous_time_term = value

    def __init__(self, config):
        self.config = config

        self._tld_matcher = StringMatcher()
        self.tld_matcher().init(BaseURL.TldList)

        self._regexes = [
            ReVal(config.ip_url_regex, Constants.URL_REGEX),
            ReVal(config.url_regex, Constants.URL_REGEX),
            ReVal(RegExpUtility.get_safe_reg_exp(BaseURL.UrlRegex2), Constants.URL_REGEX)
        ]

        self._ambiguous_time_term = ReVal(RegExpUtility.get_safe_reg_exp(BaseURL.AmbiguousTimeTerm),
                                          Constants.URL_REGEX)
