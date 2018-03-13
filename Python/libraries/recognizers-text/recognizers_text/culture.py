class Culture:
    English: str='en-us'
    Chinese: str='zh-cn'
    Spanish: str='es-es'

class BaseCultureInfo:
    def __init__(self, culture_code: str):
        self.code: str = culture_code

    def format(self, value: object) -> str:
        return repr(value)