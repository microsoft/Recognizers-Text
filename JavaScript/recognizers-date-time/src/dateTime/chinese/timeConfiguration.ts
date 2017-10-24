import { ExtractResult, RegExpUtility, CultureInfo, Culture, StringUtility } from "recognizers-text-number";
import { NumberWithUnitExtractor, ChineseNumberWithUnitExtractorConfiguration } from "recognizers-text-number-with-unit";
import { BaseDateTimeExtractor, DateTimeExtra } from "./baseDateTime";
import { BaseTimeParser } from "../baseTime";
import { Constants, TimeTypeConstants } from "../constants"
import { IDateTimeParser, DateTimeParseResult } from "../parsers"
import { DateTimeResolutionResult, FormatUtil, DateUtils } from "../utilities";
import { ChineseDateTime } from "../../resources/chineseDateTime";

export enum TimeType {
    ChineseTime,
    LessTime,
    DigitTime
}

class TimeResult {
    hour: number;
    minute: number;
    second: number;
    lowBound: number;

    constructor(hour: number, minute: number, second: number, lowBound?: number) {
        this.hour = hour;
        this.minute = minute;
        this.second = second;
        this.lowBound = lowBound ? lowBound : -1;
    }
}

export class ChineseTimeExtractor extends BaseDateTimeExtractor<TimeType> {
    protected extractorName = Constants.SYS_DATETIME_TIME; // "Time";

    constructor() {
        super(new Map<RegExp, TimeType>([
            [ RegExpUtility.getSafeRegExp(ChineseDateTime.TimeRegexes1), TimeType.ChineseTime ],
            [ RegExpUtility.getSafeRegExp(ChineseDateTime.TimeRegexes2), TimeType.DigitTime ],
            [ RegExpUtility.getSafeRegExp(ChineseDateTime.TimeRegexes3), TimeType.LessTime ]
        ]));
    }
}

export class ChineseTimeParser extends BaseTimeParser {
    private readonly numbersMap: ReadonlyMap<string, number>;
    private readonly lowBoundMap: ReadonlyMap<string, number>;
    private readonly functionMap: ReadonlyMap<TimeType, (extra: DateTimeExtra<TimeType>) => TimeResult>;
    private readonly onlyDigitMatch: RegExp;

    constructor() {
        super(null);
        this.functionMap = new Map<TimeType, (extra: DateTimeExtra<TimeType>) => TimeResult>([
            [TimeType.DigitTime, x => this.handleDigit(x)],
            [TimeType.ChineseTime, x => this.handleChinese(x)],
            [TimeType.LessTime, x => this.handleLess(x)]
        ]);
        this.onlyDigitMatch = RegExpUtility.getSafeRegExp('\\d+');
        this.numbersMap = ChineseDateTime.TimeNumberDictionary;
        this.lowBoundMap = ChineseDateTime.TimeLowBoundDesc;
    }

    public parse(er: ExtractResult, referenceTime?: Date): DateTimeParseResult | null {
        if (!referenceTime) referenceTime = new Date();
        
        let extra: DateTimeExtra<TimeType> = er.data;
        if (!extra) {
            return null;
        }

        let timeResult = this.functionMap.get(extra.dataType)(extra);
        let parseResult = this.packTimeResult(extra, timeResult, referenceTime);

        if (parseResult.success) {
            parseResult.futureResolution = new Map<string, string>([
                [TimeTypeConstants.TIME, FormatUtil.formatTime(parseResult.futureValue)]
            ]);
            parseResult.pastResolution = new Map<string, string>([
                [TimeTypeConstants.TIME, FormatUtil.formatTime(parseResult.pastValue)]
            ]);
        }

        let result = new DateTimeParseResult(er);
        result.value = parseResult;
        result.data = timeResult;
        result.resolutionStr = '';
        result.timexStr = parseResult.timex;

        return result;
    }

