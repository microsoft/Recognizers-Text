import { Constants } from "./constants";
import { Token, AgoLaterUtil, IDateTimeUtilityConfiguration } from "./utilities";
import { IExtractor, ExtractResult, BaseNumberExtractor } from "../number/extractors"
import { BaseNumberParser } from "../number/parsers"
import { Match, RegExpUtility, isNullOrWhitespace, isWhitespace } from "../utilities";
import { BaseDateTime } from "../resources/baseDateTime";
import * as _ from "lodash";

export interface IDateExtractorConfiguration {
    dateRegexList: RegExp[],
    implicitDateList: RegExp[],
    monthEnd: RegExp,
    ofMonth: RegExp,
    dateUnitRegex: RegExp,
    ordinalExtractor: BaseNumberExtractor,
    integerExtractor: BaseNumberExtractor,
    numberParser: BaseNumberParser
    durationExtractor: IExtractor
    utilityConfiguration: IDateTimeUtilityConfiguration
}

export interface IDurationExtractorConfiguration {
    allRegex: RegExp,
    halfRegex: RegExp,
    followedUnit: RegExp,
    numberCombinedWithUnit: RegExp,
    anUnitRegex: RegExp,
    inExactNumberUnitRegex: RegExp,
    suffixAndRegex: RegExp,
    cardinalExtractor: BaseNumberExtractor
}

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
    cardinalExtractor: BaseNumberExtractor
    durationExtractor: BaseDurationExtractor
    getFromTokenIndex(source: string): { matched: boolean, index: number };
    getBetweenTokenIndex(source: string): { matched: boolean, index: number };
    hasConnectorToken(source: string): boolean;
}

export interface ITimeExtractorConfiguration {
    timeRegexList: RegExp[]
    atRegex: RegExp
    ishRegex: RegExp
}

export interface IDateTimeExtractorConfiguration {
    datePointExtractor: BaseDateExtractor
    timePointExtractor: BaseTimeExtractor
    durationExtractor: BaseDurationExtractor
    suffixRegex: RegExp
    nowRegex: RegExp
    timeOfTodayAfterRegex: RegExp
    simpleTimeOfTodayAfterRegex: RegExp
    nightRegex: RegExp
    timeOfTodayBeforeRegex: RegExp
    simpleTimeOfTodayBeforeRegex: RegExp
    theEndOfRegex: RegExp
    unitRegex: RegExp
    utilityConfiguration: IDateTimeUtilityConfiguration
    isConnectorToken(source: string): boolean
}

export interface IDateTimePeriodExtractorConfiguration {
    cardinalExtractor: BaseNumberExtractor
    singleDateExtractor: BaseDateExtractor
    singleTimeExtractor: BaseTimeExtractor
    singleDateTimeExtractor: BaseDateTimeExtractor
    durationExtractor: BaseDurationExtractor
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
    getFromTokenIndex(source: string): { matched: boolean, index: number };
    getBetweenTokenIndex(source: string): { matched: boolean, index: number };
    hasConnectorToken(source: string): boolean;
}

export interface ITimePeriodExtractorConfiguration {
    simpleCasesRegex: RegExp[];
    tillRegex: RegExp;
    timeOfDayRegex: RegExp;
    singleTimeExtractor: IExtractor;
    getFromTokenIndex(source: string): { matched: boolean, index: number };
    getBetweenTokenIndex(source: string): { matched: boolean, index: number };
    hasConnectorToken(source: string): boolean;
}

export interface IHolidayExtractorConfiguration {
    holidayRegexes: RegExp[]
}

export enum DateTimeOptions {
    None, SkipFromToMerge
}

export interface IMergedExtractorConfiguration {
    dateExtractor: BaseDateExtractor
    timeExtractor: BaseTimeExtractor
    dateTimeExtractor: BaseDateTimeExtractor
    datePeriodExtractor: BaseDatePeriodExtractor
    timePeriodExtractor: BaseTimePeriodExtractor
    dateTimePeriodExtractor: BaseDateTimePeriodExtractor
    holidayExtractor: BaseHolidayExtractor
    durationExtractor: BaseDurationExtractor
    setExtractor: BaseSetExtractor
    AfterRegex: RegExp
    BeforeRegex: RegExp
    FromToRegex: RegExp
    singleAmbiguousMonthRegex: RegExp
    prepositionSuffixRegex: RegExp
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
            let num = _.toNumber(this.config.numberParser.parse(result).value);
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

export class BaseTimeExtractor implements IExtractor {
    private readonly extractorName = Constants.SYS_DATETIME_TIME; // "Time";

