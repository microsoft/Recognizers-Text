import { IExtractor, ExtractResult, BaseNumberParser, BaseNumberExtractor, RegExpUtility, StringUtility } from "recognizers-text-number"
import { Constants, TimeTypeConstants } from "../constants";
import { IDateTimeExtractorConfiguration, BaseDateTimeExtractor } from "../baseDateTime";
import { BaseDurationExtractor, BaseDurationParser } from "../baseDuration"
import { BaseDateExtractor, BaseDateParser } from "../baseDate";
import { BaseTimeExtractor, BaseTimeParser } from "../baseTime";
import { BaseDatePeriodExtractor, BaseDatePeriodParser } from "../baseDatePeriod";
import { BaseTimePeriodExtractor, BaseTimePeriodParser } from "../baseTimePeriod";
import { BaseDateTimePeriodExtractor, BaseDateTimePeriodParser } from "../baseDateTimePeriod";
import { ChineseDurationExtractor } from "./durationConfiguration";
import { ChineseTimeExtractor } from "./timeConfiguration";
import { ChineseDateExtractor } from "./dateConfiguration";
import { Token, IDateTimeUtilityConfiguration } from "../utilities";
import { ChineseDateTime } from "../../resources/chineseDateTime";

class ChineseDateTimeExtractorConfiguration implements IDateTimeExtractorConfiguration {
    readonly datePointExtractor: ChineseDateExtractor
    readonly timePointExtractor: ChineseTimeExtractor
    readonly durationExtractor: BaseDurationExtractor
    readonly suffixRegex: RegExp
    readonly nowRegex: RegExp
    readonly timeOfTodayAfterRegex: RegExp
    readonly simpleTimeOfTodayAfterRegex: RegExp
    readonly nightRegex: RegExp
    readonly timeOfTodayBeforeRegex: RegExp
    readonly simpleTimeOfTodayBeforeRegex: RegExp
    readonly theEndOfRegex: RegExp
    readonly unitRegex: RegExp
    readonly prepositionRegex: RegExp
    readonly utilityConfiguration: IDateTimeUtilityConfiguration

    constructor() {
        this.datePointExtractor = new ChineseDateExtractor();
        this.timePointExtractor = new ChineseTimeExtractor();
        this.prepositionRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.PrepositionRegex);
        this.nowRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.NowRegex);
        this.nightRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.NightRegex);
        this.timeOfTodayBeforeRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.TimeOfTodayRegex);
    }
            
    isConnectorToken(source: string): boolean { 
        return StringUtility.isNullOrEmpty(source)
            || source === ','
            || RegExpUtility.isMatch(this.prepositionRegex, source)
    }
}

export class ChineseDateTimeExtractor extends BaseDateTimeExtractor {
    constructor() {
        super(new ChineseDateTimeExtractorConfiguration());
    }

    extract(source: string): Array<ExtractResult> {
        let tokens: Array<Token> = new Array<Token>()
        .concat(this.mergeDateAndTime(source))
        .concat(this.basicRegexMatch(source))
        .concat(this.timeOfToday(source));
        let result = Token.mergeAllTokens(tokens, source, this.extractorName);
        return result;
    }

    protected mergeDateAndTime(source: string): Array<Token> {
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
            if (ers[i].type === Constants.SYS_DATETIME_DATE && ers[j].type === Constants.SYS_DATETIME_TIME) {
                let middleBegin = ers[i].start + ers[i].length;
                let middleEnd = ers[j].start;
                if (middleBegin > middleEnd) {
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

        return tokens;
    }

    private timeOfToday(source: string): Array<Token> {
        let tokens: Array<Token> = new Array<Token>();
        this.config.timePointExtractor.extract(source).forEach(er => {
            let beforeStr = source.substr(0, er.start);
            let innerMatch = RegExpUtility.getMatches(this.config.nightRegex, er.text).pop();
            if (innerMatch && innerMatch.index === 0) {
                beforeStr = source.substr(0, er.start + innerMatch.length);
            }

            if (StringUtility.isNullOrWhitespace(beforeStr)) return;

            let match = RegExpUtility.getMatches(this.config.timeOfTodayBeforeRegex, beforeStr).pop();
            if (match && StringUtility.isNullOrWhitespace(beforeStr.substr(match.index + match.length))) {
                let begin = match.index;
                let end = er.start + er.length;
                tokens.push(new Token(begin, end));
            }
        });
        return tokens;
    }
}