import { EnglishTimeExtractorConfiguration } from "./english/extractorConfiguration";
import { IParser, ParseResult } from "../number/parsers";
import { ExtractResult, IExtractor } from "../number/extractors";
import { IDateTimeUtilityConfiguration } from "./utilities"
import { BaseDateTime } from "../resources/baseDateTime";
import { Constants, TimeTypeConstants } from "./constants";
import { FormatUtil, DateTimeResolutionResult, DateUtils, DayOfWeek, MatchingUtil, AgoLaterUtil } from "./utilities";
import { RegExpUtility, Match, isNullOrEmpty, isNullOrWhitespace } from "../utilities";

export class DateTimeParseResult extends ParseResult {
    // TimexStr is only used in extractors related with date and time
    // It will output the TIMEX representation of a time string.
    timexStr: string
}

export interface IDateTimeParser extends IParser {
    parse(extResult: ExtractResult, referenceDate?: Date): DateTimeParseResult | null;
}

export interface ICommonDateTimeParserConfiguration {
    cardinalExtractor: IExtractor;
    integerExtractor: IExtractor;
    ordinalExtractor: IExtractor;
    numberParser: IParser;
    dateExtractor: IExtractor;
    timeExtractor: IExtractor;
    dateTimeExtractor: IExtractor;
    durationExtractor: IExtractor;
    datePeriodExtractor: IExtractor;
    timePeriodExtractor: IExtractor;
    dateTimePeriodExtractor: IExtractor;
    dateParser: IDateTimeParser;
    timeParser: IDateTimeParser;
    dateTimeParser: IDateTimeParser;
    durationParser: IDateTimeParser;
    datePeriodParser: IDateTimeParser;
    timePeriodParser: IDateTimeParser;
    dateTimePeriodParser: IDateTimeParser;
    monthOfYear: ReadonlyMap<string, number>;
    numbers: ReadonlyMap<string, number>;
    unitValueMap: ReadonlyMap<string, number>;
    seasonMap: ReadonlyMap<string, string>;
    unitMap: ReadonlyMap<string, string>;
    cardinalMap: ReadonlyMap<string, number>;
    dayOfMonth: ReadonlyMap<string, number>;
    dayOfWeek: ReadonlyMap<string, number>;
    doubleNumbers: ReadonlyMap<string, number>;
    utilityConfiguration: IDateTimeUtilityConfiguration;
}

export abstract class BaseDateParserConfiguration implements ICommonDateTimeParserConfiguration {
    cardinalExtractor: IExtractor;
    integerExtractor: IExtractor;
    ordinalExtractor: IExtractor;
    numberParser: IParser;
    dateExtractor: IExtractor;
    timeExtractor: IExtractor;
    dateTimeExtractor: IExtractor;
    durationExtractor: IExtractor;
    datePeriodExtractor: IExtractor;
    timePeriodExtractor: IExtractor;
    dateTimePeriodExtractor: IExtractor;
    dateParser: IDateTimeParser;
    timeParser: IDateTimeParser;
    dateTimeParser: IDateTimeParser;
    durationParser: IDateTimeParser;
    datePeriodParser: IDateTimeParser;
    timePeriodParser: IDateTimeParser;
    dateTimePeriodParser: IDateTimeParser;
    monthOfYear: ReadonlyMap<string, number>;
    numbers: ReadonlyMap<string, number>;
    unitValueMap: ReadonlyMap<string, number>;
    seasonMap: ReadonlyMap<string, string>;
    unitMap: ReadonlyMap<string, string>;
    cardinalMap: ReadonlyMap<string, number>;
    dayOfMonth: ReadonlyMap<string, number>;
    dayOfWeek: ReadonlyMap<string, number>;
    doubleNumbers: ReadonlyMap<string, number>;
    utilityConfiguration: IDateTimeUtilityConfiguration;

    constructor() {
        this.dayOfMonth = BaseDateTime.DayOfMonthDictionary;
    }
}

export interface ITimeParserConfiguration {
    timeTokenPrefix: string;
    atRegex: RegExp
    timeRegexes: RegExp[];
    numbers: ReadonlyMap<string, number>;
    adjustByPrefix(prefix: string, adjust: { hour: number, min: number, hasMin: boolean });
    adjustBySuffix(suffix: string, adjust: { hour: number, min: number, hasMin: boolean, hasAm: boolean, hasPm: boolean });
}

export interface IDateParserConfiguration {
    ordinalExtractor: IExtractor
    integerExtractor: IExtractor
    cardinalExtractor: IExtractor
    durationExtractor: IExtractor
    durationParser: IDateTimeParser
    numberParser: IParser
    monthOfYear: ReadonlyMap<string, number>
    dayOfMonth: ReadonlyMap<string, number>
    dayOfWeek: ReadonlyMap<string, number>
    unitMap: ReadonlyMap<string, string>
    cardinalMap: ReadonlyMap<string, number>
    dateRegex: RegExp[]
    onRegex: RegExp
    specialDayRegex: RegExp
    nextRegex: RegExp
    unitRegex: RegExp
    monthRegex: RegExp
    strictWeekDay: RegExp
    lastRegex: RegExp
    thisRegex: RegExp
    weekDayOfMonthRegex: RegExp
    utilityConfiguration: IDateTimeUtilityConfiguration
    dateTokenPrefix: string
    getSwiftDay(source: string): number
    getSwiftMonth(source: string): number
    isCardinalLast(source: string): boolean
}

export interface IDurationParserConfiguration {
    cardinalExtractor: IExtractor
    numberParser: IParser
    followedUnit: RegExp
    suffixAndRegex: RegExp
    numberCombinedWithUnit: RegExp
    anUnitRegex: RegExp
    allDateUnitRegex: RegExp
    halfDateUnitRegex: RegExp
    inExactNumberUnitRegex: RegExp
    unitMap: ReadonlyMap<string, string>
    unitValueMap: ReadonlyMap<string, number>
    doubleNumbers: ReadonlyMap<string, number>
}

export interface IDatePeriodParserConfiguration {
    DateExtractor: IExtractor
    DateParser: IDateTimeParser
    DurationExtractor: IExtractor
    DurationParser: IDateTimeParser
    MonthFrontBetweenRegex: RegExp
    BetweenRegex: RegExp
    MonthFrontSimpleCasesRegex: RegExp
    SimpleCasesRegex: RegExp
    OneWordPeriodRegex: RegExp
    MonthWithYear: RegExp
    MonthNumWithYear: RegExp
    YearRegex: RegExp
    PastRegex: RegExp
    FutureRegex: RegExp
    InConnectorRegex: RegExp
    WeekOfMonthRegex: RegExp
    WeekOfYearRegex: RegExp
    QuarterRegex: RegExp
    QuarterRegexYearFront: RegExp
    SeasonRegex: RegExp
    WeekOfRegex: RegExp
    MonthOfRegex: RegExp
    WhichWeekRegex: RegExp
    TokenBeforeDate: string
    DayOfMonth: ReadonlyMap<string, number>
    MonthOfYear: ReadonlyMap<string, number>
    CardinalMap: ReadonlyMap<string, number>
    SeasonMap: ReadonlyMap<string, string>
    getSwiftDayOrMonth(source: string): number
    GetSwiftYear(source: string): number
    IsFuture(source: string): boolean
    IsYearToDate(source: string): boolean
    IsMonthToDate(source: string): boolean
    IsWeekOnly(source: string): boolean
    IsWeekend(source: string): boolean
    IsMonthOnly(source: string): boolean
    IsYearOnly(source: string): boolean
    IsLastCardinal(source: string): boolean
}

export class BaseTimeParser implements IDateTimeParser {
    readonly ParserName = Constants.SYS_DATETIME_TIME; //"Time";
    readonly config: ITimeParserConfiguration;

    constructor(configuration: ITimeParserConfiguration) {
        this.config = configuration;
    }

    public parse(er: ExtractResult, referenceTime?: Date): DateTimeParseResult | null {
        if (!referenceTime) referenceTime = new Date();
        let value = null;
        if (er.type === this.ParserName) {
            let innerResult = this.internalParse(er.text, referenceTime);
            if (innerResult.success) {
                innerResult.futureResolution = new Map<string, string>(
                    [
                        [TimeTypeConstants.TIME, FormatUtil.formatTime(innerResult.futureValue)]
                    ]);

                innerResult.pastResolution = new Map<string, string>(
                    [
                        [TimeTypeConstants.TIME, FormatUtil.formatTime(innerResult.pastValue)]
                    ]);

                value = innerResult;
            }
        }

        let ret = new DateTimeParseResult(er);
        ret.value = value,
            ret.timexStr = value == null ? "" : value.timex,
            ret.resolutionStr = ""

        return ret;
    }

    internalParse(text: string, referenceTime: Date): DateTimeResolutionResult {
        let innerResult = this.parseBasicRegexMatch(text, referenceTime);
        return innerResult;
    }

    // parse basic patterns in TimeRegexList
    private parseBasicRegexMatch(text: string, referenceTime: Date): DateTimeResolutionResult {
        let trimmedText = text.trim().toLowerCase();
        let offset = 0;

        let matches = RegExpUtility.getMatches(this.config.atRegex, trimmedText);
        if (matches.length === 0) {
            matches = RegExpUtility.getMatches(this.config.atRegex, this.config.timeTokenPrefix + trimmedText);
            offset = this.config.timeTokenPrefix.length;
        }

        if (matches.length > 0 && matches[0].index === offset && matches[0].length === trimmedText.length) {
            return this.match2Time(matches[0], referenceTime);
        }

        for (let regex of this.config.timeRegexes) {
            offset = 0;
            matches = RegExpUtility.getMatches(regex, trimmedText);

            if (matches.length && matches[0].index === offset && matches[0].length === trimmedText.length) {
                return this.match2Time(matches[0], referenceTime);
            }
        }

        return new DateTimeResolutionResult();
    }

    private match2Time(match: Match, referenceTime: Date): DateTimeResolutionResult {
        let ret = new DateTimeResolutionResult();
        let hour = 0,
            min = 0,
            second = 0,
            day = referenceTime.getDate(),
            month = referenceTime.getMonth(),
            year = referenceTime.getFullYear();
        let hasMin = false, hasSec = false, hasAm = false, hasPm = false, hasMid = false;

        let engTimeStr = match.groups('engtime').value;
        if (!isNullOrWhitespace(engTimeStr)) {
            // get hour
            let hourStr = match.groups('hournum').value.toLowerCase();
            hour = this.config.numbers.get(hourStr);

            // get minute
            let minStr = match.groups('minnum').value;
            let tensStr = match.groups('tens').value;

            if (!isNullOrWhitespace(minStr)) {
                min = this.config.numbers.get(minStr);
                if (tensStr) {
                    min += this.config.numbers.get(tensStr);
                }
                hasMin = true;
            }
        }
        else if (!isNullOrWhitespace(match.groups('mid').value)) {
            hasMid = true;
            if (!isNullOrWhitespace(match.groups('midnight').value)) {
                hour = 0;
                min = 0;
                second = 0;
            }
            else if (!isNullOrWhitespace(match.groups('midmorning').value)) {
                hour = 10;
                min = 0;
                second = 0;
            }
            else if (!isNullOrWhitespace(match.groups('midafternoon').value)) {
                hour = 14;
                min = 0;
                second = 0;
            }
            else if (!isNullOrWhitespace(match.groups('midday').value)) {
                hour = 12;
                min = 0;
                second = 0;
            }
        }
        else {
            // get hour
            let hourStr = match.groups('hour').value;
            if (isNullOrWhitespace(hourStr)) {
                hourStr = match.groups('hournum').value.toLowerCase();
                hour = this.config.numbers.get(hourStr);
                if (!hour) {
                    return ret;
                }
            }
            else {
                hour = Number.parseInt(hourStr);
                if (!hour) {
                    hour = this.config.numbers.get(hourStr);
                    if (!hour) {
                        return ret;
                    }
                }
            }

            // get minute
            let minStr = match.groups('min').value.toLowerCase();
            if (isNullOrWhitespace(minStr)) {
                minStr = match.groups('minnum').value;
                if (!isNullOrWhitespace(minStr)) {
                    min = this.config.numbers.get(minStr);
                    hasMin = true;
                }

                let tensStr = match.groups('tens').value;
                if (!isNullOrWhitespace(tensStr)) {
                    min += this.config.numbers.get(tensStr);
                    hasMin = true;
                }
            }
            else {
                min = Number.parseInt(minStr);
                hasMin = true;
            }

            // get second
            let secStr = match.groups('sec').value.toLowerCase();
            if (!isNullOrWhitespace(secStr)) {
                second = Number.parseInt(secStr);
                hasSec = true;
            }
        }

        // adjust by desc string
        let descStr = match.groups('desc').value.toLowerCase();
        if (!isNullOrWhitespace(descStr)) {
            if (descStr.startsWith("a")) {
                if (hour >= 12) {
                    hour -= 12;
                }
                hasAm = true;
            }
            else if (descStr.startsWith("p")) {
                if (hour < 12) {
                    hour += 12;
                }
                hasPm = true;
            }
        }

        // adjust min by prefix
        let timePrefix = match.groups('prefix').value.toLowerCase();
        if (!isNullOrWhitespace(timePrefix)) {
            let adjust = { hour: hour, min: min, hasMin: hasMin };
            this.config.adjustByPrefix(timePrefix, adjust);
            hour = adjust.hour; min = adjust.min; hasMin = adjust.hasMin;
        }

        // adjust hour by suffix
        let timeSuffix = match.groups('suffix').value.toLowerCase();
        if (!isNullOrWhitespace(timeSuffix)) {
            let adjust = { hour: hour, min: min, hasMin: hasMin, hasAm: hasAm, hasPm: hasPm };
            this.config.adjustBySuffix(timeSuffix, adjust);
            hour = adjust.hour; min = adjust.min; hasMin = adjust.hasMin; hasAm = adjust.hasAm; hasPm = adjust.hasPm;
        }

        if (hour === 24) {
            hour = 0;
        }

        ret.timex = "T" + FormatUtil.toString(hour, 2);
        if (hasMin) {
            ret.timex += ":" + FormatUtil.toString(min, 2);
        }

        if (hasSec) {
            ret.timex += ":" + FormatUtil.toString(second, 2);
        }

        if (hour <= 12 && !hasPm && !hasAm) {
            ret.comment = "ampm";
        }

        ret.futureValue = ret.pastValue = new Date(year, month, day, hour, min, second);
        ret.success = true;

        return ret;
    }
}

