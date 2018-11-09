package com.microsoft.recognizers.text.datetime.extractors;

import java.util.ArrayList;
import com.microsoft.recognizers.text.ExtractResult;
import com.microsoft.recognizers.text.datetime.Constants;
import com.microsoft.recognizers.text.datetime.extractors.config.IHolidayExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.utilities.Token;
import com.microsoft.recognizers.text.utilities.Match;
import com.microsoft.recognizers.text.utilities.RegExpUtility;

import java.time.LocalDateTime;
import java.util.regex.Pattern;
import java.util.List;

public class BaseHolidayExtractor implements IDateTimeExtractor {

    private final IHolidayExtractorConfiguration config;

    @Override
    public String getExtractorName() {
        return Constants.SYS_DATETIME_DATE;
    }

    public BaseHolidayExtractor(IHolidayExtractorConfiguration config) {
        this.config = config;
    }

    @Override
    public final List<ExtractResult> extract(String input, LocalDateTime reference)
    {
        List<Token> tokens = new ArrayList<>();
        tokens.addAll(HolidayMatch(input));
        return Token.mergeAllTokens(tokens, input, getExtractorName());
    }

    @Override
    public final List<ExtractResult> extract(String input)
    {
        return this.extract(input, LocalDateTime.now());
    }

    private List<Token> HolidayMatch(String text)
    {
        List<Token> ret = new ArrayList<>();
        for (Pattern regex : this.config.getHolidayRegexes())
        {
            Match[] matches = RegExpUtility.getMatches(regex, text);
            for (Match match : matches) {
                ret.add(new Token(match.index, match.index + match.length));
            }
        }
        return  ret;
    }
}
