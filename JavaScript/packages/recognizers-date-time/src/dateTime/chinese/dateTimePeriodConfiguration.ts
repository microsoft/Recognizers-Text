import { IExtractor, IParser, ExtractResult, RegExpUtility, StringUtility } from "@microsoft/recognizers-text";
import { ChineseNumberParserConfiguration, AgnosticNumberParserFactory, AgnosticNumberParserType, BaseNumberParser, BaseNumberExtractor, ChineseCardinalExtractor } from "@microsoft/recognizers-text-number"
import { Constants, TimeTypeConstants } from "../constants";
import { IDateTimePeriodExtractorConfiguration, BaseDateTimePeriodExtractor, IDateTimePeriodParserConfiguration, BaseDateTimePeriodParser } from "../baseDateTimePeriod";
import { BaseDurationExtractor, BaseDurationParser } from "../baseDuration"
import { BaseDateExtractor, BaseDateParser } from "../baseDate";
import { BaseTimeExtractor, BaseTimeParser } from "../baseTime";
import { BaseDatePeriodExtractor, BaseDatePeriodParser } from "../baseDatePeriod";
import { BaseTimePeriodExtractor, BaseTimePeriodParser } from "../baseTimePeriod";
import { ChineseDurationExtractor } from "./durationConfiguration";
import { ChineseTimeExtractor, ChineseTimeParser } from "./timeConfiguration";
import { ChineseTimePeriodExtractor, ChineseTimePeriodParser } from "./timePeriodConfiguration";
import { ChineseDateExtractor, ChineseDateParser } from "./dateConfiguration";
import { ChineseDateTimeExtractor, ChineseDateTimeParser } from "./dateTimeConfiguration";
import { DateUtils, Token, IDateTimeUtilityConfiguration, DateTimeResolutionResult, FormatUtil, StringMap } from "../utilities";
import { IDateTimeParser, DateTimeParseResult } from "../parsers"
import { ChineseDateTime } from "../../resources/chineseDateTime";
import { IDateTimeExtractor } from "../baseDateTime";

class ChineseDateTimePeriodExtractorConfiguration implements IDateTimePeriodExtractorConfiguration {
    readonly cardinalExtractor: ChineseCardinalExtractor
    readonly singleDateExtractor: BaseDateExtractor
    readonly singleTimeExtractor: ChineseTimeExtractor
    readonly singleDateTimeExtractor: ChineseDateTimeExtractor
    readonly durationExtractor: BaseDurationExtractor
    readonly timePeriodExtractor: IDateTimeExtractor
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
    readonly generalEndingRegex: RegExp
    readonly middlePauseRegex: RegExp

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

    extract(source: string, refDate: Date): Array<ExtractResult> {
        if (!refDate) refDate = new Date();
        let referenceDate = refDate;

        let tokens: Array<Token> = new Array<Token>()
        .concat(this.mergeDateAndTimePeriod(source, referenceDate))
        .concat(this.mergeTwoTimePoints(source, referenceDate))
        .concat(this.matchNubmerWithUnit(source))
        .concat(this.matchNight(source, referenceDate))
        let result = Token.mergeAllTokens(tokens, source, this.extractorName);
        return result;
    }

