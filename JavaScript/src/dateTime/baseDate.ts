import { Constants, TimeTypeConstants } from "./constants";
import { IExtractor, ExtractResult, BaseNumberExtractor } from "../number/extractors"
import { BaseNumberParser } from "../number/parsers"
import { RegExpUtility, Match, StringUtility } from "../utilities";
import { Token, FormatUtil, DateTimeResolutionResult, IDateTimeUtilityConfiguration, AgoLaterUtil, AgoLaterMode, DateUtils } from "./utilities";
import { BaseDurationExtractor, BaseDurationParser } from "./baseDuration"
import { IDateTimeParser, DateTimeParseResult } from "./parsers"
import { toNumber } from "lodash";

export interface IDateExtractorConfiguration {
    dateRegexList: RegExp[],
    implicitDateList: RegExp[],
    monthEnd: RegExp,
    ofMonth: RegExp,
    dateUnitRegex: RegExp,
    ordinalExtractor: BaseNumberExtractor,
    integerExtractor: BaseNumberExtractor,
    numberParser: BaseNumberParser
    durationExtractor: BaseDurationExtractor
    utilityConfiguration: IDateTimeUtilityConfiguration
}

export class BaseDateExtractor implements IExtractor {
    private readonly extractorName = Constants.SYS_DATETIME_DATE;
    private readonly config: IDateExtractorConfiguration;

    constructor(config: IDateExtractorConfiguration) {
        this.config = config;
    }

    extract(source: string): Array<ExtractResult> {
        let tokens: Array<Token> = new Array<Token>()
        .concat(this.basicRegexMatch(source))
        .concat(this.implicitDate(source))
        .concat(this.numberWithMonth(source))
        .concat(this.durationWithBeforeAndAfter(source));
        let result = Token.mergeAllTokens(tokens, source, this.extractorName);
        return result;
    }

    private basicRegexMatch(source: string): Array<Token> {
        let ret = [];
        this.config.dateRegexList.forEach(regexp => {
            let matches = RegExpUtility.getMatches(regexp, source);
            matches.forEach(match => {
                ret.push(new Token(match.index, match.index + match.length));
            });
        });
        return ret;
    }

    private implicitDate(source: string): Array<Token> {
        let ret = [];
        this.config.implicitDateList.forEach(regexp => {
            let matches = RegExpUtility.getMatches(regexp, source);
            matches.forEach(match => {
                ret.push(new Token(match.index, match.index + match.length));
            });
        });
        return ret;
    }

    private numberWithMonth(source: string): Array<Token> {
        let ret = [];
        let er = Array.prototype.concat(this.config.ordinalExtractor.extract(source), this.config.integerExtractor.extract(source));
        er.forEach(result => {
            let num = toNumber(this.config.numberParser.parse(result).value);
            if (num < 1 || num > 31) {
                return;
            }
            if (result.start > 0) {
                let frontString = source.substring(0, result.start | 0);
                let match = RegExpUtility.getMatches(this.config.monthEnd, frontString)[0];
                if (match && match.length) {
                    ret.push(new Token(match.index, match.index + match.length + result.length));
                    return;
                }
            }
            if (result.start + result.length < source.length) {
                let afterString = source.substring(result.start + result.length);
                let match = RegExpUtility.getMatches(this.config.ofMonth, afterString)[0];
                if (match && match.length) {
                    ret.push(new Token(result.start, result.start + result.length + match.length));
                    return;
                }
            }
        });
        return ret;
    }

    private durationWithBeforeAndAfter(source: string): Array<Token> {
        let ret = [];
        let durEx = this.config.durationExtractor.extract(source);
        durEx.forEach(er => {
            let match = RegExpUtility.getMatches(this.config.dateUnitRegex, er.text).pop();
            if (!match) return;
            ret = AgoLaterUtil.extractorDurationWithBeforeAndAfter(source, er, ret, this.config.utilityConfiguration);
        });
        return ret;
    }
}

