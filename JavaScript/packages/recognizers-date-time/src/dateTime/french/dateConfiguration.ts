import { RegExpUtility } from "@microsoft/recognizers-text";
import { BaseNumberExtractor, BaseNumberParser, FrenchOrdinalExtractor, FrenchIntegerExtractor, FrenchNumberParserConfiguration } from "@microsoft/recognizers-text-number";
import { FrenchDateTime } from "../../resources/frenchDateTime";
import { IDateExtractorConfiguration, IDateParserConfiguration } from "../baseDate";
import { BaseDurationExtractor, BaseDurationParser } from "../baseDuration";
import { IDateTimeUtilityConfiguration } from "../utilities";
import { FrenchDateTimeUtilityConfiguration, FrenchCommonDateTimeParserConfiguration } from "./baseConfiguration";
import { FrenchDurationExtractorConfiguration } from "./durationConfiguration";
import { IDateTimeExtractor } from "../baseDateTime";
import { Constants } from "../constants";

export class FrenchDateExtractorConfiguration implements IDateExtractorConfiguration {
    readonly dateRegexList: RegExp[];
    readonly implicitDateList: RegExp[];
    readonly monthEnd: RegExp;
    readonly ofMonth: RegExp;
    readonly dateUnitRegex: RegExp;
    readonly forTheRegex: RegExp;
    readonly weekDayAndDayOfMonthRegex: RegExp;
    readonly relativeMonthRegex: RegExp;
    readonly strictRelativeRegex: RegExp;
    readonly weekDayRegex: RegExp;
    readonly dayOfWeek: ReadonlyMap<string, number>;
    readonly nonDateUnitRegex: RegExp;
    readonly ordinalExtractor: BaseNumberExtractor;
    readonly integerExtractor: BaseNumberExtractor;
    readonly numberParser: BaseNumberParser;
    readonly durationExtractor: IDateTimeExtractor;
    readonly utilityConfiguration: IDateTimeUtilityConfiguration;

    constructor(dmyDateFormat: boolean) {

        let enableDmy = dmyDateFormat || FrenchDateTime.DefaultLanguageFallback === Constants.DefaultLanguageFallback_DMY;

        this.dateRegexList = [
            RegExpUtility.getSafeRegExp(FrenchDateTime.DateExtractor1, "gis"),
            RegExpUtility.getSafeRegExp(FrenchDateTime.DateExtractor2, "gis"),
            RegExpUtility.getSafeRegExp(FrenchDateTime.DateExtractor3, "gis"),

            enableDmy ?
                RegExpUtility.getSafeRegExp(FrenchDateTime.DateExtractor5, "gis") :
                RegExpUtility.getSafeRegExp(FrenchDateTime.DateExtractor4, "gis"),

            enableDmy ?
                RegExpUtility.getSafeRegExp(FrenchDateTime.DateExtractor4, "gis") :
                RegExpUtility.getSafeRegExp(FrenchDateTime.DateExtractor5, "gis"),

            enableDmy ?
                RegExpUtility.getSafeRegExp(FrenchDateTime.DateExtractor7, "gis") :
                RegExpUtility.getSafeRegExp(FrenchDateTime.DateExtractor6, "gis"),

            enableDmy ?
                RegExpUtility.getSafeRegExp(FrenchDateTime.DateExtractor6, "gis") :
                RegExpUtility.getSafeRegExp(FrenchDateTime.DateExtractor7, "gis"),

            RegExpUtility.getSafeRegExp(FrenchDateTime.DateExtractor8, "gis"),
            RegExpUtility.getSafeRegExp(FrenchDateTime.DateExtractor9, "gis"),
            RegExpUtility.getSafeRegExp(FrenchDateTime.DateExtractorA, "gis"),
        ];
        this.implicitDateList = [
            RegExpUtility.getSafeRegExp(FrenchDateTime.OnRegex, "gis"),
            RegExpUtility.getSafeRegExp(FrenchDateTime.RelaxedOnRegex, "gis"),
            RegExpUtility.getSafeRegExp(FrenchDateTime.SpecialDayRegex, "gis"),
            RegExpUtility.getSafeRegExp(FrenchDateTime.ThisRegex, "gis"),
            RegExpUtility.getSafeRegExp(FrenchDateTime.LastDateRegex, "gis"),
            RegExpUtility.getSafeRegExp(FrenchDateTime.NextDateRegex, "gis"),
            RegExpUtility.getSafeRegExp(FrenchDateTime.StrictWeekDay, "gis"),
            RegExpUtility.getSafeRegExp(FrenchDateTime.WeekDayOfMonthRegex, "gis"),
            RegExpUtility.getSafeRegExp(FrenchDateTime.SpecialDate, "gis")
        ];

        this.monthEnd = RegExpUtility.getSafeRegExp(FrenchDateTime.MonthEnd, "gis");
        this.ofMonth = RegExpUtility.getSafeRegExp(FrenchDateTime.OfMonth, "gis");
        this.dateUnitRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.DateUnitRegex, "gis");
        this.forTheRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.ForTheRegex, "gis");
        this.weekDayAndDayOfMonthRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.WeekDayAndDayOfMonthRegex, "gis");
        this.relativeMonthRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.RelativeMonthRegex, "gis");
        this.strictRelativeRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.StrictRelativeRegex, "gis");
        this.weekDayRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.WeekDayRegex, "gis");
        this.dayOfWeek = FrenchDateTime.DayOfWeek;
        this.ordinalExtractor = new FrenchOrdinalExtractor();
        this.integerExtractor = new FrenchIntegerExtractor();
        this.numberParser = new BaseNumberParser(new FrenchNumberParserConfiguration());
        this.durationExtractor = new BaseDurationExtractor(new FrenchDurationExtractorConfiguration());
        this.utilityConfiguration = new FrenchDateTimeUtilityConfiguration();
        this.nonDateUnitRegex = RegExpUtility.getSafeRegExp("(?<unit>heure|heures|hrs|secondes|seconde|secs|sec|minutes|minute|mins)\b", "gis");
    }
}

