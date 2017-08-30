import {
    BaseDateExtractor,
    BaseTimeExtractor,
    BaseDateTimeExtractor,
    BaseDurationExtractor,
    BaseDatePeriodExtractor,
    BaseTimePeriodExtractor,
    BaseDateTimePeriodExtractor
} from "../extractors";
import {
    EnglishTimeExtractorConfiguration,
    EnglishDateExtractorConfiguration,
    EnglishDateTimeExtractorConfiguration,
    EnglishDurationExtractorConfiguration,
    EnglishDatePeriodExtractorConfiguration,
    EnglishTimePeriodExtractorConfiguration,
    EnglishDateTimePeriodExtractorConfiguration
} from "./extractorConfiguration";
import {
    BaseDateParserConfiguration,
    ITimeParserConfiguration,
    ICommonDateTimeParserConfiguration,
    IDateParserConfiguration,
    BaseTimeParser,
    IDateTimeParser,
    ITimePeriodParserConfiguration,
    IDurationParserConfiguration,
    IDatePeriodParserConfiguration,
    BaseDateParser,
    BaseDurationParser,
    BaseDatePeriodParser,
    ISetParserConfiguration,
    BaseTimePeriodParser,
    IDateTimeParserConfiguration,
    BaseDateTimeParser,
    BaseHolidayParserConfiguration
} from "../parsers";
import {
    EnglishCardinalExtractor,
    EnglishIntegerExtractor,
    EnglishOrdinalExtractor
} from "../../number/english/extractors";
import { BaseNumberParser, IParser } from "../../number/parsers";
import { IExtractor } from "../../number/extractors";
import { EnglishNumberParserConfiguration } from "../../number/english/parserConfiguration";
import { CultureInfo, Culture } from "../../culture";
import { EnglishNumeric } from "../../resources/englishNumeric";
import { EnglishDateTime } from "../../resources/englishDateTime"
import { BaseDateTime } from "../../resources/baseDateTime"
import { EnlighDatetimeUtilityConfiguration } from "./utilities";
import { RegExpUtility } from "../../utilities";
import { FormatUtil, DateTimeResolutionResult, IDateTimeUtilityConfiguration } from "../utilities"
import * as XRegExp from 'xregexp';

export class EnglishCommonDateTimeParserConfiguration extends BaseDateParserConfiguration {
    constructor() {
        super();
        this.utilityConfiguration = new EnlighDatetimeUtilityConfiguration();
        this.unitMap = EnglishDateTime.UnitMap;
        this.unitValueMap = EnglishDateTime.UnitValueMap;
        this.seasonMap = EnglishDateTime.SeasonMap;
        this.cardinalMap = EnglishDateTime.CardinalMap;
        this.dayOfWeek = EnglishDateTime.DayOfWeek;
        this.monthOfYear = EnglishDateTime.MonthOfYear;
        this.numbers = EnglishDateTime.Numbers;
        this.doubleNumbers = EnglishDateTime.DoubleNumbers;
        this.cardinalExtractor = new EnglishCardinalExtractor();
        this.integerExtractor = new EnglishIntegerExtractor();
        this.ordinalExtractor = new EnglishOrdinalExtractor();
        this.dayOfMonth = new Map<string, number>([...BaseDateTime.DayOfMonthDictionary, ...EnglishDateTime.DayOfMonth]);
        this.numberParser = new BaseNumberParser(new EnglishNumberParserConfiguration());
        this.dateExtractor = new BaseDateExtractor(new EnglishDateExtractorConfiguration());
        this.timeExtractor = new BaseTimeExtractor(new EnglishTimeExtractorConfiguration());
        this.dateTimeExtractor = new BaseDateTimeExtractor(new EnglishDateTimeExtractorConfiguration());
        this.durationExtractor = new BaseDurationExtractor(new EnglishDurationExtractorConfiguration());
        this.datePeriodExtractor = new BaseDatePeriodExtractor(new EnglishDatePeriodExtractorConfiguration());
        this.timePeriodExtractor = new BaseTimePeriodExtractor(new EnglishTimePeriodExtractorConfiguration());
        this.dateTimePeriodExtractor = new BaseDateTimePeriodExtractor(new EnglishDateTimePeriodExtractorConfiguration());
        this.dateParser = new BaseDateParser(new EnglishDateParserConfiguration(this));
        this.timeParser = new EnglishTimeParser(new EnglishTimeParserConfiguration(this));
        this.dateTimeParser = new BaseDateTimeParser(new EnglishDateTimeParserConfiguration(this));
        this.durationParser = new BaseDurationParser(new EnglishDurationParserConfiguration(this));
        // this.dateTimeParser = new BaseDateTimeParser(new EnglishDateTimeParserConfiguration(this));
        this.durationParser = new BaseDurationParser(new EnglishDurationParserConfiguration(this));
        // this.datePeriodParser = new BaseDatePeriodParser(new EnglishDatePeriodParserConfiguration(this));
        this.timePeriodParser = new BaseTimePeriodParser(new EnglishTimePeriodParserConfiguration(this));
        // this.dateTimePeriodParser = new BaseDateTimePeriodParser(new EnglishDateTimePeriodParserConfiguration(this));
    }
}

