import { RegExpUtility, IExtractor } from "@microsoft/recognizers-text";
import { ITimePeriodExtractorConfiguration, ITimePeriodParserConfiguration } from "../baseTimePeriod";
import { BaseTimeExtractor, BaseTimeParser } from "../baseTime";
import { IDateTimeUtilityConfiguration } from "../utilities";
import { SpanishTimeExtractorConfiguration } from "./timeConfiguration";
import { SpanishDateTimeUtilityConfiguration } from "./baseConfiguration";
import { SpanishDateTime } from "../../resources/spanishDateTime";
import { ICommonDateTimeParserConfiguration } from "../parsers";
import { IDateTimeExtractor } from "../baseDateTime";
import { EnglishIntegerExtractor, NumberMode } from "@microsoft/recognizers-text-number";

export class SpanishTimePeriodExtractorConfiguration implements ITimePeriodExtractorConfiguration {
    readonly simpleCasesRegex: RegExp[];
    readonly tillRegex: RegExp;
    readonly timeOfDayRegex: RegExp;
    readonly generalEndingRegex: RegExp;
    readonly singleTimeExtractor: IDateTimeExtractor;
    readonly integerExtractor: IExtractor;
    readonly utilityConfiguration: SpanishDateTimeUtilityConfiguration;

    readonly fromRegex: RegExp;
    readonly connectorAndRegex: RegExp;
    readonly betweenRegex: RegExp;

    constructor() {
        this.singleTimeExtractor = new BaseTimeExtractor(new SpanishTimeExtractorConfiguration());
        this.integerExtractor = new EnglishIntegerExtractor();
        this.utilityConfiguration = new SpanishDateTimeUtilityConfiguration();

        this.simpleCasesRegex = [
            RegExpUtility.getSafeRegExp(SpanishDateTime.PureNumFromTo, "gis"),
            RegExpUtility.getSafeRegExp(SpanishDateTime.PureNumBetweenAnd, "gis")
        ];

        this.tillRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.TillRegex, "gis");
        this.timeOfDayRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.TimeOfDayRegex, "gis");
        this.generalEndingRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.GeneralEndingRegex, "gis");

        this.fromRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.FromRegex, "gis");
        this.connectorAndRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.ConnectorAndRegex, "gis");
        this.betweenRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.BetweenRegex, "gis");
    }

    getFromTokenIndex(text: string): { matched: boolean; index: number; } {
        return RegExpUtility.getFirstMatchIndex(this.fromRegex, text);
    }

    hasConnectorToken(text: string): boolean {
        return RegExpUtility.getFirstMatchIndex(this.connectorAndRegex, text).matched;
    }

    getBetweenTokenIndex(text: string): { matched: boolean; index: number; } {
        return RegExpUtility.getFirstMatchIndex(this.betweenRegex, text);
    }
}

export class SpanishTimePeriodParserConfiguration implements ITimePeriodParserConfiguration {
    readonly timeExtractor: IDateTimeExtractor;
    readonly timeParser: BaseTimeParser;
    readonly integerExtractor: IExtractor;
    readonly pureNumberFromToRegex: RegExp;
    readonly pureNumberBetweenAndRegex: RegExp;
    readonly timeOfDayRegex: RegExp;
    readonly tillRegex: RegExp;
    readonly numbers: ReadonlyMap<string, number>;
    readonly utilityConfiguration: IDateTimeUtilityConfiguration;
    readonly specificTimeFromToRegex: RegExp;
    readonly specificTimeBetweenAndRegex: RegExp;

    constructor(config: ICommonDateTimeParserConfiguration) {
        this.timeExtractor = config.timeExtractor;
        this.timeParser = config.timeParser;
        this.integerExtractor = config.integerExtractor;
        this.numbers = config.numbers;
        this.utilityConfiguration = config.utilityConfiguration;
        this.pureNumberFromToRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.PureNumFromTo, "gis");
        this.pureNumberBetweenAndRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.PureNumBetweenAnd, "gis");
        this.timeOfDayRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.TimeOfDayRegex, "gis");
        this.tillRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.TillRegex, "gis");
        this.specificTimeFromToRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.SpecificTimeFromTo);
        this.specificTimeBetweenAndRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.SpecificTimeBetweenAnd);
    }

    getMatchedTimexRange(text: string): { matched: boolean; timex: string; beginHour: number; endHour: number; endMin: number; } {

        let trimedText = text.trim().toLowerCase();

        let beginHour = 0;
        let endHour = 0;
        let endMin = 0;
        let timex = "";

        if (trimedText.endsWith("madrugada")) {
            timex = "TDA";
            beginHour = 4;
            endHour = 8;
        }
        else if (trimedText.endsWith("mañana")) {
            timex = "TMO";
            beginHour = 8;
            endHour = 12;
        }
        else if (trimedText.includes("pasado mediodia") || trimedText.includes("pasado el mediodia")) {
            timex = "TAF";
            beginHour = 12;
            endHour = 16;
        }
        else if (trimedText.endsWith("tarde")) {
            timex = "TEV";
            beginHour = 16;
            endHour = 20;
        }
        else if (trimedText.endsWith("noche")) {
            timex = "TNI";
            beginHour = 20;
            endHour = 23;
            endMin = 59;
        }
        else {
            timex = null;
            return {
                matched: false,
                timex,
                beginHour,
                endHour,
                endMin
            };
        }

        return {
            matched: true,
            timex,
            beginHour,
            endHour,
            endMin
        };
    }
}