    private readonly config: ITimeExtractorConfiguration;

    constructor(config: ITimeExtractorConfiguration) {
        this.config = config;
    }

    extract(text: string): Array<ExtractResult> {
        let tokens: Array<Token> = new Array<Token>()
            .concat(this.basicRegexMatch(text))
            .concat(this.atRegexMatch(text))
            .concat(this.specialsRegexMatch(text));

        let result = Token.mergeAllTokens(tokens, text, this.extractorName);
        return result;
    }

    basicRegexMatch(text: string): Array<Token> {
        let ret = [];
        this.config.timeRegexList.forEach(regexp => {
            let matches = RegExpUtility.getMatches(regexp, text);
            matches.forEach(match => {
                ret.push(new Token(match.index, match.index + match.length));
            });
        });
        return ret;
    }

    atRegexMatch(text: string): Array<Token> {
        let ret = [];
        // handle "at 5", "at seven"
        let matches = RegExpUtility.getMatches(this.config.atRegex, text);
        matches.forEach(match => {
            if (match.index + match.length < text.length &&
                text.charAt(match.index + match.length) === '%') {
                return;
            }
            ret.push(new Token(match.index, match.index + match.length));
        });
        return ret;
    }

    specialsRegexMatch(text: string): Array<Token> {
        let ret = [];
        // handle "ish"
        if (this.config.ishRegex != null) {
            let matches = RegExpUtility.getMatches(this.config.ishRegex, text);
            matches.forEach(match => {
                ret.push(new Token(match.index, match.index + match.length));
            });
        }
        return ret;
    }
}

export class BaseDurationExtractor implements IExtractor {
    private readonly extractorName = Constants.SYS_DATETIME_DURATION;

    private readonly config: IDurationExtractorConfiguration;

    constructor(config: IDurationExtractorConfiguration) {
        this.config = config;
    }

    extract(source: string): Array<ExtractResult> {
        let baseTokens = this.numberWithUnit(source);
        let tokens: Array<Token> = new Array<Token>()
            .concat(baseTokens)
            .concat(this.numberWithUnitAndSuffix(source, baseTokens))
            .concat(this.implicitDuration(source))
        let result = Token.mergeAllTokens(tokens, source, this.extractorName);
        return result;
    }

    private numberWithUnit(source: string): Array<Token> {
        return this.config.cardinalExtractor.extract(source)
            .map(o => {
                let afterString = source.substring(o.start + o.length);
                let match = RegExpUtility.getMatches(this.config.followedUnit, afterString)[0];
                if (match && match.index === 0) {
                    return new Token(o.start | 0, o.start + o.length + match.length);
                }
            }).filter(o => o !== undefined)
            .concat(this.getTokensFromRegex(this.config.numberCombinedWithUnit, source))
            .concat(this.getTokensFromRegex(this.config.anUnitRegex, source))
            .concat(this.getTokensFromRegex(this.config.inExactNumberUnitRegex, source));
    }

    private numberWithUnitAndSuffix(source: string, ers: Token[]): Array<Token> {
        return ers.map(o => {
            let afterString = source.substring(o.start + o.length);
            let match = RegExpUtility.getMatches(this.config.suffixAndRegex, afterString)[0];
            if (match && match.index === 0) {
                return new Token(o.start | 0, o.start + o.length + match.length);
            }
        });
    }

    private implicitDuration(source: string): Array<Token> {
        return this.getTokensFromRegex(this.config.allRegex, source)
            .concat(this.getTokensFromRegex(this.config.halfRegex, source));
    }

    private getTokensFromRegex(regexp: RegExp, source: string): Array<Token> {
        return RegExpUtility.getMatches(regexp, source)
            .map(o => new Token(o.index, o.index + o.length));
    }
}

export class BaseDatePeriodExtractor implements IExtractor {
    private readonly extractorName = Constants.SYS_DATETIME_DATEPERIOD;

    private readonly config: IDatePeriodExtractorConfiguration;

