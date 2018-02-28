import { RegExpUtility } from "@microsoft/recognizers-text";
import { IBooleanExtractorConfiguration } from "../extractors";
import { EnglishChoice } from "../../resources/englishChoice";

export class EnglishBooleanExtractorConfiguration implements IBooleanExtractorConfiguration {
    readonly regexTrue: RegExp;
    readonly regexFalse: RegExp;
    readonly tokenRegex: RegExp;
    readonly onlyTopMatch: boolean;

    constructor(onlyTopMatch: boolean = true) {
        this.regexTrue = RegExpUtility.getSafeRegExp(EnglishChoice.TrueRegex);
        this.regexFalse = RegExpUtility.getSafeRegExp(EnglishChoice.FalseRegex);
        this.tokenRegex = RegExpUtility.getSafeRegExp(EnglishChoice.TokenizerRegex, 'is');
        this.onlyTopMatch = onlyTopMatch;
    }
}