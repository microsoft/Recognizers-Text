import { IExtractor, IParser, ExtractResult, QueryProcessor, MetaData, Match, StringUtility } from "@microsoft/recognizers-text";
import { RegExpUtility } from "@microsoft/recognizers-text";
import { IDateTimeParser, DateTimeParseResult } from "../dateTime/parsers";
import { Constants, TimeTypeConstants } from "../dateTime/constants";
import { IDateTimeExtractor } from "./baseDateTime";
import { BaseNumberParser } from "@microsoft/recognizers-text-number";

export class Token {
    constructor(start: number, end: number, metaData: MetaData = null) {
        this.start = start;
        this.end = end;
        this.metaData = metaData;
    }

    start: number;
    end: number;
    metaData: MetaData;

    get length(): number {
        return this.end - this.start;
    }

    static mergeAllTokens(tokens: Token[], source: string, extractorName: string): ExtractResult[] {
        let ret: ExtractResult[] = [];
        let mergedTokens: Token[] = [];
        tokens = tokens.sort((a, b) => {
            return a.start < b.start ? -1 : 1;
        });
        tokens.forEach(token => {
            if (token) {
                let bAdd = true;
                for (let index = 0; index < mergedTokens.length && bAdd; index++) {
                    let mergedToken = mergedTokens[index];
                    if (token.start >= mergedToken.start && token.end <= mergedToken.end) {
                        bAdd = false;
                    }
                    if (token.start > mergedToken.start && token.start < mergedToken.end) {
                        bAdd = false;
                    }
                    if (token.start <= mergedToken.start && token.end >= mergedToken.end) {
                        bAdd = false;
                        mergedTokens[index] = token;
                    }
                }
                if (bAdd) {
                    mergedTokens.push(token);
                }
            }
        });
        mergedTokens.forEach(token => {
            ret.push({
                start: token.start,
                length: token.length,
                text: source.substr(token.start, token.length),
                type: extractorName,
                metaData: token.metaData
            });
        });
        return ret;
    }
}

export interface IDateTimeUtilityConfiguration {
    agoRegex: RegExp
    laterRegex: RegExp
    inConnectorRegex: RegExp
    rangeUnitRegex: RegExp
    amDescRegex: RegExp
    pmDescRegex: RegExp
    amPmDescRegex: RegExp
}

export enum AgoLaterMode {
    Date, DateTime
}

export class AgoLaterUtil {
    static extractorDurationWithBeforeAndAfter(source: string, er: ExtractResult, ret: Token[], config: IDateTimeUtilityConfiguration): Token[] {
        let pos = er.start + er.length;
        if (pos <= source.length) {
            let afterString = source.substring(pos);
            let beforeString = source.substring(0, er.start);
            let index = -1;
            let value = MatchingUtil.getAgoLaterIndex(afterString, config.agoRegex);
            if (value.matched) {
                ret.push(new Token(er.start, er.start + er.length + value.index));
            }
            else {
                value = MatchingUtil.getAgoLaterIndex(afterString, config.laterRegex);
                if (value.matched) {
                    ret.push(new Token(er.start, er.start + er.length + value.index));
                }
                else {
                    value = MatchingUtil.getInIndex(beforeString, config.inConnectorRegex);
                    // for range unit like "week, month, year", it should output dateRange or datetimeRange
                    if (RegExpUtility.getMatches(config.rangeUnitRegex, er.text).length > 0) {
                        return ret;
                    }
                    if (value.matched && er.start && er.length && er.start >= value.index) {
                        ret.push(new Token(er.start - value.index, er.start + er.length));
                    }
                }
            }
        }
        return ret;
    }

    static parseDurationWithAgoAndLater(source: string, referenceDate: Date, durationExtractor: IDateTimeExtractor, durationParser: IDateTimeParser, unitMap: ReadonlyMap<string, string>, unitRegex: RegExp, utilityConfiguration: IDateTimeUtilityConfiguration, mode: AgoLaterMode): DateTimeResolutionResult {
        let result = new DateTimeResolutionResult();
        let duration = durationExtractor.extract(source, referenceDate).pop();
        if (!duration) {
            return result;
        }
        let pr = durationParser.parse(duration, referenceDate);
        if (!pr) {
            return result;
        }
        let match = RegExpUtility.getMatches(unitRegex, source).pop();
        if (!match) {
            return result;
        }
        let afterStr = source.substr(duration.start + duration.length);
        let beforeStr = source.substr(0, duration.start);
        let srcUnit = match.groups('unit').value;
        let durationResult: DateTimeResolutionResult = pr.value;
        let numStr = durationResult.timex.substr(0, durationResult.timex.length - 1)
            .replace('P', '')
            .replace('T', '');
        let num = Number.parseInt(numStr, 10);
        if (!num) {
            return result;
        }
        return AgoLaterUtil.getAgoLaterResult(pr, num, unitMap, srcUnit, afterStr, beforeStr, referenceDate, utilityConfiguration, mode);
    }

