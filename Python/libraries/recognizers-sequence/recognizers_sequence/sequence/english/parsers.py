from typing import Optional

import regex as re
from recognizers_sequence.sequence.parsers import SequenceParser, BaseIpParser
from recognizers_sequence.resources import BasePhoneNumbers, BaseEmail, BaseGUID
from recognizers_text.parser import Parser, ParseResult
from recognizers_text import ExtractResult, Pattern, regex


class PhoneNumberParser(SequenceParser):
    score_upper_limit = 100
    score_lower_limit = 0
    base_score = 30
    country_code_award = 40
    area_code_award = 30
    formatted_award = 20
    length_award = 10
    typical_format_deduction_score = 40
    continue_digit_deduction_score = 10
    tail_same_deduction_score = 10
    continue_format_indicator_deduction_score = 20
    wrong_format_indicator_deduction_score = 20
    max_format_indicator_num = 3
    max_length_award_num = 3
    tail_same_limit = 2
    phone_number_length_base = 8
    pure_digit_length_limit = 11
    complete_bracket_regex = re.compile('\\(.*\\)')
    single_bracket_regex = re.compile('\\(|\\)')
    tail_same_digit_regex = re.compile('([\\d])\\1{2,10}$')
    pure_digit_regex = re.compile('^\\d*$')
    continue_digit_regex = re.compile('\\d{5}\\d*')
    digit_regex = re.compile('\\d')

    def score_phone_number(self, phone_number_text) -> float:
        score = self.base_score

        country_code_regex = re.compile(BasePhoneNumbers.CountryCodeRegex)
        area_code_regex = re.compile(BasePhoneNumbers.AreaCodeIndicatorRegex)
        format_indicator_regex = re.compile(
            BasePhoneNumbers.FormatIndicatorRegex, re.IGNORECASE | re.DOTALL)
        no_area_code_USphonenumber_regex = re.compile(
            BasePhoneNumbers.NoAreaCodeUSPhoneNumberRegex)

        # Country code score or area code score
        score += self.country_code_award if country_code_regex.search(
            phone_number_text) else self.area_code_award if area_code_regex.search(
            phone_number_text) else 0

        # Formatted score
        if format_indicator_regex.search(phone_number_text):
            format_matches = list(
                format_indicator_regex.finditer(phone_number_text))
            format_indicator_count = len(format_matches)
            score += min(format_indicator_count,
                         self.max_format_indicator_num) * self.formatted_award
            score -= self.continue_format_indicator_deduction_score if any(
                len(match[0]) > 1 for match in format_matches) else 0
            if self.single_bracket_regex.search(phone_number_text) and not \
                    self.complete_bracket_regex.search(phone_number_text):
                score -= self.wrong_format_indicator_deduction_score

        # Length score
        if self.digit_regex.search(phone_number_text):
            score += min((len(list(self.digit_regex.finditer(phone_number_text))) - self.phone_number_length_base),
                         self.max_length_award_num) * self.length_award

        # Same tailing digit deduction
        if self.tail_same_digit_regex.search(phone_number_text):
            score -= (len(self.tail_same_digit_regex.search(phone_number_text)[
                0]) - self.tail_same_limit) * self.tail_same_deduction_score

        # Pure digit deduction
        if self.pure_digit_regex.search(phone_number_text):
            score -= (len(phone_number_text) - self.pure_digit_length_limit) * self.length_award \
                if len(phone_number_text) > self.pure_digit_length_limit else 0

        # Special format deduction
        for pattern in BasePhoneNumbers.TypicalDeductionRegexList:
            if re.search(pattern, phone_number_text):
                score -= self.typical_format_deduction_score
                break

        # Continue digit deduction
        if self.continue_digit_regex.search(phone_number_text):
            score -= max(len(list(self.continue_digit_regex.finditer(phone_number_text))) - 1,
                         0) * self.continue_digit_deduction_score

        # Special award for special USphonenumber, i.e. 223-4567 or 223 - 4567
        if no_area_code_USphonenumber_regex.match(phone_number_text):
            score += self.length_award * 1.5

        return max(min(score, self.score_upper_limit), self.score_lower_limit) / (
            self.score_upper_limit - self.score_lower_limit)

    def parse(self, source: ExtractResult):
        result = ParseResult(source)
        result.resolution_str = source.text
        result.value = self.score_phone_number(source.text)
        return result


class EmailParser(SequenceParser):
    pass


class IpParser(BaseIpParser):
    pass


class MentionParser(SequenceParser):
    pass


class HashtagParser(SequenceParser):
    pass


class URLParser(SequenceParser):
    pass


class GUIDParser(SequenceParser):
    score_upper_limit = 100
    score_lower_limit = 0
    base_score = 100
    no_boundary_penalty = 10
    no_format_penalty = 10
    pure_digit_penalty = 15
    pure_digit_regex = re.compile('^\\d*$')
    format_regex = re.compile('-')

    def parse(self, source: ExtractResult):
        res = ParseResult(source)
        res.resolution_str = source.text
        res.start = source.start
        res.length = source.length
        res.text = source.text
        res.type = source.type
        res.value = self.score_guid(source.text)
        return res

    def score_guid(self, guid_text) -> float:
        score = self.base_score
        guid_element_regex = BaseGUID.GUIDRegexElement
        element_match = list(regex.finditer(guid_element_regex, guid_text))
        element_match = list(filter(None, map(lambda m: m.group(), element_match)))

        for match in element_match:
            start_index = guid_text.index(match)
            score -= self.no_boundary_penalty if start_index == 0 else 0
            score -= 0 if self.format_regex.search(match) else self.no_format_penalty
            score -= self.pure_digit_penalty if self.pure_digit_regex.search(guid_text) else 0

        return max(min(score, self.score_upper_limit), self.score_lower_limit) / (
            self.score_upper_limit - self.score_lower_limit)
