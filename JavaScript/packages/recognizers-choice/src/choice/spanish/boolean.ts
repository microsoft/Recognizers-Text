import { RegExpUtility } from "@microsoft/recognizers-text";
import { IBooleanExtractorConfiguration } from "../extractors";
import { SpanishChoice } from "../../resources/spanishChoice";

export class SpanishBooleanExtractorConfiguration implements IBooleanExtractorConfiguration {
    readonly regexTrue: RegExp;
    readonly regexFalse: RegExp;
    readonly tokenRegex: RegExp;
    readonly onlyTopMatch: boolean;

    constructor(onlyTopMatch: boolean = true) {
        this.regexTrue = RegExpUtility.getSafeRegExp(SpanishChoice.TrueRegex);
        this.regexFalse = RegExpUtility.getSafeRegExp(SpanishChoice.FalseRegex);
        this.tokenRegex = RegExpUtility.getSafeRegExp(SpanishChoice.TokenizerRegex, 'is');
        this.onlyTopMatch = onlyTopMatch;
    }
}