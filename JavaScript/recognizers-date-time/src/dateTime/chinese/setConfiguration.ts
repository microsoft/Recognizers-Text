import { IExtractor, ExtractResult, BaseNumberParser, BaseNumberExtractor, RegExpUtility } from "recognizers-text-number"
import { ISetExtractorConfiguration, BaseSetExtractor } from "../baseSet";
import { BaseDurationExtractor, BaseDurationParser } from "../baseDuration"
import { BaseDateExtractor, BaseDateParser } from "../baseDate";
import { BaseTimeExtractor, BaseTimeParser } from "../baseTime";
import { BaseDatePeriodExtractor, BaseDatePeriodParser } from "../baseDatePeriod";
import { BaseTimePeriodExtractor, BaseTimePeriodParser } from "../baseTimePeriod";
import { BaseDateTimeExtractor, BaseDateTimeParser } from "../baseDateTime";
import { BaseDateTimePeriodExtractor, BaseDateTimePeriodParser } from "../baseDateTimePeriod";
import { ChineseDurationExtractor } from "./durationConfiguration";
import { ChineseTimeExtractor } from "./timeConfiguration";
import { ChineseDateExtractor } from "./dateConfiguration";
import { ChineseDateTimeExtractor } from "./dateTimeConfiguration";
import { Token, IDateTimeUtilityConfiguration } from "../utilities";
import { ChineseDateTime } from "../../resources/chineseDateTime";

class ChineseSetExtractorConfiguration implements ISetExtractorConfiguration {
    readonly lastRegex: RegExp;
    readonly eachPrefixRegex: RegExp;
    readonly periodicRegex: RegExp;
    readonly eachUnitRegex: RegExp;
    readonly eachDayRegex: RegExp;
    readonly beforeEachDayRegex: RegExp;
    readonly setWeekDayRegex: RegExp;
    readonly setEachRegex: RegExp;
    readonly durationExtractor: ChineseDurationExtractor;
    readonly timeExtractor: ChineseTimeExtractor;
    readonly dateExtractor: ChineseDateExtractor;
    readonly dateTimeExtractor: BaseDateTimeExtractor;
    readonly datePeriodExtractor: BaseDatePeriodExtractor;
    readonly timePeriodExtractor: BaseTimePeriodExtractor;
    readonly dateTimePeriodExtractor: BaseDateTimePeriodExtractor;

    constructor() {
        this.eachUnitRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.SetEachUnitRegex);
        this.durationExtractor = new ChineseDurationExtractor();
        this.lastRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.SetLastRegex);
        this.eachPrefixRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.SetEachPrefixRegex);
        this.timeExtractor = new ChineseTimeExtractor();
        this.beforeEachDayRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.SetEachDayRegex);
        this.eachDayRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.SetEachDayRegex);
        this.dateExtractor = new ChineseDateExtractor();
        this.dateTimeExtractor = new ChineseDateTimeExtractor();
    }
}

export class ChineseSetExtractor extends BaseSetExtractor {

    constructor() {
        super(new ChineseSetExtractorConfiguration());
    }

    extract(source: string): Array<ExtractResult> {
        let tokens: Array<Token> = new Array<Token>()
            .concat(super.matchEachUnit(source))
            .concat(super.matchEachDuration(source))
            .concat(this.matchEachSpecific(this.config.timeExtractor, this.config.eachDayRegex, source))
            .concat(this.matchEachSpecific(this.config.dateExtractor, this.config.eachPrefixRegex, source))
            .concat(this.matchEachSpecific(this.config.dateTimeExtractor, this.config.eachPrefixRegex, source))
        let result = Token.mergeAllTokens(tokens, source, this.extractorName);
        return result;
    }

    private matchEachSpecific(extractor: IExtractor, eachRegex: RegExp, source: string) {
        let ret = [];
        extractor.extract(source).forEach(er => {
            let beforeStr = source.substr(0, er.start);
            let beforeMatch = RegExpUtility.getMatches(eachRegex, beforeStr).pop();
            if (beforeMatch) {
                ret.push(new Token(beforeMatch.index, er.start + er.length))
            }
        });
        return ret;
    }
}