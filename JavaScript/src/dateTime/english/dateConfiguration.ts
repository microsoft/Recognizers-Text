import { IDateExtractorConfiguration, IDateParserConfiguration } from "../baseDate"
import { BaseDurationExtractor, BaseDurationParser } from "../baseDuration"
import { IDateTimeUtilityConfiguration } from "../utilities";
import { RegExpUtility } from "../../utilities";
import { BaseNumberParser } from "../../number/parsers"
import { BaseNumberExtractor } from "../../number/extractors"
import { EnglishOrdinalExtractor, EnglishIntegerExtractor } from "../../number/english/extractors"
import { EnglishDateTime } from "../../resources/englishDateTime";
import { EnglishNumberParserConfiguration } from "../../number/english/parserConfiguration"
import { EnglishCommonDateTimeParserConfiguration, EnglishDateTimeUtilityConfiguration } from "./baseConfiguration"
import { EnglishDurationExtractorConfiguration } from "./durationConfiguration"

export class EnglishDateExtractorConfiguration implements IDateExtractorConfiguration {
    readonly dateRegexList: RegExp[];
    readonly implicitDateList: RegExp[];
    readonly monthEnd: RegExp;
    readonly ofMonth: RegExp;
    readonly dateUnitRegex: RegExp;
    readonly ordinalExtractor: BaseNumberExtractor;
    readonly integerExtractor: BaseNumberExtractor;
    readonly numberParser: BaseNumberParser;
    readonly durationExtractor: BaseDurationExtractor;
    readonly utilityConfiguration: IDateTimeUtilityConfiguration;

    constructor() {
        this.dateRegexList = [
            RegExpUtility.getSafeRegExp(EnglishDateTime.DateExtractor1, "gis"),
            RegExpUtility.getSafeRegExp(EnglishDateTime.DateExtractor2, "gis"),
            RegExpUtility.getSafeRegExp(EnglishDateTime.DateExtractor3, "gis"),
            RegExpUtility.getSafeRegExp(EnglishDateTime.DateExtractor4, "gis"),
            RegExpUtility.getSafeRegExp(EnglishDateTime.DateExtractor5, "gis"),
            RegExpUtility.getSafeRegExp(EnglishDateTime.DateExtractor6, "gis"),
            RegExpUtility.getSafeRegExp(EnglishDateTime.DateExtractor7, "gis"),
            RegExpUtility.getSafeRegExp(EnglishDateTime.DateExtractor8, "gis"),
            RegExpUtility.getSafeRegExp(EnglishDateTime.DateExtractor9, "gis"),
            RegExpUtility.getSafeRegExp(EnglishDateTime.DateExtractorA, "gis"),
        ];
        this.implicitDateList = [
            RegExpUtility.getSafeRegExp(EnglishDateTime.OnRegex, "gis"),
            RegExpUtility.getSafeRegExp(EnglishDateTime.RelaxedOnRegex, "gis"),
            RegExpUtility.getSafeRegExp(EnglishDateTime.SpecialDayRegex, "gis"),
            RegExpUtility.getSafeRegExp(EnglishDateTime.ThisRegex, "gis"),
            RegExpUtility.getSafeRegExp(EnglishDateTime.LastDateRegex, "gis"),
            RegExpUtility.getSafeRegExp(EnglishDateTime.NextDateRegex, "gis"),
            RegExpUtility.getSafeRegExp(EnglishDateTime.StrictWeekDay, "gis"),
            RegExpUtility.getSafeRegExp(EnglishDateTime.WeekDayOfMonthRegex, "gis"),
            RegExpUtility.getSafeRegExp(EnglishDateTime.SpecialDate, "gis"),
        ];
        this.monthEnd = RegExpUtility.getSafeRegExp(EnglishDateTime.MonthEnd, "gis");
        this.ofMonth = RegExpUtility.getSafeRegExp(EnglishDateTime.OfMonth, "gis");
        this.dateUnitRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.DateUnitRegex, "gis");
        this.ordinalExtractor = new EnglishOrdinalExtractor();
        this.integerExtractor = new EnglishIntegerExtractor();
        this.numberParser = new BaseNumberParser(new EnglishNumberParserConfiguration());
        this.durationExtractor = new BaseDurationExtractor(new EnglishDurationExtractorConfiguration());
        this.utilityConfiguration = new EnglishDateTimeUtilityConfiguration();
    }
}

