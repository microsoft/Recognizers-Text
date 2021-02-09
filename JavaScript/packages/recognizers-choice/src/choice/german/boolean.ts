import { RegExpUtility } from "@microsoft/recognizers-text";
import { IBooleanExtractorConfiguration } from "../extractors";
import { GermanChoice } from "../../resources/germanChoice";

export class GermanBooleanExtractorConfiguration implements IBooleanExtractorConfiguration {
    readonly regexTrue: RegExp;
    readonly regexFalse: RegExp;
    readonly tokenRegex: RegExp;
    readonly emojiSkinToneRegex: RegExp;
    readonly onlyTopMatch: boolean;

    constructor(onlyTopMatch: boolean = true) {
        this.emojiSkinToneRegex = RegExpUtility.getSafeRegExp(GermanChoice.SkinToneRegex);
        this.regexTrue = RegExpUtility.getSafeRegExp(GermanChoice.TrueRegex);
        this.regexFalse = RegExpUtility.getSafeRegExp(GermanChoice.FalseRegex);
        this.tokenRegex = RegExpUtility.getSafeRegExp(GermanChoice.TokenizerRegex, 'is');
        this.onlyTopMatch = onlyTopMatch;
    }
}