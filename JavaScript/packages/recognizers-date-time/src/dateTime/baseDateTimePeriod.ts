import { Constants, TimeTypeConstants } from "./constants";
import { IExtractor, ExtractResult, BaseNumberExtractor, RegExpUtility, StringUtility } from "recognizers-text-number"
import { BaseDateExtractor, BaseDateParser } from "./baseDate"
import { BaseTimeExtractor, BaseTimeParser } from "./baseTime"
import { IDateTimeExtractor, BaseDateTimeExtractor, BaseDateTimeParser } from "./baseDateTime"
import { BaseDurationExtractor, BaseDurationParser } from "./baseDuration"
import { IDateTimeParser, DateTimeParseResult } from "./parsers"
import { FormatUtil, DateUtils, Token, DateTimeResolutionResult, StringMap } from "./utilities";

export interface IDateTimePeriodExtractorConfiguration {
    cardinalExtractor: BaseNumberExtractor
    singleDateExtractor: IDateTimeExtractor
    singleTimeExtractor: IDateTimeExtractor
    singleDateTimeExtractor: IDateTimeExtractor
    durationExtractor: IDateTimeExtractor
    simpleCasesRegexes: RegExp[]
    prepositionRegex: RegExp
    tillRegex: RegExp
    specificTimeOfDayRegex: RegExp
    timeOfDayRegex: RegExp
    periodTimeOfDayWithDateRegex: RegExp
    followedUnit: RegExp
    numberCombinedWithUnit: RegExp
    timeUnitRegex: RegExp
    pastPrefixRegex: RegExp
    nextPrefixRegex: RegExp
    relativeTimeUnitRegex: RegExp
    restOfDateTimeRegex: RegExp
    getFromTokenIndex(source: string): { matched: boolean, index: number };
    getBetweenTokenIndex(source: string): { matched: boolean, index: number };
    hasConnectorToken(source: string): boolean;
}

export class BaseDateTimePeriodExtractor implements IDateTimeExtractor {
    protected readonly extractorName = Constants.SYS_DATETIME_DATETIMEPERIOD;
    protected readonly config: IDateTimePeriodExtractorConfiguration;

    constructor(config: IDateTimePeriodExtractorConfiguration) {
        this.config = config;
    }

    extract(source: string, refDate: Date): Array<ExtractResult> {
        if (!refDate) refDate = new Date();
        let referenceDate = refDate;

        let tokens: Array<Token> = new Array<Token>()
        .concat(this.matchSimpleCases(source, referenceDate))
        .concat(this.mergeTwoTimePoints(source, referenceDate))
        .concat(this.matchDuration(source, referenceDate))
        .concat(this.matchNight(source, referenceDate))
        .concat(this.matchRelativeUnit(source));
        let result = Token.mergeAllTokens(tokens, source, this.extractorName);
        return result;
    }

    private matchSimpleCases(source: string, refDate: Date): Array<Token> {
        let tokens: Array<Token> = new Array<Token>();
        this.config.simpleCasesRegexes.forEach(regexp => {
            RegExpUtility.getMatches(regexp, source).forEach(match => {
                let followedStr = source.substr(match.index + match.length);
                if (StringUtility.isNullOrWhitespace(followedStr)) {
                    let beforeStr = source.substr(0, match.index);
                    let ers = this.config.singleDateExtractor.extract(beforeStr, refDate);
                    if (ers && ers.length > 0) {
                        let er = ers[0];
                        let begin = er.start;
                        let end = er.start + er.length;
                        let middleStr = beforeStr.substr(begin + er.length).trim().toLowerCase();
                        if (StringUtility.isNullOrWhitespace(middleStr) || RegExpUtility.getMatches(this.config.prepositionRegex, middleStr).length > 0) {
                            tokens.push(new Token(begin, match.index + match.length));
                        }
                    }
                } else {
                    let ers = this.config.singleDateExtractor.extract(followedStr, refDate);
                    if (ers && ers.length > 0) {
                        let er = ers[0];
                        let begin = er.start;
                        let end = er.start + er.length;
                        let middleStr = followedStr.substr(0, begin).trim().toLowerCase();
                        if (StringUtility.isNullOrWhitespace(middleStr) || RegExpUtility.getMatches(this.config.prepositionRegex, middleStr).length > 0) {
                            tokens.push(new Token(match.index, match.index + match.length + end));
                        }
                    }
                }
            });
        });
        return tokens;
    }

