import { IDateExtractorConfiguration, IDateParserConfiguration } from "../baseDate"
import { BaseDurationExtractor, BaseDurationParser } from "../baseDuration"
import { IDateTimeUtilityConfiguration } from "../utilities";
import { RegExpUtility } from "@microsoft/recognizers-text";
import { BaseNumberParser, BaseNumberExtractor, EnglishOrdinalExtractor, EnglishIntegerExtractor, EnglishNumberParserConfiguration } from "@microsoft/recognizers-text-number";
import { EnglishDateTime } from "../../resources/englishDateTime";
import { EnglishCommonDateTimeParserConfiguration, EnglishDateTimeUtilityConfiguration } from "./baseConfiguration"
import { EnglishDurationExtractorConfiguration } from "./durationConfiguration"
import { IDateTimeParser } from "../parsers"
import { IDateTimeExtractor } from "../baseDateTime";
import { Constants } from "../constants";

export class EnglishDateExtractorConfiguration implements IDateExtractorConfiguration {
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
            RegExpUtility.getSafeRegExp(EnglishDateTime.DateExtractor1),
            RegExpUtility.getSafeRegExp(EnglishDateTime.DateExtractor2),
            RegExpUtility.getSafeRegExp(EnglishDateTime.DateExtractor3),

            EnglishDateTime.DefaultLanguageFallback === Constants.DefaultLanguageFallback_MDY?
                RegExpUtility.getSafeRegExp(EnglishDateTime.DateExtractor4):
                RegExpUtility.getSafeRegExp(EnglishDateTime.DateExtractor5),

            EnglishDateTime.DefaultLanguageFallback === Constants.DefaultLanguageFallback_MDY?
                RegExpUtility.getSafeRegExp(EnglishDateTime.DateExtractor5):
                RegExpUtility.getSafeRegExp(EnglishDateTime.DateExtractor4),

            RegExpUtility.getSafeRegExp(EnglishDateTime.DateExtractor6),

            EnglishDateTime.DefaultLanguageFallback === Constants.DefaultLanguageFallback_MDY?
                RegExpUtility.getSafeRegExp(EnglishDateTime.DateExtractor7L):
                RegExpUtility.getSafeRegExp(EnglishDateTime.DateExtractor9L),

            EnglishDateTime.DefaultLanguageFallback === Constants.DefaultLanguageFallback_MDY?
                RegExpUtility.getSafeRegExp(EnglishDateTime.DateExtractor7S):
                RegExpUtility.getSafeRegExp(EnglishDateTime.DateExtractor9S),

            RegExpUtility.getSafeRegExp(EnglishDateTime.DateExtractor8),

            EnglishDateTime.DefaultLanguageFallback === Constants.DefaultLanguageFallback_MDY?
                RegExpUtility.getSafeRegExp(EnglishDateTime.DateExtractor9L):
                RegExpUtility.getSafeRegExp(EnglishDateTime.DateExtractor7L),

            EnglishDateTime.DefaultLanguageFallback === Constants.DefaultLanguageFallback_MDY?
                RegExpUtility.getSafeRegExp(EnglishDateTime.DateExtractor9S):
                RegExpUtility.getSafeRegExp(EnglishDateTime.DateExtractor7S),

