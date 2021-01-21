import { IExtractor, ExtractResult, RegExpUtility, Match, StringUtility } from "@microsoft/recognizers-text";
import { Constants, TimeTypeConstants } from "./constants";
import { BaseNumberExtractor, BaseNumberParser } from "@microsoft/recognizers-text-number";
import { Token, DateTimeFormatUtil, DateTimeResolutionResult, DateUtils, DayOfWeek, StringMap, AbstractYearExtractor, TimexUtil } from "./utilities";
import { BaseDurationExtractor, BaseDurationParser } from "./baseDuration";
import { IDateTimeParser, DateTimeParseResult } from "./parsers";
import { BaseDateExtractor, BaseDateParser } from "./baseDate";
import { IDateTimeExtractor } from "./baseDateTime";

export interface IDatePeriodExtractorConfiguration {
    simpleCasesRegexes: RegExp[]
    illegalYearRegex: RegExp
    YearRegex: RegExp
    tillRegex: RegExp
    followedUnit: RegExp
    numberCombinedWithUnit: RegExp
    pastRegex: RegExp
    futureRegex: RegExp
    weekOfRegex: RegExp
    monthOfRegex: RegExp
    dateUnitRegex: RegExp
    inConnectorRegex: RegExp
    rangeUnitRegex: RegExp
    nowRegex: RegExp
    datePointExtractor: IDateTimeExtractor
    integerExtractor: BaseNumberExtractor
    numberParser: BaseNumberParser
    durationExtractor: IDateTimeExtractor
    getFromTokenIndex(source: string): { matched: boolean, index: number };
    getBetweenTokenIndex(source: string): { matched: boolean, index: number };
    hasConnectorToken(source: string): boolean;
}

export class BaseDatePeriodExtractor implements IDateTimeExtractor {
    protected readonly extractorName = Constants.SYS_DATETIME_DATEPERIOD;
    protected readonly config: IDatePeriodExtractorConfiguration;

    constructor(config: IDatePeriodExtractorConfiguration) {
        this.config = config;
    }

    extract(source: string, refDate: Date): ExtractResult[] {
        if (!refDate) {
            refDate = new Date();
        }
        let referenceDate = refDate;

        let tokens: Token[] = new Array<Token>();
        tokens = tokens.concat(this.matchSimpleCases(source));
        tokens = tokens.concat(this.mergeTwoTimePoints(source, referenceDate));
        tokens = tokens.concat(this.matchDuration(source, referenceDate));
        tokens = tokens.concat(this.singleTimePointWithPatterns(source, referenceDate));
        let result = Token.mergeAllTokens(tokens, source, this.extractorName);
        return result;
    }

    protected matchSimpleCases(source: string): Token[] {
        let tokens: Token[] = new Array<Token>();
        this.config.simpleCasesRegexes.forEach(regexp => {
            RegExpUtility.getMatches(regexp, source).forEach(match => {
                let addToken = true;
                let matchYear = RegExpUtility.getMatches(this.config.YearRegex, match.value).pop();
                if (matchYear && matchYear.length === match.value.length) {
                    let yearStr = matchYear.groups('year').value;
                    if (StringUtility.isNullOrEmpty(yearStr)) {
                        let year = AbstractYearExtractor.getYearFromText(matchYear, this.config.numberParser);
                        if (!(year >= Constants.MinYearNum && year <= Constants.MaxYearNum)) {
                            addToken = false;
                        }
                    }
                }

                if (match.length === Constants.FourDigitsYearLength && RegExpUtility.isMatch(this.config.YearRegex, match.value) && this.infixBoundaryCheck(match, source)) {
                    let substr = source.substr(match.index - 1, 6);
                    if (RegExpUtility.isMatch(this.config.illegalYearRegex, substr)) {
                        addToken = false;
                    }
                }

                if (addToken) {
                    tokens.push(new Token(match.index, match.index + match.length));
                }
            });
        });
        return tokens;
    }

    protected mergeTwoTimePoints(source: string, refDate: Date): Token[] {
        let tokens: Token[] = new Array<Token>();
        let er = this.config.datePointExtractor.extract(source, refDate);
        if (er.length <= 1) {
            let matches = RegExpUtility.getMatches(this.config.nowRegex, source);
            if (matches.length) {
                let nowEr = new ExtractResult();
                nowEr.start = matches[0].index;
                nowEr.length = matches[0].length;
                er.push(nowEr);
                er = er.sort((x, y) => (x.start - y.start));
            }
            else {
                return tokens;
            }

        }

        let idx = 0;
        while (idx < er.length - 1) {
            let middleBegin = er[idx].start + (er[idx].length || 0);
            let middleEnd = er[idx + 1].start || 0;
            if (middleBegin >= middleEnd) {
                idx++;
                continue;
            }
            let middleStr = source.substr(middleBegin, middleEnd - middleBegin).trim().toLowerCase();
            let match = RegExpUtility.getMatches(this.config.tillRegex, middleStr);
            if (match && match.length > 0 && match[0].index === 0 && match[0].length === middleStr.length) {
                let periodBegin = er[idx].start || 0;
                let periodEnd = (er[idx + 1].start || 0) + (er[idx + 1].length || 0);

                let beforeStr = source.substring(0, periodBegin).trim().toLowerCase();
                let fromTokenIndex = this.config.getFromTokenIndex(beforeStr);
                let betweenTokenIndex = this.config.getBetweenTokenIndex(beforeStr);
                if (fromTokenIndex.matched || betweenTokenIndex.matched) {
                    periodBegin = fromTokenIndex.matched ? fromTokenIndex.index : betweenTokenIndex.index;
                }
                tokens.push(new Token(periodBegin, periodEnd));
                idx += 2;
                continue;
            }

            if (this.config.hasConnectorToken(middleStr)) {
                let periodBegin = er[idx].start || 0;
                let periodEnd = (er[idx + 1].start || 0) + (er[idx + 1].length || 0);

                let beforeStr = source.substring(0, periodBegin).trim().toLowerCase();
                let betweenTokenIndex = this.config.getBetweenTokenIndex(beforeStr);
                if (betweenTokenIndex.matched) {
                    periodBegin = betweenTokenIndex.index;
                    tokens.push(new Token(periodBegin, periodEnd));
                    idx += 2;
                    continue;
                }
            }
            idx++;
        }
        return tokens;
    }

