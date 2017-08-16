import {
    IDateExtractorConfiguration,
    ITimeExtractorConfiguration,
    IDurationExtractorConfiguration,
    IDatePeriodExtractorConfiguration,
    TokenIndex,
    BaseDurationExtractor,
    BaseDateExtractor
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
    readonly NonDateUnitRegex: RegExp;
    readonly ordinalExtractor: EnglishOrdinalExtractor;
    readonly integerExtractor: EnglishIntegerExtractor;
    readonly numberParser: BaseNumberParser;
    readonly durationExtractor: BaseDurationExtractor;
    readonly dateTimeUtilityConfiguration;

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
    simpleCasesRegexes: RegExp[];
    tillRegex: RegExp;
    followedUnit: RegExp;
    numberCombinedWithUnit: RegExp;
    pastRegex: RegExp;
    futureRegex: RegExp;
    weekOfRegex: RegExp;
    monthOfRegex: RegExp;
    datePointExtractor: IExtractor;
    cardinalExtractor: IExtractor;

    constructor() {
        this.simpleCasesRegexes = [
            RegExpUtility.getSafeRegExp(EnglishDateTime.SimpleCasesRegex, "gis"),
            RegExpUtility.getSafeRegExp(EnglishDateTime.BetweenRegex, "gis"),
            RegExpUtility.getSafeRegExp(EnglishDateTime.OneWordPeriodRegex, "gis"),
            RegExpUtility.getSafeRegExp(EnglishDateTime.MonthWithYear, "gis"),
            RegExpUtility.getSafeRegExp(EnglishDateTime.MonthNumWithYear, "gis"),
            RegExpUtility.getSafeRegExp(EnglishDateTime.YearRegex, "gis"),
            RegExpUtility.getSafeRegExp(EnglishDateTime.WeekOfMonthRegex, "gis"),
            RegExpUtility.getSafeRegExp(EnglishDateTime.WeekOfYearRegex, "gis"),
            RegExpUtility.getSafeRegExp(EnglishDateTime.MonthFrontBetweenRegex, "gis"),
            RegExpUtility.getSafeRegExp(EnglishDateTime.MonthFrontSimpleCasesRegex, "gis"),
            RegExpUtility.getSafeRegExp(EnglishDateTime.QuarterRegex, "gis"),
            RegExpUtility.getSafeRegExp(EnglishDateTime.QuarterRegexYearFront, "gis"),
            RegExpUtility.getSafeRegExp(EnglishDateTime.SeasonRegex, "gis"),
            RegExpUtility.getSafeRegExp(EnglishDateTime.WhichWeekRegex, "gis")
        ];
        this.tillRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.TillRegex, "gis");
        this.followedUnit = RegExpUtility.getSafeRegExp(EnglishDateTime.FollowedUnit, "gis");
        this.numberCombinedWithUnit = RegExpUtility.getSafeRegExp(EnglishDateTime.NumberCombinedWithUnit, "gis");
        this.pastRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.PastRegex, "gis");
        this.futureRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.FutureRegex, "gis");
        this.weekOfRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.WeekOfRegex, "gis");
        this.monthOfRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.MonthOfRegex, "gis");
        this.datePointExtractor = new BaseDateExtractor(new EnglishDateExtractorConfiguration());
        this.cardinalExtractor = new EnglishCardinalExtractor();
    }

    public getFromTokenIndex(text: string): TokenIndex {
        let index = -1;
        if (text.endsWith("from")) {
            index = text.lastIndexOf("from");
            return new TokenIndex(true, index);
        }
        return new TokenIndex(false, index);
    }

    public getBetweenTokenIndex(text: string): TokenIndex {
        let index = -1;
        if (text.endsWith("between")) {
            index = text.lastIndexOf("between");
            return new TokenIndex(true, index);
        }
        return new TokenIndex(false, index);
    }

    public hasConnectorToken(text: string): boolean {
        return text == "and";
    }
}