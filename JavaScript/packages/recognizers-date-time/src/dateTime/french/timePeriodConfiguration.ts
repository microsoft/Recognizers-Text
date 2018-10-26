import { RegExpUtility, IExtractor } from "@microsoft/recognizers-text";
import { ITimePeriodExtractorConfiguration, ITimePeriodParserConfiguration } from "../baseTimePeriod";
import { BaseTimeExtractor, BaseTimeParser } from "../baseTime";
import { IDateTimeUtilityConfiguration, TimexUtil } from "../utilities";
import { FrenchTimeExtractorConfiguration } from "./timeConfiguration";
import { FrenchDateTimeUtilityConfiguration } from "./baseConfiguration";
import { FrenchDateTime } from "../../resources/frenchDateTime";
import { ICommonDateTimeParserConfiguration } from "../parsers";
import { IDateTimeExtractor } from "../baseDateTime";
import { EnglishIntegerExtractor, NumberMode } from "@microsoft/recognizers-text-number";
import { Constants } from "../constants";
import { EnglishDateTime } from "../../resources/englishDateTime";

export class FrenchTimePeriodExtractorConfiguration implements ITimePeriodExtractorConfiguration {
    readonly simpleCasesRegex: RegExp[];
    readonly tillRegex: RegExp;
    readonly timeOfDayRegex: RegExp;
    readonly generalEndingRegex: RegExp;
    readonly singleTimeExtractor: IDateTimeExtractor;
    readonly integerExtractor: IExtractor;
    readonly utilityConfiguration: FrenchDateTimeUtilityConfiguration;

    readonly fromRegex: RegExp;
    readonly connectorAndRegex: RegExp;
    readonly beforeRegex: RegExp;

    constructor() {
        this.singleTimeExtractor = new BaseTimeExtractor(new FrenchTimeExtractorConfiguration());
        this.integerExtractor = new EnglishIntegerExtractor();
        this.utilityConfiguration = new FrenchDateTimeUtilityConfiguration();

        this.simpleCasesRegex = [
            RegExpUtility.getSafeRegExp(FrenchDateTime.PureNumFromTo, "gis"),
            RegExpUtility.getSafeRegExp(FrenchDateTime.PureNumBetweenAnd, "gis"),
            RegExpUtility.getSafeRegExp(FrenchDateTime.PmRegex, "gis"),
            RegExpUtility.getSafeRegExp(FrenchDateTime.AmRegex, "gis")
        ];

        this.tillRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.TillRegex, "gis");
        this.timeOfDayRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.TimeOfDayRegex, "gis");
        this.generalEndingRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.GeneralEndingRegex, "gis");

        this.fromRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.FromRegex2, "gis");
        this.connectorAndRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.ConnectorAndRegex, "gis");
        this.beforeRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.BeforeRegex2, "gis");
    }

    getFromTokenIndex(text: string): { matched: boolean; index: number; } {
        return RegExpUtility.getFirstMatchIndex(this.fromRegex, text);
    }

    hasConnectorToken(text: string): boolean {
        return RegExpUtility.getFirstMatchIndex(this.connectorAndRegex, text).matched;
    }

    getBetweenTokenIndex(text: string): { matched: boolean; index: number; } {
        return RegExpUtility.getFirstMatchIndex(this.beforeRegex, text);
    }
}

export class FrenchTimePeriodParserConfiguration implements ITimePeriodParserConfiguration {
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
        this.pureNumberFromToRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.PureNumFromTo, "gis");
        this.pureNumberBetweenAndRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.PureNumBetweenAnd, "gis");
        this.timeOfDayRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.TimeOfDayRegex, "gis");
        this.tillRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.TillRegex, "gis");
        this.specificTimeFromToRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.SpecificTimeFromTo);
        this.specificTimeBetweenAndRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.SpecificTimeBetweenAnd);
    }

    getMatchedTimexRange(text: string): { matched: boolean; timex: string; beginHour: number; endHour: number; endMin: number; } {

        let trimmedText = text.trim().toLowerCase();

        if (trimmedText.endsWith("s"))
        {
            trimmedText = trimmedText.substring(0, trimmedText.length - 1);
        }

        let beginHour = 0;
        let endHour = 0;
        let endMin = 0;
        let timex = "";

        let timeOfDay = "";
        if (FrenchDateTime.MorningTermList.some(o => trimmedText.endsWith(o))) {
            timeOfDay = Constants.Morning;
        } else if (FrenchDateTime.AfternoonTermList.some(o => trimmedText.endsWith(o))) {
            timeOfDay = Constants.Afternoon;
        } else if (FrenchDateTime.EveningTermList.some(o => trimmedText.endsWith(o))) {
            timeOfDay = Constants.Evening;
        }
        else if (trimmedText === FrenchDateTime.DaytimeTermList[0] ||
            trimmedText.endsWith(FrenchDateTime.DaytimeTermList[1]) ||
            trimmedText.endsWith(FrenchDateTime.DaytimeTermList[2])) {
            timeOfDay = Constants.Daytime;
        }
        else if (FrenchDateTime.NightTermList.some(o => trimmedText.endsWith(o)))
        {
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