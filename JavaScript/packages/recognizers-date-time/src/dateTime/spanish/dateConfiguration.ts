import { RegExpUtility } from "@microsoft/recognizers-text";
import { BaseNumberExtractor, BaseNumberParser, SpanishOrdinalExtractor, SpanishIntegerExtractor, SpanishNumberParserConfiguration } from "@microsoft/recognizers-text-number";
import { SpanishDateTime } from "../../resources/spanishDateTime";
import { IDateExtractorConfiguration, IDateParserConfiguration } from "../baseDate";
import { BaseDurationExtractor, BaseDurationParser } from "../baseDuration";
import { IDateTimeUtilityConfiguration } from "../utilities";
import { SpanishDateTimeUtilityConfiguration, SpanishCommonDateTimeParserConfiguration } from "./baseConfiguration";
import { SpanishDurationExtractorConfiguration } from "./durationConfiguration";
import { IDateTimeExtractor } from "../baseDateTime";
import { Constants } from "../constants";

export class SpanishDateExtractorConfiguration implements IDateExtractorConfiguration {
    readonly dateRegexList: RegExp[];
    readonly implicitDateList: RegExp[];
    readonly monthEnd: RegExp;
    readonly ofMonth: RegExp;
    readonly dateUnitRegex: RegExp;
    readonly forTheRegex: RegExp;
    readonly weekDayAndDayOfMonthRegex: RegExp;
    readonly relativeMonthRegex: RegExp;
    readonly weekDayRegex: RegExp;
    readonly dayOfWeek: ReadonlyMap<string, number>;
    readonly ordinalExtractor: BaseNumberExtractor;
    readonly integerExtractor: BaseNumberExtractor;
    readonly numberParser: BaseNumberParser;
    readonly durationExtractor: IDateTimeExtractor;
    readonly utilityConfiguration: IDateTimeUtilityConfiguration;

    constructor() {
        this.dateRegexList = [
            RegExpUtility.getSafeRegExp(SpanishDateTime.DateExtractor1, "gis"),
            RegExpUtility.getSafeRegExp(SpanishDateTime.DateExtractor2, "gis"),
            RegExpUtility.getSafeRegExp(SpanishDateTime.DateExtractor3, "gis"),

            SpanishDateTime.DefaultLanguageFallback === Constants.DefaultLanguageFallback_DMY?
                RegExpUtility.getSafeRegExp(SpanishDateTime.DateExtractor5, "gis"):
                RegExpUtility.getSafeRegExp(SpanishDateTime.DateExtractor4, "gis"),

            SpanishDateTime.DefaultLanguageFallback === Constants.DefaultLanguageFallback_DMY?
                RegExpUtility.getSafeRegExp(SpanishDateTime.DateExtractor4, "gis"):
                RegExpUtility.getSafeRegExp(SpanishDateTime.DateExtractor5, "gis"),

            SpanishDateTime.DefaultLanguageFallback === Constants.DefaultLanguageFallback_DMY?
                RegExpUtility.getSafeRegExp(SpanishDateTime.DateExtractor8, "gis"):
                RegExpUtility.getSafeRegExp(SpanishDateTime.DateExtractor6, "gis"),

            SpanishDateTime.DefaultLanguageFallback === Constants.DefaultLanguageFallback_DMY?
                RegExpUtility.getSafeRegExp(SpanishDateTime.DateExtractor6, "gis"):
                RegExpUtility.getSafeRegExp(SpanishDateTime.DateExtractor8, "gis"),

            SpanishDateTime.DefaultLanguageFallback === Constants.DefaultLanguageFallback_DMY?
                RegExpUtility.getSafeRegExp(SpanishDateTime.DateExtractor9, "gis"):
                RegExpUtility.getSafeRegExp(SpanishDateTime.DateExtractor7, "gis"),

            SpanishDateTime.DefaultLanguageFallback === Constants.DefaultLanguageFallback_DMY?
                RegExpUtility.getSafeRegExp(SpanishDateTime.DateExtractor7, "gis"):
                RegExpUtility.getSafeRegExp(SpanishDateTime.DateExtractor9, "gis"),
                
            RegExpUtility.getSafeRegExp(SpanishDateTime.DateExtractor10, "gis"),
        ];
        this.implicitDateList = [
            RegExpUtility.getSafeRegExp(SpanishDateTime.OnRegex, "gis"),
            RegExpUtility.getSafeRegExp(SpanishDateTime.RelaxedOnRegex, "gis"),
            RegExpUtility.getSafeRegExp(SpanishDateTime.SpecialDayRegex, "gis"),
            RegExpUtility.getSafeRegExp(SpanishDateTime.ThisRegex, "gis"),
            RegExpUtility.getSafeRegExp(SpanishDateTime.LastDateRegex, "gis"),
            RegExpUtility.getSafeRegExp(SpanishDateTime.NextDateRegex, "gis"),
            RegExpUtility.getSafeRegExp(SpanishDateTime.WeekDayRegex, "gis"),
            RegExpUtility.getSafeRegExp(SpanishDateTime.WeekDayOfMonthRegex, "gis"),
            RegExpUtility.getSafeRegExp(SpanishDateTime.SpecialDateRegex, "gis")
        ];

        this.monthEnd = RegExpUtility.getSafeRegExp(SpanishDateTime.MonthEndRegex, "gis");
        this.ofMonth = RegExpUtility.getSafeRegExp(SpanishDateTime.OfMonthRegex, "gis");
        this.dateUnitRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.DateUnitRegex, "gis");
        this.forTheRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.ForTheRegex, "gis");
        this.weekDayAndDayOfMonthRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.WeekDayAndDayOfMonthRegex, "gis");
        this.relativeMonthRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.RelativeMonthRegex, "gis");
        this.weekDayRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.WeekDayRegex, "gis");
        this.dayOfWeek = SpanishDateTime.DayOfWeek;
        this.ordinalExtractor = new SpanishOrdinalExtractor();
        this.integerExtractor = new SpanishIntegerExtractor();
        this.numberParser = new BaseNumberParser(new SpanishNumberParserConfiguration());
        this.durationExtractor = new BaseDurationExtractor(new SpanishDurationExtractorConfiguration());
        this.utilityConfiguration = new SpanishDateTimeUtilityConfiguration();
    }
}

