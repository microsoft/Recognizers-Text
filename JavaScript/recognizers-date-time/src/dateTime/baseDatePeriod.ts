import { Constants, TimeTypeConstants } from "./constants";
import { IExtractor, ExtractResult, BaseNumberExtractor, RegExpUtility, Match, StringUtility } from "recognizers-text-number"
import { Token, FormatUtil, DateTimeResolutionResult, DateUtils, DayOfWeek } from "./utilities";
import { BaseDurationExtractor, BaseDurationParser } from "./baseDuration"
import { IDateTimeParser, DateTimeParseResult } from "./parsers"
import { BaseDateExtractor, BaseDateParser } from "./baseDate"

export interface IDatePeriodExtractorConfiguration {
    simpleCasesRegexes: RegExp[]
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
    datePointExtractor: BaseDateExtractor
    integerExtractor: BaseNumberExtractor
    durationExtractor: BaseDurationExtractor
    getFromTokenIndex(source: string): { matched: boolean, index: number };
    getBetweenTokenIndex(source: string): { matched: boolean, index: number };
    hasConnectorToken(source: string): boolean;
}

export class BaseDatePeriodExtractor implements IExtractor {
    protected readonly extractorName = Constants.SYS_DATETIME_DATEPERIOD;
    protected readonly config: IDatePeriodExtractorConfiguration;

    constructor(config: IDatePeriodExtractorConfiguration) {
        this.config = config;
    }

    extract(source: string): Array<ExtractResult> {
        let tokens: Array<Token> = new Array<Token>();
        tokens = tokens.concat(this.matchSimpleCases(source));
        tokens = tokens.concat(this.mergeTwoTimePoints(source));
        tokens = tokens.concat(this.matchDuration(source));
        tokens = tokens.concat(this.singleTimePointWithPatterns(source));
        let result = Token.mergeAllTokens(tokens, source, this.extractorName);
        return result;
    }

    protected matchSimpleCases(source: string): Array<Token> {
        let tokens: Array<Token> = new Array<Token>();
        this.config.simpleCasesRegexes.forEach(regexp => {
            RegExpUtility.getMatches(regexp, source).forEach(match => {
                tokens.push(new Token(match.index, match.index + match.length));
            });
        });
        return tokens;
    }

