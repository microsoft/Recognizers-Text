import { IDateTimePeriodExtractorConfiguration, IDateTimePeriodParserConfiguration, BaseDateTimePeriodParser } from "../baseDateTimePeriod";
import { BaseNumberExtractor, IExtractor } from "../../number/extractors";
import { BaseDateExtractor, BaseDateParser } from "../baseDate";
import { BaseTimeExtractor, BaseTimeParser } from "../baseTime";
import { BaseDateTimeExtractor, BaseDateTimeParser } from "../baseDateTime";
import { BaseDurationExtractor, BaseDurationParser } from "../baseDuration";
import { IDateTimeParser, ICommonDateTimeParserConfiguration } from "../parsers";
import { SpanishCardinalExtractor } from "../../number/spanish/extractors";
import { SpanishDateExtractorConfiguration } from "./dateConfiguration";
import { SpanishDateTimeExtractorConfiguration } from "./dateTimeConfiguration";
import { SpanishDurationExtractorConfiguration } from "./durationConfiguration";
import { RegExpUtility } from "../../utilities";
import { SpanishDateTime } from "../../resources/spanishDateTime";
import { IParser } from "../../number/parsers";
import { SpanishTimeExtractorConfiguration } from "./timeConfiguration";
import { DateTimeResolutionResult, DateUtils, FormatUtil } from "../utilities";

export class SpanishDateTimePeriodExtractorConfiguration implements IDateTimePeriodExtractorConfiguration {
    readonly cardinalExtractor: BaseNumberExtractor;
    readonly singleDateExtractor: BaseDateExtractor;
    readonly singleTimeExtractor: BaseTimeExtractor;
    readonly singleDateTimeExtractor: BaseDateTimeExtractor;
    readonly durationExtractor: BaseDurationExtractor;
    readonly simpleCasesRegexes: RegExp[];
    readonly prepositionRegex: RegExp;
    readonly tillRegex: RegExp;
    readonly specificTimeOfDayRegex: RegExp;
    readonly timeOfDayRegex: RegExp;
    readonly periodTimeOfDayWithDateRegex: RegExp;
    readonly followedUnit: RegExp;
    readonly numberCombinedWithUnit: RegExp;
    readonly timeUnitRegex: RegExp;
    readonly pastPrefixRegex: RegExp;
    readonly nextPrefixRegex: RegExp;
    readonly relativeTimeUnitRegex: RegExp;
    readonly restOfDateTimeRegex: RegExp;
    readonly weekDayRegex: RegExp;

    readonly fromRegex: RegExp;
    readonly connectorAndRegex: RegExp;
    readonly betweenRegex: RegExp;


    constructor() {
        this.simpleCasesRegexes = [
            RegExpUtility.getSafeRegExp(SpanishDateTime.PureNumFromTo, "gis"),
            RegExpUtility.getSafeRegExp(SpanishDateTime.PureNumBetweenAnd, "gis")
        ]

        this.prepositionRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.PrepositionRegex, "gis");
        this.tillRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.TillRegex, "gis");
        this.specificTimeOfDayRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.SpecificTimeOfDayRegex, "gis");
        this.timeOfDayRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.TimeOfDayRegex, "gis");
        this.followedUnit = RegExpUtility.getSafeRegExp(SpanishDateTime.FollowedUnit, "gis");
        this.timeUnitRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.UnitRegex, "gis");
        this.pastPrefixRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.PastRegex, "gis");
        this.nextPrefixRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.FutureRegex, "gis");
        this.numberCombinedWithUnit = RegExpUtility.getSafeRegExp(SpanishDateTime.DateTimePeriod_NumberCombinedWithUnit, "gis");
        this.weekDayRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.WeekDayRegex, "gis");
        this.periodTimeOfDayWithDateRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.PeriodTimeOfDayWithDateRegex, "gis");
        this.relativeTimeUnitRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.RelativeTimeUnitRegex, "gis");
        this.restOfDateTimeRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.RestOfDateTimeRegex, "gis");

        this.fromRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.FromRegex, "gis");
        this.connectorAndRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.ConnectorAndRegex, "gis");
        this.betweenRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.BetweenRegex, "gis");

        this.cardinalExtractor = new SpanishCardinalExtractor();

        this.singleDateExtractor = new BaseDateExtractor(new SpanishDateExtractorConfiguration());
        this.singleTimeExtractor = new BaseTimeExtractor(new SpanishTimeExtractorConfiguration());
        this.singleDateTimeExtractor = new BaseDateTimeExtractor(new SpanishDateTimeExtractorConfiguration());
        this.durationExtractor = new BaseDurationExtractor(new SpanishDurationExtractorConfiguration());
    }

    getFromTokenIndex(source: string): { matched: boolean; index: number; } {
        return RegExpUtility.getFirstMatchIndex(this.fromRegex, source);
    }

    getBetweenTokenIndex(source: string): { matched: boolean; index: number; } {
        return RegExpUtility.getFirstMatchIndex(this.betweenRegex, source);
    }

    hasConnectorToken(source: string): boolean {
        return RegExpUtility.getFirstMatchIndex(this.connectorAndRegex, source).matched;
    }
}