    static getAgoLaterResult(durationParseResult: DateTimeParseResult, num: number, unitMap: ReadonlyMap<string, string>, srcUnit: string, afterStr: string, beforeStr: string, referenceDate: Date, utilityConfiguration: IDateTimeUtilityConfiguration, mode: AgoLaterMode) {
        let result = new DateTimeResolutionResult();
        let unitStr = unitMap.get(srcUnit);
        if (!unitStr) {
            return result;
        }
        let numStr = num.toString();
        let containsAgo = MatchingUtil.containsAgoLaterIndex(afterStr, utilityConfiguration.agoRegex);
        let containsLaterOrIn = MatchingUtil.containsAgoLaterIndex(afterStr, utilityConfiguration.laterRegex) || MatchingUtil.containsInIndex(beforeStr, utilityConfiguration.inConnectorRegex);
        if (containsAgo) {
            result = AgoLaterUtil.getDateResult(unitStr, num, referenceDate, false, mode);
            durationParseResult.value.mod = TimeTypeConstants.beforeMod;
            result.subDateTimeEntities = [durationParseResult];
            return result;
        }
        if (containsLaterOrIn) {
            result = AgoLaterUtil.getDateResult(unitStr, num, referenceDate, true, mode);
            durationParseResult.value.mod = TimeTypeConstants.afterMod;
            result.subDateTimeEntities = [durationParseResult];
            return result;
        }
        return result;
    }

    static getDateResult(unitStr: string, num: number, referenceDate: Date, isFuture: boolean, mode: AgoLaterMode): DateTimeResolutionResult {
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
        result.timex = mode === AgoLaterMode.Date ? DateTimeFormatUtil.luisDateFromDate(value) : DateTimeFormatUtil.luisDateTime(value);
        result.futureValue = value;
        result.pastValue = value;
        result.success = true;
        return result;
    }
}

export interface MatchedIndex {
    matched: boolean,
    index: number
}

export class MatchingUtil {
    public static getAgoLaterIndex(source: string, regex: RegExp): MatchedIndex {
        let result: MatchedIndex = { matched: false, index: -1 };
        let referencedMatches = RegExpUtility.getMatches(regex, source.trim().toLowerCase());
        if (referencedMatches && referencedMatches.length > 0 && referencedMatches[0].index === 0) {
            result.index = source.toLowerCase().indexOf(referencedMatches[0].value) + referencedMatches[0].length;
            result.matched = true;
        }
        return result;
    }

    public static getInIndex(source: string, regex: RegExp): MatchedIndex {
        let result: MatchedIndex = { matched: false, index: -1 };
        let referencedMatch = RegExpUtility.getMatches(regex, source.trim().toLowerCase().split(' ').pop());
        if (referencedMatch && referencedMatch.length > 0) {
            result.index = source.length - source.toLowerCase().lastIndexOf(referencedMatch[0].value);
            result.matched = true;
        }
        return result;
    }

    public static containsAgoLaterIndex(source: string, regex: RegExp): boolean {
        return this.getAgoLaterIndex(source, regex).matched;
    }

    public static containsInIndex(source: string, regex: RegExp): boolean {
        return this.getInIndex(source, regex).matched;
    }
}

export class DateTimeFormatUtil {
    public static readonly HourTimexRegex = RegExpUtility.getSafeRegExp(String.raw`(?<!P)T\d{2}`, "gis");

    // Emulates .NET ToString("D{size}")
    public static toString(num: number, size: number): string {
        let s = "000000" + (num || "");
        return s.substr(s.length - size);
    }