    protected mergeTwoTimePoints(source: string, refDate: Date): Array<Token> {
        let tokens: Array<Token> = new Array<Token>();
        let ersDateTime = this.config.singleDateTimeExtractor.extract(source, refDate);
        let ersTime = this.config.singleTimeExtractor.extract(source, refDate);
        let innerMarks: ExtractResult[] = [];
        let j = 0;
        ersDateTime.forEach((erDateTime, index) => {
            innerMarks.push(erDateTime);
            while (j < ersTime.length && ersTime[j].start + ersTime[j].length < erDateTime.start) {
                innerMarks.push(ersTime[j++]);
            }
            while (j < ersTime.length && ExtractResult.isOverlap(ersTime[j], erDateTime)) {
                j++;
            }
        });
        while (j < ersTime.length) {
            innerMarks.push(ersTime[j++]);
        }
        innerMarks = innerMarks.sort((erA, erB) => erA.start < erB.start ? -1 : erA.start === erB.start ? 0 : 1);
        let idx = 0;
        while (idx < innerMarks.length - 1) {
            let currentMark = innerMarks[idx];
            let nextMark = innerMarks[idx + 1];
            if (currentMark.type === Constants.SYS_DATETIME_TIME && nextMark.type === Constants.SYS_DATETIME_TIME) {
                idx++;
                continue;
            }
            let middleBegin = currentMark.start + currentMark.length;
            let middleEnd = nextMark.start;

            let middleStr = source.substr(middleBegin, middleEnd - middleBegin).trim().toLowerCase();
            let matches = RegExpUtility.getMatches(this.config.tillRegex, middleStr);
            if (matches && matches.length > 0 && matches[0].index === 0 && matches[0].length === middleStr.length) {
                let periodBegin = currentMark.start;
                let periodEnd = nextMark.start + nextMark.length;
                let beforeStr = source.substr(0, periodBegin).trim().toLowerCase();
                let matchFrom = this.config.getFromTokenIndex(beforeStr);
                let fromTokenIndex = matchFrom.matched ? matchFrom : this.config.getBetweenTokenIndex(beforeStr);
                if (fromTokenIndex.matched) {
                    periodBegin = fromTokenIndex.index;
                }
                tokens.push(new Token(periodBegin, periodEnd))
                idx += 2;
                continue;
            }
            if (this.config.hasConnectorToken(middleStr)) {
                let periodBegin = currentMark.start;
                let periodEnd = nextMark.start + nextMark.length;
                let beforeStr = source.substr(0, periodBegin).trim().toLowerCase();
                let betweenTokenIndex = this.config.getBetweenTokenIndex(beforeStr);
                if (betweenTokenIndex.matched) {
                    periodBegin = betweenTokenIndex.index;
                    tokens.push(new Token(periodBegin, periodEnd))
                    idx += 2;
                    continue;
                }
            }
            idx++;
        };
        return tokens;
    }

    private matchDuration(source: string, refDate: Date): Array<Token> {
        let tokens: Array<Token> = new Array<Token>();
        let durations: Array<Token> = new Array<Token>();
        this.config.durationExtractor.extract(source, refDate).forEach(duration => {
            let match = RegExpUtility.getMatches(this.config.timeUnitRegex, duration.text).pop();
            if (match) {
                durations.push(new Token(duration.start, duration.start + duration.length));
            }
        });
        durations.forEach(duration => {
            let beforeStr = source.substr(0, duration.start).toLowerCase()
            if (StringUtility.isNullOrWhitespace(beforeStr)) return;
            let match = RegExpUtility.getMatches(this.config.pastPrefixRegex, beforeStr).pop();
            if (match && StringUtility.isNullOrWhitespace(beforeStr.substr(match.index + match.length))) {
                tokens.push(new Token(match.index, duration.end))
                return;
            }
            match = RegExpUtility.getMatches(this.config.nextPrefixRegex, beforeStr).pop();
            if (match && StringUtility.isNullOrWhitespace(beforeStr.substr(match.index + match.length))) {
                tokens.push(new Token(match.index, duration.end))
            }
        });
        return tokens;
    }

