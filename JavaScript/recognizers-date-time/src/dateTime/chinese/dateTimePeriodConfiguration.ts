import { IExtractor, ExtractResult, BaseNumberParser, BaseNumberExtractor, RegExpUtility, StringUtility, ChineseCardinalExtractor } from "recognizers-text-number"
import { Constants, TimeTypeConstants } from "../constants";
import { IDateTimePeriodExtractorConfiguration, BaseDateTimePeriodExtractor } from "../baseDateTimePeriod";
import { BaseDurationExtractor, BaseDurationParser } from "../baseDuration"
import { BaseDateExtractor, BaseDateParser } from "../baseDate";
import { BaseTimeExtractor, BaseTimeParser } from "../baseTime";
import { BaseDatePeriodExtractor, BaseDatePeriodParser } from "../baseDatePeriod";
import { BaseTimePeriodExtractor, BaseTimePeriodParser } from "../baseTimePeriod";
import { ChineseDurationExtractor } from "./durationConfiguration";
import { ChineseTimeExtractor } from "./timeConfiguration";
import { ChineseDateExtractor } from "./dateConfiguration";
import { ChineseDateTimeExtractor } from "./dateTimeConfiguration";
import { Token, IDateTimeUtilityConfiguration } from "../utilities";
import { ChineseDateTime } from "../../resources/chineseDateTime";

export class ChineseDateTimePeriodExtractorConfiguration implements IDateTimePeriodExtractorConfiguration {
    readonly cardinalExtractor: ChineseCardinalExtractor
    readonly singleDateExtractor: BaseDateExtractor
    readonly singleTimeExtractor: ChineseTimeExtractor
    readonly singleDateTimeExtractor: ChineseDateTimeExtractor
    readonly durationExtractor: BaseDurationExtractor
    readonly simpleCasesRegexes: RegExp[]
    readonly prepositionRegex: RegExp
    readonly tillRegex: RegExp
    readonly specificTimeOfDayRegex: RegExp
    readonly timeOfDayRegex: RegExp
    readonly periodTimeOfDayWithDateRegex: RegExp
    readonly followedUnit: RegExp
    readonly numberCombinedWithUnit: RegExp
    readonly timeUnitRegex: RegExp
    readonly pastPrefixRegex: RegExp
    readonly nextPrefixRegex: RegExp
    readonly rangeConnectorRegex: RegExp
    readonly relativeTimeUnitRegex: RegExp
    readonly restOfDateTimeRegex: RegExp
    
    getFromTokenIndex(source: string) {
        let result = { matched: false, index: -1 };
        if (source.endsWith("从")) {
            result.index = source.lastIndexOf("从");
            result.matched = true;
        }
        return result;
    };

    getBetweenTokenIndex(source: string) {
        return { matched: false, index: -1 };
    };

    hasConnectorToken(source: string): boolean {
        return (source === '和' || source === ' 与' || source === '到');
    };

    constructor() {
        this.singleDateExtractor = new ChineseDateExtractor();
        this.singleTimeExtractor = new ChineseTimeExtractor();
        this.singleDateTimeExtractor = new ChineseDateTimeExtractor();
        this.prepositionRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.DateTimePeriodPrepositionRegex);
        this.tillRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.DateTimePeriodTillRegex);
        this.cardinalExtractor = new ChineseCardinalExtractor();
        this.followedUnit = RegExpUtility.getSafeRegExp(ChineseDateTime.DateTimePeriodFollowedUnit);
        this.timeUnitRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.DateTimePeriodUnitRegex);
        this.specificTimeOfDayRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.SpecificTimeOfDayRegex);
        this.timeOfDayRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.TimeOfDayRegex)
    }
}

export class ChineseDateTimePeriodExtractor extends BaseDateTimePeriodExtractor {
    private readonly zhijianRegex: RegExp;
    private readonly pastRegex: RegExp;
    private readonly futureRegex: RegExp;

