from recognizers_text.Matcher.string_matcher import StringMatcher
from recognizers_text.Matcher.match_result import MatchResult


class TestStringMatcher:

    @staticmethod
    def test_simple_string_matcher():
        values = ["China", "Beijing", "City"]
        string_matcher = StringMatcher()
        string_matcher.init(values)
        for value in values:
            match = string_matcher.find(value)
            if match is not None:
                assert value == match.text()

    @staticmethod
    def test_simple_with_ids_string_matcher():
        values = ["China", "Beijing", "City"]
        ids = ["1", "2", "3"]
        string_matcher = StringMatcher()
        string_matcher.init(values, ids)
        for i in range(0, len(values)):
            value = values[i]
            match = string_matcher.find(value)
            if match is not None:
                assert value == match.text()
                assert ids[i] == match.canonical_values()

    @staticmethod
    def test_string_matcher():
        utc_8_value = 'UTC+08:00'
        utc_8_words = ['beijing time', 'chongqing time', 'hong kong time', 'urumqi time']
        utc_2_value = 'UTC+02:00'
        utc_2_words = ['cairo time', 'beirut time', 'gaza time', 'amman time']
        value_dictionary = {utc_2_value: utc_2_words, utc_8_value: utc_8_words}
        string_matcher = StringMatcher()
        string_matcher.init(value_dictionary)

        for value in utc_8_words:
            sentence = 'please change {}, thanks'.format(value)
            matches: [MatchResult] = string_matcher.find(sentence)
            assert value == next((e.text for e in matches), None)
            assert utc_8_value == next((e.canonical_values for e in matches), None)
            assert 14 == next((e.start for e in matches), None)

        for value in utc_2_words:
            sentence = 'please change {}, thanks'.format(value)
            matches: [MatchResult] = string_matcher.find(sentence)
            assert value == next((e.text for e in matches), None)
            assert utc_2_value == next((e.canonical_values for e in matches), None)
            assert 14 == next((e.start for e in matches), None)
