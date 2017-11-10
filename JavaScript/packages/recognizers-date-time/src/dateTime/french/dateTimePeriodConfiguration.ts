import { BaseNumberExtractor, IExtractor, FrenchCardinalExtractor, RegExpUtility, IParser } from "recognizers-text-number";
import { IDateTimePeriodExtractorConfiguration, IDateTimePeriodParserConfiguration, BaseDateTimePeriodParser } from "../baseDateTimePeriod";
import { BaseDateExtractor, BaseDateParser } from "../baseDate";
import { BaseTimeExtractor, BaseTimeParser } from "../baseTime";
import { IDateTimeExtractor, BaseDateTimeExtractor, BaseDateTimeParser } from "../baseDateTime";
import { BaseDurationExtractor, BaseDurationParser } from "../baseDuration";
import { IDateTimeParser, ICommonDateTimeParserConfiguration } from "../parsers";
import { FrenchDateExtractorConfiguration } from "./dateConfiguration";
import { FrenchDateTimeExtractorConfiguration } from "./dateTimeConfiguration";
import { FrenchDurationExtractorConfiguration } from "./durationConfiguration";
import { FrenchDateTime } from "../../resources/frenchDateTime";
import { FrenchTimeExtractorConfiguration } from "./timeConfiguration";
import { DateTimeResolutionResult, DateUtils, FormatUtil } from "../utilities";

export class FrenchDateTimePeriodExtractorConfiguration implements IDateTimePeriodExtractorConfiguration {
    readonly cardinalExtractor: BaseNumberExtractor;
    readonly singleDateExtractor: IDateTimeExtractor;
    readonly singleTimeExtractor: IDateTimeExtractor;
    readonly singleDateTimeExtractor: IDateTimeExtractor;
    readonly durationExtractor: IDateTimeExtractor;
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
    readonly beforeRegex: RegExp;


    constructor() {
        this.simpleCasesRegexes = [
            RegExpUtility.getSafeRegExp(FrenchDateTime.PureNumFromTo, "gis"),
            RegExpUtility.getSafeRegExp(FrenchDateTime.PureNumBetweenAnd, "gis"),
            RegExpUtility.getSafeRegExp(FrenchDateTime.SpecificTimeOfDayRegex, "gis")
        ]

        this.prepositionRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.PrepositionRegex, "gis");
        this.tillRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.TillRegex, "gis");
        this.specificTimeOfDayRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.PeriodSpecificTimeOfDayRegex, "gis");
        this.timeOfDayRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.PeriodTimeOfDayRegex, "gis");
        this.followedUnit = RegExpUtility.getSafeRegExp(FrenchDateTime.TimeFollowedUnit, "gis");
        this.timeUnitRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.TimeUnitRegex, "gis");
        this.pastPrefixRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.PastSuffixRegex, "gis");
        this.nextPrefixRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.NextSuffixRegex, "gis");
        this.numberCombinedWithUnit = RegExpUtility.getSafeRegExp(FrenchDateTime.TimeNumberCombinedWithUnit, "gis");
        this.weekDayRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.WeekDayRegex, "gis");
        this.periodTimeOfDayWithDateRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.PeriodTimeOfDayWithDateRegex, "gis");
        this.relativeTimeUnitRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.RelativeTimeUnitRegex, "gis");
        this.restOfDateTimeRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.RestOfDateTimeRegex, "gis");

        this.fromRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.FromRegex2, "gis");
        this.connectorAndRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.ConnectorAndRegex, "gis");
        this.beforeRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.BeforeRegex, "gis");

        this.cardinalExtractor = new FrenchCardinalExtractor();

        this.singleDateExtractor = new BaseDateExtractor(new FrenchDateExtractorConfiguration());
        this.singleTimeExtractor = new BaseTimeExtractor(new FrenchTimeExtractorConfiguration());
        this.singleDateTimeExtractor = new BaseDateTimeExtractor(new FrenchDateTimeExtractorConfiguration());
        this.durationExtractor = new BaseDurationExtractor(new FrenchDurationExtractorConfiguration());
    }

    getFromTokenIndex(source: string): { matched: boolean; index: number; } {
        return RegExpUtility.getFirstMatchIndex(this.fromRegex, source);
    }

    getBetweenTokenIndex(source: string): { matched: boolean; index: number; } {
        return RegExpUtility.getFirstMatchIndex(this.beforeRegex, source);
    }

    hasConnectorToken(source: string): boolean {
        return RegExpUtility.getFirstMatchIndex(this.connectorAndRegex, source).matched;
    }
}