export interface ITimePeriodParserConfiguration {
    timeExtractor: IExtractor;
    timeParser: IDateTimeParser;
    pureNumberFromToRegex: RegExp;
    pureNumberBetweenAndRegex: RegExp;
    timeOfDayRegex: RegExp;
    numbers: ReadonlyMap<string, number>;
    utilityConfiguration: IDateTimeUtilityConfiguration;
    getMatchedTimexRange(text: string): {
        matched: boolean, timex: string, beginHour: number, endHour: number, endMin: number
    };
}

export class BaseTimePeriodParser implements IDateTimeParser {
    public static readonly ParserName = Constants.SYS_DATETIME_TIMEPERIOD; //"TimePeriod";

    private readonly config: ITimePeriodParserConfiguration;

    constructor(configuration: ITimePeriodParserConfiguration) {
        this.config = configuration;
    }

    public parse(er: ExtractResult, refTime?: Date): DateTimeParseResult {
        let referenceTime = refTime || new Date();
        let value = null;
        if (er.type === BaseTimePeriodParser.ParserName) {
            let innerResult = this.parseSimpleCases(er.text, referenceTime);
            if (!innerResult.success) {
                innerResult = this.mergeTwoTimePoints(er.text, referenceTime);
            }
            if (!innerResult.success) {
                innerResult = this.parseNight(er.text, referenceTime);
            }
            if (innerResult.success) {
                innerResult.futureResolution = new Map<string, string>(
                    [
                        [
                            TimeTypeConstants.START_TIME,
                            FormatUtil.formatTime(innerResult.futureValue.item1)
                        ],
                        [
                            TimeTypeConstants.END_TIME,
                            FormatUtil.formatTime(innerResult.futureValue.item2)
                        ]
                    ]);

                innerResult.pastResolution = new Map<string, string>(
                    [
                        [
                            TimeTypeConstants.START_TIME,
                            FormatUtil.formatTime(innerResult.pastValue.item1)
                        ],
                        [
                            TimeTypeConstants.END_TIME,
                            FormatUtil.formatTime(innerResult.pastValue.item2)
                        ]
                    ]);

                value = innerResult;
            }
        }

        let ret = new DateTimeParseResult(er);
        ret.value = value;
        ret.timexStr = value == null ? "" : value.timex;
        ret.resolutionStr = "";

        return ret;
    }

    private parseSimpleCases(text: string, referenceTime: Date): DateTimeResolutionResult {
        let ret = new DateTimeResolutionResult();
        let year = referenceTime.getFullYear(), month = referenceTime.getMonth(), day = referenceTime.getDate();
        let trimedText = text.trim().toLowerCase();

        let matches = RegExpUtility.getMatches(this.config.pureNumberFromToRegex, trimedText);
        if (!matches.length) {
            matches = RegExpUtility.getMatches(this.config.pureNumberBetweenAndRegex, trimedText);
        }

        if (matches.length && matches[0].index === 0) {
            // this "from .. to .." pattern is valid if followed by a Date OR "pm"
            let isValid = false;

            // get hours
            let hourGroup = matches[0].groups('hour');
            let hourStr = hourGroup.captures[0];

            let beginHour = this.config.numbers.get(hourStr);
            if (!beginHour) {
                beginHour = Number.parseInt(hourStr);
            }

            hourStr = hourGroup.captures[1];

            let endHour = this.config.numbers.get(hourStr);
            if (!endHour) {
                endHour = Number.parseInt(hourStr);
            }

            // parse "pm" 
            let leftDesc = matches[0].groups("leftDesc").value;
            let rightDesc = matches[0].groups("rightDesc").value;
            let pmStr = matches[0].groups("pm").value;
            let amStr = matches[0].groups("am").value;
            // The "ampm" only occurs in time, don't have to consider it here
            if (isNullOrWhitespace(leftDesc)) {
                let rightAmValid = !isNullOrEmpty(rightDesc) &&
                    RegExpUtility.getMatches(this.config.utilityConfiguration.amDescRegex, rightDesc.toLowerCase()).length;
                let rightPmValid = !isNullOrEmpty(rightDesc) &&
                    RegExpUtility.getMatches(this.config.utilityConfiguration.pmDescRegex, rightDesc.toLowerCase()).length;
                if (!isNullOrEmpty(amStr) || rightAmValid) {

                    if (beginHour >= 12) {
                        beginHour -= 12;
                    }
                    if (endHour >= 12) {
                        endHour -= 12;
                    }
                    isValid = true;
                }
                else if (!isNullOrEmpty(pmStr) || rightPmValid) {
                    if (beginHour < 12) {
                        beginHour += 12;
                    }
                    if (endHour < 12) {
                        endHour += 12;
                    }
                    isValid = true;
                }
            }

            if (isValid) {
                let beginStr = "T" + FormatUtil.toString(beginHour, 2);
                let endStr = "T" + FormatUtil.toString(endHour, 2);

                ret.timex = `(${beginStr},${endStr},PT${endHour - beginHour}H)`;

                ret.futureValue = ret.pastValue = {
                    item1: new Date(year, month, day, beginHour, 0, 0),
                    item2: new Date(year, month, day, endHour, 0, 0)
                };

                ret.success = true;

                return ret;
            }
        }
        return ret;
    }

    private mergeTwoTimePoints(text: string, referenceTime: Date): DateTimeResolutionResult {
        let ret = new DateTimeResolutionResult();
        let ers = this.config.timeExtractor.extract(text);
        let pr1: DateTimeParseResult = null;
        let pr2: DateTimeParseResult = null;
        if (ers.length !== 2) {
            return ret;
        }

        pr1 = this.config.timeParser.parse(ers[0], referenceTime);
        pr2 = this.config.timeParser.parse(ers[1], referenceTime);

        if (pr1.value == null || pr2.value == null) {
            return ret;
        }

        let beginTime: Date = pr1.value.futureValue;
        let endTime: Date = pr2.value.futureValue;

        ret.timex = `(${pr1.timexStr},${pr2.timexStr},PT${new Date(endTime.getTime() - beginTime.getTime()).getUTCHours()}H)`;
        ret.futureValue = ret.pastValue = { item1: beginTime, item2: endTime };
        ret.success = true;

        let ampmStr1 = pr1.value.comment;
        let ampmStr2 = pr2.value.comment;
        if (ampmStr1 && ampmStr1.endsWith("ampm") && ampmStr2 && ampmStr2.endsWith("ampm")) {
            ret.comment = "ampm";
        }

        return ret;
    }

    // parse "morning", "afternoon", "night"
    private parseNight(text: string, referenceTime: Date): DateTimeResolutionResult {
        let day = referenceTime.getDate(),
            month = referenceTime.getMonth(),
            year = referenceTime.getFullYear();
        let ret = new DateTimeResolutionResult();

        // extract early/late prefix from text
        let matches = RegExpUtility.getMatches(this.config.timeOfDayRegex, text);
        let hasEarly = false, hasLate = false;
        if (matches.length) {
            if (!isNullOrEmpty(matches[0].groups("early").value)) {
                let early = matches[0].groups("early").value;
                text = text.replace(early, "");
                hasEarly = true;
                ret.comment = "early";
            }
            if (!hasEarly && !isNullOrEmpty(matches[0].groups("late").value)) {
                let late = matches[0].groups("late").value;
                text = text.replace(late, "");
                hasLate = true;
                ret.comment = "late";
            }
        }

        let timexRange = this.config.getMatchedTimexRange(text);
        if (!timexRange.matched) {
            return new DateTimeResolutionResult();
        }

        // modify time period if "early" or "late" is existed
        if (hasEarly) {
            timexRange.endHour = timexRange.beginHour + 2;
            // handling case: night end with 23:59
            if (timexRange.endMin === 59) {
                timexRange.endMin = 0;
            }
        }
        else if (hasLate) {
            timexRange.beginHour = timexRange.beginHour + 2;
        }

        ret.timex = timexRange.timex;

        ret.futureValue = ret.pastValue = {
            item1: new Date(year, month, day, timexRange.beginHour, 0, 0),
            item2: new Date(year, month, day, timexRange.endHour, timexRange.endMin, timexRange.endMin)
        };

        ret.success = true;

        return ret;
    }
}

export class BaseDateParser implements IDateTimeParser {
    private readonly parserName = Constants.SYS_DATETIME_DATE;

    private readonly config: IDateParserConfiguration;

    constructor(config: IDateParserConfiguration) {
        this.config = config;
    }

    parse(extractorResult: ExtractResult, referenceDate?: Date): DateTimeParseResult | null {
        if (!referenceDate) referenceDate = new Date();
        let resultValue;
        if (extractorResult.type === this.parserName) {
            let source = extractorResult.text.toLowerCase();
            let innerResult = this.parseBasicRegexMatch(source, referenceDate);
            if (!innerResult.success) {
                innerResult = this.parseImplicitDate(source, referenceDate);
            }
            if (!innerResult.success) {
                innerResult = this.parseWeekdayOfMonth(source, referenceDate);
            }
            if (!innerResult.success) {
                innerResult = this.parseNumberWithMonth(source, referenceDate);
            }
            if (!innerResult.success) {
                innerResult = this.parserDurationWithAgoAndLater(source, referenceDate);
            }
            if (innerResult.success) {
                innerResult.futureResolution = new Map<string, string>()
                    .set(TimeTypeConstants.DATE, FormatUtil.formatDate(innerResult.futureValue));
                innerResult.pastResolution = new Map<string, string>()
                    .set(TimeTypeConstants.DATE, FormatUtil.formatDate(innerResult.pastValue));
                resultValue = innerResult;
            }
        }
        let result = new DateTimeParseResult(extractorResult);
        result.value = resultValue;
        result.timexStr = resultValue ? resultValue.timex : '';
        result.resolutionStr = '';

        return result;
    }

    private parseBasicRegexMatch(source: string, referenceDate: Date): DateTimeResolutionResult {
        let trimmedSource = source.trim();
        let result = new DateTimeResolutionResult();
        this.config.dateRegex.some(regex => {
            let offset = 0;
            let match = RegExpUtility.getMatches(regex, trimmedSource).pop();
            if (!match) {
                match = RegExpUtility.getMatches(regex, this.config.dateTokenPrefix + trimmedSource).pop();
                offset = this.config.dateTokenPrefix.length;
            }
            if (match && match.index === offset && match.length === trimmedSource.length) {
                result = this.matchToDate(match, referenceDate);
                return true;
            }
        });
        return result;
    }

    private parseImplicitDate(source: string, referenceDate: Date): DateTimeResolutionResult {
        let trimedSource = source.trim();
        let result = new DateTimeResolutionResult();
        // handle "on 12"
        let match = RegExpUtility.getMatches(this.config.onRegex, this.config.dateTokenPrefix + trimedSource).pop();
        if (match && match.index === this.config.dateTokenPrefix.length && match.length === trimedSource.length) {
            let day = 0;
            let month = referenceDate.getMonth();
            let year = referenceDate.getFullYear();
            let dayStr = match.groups('day').value;
            day = this.config.dayOfMonth.get(dayStr);
            result.timex = FormatUtil.luisDate(-1, -1, day);
            let futureDate = new Date(year, month + 1, day);
            let pastDate = new Date(year, month - 1, day);
            let guessedDate = new Date(year, month, day);
            if (guessedDate) {
                if (guessedDate >= referenceDate) {
                    futureDate = guessedDate;
                }
                if (guessedDate < referenceDate) {
                    pastDate = guessedDate;
                }
            }
            result.futureValue = futureDate;
            result.pastValue = pastDate;
            result.success = true;
            return result;
        }

        // handle "today", "the day before yesterday"
        match = RegExpUtility.getMatches(this.config.specialDayRegex, trimedSource).pop();
        if (match && match.index === 0 && match.length === trimedSource.length) {
            let swift = this.config.getSwiftDay(match.value);
            let value = new Date(referenceDate.getFullYear(), referenceDate.getMonth(), referenceDate.getDate() + swift);
            result.timex = FormatUtil.luisDateFromDate(value);
            result.futureValue = value;
            result.pastValue = value;
            result.success = true;
            return result;
        }

        // handle "next Sunday"
        match = RegExpUtility.getMatches(this.config.nextRegex, trimedSource).pop();
        if (match && match.index === 0 && match.length === trimedSource.length) {
            let weekdayStr = match.groups('weekday').value;
            let value = DateUtils.next(referenceDate, this.config.dayOfWeek.get(weekdayStr));

            result.timex = FormatUtil.luisDateFromDate(value);
            result.futureValue = value;
            result.pastValue = value;
            result.success = true;
            return result;
        }

        // handle "this Friday"
        match = RegExpUtility.getMatches(this.config.thisRegex, trimedSource).pop();
        if (match && match.index === 0 && match.length === trimedSource.length) {
            let weekdayStr = match.groups('weekday').value;
            let value = DateUtils.this(referenceDate, this.config.dayOfWeek.get(weekdayStr));

            result.timex = FormatUtil.luisDateFromDate(value);
            result.futureValue = value;
            result.pastValue = value;
            result.success = true;
            return result;
        }

        // handle "last Friday", "last mon"
        match = RegExpUtility.getMatches(this.config.lastRegex, trimedSource).pop();
        if (match && match.index === 0 && match.length === trimedSource.length) {
            let weekdayStr = match.groups('weekday').value;
            let value = DateUtils.last(referenceDate, this.config.dayOfWeek.get(weekdayStr));

            result.timex = FormatUtil.luisDateFromDate(value);
            result.futureValue = value;
            result.pastValue = value;
            result.success = true;
            return result;
        }

        // handle "Friday"
        match = RegExpUtility.getMatches(this.config.strictWeekDay, trimedSource).pop();
        if (match && match.index === 0 && match.length === trimedSource.length) {
            let weekdayStr = match.groups('weekday').value;
            let weekday = this.config.dayOfWeek.get(weekdayStr);
            let value = DateUtils.this(referenceDate, this.config.dayOfWeek.get(weekdayStr));

            if (weekday === 0) weekday = 7;
            if (weekday < referenceDate.getDay()) value = DateUtils.next(referenceDate, weekday);
            result.timex = 'XXXX-WXX-' + weekday;
            let futureDate = new Date(value);
            let pastDate = new Date(value);
            if (futureDate < referenceDate) futureDate.setDate(value.getDate() + 7);
            if (pastDate >= referenceDate) pastDate.setDate(value.getDate() - 7);

            result.futureValue = futureDate;
            result.pastValue = pastDate;
            result.success = true;
            return result;
        }
        return result;
    }

