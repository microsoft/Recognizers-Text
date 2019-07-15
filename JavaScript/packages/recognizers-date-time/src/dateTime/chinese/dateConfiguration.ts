import { IExtractor, ExtractResult, StringUtility, Match, RegExpUtility } from "@microsoft/recognizers-text";
import { ChineseIntegerExtractor, AgnosticNumberParserFactory, ChineseNumberParserConfiguration, AgnosticNumberParserType, BaseNumberParser, BaseNumberExtractor } from "@microsoft/recognizers-text-number"
import { Constants as NumberConstants } from "@microsoft/recognizers-text-number"
import { IDateExtractorConfiguration, IDateParserConfiguration, BaseDateExtractor, BaseDateParser } from "../baseDate";
import { BaseDurationExtractor, BaseDurationParser } from "../baseDuration"
import { Constants, TimeTypeConstants } from "../constants"
import { ChineseDurationExtractor } from "./durationConfiguration";
import { Token, DateTimeFormatUtil, DateUtils, DateTimeResolutionResult, IDateTimeUtilityConfiguration, StringMap } from "../utilities";
import { ChineseDateTime } from "../../resources/chineseDateTime";
import { IDateTimeParser, DateTimeParseResult } from "../parsers"

class ChineseDateExtractorConfiguration implements IDateExtractorConfiguration {
    readonly dateRegexList: RegExp[];
    readonly implicitDateList: RegExp[];
    readonly monthEnd: RegExp;
    readonly ofMonth: RegExp;
    readonly dateUnitRegex: RegExp;
    readonly forTheRegex: RegExp;
    readonly weekDayAndDayOfMonthRegex: RegExp;
    readonly relativeMonthRegex: RegExp;
    readonly strictRelativeRegex: RegExp;
    readonly weekDayRegex: RegExp;
    readonly dayOfWeek: ReadonlyMap<string, number>;
    readonly ordinalExtractor: BaseNumberExtractor;
    readonly integerExtractor: BaseNumberExtractor;
    readonly numberParser: BaseNumberParser;
    readonly durationExtractor: BaseDurationExtractor;
    readonly utilityConfiguration: IDateTimeUtilityConfiguration;

    constructor(dmyDateFormat: boolean) {

        let enableDmy = dmyDateFormat || ChineseDateTime.DefaultLanguageFallback === Constants.DefaultLanguageFallback_DMY;

        this.dateRegexList = [
            RegExpUtility.getSafeRegExp(ChineseDateTime.DateRegexList1),
            RegExpUtility.getSafeRegExp(ChineseDateTime.DateRegexList2),
            RegExpUtility.getSafeRegExp(ChineseDateTime.DateRegexList3),
            RegExpUtility.getSafeRegExp(ChineseDateTime.DateRegexList4),
            RegExpUtility.getSafeRegExp(ChineseDateTime.DateRegexList5),

            enableDmy? 
                RegExpUtility.getSafeRegExp(ChineseDateTime.DateRegexList7):
                RegExpUtility.getSafeRegExp(ChineseDateTime.DateRegexList6),

            enableDmy? 
                RegExpUtility.getSafeRegExp(ChineseDateTime.DateRegexList6):
                RegExpUtility.getSafeRegExp(ChineseDateTime.DateRegexList7),

            RegExpUtility.getSafeRegExp(ChineseDateTime.DateRegexList8)
        ];
        this.implicitDateList = [
            RegExpUtility.getSafeRegExp(ChineseDateTime.LunarRegex),
            RegExpUtility.getSafeRegExp(ChineseDateTime.SpecialDayRegex),
            RegExpUtility.getSafeRegExp(ChineseDateTime.DateThisRegex),
            RegExpUtility.getSafeRegExp(ChineseDateTime.DateLastRegex),
            RegExpUtility.getSafeRegExp(ChineseDateTime.DateNextRegex),
            RegExpUtility.getSafeRegExp(ChineseDateTime.WeekDayRegex),
            RegExpUtility.getSafeRegExp(ChineseDateTime.WeekDayOfMonthRegex),
            RegExpUtility.getSafeRegExp(ChineseDateTime.SpecialDate)
        ];
    }
}