export class SpanishDateTimePeriodParserConfiguration implements IDateTimePeriodParserConfiguration {
    readonly pureNumberFromToRegex: RegExp;
    readonly pureNumberBetweenAndRegex: RegExp;
    readonly periodTimeOfDayWithDateRegex: RegExp;
    readonly specificTimeOfDayRegex: RegExp;
    readonly pastRegex: RegExp;
    readonly futureRegex: RegExp;
    readonly relativeTimeUnitRegex: RegExp;
    readonly restOfDateTimeRegex: RegExp;
    readonly timeOfDayRegex: RegExp;
    readonly numberCombinedWithUnitRegex: RegExp;
    readonly unitRegex: RegExp;

    readonly nextPrefixRegex: RegExp;
    readonly pastPrefixRegex: RegExp;
    readonly thisPrefixRegex: RegExp;

    readonly numbers: ReadonlyMap<string, number>;
    readonly unitMap: ReadonlyMap<string, string>;
    readonly dateExtractor: BaseDateExtractor;
    readonly timeExtractor: BaseTimeExtractor;
    readonly dateTimeExtractor: BaseDateTimeExtractor;
    readonly timePeriodExtractor: IExtractor;
    readonly durationExtractor: BaseDurationExtractor;
    readonly dateParser: BaseDateParser;
    readonly timeParser: BaseTimeParser;
    readonly dateTimeParser: BaseDateTimeParser;
    readonly timePeriodParser: IDateTimeParser;
    readonly durationParser: BaseDurationParser;
    readonly cardinalExtractor: IExtractor;
    readonly numberParser: IParser;

    constructor(config: ICommonDateTimeParserConfiguration) {
        this.dateExtractor = config.dateExtractor;
        this.timeExtractor = config.timeExtractor;
        this.dateTimeExtractor = config.dateTimeExtractor;
        this.timePeriodExtractor = config.timePeriodExtractor;
        this.cardinalExtractor = config.cardinalExtractor;
        this.durationExtractor = config.durationExtractor;
        this.numberParser = config.numberParser;
        this.dateParser = config.dateParser;
        this.timeParser = config.timeParser;
        this.dateTimeParser = config.dateTimeParser;
        this.timePeriodParser = config.timePeriodParser;
        this.durationParser = config.durationParser;
        this.unitMap = config.unitMap;
        this.numbers = config.numbers;

        this.nextPrefixRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.NextPrefixRegex, "gis");
        this.pastPrefixRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.PastPrefixRegex, "gis");
        this.thisPrefixRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.ThisPrefixRegex, "gis");

        this.pureNumberFromToRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.PureNumFromTo, "gis");
        this.pureNumberBetweenAndRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.PureNumBetweenAnd, "gis");
        this.specificTimeOfDayRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.SpecificTimeOfDayRegex, "gis");
        this.timeOfDayRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.TimeOfDayRegex, "gis");
        this.pastRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.PastRegex, "gis");
        this.futureRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.FutureRegex, "gis");
        this.numberCombinedWithUnitRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.DateTimePeriod_NumberCombinedWithUnit, "gis");
        this.unitRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.UnitRegex, "gis");
        this.periodTimeOfDayWithDateRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.PeriodTimeOfDayWithDateRegex, "gis");
        this.relativeTimeUnitRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.RelativeTimeUnitRegex, "gis");
        this.restOfDateTimeRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.RestOfDateTimeRegex, "gis");
    }

    getMatchedTimeRange(source: string): { timeStr: string; beginHour: number; endHour: number; endMin: number; success: boolean; } {
        let trimedText = source.trim().toLowerCase();
        let timeStr = "";
        let beginHour = 0;
        let endHour = 0;
        let endMin = 0;

        if (trimedText.endsWith("madrugada")) {
            timeStr = "TDA";
            beginHour = 4;
            endHour = 8;
        }
        else if (trimedText.endsWith("mañana")) {
            timeStr = "TMO";
            beginHour = 8;
            endHour = 12;
        }
        else if (trimedText.includes("pasado mediodia") || trimedText.includes("pasado el mediodia")) {
            timeStr = "TAF";
            beginHour = 12;
            endHour = 16;
        }
        else if (trimedText.endsWith("tarde")) {
            timeStr = "TEV";
            beginHour = 16;
            endHour = 20;
        }
        else if (trimedText.endsWith("noche")) {
            timeStr = "TNI";
            beginHour = 20;
            endHour = 23;
            endMin = 59;
        }
        else {
            timeStr = null;
            return {
                success: false,
                timeStr,
                beginHour,
                endHour,
                endMin
            };
        }

        return {
            success: true,
            timeStr,
            beginHour,
            endHour,
            endMin
        };
    }

    getSwiftPrefix(source: string): number {
        let trimedText = source.trim().toLowerCase();
        let swift = 0;

        // TODO: Replace with a regex
        if (RegExpUtility.getFirstMatchIndex(this.pastPrefixRegex, trimedText).matched ||
            trimedText === "anoche") {
            swift = -1;
        }
        else if (RegExpUtility.getFirstMatchIndex(this.nextPrefixRegex, trimedText).matched) {
            swift = 1;
        }

        return swift;
    }
}

