package com.microsoft.recognizers.text.datetime.extractors;

import com.microsoft.recognizers.text.ExtractResult;
import com.microsoft.recognizers.text.Metadata;
import com.microsoft.recognizers.text.datetime.Constants;
import com.microsoft.recognizers.text.datetime.extractors.config.IHolidayExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.utilities.Token;
import com.microsoft.recognizers.text.utilities.Match;
import com.microsoft.recognizers.text.utilities.RegExpUtility;

import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.List;
import java.util.regex.Pattern;

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
    public final List<ExtractResult> extract(String input, LocalDateTime reference) {
        List<Token> tokens = new ArrayList<>();
        tokens.addAll(holidayMatch(input));
        List<ExtractResult> ers = Token.mergeAllTokens(tokens, input, getExtractorName());
        for (ExtractResult er : ers) {
            Metadata metadata = new Metadata() {
                {
                    setIsHoliday(true);
                }
            };
            er.setMetadata(metadata);
        }
        return ers;
    }

    @Override
    public final List<ExtractResult> extract(String input) {
        return this.extract(input, LocalDateTime.now());
    }

    private List<Token> holidayMatch(String text) {
        List<Token> ret = new ArrayList<>();
        for (Pattern regex : this.config.getHolidayRegexes()) {
            Match[] matches = RegExpUtility.getMatches(regex, text);
            for (Match match : matches) {
                ret.add(new Token(match.index, match.index + match.length));
            }
        }
        return ret;
    }
}
