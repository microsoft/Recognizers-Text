import { RegExpUtility } from "@microsoft/recognizers-text";
import { IDateTimeExtractorConfiguration, IDateTimeParserConfiguration, IDateTimeExtractor } from "../baseDateTime";
import { BaseNumberExtractor, BaseNumberParser } from "@microsoft/recognizers-text-number";
import { BaseDateExtractor, BaseDateParser } from "../baseDate";
import { BaseTimeExtractor, BaseTimeParser } from "../baseTime";
import { BaseDurationExtractor, BaseDurationParser } from "../baseDuration";
import { IDateTimeUtilityConfiguration } from "../utilities";
import { FrenchDateExtractorConfiguration } from "./dateConfiguration";
import { FrenchDurationExtractorConfiguration } from "./durationConfiguration";
import { FrenchDateTimeUtilityConfiguration } from "./baseConfiguration";
import { FrenchDateTime } from "../../resources/frenchDateTime";
import { ICommonDateTimeParserConfiguration } from "../parsers";
import { FrenchTimeExtractorConfiguration } from "./timeConfiguration";

export class FrenchDateTimeExtractorConfiguration implements IDateTimeExtractorConfiguration {
    readonly datePointExtractor: BaseDateExtractor;
    readonly timePointExtractor: BaseTimeExtractor;
    readonly durationExtractor: BaseDurationExtractor;
    readonly suffixRegex: RegExp;
    readonly nowRegex: RegExp;
    readonly timeOfTodayAfterRegex: RegExp;
    readonly timeOfDayRegex: RegExp;
    readonly specificTimeOfDayRegex: RegExp;
    readonly simpleTimeOfTodayAfterRegex: RegExp;
    readonly nightRegex: RegExp;
    readonly timeOfTodayBeforeRegex: RegExp;
    readonly simpleTimeOfTodayBeforeRegex: RegExp;
    readonly specificEndOfRegex: RegExp;
    readonly unspecificEndOfRegex: RegExp;
    readonly unitRegex: RegExp;
    readonly utilityConfiguration: IDateTimeUtilityConfiguration;
    readonly prepositionRegex: RegExp;
    readonly connectorRegex: RegExp;


    constructor() {
        this.prepositionRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.PrepositionRegex, "gis");
        this.nowRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.NowRegex, "gis");
        this.suffixRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.SuffixRegex, "gis");

        this.timeOfDayRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.TimeOfDayRegex, "gis");
        this.specificTimeOfDayRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.SpecificTimeOfDayRegex, "gis");
        this.timeOfTodayAfterRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.TimeOfTodayAfterRegex, "gis");
        this.timeOfTodayBeforeRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.TimeOfTodayBeforeRegex, "gis");
        this.simpleTimeOfTodayAfterRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.SimpleTimeOfTodayAfterRegex, "gis");
        this.simpleTimeOfTodayBeforeRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.SimpleTimeOfTodayBeforeRegex, "gis");
        this.specificEndOfRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.SpecificEndOfRegex, "gis");
        this.unspecificEndOfRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.UnspecificEndOfRegex, "gis");
        this.unitRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.TimeUnitRegex, "gis");
        this.connectorRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.ConnectorRegex, "gis");
        this.nightRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.NightRegex, "gis");

        this.datePointExtractor = new BaseDateExtractor(new FrenchDateExtractorConfiguration());
        this.timePointExtractor = new BaseTimeExtractor(new FrenchTimeExtractorConfiguration());
        this.durationExtractor = new BaseDurationExtractor(new FrenchDurationExtractorConfiguration());
        this.utilityConfiguration = new FrenchDateTimeUtilityConfiguration();
    }

    isConnectorToken(source: string): boolean {
        
        return (source === "" ||
            RegExpUtility.getFirstMatchIndex(this.prepositionRegex, source).matched ||
            RegExpUtility.getFirstMatchIndex(this.connectorRegex, source).matched);
    }
}

