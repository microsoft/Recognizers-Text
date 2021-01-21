import { IExtractor, ExtractResult, StringUtility, Match, RegExpUtility, MetaData } from "@microsoft/recognizers-text";
import { ChineseIntegerExtractor, AgnosticNumberParserFactory, ChineseNumberParserConfiguration, AgnosticNumberParserType, BaseNumberParser, BaseNumberExtractor } from "@microsoft/recognizers-text-number";
import { Constants as NumberConstants } from "@microsoft/recognizers-text-number";
import { IDateExtractorConfiguration, IDateParserConfiguration, BaseDateExtractor, BaseDateParser } from "../baseDate";
import { BaseDurationExtractor, BaseDurationParser } from "../baseDuration";
import { Constants, TimeTypeConstants } from "../constants";
import { ChineseDurationExtractor } from "./durationConfiguration";
import { Token, DateTimeFormatUtil, DateUtils, DateTimeResolutionResult, IDateTimeUtilityConfiguration, StringMap } from "../utilities";
import { ChineseDateTime } from "../../resources/chineseDateTime";
import { IDateTimeParser, DateTimeParseResult } from "../parsers";

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
        let enableYmd= ChineseDateTime.DefaultLanguageFallback === Constants.DefaultLanguageFallback_YMD;

        this.dateRegexList = [
            RegExpUtility.getSafeRegExp(ChineseDateTime.DateRegexList1),
            RegExpUtility.getSafeRegExp(ChineseDateTime.DateRegexList2),
            RegExpUtility.getSafeRegExp(ChineseDateTime.DateRegexList3),
            // 2015-12-23 - This regex represents the standard format in Chinese dates (YMD) and has precedence over other orderings
            RegExpUtility.getSafeRegExp(ChineseDateTime.DateRegexList8),

            // Regex precedence where the order between D and M varies is controlled by DefaultLanguageFallback
            enableDmy ?
                RegExpUtility.getSafeRegExp(ChineseDateTime.DateRegexList5) :
                RegExpUtility.getSafeRegExp(ChineseDateTime.DateRegexList4),

            enableDmy ?
                RegExpUtility.getSafeRegExp(ChineseDateTime.DateRegexList4) :
                RegExpUtility.getSafeRegExp(ChineseDateTime.DateRegexList5),

            enableDmy ?
                RegExpUtility.getSafeRegExp(ChineseDateTime.DateRegexList7) :
                (enableYmd ?
                    RegExpUtility.getSafeRegExp(ChineseDateTime.DateRegexList7) :
                    RegExpUtility.getSafeRegExp(ChineseDateTime.DateRegexList6)),

            enableDmy ?
                RegExpUtility.getSafeRegExp(ChineseDateTime.DateRegexList6) :
                (enableYmd ?
                    RegExpUtility.getSafeRegExp(ChineseDateTime.DateRegexList6) :
                    RegExpUtility.getSafeRegExp(ChineseDateTime.DateRegexList7)),
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
    static beforeRegex: RegExp = RegExpUtility.getSafeRegExp(ChineseDateTime.BeforeRegex);
    static afterRegex: RegExp = RegExpUtility.getSafeRegExp(ChineseDateTime.AfterRegex);
    static dateTimePeriodUnitRegex: RegExp = RegExpUtility.getSafeRegExp(ChineseDateTime.DateTimePeriodUnitRegex);
    private readonly durationExtractor: ChineseDurationExtractor;

    constructor(dmyDateFormat: boolean) {
        super(new ChineseDateExtractorConfiguration(dmyDateFormat));
        this.durationExtractor = new ChineseDurationExtractor();
    }

    extract(source: string, refDate: Date): ExtractResult[] {
        if (!refDate) {
            refDate = new Date();
        }
        let referenceDate = refDate;

        let tokens: Token[] = new Array<Token>()
            .concat(super.basicRegexMatch(source))
            .concat(super.implicitDate(source))
            .concat(this.durationWithAgoAndLater(source, referenceDate));
        let result = Token.mergeAllTokens(tokens, source, this.extractorName);
        return result;
    }

    protected durationWithAgoAndLater(source: string, refDate: Date): Token[] {
        let ret = [];
        let durationEr = this.durationExtractor.extract(source, refDate);
        durationEr.forEach(er => {
            if (!RegExpUtility.isMatch(ChineseDateExtractor.dateTimePeriodUnitRegex, er.text)) {
                let pos = er.start + er.length;
                if (pos < source.length) {
                    let suffix = source.substr(pos, 1);
                    let beforeMatch = RegExpUtility.getMatches(ChineseDateExtractor.beforeRegex, suffix).pop();
                    let afterMatch = RegExpUtility.getMatches(ChineseDateExtractor.afterRegex, suffix).pop();

                    if (beforeMatch && suffix.startsWith(beforeMatch.value) || afterMatch && suffix.startsWith(afterMatch.value)) {
                        let metadata = new MetaData();
                        metadata.IsDurationWithAgoAndLater = true;
                        ret.push(new Token(er.start, pos + 1, metadata));
                    }
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
        }
        else if (trimmedSource.startsWith('明')) {
            swift = 1;
        }
        else if (trimmedSource.startsWith('昨')) {
            swift = -1;
        }
        else if (trimmedSource === '大后天' || trimmedSource === '大後天') {
            swift = 3;
        }
        else if (trimmedSource === '大前天') {
            swift = -3;
        }
        else if (trimmedSource === '后天' || trimmedSource === '後天') {
            swift = 2;
        }
        else if (trimmedSource === '前天') {
            swift = -2;
        }
        return swift;
    }

    getSwiftMonthOrYear(source: string): number {
        let trimmedSource = source.trim().toLowerCase();
        let swift = 0;
        if (trimmedSource.startsWith(ChineseDateTime.ParserConfigurationNextMonthToken)) {
            swift = 1;
        }
        else if (trimmedSource.startsWith(ChineseDateTime.ParserConfigurationLastMonthToken)) {
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
        this.unitRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.DateUnitRegex);
        this.unitMap = ChineseDateTime.ParserConfigurationUnitMap;
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
    private readonly monthMaxDays: number[];
    private readonly durationExtractor: ChineseDurationExtractor;
    readonly dynastyStartYear: string;
    readonly dynastyYearRegex: RegExp;
    readonly dynastyYearMap: ReadonlyMap<string, number>;

    constructor(dmyDateFormat: boolean) {
        let config = new ChineseDateParserConfiguration(dmyDateFormat);
        super(config);
        this.lunarRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.LunarRegex);
        this.specialDateRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.SpecialDate);
        this.tokenNextRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.NextPrefixRegex);
        this.tokenLastRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.LastPrefixRegex);
        this.monthMaxDays = [31, 29, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];
        this.durationExtractor = new ChineseDurationExtractor();
        this.dynastyStartYear = ChineseDateTime.DynastyStartYear;
        this.dynastyYearRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.DynastyYearRegex);
        this.dynastyYearMap = ChineseDateTime.DynastyYearMap;
    }

    parse(extractorResult: ExtractResult, referenceDate?: Date): DateTimeParseResult | null {
        if (!referenceDate) {
            referenceDate = new Date();
        }
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
                }
                else if (RegExpUtility.isMatch(this.tokenLastRegex, monthStr)) {
                    month--;
                    if (month === Constants.MinMonth - 1) {
                        month = Constants.MaxMonth;
                        year--;
                    }
                }
                if (hasYear) {
                    if (RegExpUtility.isMatch(this.tokenNextRegex, yearStr)) {
                        year++;
                    }
                    else if (RegExpUtility.isMatch(this.tokenLastRegex, yearStr)) {
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
                else if (!isFutureValid && !isPastValid) {
                    futureDate = pastDate = DateUtils.safeCreateFromMinValue(pastYear, pastMonth, day);
                }
                else {
                    futureDate = pastDate = DateUtils.safeCreateFromMinValue(year, month, day);
                }
            }
            else {
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
                        else if (DateUtils.isFeb29th(year, month - 1, day)) {
                            pastDate = DateUtils.addMonths(pastDate, -2);
                        }
                    }
                }
                else if (hasMonth && !hasYear) {
                    if (futureDate < referenceDate) {
                        if (DateUtils.isValidDate(year + 1, month, day)) {
                            futureDate = DateUtils.addYears(futureDate, 1);
                        }
                    }
                    if (pastDate >= referenceDate) {
                        if (DateUtils.isValidDate(year - 1, month, day)) {
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

            if (weekday === 0) {
                weekday = 7;
            }
            if (weekday < referenceDate.getDay()) {
                value = DateUtils.next(referenceDate, weekday);
            }
            result.timex = 'XXXX-WXX-' + weekday;
            let futureDate = new Date(value);
            let pastDate = new Date(value);
            if (futureDate < referenceDate) {
                futureDate = DateUtils.addDays(futureDate, 7);
            }
            if (pastDate >= referenceDate) {
                pastDate = DateUtils.addDays(pastDate, -7);
            }

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
                if (year < 100 && year >= Constants.MinTwoDigitYearPastNum) {
                    year += 1900;
                }
                else if (year >= 0 && year < Constants.MaxTwoDigitYearFutureNum) {
                    year += 2000;
                }
            }
        }
        let noYear = false;
        if (year === 0) {
            year = referenceDate.getFullYear();
            result.timex = DateTimeFormatUtil.luisDate(-1, month, day);
            noYear = true;
        }
        else {
            result.timex = DateTimeFormatUtil.luisDate(year, month, day);
        }

        let futurePastDates = DateUtils.generateDates(noYear, referenceDate, year, month, day);

        result.futureValue = futurePastDates.future;
        result.pastValue = futurePastDates.past;
        result.success = true;
        return result;
    }

    // convert Chinese Number to Integer
    private parseChineseWrittenNumberToValue(source: string): number {
        let num = -1;
        let er = this.config.integerExtractor.extract(source);
        if (er && er[0].type === NumberConstants.SYS_NUM_INTEGER) {
            num = Number.parseInt(this.config.numberParser.parse(er[0]).value);
        }

        return num;
    }

    private convertChineseYearToNumber(source: string): number {
        let year = 0;
        
        let dynastyYear = DateUtils.parseChineseDynastyYear(source, this.dynastyYearRegex, this.dynastyYearMap, this.dynastyStartYear, this.config.integerExtractor, this.config.numberParser);
        if (dynastyYear > 0) {
            return dynastyYear;
        }

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

        if (!DateUtils.isLeapYear(year) && month === 1) {
            maxDay -= 1;
        }

        return maxDay;
    }

    private isValidDate(year: number, month: number, day: number): boolean {
        if (month < Constants.MinMonth) {
            year--;
            month = Constants.MaxMonth;
        }

        if (month > Constants.MaxMonth) {
            year++;
            month = Constants.MinMonth;
        }
        return DateUtils.isValidDate(year, month, day);
    }

    // Handle cases like "三天前"
    protected parserDurationWithAgoAndLater(source: string, referenceDate: Date): DateTimeResolutionResult {
        let result = new DateTimeResolutionResult();
        let durationRes = this.durationExtractor.extract(source, referenceDate);

        if (durationRes) {
            let match = RegExpUtility.getMatches(this.config.unitRegex, source).pop();
            if (match) {
                let suffix = source.substring(durationRes[0].start + durationRes[0].length);
                let srcUnit = match.groups('unit').value;

                let numberStr = source.substring(durationRes[0].start, match.index - durationRes[0].start);
                let number =  this.parseChineseWrittenNumberToValue(numberStr);

                if (this.config.unitMap.has(srcUnit)) {
                    let unitStr = this.config.unitMap.get(srcUnit);

                    let beforeMatch = RegExpUtility.getMatches(ChineseDateExtractor.beforeRegex, suffix).pop();
                    if (beforeMatch && suffix.startsWith(beforeMatch.value)) {
                        let date : Date;
                        switch (unitStr) {
                            case Constants.TimexDay:
                                date = DateUtils.addDays(referenceDate, -number);
                                break;
                            case Constants.TimexWeek:
                                date = DateUtils.addDays(referenceDate, -7 * number);
                                break;
                            case Constants.TimexMonthFull:
                                date = DateUtils.addMonths(referenceDate, -number);
                                break;
                            case Constants.TimexYear:
                                date = DateUtils.addYears(referenceDate, -number);
                                break;
                            default:
                                return result;
                        }

                        result.timex = DateTimeFormatUtil.luisDateFromDate(date);
                        result.futureValue = result.pastValue = date;
                        result.success = true;
                        return result;
                    }

                    let afterMatch = RegExpUtility.getMatches(ChineseDateExtractor.afterRegex, suffix).pop();
                    if (afterMatch && suffix.startsWith(afterMatch.value)) {
                        let date: Date;
                        switch (unitStr) {
                            case Constants.TimexDay:
                                    date = DateUtils.addDays(referenceDate, number);
                                break;
                            case Constants.TimexWeek:
                                    date = DateUtils.addDays(referenceDate, 7 * number);
                                break;
                            case Constants.TimexMonthFull:
                                    date = DateUtils.addMonths(referenceDate, number);
                                break;
                            case Constants.TimexYear:
                                    date = DateUtils.addYears(referenceDate, number);
                                break;
                            default:
                                return result;
                        }

                        result.timex = DateTimeFormatUtil.luisDateFromDate(date);
                        result.futureValue = result.pastValue = date;
                        result.success = true;
                        return result;
                    }
                }

            }
        }

        return result;
    }
}