export class ChineseDateExtractor extends BaseDateExtractor {
    private readonly durationExtractor: ChineseDurationExtractor;

    constructor(dmyDateFormat: boolean) {
        super(new ChineseDateExtractorConfiguration(dmyDateFormat));
        this.durationExtractor = new ChineseDurationExtractor();
    }

    extract(source: string, refDate: Date): Array<ExtractResult> {
        if (!refDate) refDate = new Date();
        let referenceDate = refDate;

        let tokens: Array<Token> = new Array<Token>()
            .concat(super.basicRegexMatch(source))
            .concat(super.implicitDate(source))
            .concat(this.durationWithBeforeAndAfter(source, referenceDate));
        let result = Token.mergeAllTokens(tokens, source, this.extractorName);
        return result;
    }

    protected durationWithBeforeAndAfter(source: string, refDate: Date): Array<Token> {
        let ret = [];
        let durEx = this.durationExtractor.extract(source, refDate);
        durEx.forEach(er => {
            let pos = er.start + er.length;
            if (pos < source.length) {
                let nextChar = source.substr(pos, 1);
                if (nextChar === '前' || nextChar === '后') {
                    ret.push(new Token(er.start, pos + 1));
                }
            }
        });
        return ret;
    }
}

class ChineseDateParserConfiguration implements IDateParserConfiguration {
    readonly ordinalExtractor: BaseNumberExtractor
    readonly integerExtractor: BaseNumberExtractor
    readonly cardinalExtractor: BaseNumberExtractor
    readonly durationExtractor: BaseDurationExtractor
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
    readonly strictRelativeRegex: RegExp;
    readonly relativeWeekDayRegex: RegExp;
    readonly utilityConfiguration: IDateTimeUtilityConfiguration
    readonly dateTokenPrefix: string

    getSwiftDay(source: string): number {
        let trimmedSource = source.trim().toLowerCase();
        let swift = 0;
        if (trimmedSource === '今天' || trimmedSource === '今日' || trimmedSource === '最近') {
            swift = 0;
        } else if (trimmedSource.startsWith('明')) {
            swift = 1;
        } else if (trimmedSource.startsWith('昨')) {
            swift = -1;
        } else if (trimmedSource === '大后天' || trimmedSource === '大後天') {
            swift = 3;
        } else if (trimmedSource === '大前天') {
            swift = -3;
        } else if (trimmedSource === '后天' || trimmedSource === '後天') {
            swift = 2;
        } else if (trimmedSource === '前天') {
            swift = -2;
        }
        return swift;
    }

    getSwiftMonthOrYear(source: string): number {
        let trimmedSource = source.trim().toLowerCase();
        let swift = 0;
        if (trimmedSource.startsWith(ChineseDateTime.ParserConfigurationNextMonthToken)) {
            swift = 1;
        } else if (trimmedSource.startsWith(ChineseDateTime.ParserConfigurationLastMonthToken)) {
            swift = -1;
        }
        return swift;
    }

    getSwift(source: string): number {
        return null;
    }

    isCardinalLast(source: string): boolean {
        return source === ChineseDateTime.ParserConfigurationLastWeekDayToken;
    }

    constructor(dmyDateFormat: boolean) {
        this.dateRegex = new ChineseDateExtractorConfiguration(dmyDateFormat).dateRegexList;
        this.monthOfYear = ChineseDateTime.ParserConfigurationMonthOfYear;
        this.dayOfMonth = ChineseDateTime.ParserConfigurationDayOfMonth;
        this.dayOfWeek = ChineseDateTime.ParserConfigurationDayOfWeek;
        this.specialDayRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.SpecialDayRegex);
        this.specialDayWithNumRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.SpecialDayWithNumRegex);
        this.thisRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.DateThisRegex);
        this.nextRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.DateNextRegex);
        this.lastRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.DateLastRegex);
        this.weekDayRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.WeekDayRegex);
        this.integerExtractor = new ChineseIntegerExtractor();
        this.numberParser = AgnosticNumberParserFactory.getParser(AgnosticNumberParserType.Number, new ChineseNumberParserConfiguration());

    }
}

