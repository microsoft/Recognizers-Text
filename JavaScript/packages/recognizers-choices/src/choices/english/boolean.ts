import { RegExpUtility } from "recognizers-text-base";
import { IBooleanExtractorConfiguration } from "../extractors";
import { EnglishChoices } from "../../resources/englishChoices";

export class EnglishBooleanExtractorConfiguration implements IBooleanExtractorConfiguration {
    readonly regexTrue: RegExp;
    readonly regexFalse: RegExp;
    readonly tokenRegex: RegExp;
    readonly onlyTopMatch: boolean;

    constructor(onlyTopMatch: boolean = true) {
        this.regexTrue = RegExpUtility.getSafeRegExp(EnglishChoices.TrueRegex);
        this.regexFalse = RegExpUtility.getSafeRegExp(EnglishChoices.FalseRegex);
        this.tokenRegex = RegExpUtility.getSafeRegExp(EnglishChoices.TokenizerRegex);
        this.onlyTopMatch = onlyTopMatch;
    }
}