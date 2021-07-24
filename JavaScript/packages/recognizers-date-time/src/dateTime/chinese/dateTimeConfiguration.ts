// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

import { IExtractor, ExtractResult, RegExpUtility, StringUtility, MetaData } from "@microsoft/recognizers-text";
import { AgnosticNumberParserFactory, BaseNumberParser, BaseNumberExtractor, ChineseIntegerExtractor, AgnosticNumberParserType, ChineseNumberParserConfiguration } from "@microsoft/recognizers-text-number";
import { Constants as NumberConstants  } from "@microsoft/recognizers-text-number";
import { Constants , TimeTypeConstants } from "../constants";
import { IDateTimeExtractor, IDateTimeExtractorConfiguration, BaseDateTimeExtractor, IDateTimeParserConfiguration, BaseDateTimeParser } from "../baseDateTime";
import { BaseDurationExtractor, BaseDurationParser } from "../baseDuration";
import { BaseDateExtractor, BaseDateParser } from "../baseDate";
import { BaseTimeExtractor, BaseTimeParser } from "../baseTime";
import { BaseDatePeriodExtractor, BaseDatePeriodParser } from "../baseDatePeriod";
import { BaseTimePeriodExtractor, BaseTimePeriodParser } from "../baseTimePeriod";
import { BaseDateTimePeriodExtractor, BaseDateTimePeriodParser } from "../baseDateTimePeriod";
import { ChineseDurationExtractor } from "./durationConfiguration";
import { ChineseTimeExtractor, ChineseTimeParser } from "./timeConfiguration";
import { ChineseDateExtractor, ChineseDateParser } from "./dateConfiguration";
import { Token, IDateTimeUtilityConfiguration, DateUtils, DateTimeFormatUtil, DateTimeResolutionResult, StringMap } from "../utilities";
import { ChineseDateTime } from "../../resources/chineseDateTime";
import { IDateTimeParser, DateTimeParseResult } from "../parsers";

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
    readonly specificEndOfRegex: RegExp
    readonly unspecificEndOfRegex: RegExp
    readonly unitRegex: RegExp
    readonly prepositionRegex: RegExp
    readonly utilityConfiguration: IDateTimeUtilityConfiguration

    constructor(dmyDateFormat: boolean) {
        this.datePointExtractor = new ChineseDateExtractor(dmyDateFormat);
        this.timePointExtractor = new ChineseTimeExtractor();
        this.prepositionRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.PrepositionRegex);
        this.nowRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.NowRegex);
        this.nightRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.NightRegex);
        this.timeOfTodayBeforeRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.TimeOfTodayRegex);
    }

    isConnectorToken(source: string): boolean {
        return StringUtility.isNullOrEmpty(source)
            || source === ','
            || RegExpUtility.isMatch(this.prepositionRegex, source);
    }
}

export class ChineseDateTimeExtractor extends BaseDateTimeExtractor {
    static beforeRegex: RegExp = RegExpUtility.getSafeRegExp(ChineseDateTime.BeforeRegex);
    static afterRegex: RegExp = RegExpUtility.getSafeRegExp(ChineseDateTime.AfterRegex);
    static dateTimePeriodUnitRegex: RegExp = RegExpUtility.getSafeRegExp(ChineseDateTime.DateTimePeriodUnitRegex);
    private readonly durationExtractor: ChineseDurationExtractor;

    constructor(dmyDateFormat: boolean) {
        super(new ChineseDateTimeExtractorConfiguration(dmyDateFormat));
        this.durationExtractor = new ChineseDurationExtractor();
    }

    extract(source: string, refDate: Date): ExtractResult[] {
        if (!refDate) {
            refDate = new Date();
        }
        let referenceDate = refDate;

        let tokens: Token[] = new Array<Token>()
            .concat(this.mergeDateAndTime(source, referenceDate))
            .concat(this.basicRegexMatch(source))
            .concat(this.timeOfToday(source, referenceDate))
            .concat(this.durationWithAgoAndLater(source, referenceDate));
        let result = Token.mergeAllTokens(tokens, source, this.extractorName);
        return result;
    }