    private matchDuration(source: string, refDate: Date): Token[] {
        let tokens: Token[] = new Array<Token>();
        let durations: Token[] = new Array<Token>();
        this.config.durationExtractor.extract(source, refDate).forEach(durationEx => {
            let match = RegExpUtility.getMatches(this.config.dateUnitRegex, durationEx.text).pop();
            if (match) {
                durations.push(new Token(durationEx.start, durationEx.start + durationEx.length));
            }
        });
        durations.forEach(duration => {
            let beforeStr = source.substring(0, duration.start).toLowerCase();
            if (StringUtility.isNullOrWhitespace(beforeStr)) {
                return;
            }
            let match = RegExpUtility.getMatches(this.config.pastRegex, beforeStr).pop();
            if (this.matchRegexInPrefix(beforeStr, match)) {
                tokens.push(new Token(match.index, duration.end));
                return;
            }
            match = RegExpUtility.getMatches(this.config.futureRegex, beforeStr).pop();
            if (this.matchRegexInPrefix(beforeStr, match)) {
                tokens.push(new Token(match.index, duration.end));
                return;
            }
            match = RegExpUtility.getMatches(this.config.inConnectorRegex, beforeStr).pop();
            if (this.matchRegexInPrefix(beforeStr, match)) {
                let rangeStr = source.substr(duration.start, duration.length);
                let rangeMatch = RegExpUtility.getMatches(this.config.rangeUnitRegex, rangeStr).pop();
                if (rangeMatch) {
                    tokens.push(new Token(match.index, duration.end));
                }
                return;
            }
        });
        return tokens;
    }

    private singleTimePointWithPatterns(source: string, refDate: Date): Token[] {
        let tokens: Token[] = new Array<Token>();
        let ers = this.config.datePointExtractor.extract(source, refDate);
        if (ers.length < 1) {
            return tokens;
        }
        ers.forEach(er => {
            if (er.start && er.length) {
                let beforeStr = source.substring(0, er.start);
                tokens = tokens
                    .concat(this.getTokenForRegexMatching(beforeStr, this.config.weekOfRegex, er))
                    .concat(this.getTokenForRegexMatching(beforeStr, this.config.monthOfRegex, er));
            }
        });
        return tokens;
    }

    private getTokenForRegexMatching(source: string, regexp: RegExp, er: ExtractResult): Token[] {
        let tokens: Token[] = new Array<Token>();
        let match = RegExpUtility.getMatches(regexp, source).shift();
        if (match && source.trim().endsWith(match.value.trim())) {
            let startIndex = source.lastIndexOf(match.value);
            tokens.push(new Token(startIndex, er.start + er.length));
        }
        return tokens;
    }

    private matchRegexInPrefix(source: string, match: Match): boolean {
        return (match && StringUtility.isNullOrWhitespace(source.substring(match.index + match.length)));
    }

    private infixBoundaryCheck(match: Match, source: string): boolean {
        let isMatchInfixOfSource = false;
        if (match.index > 0 && match.index + match.length < source.length) {
            if (source.substr(match.index, match.length) === match.value) {
                isMatchInfixOfSource = true;
            }
        }

        return isMatchInfixOfSource;
    }
}

export interface IDatePeriodParserConfiguration {
    dateExtractor: IDateTimeExtractor
    dateParser: BaseDateParser
    durationExtractor: IDateTimeExtractor
    durationParser: BaseDurationParser
    numberParser: BaseNumberParser
    monthFrontBetweenRegex: RegExp
    betweenRegex: RegExp
    monthFrontSimpleCasesRegex: RegExp
    simpleCasesRegex: RegExp
    oneWordPeriodRegex: RegExp
    monthWithYear: RegExp
    monthNumWithYear: RegExp
    yearRegex: RegExp
    relativeRegex: RegExp
    pastRegex: RegExp
    futureRegex: RegExp
    inConnectorRegex: RegExp
    weekOfMonthRegex: RegExp
    weekOfYearRegex: RegExp
    quarterRegex: RegExp
    quarterRegexYearFront: RegExp
    allHalfYearRegex: RegExp
    seasonRegex: RegExp
    weekOfRegex: RegExp
    monthOfRegex: RegExp
    whichWeekRegex: RegExp
    restOfDateRegex: RegExp
    laterEarlyPeriodRegex: RegExp
    weekWithWeekDayRangeRegex: RegExp
    unspecificEndOfRangeRegex: RegExp
    nowRegex: RegExp
    tokenBeforeDate: string
    dayOfMonth: ReadonlyMap<string, number>
    monthOfYear: ReadonlyMap<string, number>
    cardinalMap: ReadonlyMap<string, number>
    seasonMap: ReadonlyMap<string, string>
    unitMap: ReadonlyMap<string, string>
    getSwiftDayOrMonth(source: string): number
    getSwiftYear(source: string): number
    isFuture(source: string): boolean
    isYearToDate(source: string): boolean
    isMonthToDate(source: string): boolean
    isWeekOnly(source: string): boolean
    isWeekend(source: string): boolean
    isMonthOnly(source: string): boolean
    isYearOnly(source: string): boolean
    isLastCardinal(source: string): boolean
}

export class BaseDatePeriodParser implements IDateTimeParser {
    protected readonly parserName = Constants.SYS_DATETIME_DATEPERIOD;
    protected readonly config: IDatePeriodParserConfiguration;

    protected readonly inclusiveEndPeriod;
    private readonly weekOfComment = 'WeekOf';
    private readonly monthOfComment = 'MonthOf';

    constructor(config: IDatePeriodParserConfiguration, inclusiveEndPeriod = false) {
        this.config = config;
        this.inclusiveEndPeriod = inclusiveEndPeriod;
    }

