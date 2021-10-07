#  Copyright (c) Microsoft Corporation. All rights reserved.
#  Licensed under the MIT License.

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

        return (hexa_list[0] <= uc <= hexa_list[1]) or (hexa_list[2] <= uc <= hexa_list[3])

    def is_japanese(self, c: chr):
        uc = ord(c)
        hexa_list = [0x3040, 0x309F, 0x30A0, 0x30FF, 0xFF66, 0xFF9D]
        return (hexa_list[0] <= uc <= hexa_list[1]) or (hexa_list[2] <= uc <= hexa_list[3]) or \
               (hexa_list[4] <= uc <= hexa_list[5])

    def is_korean(self, c: chr):
        uc = ord(c)
        hexa_list = [0xAC00, 0xD7AF, 0x1100, 0x11FF, 0x3130, 0x318F, 0xFFB0, 0xFFDC]
        return (hexa_list[0] <= uc <= hexa_list[1]) or (hexa_list[2] <= uc <= hexa_list[3]) or \
               (hexa_list[4] <= uc <= hexa_list[5]) or (hexa_list[6] <= uc <= hexa_list[7])

    def is_cjk(self, c: chr):
        return self.is_chinese(c) or self.is_japanese(c) or self.is_korean(c)
