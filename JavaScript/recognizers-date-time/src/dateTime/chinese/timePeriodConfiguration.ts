import { ExtractResult, RegExpUtility, CultureInfo, Culture } from "recognizers-text-number";
import { NumberWithUnitExtractor, ChineseNumberWithUnitExtractorConfiguration } from "recognizers-text-number-with-unit";
import { BaseDateTimeExtractor } from "./baseDateTime";
import { Constants, TimeTypeConstants } from "../constants"
import { ChineseDateTime } from "../../resources/chineseDateTime";

export enum TimePeriodType {
    ShortTime,
    FullTime
}

export class ChineseTimePeriodExtractor extends BaseDateTimeExtractor<TimePeriodType> {
    protected extractorName = Constants.SYS_DATETIME_TIMEPERIOD; // "time range";

    constructor() {
        super(new Map<RegExp, TimePeriodType>([
            [ RegExpUtility.getSafeRegExp(ChineseDateTime.TimePeriodRegexes1), TimePeriodType.FullTime ],
            [ RegExpUtility.getSafeRegExp(ChineseDateTime.TimePeriodRegexes2), TimePeriodType.ShortTime ]
        ]));
    }
}