export class FrenchDateTimeParserConfiguration implements IDateTimeParserConfiguration {
    readonly tokenBeforeDate: string;
    readonly tokenBeforeTime: string;
    readonly dateExtractor: IDateTimeExtractor;
    readonly timeExtractor: IDateTimeExtractor;
    readonly dateParser: BaseDateParser;
    readonly timeParser: BaseTimeParser;
    readonly cardinalExtractor: BaseNumberExtractor;
    readonly numberParser: BaseNumberParser;
    readonly durationExtractor: IDateTimeExtractor;
    readonly durationParser: BaseDurationParser;
    readonly nowRegex: RegExp;
    readonly amTimeRegex: RegExp;
    readonly pmTimeRegex: RegExp;
    readonly simpleTimeOfTodayAfterRegex: RegExp;
    readonly simpleTimeOfTodayBeforeRegex: RegExp;
    readonly specificTimeOfDayRegex: RegExp;
    readonly specificEndOfRegex: RegExp;
    readonly unspecificEndOfRegex: RegExp;
    readonly unitRegex: RegExp;
    readonly unitMap: ReadonlyMap<string, string>;
    readonly numbers: ReadonlyMap<string, number>;
    readonly utilityConfiguration: IDateTimeUtilityConfiguration;

    readonly nextPrefixRegex: RegExp;
    readonly previousPrefixRegex: RegExp;

    constructor(config: ICommonDateTimeParserConfiguration) {
        this.tokenBeforeDate = FrenchDateTime.TokenBeforeDate;
        this.tokenBeforeTime = FrenchDateTime.TokenBeforeTime;
        this.nowRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.NowRegex, "gis");
        this.amTimeRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.AMTimeRegex, "gis");
        this.pmTimeRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.PMTimeRegex, "gis");
        this.simpleTimeOfTodayAfterRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.SimpleTimeOfTodayAfterRegex, "gis");
        this.simpleTimeOfTodayBeforeRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.SimpleTimeOfTodayBeforeRegex, "gis");
        this.specificTimeOfDayRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.SpecificTimeOfDayRegex, "gis");
        this.specificEndOfRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.SpecificEndOfRegex, "gis");
        this.unspecificEndOfRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.UnspecificEndOfRegex, "gis");
        this.unitRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.TimeUnitRegex, "gis");

        this.dateExtractor = config.dateExtractor;
        this.timeExtractor = config.timeExtractor;
        this.dateParser = config.dateParser;
        this.timeParser = config.timeParser;
        this.numbers = config.numbers;
        this.cardinalExtractor = config.cardinalExtractor;
        this.numberParser = config.numberParser;
        this.durationExtractor = config.durationExtractor;
        this.durationParser = config.durationParser;
        this.unitMap = config.unitMap;
        this.utilityConfiguration = config.utilityConfiguration;
    }

    haveAmbiguousToken(text: string, matchedText: string): boolean {
        return false;
    }

    getMatchedNowTimex(text: string): { matched: boolean; timex: string; } {
        let trimedText = text.trim().toLowerCase();
        let timex = "";
        if (trimedText.endsWith("maintenant")) {
            timex = "PRESENT_REF";
        }
        else if (trimedText === "récemment" || 
            trimedText === "précédemment" ||
            trimedText === "auparavant") {
            timex = "PAST_REF";
        }
        else if (trimedText === "dès que possible" || 
            trimedText === "dqp") {
            timex = "FUTURE_REF";
        }
        else {
            return {
                matched: false,
                timex: null
            };
        }

        return {
            matched: true,
            timex: timex
        };
    }

    getSwiftDay(text: string): number {
        let trimedText = text.trim().toLowerCase();
        let swift = 0;

        if (trimedText.startsWith("prochain") || 
            trimedText.endsWith("prochain") ||
            trimedText.startsWith("prochaine") || 
            trimedText.endsWith("prochaine")) {
            swift = 1;
        }
        else if (trimedText.startsWith("dernier") || 
            trimedText.startsWith("dernière") ||
            trimedText.endsWith("dernier") || 
            trimedText.endsWith("dernière")) {
            swift = -1;
        }

        return swift;

    }

    getHour(text: string, hour: number): number {
        let trimedText = text.trim().toLowerCase();
        let result = hour;

        // TODO: Replace with a regex
        if (trimedText.endsWith("matin") && hour >= 12) {
            result -= 12;
        }
        else if (!trimedText.endsWith("matin") && hour < 12) {
            result += 12;
        }

        return result;
    }
}