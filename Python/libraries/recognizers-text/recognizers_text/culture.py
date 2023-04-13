#  Copyright (c) Microsoft Corporation. All rights reserved.
#  Licensed under the MIT License.

class Culture:
    Chinese: str = 'zh-cn'
    Dutch: str = 'nl-nl'
    English: str = 'en-us'
    EnglishOthers: str = 'en-*'
    French: str = 'fr-fr'
    Italian: str = 'it-it'
    Japanese: str = 'ja-jp'
    Korean: str = 'ko-kr'
    Portuguese: str = 'pt-br'
    Spanish: str = 'es-es'
    SpanishMexican: str = 'es-mx'
    Turkish: str = 'tr-tr'
    German: str = 'de-de'

    @staticmethod
    def _get_supported_culture_codes():
        return [
            Culture.English,
            Culture.EnglishOthers,
            Culture.Dutch,
            Culture.Chinese,
            Culture.French,
            Culture.Italian,
            Culture.Japanese,
            Culture.Korean,
            Culture.Portuguese,
            Culture.Spanish,
            Culture.SpanishMexican,
            Culture.Turkish,
            Culture.German
        ]

    @staticmethod
    def map_to_nearest_language(culture_code: str):
        if not culture_code:
            return
        culture_code = culture_code.lower()
        supported_culture_codes = Culture._get_supported_culture_codes()
        if culture_code not in supported_culture_codes:
            culture_prefix = culture_code.split('-')[0].strip()
            possible_cultures = []
            for supportedCultureCode in supported_culture_codes:
                if supportedCultureCode.startswith(culture_prefix):
                    possible_cultures.append(supportedCultureCode)

            if possible_cultures:
                if len(possible_cultures) > 1:
                    for code in possible_cultures:
                        if '*' in code:
                            culture_code = code
                else:
                    culture_code = possible_cultures[0]
        return culture_code


class BaseCultureInfo:
    def __init__(self, culture_code: str):
        self.code: str = culture_code

    def format(self, value: object) -> str:
        return repr(value)
