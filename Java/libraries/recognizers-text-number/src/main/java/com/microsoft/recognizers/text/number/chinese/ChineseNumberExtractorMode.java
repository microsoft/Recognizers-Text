package com.microsoft.recognizers.text.number.chinese;

/**
 * These modes only apply to ChineseNumberExtractor.
 * The default more urilizes an allow list to avoid extracting numbers in ambiguous/undesired combinations of Chinese ideograms.
 * ExtractAll mode is to be used in cases where extraction should be more aggressive (e.g. in Units extraction).
 */
public enum ChineseNumberExtractorMode {
    /**
     * Number extraction with an allow list that filters what numbers to extract.
     */
    Default,

    /**
     * Extract all number-related terms aggressively.
     */
    ExtractAll
}