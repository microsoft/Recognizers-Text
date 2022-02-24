// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.text.number.japanese;

/**
 * These modes only apply to JapaneseNumberExtractor.
 * The default more urilizes an allow list to avoid extracting numbers in ambiguous/undesired combinations of Japanese ideograms.
 * ExtractAll mode is to be used in cases where extraction should be more aggressive (e.g. in Units extraction).
 */
public enum JapaneseNumberExtractorMode {
    /**
     * Number extraction with an allow list that filters what numbers to extract.
     */
    Default,

    /**
     * Extract all number-related terms aggressively.
     */
    ExtractAll
}