    public static luisDate(year: number, month: number, day: number): string {
        if (year === -1) {
            if (month === -1) {
                return ["XXXX", "XX", DateTimeFormatUtil.toString(day, 2)].join("-");
            }

            return ["XXXX", DateTimeFormatUtil.toString(month + 1, 2), DateTimeFormatUtil.toString(day, 2)].join("-");
        }

        return [DateTimeFormatUtil.toString(year, 4), DateTimeFormatUtil.toString(month + 1, 2), DateTimeFormatUtil.toString(day, 2)].join("-");
    }

    public static luisDateFromDate(date: Date): string {
        return DateTimeFormatUtil.luisDate(date.getFullYear(), date.getMonth(), date.getDate());
    }

    public static luisTime(hour: number, min: number, second: number): string {
        return [DateTimeFormatUtil.toString(hour, 2), DateTimeFormatUtil.toString(min, 2), DateTimeFormatUtil.toString(second, 2)].join(":");
    }

    public static luisTimeFromDate(time: Date): string {
        return DateTimeFormatUtil.luisTime(time.getHours(), time.getMinutes(), time.getSeconds());
    }

    public static luisDateTime(time: Date): string {
        return `${DateTimeFormatUtil.luisDateFromDate(time)}T${DateTimeFormatUtil.luisTimeFromDate(time)}`;
    }

    public static formatDate(date: Date): string {
        return [DateTimeFormatUtil.toString(date.getFullYear(), 4),
        DateTimeFormatUtil.toString(date.getMonth() + 1, 2),
        DateTimeFormatUtil.toString(date.getDate(), 2)].join("-");
    }

    public static formatTime(time: Date) {
        return [DateTimeFormatUtil.toString(time.getHours(), 2),
        DateTimeFormatUtil.toString(time.getMinutes(), 2),
        DateTimeFormatUtil.toString(time.getSeconds(), 2)].join(":");
    }

    public static formatDateTime(datetime: Date): string {
        return `${DateTimeFormatUtil.formatDate(datetime)} ${DateTimeFormatUtil.formatTime(datetime)}`;
    }

    public static shortTime(hour: number, minute: number, second: number): string {
        if (minute < 0 && second < 0) {
            return `T${DateTimeFormatUtil.toString(hour, 2)}`;
        }
        else if (second < 0) {
            return `T${DateTimeFormatUtil.toString(hour, 2)}:${DateTimeFormatUtil.toString(minute, 2)}`;
        }
        return `T${DateTimeFormatUtil.toString(hour, 2)}:${DateTimeFormatUtil.toString(minute, 2)}:${DateTimeFormatUtil.toString(second, 2)}`;
    }

    public static luisTimeSpan(from: Date, to: Date): string {
        let result = 'PT';
        let span = DateUtils.totalHoursFloor(from, to);
        if (span > 0) {
            result = `${result}${span}H`;
        }

        span = DateUtils.totalMinutesFloor(from, to) - (span * 60);
        if (span > 0 && span < 60) {
            result = `${result}${span}M`;
        }

        span = DateUtils.totalSeconds(from, to) - (span * 60);
        if (span > 0 && span < 60) {
            result = `${result}${span}S`;
        }

        return result;
    }

    public static allStringToPm(timeStr: string): string {
        let matches = RegExpUtility.getMatches(DateTimeFormatUtil.HourTimexRegex, timeStr);
        let split = Array<string>();
        let lastPos = 0;
        matches.forEach(match => {
            if (lastPos !== match.index) {
                split.push(timeStr.substring(lastPos, match.index));
            }
            split.push(timeStr.substring(match.index, match.index + match.length));
            lastPos = match.index + match.length;
        });

        if (timeStr.substring(lastPos)) {
            split.push(timeStr.substring(lastPos));
        }

        for (let i = 0; i < split.length; i += 1) {
            if (RegExpUtility.getMatches(DateTimeFormatUtil.HourTimexRegex, split[i]).length > 0) {
                split[i] = DateTimeFormatUtil.toPm(split[i]);
            }
        }

        return split.join('');
    }

    public static toPm(timeStr: string): string {
        let hasT = false;
        if (timeStr.startsWith("T")) {
            hasT = true;
            timeStr = timeStr.substring(1);
        }

        let split = timeStr.split(':');
        let hour = parseInt(split[0], 10);
        hour = (hour === 12) ? 0 : hour + 12;
        split[0] = DateTimeFormatUtil.toString(hour, 2);

        return (hasT ? "T" : "") + split.join(":");
    }
}

export class StringMap {
    [key: string]: string;
}

