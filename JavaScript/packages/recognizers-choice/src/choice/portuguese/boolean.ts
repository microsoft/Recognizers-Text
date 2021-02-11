import { RegExpUtility } from "@microsoft/recognizers-text";
import { IBooleanExtractorConfiguration } from "../extractors";
import { PortugueseChoice } from "../../resources/portugueseChoice";

export class PortugueseBooleanExtractorConfiguration implements IBooleanExtractorConfiguration {
    readonly regexTrue: RegExp;
    readonly regexFalse: RegExp;
    readonly tokenRegex: RegExp;
    readonly emojiSkinToneRegex: RegExp;
    readonly onlyTopMatch: boolean;

    constructor(onlyTopMatch: boolean = true) {
        this.emojiSkinToneRegex = RegExpUtility.getSafeRegExp(PortugueseChoice.SkinToneRegex);
        this.regexTrue = RegExpUtility.getSafeRegExp(PortugueseChoice.TrueRegex);
        this.regexFalse = RegExpUtility.getSafeRegExp(PortugueseChoice.FalseRegex);
        this.tokenRegex = RegExpUtility.getSafeRegExp(PortugueseChoice.TokenizerRegex, 'is');
        this.onlyTopMatch = onlyTopMatch;
    }
}