    private parseNumberWithMonth(source: string, referenceDate: Date): DateTimeResolutionResult {
        let trimedSource = source.trim();
        let result = new DateTimeResolutionResult();
        let match = RegExpUtility.getMatches(this.config.monthRegex, trimedSource).pop();
        if (!match) return result;
        let er = this.config.ordinalExtractor.extract(trimedSource);
        if (!er || er.length === 0) {
            er = this.config.integerExtractor.extract(trimedSource);
        }
        if (!er || er.length === 0) return result;

        let year = referenceDate.getFullYear();
        let month = this.config.monthOfYear.get(match.value.trim()) - 1;
        let day = Number.parseInt(this.config.numberParser.parse(er[0]).value);

        result.timex = FormatUtil.luisDate(-1, month, day);
        let futureDate = new Date(year, month, day);
        let pastDate = new Date(year, month, day);
        if (futureDate < referenceDate) futureDate.setFullYear(year + 1);
        if (pastDate >= referenceDate) pastDate.setFullYear(year - 1);

        result.futureValue = futureDate;
        result.pastValue = pastDate;
        result.success = true;
        return result;
    }

    private parserDurationWithAgoAndLater(source: string, referenceDate: Date): DateTimeResolutionResult {
        return parserDurationWithAgoAndLater(
            source,
            referenceDate,
            this.config.durationExtractor,
            this.config.durationParser,
            this.config.unitMap,
            this.config.unitRegex,
            this.config.utilityConfiguration,
            AgoLaterMode.Date
        );
    }

    private parseWeekdayOfMonth(source: string, referenceDate: Date): DateTimeResolutionResult {
        let trimedSource = source.trim();
        let result = new DateTimeResolutionResult();
        let match = RegExpUtility.getMatches(this.config.weekDayOfMonthRegex, trimedSource).pop();
        if (!match) return result;
        let cardinalStr = match.groups('cardinal').value;
        let weekdayStr = match.groups('weekday').value;
        let monthStr = match.groups('month').value;
        let noYear = false;
        let cardinal = this.config.isCardinalLast(cardinalStr) ? 5 : this.config.cardinalMap.get(cardinalStr);
        let weekday = this.config.dayOfWeek.get(weekdayStr);
        let month = referenceDate.getMonth();
        let year = referenceDate.getFullYear();
        if (isNullOrEmpty(monthStr)) {
            let swift = this.config.getSwiftMonth(trimedSource);
            let temp = new Date(referenceDate);
            temp.setMonth(referenceDate.getMonth() + swift);
            month = temp.getMonth();
            year = temp.getFullYear();
        } else {
            month = this.config.monthOfYear.get(monthStr) - 1;
            noYear = true;
        }
        let value = this.computeDate(cardinal, weekday, month, year);
        if (value.getMonth() !== month) {
            cardinal -= 1;
            value.setDate(value.getDate() - 7);
        }
        let futureDate = value;
        let pastDate = value;
        if (noYear && futureDate < referenceDate) {
            futureDate = this.computeDate(cardinal, weekday, month, year + 1);
            if (futureDate.getMonth() !== month) futureDate.setDate(futureDate.getDate() - 7);
        }
        if (noYear && pastDate >= referenceDate) {
            pastDate = this.computeDate(cardinal, weekday, month, year - 1);
            if (pastDate.getMonth() !== month) pastDate.setDate(pastDate.getDate() - 7);
        }
        result.timex = '';
        result.futureValue = futureDate;
        result.pastValue = pastDate;
        result.success = true;
        return result;
    }

    private matchToDate(match: Match, referenceDate: Date): DateTimeResolutionResult {
        let result = new DateTimeResolutionResult();
        let yearStr = match.groups('year').value;
        let monthStr = match.groups('month').value;
        let dayStr = match.groups('day').value;
        let month = 0;
        let day = 0;
        let year = 0;
        if (this.config.monthOfYear.has(monthStr) && this.config.dayOfMonth.has(dayStr)) {
            month = this.config.monthOfYear.get(monthStr) - 1;
            day = this.config.dayOfMonth.get(dayStr);
            if (!isNullOrEmpty(yearStr)) {
                year = Number.parseInt(yearStr);
                if (year < 100 && year >= 90) year += 1900;
                else if (year < 100 && year < 20) year += 2000;
            }
        }
        let noYear = false;
        if (year === 0) {
            year = referenceDate.getFullYear();
            result.timex = FormatUtil.luisDate(-1, month, day);
            noYear = true;
        } else {
            result.timex = FormatUtil.luisDate(year, month, day);
        }
        let futureDate = new Date(year, month, day);
        let pastDate = new Date(year, month, day);
        if (noYear && futureDate < referenceDate) {
            futureDate = new Date(year + 1, month, day);
        }
        if (noYear && pastDate >= referenceDate) {
            futureDate = new Date(year - 1, month, day);
        }
        result.futureValue = futureDate;
        result.pastValue = pastDate;
        result.success = true;
        return result;
    }

    private computeDate(cardinal: number, weekday: number, month: number, year: number) {
        let firstDay = new Date(year, month, 1);
        let firstWeekday = DateUtils.this(firstDay, weekday);
        if (weekday === 0) weekday = 7;
        if (weekday < firstDay.getDay()) firstWeekday = DateUtils.next(firstDay, weekday);
        firstWeekday.setDate(firstWeekday.getDate() + (7 * (cardinal - 1)));
        return firstWeekday;
    }
}

export class BaseDurationParser implements IDateTimeParser {
    private readonly parserName = Constants.SYS_DATETIME_DURATION;

    private readonly config: IDurationParserConfiguration;

    constructor(config: IDurationParserConfiguration) {
        this.config = config;
    }

    parse(extractorResult: ExtractResult, referenceDate?: Date): DateTimeParseResult | null {
        if (!referenceDate) referenceDate = new Date();
        let resultValue;
        if (extractorResult.type === this.parserName) {
            let source = extractorResult.text.toLowerCase();
            let innerResult = this.parseNumberWithUnit(source, referenceDate);
            if (!innerResult.success) {
                innerResult = this.parseImplicitDuration(source, referenceDate);
            }
            if (innerResult.success) {
                innerResult.futureResolution = new Map<string, string>()
                    .set(TimeTypeConstants.DURATION, innerResult.futureValue);
                innerResult.pastResolution = new Map<string, string>()
                    .set(TimeTypeConstants.DURATION, innerResult.pastValue);
                resultValue = innerResult;
            }
        }
        let result = new DateTimeParseResult(extractorResult);
        result.value = resultValue;
        result.timexStr = resultValue ? resultValue.timex : '';
        result.resolutionStr = '';

        return result;
    }

    private parseNumberWithUnit(source: string, referenceDate: Date): DateTimeResolutionResult {
        let trimmedSource = source.trim();
        let result = this.parseNumberSpaceUnit(trimmedSource);
        if (!result.success) {
            result = this.parseNumberCombinedUnit(trimmedSource);
        }
        if (!result.success) {
            result = this.parseAnUnit(trimmedSource);
        }
        if (!result.success) {
            result = this.parseInExactNumberUnit(trimmedSource);
        }
        return result;
    }

    private parseImplicitDuration(source: string, referenceDate: Date): DateTimeResolutionResult {
        let trimmedSource = source.trim();
        let result = this.getResultFromRegex(this.config.allDateUnitRegex, trimmedSource, 1);
        if (!result.success) {
            result = this.getResultFromRegex(this.config.halfDateUnitRegex, trimmedSource, 0.5);
        }
        return result;
    }

    private getResultFromRegex(regex: RegExp, source: string, num: number): DateTimeResolutionResult {
        let result = new DateTimeResolutionResult();
        let match = RegExpUtility.getMatches(regex, source).pop();
        if (!match) return result;

        let sourceUnit = match.groups('unit').value;
        if (!this.config.unitMap.has(sourceUnit)) return result;

        let unitStr = this.config.unitMap.get(sourceUnit);
        result.timex = `P${this.isLessThanDay(unitStr) ? 'T' : ''}${num}${unitStr[0]}`;
        result.futureValue = num * this.config.unitValueMap.get(sourceUnit);
        result.pastValue = result.futureValue;
        result.success = true;
        return result;
    }

    private parseNumberSpaceUnit(source: string): DateTimeResolutionResult {
        let result = new DateTimeResolutionResult();
        let suffixStr = source;
        let ers = this.config.cardinalExtractor.extract(source);
        if (ers && ers.length === 1) {
            let er = ers[0];
            let sourceUnit = '';
            let pr = this.config.numberParser.parse(er);
            let noNumStr = source.substr(er.start + er.length).trim().toLowerCase();
            let match = RegExpUtility.getMatches(this.config.followedUnit, noNumStr).pop();
            if (match) {
                sourceUnit = match.groups('unit').value;
                suffixStr = match.groups('suffix').value;
            }
            if (this.config.unitMap.has(sourceUnit)) {
                let num = Number.parseFloat(pr.value) + this.parseNumberWithUnitAndSuffix(suffixStr);
                let unitStr = this.config.unitMap.get(sourceUnit);

                result.timex = `P${this.isLessThanDay(unitStr) ? 'T' : ''}${num}${unitStr[0]}`;
                result.futureValue = num * this.config.unitValueMap.get(sourceUnit);
                result.pastValue = result.futureValue;
                result.success = true;
                return result;
            }
        }
        return result;
    }

    private parseNumberWithUnitAndSuffix(source: string): number {
        let match = RegExpUtility.getMatches(this.config.suffixAndRegex, source).pop();
        if (match) {
            let numStr = match.groups('suffix_num').value;
            if (this.config.doubleNumbers.has(numStr)) {
                return this.config.doubleNumbers.get(numStr);
            }
        }
        return 0;
    }

    private parseNumberCombinedUnit(source: string): DateTimeResolutionResult {
        let result = new DateTimeResolutionResult();
        let match = RegExpUtility.getMatches(this.config.numberCombinedWithUnit, source).pop();
        if (!match) return result;
        let num = Number.parseFloat(match.groups('num').value) + this.parseNumberWithUnitAndSuffix(source);

        let sourceUnit = match.groups('unit').value;
        if (this.config.unitMap.has(sourceUnit)) {
            let unitStr = this.config.unitMap.get(sourceUnit);
            if (num > 1000 && (unitStr === 'Y' || unitStr === 'MON' || unitStr === 'W')) {
                return result;
            }

            result.timex = `P${this.isLessThanDay(unitStr) ? 'T' : ''}${num}${unitStr[0]}`;
            result.futureValue = num * this.config.unitValueMap.get(sourceUnit);
            result.pastValue = result.futureValue;
            result.success = true;
            return result;
        }
        return result;
    }

    private parseAnUnit(source: string): DateTimeResolutionResult {
        let result = new DateTimeResolutionResult();
        let match = RegExpUtility.getMatches(this.config.anUnitRegex, source).pop();
        if (!match) {
            match = RegExpUtility.getMatches(this.config.halfDateUnitRegex, source).pop();
        }
        if (!match) return result;
        let num = isNullOrEmpty(match.groups('half').value) ? 1 : 0.5
        num += this.parseNumberWithUnitAndSuffix(source);
        let numStr = num.toString();

        let sourceUnit = match.groups('unit').value;
        if (this.config.unitMap.has(sourceUnit)) {
            let unitStr = this.config.unitMap.get(sourceUnit);

            result.timex = `P${this.isLessThanDay(unitStr) ? 'T' : ''}${num}${unitStr[0]}`;
            result.futureValue = num * this.config.unitValueMap.get(sourceUnit);
            result.pastValue = result.futureValue;
            result.success = true;
            return result;
        }
        return result;
    }

    private parseInExactNumberUnit(source: string): DateTimeResolutionResult {
        let result = new DateTimeResolutionResult();
        let match = RegExpUtility.getMatches(this.config.inExactNumberUnitRegex, source).pop();
        if (!match) return result;

        let num = 3;
        let numStr = num.toString();

        let sourceUnit = match.groups('unit').value;
        if (this.config.unitMap.has(sourceUnit)) {
            let unitStr = this.config.unitMap.get(sourceUnit);
            if (num > 1000 && (unitStr === 'Y' || unitStr === 'MON' || unitStr === 'W')) {
                return result;
            }

            result.timex = `P${this.isLessThanDay(unitStr) ? 'T' : ''}${num}${unitStr[0]}`;
            result.futureValue = num * this.config.unitValueMap.get(sourceUnit);
            result.pastValue = result.futureValue;
            result.success = true;
            return result;
        }
        return result;
    }

    private isLessThanDay(source: string): boolean {
        return (source === 'S') || (source === 'M') || (source === 'H')
    }
}

export class BaseDatePeriodParser implements IDateTimeParser {
    private readonly parserName = Constants.SYS_DATETIME_DATEPERIOD;

    private readonly config: IDatePeriodParserConfiguration;

    private readonly inclusiveEndPeriod = false;
    private readonly weekOfComment = 'WeekOf';
    private readonly monthOfComment = 'MonthOf';

    constructor(config: IDatePeriodParserConfiguration) {
        this.config = config;
    }

    parse(extractorResult: ExtractResult, referenceDate?: Date): DateTimeParseResult | null {
        if (!referenceDate) referenceDate = new Date();
        let resultValue;
        if (extractorResult.type === this.parserName) {
            let source = extractorResult.text.trim().toLowerCase();
            let innerResult = this.parseMonthWithYear(source, referenceDate);
            if (!innerResult.success) {
                innerResult = this.parseSimpleCases(source, referenceDate);
            }
            if (!innerResult.success) {
                innerResult = this.parseOneWordPeriod(source, referenceDate);
            }
            if (!innerResult.success) {
                innerResult = this.mergeTwoTimePoints(source, referenceDate);
            }
            if (!innerResult.success) {
                innerResult = this.parseYear(source, referenceDate);
            }
            if (!innerResult.success) {
                innerResult = this.parseDuration(source, referenceDate);
            }
            if (!innerResult.success) {
                innerResult = this.parseWeekOfMonth(source, referenceDate);
            }
            if (!innerResult.success) {
                innerResult = this.parseWeekOfYear(source, referenceDate);
            }
            if (!innerResult.success) {
                innerResult = this.parseQuarter(source, referenceDate);
            }
            if (!innerResult.success) {
                innerResult = this.parseSeason(source, referenceDate);
            }
            if (!innerResult.success) {
                innerResult = this.parseWhichWeek(source, referenceDate);
            }
            if (!innerResult.success) {
                innerResult = this.parseWeekOfDate(source, referenceDate);
            }
            if (!innerResult.success) {
                innerResult = this.parseMonthOfDate(source, referenceDate);
            }
            if (innerResult.success) {
                if (innerResult.futureValue && innerResult.pastValue) {
                    innerResult.futureResolution = new Map<string, string>()
                        .set(TimeTypeConstants.START_DATE, innerResult.futureValue.item1)
                        .set(TimeTypeConstants.END_DATE, innerResult.futureValue.item2);
                    innerResult.pastResolution = new Map<string, string>()
                        .set(TimeTypeConstants.START_DATE, innerResult.pastValue.item1)
                        .set(TimeTypeConstants.END_DATE, innerResult.pastValue.item2);
                } else {
                    innerResult.futureResolution = new Map<string, string>();
                    innerResult.pastResolution = new Map<string, string>();
                }
                resultValue = innerResult;
            }
        }
        let result = new DateTimeParseResult(extractorResult);
        result.value = resultValue;
        result.timexStr = resultValue ? resultValue.timex : '';
        result.resolutionStr = '';

        return result;
    }
    
