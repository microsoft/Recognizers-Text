// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

import { RegExpUtility, ExtractResult, IExtractor } from "@microsoft/recognizers-text";
import { CultureInfo, Culture, EnglishIntegerExtractor } from "@microsoft/recognizers-text-number";
import { NumberWithUnitExtractor, ChineseNumberWithUnitExtractorConfiguration } from "@microsoft/recognizers-text-number-with-unit";
import { BaseDateTimeExtractor, DateTimeExtra, TimeResult, TimeResolutionUtils } from "./baseDateTime";
import { Constants, TimeTypeConstants } from "../constants";
import { ChineseDateTime } from "../../resources/chineseDateTime";
import { DateTimeResolutionResult, DateTimeFormatUtil, DateUtils, StringMap, TimexUtil } from "../utilities";
import { BaseTimePeriodParser, ITimePeriodParserConfiguration } from "../baseTimePeriod";
import { IDateTimeParser, DateTimeParseResult } from "../parsers";
import { ChineseTimeParser } from "./timeConfiguration";
import { IDateTimeExtractor } from "../baseDateTime";

export enum TimePeriodType {
    ShortTime,
    FullTime
}

export class ChineseTimePeriodExtractor extends BaseDateTimeExtractor<TimePeriodType> {
    protected extractorName = Constants.SYS_DATETIME_TIMEPERIOD; // "time range";

    constructor() {
        super(new Map<RegExp, TimePeriodType>([
            [RegExpUtility.getSafeRegExp(ChineseDateTime.TimePeriodRegexes1), TimePeriodType.FullTime],
            [RegExpUtility.getSafeRegExp(ChineseDateTime.TimePeriodRegexes2), TimePeriodType.ShortTime],
            [RegExpUtility.getSafeRegExp(ChineseDateTime.TimeOfDayRegex), TimePeriodType.ShortTime]
        ]));
    }
}

class ChineseTimePeriodParserConfiguration implements ITimePeriodParserConfiguration {
    timeExtractor: IDateTimeExtractor;
    timeParser: ChineseTimeParser;
    integerExtractor: IExtractor;
    pureNumberFromToRegex: RegExp;
    pureNumberBetweenAndRegex: RegExp;
    timeOfDayRegex: RegExp;
    tillRegex: RegExp;
    numbers: ReadonlyMap<string, number>;
    utilityConfiguration: any;
    specificTimeFromToRegex: RegExp;
    specificTimeBetweenAndRegex: RegExp;

    constructor() {
        this.timeParser = new ChineseTimeParser();
        this.integerExtractor = new EnglishIntegerExtractor();
    }

    getMatchedTimexRange(text: string): any {
        return null;
    }
}

export class ChineseTimePeriodParser extends BaseTimePeriodParser {
    private readonly dayDescriptionRegex: RegExp;
    private readonly onlyDigitMatch: RegExp;
    private readonly lowBoundMap: ReadonlyMap<string, number>;
    private readonly numbersMap: ReadonlyMap<string, number>;