    private handleLess(extra: DateTimeExtra<TimeType>): TimeResult {
        let hour = this.matchToValue(extra.namedEntity('hour').value);
        let quarter = this.matchToValue(extra.namedEntity('quarter').value);
        let minute = !StringUtility.isNullOrEmpty(extra.namedEntity('half').value)
            ? 30
            : quarter !== -1 ? quarter * 15 : 0;
        let second = this.matchToValue(extra.namedEntity('sec').value);
        let less = this.matchToValue(extra.namedEntity('min').value);

        let all = hour * 60 + minute - less;
        if (all < 0) {
            all += 1440;
        }

        return new TimeResult(all / 60, all % 60, second);
    }
    
    private handleChinese(extra: DateTimeExtra<TimeType>): TimeResult {
        let hour = this.matchToValue(extra.namedEntity('hour').value);
        let quarter = this.matchToValue(extra.namedEntity('quarter').value);
        let minute = !StringUtility.isNullOrEmpty(extra.namedEntity('half').value)
            ? 30
            : quarter !== -1 ? quarter
            : this.matchToValue(extra.namedEntity('min').value);
        let second = this.matchToValue(extra.namedEntity('sec').value);

        return new TimeResult(hour, minute, second);
    }
    
    private handleDigit(extra: DateTimeExtra<TimeType>): TimeResult {
        return new TimeResult(
            this.matchToValue(extra.namedEntity('hour').value),
            this.matchToValue(extra.namedEntity('min').value),
            this.matchToValue(extra.namedEntity('sec').value)
        );
    }

    private packTimeResult(extra: DateTimeExtra<TimeType>, timeResult: TimeResult, referenceTime: Date): DateTimeResolutionResult {
        let result = new DateTimeResolutionResult();
        let dayDescription = extra.namedEntity('daydesc').value;
        let noDescription = StringUtility.isNullOrEmpty(dayDescription);
        if (noDescription) {
            result.comment = 'ampm';
        } else {
            this.addDescription(timeResult, dayDescription);
        }

        let hour = timeResult.hour > 0 ? timeResult.hour : 0;
        let min = timeResult.minute > 0 ? timeResult.minute : 0;
        let sec = timeResult.second > 0 ? timeResult.second : 0;
        let day = referenceTime.getDate();
        let month = referenceTime.getMonth();
        let year = referenceTime.getFullYear();

        let timex = 'T';
        if (timeResult.hour >= 0) {
            timex = timex + FormatUtil.toString(timeResult.hour, 2);
            if (timeResult.minute >= 0) {
                timex = timex + ':' + FormatUtil.toString(timeResult.minute, 2);
                if (timeResult.second >= 0) {
                    timex = timex + ':' + FormatUtil.toString(timeResult.second, 2);
                }
            }
        }
        if (hour === 24) {
            hour = 0;
        }
        
        result.futureValue = DateUtils.safeCreateFromMinValue(year, month, day, hour, min, sec);
        result.pastValue = DateUtils.safeCreateFromMinValue(year, month, day, hour, min, sec);
        result.timex = timex;
        result.success = true;

        return result;
    }

    private addDescription(timeResult: TimeResult, description: string) {
        if (this.lowBoundMap.has(description) && timeResult.hour < this.lowBoundMap.get(description)) {
            timeResult.hour += 12;
            timeResult.lowBound = this.lowBoundMap.get(description);
        } else {
            timeResult.lowBound = 0;
        }
    }

    private matchToValue(source: string): number {
        if (StringUtility.isNullOrEmpty(source)) {
            return -1;
        }

        if (RegExpUtility.isMatch(this.onlyDigitMatch, source)) {
            return Number.parseInt(source);
        }

        if (source.length === 1) {
            return this.numbersMap.get(source);
        }

        let value = 1;
        for (let index = 0; index < source.length; index++) {
            let char = source.charAt(index);
            if (char === '十') {
                value *= 10;
            } else if (index === 0) {
                value *= this.numbersMap.get(char);
            } else {
                value += this.numbersMap.get(char);
            }
        }

        return value;
    }
}