    private parseMonthWithYear(source: string, referenceDate: Date): DateTimeResolutionResult {
        let trimmedSource = source.trim().toLowerCase();
        let result = new DateTimeResolutionResult();
        let match = RegExpUtility.getMatches(this.config.MonthWithYear, trimmedSource).pop();
        if (!match) {
            match = RegExpUtility.getMatches(this.config.MonthNumWithYear, trimmedSource).pop();
        }
        if (!match || match.length !== trimmedSource.length) return result;

        let monthStr = match.groups('month').value;
        let yearStr = match.groups('year').value;
        let orderStr = match.groups('order').value;

        let month = this.config.MonthOfYear.get(monthStr) - 1;
        let year = Number.parseInt(yearStr);
        if (!year || isNaN(year)) {
            let swift = this.config.GetSwiftYear(orderStr);
            if (swift < -1) return result;
            year = referenceDate.getFullYear() + swift;
        }
        let beginDate = DateUtils.safeCreateFromValue(DateUtils.minValue(), year, month, 1);
        let endDate = DateUtils.addDays(DateUtils.addMonths(beginDate, 1), this.inclusiveEndPeriod ? -1 : 0);
        result.futureValue = [beginDate, endDate];
        result.pastValue = [beginDate, endDate];
        result.timex = `${FormatUtil.toString(year, 4)}-${FormatUtil.toString(month + 1, 2)}`;
        result.success = true;
        return result;
    }
    
    private parseSimpleCases(source: string, referenceDate: Date): DateTimeResolutionResult {
        let result = new DateTimeResolutionResult();
        let year = referenceDate.getFullYear();
        let month = referenceDate.getMonth();
        let noYear = false;
        let match = RegExpUtility.getMatches(this.config.MonthFrontBetweenRegex, source).pop();
        if (!match) {
            match = RegExpUtility.getMatches(this.config.BetweenRegex, source).pop();
        }
        if (!match) {
            match = RegExpUtility.getMatches(this.config.MonthFrontSimpleCasesRegex, source).pop();
        }
        if (!match) {
            match = RegExpUtility.getMatches(this.config.SimpleCasesRegex, source).pop();
        }
        if (!match || match.index !== 0 || match.length !== source.length) return result;
        let days = match.groups('day');
        let beginDay = this.config.DayOfMonth.get(days.captures[0]);
        let endDay = this.config.DayOfMonth.get(days.captures[1]);
        let monthStr = match.groups('month').value;
        if (!isNullOrEmpty(monthStr)) {
            month = this.config.MonthOfYear.get(monthStr) - 1;
            noYear = true;
        } else {
            monthStr = match.groups('relmonth').value;
            month += this.config.getSwiftDayOrMonth(monthStr);
            if (month < 0) {
                month = 0;
                year--;
            } else if (month > 11) {
                month = 11;
                year++;
            }
        }
        let beginDateLuis = FormatUtil.luisDate(this.config.IsFuture(monthStr) ? year : -1, month, beginDay);
        let endDateLuis = FormatUtil.luisDate(this.config.IsFuture(monthStr) ? year : -1, month, endDay);

        let yearStr = match.groups('year').value;
        if (!isNullOrEmpty(yearStr)) {
            year = Number.parseInt(yearStr);
            noYear = false;
        }
        let futureYear = year;
        let pastYear = year;
        let startDate = DateUtils.safeCreateFromValue(DateUtils.minValue(), year, month, beginDay);
        if (noYear && startDate < referenceDate) futureYear++;
        if (noYear && startDate >= referenceDate) pastYear--;

        result.timex = `(${beginDateLuis},${endDateLuis},P${endDay - beginDay}D)`;
        result.futureValue = [
            DateUtils.safeCreateFromValue(DateUtils.minValue(), futureYear, month, beginDay),
            DateUtils.safeCreateFromValue(DateUtils.minValue(), futureYear, month, endDay),
        ];
        result.pastValue = [
            DateUtils.safeCreateFromValue(DateUtils.minValue(), pastYear, month, beginDay),
            DateUtils.safeCreateFromValue(DateUtils.minValue(), pastYear, month, endDay),
        ];
        result.success = true;
        return result;
    }
    
    private parseOneWordPeriod(source: string, referenceDate: Date): DateTimeResolutionResult {
        let result = new DateTimeResolutionResult();
        let year = referenceDate.getFullYear();
        let month = referenceDate.getMonth();

        if (this.config.IsYearToDate(source)) {
            result.timex = FormatUtil.toString(year, 4);
            result.futureValue = [DateUtils.safeCreateFromValue(DateUtils.minValue(), year, 0, 1), referenceDate];
            result.pastValue = [DateUtils.safeCreateFromValue(DateUtils.minValue(), year, 0, 1), referenceDate];
            result.success = true;
            return result;
        }
        if (this.config.IsMonthToDate(source)) {
            result.timex = `${FormatUtil.toString(year, 4)}-${FormatUtil.toString(month + 1, 2)}`;
            result.futureValue = [DateUtils.safeCreateFromValue(DateUtils.minValue(), year, month, 1), referenceDate];
            result.pastValue = [DateUtils.safeCreateFromValue(DateUtils.minValue(), year, month, 1), referenceDate];
            result.success = true;
            return result;
        }

        let futureYear = year;
        let pastYear = year;
        let match = RegExpUtility.getMatches(this.config.OneWordPeriodRegex, source).pop();
        if (!match || match.index !== 0 || match.length !== source.length) return result;
        let monthStr = match.groups('month').value;
        if (!isNullOrEmpty(monthStr)) {
            let swift = this.config.GetSwiftYear(source);
            month = this.config.MonthOfYear.get(monthStr) - 1;
            if (swift >= -1) {
                result.timex = `${FormatUtil.toString(year + swift, 4)}-${FormatUtil.toString(month + 1, 2)}`;
                year += swift;
                futureYear = year;
                pastYear = year;
            } else {
                result.timex = `XXXX-${FormatUtil.toString(month + 1, 2)}`;
                if (month < referenceDate.getMonth()) futureYear++;
                if (month >= referenceDate.getMonth()) pastYear--;
            }
        } else {
            let swift = this.config.getSwiftDayOrMonth(source);
            if (this.config.IsWeekOnly(source)) {
                let monday = DateUtils.addDays(DateUtils.this(referenceDate, DayOfWeek.Monday), 7 * swift);
                let sunday = DateUtils.addDays(DateUtils.this(referenceDate, DayOfWeek.Sunday), (7 * swift) + (this.inclusiveEndPeriod ? 0 : 1));

                result.timex = `${FormatUtil.toString(monday.getFullYear(), 4)}-W${FormatUtil.toString(DateUtils.getWeekNumber(monday).weekNo, 2)}`;
                result.futureValue = [monday, sunday];
                result.pastValue = [monday, sunday];
                result.success = true;
                return result;
            }
            if (this.config.IsWeekend(source)) {
                let beginDate = DateUtils.addDays(DateUtils.this(referenceDate, DayOfWeek.Saturday), 7 * swift);
                let endDate = DateUtils.addDays(DateUtils.this(referenceDate, DayOfWeek.Sunday), (7 * swift) + (this.inclusiveEndPeriod ? 0 : 1));

                result.timex = `${FormatUtil.toString(beginDate.getFullYear(), 4)}-W${FormatUtil.toString(DateUtils.getWeekNumber(beginDate).weekNo, 2)}-WE`;
                result.futureValue = [beginDate, endDate];
                result.pastValue = [beginDate, endDate];
                result.success = true;
                return result;
            }
            if (this.config.IsMonthOnly(source)) {
                let tempDate = new Date(referenceDate);
                tempDate.setMonth(referenceDate.getMonth() + swift);
                month = tempDate.getMonth();
                year = tempDate.getFullYear();
                result.timex = `${FormatUtil.toString(year, 4)}-${FormatUtil.toString(month + 1, 2)}`;
                futureYear = year;
                pastYear = year;
            } else if (this.config.IsYearOnly(source)) {
                let tempDate = new Date(referenceDate);
                tempDate.setFullYear(referenceDate.getFullYear() + swift);
                year = tempDate.getFullYear();
                result.timex = FormatUtil.toString(year, 4);
                result.futureValue = [
                    DateUtils.safeCreateFromValue(DateUtils.minValue(), year, 0, 1),
                    DateUtils.addDays(DateUtils.safeCreateFromValue(DateUtils.minValue(), year, 11, 31), this.inclusiveEndPeriod ? 0 : 1)
                ];
                result.pastValue = [
                    DateUtils.safeCreateFromValue(DateUtils.minValue(), year, 0, 1),
                    DateUtils.addDays(DateUtils.safeCreateFromValue(DateUtils.minValue(), year, 11, 31), this.inclusiveEndPeriod ? 0 : 1)
                ];
                result.success = true;
                return result;
            }
        }
        result.futureValue = [
            DateUtils.safeCreateFromValue(DateUtils.minValue(), futureYear, month, 1), 
            DateUtils.addDays(DateUtils.addMonths(DateUtils.safeCreateFromValue(DateUtils.minValue(), futureYear, month, 1), 1), this.inclusiveEndPeriod ? -1 : 0)
        ];
        result.pastValue = [
            DateUtils.safeCreateFromValue(DateUtils.minValue(), pastYear, month, 1), 
            DateUtils.addDays(DateUtils.addMonths(DateUtils.safeCreateFromValue(DateUtils.minValue(), pastYear, month, 1), 1), this.inclusiveEndPeriod ? -1 : 0)
        ];
        result.success = true;
        return result;
    }
    
    private mergeTwoTimePoints(source: string, referenceDate: Date): DateTimeResolutionResult {
        let trimmedSource = source.trim();
        let result = new DateTimeResolutionResult();
        let ers = this.config.DateExtractor.extract(trimmedSource);
        if (!ers || ers.length < 2) {
            ers = this.config.DateExtractor.extract(this.config.TokenBeforeDate + trimmedSource)
                .map(er => {
                    er.start -= this.config.TokenBeforeDate.length;
                    return er;
                });
            if (!ers || ers.length < 2) return result;
        }
        let prs = ers.map(er => this.config.DateParser.parse(er, referenceDate)).filter(pr => pr);
        if (prs.length < 2) return result;

        let prBegin = prs[0];
        let prEnd = prs[1];
        let futureBegin = prBegin.value.futureValue;
        let futureEnd = prEnd.value.futureValue;
        let pastBegin = prBegin.value.pastValue;
        let pastEnd = prEnd.value.pastValue;

        result.timex = `(${prBegin.timexStr},${prEnd.timexStr},P${DateUtils.diffDays(futureEnd, futureBegin)}D)`;
        result.futureValue = [futureBegin, futureEnd];
        result.pastValue = [pastBegin, pastEnd];
        result.success = true;
        return result;
    }
    
    private parseYear(source: string, referenceDate: Date): DateTimeResolutionResult {
        let trimmedSource = source.trim();
        let result = new DateTimeResolutionResult();
        let match = RegExpUtility.getMatches(this.config.YearRegex, trimmedSource).pop();
        if (!match || match.length !== trimmedSource.length) return result;

        let year = Number.parseInt(match.value);
        let beginDate = DateUtils.safeCreateFromValue(DateUtils.minValue(), year, 0, 1);
        let endDate = DateUtils.addDays(DateUtils.safeCreateFromValue(DateUtils.minValue(), year + 1, 0, 1), this.inclusiveEndPeriod ? -1 : 0);
        result.timex = FormatUtil.toString(year, 4);
        result.futureValue = [beginDate, endDate];
        result.pastValue = [beginDate, endDate];
        result.success = true;
        return result;
    }
    
    private parseDuration(source: string, referenceDate: Date): DateTimeResolutionResult {
        let result = new DateTimeResolutionResult();
        let ers = this.config.DurationExtractor.extract(source);
        if (!ers || ers.length !== 1) return result;

        let pr = this.config.DurationParser.parse(ers[0]);
        if (pr === null) return result;

        let beforeStr = source.substr(0, pr.start).trim();
        let durationResult: DateTimeResolutionResult = pr.value;
        if (isNullOrEmpty(durationResult.timex)) return result;

        let beginDate = new Date(referenceDate);
        let endDate = new Date(referenceDate);
        let prefixMatch = RegExpUtility.getMatches(this.config.PastRegex, beforeStr).pop();
        if (prefixMatch) {
            beginDate = this.getSwiftDate(endDate, durationResult.timex, false);
        }
        prefixMatch = RegExpUtility.getMatches(this.config.FutureRegex, beforeStr).pop();
        if (prefixMatch && prefixMatch.length === beforeStr.length) {
            beginDate = new Date(referenceDate.getFullYear(), referenceDate.getMonth(), referenceDate.getDate() + 1);
            endDate = this.getSwiftDate(beginDate, durationResult.timex, true);
        }
        prefixMatch = RegExpUtility.getMatches(this.config.InConnectorRegex, beforeStr).pop();
        if (prefixMatch && prefixMatch.length === beforeStr.length) {
            beginDate = new Date(referenceDate.getFullYear(), referenceDate.getMonth(), referenceDate.getDate() + 1);
            endDate = this.getSwiftDate(beginDate, durationResult.timex, true);

            let unit = durationResult.timex.substr(durationResult.timex.length - 1);
            durationResult.timex = `P1${unit}`;
            beginDate = this.getSwiftDate(endDate, durationResult.timex, false);
        }
        if (beginDate === endDate) return result;
        if (this.inclusiveEndPeriod) {
            endDate.setDate(endDate.getDate() - 1);
        }

        result.timex = `(${FormatUtil.luisDateFromDate(beginDate)},${FormatUtil.luisDateFromDate(endDate)},${durationResult.timex})`;
        result.futureValue = [beginDate, endDate];
        result.pastValue = [beginDate, endDate];
        result.success = true;
        return result;
    }

    private getSwiftDate(date: Date, timex: string, isPositiveSwift: boolean): Date {
        let result = new Date(date);
        let numStr = timex.replace('P', '').substr(0, timex.length - 2);
        let unitStr = timex.substr(timex.length - 1);
        let swift = Number.parseInt(numStr) || 0;
        if (swift === 0) return result;
        
        if (!isPositiveSwift) swift *= -1;
        switch(unitStr) {
            case 'D': result.setDate(date.getDate() + swift); break;
            case 'W': result.setDate(date.getDate() + (7 * swift)); break;
            case 'M': result.setMonth(date.getMonth() + swift); break;
            case 'Y': result.setFullYear(date.getFullYear() + swift); break;
        }
        return result;
    }
    