    constructor() {
        super(new ChineseDateTimePeriodExtractorConfiguration());
        this.zhijianRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.ZhijianRegex);
        this.pastRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.PastRegex);
        this.futureRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.FutureRegex);
    }

    extract(source: string): Array<ExtractResult> {
        let tokens: Array<Token> = new Array<Token>()
        .concat(this.mergeDateAndTimePeriod(source))
        .concat(this.mergeTwoTimePoints(source))
        .concat(this.matchNubmerWithUnit(source))
        .concat(this.matchNight(source))
        let result = Token.mergeAllTokens(tokens, source, this.extractorName);
        return result;
    }

    private mergeDateAndTimePeriod(source: string): Array<Token> {
        let tokens: Array<Token> = new Array<Token>();
        let ersDate = this.config.singleDateExtractor.extract(source);
        let ersTime = this.config.singleTimeExtractor.extract(source);
        let timeResults = new Array<ExtractResult>();
        let j = 0;
        for (let i = 0; i < ersDate.length; i++) {
            timeResults.push(ersDate[i]);
            while (j < ersTime.length && ersTime[j].start + ersTime[j].length <= ersDate[i].start) {
                timeResults.push(ersTime[j]);
                j++;
            }

            while (j < ersTime.length && ExtractResult.isOverlap(ersTime[j], ersDate[i])) {
                j++;
            }
        }

        for (j; j < ersTime.length; j++) {
            timeResults.push(ersTime[j]);
        }
        timeResults = timeResults.sort((a, b) => a.start > b.start ? 1 : a.start < b.start ? -1 : 0);

        let idx = 0;
        while (idx < timeResults.length - 1) {
            let current = timeResults[idx];
            let next = timeResults[idx + 1];
            if (current.type === Constants.SYS_DATETIME_DATE && next.type === Constants.SYS_DATETIME_TIMEPERIOD) {
                let middleBegin = current.start + current.length;
                let middleEnd = next.start;
                let middleStr = source.substring(middleBegin, middleEnd).trim();
                if (StringUtility.isNullOrWhitespace(middleStr) || RegExpUtility.isMatch(this.config.prepositionRegex, middleStr)) {
                    let periodBegin = current.start;
                    let periodEnd = next.start + next.length;
                    tokens.push(new Token(periodBegin, periodEnd));
                }
                idx++;
            }
            idx++;
        }

        return tokens;
    }

    protected mergeTwoTimePoints(source: string): Array<Token> {
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
                let afterStr = source.substr(periodEnd).trim().toLowerCase();
                let match = RegExpUtility.getMatches(this.zhijianRegex, afterStr).pop();
                if (match) {
                    tokens.push(new Token(periodBegin, periodEnd + match.length))
                    idx += 2;
                    continue;
                }
            }
            idx++;
        };
        return tokens;
    }

    private matchNubmerWithUnit(source: string): Array<Token> {
        let tokens: Array<Token> = new Array<Token>();
        let durations = new Array<Token>();
        this.config.cardinalExtractor.extract(source).forEach(er => {
            let afterStr = source.substr(er.start + er.length);
            let followedUnitMatch = RegExpUtility.getMatches(this.config.followedUnit, afterStr).pop();
            if (followedUnitMatch && followedUnitMatch.index === 0) {
                durations.push(new Token(er.start, er.start + er.length + followedUnitMatch.length));
            }
        });

        RegExpUtility.getMatches(this.config.timeUnitRegex, source).forEach(match => {
            durations.push(new Token(match.index, match.index + match.length));
        });

        durations.forEach(duration => {
            let beforeStr = source.substr(0, duration.start).toLowerCase();
            if (StringUtility.isNullOrWhitespace(beforeStr)) {
                return;
            }

            let match = RegExpUtility.getMatches(this.pastRegex, beforeStr).pop();
            if (match && StringUtility.isNullOrWhitespace(beforeStr.substr(match.index + match.length))) {
                tokens.push(new Token(match.index, duration.end));
                return;
            }

            match = RegExpUtility.getMatches(this.futureRegex, beforeStr).pop();
            if (match && StringUtility.isNullOrWhitespace(beforeStr.substr(match.index + match.length))) {
                tokens.push(new Token(match.index, duration.end));
                return;
            }
        });

        return tokens;
    }

    protected matchNight(source: string): Array<Token> {
        let tokens: Array<Token> = new Array<Token>();
        RegExpUtility.getMatches(this.config.specificTimeOfDayRegex, source).forEach(match => {
            tokens.push(new Token(match.index, match.index + match.length))
        });

        this.config.singleDateExtractor.extract(source).forEach(er => {
            let afterStr = source.substr(er.start + er.length);
            let match = RegExpUtility.getMatches(this.config.timeOfDayRegex, afterStr).pop();
            if (match) {
                let middleStr = source.substr(0, match.index);
                if (StringUtility.isNullOrWhitespace(middleStr) || RegExpUtility.isMatch(this.config.prepositionRegex, middleStr)) {
                    tokens.push(new Token(er.start, er.start + er.length + match.index + match.length))
                }
            }
        });

        return tokens;
    }
}