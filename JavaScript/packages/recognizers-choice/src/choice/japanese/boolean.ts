import { RegExpUtility } from "@microsoft/recognizers-text";
import { IBooleanExtractorConfiguration } from "../extractors";
import { JapaneseChoice } from "../../resources/japaneseChoice";

export class JapaneseBooleanExtractorConfiguration implements IBooleanExtractorConfiguration {
    readonly regexTrue: RegExp;
    readonly regexFalse: RegExp;
    readonly tokenRegex: RegExp;
    readonly emojiSkinToneRegex: RegExp;
    readonly onlyTopMatch: boolean;

    constructor(onlyTopMatch: boolean = true) {
        this.emojiSkinToneRegex = RegExpUtility.getSafeRegExp(JapaneseChoice.SkinToneRegex);
        this.regexTrue = RegExpUtility.getSafeRegExp(JapaneseChoice.TrueRegex);
        this.regexFalse = RegExpUtility.getSafeRegExp(JapaneseChoice.FalseRegex);
        this.tokenRegex = RegExpUtility.getSafeRegExp(JapaneseChoice.TokenizerRegex, 'is');
        this.onlyTopMatch = onlyTopMatch;
    }
}