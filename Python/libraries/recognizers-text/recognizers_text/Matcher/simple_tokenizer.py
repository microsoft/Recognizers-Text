from .tokenizer import Tokenizer
from .token import Token


class SimpleTokenizer(Tokenizer):

    def tokenize(self, input: str) -> []:
        tokens = []

        if not input:
            return tokens

        in_token = False
        token_start = 0
        chars = list(input)
        for i in range(0, len(chars)):

            c = chars[i]
            if str.isspace(c):
                if in_token:
                    tokens.append(Token(token_start, i - token_start,
                                        input[token_start: token_start + (i - token_start)]))
                    in_token = False
            elif not (str.isdigit(c) or str.isalpha(c)) or self.is_cjk(c):

                # Non-splittable currency units (as "$") are treated as regular letters. For instance, 'us$' should be
                # a single token
                if in_token:

                    tokens.append(Token(token_start, i - token_start,
                                        input[token_start: token_start + (i - token_start)]))
                    in_token = False

                tokens.append(Token(i, 1, input[i: token_start + (i - token_start) + 1]))

            else:

                if not in_token:
                    token_start = i
                    in_token = True

        if in_token:
            tokens.append(Token(token_start, len(chars) - token_start,
                                input[token_start: token_start + (len(chars) - token_start)]))

        return tokens

    def is_chinese(self, c: chr):
        uc = ord(c)
        hexa_list = [0x4E00, 0x9FBF, 0x3400, 0x4DBF]
        char_list = self.to_ord(hexa_list)

        return (char_list[0] <= uc <= char_list[1]) or (char_list[2] <= uc <= char_list[3])

    def is_japanese(self, c: chr):
        uc = ord(c)
        hexa_list = [0x3040, 0x309F, 0x30A0, 0x30FF, 0xFF66, 0xFF9D]
        char_list = self.to_ord(hexa_list)
        return (char_list[0] <= uc <= char_list[1]) or (char_list[2] <= uc <= char_list[3]) or \
               (char_list[4] <= uc <= char_list[5])

    def is_korean(self, c: chr):
        uc = ord(c)
        hexa_list = [0xAC00, 0xD7AF, 0x1100, 0x11FF, 0x3130, 0x318F, 0xFFB0, 0xFFDC]
        char_list = self.to_ord(hexa_list)
        return (char_list[0] <= uc <= char_list[1]) or (char_list[2] <= uc <= char_list[3]) or \
               (char_list[4] <= uc <= char_list[5]) or (char_list[6] <= uc <= char_list[7])

    def is_cjk(self, c: chr):
        return self.is_chinese(c) or self.is_japanese(c) or self.is_korean(c)

    @staticmethod
    def to_ord(hexa_list):
        return list(map(lambda a: ord(chr(a)), hexa_list))
