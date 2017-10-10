import { EnglishCardinalExtractor, EnglishIntegerExtractor, EnglishOrdinalExtractor, BaseNumberParser, EnglishNumberParserConfiguration, RegExpUtility } from "recognizers-text-number";
import { EnglishDateTime } from "../../resources/englishDateTime"
import { BaseDateTime } from "../../resources/baseDateTime"
import { FormatUtil, DateTimeResolutionResult, IDateTimeUtilityConfiguration } from "../utilities"
import { BaseDateParserConfiguration } from "../parsers"
import { BaseDateExtractor, BaseDateParser} from "../baseDate"
import { BaseTimeExtractor} from "../baseTime"
import { BaseDatePeriodExtractor, BaseDatePeriodParser} from "../baseDatePeriod"
import { BaseTimePeriodExtractor, BaseTimePeriodParser} from "../baseTimePeriod"
import { BaseDateTimeExtractor, BaseDateTimeParser} from "../baseDateTime"
import { BaseDateTimePeriodExtractor, BaseDateTimePeriodParser} from "../baseDateTimePeriod"
import { BaseDurationExtractor, BaseDurationParser} from "../baseDuration"
import { EnglishDurationExtractorConfiguration, EnglishDurationParserConfiguration } from "./durationConfiguration"
import { EnglishTimeExtractorConfiguration, EnglishTimeParserConfiguration } from "./timeConfiguration"
import { EnglishDateExtractorConfiguration, EnglishDateParserConfiguration } from "./dateConfiguration"
import { EnglishDateTimeExtractorConfiguration, EnglishDateTimeParserConfiguration } from "./dateTimeConfiguration"
import { EnglishTimePeriodExtractorConfiguration, EnglishTimePeriodParserConfiguration } from "./timePeriodConfiguration"
import { EnglishDatePeriodExtractorConfiguration, EnglishDatePeriodParserConfiguration } from "./datePeriodConfiguration"
import { EnglishDateTimePeriodExtractorConfiguration, EnglishDateTimePeriodParserConfiguration } from "./dateTimePeriodConfiguration"
import { EnglishTimeParser } from "./parsers"

export class EnglishDateTimeUtilityConfiguration implements IDateTimeUtilityConfiguration {
    readonly agoRegex: RegExp;
    readonly laterRegex: RegExp;
    readonly inConnectorRegex: RegExp;
    readonly rangeUnitRegex: RegExp;
    readonly amDescRegex: RegExp;
    readonly pmDescRegex: RegExp;
    readonly amPmDescRegex: RegExp;

    constructor() {
        this.laterRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.LaterRegex);
        this.agoRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.AgoRegex);
        this.inConnectorRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.InConnectorRegex);
        this.rangeUnitRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.RangeUnitRegex);
        this.amDescRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.AmDescRegex);
        this.pmDescRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.PmDescRegex);
        this.amPmDescRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.AmPmDescRegex);
    }
}

export class EnglishCommonDateTimeParserConfiguration extends BaseDateParserConfiguration {
    constructor() {
        super();
        this.utilityConfiguration = new EnglishDateTimeUtilityConfiguration();
        this.unitMap = EnglishDateTime.UnitMap;
        this.unitValueMap = EnglishDateTime.UnitValueMap;
        this.seasonMap = EnglishDateTime.SeasonMap;
        this.cardinalMap = EnglishDateTime.CardinalMap;
        this.dayOfWeek = EnglishDateTime.DayOfWeek;
        this.monthOfYear = EnglishDateTime.MonthOfYear;
        this.numbers = EnglishDateTime.Numbers;
        this.doubleNumbers = EnglishDateTime.DoubleNumbers;
        this.cardinalExtractor = new EnglishCardinalExtractor();
        this.integerExtractor = new EnglishIntegerExtractor();
        this.ordinalExtractor = new EnglishOrdinalExtractor();
        this.dayOfMonth = new Map<string, number>([...BaseDateTime.DayOfMonthDictionary, ...EnglishDateTime.DayOfMonth]);
        this.numberParser = new BaseNumberParser(new EnglishNumberParserConfiguration());
        this.dateExtractor = new BaseDateExtractor(new EnglishDateExtractorConfiguration());
        this.timeExtractor = new BaseTimeExtractor(new EnglishTimeExtractorConfiguration());
        this.dateTimeExtractor = new BaseDateTimeExtractor(new EnglishDateTimeExtractorConfiguration());
        this.durationExtractor = new BaseDurationExtractor(new EnglishDurationExtractorConfiguration());
        this.datePeriodExtractor = new BaseDatePeriodExtractor(new EnglishDatePeriodExtractorConfiguration());
        this.timePeriodExtractor = new BaseTimePeriodExtractor(new EnglishTimePeriodExtractorConfiguration());
        this.dateTimePeriodExtractor = new BaseDateTimePeriodExtractor(new EnglishDateTimePeriodExtractorConfiguration());
        this.durationParser = new BaseDurationParser(new EnglishDurationParserConfiguration(this));
        this.dateParser = new BaseDateParser(new EnglishDateParserConfiguration(this));
        this.timeParser = new EnglishTimeParser(new EnglishTimeParserConfiguration(this));
        this.dateTimeParser = new BaseDateTimeParser(new EnglishDateTimeParserConfiguration(this));
        this.datePeriodParser = new BaseDatePeriodParser(new EnglishDatePeriodParserConfiguration(this));
        this.timePeriodParser = new BaseTimePeriodParser(new EnglishTimePeriodParserConfiguration(this));
        this.dateTimePeriodParser = new BaseDateTimePeriodParser(new EnglishDateTimePeriodParserConfiguration(this));
    }
}