    private mergeDateAndTimePeriod(source: string, refDate: Date): Array<Token> {
        let tokens: Array<Token> = new Array<Token>();
        let ersDate = this.config.singleDateExtractor.extract(source, refDate);
        let ersTime = this.config.singleTimeExtractor.extract(source, refDate);
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

    protected mergeTwoTimePoints(source: string, refDate: Date): Array<Token> {
        let tokens: Array<Token> = new Array<Token>();
        let ersDateTime = this.config.singleDateTimeExtractor.extract(source, refDate);
        let ersTime = this.config.singleTimeExtractor.extract(source, refDate);
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

    protected matchNight(source: string, refDate: Date): Array<Token> {
        let tokens: Array<Token> = new Array<Token>();
        RegExpUtility.getMatches(this.config.specificTimeOfDayRegex, source).forEach(match => {
            tokens.push(new Token(match.index, match.index + match.length))
        });

        this.config.singleDateExtractor.extract(source, refDate).forEach(er => {
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

class ChineseDateTimePeriodParserConfiguration implements IDateTimePeriodParserConfiguration {
    readonly pureNumberFromToRegex: RegExp
    readonly pureNumberBetweenAndRegex: RegExp
    readonly periodTimeOfDayWithDateRegex: RegExp
    readonly specificTimeOfDayRegex: RegExp
    readonly pastRegex: RegExp
    readonly futureRegex: RegExp
    readonly relativeTimeUnitRegex: RegExp
    readonly restOfDateTimeRegex: RegExp
    readonly numbers: ReadonlyMap<string, number>
    readonly unitMap: ReadonlyMap<string, string>
    readonly dateExtractor: BaseDateExtractor
    readonly timeExtractor: IDateTimeExtractor
    readonly dateTimeExtractor: ChineseDateTimeExtractor
    readonly timePeriodExtractor: IDateTimeExtractor
    readonly durationExtractor: BaseDurationExtractor
    readonly dateParser: BaseDateParser
    readonly timeParser: BaseTimeParser
    readonly dateTimeParser: ChineseDateTimeParser
    readonly timePeriodParser: BaseTimePeriodParser
    readonly durationParser: BaseDurationParser

    constructor() {
        this.dateExtractor = new ChineseDateExtractor();
        this.timeExtractor = new ChineseTimeExtractor();
        this.dateTimeExtractor = new ChineseDateTimeExtractor();
        this.timePeriodExtractor = new ChineseTimePeriodExtractor();
        this.dateParser = new ChineseDateParser();
        this.timeParser = new ChineseTimeParser();
        this.dateTimeParser = new ChineseDateTimeParser();
        this.timePeriodParser = new ChineseTimePeriodParser();
        this.specificTimeOfDayRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.SpecificTimeOfDayRegex);
        this.relativeTimeUnitRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.TimeOfDayRegex);
        this.pastRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.PastRegex);
        this.futureRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.FutureRegex);
        this.unitMap = ChineseDateTime.ParserConfigurationUnitMap;
    }

    getMatchedTimeRange(source: string): { timeStr: string; beginHour: number; endHour: number; endMin: number; success: boolean; swift?: number } {
        let swift = 0;
        let beginHour = 0;
        let endHour = 0;
        let endMin = 0;
        let timeStr = '';
        switch (source) {
            case '今晚':
                swift = 0;
                timeStr = 'TEV';
                beginHour = 16;
                endHour = 20;
            break;
            case '今早':
            case '今晨':
                swift = 0;
                timeStr = 'TMO';
                beginHour = 8;
                endHour = 12;
            break;
            case '明晚':
                swift = 1;
                timeStr = 'TEV';
                beginHour = 16;
                endHour = 20;
            break;
            case '明早':
            case '明晨':
                swift = 1;
                timeStr = 'TMO';
                beginHour = 8;
                endHour = 12;
            break;
            case '昨晚':
                swift = -1;
                timeStr = 'TEV';
                beginHour = 16;
                endHour = 20;
            break;
            default:
                return {
                    timeStr: '',
                    beginHour: 0,
                    endHour: 0,
                    endMin: 0,
                    swift: 0,
                    success: false
                }
        }
        return {
            timeStr: timeStr,
            beginHour: beginHour,
            endHour: endHour,
            endMin: endMin,
            swift: swift,
            success: true
        };
    }

    getSwiftPrefix(source: string): number {
        return null;
    }
}

export class ChineseDateTimePeriodParser extends BaseDateTimePeriodParser {
    private readonly TMORegex: RegExp;
    private readonly TAFRegex: RegExp;
    private readonly TEVRegex: RegExp;
    private readonly TNIRegex; RegExp;
    private readonly unitRegex: RegExp;
    private readonly timeOfDayRegex: RegExp;
    private readonly cardinalExtractor: IExtractor;
    private readonly cardinalParser: IParser;

    constructor() {
        let config = new ChineseDateTimePeriodParserConfiguration();
        super(config);
        this.TMORegex = RegExpUtility.getSafeRegExp(ChineseDateTime.DateTimePeriodMORegex);
        this.TAFRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.DateTimePeriodAFRegex);
        this.TEVRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.DateTimePeriodEVRegex);
        this.TNIRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.DateTimePeriodNIRegex);
        this.unitRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.DateTimePeriodUnitRegex);
        this.timeOfDayRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.TimeOfDayRegex);
        this.cardinalExtractor = new ChineseCardinalExtractor();
        this.cardinalParser = AgnosticNumberParserFactory.getParser(AgnosticNumberParserType.Cardinal, new ChineseNumberParserConfiguration());
    }

    parse(extractorResult: ExtractResult, referenceDate?: Date): DateTimeParseResult | null {
        if (!referenceDate) referenceDate = new Date();
        let resultValue;
        if (extractorResult.type === this.parserName) {
            let source = extractorResult.text.trim().toLowerCase();
            let innerResult = this.mergeDateAndTimePeriods(source, referenceDate);
            if (!innerResult.success) {
                innerResult = this.mergeTwoTimePoints(source, referenceDate);
            }
            if (!innerResult.success) {
                innerResult = this.parseSpecificTimeOfDay(source, referenceDate);
            }
            if (!innerResult.success) {
                innerResult = this.parseNumberWithUnit(source, referenceDate);
            }
            if (innerResult.success) {
                innerResult.futureResolution = {};
                innerResult.futureResolution[TimeTypeConstants.START_DATETIME] = FormatUtil.formatDateTime(innerResult.futureValue[0]);
                innerResult.futureResolution[TimeTypeConstants.END_DATETIME] = FormatUtil.formatDateTime(innerResult.futureValue[1]);
                innerResult.pastResolution = {};
                innerResult.pastResolution[TimeTypeConstants.START_DATETIME] = FormatUtil.formatDateTime(innerResult.pastValue[0]);
                innerResult.pastResolution[TimeTypeConstants.END_DATETIME] = FormatUtil.formatDateTime(innerResult.pastValue[1]);
                resultValue = innerResult;
            }
        }
        let result = new DateTimeParseResult(extractorResult);
        result.value = resultValue;
        result.timexStr = resultValue ? resultValue.timex : '';
        result.resolutionStr = '';

        return result;
    }

    protected mergeDateAndTimePeriods(text: string, referenceTime: Date): DateTimeResolutionResult {
        let result = new DateTimeResolutionResult();

        let erDate = this.config.dateExtractor.extract(text, referenceTime).pop();
        let erTimePeriod = this.config.timePeriodExtractor.extract(text, referenceTime).pop();
        if (!erDate || !erTimePeriod) return result;

        let prDate = this.config.dateParser.parse(erDate, referenceTime);
        let prTimePeriod = this.config.timePeriodParser.parse(erTimePeriod, referenceTime);

        let split = prTimePeriod.timexStr.split('T');
        if (split.length !== 4) {
            return result;
        }

        let beginTime: Date = prTimePeriod.value.futureValue.item1;
        let endTime: Date = prTimePeriod.value.futureValue.item2;

        let futureDate: Date = prDate.value.futureValue;
        let pastDate: Date = prDate.value.pastValue;

        result.futureValue = [
            DateUtils.safeCreateFromMinValueWithDateAndTime(futureDate, beginTime),
            DateUtils.safeCreateFromMinValueWithDateAndTime(futureDate, endTime)
        ]
        result.pastValue = [
            DateUtils.safeCreateFromMinValueWithDateAndTime(pastDate, beginTime),
            DateUtils.safeCreateFromMinValueWithDateAndTime(pastDate, endTime)
        ]
        let dateTimex = prDate.timexStr;
        result.timex = `${split[0]}${dateTimex}T${split[1]}${dateTimex}T${split[2]}T${split[3]}`;
        result.success = true;

        return result;
    }

    protected mergeTwoTimePoints(text: string, referenceTime: Date): DateTimeResolutionResult {
        let result = new DateTimeResolutionResult();
        let prs: { begin: DateTimeParseResult, end: DateTimeParseResult };
        let timeErs = this.config.timeExtractor.extract(text, referenceTime);
        let datetimeErs = this.config.dateTimeExtractor.extract(text, referenceTime);
        let bothHasDate = false;
        let beginHasDate = false;
        let endHasDate = false;

        if (datetimeErs.length === 2) {
            prs = this.getTwoPoints(datetimeErs[0], datetimeErs[1], this.config.dateTimeParser, this.config.dateTimeParser, referenceTime);
            bothHasDate = true;
        } else if (datetimeErs.length === 1 && timeErs.length === 2) {
            if (ExtractResult.isOverlap(datetimeErs[0], timeErs[0])) {
                prs = this.getTwoPoints(datetimeErs[0], timeErs[1], this.config.dateTimeParser, this.config.timeParser, referenceTime);
                beginHasDate = true;
            } else {
                prs = this.getTwoPoints(timeErs[0], datetimeErs[0], this.config.timeParser, this.config.dateTimeParser, referenceTime);
                endHasDate = true;
            }
        } else if (datetimeErs.length === 1 && timeErs.length === 1) {
            if (timeErs[0].start < datetimeErs[0].start) {
                prs = this.getTwoPoints(timeErs[0], datetimeErs[0], this.config.timeParser, this.config.dateTimeParser, referenceTime);
                endHasDate = true;
            } else {
                prs = this.getTwoPoints(datetimeErs[0], timeErs[0], this.config.dateTimeParser, this.config.timeParser, referenceTime);
                beginHasDate = true;
            }
        }
        if (!prs || !prs.begin.value || !prs.end.value) return result;

        let futureBegin: Date = prs.begin.value.futureValue;
        let futureEnd: Date = prs.end.value.futureValue;
        let pastBegin: Date = prs.begin.value.pastValue;
        let pastEnd: Date = prs.end.value.pastValue;

        if (futureBegin.getTime() > futureEnd.getTime()) futureBegin = pastBegin;
        if (pastEnd.getTime() < pastBegin.getTime()) pastEnd = futureEnd;

        let rightTime = DateUtils.safeCreateFromMinValueWithDateAndTime(referenceTime);
        let leftTime = DateUtils.safeCreateFromMinValueWithDateAndTime(referenceTime);

        if (bothHasDate) {
            rightTime = DateUtils.safeCreateFromMinValueWithDateAndTime(futureEnd);
            leftTime = DateUtils.safeCreateFromMinValueWithDateAndTime(futureBegin);
        } else if (beginHasDate){
            // TODO: Handle "明天下午两点到五点"
            futureEnd = DateUtils.safeCreateFromMinValueWithDateAndTime(futureBegin, futureEnd);
            pastEnd = DateUtils.safeCreateFromMinValueWithDateAndTime(pastBegin, pastEnd);
            leftTime = DateUtils.safeCreateFromMinValueWithDateAndTime(futureBegin);
        } else if (endHasDate) {
            // TODO: Handle "明天下午两点到五点"
            futureBegin = DateUtils.safeCreateFromMinValueWithDateAndTime(futureEnd, futureBegin);
            pastBegin = DateUtils.safeCreateFromMinValueWithDateAndTime(pastEnd, pastBegin);
            rightTime = DateUtils.safeCreateFromMinValueWithDateAndTime(futureEnd);
        }

        let leftResult: DateTimeResolutionResult = prs.begin.value;
        let rightResult: DateTimeResolutionResult = prs.end.value;
        let leftResultTime: Date = leftResult.futureValue;
        let rightResultTime: Date = rightResult.futureValue;

        leftTime = DateUtils.addTime(leftTime, leftResultTime);
        rightTime = DateUtils.addTime(rightTime, rightResultTime);

        // the right side time contains "ampm", while the left side doesn't
        if (rightResult.comment === 'ampm' && !leftResult.comment && rightTime.getTime() < leftTime.getTime()) {
            rightTime = DateUtils.addHours(rightTime, 12);
        }

        if (rightTime.getTime() < leftTime.getTime()) {
            rightTime = DateUtils.addDays(rightTime, 1);
        }

        result.futureValue = [leftTime, rightTime];
        result.pastValue = [leftTime, rightTime]

        let hasFuzzyTimex = prs.begin.timexStr.includes('X') || prs.end.timexStr.includes('X');
        let leftTimex = hasFuzzyTimex ? prs.begin.timexStr : FormatUtil.luisDateTime(leftTime);
        let rightTimex = hasFuzzyTimex ? prs.end.timexStr : FormatUtil.luisDateTime(rightTime);
        let hoursBetween = DateUtils.totalHours(rightTime, leftTime);

        result.timex = `(${leftTimex},${rightTimex},PT${hoursBetween}H)`;
        result.success = true;

        return result;
    }

    protected parseSpecificTimeOfDay(text: string, referenceTime: Date): DateTimeResolutionResult {
        let result = new DateTimeResolutionResult();
        let source = text.trim().toLowerCase();

        let match = RegExpUtility.getMatches(this.config.specificTimeOfDayRegex, source).pop();
        if (match && match.index === 0 && match.length === source.length) {
            let values = this.config.getMatchedTimeRange(source);
            if (!values.success) {
                return result;
            }
            let swift = (values as any).swift;

            let date = DateUtils.addDays(referenceTime, swift);
            date.setHours(0);
            date.setMinutes(0);
            date.setSeconds(0);

            result.timex = FormatUtil.formatDate(date) + values.timeStr;
            result.futureValue = [
                DateUtils.safeCreateFromMinValue(date.getFullYear(), date.getMonth(), date.getDate(), values.beginHour, 0, 0),
                DateUtils.safeCreateFromMinValue(date.getFullYear(), date.getMonth(), date.getDate(), values.endHour, values.endMin, values.endMin)
            ];
            result.pastValue = [
                DateUtils.safeCreateFromMinValue(date.getFullYear(), date.getMonth(), date.getDate(), values.beginHour, 0, 0),
                DateUtils.safeCreateFromMinValue(date.getFullYear(), date.getMonth(), date.getDate(), values.endHour, values.endMin, values.endMin)
            ];
            result.success = true;
            return result;
        }

        let beginHour = 0;
        let endHour = 0;
        let endMin = 0;
        let timeStr = '';

        // handle morning, afternoon..
        if (RegExpUtility.isMatch(this.TMORegex, source)) {
            timeStr = 'TMO';
            beginHour = 8;
            endHour = 12;
        } else if (RegExpUtility.isMatch(this.TAFRegex, source)) {
            timeStr = 'TAF';
            beginHour = 12;
            endHour = 16;
        } else if (RegExpUtility.isMatch(this.TEVRegex, source)) {
            timeStr = 'TEV';
            beginHour = 16;
            endHour = 20;
        } else if (RegExpUtility.isMatch(this.TNIRegex, source)) {
            timeStr = 'TNI';
            beginHour = 20;
            endHour = 23;
            endMin = 59;
        } else {
            return result;
        }

        // handle Date followed by morning, afternoon
        let timeMatch = RegExpUtility.getMatches(this.timeOfDayRegex, source).pop();
        if (!timeMatch) return result;

        let beforeStr = source.substr(0, timeMatch.index).trim();
        let erDate = this.config.dateExtractor.extract(beforeStr, referenceTime).pop();
        if (!erDate || erDate.length !== beforeStr.length) return result;

        let prDate = this.config.dateParser.parse(erDate, referenceTime);
        let futureDate: Date = prDate.value.futureValue;
        let pastDate: Date = prDate.value.pastValue;

        result.timex = prDate.timexStr + timeStr;
        result.futureValue = [
            DateUtils.safeCreateFromMinValue(futureDate.getFullYear(), futureDate.getMonth(), futureDate.getDate(), beginHour, 0, 0),
            DateUtils.safeCreateFromMinValue(futureDate.getFullYear(), futureDate.getMonth(), futureDate.getDate(), endHour, endMin, endMin)
        ];
        result.pastValue = [
            DateUtils.safeCreateFromMinValue(pastDate.getFullYear(), pastDate.getMonth(), pastDate.getDate(), beginHour, 0, 0),
            DateUtils.safeCreateFromMinValue(pastDate.getFullYear(), pastDate.getMonth(), pastDate.getDate(), endHour, endMin, endMin)
        ];
        result.success = true;

        return result;
    }

    protected parseNumberWithUnit(text: string, referenceTime: Date): DateTimeResolutionResult {
        let result = new DateTimeResolutionResult();
        let ers = this.cardinalExtractor.extract(text);
        
        if (ers.length === 1) {
            let er = ers[0];

            let pr = this.cardinalParser.parse(er);
            let sourceUnit = text.substr(er.start + er.length).trim().toLowerCase();
            if (sourceUnit.startsWith('个')) {
                sourceUnit = sourceUnit.substr(1);
            }
    
            let beforeStr = text.substr(0, er.start).trim().toLowerCase();
    
            return this.parseCommonDurationWithUnit(beforeStr, sourceUnit, pr.resolutionStr, pr.value, referenceTime);
        }
        
        // handle "last hour"
        let match = RegExpUtility.getMatches(this.unitRegex, text).pop();

        if (match) {
            let srcUnit = match.groups('unit').value;
            let beforeStr = text.substr(0, match.index).trim().toLowerCase();

            return this.parseCommonDurationWithUnit(beforeStr, srcUnit, '1', 1, referenceTime);
        }

        return result;
    }

    protected parseDuration(text: string, referenceTime: Date): DateTimeResolutionResult {
        let result = new DateTimeResolutionResult();
        let match = RegExpUtility.getMatches(this.config.relativeTimeUnitRegex, text).pop();
        if (!match) return result;

        let sourceUnit = match.groups('unit').value.toLowerCase();
        let beforeStr = text.substr(0, match.index).trim().toLowerCase();

        return this.parseCommonDurationWithUnit(beforeStr, sourceUnit, '1', 1, referenceTime);
    }

    private parseCommonDurationWithUnit(beforeStr: string, sourceUnit: string, numStr: string, swift: number, referenceDate: Date): DateTimeResolutionResult {
        let result = new DateTimeResolutionResult();

        if (!this.config.unitMap.has(sourceUnit)) return result;

        let unitStr = this.config.unitMap.get(sourceUnit);

        let pastMatch = RegExpUtility.getMatches(this.config.pastRegex, beforeStr).pop();
        let hasPast = pastMatch && pastMatch.length === beforeStr.length;

        let futureMatch = RegExpUtility.getMatches(this.config.futureRegex, beforeStr).pop();
        let hasFuture = futureMatch && futureMatch.length === beforeStr.length;

        if (!hasPast && !hasFuture) return result;

        let beginDate = new Date(referenceDate);
        let endDate = new Date(referenceDate);

        switch(unitStr) {
            case 'H':
                beginDate = hasPast ? DateUtils.addHours(beginDate, -swift) : beginDate;
                endDate = hasFuture ? DateUtils.addHours(endDate, swift) : endDate;
            break;
            case 'M':
                beginDate = hasPast ? DateUtils.addMinutes(beginDate, -swift) : beginDate;
                endDate = hasFuture ? DateUtils.addMinutes(endDate, swift) : endDate;
            break;
            case 'S':
                beginDate = hasPast ? DateUtils.addSeconds(beginDate, -swift) : beginDate;
                endDate = hasFuture ? DateUtils.addSeconds(endDate, swift) : endDate;
            break;
            default: return result;
        }

        let beginTimex = `${FormatUtil.luisDateFromDate(beginDate)}T${FormatUtil.luisTimeFromDate(beginDate)}`;
        let endTimex = `${FormatUtil.luisDateFromDate(endDate)}T${FormatUtil.luisTimeFromDate(endDate)}`;
        result.timex = `(${beginTimex},${endTimex},PT${numStr}${unitStr.charAt(0)})`;
        result.futureValue = [beginDate, endDate];
        result.pastValue = [beginDate, endDate];
        result.success = true;

        return result;
    }
}