export class EnglishDateParserConfiguration implements IDateParserConfiguration {
    readonly ordinalExtractor: BaseNumberExtractor
    readonly integerExtractor: BaseNumberExtractor
    readonly cardinalExtractor: BaseNumberExtractor
    readonly durationExtractor: BaseDurationExtractor
    readonly numberParser: BaseNumberParser
    readonly durationParser: BaseDurationParser
    readonly monthOfYear: ReadonlyMap<string, number>
    readonly dayOfMonth: ReadonlyMap<string, number>
    readonly dayOfWeek: ReadonlyMap<string, number>
    readonly unitMap: ReadonlyMap<string, string>
    readonly cardinalMap: ReadonlyMap<string, number>
    readonly dateRegex: RegExp[]
    readonly onRegex: RegExp
    readonly specialDayRegex: RegExp
    readonly nextRegex: RegExp
    readonly unitRegex: RegExp
    readonly monthRegex: RegExp
    readonly strictWeekDay: RegExp
    readonly lastRegex: RegExp
    readonly thisRegex: RegExp
    readonly weekDayOfMonthRegex: RegExp
    readonly utilityConfiguration: IDateTimeUtilityConfiguration
    readonly dateTokenPrefix: string

    // The following three regexes only used in this configuration
    // They are not used in the base parser, therefore they are not extracted
    // If the spanish date parser need the same regexes, they should be extracted
    static readonly relativeDayRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.RelativeDayRegex);
    static readonly nextPrefixRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.NextPrefixRegex);
    static readonly pastPrefixRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.PastPrefixRegex);

    constructor(config: EnglishCommonDateTimeParserConfiguration) {
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
        this.dateRegex = [
            RegExpUtility.getSafeRegExp(EnglishDateTime.DateExtractor1, "gis"),
            RegExpUtility.getSafeRegExp(EnglishDateTime.DateExtractor2, "gis"),
            RegExpUtility.getSafeRegExp(EnglishDateTime.DateExtractor3, "gis"),
            RegExpUtility.getSafeRegExp(EnglishDateTime.DateExtractor4, "gis"),
            RegExpUtility.getSafeRegExp(EnglishDateTime.DateExtractor5, "gis"),
            RegExpUtility.getSafeRegExp(EnglishDateTime.DateExtractor6, "gis"),
            RegExpUtility.getSafeRegExp(EnglishDateTime.DateExtractor7, "gis"),
            RegExpUtility.getSafeRegExp(EnglishDateTime.DateExtractor8, "gis"),
            RegExpUtility.getSafeRegExp(EnglishDateTime.DateExtractor9, "gis"),
            RegExpUtility.getSafeRegExp(EnglishDateTime.DateExtractorA, "gis"),
        ];
        this.onRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.OnRegex, "gis");
        this.specialDayRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.SpecialDayRegex, "gis");
        this.nextRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.NextDateRegex, "gis");
        this.unitRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.DateUnitRegex, "gis");
        this.monthRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.MonthRegex, "gis");
        this.strictWeekDay = RegExpUtility.getSafeRegExp(EnglishDateTime.StrictWeekDay, "gis");
        this.lastRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.LastDateRegex, "gis");
        this.thisRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.ThisRegex, "gis");
        this.weekDayOfMonthRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.WeekDayOfMonthRegex, "gis");
        this.utilityConfiguration = config.utilityConfiguration;
        this.dateTokenPrefix = EnglishDateTime.DateTokenPrefix;
    }

    getSwiftDay(source: string): number {
        let trimmedText = source.trim().toLowerCase();
        let swift = 0;
        let matches = RegExpUtility.getMatches(EnglishDateParserConfiguration.relativeDayRegex, source);
        if (trimmedText === "today") {
            swift = 0;
        } else if (trimmedText === "tomorrow" || trimmedText === "tmr") {
            swift = 1;
        } else if (trimmedText === "yesterday") {
            swift = -1;
        } else if (trimmedText.endsWith("day after tomorrow") ||
            trimmedText.endsWith("day after tmr")) {
            swift = 2;
        } else if (trimmedText.endsWith("day before yesterday")) {
            swift = -2;
        } else if (matches.length) {
            swift = this.getSwift(source);
        }
        return swift;
    }

    getSwiftMonth(source: string): number {
        return this.getSwift(source);
    }

    getSwift(source: string): number {
        let trimmedText = source.trim().toLowerCase();
        let swift = 0;
        let nextPrefixMatches = RegExpUtility.getMatches(EnglishDateParserConfiguration.nextPrefixRegex, trimmedText);
        let pastPrefixMatches = RegExpUtility.getMatches(EnglishDateParserConfiguration.pastPrefixRegex, trimmedText);
        if (nextPrefixMatches.length) {
            swift = 1;
        } else if (pastPrefixMatches.length) {
            swift = -1;
        }
        return swift;
    }

    isCardinalLast(source: string): boolean {
        let trimmedText = source.trim().toLowerCase();
        return trimmedText === "last";
    }
}
