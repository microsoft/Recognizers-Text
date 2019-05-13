import { IExtractor } from "@microsoft/recognizers-text";
import { IDateTimePeriodExtractorConfiguration, IDateTimePeriodParserConfiguration } from "../baseDateTimePeriod"
import { BaseDateExtractor, BaseDateParser } from "../baseDate";
import { BaseTimeExtractor, BaseTimeParser } from "../baseTime";
import { IDateTimeExtractor, BaseDateTimeExtractor, BaseDateTimeParser } from "../baseDateTime";
import { BaseTimePeriodExtractor } from "../baseTimePeriod";
import { BaseDurationExtractor, BaseDurationParser } from "../baseDuration"
import { RegExpUtility } from "@microsoft/recognizers-text";
import { EnglishCardinalExtractor } from "@microsoft/recognizers-text-number";
import { EnglishDateTime } from "../../resources/englishDateTime";
import { EnglishCommonDateTimeParserConfiguration } from "./baseConfiguration"
import { EnglishDurationExtractorConfiguration } from "./durationConfiguration"
import { EnglishTimeExtractorConfiguration } from "./timeConfiguration"
import { EnglishDateTimeExtractorConfiguration } from "./dateTimeConfiguration"
import { EnglishDateExtractorConfiguration } from "./dateConfiguration"
import { EnglishTimePeriodExtractorConfiguration } from "../english/timePeriodConfiguration";
import { IDateTimeParser } from "../parsers"

export class EnglishDateTimePeriodExtractorConfiguration implements IDateTimePeriodExtractorConfiguration {
    readonly cardinalExtractor: EnglishCardinalExtractor
    readonly singleDateExtractor: IDateTimeExtractor
    readonly singleTimeExtractor: IDateTimeExtractor
    readonly singleDateTimeExtractor: IDateTimeExtractor
    readonly durationExtractor: IDateTimeExtractor
    readonly timePeriodExtractor: IDateTimeExtractor
    readonly simpleCasesRegexes: RegExp[]
    readonly prepositionRegex: RegExp
    readonly tillRegex: RegExp
    readonly specificTimeOfDayRegex: RegExp
    readonly timeOfDayRegex: RegExp
    readonly periodTimeOfDayWithDateRegex: RegExp
    readonly followedUnit: RegExp
    readonly numberCombinedWithUnit: RegExp
    readonly timeUnitRegex: RegExp
    readonly previousPrefixRegex: RegExp
    readonly nextPrefixRegex: RegExp
    readonly rangeConnectorRegex: RegExp
    readonly relativeTimeUnitRegex: RegExp
    readonly restOfDateTimeRegex: RegExp
    readonly generalEndingRegex: RegExp
    readonly middlePauseRegex: RegExp

    constructor() {
        this.cardinalExtractor = new EnglishCardinalExtractor();
        this.singleDateExtractor = new BaseDateExtractor(new EnglishDateExtractorConfiguration());
        this.singleTimeExtractor = new BaseTimeExtractor(new EnglishTimeExtractorConfiguration());
        this.singleDateTimeExtractor = new BaseDateTimeExtractor(new EnglishDateTimeExtractorConfiguration());
        this.durationExtractor = new BaseDurationExtractor(new EnglishDurationExtractorConfiguration());
        this.timePeriodExtractor = new BaseTimePeriodExtractor(new EnglishTimePeriodExtractorConfiguration())
        this.simpleCasesRegexes = [
            RegExpUtility.getSafeRegExp(EnglishDateTime.PureNumFromTo),
            RegExpUtility.getSafeRegExp(EnglishDateTime.PureNumBetweenAnd),
        ]
        this.prepositionRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.PrepositionRegex);
        this.tillRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.TillRegex);
        this.specificTimeOfDayRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.PeriodSpecificTimeOfDayRegex);
        this.timeOfDayRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.PeriodTimeOfDayRegex);
        this.periodTimeOfDayWithDateRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.PeriodTimeOfDayWithDateRegex);
        this.followedUnit = RegExpUtility.getSafeRegExp(EnglishDateTime.TimeFollowedUnit);
        this.numberCombinedWithUnit = RegExpUtility.getSafeRegExp(EnglishDateTime.TimeNumberCombinedWithUnit);
        this.timeUnitRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.TimeUnitRegex);
        this.previousPrefixRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.PreviousPrefixRegex);
        this.nextPrefixRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.NextPrefixRegex);
        this.rangeConnectorRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.RangeConnectorRegex);
        this.relativeTimeUnitRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.RelativeTimeUnitRegex);
        this.restOfDateTimeRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.RestOfDateTimeRegex);
        this.generalEndingRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.GeneralEndingRegex);
        this.middlePauseRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.MiddlePauseRegex);
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
        let match = RegExpUtility.getMatches(this.rangeConnectorRegex, source).pop();
        return match && match.length === source.length;
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
        this.pureNumberFromToRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.PureNumFromTo);
        this.pureNumberBetweenAndRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.PureNumBetweenAnd);
        this.periodTimeOfDayWithDateRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.PeriodTimeOfDayWithDateRegex);
        this.specificTimeOfDayRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.SpecificTimeOfDayRegex);
        this.pastRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.PreviousPrefixRegex);
        this.futureRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.NextPrefixRegex);
        this.relativeTimeUnitRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.RelativeTimeUnitRegex);
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
        this.morningStartEndRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.MorningStartEndRegex);
        this.afternoonStartEndRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.AfternoonStartEndRegex);
        this.eveningStartEndRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.EveningStartEndRegex);
        this.nightStartEndRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.NightStartEndRegex);
        this.restOfDateTimeRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.RestOfDateTimeRegex);
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
