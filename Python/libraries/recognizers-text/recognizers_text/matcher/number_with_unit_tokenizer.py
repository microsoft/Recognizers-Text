from .simple_tokenizer import SimpleTokenizer
from .token import Token


class NumberWithUnitTokenizer(SimpleTokenizer):

    def __init__(self):
        self.__special_tokens_characters = ['$']

    @property
    def special_tokens_characters(self) -> []:
        return self.__special_tokens_characters

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
                    tokens.append(Token(token_start, i - token_start, input[token_start: token_start + (i - token_start)]))
                    in_token = False
            elif not (c in self.special_tokens_characters) and not (str.isdigit(c) or str.isalpha(c)) or \
                    self.is_chinese(c) or self.is_japanese(c):

                # Non-splittable currency units (as "$") are treated as regular letters. For instance, 'us$' should be
                # a single token
                if in_token:
                    tokens.append(Token(token_start, i - token_start, input[token_start: token_start + (i - token_start)]))
                    in_token = False

                tokens.append(Token(i, 1, input[i: token_start + (i - token_start) + 1]))

            else:
                if in_token and i > 0:
                    pre_char = chars[i - 1]
                    if self.is_splittable_unit(c, pre_char):

                        # Split if letters or non-splittable units are adjacent with digits.
                        tokens.append(Token(token_start, i - token_start, input[token_start: token_start + (i - token_start)]))
                        token_start = i
                if not in_token:
                    token_start = i
                    in_token = True

        if in_token:
            tokens.append(Token(token_start, len(chars) - token_start, input[token_start: token_start + (len(chars) - token_start)]))

        return tokens

    def is_splittable_unit(self, cur_char: chr, pre_char: chr):

        if (str.isalpha(cur_char) and str.isdigit(pre_char)) or (str.isdigit(cur_char) and str.isalpha(pre_char)):
            return True

        if (str.isdigit(cur_char) and pre_char in self.special_tokens_characters) or \
                (cur_char in self.special_tokens_characters and str.isdigit(pre_char)):
            return True

        return False