    parse(extractorResult: ExtractResult, referenceDate?: Date): DateTimeParseResult | null {
        if (!referenceDate) {
            referenceDate = new Date();
        }
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
                innerResult = this.parseWeekOfMonth(source, referenceDate);
            }
            if (!innerResult.success) {
                innerResult = this.parseWeekOfYear(source, referenceDate);
            }
            if (!innerResult.success) {
                innerResult = this.parseHalfYear(source, referenceDate);
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

            // parse duration should be at the end since it will extract "the last week" from "the last week of July"
            if (!innerResult.success) {
                innerResult = this.parseDuration(source, referenceDate);
            }

            if (innerResult.success) {
                if (innerResult.futureValue && innerResult.pastValue) {

                    innerResult.futureResolution = {};
                    innerResult.futureResolution[TimeTypeConstants.START_DATE] = DateTimeFormatUtil.formatDate(innerResult.futureValue[0]);
                    innerResult.futureResolution[TimeTypeConstants.END_DATE] = DateTimeFormatUtil.formatDate(innerResult.futureValue[1]);
                    innerResult.pastResolution = {};
                    innerResult.pastResolution[TimeTypeConstants.START_DATE] = DateTimeFormatUtil.formatDate(innerResult.pastValue[0]);
                    innerResult.pastResolution[TimeTypeConstants.END_DATE] = DateTimeFormatUtil.formatDate(innerResult.pastValue[1]);

                }
                else {
                    innerResult.futureResolution = {};
                    innerResult.pastResolution = {};
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
        let match = RegExpUtility.getMatches(this.config.monthWithYear, trimmedSource).pop();
        if (!match) {
            match = RegExpUtility.getMatches(this.config.monthNumWithYear, trimmedSource).pop();
        }
        if (!match || match.length !== trimmedSource.length) {
            return result;
        }

        let monthStr = match.groups('month').value;
        let yearStr = match.groups('year').value;
        let orderStr = match.groups('order').value;

        let month = this.config.monthOfYear.get(monthStr) - 1;
        let year = Number.parseInt(yearStr, 10);
        if (!year || isNaN(year)) {
            let swift = this.config.getSwiftYear(orderStr);
            if (swift < -1) {
                return result;
            }
            year = referenceDate.getFullYear() + swift;
        }
        let beginDate = DateUtils.safeCreateFromValue(DateUtils.minValue(), year, month, 1);
        let endDate = DateUtils.addDays(DateUtils.addMonths(beginDate, 1), this.inclusiveEndPeriod ? -1 : 0);
        result.futureValue = [beginDate, endDate];
        result.pastValue = [beginDate, endDate];
        result.timex = `${DateTimeFormatUtil.toString(year, 4)}-${DateTimeFormatUtil.toString(month + 1, 2)}`;
        result.success = true;
        return result;
    }

    protected getMatchSimpleCase(source: string): Match {
        let match = RegExpUtility.getMatches(this.config.monthFrontBetweenRegex, source).pop();
        if (!match) {
            match = RegExpUtility.getMatches(this.config.betweenRegex, source).pop();
        }
        if (!match) {
            match = RegExpUtility.getMatches(this.config.monthFrontSimpleCasesRegex, source).pop();
        }
        if (!match) {
            match = RegExpUtility.getMatches(this.config.simpleCasesRegex, source).pop();
        }
        return match;
    }

    protected parseSimpleCases(source: string, referenceDate: Date): DateTimeResolutionResult {
        let result = new DateTimeResolutionResult();
        let year = referenceDate.getFullYear();
        let month = referenceDate.getMonth();
        let noYear = true;

        let match = this.getMatchSimpleCase(source);

        if (!match || match.index !== 0 || match.length !== source.length) {
            return result;
        }
        let days = match.groups('day');
        let beginDay = this.config.dayOfMonth.get(days.captures[0]);
        let endDay = this.config.dayOfMonth.get(days.captures[1]);
        let yearStr = match.groups('year').value;
        if (!StringUtility.isNullOrEmpty(yearStr)) {
            year = Number.parseInt(yearStr, 10);
            noYear = false;
        }
        let monthStr = match.groups('month').value;
        if (!StringUtility.isNullOrEmpty(monthStr)) {
            month = this.config.monthOfYear.get(monthStr) - 1;
        }
        else {
            monthStr = match.groups('relmonth').value;
            month += this.config.getSwiftDayOrMonth(monthStr);
            if (month < 0) {
                month = 0;
                year--;
            }
            else if (month > 11) {
                month = 11;
                year++;
            }

            if (this.config.isFuture(monthStr)) {
                noYear = false;
            }
        }
        let beginDateLuis = DateTimeFormatUtil.luisDate(noYear ? -1 : year, month, beginDay);
        let endDateLuis = DateTimeFormatUtil.luisDate(noYear ? -1 : year, month, endDay);

        let futureYear = year;
        let pastYear = year;
        let startDate = DateUtils.safeCreateFromValue(DateUtils.minValue(), year, month, beginDay);
        if (noYear && startDate < referenceDate) {
            futureYear++;
        }
        if (noYear && startDate >= referenceDate) {
            pastYear--;
        }

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

    private isPresent(swift: number): boolean {
        return swift === 0;
    }

    protected parseOneWordPeriod(source: string, referenceDate: Date): DateTimeResolutionResult {
        let result = new DateTimeResolutionResult();
        let year = referenceDate.getFullYear();
        let month = referenceDate.getMonth();
        let earlyPrefix = false;
        let latePrefix = false;
        let midPrefix = false;
        let isRef = false;

        let earlierPrefix = false;
        let laterPrefix = false;

        if (this.config.isYearToDate(source)) {
            result.timex = DateTimeFormatUtil.toString(year, 4);
            result.futureValue = [DateUtils.safeCreateFromValue(DateUtils.minValue(), year, 0, 1), referenceDate];
            result.pastValue = [DateUtils.safeCreateFromValue(DateUtils.minValue(), year, 0, 1), referenceDate];
            result.success = true;
            return result;
        }
        if (this.config.isMonthToDate(source)) {
            result.timex = `${DateTimeFormatUtil.toString(year, 4)}-${DateTimeFormatUtil.toString(month + 1, 2)}`;
            result.futureValue = [DateUtils.safeCreateFromValue(DateUtils.minValue(), year, month, 1), referenceDate];
            result.pastValue = [DateUtils.safeCreateFromValue(DateUtils.minValue(), year, month, 1), referenceDate];
            result.success = true;
            return result;
        }

        let futureYear = year;
        let pastYear = year;
        let trimedText = source.trim().toLowerCase();
        let match = RegExpUtility.getMatches(this.config.oneWordPeriodRegex, trimedText).pop();

        if (!(match && match.index === 0 && match.length === trimedText.length)) {
            match = RegExpUtility.getMatches(this.config.laterEarlyPeriodRegex, trimedText).pop();
        }

        if (!match || match.index !== 0 || match.length !== trimedText.length) {
            return result;
        }

        if (match.groups("EarlyPrefix").value) {
            earlyPrefix = true;
            trimedText = match.groups("suffix").value;
            result.mod = Constants.EARLY_MOD;
        }

        if (match.groups("LatePrefix").value) {
            latePrefix = true;
            trimedText = match.groups("suffix").value;
            result.mod = Constants.LATE_MOD;
        }

        if (match.groups("MidPrefix").value) {
            latePrefix = true;
            trimedText = match.groups("suffix").value;
            result.mod = Constants.MID_MOD;
        }

        let monthStr = match.groups('month').value;
        let swift = 0;
        if (!StringUtility.isNullOrEmpty(monthStr)) {
            swift = this.config.getSwiftYear(trimedText);
        }
        else {
            swift = this.config.getSwiftDayOrMonth(trimedText);
        }

        if (RegExpUtility.isMatch(this.config.unspecificEndOfRangeRegex, match.value)) {
            latePrefix = true;
            trimedText = match.value;
            result.mod = Constants.LATE_MOD;
        }

        if (match.groups("RelEarly").value) {
            earlierPrefix = true;
            if (this.isPresent(swift)) {
                result.mod = null;
            }
        }

        if (match.groups("RelLate").value) {
            laterPrefix = true;
            if (this.isPresent(swift)) {
                result.mod = null;
            }
        }

        if (!StringUtility.isNullOrEmpty(monthStr)) {
            swift = this.config.getSwiftYear(trimedText);
            month = this.config.monthOfYear.get(monthStr) - 1;
            if (swift >= -1) {
                result.timex = `${DateTimeFormatUtil.toString(year + swift, 4)}-${DateTimeFormatUtil.toString(month + 1, 2)}`;
                year += swift;
                futureYear = year;
                pastYear = year;
            }
            else {
                result.timex = `XXXX-${DateTimeFormatUtil.toString(month + 1, 2)}`;
                if (month < referenceDate.getMonth()) {
                    futureYear++;
                }
                if (month >= referenceDate.getMonth()) {
                    pastYear--;
                }
            }
        }
        else {
            swift = this.config.getSwiftDayOrMonth(trimedText);
            if (this.config.isWeekOnly(trimedText)) {
                let monday = DateUtils.addDays(DateUtils.this(referenceDate, DayOfWeek.Monday), 7 * swift);
                let weekNumber = DateUtils.getWeekNumber(monday);

                result.timex = `${DateTimeFormatUtil.toString(weekNumber.year, 4)}-W${DateTimeFormatUtil.toString(weekNumber.weekNo, 2)}`;

                let beginDate = DateUtils.addDays(DateUtils.this(referenceDate, DayOfWeek.Monday), 7 * swift);
                let endDate = this.inclusiveEndPeriod
                    ? DateUtils.addDays(DateUtils.this(referenceDate, DayOfWeek.Sunday), 7 * swift)
                    : DateUtils.addDays(
                        DateUtils.addDays(DateUtils.this(referenceDate, DayOfWeek.Sunday), 7 * swift), 1);

                if (earlyPrefix) {
                    endDate = this.inclusiveEndPeriod
                        ? DateUtils.addDays(DateUtils.this(referenceDate, DayOfWeek.Wednesday), 7 * swift)
                        : DateUtils.addDays(
                            DateUtils.addDays(DateUtils.this(referenceDate, DayOfWeek.Wednesday), 7 * swift), 1);
                }

                if (latePrefix) {
                    beginDate = DateUtils.addDays(DateUtils.this(referenceDate, DayOfWeek.Thursday), 7 * swift);
                }

                if (earlierPrefix && swift === 0) {
                    if (endDate > referenceDate) {
                        endDate = referenceDate;
                    }
                }
                else if (laterPrefix && swift === 0) {
                    if (beginDate < referenceDate) {
                        beginDate = referenceDate;
                    }
                }

                result.futureValue = [beginDate, endDate];
                result.pastValue = [beginDate, endDate];
                result.success = true;
                return result;
            }
            if (this.config.isWeekend(trimedText)) {
                let beginDate = DateUtils.addDays(DateUtils.this(referenceDate, DayOfWeek.Saturday), 7 * swift);
                let endDate = DateUtils.addDays(DateUtils.this(referenceDate, DayOfWeek.Sunday), (7 * swift) + (this.inclusiveEndPeriod ? 0 : 1));

                result.timex = `${DateTimeFormatUtil.toString(beginDate.getFullYear(), 4)}-W${DateTimeFormatUtil.toString(DateUtils.getWeekNumber(beginDate).weekNo, 2)}-WE`;
                result.futureValue = [beginDate, endDate];
                result.pastValue = [beginDate, endDate];
                result.success = true;
                return result;
            }
            if (this.config.isMonthOnly(trimedText)) {
                let tempDate = new Date(referenceDate);
                tempDate.setMonth(referenceDate.getMonth() + swift);
                month = tempDate.getMonth();
                year = tempDate.getFullYear();
                result.timex = `${DateTimeFormatUtil.toString(year, 4)}-${DateTimeFormatUtil.toString(month + 1, 2)}`;
                futureYear = year;
                pastYear = year;
            }
            else if (this.config.isYearOnly(trimedText)) {
                let tempDate = new Date(referenceDate);
                tempDate.setFullYear(referenceDate.getFullYear() + swift);
                year = tempDate.getFullYear();
                let beginDate = DateUtils.safeCreateFromMinValue(year, 0, 1);
                let endDate = this.inclusiveEndPeriod
                    ? DateUtils.safeCreateFromMinValue(year, 11, 31)
                    : DateUtils.addDays(
                        DateUtils.safeCreateFromMinValue(year, 11, 31), 1);
                if (earlyPrefix) {
                    endDate = this.inclusiveEndPeriod
                        ? DateUtils.safeCreateFromMinValue(year, 5, 30)
                        : DateUtils.addDays(
                            DateUtils.safeCreateFromMinValue(year, 5, 30), 1);
                }
                if (latePrefix) {
                    beginDate = DateUtils.safeCreateFromMinValue(year, 6, 1);
                }

                if (earlierPrefix && swift === 0) {
                    if (endDate > referenceDate) {
                        endDate = referenceDate;
                    }
                }
                else if (laterPrefix && swift === 0) {
                    if (beginDate < referenceDate) {
                        beginDate = referenceDate;
                    }
                }

                result.timex = DateTimeFormatUtil.toString(year, 4);
                result.futureValue = [beginDate, endDate];
                result.pastValue = [beginDate, endDate];
                result.success = true;
                return result;
            }
        }

        // only "month" will come to here
        let futureStart = DateUtils.safeCreateFromMinValue(futureYear, month, 1);
        let futureEnd = this.inclusiveEndPeriod
            ? DateUtils.addDays(
                DateUtils.addMonths(
                    DateUtils.safeCreateFromMinValue(futureYear, month, 1), 1), -1)
            : DateUtils.addMonths(
                DateUtils.safeCreateFromMinValue(futureYear, month, 1), 1);
        let pastStart = DateUtils.safeCreateFromMinValue(pastYear, month, 1);
        let pastEnd = this.inclusiveEndPeriod
            ? DateUtils.addDays(
                DateUtils.addMonths(
                    DateUtils.safeCreateFromMinValue(pastYear, month, 1), 1), -1)
            : DateUtils.addMonths(
                DateUtils.safeCreateFromMinValue(pastYear, month, 1), 1);
        if (earlyPrefix) {
            futureEnd = this.inclusiveEndPeriod
                ? DateUtils.safeCreateFromMinValue(futureYear, month, 15)
                : DateUtils.addDays(
                    DateUtils.safeCreateFromMinValue(futureYear, month, 15), 1);
            pastEnd = this.inclusiveEndPeriod
                ? DateUtils.safeCreateFromMinValue(pastYear, month, 15)
                : DateUtils.addDays(
                    DateUtils.safeCreateFromMinValue(pastYear, month, 15), 1);
        }
        else if (latePrefix) {
            futureStart = DateUtils.safeCreateFromMinValue(futureYear, month, 16);
            pastStart = DateUtils.safeCreateFromMinValue(pastYear, month, 16);
        }

        if (earlierPrefix && futureYear === pastYear) {
            if (futureEnd > referenceDate) {
                futureEnd = pastEnd = referenceDate;
            }
        }
        else if (laterPrefix && futureYear === pastYear) {
            if (futureStart < referenceDate) {
                futureStart = pastStart = referenceDate;
            }
        }

        result.futureValue = [futureStart, futureEnd];
        result.pastValue = [pastStart, pastEnd];
        result.success = true;
        return result;
    }

    protected mergeTwoTimePoints(source: string, referenceDate: Date): DateTimeResolutionResult {
        let trimmedSource = source.trim();
        let result = new DateTimeResolutionResult();
        let ers = this.config.dateExtractor.extract(trimmedSource, referenceDate);
        let prs: DateTimeParseResult[] = [];
        if (!ers || ers.length < 2) {
            ers = this.config.dateExtractor.extract(this.config.tokenBeforeDate + trimmedSource, referenceDate);
            if (ers.length >= 2) {
                ers = ers.map(er => {
                    er.start -= this.config.tokenBeforeDate.length;
                    return er;
                });
            }
            else {
                let nowPr = this.parseNowAsDate(source, referenceDate);
                if (!nowPr || !nowPr.start || ers.length < 1) {
                    return result;
                }
                let dataPr = this.config.dateParser.parse(ers[0], referenceDate);
                prs.push(dataPr);
                prs.push(nowPr);
                prs = prs.sort((x, y) => (x.start - y.start));
            }
        }

        if (ers.length >= 2) {
            let match = RegExpUtility.getMatches(this.config.weekWithWeekDayRangeRegex, source).pop();
            let weekPrefix: string = null;
            if (match) {
                weekPrefix = match.groups("week").value;
            }

            if (!StringUtility.isNullOrWhitespace(weekPrefix)) {
                ers[0].text = weekPrefix + " " + ers[0].text;
                ers[1].text = weekPrefix + " " + ers[1].text;
            }

            prs = ers.map(er => this.config.dateParser.parse(er, referenceDate)).filter(pr => pr);
        }

        if (prs.length < 2) {
            return result;
        }

        let prBegin = prs[0];
        let prEnd = prs[1];
        
        if (ers.length >= 2) {
            let matchYear = DateUtils.getYear(this.config, ers[0].text, ers[1].text, source);
            if (matchYear != -1) {
                prBegin = DateUtils.processDateEntityParsingResult(prBegin, matchYear);
                prEnd = DateUtils.processDateEntityParsingResult(prEnd, matchYear);
            }

            // When the case has no specified year, we should sync the future/past year due to invalid date Feb 29th.
            if (matchYear == -1 && (DateUtils.isFeb29thDate(prBegin.value.futureValue) || DateUtils.isFeb29thDate(prEnd.value.futureValue))) {
                let pastFuture = DateUtils.syncYear(prBegin, prEnd);
                prBegin = pastFuture.pr1;
                prEnd = pastFuture.pr2;
            }
        }

        let futureBegin = prBegin.value.futureValue;
        let futureEnd = prEnd.value.futureValue;
        let pastBegin = prBegin.value.pastValue;
        let pastEnd = prEnd.value.pastValue;

        if (futureBegin > futureEnd) {
            futureBegin = pastBegin;
        }

        if (pastEnd < pastBegin) {
            pastEnd = futureEnd;
        }

        result.subDateTimeEntities = prs;
        result.timex = TimexUtil.generateDatePeriodTimex(futureBegin, futureEnd, Constants.ByDay, prBegin.timexStr, prEnd.timexStr);
        if (prBegin.timexStr.startsWith(Constants.TimexFuzzyYear) && futureBegin <= DateUtils.safeCreateFromMinValue(futureBegin.getFullYear(), 1, 28) && futureEnd >= DateUtils.safeCreateFromMinValue(futureBegin.getFullYear(), 2, 1)) {
            // Handle cases like "2月28日到3月1日".
            // There may be different timexes for FutureValue and PastValue due to the different validity of Feb 29th.
            result.comment = Constants.Comment_DoubleTimex;
            let pastTimex = TimexUtil.generateDatePeriodTimex(pastBegin, pastEnd, Constants.ByDay, prBegin.timexStr, prEnd.timexStr);
            result.timex = TimexUtil.mergeTimexAlternatives(result.timex, pastTimex);
        }

        result.futureValue = [futureBegin, futureEnd];
        result.pastValue = [pastBegin, pastEnd];
        result.success = true;
        return result;
    }

    protected parseNowAsDate(source: string, referenceDate: Date): DateTimeParseResult {
        let pr = new DateTimeParseResult();
        let matches = RegExpUtility.getMatches(this.config.nowRegex, source);
        if (matches.length) {
            let value = DateUtils.safeCreateFromMinValue(referenceDate.getFullYear(), referenceDate.getMonth(), referenceDate.getDate());
            let retNow = new DateTimeResolutionResult();
            retNow.timex = DateTimeFormatUtil.luisDateFromDate(value);
            retNow.futureValue = value;
            retNow.pastValue = value;
            pr.text = matches[0].value;
            pr.start = matches[0].index;
            pr.length = matches[0].length;
            pr.value = retNow;
            pr.timexStr = retNow.timex;
        }
        return pr;
    }

    protected parseYear(source: string, referenceDate: Date): DateTimeResolutionResult {
        let trimmedSource = source.trim();
        let result = new DateTimeResolutionResult();
        let match = RegExpUtility.getMatches(this.config.yearRegex, trimmedSource).pop();
        if (!match || match.length !== trimmedSource.length) {
            return result;
        }

        let year = AbstractYearExtractor.getYearFromText(match, this.config.numberParser);
        let beginDate = DateUtils.safeCreateFromValue(DateUtils.minValue(), year, 0, 1);
        let endDate = DateUtils.addDays(DateUtils.safeCreateFromValue(DateUtils.minValue(), year + 1, 0, 1), this.inclusiveEndPeriod ? -1 : 0);
        result.timex = DateTimeFormatUtil.toString(year, 4);
        result.futureValue = [beginDate, endDate];
        result.pastValue = [beginDate, endDate];
        result.success = true;
        return result;
    }

    protected parseDuration(source: string, referenceDate: Date): DateTimeResolutionResult {
        let result = new DateTimeResolutionResult();
        let ers = this.config.durationExtractor.extract(source, referenceDate);
        let beginDate = new Date(referenceDate);
        let endDate = new Date(referenceDate);
        let restNowSunday = false;
        let durationTimex = '';

        if (ers.length === 1) {
            let pr = this.config.durationParser.parse(ers[0]);
            if (pr === null) {
                return result;
            }

            let isMatch = false;
            let beforeStr = source.substr(0, pr.start).trim();
            let mod: string;
            let durationResult: DateTimeResolutionResult = pr.value;
            if (StringUtility.isNullOrEmpty(durationResult.timex)) {
                return result;
            }

            let prefixMatch = RegExpUtility.getMatches(this.config.pastRegex, beforeStr).pop();
            if (prefixMatch) {
                mod = TimeTypeConstants.beforeMod;
                beginDate = this.getSwiftDate(endDate, durationResult.timex, false);
            }
            prefixMatch = RegExpUtility.getMatches(this.config.futureRegex, beforeStr).pop();
            if (prefixMatch && prefixMatch.length === beforeStr.length) {
                mod = TimeTypeConstants.afterMod;
                // for future the beginDate should add 1 first
                beginDate = new Date(referenceDate.getFullYear(), referenceDate.getMonth(), referenceDate.getDate() + 1);
                endDate = this.getSwiftDate(beginDate, durationResult.timex, true);
                isMatch = true;
            }
            prefixMatch = RegExpUtility.getMatches(this.config.inConnectorRegex, beforeStr).pop();
            if (prefixMatch && prefixMatch.length === beforeStr.length && !isMatch) {
                mod = TimeTypeConstants.afterMod;
                beginDate = new Date(referenceDate.getFullYear(), referenceDate.getMonth(), referenceDate.getDate() + 1);
                endDate = this.getSwiftDate(beginDate, durationResult.timex, true);

                let unit = durationResult.timex.substr(durationResult.timex.length - 1);
                durationResult.timex = `P1${unit}`;
                beginDate = this.getSwiftDate(endDate, durationResult.timex, false);
            }

            if (mod) {
                pr.value.mod = mod;
            }

            durationTimex = durationResult.timex;
            result.subDateTimeEntities = [pr];
        }

        // parse rest of
        let match = RegExpUtility.getMatches(this.config.restOfDateRegex, source).pop();
        if (match) {

            let diffDays = 0;
            let durationStr = match.groups('duration').value;
            let durationUnit = this.config.unitMap.get(durationStr);
            switch (durationUnit) {
                case 'W':
                    diffDays = 7 - ((beginDate.getDay() === 0) ? 7 : beginDate.getDay());
                    endDate = DateUtils.addDays(referenceDate, diffDays);
                    restNowSunday = (diffDays === 0);
                    break;
                case 'MON':
                    endDate = DateUtils.safeCreateFromMinValue(beginDate.getFullYear(), beginDate.getMonth(), 1);
                    endDate.setMonth(beginDate.getMonth() + 1);
                    endDate.setDate(endDate.getDate() - 1);
                    diffDays = endDate.getDate() - beginDate.getDate() + 1;
                    break;
                case 'Y':
                    endDate = DateUtils.safeCreateFromMinValue(beginDate.getFullYear(), 11, 1);
                    endDate.setMonth(endDate.getMonth() + 1);
                    endDate.setDate(endDate.getDate() - 1);
                    diffDays = DateUtils.dayOfYear(endDate) - DateUtils.dayOfYear(beginDate) + 1;
                    break;
            }
            durationTimex = `P${diffDays}D`;
        }

        if (beginDate.getTime() !== endDate.getTime() || restNowSunday) {
            endDate = DateUtils.addDays(endDate, this.inclusiveEndPeriod ? -1 : 0);
            result.timex = `(${DateTimeFormatUtil.luisDateFromDate(beginDate)},${DateTimeFormatUtil.luisDateFromDate(endDate)},${durationTimex})`;
            result.futureValue = [beginDate, endDate];
            result.pastValue = [beginDate, endDate];
            result.success = true;
        }

        return result;
    }

    private getSwiftDate(date: Date, timex: string, isPositiveSwift: boolean): Date {
        let result = new Date(date);
        let numStr = timex.replace('P', '').substr(0, timex.length - 2);
        let unitStr = timex.substr(timex.length - 1);
        let swift = Number.parseInt(numStr, 10) || 0;
        if (swift === 0) {
            return result;
        }

        if (!isPositiveSwift) {
            swift *= -1;
        }
        switch (unitStr) {
            case 'D': result.setDate(date.getDate() + swift); break;
            case 'W': result.setDate(date.getDate() + (7 * swift)); break;
            case 'M': result.setMonth(date.getMonth() + swift); break;
            case 'Y': result.setFullYear(date.getFullYear() + swift); break;
        }
        return result;
    }

    protected parseWeekOfMonth(source: string, referenceDate: Date): DateTimeResolutionResult {
        let result = new DateTimeResolutionResult();
        let match = RegExpUtility.getMatches(this.config.weekOfMonthRegex, source).pop();
        if (!match || match.length !== source.length) {
            return result;
        }

        let cardinalStr = match.groups('cardinal').value;
        let monthStr = match.groups('month').value;
        let month = referenceDate.getMonth();
        let year = referenceDate.getFullYear();
        let noYear = false;
        let cardinal = this.config.isLastCardinal(cardinalStr) ? 5
            : this.config.cardinalMap.get(cardinalStr);
        if (StringUtility.isNullOrEmpty(monthStr)) {
            let swift = this.config.getSwiftDayOrMonth(source);
            let tempDate = new Date(referenceDate);
            tempDate.setMonth(referenceDate.getMonth() + swift);
            month = tempDate.getMonth();
            year = tempDate.getFullYear();
        }
        else {
            month = this.config.monthOfYear.get(monthStr) - 1;
            noYear = true;
        }
        return this.getWeekOfMonth(cardinal, month, year, referenceDate, noYear);
    }

    protected getWeekOfMonth(cardinal: number, month: number, year: number, referenceDate: Date, noYear: boolean): DateTimeResolutionResult {
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
            `XXXX-${DateTimeFormatUtil.toString(month + 1, 2)}-W${DateTimeFormatUtil.toString(cardinal, 2)}` :
            `${DateTimeFormatUtil.toString(year, 4)}-${DateTimeFormatUtil.toString(month + 1, 2)}-W${DateTimeFormatUtil.toString(cardinal, 2)}`;
        result.futureValue = [futureDate, DateUtils.addDays(futureDate, this.inclusiveEndPeriod ? 6 : 7)];
        result.pastValue = [pastDate, DateUtils.addDays(pastDate, this.inclusiveEndPeriod ? 6 : 7)];
        result.success = true;
        return result;
    }

    private parseWeekOfYear(source: string, referenceDate: Date): DateTimeResolutionResult {
        let result = new DateTimeResolutionResult();
        let match = RegExpUtility.getMatches(this.config.weekOfYearRegex, source).pop();
        if (!match || match.length !== source.length) {
            return result;
        }

        let cardinalStr = match.groups('cardinal').value;
        let yearStr = match.groups('year').value;
        let orderStr = match.groups('order').value;

        let year = Number.parseInt(yearStr, 10);
        if (isNaN(year)) {
            let swift = this.config.getSwiftYear(orderStr);
            if (swift < -1) {
                return result;
            }
            year = referenceDate.getFullYear() + swift;
        }

        let targetWeekMonday: Date;
        if (this.config.isLastCardinal(cardinalStr)) {
            let lastDay = DateUtils.safeCreateFromMinValue(year, 11, 31);
            let lastDayWeekMonday = DateUtils.this(lastDay, DayOfWeek.Monday);
            let weekNum = DateUtils.getWeekNumber(lastDay).weekNo;
            if (weekNum === 1) {
                lastDayWeekMonday = DateUtils.this(DateUtils.addDays(lastDay, -7), DayOfWeek.Monday);
            }

            targetWeekMonday = lastDayWeekMonday;
            weekNum = DateUtils.getWeekNumber(targetWeekMonday).weekNo;

            result.timex = `${DateTimeFormatUtil.toString(year, 4)}-W${DateTimeFormatUtil.toString(weekNum, 2)}`;
        }
        else {
            let cardinal = this.config.cardinalMap.get(cardinalStr);

            let firstDay = DateUtils.safeCreateFromMinValue(year, 0, 1);
            let firstDayWeekMonday = DateUtils.this(firstDay, DayOfWeek.Monday);
            let weekNum = DateUtils.getWeekNumber(firstDay).weekNo;
            if (weekNum !== 1) {
                firstDayWeekMonday = DateUtils.this(DateUtils.addDays(firstDay, 7), DayOfWeek.Monday);
            }

            targetWeekMonday = DateUtils.addDays(firstDayWeekMonday, 7 * (cardinal - 1));
            result.timex = `${DateTimeFormatUtil.toString(year, 4)}-W${DateTimeFormatUtil.toString(cardinal, 2)}`;
        }

        result.futureValue = [targetWeekMonday, DateUtils.addDays(targetWeekMonday, this.inclusiveEndPeriod ? 6 : 7)];
        result.pastValue = [targetWeekMonday, DateUtils.addDays(targetWeekMonday, this.inclusiveEndPeriod ? 6 : 7)];
        result.success = true;

        return result;
    }

    protected parseHalfYear(source: string, referenceDate: Date): DateTimeResolutionResult {
        let result = new DateTimeResolutionResult();
        let match = RegExpUtility.getMatches(this.config.allHalfYearRegex, source).pop();
        if (!match || match.length !== source.length) {
            return result;
        }

        let cardinalStr = match.groups('cardinal').value;
        let yearStr = match.groups('year').value;
        let orderStr = match.groups('order').value;
        let numberStr = match.groups('number').value;

        let year = Number.parseInt(yearStr, 10);

        if (isNaN(year)) {
            let swift = this.config.getSwiftYear(orderStr);
            if (swift < -1) {
                return result;
            }
            year = referenceDate.getFullYear() + swift;
        }

        let quarterNum: number;
        if (!numberStr) {
            quarterNum = this.config.cardinalMap.get(cardinalStr);
        }
        else {
            quarterNum = parseInt(numberStr);
        }

        let beginDate = DateUtils.safeCreateDateResolveOverflow(year, (quarterNum - 1) * Constants.SemesterMonthCount, 1);
        let endDate = DateUtils.safeCreateDateResolveOverflow(year, quarterNum * Constants.SemesterMonthCount, 1);

        result.futureValue = [beginDate, endDate];
        result.pastValue = [beginDate, endDate];
        result.timex = `(${DateTimeFormatUtil.luisDateFromDate(beginDate)},${DateTimeFormatUtil.luisDateFromDate(endDate)},P6M)`;
        result.success = true;
        return result;
    }

    protected parseQuarter(source: string, referenceDate: Date): DateTimeResolutionResult {
        let result = new DateTimeResolutionResult();
        let match = RegExpUtility.getMatches(this.config.quarterRegex, source).pop();
        if (!match || match.length !== source.length) {
            match = RegExpUtility.getMatches(this.config.quarterRegexYearFront, source).pop();
        }
        if (!match || match.length !== source.length) {
            return result;
        }

        let cardinalStr = match.groups('cardinal').value;
        let yearStr = match.groups('year').value;
        let orderQuarterStr = match.groups('orderQuarter').value;
        let orderStr = StringUtility.isNullOrEmpty(orderQuarterStr) ? match.groups('order').value : '';
        let numberStr = match.groups('number').value;

        let noSpecificYear = false;
        let year = Number.parseInt(yearStr, 10);

        if (isNaN(year)) {
            let swift = StringUtility.isNullOrEmpty(orderQuarterStr) ? this.config.getSwiftYear(orderStr) : 0;
            if (swift < -1) {
                swift = 0;
                noSpecificYear = true;
            }
            year = referenceDate.getFullYear() + swift;
        }

        let quarterNum: number;
        if (!StringUtility.isNullOrEmpty(cardinalStr)) {
            quarterNum = this.config.cardinalMap.get(cardinalStr);
        }
        else if (!StringUtility.isNullOrEmpty(orderQuarterStr)) {
            let month = referenceDate.getMonth() + 1;
            quarterNum = Math.ceil(month / Constants.TrimesterMonthCount);
            let swift = this.config.getSwiftYear(orderQuarterStr);
            quarterNum += swift;
            if (quarterNum <= 0) {
                quarterNum += Constants.QuarterCount;
                year -= 1;
            }
            else if (quarterNum > Constants.QuarterCount) {
                quarterNum -= Constants.QuarterCount;
                year += 1;
            }
        }
        else {
            quarterNum = parseInt(numberStr);
        }

        let beginDate = DateUtils.safeCreateDateResolveOverflow(year, (quarterNum - 1) * Constants.TrimesterMonthCount, 1);
        let endDate = DateUtils.safeCreateDateResolveOverflow(year, quarterNum * Constants.TrimesterMonthCount, 1);

        if (noSpecificYear) {
            if (endDate < referenceDate) {
                result.pastValue = [beginDate, endDate];

                let futureBeginDate = DateUtils.safeCreateDateResolveOverflow(year + 1, (quarterNum - 1) * Constants.TrimesterMonthCount, 1);
                let futureEndDate = DateUtils.safeCreateDateResolveOverflow(year + 1, quarterNum * Constants.TrimesterMonthCount, 1);
                result.futureValue = [futureBeginDate, futureEndDate];
            }
            else if (endDate > referenceDate) {
                result.futureValue = [beginDate, endDate];

                let pastBeginDate = DateUtils.safeCreateDateResolveOverflow(year - 1, (quarterNum - 1) * Constants.TrimesterMonthCount, 1);
                let pastEndDate = DateUtils.safeCreateDateResolveOverflow(year - 1, quarterNum * Constants.TrimesterMonthCount, 1);
                result.pastValue = [pastBeginDate, pastEndDate];
            }
            else {
                result.futureValue = [beginDate, endDate];
                result.pastValue = [beginDate, endDate];
            }

            result.timex = `(${DateTimeFormatUtil.luisDate(-1, beginDate.getMonth(), 1)},${DateTimeFormatUtil.luisDate(-1, endDate.getMonth(), 1)},P3M)`;
        }
        else {
            result.futureValue = [beginDate, endDate];
            result.pastValue = [beginDate, endDate];
            result.timex = `(${DateTimeFormatUtil.luisDateFromDate(beginDate)},${DateTimeFormatUtil.luisDateFromDate(endDate)},P3M)`;
        }

        result.success = true;
        return result;
    }

    protected parseSeason(source: string, referenceDate: Date): DateTimeResolutionResult {
        let result = new DateTimeResolutionResult();
        let match = RegExpUtility.getMatches(this.config.seasonRegex, source).pop();
        if (!match || match.length !== source.length) {
            return result;
        }

        let swift = this.config.getSwiftYear(source);
        let yearStr = match.groups('year').value;
        let year = referenceDate.getFullYear();
        let seasonStr = match.groups('seas').value;
        let season = this.config.seasonMap.get(seasonStr);
        if (swift >= -1 || !StringUtility.isNullOrEmpty(yearStr)) {
            if (StringUtility.isNullOrEmpty(yearStr)) {
                yearStr = DateTimeFormatUtil.toString(year + swift, 4);
            }
            result.timex = `${yearStr}-${season}`;
        }
        else {
            result.timex = season;
        }
        result.success = true;
        return result;
    }

    private parseWhichWeek(source: string, referenceDate: Date): DateTimeResolutionResult {
        let result = new DateTimeResolutionResult();
        let match = RegExpUtility.getMatches(this.config.whichWeekRegex, source).pop();
        if (!match) {
            return result;
        }
        let num = Number.parseInt(match.groups('number').value, 10);
        let year = referenceDate.getFullYear();
        result.timex = `${DateTimeFormatUtil.toString(year, 4)}-W${DateTimeFormatUtil.toString(num, 2)}`;

        let firstDay = DateUtils.safeCreateFromValue(DateUtils.minValue(), year, 0, 1);
        let firstThursday = DateUtils.this(firstDay, DayOfWeek.Thursday);
        let firstWeek = DateUtils.getWeekNumber(firstThursday).weekNo;
        if (firstWeek === 1) {
            num -= 1;
        }

        let resultDate = DateUtils.addDays(firstThursday, 7 * num - 3);

        result.futureValue = [resultDate, DateUtils.addDays(resultDate, 7)];
        result.pastValue = [resultDate, DateUtils.addDays(resultDate, 7)];
        result.success = true;
        return result;
    }

    private parseWeekOfDate(source: string, referenceDate: Date): DateTimeResolutionResult {
        let result = new DateTimeResolutionResult();
        let match = RegExpUtility.getMatches(this.config.weekOfRegex, source).pop();
        let ers = this.config.dateExtractor.extract(source, referenceDate);
        if (!match || ers.length !== 1) {
            return result;
        }

        let dateResolution: DateTimeResolutionResult = this.config.dateParser.parse(ers[0], referenceDate).value;
        result.timex = dateResolution.timex;
        result.comment = this.weekOfComment;
        result.futureValue = this.getWeekRangeFromDate(dateResolution.futureValue);
        result.pastValue = this.getWeekRangeFromDate(dateResolution.pastValue);
        result.success = true;
        return result;
    }

    private parseMonthOfDate(source: string, referenceDate: Date): DateTimeResolutionResult {
        let result = new DateTimeResolutionResult();
        let match = RegExpUtility.getMatches(this.config.monthOfRegex, source).pop();
        let ers = this.config.dateExtractor.extract(source, referenceDate);
        if (!match || ers.length !== 1) {
            return result;
        }

        let dateResolution: DateTimeResolutionResult = this.config.dateParser.parse(ers[0], referenceDate).value;
        result.timex = dateResolution.timex;
        result.comment = this.monthOfComment;
        result.futureValue = this.getMonthRangeFromDate(dateResolution.futureValue);
        result.pastValue = this.getMonthRangeFromDate(dateResolution.pastValue);
        result.success = true;
        return result;
    }

    protected computeDate(cardinal: number, weekday: number, month: number, year: number) {
        let firstDay = new Date(year, month, 1);
        let firstWeekday = DateUtils.this(firstDay, weekday);
        if (weekday === 0) {
            weekday = 7;
        }
        let firstDayOfWeek = firstDay.getDay() !== 0 ? firstDay.getDay() : 7;
        if (weekday < firstDayOfWeek) {
            firstWeekday = DateUtils.next(firstDay, weekday);
        }
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