export class FrenchDateTimePeriodParserConfiguration implements IDateTimePeriodParserConfiguration {
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

    readonly morningStartEndRegex: RegExp;
    readonly afternoonStartEndRegex: RegExp;
    readonly eveningStartEndRegex: RegExp;
    readonly nightStartEndRegex: RegExp;

    readonly numbers: ReadonlyMap<string, number>;
    readonly unitMap: ReadonlyMap<string, string>;
    readonly dateExtractor: IDateTimeExtractor;
    readonly timeExtractor: IDateTimeExtractor;
    readonly dateTimeExtractor: IDateTimeExtractor;
    readonly timePeriodExtractor: IDateTimeExtractor;
    readonly durationExtractor: IDateTimeExtractor;
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

        this.nextPrefixRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.NextSuffixRegex, "gis");
        this.pastPrefixRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.PastSuffixRegex, "gis");
        this.thisPrefixRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.ThisPrefixRegex, "gis");

        this.morningStartEndRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.MorningStartEndRegex, "gis");
        this.afternoonStartEndRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.AfternoonStartEndRegex, "gis");
        this.eveningStartEndRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.EveningStartEndRegex, "gis");
        this.nightStartEndRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.NightStartEndRegex, "gis");

        this.pureNumberFromToRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.PureNumFromTo, "gis");
        this.pureNumberBetweenAndRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.PureNumBetweenAnd, "gis");
        this.specificTimeOfDayRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.SpecificTimeOfDayRegex, "gis");
        this.timeOfDayRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.TimeOfDayRegex, "gis");
        this.pastRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.PastSuffixRegex, "gis");
        this.futureRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.NextSuffixRegex, "gis");
        this.numberCombinedWithUnitRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.TimeNumberCombinedWithUnit, "gis");
        this.unitRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.TimeUnitRegex, "gis");
        this.periodTimeOfDayWithDateRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.PeriodTimeOfDayWithDateRegex, "gis");
        this.relativeTimeUnitRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.RelativeTimeUnitRegex, "gis");
        this.restOfDateTimeRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.RestOfDateTimeRegex, "gis");
    }

    getMatchedTimeRange(source: string): { timeStr: string; beginHour: number; endHour: number; endMin: number; success: boolean; } {
        let trimedText = source.trim().toLowerCase();
        let timeStr = "";
        let beginHour = 0;
        let endHour = 0;
        let endMin = 0;

        if (RegExpUtility.getFirstMatchIndex(this.morningStartEndRegex, trimedText).matched) {
            timeStr = "TMO";
            beginHour = 8;
            endHour = 12;
        }
        else if (RegExpUtility.getFirstMatchIndex(this.afternoonStartEndRegex, trimedText).matched) {
            timeStr = "TAF";
            beginHour = 12;
            endHour = 16;
        }
        else if (RegExpUtility.getFirstMatchIndex(this.eveningStartEndRegex, trimedText).matched) {
            timeStr = "TEV";
            beginHour = 16;
            endHour = 20;
        }
        else if (RegExpUtility.getFirstMatchIndex(this.nightStartEndRegex, trimedText).matched) {
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
        if (trimedText.startsWith("prochain") || 
            trimedText.endsWith("prochain") ||
            trimedText.startsWith("prochaine") || 
            trimedText.endsWith("prochaine")) {
            swift = 1;
        }
        else if (trimedText.startsWith("derniere") || 
            trimedText.startsWith("dernier")||
            trimedText.endsWith("derniere") || 
            trimedText.endsWith("dernier")) {
            swift = -1;
        }

        return swift;
    }
}