    private parseWeekOfMonth(source: string, referenceDate: Date): DateTimeResolutionResult {
        let result = new DateTimeResolutionResult();
        let match = RegExpUtility.getMatches(this.config.WeekOfMonthRegex, source).pop();
        if (!match || match.length !== source.length) return result;

        let cardinalStr = match.groups('cardinal').value;
        let monthStr = match.groups('month').value;
        let month = referenceDate.getMonth();
        let year = referenceDate.getFullYear();
        let noYear = false;
        let cardinal = this.config.IsLastCardinal(cardinalStr) ? 5
            : this.config.CardinalMap.get(cardinalStr);
        if (isNullOrEmpty(monthStr)) {
            let swift = this.config.getSwiftDayOrMonth(source);
            let tempDate = new Date(referenceDate);
            tempDate.setMonth(referenceDate.getMonth() + swift);
            month = tempDate.getMonth();
            year = tempDate.getFullYear();
        } else {
            month = this.config.MonthOfYear.get(monthStr) - 1;
            noYear = true;
        }
        return this.getWeekOfMonth(cardinal, month, year, referenceDate, noYear);
    }

    private getWeekOfMonth(cardinal: number, month: number, year: number, referenceDate: Date, noYear: boolean): DateTimeResolutionResult {
        let result = new DateTimeResolutionResult();
        let seedDate = this.computeDate(cardinal, 1, month, year);
        if (seedDate.getMonth() !== month) {
            cardinal--;
            seedDate.setDate(seedDate.getDate() - 7);
        }
        let futureDate = new Date(seedDate);
        let pastDate = new Date(seedDate);
        if (noYear && futureDate < referenceDate) {
            futureDate = this.computeDate(cardinal, 1, month, year + 1);
            if (futureDate.getMonth() !== month) {
                futureDate.setDate(futureDate.getDate() - 7);
            }
        }
        if (noYear && pastDate >= referenceDate) {
            pastDate = this.computeDate(cardinal, 1, month, year - 1);
            if (pastDate.getMonth() !== month) {
                pastDate.setDate(pastDate.getDate() - 7);
            }
        }
        result.timex = noYear ?
            `XXXX-${FormatUtil.toString(month + 1, 2)}-W${FormatUtil.toString(cardinal, 2)}` :
            `${FormatUtil.toString(year, 4)}-${FormatUtil.toString(month + 1, 2)}-W${FormatUtil.toString(cardinal, 2)}`;
        result.futureValue = [futureDate, DateUtils.addDays(futureDate, this.inclusiveEndPeriod ? 6 : 7)];
        result.pastValue = [pastDate, DateUtils.addDays(pastDate, this.inclusiveEndPeriod ? 6 : 7)];
        result.success = true;
        return result;
    }
    
    private parseWeekOfYear(source: string, referenceDate: Date): DateTimeResolutionResult {
        let result = new DateTimeResolutionResult();
        let match = RegExpUtility.getMatches(this.config.WeekOfYearRegex, source).pop();
        if (!match || match.length !== source.length) return result;

        let cardinalStr = match.groups('cardinal').value;
        let yearStr = match.groups('year').value;
        let orderStr = match.groups('order').value;
        
        let year = Number.parseInt(yearStr);
        if (isNaN(year)) {
            let swift = this.config.GetSwiftYear(orderStr);
            if (swift < -1) return result;
            year = referenceDate.getFullYear() + swift;
        }
        if (this.config.IsLastCardinal(cardinalStr)) {
            result = this.getWeekOfMonth(5, 11, year, referenceDate, false);
        } else {
            let cardinal = this.config.CardinalMap.get(cardinalStr);
            result = this.getWeekOfMonth(cardinal, 0, year, referenceDate, false);
        }
        return result;
    }
    
    private parseQuarter(source: string, referenceDate: Date): DateTimeResolutionResult {
        let result = new DateTimeResolutionResult();
        let match = RegExpUtility.getMatches(this.config.QuarterRegex, source).pop();
        if (!match || match.length !== source.length) {
            match = RegExpUtility.getMatches(this.config.QuarterRegexYearFront, source).pop();
        }
        if (!match || match.length !== source.length) return result;
        
        let cardinalStr = match.groups('cardinal').value;
        let yearStr = match.groups('year').value;
        let orderStr = match.groups('order').value;

        let year = Number.parseInt(yearStr);
        if (isNaN(year)) {
            let swift = this.config.GetSwiftYear(orderStr);
            if (swift < -1) return result;
            year = referenceDate.getFullYear() + swift;
        }
        
        let quarterNum = this.config.CardinalMap.get(cardinalStr);
        let beginDate = DateUtils.safeCreateFromValue(DateUtils.minValue(), year, quarterNum * 3 - 3, 1);
        let endDate = DateUtils.safeCreateFromValue(DateUtils.minValue(), year, quarterNum * 3, 1);
        result.futureValue = [beginDate, endDate];
        result.pastValue = [beginDate, endDate];
        result.timex = `(${FormatUtil.luisDateFromDate(beginDate)},${FormatUtil.luisDateFromDate(endDate)},P3M)`;
        result.success = true;
        return result;
    }
    
    private parseSeason(source: string, referenceDate: Date): DateTimeResolutionResult {
        let result = new DateTimeResolutionResult();
        let match = RegExpUtility.getMatches(this.config.SeasonRegex, source).pop();
        if (!match || match.length !== source.length) return result;

        let swift = this.config.GetSwiftYear(source);
        let yearStr = match.groups('year').value;
        let year = referenceDate.getFullYear();
        let seasonStr = match.groups('seas').value;
        let season = this.config.SeasonMap.get(seasonStr);
        if (swift >= -1 || !isNullOrEmpty(yearStr)) {
            if (isNullOrEmpty(yearStr)) yearStr = FormatUtil.toString(year + swift, 4);
            result.timex = `${yearStr}-${season}`;
        } else {
            result.timex = season;
        }
        result.success = true;
        return result;
    }
    
    private parseWhichWeek(source: string, referenceDate: Date): DateTimeResolutionResult {
        let result = new DateTimeResolutionResult();
        let match = RegExpUtility.getMatches(this.config.WhichWeekRegex, source).pop();
        if (!match) return result;
        let num = Number.parseInt(match.groups('number').value);
        let year = referenceDate.getFullYear();
        let firstDay = DateUtils.safeCreateFromValue(DateUtils.minValue(), year, 0, 1);
        let firstWeekday = DateUtils.this(firstDay, DayOfWeek.Monday);
        let resultDate = DateUtils.addDays(firstWeekday, 7 * num);
        result.timex = `${FormatUtil.toString(year, 4)}-W${FormatUtil.toString(num, 2)}`;
        result.futureValue = [resultDate, DateUtils.addDays(resultDate, 7)];
        result.pastValue = [resultDate, DateUtils.addDays(resultDate, 7)];
        result.success = true;
        return result;
    }
    
    private parseWeekOfDate(source: string, referenceDate: Date): DateTimeResolutionResult {
        let result = new DateTimeResolutionResult();
        let match = RegExpUtility.getMatches(this.config.WeekOfRegex, source).pop();
        let ers = this.config.DateExtractor.extract(source);
        if (!match || ers.length !== 1) return result;

        let dateResolution: DateTimeResolutionResult = this.config.DateParser.parse(ers[0], referenceDate).value;
        result.timex = dateResolution.timex;
        result.comment = this.weekOfComment;
        result.futureValue = this.getWeekRangeFromDate(dateResolution.futureValue);
        result.pastValue = this.getWeekRangeFromDate(dateResolution.pastValue);
        result.success = true;
        return result;
    }
    
    private parseMonthOfDate(source: string, referenceDate: Date): DateTimeResolutionResult {
        let result = new DateTimeResolutionResult();
        let match = RegExpUtility.getMatches(this.config.MonthOfRegex, source).pop();
        let ers = this.config.DateExtractor.extract(source);
        if (!match || ers.length !== 1) return result;

        let dateResolution: DateTimeResolutionResult = this.config.DateParser.parse(ers[0], referenceDate).value;
        result.timex = dateResolution.timex;
        result.comment = this.monthOfComment;
        result.futureValue = this.getMonthRangeFromDate(dateResolution.futureValue);
        result.pastValue = this.getMonthRangeFromDate(dateResolution.pastValue);
        result.success = true;
        return result;
    }
    
    private computeDate(cardinal: number, weekday: number, month: number, year: number) {
        let firstDay = new Date(year, month, 1);
        let firstWeekday = DateUtils.this(firstDay, weekday);
        if (weekday === 0) weekday = 7;
        if (weekday < firstDay.getDay()) firstWeekday = DateUtils.next(firstDay, weekday);
        firstWeekday.setDate(firstWeekday.getDate() + (7 * (cardinal - 1)));
        return firstWeekday;
    }

    private getWeekRangeFromDate(seedDate: Date): Date[] {
        let beginDate = DateUtils.this(seedDate, DayOfWeek.Monday);
        let endDate = DateUtils.addDays(beginDate, this.inclusiveEndPeriod ? 6 : 7);
        return [beginDate, endDate];
    }
    
    private getMonthRangeFromDate(seedDate: Date): Date[] {
        let beginDate = DateUtils.safeCreateFromValue(DateUtils.minValue(), seedDate.getFullYear(), seedDate.getMonth(), 1);
        let endDate = DateUtils.safeCreateFromValue(DateUtils.minValue(), seedDate.getFullYear(), seedDate.getMonth() + 1, 1);
        endDate.setDate(endDate.getDate() + (this.inclusiveEndPeriod ? -1 : 0));
        return [beginDate, endDate];
    }
}

export interface IDateTimePeriodParserConfiguration {
    PureNumberFromToRegex: RegExp
    PureNumberBetweenAndRegex: RegExp
    PeriodTimeOfDayWithDateRegex: RegExp
    SpecificTimeOfDayRegex: RegExp
    PastRegex: RegExp
    FutureRegex: RegExp
    RelativeTimeUnitRegex: RegExp
    Numbers: ReadonlyMap<string, number>
    UnitMap: ReadonlyMap<string, string>
    DateExtractor: IExtractor
    TimeExtractor: IExtractor
    DateTimeExtractor: IExtractor
    DurationExtractor: IExtractor
    DateParser: IDateTimeParser
    TimeParser: IDateTimeParser
    DateTimeParser: IDateTimeParser
    DurationParser: IDateTimeParser
    GetMatchedTimeRange(source: string): {timeStr: string, beginHour: number, endHour: number, endMin: number, success: boolean}
    GetSwiftPrefix(source: string): number
}

export class BaseDateTimePeriodParser implements IDateTimeParser {
    private readonly parserName = Constants.SYS_DATETIME_DATETIMEPERIOD;

    private readonly config: IDateTimePeriodParserConfiguration;

    constructor(config: IDateTimePeriodParserConfiguration) {
        this.config = config;
    }

    parse(extractorResult: ExtractResult, referenceDate?: Date): DateTimeParseResult | null {
        if (!referenceDate) referenceDate = new Date();
        let resultValue;
        if (extractorResult.type === this.parserName) {
            let source = extractorResult.text.trim().toLowerCase();
            let innerResult = this.parseSimpleCases(source, referenceDate);
            if (!innerResult.success) {
                innerResult = this.mergeTwoTimePoints(source, referenceDate);
            }
            if (!innerResult.success) {
                innerResult = this.parseSpecificTimeOfDay(source, referenceDate);
            }
            if (!innerResult.success) {
                innerResult = this.parseDuration(source, referenceDate);
            }
            if (!innerResult.success) {
                innerResult = this.parseRelativeUnit(source, referenceDate);
            }
            if (innerResult.success) {
                innerResult.futureResolution = new Map<string, string>()
                    .set(TimeTypeConstants.START_DATETIME, innerResult.futureValue[0])
                    .set(TimeTypeConstants.END_DATETIME, innerResult.futureValue[1]);
                innerResult.pastResolution = new Map<string, string>()
                    .set(TimeTypeConstants.START_DATETIME, innerResult.pastValue[0])
                    .set(TimeTypeConstants.END_DATETIME, innerResult.pastValue[1]);
                resultValue = innerResult;
            }
        }
        let result = new DateTimeParseResult(extractorResult);
        result.value = resultValue;
        result.timexStr = resultValue ? resultValue.timex : '';
        result.resolutionStr = '';

        return result;
    }
    
    private parseSimpleCases(source: string, referenceDate: Date): DateTimeResolutionResult {
        let result = new DateTimeResolutionResult();
        let match = RegExpUtility.getMatches(this.config.PureNumberFromToRegex, source).pop();
        if (!match) {
            match = RegExpUtility.getMatches(this.config.PureNumberBetweenAndRegex, source).pop();
        }
        if (!match || match.index !== 0) return result;

        let hourGroup = match.groups('hour');
        let beginHour = this.config.Numbers.get(hourGroup.captures[0]) || Number.parseInt(hourGroup.captures[0]) || 0;
        let endHour = this.config.Numbers.get(hourGroup.captures[1]) || Number.parseInt(hourGroup.captures[1]) || 0;

        let er = this.config.DateExtractor.extract(source.substr(match.length)).pop();
        if (!er) return result;

        let pr = this.config.DateParser.parse(er, referenceDate);
        if (!pr) return result;

        let dateResult: DateTimeResolutionResult = pr.value;
        let futureDate: Date = dateResult.futureValue;
        let pastDate: Date = dateResult.pastValue;
        let dateStr = pr.timexStr;

        let hasAm = false;
        let hasPm = false;
        let pmStr = match.groups('pm').value;
        let amStr = match.groups('am').value;
        let descStr = match.groups('desc').value;

        if (!isNullOrEmpty(amStr) || descStr.startsWith('a')) {
            if (beginHour >= 12) beginHour -= 12;
            if (endHour >= 12) endHour -= 12;
            hasAm = true;
        }
        if (!isNullOrEmpty(pmStr) || descStr.startsWith('p')) {
            if (beginHour < 12) beginHour += 12;
            if (endHour < 12) endHour += 12;
            hasPm = true;
        }
        if (!hasAm && !hasPm && beginHour <= 12 && endHour <= 12) {
            result.comment = "ampm";
        }

        let beginStr = `${dateStr}T${FormatUtil.toString(beginHour, 2)}`;
        let endStr = `${dateStr}T${FormatUtil.toString(endHour, 2)}`;

        result.timex = `(${beginStr},${endStr},PT${endHour - beginHour}H)`;
        result.futureValue = [
            DateUtils.safeCreateFromMinValue(futureDate.getFullYear(), futureDate.getMonth(), futureDate.getDate(), beginHour, 0, 0),
            DateUtils.safeCreateFromMinValue(futureDate.getFullYear(), futureDate.getMonth(), futureDate.getDate(), endHour, 0, 0)
        ];
        result.pastValue = [
            DateUtils.safeCreateFromMinValue(pastDate.getFullYear(), pastDate.getMonth(), pastDate.getDate(), beginHour, 0, 0),
            DateUtils.safeCreateFromMinValue(pastDate.getFullYear(), pastDate.getMonth(), pastDate.getDate(), endHour, 0, 0)
        ];
        result.success = true;
        return result;
    }
    
