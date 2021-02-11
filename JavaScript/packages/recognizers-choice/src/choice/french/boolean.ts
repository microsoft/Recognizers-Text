import { RegExpUtility } from "@microsoft/recognizers-text";
import { IBooleanExtractorConfiguration } from "../extractors";
import { FrenchChoice } from "../../resources/frenchChoice";

export class FrenchBooleanExtractorConfiguration implements IBooleanExtractorConfiguration {
    readonly regexTrue: RegExp;
    readonly regexFalse: RegExp;
    readonly tokenRegex: RegExp;
    readonly emojiSkinToneRegex: RegExp;
    readonly onlyTopMatch: boolean;

    constructor(onlyTopMatch: boolean = true) {
        this.emojiSkinToneRegex = RegExpUtility.getSafeRegExp(FrenchChoice.SkinToneRegex);
        this.regexTrue = RegExpUtility.getSafeRegExp(FrenchChoice.TrueRegex);
        this.regexFalse = RegExpUtility.getSafeRegExp(FrenchChoice.FalseRegex);
        this.tokenRegex = RegExpUtility.getSafeRegExp(FrenchChoice.TokenizerRegex, 'is');
        this.onlyTopMatch = onlyTopMatch;
    }
}