export class EnglishTimeParserConfiguration implements ITimeParserConfiguration {
    readonly timeTokenPrefix: string;
    readonly atRegex: RegExp
    readonly timeRegexes: RegExp[];
    readonly numbers: ReadonlyMap<string, number>;

    constructor(config: ICommonDateTimeParserConfiguration) {
        this.timeTokenPrefix = EnglishDateTime.TimeTokenPrefix;
        this.atRegex = EnglishTimeExtractorConfiguration.AtRegex;
        this.timeRegexes = EnglishTimeExtractorConfiguration.TimeRegexList;
        this.numbers = config.numbers;
    }

    public adjustByPrefix(prefix: string, adjust: { hour: number, min: number, hasMin: boolean }) {
        let deltaMin = 0;
        let trimedPrefix = prefix.trim().toLowerCase();

        if (trimedPrefix.startsWith("half")) {
            deltaMin = 30;
        }
        else if (trimedPrefix.startsWith("a quarter") || trimedPrefix.startsWith("quarter")) {
            deltaMin = 15;
        }
        else if (trimedPrefix.startsWith("three quarter")) {
            deltaMin = 45;
        }
        else {
            let match = RegExpUtility.getMatches(EnglishTimeExtractorConfiguration.LessThanOneHour, trimedPrefix);
            let minStr = match[0].groups("deltamin").value;
            if (minStr) {
                deltaMin = Number.parseInt(minStr);
            }
            else {
                minStr = match[0].groups("deltaminnum").value.toLowerCase();
                deltaMin = this.numbers.get(minStr);
            }
        }

        if (trimedPrefix.endsWith("to")) {
            deltaMin = -deltaMin;
        }

        adjust.min += deltaMin;
        if (adjust.min < 0) {
            adjust.min += 60;
            adjust.hour -= 1;
        }
        adjust.hasMin = true;
    }

    public adjustBySuffix(suffix: string, adjust: { hour: number, min: number, hasMin: boolean, hasAm: boolean, hasPm: boolean }) {
        let trimedSuffix = suffix.trim().toLowerCase();
        let deltaHour = 0;
        let matches = RegExpUtility.getMatches(EnglishTimeExtractorConfiguration.TimeSuffix, trimedSuffix);
        if (matches.length > 0 && matches[0].index === 0 && matches[0].length === trimedSuffix.length) {
            let oclockStr = matches[0].groups("oclock").value;
            if (!oclockStr) {
                let amStr = matches[0].groups("am").value;
                if (amStr) {
                    if (adjust.hour >= 12) {
                        deltaHour = -12;
                    }
                    adjust.hasAm = true;
                }

                let pmStr = matches[0].groups("pm").value;
                if (pmStr) {
                    if (adjust.hour < 12) {
                        deltaHour = 12;
                    }
                    adjust.hasPm = true;
                }
            }
        }

        adjust.hour = (adjust.hour + deltaHour) % 24;
    }
}

