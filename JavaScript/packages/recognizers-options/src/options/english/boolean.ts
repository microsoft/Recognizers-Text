import { RegExpUtility } from "recognizers-text";
import { IBooleanExtractorConfiguration } from "../extractors";
import { EnglishOptions } from "../../resources/englishOptions";

export class EnglishBooleanExtractorConfiguration implements IBooleanExtractorConfiguration {
    readonly regexTrue: RegExp;
    readonly regexFalse: RegExp;
    readonly tokenRegex: RegExp;
    readonly onlyTopMatch: boolean;

    constructor(onlyTopMatch: boolean = true) {
        this.regexTrue = RegExpUtility.getSafeRegExp(EnglishOptions.TrueRegex);
        this.regexFalse = RegExpUtility.getSafeRegExp(EnglishOptions.FalseRegex);
        this.tokenRegex = RegExpUtility.getSafeRegExp(EnglishOptions.TokenizerRegex);
        this.onlyTopMatch = onlyTopMatch;
    }
}