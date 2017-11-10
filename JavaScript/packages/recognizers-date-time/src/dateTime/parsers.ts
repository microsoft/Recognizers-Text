import { IParser, ParseResult, BaseNumberParser, ExtractResult, BaseNumberExtractor, RegExpUtility, Match, StringUtility } from "recognizers-text-number";
import { IDateTimeUtilityConfiguration, FormatUtil, DateTimeResolutionResult, DateUtils, DayOfWeek, MatchingUtil, AgoLaterUtil } from "./utilities";
import { BaseDateTime } from "../resources/baseDateTime";
import { Constants, TimeTypeConstants } from "./constants";
import { BaseDateExtractor, BaseDateParser} from "./baseDate"
import { BaseTimeExtractor, BaseTimeParser} from "./baseTime"
import { BaseDatePeriodExtractor, BaseDatePeriodParser} from "./baseDatePeriod"
import { BaseTimePeriodExtractor, BaseTimePeriodParser} from "./baseTimePeriod"
import { IDateTimeExtractor, BaseDateTimeExtractor, BaseDateTimeParser} from "./baseDateTime"
import { BaseDateTimePeriodExtractor, BaseDateTimePeriodParser} from "./baseDateTimePeriod"
import { BaseSetExtractor, BaseSetParser} from "./baseSet"
import { BaseDurationExtractor, BaseDurationParser} from "./baseDuration"
import { BaseHolidayExtractor, BaseHolidayParser} from "./baseHoliday"

export class DateTimeParseResult extends ParseResult {
    // TimexStr is only used in extractors related with date and time
    // It will output the TIMEX representation of a time string.
    timexStr: string
}

export interface IDateTimeParser extends IParser {
    parse(extResult: ExtractResult, referenceDate?: Date): DateTimeParseResult | null;
}

export interface ICommonDateTimeParserConfiguration {
    cardinalExtractor: BaseNumberExtractor;
    integerExtractor: BaseNumberExtractor;
    ordinalExtractor: BaseNumberExtractor;
    numberParser: BaseNumberParser;
    dateExtractor: IDateTimeExtractor;
    timeExtractor: IDateTimeExtractor;
    dateTimeExtractor: IDateTimeExtractor;
    durationExtractor: IDateTimeExtractor;
    datePeriodExtractor: IDateTimeExtractor;
    timePeriodExtractor: IDateTimeExtractor;
    dateTimePeriodExtractor: IDateTimeExtractor;
    dateParser: BaseDateParser;
    timeParser: BaseTimeParser;
    dateTimeParser: BaseDateTimeParser;
    durationParser: BaseDurationParser;
    datePeriodParser: BaseDatePeriodParser;
    timePeriodParser: BaseTimePeriodParser;
    dateTimePeriodParser: BaseDateTimePeriodParser;
    monthOfYear: ReadonlyMap<string, number>;
    numbers: ReadonlyMap<string, number>;
    unitValueMap: ReadonlyMap<string, number>;
    seasonMap: ReadonlyMap<string, string>;
    unitMap: ReadonlyMap<string, string>;
    cardinalMap: ReadonlyMap<string, number>;
    dayOfMonth: ReadonlyMap<string, number>;
    dayOfWeek: ReadonlyMap<string, number>;
    doubleNumbers: ReadonlyMap<string, number>;
    utilityConfiguration: IDateTimeUtilityConfiguration;
}

export abstract class BaseDateParserConfiguration implements ICommonDateTimeParserConfiguration {
    cardinalExtractor: BaseNumberExtractor;
    integerExtractor: BaseNumberExtractor;
    ordinalExtractor: BaseNumberExtractor;
    numberParser: BaseNumberParser;
    dateExtractor: IDateTimeExtractor;
    timeExtractor: IDateTimeExtractor;
    dateTimeExtractor: IDateTimeExtractor;
    durationExtractor: IDateTimeExtractor;
    datePeriodExtractor: IDateTimeExtractor;
    timePeriodExtractor: IDateTimeExtractor;
    dateTimePeriodExtractor: IDateTimeExtractor;
    dateParser: BaseDateParser;
    timeParser: BaseTimeParser;
    dateTimeParser: BaseDateTimeParser;
    durationParser: BaseDurationParser;
    datePeriodParser: BaseDatePeriodParser;
    timePeriodParser: BaseTimePeriodParser;
    dateTimePeriodParser: BaseDateTimePeriodParser;
    monthOfYear: ReadonlyMap<string, number>;
    numbers: ReadonlyMap<string, number>;
    unitValueMap: ReadonlyMap<string, number>;
    seasonMap: ReadonlyMap<string, string>;
    unitMap: ReadonlyMap<string, string>;
    cardinalMap: ReadonlyMap<string, number>;
    dayOfMonth: ReadonlyMap<string, number>;
    dayOfWeek: ReadonlyMap<string, number>;
    doubleNumbers: ReadonlyMap<string, number>;
    utilityConfiguration: IDateTimeUtilityConfiguration;

    constructor() {
        this.dayOfMonth = BaseDateTime.DayOfMonthDictionary;
    }
}
