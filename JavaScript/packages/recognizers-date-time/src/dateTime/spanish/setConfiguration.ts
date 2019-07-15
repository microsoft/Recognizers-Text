import { RegExpUtility } from "@microsoft/recognizers-text";
import { ISetExtractorConfiguration, ISetParserConfiguration } from "../baseSet";
import { BaseDurationExtractor, BaseDurationParser } from "../baseDuration";
import { BaseTimeExtractor, BaseTimeParser } from "../baseTime";
import { BaseDateExtractor, BaseDateParser } from "../baseDate";
import { BaseDateTimeExtractor, BaseDateTimeParser, IDateTimeExtractor } from "../baseDateTime";
import { BaseDatePeriodExtractor, BaseDatePeriodParser } from "../baseDatePeriod";
import { BaseTimePeriodExtractor, BaseTimePeriodParser } from "../baseTimePeriod";
import { BaseDateTimePeriodExtractor, BaseDateTimePeriodParser } from "../baseDateTimePeriod";
import { SpanishDateTime } from "../../resources/spanishDateTime";
import { SpanishDurationExtractorConfiguration } from "./durationConfiguration";
import { SpanishDateExtractorConfiguration } from "./dateConfiguration";
import { SpanishDateTimeExtractorConfiguration } from "./dateTimeConfiguration";
import { SpanishDatePeriodExtractorConfiguration } from "./datePeriodConfiguration";
import { SpanishDateTimePeriodExtractorConfiguration } from "./dateTimePeriodConfiguration";
import { ICommonDateTimeParserConfiguration } from "../parsers";
import { SpanishTimeExtractorConfiguration } from "./timeConfiguration";
import { SpanishTimePeriodExtractorConfiguration } from "./timePeriodConfiguration";

export class SpanishSetExtractorConfiguration implements ISetExtractorConfiguration {
    readonly lastRegex: RegExp;
    readonly eachPrefixRegex: RegExp;
    readonly periodicRegex: RegExp;
    readonly eachUnitRegex: RegExp;
    readonly eachDayRegex: RegExp;
    readonly beforeEachDayRegex: RegExp;
    readonly setWeekDayRegex: RegExp;
    readonly setEachRegex: RegExp;
    readonly durationExtractor: IDateTimeExtractor;
    readonly timeExtractor: IDateTimeExtractor;
    readonly dateExtractor: IDateTimeExtractor;
    readonly dateTimeExtractor: IDateTimeExtractor;
    readonly datePeriodExtractor: IDateTimeExtractor;
    readonly timePeriodExtractor: IDateTimeExtractor;
    readonly dateTimePeriodExtractor: IDateTimeExtractor;

    constructor(dmyDateFormat: boolean) {
        this.lastRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.LastDateRegex, "gis");
        this.periodicRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.PeriodicRegex, "gis");
        this.eachUnitRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.EachUnitRegex, "gis");
        this.eachPrefixRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.EachPrefixRegex, "gis");
        this.eachDayRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.EachDayRegex, "gis");
        this.beforeEachDayRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.BeforeEachDayRegex, "gis");
        this.setEachRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.SetEachRegex, "gis");
        this.setWeekDayRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.SetWeekDayRegex, "gis");

        this.durationExtractor = new BaseDurationExtractor(new SpanishDurationExtractorConfiguration());
        this.timeExtractor = new BaseTimeExtractor(new SpanishTimeExtractorConfiguration());
        this.dateExtractor = new BaseDateExtractor(new SpanishDateExtractorConfiguration(dmyDateFormat));
        this.dateTimeExtractor = new BaseDateTimeExtractor(new SpanishDateTimeExtractorConfiguration(dmyDateFormat));
        this.datePeriodExtractor = new BaseDatePeriodExtractor(new SpanishDatePeriodExtractorConfiguration(dmyDateFormat));
        this.timePeriodExtractor = new BaseTimePeriodExtractor(new SpanishTimePeriodExtractorConfiguration());
        this.dateTimePeriodExtractor = new BaseDateTimePeriodExtractor(new SpanishDateTimePeriodExtractorConfiguration(dmyDateFormat));
    }
}

