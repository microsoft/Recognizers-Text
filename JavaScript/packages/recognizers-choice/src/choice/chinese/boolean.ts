import { RegExpUtility } from "@microsoft/recognizers-text";
import { IBooleanExtractorConfiguration } from "../extractors";
import { ChineseChoice } from "../../resources/chineseChoice";

export class ChineseBooleanExtractorConfiguration implements IBooleanExtractorConfiguration {
    readonly regexTrue: RegExp;
    readonly regexFalse: RegExp;
    readonly tokenRegex: RegExp;
    readonly emojiSkinToneRegex: RegExp;
    readonly onlyTopMatch: boolean;

    constructor(onlyTopMatch: boolean = true) {
        this.emojiSkinToneRegex = RegExpUtility.getSafeRegExp(ChineseChoice.SkinToneRegex);
        this.regexTrue = RegExpUtility.getSafeRegExp(ChineseChoice.TrueRegex);
        this.regexFalse = RegExpUtility.getSafeRegExp(ChineseChoice.FalseRegex);
        this.tokenRegex = RegExpUtility.getSafeRegExp(ChineseChoice.TokenizerRegex, 'is');
        this.onlyTopMatch = onlyTopMatch;
    }
}