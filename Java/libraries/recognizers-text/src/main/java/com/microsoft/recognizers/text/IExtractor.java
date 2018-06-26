package com.microsoft.recognizers.text;

import java.util.List;

public interface IExtractor {
    List<ExtractResult> extract(String input);
}
