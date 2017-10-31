import { Constants, TimeTypeConstants } from "./constants";
import { IExtractor, ExtractResult, RegExpUtility, Match } from "recognizers-text-number"
import { Token, FormatUtil, DateTimeResolutionResult, DayOfWeek, DateUtils } from "./utilities";
import { IDateTimeParser, DateTimeParseResult } from "./parsers"
import { BaseDateTime } from "../resources/baseDateTime";

export interface IHolidayExtractorConfiguration {
    holidayRegexes: RegExp[]
}

export class BaseHolidayExtractor implements IExtractor {
    private readonly extractorName = Constants.SYS_DATETIME_DATE
    private readonly config: IHolidayExtractorConfiguration;

    constructor(config: IHolidayExtractorConfiguration) {
        this.config = config;
    }

    extract(source: string): Array<ExtractResult> {
        let tokens: Array<Token> = new Array<Token>()
            .concat(this.holidayMatch(source))
        let result = Token.mergeAllTokens(tokens, source, this.extractorName);
        return result;
    }

    private holidayMatch(source: string): Array<Token> {
        let ret = [];
        this.config.holidayRegexes.forEach(regex => {
            RegExpUtility.getMatches(regex, source).forEach(match => {
                ret.push(new Token(match.index, match.index + match.length))
            });
        });
        return ret;
    }
}

export interface IHolidayParserConfiguration {
    variableHolidaysTimexDictionary: ReadonlyMap<string, string>;
    holidayFuncDictionary: ReadonlyMap<string, (year: number) => Date>;
    holidayNames: ReadonlyMap<string, string[]>;
    holidayRegexList: RegExp[];
    getSwiftYear(text: string): number;
    sanitizeHolidayToken(holiday: string): string;
}

export class BaseHolidayParser implements IDateTimeParser {
    public static readonly ParserName = Constants.SYS_DATETIME_DATE; // "Date";
    protected readonly config: IHolidayParserConfiguration;

    constructor(config: IHolidayParserConfiguration) {
        this.config = config;
    }

    public parse(er: ExtractResult, referenceDate: Date): DateTimeParseResult {
        if (!referenceDate) referenceDate = new Date();
        let value = null;

        if (er.type === BaseHolidayParser.ParserName) {
            let innerResult = this.parseHolidayRegexMatch(er.text, referenceDate);

            if (innerResult.success) {
                innerResult.futureResolution = new Map<string, string>([
                    [TimeTypeConstants.DATE, FormatUtil.formatDate(innerResult.futureValue)]
                ]);
                innerResult.pastResolution = new Map<string, string>([
                    [TimeTypeConstants.DATE, FormatUtil.formatDate(innerResult.pastValue)]
                ]);
                value = innerResult;
            }
        }

        let ret = new DateTimeParseResult(er);
        ret.value = value;
        ret.timexStr = value === null ? "" : value.timex;
        ret.resolutionStr = "";

        return ret;
    }

    protected parseHolidayRegexMatch(text: string, referenceDate: Date): DateTimeResolutionResult {
        let trimmedText = text.trim();
        for (let regex of this.config.holidayRegexList) {
            let offset = 0;
            let matches = RegExpUtility.getMatches(regex, trimmedText);
            if (matches.length && matches[0].index === offset && matches[0].length === trimmedText.length) {
                // LUIS value string will be set in Match2Date method
                let ret = this.match2Date(matches[0], referenceDate);
                return ret;
            }
        }
        return new DateTimeResolutionResult();
    }

    protected match2Date(match: Match, referenceDate: Date): DateTimeResolutionResult {
        let ret = new DateTimeResolutionResult();
        let holidayStr = this.config.sanitizeHolidayToken(match.groups("holiday").value.toLowerCase());

        // get year (if exist)
        let yearStr = match.groups("year").value.toLowerCase();
        let orderStr = match.groups("order").value.toLowerCase();
        let year: number;
        let hasYear = false;

        if (yearStr) {
            year = parseInt(yearStr, 10);
            hasYear = true;
        }
        else if (orderStr) {
            let swift = this.config.getSwiftYear(orderStr);
            if (swift < -1) {
                return ret;
            }
            year = referenceDate.getFullYear() + swift;
            hasYear = true;
        }
        else {
            year = referenceDate.getFullYear();
        }

        let holidayKey: string;
        for (holidayKey of this.config.holidayNames.keys()) {
            if (this.config.holidayNames.get(holidayKey).indexOf(holidayStr) > -1) {
                break;
            }
        }

        if (holidayKey) {
            let timexStr: string;
            let value = referenceDate;
            let func = this.config.holidayFuncDictionary.get(holidayKey);
            if (func) {
                value = func(year);
                timexStr = this.config.variableHolidaysTimexDictionary.get(holidayKey);
                if (!timexStr) {
                    timexStr = `-${FormatUtil.toString(value.getMonth() + 1, 2)}-${FormatUtil.toString(value.getDate(), 2)}`;
                }
            }
            else {
                return ret;
            }

            if (value.getTime() === DateUtils.minValue().getTime()) {
                ret.timex = '';
                ret.futureValue = DateUtils.minValue();
                ret.pastValue = DateUtils.minValue();
                ret.success = true;
                return ret;
            }

            if (hasYear) {
                ret.timex = FormatUtil.toString(year, 4) + timexStr;
                ret.futureValue = ret.pastValue = new Date(year, value.getMonth(), value.getDate());
                ret.success = true;
                return ret;
            }

            ret.timex = "XXXX" + timexStr;
            ret.futureValue = this.getFutureValue(value, referenceDate, holidayKey);
            ret.pastValue = this.getPastValue(value, referenceDate, holidayKey);
            ret.success = true;

            return ret;
        }

        return ret;
    }