    constructor(config: IDatePeriodExtractorConfiguration) {
        this.config = config;
    }

    extract(source: string): Array<ExtractResult> {
        let tokens: Array<Token> = new Array<Token>()
            .concat(this.matchSimpleCases(source))
            .concat(this.mergeTwoTimePoints(source))
            .concat(this.matchDuration(source))
            .concat(this.singleTimePointWithPatterns(source));
        let result = Token.mergeAllTokens(tokens, source, this.extractorName);
        return result;
    }

    private matchSimpleCases(source: string): Array<Token> {
        let tokens: Array<Token> = new Array<Token>();
        this.config.simpleCasesRegexes.forEach(regexp => {
            RegExpUtility.getMatches(regexp, source).forEach(match => {
                tokens.push(new Token(match.index, match.index + match.length));
            });
        });
        return tokens;
    }

    private mergeTwoTimePoints(source: string): Array<Token> {
        let tokens: Array<Token> = new Array<Token>();
        let er = this.config.datePointExtractor.extract(source);
        if (er.length <= 1) {
            return tokens;
        }
        let idx = 0;
        while (idx < er.length - 1) {
            let middleBegin = er[idx].start + er[idx].length;
            let middleEnd = er[idx + 1].start;
            if (middleBegin >= middleEnd) {
                idx++;
                continue;
            }
            let middleStr = source.substr(middleBegin, middleEnd - middleBegin).trim().toLowerCase();
            let match = RegExpUtility.getMatches(this.config.tillRegex, middleStr);
            if (match && match.length > 0 && match[0].index === 0 && match[0].length === middleStr.length) {
                let periodBegin = er[idx].start;
                let periodEnd = er[idx + 1].start + er[idx + 1].length;

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
                let periodBegin = er[idx].start;
                let periodEnd = er[idx + 1].start + er[idx + 1].length;

                let beforeStr = source.substring(0, periodBegin).trim().toLowerCase();
                let betweenTokenIndex = this.config.getBetweenTokenIndex(beforeStr);
                if (betweenTokenIndex.matched) {
                    periodBegin = betweenTokenIndex.index;
                }
                tokens.push(new Token(periodBegin, periodEnd));
                idx += 2;
                continue;
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
            if (isNullOrWhitespace(beforeStr)) return;
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
        return (match && isNullOrWhitespace(source.substring(match.index + match.length)))
    }
}

export class BaseDateTimeExtractor implements IExtractor {
    private readonly extractorName = Constants.SYS_DATETIME_DATETIME;

    private readonly config: IDateTimeExtractorConfiguration;

    constructor(config: IDateTimeExtractorConfiguration) {
        this.config = config;
    }

    extract(source: string): Array<ExtractResult> {
        let tokens: Array<Token> = new Array<Token>()
            .concat(this.mergeDateAndTime(source))
            .concat(this.basicRegexMatch(source))
            .concat(this.timeOfTodayBefore(source))
            .concat(this.timeOfTodayAfter(source))
            .concat(this.specialTimeOfDate(source))
            .concat(this.durationWithBeforeAndAfter(source));
        let result = Token.mergeAllTokens(tokens, source, this.extractorName);
        return result;
    }

    private mergeDateAndTime(source: string): Array<Token> {
        let tokens: Array<Token> = new Array<Token>();
        let ers = this.config.datePointExtractor.extract(source);
        if (ers.length < 1) return tokens;
        ers = ers.concat(this.config.timePointExtractor.extract(source));
        if (ers.length < 2) return tokens;
        ers = ers.sort((erA, erB) => erA.start < erB.start ? -1 : erA.start === erB.start ? 0 : 1);
        let i = 0;
        while (i < ers.length - 1) {
            let j = i + 1;
            while (j < ers.length && ExtractResult.isOverlap(ers[i], ers[j])) {
                j++;
            }
            if (j >= ers.length) break;
            if ((ers[i].type === Constants.SYS_DATETIME_DATE && ers[j].type === Constants.SYS_DATETIME_TIME) ||
                (ers[i].type === Constants.SYS_DATETIME_TIME && ers[j].type === Constants.SYS_DATETIME_DATE)) {
                let middleBegin = ers[i].start + ers[i].length;
                let middleEnd = ers[j].start;
                if (middleBegin > middleEnd) {
                    i = j + 1;
                    continue;
                }
                let middleStr = source.substr(middleBegin, middleEnd - middleBegin).trim().toLowerCase();
                if (this.config.isConnectorToken(middleStr)) {
                    let begin = ers[i].start;
                    let end = ers[j].start + ers[j].length;
                    tokens.push(new Token(begin, end));
                }
                i = j + 1;
                continue;
            }
            i = j;
        }
        tokens.forEach((token, index) => {
            let afterStr = source.substr(token.end);
            let match = RegExpUtility.getMatches(this.config.suffixRegex, afterStr);
            if (match && match.length > 0) {
                // TODO: verify element
                token.end += match[0].length;
            }
        });
        return tokens;
    }

    private basicRegexMatch(source: string): Array<Token> {
        let tokens: Array<Token> = new Array<Token>();
        RegExpUtility.getMatches(this.config.nowRegex, source)
            .forEach(match => {
                tokens.push(new Token(match.index, match.index + match.length))
            });
        return tokens;
    }

    private timeOfTodayBefore(source: string): Array<Token> {
        let tokens: Array<Token> = new Array<Token>();
        let ers = this.config.timePointExtractor.extract(source);
        ers.forEach(er => {
            let beforeStr = source.substr(0, er.start);
            let innerMatches = RegExpUtility.getMatches(this.config.nightRegex, er.text);
            if (innerMatches && innerMatches.length > 0 && innerMatches[0].index === 0) {
                beforeStr = source.substr(0, er.start + innerMatches[0].length);
            }
            if (isNullOrWhitespace(beforeStr)) return;
            let matches = RegExpUtility.getMatches(this.config.timeOfTodayBeforeRegex, beforeStr);
            if (matches && matches.length > 0) {
                let begin = matches[0].index;
                let end = er.start + er.length;
                tokens.push(new Token(begin, end));
            }
        });
        RegExpUtility.getMatches(this.config.simpleTimeOfTodayBeforeRegex, source)
            .forEach(match => {
                tokens.push(new Token(match.index, match.index + match.length))
            });
        return tokens;
    }

    private timeOfTodayAfter(source: string): Array<Token> {
        let tokens: Array<Token> = new Array<Token>();
        let ers = this.config.timePointExtractor.extract(source);
        ers.forEach(er => {
            let afterStr = source.substr(er.start + er.length);
            if (isNullOrWhitespace(afterStr)) return;
            let matches = RegExpUtility.getMatches(this.config.timeOfTodayAfterRegex, afterStr);
            if (matches && matches.length > 0) {
                let begin = er.start;
                let end = er.start + er.length + matches[0].length
                tokens.push(new Token(begin, end));
            }
        });
        RegExpUtility.getMatches(this.config.simpleTimeOfTodayAfterRegex, source)
            .forEach(match => {
                tokens.push(new Token(match.index, match.index + match.length))
            });
        return tokens;
    }

    private specialTimeOfDate(source: string): Array<Token> {
        let tokens: Array<Token> = new Array<Token>();
        let ers = this.config.datePointExtractor.extract(source);
        ers.forEach(er => {
            let beforeStr = source.substr(0, er.start);
            let beforeMatches = RegExpUtility.getMatches(this.config.theEndOfRegex, beforeStr);
            if (beforeMatches && beforeMatches.length > 0) {
                tokens.push(new Token(beforeMatches[0].index, er.start + er.length))
            } else {
                let afterStr = source.substr(er.start + er.length);
                let afterMatches = RegExpUtility.getMatches(this.config.theEndOfRegex, afterStr);
                if (afterMatches && afterMatches.length > 0) {
                    tokens.push(new Token(er.start, er.start + er.length + afterMatches[0].index + afterMatches[0].length))
                }
            }
        });
        return tokens;
    }

    private durationWithBeforeAndAfter(source: string): Array<Token> {
        let tokens: Array<Token> = new Array<Token>();
        this.config.durationExtractor.extract(source).forEach(er => {
            let matches = RegExpUtility.getMatches(this.config.unitRegex, er.text);
            if (matches && matches.length > 0) {
                tokens = AgoLaterUtil.extractorDurationWithBeforeAndAfter(source, er, tokens, this.config.utilityConfiguration);
            }
        });
        return tokens;
    }
}

export class BaseDateTimePeriodExtractor implements IExtractor {
    private readonly extractorName = Constants.SYS_DATETIME_DATETIMEPERIOD;

    private readonly config: IDateTimePeriodExtractorConfiguration;

    constructor(config: IDateTimePeriodExtractorConfiguration) {
        this.config = config;
    }

    extract(source: string): Array<ExtractResult> {
        let tokens: Array<Token> = new Array<Token>()
            .concat(this.matchSimpleCases(source))
            .concat(this.mergeTwoTimePoints(source))
            .concat(this.matchDuration(source))
            .concat(this.matchNight(source))
            .concat(this.matchRelativeUnit(source));
        let result = Token.mergeAllTokens(tokens, source, this.extractorName);
        return result;
    }

    private matchSimpleCases(source: string): Array<Token> {
        let tokens: Array<Token> = new Array<Token>();
        this.config.simpleCasesRegexes.forEach(regexp => {
            RegExpUtility.getMatches(regexp, source).forEach(match => {
                let followedStr = source.substr(match.index + match.length);
                if (isNullOrWhitespace(followedStr)) {
                    let beforeStr = source.substr(0, match.index);
                    let ers = this.config.singleDateExtractor.extract(beforeStr);
                    if (ers && ers.length > 0) {
                        let er = ers[0];
                        let begin = er.start;
                        let end = er.start + er.length;
                        let middleStr = beforeStr.substr(begin + er.length).trim().toLowerCase();
                        if (isNullOrWhitespace(middleStr) || RegExpUtility.getMatches(this.config.prepositionRegex, middleStr).length > 0) {
                            tokens.push(new Token(begin, match.index + match.length));
                        }
                    }
                } else {
                    let ers = this.config.singleDateExtractor.extract(followedStr);
                    if (ers && ers.length > 0) {
                        let er = ers[0];
                        let begin = er.start;
                        let end = er.start + er.length;
                        let middleStr = followedStr.substr(0, begin).trim().toLowerCase();
                        if (isNullOrWhitespace(middleStr) || RegExpUtility.getMatches(this.config.prepositionRegex, middleStr).length > 0) {
                            tokens.push(new Token(match.index, match.index + match.length + end));
                        }
                    }
                }
            });
        });
        return tokens;
    }

    private mergeTwoTimePoints(source: string): Array<Token> {
        let tokens: Array<Token> = new Array<Token>();
        let ersDateTime = this.config.singleDateTimeExtractor.extract(source);
        let ersTime = this.config.singleTimeExtractor.extract(source);
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
                let fromTokenIndex = this.config.getFromTokenIndex(beforeStr);
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

    private matchDuration(source: string): Array<Token> {
        let tokens: Array<Token> = new Array<Token>();
        let durations: Array<Token> = new Array<Token>();
        this.config.durationExtractor.extract(source).forEach(duration => {
            let match = RegExpUtility.getMatches(this.config.timeUnitRegex, duration.text).pop();
            if (match) {
                durations.push(new Token(duration.start, duration.start + duration.length));
            }
        });
        durations.forEach(duration => {
            let beforeStr = source.substr(0, duration.start).toLowerCase()
            if (isNullOrWhitespace(beforeStr)) return;
            let match = RegExpUtility.getMatches(this.config.pastPrefixRegex, beforeStr).pop();
            if (match && isNullOrWhitespace(beforeStr.substr(match.index + match.length))) {
                tokens.push(new Token(match.index, duration.end))
                return;
            }
            match = RegExpUtility.getMatches(this.config.nextPrefixRegex, beforeStr).pop();
            if (match && isNullOrWhitespace(beforeStr.substr(match.index + match.length))) {
                tokens.push(new Token(match.index, duration.end))
            }
        });
        return tokens;
    }

    private matchNight(source: string): Array<Token> {
        let tokens: Array<Token> = new Array<Token>();
        RegExpUtility.getMatches(this.config.specificTimeOfDayRegex, source).forEach(match => {
            tokens.push(new Token(match.index, match.index + match.length))
        });
        this.config.singleDateExtractor.extract(source).forEach(er => {
            let afterStr = source.substr(er.start + er.length);
            let match = RegExpUtility.getMatches(this.config.periodTimeOfDayWithDateRegex, afterStr).pop();
            if (match && isNullOrWhitespace(afterStr.substr(0, match.index))) {
                tokens.push(new Token(er.start, er.start + er.length + match.index + match.length))
            }
            let beforeStr = source.substr(0, er.start);
            match = RegExpUtility.getMatches(this.config.periodTimeOfDayWithDateRegex, beforeStr).pop();
            if (match && isNullOrWhitespace(beforeStr.substr(match.index + match.length))) {
                let middleStr = source.substr(match.index + match.length, er.start - match.index - match.length);
                if (isWhitespace(middleStr)) {
                    tokens.push(new Token(match.index, er.start + er.length))
                }
            };
        });
        return tokens;
    }

    private matchRelativeUnit(source: string): Array<Token> {
        let tokens: Array<Token> = new Array<Token>();
        RegExpUtility.getMatches(this.config.relativeTimeUnitRegex, source).forEach(match => {
            tokens.push(new Token(match.index, match.index + match.length));
        });
        return tokens;
    }
}

export class BaseSetExtractor implements IExtractor {
    private readonly extractorName = Constants.SYS_DATETIME_SET

    private readonly config: ISetExtractorConfiguration;

    constructor(config: ISetExtractorConfiguration) {
        this.config = config;
    }

    extract(source: string): Array<ExtractResult> {
        let tokens: Array<Token> = new Array<Token>()
            .concat(this.matchEachUnit(source))
            .concat(this.matchEachDuration(source))
            .concat(this.timeEveryday(source))
            .concat(this.matchEach(this.config.dateExtractor, source))
            .concat(this.matchEach(this.config.timeExtractor, source))
            .concat(this.matchEach(this.config.dateTimeExtractor, source))
            .concat(this.matchEach(this.config.datePeriodExtractor, source))
            .concat(this.matchEach(this.config.timePeriodExtractor, source))
            .concat(this.matchEach(this.config.dateTimePeriodExtractor, source))
        let result = Token.mergeAllTokens(tokens, source, this.extractorName);
        return result;
    }

    private matchEachUnit(source: string): Array<Token> {
        let ret = [];
        RegExpUtility.getMatches(this.config.periodicRegex, source).forEach(match => {
            ret.push(new Token(match.index, match.index + match.length))
        });
        RegExpUtility.getMatches(this.config.eachUnitRegex, source).forEach(match => {
            ret.push(new Token(match.index, match.index + match.length))
        });
        return ret;
    }

    private matchEachDuration(source: string): Array<Token> {
        let ret = [];
        this.config.durationExtractor.extract(source).forEach(er => {
            if (RegExpUtility.getMatches(this.config.lastRegex, er.text).length > 0) return;
            let beforeStr = source.substr(0, er.start);
            let matches = RegExpUtility.getMatches(this.config.eachPrefixRegex, beforeStr);
            if (matches && matches.length > 0) {
                ret.push(new Token(matches[0].index, er.start + er.length))
            }
        });
        return ret;
    }

    private timeEveryday(source: string): Array<Token> {
        let ret = [];
        this.config.timeExtractor.extract(source).forEach(er => {
            let afterStr = source.substr(er.start + er.length);
            if (isNullOrWhitespace(afterStr) && this.config.beforeEachDayRegex) {
                let beforeStr = source.substr(0, er.start);
                let beforeMatches = RegExpUtility.getMatches(this.config.beforeEachDayRegex, beforeStr);
                if (beforeMatches && beforeMatches.length > 0) {
                    ret.push(new Token(beforeMatches[0].index, er.start + er.length))
                }
            } else {
                let afterMatches = RegExpUtility.getMatches(this.config.eachDayRegex, afterStr);
                if (afterMatches && afterMatches.length > 0) {
                    ret.push(new Token(er.start, er.start + er.length + afterMatches[0].length))
                }
            }
        });
        return ret;
    }

    private matchEach(extractor: IExtractor, source: string): Array<Token> {
        let ret = [];
        extractor.extract(source).forEach(er => {
            let beforeStr = source.substr(0, er.start);
            let matches = RegExpUtility.getMatches(this.config.eachPrefixRegex, beforeStr);
            if (matches && matches.length > 0) {
                ret.push(new Token(matches[0].index, matches[0].index + matches[0].length + er.length))
            }
        });
        return ret;
    }
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

export class BaseMergedExtractor implements IExtractor {
    private readonly config: IMergedExtractorConfiguration;
    private readonly options: DateTimeOptions;

    constructor(config: IMergedExtractorConfiguration, options: DateTimeOptions) {
        this.config = config;
        this.options = options;
    }

    extract(source: string): Array<ExtractResult> {
        let result: Array<ExtractResult> = new Array<ExtractResult>();
        this.addTo(result, this.config.dateExtractor.extract(source), source);
        this.addTo(result, this.config.timeExtractor.extract(source), source);
        this.addTo(result, this.config.durationExtractor.extract(source), source);
        this.addTo(result, this.config.datePeriodExtractor.extract(source), source);
        this.addTo(result, this.config.dateTimeExtractor.extract(source), source);
        this.addTo(result, this.config.timePeriodExtractor.extract(source), source);
        this.addTo(result, this.config.dateTimePeriodExtractor.extract(source), source);
        this.addTo(result, this.config.setExtractor.extract(source), source);
        this.addTo(result, this.config.holidayExtractor.extract(source), source);
        this.addMod(result, source);

        result = result.sort((a, b) => a.start - b.start);
        return result;
    }

    private addTo(destination: ExtractResult[], source: ExtractResult[], text: string) {
        source.forEach(value => {
            if (this.options === DateTimeOptions.SkipFromToMerge && this.shouldSkipFromMerge(value)) return;
            if (this.filterAmbiguousSingleWord(value, text)) return;
            let isFound = false;
            let overlapIndexes = new Array<number>();
            let firstIndex = -1;
            destination.forEach((dest, index) => {
                if (ExtractResult.isOverlap(dest, value)) {
                    if (firstIndex == -1) {
                        firstIndex = index;
                    }
                    isFound = true;
                    if (value.length > dest.length) {
                        overlapIndexes.push(index);
                    }
                }
            });
            if (!isFound) {
                destination.push(value)
            } else if (overlapIndexes.length) {
                let tempDst = new Array<ExtractResult>();
                for (let i = 0; i < destination.length; i++) {
                    if (overlapIndexes.indexOf(i) === -1) {
                        tempDst.push(destination[i]);
                    }
                }

                //insert at the first overlap occurence to keep the order
                tempDst.splice(firstIndex, 0, value);
                destination.length = 0;
                destination.push.apply(destination, tempDst);
            }
        });
    }

    private shouldSkipFromMerge(er: ExtractResult): boolean {
        return RegExpUtility.getMatches(this.config.FromToRegex, er.text).length > 0;
    }

    private filterAmbiguousSingleWord(er: ExtractResult, text: string): boolean {
        let matches = RegExpUtility.getMatches(this.config.singleAmbiguousMonthRegex, er.text.toLowerCase())
        if (matches.length) {
            let stringBefore = text.substring(0, er.start).replace(/\s+$/, '');
            matches = RegExpUtility.getMatches(this.config.prepositionSuffixRegex, stringBefore);
            if (!matches.length) {
                return true;
            }
        }
        return false;
    }

    private addMod(ers: ExtractResult[], source: string) {
        let lastEnd = 0;
        ers.forEach(er => {
            let beforeStr = source.substr(lastEnd, er.start).toLowerCase();
            let before = this.hasTokenIndex(beforeStr.trim(), this.config.BeforeRegex);
            if (before.matched) {
                let modLength = beforeStr.length - before.index;
                er.length += modLength;
                er.start -= modLength;
                er.text = source.substr(er.start, er.length);
            }
            let after = this.hasTokenIndex(beforeStr.trim(), this.config.AfterRegex);
            if (after.matched) {
                let modLength = beforeStr.length - after.index;
                er.length += modLength;
                er.start -= modLength;
                er.text = source.substr(er.start, er.length);
            }
        });
    }

    private hasTokenIndex(source: string, regex: RegExp): { matched: boolean, index: number } {
        let result = { matched: false, index: -1 };
        let match = RegExpUtility.getMatches(regex, source).pop();
        if (match) {
            result.matched = true
            result.index = match.index
        }
        return result;
    }
}

export class BaseTimePeriodExtractor implements IExtractor {
    readonly extractorName = Constants.SYS_DATETIME_TIMEPERIOD; //"TimePeriod";
    readonly config: ITimePeriodExtractorConfiguration;

    constructor(config: ITimePeriodExtractorConfiguration) {
        this.config = config;
    }

    extract(source: string): Array<ExtractResult> {
        let tokens: Array<Token> = new Array<Token>()
            .concat(this.matchSimpleCases(source))
            .concat(this.mergeTwoTimePoints(source))
            .concat(this.matchNight(source));
        let result = Token.mergeAllTokens(tokens, source, this.extractorName);
        return result;
    }

    private matchSimpleCases(text: string): Array<Token> {
        let ret = [];
        this.config.simpleCasesRegex.forEach(regex => {
            let matches = RegExpUtility.getMatches(regex, text);
            matches.forEach(match => {
                // is there "pm" or "am" ?
                let pmStr = match.groups("pm").value;
                let amStr = match.groups("am").value;
                let descStr = match.groups("desc").value;
                // check "pm", "am"
                if (pmStr || amStr || descStr) {
                    ret.push(new Token(match.index, match.index + match.length));
                }
            });
        });
        return ret;
    }

    private mergeTwoTimePoints(text: string): Array<Token> {
        let ret = [];
        let ers = this.config.singleTimeExtractor.extract(text);

        // merge "{TimePoint} to {TimePoint}", "between {TimePoint} and {TimePoint}"
        let idx = 0;
        while (idx < ers.length - 1) {
            let middleBegin = ers[idx].start + ers[idx].length || 0;
            let middleEnd = ers[idx + 1].start || 0;

            let middleStr = text.substring(middleBegin, middleEnd).trim().toLowerCase();
            let matches = RegExpUtility.getMatches(this.config.tillRegex, middleStr);
            // handle "{TimePoint} to {TimePoint}"
            if (matches.length > 0 && matches[0].index === 0 && matches[0].length === middleStr.length) {
                let periodBegin = ers[idx].start || 0;
                let periodEnd = (ers[idx + 1].start || 0) + (ers[idx + 1].length || 0);

                // handle "from"
                let beforeStr = text.substring(0, periodBegin).trim().toLowerCase();
                let fromIndex = this.config.getFromTokenIndex(beforeStr);
                if (fromIndex.matched) {
                    periodBegin = fromIndex.index;
                }

                ret.push(new Token(periodBegin, periodEnd));
                idx += 2;
                continue;
            }
            // handle "between {TimePoint} and {TimePoint}"
            if (this.config.hasConnectorToken(middleStr)) {
                let periodBegin = ers[idx].start || 0;
                let periodEnd = (ers[idx + 1].start || 0) + (ers[idx + 1].length || 0);

                // handle "between"
                let beforeStr = text.substring(0, periodBegin).trim().toLowerCase();
                let betweenIndex = this.config.getBetweenTokenIndex(beforeStr, );
                if (betweenIndex.matched) {
                    periodBegin = betweenIndex.index;
                    ret.push(new Token(periodBegin, periodEnd));
                    idx += 2;
                    continue;
                }
            }

            idx++;
        }

        return ret;
    }

    private matchNight(source: string): Array<Token> {
        let ret = [];
        let matches = RegExpUtility.getMatches(this.config.timeOfDayRegex, source);
        matches.forEach(match => {
            ret.push(new Token(match.index, match.index + match.length));
        });
        return ret;
    }
}

export interface ITimePeriodExtractorConfiguration {
    simpleCasesRegex: RegExp[];
    tillRegex: RegExp;
    timeOfDayRegex: RegExp;
    singleTimeExtractor: IExtractor;
    getFromTokenIndex(text: string): { matched: boolean, index: number };
    hasConnectorToken(text: string): boolean;
    getBetweenTokenIndex(text: string): { matched: boolean, index: number };
}

export interface ISetExtractorConfiguration {
    lastRegex: RegExp;
    eachPrefixRegex: RegExp;
    periodicRegex: RegExp;
    eachUnitRegex: RegExp;
    eachDayRegex: RegExp;
    beforeEachDayRegex: RegExp;
    durationExtractor: IExtractor;
    timeExtractor: IExtractor;
    dateExtractor: IExtractor;
    dateTimeExtractor: IExtractor;
    datePeriodExtractor: IExtractor;
    timePeriodExtractor: IExtractor;
    dateTimePeriodExtractor: IExtractor;
}