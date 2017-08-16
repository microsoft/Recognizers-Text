import { Constants } from "./constants";
import { Token, AgoLaterUtil, IDateTimeUtilityConfiguration } from "./utilities";
import { IExtractor, ExtractResult, BaseNumberExtractor } from "../number/extractors"
import { BaseNumberParser } from "../number/parsers"
import { Match, RegExpUtility } from "../utilities";
import { BaseDateTime } from "../resources/baseDateTime";
import * as _ from "lodash";

export interface IDateExtractorConfiguration {
    dateRegexList: RegExp[],
    implicitDateList: RegExp[],
    MonthEnd: RegExp,
    OfMonth: RegExp,
    NonDateUnitRegex: RegExp,
    ordinalExtractor: BaseNumberExtractor,
    integerExtractor: BaseNumberExtractor,
    numberParser: BaseNumberParser
    durationExtractor: IExtractor
    dateTimeUtilityConfiguration: IDateTimeUtilityConfiguration
}

export interface IDurationExtractorConfiguration {
    AllRegex: RegExp,
    HalfRegex: RegExp,
    FollowedUnit: RegExp,
    NumberCombinedWithUnit: RegExp,
    AnUnitRegex: RegExp,
    SuffixAndRegex: RegExp,
    cardinalExtractor: BaseNumberExtractor
}

export class BaseDateExtractor implements IExtractor {
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
        let result = Token.mergeAllTokens(tokens, source, Constants.SYS_DATETIME_DATE);
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
                let match = RegExpUtility.getMatches(this.config.MonthEnd, frontString)[0];
                if (match && match.length) {
                    ret.push(new Token(match.index, match.index + match.length + result.length));
                    return;
                }
            }
            if (result.start + result.length < source.length) {
                let afterString = source.substring(result.start + result.length);
                let match = RegExpUtility.getMatches(this.config.OfMonth, afterString)[0];
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
            let match = RegExpUtility.getMatches(this.config.NonDateUnitRegex, er.text);
            if (match.length > 0) return;
            ret = AgoLaterUtil.extractorDurationWithBeforeAndAfter(source, er, ret, this.config.dateTimeUtilityConfiguration);
        });
        return ret;
    }
}

export interface ITimeExtractorConfiguration {
    timeRegexList: RegExp[]
    atRegex: RegExp
    ishRegex: RegExp
}

export class BaseTimeExtractor implements IExtractor {
    readonly extractorName = Constants.SYS_DATETIME_TIME; // "Time";
    readonly hourRegex = RegExpUtility.getSafeRegExp(BaseDateTime.HourRegex, "gis");
    readonly minuteRegex = RegExpUtility.getSafeRegExp(BaseDateTime.MinuteRegex, "gis");
    readonly secondRegex = RegExpUtility.getSafeRegExp(BaseDateTime.SecondRegex, "gis");

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
                text.charAt(match.index + match.length) == '%') {
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
    private readonly config: IDurationExtractorConfiguration;

    constructor(config: IDurationExtractorConfiguration) {
        this.config = config;
    }

    extract(input: string): Array<ExtractResult> {
        let baseTokens = this.numberWithUnit(input);
        let tokens: Array<Token> = new Array<Token>()
            .concat(baseTokens)
            .concat(this.numberWithUnitAndSuffix(input, baseTokens))
            .concat(this.implicitDuration(input))
        let result = Token.mergeAllTokens(tokens, input, Constants.SYS_DATETIME_DURATION);
        return result;
    }

    private numberWithUnit(source: string): Array<Token> {
        return this.config.cardinalExtractor.extract(source)
            .map(o => {
                let afterString = source.substring(o.start + o.length);
                let match = RegExpUtility.getMatches(this.config.FollowedUnit, afterString)[0];
                if (match && match.index === 0) {
                    return new Token(o.start | 0, o.start + o.length + match.length);
                }
            }).filter(o => o !== undefined)
            .concat(this.getTokensFromRegex(this.config.NumberCombinedWithUnit, source))
            .concat(this.getTokensFromRegex(this.config.AnUnitRegex, source));
    }

    private numberWithUnitAndSuffix(source: string, ers: Token[]): Array<Token> {
        return ers.map(o => {
            let afterString = source.substring(o.start + o.length);
            let match = RegExpUtility.getMatches(this.config.SuffixAndRegex, afterString)[0];
            if (match && match.index === 0) {
                return new Token(o.start | 0, o.start + o.length + match.length);
            }
        });
    }

    private implicitDuration(source: string): Array<Token> {
        return this.getTokensFromRegex(this.config.AllRegex, source)
            .concat(this.getTokensFromRegex(this.config.HalfRegex, source));
    }

    private getTokensFromRegex(regexp: RegExp, source: string): Array<Token> {
        return RegExpUtility.getMatches(regexp, source)
            .map(o => new Token(o.index, o.index + o.length));
    }
}

export interface IDatePeriodExtractorConfiguration {
    readonly simpleCasesRegexes: RegExp[]
    readonly tillRegex: RegExp
    readonly followedUnit: RegExp
    readonly numberCombinedWithUnit: RegExp
    readonly pastRegex: RegExp
    readonly futureRegex: RegExp
    readonly weekOfRegex: RegExp
    readonly monthOfRegex: RegExp
    readonly datePointExtractor: IExtractor
    readonly cardinalExtractor: IExtractor
    getFromTokenIndex(text: string): TokenIndex
    hasConnectorToken(text: string)
    getBetweenTokenIndex(text: string): TokenIndex
}

export class TokenIndex {
    readonly found: boolean;
    readonly index: number;

