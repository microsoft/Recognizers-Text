package com.microsoft.recognizers.text.datetime.extractors;

import com.microsoft.recognizers.text.ExtractResult;
import com.microsoft.recognizers.text.IExtractor;

import java.time.LocalDateTime;
import java.util.List;

public interface IDateTimeExtractor extends IExtractor {
    String getExtractorName();

    List<ExtractResult> extract(String input, LocalDateTime reference);
}
