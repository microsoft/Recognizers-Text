import { RegExpUtility } from "@microsoft/recognizers-text";
import { SpanishCardinalExtractor, SpanishIntegerExtractor, SpanishOrdinalExtractor, BaseNumberParser, SpanishNumberParserConfiguration } from "@microsoft/recognizers-text-number";
import { IDateTimeUtilityConfiguration } from "../utilities";
import { SpanishDateTime } from "../../resources/spanishDateTime";
import { BaseDateParserConfiguration } from "../parsers";
import { BaseDateTime } from "../../resources/baseDateTime";
import { BaseDateExtractor, BaseDateParser } from "../baseDate";
import { BaseTimeExtractor, BaseTimeParser } from "../baseTime";
import { BaseDateTimeExtractor, BaseDateTimeParser } from "../baseDateTime";
import { BaseDurationExtractor, BaseDurationParser } from "../baseDuration";
import { BaseDatePeriodExtractor, BaseDatePeriodParser } from "../baseDatePeriod";
import { BaseTimePeriodExtractor, BaseTimePeriodParser } from "../baseTimePeriod";
import { BaseDateTimePeriodExtractor, BaseDateTimePeriodParser } from "../baseDateTimePeriod";
import { SpanishDateExtractorConfiguration, SpanishDateParserConfiguration } from "./dateConfiguration";
import { SpanishDateTimeExtractorConfiguration, SpanishDateTimeParserConfiguration } from "./dateTimeConfiguration";
import { SpanishDurationExtractorConfiguration, SpanishDurationParserConfiguration } from "./durationConfiguration";
import { SpanishDatePeriodExtractorConfiguration, SpanishDatePeriodParserConfiguration } from "./datePeriodConfiguration";
import { SpanishTimeExtractorConfiguration, SpanishTimeParserConfiguration } from "./timeConfiguration";
import { SpanishTimePeriodExtractorConfiguration, SpanishTimePeriodParserConfiguration } from "./timePeriodConfiguration";
import { SpanishDateTimePeriodExtractorConfiguration, SpanishDateTimePeriodParserConfiguration } from "./dateTimePeriodConfiguration";

export class SpanishDateTimeUtilityConfiguration implements IDateTimeUtilityConfiguration {
    readonly agoRegex: RegExp;
    readonly laterRegex: RegExp;
    readonly inConnectorRegex: RegExp;
    readonly rangeUnitRegex: RegExp;
    readonly amDescRegex: RegExp;
    readonly pmDescRegex: RegExp;
    readonly amPmDescRegex: RegExp;

    constructor() {
        this.laterRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.LaterRegex);
        this.agoRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.AgoRegex);
        this.inConnectorRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.InConnectorRegex);
        this.rangeUnitRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.RangeUnitRegex);
        this.amDescRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.AmDescRegex);
        this.pmDescRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.PmDescRegex);
        this.amPmDescRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.AmPmDescRegex);
    }
}

export class SpanishCommonDateTimeParserConfiguration extends BaseDateParserConfiguration {

    constructor(dmyDateFormat: boolean) {
        super();
        this.utilityConfiguration = new SpanishDateTimeUtilityConfiguration();

        this.unitMap = SpanishDateTime.UnitMap;
        this.unitValueMap = SpanishDateTime.UnitValueMap;
        this.seasonMap = SpanishDateTime.SeasonMap;
        this.cardinalMap = SpanishDateTime.CardinalMap;
        this.dayOfWeek = SpanishDateTime.DayOfWeek;
        this.monthOfYear = SpanishDateTime.MonthOfYear;
        this.numbers = SpanishDateTime.Numbers;
        this.doubleNumbers = SpanishDateTime.DoubleNumbers;

        this.cardinalExtractor = new SpanishCardinalExtractor();
        this.integerExtractor = new SpanishIntegerExtractor();
        this.ordinalExtractor = new SpanishOrdinalExtractor();

        this.numberParser = new BaseNumberParser(new SpanishNumberParserConfiguration());
        this.dateExtractor = new BaseDateExtractor(new SpanishDateExtractorConfiguration(dmyDateFormat));
        this.timeExtractor = new BaseTimeExtractor(new SpanishTimeExtractorConfiguration());
        this.dateTimeExtractor = new BaseDateTimeExtractor(new SpanishDateTimeExtractorConfiguration(dmyDateFormat));
        this.durationExtractor = new BaseDurationExtractor(new SpanishDurationExtractorConfiguration());
        this.datePeriodExtractor = new BaseDatePeriodExtractor(new SpanishDatePeriodExtractorConfiguration(dmyDateFormat));
        this.timePeriodExtractor = new BaseTimePeriodExtractor(new SpanishTimePeriodExtractorConfiguration());
        this.dateTimePeriodExtractor = new BaseDateTimePeriodExtractor(new SpanishDateTimePeriodExtractorConfiguration(dmyDateFormat));
        this.durationParser = new BaseDurationParser(new SpanishDurationParserConfiguration(this));
        this.dateParser = new BaseDateParser(new SpanishDateParserConfiguration(this, dmyDateFormat));
        this.timeParser = new BaseTimeParser(new SpanishTimeParserConfiguration(this));
        this.dateTimeParser = new BaseDateTimeParser(new SpanishDateTimeParserConfiguration(this));
        this.datePeriodParser = new BaseDatePeriodParser(new SpanishDatePeriodParserConfiguration(this));
        this.timePeriodParser = new BaseTimePeriodParser(new SpanishTimePeriodParserConfiguration(this));
        this.dateTimePeriodParser = new BaseDateTimePeriodParser(new SpanishDateTimePeriodParserConfiguration(this));
    }
}