export class DateTimeResolutionResult {
    success: boolean;
    timex: string;
    isLunar: boolean;
    mod: string;
    comment: string;
    futureResolution: StringMap;
    pastResolution: StringMap;
    futureValue: any;
    pastValue: any;
    subDateTimeEntities: any[];

    constructor() {
        this.success = false;
    }
}

export class TimeOfDayResolutionResult {
    timeX: string;
    beginHour: number;
    endHour: number;
    endMin: number;

    constructor() {
        this.timeX = null;
        this.beginHour = this.endHour = this.endMin = 0;
    }
}

export enum DayOfWeek {
    Sunday = 0,
    Monday = 1,
    Tuesday = 2,
    Wednesday = 3,
    Thursday = 4,
    Friday = 5,
    Saturday = 6
}

export class DateUtils {
    private static readonly oneDay = 24 * 60 * 60 * 1000;
    private static readonly oneHour = 60 * 60 * 1000;
    private static readonly oneMinute = 60 * 1000;
    private static readonly oneSecond = 1000;

    // Generate future/past date for cases without specific year like "Feb 29th"
    static generateDates(noYear: boolean, referenceDate: Date, year: number, month: number, day: number): { future: Date, past: Date} {
        let futureDate = this.safeCreateFromMinValue(year, month, day);
        let pastDate = this.safeCreateFromMinValue(year, month, day);
        let futureYear = year;
        let pastYear = year;
        if (noYear){
            if (this.isFeb29th(year, month, day)) {
                if (this.isLeapYear(year)) {
                    if (futureDate < referenceDate) {
                        futureDate = this.safeCreateFromMinValue(futureYear + 4, month, day);
                    }
                    else {
                        pastDate = this.safeCreateFromMinValue(pastYear - 4, month, day);
                    }
                }
                else {
                    pastYear = pastYear >> 2 << 2;
                    if (!this.isLeapYear(pastYear)) {
                        pastYear -= 4;
                    }

                    futureYear = pastYear + 4;
                    if (!this.isLeapYear(futureYear)) {
                        futureYear += 4;
                    }

                    futureDate = this.safeCreateFromMinValue(futureYear, month, day);
                    pastDate = this.safeCreateFromMinValue(pastYear, month, day);
                }
            }
            else {
                if (futureDate < referenceDate && this.isValidDate(year, month, day)) {
                    futureDate = this.safeCreateFromMinValue(year + 1, month, day);
                }
                if (pastDate >= referenceDate && this.isValidDate(year, month, day)) {
                    pastDate = this.safeCreateFromMinValue(year - 1, month, day);
                }
            }
        }

        return { future: futureDate, past: pastDate };
    }

    static parseChineseDynastyYear(yearStr: string, dynastyYearRegex: RegExp, dynastyYearMap: ReadonlyMap<string, number>, dynastyStartYear: string, integerExtractor: IExtractor, numberParser: IParser): number {
        let year = -1;
        let regionTitleMatch = RegExpUtility.getMatches(dynastyYearRegex, yearStr).pop();
        if (regionTitleMatch && regionTitleMatch.index === 0 && regionTitleMatch.length === yearStr.length) {
            // handle "康熙元年" refer to https://zh.wikipedia.org/wiki/%E5%B9%B4%E5%8F%B7
            let dynastyYearStr = regionTitleMatch.groups('dynasty').value;
            let biasYearStr = regionTitleMatch.groups('biasYear').value;
            let basicYear = dynastyYearMap.get(dynastyYearStr);
            let biasYear = 1;
            if (biasYearStr != dynastyStartYear) {
                let er = integerExtractor.extract(biasYearStr).pop();
                biasYear = Number.parseInt(numberParser.parse(er).value);
            }
            year = basicYear + biasYear - 1;
        }

        return year;
    }

    static next(from: Date, dayOfWeek: DayOfWeek): Date {
        let start = from.getDay();
        let target = dayOfWeek;
        
        if (start === 0) {
            start = 7;
        }
        
        if (target === 0) {
            target = 7;
        }
        
        let result = new Date(from);
        result.setDate(from.getDate() + target - start + 7);
        return result;
    }

    static this(from: Date, dayOfWeek: DayOfWeek): Date {
        let start = from.getDay();
        let target = dayOfWeek;
        
        if (start === 0) {
            start = 7;
        }
        
        if (target === 0) {
            target = 7;
        }
        
        let result = new Date(from);
        result.setDate(from.getDate() + target - start);
        return result;
    }

