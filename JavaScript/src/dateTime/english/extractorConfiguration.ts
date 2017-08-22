import {
    IDateExtractorConfiguration,
    ITimeExtractorConfiguration,
    IDurationExtractorConfiguration,
    IDatePeriodExtractorConfiguration,
    IDateTimeExtractorConfiguration,
    IDateTimePeriodExtractorConfiguration,
    BaseDurationExtractor,
    BaseDateExtractor,
    BaseTimeExtractor,
    BaseDateTimeExtractor
} from "../extractors";
import { IDateTimeUtilityConfiguration } from "../utilities";
import { EnglishOrdinalExtractor, EnglishIntegerExtractor, EnglishCardinalExtractor } from "../../number/english/extractors"
import { EnglishNumberParserConfiguration } from "../../number/english/parserConfiguration"
import { BaseNumberParser } from "../../number/parsers"
import { IExtractor } from "../../number/extractors"
import { EnglishDateTime } from "../../resources/englishDateTime";
import { Match, RegExpUtility, isNullOrWhitespace} from "../../utilities";

export class EnglishDateExtractorConfiguration implements IDateExtractorConfiguration {
    readonly dateRegexList: RegExp[];
    readonly implicitDateList: RegExp[];
    readonly monthEnd: RegExp;
    readonly ofMonth: RegExp;
    readonly nonDateUnitRegex: RegExp;
    readonly ordinalExtractor: EnglishOrdinalExtractor;
    readonly integerExtractor: EnglishIntegerExtractor;
    readonly numberParser: BaseNumberParser;
    readonly durationExtractor: BaseDurationExtractor;
    readonly dateTimeUtilityConfiguration: IDateTimeUtilityConfiguration;

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
        this.monthEnd = RegExpUtility.getSafeRegExp(EnglishDateTime.MonthEnd, "gis");
        this.ofMonth = RegExpUtility.getSafeRegExp(EnglishDateTime.OfMonth, "gis");
        this.nonDateUnitRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.NonDateUnitRegex, "gis");
        this.ordinalExtractor = new EnglishOrdinalExtractor();
        this.integerExtractor = new EnglishIntegerExtractor();
        this.numberParser = new BaseNumberParser(new EnglishNumberParserConfiguration());
        this.durationExtractor = new BaseDurationExtractor(new EnglishDurationExtractorConfiguration());
        this.dateTimeUtilityConfiguration = {
            agoRegex: RegExpUtility.getSafeRegExp(EnglishDateTime.AgoRegex, "gis"),
            laterRegex: RegExpUtility.getSafeRegExp(EnglishDateTime.LaterRegex, "gis"),
            inConnectorRegex: RegExpUtility.getSafeRegExp(EnglishDateTime.InConnectorRegex, "gis"),
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
    readonly allRegex: RegExp
    readonly halfRegex: RegExp
    readonly followedUnit: RegExp
    readonly numberCombinedWithUnit: RegExp
    readonly anUnitRegex: RegExp
    readonly suffixAndRegex: RegExp
    readonly cardinalExtractor: EnglishCardinalExtractor

    constructor() {
        this.allRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.AllRegex, "gis");
        this.halfRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.HalfRegex, "gis");
        this.followedUnit = RegExpUtility.getSafeRegExp(EnglishDateTime.DurationFollowedUnit, "gis");
        this.numberCombinedWithUnit = RegExpUtility.getSafeRegExp(EnglishDateTime.NumberCombinedWithDurationUnit, "gis");
        this.anUnitRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.AnUnitRegex, "gis");
        this.suffixAndRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.SuffixAndRegex, "gis");
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
    readonly rangeConnectorRegex: RegExp
    
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
            RegExpUtility.getSafeRegExp(EnglishDateTime.WhichWeekRegex, "gis"),
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
        this.rangeConnectorRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.RangeConnectorRegex, "gis");
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

    hasConnectorToken(source: string): boolean {
        return RegExpUtility.getMatches(this.rangeConnectorRegex, source).length > 0;
    };
}

export class EnglishDateTimeExtractorConfiguration implements IDateTimeExtractorConfiguration {
    readonly datePointExtractor: BaseDateExtractor
    readonly timePointExtractor: BaseTimeExtractor
    readonly durationExtractor: BaseDurationExtractor
    readonly suffixRegex: RegExp
    readonly nowRegex: RegExp
    readonly timeOfTodayAfterRegex: RegExp
    readonly simpleTimeOfTodayAfterRegex: RegExp
    readonly nightRegex: RegExp
    readonly timeOfTodayBeforeRegex: RegExp
    readonly simpleTimeOfTodayBeforeRegex: RegExp
    readonly theEndOfRegex: RegExp
    readonly unitRegex: RegExp
    readonly prepositionRegex: RegExp
    readonly dateTimeUtilityConfiguration: IDateTimeUtilityConfiguration

