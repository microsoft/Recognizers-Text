import { ExtractResult, RegExpUtility, CultureInfo, Culture } from "recognizers-text-number";
import { NumberWithUnitExtractor, ChineseNumberWithUnitExtractorConfiguration } from "recognizers-text-number-with-unit";
import { BaseDateTimeExtractor } from "./baseDateTime";
import { Constants, TimeTypeConstants } from "../constants"
import { ChineseDateTime } from "../../resources/chineseDateTime";

export enum TimeType {
    ChineseTime,
    LessTime,
    DigitTime
}

export class ChineseTimeExtractor extends BaseDateTimeExtractor<TimeType> {
    protected extractorName = Constants.SYS_DATETIME_TIME; // "Time";

    constructor() {
        super(new Map<RegExp, TimeType>([
            [ RegExpUtility.getSafeRegExp(ChineseDateTime.TimeRegexes1), TimeType.ChineseTime ],
            [ RegExpUtility.getSafeRegExp(ChineseDateTime.TimeRegexes2), TimeType.LessTime ],
            [ RegExpUtility.getSafeRegExp(ChineseDateTime.TimeRegexes3), TimeType.DigitTime ]
        ]));
    }
}