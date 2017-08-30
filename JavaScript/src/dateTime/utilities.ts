import { IExtractor, ExtractResult } from "../number/extractors"
import { IParser } from "../number/parsers"
import { AgoLaterMode } from "./parsers"
import { RegExpUtility } from "../utilities"

export class Token {
    constructor(start: number, end: number) {
        this.start = start;
        this.end = end;
    }

    start: number;
    end: number;

    get length(): number {
        return this.end - this.start;
    }

    static mergeAllTokens(tokens: Token[], source: string, extractorName: string): Array<ExtractResult> {
        let ret: Array<ExtractResult> = [];
        let mergedTokens: Array<Token> = [];
        tokens = tokens.sort((a, b) => { return a.start < b.start ? -1 : 1 });
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
                type: extractorName
            });
        });
        return ret;
    }
}

export interface IDateTimeUtilityConfiguration {
    agoRegex: RegExp
    laterRegex: RegExp
    inConnectorRegex: RegExp
    amDescRegex: RegExp
    pmDescRegex: RegExp
    amPmDescRegex: RegExp
}

export class AgoLaterUtil {
    static extractorDurationWithBeforeAndAfter(source: string, er: ExtractResult, ret: Token[], config: IDateTimeUtilityConfiguration): Array<Token> {
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
                    if (value.matched && er.start && er.length && er.start > value.index) {
                        ret.push(new Token(er.start - value.index, er.start + er.length));
                    }
                }
            }
        }
        return ret;
    }

    static parseDurationWithAgoAndLater( text:string, 
        referenceTime:Date, 
         durationExtractor:IExtractor,
         durationParser:IParser, 
        unitMap:ReadonlyMap<string,string>,
        unitRegex:RegExp    ,
         utilityConfiguration:IDateTimeUtilityConfiguration,
         mode:AgoLaterMode):DateTimeResolutionResult
    {
        let ret = new DateTimeResolutionResult();
        let durationRes = durationExtractor.extract(text);
        if (durationRes.length > 0)
        {
            let pr = durationParser.parse(durationRes[0]);
            let matches =RegExpUtility.getMatches( unitRegex,text);
            if (matches.length)
            {
                var afterStr =
                    text.substring(durationRes[0].start + durationRes[0].length)
                        .trim()
                        .toLowerCase();
                var beforeStr =
                    text.substring(0, durationRes[0].start)
                        .trim()
                        .toLowerCase();
                var srcUnit = matches[0].groups("unit").value.toLowerCase();

                if (pr.value!=null)
                {
                    var durationResult = pr.value;
                    var numStr = durationResult.timex.substring(0, durationResult.timex.length - 1)
                        .replace("P", "")
                        .replace("T", "");

                    let number = parseFloat(numStr);
                    if (!Number.isNaN(number))
                    {
                        return AgoLaterUtil.getAgoLaterResult(number,
                            unitMap,
                            srcUnit,
                            afterStr,
                            beforeStr,
                            referenceTime,
                            utilityConfiguration,
                            mode);
                    }
                }
            }
        }
        return ret;
    }

    static getAgoLaterResult(numberParam:number,
        unitMap:ReadonlyMap<string, string> ,
         srcUnit:string,
        afterStr:string,
        beforeStr:string,
         referenceTime:Date,
         utilityConfiguration:IDateTimeUtilityConfiguration,
         mode:AgoLaterMode):DateTimeResolutionResult
    {
        let ret = new DateTimeResolutionResult();

        if (unitMap.has(srcUnit))
        {
            var unitStr = unitMap.get(srcUnit);
            var numStr = numberParam.toString();
            if (MatchingUtil.containsAgoLaterIndex(afterStr, utilityConfiguration.agoRegex))
            {
                if (mode==AgoLaterMode.Date)
                {
                    return AgoLaterUtil.getDateResult(unitStr, numStr, referenceTime, false);
                }

                if (mode==AgoLaterMode.DateTime)
                {
                    return AgoLaterUtil.getDateTimeResult(unitStr, numStr, referenceTime, false);
                }
            }

            if (MatchingUtil.containsAgoLaterIndex(afterStr, utilityConfiguration.laterRegex)
                || MatchingUtil.containsInIndex(beforeStr, utilityConfiguration.inConnectorRegex))
            {
                if (mode==AgoLaterMode.Date)
                {
                    return AgoLaterUtil.getDateResult(unitStr, numStr, referenceTime, true);
                }

                if (mode==AgoLaterMode.DateTime)
                {
                    return AgoLaterUtil.getDateTimeResult(unitStr, numStr, referenceTime, true);
                }
            }
        }
        return ret;
    }

    static getDateResult(unitStr:string , numStr:string , referenceDate:Date, future:boolean):DateTimeResolutionResult
    {
        let date=new Date(referenceDate);
        let ret = new DateTimeResolutionResult();
        let futureOrPast = future ? 1 : -1;

        switch (unitStr)
        {
            case "D":{
                date.setDate(date.getDate()+(parseFloat(numStr) * futureOrPast));
                break;
            } 
            case "W":{
                date.setDate(date.getDate()+(7 * parseFloat(numStr) * futureOrPast));
                break;
            }
            case "MON": {
                date.setMonth(date.getDate()+(Math.round(parseFloat(numStr)) * futureOrPast));
                break;
            }
            case "Y": {
                date.setFullYear(date.getFullYear() + (Math.round(parseFloat(numStr)) * futureOrPast));
                break;
                }
            default: {
                return ret;
            }
        }

        ret.timex = `${FormatUtil.luisDateFromDate(date)}`;
        ret.futureValue = ret.pastValue = date;
        ret.success = true;
        return ret;
    }

    static getDateTimeResult( unitStr:string,  numStr:string, referenceTime:Date, future: boolean):DateTimeResolutionResult
    {
        let time=new Date(referenceTime);
        var ret = new DateTimeResolutionResult();
        let futureOrPast = future ? 1 : -1;

        switch (unitStr) {
            case "H": {
                time.setHours(time.getHours()+parseFloat(numStr)*futureOrPast);
                break;
            }
            case "M": {
                time.setHours(time.getHours()+parseFloat(numStr) * futureOrPast);
                break;
            }
            case "S": {
                time.setHours(time.getHours()+parseFloat(numStr) * futureOrPast);
                break;
            }
            default: {
                return ret;
            }
        }

        ret.timex = `${FormatUtil.luisDateTime(time)}`;
        ret.futureValue = ret.pastValue = time;
        ret.success = true;
        return ret;
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
        if (referencedMatches && referencedMatches.length > 0) {
            result.index = source.toLowerCase().lastIndexOf(referencedMatches[0].value) + referencedMatches[0].length;
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

export class FormatUtil {
    public static readonly HourTimexRegex = RegExpUtility.getSafeRegExp("(?<nlb>P)T\d{2}", "gis");

    // Emulates .NET ToString("D{size}")
    public static toString(num: number, size: number): string {
        let s = "000000" + (num || "");
        return s.substr(s.length - size);
    }

    public static luisDate(year: number, month: number, day: number): string {
        if (year == -1) {
            if (month == -1) {
                return new Array("XXXX", "XX", FormatUtil.toString(day, 2)).join("-");
            }

            return new Array("XXXX", FormatUtil.toString(month + 1, 2), FormatUtil.toString(day, 2)).join("-");
        }

        return new Array(FormatUtil.toString(year, 4), FormatUtil.toString(month + 1, 2), FormatUtil.toString(day, 2)).join("-");
    }

    public static luisDateFromDate(date: Date): string {
        return FormatUtil.luisDate(date.getFullYear(), date.getMonth(), date.getDate());
    }

    public static luisTime(hour: number, min: number, second: number): string {
        return new Array(FormatUtil.toString(hour, 2), FormatUtil.toString(min, 2), FormatUtil.toString(second, 2)).join(":");
    }

    public static luisTimeFromDate(time: Date): string {
        return FormatUtil.luisTime(time.getHours(), time.getMinutes(), time.getSeconds());
    }

    public static luisDateTime(time: Date): string {
        return `${FormatUtil.luisDateFromDate(time)}T${FormatUtil.luisTimeFromDate(time)}`;
    }

    public static formatDate(date: Date): string {
        return new Array(FormatUtil.toString(date.getFullYear(), 4),
            FormatUtil.toString(date.getMonth()+1, 2),
            FormatUtil.toString(date.getDate(), 2)).join("-");
    }

    public static formatTime(time: Date) {
        return new Array(FormatUtil.toString(time.getHours(), 2),
            FormatUtil.toString(time.getMinutes(), 2),
            FormatUtil.toString(time.getSeconds(), 2)).join(":");
    }

    public static FormatDateTime(datetime: Date): string {
        return `${FormatUtil.formatDate(datetime)} ${FormatUtil.formatTime(datetime)}`;
    }

    public static AllStringToPm(timeStr: string): string {
        let matches = RegExpUtility.getMatches(FormatUtil.HourTimexRegex, timeStr);
        let splited = Array<string>();
        let lastPos = 0;
        matches.forEach(match => {
            if (lastPos != match.index)
                splited.push(timeStr.substring(lastPos, match.index));
            splited.push(timeStr.substring(match.index, match.length));
            lastPos = match.index + match.length;
        });

        if (timeStr.substring(lastPos)) {
            splited.push(timeStr.substring(lastPos));
        }

        for (let i = 0; i < splited.length; i += 1) {
            if (RegExpUtility.getMatches(FormatUtil.HourTimexRegex, splited[i]).length > 0) {
                splited[i] = FormatUtil.toPm(splited[i]);
            }
        }

        return splited.join();
    }

    public static toPm(timeStr: string): string {
        let hasT = false;
        if (timeStr.startsWith("T")) {
            hasT = true;
            timeStr = timeStr.substring(1);
        }

        let splited = timeStr.split(':');
        let hour = parseInt(splited[0]);
        splited[0] = FormatUtil.toString(hour + 12, 2);

        return (hasT ? "T" : "") + splited.join(":");
    }
}

export class DateTimeResolutionResult {
    success: boolean;
    timex: string;
    isLunar: boolean;
    mod: string;
    comment: string;
    futureResolution: Map<string, string>;
    pastResolution: Map<string, string>;
    futureValue: any;
    pastValue: any;

    constructor() {
        this.success = false;
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

    static next(from: Date, dayOfWeek: DayOfWeek): Date {
        let start = from.getDay();
        let target = dayOfWeek;
        if (start === 0) start = 7;
        if (target === 0) target = 7;
        let result = new Date(from);
        result.setDate(from.getDate() + target - start + 7);
        return result;
    }

    static this(from: Date, dayOfWeek: DayOfWeek): Date {
        let start = from.getDay();
        let target = dayOfWeek;
        if (start === 0) start = 7;
        if (target === 0) target = 7;
        let result = new Date(from);
        result.setDate(from.getDate() + target - start);
        return result;
    }

    static last(from: Date, dayOfWeek: DayOfWeek): Date {
        let start = from.getDay();
        let target = dayOfWeek;
        if (start === 0) start = 7;
        if (target === 0) target = 7;
        let result = new Date(from);
        result.setDate(from.getDate() + target - start - 7);
        return result;
    }

    static diffDays(from: Date, to: Date): number {
        return Math.round(Math.abs((from.getTime() - to.getTime())/this.oneDay));
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

    static getWeekNumber(referenceDate: Date): {weekNo: number, year: number} {
        let date = new Date(Date.UTC(referenceDate.getFullYear(), referenceDate.getMonth(), referenceDate.getDate()));
        date.setUTCDate(date.getUTCDate() + 4 - (date.getUTCDay() || 7));
        let yearStart = new Date(Date.UTC(date.getUTCFullYear(), 0, 1));
        let weekNo = Math.ceil((((date.getTime() - yearStart.getTime()) / 86400000) + 1) / 7);
        return {weekNo: weekNo, year: date.getUTCFullYear()}
    }

    static minValue(): Date { return new Date(1, 0, 1, 0, 0, 0, 0); }

    static safeCreateFromValue(seedDate: Date, year: number, month: number, day: number, hour = 0, minute = 0, second = 0) {
        let result = new Date(seedDate);
        if (this.isValidDate(year, month, day) && this.isValidTime(hour, minute, second)) {
            result = new Date(year, month, day, hour, minute, second, 0);
        }
        return result;
    }

    static isLeapYear(year: number): boolean {
      return ((year % 4 === 0) && (year % 100 !== 0)) || (year % 400 === 0);
    }

    private static validDays(year: number) { return [ 31, this.isLeapYear(year) ? 29 : 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31] }

    private static isValidDate(year: number, month: number, day: number): boolean {
        return year > 0 && year <= 9999
            && month >= 0 && month < 12
            && day > 0 && day <= this.validDays(year)[month];
    }

    private static isValidTime(hour: number, minute: number, second: number) {
        return hour >= 0 && hour < 24
            && minute >= 0 && minute < 60
            && second >= 0 && minute < 60;
    }

}