export class SpanishDateParserConfiguration implements IDateParserConfiguration {
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
    readonly monthRegex: RegExp;
    readonly weekDayRegex: RegExp;
    readonly lastRegex: RegExp;
    readonly thisRegex: RegExp;
    readonly weekDayOfMonthRegex: RegExp;
    readonly forTheRegex: RegExp;
    readonly weekDayAndDayOfMonthRegex: RegExp;
    readonly relativeMonthRegex: RegExp;
    readonly relativeWeekDayRegex: RegExp;
    readonly utilityConfiguration: IDateTimeUtilityConfiguration;
    readonly dateTokenPrefix: string;

    // TODO: implement the relative day regex if needed. If yes, they should be abstracted
    static readonly relativeDayRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.RelativeDayRegex);
    static readonly nextPrefixRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.NextPrefixRegex);
    static readonly previousPrefixRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.PreviousPrefixRegex);

    constructor(config: SpanishCommonDateTimeParserConfiguration) {
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
        this.dateRegex = new SpanishDateExtractorConfiguration().dateRegexList;
        this.onRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.OnRegex, "gis");
        this.specialDayRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.SpecialDayRegex, "gis");
        this.specialDayWithNumRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.SpecialDayWithNumRegex, "gis");
        this.nextRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.NextDateRegex, "gis");
        this.unitRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.DateUnitRegex, "gis");
        this.monthRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.MonthRegex, "gis");
        this.weekDayRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.WeekDayRegex, "gis");
        this.lastRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.LastDateRegex, "gis");
        this.thisRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.ThisRegex, "gis");
        this.weekDayOfMonthRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.WeekDayOfMonthRegex, "gis");
        this.forTheRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.ForTheRegex, "gis");
        this.weekDayAndDayOfMonthRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.WeekDayAndDayOfMonthRegex, "gis");
        this.relativeMonthRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.RelativeMonthRegex, "gis");
        this.relativeWeekDayRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.RelativeWeekDayRegex, "gis");
        this.utilityConfiguration = config.utilityConfiguration;
        this.dateTokenPrefix = SpanishDateTime.DateTokenPrefix;
    }

    getSwiftDay(source: string): number {

        let trimedText = SpanishDateParserConfiguration.normalize(source.trim().toLowerCase());
        let swift = 0;

        // TODO: add the relative day logic if needed. If yes, the whole method should be abstracted.
        if (trimedText === "hoy" || trimedText === "el dia") {
            swift = 0;
        } else if (trimedText === "mañana" ||
            trimedText.endsWith("dia siguiente") ||
            trimedText.endsWith("el dia de mañana") ||
            trimedText.endsWith("proximo dia")) {
            swift = 1;
        } else if (trimedText === "ayer") {
            swift = -1;
        } else if (trimedText.endsWith("pasado mañana") ||
            trimedText.endsWith("dia despues de mañana")) {
            swift = 2;
        } else if (trimedText.endsWith("anteayer") ||
            trimedText.endsWith("dia antes de ayer")) {
            swift = -2;
        } else if (trimedText.endsWith("ultimo dia")) {
            swift = -1;
        }

        return swift;
    }

    getSwiftMonth(source: string): number {
        let trimedText = source.trim().toLowerCase();
        let swift = 0;
        if (RegExpUtility.getMatches(SpanishDateParserConfiguration.nextPrefixRegex, trimedText).length) {
            swift = 1;
        }

        if (RegExpUtility.getMatches(SpanishDateParserConfiguration.previousPrefixRegex, trimedText).length) {
            swift = -1;
        }

        return swift;
    }

    isCardinalLast(source: string): boolean {
        let trimedText = source.trim().toLowerCase();
        return RegExpUtility.getMatches(SpanishDateParserConfiguration.previousPrefixRegex, trimedText).length > 0;
    }

    private static normalize(source: string): string {
        return source
            .replace(/á/g, "a")
            .replace(/é/g, "e")
            .replace(/í/g, "i")
            .replace(/ó/g, "o")
            .replace(/ú/g, "u");
    }
}