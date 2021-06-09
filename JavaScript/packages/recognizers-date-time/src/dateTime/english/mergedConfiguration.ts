import { IMergedExtractorConfiguration, IMergedParserConfiguration } from "../baseMerged";
import { BaseDateExtractor, BaseDateParser } from "../baseDate";
import { BaseTimeExtractor, BaseTimeParser } from "../baseTime";
import { BaseSetExtractor, BaseSetParser } from "../baseSet";
import { BaseHolidayExtractor, BaseHolidayParser } from "../baseHoliday";
import { BaseDatePeriodExtractor, BaseDatePeriodParser } from "../baseDatePeriod";
import { BaseTimePeriodExtractor, BaseTimePeriodParser } from "../baseTimePeriod";
import { IDateTimeExtractor, BaseDateTimeExtractor, BaseDateTimeParser } from "../baseDateTime";
import { BaseDateTimePeriodExtractor, BaseDateTimePeriodParser } from "../baseDateTimePeriod";
import { BaseDurationExtractor, BaseDurationParser } from "../baseDuration";
import { RegExpUtility } from "@microsoft/recognizers-text";
import { BaseNumberExtractor, EnglishIntegerExtractor } from "@microsoft/recognizers-text-number";
import { EnglishDateTime } from "../../resources/englishDateTime";
import { EnglishCommonDateTimeParserConfiguration } from "./baseConfiguration";
import { EnglishDurationExtractorConfiguration } from "./durationConfiguration";
import { EnglishTimeExtractorConfiguration } from "./timeConfiguration";
import { EnglishDateExtractorConfiguration } from "./dateConfiguration";
import { EnglishDateTimeExtractorConfiguration } from "./dateTimeConfiguration";
import { EnglishTimePeriodExtractorConfiguration } from "./timePeriodConfiguration";
import { EnglishDatePeriodExtractorConfiguration } from "./datePeriodConfiguration";
import { EnglishDateTimePeriodExtractorConfiguration } from "./dateTimePeriodConfiguration";
import { EnglishSetExtractorConfiguration, EnglishSetParserConfiguration } from "./setConfiguration";
import { EnglishHolidayExtractorConfiguration, EnglishHolidayParserConfiguration } from "./holidayConfiguration";
import { DefinitionLoader } from "../utilities";

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
    readonly ambiguousRangeModifierPrefix: RegExp
    readonly potentialAmbiguousRangeRegex: RegExp
    readonly numberEndingPattern: RegExp
    readonly unspecificDatePeriodRegex: RegExp
    readonly filterWordRegexList: RegExp[]
    readonly AmbiguityFiltersDict: Map<RegExp, RegExp>

    constructor(dmyDateFormat: boolean = false) {
        this.dateExtractor = new BaseDateExtractor(new EnglishDateExtractorConfiguration(dmyDateFormat));
        this.timeExtractor = new BaseTimeExtractor(new EnglishTimeExtractorConfiguration());
        this.dateTimeExtractor = new BaseDateTimeExtractor(new EnglishDateTimeExtractorConfiguration(dmyDateFormat));
        this.datePeriodExtractor = new BaseDatePeriodExtractor(new EnglishDatePeriodExtractorConfiguration(dmyDateFormat));
        this.timePeriodExtractor = new BaseTimePeriodExtractor(new EnglishTimePeriodExtractorConfiguration());
        this.dateTimePeriodExtractor = new BaseDateTimePeriodExtractor(new EnglishDateTimePeriodExtractorConfiguration(dmyDateFormat));
        this.holidayExtractor = new BaseHolidayExtractor(new EnglishHolidayExtractorConfiguration());
        this.durationExtractor = new BaseDurationExtractor(new EnglishDurationExtractorConfiguration());
        this.setExtractor = new BaseSetExtractor(new EnglishSetExtractorConfiguration(dmyDateFormat));
        this.integerExtractor = new EnglishIntegerExtractor();
        this.afterRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.AfterRegex);
        this.sinceRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.SinceRegex);
        this.beforeRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.BeforeRegex);
        this.fromToRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.FromToRegex);
        this.singleAmbiguousMonthRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.SingleAmbiguousMonthRegex);
        this.prepositionSuffixRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.PrepositionSuffixRegex);
        this.ambiguousRangeModifierPrefix = RegExpUtility.getSafeRegExp(EnglishDateTime.AmbiguousRangeModifierPrefix);
        this.potentialAmbiguousRangeRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.FromToRegex);
        this.numberEndingPattern = RegExpUtility.getSafeRegExp(EnglishDateTime.NumberEndingPattern);
        this.unspecificDatePeriodRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.UnspecificDatePeriodRegex);
        this.filterWordRegexList = [
            RegExpUtility.getSafeRegExp(EnglishDateTime.OneOnOneRegex)
        ];
        this.AmbiguityFiltersDict = DefinitionLoader.LoadAmbiguityFilters(EnglishDateTime.AmbiguityFiltersDict);
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
