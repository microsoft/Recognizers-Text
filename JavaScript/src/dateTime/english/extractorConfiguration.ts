import { IDateExtractorConfiguration, IDurationExtractorConfiguration, BaseDurationExtractor } from "../extractors";
import { EnglishOrdinalExtractor, EnglishIntegerExtractor, EnglishCardinalExtractor } from "../../number/english/extractors"
import { EnglishNumberParserConfiguration } from "../../number/english/parserConfiguration"
import { BaseNumberParser } from "../../number/parsers"
import { EnglishDateTime } from "../../resources/englishDateTime";
import { Match, RegExpUtility } from "../../utilities";
import * as XRegExp from "xregexp";

export class EnglishDateExtractorConfiguration implements IDateExtractorConfiguration {
    readonly dateRegexList: RegExp[];
    readonly implicitDateList: RegExp[];
    readonly MonthEnd: RegExp;
    readonly OfMonth: RegExp;
    readonly NonDateUnitRegex: RegExp;
    readonly ordinalExtractor: EnglishOrdinalExtractor;
    readonly integerExtractor: EnglishIntegerExtractor;
    readonly numberParser: BaseNumberParser;
    readonly durationExtractor: BaseDurationExtractor;
    readonly dateTimeUtilityConfiguration;

    constructor() {
        this.dateRegexList = [
            XRegExp(EnglishDateTime.DateExtractor1, "gis"),
            XRegExp(EnglishDateTime.DateExtractor2, "gis"),
            XRegExp(EnglishDateTime.DateExtractor3, "gis"),
            XRegExp(EnglishDateTime.DateExtractor4, "gis"),
            XRegExp(EnglishDateTime.DateExtractor5, "gis"),
            XRegExp(EnglishDateTime.DateExtractor6, "gis"),
            XRegExp(EnglishDateTime.DateExtractor7, "gis"),
            XRegExp(EnglishDateTime.DateExtractor8, "gis"),
            XRegExp(EnglishDateTime.DateExtractor9, "gis"),
            XRegExp(EnglishDateTime.DateExtractorA, "gis"),
        ];
        this.implicitDateList = [
            XRegExp(EnglishDateTime.OnRegex, "gis"),
            XRegExp(EnglishDateTime.RelaxedOnRegex, "gis"),
            XRegExp(EnglishDateTime.SpecialDayRegex, "gis"),
            XRegExp(RegExpUtility.sanitizeGroups(EnglishDateTime.ThisRegex), "gis"),
            XRegExp(RegExpUtility.sanitizeGroups(EnglishDateTime.LastRegex), "gis"),
            XRegExp(RegExpUtility.sanitizeGroups(EnglishDateTime.NextRegex), "gis"),
            XRegExp(EnglishDateTime.StrictWeekDay, "gis"),
            XRegExp(EnglishDateTime.WeekDayOfMonthRegex, "gis"),
            XRegExp(EnglishDateTime.SpecialDate, "gis"),
        ];
        this.MonthEnd = XRegExp(EnglishDateTime.MonthEnd, "gis");
        this.OfMonth = XRegExp(EnglishDateTime.OfMonth, "gis");
        this.NonDateUnitRegex = XRegExp(EnglishDateTime.NonDateUnitRegex, "gis");
        this.ordinalExtractor = new EnglishOrdinalExtractor();
        this.integerExtractor = new EnglishIntegerExtractor();
        this.numberParser = new BaseNumberParser(new EnglishNumberParserConfiguration());
        this.durationExtractor = new BaseDurationExtractor(new EnglishDurationExtractorConfiguration());
        this.dateTimeUtilityConfiguration = {
            agoStringList: ["ago"],
            laterStringList: ["later", "from now"],
            inStringList: ["in"]
        };
    }
}

export class EnglishDurationExtractorConfiguration implements IDurationExtractorConfiguration {
    readonly AllRegex: RegExp
    readonly HalfRegex: RegExp
    readonly FollowedUnit: RegExp
    readonly NumberCombinedWithUnit: RegExp
    readonly AnUnitRegex: RegExp
    readonly SuffixAndRegex: RegExp
    readonly cardinalExtractor: EnglishCardinalExtractor

    constructor() {
        this.AllRegex = XRegExp(EnglishDateTime.AllRegex, "gis");
        this.HalfRegex = XRegExp(EnglishDateTime.HalfRegex, "gis");
        this.FollowedUnit = XRegExp(EnglishDateTime.DurationFollowedUnit, "gis");
        this.NumberCombinedWithUnit = XRegExp(EnglishDateTime.NumberCombinedWithDurationUnit, "gis");
        this.AnUnitRegex = XRegExp(EnglishDateTime.AnUnitRegex, "gis");
        this.SuffixAndRegex = XRegExp(EnglishDateTime.SuffixAndRegex, "gis");
        this.cardinalExtractor = new EnglishCardinalExtractor();
    }
}