import { IExtractor, ExtractResult, BaseNumberParser, BaseNumberExtractor, RegExpUtility, StringUtility } from "recognizers-text-number"
import { ISetExtractorConfiguration, BaseSetExtractor, ISetParserConfiguration, BaseSetParser } from "../baseSet";
import { BaseDurationExtractor, BaseDurationParser } from "../baseDuration"
import { IDateTimeParser, DateTimeParseResult } from "../parsers"
import { Constants, TimeTypeConstants } from "../constants";
import { BaseDateExtractor, BaseDateParser } from "../baseDate";
import { BaseTimeExtractor, BaseTimeParser } from "../baseTime";
import { BaseDatePeriodExtractor, BaseDatePeriodParser } from "../baseDatePeriod";
import { BaseTimePeriodExtractor, BaseTimePeriodParser } from "../baseTimePeriod";
import { BaseDateTimeExtractor, BaseDateTimeParser, IDateTimeExtractor } from "../baseDateTime";
import { BaseDateTimePeriodExtractor, BaseDateTimePeriodParser } from "../baseDateTimePeriod";
import { ChineseDurationExtractor, ChineseDurationParser } from "./durationConfiguration";
import { ChineseTimeExtractor, ChineseTimeParser } from "./timeConfiguration";
import { ChineseDateExtractor, ChineseDateParser } from "./dateConfiguration";
import { ChineseDateTimeExtractor, ChineseDateTimeParser } from "./dateTimeConfiguration";
import { Token, IDateTimeUtilityConfiguration, DateTimeResolutionResult, StringMap } from "../utilities";
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

    extract(source: string, refDate: Date): Array<ExtractResult> {
        if (!refDate) refDate = new Date();
        let referenceDate = refDate;

        let tokens: Array<Token> = new Array<Token>()
            .concat(super.matchEachUnit(source))
            .concat(super.matchEachDuration(source, referenceDate))
            .concat(this.matchEachSpecific(this.config.timeExtractor, this.config.eachDayRegex, source, referenceDate))
            .concat(this.matchEachSpecific(this.config.dateExtractor, this.config.eachPrefixRegex, source, referenceDate))
            .concat(this.matchEachSpecific(this.config.dateTimeExtractor, this.config.eachPrefixRegex, source, referenceDate))
        let result = Token.mergeAllTokens(tokens, source, this.extractorName);
        return result;
    }

    private matchEachSpecific(extractor: IDateTimeExtractor, eachRegex: RegExp, source: string, refDate: Date) {
        let ret = [];
        extractor.extract(source, refDate).forEach(er => {
            let beforeStr = source.substr(0, er.start);
            let beforeMatch = RegExpUtility.getMatches(eachRegex, beforeStr).pop();
            if (beforeMatch) {
                ret.push(new Token(beforeMatch.index, er.start + er.length))
            }
        });
        return ret;
    }
}

class ChineseSetParserConfiguration implements ISetParserConfiguration {
    readonly durationExtractor: IDateTimeExtractor;
    readonly durationParser: BaseDurationParser;
    readonly timeExtractor: IDateTimeExtractor;
    readonly timeParser: BaseTimeParser;
    readonly dateExtractor: BaseDateExtractor;
    readonly dateParser: BaseDateParser;
    readonly dateTimeExtractor: BaseDateTimeExtractor;
    readonly dateTimeParser: BaseDateTimeParser;
    readonly datePeriodExtractor: BaseDatePeriodExtractor;
    readonly datePeriodParser: BaseDatePeriodParser;
    readonly timePeriodExtractor: BaseTimePeriodExtractor;
    readonly timePeriodParser: BaseTimePeriodParser;
    readonly dateTimePeriodExtractor: BaseDateTimePeriodExtractor;
    readonly dateTimePeriodParser: BaseDateTimePeriodParser;
    readonly unitMap: ReadonlyMap<string, string>;
    readonly eachPrefixRegex: RegExp;
    readonly periodicRegex: RegExp;
    readonly eachUnitRegex: RegExp;
    readonly eachDayRegex: RegExp;
    readonly setWeekDayRegex: RegExp;
    readonly setEachRegex: RegExp;

