import { RegExpUtility, BaseNumberExtractor, FrenchIntegerExtractor } from "recognizers-text-number";
import { IMergedExtractorConfiguration, IMergedParserConfiguration } from "../baseMerged";
import { BaseDateExtractor, BaseDateParser } from "../baseDate";
import { BaseTimeExtractor, BaseTimeParser } from "../baseTime";
import { IDateTimeExtractor, BaseDateTimeExtractor, BaseDateTimeParser } from "../baseDateTime";
import { BaseDatePeriodExtractor, BaseDatePeriodParser } from "../baseDatePeriod";
import { BaseTimePeriodExtractor, BaseTimePeriodParser } from "../baseTimePeriod";
import { BaseDateTimePeriodExtractor, BaseDateTimePeriodParser } from "../baseDateTimePeriod";
import { BaseHolidayExtractor, BaseHolidayParser } from "../baseHoliday";
import { BaseDurationExtractor, BaseDurationParser } from "../baseDuration";
import { BaseSetExtractor, BaseSetParser } from "../baseSet";
import { FrenchDateTime } from "../../resources/frenchDateTime";
import { FrenchDateExtractorConfiguration } from "./dateConfiguration";
import { FrenchDateTimeExtractorConfiguration } from "./dateTimeConfiguration";
import { FrenchDatePeriodExtractorConfiguration, FrenchDatePeriodParserConfiguration } from "./datePeriodConfiguration";
import { FrenchDurationExtractorConfiguration } from "./durationConfiguration";
import { FrenchHolidayExtractorConfiguration, FrenchHolidayParserConfiguration } from "./holidayConfiguration";
import { FrenchCommonDateTimeParserConfiguration } from "./baseConfiguration";
import { FrenchTimeExtractorConfiguration } from "./timeConfiguration";
import { FrenchTimePeriodExtractorConfiguration, FrenchTimePeriodParserConfiguration } from "./timePeriodConfiguration";
import { FrenchDateTimePeriodExtractorConfiguration, FrenchDateTimePeriodParserConfiguration } from "./dateTimePeriodConfiguration";
import { FrenchSetExtractorConfiguration, FrenchSetParserConfiguration } from "./setConfiguration";

export class FrenchMergedExtractorConfiguration implements IMergedExtractorConfiguration {
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
    readonly numberEndingPattern: RegExp

    constructor() {
        this.beforeRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.BeforeRegex, "gis");
        this.afterRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.AfterRegex, "gis");
        this.sinceRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.SinceRegex, "gis");
        this.fromToRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.FromToRegex, "gis");
        this.singleAmbiguousMonthRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.SingleAmbiguousMonthRegex, "gis");
        this.prepositionSuffixRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.PrepositionSuffixRegex, "gis");
        this.numberEndingPattern = RegExpUtility.getSafeRegExp(FrenchDateTime.NumberEndingPattern);

        this.dateExtractor = new BaseDateExtractor(new FrenchDateExtractorConfiguration());
        this.timeExtractor = new BaseTimeExtractor(new FrenchTimeExtractorConfiguration());
        this.dateTimeExtractor = new BaseDateTimeExtractor(new FrenchDateTimeExtractorConfiguration());
        this.datePeriodExtractor = new BaseDatePeriodExtractor(new FrenchDatePeriodExtractorConfiguration());
        this.timePeriodExtractor = new BaseTimePeriodExtractor(new FrenchTimePeriodExtractorConfiguration());
        this.dateTimePeriodExtractor = new BaseDateTimePeriodExtractor(new FrenchDateTimePeriodExtractorConfiguration());
        this.durationExtractor = new BaseDurationExtractor(new FrenchDurationExtractorConfiguration());
        this.setExtractor = new BaseSetExtractor(new FrenchSetExtractorConfiguration());
        this.holidayExtractor = new BaseHolidayExtractor(new FrenchHolidayExtractorConfiguration());
        this.integerExtractor = new FrenchIntegerExtractor();
    }
}

export class FrenchMergedParserConfiguration extends FrenchCommonDateTimeParserConfiguration implements IMergedParserConfiguration {
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

    constructor() {
        super();

        this.beforeRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.BeforeRegex, "gis");
        this.afterRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.AfterRegex, "gis");
        this.sinceRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.SinceRegex, "gis");

        this.datePeriodParser = new BaseDatePeriodParser(new FrenchDatePeriodParserConfiguration(this));
        this.timePeriodParser = new BaseTimePeriodParser(new FrenchTimePeriodParserConfiguration(this));
        this.setParser = new BaseSetParser(new FrenchSetParserConfiguration(this));
        this.holidayParser = new BaseHolidayParser(new FrenchHolidayParserConfiguration());
    }
}