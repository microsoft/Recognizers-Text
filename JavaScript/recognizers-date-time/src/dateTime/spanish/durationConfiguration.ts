import { BaseNumberExtractor, BaseNumberParser, RegExpUtility, SpanishCardinalExtractor } from "recognizers-text-number";
import { IDurationExtractorConfiguration, IDurationParserConfiguration } from "../baseDuration";
import { SpanishDateTime } from "../../resources/spanishDateTime";
import { ICommonDateTimeParserConfiguration } from "../parsers";

export class SpanishDurationExtractorConfiguration implements IDurationExtractorConfiguration {
    readonly allRegex: RegExp;
    readonly halfRegex: RegExp;
    readonly followedUnit: RegExp;
    readonly numberCombinedWithUnit: RegExp;
    readonly anUnitRegex: RegExp;
    readonly inExactNumberUnitRegex: RegExp;
    readonly suffixAndRegex: RegExp;
    readonly relativeDurationUnitRegex: RegExp;
    readonly cardinalExtractor: BaseNumberExtractor;

    constructor() {
        this.allRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.AllRegex, "gis");
        this.halfRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.HalfRegex, "gis");
        this.followedUnit = RegExpUtility.getSafeRegExp(SpanishDateTime.FollowedUnit, "gis");
        this.numberCombinedWithUnit = RegExpUtility.getSafeRegExp(SpanishDateTime.DurationNumberCombinedWithUnit, "gis");
        this.anUnitRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.AnUnitRegex, "gis");
        this.inExactNumberUnitRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.InExactNumberUnitRegex, "gis");
        this.suffixAndRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.SuffixAndRegex, "gis");
        this.relativeDurationUnitRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.RelativeDurationUnitRegex, "gis");
        this.cardinalExtractor = new SpanishCardinalExtractor();
    }
}

export class SpanishDurationParserConfiguration implements IDurationParserConfiguration {
    readonly cardinalExtractor: BaseNumberExtractor;
    readonly numberParser: BaseNumberParser;
    readonly followedUnit: RegExp;
    readonly suffixAndRegex: RegExp;
    readonly numberCombinedWithUnit: RegExp;
    readonly anUnitRegex: RegExp;
    readonly allDateUnitRegex: RegExp;
    readonly halfDateUnitRegex: RegExp;
    readonly inExactNumberUnitRegex: RegExp;
    readonly unitMap: ReadonlyMap<string, string>;
    readonly unitValueMap: ReadonlyMap<string, number>;
    readonly doubleNumbers: ReadonlyMap<string, number>;

    constructor(config: ICommonDateTimeParserConfiguration) {
        this.cardinalExtractor = config.cardinalExtractor;
        this.numberParser = config.numberParser;
        this.followedUnit = RegExpUtility.getSafeRegExp(SpanishDateTime.FollowedUnit);
        this.suffixAndRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.SuffixAndRegex);
        this.numberCombinedWithUnit = RegExpUtility.getSafeRegExp(SpanishDateTime.DurationNumberCombinedWithUnit);
        this.anUnitRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.AnUnitRegex);
        this.allDateUnitRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.AllRegex);
        this.halfDateUnitRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.HalfRegex);
        this.inExactNumberUnitRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.InExactNumberUnitRegex);
        this.unitMap = config.unitMap;
        this.unitValueMap = config.unitValueMap;
        this.doubleNumbers = config.doubleNumbers;
    }
}