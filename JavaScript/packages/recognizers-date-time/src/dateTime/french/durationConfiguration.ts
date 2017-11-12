import { BaseNumberExtractor, BaseNumberParser, RegExpUtility, FrenchCardinalExtractor } from "recognizers-text-number";
import { IDurationExtractorConfiguration, IDurationParserConfiguration } from "../baseDuration";
import { FrenchDateTime } from "../../resources/frenchDateTime";
import { ICommonDateTimeParserConfiguration } from "../parsers";

export class FrenchDurationExtractorConfiguration implements IDurationExtractorConfiguration {
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
        this.allRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.AllRegex, "gis");
        this.halfRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.HalfRegex, "gis");
        this.followedUnit = RegExpUtility.getSafeRegExp(FrenchDateTime.DurationFollowedUnit, "gis");
        this.numberCombinedWithUnit = RegExpUtility.getSafeRegExp(FrenchDateTime.NumberCombinedWithDurationUnit, "gis");
        this.anUnitRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.AnUnitRegex, "gis");
        this.inExactNumberUnitRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.InExactNumberUnitRegex, "gis");
        this.suffixAndRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.SuffixAndRegex, "gis");
        this.relativeDurationUnitRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.RelativeDurationUnitRegex, "gis");
        this.cardinalExtractor = new FrenchCardinalExtractor();
    }
}

export class FrenchDurationParserConfiguration implements IDurationParserConfiguration {
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
        this.followedUnit = RegExpUtility.getSafeRegExp(FrenchDateTime.DurationFollowedUnit);
        this.suffixAndRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.SuffixAndRegex);
        this.numberCombinedWithUnit = RegExpUtility.getSafeRegExp(FrenchDateTime.NumberCombinedWithDurationUnit);
        this.anUnitRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.AnUnitRegex);
        this.allDateUnitRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.AllRegex);
        this.halfDateUnitRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.HalfRegex);
        this.inExactNumberUnitRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.InExactNumberUnitRegex);
        this.unitMap = config.unitMap;
        this.unitValueMap = config.unitValueMap;
        this.doubleNumbers = config.doubleNumbers;
    }
}