export class EnglishTimePeriodParserConfiguration implements ITimePeriodParserConfiguration {
    timeExtractor: IExtractor;
    timeParser: IDateTimeParser;
    pureNumberFromToRegex: RegExp;
    pureNumberBetweenAndRegex: RegExp;
    timeOfDayRegex: RegExp;
    numbers: ReadonlyMap<string, number>;
    utilityConfiguration: IDateTimeUtilityConfiguration;

    constructor(config: ICommonDateTimeParserConfiguration) {
        this.timeExtractor = config.timeExtractor;
        this.timeParser = config.timeParser;
        this.pureNumberFromToRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.PureNumFromTo);
        this.pureNumberBetweenAndRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.PureNumBetweenAnd);
        this.timeOfDayRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.TimeOfDayRegex);
        this.numbers = config.numbers;
        this.utilityConfiguration = config.utilityConfiguration;
    }

    getMatchedTimexRange(text: string): {
        matched: boolean, timex: string, beginHour: number, endHour: number, endMin: number
    } {
        let trimedText = text.trim().toLowerCase();
        if (trimedText.endsWith("s")) {
            trimedText = trimedText.substring(0, trimedText.length - 1);
        }
        let result = {
            matched: false,
            timex: '',
            beginHour: 0,
            endHour: 0,
            endMin: 0
        };
        if (trimedText.endsWith("morning")) {
            result.timex = "TMO";
            result.beginHour = 8;
            result.endHour = 12;
        }
        else if (trimedText.endsWith("afternoon")) {
            result.timex = "TAF";
            result.beginHour = 12;
            result.endHour = 16;
        }
        else if (trimedText.endsWith("evening")) {
            result.timex = "TEV";
            result.beginHour = 16;
            result.endHour = 20;
        }
        else if (trimedText === "daytime") {
            result.timex = "TDT";
            result.beginHour = 8;
            result.endHour = 18;
        }
        else if (trimedText.endsWith("night")) {
            result.timex = "TNI";
            result.beginHour = 20;
            result.endHour = 23;
            result.endMin = 59;
        }
        else {
            result.timex = null;
            result.matched = false;
            return result;
        }

        result.matched = true;
        return result;
    }
}

export class EnglishTimeParser extends BaseTimeParser {
    constructor(configuration: ITimeParserConfiguration) {
        super(configuration);
    }

    internalParse(text: string, referenceTime: Date): DateTimeResolutionResult {
        let innerResult = super.internalParse(text, referenceTime);
        if (!innerResult.success) {
            innerResult = this.parseIsh(text, referenceTime);
        }
        return innerResult;
    }

    // parse "noonish", "11-ish"
    private parseIsh(text: string, referenceTime: Date): DateTimeResolutionResult {
        let ret = new DateTimeResolutionResult();
        let trimedText = text.toLowerCase().trim();

        let matches = RegExpUtility.getMatches(EnglishTimeExtractorConfiguration.IshRegex, trimedText);
        if (matches.length > 0 && matches[0].length === trimedText.length) {
            let hourStr = matches[0].groups("hour").value;
            let hour = 12;
            if (hourStr) {
                hour = Number.parseInt(hourStr);
            }

            ret.timex = "T" + FormatUtil.toString(hour, 2);
            ret.futureValue =
                ret.pastValue =
                new Date(referenceTime.getFullYear(), referenceTime.getMonth(), referenceTime.getDate(), hour, 0, 0);
            ret.success = true;
            return ret;
        }

        return ret;
    }
}

export class EnglishDateParserConfiguration implements IDateParserConfiguration {
    readonly ordinalExtractor: IExtractor
    readonly integerExtractor: IExtractor
    readonly cardinalExtractor: IExtractor
    readonly durationExtractor: IExtractor
    readonly numberParser: IParser
    readonly durationParser: IDateTimeParser
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

    constructor(config: ICommonDateTimeParserConfiguration) {
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
        let trimedText = source.trim().toLowerCase();
        let swift = 0;
        if (trimedText === "today" || trimedText === "the day") {
            swift = 0;
        } else if (trimedText === "tomorrow" || trimedText === "tmr" ||
            trimedText === "next day" || trimedText === "the next day") {
            swift = 1;
        } else if (trimedText === "yesterday") {
            swift = -1;
        } else if (trimedText.endsWith("day after tomorrow") ||
            trimedText.endsWith("day after tmr")) {
            swift = 2;
        } else if (trimedText.endsWith("day before yesterday")) {
            swift = -2;
        } else if (trimedText.endsWith("last day")) {
            swift = -1;
        }
        return swift;
    }

