import { IDateExtractorConfiguration, IDurationExtractorConfiguration, IDatePeriodExtractorConfiguration, BaseDurationExtractor, BaseDateExtractor } from "../extractors";
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

export class EnglishDatePeriodExtractorConfiguration implements IDatePeriodExtractorConfiguration {
    readonly simpleCasesRegexes: RegExp[]
    readonly tillRegex: RegExp
    readonly followedUnit: RegExp
    readonly numberCombinedWithUnit: RegExp
    readonly pastRegex: RegExp
    readonly futureRegex: RegExp
    readonly weekOfRegex: RegExp
    readonly monthOfRegex: RegExp
    readonly datePointExtractor: BaseDateExtractor
    readonly cardinalExtractor: EnglishCardinalExtractor
    
    constructor() {
        this.simpleCasesRegexes = [
            XRegExp(RegExpUtility.sanitizeGroups(EnglishDateTime.SimpleCasesRegex), "gis"),
            XRegExp(RegExpUtility.sanitizeGroups(EnglishDateTime.BetweenRegex), "gis"),
            XRegExp(EnglishDateTime.OneWordPeriodRegex, "gis"),
            XRegExp(EnglishDateTime.MonthWithYear, "gis"),
            XRegExp(RegExpUtility.sanitizeGroups(EnglishDateTime.MonthNumWithYear), "gis"),
            XRegExp(EnglishDateTime.YearRegex, "gis"),
            XRegExp(EnglishDateTime.WeekOfMonthRegex, "gis"),
            XRegExp(EnglishDateTime.WeekOfYearRegex, "gis"),
            XRegExp(RegExpUtility.sanitizeGroups(EnglishDateTime.MonthFrontBetweenRegex), "gis"),
            XRegExp(RegExpUtility.sanitizeGroups(EnglishDateTime.MonthFrontSimpleCasesRegex), "gis"),
            XRegExp(EnglishDateTime.QuarterRegex, "gis"),
            XRegExp(EnglishDateTime.QuarterRegexYearFront, "gis"),
            XRegExp(EnglishDateTime.SeasonRegex, "gis"),
            XRegExp(EnglishDateTime.WhichWeekRegex, "gis"),
        ];
        this.tillRegex = XRegExp(EnglishDateTime.TillRegex, "gis");
        this.followedUnit = XRegExp(EnglishDateTime.FollowedUnit, "gis");
        this.numberCombinedWithUnit = XRegExp(EnglishDateTime.NumberCombinedWithUnit, "gis");
        this.pastRegex = XRegExp(EnglishDateTime.PastRegex, "gis");
        this.futureRegex = XRegExp(EnglishDateTime.FutureRegex, "gis");
        this.weekOfRegex = XRegExp(EnglishDateTime.WeekOfRegex, "gis");
        this.monthOfRegex = XRegExp(EnglishDateTime.MonthOfRegex, "gis");
        this.datePointExtractor = new BaseDateExtractor(new EnglishDateExtractorConfiguration());
        this.cardinalExtractor = new EnglishCardinalExtractor();
    }

    getFromTokenIndex(source: string) {
        let result = {matched: false, index: -1};
        if (source.endsWith("from")) {
            result.index = source.lastIndexOf("from");
            result.matched = true;
        }
        return result;
    };

    getBetweenTokenIndex(source: string) {
        let result = {matched: false, index: -1};
        if (source.endsWith("between")) {
            result.index = source.lastIndexOf("between");
            result.matched = true;
        }
        return result;
    };

    hasConnectorToken(source: string) {
        return source === "and";
    };
}