package com.microsoft.recognizers.text.number.english.parsers;

import com.microsoft.recognizers.text.Culture;
import com.microsoft.recognizers.text.CultureInfo;
import com.microsoft.recognizers.text.ParseResult;
import com.microsoft.recognizers.text.number.NumberOptions;
import com.microsoft.recognizers.text.number.parsers.BaseNumberParserConfiguration;
import com.microsoft.recognizers.text.number.resources.EnglishNumeric;

import java.util.ArrayList;
import java.util.List;
import java.util.regex.Pattern;

public class EnglishNumberParserConfiguration extends BaseNumberParserConfiguration {

    public EnglishNumberParserConfiguration() {
        this(NumberOptions.None);
    }

    public EnglishNumberParserConfiguration(NumberOptions options) {
        this(new CultureInfo(Culture.English), options);
    }

    public EnglishNumberParserConfiguration(CultureInfo cultureInfo, NumberOptions options) {
        super(
                EnglishNumeric.LangMarker,
                cultureInfo,
                options,
                EnglishNumeric.NonDecimalSeparatorChar,
                EnglishNumeric.DecimalSeparatorChar,
                EnglishNumeric.FractionMarkerToken,
                EnglishNumeric.HalfADozenText,
                EnglishNumeric.WordSeparatorToken,
                EnglishNumeric.WrittenDecimalSeparatorTexts,
                EnglishNumeric.WrittenGroupSeparatorTexts,
                EnglishNumeric.WrittenIntegerSeparatorTexts,
                EnglishNumeric.WrittenFractionSeparatorTexts,
                EnglishNumeric.CardinalNumberMap,
                EnglishNumeric.OrdinalNumberMap,
                EnglishNumeric.RoundNumberMap,
                Pattern.compile(EnglishNumeric.HalfADozenRegex, Pattern.CASE_INSENSITIVE),
                Pattern.compile(EnglishNumeric.DigitalNumberRegex, Pattern.CASE_INSENSITIVE),
                Pattern.compile(EnglishNumeric.NegativeNumberSignRegex, Pattern.CASE_INSENSITIVE),
                Pattern.compile(EnglishNumeric.FractionPrepositionRegex, Pattern.CASE_INSENSITIVE));
    }

    @Override
    public List<String> normalizeTokenSet(List<String> tokens, ParseResult context) {
        List<String> words = new ArrayList<>();

        for (int i = 0; i < tokens.size(); i++) {
            if (tokens.get(i).contains("-")) {
                String[] splitTokens = tokens.get(i).split(Pattern.quote("-"));
                if (splitTokens.length == 2 && getOrdinalNumberMap().containsKey(splitTokens[1])) {
                    words.add(splitTokens[0]);
                    words.add(splitTokens[1]);
                } else {
                    words.add(tokens.get(i));
                }
            } else if (i < tokens.size() - 2 && tokens.get(i + 1).equals("-")) {
                if (getOrdinalNumberMap().containsKey(tokens.get(i + 2))) {
                    words.add(tokens.get(i));
                    words.add(tokens.get(i + 2));
                } else {
                    words.add(tokens.get(i) + tokens.get(i + 1) + tokens.get(i + 2));
                }

                i += 2;
            } else {
                words.add(tokens.get(i));
            }
        }

        return words;
    }

    @Override
    public long resolveCompositeNumber(String numberStr) {
        if (numberStr.contains("-")) {
            String[] numbers = numberStr.split(Pattern.quote("-"));
            long ret = 0;
            for (String number : numbers) {
                if (getOrdinalNumberMap().containsKey(number)) {
                    ret += getOrdinalNumberMap().get(number);
                } else if (getCardinalNumberMap().containsKey(number)) {
                    ret += getCardinalNumberMap().get(number);
                }
            }

            return ret;
        }

        if (getOrdinalNumberMap().containsKey(numberStr)) {
            return getOrdinalNumberMap().get(numberStr);
        }

        if (getCardinalNumberMap().containsKey(numberStr)) {
            return getCardinalNumberMap().get(numberStr);
        }

        return 0;
    }
}
