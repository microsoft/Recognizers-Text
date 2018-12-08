package com.microsoft.recognizers.text.datetime.parsers;

import com.microsoft.recognizers.text.ExtractResult;
import com.microsoft.recognizers.text.IParser;

import java.time.LocalDateTime;
import java.util.List;

public interface IDateTimeParser extends IParser {
    String getParserName();

    DateTimeParseResult parse(ExtractResult er, LocalDateTime reference);

    List<DateTimeParseResult> filterResults(String query, List<DateTimeParseResult> candidateResults);
}