export class FrenchDateParserConfiguration implements IDateParserConfiguration {
    readonly ordinalExtractor: BaseNumberExtractor;
    readonly integerExtractor: BaseNumberExtractor;
    readonly cardinalExtractor: BaseNumberExtractor;
    readonly durationExtractor: IDateTimeExtractor;
    readonly durationParser: BaseDurationParser;
    readonly numberParser: BaseNumberParser;
    readonly monthOfYear: ReadonlyMap<string, number>;
    readonly dayOfMonth: ReadonlyMap<string, number>;
    readonly dayOfWeek: ReadonlyMap<string, number>;
    readonly unitMap: ReadonlyMap<string, string>;
    readonly cardinalMap: ReadonlyMap<string, number>;
    readonly dateRegex: RegExp[];
    readonly onRegex: RegExp;
    readonly specialDayRegex: RegExp;
    readonly specialDayWithNumRegex: RegExp;
    readonly nextRegex: RegExp;
    readonly unitRegex: RegExp;
    readonly strictWeekDay: RegExp;
    readonly monthRegex: RegExp;
    readonly weekDayRegex: RegExp;
    readonly lastRegex: RegExp;
    readonly thisRegex: RegExp;
    readonly weekDayOfMonthRegex: RegExp;
    readonly forTheRegex: RegExp;
    readonly weekDayAndDayOfMonthRegex: RegExp;
    readonly relativeMonthRegex: RegExp;
    readonly strictRelativeRegex: RegExp;
    readonly relativeWeekDayRegex: RegExp;
    readonly utilityConfiguration: IDateTimeUtilityConfiguration;
    readonly dateTokenPrefix: string;

    constructor(config: FrenchCommonDateTimeParserConfiguration, dmyDateFormat: boolean) {
        this.ordinalExtractor = config.ordinalExtractor;
        this.integerExtractor = config.integerExtractor;
        this.cardinalExtractor = config.cardinalExtractor;
        this.durationExtractor = config.durationExtractor;
        this.numberParser = config.numberParser;
        this.durationParser = config.durationParser;
        this.monthOfYear = config.monthOfYear;
        this.dayOfMonth = config.dayOfMonth;
        this.dayOfWeek = config.dayOfWeek;
        this.unitMap = config.unitMap;
        this.cardinalMap = config.cardinalMap;
        this.dateRegex = new FrenchDateExtractorConfiguration(dmyDateFormat).dateRegexList;
        this.onRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.OnRegex, "gis");
        this.specialDayRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.SpecialDayRegex, "gis");
        this.specialDayWithNumRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.SpecialDayWithNumRegex, "gis");
        this.nextRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.NextDateRegex, "gis");
        this.unitRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.DateUnitRegex, "gis");
        this.monthRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.MonthRegex, "gis");
        this.weekDayRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.WeekDayRegex, "gis");
        this.strictWeekDay = RegExpUtility.getSafeRegExp(FrenchDateTime.StrictWeekDay, "gis");
        this.lastRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.LastDateRegex, "gis");
        this.thisRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.ThisRegex, "gis");
        this.weekDayOfMonthRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.WeekDayOfMonthRegex, "gis");
        this.forTheRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.ForTheRegex, "gis");
        this.weekDayAndDayOfMonthRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.WeekDayAndDayOfMonthRegex, "gis");
        this.relativeMonthRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.RelativeMonthRegex, "gis");
        this.strictRelativeRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.StrictRelativeRegex, "gis");
        this.relativeWeekDayRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.RelativeWeekDayRegex, "gis");
        this.utilityConfiguration = config.utilityConfiguration;
        this.dateTokenPrefix = FrenchDateTime.DateTokenPrefix;
    }

    getSwiftDay(source: string): number {

        let trimedText = source.trim().toLowerCase();
        let swift = 0;

        if (trimedText === "aujourd'hui" || trimedText === "auj") {
            swift = 0;
        }
        else if (trimedText === "demain" ||
            trimedText.endsWith("a2m1") ||
            trimedText.endsWith("lendemain") ||
            trimedText.endsWith("jour suivant")) {
            swift = 1;
        }
        else if (trimedText === "hier") {
            swift = -1;
        }
        else if (trimedText.endsWith("après demain") ||
            trimedText.endsWith("après-demain") ||
            trimedText.endsWith("apres-demain")) {
            swift = 2;
        }
        else if (trimedText.endsWith("avant-hier") ||
            trimedText.endsWith("avant hier")) {
            swift = -2;
        }
        else if (trimedText.endsWith("dernier")) {
            swift = -1;
        }

        return swift;
    }

    getSwiftMonthOrYear(source: string): number {
        let trimedText = source.trim().toLowerCase();
        let swift = 0;
        if (trimedText.endsWith("prochaine") || trimedText.endsWith("prochain")) {
            swift = 1;
        }
        else if (trimedText === "dernière" ||
            trimedText.endsWith("dernières") ||
            trimedText.endsWith("derniere") ||
            trimedText.endsWith("dernieres")) {
            swift = -1;
        }

        return swift;
    }

    isCardinalLast(source: string): boolean {
        let trimedText = source.trim().toLowerCase();
        return (trimedText.endsWith("dernière") ||
            trimedText.endsWith("dernières") ||
            trimedText.endsWith("derniere") ||
            trimedText.endsWith("dernieres"));
    }
}