    getSwiftMonth(source: string): number {
        let trimedText = source.trim().toLowerCase();
        let swift = 0;
        if (trimedText.startsWith("next") || trimedText.startsWith("upcoming")) {
            swift = 1;
        } else if (trimedText.startsWith("last")) {
            swift = -1;
        }
        return swift;
    }

    isCardinalLast(source: string): boolean {
        let trimedText = source.trim().toLowerCase();
        return trimedText === "last";
    }
}

export class EnglishDurationParserConfiguration implements IDurationParserConfiguration {
    readonly cardinalExtractor: IExtractor
    readonly numberParser: IParser
    readonly followedUnit: RegExp
    readonly suffixAndRegex: RegExp
    readonly numberCombinedWithUnit: RegExp
    readonly anUnitRegex: RegExp
    readonly allDateUnitRegex: RegExp
    readonly halfDateUnitRegex: RegExp
    readonly inExactNumberUnitRegex: RegExp
    readonly unitMap: ReadonlyMap<string, string>
    readonly unitValueMap: ReadonlyMap<string, number>
    readonly doubleNumbers: ReadonlyMap<string, number>

    constructor(config: ICommonDateTimeParserConfiguration) {
        this.cardinalExtractor = config.cardinalExtractor;
        this.numberParser = config.numberParser;
        this.followedUnit = RegExpUtility.getSafeRegExp(EnglishDateTime.DurationFollowedUnit);
        this.suffixAndRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.SuffixAndRegex);
        this.numberCombinedWithUnit = RegExpUtility.getSafeRegExp(EnglishDateTime.NumberCombinedWithDurationUnit);
        this.anUnitRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.AnUnitRegex);
        this.allDateUnitRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.AllRegex);
        this.halfDateUnitRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.HalfRegex);
        this.inExactNumberUnitRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.InExactNumberUnitRegex);
        this.unitMap = config.unitMap;
        this.unitValueMap = config.unitValueMap;
        this.doubleNumbers = config.doubleNumbers;
    }
}

export class EnglishDatePeriodParserConfiguration implements IDatePeriodParserConfiguration {
    readonly DateExtractor: IExtractor
    readonly DateParser: IDateTimeParser
    readonly DurationExtractor: IExtractor
    readonly DurationParser: IDateTimeParser
    readonly MonthFrontBetweenRegex: RegExp
    readonly BetweenRegex: RegExp
    readonly MonthFrontSimpleCasesRegex: RegExp
    readonly SimpleCasesRegex: RegExp
    readonly OneWordPeriodRegex: RegExp
    readonly MonthWithYear: RegExp
    readonly MonthNumWithYear: RegExp
    readonly YearRegex: RegExp
    readonly PastRegex: RegExp
    readonly FutureRegex: RegExp
    readonly InConnectorRegex: RegExp
    readonly WeekOfMonthRegex: RegExp
    readonly WeekOfYearRegex: RegExp
    readonly QuarterRegex: RegExp
    readonly QuarterRegexYearFront: RegExp
    readonly SeasonRegex: RegExp
    readonly WeekOfRegex: RegExp
    readonly MonthOfRegex: RegExp
    readonly WhichWeekRegex: RegExp
    readonly NextPrefixRegex: RegExp
    readonly PastPrefixRegex: RegExp
    readonly ThisPrefixRegex: RegExp
    readonly TokenBeforeDate: string
    readonly DayOfMonth: ReadonlyMap<string, number>
    readonly MonthOfYear: ReadonlyMap<string, number>
    readonly CardinalMap: ReadonlyMap<string, number>
    readonly SeasonMap: ReadonlyMap<string, string>