    static last(from: Date, dayOfWeek: DayOfWeek): Date {
        let start = from.getDay();
        let target = dayOfWeek;
        
        if (start === 0) {
            start = 7;
        }
        
        if (target === 0) {
            target = 7;
        }
        
        let result = new Date(from);
        result.setDate(from.getDate() + target - start - 7);
        return result;
    }

    static diffDays(from: Date, to: Date): number {
        return Math.round(Math.abs((from.getTime() - to.getTime()) / this.oneDay));
    }

    static totalHours(from: Date, to: Date): number {
        // Fix to mimic .NET's Convert.ToInt32()
        // C#: Math.Round(4.5) === 4
        // C#: Convert.ToInt32(4.5) === 4
        // JS: Math.round(4.5) === 5 !!
        let fromEpoch = from.getTime() - (from.getTimezoneOffset() * 60 * 1000);
        let toEpoch = to.getTime() - (to.getTimezoneOffset() * 60 * 1000);
        return Math.round(Math.abs(fromEpoch - toEpoch - 0.00001) / this.oneHour);
    }

    static totalHoursFloor(from: Date, to: Date): number {
        let fromEpoch = from.getTime() - (from.getTimezoneOffset() * this.oneMinute);
        let toEpoch = to.getTime() - (to.getTimezoneOffset() * this.oneMinute);
        return Math.floor(Math.abs(fromEpoch - toEpoch) / this.oneHour);
    }

    static totalMinutesFloor(from: Date, to: Date): number {
        let fromEpoch = from.getTime() - (from.getTimezoneOffset() * this.oneMinute);
        let toEpoch = to.getTime() - (to.getTimezoneOffset() * this.oneMinute);
        return Math.floor(Math.abs(fromEpoch - toEpoch) / this.oneMinute);
    }

    static totalSeconds(from: Date, to: Date): number {
        let fromEpoch = from.getTime() - (from.getTimezoneOffset() * 60 * 1000);
        let toEpoch = to.getTime() - (to.getTimezoneOffset() * 60 * 1000);
        return Math.round(Math.abs(fromEpoch - toEpoch) / this.oneSecond);
    }

    static addTime(seedDate: Date, timeToAdd: Date): Date {
        let date = new Date(seedDate);
        date.setHours(seedDate.getHours() + timeToAdd.getHours());
        date.setMinutes(seedDate.getMinutes() + timeToAdd.getMinutes());
        date.setSeconds(seedDate.getSeconds() + timeToAdd.getSeconds());
        return date;
    }

    static addSeconds(seedDate: Date, secondsToAdd: number): Date {
        let date = new Date(seedDate);
        date.setSeconds(seedDate.getSeconds() + secondsToAdd);
        return date;
    }

    static addMinutes(seedDate: Date, minutesToAdd: number): Date {
        let date = new Date(seedDate);
        date.setMinutes(seedDate.getMinutes() + minutesToAdd);
        return date;
    }

    static addHours(seedDate: Date, hoursToAdd: number): Date {
        let date = new Date(seedDate);
        date.setHours(seedDate.getHours() + hoursToAdd);
        return date;
    }

    static addDays(seedDate: Date, daysToAdd: number): Date {
        let date = new Date(seedDate);
        date.setDate(seedDate.getDate() + daysToAdd);
        return date;
    }

    static addMonths(seedDate: Date, monthsToAdd: number): Date {
        let date = new Date(seedDate);
        date.setMonth(seedDate.getMonth() + monthsToAdd);
        return date;
    }

    static addYears(seedDate: Date, yearsToAdd: number): Date {
        let date = new Date(seedDate);
        date.setFullYear(seedDate.getFullYear() + yearsToAdd);
        return date;
    }