export class ChineseDateParser extends BaseDateParser {
    private readonly lunarRegex: RegExp
    private readonly specialDateRegex: RegExp
    private readonly tokenNextRegex: RegExp
    private readonly tokenLastRegex: RegExp
    private readonly monthMaxDays: Array<number>;

    constructor(dmyDateFormat: boolean) {
        let config = new ChineseDateParserConfiguration(dmyDateFormat);
        super(config);
        this.lunarRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.LunarRegex);
        this.specialDateRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.SpecialDate);
        this.tokenNextRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.DateNextRe);
        this.tokenLastRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.DateLastRe);
        this.monthMaxDays = [ 31, 29, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 ];
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
                // TODO create test
                innerResult = this.parserDurationWithAgoAndLater(source, referenceDate);
            }
            if (innerResult.success) {
                innerResult.futureResolution = {};
                innerResult.futureResolution[TimeTypeConstants.DATE] = DateTimeFormatUtil.formatDate(innerResult.futureValue);
                innerResult.pastResolution = {};
                innerResult.pastResolution[TimeTypeConstants.DATE] = DateTimeFormatUtil.formatDate(innerResult.pastValue);
                innerResult.isLunar = this.parseLunarCalendar(source);
                resultValue = innerResult;
            }
        }
        let result = new DateTimeParseResult(extractorResult);
        result.value = resultValue;
        result.timexStr = resultValue ? resultValue.timex : '';
        result.resolutionStr = '';

        return result;
    }

    private parseLunarCalendar(source: string): boolean {
        return RegExpUtility.isMatch(this.lunarRegex, source.trim());
    }

    protected parseBasicRegexMatch(source: string, referenceDate: Date): DateTimeResolutionResult {
        let trimmedSource = source.trim();
        let result = new DateTimeResolutionResult();
        this.config.dateRegex.some(regex => {
            let match = RegExpUtility.getMatches(regex, trimmedSource).pop();
            if (match && match.index === 0 && match.length === trimmedSource.length) {
                result = this.matchToDate(match, referenceDate);
                return true;
            }
        });
        return result;
    }

    protected parseImplicitDate(source: string, referenceDate: Date): DateTimeResolutionResult {
        let trimmedSource = source.trim();
        let result = new DateTimeResolutionResult();
        // handle "on 12"
        let match = RegExpUtility.getMatches(this.specialDateRegex, trimmedSource).pop();
        if (match && match.length === trimmedSource.length) {
            let day = 0;
            let month = referenceDate.getMonth();
            let year = referenceDate.getFullYear();
            let yearStr = match.groups('thisyear').value;
            let monthStr = match.groups('thismonth').value;
            let dayStr = match.groups('day').value;
            day = this.config.dayOfMonth.get(dayStr);

            let hasYear = !StringUtility.isNullOrEmpty(yearStr);
            let hasMonth = !StringUtility.isNullOrEmpty(monthStr);

            if (hasMonth) {
                if (RegExpUtility.isMatch(this.tokenNextRegex, monthStr)) {
                    month++;
                    if (month === Constants.MaxMonth + 1) {
                        month = Constants.MinMonth;
                        year++;
                    }
                } else if (RegExpUtility.isMatch(this.tokenLastRegex, monthStr)) {
                    month--;
                    if (month === Constants.MinMonth - 1) {
                        month = Constants.MaxMonth;
                        year--;
                    }
                }
                if (hasYear) {
                    if (RegExpUtility.isMatch(this.tokenNextRegex, yearStr)) {
                        year++;
                    } else if (RegExpUtility.isMatch(this.tokenLastRegex, yearStr)) {
                        year--;
                    }
                }
            }

            result.timex = DateTimeFormatUtil.luisDate(hasYear ? year : -1, hasMonth ? month : -1, day);
            let futureDate: Date;
            let pastDate: Date;

            if (day > this.getMonthMaxDay(year, month)) {
                let futureMonth = month + 1;
                let pastMonth = month - 1;
                let futureYear = year;
                let pastYear = year;

                if (futureMonth === Constants.MaxMonth + 1) {
                    futureMonth = Constants.MinMonth;
                    futureYear = year++;
                }
                if (pastMonth === Constants.MinMonth - 1) {
                    pastMonth = Constants.MaxMonth;
                    pastYear = year--;
                }

                let isFutureValid = DateUtils.isValidDate(futureYear, futureMonth, day);
                let isPastValid = DateUtils.isValidDate(pastYear, pastMonth, day);

                if (isFutureValid && isPastValid) {
                    futureDate = DateUtils.safeCreateFromMinValue(futureYear, futureMonth, day);
                    pastDate = DateUtils.safeCreateFromMinValue(pastYear, pastMonth, day);
                }
                else if (isFutureValid && !isPastValid) {
                    futureDate = pastDate = DateUtils.safeCreateFromMinValue(futureYear, futureMonth, day);
                }
                else if (!isFutureValid && !isPastValid){
                    futureDate = pastDate = DateUtils.safeCreateFromMinValue(pastYear, pastMonth, day);
                }
                else {
                    futureDate = pastDate = DateUtils.safeCreateFromMinValue(year, month, day);
                }
            } else {
                futureDate = DateUtils.safeCreateFromMinValue(year, month, day);
                pastDate = DateUtils.safeCreateFromMinValue(year, month, day);
                
                if (!hasMonth) {
                    if (futureDate < referenceDate) {
                        if (this.isValidDate(year, month + 1, day)) {
                            futureDate = DateUtils.addMonths(futureDate, 1);
                        }
                    }
                    if (pastDate >= referenceDate) {
                        if (this.isValidDate(year, month - 1, day)) {
                            pastDate = DateUtils.addMonths(pastDate, -1);
                        }
                        else if (this.isNonleapYearFeb29th(year, month - 1, day)){
                            pastDate = DateUtils.addMonths(pastDate, -2);
                        }
                    }
                } else if (hasMonth && !hasYear) {
                    if (futureDate < referenceDate) {
                        if (DateUtils.isValidDate(year + 1, month, day)) {
                            futureDate = DateUtils.addYears(futureDate, 1);
                        }
                    }
                    if (pastDate >= referenceDate) {
                        if (DateUtils.isValidDate(year - 1, month, day)){
                            pastDate = DateUtils.addYears(pastDate, -1);
                        }
                    }
                }
            }

            result.futureValue = futureDate;
            result.pastValue = pastDate;
            result.success = true;
            return result;
        }

        // handle cases like "昨日", "明日", "大后天"
        match = RegExpUtility.getMatches(this.config.specialDayRegex, trimmedSource).pop();
        if (match && match.index === 0 && match.length === trimmedSource.length) {
            let swift = this.config.getSwiftDay(match.value);
            let value = DateUtils.addDays(referenceDate, swift);

            result.timex = DateTimeFormatUtil.luisDateFromDate(value);
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

            result.timex = DateTimeFormatUtil.luisDateFromDate(value);
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

            result.timex = DateTimeFormatUtil.luisDateFromDate(value);
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

            result.timex = DateTimeFormatUtil.luisDateFromDate(value);
            result.futureValue = value;
            result.pastValue = value;
            result.success = true;
            return result;
        }

        // handle "Friday"
        match = RegExpUtility.getMatches(this.config.weekDayRegex, trimmedSource).pop();
        if (match && match.index === 0 && match.length === trimmedSource.length) {
            let weekdayStr = match.groups('weekday').value;
            let weekday = this.config.dayOfWeek.get(weekdayStr);
            let value = DateUtils.this(referenceDate, weekday);

            if (weekday === 0) weekday = 7;
            if (weekday < referenceDate.getDay()) value = DateUtils.next(referenceDate, weekday);
            result.timex = 'XXXX-WXX-' + weekday;
            let futureDate = new Date(value);
            let pastDate = new Date(value);
            if (futureDate < referenceDate) futureDate = DateUtils.addDays(futureDate, 7);
            if (pastDate >= referenceDate) pastDate = DateUtils.addDays(pastDate, -7);

            result.futureValue = futureDate;
            result.pastValue = pastDate;
            result.success = true;
            return result;
        }

        return result;
    }

    protected matchToDate(match: Match, referenceDate: Date): DateTimeResolutionResult {
        let result = new DateTimeResolutionResult();
        let yearStr = match.groups('year').value;
        let yearChs = match.groups('yearchs').value;
        let monthStr = match.groups('month').value;
        let dayStr = match.groups('day').value;
        let month = 0;
        let day = 0;
        let year = 0;
        let yearTemp = this.convertChineseYearToNumber(yearChs);
        year = yearTemp === -1 ? 0 : yearTemp;

        if (this.config.monthOfYear.has(monthStr) && this.config.dayOfMonth.has(dayStr)) {
            month = this.getMonthOfYear(monthStr);
            day = this.getDayOfMonth(dayStr);
            if (!StringUtility.isNullOrEmpty(yearStr)) {
                year = Number.parseInt(yearStr, 10);
                if (year < 100 && year >= Constants.MinTwoDigitYearPastNum) year += 1900;
                else if (year >= 0 && year < Constants.MaxTwoDigitYearFutureNum) year += 2000;
            }
        }
        let noYear = false;
        if (year === 0) {
            year = referenceDate.getFullYear();
            result.timex = DateTimeFormatUtil.luisDate(-1, month, day);
            noYear = true;
        } else {
            result.timex = DateTimeFormatUtil.luisDate(year, month, day);
        }
        let futureDate = DateUtils.safeCreateFromMinValue(year, month, day);
        let pastDate = DateUtils.safeCreateFromMinValue(year, month, day);
        if (noYear && futureDate < referenceDate) {
            futureDate = DateUtils.safeCreateFromMinValue(year + 1, month, day);
        }
        if (noYear && pastDate >= referenceDate) {
            pastDate = DateUtils.safeCreateFromMinValue(year - 1, month, day);
        }
        result.futureValue = futureDate;
        result.pastValue = pastDate;
        result.success = true;
        return result;
    }

    private convertChineseYearToNumber(source: string): number {
        let year = 0;
        let er = this.config.integerExtractor.extract(source).pop();
        if (er && er.type === NumberConstants.SYS_NUM_INTEGER) {
            year = Number.parseInt(this.config.numberParser.parse(er).value);
        }
        if (year < 10) {
            year = 0;
            for (let i = 0; i < source.length; i++) {
                let char = source.charAt(i);
                year *= 10;
                let er = this.config.integerExtractor.extract(char).pop();
                if (er && er.type === NumberConstants.SYS_NUM_INTEGER) {
                    year += Number.parseInt(this.config.numberParser.parse(er).value);
                }
            }
        }
        return year < 10 ? -1 : year;
    }

    private getMonthOfYear(source: string): number {
        let month = this.config.monthOfYear.get(source) > 12
            ? this.config.monthOfYear.get(source) % 12
            : this.config.monthOfYear.get(source);
        return month - 1;
    }

    private getDayOfMonth(source: string): number {
        return this.config.dayOfMonth.get(source) > 31
            ? this.config.dayOfMonth.get(source) % 31
            : this.config.dayOfMonth.get(source);
    }

    private getMonthMaxDay(year: number, month: number): number {
        let maxDay = this.monthMaxDays[month];

            if (!DateUtils.isLeapYear(year) && month === 1)
            {
                maxDay -= 1;
            }

            return maxDay;
    }

    private isValidDate(year: number, month: number, day: number): boolean {
        if (month < Constants.MinMonth)
            {
                year--;
                month = Constants.MaxMonth;
            }

            if (month > Constants.MaxMonth)
            {
                year++;
                month = Constants.MinMonth;
            }
            return DateUtils.isValidDate(year, month, day);
    }

    private isNonleapYearFeb29th(year: number, month: number, day: number): boolean {
        return !DateUtils.isLeapYear(year) && month === 1 && day === 29;
    }
}