    protected matchNight(source: string, refDate: Date): Array<Token> {
        let tokens: Array<Token> = new Array<Token>();
        RegExpUtility.getMatches(this.config.specificTimeOfDayRegex, source).forEach(match => {
            tokens.push(new Token(match.index, match.index + match.length))
        });
        this.config.singleDateExtractor.extract(source, refDate).forEach(er => {
            let afterStr = source.substr(er.start + er.length);
            let match = RegExpUtility.getMatches(this.config.periodTimeOfDayWithDateRegex, afterStr).pop();
            if (match && StringUtility.isNullOrWhitespace(afterStr.substr(0, match.index))) {
                tokens.push(new Token(er.start, er.start + er.length + match.index + match.length))
            }
            let beforeStr = source.substr(0, er.start);
            match = RegExpUtility.getMatches(this.config.periodTimeOfDayWithDateRegex, beforeStr).pop();
            if (match && StringUtility.isNullOrWhitespace(beforeStr.substr(match.index + match.length))) {
                let middleStr = source.substr(match.index + match.length, er.start - match.index - match.length);
                if (StringUtility.isWhitespace(middleStr)) {
                    tokens.push(new Token(match.index, er.start + er.length))
                }
            };
        });
        return tokens;
    }

    private matchRelativeUnit(source: string): Array<Token> {
        let tokens: Array<Token> = new Array<Token>();
        let matches = RegExpUtility.getMatches(this.config.relativeTimeUnitRegex, source);
        if (matches.length === 0) {
            matches = RegExpUtility.getMatches(this.config.restOfDateTimeRegex, source);
        }
        matches.forEach(match => {
            tokens.push(new Token(match.index, match.index + match.length));
        });
        return tokens;
    }
}

export interface IDateTimePeriodParserConfiguration {
    pureNumberFromToRegex: RegExp
    pureNumberBetweenAndRegex: RegExp
    periodTimeOfDayWithDateRegex: RegExp
    specificTimeOfDayRegex: RegExp
    pastRegex: RegExp
    futureRegex: RegExp
    relativeTimeUnitRegex: RegExp
    restOfDateTimeRegex: RegExp
    numbers: ReadonlyMap<string, number>
    unitMap: ReadonlyMap<string, string>
    dateExtractor: IDateTimeExtractor
    timeExtractor: IDateTimeExtractor
    dateTimeExtractor: IDateTimeExtractor
    timePeriodExtractor: IDateTimeExtractor
    durationExtractor: IDateTimeExtractor
    dateParser: BaseDateParser
    timeParser: BaseTimeParser
    dateTimeParser: BaseDateTimeParser
    timePeriodParser: IDateTimeParser
    durationParser: BaseDurationParser
    getMatchedTimeRange(source: string): { timeStr: string, beginHour: number, endHour: number, endMin: number, success: boolean }
    getSwiftPrefix(source: string): number
}

export class BaseDateTimePeriodParser implements IDateTimeParser {
    protected readonly parserName = Constants.SYS_DATETIME_DATETIMEPERIOD;
    protected readonly config: IDateTimePeriodParserConfiguration;

    constructor(config: IDateTimePeriodParserConfiguration) {
        this.config = config;
    }

