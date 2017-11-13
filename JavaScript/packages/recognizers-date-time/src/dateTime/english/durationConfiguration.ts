import { IDurationExtractorConfiguration, IDurationParserConfiguration } from "../baseDuration"
import { BaseNumberExtractor, RegExpUtility, BaseNumberParser, EnglishCardinalExtractor } from "recognizers-text-number"
import { EnglishDateTime } from "../../resources/englishDateTime";
import { ICommonDateTimeParserConfiguration } from "../parsers"

export class EnglishDurationExtractorConfiguration implements IDurationExtractorConfiguration {
    readonly allRegex: RegExp
    readonly halfRegex: RegExp
    readonly followedUnit: RegExp
    readonly numberCombinedWithUnit: RegExp
    readonly anUnitRegex: RegExp
    readonly inExactNumberUnitRegex: RegExp
    readonly suffixAndRegex: RegExp
    readonly relativeDurationUnitRegex: RegExp
    readonly cardinalExtractor: EnglishCardinalExtractor

    constructor() {
        this.allRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.AllRegex);
        this.halfRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.HalfRegex);
        this.followedUnit = RegExpUtility.getSafeRegExp(EnglishDateTime.DurationFollowedUnit);
        this.numberCombinedWithUnit = RegExpUtility.getSafeRegExp(EnglishDateTime.NumberCombinedWithDurationUnit);
        this.anUnitRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.AnUnitRegex);
        this.inExactNumberUnitRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.InExactNumberUnitRegex);
        this.suffixAndRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.SuffixAndRegex);
        this.relativeDurationUnitRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.RelativeDurationUnitRegex)
        this.cardinalExtractor = new EnglishCardinalExtractor();
    }
}

export class EnglishDurationParserConfiguration implements IDurationParserConfiguration {
    readonly cardinalExtractor: BaseNumberExtractor
    readonly numberParser: BaseNumberParser
    readonly followedUnit: RegExp
    readonly suffixAndRegex: RegExp
    readonly numberCombinedWithUnit: RegExp
    readonly anUnitRegex: RegExp
    readonly allDateUnitRegex: RegExp
    readonly halfDateUnitRegex: RegExp
    readonly inExactNumberUnitRegex: RegExp
    readonly unitMap: ReadonlyMap<string, string>
    readonly unitValueMap: ReadonlyMap<string, number>
    readonly doubleNumbers: ReadonlyMap<string, number>

    constructor(config: ICommonDateTimeParserConfiguration) {
        this.cardinalExtractor = config.cardinalExtractor;
        this.numberParser = config.numberParser;
        this.followedUnit = RegExpUtility.getSafeRegExp(EnglishDateTime.DurationFollowedUnit);
        this.suffixAndRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.SuffixAndRegex);
        this.numberCombinedWithUnit = RegExpUtility.getSafeRegExp(EnglishDateTime.NumberCombinedWithDurationUnit);
        this.anUnitRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.AnUnitRegex);
        this.allDateUnitRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.AllRegex);
        this.halfDateUnitRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.HalfRegex);
        this.inExactNumberUnitRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.InExactNumberUnitRegex);
        this.unitMap = config.unitMap;
        this.unitValueMap = config.unitValueMap;
        this.doubleNumbers = config.doubleNumbers;
    }
}
