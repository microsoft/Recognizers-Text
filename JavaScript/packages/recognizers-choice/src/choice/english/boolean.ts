// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

import { RegExpUtility } from "@microsoft/recognizers-text";
import { IBooleanExtractorConfiguration } from "../extractors";
import { EnglishChoice } from "../../resources/englishChoice";

export class EnglishBooleanExtractorConfiguration implements IBooleanExtractorConfiguration {
    readonly regexTrue: RegExp;
    readonly regexFalse: RegExp;
    readonly tokenRegex: RegExp;
    readonly emojiSkinToneRegex: RegExp;
    readonly onlyTopMatch: boolean;

    constructor(onlyTopMatch: boolean = true) {
        this.emojiSkinToneRegex = RegExpUtility.getSafeRegExp(EnglishChoice.SkinToneRegex);
        this.regexTrue = RegExpUtility.getSafeRegExp(EnglishChoice.TrueRegex);
        this.regexFalse = RegExpUtility.getSafeRegExp(EnglishChoice.FalseRegex);
        this.tokenRegex = RegExpUtility.getSafeRegExp(EnglishChoice.TokenizerRegex, 'is');
        this.onlyTopMatch = onlyTopMatch;
    }
}