    protected mergeTwoTimePoints(source: string): Array<Token> {
        let tokens: Array<Token> = new Array<Token>();
        let er = this.config.datePointExtractor.extract(source);
        if (er.length <= 1) {
            return tokens;
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

    private matchDuration(source: string): Array<Token> {
        let tokens: Array<Token> = new Array<Token>();
        let durations: Array<Token> = new Array<Token>();
        this.config.durationExtractor.extract(source).forEach(durationEx => {
            let match = RegExpUtility.getMatches(this.config.dateUnitRegex, durationEx.text).pop();
            if (match) {
                durations.push(new Token(durationEx.start, durationEx.start + durationEx.length))
            }
        });
        durations.forEach(duration => {
            let beforeStr = source.substring(0, duration.start).toLowerCase();
            if (StringUtility.isNullOrWhitespace(beforeStr)) return;
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

    private singleTimePointWithPatterns(source: string): Array<Token> {
        let tokens: Array<Token> = new Array<Token>();
        let ers = this.config.datePointExtractor.extract(source);
        if (ers.length < 1) return tokens;
        ers.forEach(er => {
            if (er.start && er.length) {
                let beforeStr = source.substring(0, er.start);
                tokens = tokens
                .concat(this.getTokenForRegexMatching(beforeStr, this.config.weekOfRegex, er))
                .concat(this.getTokenForRegexMatching(beforeStr, this.config.monthOfRegex, er))
            }
        });
        return tokens;
    }

    private getTokenForRegexMatching(source: string, regexp: RegExp, er: ExtractResult): Array<Token> {
        let tokens: Array<Token> = new Array<Token>();
        let match = RegExpUtility.getMatches(regexp, source).shift();
        if (match && source.trim().endsWith(match.value.trim())) {
            let startIndex = source.lastIndexOf(match.value);
            tokens.push(new Token(startIndex, er.start + er.length));
        }
        return tokens;
    }

    private matchRegexInPrefix(source: string, match: Match): boolean {
        return (match && StringUtility.isNullOrWhitespace(source.substring(match.index + match.length)))
    }
}

export interface IDatePeriodParserConfiguration {
    dateExtractor: BaseDateExtractor
    dateParser: BaseDateParser
    durationExtractor: IExtractor
    durationParser: BaseDurationParser
    monthFrontBetweenRegex: RegExp
    betweenRegex: RegExp
    monthFrontSimpleCasesRegex: RegExp
    simpleCasesRegex: RegExp
    oneWordPeriodRegex: RegExp
    monthWithYear: RegExp
    monthNumWithYear: RegExp
    yearRegex: RegExp
    pastRegex: RegExp
    futureRegex: RegExp
    inConnectorRegex: RegExp
    weekOfMonthRegex: RegExp
    weekOfYearRegex: RegExp
    quarterRegex: RegExp
    quarterRegexYearFront: RegExp
    seasonRegex: RegExp
    weekOfRegex: RegExp
    monthOfRegex: RegExp
    whichWeekRegex: RegExp
    restOfDateRegex: RegExp
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

            // parse duration should be at the end since it will extract "the last week" from "the last week of July"
            if (!innerResult.success) {
                innerResult = this.parseDuration(source, referenceDate);
            }

            if (innerResult.success) {
                if (innerResult.futureValue && innerResult.pastValue) {
                    innerResult.futureResolution = new Map<string, string>()
                    .set(TimeTypeConstants.START_DATE, innerResult.futureValue[0])
                    .set(TimeTypeConstants.END_DATE, innerResult.futureValue[1]);
                    innerResult.pastResolution = new Map<string, string>()
                    .set(TimeTypeConstants.START_DATE, innerResult.pastValue[0])
                    .set(TimeTypeConstants.END_DATE, innerResult.pastValue[1]);
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
        let match = RegExpUtility.getMatches(this.config.monthWithYear, trimmedSource).pop();
        if (!match) {
            match = RegExpUtility.getMatches(this.config.monthNumWithYear, trimmedSource).pop();
        }
        if (!match || match.length !== trimmedSource.length) return result;

        let monthStr = match.groups('month').value;
        let yearStr = match.groups('year').value;
        let orderStr = match.groups('order').value;

        let month = this.config.monthOfYear.get(monthStr) - 1;
        let year = Number.parseInt(yearStr, 10);
        if (!year || isNaN(year)) {
            let swift = this.config.getSwiftYear(orderStr);
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
        let noYear = false;

        let match = this.getMatchSimpleCase(source);
        
        if (!match || match.index !== 0 || match.length !== source.length) return result;
        let days = match.groups('day');
        let beginDay = this.config.dayOfMonth.get(days.captures[0]);
        let endDay = this.config.dayOfMonth.get(days.captures[1]);
        let monthStr = match.groups('month').value;
        if (!StringUtility.isNullOrEmpty(monthStr)) {
            month = this.config.monthOfYear.get(monthStr) - 1;
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
        let beginDateLuis = FormatUtil.luisDate(this.config.isFuture(monthStr) ? year : -1, month, beginDay);
        let endDateLuis = FormatUtil.luisDate(this.config.isFuture(monthStr) ? year : -1, month, endDay);

        let yearStr = match.groups('year').value;
        if (!StringUtility.isNullOrEmpty(yearStr)) {
            year = Number.parseInt(yearStr, 10);
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

    protected parseOneWordPeriod(source: string, referenceDate: Date): DateTimeResolutionResult {
        let result = new DateTimeResolutionResult();
        let year = referenceDate.getFullYear();
        let month = referenceDate.getMonth();

        if (this.config.isYearToDate(source)) {
            result.timex = FormatUtil.toString(year, 4);
            result.futureValue = [DateUtils.safeCreateFromValue(DateUtils.minValue(), year, 0, 1), referenceDate];
            result.pastValue = [DateUtils.safeCreateFromValue(DateUtils.minValue(), year, 0, 1), referenceDate];
            result.success = true;
            return result;
        }
        if (this.config.isMonthToDate(source)) {
            result.timex = `${FormatUtil.toString(year, 4)}-${FormatUtil.toString(month + 1, 2)}`;
            result.futureValue = [DateUtils.safeCreateFromValue(DateUtils.minValue(), year, month, 1), referenceDate];
            result.pastValue = [DateUtils.safeCreateFromValue(DateUtils.minValue(), year, month, 1), referenceDate];
            result.success = true;
            return result;
        }

        let futureYear = year;
        let pastYear = year;
        let match = RegExpUtility.getMatches(this.config.oneWordPeriodRegex, source).pop();
        if (!match || match.index !== 0 || match.length !== source.length) return result;
        let monthStr = match.groups('month').value;
        if (!StringUtility.isNullOrEmpty(monthStr)) {
            let swift = this.config.getSwiftYear(source);
            month = this.config.monthOfYear.get(monthStr) - 1;
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
            if (this.config.isWeekOnly(source)) {
                let monday = DateUtils.addDays(DateUtils.this(referenceDate, DayOfWeek.Monday), 7 * swift);
                let sunday = DateUtils.addDays(DateUtils.this(referenceDate, DayOfWeek.Sunday), (7 * swift) + (this.inclusiveEndPeriod ? 0 : 1));

                result.timex = `${FormatUtil.toString(monday.getFullYear(), 4)}-W${FormatUtil.toString(DateUtils.getWeekNumber(monday).weekNo, 2)}`;
                result.futureValue = [monday, sunday];
                result.pastValue = [monday, sunday];
                result.success = true;
                return result;
            }
            if (this.config.isWeekend(source)) {
                let beginDate = DateUtils.addDays(DateUtils.this(referenceDate, DayOfWeek.Saturday), 7 * swift);
                let endDate = DateUtils.addDays(DateUtils.this(referenceDate, DayOfWeek.Sunday), (7 * swift) + (this.inclusiveEndPeriod ? 0 : 1));

                result.timex = `${FormatUtil.toString(beginDate.getFullYear(), 4)}-W${FormatUtil.toString(DateUtils.getWeekNumber(beginDate).weekNo, 2)}-WE`;
                result.futureValue = [beginDate, endDate];
                result.pastValue = [beginDate, endDate];
                result.success = true;
                return result;
            }
            if (this.config.isMonthOnly(source)) {
                let tempDate = new Date(referenceDate);
                tempDate.setMonth(referenceDate.getMonth() + swift);
                month = tempDate.getMonth();
                year = tempDate.getFullYear();
                result.timex = `${FormatUtil.toString(year, 4)}-${FormatUtil.toString(month + 1, 2)}`;
                futureYear = year;
                pastYear = year;
            } else if (this.config.isYearOnly(source)) {
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

    protected mergeTwoTimePoints(source: string, referenceDate: Date): DateTimeResolutionResult {
        let trimmedSource = source.trim();
        let result = new DateTimeResolutionResult();
        let ers = this.config.dateExtractor.extract(trimmedSource);
        if (!ers || ers.length < 2) {
            ers = this.config.dateExtractor.extract(this.config.tokenBeforeDate + trimmedSource)
            .map(er => {
                er.start -= this.config.tokenBeforeDate.length;
                return er;
            });
            if (!ers || ers.length < 2) return result;
        }
        let prs = ers.map(er => this.config.dateParser.parse(er, referenceDate)).filter(pr => pr);
        if (prs.length < 2) return result;

        let prBegin = prs[0];
        let prEnd = prs[1];
        let futureBegin = prBegin.value.futureValue;
        let futureEnd = prEnd.value.futureValue;
        let pastBegin = prBegin.value.pastValue;
        let pastEnd = prEnd.value.pastValue;

        result.subDateTimeEntities = prs;
        result.timex = `(${prBegin.timexStr},${prEnd.timexStr},P${DateUtils.diffDays(futureEnd, futureBegin)}D)`;
        result.futureValue = [futureBegin, futureEnd];
        result.pastValue = [pastBegin, pastEnd];
        result.success = true;
        return result;
    }

    protected parseYear(source: string, referenceDate: Date): DateTimeResolutionResult {
        let trimmedSource = source.trim();
        let result = new DateTimeResolutionResult();
        let match = RegExpUtility.getMatches(this.config.yearRegex, trimmedSource).pop();
        if (!match || match.length !== trimmedSource.length) return result;

        let year = Number.parseInt(match.value, 10);
        let beginDate = DateUtils.safeCreateFromValue(DateUtils.minValue(), year, 0, 1);
        let endDate = DateUtils.addDays(DateUtils.safeCreateFromValue(DateUtils.minValue(), year + 1, 0, 1), this.inclusiveEndPeriod ? -1 : 0);
        result.timex = FormatUtil.toString(year, 4);
        result.futureValue = [beginDate, endDate];
        result.pastValue = [beginDate, endDate];
        result.success = true;
        return result;
    }

    protected parseDuration(source: string, referenceDate: Date): DateTimeResolutionResult {
        let result = new DateTimeResolutionResult();
        let ers = this.config.durationExtractor.extract(source);
        let beginDate = new Date(referenceDate);
        let endDate = new Date(referenceDate);
        let restNowSunday = false;
        let durationTimex = '';

        if (ers.length === 1) {
            let pr = this.config.durationParser.parse(ers[0]);
            if (pr === null) return result;

            let beforeStr = source.substr(0, pr.start).trim();
            let mod: string;
            let durationResult: DateTimeResolutionResult = pr.value;
            if (StringUtility.isNullOrEmpty(durationResult.timex)) return result;

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
            }
            prefixMatch = RegExpUtility.getMatches(this.config.inConnectorRegex, beforeStr).pop();
            if (prefixMatch && prefixMatch.length === beforeStr.length) {
                mod = TimeTypeConstants.afterMod
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
            result.timex = `(${FormatUtil.luisDateFromDate(beginDate)},${FormatUtil.luisDateFromDate(endDate)},${durationTimex})`;
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
        if (swift === 0) return result;

        if (!isPositiveSwift) swift *= -1;
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
        if (!match || match.length !== source.length) return result;

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
        } else {
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
        `XXXX-${FormatUtil.toString(month + 1, 2)}-W${FormatUtil.toString(cardinal, 2)}` :
        `${FormatUtil.toString(year, 4)}-${FormatUtil.toString(month + 1, 2)}-W${FormatUtil.toString(cardinal, 2)}`;
        result.futureValue = [futureDate, DateUtils.addDays(futureDate, this.inclusiveEndPeriod ? 6 : 7)];
        result.pastValue = [pastDate, DateUtils.addDays(pastDate, this.inclusiveEndPeriod ? 6 : 7)];
        result.success = true;
        return result;
    }

    private parseWeekOfYear(source: string, referenceDate: Date): DateTimeResolutionResult {
        let result = new DateTimeResolutionResult();
        let match = RegExpUtility.getMatches(this.config.weekOfYearRegex, source).pop();
        if (!match || match.length !== source.length) return result;

        let cardinalStr = match.groups('cardinal').value;
        let yearStr = match.groups('year').value;
        let orderStr = match.groups('order').value;

        let year = Number.parseInt(yearStr, 10);
        if (isNaN(year)) {
            let swift = this.config.getSwiftYear(orderStr);
            if (swift < -1) return result;
            year = referenceDate.getFullYear() + swift;
        }
        if (this.config.isLastCardinal(cardinalStr)) {
            result = this.getWeekOfMonth(5, 11, year, referenceDate, false);
        } else {
            let cardinal = this.config.cardinalMap.get(cardinalStr);
            result = this.getWeekOfMonth(cardinal, 0, year, referenceDate, false);
        }
        return result;
    }

    protected parseQuarter(source: string, referenceDate: Date): DateTimeResolutionResult {
        let result = new DateTimeResolutionResult();
        let match = RegExpUtility.getMatches(this.config.quarterRegex, source).pop();
        if (!match || match.length !== source.length) {
            match = RegExpUtility.getMatches(this.config.quarterRegexYearFront, source).pop();
        }
        if (!match || match.length !== source.length) return result;

        let cardinalStr = match.groups('cardinal').value;
        let yearStr = match.groups('year').value;
        let orderStr = match.groups('order').value;

        let year = Number.parseInt(yearStr, 10);
        if (isNaN(year)) {
            let swift = this.config.getSwiftYear(orderStr);
            if (swift < -1) return result;
            year = referenceDate.getFullYear() + swift;
        }

        let quarterNum = this.config.cardinalMap.get(cardinalStr);
        let beginDate = DateUtils.safeCreateFromValue(DateUtils.minValue(), year, quarterNum * 3 - 3, 1);
        let endDate = DateUtils.safeCreateFromValue(DateUtils.minValue(), year, quarterNum * 3, 1);
        result.futureValue = [beginDate, endDate];
        result.pastValue = [beginDate, endDate];
        result.timex = `(${FormatUtil.luisDateFromDate(beginDate)},${FormatUtil.luisDateFromDate(endDate)},P3M)`;
        result.success = true;
        return result;
    }

    protected parseSeason(source: string, referenceDate: Date): DateTimeResolutionResult {
        let result = new DateTimeResolutionResult();
        let match = RegExpUtility.getMatches(this.config.seasonRegex, source).pop();
        if (!match || match.length !== source.length) return result;

        let swift = this.config.getSwiftYear(source);
        let yearStr = match.groups('year').value;
        let year = referenceDate.getFullYear();
        let seasonStr = match.groups('seas').value;
        let season = this.config.seasonMap.get(seasonStr);
        if (swift >= -1 || !StringUtility.isNullOrEmpty(yearStr)) {
            if (StringUtility.isNullOrEmpty(yearStr)) yearStr = FormatUtil.toString(year + swift, 4);
            result.timex = `${yearStr}-${season}`;
        } else {
            result.timex = season;
        }
        result.success = true;
        return result;
    }

    private parseWhichWeek(source: string, referenceDate: Date): DateTimeResolutionResult {
        let result = new DateTimeResolutionResult();
        let match = RegExpUtility.getMatches(this.config.whichWeekRegex, source).pop();
        if (!match) return result;
        let num = Number.parseInt(match.groups('number').value, 10);
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
        let match = RegExpUtility.getMatches(this.config.weekOfRegex, source).pop();
        let ers = this.config.dateExtractor.extract(source);
        if (!match || ers.length !== 1) return result;

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
        let ers = this.config.dateExtractor.extract(source);
        if (!match || ers.length !== 1) return result;

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
        if (weekday === 0) weekday = 7;
        let firstDayOfWeek = firstDay.getDay() !== 0 ? firstDay.getDay() : 7;
        if (weekday < firstDayOfWeek) firstWeekday = DateUtils.next(firstDay, weekday);
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