            RegExpUtility.getSafeRegExp(EnglishDateTime.DateExtractorA),
        ];
        this.implicitDateList = [
            RegExpUtility.getSafeRegExp(EnglishDateTime.OnRegex),
            RegExpUtility.getSafeRegExp(EnglishDateTime.RelaxedOnRegex),
            RegExpUtility.getSafeRegExp(EnglishDateTime.SpecialDayRegex),
            RegExpUtility.getSafeRegExp(EnglishDateTime.SpecialDayWithNumRegex),
            RegExpUtility.getSafeRegExp(EnglishDateTime.ThisRegex),
            RegExpUtility.getSafeRegExp(EnglishDateTime.LastDateRegex),
            RegExpUtility.getSafeRegExp(EnglishDateTime.NextDateRegex),
            RegExpUtility.getSafeRegExp(EnglishDateTime.SingleWeekDayRegex),
            RegExpUtility.getSafeRegExp(EnglishDateTime.WeekDayOfMonthRegex),
            RegExpUtility.getSafeRegExp(EnglishDateTime.SpecialDate),
            RegExpUtility.getSafeRegExp(EnglishDateTime.RelativeWeekDayRegex),
        ];
        this.monthEnd = RegExpUtility.getSafeRegExp(EnglishDateTime.MonthEnd);
        this.ofMonth = RegExpUtility.getSafeRegExp(EnglishDateTime.OfMonth);
        this.dateUnitRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.DateUnitRegex);
        this.forTheRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.ForTheRegex);
        this.weekDayAndDayOfMonthRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.WeekDayAndDayOfMonthRegex);
        this.relativeMonthRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.RelativeMonthRegex);
        this.weekDayRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.WeekDayRegex);
        this.dayOfWeek = EnglishDateTime.DayOfWeek;
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
    readonly durationExtractor: IDateTimeExtractor
    readonly numberParser: BaseNumberParser
    readonly durationParser: IDateTimeParser
    readonly monthOfYear: ReadonlyMap<string, number>
    readonly dayOfMonth: ReadonlyMap<string, number>
    readonly dayOfWeek: ReadonlyMap<string, number>
    readonly unitMap: ReadonlyMap<string, string>
    readonly cardinalMap: ReadonlyMap<string, number>
    readonly dateRegex: RegExp[]
    readonly onRegex: RegExp
    readonly specialDayRegex: RegExp
    readonly specialDayWithNumRegex: RegExp
    readonly nextRegex: RegExp
    readonly unitRegex: RegExp
    readonly monthRegex: RegExp
    readonly weekDayRegex: RegExp
    readonly lastRegex: RegExp
    readonly thisRegex: RegExp
    readonly weekDayOfMonthRegex: RegExp
    readonly forTheRegex: RegExp;
    readonly weekDayAndDayOfMonthRegex: RegExp;
    readonly relativeMonthRegex: RegExp;
    readonly relativeWeekDayRegex: RegExp;
    readonly utilityConfiguration: IDateTimeUtilityConfiguration
    readonly dateTokenPrefix: string

    // The following three regexes only used in this configuration
    // They are not used in the base parser, therefore they are not extracted
    // If the spanish date parser need the same regexes, they should be extracted
    static readonly relativeDayRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.RelativeDayRegex);
    static readonly nextPrefixRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.NextPrefixRegex);
    static readonly previousPrefixRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.PreviousPrefixRegex);

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
        this.dateRegex = new EnglishDateExtractorConfiguration().dateRegexList;
        this.onRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.OnRegex);
        this.specialDayRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.SpecialDayRegex);
        this.specialDayWithNumRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.SpecialDayWithNumRegex);
        this.nextRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.NextDateRegex);
        this.unitRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.DateUnitRegex);
        this.monthRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.MonthRegex);
        this.weekDayRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.WeekDayRegex);
        this.lastRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.LastDateRegex);
        this.thisRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.ThisRegex);
        this.weekDayOfMonthRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.WeekDayOfMonthRegex);
        this.forTheRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.ForTheRegex);
        this.weekDayAndDayOfMonthRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.WeekDayAndDayOfMonthRegex);
        this.relativeMonthRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.RelativeMonthRegex);
        this.relativeWeekDayRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.RelativeWeekDayRegex);
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
        } else if (trimmedText.endsWith("day after")) {
            swift = 1;
        } else if (trimmedText.endsWith("day before")) {
            swift = -1;
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
        let pastPrefixMatches = RegExpUtility.getMatches(EnglishDateParserConfiguration.previousPrefixRegex, trimmedText);
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
