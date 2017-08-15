import {
    IDateExtractorConfiguration,
    ITimeExtractorConfiguration,
    IDurationExtractorConfiguration,
    BaseDurationExtractor
} from "../extractors";
import { EnglishOrdinalExtractor, EnglishIntegerExtractor, EnglishCardinalExtractor } from "../../number/english/extractors"
import { EnglishNumberParserConfiguration } from "../../number/english/parserConfiguration"
import { BaseNumberParser } from "../../number/parsers"
import { IExtractor } from "../../number/extractors"
import { EnglishDateTime } from "../../resources/englishDateTime";
import { Match, RegExpUtility } from "../../utilities";
import * as XRegExp from "XRegExp";

export class EnglishDateExtractorConfiguration implements IDateExtractorConfiguration {
    readonly dateRegexList: RegExp[];
    readonly implicitDateList: RegExp[];
    readonly MonthEnd: RegExp;
    readonly OfMonth: RegExp;
    readonly ordinalExtractor: EnglishOrdinalExtractor;
    readonly integerExtractor: EnglishIntegerExtractor;
    readonly numberParser: BaseNumberParser;

    constructor() {
        this.dateRegexList = [
            RegExpUtility.getSafeRegExp(EnglishDateTime.DateExtractor1, "gis"),
            RegExpUtility.getSafeRegExp(EnglishDateTime.DateExtractor2, "gis"),
            RegExpUtility.getSafeRegExp(EnglishDateTime.DateExtractor3, "gis"),
            RegExpUtility.getSafeRegExp(EnglishDateTime.DateExtractor4, "gis"),
            RegExpUtility.getSafeRegExp(EnglishDateTime.DateExtractor5, "gis"),
            RegExpUtility.getSafeRegExp(EnglishDateTime.DateExtractor6, "gis"),
            RegExpUtility.getSafeRegExp(EnglishDateTime.DateExtractor7, "gis"),
            RegExpUtility.getSafeRegExp(EnglishDateTime.DateExtractor8, "gis"),
            RegExpUtility.getSafeRegExp(EnglishDateTime.DateExtractor9, "gis"),
            RegExpUtility.getSafeRegExp(EnglishDateTime.DateExtractorA, "gis"),
        ];
        this.implicitDateList = [
            RegExpUtility.getSafeRegExp(EnglishDateTime.OnRegex, "gis"),
            RegExpUtility.getSafeRegExp(EnglishDateTime.RelaxedOnRegex, "gis"),
            RegExpUtility.getSafeRegExp(EnglishDateTime.SpecialDayRegex, "gis"),
            RegExpUtility.getSafeRegExp(EnglishDateTime.ThisRegex, "gis"),
            RegExpUtility.getSafeRegExp(EnglishDateTime.LastRegex, "gis"),
            RegExpUtility.getSafeRegExp(EnglishDateTime.NextRegex, "gis"),
            RegExpUtility.getSafeRegExp(EnglishDateTime.StrictWeekDay, "gis"),
            RegExpUtility.getSafeRegExp(EnglishDateTime.WeekDayOfMonthRegex, "gis"),
            RegExpUtility.getSafeRegExp(EnglishDateTime.SpecialDate, "gis"),
        ];
        this.MonthEnd = RegExpUtility.getSafeRegExp(EnglishDateTime.MonthEnd, "gis");
        this.OfMonth = RegExpUtility.getSafeRegExp(EnglishDateTime.OfMonth, "gis");
        this.ordinalExtractor = new EnglishOrdinalExtractor();
        this.integerExtractor = new EnglishIntegerExtractor();
        this.numberParser = new BaseNumberParser(new EnglishNumberParserConfiguration());
    }
}
export class EnglishTimeExtractorConfiguration implements ITimeExtractorConfiguration {
    readonly timeRegexList: RegExp[];
    readonly atRegex: RegExp;
    readonly ishRegex: RegExp;

    public DurationExtractor: IExtractor;

    constructor() {
        this.timeRegexList = [
            // (three min past)? seven|7|(senven thirty) pm
            RegExpUtility.getSafeRegExp(EnglishDateTime.TimeRegex1, "gis"),
            // (three min past)? 3:00(:00)? (pm)?
            RegExpUtility.getSafeRegExp(EnglishDateTime.TimeRegex2, "gis"),
            // (three min past)? 3.00 (pm)?
            RegExpUtility.getSafeRegExp(EnglishDateTime.TimeRegex3, "gis"),
            // (three min past) (five thirty|seven|7|7:00(:00)?) (pm)? (in the night)
            RegExpUtility.getSafeRegExp(EnglishDateTime.TimeRegex4, "gis"),
            // (three min past) (five thirty|seven|7|7:00(:00)?) (pm)?
            RegExpUtility.getSafeRegExp(EnglishDateTime.TimeRegex5, "gis"),
            // (five thirty|seven|7|7:00(:00)?) (pm)? (in the night)
            RegExpUtility.getSafeRegExp(EnglishDateTime.TimeRegex6, "gis"),
            // (in the night) at (five thirty|seven|7|7:00(:00)?) (pm)?
            RegExpUtility.getSafeRegExp(EnglishDateTime.TimeRegex7, "gis"),
            // (in the night) (five thirty|seven|7|7:00(:00)?) (pm)?
            RegExpUtility.getSafeRegExp(EnglishDateTime.TimeRegex8, "gis"),
            // 340pm
            RegExpUtility.getSafeRegExp(EnglishDateTime.ConnectNumRegex, "gis")
        ];
        this.atRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.AtRegex, "gis");
        this.ishRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.IshRegex, "gis");
        this.DurationExtractor = new BaseDurationExtractor(new EnglishDurationExtractorConfiguration());
    }
}
export class EnglishDurationExtractorConfiguration implements IDurationExtractorConfiguration {
    readonly followedUnit: RegExp;
    readonly numberCombinedWithUnit: RegExp;
    readonly anUnitRegex: RegExp;
    readonly allRegex: RegExp;
    readonly halfRegex: RegExp;
    readonly suffixAndRegex: RegExp;
    readonly cardinalExtractor: IExtractor;

    constructor() {
        this.cardinalExtractor = new EnglishCardinalExtractor();
        this.followedUnit = RegExpUtility.getSafeRegExp(EnglishDateTime.DurationUnitRegex, "gis");
        this.numberCombinedWithUnit = RegExpUtility.getSafeRegExp(EnglishDateTime.NumberCombinedWithDurationUnit, "gis");
        this.anUnitRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.AnUnitRegex, "gis");
        this.allRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.AllRegex, "gis");
        this.halfRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.HalfRegex, "gis");
        this.suffixAndRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.SuffixAndRegex, "gis");
    }
}