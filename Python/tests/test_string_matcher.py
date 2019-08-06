from recognizers_text.Matcher.simple_tokenizer import SimpleTokenizer


class TestStringMatcher:

    @staticmethod
    def test_simple_string_matcher():
        tokenizer = SimpleTokenizer()
        text = '   Hi, could     you give me a beer, please?'
        tokenized_text = tokenizer.tokenize(text)

        assert "Hi" == tokenized_text[0].text
        assert 11 == len(tokenized_text)

    @staticmethod
    def test_chinese_tokenized():
        tokenizer = SimpleTokenizer()
        text = "你好，请给我一杯啤酒！"
        tokenized_text = tokenizer.tokenize(text)

        assert "你" == tokenized_text[0].text
        assert 11 == len(tokenized_text)

    @staticmethod
    def test_mixed_tokenized():
        tokenizer = SimpleTokenizer()
        text = "Hello，请给我1杯beer谢谢！"
        tokenized_text = tokenizer.tokenize(text)

        assert "Hello" == tokenized_text[0].text
        assert 11 == len(tokenized_text)