    static getWeekNumber(referenceDate: Date): { weekNo: number, year: number } {
        // Create a copy of this date object
        let target = new Date(referenceDate.valueOf());

        // ISO week date weeks start on monday
        // so correct the day number
        let dayNr = (referenceDate.getDay() + 6) % 7;

        // ISO 8601 states that week 1 is the week
        // with the first thursday of that year.
        // Set the target date to the thursday in the target week
        target.setDate(target.getDate() - dayNr + 3);

        // Store the millisecond value of the target date
        let firstThursday = target.valueOf();
        let thursday = new Date(firstThursday);

        // Set the target to the first thursday of the year
        // First set the target to january first
        target.setMonth(0, 1);
        // Not a thursday? Correct the date to the next thursday
        if (target.getDay() !== 4) {
            target.setMonth(0, 1 + ((4 - target.getDay()) + 7) % 7);
        }

        // The weeknumber is the number of weeks between the 
        // first thursday of the year and the thursday in the target week
        let weekNo = 1 + Math.ceil((firstThursday - target.valueOf()) / 604800000); // 604800000 = 7 * 24 * 3600 * 1000
        return { weekNo: weekNo, year: thursday.getFullYear() };
    }

    static minValue(): Date {
        let date = new Date(1, 0, 1, 0, 0, 0, 0);
        date.setFullYear(1);
        return date;
    }

    static safeCreateFromValue(seedDate: Date, year: number, month: number, day: number, hour = 0, minute = 0, second = 0) {
        if (this.isValidDate(year, month, day) && this.isValidTime(hour, minute, second)) {
            return new Date(year, month, day, hour, minute, second, 0);
        }
        return seedDate;
    }

    static safeCreateFromMinValue(year: number, month: number, day: number, hour = 0, minute = 0, second = 0) {
        return this.safeCreateFromValue(this.minValue(), year, month, day, hour, minute, second);
    }

    // Resolve month overflow
    static safeCreateDateResolveOverflow(year: number, month: number, day: number): Date {
        if (month >= 12) {
            year += (month + 1) / 12;
            month %= 12;
        }
        return this.safeCreateFromMinValue(year, month, day);
    }

    static safeCreateFromMinValueWithDateAndTime(date: Date, time?: Date): Date {
        return this.safeCreateFromMinValue(
            date.getFullYear(), date.getMonth(), date.getDate(),
            time ? time.getHours() : 0, time ? time.getMinutes() : 0, time ? time.getSeconds() : 0
        );
    }

    static isLeapYear(year: number): boolean {
        return ((year % 4 === 0) && (year % 100 !== 0)) || (year % 400 === 0);
    }

    static dayOfYear(date: Date): number {
        let start = new Date(date.getFullYear(), 0, 1);
        let diffDays = date.valueOf() - start.valueOf();
        return Math.floor(diffDays / DateUtils.oneDay);
    }

    static isDafaultValue(date: Date): boolean {
        return date.getTime() === this.minValue().getTime();
    }

    static isFeb29th(year: number, month: number, day: number): boolean {
        return month === 1 && day === 29;
    }

    static isFeb29thDate(date: Date): boolean {
        return date.getMonth() === 1 && date.getDate() === 29;
    }

    static isValidDate(year: number, month: number, day: number): boolean {
        return year > 0 && year <= 9999
            && month >= 0 && month < 12
            && day > 0 && day <= this.validDays(year)[month];
    }

    static isValidDateType(date: Date): boolean {
        return DateUtils.isValidDate(date.getFullYear(), date.getMonth(), date.getDate());
    }

    static isEmpty(date: Date) {
        return date.getFullYear() == Constants.InvalidYear;
    }

    static setDateWithContext(originDate: Date, year: number) {
        if (!DateUtils.isDafaultValue(originDate)) {
            return DateUtils.safeCreateFromMinValue(year, originDate.getMonth(), originDate.getDate());
        }

        return originDate;
    }

    static getYear(config, startText: string, endText: string, text: string): number {
        let contextYear = -1;
        let isEndDatePureYear = false;
        let isDateRelative = false;

        let yearMatchForEndDate = RegExpUtility.getMatches(config.yearRegex, endText);

        if (yearMatchForEndDate && yearMatchForEndDate.length == 1 && yearMatchForEndDate[0].length == endText.length)
        {
            isEndDatePureYear = true;
        }

        let relativeMatchForStartDate = RegExpUtility.getMatches(config.relativeRegex, startText);
        let relativeMatchForEndDate = RegExpUtility.getMatches(config.relativeRegex, endText);
        isDateRelative = (relativeMatchForStartDate && relativeMatchForStartDate.length > 0) || (relativeMatchForEndDate && relativeMatchForEndDate.length > 0);

        if (!isEndDatePureYear && !isDateRelative) {
            let matchYear = RegExpUtility.getMatches(config.yearRegex, text);
            for (let match of matchYear) {
                let year = AbstractYearExtractor.getYearFromText(match, config.numberParser);
                if (year != -1) {
                    if (contextYear == -1) {
                        contextYear = year;
                    }
                    else {
                        if (contextYear != year) {
                            contextYear = -1;
                            break;
                        }
                    }
                }
            }
        }

        return contextYear;
    }

