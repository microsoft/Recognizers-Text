import { IParser, ParseResult } from "../number/parsers";
import { ExtractResult, IExtractor } from "../number/extractors";
import { IDateTimeUtilityConfiguration } from "./utilities"
import { BaseDateTime } from "../resources/baseDateTime";
import { Constants, TimeTypeConstants } from "./constants";
import { FormatUtil, DateTimeResolutionResult } from "./utilities";
import { RegExpUtility,Match } from "../utilities";

export class DateTimeParseResult extends ParseResult {
    // TimexStr is only used in extractors related with date and time
    // It will output the TIMEX representation of a time string.
    timexStr: string
}

export interface IDateTimeParser extends IParser {
    parse(extResult: ExtractResult): ParseResult | null;
    parseWithReferenceTime(extResult: ExtractResult, referenceDate?: Date): DateTimeParseResult | null;
}

export interface ICommonDateTimeParserConfiguration {
    cardinalExtractor: IExtractor;
    integerExtractor: IExtractor;
    ordinalExtractor: IExtractor;
    numberParser: IParser;
    dateExtractor: IExtractor;
    timeExtractor: IExtractor;
    dateTimeExtractor: IExtractor;
    durationExtractor: IExtractor;
    datePeriodExtractor: IExtractor;
    timePeriodExtractor: IExtractor;
    dateTimePeriodExtractor: IExtractor;
    dateParser: IDateTimeParser;
    timeParser: IDateTimeParser;
    dateTimeParser: IDateTimeParser;
    durationParser: IDateTimeParser;
    datePeriodParser: IDateTimeParser;
    timePeriodParser: IDateTimeParser;
    dateTimePeriodParser: IDateTimeParser;
    monthOfYear: ReadonlyMap<string, number>;
    numbers: ReadonlyMap<string, number>;
    unitValueMap: ReadonlyMap<string, number>;
    seasonMap: ReadonlyMap<string, string>;
    unitMap: ReadonlyMap<string, string>;
    cardinalMap: ReadonlyMap<string, number>;
    dayOfMonth: ReadonlyMap<string, number>;
    dayOfWeek: ReadonlyMap<string, number>;
    doubleNumbers: ReadonlyMap<string, number>;
    utilityConfiguration: IDateTimeUtilityConfiguration;
}

export abstract class BaseDateParserConfiguration implements ICommonDateTimeParserConfiguration {
    cardinalExtractor: IExtractor;
    integerExtractor: IExtractor;
    ordinalExtractor: IExtractor;
    numberParser: IParser;
    dateExtractor: IExtractor;
    timeExtractor: IExtractor;
    dateTimeExtractor: IExtractor;
    durationExtractor: IExtractor;
    datePeriodExtractor: IExtractor;
    timePeriodExtractor: IExtractor;
    dateTimePeriodExtractor: IExtractor;
    dateParser: IDateTimeParser;
    timeParser: IDateTimeParser;
    dateTimeParser: IDateTimeParser;
    durationParser: IDateTimeParser;
    datePeriodParser: IDateTimeParser;
    timePeriodParser: IDateTimeParser;
    dateTimePeriodParser: IDateTimeParser;
    monthOfYear: ReadonlyMap<string, number>;
    numbers: ReadonlyMap<string, number>;
    unitValueMap: ReadonlyMap<string, number>;
    seasonMap: ReadonlyMap<string, string>;
    unitMap: ReadonlyMap<string, string>;
    cardinalMap: ReadonlyMap<string, number>;
    dayOfMonth: ReadonlyMap<string, number>;
    dayOfWeek: ReadonlyMap<string, number>;
    doubleNumbers: ReadonlyMap<string, number>;
    utilityConfiguration: IDateTimeUtilityConfiguration;

    constructor() {
        this.dayOfMonth = BaseDateTime.DayOfMonthDictionary;
    }
}

export interface ITimeParserConfiguration {
    timeTokenPrefix: string;
    atRegex: RegExp
    timeRegexes: RegExp[];
    numbers: ReadonlyMap<string, number>;
    adjustByPrefix(prefix: string, adjust: { hour: number, min: number, hasMin: boolean });
    adjustBySuffix(suffix: string, adjust: { hour: number, min: number, hasMin: boolean, hasAm: boolean, hasPm: boolean });
}

export class BaseTimeParser implements IDateTimeParser
{
    readonly ParserName = Constants.SYS_DATETIME_TIME; //"Time";
    readonly config:ITimeParserConfiguration;

    constructor( configuration:ITimeParserConfiguration)
    {
        this.config = configuration;
    }

    public parse( result:ExtractResult):ParseResult
    {
        return this.parseWithReferenceTime(result, new Date());
    }

    public parseWithReferenceTime(er: ExtractResult, referenceTime: Date): DateTimeParseResult {
        let value = null;
        if (er.type == this.ParserName) {
            let innerResult = this.internalParse(er.text, referenceTime);
            if (innerResult.success) {
                innerResult.futureResolution = new Map<string, string>(
                    [
                        [TimeTypeConstants.TIME, FormatUtil.formatTime(innerResult.futureValue)]
                    ]);

                innerResult.pastResolution = new Map<string, string>(
                    [
                        [TimeTypeConstants.TIME, FormatUtil.formatTime(innerResult.pastValue)]
                    ]);

                value = innerResult;
            }
        }

        let ret = new DateTimeParseResult(er);
        ret.value = value,
        ret.timexStr = value == null ? "" : value.Timex,
        ret.resolutionStr = ""

        return ret;
    }