    constructor(config: EnglishCommonDateTimeParserConfiguration) {
        this.DateExtractor = config.dateExtractor;
        this.DateParser = config.dateParser;
        this.DurationExtractor = config.durationExtractor;
        this.DurationParser = config.durationParser;
        this.MonthFrontBetweenRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.MonthFrontBetweenRegex);
        this.BetweenRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.BetweenRegex);
        this.MonthFrontSimpleCasesRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.MonthFrontSimpleCasesRegex);
        this.SimpleCasesRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.SimpleCasesRegex);
        this.OneWordPeriodRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.OneWordPeriodRegex);
        this.MonthWithYear = RegExpUtility.getSafeRegExp(EnglishDateTime.MonthWithYear);
        this.MonthNumWithYear = RegExpUtility.getSafeRegExp(EnglishDateTime.MonthNumWithYear);
        this.YearRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.YearRegex);
        this.PastRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.PastPrefixRegex);
        this.FutureRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.NextPrefixRegex);
        this.InConnectorRegex = config.utilityConfiguration.inConnectorRegex;
        this.WeekOfMonthRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.WeekOfMonthRegex);
        this.WeekOfYearRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.WeekOfYearRegex);
        this.QuarterRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.QuarterRegex);
        this.QuarterRegexYearFront = RegExpUtility.getSafeRegExp(EnglishDateTime.QuarterRegexYearFront);
        this.SeasonRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.SeasonRegex);
        this.WeekOfRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.WeekOfRegex);
        this.MonthOfRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.MonthOfRegex);
        this.WhichWeekRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.WhichWeekRegex);
        this.NextPrefixRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.NextPrefixRegex);
        this.PastPrefixRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.PastPrefixRegex);
        this.ThisPrefixRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.ThisPrefixRegex);
        this.TokenBeforeDate = EnglishDateTime.TokenBeforeDate;
        this.DayOfMonth = config.dayOfMonth;
        this.MonthOfYear = config.monthOfYear;
        this.CardinalMap = config.cardinalMap;
        this.SeasonMap = config.seasonMap;
    }

    getSwiftDayOrMonth(source: string): number {
        let trimedSource = source.trim().toLowerCase();
        let swift = 0;
        if (RegExpUtility.getMatches(this.NextPrefixRegex, trimedSource).length > 0) {
            swift = 1;
        } else if (RegExpUtility.getMatches(this.PastPrefixRegex, trimedSource).length > 0) {
            swift = -1;
        }
        return swift;
    }

    GetSwiftYear(source: string): number {
        let trimedSource = source.trim().toLowerCase();
        let swift = -10;
        if (RegExpUtility.getMatches(this.NextPrefixRegex, trimedSource).length > 0) {
            swift = 1;
        } else if (RegExpUtility.getMatches(this.PastPrefixRegex, trimedSource).length > 0) {
            swift = -1;
        } else if (RegExpUtility.getMatches(this.ThisPrefixRegex, trimedSource).length > 0) {
            swift = 0;
        }
        return swift;
    }
    
    IsFuture(source: string): boolean {
        let trimedSource = source.trim().toLowerCase();
        return (trimedSource.startsWith('this') || trimedSource.startsWith('next'));
    }
    
    IsYearToDate(source: string): boolean {
        let trimedSource = source.trim().toLowerCase();
        return trimedSource === 'year to date';
    }
    
    IsMonthToDate(source: string): boolean {
        let trimedSource = source.trim().toLowerCase();
        return trimedSource === 'month to date';
    }
    
    IsWeekOnly(source: string): boolean {
        let trimedSource = source.trim().toLowerCase();
        return trimedSource.endsWith('week');
    }
    
    IsWeekend(source: string): boolean {
        let trimedSource = source.trim().toLowerCase();
        return trimedSource.endsWith('weekend');
    }
    
    IsMonthOnly(source: string): boolean {
        let trimedSource = source.trim().toLowerCase();
        return trimedSource.endsWith('month');
    }
    
    IsYearOnly(source: string): boolean {
        let trimedSource = source.trim().toLowerCase();
        return trimedSource.endsWith('year');
    }

    IsLastCardinal(source: string): boolean {
        let trimedSource = source.trim().toLowerCase();
        return trimedSource === 'last';
    }
}

