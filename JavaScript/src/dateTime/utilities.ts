import { ExtractResult } from "../number/extractors"
import { RegExpUtility } from "../utilities";

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
    agoStringList: string[]
    laterStringList: string[]
    inStringList: string[]
}

export class AgoLaterUtil {
    static extractorDurationWithBeforeAndAfter(source: string, er: ExtractResult, ret: Token[], config: IDateTimeUtilityConfiguration): Array<Token> {
        let pos = er.start + er.length;
        if (pos <= source.length) {
            let afterString = source.substring(pos);
            let beforeString = source.substring(0, er.start);
            let index = -1;
            let value = MatchingUtil.getAgoLaterIndex(afterString, config.agoStringList);
            if (value.matched) {
                ret.push(new Token(er.start, er.start + er.length + value.index));
            }
            else {
                value = MatchingUtil.getAgoLaterIndex(afterString, config.laterStringList);
                if (value.matched) {
                    ret.push(new Token(er.start, er.start + er.length + value.index));
                }
                else {
                    value = MatchingUtil.getInIndex(beforeString, config.inStringList);
                    if (er.start && er.length && er.start > value.index) {
                        ret.push(new Token(er.start - value.index, er.start + er.length));
                    }
                }
            }
        }
        return ret;
    }
}

export interface MatchedIndex {
    matched: boolean,
    index: number
}

export class MatchingUtil {
    static getAgoLaterIndex(source: string, referenceList: string[]): MatchedIndex {
        let result: MatchedIndex = { matched: false, index: -1 };
        let referencedMatch = referenceList.find(o => source.trim().toLowerCase().startsWith(o));
        if (referencedMatch) {
            result.index = source.toLowerCase().lastIndexOf(referencedMatch) + referencedMatch.length;
            result.matched = true;
        }
        return result;
    }

    static getInIndex(source: string, referenceList: string[]): MatchedIndex {
        let result: MatchedIndex = { matched: false, index: -1 };
        let referencedMatch = referenceList.find(o => source.trim().toLowerCase().split(' ').pop().endsWith(o));
        if (referencedMatch) {
            result.index = source.length - source.toLowerCase().lastIndexOf(referencedMatch);
            result.matched = true;
        }
        return result;
    }
}

export class FormatUtil {
    public static readonly HourTimexRegex = RegExpUtility.getSafeRegExp("(?<nlb>P)T\d{2}", "gis");

    // Emulates .NET ToString("D{size}")
    public static toString(num: number, size: number): string {
        let s = "000000" + num;
        return s.substr(s.length - size);
    }

    public static luisDate(year: number, month: number, day: number): string {
        if (year == -1) {
            if (month == -1) {
                return new Array("XXXX", "XX", FormatUtil.toString(day, 2)).join("-");
            }

            return new Array("XXXX", FormatUtil.toString(month, 2), FormatUtil.toString(day, 2)).join("-");
        }

        return new Array(FormatUtil.toString(year, 4), FormatUtil.toString(month, 2), FormatUtil.toString(day, 2)).join("-");
    }

    public static luisDateFromDate(date: Date): string {
        return FormatUtil.luisDate(date.getFullYear(), date.getMonth(), date.getDay());
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
            FormatUtil.toString(date.getMonth(), 2),
            FormatUtil.toString(date.getDay(), 2)).join("-");
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