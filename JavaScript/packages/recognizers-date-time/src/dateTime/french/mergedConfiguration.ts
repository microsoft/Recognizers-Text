import { RegExpUtility } from "@microsoft/recognizers-text";
import { BaseNumberExtractor, FrenchIntegerExtractor } from "@microsoft/recognizers-text-number";
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
    readonly ambiguousRangeModifierPrefix: RegExp;
    readonly potentialAmbiguousRangeRegex: RegExp;
    readonly numberEndingPattern: RegExp;
    readonly unspecificDatePeriodRegex: RegExp;
    readonly filterWordRegexList: RegExp[];

    constructor(dmyDateFormat: boolean = false) {
        this.beforeRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.BeforeRegex);
        this.afterRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.AfterRegex);
        this.sinceRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.SinceRegex);
        this.fromToRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.FromToRegex);
        this.singleAmbiguousMonthRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.SingleAmbiguousMonthRegex);
        this.prepositionSuffixRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.PrepositionSuffixRegex);
        this.ambiguousRangeModifierPrefix = null;
        this.potentialAmbiguousRangeRegex = null;
        this.numberEndingPattern = RegExpUtility.getSafeRegExp(FrenchDateTime.NumberEndingPattern);
        this.unspecificDatePeriodRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.UnspecificDatePeriodRegex);

        this.dateExtractor = new BaseDateExtractor(new FrenchDateExtractorConfiguration(dmyDateFormat));
        this.timeExtractor = new BaseTimeExtractor(new FrenchTimeExtractorConfiguration());
        this.dateTimeExtractor = new BaseDateTimeExtractor(new FrenchDateTimeExtractorConfiguration(dmyDateFormat));
        this.datePeriodExtractor = new BaseDatePeriodExtractor(new FrenchDatePeriodExtractorConfiguration(dmyDateFormat));
        this.timePeriodExtractor = new BaseTimePeriodExtractor(new FrenchTimePeriodExtractorConfiguration());
        this.dateTimePeriodExtractor = new BaseDateTimePeriodExtractor(new FrenchDateTimePeriodExtractorConfiguration(dmyDateFormat));
        this.durationExtractor = new BaseDurationExtractor(new FrenchDurationExtractorConfiguration());
        this.setExtractor = new BaseSetExtractor(new FrenchSetExtractorConfiguration(dmyDateFormat));
        this.holidayExtractor = new BaseHolidayExtractor(new FrenchHolidayExtractorConfiguration());
        this.integerExtractor = new FrenchIntegerExtractor();
        this.filterWordRegexList = [];
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

    constructor(dmyDateFormat: boolean = false) {
        super(dmyDateFormat);

        this.beforeRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.BeforeRegex);
        this.afterRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.AfterRegex);
        this.sinceRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.SinceRegex);

        this.datePeriodParser = new BaseDatePeriodParser(new FrenchDatePeriodParserConfiguration(this));
        this.timePeriodParser = new BaseTimePeriodParser(new FrenchTimePeriodParserConfiguration(this));
        this.setParser = new BaseSetParser(new FrenchSetParserConfiguration(this));
        this.holidayParser = new BaseHolidayParser(new FrenchHolidayParserConfiguration());
    }
}