    private mergeTwoTimePoints(source: string, referenceDate: Date): DateTimeResolutionResult {
        let result = new DateTimeResolutionResult();
        let prs: {begin: DateTimeParseResult, end: DateTimeParseResult};
        let timeErs = this.config.TimeExtractor.extract(source);
        let datetimeErs = this.config.DateTimeExtractor.extract(source);
        let bothHasDate = false;
        let beginHasDate = false;
        let endHasDate = false;

        if (datetimeErs.length === 2) {
            prs = this.getTwoPoints(datetimeErs[0], datetimeErs[1], this.config.DateTimeParser, this.config.DateTimeParser, referenceDate);
            bothHasDate = true;
        } else if (datetimeErs.length === 1 && timeErs.length === 2) {
            if (ExtractResult.isOverlap(datetimeErs[0], timeErs[0])) {
                prs = this.getTwoPoints(datetimeErs[0], timeErs[1], this.config.DateTimeParser, this.config.TimeParser, referenceDate);
                beginHasDate = true;
            } else {
                prs = this.getTwoPoints(timeErs[0], datetimeErs[0], this.config.TimeParser, this.config.DateTimeParser, referenceDate);
                endHasDate = true;
            }
        } else if (datetimeErs.length === 1 && timeErs.length === 1) {
            if (timeErs[0].start < datetimeErs[0].start) {
                prs = this.getTwoPoints(timeErs[0], datetimeErs[0], this.config.TimeParser, this.config.DateTimeParser, referenceDate);
                endHasDate = true;
            } else {
                prs = this.getTwoPoints(datetimeErs[0], timeErs[0], this.config.DateTimeParser, this.config.TimeParser, referenceDate);
                beginHasDate = true;
            }
        }
        if (!prs || !prs.begin || !prs.end) return result;

        let begin: DateTimeResolutionResult = prs.begin.value;
        let end: DateTimeResolutionResult = prs.end.value;

        let futureBegin: Date = begin.futureValue;
        let futureEnd: Date = end.futureValue;
        let pastBegin: Date = begin.pastValue;
        let pastEnd: Date = end.pastValue;

        if (bothHasDate) {
            if (futureBegin > futureEnd) futureBegin = pastBegin;
            if (pastEnd < pastBegin) pastEnd = futureEnd;
            result.timex = `(${prs.begin.timexStr},${prs.end.timexStr},PT${DateUtils.totalHours(futureEnd, futureBegin)}H)`;
        } else if (beginHasDate) {
            futureEnd = DateUtils.safeCreateFromMinValue(futureBegin.getFullYear(), futureBegin.getMonth(), futureBegin.getDate(), futureEnd.getHours(), futureEnd.getMinutes(), futureEnd.getSeconds());
            pastEnd = DateUtils.safeCreateFromMinValue(pastBegin.getFullYear(), pastBegin.getMonth(), pastBegin.getDate(), pastEnd.getHours(), pastEnd.getMinutes(), pastEnd.getSeconds());
            let dateStr = prs.begin.timexStr.split('T').pop();
            result.timex = `(${prs.begin.timexStr},${dateStr}${prs.end.timexStr},PT${DateUtils.totalHours(futureEnd, futureBegin)}H)`;
        } else if(endHasDate) {
            futureBegin = DateUtils.safeCreateFromMinValue(futureEnd.getFullYear(), futureEnd.getMonth(), futureEnd.getDate(), futureBegin.getHours(), futureBegin.getMinutes(), futureBegin.getSeconds());
            pastBegin = DateUtils.safeCreateFromMinValue(pastEnd.getFullYear(), pastEnd.getMonth(), pastEnd.getDate(), pastBegin.getHours(), pastBegin.getMinutes(), pastBegin.getSeconds());
            let dateStr = prs.end.timexStr.split('T')[0];
            result.timex = `(${dateStr}${prs.begin.timexStr},${prs.end.timexStr},PT${DateUtils.totalHours(futureEnd, futureBegin)}H)`;
        }
        if (!isNullOrEmpty(begin.comment) && begin.comment.endsWith('ampm') && !isNullOrEmpty(end.comment) && end.comment.endsWith('ampm')) {
            result.comment = 'ampm';
        }

        result.futureValue = [futureBegin, futureEnd];
        result.pastValue = [pastBegin, pastEnd];
        result.success = true;
        return result;
    }

    private getTwoPoints(beginEr: ExtractResult, endEr: ExtractResult, beginParser: IDateTimeParser, endParser: IDateTimeParser, referenceDate: Date)
        : { begin: DateTimeParseResult, end: DateTimeParseResult } {
        let beginPr = beginParser.parse(beginEr, referenceDate);
        let endPr = endParser.parse(endEr, referenceDate);
        return { begin: beginPr, end: endPr };
    }
    
    private parseSpecificTimeOfDay(source: string, referenceDate: Date): DateTimeResolutionResult {
        let result = new DateTimeResolutionResult();
        let timeText = source;
        let hasEarly = false;
        let hasLate = false;

        let match = RegExpUtility.getMatches(this.config.PeriodTimeOfDayWithDateRegex, source).pop();
        if (match) {
            timeText = match.groups('timeOfDay').value;
            if (!isNullOrEmpty(match.groups('early').value)) {
                hasEarly = true;
                result.comment = 'early';
            } else if (!isNullOrEmpty(match.groups('late').value)) {
                hasLate = true;
                result.comment = 'late';
            }
        }

        let matched = this.config.GetMatchedTimeRange(timeText);
        if (!matched || !matched.success) return result;

        if (hasEarly) {
            matched.endHour = matched.beginHour + 2;
            if (matched.endMin === 59) matched.endMin = 0;
        } else if (hasLate) {
            matched.beginHour += 2;
        }

        match = RegExpUtility.getMatches(this.config.SpecificTimeOfDayRegex, source).pop();
        if (match && match.index === 0 && match.length === source.length) {
            let swift = this.config.GetSwiftPrefix(source);
            let date = DateUtils.addDays(referenceDate, swift);
            result.timex = FormatUtil.formatDate(date) + matched.timeStr;
            result.futureValue = [
                DateUtils.safeCreateFromMinValue(date.getFullYear(), date.getMonth(), date.getDate(), matched.beginHour, 0, 0),
                DateUtils.safeCreateFromMinValue(date.getFullYear(), date.getMonth(), date.getDate(), matched.endHour, matched.endMin, matched.endMin),
            ];
            result.pastValue = [
                DateUtils.safeCreateFromMinValue(date.getFullYear(), date.getMonth(), date.getDate(), matched.beginHour, 0, 0),
                DateUtils.safeCreateFromMinValue(date.getFullYear(), date.getMonth(), date.getDate(), matched.endHour, matched.endMin, matched.endMin),
            ];
            result.success = true;
            return result;    
        }

        match = RegExpUtility.getMatches(this.config.PeriodTimeOfDayWithDateRegex, source).pop();
        if (!match) return result;

        let beforeStr = source.substr(0, match.index).trim();
        let ers = this.config.DateExtractor.extract(beforeStr);
        if (ers.length === 0 || ers[0].length !== beforeStr.length) {
            let afterStr = source.substr(match.index + match.length).trim();
            ers = this.config.DateExtractor.extract(afterStr);
            if (ers.length === 0 || ers[0].length !== afterStr.length) return result;
        }

        let pr = this.config.DateParser.parse(ers[0], referenceDate);
        if (!pr) return result;

        let futureDate: Date = pr.value.futureValue;
        let pastDate: Date =  pr.value.pastValue;

        result.timex = pr.timexStr + matched.timeStr;
        result.futureValue = [
            DateUtils.safeCreateFromMinValue(futureDate.getFullYear(), futureDate.getMonth(), futureDate.getDate(), matched.beginHour, 0, 0),
            DateUtils.safeCreateFromMinValue(futureDate.getFullYear(), futureDate.getMonth(), futureDate.getDate(), matched.endHour, matched.endMin, matched.endMin),
        ];
        result.pastValue = [
            DateUtils.safeCreateFromMinValue(pastDate.getFullYear(), pastDate.getMonth(), pastDate.getDate(), matched.beginHour, 0, 0),
            DateUtils.safeCreateFromMinValue(pastDate.getFullYear(), pastDate.getMonth(), pastDate.getDate(), matched.endHour, matched.endMin, matched.endMin),
        ];
        result.success = true;
        return result;
    }
    
    private parseDuration(source: string, referenceDate: Date): DateTimeResolutionResult {
        let result = new DateTimeResolutionResult();
        let ers = this.config.DurationExtractor.extract(source);
        if (!ers || ers.length !== 1) return result;

        let pr = this.config.DurationParser.parse(ers[0], referenceDate);
        if (!pr) return result;

        let beforeStr = source.substr(0, pr.start).trim();
        let durationResult: DateTimeResolutionResult = pr.value;
        let swiftSecond = 0;
        if (Number.isFinite(durationResult.pastValue) && Number.isFinite(durationResult.futureValue)) {
            swiftSecond = Math.round(durationResult.futureValue);
        }
        let beginTime = new Date(referenceDate);
        let endTime = new Date(referenceDate);
        let prefixMatch = RegExpUtility.getMatches(this.config.PastRegex, beforeStr).pop();
        if (prefixMatch && prefixMatch.length === beforeStr.length) {
            beginTime.setSeconds(referenceDate.getSeconds() - swiftSecond);
        }
        prefixMatch = RegExpUtility.getMatches(this.config.FutureRegex, beforeStr).pop();
        if (prefixMatch && prefixMatch.length === beforeStr.length) {
            endTime = new Date(beginTime);
            endTime.setSeconds(beginTime.getSeconds() + swiftSecond);
        }

        let luisDateBegin = FormatUtil.luisDateFromDate(beginTime);
        let luisTimeBegin = FormatUtil.luisTimeFromDate(beginTime);
        let luisDateEnd = FormatUtil.luisDateFromDate(endTime);
        let luisTimeEnd = FormatUtil.luisTimeFromDate(endTime);

        result.timex = `(${luisDateBegin}T${luisTimeBegin},${luisDateEnd}T${luisTimeEnd},${durationResult.timex})`;
        result.futureValue = [beginTime, endTime];
        result.pastValue = [beginTime, endTime];
        result.success = true;
        return result;
    }

    private isFloat(value: any): boolean {
        return Number.isFinite(value) && !Number.isInteger(value);
    }
    
    private parseRelativeUnit(source: string, referenceDate: Date): DateTimeResolutionResult {
        let result = new DateTimeResolutionResult();
        let match = RegExpUtility.getMatches(this.config.RelativeTimeUnitRegex, source).pop();
        if (!match) return result;

        let srcUnit = match.groups('unit').value;
        let unitStr = this.config.UnitMap.get(srcUnit);

        if (!unitStr) return result;
        let swift = 1;
        let prefixMatch = RegExpUtility.getMatches(this.config.PastRegex, source).pop();
        if (prefixMatch) swift = -1;

        let beginTime = new Date(referenceDate);
        let endTime = new Date(referenceDate);

        switch (unitStr) {
            case 'H':
                beginTime.setHours(beginTime.getHours() + (swift > 0 ? 0 : swift));
                endTime.setHours(endTime.getHours() + (swift > 0 ? swift : 0));
                break;
            case 'M':
                beginTime.setMinutes(beginTime.getMinutes() + (swift > 0 ? 0 : swift));
                endTime.setMinutes(endTime.getMinutes() + (swift > 0 ? swift : 0));
                break;
            case 'S':
                beginTime.setSeconds(beginTime.getSeconds() + (swift > 0 ? 0 : swift));
                endTime.setSeconds(endTime.getSeconds() + (swift > 0 ? swift : 0));
                break;
            default: return result;
        }
        
        let luisDateBegin = FormatUtil.luisDateFromDate(beginTime);
        let luisTimeBegin = FormatUtil.luisTimeFromDate(beginTime);
        let luisDateEnd = FormatUtil.luisDateFromDate(endTime);
        let luisTimeEnd = FormatUtil.luisTimeFromDate(endTime);

        result.timex = `(${luisDateBegin}T${luisTimeBegin},${luisDateEnd}T${luisTimeEnd},PT1${unitStr[0]})`;
        result.futureValue = [beginTime, endTime];
        result.pastValue = [beginTime, endTime];
        result.success = true;
        return result;
    }
}

export interface IMergedParserConfiguration {
    BeforeRegex: RegExp
    AfterRegex: RegExp
    DateParser: IDateTimeParser
    HolidayParser: IDateTimeParser
    TimeParser: IDateTimeParser
    DateTimeParser: IDateTimeParser
    DatePeriodParser: IDateTimeParser
    TimePeriodParser: IDateTimeParser
    DateTimePeriodParser: IDateTimeParser
    DurationParser: IDateTimeParser
    SetParser: IDateTimeParser
}

export class BaseMergedParser implements IDateTimeParser {
    readonly parserName = 'datetimeV2';

    private readonly config: IMergedParserConfiguration;

    private readonly dateMinValue = FormatUtil.formatDate(DateUtils.minValue());
    private readonly dateTimeMinValue = FormatUtil.FormatDateTime(DateUtils.minValue());

    constructor(config: IMergedParserConfiguration) {
        this.config = config;
    }

    parse(extractorResult: ExtractResult, referenceDate?: Date): DateTimeParseResult | null {
        if (!referenceDate) referenceDate = new Date();
        let matchMode = MatchMode.None;
        let match = RegExpUtility.getMatches(this.config.BeforeRegex, extractorResult.text).pop();
        if (match) {
            matchMode = MatchMode.Before;
        } else {
            match = RegExpUtility.getMatches(this.config.AfterRegex, extractorResult.text).pop();
            if (match) matchMode = MatchMode.After;
        }
        if (match) {
            extractorResult.start += match.length;
            extractorResult.length -= match.length;
            extractorResult.text = extractorResult.text.substr(match.length);
        }
        let result = this.getParseResult(extractorResult, referenceDate);
        if (!result) return null;

        if (match && matchMode !== MatchMode.None && result.value) {
            result.start -= match.length;
            result.length += match.length;
            result.text = match.value + result.text;
            let value: DateTimeResolutionResult = result.value;
            value.mod = matchMode === MatchMode.Before ? TimeTypeConstants.beforeMod : TimeTypeConstants.afterMod;
            result.value = value;
        }
        result.value = this.dateTimeResolution(result, matchMode);
        result.type = `${this.parserName}.${this.determineDateTimeType(extractorResult.type, matchMode)}`;
        return result;
    }

