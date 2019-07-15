import { RegExpUtility } from "@microsoft/recognizers-text";
import { FrenchCardinalExtractor, FrenchIntegerExtractor, FrenchOrdinalExtractor, BaseNumberParser, FrenchNumberParserConfiguration } from "@microsoft/recognizers-text-number";
import { IDateTimeUtilityConfiguration } from "../utilities";
import { FrenchDateTime } from "../../resources/frenchDateTime";
import { BaseDateParserConfiguration } from "../parsers";
import { BaseDateTime } from "../../resources/baseDateTime";
import { BaseDateExtractor, BaseDateParser } from "../baseDate";
import { BaseTimeExtractor, BaseTimeParser } from "../baseTime";
import { BaseDateTimeExtractor, BaseDateTimeParser } from "../baseDateTime";
import { BaseDurationExtractor, BaseDurationParser } from "../baseDuration";
import { BaseDatePeriodExtractor, BaseDatePeriodParser } from "../baseDatePeriod";
import { BaseTimePeriodExtractor, BaseTimePeriodParser } from "../baseTimePeriod";
import { BaseDateTimePeriodExtractor, BaseDateTimePeriodParser } from "../baseDateTimePeriod";
import { FrenchDateExtractorConfiguration, FrenchDateParserConfiguration } from "./dateConfiguration";
import { FrenchDateTimeExtractorConfiguration, FrenchDateTimeParserConfiguration } from "./dateTimeConfiguration";
import { FrenchDurationExtractorConfiguration, FrenchDurationParserConfiguration } from "./durationConfiguration";
import { FrenchDatePeriodExtractorConfiguration, FrenchDatePeriodParserConfiguration } from "./datePeriodConfiguration";
import { FrenchTimeExtractorConfiguration, FrenchTimeParserConfiguration } from "./timeConfiguration";
import { FrenchTimePeriodExtractorConfiguration, FrenchTimePeriodParserConfiguration } from "./timePeriodConfiguration";
import { FrenchDateTimePeriodExtractorConfiguration, FrenchDateTimePeriodParserConfiguration } from "./dateTimePeriodConfiguration";

export class FrenchDateTimeUtilityConfiguration implements IDateTimeUtilityConfiguration {
    readonly agoRegex: RegExp;
    readonly laterRegex: RegExp;
    readonly inConnectorRegex: RegExp;
    readonly rangeUnitRegex: RegExp;
    readonly amDescRegex: RegExp;
    readonly pmDescRegex: RegExp;
    readonly amPmDescRegex: RegExp;

    constructor() {
        this.laterRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.LaterRegex);
        this.agoRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.AgoPrefixRegex);
        this.inConnectorRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.InConnectorRegex);
        this.rangeUnitRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.RangeUnitRegex);
        this.amDescRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.AmDescRegex);
        this.pmDescRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.PmDescRegex);
        this.amPmDescRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.AmPmDescRegex);
    }
}

export class FrenchCommonDateTimeParserConfiguration extends BaseDateParserConfiguration {

    constructor(dmyDateFormat: boolean) {
        super();
        this.utilityConfiguration = new FrenchDateTimeUtilityConfiguration();

        this.unitMap = FrenchDateTime.UnitMap;
        this.unitValueMap = FrenchDateTime.UnitValueMap;
        this.seasonMap = FrenchDateTime.SeasonMap;
        this.cardinalMap = FrenchDateTime.CardinalMap;
        this.dayOfWeek = FrenchDateTime.DayOfWeek;
        this.monthOfYear = FrenchDateTime.MonthOfYear;
        this.numbers = FrenchDateTime.Numbers;
        this.doubleNumbers = FrenchDateTime.DoubleNumbers;

        this.cardinalExtractor = new FrenchCardinalExtractor();
        this.integerExtractor = new FrenchIntegerExtractor();
        this.ordinalExtractor = new FrenchOrdinalExtractor();

        this.numberParser = new BaseNumberParser(new FrenchNumberParserConfiguration());
        this.dateExtractor = new BaseDateExtractor(new FrenchDateExtractorConfiguration(dmyDateFormat));
        this.timeExtractor = new BaseTimeExtractor(new FrenchTimeExtractorConfiguration());
        this.dateTimeExtractor = new BaseDateTimeExtractor(new FrenchDateTimeExtractorConfiguration(dmyDateFormat));
        this.durationExtractor = new BaseDurationExtractor(new FrenchDurationExtractorConfiguration());
        this.datePeriodExtractor = new BaseDatePeriodExtractor(new FrenchDatePeriodExtractorConfiguration(dmyDateFormat));
        this.timePeriodExtractor = new BaseTimePeriodExtractor(new FrenchTimePeriodExtractorConfiguration());
        this.dateTimePeriodExtractor = new BaseDateTimePeriodExtractor(new FrenchDateTimePeriodExtractorConfiguration(dmyDateFormat));
        this.durationParser = new BaseDurationParser(new FrenchDurationParserConfiguration(this));
        this.dateParser = new BaseDateParser(new FrenchDateParserConfiguration(this, dmyDateFormat));
        this.timeParser = new BaseTimeParser(new FrenchTimeParserConfiguration(this));
        this.dateTimeParser = new BaseDateTimeParser(new FrenchDateTimeParserConfiguration(this));
        this.datePeriodParser = new BaseDatePeriodParser(new FrenchDatePeriodParserConfiguration(this));
        this.timePeriodParser = new BaseTimePeriodParser(new FrenchTimePeriodParserConfiguration(this));
        this.dateTimePeriodParser = new BaseDateTimePeriodParser(new FrenchDateTimePeriodParserConfiguration(this));
        this.dayOfMonth = new Map<string, number>([...BaseDateTime.DayOfMonthDictionary, ...FrenchDateTime.DayOfMonth]);
    }
}