    internalParse( text:string, referenceTime:Date):DateTimeResolutionResult
    {
        let innerResult = this.parseBasicRegexMatch(text, referenceTime);
        return innerResult;
    }

    // parse basic patterns in TimeRegexList
    private parseBasicRegexMatch(text: string, referenceTime: Date): DateTimeResolutionResult {
        let trimedText = text.trim().toLowerCase();
        let offset = 0;

        let matches = RegExpUtility.getMatches(this.config.atRegex, trimedText);
        if (matches.length == 0) {
            matches = RegExpUtility.getMatches(this.config.atRegex, this.config.timeTokenPrefix + trimedText);
            offset = this.config.timeTokenPrefix.length;
        }

        if (matches.length > 0 && matches[0].index == offset && matches[0].length == trimedText.length) {
            return this.match2Time(matches[0], referenceTime);
        }

        this.config.timeRegexes.forEach(regex => {
            offset = 0;
            matches = RegExpUtility.getMatches(regex, trimedText);

            if (matches.length && matches[0].index == offset && matches[0].length == trimedText.length) {
                return this.match2Time(matches[0], referenceTime);
            }
        });

        return new DateTimeResolutionResult();
    }

    private  match2Time( match:Match,  referenceTime:Date):DateTimeResolutionResult
    {
        let ret = new DateTimeResolutionResult();
        let hour = 0,
            min = 0,
            second = 0,
            day = referenceTime.getDay(),
            month = referenceTime.getMonth(),
            year = referenceTime.getFullYear();
        let hasMin = false, hasSec = false, hasAm = false, hasPm = false;

        let engTimeStr = match.groups["engtime"];
        if (!engTimeStr)
        {
            // get hour
            let hourStr = match.groups["hournum"].toLowerCase();
            hour = this.config.numbers[hourStr];

            // get minute
            let minStr = match.groups["minnum"];
            let tensStr = match.groups["tens"];

            if (minStr)
            {
                min = this.config.numbers[minStr];
                if (!tensStr)
                {
                    min += this.config.numbers[tensStr];
                }
                hasMin = true;
            }
        }
        else
        {
            // get hour
            let hourStr = match.groups["hour"];
            if (!(hourStr))
            {
                hourStr = match.groups["hournum"].ToLowerCase();
                hour=this.config.numbers[hourStr];
                if (!hour)
                {
                    return ret;
                }
            }
            else
            {
                hour = parseInt(hourStr);
            }

            // get minute
            let minStr = match.groups["min"].toLowerCase();
            if (!(minStr))
            {
                minStr = match.groups["minnum"];
                if (minStr)
                {
                    min = this.config.numbers[minStr];
                    hasMin = true;
                }

                let tensStr = match.groups["tens"];
                if (tensStr)
                {
                    min += this.config.numbers[tensStr];
                    hasMin = true;
                }
            }
            else
            {
                min = parseInt(minStr);
                hasMin = true;
            }

            // get second
            let secStr = match.groups["sec"].toLowerCase();
            if (secStr)
            {
                second = parseInt(secStr);
                hasSec = true;
            }
        }

        //adjust by desc string
        let descStr = match.groups["desc"].toLowerCase();
        if (descStr)
        {
            if (descStr.toLower().StartsWith("a"))
            {
                if (hour >= 12)
                {
                    hour -= 12;
                }
                hasAm = true;
            }
            else if (descStr.toLowerCase().startsWith("p"))
            {
                if (hour < 12)
                {
                    hour += 12;
                }
                hasPm = true;
            }
        }

        // adjust min by prefix
        let timePrefix = match.groups["prefix"].toLowerCase();
        if (timePrefix) {
            let adjust = { hour: hour, min: min, hasMin: hasMin };
            this.config.adjustByPrefix(timePrefix, adjust);
            hour = adjust.hour; min = adjust.min; hasMin = adjust.hasMin;
        }

        // adjust hour by suffix
        let timeSuffix = match.groups["suffix"].toLowerCase();
        if (timeSuffix) {
            let adjust = { hour: hour, min: min, hasMin: hasMin, hasAm: hasAm, hasPm: hasPm };
            this.config.adjustBySuffix(timeSuffix, adjust);
            hour = adjust.hour; min = adjust.min; hasMin = adjust.hasMin; hasAm = adjust.hasAm; hasPm = adjust.hasPm;
        }

        if (hour == 24)
        {
            hour = 0;
        }

        ret.timex = "T" + FormatUtil.toString(hour,2);
        if (hasMin)
        {
            ret.timex += ":" + FormatUtil.toString(min,2);
        }

        if (hasSec)
        {
            ret.timex += ":" + FormatUtil.toString(second,2);
        }

        if (hour <= 12 && !hasPm && !hasAm)
        {
            ret.comment = "ampm";
        }

        ret.futureValue = ret.pastValue = new Date(year, month, day, hour, min, second);
        ret.success = true;

        return ret;
    }
}