    constructor() {
        let config = new ChineseTimePeriodParserConfiguration();
        super(config);
        this.dayDescriptionRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.TimeDayDescRegex);
        this.onlyDigitMatch = RegExpUtility.getSafeRegExp('\\d+');
        this.numbersMap = ChineseDateTime.TimeNumberDictionary;
        this.lowBoundMap = ChineseDateTime.TimeLowBoundDesc;
    }

    public parse(er: ExtractResult, referenceTime?: Date): DateTimeParseResult {
        if (!referenceTime) {
            referenceTime = new Date();
        }

        let result = new DateTimeParseResult(er);
        let extra: DateTimeExtra<TimePeriodType> = er.data;

        if (!extra) {
            return result;
        }

        let parseResult = this.parseChineseTimeOfDay(er.text, referenceTime);

        if (!parseResult.success) {
            parseResult = this.parseTimePeriod(extra, referenceTime);
        }

        if (parseResult.success) {
            parseResult.futureResolution = {};
            parseResult.futureResolution[TimeTypeConstants.START_TIME] = DateTimeFormatUtil.formatTime(parseResult.futureValue.item1);
            parseResult.futureResolution[TimeTypeConstants.END_TIME] = DateTimeFormatUtil.formatTime(parseResult.futureValue.item2);
            parseResult.pastResolution = {};
            parseResult.pastResolution[TimeTypeConstants.START_TIME] = DateTimeFormatUtil.formatTime(parseResult.pastValue.item1);
            parseResult.pastResolution[TimeTypeConstants.END_TIME] = DateTimeFormatUtil.formatTime(parseResult.pastValue.item2);
        }

        result.value = parseResult;
        result.resolutionStr = '';
        result.timexStr = parseResult.timex;

        return result;
    }

    private parseChineseTimeOfDay(text: string, referenceTime: Date): DateTimeResolutionResult {
        let day = referenceTime.getDay();
        let month = referenceTime.getMonth();
        let year = referenceTime.getFullYear();
        let ret = new DateTimeResolutionResult();

        let parameters = this.GetMatchedTimexRange(text);
        if (!parameters.matched) {
            return new DateTimeResolutionResult();
        }

        ret.timex = parameters.timex;
        ret.futureValue = ret.pastValue = {
            item1: DateUtils.safeCreateFromMinValue(year, month, day, parameters.beginHour, 0, 0),
            item2: DateUtils.safeCreateFromMinValue(year, month, day, parameters.endHour, parameters.endMin, 0)
        };
        ret.success = true;

        return ret;
    }

    private GetMatchedTimexRange(text: string): { matched: boolean, timex: string, beginHour: number, endHour: number, endMin: number } {
        let trimmedText = text.trim();
        let matched = false;
        let timex = null;
        let beginHour = 0;
        let endHour = 0;
        let endMin = 0;

        let timeOfDay = "";
        if (ChineseDateTime.MorningTermList.some(o => trimmedText.endsWith(o))) {
            timeOfDay = Constants.Morning;
        }
        else if (ChineseDateTime.MidDayTermList.some(o => trimmedText.endsWith(o))) {
            timeOfDay = Constants.MidDay;
        }
        else if (ChineseDateTime.AfternoonTermList.some(o => trimmedText.endsWith(o))) {
            timeOfDay = Constants.Afternoon;
        }
        else if (ChineseDateTime.EveningTermList.some(o => trimmedText.endsWith(o))) {
            timeOfDay = Constants.Evening;
        }
        else if (ChineseDateTime.DaytimeTermList.some(o => trimmedText.localeCompare(o) == 0)) {
            timeOfDay = Constants.Daytime;
        }
        else if (ChineseDateTime.NightTermList.some(o => trimmedText.endsWith(o))) {
            timeOfDay = Constants.Night;
        }
        else {
            timex = null;
            matched = false;
            return { matched, timex, beginHour, endHour, endMin };
        }

        let parseResult = TimexUtil.parseTimeOfDay(timeOfDay);
        timex = parseResult.timeX;
        beginHour = parseResult.beginHour;
        endHour = parseResult.endHour;
        endMin = parseResult.endMin;

        matched = true;
        return { matched, timex, beginHour, endHour, endMin };
    }

    private parseTimePeriod(extra: DateTimeExtra<TimePeriodType>, referenceTime: Date): DateTimeResolutionResult {
        let result = new DateTimeResolutionResult();

        let leftEntity = extra.namedEntity('left');
        let leftResult = extra.dataType === TimePeriodType.FullTime
            ? this.getParseTimeResult(leftEntity, referenceTime)
            : this.getShortLeft(leftEntity.value);

        let rightEntity = extra.namedEntity('right');
        let rightResult = this.getParseTimeResult(rightEntity, referenceTime);

        // the right side doesn't contain desc while the left side does
        if (rightResult.lowBound === -1 && leftResult.lowBound !== -1 && rightResult.hour <= leftResult.lowBound) {
            rightResult.hour += 12;
        }

        let leftDate = this.buildDate(leftResult, referenceTime);
        let rightDate = this.buildDate(rightResult, referenceTime);

        if (rightDate.getHours() < leftDate.getHours()) {
            rightDate = DateUtils.addDays(rightDate, 1);
        }

        result.futureValue = result.pastValue = {
            item1: leftDate,
            item2: rightDate
        };
        let leftTimex = this.buildTimex(leftResult);
        let rightTimex = this.buildTimex(rightResult);
        let spanTimex = this.buildSpan(leftResult, rightResult);
        result.timex = `(${leftTimex},${rightTimex},${spanTimex})`;
        result.success = true;

        return result;
    }

    private getParseTimeResult(entity: any, referenceTime: Date): TimeResult {
        let extractResult: ExtractResult = {
            start: entity.index,
            length: entity.length,
            text: entity.value,
            type: Constants.SYS_DATETIME_TIME
        };
        let result = this.config.timeParser.parse(extractResult, referenceTime);
        return result.data;
    }

    private getShortLeft(source: string): TimeResult {
        let description = '';
        if (RegExpUtility.isMatch(this.dayDescriptionRegex, source)) {
            description = source.substr(0, source.length - 1);
        }

        let hour = TimeResolutionUtils.matchToValue(this.onlyDigitMatch, this.numbersMap, source.substr(source.length - 1));
        let timeResult = new TimeResult(hour, -1, -1);
        TimeResolutionUtils.addDescription(this.lowBoundMap, timeResult, description);
        return timeResult;
    }

    private buildDate(time: TimeResult, referenceTime: Date): Date {
        let day = referenceTime.getDate();
        let month = referenceTime.getMonth();
        let year = referenceTime.getFullYear();

        let hour = time.hour > 0 ? time.hour : 0;
        let min = time.minute > 0 ? time.minute : 0;
        let sec = time.second > 0 ? time.second : 0;

        return DateUtils.safeCreateFromMinValue(year, month, day, hour, min, sec);
    }

    private buildTimex(timeResult: TimeResult): string {
        let timex = 'T';
        if (timeResult.hour >= 0) {
            timex = timex + DateTimeFormatUtil.toString(timeResult.hour, 2);
            if (timeResult.minute >= 0) {
                timex = timex + ':' + DateTimeFormatUtil.toString(timeResult.minute, 2);
                if (timeResult.second >= 0) {
                    timex = timex + ':' + DateTimeFormatUtil.toString(timeResult.second, 2);
                }
            }
        }
        return timex;
    }

    private buildSpan(left: TimeResult, right: TimeResult): string {
        left = this.sanitizeTimeResult(left);
        right = this.sanitizeTimeResult(right);

        let spanHour = right.hour - left.hour;
        let spanMin = right.minute - left.minute;
        let spanSec = right.second - left.second;

        if (spanSec < 0) {
            spanSec += 60;
            spanMin -= 1;
        }

        if (spanMin < 0) {
            spanMin += 60;
            spanHour -= 1;
        }

        if (spanHour < 0) {
            spanHour += 24;
        }
        let spanTimex = `PT`;
        if (spanHour > 0 ) {
            spanTimex = spanTimex + `${spanHour}H`;
        }
        if (spanMin !== 0 && spanSec === 0) {
            spanTimex = spanTimex + `${spanMin}M`;
        }
        else if (spanSec !== 0) {
            spanTimex = spanTimex + `${spanMin}M${spanSec}S`;
        }
        return spanTimex;
    }

    private sanitizeTimeResult(timeResult: TimeResult): TimeResult {
        return new TimeResult(
            timeResult.hour,
            timeResult.minute === -1 ? 0 : timeResult.minute,
            timeResult.second === -1 ? 0 : timeResult.second
        );
    }
}