export class EnglishSetParserConfiguration implements ISetParserConfiguration {
    durationExtractor: IExtractor;
    durationParser: IDateTimeParser;
    timeExtractor: IExtractor;
    timeParser: IDateTimeParser;
    dateExtractor: IExtractor;
    dateParser: IDateTimeParser;
    dateTimeExtractor: IExtractor;
    dateTimeParser: IDateTimeParser;
    datePeriodExtractor: IExtractor;
    datePeriodParser: IDateTimeParser;
    timePeriodExtractor: IExtractor;
    timePeriodParser: IDateTimeParser;
    dateTimePeriodExtractor: IExtractor;
    dateTimePeriodParser: IDateTimeParser;
    unitMap: ReadonlyMap<string, string>;
    eachPrefixRegex: RegExp;
    periodicRegex: RegExp;
    eachUnitRegex: RegExp;
    eachDayRegex: RegExp;

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

        this.eachPrefixRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.EachPrefixRegex);
        this.periodicRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.PeriodicRegex);
        this.eachUnitRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.EachUnitRegex);
        this.eachDayRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.EachDayRegex);
    }

    public getMatchedDailyTimex(text: string): { matched: boolean, timex: string } {
        let timex = "";
        let trimedText = text.trim().toLowerCase();
        if (trimedText == "daily") {
            timex = "P1D";
        }
        else if (trimedText == "weekly") {
            timex = "P1W";
        }
        else if (trimedText == "biweekly") {
            timex = "P2W";
        }
        else if (trimedText == "monthly") {
            timex = "P1M";
        }
        else if (trimedText == "yearly" || trimedText == "annually" || trimedText == "annual") {
            timex = "P1Y";
        }
        else {
            timex = null;
            return { matched: false, timex: timex };
        }
        return { matched: true, timex: timex };
    }

    public getMatchedUnitTimex(text: string): { matched: boolean, timex: string } {
        let timex = "";
        var trimedText = text.trim().toLowerCase();
        if (trimedText == "day") {
            timex = "P1D";
        }
        else if (trimedText == "week") {
            timex = "P1W";
        }
        else if (trimedText == "month") {
            timex = "P1M";
        }
        else if (trimedText == "year") {
            timex = "P1Y";
        }
        else {
            timex = null;
            return { matched: false, timex: timex };
        }

        return { matched: true, timex: timex };
    }
}

export class EnglishDateTimeParserConfiguration implements IDateTimeParserConfiguration {
    tokenBeforeDate: string;
    tokenBeforeTime: string;
    dateExtractor: IExtractor;
    timeExtractor: IExtractor;
    dateParser: IDateTimeParser;
    timeParser: IDateTimeParser;
    cardinalExtractor: IExtractor;
    numberParser: IParser;
    durationExtractor: IExtractor;
    durationParser: IParser;
    nowRegex: RegExp;
    aMTimeRegex: RegExp;
    pMTimeRegex: RegExp;
    simpleTimeOfTodayAfterRegex: RegExp;
    simpleTimeOfTodayBeforeRegex: RegExp;
    specificTimeOfDayRegex: RegExp;
    theEndOfRegex: RegExp;
    unitRegex: RegExp;
    unitMap: ReadonlyMap<string, string>;
    numbers: ReadonlyMap<string, number>;
    utilityConfiguration: IDateTimeUtilityConfiguration;