export class SpanishDateTimePeriodParser extends BaseDateTimePeriodParser {
    constructor(config: IDateTimePeriodParserConfiguration) {
        super(config);
    }

    protected parseSpecificTimeOfDay(source: string, referenceDate: Date): DateTimeResolutionResult {

        let ret = new DateTimeResolutionResult();
        let trimedText = source.trim().toLowerCase();

        // handle morning, afternoon..
        let beginHour = 0;
        let endHour = 0;
        let endMin = 0;
        let timeStr = "";

        let match = this.config.getMatchedTimeRange(trimedText);
        if (!match.success) {
            return ret;
        }

        let matches = RegExpUtility.getMatches(this.config.specificTimeOfDayRegex, trimedText);
        if (matches.length && matches[0].index === 0 && matches[0].length === trimedText.length) {
            let swift = this.config.getSwiftPrefix(trimedText);

            let date = DateUtils.addDays(referenceDate, swift);
            let day = date.getDate();
            let month = date.getMonth();
            let year = date.getFullYear();;

            ret.timex = FormatUtil.toString(year, 4) + timeStr;

            ;

            ret.pastValue = ret.futureValue = [
                DateUtils.safeCreateFromValue(DateUtils.minValue(), year, month, day, beginHour, 0, 0),
                DateUtils.safeCreateFromValue(DateUtils.minValue(), year, month, day, endHour, endMin, endMin),
            ];

            ret.success = true;
            return ret;
        }

        let startIndex = trimedText.indexOf(SpanishDateTime.Tomorrow) === 0 ? SpanishDateTime.Tomorrow.length : 0;

        // handle Date followed by morning, afternoon
        // Add handling code to handle morning, afternoon followed by Date
        // Add handling code to handle early/late morning, afternoon
        // TODO: use regex from config: match = this.config.TimeOfDayRegex.Match(trimedText.Substring(startIndex));
        matches = RegExpUtility.getMatches(RegExpUtility.getSafeRegExp(SpanishDateTime.TimeOfDayRegex), trimedText.substring(startIndex));
        if (matches.length) {
            let match = matches[0];
            let beforeStr = trimedText.substring(0, match.index + startIndex).trim();
            let ers = this.config.dateExtractor.extract(beforeStr);
            if (ers.length === 0) {
                return ret;
            }

            let pr = this.config.dateParser.parse(ers[0], referenceDate);

            let futureDate = (pr.value as DateTimeResolutionResult).futureValue;
            let pastDate = (pr.value as DateTimeResolutionResult).pastValue;

            ret.timex = pr.timexStr + timeStr;

            ret.futureValue = [
                DateUtils.safeCreateFromValue(DateUtils.minValue(), futureDate.Year, futureDate.Month, futureDate.Day, beginHour, 0, 0),
                DateUtils.safeCreateFromValue(DateUtils.minValue(), futureDate.Year, futureDate.Month, futureDate.Day, endHour, endMin, endMin)
            ];

            ret.pastValue = [
                DateUtils.safeCreateFromValue(DateUtils.minValue(), pastDate.Year, pastDate.Month, pastDate.Day, beginHour, 0, 0),
                DateUtils.safeCreateFromValue(DateUtils.minValue(), pastDate.Year, pastDate.Month, pastDate.Day, endHour, endMin, endMin)
            ];

            ret.success = true;

            return ret;
        }

        return ret;

    }
}