    protected getParseResult(extractorResult: ExtractResult, referenceDate: Date): DateTimeParseResult | null {
        let extractorType = extractorResult.type;
        if (extractorType === Constants.SYS_DATETIME_DATE) {
            let pr = this.config.DateParser.parse(extractorResult, referenceDate);
            if (!pr || !pr.value) return this.config.HolidayParser.parse(extractorResult, referenceDate);
            return pr;
        }
        if (extractorType === Constants.SYS_DATETIME_TIME) {
            return this.config.TimeParser.parse(extractorResult, referenceDate);
        }
        if (extractorType === Constants.SYS_DATETIME_DATETIME) {
            return this.config.DateTimeParser.parse(extractorResult, referenceDate);
        }
        if (extractorType === Constants.SYS_DATETIME_DATEPERIOD) {
            return this.config.DatePeriodParser.parse(extractorResult, referenceDate);
        }
        if (extractorType === Constants.SYS_DATETIME_TIMEPERIOD) {
            return this.config.TimePeriodParser.parse(extractorResult, referenceDate);
        }
        if (extractorType === Constants.SYS_DATETIME_DATETIMEPERIOD) {
            return this.config.DateTimePeriodParser.parse(extractorResult, referenceDate);
        }
        if (extractorType === Constants.SYS_DATETIME_DURATION) {
            return this.config.DurationParser.parse(extractorResult, referenceDate);
        }
        if (extractorType === Constants.SYS_DATETIME_SET) {
            return this.config.SetParser.parse(extractorResult, referenceDate);
        }
        return null;
    }

    private determineDateTimeType(type: string, matchMode: MatchMode): string {
        if (matchMode !== MatchMode.None) {
            if (type === Constants.SYS_DATETIME_DATE) return Constants.SYS_DATETIME_DATEPERIOD;
            if (type === Constants.SYS_DATETIME_TIME) return Constants.SYS_DATETIME_TIMEPERIOD;
            if (type === Constants.SYS_DATETIME_DATETIME) return Constants.SYS_DATETIME_DATETIMEPERIOD;
        }
        return type;
    }

    private dateTimeResolution(slot: DateTimeParseResult, matchMode: MatchMode): Map<string, any> {
        if (!slot) return null;
        
        let result = new Map<string, any>();
        let resolutions = new Array<Map<string, string>>();

        let type = slot.type;
        let outputType = this.determineDateTimeType(type, matchMode);
        let timex = slot.timexStr;

        let value: DateTimeResolutionResult = slot.value;
        if (!value) return null;

        let isLunar = value.isLunar;
        let mod = value.mod;
        let comment = value.comment;

        if (!isNullOrEmpty(timex)) result.set('timex', timex);
        if (!isNullOrEmpty(comment)) result.set('Comment', comment);
        if (!isNullOrEmpty(mod)) result.set('Mod', mod);
        if (!isNullOrEmpty(type)) result.set('type', type);

        let futureResolution = value.futureResolution;
        let pastResolution = value.pastResolution;

        let future = this.generateFromResolution(type, futureResolution, mod);
        let past = this.generateFromResolution(type, pastResolution, mod);

        let futureValues = Array.from(future.values()).sort();
        let pastValues = Array.from(past.values()).sort();
        if (futureValues.length === pastValues.length && futureValues.every((v,i) => v === pastValues[i])) {
            if (past.size > 0) result.set('resolve', past);
        } else {
            if (past.size > 0) result.set('resolveToPast', past);
            if (future.size > 0) result.set('resolveToFuture', future);
        }

        if (comment && comment === 'ampm') {
            if (result.has('resolve')) {
                this.resolveAMPM(result, 'resolve');
            } else {
                this.resolveAMPM(result, 'resolveToPast');
                this.resolveAMPM(result, 'resolveToFuture');
            }
        }

        if (isLunar) result.set('isLunar', isLunar);

        result.forEach((value, key) => {
            if (value instanceof Map) {
                let newValues = new Map<string, string>();
                if (!isNullOrEmpty(timex)) newValues.set('timex', timex);
                if (!isNullOrEmpty(type)) newValues.set('type', type);
                value.forEach((innerValue, innerKey) => {
                    newValues.set(innerKey, innerValue);
                });
                resolutions.push(newValues);
            }
        });

        if (past.size === 0 && future.size === 0) {
            resolutions.push(new Map<string, string>()
                .set('timex', timex)
                .set('type', outputType)
                .set('value', 'not resolved'));
        }
        return new Map<string, any>().set('values', resolutions);
    }

    private generateFromResolution(type: string, resolutions: Map<string, string>, mod: string): Map<string, string> {
        let result = new Map<string, string>();
        switch (type) {
            case Constants.SYS_DATETIME_DATETIME:
                this.addSingleDateTimeToResolution(resolutions, TimeTypeConstants.DATETIME, mod, result);
                break;
            case Constants.SYS_DATETIME_TIME:
                this.addSingleDateTimeToResolution(resolutions, TimeTypeConstants.TIME, mod, result);
                break;
            case Constants.SYS_DATETIME_DATE:
                this.addSingleDateTimeToResolution(resolutions, TimeTypeConstants.DATE, mod, result);
                break;
            case Constants.SYS_DATETIME_DURATION:
                if (resolutions.has(TimeTypeConstants.DURATION)) {
                    result.set(TimeTypeConstants.VALUE, resolutions.get(TimeTypeConstants.DURATION));
                }
                break;
            case Constants.SYS_DATETIME_TIMEPERIOD:
                this.addPeriodToResolution(resolutions, TimeTypeConstants.START_TIME, TimeTypeConstants.END_TIME, mod, result);
                break;
            case Constants.SYS_DATETIME_DATEPERIOD:
                this.addPeriodToResolution(resolutions, TimeTypeConstants.START_DATE, TimeTypeConstants.END_DATE, mod, result);
                break;
            case Constants.SYS_DATETIME_DATETIMEPERIOD:
                this.addPeriodToResolution(resolutions, TimeTypeConstants.START_DATETIME, TimeTypeConstants.END_DATETIME, mod, result);
                break;
        }
        return result;
    }

    private addSingleDateTimeToResolution(resolutions: Map<string, string>, type: string, mod: string, result: Map<string, string>) {
        let key = TimeTypeConstants.VALUE;
        let value = resolutions.get(type);
        if (!value || this.dateMinValue === value || this.dateTimeMinValue === value) return;

        if (!isNullOrEmpty(mod)) {
            if (mod === TimeTypeConstants.beforeMod) key = TimeTypeConstants.END;
            else if (mod === TimeTypeConstants.afterMod) key = TimeTypeConstants.START;
        }
        result.set(key, value);
    }

    private addPeriodToResolution(resolutions: Map<string, string>, startType: string, endType: string, mod: string, result: Map<string, string>) {
        let start = resolutions.get(startType);
        let end = resolutions.get(endType);
        if (!isNullOrEmpty(mod)) {
            if (mod === TimeTypeConstants.beforeMod) {
                result.set(TimeTypeConstants.END, start);
                return;
            }
            if (mod === TimeTypeConstants.afterMod) {
                result.set(TimeTypeConstants.START, end);
                return;
            }
        }
        if (isNullOrEmpty(start) || isNullOrEmpty(end)) return;

        result.set(TimeTypeConstants.START, start);
        result.set(TimeTypeConstants.END, end);
    }

    private resolveAMPM(valuesMap: Map<string, any>, keyName: string) {
        if (!valuesMap.has(keyName)) return;

        let resolution: Map<string, string> = valuesMap.get(keyName);
        if (!resolution.has('timex')) return;

        let timex = resolution.get('timex');
        valuesMap.delete(keyName);
        valuesMap.set(keyName + 'Am', resolution);

        let resolutionPm = new Map<string, string>();
        switch (valuesMap.get('type')) {
            case Constants.SYS_DATETIME_TIME:
                resolutionPm.set(TimeTypeConstants.VALUE, FormatUtil.toPm(resolution.get(TimeTypeConstants.VALUE)));
                resolutionPm.set('timex', FormatUtil.toPm(timex));
                break;
            case Constants.SYS_DATETIME_DATETIME:
                let splitValue = resolution.get(TimeTypeConstants.VALUE).split(' ');
                resolutionPm.set(TimeTypeConstants.VALUE, `${splitValue[0]} ${FormatUtil.toPm(splitValue[1])}`);
                resolutionPm.set('timex', FormatUtil.AllStringToPm(timex));
                break;
            case Constants.SYS_DATETIME_TIMEPERIOD:
                if (resolution.has(TimeTypeConstants.START)) resolutionPm.set(TimeTypeConstants.START, FormatUtil.toPm(resolution.get(TimeTypeConstants.START)));
                if (resolution.has(TimeTypeConstants.END)) resolutionPm.set(TimeTypeConstants.END, FormatUtil.toPm(resolution.get(TimeTypeConstants.END)));
                resolutionPm.set('timex', FormatUtil.AllStringToPm(timex));
                break;
            case Constants.SYS_DATETIME_DATETIMEPERIOD:
                if (resolution.has(TimeTypeConstants.START)) {
                    let splitValue = resolution.get(TimeTypeConstants.START).split(' ');
                    resolutionPm.set(TimeTypeConstants.START, `${splitValue[0]} ${FormatUtil.toPm(splitValue[1])}`);
                }
                if (resolution.has(TimeTypeConstants.END)) {
                    let splitValue = resolution.get(TimeTypeConstants.END).split(' ');
                    resolutionPm.set(TimeTypeConstants.END, `${splitValue[0]} ${FormatUtil.toPm(splitValue[1])}`);
                }
                resolutionPm.set('timex', FormatUtil.AllStringToPm(timex));
                break;
        }
        valuesMap.set(keyName + 'Pm', resolutionPm);
    }
}

enum MatchMode {
    Before, After, None
}

export enum AgoLaterMode {
    Date, DateTime
}

function parserDurationWithAgoAndLater(source: string, referenceDate: Date, durationExtractor: IExtractor, durationParser: IDateTimeParser, unitMap: ReadonlyMap<string, string>, unitRegex: RegExp, utilityConfiguration: IDateTimeUtilityConfiguration, mode: AgoLaterMode): DateTimeResolutionResult {
    let result = new DateTimeResolutionResult();
    let duration = durationExtractor.extract(source).pop();
    if (!duration) return result;
    let pr = durationParser.parse(duration);
    if (!pr) return result;
    let match = RegExpUtility.getMatches(unitRegex, source).pop();
    if (!match) return result;
    let afterStr = source.substr(duration.start + duration.length);
    let beforeStr = source.substr(0, duration.start);
    let srcUnit = match.groups('unit').value;
    let durationResult: DateTimeResolutionResult = pr.value;
    let numStr = durationResult.timex.substr(0, durationResult.timex.length - 1)
        .replace('P', '')
        .replace('T', '');
    let num = Number.parseInt(numStr);
    if (!num) return result;
    return getAgoLaterResult(num, unitMap, srcUnit, afterStr, beforeStr, referenceDate, utilityConfiguration, mode);
}

function getAgoLaterResult(num: number, unitMap: ReadonlyMap<string, string>, srcUnit: string, afterStr: string, beforeStr: string, referenceDate: Date, utilityConfiguration: IDateTimeUtilityConfiguration, mode: AgoLaterMode) {
    let result = new DateTimeResolutionResult();
    let unitStr = unitMap.get(srcUnit);
    if (!unitStr) return result;
    let numStr = num.toString();
    let containsAgo = MatchingUtil.containsAgoLaterIndex(afterStr, utilityConfiguration.agoRegex);
    let containsLaterOrIn = MatchingUtil.containsAgoLaterIndex(afterStr, utilityConfiguration.laterRegex) || MatchingUtil.containsInIndex(beforeStr, utilityConfiguration.inConnectorRegex);
    if (containsAgo) {
        return getDateResult(unitStr, num, referenceDate, false, mode);
    }
    if (containsLaterOrIn) {
        return getDateResult(unitStr, num, referenceDate, true, mode);
    }
    return result;
}

function getDateResult(unitStr: string, num: number, referenceDate: Date, isFuture: boolean, mode: AgoLaterMode): DateTimeResolutionResult {
    let value = new Date(referenceDate);
    let result = new DateTimeResolutionResult();
    let swift = isFuture ? 1 : -1;
    switch (unitStr) {
        case 'D': value.setDate(referenceDate.getDate() + (num * swift)); break;
        case 'W': value.setDate(referenceDate.getDate() + (num * swift * 7)); break;
        case 'MON': value.setMonth(referenceDate.getMonth() + (num * swift)); break;
        case 'Y': value.setFullYear(referenceDate.getFullYear() + (num * swift)); break;
        case 'H': value.setHours(referenceDate.getHours() + (num * swift)); break;
        case 'M': value.setMinutes(referenceDate.getMinutes() + (num * swift)); break;
        case 'S': value.setSeconds(referenceDate.getSeconds() + (num * swift)); break;
        default: return result;
    }
    result.timex = mode === AgoLaterMode.Date ? FormatUtil.luisDateFromDate(value) : FormatUtil.luisTimeFromDate(value);
    result.futureValue = value;
    result.pastValue = value;
    result.success = true;
    return result;
}

export interface ISetParserConfiguration {
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
    getMatchedDailyTimex(text: string): { matched: boolean, timex: string };
    getMatchedUnitTimex(text: string): { matched: boolean, timex: string };
}

export class BaseSetParser implements IDateTimeParser {
    public static readonly ParserName = Constants.SYS_DATETIME_SET;
    private readonly config: ISetParserConfiguration;

    constructor(configuration: ISetParserConfiguration) {
        this.config = configuration;
    }