    constructor(config: ICommonDateTimeParserConfiguration) {
        this.tokenBeforeDate = EnglishDateTime.TokenBeforeDate;
        this.tokenBeforeTime = EnglishDateTime.TokenBeforeTime;
        this.dateExtractor = config.dateExtractor;
        this.timeExtractor = config.timeExtractor;
        this.dateParser = config.dateParser;
        this.timeParser = config.timeParser;
        this.nowRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.NowRegex);
        this.aMTimeRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.AMTimeRegex);
        this.pMTimeRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.PMTimeRegex);
        this.simpleTimeOfTodayAfterRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.SimpleTimeOfTodayAfterRegex);
        this.simpleTimeOfTodayBeforeRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.SimpleTimeOfTodayBeforeRegex);
        this.specificTimeOfDayRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.SpecificTimeOfDayRegex);
        this.theEndOfRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.TheEndOfRegex);
        this.unitRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.TimeUnitRegex);
        this.numbers = config.numbers;
        this.cardinalExtractor = config.cardinalExtractor;
        this.numberParser = config.numberParser;
        this.durationExtractor = config.durationExtractor;
        this.durationParser = config.durationParser;
        this.unitMap = config.unitMap;
        this.utilityConfiguration = config.utilityConfiguration;
    }

    public getHour(text: string, hour: number): number {
        let trimedText = text.trim().toLowerCase();
        let result = hour;
        if (trimedText.endsWith("morning") && hour >= 12) {
            result -= 12;
        }
        else if (!trimedText.endsWith("morning") && hour < 12) {
            result += 12;
        }
        return result;
    }

    public getMatchedNowTimex(text: string): { matched: boolean, timex: string } {
        var trimedText = text.trim().toLowerCase();
        let timex: string;
        if (trimedText.endsWith("now")) {
            timex = "PRESENT_REF";
        }
        else if (trimedText == "recently" || trimedText == "previously") {
            timex = "PAST_REF";
        }
        else if (trimedText == "as soon as possible" || trimedText == "asap") {
            timex = "FUTURE_REF";
        }
        else {
            timex = null;
            return { matched: false, timex: timex };
        }
        return { matched: true, timex: timex };
    }

    public getSwiftDay(text: string): number {
        var trimedText = text.trim().toLowerCase();
        var swift = 0;
        if (trimedText.startsWith("next")) {
            swift = 1;
        }
        else if (trimedText.startsWith("last")) {
            swift = -1;
        }
        return swift;
    }

    public haveAmbiguousToken(text: string, matchedText: string): boolean { return false; }
}

export class EnglishHolidayParserConfiguration extends BaseHolidayParserConfiguration
{
    constructor()
    {
        super();
        this.holidayRegexList = [
            RegExpUtility.getSafeRegExp(EnglishDateTime.HolidayRegex1, "gis"),
            RegExpUtility.getSafeRegExp(EnglishDateTime.HolidayRegex2, "gis"),
            RegExpUtility.getSafeRegExp(EnglishDateTime.HolidayRegex3, "gis")
        ];
        this.holidayNames = EnglishDateTime.HolidayNames;
    }

    protected InitHolidayFuncs(): ReadonlyMap<string, (year: number) => Date> {
        return new Map<string, (year: number) => Date>(
            [
                ...super.initHolidayFuncs(),
                ["maosbirthday", EnglishHolidayParserConfiguration.MaoBirthday],
                ["yuandan", EnglishHolidayParserConfiguration.NewYear],
                ["teachersday", EnglishHolidayParserConfiguration.TeacherDay],
                ["singleday", EnglishHolidayParserConfiguration.SinglesDay],
                ["allsaintsday", EnglishHolidayParserConfiguration.HalloweenDay],
                ["youthday", EnglishHolidayParserConfiguration.YouthDay],
                ["childrenday", EnglishHolidayParserConfiguration.ChildrenDay],
                ["femaleday", EnglishHolidayParserConfiguration.FemaleDay],
                ["treeplantingday", EnglishHolidayParserConfiguration.TreePlantDay],
                ["arborday", EnglishHolidayParserConfiguration.TreePlantDay],
                ["girlsday", EnglishHolidayParserConfiguration.GirlsDay],
                ["whiteloverday", EnglishHolidayParserConfiguration.WhiteLoverDay],
                ["loverday", EnglishHolidayParserConfiguration.ValentinesDay],
                ["christmas", EnglishHolidayParserConfiguration.ChristmasDay],
                ["xmas", EnglishHolidayParserConfiguration.ChristmasDay],
                ["newyear", EnglishHolidayParserConfiguration.NewYear],
                ["newyearday", EnglishHolidayParserConfiguration.NewYear],
                ["newyearsday", EnglishHolidayParserConfiguration.NewYear],
                ["inaugurationday", EnglishHolidayParserConfiguration.InaugurationDay],
                ["groundhougday", EnglishHolidayParserConfiguration.GroundhogDay],
                ["valentinesday", EnglishHolidayParserConfiguration.ValentinesDay],
                ["stpatrickday", EnglishHolidayParserConfiguration.StPatrickDay],
                ["aprilfools", EnglishHolidayParserConfiguration.FoolDay],
                ["stgeorgeday", EnglishHolidayParserConfiguration.StGeorgeDay],
                ["mayday", EnglishHolidayParserConfiguration.Mayday],
                ["cincodemayoday", EnglishHolidayParserConfiguration.CincoDeMayoday],
                ["baptisteday", EnglishHolidayParserConfiguration.BaptisteDay],
                ["usindependenceday", EnglishHolidayParserConfiguration.UsaIndependenceDay],
                ["independenceday", EnglishHolidayParserConfiguration.UsaIndependenceDay],
                ["bastilleday", EnglishHolidayParserConfiguration.BastilleDay],
                ["halloweenday", EnglishHolidayParserConfiguration.HalloweenDay],
                ["allhallowday", EnglishHolidayParserConfiguration.AllHallowDay],
                ["allsoulsday", EnglishHolidayParserConfiguration.AllSoulsday],
                ["guyfawkesday", EnglishHolidayParserConfiguration.GuyFawkesDay],
                ["veteransday", EnglishHolidayParserConfiguration.Veteransday],
                ["christmaseve", EnglishHolidayParserConfiguration.ChristmasEve],
                ["newyeareve", EnglishHolidayParserConfiguration.NewYearEve],
            ]);
    }