    static processDateEntityParsingResult(pr: DateTimeParseResult, year: number) {
        if (year != -1){
            pr.timexStr = TimexUtil.setTimexWithContext(pr.timexStr, year);
            pr.value = DateUtils.syncYearResolution(pr.value, year, year);
        }

        return pr;
    }

    // This method is to ensure the year of begin date is same with the end date in no year situation.
    static syncYear(pr1: DateTimeParseResult, pr2: DateTimeParseResult): { pr1: DateTimeParseResult, pr2: DateTimeParseResult } {
        let futureYear;
        let pastYear;
        if (DateUtils.isFeb29thDate(pr1.value.futureValue)) {
            futureYear = pr1.value.futureValue.getFullYear();
            pastYear = pr1.value.pastValue.getFullYear();
            pr2.value = DateUtils.syncYearResolution(pr2.value, futureYear, pastYear);
        }
        else if (DateUtils.isFeb29thDate(pr2.value.futureValue)) {
            futureYear = pr2.value.FftureValue.getFullYear();
            pastYear = pr2.value.pastValue.getFullYear();
            pr1.value = DateUtils.syncYearResolution(pr1.value, futureYear, pastYear);
        }

        return {pr1, pr2};
    }

    static syncYearResolution(resolutionResult: DateTimeResolutionResult , futureYear: number, pastYear: number): DateTimeResolutionResult {
        resolutionResult.futureValue = DateUtils.setDateWithContext(resolutionResult.futureValue, futureYear);
        resolutionResult.pastValue = DateUtils.setDateWithContext(resolutionResult.pastValue, pastYear);

        return resolutionResult;
    }

    private static validDays(year: number) {
        return [31, this.isLeapYear(year) ? 29 : 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];
    }

    private static isValidTime(hour: number, minute: number, second: number) {
        return hour >= 0 && hour < 24
            && minute >= 0 && minute < 60
            && second >= 0 && minute < 60;
    }
}

export class HolidayFunctions {

    static calculateHolidayByEaster(year: number, days = 0): Date {

        let day = 0;
        let month = 2; // In JavaScript month is 0 indexed

        let g = year % 19;
        let c = year / 100;
        let h = (c - ~~(c / 4) - ~~(((8 * c) + 13) / 25) + (19 * g) + 15) % 30;
        let i = h - (~~(h / 28) * (1 - (~~(h / 28) * (29 / (h + 1)) * ~~((21 - g) / 11))));

        day = i - ((year + ~~(year / 4) + i + 2 - c + ~~(c / 4)) % 7) + 28;

        if (day > 31) {
            month++;
            day -= 31;
        }

        return DateUtils.addDays(DateUtils.safeCreateFromMinValue(year, month, day), days);
    }

}

export class TimexUtil {
    public static parseTimeOfDay(tod: string): TimeOfDayResolutionResult {
        let result = new TimeOfDayResolutionResult();
        switch (tod) {
            case Constants.EarlyMorning:
                result.timeX = Constants.EarlyMorning;
                result.beginHour = 4;
                result.endHour = 8;
                break;
            case Constants.Morning:
                result.timeX = Constants.Morning;
                result.beginHour = 8;
                result.endHour = 12;
                break;
            case Constants.MidDay:
                result.timeX = Constants.MidDay;
                result.beginHour = 11;
                result.endHour = 13;
                break;
            case Constants.Afternoon:
                result.timeX = Constants.Afternoon;
                result.beginHour = 12;
                result.endHour = 16;
                break;
            case Constants.Evening:
                result.timeX = Constants.Evening;
                result.beginHour = 16;
                result.endHour = 20;
                break;
            case Constants.Daytime:
                result.timeX = Constants.Daytime;
                result.beginHour = 8;
                result.endHour = 18;
                break;
            case Constants.BusinessHour:
                result.timeX = Constants.BusinessHour;
                result.beginHour = 8;
                result.endHour = 18;
                break;
            case Constants.Night:
                result.timeX = Constants.Night;
                result.beginHour = 20;
                result.endHour = 23;
                result.endMin = 59;
                break;
            default:
                break;
        }

        return result;
    }