    protected mergeDateAndTime(source: string, refDate: Date): Token[] {
        let tokens: Token[] = new Array<Token>();
        let ers = this.config.datePointExtractor.extract(source, refDate);
        if (ers.length < 1) {
            return tokens;
        }
        ers = ers.concat(this.config.timePointExtractor.extract(source, refDate));
        if (ers.length < 2) {
            return tokens;
        }
        ers = ers.sort((erA, erB) => erA.start < erB.start ? -1 : erA.start === erB.start ? 0 : 1);
        let i = 0;
        while (i < ers.length - 1) {
            let j = i + 1;
            while (j < ers.length && ExtractResult.isOverlap(ers[i], ers[j])) {
                j++;
            }
            if (j >= ers.length) {
                break;
            }
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

    private timeOfToday(source: string, refDate: Date): Token[] {
        let tokens: Token[] = new Array<Token>();
        this.config.timePointExtractor.extract(source, refDate).forEach(er => {
            let beforeStr = source.substr(0, er.start);
            let innerMatch = RegExpUtility.getMatches(this.config.nightRegex, er.text).pop();
            if (innerMatch && innerMatch.index === 0) {
                beforeStr = source.substr(0, er.start + innerMatch.length);
            }

            if (StringUtility.isNullOrWhitespace(beforeStr)) {
                return;
            }

            let match = RegExpUtility.getMatches(this.config.timeOfTodayBeforeRegex, beforeStr).pop();
            if (match && StringUtility.isNullOrWhitespace(beforeStr.substr(match.index + match.length))) {
                let begin = match.index;
                let end = er.start + er.length;
                tokens.push(new Token(begin, end));
            }
        });
        return tokens;
    }

    // Process case like "5分钟前" "二小时后"
    protected durationWithAgoAndLater(source: string, refDate: Date): Token[] {
        let ret = [];
        let durationEr = this.durationExtractor.extract(source, refDate);
        durationEr.forEach(er => {
            let pos = er.start + er.length;
            if (pos < source.length) {
                let suffix = source.substr(pos, 1);
                let beforeMatch = RegExpUtility.getMatches(ChineseDateTimeExtractor.beforeRegex, suffix).pop();
                let afterMatch = RegExpUtility.getMatches(ChineseDateTimeExtractor.afterRegex, suffix).pop();

                if (beforeMatch && suffix.startsWith(beforeMatch.value) || afterMatch && suffix.startsWith(afterMatch.value)) {
                    let metadata = new MetaData();
                    metadata.IsDurationWithAgoAndLater = true;
                    ret.push(new Token(er.start, pos + 1, metadata));
                }
            }
        });
        return ret;
    }
}

class ChineseDateTimeParserConfiguration implements IDateTimeParserConfiguration {
    readonly tokenBeforeDate: string;
    readonly tokenBeforeTime: string;
    readonly dateExtractor: IDateTimeExtractor;
    readonly timeExtractor: ChineseTimeExtractor;
    readonly dateParser: ChineseDateParser;
    readonly timeParser: ChineseTimeParser;
    readonly cardinalExtractor: BaseNumberExtractor;
    readonly numberParser: BaseNumberParser;
    readonly durationExtractor: BaseDurationExtractor;
    readonly durationParser: BaseDurationParser;
    readonly nowRegex: RegExp;
    readonly amTimeRegex: RegExp;
    readonly pmTimeRegex: RegExp;
    readonly simpleTimeOfTodayAfterRegex: RegExp;
    readonly simpleTimeOfTodayBeforeRegex: RegExp;
    readonly specificTimeOfDayRegex: RegExp;
    readonly specificEndOfRegex: RegExp;
    readonly unspecificEndOfRegex: RegExp;
    readonly unitRegex: RegExp;
    readonly unitMap: ReadonlyMap<string, string>;
    readonly numbers: ReadonlyMap<string, number>;
    readonly utilityConfiguration: IDateTimeUtilityConfiguration;

    constructor(dmyDateFormat: boolean) {
        this.dateExtractor = new ChineseDateExtractor(dmyDateFormat);
        this.timeExtractor = new ChineseTimeExtractor();
        this.dateParser = new ChineseDateParser(dmyDateFormat);
        this.timeParser = new ChineseTimeParser();
        this.pmTimeRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.DateTimeSimplePmRegex);
        this.amTimeRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.DateTimeSimpleAmRegex);
        this.specificTimeOfDayRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.TimeOfTodayRegex);
        this.nowRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.NowRegex);
        this.numberParser = AgnosticNumberParserFactory.getParser(AgnosticNumberParserType.Number, new ChineseNumberParserConfiguration());
        this.unitRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.DateUnitRegex);
        this.unitMap = ChineseDateTime.ParserConfigurationUnitMap;
    }

    haveAmbiguousToken(text: string, matchedText: string): boolean {
        return null;
    }

    getMatchedNowTimex(text: string): { matched: boolean; timex: string; } {
        let trimmedText = text.trim().toLowerCase();
        if (trimmedText.endsWith('现在')) {
            return { matched: true, timex: 'PRESENT_REF' };
        }
        else if (trimmedText === '刚刚才' || trimmedText === '刚刚' || trimmedText === '刚才') {
            return { matched: true, timex: 'PAST_REF' };
        }
        else if (trimmedText === '立刻' || trimmedText === '马上') {
            return { matched: true, timex: 'FUTURE_REF' };
        }
        return { matched: false, timex: null };
    }

    getSwiftDay(text: string): number {
        let swift = 0;
        if (text === '明晚' || text === '明早' || text === '明晨') {
            swift = 1;
        }
        else if (text === '昨晚') {
            swift = -1;
        }
        return swift;
    }

    getHour(text: string, hour: number): number {
        let result = hour;
        if (hour < 12 && ['今晚', '明晚', '昨晚'].some(o => o === text)) {
            result += 12;
        }
        else if (hour >= 12 && ['今早', '今晨', '明早', '明晨'].some(o => o === text)) {
            result -= 12;
        }
        return result;
    }
}