    constructor(found: boolean, index: number) {
        this.found = found;
        this.index = index;
    }
}

export class BaseDatePeriodExtractor implements IExtractor {
    private readonly extractorName = Constants.SYS_DATETIME_DATEPERIOD; // "DatePeriod";
    private readonly config: IDatePeriodExtractorConfiguration;

    constructor(config: IDatePeriodExtractorConfiguration) {
        this.config = config;
    }

    extract(input: string): Array<ExtractResult> {
        let tokens: Array<Token> = new Array<Token>()
            .concat(this.matchSimpleCases(input))
            .concat(this.mergeTwoTimePoints(input))
            .concat(this.matchNumberWithUnit(input))
            .concat(this.singleTimePointWithPatterns(input));

        let result = Token.mergeAllTokens(tokens, input, this.extractorName);
        return result;

    }

    private matchSimpleCases(source: string): Array<Token> {
        let ret = [];
        this.config.simpleCasesRegexes.forEach(regexp => {
            let matches = RegExpUtility.getMatches(regexp, source);
            matches.forEach(match => {
                ret.push(new Token(match.index, match.index + match.length));
            });
        });
        return ret;
    }

    private mergeTwoTimePoints(source: string): Array<Token> {
        let ret = [];
        let er = this.config.datePointExtractor.extract(source);
        if (er.length <= 1) {
            return ret;
        }

        // merge '{TimePoint} to {TimePoint}'
        let idx = 0;
        while (idx < er.length - 1) {
            let middleBegin = er[idx].start + er[idx].length || 0;
            let middleEnd = er[idx + 1].start || 0;
            if (middleBegin >= middleEnd) {
                idx++;
                continue;
            }

            let middleStr = source.substring(middleBegin, middleEnd - middleBegin).trim().toLowerCase();
            let matches = RegExpUtility.getMatches(this.config.tillRegex, middleStr);
            if (matches.length > 0 && matches[0].index == 0 && matches[0].length == middleStr.length) {
                let periodBegin = er[idx].start || 0;
                let periodEnd = (er[idx + 1].start || 0) + (er[idx + 1].length || 0);

                // handle "desde"
                let beforeStr = source.substring(0, periodBegin).trim().toLowerCase();
                let fromTokenIndex = this.config.getFromTokenIndex(beforeStr);
                if (fromTokenIndex.found) {
                    periodBegin = fromTokenIndex.index;
                }

                ret.push(new Token(periodBegin, periodEnd));
                idx += 2;
                continue;
            }

            if (this.config.hasConnectorToken(middleStr)) {
                let periodBegin = er[idx].start || 0;
                let periodEnd = (er[idx + 1].start || 0) + (er[idx + 1].length || 0);

                // handle "entre"
                let beforeStr = source.substring(0, periodBegin).trim().toLowerCase();
                let beforeTokenIndex = this.config.getBetweenTokenIndex(beforeStr);
                if (beforeTokenIndex.found) {
                    periodBegin = beforeTokenIndex.index;
                }

                ret.push(new Token(periodBegin, periodEnd));
                idx += 2;
                continue;
            }
            idx++;
        }

        return ret;
    }

    //Extract the month of date, week of date to a date range
    private singleTimePointWithPatterns(source: string): Array<Token> {
        let ret = [];
        let er = this.config.datePointExtractor.extract(source);
        if (er.length < 1) {
            return ret;
        }

        er.forEach(extractionResult => {
            if (extractionResult.start != null && extractionResult.length != null) {
                let beforeString = source.substring(0, extractionResult.start);
                ret.push(this.getTokenForRegexMatching(beforeString, this.config.weekOfRegex, extractionResult));
                ret.push(this.getTokenForRegexMatching(beforeString, this.config.monthOfRegex, extractionResult));
            }
        });

        return ret;
    }

    private getTokenForRegexMatching(text: string, regex: RegExp, er: ExtractResult) {
        let ret = [];
        let matches = RegExpUtility.getMatches(regex, text);
        if (matches.length > 0 && text.trim().endsWith(matches[0].value.trim())) {
            let startIndex = text.lastIndexOf(matches[0].value);
            ret.push(new Token(startIndex, er.start + er.length));
        }
        return ret;
    }

    private matchNumberWithUnit(source: string): Array<Token> {
        let ret = [];
        let durations = [];

        let ers = this.config.cardinalExtractor.extract(source);
        ers.forEach(er => {
            let afterStr = source.substring(er.start + er.length || 0);
            let matches = RegExpUtility.getMatches(this.config.followedUnit, afterStr);
            if (matches.length > 0 && matches[0].index == 0) {
                durations.push(new Token(er.start || 0, (er.start + er.length || 0) + matches[0].length));
            }
        });

        let matches = RegExpUtility.getMatches(this.config.numberCombinedWithUnit, source);
        if (matches.length > 0) {
            matches.forEach(match => {
                durations.push(new Token(match.index, match.index + match.length));
            });
        }

        durations.forEach(duration => {
            let beforeStr = source.substring(0, duration.Start).toLowerCase();
            if (!beforeStr) {
                return;
            }

            let matches = RegExpUtility.getMatches(this.config.pastRegex, beforeStr);
            if (matches.length > 0 && !(beforeStr.substring(matches[0].index + matches[0].length))) {
                ret.push(new Token(matches[0].index, duration.end));
                return;
            }

            matches = RegExpUtility.getMatches(this.config.futureRegex, beforeStr);
            if (matches.length > 0 && !(beforeStr.substring(matches[0].index + matches[0].length))) {
                ret.push(new Token(matches[0].index, duration.End));
            }
        });

        return ret;
    }
}