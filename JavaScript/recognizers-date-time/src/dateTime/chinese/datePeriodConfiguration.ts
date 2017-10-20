import { IExtractor, ExtractResult, BaseNumberExtractor, ChineseIntegerExtractor, RegExpUtility, StringUtility } from "recognizers-text-number"
import { IDatePeriodExtractorConfiguration, BaseDatePeriodExtractor } from "../baseDatePeriod";
import { ChineseDateExtractor } from "./dateConfiguration";
import { BaseDurationExtractor, BaseDurationParser } from "../baseDuration"
import { ChineseDurationExtractor } from "./durationConfiguration";
import { Token, IDateTimeUtilityConfiguration } from "../utilities";
import { ChineseDateTime } from "../../resources/chineseDateTime";

export class ChineseDatePeriodExtractorConfiguration implements IDatePeriodExtractorConfiguration {
    readonly simpleCasesRegexes: RegExp[]
    readonly tillRegex: RegExp
    readonly followedUnit: RegExp
    readonly numberCombinedWithUnit: RegExp
    readonly pastRegex: RegExp
    readonly futureRegex: RegExp
    readonly weekOfRegex: RegExp
    readonly monthOfRegex: RegExp
    readonly dateUnitRegex: RegExp
    readonly inConnectorRegex: RegExp
    readonly rangeUnitRegex: RegExp
    readonly datePointExtractor: ChineseDateExtractor
    readonly integerExtractor: BaseNumberExtractor
    readonly durationExtractor: BaseDurationExtractor
    readonly rangeConnectorRegex: RegExp

    constructor() {
        this.simpleCasesRegexes = [
            RegExpUtility.getSafeRegExp(ChineseDateTime.SimpleCasesRegex),
            RegExpUtility.getSafeRegExp(ChineseDateTime.OneWordPeriodRegex),
            RegExpUtility.getSafeRegExp(ChineseDateTime.StrictYearRegex),
            RegExpUtility.getSafeRegExp(ChineseDateTime.YearToYear),
            RegExpUtility.getSafeRegExp(ChineseDateTime.YearAndMonth),
            RegExpUtility.getSafeRegExp(ChineseDateTime.PureNumYearAndMonth),
            RegExpUtility.getSafeRegExp(ChineseDateTime.DatePeriodYearInChineseRegex),
            RegExpUtility.getSafeRegExp(ChineseDateTime.WeekOfMonthRegex),
            RegExpUtility.getSafeRegExp(ChineseDateTime.SeasonWithYear),
            RegExpUtility.getSafeRegExp(ChineseDateTime.QuarterRegex),
        ];
        this.datePointExtractor = new ChineseDateExtractor();
        this.integerExtractor = new ChineseIntegerExtractor();
        this.tillRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.DatePeriodTillRegex)
        this.followedUnit = RegExpUtility.getSafeRegExp(ChineseDateTime.FollowedUnit);
        this.numberCombinedWithUnit = RegExpUtility.getSafeRegExp(ChineseDateTime.NumberCombinedWithUnit);
        this.pastRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.PastRegex);
        this.futureRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.FutureRegex);
    }

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

    hasConnectorToken(source: string) {
        return false;
    }
}

export class ChineseDatePeriodExtractor extends BaseDatePeriodExtractor {
    constructor() {
        super(new ChineseDatePeriodExtractorConfiguration());
    }

    extract(source: string): Array<ExtractResult> {
        let tokens: Array<Token> = new Array<Token>()
            .concat(super.matchSimpleCases(source))
            .concat(super.mergeTwoTimePoints(source))
            .concat(this.matchNumberWithUnit(source));
        let result = Token.mergeAllTokens(tokens, source, this.extractorName);
        return result;
    }

    private matchNumberWithUnit(source: string): Array<Token> {
        let tokens: Array<Token> = new Array<Token>();
        let durations = new Array<Token>();
        this.config.integerExtractor.extract(source).forEach(er => {
            let afterStr = source.substr(er.start + er.length);
            let followedUnitMatch = RegExpUtility.getMatches(this.config.followedUnit, afterStr).pop();
            if (followedUnitMatch && followedUnitMatch.index === 0) {
                durations.push(new Token(er.start, er.start + er.length + followedUnitMatch.length));
            }
        });

        RegExpUtility.getMatches(this.config.numberCombinedWithUnit, source).forEach(match => {
            durations.push(new Token(match.index, match.index + match.length));
        });

        durations.forEach(duration => {
            let beforeStr = source.substr(0, duration.start).toLowerCase();
            if (StringUtility.isNullOrWhitespace(beforeStr)) {
                return;
            }

            let match = RegExpUtility.getMatches(this.config.pastRegex, beforeStr).pop();
            if (match && StringUtility.isNullOrWhitespace(beforeStr.substr(match.index + match.length))) {
                tokens.push(new Token(match.index, duration.end));
                return;
            }

            match = RegExpUtility.getMatches(this.config.futureRegex, beforeStr).pop();
            if (match && StringUtility.isNullOrWhitespace(beforeStr.substr(match.index + match.length))) {
                tokens.push(new Token(match.index, duration.end));
                return;
            }
        });

        return tokens;
    }
}