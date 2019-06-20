from recognizers_sequence.resources.chinese_phone_numbers import ChinesePhoneNumbers
from recognizers_sequence.sequence.extractors import *
from recognizers_text.culture import Culture
import regex as re


class ChinesePhoneNumberExtractorConfiguration(BaseSequenceExtractorConfiguration):
    @property
    def br_phone_number_regex(self) -> Pattern:
        return self._BRPhoneNumberRegex

    @property
    def general_phone_number_regex(self) -> Pattern:
        return self._GeneralPhoneNumberRegex

    @property
    def uk_phone_number_regex(self) -> Pattern:
        return self._UKPhoneNumberRegex

    @property
    def de_phone_number_regex(self) -> Pattern:
        return self._DEPhoneNumberRegex

    @property
    def us_phone_number_regex(self) -> Pattern:
        return self._USPhoneNumberRegex

    @property
    def cn_phone_number_regex(self) -> Pattern:
        return self._CNPhoneNumberRegex

    @property
    def dk_phone_number_regex(self) -> Pattern:
        return self._DKPhoneNumberRegex

    @property
    def it_phone_number_regex(self) -> Pattern:
        return self._ITPhoneNumberRegex

    @property
    def nl_phone_number_regex(self) -> Pattern:
        return self._NLPhoneNumberRegex

    @property
    def special_phone_number_regex(self) -> Pattern:
        return self._SpecialPhoneNumberRegex

    def __init__(self, culture_info: CultureInfo = None):
        if culture_info is None:
            culture_info = CultureInfo(Culture.Chinese)
        super().__init__(culture_info)
        self._BRPhoneNumberRegex = RegExpUtility.get_safe_reg_exp(ChinesePhoneNumbers.BRPhoneNumberRegex)
        self._GeneralPhoneNumberRegex = RegExpUtility.get_safe_reg_exp(ChinesePhoneNumbers.GeneralPhoneNumberRegex)
        self._UKPhoneNumberRegex = RegExpUtility.get_safe_reg_exp(ChinesePhoneNumbers.UKPhoneNumberRegex)
        self._DEPhoneNumberRegex = RegExpUtility.get_safe_reg_exp(ChinesePhoneNumbers.DEPhoneNumberRegex)
        self._USPhoneNumberRegex = RegExpUtility.get_safe_reg_exp(ChinesePhoneNumbers.USPhoneNumberRegex)
        self._CNPhoneNumberRegex = RegExpUtility.get_safe_reg_exp(ChinesePhoneNumbers.CNPhoneNumberRegex)
        self._DKPhoneNumberRegex = RegExpUtility.get_safe_reg_exp(ChinesePhoneNumbers.DKPhoneNumberRegex)
        self._ITPhoneNumberRegex = RegExpUtility.get_safe_reg_exp(ChinesePhoneNumbers.ITPhoneNumberRegex)
        self._NLPhoneNumberRegex = RegExpUtility.get_safe_reg_exp(ChinesePhoneNumbers.NLPhoneNumberRegex)
        self._SpecialPhoneNumberRegex = RegExpUtility.get_safe_reg_exp(ChinesePhoneNumbers.SpecialPhoneNumberRegex)



