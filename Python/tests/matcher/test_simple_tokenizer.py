#  Copyright (c) Microsoft Corporation. All rights reserved.
#  Licensed under the MIT License.

from recognizers_text.matcher.simple_tokenizer import SimpleTokenizer


class TestSimpleTokenizer:

    @staticmethod
    def test_english_tokenized():
        tokenizer = SimpleTokenizer()
        text = '   Hi, could     you give me a beer, please?'
        tokenized_text = tokenizer.tokenize(text)

        assert "Hi" == tokenized_text[0].text
        assert 11 == len(tokenized_text)

    @staticmethod
    def test_chinese_tokenized():
        tokenizer = SimpleTokenizer()
        text = '你好，请给我一杯啤酒！'
        tokenized_text = tokenizer.tokenize(text)

        assert "你" == tokenized_text[0].text
        assert 11 == len(tokenized_text)

    @staticmethod
    def test_mixed_tokenized():
        tokenizer = SimpleTokenizer()
        text = 'Hello，请给我1杯beer谢谢！'
        tokenized_text = tokenizer.tokenize(text)

        assert 'Hello' == tokenized_text[0].text
        assert 11 == len(tokenized_text)
