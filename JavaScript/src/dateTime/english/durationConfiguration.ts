import { IDurationExtractorConfiguration, IDurationParserConfiguration } from "../baseDuration"
import { BaseNumberExtractor } from "../../number/extractors"
import { RegExpUtility } from "../../utilities";
import { EnglishDateTime } from "../../resources/englishDateTime";
import { BaseNumberParser } from "../../number/parsers"
import { EnglishCardinalExtractor } from "../../number/english/extractors"
import { ICommonDateTimeParserConfiguration } from "../parsers"

export class EnglishDurationExtractorConfiguration implements IDurationExtractorConfiguration {
    readonly allRegex: RegExp
    readonly halfRegex: RegExp
    readonly followedUnit: RegExp
    readonly numberCombinedWithUnit: RegExp
    readonly anUnitRegex: RegExp
    readonly inExactNumberUnitRegex: RegExp
    readonly suffixAndRegex: RegExp
    readonly cardinalExtractor: EnglishCardinalExtractor

    constructor() {
        this.allRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.AllRegex, "gis");
        this.halfRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.HalfRegex, "gis");
        this.followedUnit = RegExpUtility.getSafeRegExp(EnglishDateTime.DurationFollowedUnit, "gis");
        this.numberCombinedWithUnit = RegExpUtility.getSafeRegExp(EnglishDateTime.NumberCombinedWithDurationUnit, "gis");
        this.anUnitRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.AnUnitRegex, "gis");
        this.inExactNumberUnitRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.InExactNumberUnitRegex, "gis");
        this.suffixAndRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.SuffixAndRegex, "gis");
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
