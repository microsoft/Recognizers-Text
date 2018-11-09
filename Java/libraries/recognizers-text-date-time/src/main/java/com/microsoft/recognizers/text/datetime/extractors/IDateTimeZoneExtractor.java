package com.microsoft.recognizers.text.datetime.extractors;

import com.microsoft.recognizers.text.ExtractResult;

import java.util.List;

public interface IDateTimeZoneExtractor extends IDateTimeExtractor {
    List<ExtractResult> removeAmbiguousTimezone(List<ExtractResult> extractResults);
}
