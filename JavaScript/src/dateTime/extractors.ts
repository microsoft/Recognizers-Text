import { Constants } from "./constants";
import { Token, AgoLaterUtil, IDateTimeUtilityConfiguration } from "./utilities";
import { IExtractor, ExtractResult, BaseNumberExtractor } from "../number/extractors"
import { BaseNumberParser } from "../number/parsers"
import { Match, RegExpUtility, isNullOrWhitespace } from "../utilities";
import { BaseDateTime } from "../resources/baseDateTime";
import * as _ from "lodash";

export interface IDateExtractorConfiguration {
    dateRegexList: RegExp[],
    implicitDateList: RegExp[],
    monthEnd: RegExp,
    ofMonth: RegExp,
    nonDateUnitRegex: RegExp,
    ordinalExtractor: BaseNumberExtractor,
    integerExtractor: BaseNumberExtractor,
    numberParser: BaseNumberParser
    durationExtractor: IExtractor
    dateTimeUtilityConfiguration: IDateTimeUtilityConfiguration
}

export interface IDurationExtractorConfiguration {
    allRegex: RegExp,
    halfRegex: RegExp,
    followedUnit: RegExp,
    numberCombinedWithUnit: RegExp,
    anUnitRegex: RegExp,
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
    datePointExtractor: BaseDateExtractor
    cardinalExtractor: BaseNumberExtractor
    getFromTokenIndex(source: string): {matched: boolean, index: number};
    getBetweenTokenIndex(source: string): {matched: boolean, index: number};
    hasConnectorToken(source: string): boolean;
}

export interface ITimeExtractorConfiguration {
    timeRegexList: RegExp[]
    atRegex: RegExp
    ishRegex: RegExp
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
            let match = RegExpUtility.getMatches(this.config.nonDateUnitRegex, er.text);
            if (match.length > 0) return;
            ret = AgoLaterUtil.extractorDurationWithBeforeAndAfter(source, er, ret, this.config.dateTimeUtilityConfiguration);
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
            .concat(this.getTokensFromRegex(this.config.anUnitRegex, source));
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

    private implicitDuration(source: string) : Array<Token> {
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
            .concat(this.matchNumberWithUnit(source))
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
        while(idx < er.length - 1) {
            let middleBegin = er[idx].start + er[idx].length;
            let middleEnd = er[idx + 1].start;
            if (middleBegin >= middleEnd) {
                idx++;
                continue;
            }
            let middleStr = source.substr(middleBegin, middleEnd - middleBegin).trim().toLowerCase();
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
}