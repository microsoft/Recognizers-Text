import { Constants } from "./constants";
import { Token, AgoLaterUtil, IDateTimeUtilityConfiguration } from "./utilities";
import { IExtractor, ExtractResult, BaseNumberExtractor } from "../number/extractors"
import { BaseNumberParser } from "../number/parsers"
import { Match, RegExpUtility } from "../utilities";
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

export interface IDatePeriodExtractorConfiguration {
    simpleCasesRegexes: RegExp[]
    tillRegex: RegExp
    followedUnit: RegExp
    numberCombinedWithUnit: RegExp
    pastRegex: RegExp
    futureRegex: RegExp
    weekOfRegex: RegExp
    monthOfRegex: RegExp
    datePointExtractor: BaseDateExtractor
    cardinalExtractor: BaseNumberExtractor
    getFromTokenIndex(source: string): {matched: boolean, index: number};
    getBetweenTokenIndex(source: string): {matched: boolean, index: number};
    hasConnectorToken(source: string): boolean;
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

export class BaseDurationExtractor implements IExtractor {
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
        let result = Token.mergeAllTokens(tokens, source, Constants.SYS_DATETIME_DURATION);
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

    private implicitDuration(source: string) : Array<Token> {
        return this.getTokensFromRegex(this.config.AllRegex, source)
            .concat(this.getTokensFromRegex(this.config.HalfRegex, source));
    }

    private getTokensFromRegex(regexp: RegExp, source: string): Array<Token> {
        return RegExpUtility.getMatches(regexp, source)
            .map(o => new Token(o.index, o.index + o.length));
    }
}

export class BaseDatePeriodExtractor implements IExtractor {
    private readonly config: IDatePeriodExtractorConfiguration;

    constructor(config: IDatePeriodExtractorConfiguration) {
        this.config = config;
    }

    extract(source: string): Array<ExtractResult> {
        let tokens: Array<Token> = new Array<Token>()
            .concat(this.matchSimpleCases(source))
            .concat(this.mergeTwoTimePoints(source))
            .concat(this.matchNumberWithUnit(source))
            .concat(this.singleTimePointWithPatterns(source));
        let result = Token.mergeAllTokens(tokens, source, Constants.SYS_DATETIME_DATEPERIOD);
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
        while(idx < er.length - 1) {
            let middleBegin = er[idx].start + er[idx].length;
            let middleEnd = er[idx + 1].start;
            if (middleBegin >= middleEnd) {
                idx++;
                continue;
            }
            let middleStr = source.substring(middleBegin, middleEnd - middleBegin).trim().toLowerCase();
            let match = RegExpUtility.getMatches(this.config.tillRegex, middleStr);
            if (match && match.length > 0 && match[0].index ===0 && match[0].length === middleStr.length) {
                let periodBegin = er[idx].start;
                let periodEnd = er[idx + 1].start + er[idx + 1].length;

                let beforeStr = source.substring(0, periodBegin).trim().toLowerCase();
                let fromTokenIndex = this.config.getFromTokenIndex(beforeStr);
                if (fromTokenIndex.matched) {
                    periodBegin = fromTokenIndex.index;
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
                if (betweenTokenIndex.matched)
                {
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

    private matchNumberWithUnit(source: string): Array<Token> {
        let tokens: Array<Token> = new Array<Token>();
        let duration: Array<Token> = new Array<Token>();
        let ers = this.config.cardinalExtractor.extract(source);
        ers.forEach(er => {
            let afterStr = source.substring(er.start + er.length);
            let match = RegExpUtility.getMatches(this.config.followedUnit, afterStr);
            if (match && match.length > 0 && match[0].index === 0) {
                duration.push(new Token(er.start, er.start + er.length + match[0].length));
            }
        });
        let matches = RegExpUtility.getMatches(this.config.numberCombinedWithUnit, source);
        if (matches && matches.length > 0) {
            matches.forEach(match => {
                duration.push(new Token(match.index, match.index + match.length));
            });
        }
        duration.forEach(duration => {
            let beforeStr = source.substring(0, duration.start).toLowerCase();
            if (isNullOrWhitespace(beforeStr)) return;
            let match = RegExpUtility.getMatches(this.config.pastRegex, beforeStr)
            if (match && match.length > 0 && isNullOrWhitespace(beforeStr.substring(match[0].index + match[0].length))) {
                tokens.push(new Token(match[0].index, duration.end));
            }
            match = RegExpUtility.getMatches(this.config.futureRegex, beforeStr)
            if (match && match.length > 0 && isNullOrWhitespace(beforeStr.substring(match[0].index + match[0].length))) {
                tokens.push(new Token(match[0].index, duration.end));
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
                tokens
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
}

function isNullOrWhitespace(input: string): boolean {
    return !input || !input.trim();
}