export class ChineseDateTimeParser extends BaseDateTimeParser {
    private readonly durationExtractor: ChineseDurationExtractor;
    private readonly integerExtractor: BaseNumberExtractor
    constructor(dmyDateFormat: boolean) {
        let config = new ChineseDateTimeParserConfiguration(dmyDateFormat);
        super(config);
        this.durationExtractor = new ChineseDurationExtractor();
        this.integerExtractor = new ChineseIntegerExtractor();
    }

    parse(er: ExtractResult, refTime?: Date): DateTimeParseResult {
        if (!refTime) {
            refTime = new Date();
        }
        let referenceTime = refTime;

        let value = null;
        if (er.type === BaseDateTimeParser.ParserName) {
            let innerResult = this.mergeDateAndTime(er.text, referenceTime);
            if (!innerResult.success) {
                innerResult = this.parseBasicRegex(er.text, referenceTime);
            }
            if (!innerResult.success) {
                innerResult = this.parseTimeOfToday(er.text, referenceTime);
            }
            if (!innerResult.success) {
                innerResult = this.parserDurationWithAgoAndLater(er.text, referenceTime);
            }
            if (innerResult.success) {
                innerResult.futureResolution = {};
                innerResult.futureResolution[TimeTypeConstants.DATETIME] = DateTimeFormatUtil.formatDateTime(innerResult.futureValue);
                innerResult.pastResolution = {};
                innerResult.pastResolution[TimeTypeConstants.DATETIME] = DateTimeFormatUtil.formatDateTime(innerResult.pastValue);
                value = innerResult;
            }
        }

        let ret = new DateTimeParseResult(er); {
            ret.value = value,
                ret.timexStr = value === null ? "" : value.timex,
                ret.resolutionStr = "";
        };

        return ret;
    }

    // merge a Date entity and a Time entity
    protected mergeDateAndTime(text: string, referenceTime: Date): DateTimeResolutionResult {
        let ret = new DateTimeResolutionResult();

        let er1 = this.config.dateExtractor.extract(text, referenceTime);
        if (er1.length === 0) {
            return ret;
        }

        let er2 = this.config.timeExtractor.extract(text, referenceTime);
        if (er2.length === 0) {
            return ret;
        }

        let pr1 = this.config.dateParser.parse(er1[0], new Date(referenceTime.toDateString()));
        let pr2 = this.config.timeParser.parse(er2[0], referenceTime);
        if (pr1.value === null || pr2.value === null) {
            return ret;
        }

        let futureDate = pr1.value.futureValue;
        let pastDate = pr1.value.pastValue;
        let time = pr2.value.futureValue;

        let hour = time.getHours();
        let min = time.getMinutes();
        let sec = time.getSeconds();

        // handle morning, afternoon
        if (RegExpUtility.getMatches(this.config.pmTimeRegex, text).length && hour < 12) {
            hour += 12;
        }
        else if (RegExpUtility.getMatches(this.config.amTimeRegex, text).length && hour >= 12) {
            hour -= 12;
        }

        let timeStr = pr2.timexStr;
        if (timeStr.endsWith("ampm")) {
            timeStr = timeStr.substring(0, timeStr.length - 4);
        }

        timeStr = "T" + DateTimeFormatUtil.toString(hour, 2) + timeStr.substring(3);
        ret.timex = pr1.timexStr + timeStr;

        let val = pr2.value;
        if (hour <= 12 && !RegExpUtility.getMatches(this.config.pmTimeRegex, text).length
            && !RegExpUtility.getMatches(this.config.amTimeRegex, text).length && val.comment) {
            ret.comment = "ampm";
        }

        ret.futureValue = new Date(futureDate.getFullYear(), futureDate.getMonth(), futureDate.getDate(), hour, min, sec);
        ret.pastValue = new Date(pastDate.getFullYear(), pastDate.getMonth(), pastDate.getDate(), hour, min, sec);
        ret.success = true;

        return ret;
    }

