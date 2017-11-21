import { IMergedExtractorConfiguration, IMergedParserConfiguration } from "../baseMerged"
import { BaseDateExtractor, BaseDateParser } from "../baseDate";
import { BaseTimeExtractor, BaseTimeParser } from "../baseTime";
import { BaseSetExtractor, BaseSetParser } from "../baseSet";
import { BaseHolidayExtractor, BaseHolidayParser } from "../baseHoliday";
import { BaseDatePeriodExtractor, BaseDatePeriodParser } from "../baseDatePeriod";
import { BaseTimePeriodExtractor, BaseTimePeriodParser } from "../baseTimePeriod";
import { IDateTimeExtractor, BaseDateTimeExtractor, BaseDateTimeParser } from "../baseDateTime";
import { BaseDateTimePeriodExtractor, BaseDateTimePeriodParser } from "../baseDateTimePeriod";
import { BaseDurationExtractor, BaseDurationParser } from "../baseDuration"
import { RegExpUtility, BaseNumberExtractor, EnglishIntegerExtractor } from "recognizers-text-number";
import { EnglishDateTime } from "../../resources/englishDateTime";
import { EnglishCommonDateTimeParserConfiguration } from "./baseConfiguration"
import { EnglishDurationExtractorConfiguration } from "./durationConfiguration"
import { EnglishTimeExtractorConfiguration } from "./timeConfiguration"
import { EnglishDateExtractorConfiguration } from "./dateConfiguration"
import { EnglishDateTimeExtractorConfiguration } from "./dateTimeConfiguration"
import { EnglishTimePeriodExtractorConfiguration } from "./timePeriodConfiguration"
import { EnglishDatePeriodExtractorConfiguration } from "./datePeriodConfiguration"
import { EnglishDateTimePeriodExtractorConfiguration } from "./dateTimePeriodConfiguration"
import { EnglishSetExtractorConfiguration, EnglishSetParserConfiguration } from "./setConfiguration"
import { EnglishHolidayExtractorConfiguration, EnglishHolidayParserConfiguration } from "./holidayConfiguration"

export class EnglishMergedExtractorConfiguration implements IMergedExtractorConfiguration {
    readonly dateExtractor: IDateTimeExtractor
    readonly timeExtractor: IDateTimeExtractor
    readonly dateTimeExtractor: IDateTimeExtractor
    readonly datePeriodExtractor: IDateTimeExtractor
    readonly timePeriodExtractor: IDateTimeExtractor
    readonly dateTimePeriodExtractor: IDateTimeExtractor
    readonly holidayExtractor: IDateTimeExtractor
    readonly durationExtractor: IDateTimeExtractor
    readonly setExtractor: IDateTimeExtractor
    readonly integerExtractor: BaseNumberExtractor
    readonly afterRegex: RegExp
    readonly sinceRegex: RegExp
    readonly beforeRegex: RegExp
    readonly fromToRegex: RegExp
    readonly singleAmbiguousMonthRegex: RegExp
    readonly prepositionSuffixRegex: RegExp
    readonly numberEndingPattern: RegExp

    constructor() {
        this.dateExtractor = new BaseDateExtractor(new EnglishDateExtractorConfiguration());
        this.timeExtractor = new BaseTimeExtractor(new EnglishTimeExtractorConfiguration());
        this.dateTimeExtractor = new BaseDateTimeExtractor(new EnglishDateTimeExtractorConfiguration());
        this.datePeriodExtractor = new BaseDatePeriodExtractor(new EnglishDatePeriodExtractorConfiguration());
        this.timePeriodExtractor = new BaseTimePeriodExtractor(new EnglishTimePeriodExtractorConfiguration());
        this.dateTimePeriodExtractor = new BaseDateTimePeriodExtractor(new EnglishDateTimePeriodExtractorConfiguration());
        this.holidayExtractor = new BaseHolidayExtractor(new EnglishHolidayExtractorConfiguration());
        this.durationExtractor = new BaseDurationExtractor(new EnglishDurationExtractorConfiguration());
        this.setExtractor = new BaseSetExtractor(new EnglishSetExtractorConfiguration());
        this.integerExtractor = new EnglishIntegerExtractor();
        this.afterRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.AfterRegex);
        this.sinceRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.SinceRegex);
        this.beforeRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.BeforeRegex);
        this.fromToRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.FromToRegex);
        this.singleAmbiguousMonthRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.SingleAmbiguousMonthRegex);
        this.prepositionSuffixRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.PrepositionSuffixRegex);
        this.numberEndingPattern = RegExpUtility.getSafeRegExp(EnglishDateTime.NumberEndingPattern);
    }
}

export class EnglishMergedParserConfiguration implements IMergedParserConfiguration {
    readonly beforeRegex: RegExp
    readonly afterRegex: RegExp
    readonly sinceRegex: RegExp
    readonly dateParser: BaseDateParser
    readonly holidayParser: BaseHolidayParser
    readonly timeParser: BaseTimeParser
    readonly dateTimeParser: BaseDateTimeParser
    readonly datePeriodParser: BaseDatePeriodParser
    readonly timePeriodParser: BaseTimePeriodParser
    readonly dateTimePeriodParser: BaseDateTimePeriodParser
    readonly durationParser: BaseDurationParser
    readonly setParser: BaseSetParser

    constructor(config: EnglishCommonDateTimeParserConfiguration) {
        this.beforeRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.BeforeRegex);
        this.afterRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.AfterRegex);
        this.sinceRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.SinceRegex);
        this.holidayParser = new BaseHolidayParser(new EnglishHolidayParserConfiguration());
        this.dateParser = config.dateParser;
        this.timeParser = config.timeParser;
        this.dateTimeParser = config.dateTimeParser;
        this.datePeriodParser = config.datePeriodParser;
        this.timePeriodParser = config.timePeriodParser;
        this.dateTimePeriodParser = config.dateTimePeriodParser;
        this.durationParser = config.durationParser;
        this.setParser = new BaseSetParser(new EnglishSetParserConfiguration(config));
    }
}