export class SpanishSetParserConfiguration implements ISetParserConfiguration {
    readonly durationExtractor: IDateTimeExtractor;
    readonly durationParser: BaseDurationParser;
    readonly timeExtractor: IDateTimeExtractor;
    readonly timeParser: BaseTimeParser;
    readonly dateExtractor: IDateTimeExtractor;
    readonly dateParser: BaseDateParser;
    readonly dateTimeExtractor: IDateTimeExtractor;
    readonly dateTimeParser: BaseDateTimeParser;
    readonly datePeriodExtractor: IDateTimeExtractor;
    readonly datePeriodParser: BaseDatePeriodParser;
    readonly timePeriodExtractor: IDateTimeExtractor;
    readonly timePeriodParser: BaseTimePeriodParser;
    readonly dateTimePeriodExtractor: IDateTimeExtractor;
    readonly dateTimePeriodParser: BaseDateTimePeriodParser;
    readonly unitMap: ReadonlyMap<string, string>;
    readonly eachPrefixRegex: RegExp;
    readonly periodicRegex: RegExp;
    readonly eachUnitRegex: RegExp;
    readonly eachDayRegex: RegExp;
    readonly setWeekDayRegex: RegExp;
    readonly setEachRegex: RegExp;

    constructor(config: ICommonDateTimeParserConfiguration) {
        this.durationExtractor = config.durationExtractor;
        this.timeExtractor = config.timeExtractor;
        this.dateExtractor = config.dateExtractor;
        this.dateTimeExtractor = config.dateTimeExtractor;
        this.datePeriodExtractor = config.datePeriodExtractor;
        this.timePeriodExtractor = config.timePeriodExtractor;
        this.dateTimePeriodExtractor = config.dateTimePeriodExtractor;
        this.durationParser = config.durationParser;
        this.timeParser = config.timeParser;
        this.dateParser = config.dateParser;
        this.dateTimeParser = config.dateTimeParser;
        this.datePeriodParser = config.datePeriodParser;
        this.timePeriodParser = config.timePeriodParser;
        this.dateTimePeriodParser = config.dateTimePeriodParser;
        this.unitMap = config.unitMap;

        this.eachPrefixRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.EachPrefixRegex, "gis");
        this.periodicRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.PeriodicRegex, "gis");
        this.eachUnitRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.EachUnitRegex, "gis");
        this.eachDayRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.EachDayRegex, "gis");
        this.setWeekDayRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.SetWeekDayRegex, "gis");
        this.setEachRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.SetEachRegex, "gis");
    }

    getMatchedDailyTimex(text: string): { matched: boolean; timex: string; } {
        let trimedText = text.trim().toLowerCase();
        let timex = "";

        if (trimedText.endsWith("diario") || trimedText.endsWith("diariamente")) {
            timex = "P1D";
        }
        else if (trimedText === "semanalmente") {
            timex = "P1W";
        }
        else if (trimedText === "quincenalmente") {
            timex = "P2W";
        }
        else if (trimedText === "mensualmente") {
            timex = "P1M";
        }
        else if (trimedText === "anualmente") {
            timex = "P1Y";
        }
        else {
            timex = null;
            return {
                timex,
                matched: false
            };
        }

        return {
            timex,
            matched: true
        }
    }

    getMatchedUnitTimex(text: string): { matched: boolean; timex: string; } {
        let trimedText = text.trim().toLowerCase();
        let timex = "";

        if (trimedText === "día" || trimedText === "dia" ||
            trimedText === "días" || trimedText === "dias") {
            timex = "P1D";
        }
        else if (trimedText === "semana" || trimedText === "semanas") {
            timex = "P1W";
        }
        else if (trimedText === "mes" || trimedText === "meses") {
            timex = "P1M";
        }
        else if (trimedText === "año" || trimedText === "años") {
            timex = "P1Y";
        }
        else {
            timex = null;
            return {
                matched: false,
                timex
            };
        }

        return {
            matched: true,
            timex
        };
    }
}