    constructor() {
        this.dateExtractor = new ChineseDateExtractor();
        this.timeExtractor = new ChineseTimeExtractor();
        this.durationExtractor = new ChineseDurationExtractor();
        this.dateTimeExtractor = new ChineseDateTimeExtractor();
        this.dateParser = new ChineseDateParser();
        this.timeParser = new ChineseTimeParser();
        this.durationParser = new ChineseDurationParser();
        this.dateTimeParser = new ChineseDateTimeParser();
        this.unitMap = ChineseDateTime.ParserConfigurationUnitMap;
        this.eachUnitRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.SetEachUnitRegex);
        this.eachDayRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.SetEachDayRegex);
        this.eachPrefixRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.SetEachPrefixRegex);
    }

    public getMatchedDailyTimex(text: string): { matched: boolean, timex: string } {
        return null;
    }

    public getMatchedUnitTimex(source: string): { matched: boolean, timex: string } {
        let timex = '';
        if (source === '天' || source === '日') timex = 'P1D'
        else if (source === '周' || source === '星期') timex = 'P1W'
        else if (source === '月') timex = 'P1M'
        else if (source === '年') timex = 'P1Y'
        return { matched: timex !== '', timex: timex };
    }
}

export class ChineseSetParser extends BaseSetParser {

    constructor() {
        let config = new ChineseSetParserConfiguration();
        super(config);
    }

    parse(er: ExtractResult, referenceDate?: Date): DateTimeParseResult | null {
        if (!referenceDate) referenceDate = new Date();
        let value = null;
        if (er.type === BaseSetParser.ParserName) {
            let innerResult = this.parseEachUnit(er.text);
            if (!innerResult.success) {
                innerResult = this.parseEachDuration(er.text, referenceDate);
            }
            if (!innerResult.success) {
                innerResult = this.parserTimeEveryday(er.text, referenceDate);
            }
            if (!innerResult.success) {
                innerResult = this.parseEach(this.config.dateTimeExtractor, this.config.dateTimeParser, er.text, referenceDate);
            }
            if (!innerResult.success) {
                innerResult = this.parseEach(this.config.dateExtractor, this.config.dateParser, er.text, referenceDate);
            }
            if (innerResult.success) {
                innerResult.futureResolution = {};
                innerResult.futureResolution[TimeTypeConstants.SET] = innerResult.futureValue;
                innerResult.pastResolution = {};
                innerResult.pastResolution[TimeTypeConstants.SET] = innerResult.pastValue;
                value = innerResult;
            }
        }

        let ret = new DateTimeParseResult(er);
        ret.value = value,
        ret.timexStr = value === null ? "" : value.timex,
        ret.resolutionStr = ""

        return ret;
    }

    protected parseEachUnit(text: string): DateTimeResolutionResult {
        let ret = new DateTimeResolutionResult();

        // handle "each month"
        let match = RegExpUtility.getMatches(this.config.eachUnitRegex, text).pop();
        if (!match || match.length !== text.length) return ret;

        let sourceUnit = match.groups("unit").value;
        if (StringUtility.isNullOrEmpty(sourceUnit) || !this.config.unitMap.has(sourceUnit)) return ret;

        let getMatchedUnitTimex = this.config.getMatchedUnitTimex(sourceUnit);
        if (!getMatchedUnitTimex.matched) return ret;

        ret.timex = getMatchedUnitTimex.timex;
        ret.futureValue = "Set: " + ret.timex;
        ret.pastValue = "Set: " + ret.timex;
        ret.success = true;
        return ret;
    }

    protected parserTimeEveryday(text: string, refDate: Date): DateTimeResolutionResult {
        let result = new DateTimeResolutionResult();
        let ers = this.config.timeExtractor.extract(text, refDate);
        if (ers.length !== 1) return result;

        let er = ers[0];
        let beforeStr = text.substr(0, er.start);
        let match = RegExpUtility.getMatches(this.config.eachDayRegex, beforeStr).pop();
        if (!match) return result;

        let pr = this.config.timeParser.parse(er);
        result.timex = pr.timexStr;
        result.futureValue = "Set: " + result.timex;
        result.pastValue = "Set: " + result.timex;
        result.success = true;

        return result;
    }

    protected parseEach(extractor: IDateTimeExtractor, parser: IDateTimeParser, text: string, refDate: Date): DateTimeResolutionResult {
        let result = new DateTimeResolutionResult();
        let ers = extractor.extract(text, refDate);
        if (ers.length !== 1) return result;

        let er = ers[0];
        let beforeStr = text.substr(0, er.start);
        let match = RegExpUtility.getMatches(this.config.eachPrefixRegex, beforeStr).pop();
        if (!match) return result;

        let timex = parser.parse(er).timexStr;
        result.timex = timex;
        result.futureValue = `Set: ${timex}`;
        result.pastValue = `Set: ${timex}`;
        result.success = true;

        return result;
    }
}