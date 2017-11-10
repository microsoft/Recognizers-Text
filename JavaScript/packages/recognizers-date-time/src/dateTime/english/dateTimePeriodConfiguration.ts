import { IDateTimePeriodExtractorConfiguration, IDateTimePeriodParserConfiguration } from "../baseDateTimePeriod"
import { BaseDateExtractor, BaseDateParser } from "../baseDate";
import { BaseTimeExtractor, BaseTimeParser } from "../baseTime";
import { IDateTimeExtractor, BaseDateTimeExtractor, BaseDateTimeParser } from "../baseDateTime";
import { BaseDurationExtractor, BaseDurationParser } from "../baseDuration"
import { IExtractor, RegExpUtility, EnglishCardinalExtractor } from "recognizers-text-number";
import { EnglishDateTime } from "../../resources/englishDateTime";
import { EnglishCommonDateTimeParserConfiguration } from "./baseConfiguration"
import { EnglishDurationExtractorConfiguration } from "./durationConfiguration"
import { EnglishTimeExtractorConfiguration } from "./timeConfiguration"
import { EnglishDateTimeExtractorConfiguration } from "./dateTimeConfiguration"
import { EnglishDateExtractorConfiguration } from "./dateConfiguration"
import { IDateTimeParser } from "../parsers"

export class EnglishDateTimePeriodExtractorConfiguration implements IDateTimePeriodExtractorConfiguration {
    readonly cardinalExtractor: EnglishCardinalExtractor
    readonly singleDateExtractor: IDateTimeExtractor
    readonly singleTimeExtractor: IDateTimeExtractor
    readonly singleDateTimeExtractor: IDateTimeExtractor
    readonly durationExtractor: IDateTimeExtractor
    readonly simpleCasesRegexes: RegExp[]
    readonly prepositionRegex: RegExp
    readonly tillRegex: RegExp
    readonly specificTimeOfDayRegex: RegExp
    readonly timeOfDayRegex: RegExp
    readonly periodTimeOfDayWithDateRegex: RegExp
    readonly followedUnit: RegExp
    readonly numberCombinedWithUnit: RegExp
    readonly timeUnitRegex: RegExp
    readonly pastPrefixRegex: RegExp
    readonly nextPrefixRegex: RegExp
    readonly rangeConnectorRegex: RegExp
    readonly relativeTimeUnitRegex: RegExp
    readonly restOfDateTimeRegex: RegExp