    private getFutureValue(value: Date, referenceDate: Date, holiday: string): Date {
        if (value < referenceDate) {
            let func = this.config.holidayFuncDictionary.get(holiday);
            if (func) {
                return func(value.getFullYear() + 1);
            }
        }
        return value;
    }

    private getPastValue(value: Date, referenceDate: Date, holiday: string): Date {
        if (value >= referenceDate) {
            let func = this.config.holidayFuncDictionary.get(holiday);
            if (func) {
                return func(value.getFullYear() - 1);
            }
        }
        return value;
    }
}

export abstract class BaseHolidayParserConfiguration implements IHolidayParserConfiguration {
    variableHolidaysTimexDictionary: ReadonlyMap<string, string>;
    holidayFuncDictionary: ReadonlyMap<string, (year: number) => Date>;
    holidayNames: ReadonlyMap<string, string[]>;
    holidayRegexList: RegExp[];
    abstract getSwiftYear(text: string): number;
    abstract sanitizeHolidayToken(holiday: string): string;

    constructor() {
        this.variableHolidaysTimexDictionary = BaseDateTime.VariableHolidaysTimexDictionary;
        this.holidayFuncDictionary = this.initHolidayFuncs();
    }

    // TODO auto-generate from YAML
    protected initHolidayFuncs(): ReadonlyMap<string, (year: number) => Date> {
        return new Map<string, (year: number) => Date>(
            [
                ["fathers", BaseHolidayParserConfiguration.FathersDay],
                ["mothers", BaseHolidayParserConfiguration.MothersDay],
                ["thanksgivingday", BaseHolidayParserConfiguration.ThanksgivingDay],
                ["thanksgiving", BaseHolidayParserConfiguration.ThanksgivingDay],
                ["martinlutherking", BaseHolidayParserConfiguration.MartinLutherKingDay],
                ["washingtonsbirthday", BaseHolidayParserConfiguration.WashingtonsBirthday],
                ["canberra", BaseHolidayParserConfiguration.CanberraDay],
                ["labour", BaseHolidayParserConfiguration.LabourDay],
                ["columbus", BaseHolidayParserConfiguration.ColumbusDay],
                ["memorial", BaseHolidayParserConfiguration.MemorialDay]
            ]);
    }

    // All months are zero-based (-1)
    // TODO auto-generate from YAML
    protected static MothersDay(year: number): Date { return new Date(year, 5 - 1, BaseHolidayParserConfiguration.getDay(year, 5 - 1, 1, DayOfWeek.Sunday)); }

    protected static FathersDay(year: number): Date { return new Date(year, 6 - 1, BaseHolidayParserConfiguration.getDay(year, 6 - 1, 2, DayOfWeek.Sunday)); }

    private static MartinLutherKingDay(year: number): Date { return new Date(year, 1 - 1, BaseHolidayParserConfiguration.getDay(year, 1 - 1, 2, DayOfWeek.Monday)); }

    private static WashingtonsBirthday(year: number): Date { return new Date(year, 2 - 1, BaseHolidayParserConfiguration.getDay(year, 2 - 1, 2, DayOfWeek.Monday)); }

    private static CanberraDay(year: number): Date { return new Date(year, 3 - 1, BaseHolidayParserConfiguration.getDay(year, 3 - 1, 0, DayOfWeek.Monday)); }

    protected static MemorialDay(year: number): Date { return new Date(year, 5 - 1, BaseHolidayParserConfiguration.getLastDay(year, 5 - 1, DayOfWeek.Monday)); }

    protected static LabourDay(year: number): Date { return new Date(year, 9 - 1, BaseHolidayParserConfiguration.getDay(year, 9 - 1, 0, DayOfWeek.Monday)); }

    protected static ColumbusDay(year: number): Date { return new Date(year, 10 - 1, BaseHolidayParserConfiguration.getDay(year, 10 - 1, 1, DayOfWeek.Monday)); }

    protected static ThanksgivingDay(year: number): Date { return new Date(year, 11 - 1, BaseHolidayParserConfiguration.getDay(year, 11 - 1, 3, DayOfWeek.Thursday)); }

    protected static getDay(year: number, month: number, week: number, dayOfWeek: DayOfWeek): number {
        let days = Array.apply(null, new Array(new Date(year, month, 0).getDate())).map(function (x, i) { return i + 1 });
        days = days.filter(function (day) {
            return new Date(year, month, day).getDay() === dayOfWeek;
        });
        return days[week];
    }

    protected static getLastDay(year: number, month: number, dayOfWeek: DayOfWeek): number {
        let days = Array.apply(null, new Array(new Date(year, month, 0).getDate())).map(function (x, i) { return i + 1 });
        days = days.filter(function (day) {
            return new Date(year, month, day).getDay() === dayOfWeek;
        });
        return days[days.length - 1];
    }
}