    private static NewYear(year: number): Date { return new Date(year, 1, 1); }
    private static NewYearEve(year: number): Date { return new Date(year, 12, 31); }
    private static ChristmasDay(year: number): Date { return new Date(year, 12, 25); }
    private static ChristmasEve(year: number): Date { return new Date(year, 12, 24); }
    private static ValentinesDay(year: number): Date { return new Date(year, 2, 14); }
    private static WhiteLoverDay(year: number): Date { return new Date(year, 3, 14); }
    private static FoolDay(year: number): Date { return new Date(year, 4, 1); }
    private static GirlsDay(year: number): Date { return new Date(year, 3, 7); }
    private static TreePlantDay(year: number): Date { return new Date(year, 3, 12); }
    private static FemaleDay(year: number): Date { return new Date(year, 3, 8); }
    private static ChildrenDay(year: number): Date { return new Date(year, 6, 1); }
    private static YouthDay(year: number): Date { return new Date(year, 5, 4); }
    private static TeacherDay(year: number): Date { return new Date(year, 9, 10); }
    private static SinglesDay(year: number): Date { return new Date(year, 11, 11); }
    private static MaoBirthday(year: number): Date { return new Date(year, 12, 26); }
    private static InaugurationDay(year: number): Date { return new Date(year, 1, 20); }
    private static GroundhogDay(year: number): Date { return new Date(year, 2, 2); }
    private static StPatrickDay(year: number): Date { return new Date(year, 3, 17); }
    private static StGeorgeDay(year: number): Date { return new Date(year, 4, 23); }
    private static Mayday(year: number): Date { return new Date(year, 5, 1); }
    private static CincoDeMayoday(year: number): Date { return new Date(year, 5, 5); }
    private static BaptisteDay(year: number): Date { return new Date(year, 6, 24); }
    private static UsaIndependenceDay(year: number): Date { return new Date(year, 7, 4); }
    private static BastilleDay(year: number): Date { return new Date(year, 7, 14); }
    private static HalloweenDay(year: number): Date { return new Date(year, 10, 31); }
    private static AllHallowDay(year: number): Date { return new Date(year, 11, 1); }
    private static AllSoulsday(year: number): Date { return new Date(year, 11, 2); }
    private static GuyFawkesDay(year: number): Date { return new Date(year, 11, 5); }
    private static Veteransday(year: number): Date { return new Date(year, 11, 11); }
    
    public getSwiftYear(text: string): number {
        var trimedText = text.trim().toLowerCase();
        var swift = -10;
        if (trimedText.startsWith("next")) {
            swift = 1;
        }
        else if (trimedText.startsWith("last")) {
            swift = -1;
        }
        else if (trimedText.startsWith("this")) {
            swift = 0;
        }
        return swift;
    }

    public sanitizeHolidayToken(holiday: string): string {
        return holiday
            .replace(" ", "")
            .replace("'", "");
    }
}