    constructor() {
        this.cardinalExtractor = new EnglishCardinalExtractor();
        this.singleDateExtractor = new BaseDateExtractor(new EnglishDateExtractorConfiguration());
        this.singleTimeExtractor = new BaseTimeExtractor(new EnglishTimeExtractorConfiguration());
        this.singleDateTimeExtractor = new BaseDateTimeExtractor(new EnglishDateTimeExtractorConfiguration());
        this.durationExtractor = new BaseDurationExtractor(new EnglishDurationExtractorConfiguration());
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
        this.timeUnitRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.TimeUnitRegex, "gis");
        this.pastPrefixRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.PastPrefixRegex, "gis");
        this.nextPrefixRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.NextPrefixRegex, "gis");
        this.rangeConnectorRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.RangeConnectorRegex, "gis");
        this.relativeTimeUnitRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.RelativeTimeUnitRegex, "gis");
        this.restOfDateTimeRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.RestOfDateTimeRegex, "gis");
    }

    getFromTokenIndex(source: string) {
        let result = { matched: false, index: -1 };
        if (source.endsWith("from")) {
            result.index = source.lastIndexOf("from");
            result.matched = true;
        }
        return result;
    };

    getBetweenTokenIndex(source: string) {
        let result = { matched: false, index: -1 };
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

export class EnglishDateTimePeriodParserConfiguration implements IDateTimePeriodParserConfiguration {
    readonly pureNumberFromToRegex: RegExp
    readonly pureNumberBetweenAndRegex: RegExp
    readonly periodTimeOfDayWithDateRegex: RegExp
    readonly specificTimeOfDayRegex: RegExp
    readonly pastRegex: RegExp
    readonly futureRegex: RegExp
    readonly relativeTimeUnitRegex: RegExp
    readonly numbers: ReadonlyMap<string, number>
    readonly unitMap: ReadonlyMap<string, string>
    readonly dateExtractor: IDateTimeExtractor
    readonly timePeriodExtractor: IDateTimeExtractor
    readonly timeExtractor: IDateTimeExtractor
    readonly dateTimeExtractor: IDateTimeExtractor
    readonly durationExtractor: IDateTimeExtractor
    readonly dateParser: BaseDateParser
    readonly timeParser: BaseTimeParser
    readonly dateTimeParser: BaseDateTimeParser
    readonly timePeriodParser: IDateTimeParser
    readonly durationParser: BaseDurationParser
    readonly morningStartEndRegex: RegExp
    readonly afternoonStartEndRegex: RegExp
    readonly eveningStartEndRegex: RegExp
    readonly nightStartEndRegex: RegExp
    readonly restOfDateTimeRegex: RegExp

    constructor(config: EnglishCommonDateTimeParserConfiguration) {
        this.pureNumberFromToRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.PureNumFromTo, "gis");
        this.pureNumberBetweenAndRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.PureNumBetweenAnd, "gis");
        this.periodTimeOfDayWithDateRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.PeriodTimeOfDayWithDateRegex, "gis");
        this.specificTimeOfDayRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.SpecificTimeOfDayRegex, "gis");
        this.pastRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.PastPrefixRegex, "gis");
        this.futureRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.NextPrefixRegex, "gis");
        this.relativeTimeUnitRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.RelativeTimeUnitRegex, "gis");
        this.numbers = config.numbers;
        this.unitMap = config.unitMap;
        this.dateExtractor = config.dateExtractor;
        this.timePeriodExtractor = config.timePeriodExtractor;
        this.timeExtractor = config.timeExtractor;
        this.dateTimeExtractor = config.dateTimeExtractor;
        this.durationExtractor = config.durationExtractor;
        this.dateParser = config.dateParser;
        this.timeParser = config.timeParser;
        this.dateTimeParser = config.dateTimeParser;
        this.timePeriodParser = config.timePeriodParser;
        this.durationParser = config.durationParser;
        this.morningStartEndRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.MorningStartEndRegex, "gis");
        this.afternoonStartEndRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.AfternoonStartEndRegex, "gis");
        this.eveningStartEndRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.EveningStartEndRegex, "gis");
        this.nightStartEndRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.NightStartEndRegex, "gis");
        this.restOfDateTimeRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.RestOfDateTimeRegex, "gis");
    }

    getMatchedTimeRange(source: string): { timeStr: string, beginHour: number, endHour: number, endMin: number, success: boolean } {
        let timeStr: string;
        let beginHour = 0;
        let endHour = 0;
        let endMin = 0;
        let success = false;
        if (RegExpUtility.getMatches(this.morningStartEndRegex, source).length > 0) {
            timeStr = 'TMO';
            beginHour = 8;
            endHour = 12;
            success = true;
        } else if (RegExpUtility.getMatches(this.afternoonStartEndRegex, source).length > 0) {
            timeStr = 'TAF';
            beginHour = 12;
            endHour = 16;
            success = true;
        } else if (RegExpUtility.getMatches(this.eveningStartEndRegex, source).length > 0) {
            timeStr = 'TEV';
            beginHour = 16;
            endHour = 20;
            success = true;
        } else if (RegExpUtility.getMatches(this.nightStartEndRegex, source).length > 0) {
            timeStr = 'TNI';
            beginHour = 20;
            endHour = 23;
            endMin = 59;
            success = true;
        }
        return { timeStr: timeStr, beginHour: beginHour, endHour: endHour, endMin: endMin, success: success };
    }

    getSwiftPrefix(source: string): number {
        let swift = 0;
        if (source.startsWith('next')) swift = 1;
        else if (source.startsWith('last')) swift = -1;
        return swift;
    }
}
