import { RegExpUtility, IExtractor } from "@microsoft/recognizers-text";
import { ITimePeriodExtractorConfiguration, ITimePeriodParserConfiguration } from "../baseTimePeriod";
import { BaseTimeExtractor, BaseTimeParser } from "../baseTime";
import { IDateTimeUtilityConfiguration } from "../utilities";
import { FrenchTimeExtractorConfiguration } from "./timeConfiguration";
import { FrenchDateTimeUtilityConfiguration } from "./baseConfiguration";
import { FrenchDateTime } from "../../resources/frenchDateTime";
import { ICommonDateTimeParserConfiguration } from "../parsers";
import { IDateTimeExtractor } from "../baseDateTime";
import { EnglishIntegerExtractor, NumberMode } from "@microsoft/recognizers-text-number";

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

        let trimedText = text.trim().toLowerCase();

        if (trimedText.endsWith("s"))
        {
            trimedText = trimedText.substring(0, trimedText.length - 1);
        }

        let beginHour = 0;
        let endHour = 0;
        let endMin = 0;
        let timex = "";

        if (trimedText.endsWith("matinee") || 
            trimedText.endsWith("matin") || 
            trimedText.endsWith("matinée")) {
            timex = "TMO";
            beginHour = 8;
            endHour = 12;
        }
        else if (trimedText.endsWith("apres-midi")||
            trimedText.endsWith("apres midi") || 
            trimedText.endsWith("après midi") || 
            trimedText.endsWith("après-midi")) {
            timex = "TAF";
            beginHour = 12;
            endHour = 16;
        }
        else if (trimedText.endsWith("soir") || 
            trimedText.endsWith("soiree") || 
            trimedText.endsWith("soirée")) {
            timex = "TEV";
            beginHour = 16;
            endHour = 20;
        }
        else if (trimedText === "jour" || 
            trimedText.endsWith("journee") || 
            trimedText.endsWith("journée")) {
            timex = "TDT";
            beginHour = 8;
            endHour = 18;
        }
        else if (trimedText.endsWith("nuit")) {
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