    static setTimexWithContext(timex: string, year: number) {
        return timex.replace(Constants.TimexFuzzyYear, DateTimeFormatUtil.toString(year, 4));
    }

    static getDatePeriodTimexUnitCount(begin: Date, end: Date, timeType: string): number {
        let unitCount;
        switch (timeType) {
            case Constants.ByDay:
                unitCount = DateUtils.diffDays(end, begin);
                break;
            case Constants.ByWeek:
                unitCount = DateUtils.diffDays(end, begin) / 7;
                break;
            case Constants.ByMonth:
                unitCount = (end.getFullYear() - begin.getFullYear())  * 12 + (end.getMonth() - begin.getMonth());
                break;
            default:
                unitCount = (end.getFullYear() - begin.getFullYear()) + (end.getMonth() - begin.getMonth()) / 12.0;
                break;
        }

        return unitCount;
    }

    public static generateDatePeriodTimex(begin: Date, end: Date, timexType: string, timex1: string, timex2: string) {
        let boundaryValid = !DateUtils.isDafaultValue(begin) && !DateUtils.isDafaultValue(end);
        var unitCount = boundaryValid ? TimexUtil.getDatePeriodTimexUnitCount(begin, end, timexType) : "X";
        var datePeriodTimex = `P${unitCount}${Constants.DatePeriodTimexTypeToTimexSuffix.get(timexType)}`;
        return `(${timex1},${timex2},${datePeriodTimex})`;
    }

    public static mergeTimexAlternatives(timex1: string, timex2: string): string {
        return timex1 === timex2 ? timex1 : `${timex1}${Constants.CompositeTimexDelimiter}${timex2}`;
    }
}

export class AbstractYearExtractor {

    public static getYearFromText(match: Match, numberParser: BaseNumberParser): number {

        let year = -1;

        let yearStr = match.groups('year').value;
        let writtenYearStr = match.groups('fullyear').value;
        if (!StringUtility.isNullOrEmpty(yearStr) && !(yearStr == writtenYearStr)) {
            year = Number.parseInt(yearStr, 10);
            if (year < 100 && year >= Constants.MinTwoDigitYearPastNum)
            {
                year += 1900;
            }
            else if (year >= 0 && year < Constants.MaxTwoDigitYearFutureNum)
            {
                year += 2000;
            }
        }
        else { 
            let firstTwoYearNumStr = match.groups('firsttwoyearnum').value;
            if (!StringUtility.isNullOrEmpty(firstTwoYearNumStr)) {
                let er = new ExtractResult();
                er.text = firstTwoYearNumStr;
                er.start = match.groups('firsttwoyearnum').index;
                er.length = match.groups('firsttwoyearnum').length;

                let firstTwoYearNum = Number.parseInt(numberParser.parse(er).value, 10);

                let lastTwoYearNum = 0;
                let lastTwoYearNumStr = match.groups('lasttwoyearnum').value;
                if (!StringUtility.isNullOrEmpty(lastTwoYearNumStr)) {
                    er.text = lastTwoYearNumStr;
                    er.start = match.groups('lasttwoyearnum').index;
                    er.length = match.groups('lasttwoyearnum').length;

                    lastTwoYearNum = Number.parseInt(numberParser.parse(er).value, 10);
                }

                if (firstTwoYearNum < 100 && lastTwoYearNum === 0 || firstTwoYearNum < 100 && firstTwoYearNum % 10 === 0 && lastTwoYearNumStr.trim().split(' ').length === 1) {
                    year = -1;
                }

                if (firstTwoYearNum >= 100) {
                    year = (firstTwoYearNum + lastTwoYearNum);
                }
                else {
                    year = (firstTwoYearNum * 100 + lastTwoYearNum);
                }
            }
            else {
                if (!StringUtility.isNullOrEmpty(writtenYearStr)) {
                    let er = new ExtractResult();
                    er.text = writtenYearStr;
                    er.start = match.groups('fullyear').index;
                    er.length = match.groups('fullyear').length;

                    let year = Number.parseInt(numberParser.parse(er).value, 10);

                    if (year < 100 && year >= Constants.MinTwoDigitYearPastNum)
                    {
                        year += 1900;
                    }
                    else if (year >= 0 && year < Constants.MaxTwoDigitYearFutureNum)
                    {
                        year += 2000;
                    }
                }
            }
        }

        return year;
    }
}