    parse(er: ExtractResult, referenceDate?: Date): DateTimeParseResult | null {
        if (!referenceDate) referenceDate = new Date();
        let value = null;
        if (er.type == BaseSetParser.ParserName) {
            let innerResult = this.parseEachUnit(er.text);
            if (!innerResult.success) {
                innerResult = this.parseEachDuration(er.text);
            }

            if (!innerResult.success) {
                innerResult = this.parserTimeEveryday(er.text);
            }

            // NOTE: Please do not change the order of following function
            // datetimeperiod>dateperiod>timeperiod>datetime>date>time
            if (!innerResult.success) {
                innerResult = this.parseEach(this.config.dateTimePeriodExtractor, this.config.dateTimePeriodParser, er.text);
            }

            if (!innerResult.success) {
                innerResult = this.parseEach(this.config.datePeriodExtractor, this.config.datePeriodParser, er.text);
            }

            if (!innerResult.success) {
                innerResult = this.parseEach(this.config.timePeriodExtractor, this.config.timePeriodParser, er.text);
            }

            if (!innerResult.success) {
                innerResult = this.parseEach(this.config.dateTimeExtractor, this.config.dateTimeParser, er.text);
            }

            if (!innerResult.success) {
                innerResult = this.parseEach(this.config.dateExtractor, this.config.dateParser, er.text);
            }

            if (!innerResult.success) {
                innerResult = this.parseEach(this.config.timeExtractor, this.config.timeParser, er.text);
            }

            if (innerResult.success) {
                innerResult.futureResolution = new Map<string, string>(
                    [
                        [TimeTypeConstants.SET, innerResult.futureValue]
                    ]);

                innerResult.pastResolution = new Map<string, string>(
                    [
                        [TimeTypeConstants.SET, innerResult.pastValue]
                    ]);

                value = innerResult;
            }
        }

        let ret = new DateTimeParseResult(er);
        ret.value = value,
            ret.timexStr = value == null ? "" : value.timex,
            ret.resolutionStr = ""

        return ret;
    }

    private parseEachDuration(text: string): DateTimeResolutionResult {
        let ret = new DateTimeResolutionResult();
        let ers = this.config.durationExtractor.extract(text);
        if (ers.length != 1 || text.substring(ers[0].start + ers[0].length || 0)) {
            return ret;
        }

        let beforeStr = text.substring(0, ers[0].start || 0);
        let matches = RegExpUtility.getMatches(this.config.eachPrefixRegex, beforeStr);
        if (matches.length) {
            let pr = this.config.durationParser.parse(ers[0], new Date());
            ret.timex = pr.timexStr;
            ret.futureValue = ret.pastValue = "Set: " + pr.timexStr;
            ret.success = true;
            return ret;
        }

        return ret;
    }

    private parseEachUnit(text: string): DateTimeResolutionResult {
        let ret = new DateTimeResolutionResult();
        // handle "daily", "weekly"
        let matches = RegExpUtility.getMatches(this.config.periodicRegex, text);
        if (matches.length) {
            let getMatchedDailyTimex = this.config.getMatchedDailyTimex(text);
            if (!getMatchedDailyTimex.matched) {
                return ret;
            }

            ret.timex = getMatchedDailyTimex.timex;
            ret.futureValue = ret.pastValue = "Set: " + ret.timex;
            ret.success = true;

            return ret;
        }

        // handle "each month"
        matches = RegExpUtility.getMatches(this.config.eachUnitRegex, text);
        if (matches.length && matches[0].length == text.length) {
            let sourceUnit = matches[0].groups("unit").value;
            if (sourceUnit && this.config.unitMap.has(sourceUnit)) {
                let getMatchedUnitTimex = this.config.getMatchedUnitTimex(sourceUnit);
                if (!getMatchedUnitTimex.matched) {
                    return ret;
                }

                ret.timex = getMatchedUnitTimex.timex;
                ret.futureValue = ret.pastValue = "Set: " + ret.timex;
                ret.success = true;
                return ret;
            }
        }

        return ret;
    }

    private parserTimeEveryday(text: string): DateTimeResolutionResult {
        let ret = new DateTimeResolutionResult();
        let ers = this.config.timeExtractor.extract(text);
        if (ers.length != 1) {
            return ret;
        }

        let afterStr = text.replace(ers[0].text, "");
        let matches = RegExpUtility.getMatches(this.config.eachDayRegex, afterStr);
        if (matches.length) {
            let pr = this.config.timeParser.parse(ers[0], new Date());
            ret.timex = pr.timexStr;
            ret.futureValue = ret.pastValue = "Set: " + ret.timex;
            ret.success = true;
            return ret;
        }

        return ret;
    }

    private parseEach(extractor: IExtractor, parser: IDateTimeParser, text: string): DateTimeResolutionResult {
        let ret = new DateTimeResolutionResult();
        let ers = extractor.extract(text);
        if (ers.length != 1) {
            return ret;
        }

        let beforeStr = text.substring(0, ers[0].start || 0);
        let matches = RegExpUtility.getMatches(this.config.eachPrefixRegex, beforeStr);
        if (matches.length) {
            let pr = parser.parse(ers[0], new Date());
            ret.timex = pr.timexStr;
            ret.futureValue = ret.pastValue = "Set: " + ret.timex;
            ret.success = true;
            return ret;
        }

        return ret;
    }
}

export interface IDateTimeParserConfiguration {
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
    haveAmbiguousToken(text: string, matchedText: string): boolean;
    getMatchedNowTimex(text: string): { matched: boolean, timex: string };
    getSwiftDay(text: string): number;
    getHour(text: string, hour: number): number;
    utilityConfiguration: IDateTimeUtilityConfiguration;
}

export class BaseDateTimeParser implements IDateTimeParser {
    public static readonly ParserName = Constants.SYS_DATETIME_DATETIME; // "DateTime";

    private readonly config: IDateTimeParserConfiguration;

    constructor(configuration: IDateTimeParserConfiguration) {
        this.config = configuration;
    }

    public parse(er: ExtractResult, refTime: Date): DateTimeParseResult {
        if (!refTime) refTime = new Date();
        let referenceTime = refTime;

        let value = null;
        if (er.type == BaseDateTimeParser.ParserName) {
            let innerResult = this.mergeDateAndTime(er.text, referenceTime);
            if (!innerResult.success) {
                innerResult = this.parseBasicRegex(er.text, referenceTime);
            }

            if (!innerResult.success) {
                innerResult = this.parseTimeOfToday(er.text, referenceTime);
            }

            if (!innerResult.success) {
                innerResult = this.parseSpecailTimeOfDate(er.text, referenceTime);
            }

            if (!innerResult.success) {
                innerResult = this.parserDurationWithAgoAndLater(er.text, referenceTime);
            }

            if (innerResult.success) {
                innerResult.futureResolution = new Map<string, string>(
                    [
                        [TimeTypeConstants.DATETIME, FormatUtil.FormatDateTime(innerResult.futureValue)]
                    ]);

                innerResult.pastResolution = new Map<string, string>(
                    [
                        [TimeTypeConstants.DATETIME, FormatUtil.FormatDateTime(innerResult.pastValue)]
                    ]);
                value = innerResult;
            }
        }

        let ret = new DateTimeParseResult(er);
        {
            ret.value = value,
                ret.timexStr = value == null ? "" : value.timex,
                ret.resolutionStr = ""
        };
        return ret;
    }

    private parseBasicRegex(text: string, referenceTime: Date): DateTimeResolutionResult {
        let ret = new DateTimeResolutionResult();
        let trimedText = text.trim().toLowerCase();

        // handle "now"
        let matches = RegExpUtility.getMatches(this.config.nowRegex, trimedText);
        if (matches.length && matches[0].index == 0 && matches[0].length == trimedText.length) {
            let getMatchedNowTimex = this.config.getMatchedNowTimex(trimedText);
            ret.timex = getMatchedNowTimex.timex;
            ret.futureValue = ret.pastValue = referenceTime;
            ret.success = true;
            return ret;
        }

        return ret;
    }

    // merge a Date entity and a Time entity
    private mergeDateAndTime(text: string, referenceTime: Date): DateTimeResolutionResult {
        let ret = new DateTimeResolutionResult();

        let er1 = this.config.dateExtractor.extract(text);
        if (er1.length == 0) {
            er1 = this.config.dateExtractor.extract(this.config.tokenBeforeDate + text);
            if (er1.length == 1) {
                er1[0].start -= this.config.tokenBeforeDate.length;
            }
            else {
                return ret;
            }
        }
        else {
            // this is to understand if there is an ambiguous token in the text. For some languages (e.g. spanish)
            // the same word could mean different things (e.g a time in the day or an specific day).
            if (this.config.haveAmbiguousToken(text, er1[0].text)) {
                return ret;
            }
        }

        let er2 = this.config.timeExtractor.extract(text);
        if (er2.length == 0) {
            // here we filter out "morning, afternoon, night..." time entities
            er2 = this.config.timeExtractor.extract(this.config.tokenBeforeTime + text);
            if (er2.length == 1) {
                er2[0].start -= this.config.tokenBeforeTime.length;
            }
            else {
                return ret;
            }
        }

        // handle case "Oct. 5 in the afternoon at 7:00"
        // in this case "5 in the afternoon" will be extract as a Time entity
        let correctTimeIdx = 0;
        while (correctTimeIdx < er2.length && ExtractResult.isOverlap(er2[correctTimeIdx], er1[0])) {
            correctTimeIdx++;
        }

        if (correctTimeIdx >= er2.length) {
            return ret;
        }

        let pr1 = this.config.dateParser.parse(er1[0], new Date(referenceTime.toDateString()))
        let pr2 = this.config.timeParser.parse(er2[correctTimeIdx], referenceTime);
        if (pr1.value == null || pr2.value == null) {
            return ret;
        }

        let futureDate = pr1.value.futureValue;
        let pastDate = pr1.value.pastValue;
        let time = pr2.value.futureValue;

        let hour = time.getHours();
        let min = time.getMinutes();
        let sec = time.getSeconds();

        // handle morning, afternoon
        if (RegExpUtility.getMatches(this.config.pMTimeRegex, text).length && hour < 12) {
            hour += 12;
        }
        else if (RegExpUtility.getMatches(this.config.aMTimeRegex, text).length && hour >= 12) {
            hour -= 12;
        }

        let timeStr = pr2.timexStr;
        if (timeStr.endsWith("ampm")) {
            timeStr = timeStr.substring(0, timeStr.length - 4);
        }
        timeStr = "T" + FormatUtil.toString(hour, 2) + timeStr.substring(3);
        ret.timex = pr1.timexStr + timeStr;

        let val = pr2.value;
        if (hour <= 12 && !RegExpUtility.getMatches(this.config.pMTimeRegex, text).length
            && !RegExpUtility.getMatches(this.config.aMTimeRegex, text).length &&
            val.comment) {
            //ret.Timex += "ampm";
            ret.comment = "ampm";
        }
        ret.futureValue = new Date(futureDate.getFullYear(), futureDate.getMonth(), futureDate.getDate(), hour, min, sec);
        ret.pastValue = new Date(pastDate.getFullYear(), pastDate.getMonth(), pastDate.getDate(), hour, min, sec);
        ret.success = true;

        return ret;
    }

    private parseTimeOfToday(text: string, referenceTime: Date): DateTimeResolutionResult {
        let ret = new DateTimeResolutionResult();
        let trimedText = text.toLowerCase().trim();

        let hour = 0, min = 0, sec = 0;
        let timeStr: string;

        let wholeMatches = RegExpUtility.getMatches(this.config.simpleTimeOfTodayAfterRegex, trimedText);
        if (!(wholeMatches.length && wholeMatches[0].length == trimedText.length))
            wholeMatches = RegExpUtility.getMatches(this.config.simpleTimeOfTodayBeforeRegex, trimedText);
        if (wholeMatches.length && wholeMatches[0].length == trimedText.length) {
            let hourStr = wholeMatches[0].groups("hour").value;
            if (!hourStr) {
                hourStr = wholeMatches[0].groups("hournum").value.toLowerCase();
                hour = this.config.numbers.get(hourStr);
            }
            else {
                hour = parseInt(hourStr);
            }
            timeStr = "T" + FormatUtil.toString(hour, 2);
        }
        else {
            let ers = this.config.timeExtractor.extract(trimedText);
            if (ers.length != 1) {
                ers = this.config.timeExtractor.extract(this.config.tokenBeforeTime + trimedText);
                if (ers.length == 1) {
                    ers[0].start -= this.config.tokenBeforeTime.length;
                }
                else {
                    return ret;
                }
            }

            let pr = this.config.timeParser.parse(ers[0], referenceTime);
            if (pr.value == null) {
                return ret;
            }

            let time = pr.value.futureValue;

            hour = time.getHours();
            min = time.getMinutes();
            sec = time.getSeconds();
            timeStr = pr.timexStr;
        }


        let matches = RegExpUtility.getMatches(this.config.specificTimeOfDayRegex, trimedText);

        if (matches.length) {
            let matchStr = matches[0].value.toLowerCase();

            // handle "last", "next"
            let swift = this.config.getSwiftDay(matchStr);

            let date = new Date(referenceTime);
            date.setDate(date.getDate() + swift);

            // handle "morning", "afternoon"
            hour = this.config.getHour(matchStr, hour);

            // in this situation, luisStr cannot end up with "ampm", because we always have a "morning" or "night"
            if (timeStr.endsWith("ampm")) {
                timeStr = timeStr.substring(0, timeStr.length - 4);
            }
            timeStr = "T" + FormatUtil.toString(hour, 2) + timeStr.substring(3);

            ret.timex = FormatUtil.formatDate(date) + timeStr;
            ret.futureValue = ret.pastValue = new Date(date.getFullYear(), date.getMonth(), date.getDate(), hour, min, sec);
            ret.success = true;
            return ret;
        }

        return ret;
    }

    private parseSpecailTimeOfDate(text: string, refeDateTime: Date): DateTimeResolutionResult {
        let ret = new DateTimeResolutionResult();
        let ers = this.config.dateExtractor.extract(text);
        if (ers.length != 1) {
            return ret;
        }
        let beforeStr = text.substring(0, ers[0].start || 0);
        if (RegExpUtility.getMatches(this.config.theEndOfRegex, beforeStr).length) {
            let pr = this.config.dateParser.parse(ers[0], refeDateTime);
            let futureDate = new Date(pr.value.futureValue);
            let pastDate = new Date(pr.value.pastValue);
            ret.timex = pr.timexStr + "T23:59";
            futureDate.setDate(futureDate.getDate() + 1);
            futureDate.setMinutes(futureDate.getMinutes() - 1);
            ret.futureValue = futureDate;
            pastDate.setDate(pastDate.getDate() + 1);
            pastDate.setMinutes(pastDate.getMinutes() - 1);
            ret.pastValue = pastDate;
            ret.success = true;
            return ret;
        }

        return ret;
    }

    // handle like "two hours ago" 
    private parserDurationWithAgoAndLater(text: string, referenceTime: Date): DateTimeResolutionResult {
        return AgoLaterUtil.parseDurationWithAgoAndLater(
            text,
            referenceTime,
            this.config.durationExtractor,
            this.config.durationParser,
            this.config.unitMap,
            this.config.unitRegex,
            this.config.utilityConfiguration,
            AgoLaterMode.DateTime
            );
    }
}