import regex as re
from recognizers_sequence.sequence.parsers import SequenceParser
from recognizers_sequence.resources import BasePhoneNumbers, BaseEmail
from recognizers_text.parser import Parser, ParseResult
from recognizers_text import ExtractResult
from recognizers_text.utilities import RegExpUtility


class PhoneNumberParser(SequenceParser):
    scoreUpperLimit = 100
    scoreLowerLimit = 0
    baseScore = 30
    countryCodeAward = 40
    areaCodeAward = 30
    formattedAward = 20
    lengthAward = 10
    typicalFormatDeductionScore = 40
    continueDigitDeductionScore = 10
    tailSameDeductionScore = 10
    continueFormatIndicatorDeductionScore = 20
    wrongFormatIndicatorDeductionScore = 20
    maxFormatIndicatorNum = 3
    maxLengthAwardNum = 3
    tailSameLimit = 2
    phoneNumberLengthBase = 8
    pureDigitLengthLimit = 11
    completeBracketRegex = re.compile('\\(.*\\)')
    singleBracketRegex = re.compile('\\(|\\)')
    tailSameDigitRegex = re.compile('([\\d])\\1{2,10}$')
    pureDigitRegex = re.compile('^\\d*$')
    continueDigitRegex = re.compile('\\d{5}\\d*')
    digitRegex = re.compile('\\d')

    def score_phone_number(self, phone_number_text) -> float:
        score = self.baseScore

        country_code_regex = re.compile(BasePhoneNumbers.CountryCodeRegex)
        area_code_regex = re.compile(BasePhoneNumbers.AreaCodeIndicatorRegex)
        format_indicator_regex = re.compile(BasePhoneNumbers.FormatIndicatorRegex, re.IGNORECASE | re.DOTALL)
        no_area_code_USphonenumber_regex = re.compile(BasePhoneNumbers.NoAreaCodeUSPhoneNumberRegex)

        # Country code score or area code score
        score += self.countryCodeAward if country_code_regex.search(
            phone_number_text) else self.areaCodeAward if area_code_regex.search(
            phone_number_text) else 0

        # Formatted score
        if format_indicator_regex.search(phone_number_text):
            format_matches = list(format_indicator_regex.finditer(phone_number_text))
            format_indicator_count = len(format_matches)
            score += min(format_indicator_count, self.maxFormatIndicatorNum) * self.formattedAward
            score -= self.continueFormatIndicatorDeductionScore if any(
                len(match[0]) > 1 for match in format_matches) else 0
            if self.singleBracketRegex.search(phone_number_text) and not \
                    self.completeBracketRegex.search(phone_number_text):
                score -= self.wrongFormatIndicatorDeductionScore

        # Length score
        if self.digitRegex.search(phone_number_text):
            score += min((len(list(self.digitRegex.finditer(phone_number_text))) - self.phoneNumberLengthBase),
                         self.maxLengthAwardNum) * self.lengthAward

        # Same tailing digit deduction
        if self.tailSameDigitRegex.search(phone_number_text):
            score -= (len(self.tailSameDigitRegex.search(phone_number_text)[
                              0]) - self.tailSameLimit) * self.tailSameDeductionScore

        # Pure digit deduction
        if self.pureDigitRegex.search(phone_number_text):
            score -= (len(phone_number_text) - self.pureDigitLengthLimit) * self.lengthAward \
                if len(phone_number_text) > self.pureDigitLengthLimit else 0

        # Special format deduction
        for pattern in BasePhoneNumbers.TypicalDeductionRegexList:
            if re.search(pattern, phone_number_text):
                score -= self.typicalFormatDeductionScore
                break

        # Continue digit deduction
        if self.continueDigitRegex.search(phone_number_text):
            score -= max(len(list(self.continueDigitRegex.finditer(phone_number_text))) - 1,
                         0) * self.continueDigitDeductionScore

        # Special award for special USphonenumber, i.e. 223-4567 or 223 - 4567
        if no_area_code_USphonenumber_regex.match(phone_number_text):
            score += self.lengthAward * 1.5

        return max(min(score, self.scoreUpperLimit), self.scoreLowerLimit) / (
                self.scoreUpperLimit - self.scoreLowerLimit)

    def parse(self, source: ExtractResult):
        result = ParseResult(source)
        result.resolution_str = source.text
        result.value = self.score_phone_number(source.text)
        return result

class EmailParser(SequenceParser):
    pass