    protected parseTimeOfToday(text: string, referenceTime: Date): DateTimeResolutionResult {
        let ret = new DateTimeResolutionResult();

        let ers = this.config.timeExtractor.extract(text, referenceTime);
        if (ers.length !== 1) {
            return ret;
        }

        let pr = this.config.timeParser.parse(ers[0], referenceTime);
        if (pr.value === null) {
            return ret;
        }

        let time = pr.value.futureValue;

        let hour = time.getHours();
        let min = time.getMinutes();
        let sec = time.getSeconds();
        let timeStr = pr.timexStr;

        let match = RegExpUtility.getMatches(this.config.specificTimeOfDayRegex, text).pop();

        if (match) {
            let matchStr = match.value.toLowerCase();

            // handle "last", "next"
            let swift = this.config.getSwiftDay(matchStr);
            let date = DateUtils.addDays(referenceTime, swift);

            hour = this.config.getHour(matchStr, hour);

            // in this situation, luisStr cannot end up with "ampm", because we always have a "morning" or "night"
            if (timeStr.endsWith("ampm")) {
                timeStr = timeStr.substring(0, timeStr.length - 4);
            }
            timeStr = "T" + DateTimeFormatUtil.toString(hour, 2) + timeStr.substring(3);

            ret.timex = DateTimeFormatUtil.formatDate(date) + timeStr;
            ret.futureValue = ret.pastValue = new Date(date.getFullYear(), date.getMonth(), date.getDate(), hour, min, sec);
            ret.success = true;
            return ret;
        }

        return ret;
    }

    // Handle cases like "5分钟前", "1小时以后"
    protected parserDurationWithAgoAndLater(source: string, referenceDate: Date): DateTimeResolutionResult {
        let result = new DateTimeResolutionResult();
        let durationRes = this.durationExtractor.extract(source, referenceDate);

        if (durationRes) {
            let match = RegExpUtility.getMatches(ChineseDateTimeExtractor.dateTimePeriodUnitRegex, source).pop();
            if (match) {
                let suffix = source.substring(durationRes[0].start + durationRes[0].length);
                let srcUnit = match.groups('unit').value;

                let numberStr = source.substring(durationRes[0].start, match.index - durationRes[0].start);
                let number =  this.parseChineseWrittenNumberToValue(numberStr);

                if (this.config.unitMap.has(srcUnit)) {
                    let unitStr = this.config.unitMap.get(srcUnit);

                    let beforeMatch = RegExpUtility.getMatches(ChineseDateTimeExtractor.beforeRegex, suffix).pop();
                    if (beforeMatch && suffix.startsWith(beforeMatch.value)) {
                        let date : Date;
                        switch (unitStr) {
                            case Constants.TimexHour:
                                date = DateUtils.addHours(referenceDate, -number);
                                break;
                            case Constants.TimexMinute:
                                date = DateUtils.addMinutes(referenceDate, -number);
                                break;
                            case Constants.TimexSecond:
                                date = DateUtils.addSeconds(referenceDate, -number);
                                break;
                            default:
                                return result;
                        }

                        result.timex = DateTimeFormatUtil.luisDateFromDate(date);
                        result.futureValue = result.pastValue = date;
                        result.success = true;
                        return result;
                    }

                    let afterMatch = RegExpUtility.getMatches(ChineseDateExtractor.afterRegex, suffix).pop();
                    if (afterMatch && suffix.startsWith(afterMatch.value)) {
                        let date: Date;
                        switch (unitStr) {
                            case Constants.TimexHour:
                                date = DateUtils.addHours(referenceDate, number);
                                break;
                            case Constants.TimexMinute:
                                date = DateUtils.addMinutes(referenceDate, number);
                                break;
                            case Constants.TimexSecond:
                                date = DateUtils.addSeconds(referenceDate, number);
                                break;
                            default:
                                return result;
                        }

                        result.timex = DateTimeFormatUtil.luisDateFromDate(date);
                        result.futureValue = result.pastValue = date;
                        result.success = true;
                        return result;
                    }
                }

            }
        }

        return result;
    }

    // convert Chinese Number to Integer
    private parseChineseWrittenNumberToValue(source: string): number {
        let num = -1;
        let er = this.integerExtractor.extract(source);
        if (er && er[0].type === NumberConstants.SYS_NUM_INTEGER) {
            num = Number.parseInt(this.config.numberParser.parse(er[0]).value);
        }

        return num;
    }
}