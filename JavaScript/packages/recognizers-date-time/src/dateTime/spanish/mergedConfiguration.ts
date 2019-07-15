import { RegExpUtility } from "@microsoft/recognizers-text";
import { BaseNumberExtractor, SpanishIntegerExtractor } from "@microsoft/recognizers-text-number";
import { IMergedExtractorConfiguration, IMergedParserConfiguration } from "../baseMerged";
import { BaseDateExtractor, BaseDateParser } from "../baseDate";
import { BaseTimeExtractor, BaseTimeParser } from "../baseTime";
import { BaseDateTimeExtractor, BaseDateTimeParser, IDateTimeExtractor } from "../baseDateTime";
import { BaseDatePeriodExtractor, BaseDatePeriodParser } from "../baseDatePeriod";
import { BaseTimePeriodExtractor, BaseTimePeriodParser } from "../baseTimePeriod";
import { BaseDateTimePeriodExtractor, BaseDateTimePeriodParser } from "../baseDateTimePeriod";
import { BaseHolidayExtractor, BaseHolidayParser } from "../baseHoliday";
import { BaseDurationExtractor, BaseDurationParser } from "../baseDuration";
import { BaseSetExtractor, BaseSetParser } from "../baseSet";
import { SpanishDateTime } from "../../resources/spanishDateTime";
import { SpanishDateExtractorConfiguration } from "./dateConfiguration";
import { SpanishDateTimeExtractorConfiguration } from "./dateTimeConfiguration";
import { SpanishDatePeriodExtractorConfiguration, SpanishDatePeriodParserConfiguration } from "./datePeriodConfiguration";
import { SpanishDurationExtractorConfiguration } from "./durationConfiguration";
import { SpanishHolidayExtractorConfiguration, SpanishHolidayParserConfiguration } from "./holidayConfiguration";
import { SpanishCommonDateTimeParserConfiguration } from "./baseConfiguration";
import { SpanishTimeExtractorConfiguration } from "./timeConfiguration";
import { SpanishTimePeriodExtractorConfiguration, SpanishTimePeriodParserConfiguration } from "./timePeriodConfiguration";
import { SpanishDateTimePeriodExtractorConfiguration, SpanishDateTimePeriodParserConfiguration } from "./dateTimePeriodConfiguration";
import { SpanishSetExtractorConfiguration, SpanishSetParserConfiguration } from "./setConfiguration";
import { SpanishDateTimePeriodParser } from "./dateTimePeriodParser";

export class SpanishMergedExtractorConfiguration implements IMergedExtractorConfiguration {
    readonly dateExtractor: IDateTimeExtractor;
    readonly timeExtractor: IDateTimeExtractor;
    readonly dateTimeExtractor: IDateTimeExtractor;
    readonly datePeriodExtractor: IDateTimeExtractor;
    readonly timePeriodExtractor: IDateTimeExtractor;
    readonly dateTimePeriodExtractor: IDateTimeExtractor;
    readonly holidayExtractor: IDateTimeExtractor;
    readonly durationExtractor: IDateTimeExtractor;
    readonly setExtractor: IDateTimeExtractor;
    readonly integerExtractor: BaseNumberExtractor;
    readonly afterRegex: RegExp;
    readonly beforeRegex: RegExp;
    readonly sinceRegex: RegExp;
    readonly fromToRegex: RegExp;
    readonly singleAmbiguousMonthRegex: RegExp;
    readonly prepositionSuffixRegex: RegExp;
    readonly numberEndingPattern: RegExp;
    readonly filterWordRegexList: RegExp[];

    constructor(dmyDateFormat: boolean = false) {
        this.beforeRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.BeforeRegex);
        this.afterRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.AfterRegex);
        this.sinceRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.SinceRegex);
        this.fromToRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.FromToRegex);
        this.singleAmbiguousMonthRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.SingleAmbiguousMonthRegex);
        this.prepositionSuffixRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.PrepositionSuffixRegex);
        this.numberEndingPattern = RegExpUtility.getSafeRegExp(SpanishDateTime.NumberEndingPattern);

        this.dateExtractor = new BaseDateExtractor(new SpanishDateExtractorConfiguration(dmyDateFormat));
        this.timeExtractor = new BaseTimeExtractor(new SpanishTimeExtractorConfiguration());
        this.dateTimeExtractor = new BaseDateTimeExtractor(new SpanishDateTimeExtractorConfiguration(dmyDateFormat));
        this.datePeriodExtractor = new BaseDatePeriodExtractor(new SpanishDatePeriodExtractorConfiguration(dmyDateFormat));
        this.timePeriodExtractor = new BaseTimePeriodExtractor(new SpanishTimePeriodExtractorConfiguration());
        this.dateTimePeriodExtractor = new BaseDateTimePeriodExtractor(new SpanishDateTimePeriodExtractorConfiguration(dmyDateFormat));
        this.durationExtractor = new BaseDurationExtractor(new SpanishDurationExtractorConfiguration());
        this.setExtractor = new BaseSetExtractor(new SpanishSetExtractorConfiguration(dmyDateFormat));
        this.holidayExtractor = new BaseHolidayExtractor(new SpanishHolidayExtractorConfiguration());
        this.integerExtractor = new SpanishIntegerExtractor();
        this.filterWordRegexList = [];
    }
}

export class SpanishMergedParserConfiguration extends SpanishCommonDateTimeParserConfiguration implements IMergedParserConfiguration {
    readonly beforeRegex: RegExp;
    readonly afterRegex: RegExp;
    readonly sinceRegex: RegExp;
    readonly dateParser: BaseDateParser;
    readonly holidayParser: BaseHolidayParser;
    readonly timeParser: BaseTimeParser;
    readonly dateTimeParser: BaseDateTimeParser;
    readonly datePeriodParser: BaseDatePeriodParser;
    readonly timePeriodParser: BaseTimePeriodParser;
    readonly dateTimePeriodParser: BaseDateTimePeriodParser;
    readonly durationParser: BaseDurationParser;
    readonly setParser: BaseSetParser;

    constructor(dmyDateFormat: boolean = false) {
        super(dmyDateFormat);

        this.beforeRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.BeforeRegex);
        this.afterRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.AfterRegex);
        this.sinceRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.SinceRegex);

        this.datePeriodParser = new BaseDatePeriodParser(new SpanishDatePeriodParserConfiguration(this));
        this.timePeriodParser = new BaseTimePeriodParser(new SpanishTimePeriodParserConfiguration(this));
        this.dateTimePeriodParser = new SpanishDateTimePeriodParser(new SpanishDateTimePeriodParserConfiguration(this));
        this.setParser = new BaseSetParser(new SpanishSetParserConfiguration(this));
        this.holidayParser = new BaseHolidayParser(new SpanishHolidayParserConfiguration());
    }
}