    parse(extractorResult: ExtractResult, referenceDate?: Date): DateTimeParseResult | null {
        if (!referenceDate) referenceDate = new Date();
        let resultValue;
        if (extractorResult.type === this.parserName) {
            let source = extractorResult.text.trim().toLowerCase();
            let innerResult = this.mergeDateAndTimePeriods(source, referenceDate);
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
                innerResult.futureResolution = {};
                innerResult.futureResolution[TimeTypeConstants.START_DATETIME] = FormatUtil.formatDateTime(innerResult.futureValue[0]);
                innerResult.futureResolution[TimeTypeConstants.END_DATETIME] = FormatUtil.formatDateTime(innerResult.futureValue[1]);
                innerResult.pastResolution = {};
                innerResult.pastResolution[TimeTypeConstants.START_DATETIME] = FormatUtil.formatDateTime(innerResult.pastValue[0]);
                innerResult.pastResolution[TimeTypeConstants.END_DATETIME] = FormatUtil.formatDateTime(innerResult.pastValue[1]);
                resultValue = innerResult;
            }
        }
        let result = new DateTimeParseResult(extractorResult);
        result.value = resultValue;
        result.timexStr = resultValue ? resultValue.timex : '';
        result.resolutionStr = '';

        return result;
    }

    protected mergeDateAndTimePeriods( text:string, referenceTime:Date):DateTimeResolutionResult
    {
        let ret = new DateTimeResolutionResult();
        let trimedText = text.trim().toLowerCase();

        let er = this.config.timePeriodExtractor.extract(trimedText, referenceTime);
        if (er.length !== 1)
        {
            return this.parseSimpleCases(text, referenceTime);
        }

        let timePeriodParseResult = this.config.timePeriodParser.parse(er[0]);
        let timePeriodResolutionResult = timePeriodParseResult.value;

        if (!timePeriodResolutionResult)
        {
            return this.parseSimpleCases(text, referenceTime);
        }

        let timePeriodTimex = timePeriodResolutionResult.timex;
        // if it is a range type timex
        if (!StringUtility.isNullOrEmpty(timePeriodTimex)
            && timePeriodTimex.startsWith("("))
        {
            let dateResult = this.config.dateExtractor.extract(trimedText.replace(er[0].text, ""), referenceTime);
            let dateStr = "";
            let futureTime:Date;
            let pastTime: Date;
            if (dateResult.length === 1 && trimedText.replace(er[0].text, "").trim() === dateResult[0].text) {
                let pr = this.config.dateParser.parse(dateResult[0], referenceTime);
                if (pr.value) {
                    futureTime = pr.value.futureValue;
                    pastTime = pr.value.pastValue;

                    dateStr = pr.timexStr;
                }
                else {
                    return this.parseSimpleCases(text, referenceTime);
                }

                timePeriodTimex = timePeriodTimex.replace("(", "").replace(")", "");
                let timePeriodTimexArray = timePeriodTimex.split(',');
                let timePeriodFutureValue = timePeriodResolutionResult.futureValue;
                let beginTime = timePeriodFutureValue.item1;
                let endTime = timePeriodFutureValue.item2;

                if (timePeriodTimexArray.length === 3) {
                    let beginStr = dateStr + timePeriodTimexArray[0];
                    let endStr = dateStr + timePeriodTimexArray[1];

                    ret.timex = `(${beginStr},${endStr},${timePeriodTimexArray[2]})`;

                    ret.futureValue = [
                        DateUtils.safeCreateFromMinValue(futureTime.getFullYear(), futureTime.getMonth(), futureTime.getDate(),
                            beginTime.getHours(), beginTime.getMinutes(), beginTime.getSeconds()),
                        DateUtils.safeCreateFromMinValue(futureTime.getFullYear(), futureTime.getMonth(), futureTime.getDate(),
                            endTime.getHours(), endTime.getMinutes(), endTime.getSeconds())];

                    ret.pastValue = [
                        DateUtils.safeCreateFromMinValue(pastTime.getFullYear(), pastTime.getMonth(), pastTime.getDate(),
                            beginTime.getHours(), beginTime.getMinutes(), beginTime.getSeconds()),
                        DateUtils.safeCreateFromMinValue(pastTime.getFullYear(), pastTime.getMonth(), pastTime.getDate(),
                            endTime.getHours(), endTime.getMinutes(), endTime.getSeconds())];

                    if (!StringUtility.isNullOrEmpty(timePeriodResolutionResult.comment)
                        && timePeriodResolutionResult.comment === "ampm") {
                        ret.comment = "ampm";
                    }

                    ret.success = true;
                    ret.subDateTimeEntities = [pr, timePeriodParseResult];

                    return ret;
                }
            }
            else
            {
                return this.parseSimpleCases(text, referenceTime);
            }
        }

        return this.parseSimpleCases(text, referenceTime);
    }

    private parseSimpleCases(source: string, referenceDate: Date): DateTimeResolutionResult {
        let result = new DateTimeResolutionResult();
        let match = RegExpUtility.getMatches(this.config.pureNumberFromToRegex, source).pop();
        if (!match) {
            match = RegExpUtility.getMatches(this.config.pureNumberBetweenAndRegex, source).pop();
        }
        if (!match || match.index !== 0) return result;

        let hourGroup = match.groups('hour');
        let beginHour = this.config.numbers.get(hourGroup.captures[0]) || Number.parseInt(hourGroup.captures[0], 10) || 0;
        let endHour = this.config.numbers.get(hourGroup.captures[1]) || Number.parseInt(hourGroup.captures[1], 10) || 0;

        let er = this.config.dateExtractor.extract(source.replace(match.value,""), referenceDate).pop();
        if (!er) return result;

        let pr = this.config.dateParser.parse(er, referenceDate);
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

        if (!StringUtility.isNullOrEmpty(amStr) || descStr.startsWith('a')) {
            if (beginHour >= 12) beginHour -= 12;
            if (endHour >= 12) endHour -= 12;
            hasAm = true;
        }
        if (!StringUtility.isNullOrEmpty(pmStr) || descStr.startsWith('p')) {
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

    protected mergeTwoTimePoints(source: string, referenceDate: Date): DateTimeResolutionResult {
        let result = new DateTimeResolutionResult();
        let prs: { begin: DateTimeParseResult, end: DateTimeParseResult };
        let timeErs = this.config.timeExtractor.extract(source, referenceDate);
        let datetimeErs = this.config.dateTimeExtractor.extract(source, referenceDate);
        let bothHasDate = false;
        let beginHasDate = false;
        let endHasDate = false;

        if (datetimeErs.length === 2) {
            prs = this.getTwoPoints(datetimeErs[0], datetimeErs[1], this.config.dateTimeParser, this.config.dateTimeParser, referenceDate);
            bothHasDate = true;
        } else if (datetimeErs.length === 1 && timeErs.length === 2) {
            if (ExtractResult.isOverlap(datetimeErs[0], timeErs[0])) {
                prs = this.getTwoPoints(datetimeErs[0], timeErs[1], this.config.dateTimeParser, this.config.timeParser, referenceDate);
                beginHasDate = true;
            } else {
                prs = this.getTwoPoints(timeErs[0], datetimeErs[0], this.config.timeParser, this.config.dateTimeParser, referenceDate);
                endHasDate = true;
            }
        } else if (datetimeErs.length === 1 && timeErs.length === 1) {
            if (timeErs[0].start < datetimeErs[0].start) {
                prs = this.getTwoPoints(timeErs[0], datetimeErs[0], this.config.timeParser, this.config.dateTimeParser, referenceDate);
                endHasDate = true;
            } else {
                prs = this.getTwoPoints(datetimeErs[0], timeErs[0], this.config.dateTimeParser, this.config.timeParser, referenceDate);
                beginHasDate = true;
            }
        }
        if (!prs || !prs.begin.value || !prs.end.value) return result;

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
        } else if (endHasDate) {
            futureBegin = DateUtils.safeCreateFromMinValue(futureEnd.getFullYear(), futureEnd.getMonth(), futureEnd.getDate(), futureBegin.getHours(), futureBegin.getMinutes(), futureBegin.getSeconds());
            pastBegin = DateUtils.safeCreateFromMinValue(pastEnd.getFullYear(), pastEnd.getMonth(), pastEnd.getDate(), pastBegin.getHours(), pastBegin.getMinutes(), pastBegin.getSeconds());
            let dateStr = prs.end.timexStr.split('T')[0];
            result.timex = `(${dateStr}${prs.begin.timexStr},${prs.end.timexStr},PT${DateUtils.totalHours(futureEnd, futureBegin)}H)`;
        }
        if (!StringUtility.isNullOrEmpty(begin.comment) && begin.comment.endsWith('ampm') && !StringUtility.isNullOrEmpty(end.comment) && end.comment.endsWith('ampm')) {
            result.comment = 'ampm';
        }

        result.futureValue = [futureBegin, futureEnd];
        result.pastValue = [pastBegin, pastEnd];
        result.success = true;
        result.subDateTimeEntities = [prs.begin, prs.end];
        return result;
    }

    protected getTwoPoints(beginEr: ExtractResult, endEr: ExtractResult, beginParser: IDateTimeParser, endParser: IDateTimeParser, referenceDate: Date)
    : { begin: DateTimeParseResult, end: DateTimeParseResult } {
        let beginPr = beginParser.parse(beginEr, referenceDate);
        let endPr = endParser.parse(endEr, referenceDate);
        return { begin: beginPr, end: endPr };
    }

    protected parseSpecificTimeOfDay(source: string, referenceDate: Date): DateTimeResolutionResult {
        let result = new DateTimeResolutionResult();
        let timeText = source;
        let hasEarly = false;
        let hasLate = false;

        let match = RegExpUtility.getMatches(this.config.periodTimeOfDayWithDateRegex, source).pop();
        if (match) {
            timeText = match.groups('timeOfDay').value;
            if (!StringUtility.isNullOrEmpty(match.groups('early').value)) {
                hasEarly = true;
                result.comment = 'early';
            } else if (!StringUtility.isNullOrEmpty(match.groups('late').value)) {
                hasLate = true;
                result.comment = 'late';
            }
        }

        let matched = this.config.getMatchedTimeRange(timeText);
        if (!matched || !matched.success) return result;

        if (hasEarly) {
            matched.endHour = matched.beginHour + 2;
            if (matched.endMin === 59) matched.endMin = 0;
        } else if (hasLate) {
            matched.beginHour += 2;
        }

        match = RegExpUtility.getMatches(this.config.specificTimeOfDayRegex, source).pop();
        if (match && match.index === 0 && match.length === source.length) {
            let swift = this.config.getSwiftPrefix(source);
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

        match = RegExpUtility.getMatches(this.config.periodTimeOfDayWithDateRegex, source).pop();
        if (!match) return result;

        let beforeStr = source.substr(0, match.index).trim();
        let ers = this.config.dateExtractor.extract(beforeStr, referenceDate);
        if (ers.length === 0 || ers[0].length !== beforeStr.length) {
            let afterStr = source.substr(match.index + match.length).trim();
            ers = this.config.dateExtractor.extract(afterStr, referenceDate);
            if (ers.length === 0 || ers[0].length !== afterStr.length) return result;
        }

        let pr = this.config.dateParser.parse(ers[0], referenceDate);
        if (!pr) return result;

        let futureDate: Date = pr.value.futureValue;
        let pastDate: Date = pr.value.pastValue;

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

    protected parseDuration(source: string, referenceDate: Date): DateTimeResolutionResult {
        let result = new DateTimeResolutionResult();

        // for rest of datetime, it will be handled in next function
        let restOfDateTimeMatch = RegExpUtility.getMatches(this.config.restOfDateTimeRegex, source);
        if (restOfDateTimeMatch.length)
        {
            return result;
        }

        let ers = this.config.durationExtractor.extract(source, referenceDate);
        if (!ers || ers.length !== 1) return result;

        let pr = this.config.durationParser.parse(ers[0], referenceDate);
        if (!pr) return result;

        let beforeStr = source.substr(0, pr.start).trim();
        let durationResult: DateTimeResolutionResult = pr.value;
        let swiftSecond = 0;
        let mod: string;
        if (Number.isFinite(durationResult.pastValue) && Number.isFinite(durationResult.futureValue)) {
            swiftSecond = Math.round(durationResult.futureValue);
        }
        let beginTime = new Date(referenceDate);
        let endTime = new Date(referenceDate);
        let prefixMatch = RegExpUtility.getMatches(this.config.pastRegex, beforeStr).pop();
        if (prefixMatch && prefixMatch.length === beforeStr.length) {
            mod = TimeTypeConstants.beforeMod;
            beginTime.setSeconds(referenceDate.getSeconds() - swiftSecond);
        }
        prefixMatch = RegExpUtility.getMatches(this.config.futureRegex, beforeStr).pop();
        if (prefixMatch && prefixMatch.length === beforeStr.length) {
            mod = TimeTypeConstants.afterMod;
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

        if (mod) {
            pr.value.mod = mod;
        }
        result.subDateTimeEntities = [pr];

        return result;
    }

    private isFloat(value: any): boolean {
        return Number.isFinite(value) && !Number.isInteger(value);
    }

    private parseRelativeUnit(source: string, referenceDate: Date): DateTimeResolutionResult {
        let result = new DateTimeResolutionResult();
        let match = RegExpUtility.getMatches(this.config.relativeTimeUnitRegex, source).pop();
        if (!match) {
            match = RegExpUtility.getMatches(this.config.restOfDateTimeRegex, source).pop();
        }
        if (!match) return result;

        let srcUnit = match.groups('unit').value;
        let unitStr = this.config.unitMap.get(srcUnit);

        if (!unitStr) return result;
        let swift = 1;
        let prefixMatch = RegExpUtility.getMatches(this.config.pastRegex, source).pop();
        if (prefixMatch) swift = -1;

        let beginTime = new Date(referenceDate);
        let endTime = new Date(referenceDate);
        let ptTimex = '';

        switch (unitStr) {
            case 'D':
                endTime = DateUtils.safeCreateFromMinValue(beginTime.getFullYear(), beginTime.getMonth(),beginTime.getDate());
                endTime.setDate(endTime.getDate() + 1);
                endTime.setSeconds(endTime.getSeconds() - 1);
                ptTimex = `PT${DateUtils.totalSeconds(endTime, beginTime)}S`;
            break;
            case 'H':
                beginTime.setHours(beginTime.getHours() + (swift > 0 ? 0 : swift));
                endTime.setHours(endTime.getHours() + (swift > 0 ? swift : 0));
                ptTimex = `PT1H`;
            break;
            case 'M':
                beginTime.setMinutes(beginTime.getMinutes() + (swift > 0 ? 0 : swift));
                endTime.setMinutes(endTime.getMinutes() + (swift > 0 ? swift : 0));
                ptTimex = `PT1M`;
            break;
            case 'S':
                beginTime.setSeconds(beginTime.getSeconds() + (swift > 0 ? 0 : swift));
                endTime.setSeconds(endTime.getSeconds() + (swift > 0 ? swift : 0));
                ptTimex = `PT1S`;
            break;
            default: return result;
        }

        let luisDateBegin = FormatUtil.luisDateFromDate(beginTime);
        let luisTimeBegin = FormatUtil.luisTimeFromDate(beginTime);
        let luisDateEnd = FormatUtil.luisDateFromDate(endTime);
        let luisTimeEnd = FormatUtil.luisTimeFromDate(endTime);

        result.timex = `(${luisDateBegin}T${luisTimeBegin},${luisDateEnd}T${luisTimeEnd},${ptTimex})`;
        result.futureValue = [beginTime, endTime];
        result.pastValue = [beginTime, endTime];
        result.success = true;
        return result;
    }
}
