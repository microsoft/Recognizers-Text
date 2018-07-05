import { RegExpUtility } from "@microsoft/recognizers-text";
import { ISetExtractorConfiguration, ISetParserConfiguration } from "../baseSet";
import { BaseDurationExtractor, BaseDurationParser } from "../baseDuration";
import { BaseTimeExtractor, BaseTimeParser } from "../baseTime";
import { BaseDateExtractor, BaseDateParser } from "../baseDate";
import { IDateTimeExtractor, BaseDateTimeExtractor, BaseDateTimeParser } from "../baseDateTime";
import { BaseDatePeriodExtractor, BaseDatePeriodParser } from "../baseDatePeriod";
import { BaseTimePeriodExtractor, BaseTimePeriodParser } from "../baseTimePeriod";
import { BaseDateTimePeriodExtractor, BaseDateTimePeriodParser } from "../baseDateTimePeriod";
import { FrenchDateTime } from "../../resources/frenchDateTime";
import { FrenchDurationExtractorConfiguration } from "./durationConfiguration";
import { FrenchDateExtractorConfiguration } from "./dateConfiguration";
import { FrenchDateTimeExtractorConfiguration } from "./dateTimeConfiguration";
import { FrenchDatePeriodExtractorConfiguration } from "./datePeriodConfiguration";
import { FrenchDateTimePeriodExtractorConfiguration } from "./dateTimePeriodConfiguration";
import { ICommonDateTimeParserConfiguration } from "../parsers";
import { FrenchTimeExtractorConfiguration } from "./timeConfiguration";
import { FrenchTimePeriodExtractorConfiguration } from "./timePeriodConfiguration";

export class FrenchSetExtractorConfiguration implements ISetExtractorConfiguration {
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

    constructor() {
        this.lastRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.SetLastRegex, "gis");
        this.periodicRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.PeriodicRegex, "gis");
        this.eachUnitRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.EachUnitRegex, "gis");
        this.eachPrefixRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.EachPrefixRegex, "gis");
        this.eachDayRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.EachDayRegex, "gis");
        this.beforeEachDayRegex = null;
        this.setEachRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.SetEachRegex, "gis");
        this.setWeekDayRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.SetWeekDayRegex, "gis");

        this.durationExtractor = new BaseDurationExtractor(new FrenchDurationExtractorConfiguration());
        this.timeExtractor = new BaseTimeExtractor(new FrenchTimeExtractorConfiguration());
        this.dateExtractor = new BaseDateExtractor(new FrenchDateExtractorConfiguration());
        this.dateTimeExtractor = new BaseDateTimeExtractor(new FrenchDateTimeExtractorConfiguration());
        this.datePeriodExtractor = new BaseDatePeriodExtractor(new FrenchDatePeriodExtractorConfiguration());
        this.timePeriodExtractor = new BaseTimePeriodExtractor(new FrenchTimePeriodExtractorConfiguration());
        this.dateTimePeriodExtractor = new BaseDateTimePeriodExtractor(new FrenchDateTimePeriodExtractorConfiguration());
    }
}

export class FrenchSetParserConfiguration implements ISetParserConfiguration {
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

        this.eachPrefixRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.EachPrefixRegex, "gis");
        this.periodicRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.PeriodicRegex, "gis");
        this.eachUnitRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.EachUnitRegex, "gis");
        this.eachDayRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.EachDayRegex, "gis");
        this.setWeekDayRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.SetWeekDayRegex, "gis");
        this.setEachRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.SetEachRegex, "gis");
    }

    getMatchedDailyTimex(text: string): { matched: boolean; timex: string; } {
        let trimedText = text.trim().toLowerCase();
        let timex = "";

        if (trimedText === "quotidien" || trimedText === "quotidienne" || 
            trimedText === "jours" || trimedText === "journellement") {
            timex = "P1D";
        }
        else if (trimedText === "hebdomadaire") {
            timex = "P1W";
        }
        else if (trimedText === "bihebdomadaire") {
            timex = "P2W";
        }
        else if (trimedText === "mensuel" || trimedText === "mensuelle") {
            timex = "P1M";
        }
        else if (trimedText === "annuel" || trimedText === "annuellement") {
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

        if (trimedText === "jour" || trimedText === "journee") {
            timex = "P1D";
        }
        else if (trimedText === "semaine") {
            timex = "P1W";
        }
        else if (trimedText === "mois") {
            timex = "P1M";
        }
        else if (trimedText === "an" || trimedText === "annee") {
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