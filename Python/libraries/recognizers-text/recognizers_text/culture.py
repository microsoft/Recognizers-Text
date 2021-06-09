class Culture:
    Chinese: str = 'zh-cn'
    Dutch: str = 'nl-nl'
    English: str = 'en-us'
    French: str = 'fr-fr'
    Italian: str = 'it-it'
    Japanese: str = 'ja-jp'
    Korean: str = 'ko-kr'
    Portuguese: str = 'pt-br'
    Spanish: str = 'es-es'
    Turkish: str = 'tr-tr'
    German: str = 'de-de'


class BaseCultureInfo:
    def __init__(self, culture_code: str):
        self.code: str = culture_code

    def format(self, value: object) -> str:
        return repr(value)
