import { RegExpUtility } from "@microsoft/recognizers-text";
import { IBooleanExtractorConfiguration } from "../extractors";
import { DutchChoice } from "../../resources/dutchChoice";

export class DutchBooleanExtractorConfiguration implements IBooleanExtractorConfiguration {
    readonly regexTrue: RegExp;
    readonly regexFalse: RegExp;
    readonly tokenRegex: RegExp;
    readonly emojiSkinToneRegex: RegExp;
    readonly onlyTopMatch: boolean;

    constructor(onlyTopMatch: boolean = true) {
        this.emojiSkinToneRegex = RegExpUtility.getSafeRegExp(DutchChoice.SkinToneRegex);
        this.regexTrue = RegExpUtility.getSafeRegExp(DutchChoice.TrueRegex);
        this.regexFalse = RegExpUtility.getSafeRegExp(DutchChoice.FalseRegex);
        this.tokenRegex = RegExpUtility.getSafeRegExp(DutchChoice.TokenizerRegex, 'is');
        this.onlyTopMatch = onlyTopMatch;
    }
}