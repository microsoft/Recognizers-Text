package com.microsoft.recognizers.text.datetime.extractors;

import com.microsoft.recognizers.text.ExtractResult;

import java.time.LocalDateTime;
import java.util.List;

public interface IDateTimeListExtractor {
    String getExtractorName();

    List<ExtractResult> extract(List<ExtractResult> extractResults, String text, LocalDateTime reference);
}