export interface IDateParserConfiguration {
    ordinalExtractor: BaseNumberExtractor
    integerExtractor: BaseNumberExtractor
    cardinalExtractor: BaseNumberExtractor
    durationExtractor: BaseDurationExtractor
    durationParser: BaseDurationParser
    numberParser: BaseNumberParser
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
    weekDayRegex: RegExp
    lastRegex: RegExp
    thisRegex: RegExp
    weekDayOfMonthRegex: RegExp
    utilityConfiguration: IDateTimeUtilityConfiguration
    dateTokenPrefix: string
    getSwiftDay(source: string): number
    getSwiftMonth(source: string): number
    isCardinalLast(source: string): boolean
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
        let trimmedSource = source.trim();
        let result = new DateTimeResolutionResult();
        // handle "on 12"
        let match = RegExpUtility.getMatches(this.config.onRegex, this.config.dateTokenPrefix + trimmedSource).pop();
        if (match && match.index === this.config.dateTokenPrefix.length && match.length === trimmedSource.length) {
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
        match = RegExpUtility.getMatches(this.config.specialDayRegex, trimmedSource).pop();
        if (match && match.index === 0 && match.length === trimmedSource.length) {
            let swift = this.config.getSwiftDay(match.value);
            let value = new Date(referenceDate.getFullYear(), referenceDate.getMonth(), referenceDate.getDate() + swift);
            result.timex = FormatUtil.luisDateFromDate(value);
            result.futureValue = value;
            result.pastValue = value;
            result.success = true;
            return result;
        }

        // handle "next Sunday"
        match = RegExpUtility.getMatches(this.config.nextRegex, trimmedSource).pop();
        if (match && match.index === 0 && match.length === trimmedSource.length) {
            let weekdayStr = match.groups('weekday').value;
            let value = DateUtils.next(referenceDate, this.config.dayOfWeek.get(weekdayStr));

            result.timex = FormatUtil.luisDateFromDate(value);
            result.futureValue = value;
            result.pastValue = value;
            result.success = true;
            return result;
        }

        // handle "this Friday"
        match = RegExpUtility.getMatches(this.config.thisRegex, trimmedSource).pop();
        if (match && match.index === 0 && match.length === trimmedSource.length) {
            let weekdayStr = match.groups('weekday').value;
            let value = DateUtils.this(referenceDate, this.config.dayOfWeek.get(weekdayStr));

            result.timex = FormatUtil.luisDateFromDate(value);
            result.futureValue = value;
            result.pastValue = value;
            result.success = true;
            return result;
        }

        // handle "last Friday", "last mon"
        match = RegExpUtility.getMatches(this.config.lastRegex, trimmedSource).pop();
        if (match && match.index === 0 && match.length === trimmedSource.length) {
            let weekdayStr = match.groups('weekday').value;
            let value = DateUtils.last(referenceDate, this.config.dayOfWeek.get(weekdayStr));

            result.timex = FormatUtil.luisDateFromDate(value);
            result.futureValue = value;
            result.pastValue = value;
            result.success = true;
            return result;
        }

        // handle "Friday"
        match = RegExpUtility.getMatches(this.config.weekDayRegex , trimmedSource).pop();
        if (match && match.index === 0 && match.length === trimmedSource.length) {
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
        let trimmedSource = source.trim();
        let result = new DateTimeResolutionResult();
        let match = RegExpUtility.getMatches(this.config.monthRegex, trimmedSource).pop();
        if (!match) return result;
        let er = this.config.ordinalExtractor.extract(trimmedSource);
        if (!er || er.length === 0) {
            er = this.config.integerExtractor.extract(trimmedSource);
        }
        if (!er || er.length === 0) return result;

        let year = referenceDate.getFullYear();
        let month = this.config.monthOfYear.get(match.value.trim()) - 1;
        let day = Number.parseInt(this.config.numberParser.parse(er[0]).value, 10);

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
        return AgoLaterUtil.parseDurationWithAgoAndLater(
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
        let trimmedSource = source.trim();
        let result = new DateTimeResolutionResult();
        let match = RegExpUtility.getMatches(this.config.weekDayOfMonthRegex, trimmedSource).pop();
        if (!match) return result;
        let cardinalStr = match.groups('cardinal').value;
        let weekdayStr = match.groups('weekday').value;
        let monthStr = match.groups('month').value;
        let noYear = false;
        let cardinal = this.config.isCardinalLast(cardinalStr) ? 5 : this.config.cardinalMap.get(cardinalStr);
        let weekday = this.config.dayOfWeek.get(weekdayStr);
        let month = referenceDate.getMonth();
        let year = referenceDate.getFullYear();
        if (StringUtility.isNullOrEmpty(monthStr)) {
            let swift = this.config.getSwiftMonth(trimmedSource);
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
            if (!StringUtility.isNullOrEmpty(yearStr)) {
                year = Number.parseInt(yearStr, 10);
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