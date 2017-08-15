import { Constants } from "./constants";
import { Token } from "./utilities";
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
    ordinalExtractor: BaseNumberExtractor,
    integerExtractor: BaseNumberExtractor,
    numberParser: BaseNumberParser
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
        // TODO add support for duration extractor
        return ret;
    }
}

export interface ITimeExtractorConfiguration {
    timeRegexList: RegExp[]
    atRegex: RegExp
    ishRegex: RegExp
}

export interface IDurationExtractorConfiguration {
    followedUnit: RegExp
    numberCombinedWithUnit: RegExp
    anUnitRegex: RegExp
    allRegex: RegExp
    halfRegex: RegExp
    suffixAndRegex: RegExp
    cardinalExtractor: IExtractor
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
    readonly extractorName = Constants.SYS_DATETIME_DURATION;
    readonly config: IDurationExtractorConfiguration;

    constructor(config: IDurationExtractorConfiguration) {
        this.config = config;
    }

    extract(text: string): Array<ExtractResult> {
        let tokens: Array<Token> = new Array<Token>()
            .concat(this.numberWithUnit(text))
            .concat(this.numberWithUnitAndSuffix(text, this.numberWithUnit(text)))
            .concat(this.implicitDuration(text));

        let result = Token.mergeAllTokens(tokens, text, this.extractorName);
        return result;
    }

    // handle cases look like: {number} {unit}? and {an|a} {half|quarter} {unit}?
    // define the part "and {an|a} {half|quarter}" as Suffix
    numberWithUnitAndSuffix(text: string, ers: Array<Token>): Array<Token> {
        let ret = [];
        ers.forEach(er => {
            let afterStr = text.substring(er.start + er.length);
            let matches = RegExpUtility.getMatches(this.config.suffixAndRegex, afterStr);
            if (matches.length > 0 && matches[0].index == 0) {
                ret.push(new Token(matches[0].index, matches[0].index + matches[0].length));
            }
        });
        return ret;
    }

    // simple cases made by a number followed an unit
    numberWithUnit(text: string): Array<Token> {
        let ret = [];
        let ers = this.config.cardinalExtractor.extract(text);
        ers.forEach(er => {
            let afterStr = text.substring(er.start + er.length);
            let matches = RegExpUtility.getMatches(this.config.followedUnit, afterStr);
            if (matches.length > 0 && matches[0].index == 0) {
                ret.push(new Token(matches[0].index, matches[0].index + matches[0].length));
            }
        });

        // handle "3hrs"
        let matches = RegExpUtility.getMatches(this.config.numberCombinedWithUnit, text);
        matches.forEach(match => {
            ret.push(new Token(match.index, match.index + match.length));
        });

        // handle "an hour"
        matches = RegExpUtility.getMatches(this.config.anUnitRegex, text);
        matches.forEach(match => {
            ret.push({ start: match.index, end: match.index + match.length });
        });

        return ret;
    }

    // handle cases that don't contain nubmer
    implicitDuration(text: string): Array<Token> {
        let ret = [];
        // handle "all day", "all year"
        ret.push(this.getTokenFromRegex(this.config.allRegex, text));

        // handle "half day", "half year"
        ret.push(this.getTokenFromRegex(this.config.halfRegex, text));

        return ret;
    }

    getTokenFromRegex(regex: RegExp, text: string) {
        let ret = [];
        var matches = RegExpUtility.getMatches(regex, text);
        matches.forEach(match => {
            ret.push(new Token(match.index, match.index + match.length));
        });
        return ret;
    }
}