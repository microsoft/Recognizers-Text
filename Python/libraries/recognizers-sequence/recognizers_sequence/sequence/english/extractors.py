from recognizers_sequence.sequence.extractors import *
import regex as re


class PhoneNumberExtractor(BasePhoneNumberExtractor):
    def extract(self, source: str):
        ers = super().extract(source)
        for m in re.finditer(BasePhoneNumbers.PhoneNumberMaskRegex, source):
            ers = [er for er in ers if er.start < m.start() or er.end > m.end()]
        return ers


class EmailExtractor(BaseEmailExtractor):
    pass