    constructor() {
        this.datePointExtractor = new BaseDateExtractor(new EnglishDateExtractorConfiguration());
        this.timePointExtractor = new BaseTimeExtractor(new EnglishTimeExtractorConfiguration());
        this.durationExtractor = new BaseDurationExtractor(new EnglishDurationExtractorConfiguration());
        this.suffixRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.SuffixRegex, "gis");
        this.nowRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.NowRegex, "gis");
        this.timeOfTodayAfterRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.TimeOfTodayAfterRegex, "gis");
        this.simpleTimeOfTodayAfterRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.SimpleTimeOfTodayAfterRegex, "gis");
        this.nightRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.TimeOfDayRegex, "gis");
        this.timeOfTodayBeforeRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.TimeOfTodayBeforeRegex, "gis");
        this.simpleTimeOfTodayBeforeRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.SimpleTimeOfTodayBeforeRegex, "gis");
        this.theEndOfRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.TheEndOfRegex, "gis");
        this.unitRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.TimeUnitRegex, "gis");
        this.prepositionRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.PrepositionRegex, "gis");
        this.dateTimeUtilityConfiguration = {
            agoRegex: RegExpUtility.getSafeRegExp(EnglishDateTime.AgoRegex, "gis"),
            laterRegex: RegExpUtility.getSafeRegExp(EnglishDateTime.LaterRegex, "gis"),
            inConnectorRegex: RegExpUtility.getSafeRegExp(EnglishDateTime.InConnectorRegex, "gis"),
        };
    }

    isConnectorToken(source: string): boolean {
        return (isNullOrWhitespace(source)
            || source === "," 
            || source === "t" 
            || source === "for" 
            || source === "around"
            || RegExpUtility.getMatches(this.prepositionRegex, source).length > 0);
    }
}

export class EnglishDateTimePeriodExtractorConfiguration implements IDateTimePeriodExtractorConfiguration {
    readonly cardinalExtractor: EnglishCardinalExtractor
    readonly singleDateExtractor: BaseDateExtractor
    readonly singleTimeExtractor: BaseTimeExtractor
    readonly singleDateTimeExtractor: BaseDateTimeExtractor
    readonly simpleCasesRegexes: RegExp[]
    readonly prepositionRegex: RegExp
    readonly tillRegex: RegExp
    readonly specificTimeOfDayRegex: RegExp
    readonly timeOfDayRegex: RegExp
    readonly periodTimeOfDayWithDateRegex: RegExp
    readonly followedUnit: RegExp
    readonly numberCombinedWithUnit: RegExp
    readonly unitRegex: RegExp
    readonly pastRegex: RegExp
    readonly futureRegex: RegExp
    readonly rangeConnectorRegex: RegExp

    constructor() {
        this.cardinalExtractor = new EnglishCardinalExtractor();
        this.singleDateExtractor = new BaseDateExtractor(new EnglishDateExtractorConfiguration());
        this.singleTimeExtractor = new BaseTimeExtractor(new EnglishTimeExtractorConfiguration());
        this.singleDateTimeExtractor = new BaseDateTimeExtractor(new EnglishDateTimeExtractorConfiguration());
        this.simpleCasesRegexes = [
             RegExpUtility.getSafeRegExp(EnglishDateTime.PureNumFromTo, "gis"),
             RegExpUtility.getSafeRegExp(EnglishDateTime.PureNumBetweenAnd, "gis"),
        ]
        this.prepositionRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.PrepositionRegex, "gis");
        this.tillRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.TillRegex, "gis");
        this.specificTimeOfDayRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.PeriodSpecificTimeOfDayRegex, "gis");
        this.timeOfDayRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.PeriodTimeOfDayRegex, "gis");
        this.periodTimeOfDayWithDateRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.PeriodTimeOfDayWithDateRegex, "gis");
        this.followedUnit = RegExpUtility.getSafeRegExp(EnglishDateTime.TimeFollowedUnit, "gis");
        this.numberCombinedWithUnit = RegExpUtility.getSafeRegExp(EnglishDateTime.TimeNumberCombinedWithUnit, "gis");
        this.unitRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.TimeUnitRegex, "gis");
        this.pastRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.PastRegex, "gis");
        this.futureRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.FutureRegex, "gis");
        this.rangeConnectorRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.RangeConnectorRegex, "gis");
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

    hasConnectorToken(source: string): boolean {
        return RegExpUtility.getMatches(this.rangeConnectorRegex, source).length > 0;
    };
}