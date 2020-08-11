import { RegExpUtility, IExtractor } from "@microsoft/recognizers-text";
import { ITimePeriodExtractorConfiguration, ITimePeriodParserConfiguration } from "../baseTimePeriod";
import { BaseTimeExtractor, BaseTimeParser } from "../baseTime";
import { IDateTimeUtilityConfiguration, TimexUtil } from "../utilities";
import { SpanishTimeExtractorConfiguration } from "./timeConfiguration";
import { SpanishDateTimeUtilityConfiguration } from "./baseConfiguration";
import { SpanishDateTime } from "../../resources/spanishDateTime";
import { ICommonDateTimeParserConfiguration } from "../parsers";
import { IDateTimeExtractor } from "../baseDateTime";
import { EnglishIntegerExtractor, NumberMode } from "@microsoft/recognizers-text-number";
import { Constants } from "../constants";

export class SpanishTimePeriodExtractorConfiguration implements ITimePeriodExtractorConfiguration {
    readonly simpleCasesRegex: RegExp[];
    readonly tillRegex: RegExp;
    readonly timeOfDayRegex: RegExp;
    readonly generalEndingRegex: RegExp;
    readonly singleTimeExtractor: IDateTimeExtractor;
    readonly integerExtractor: IExtractor;
    readonly utilityConfiguration: SpanishDateTimeUtilityConfiguration;

    readonly fromRegex: RegExp;
    readonly RangeConnectorRegex: RegExp;
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
        this.RangeConnectorRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.RangeConnectorRegex, "gis");
        this.betweenRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.BetweenRegex, "gis");
    }

    getFromTokenIndex(text: string): { matched: boolean; index: number; } {
        return RegExpUtility.getFirstMatchIndex(this.fromRegex, text);
    }

    hasConnectorToken(text: string): boolean {
        return RegExpUtility.getFirstMatchIndex(this.RangeConnectorRegex, text).matched;
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

        let trimmedText = text.trim().toLowerCase();

        let beginHour = 0;
        let endHour = 0;
        let endMin = 0;
        let timex = "";

        let timeOfDay = "";
        if (SpanishDateTime.EarlyMorningTermList.some(o => trimmedText.endsWith(o))) {
            timeOfDay = Constants.EarlyMorning;
        }
        else if (SpanishDateTime.MorningTermList.some(o => trimmedText.endsWith(o))) {
            timeOfDay = Constants.Morning;
        }
        else if (SpanishDateTime.AfternoonTermList.some(o => trimmedText.includes(o))) {
            timeOfDay = Constants.Afternoon;
        }
        else if (SpanishDateTime.EveningTermList.some(o => trimmedText.endsWith(o))) {
            timeOfDay = Constants.Evening;
        }
        else if (SpanishDateTime.NightTermList.some(o => trimmedText.endsWith(o))) {
            timeOfDay = Constants.Night;
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

        let parseResult = TimexUtil.parseTimeOfDay(timeOfDay);
        timex = parseResult.timeX;
        beginHour = parseResult.beginHour;
        endHour = parseResult.endHour;
        endMin = parseResult.endMin;

        return {
            matched: true,
            timex,
            beginHour,
            endHour,
            endMin
        };
    }
}