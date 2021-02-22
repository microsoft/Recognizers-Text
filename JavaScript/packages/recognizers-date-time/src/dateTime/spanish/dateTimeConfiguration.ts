import { RegExpUtility } from "@microsoft/recognizers-text";
import { IDateTimeExtractorConfiguration, IDateTimeParserConfiguration, IDateTimeExtractor } from "../baseDateTime";
import { BaseNumberExtractor, BaseNumberParser } from "@microsoft/recognizers-text-number";
import { BaseDateExtractor, BaseDateParser } from "../baseDate";
import { BaseTimeExtractor, BaseTimeParser } from "../baseTime";
import { BaseDurationExtractor, BaseDurationParser } from "../baseDuration";
import { IDateTimeUtilityConfiguration } from "../utilities";
import { SpanishDateExtractorConfiguration } from "./dateConfiguration";
import { SpanishDurationExtractorConfiguration } from "./durationConfiguration";
import { SpanishDateTimeUtilityConfiguration } from "./baseConfiguration";
import { SpanishDateTime } from "../../resources/spanishDateTime";
import { ICommonDateTimeParserConfiguration } from "../parsers";
import { SpanishTimeExtractorConfiguration } from "./timeConfiguration";

export class SpanishDateTimeExtractorConfiguration implements IDateTimeExtractorConfiguration {
    readonly datePointExtractor: IDateTimeExtractor;
    readonly timePointExtractor: IDateTimeExtractor;
    readonly durationExtractor: IDateTimeExtractor;
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


    constructor(dmyDateFormat: boolean) {
        this.prepositionRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.PrepositionRegex, "gis");
        this.nowRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.NowRegex, "gis");
        this.suffixRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.SuffixRegex, "gis");

        this.timeOfDayRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.TimeOfDayRegex, "gis");
        this.specificTimeOfDayRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.SpecificTimeOfDayRegex, "gis");
        this.timeOfTodayAfterRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.TimeOfTodayAfterRegex, "gis");
        this.timeOfTodayBeforeRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.TimeOfTodayBeforeRegex, "gis");
        this.simpleTimeOfTodayAfterRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.SimpleTimeOfTodayAfterRegex, "gis");
        this.simpleTimeOfTodayBeforeRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.SimpleTimeOfTodayBeforeRegex, "gis");
        this.specificEndOfRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.SpecificEndOfRegex, "gis");
        this.unspecificEndOfRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.UnspecificEndOfRegex, "gis");
        this.unitRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.UnitRegex, "gis");
        this.connectorRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.ConnectorRegex, "gis");
        this.nightRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.NightRegex, "gis");

        this.datePointExtractor = new BaseDateExtractor(new SpanishDateExtractorConfiguration(dmyDateFormat));
        this.timePointExtractor = new BaseTimeExtractor(new SpanishTimeExtractorConfiguration());
        this.durationExtractor = new BaseDurationExtractor(new SpanishDurationExtractorConfiguration());
        this.utilityConfiguration = new SpanishDateTimeUtilityConfiguration();
    }

    isConnectorToken(source: string): boolean {
        let trimmed = source.trim();
        return trimmed === ""
            || RegExpUtility.getFirstMatchIndex(this.prepositionRegex, source).matched
            || RegExpUtility.getFirstMatchIndex(this.connectorRegex, source).matched;
    }
}

export class SpanishDateTimeParserConfiguration implements IDateTimeParserConfiguration {
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
    readonly lastNightTimeRegex: RegExp;

    constructor(config: ICommonDateTimeParserConfiguration) {
        this.tokenBeforeDate = SpanishDateTime.TokenBeforeDate;
        this.tokenBeforeTime = SpanishDateTime.TokenBeforeTime;
        this.nowRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.NowRegex, "gis");
        this.amTimeRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.AmTimeRegex, "gis");
        this.pmTimeRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.PmTimeRegex, "gis");
        this.simpleTimeOfTodayAfterRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.SimpleTimeOfTodayAfterRegex, "gis");
        this.simpleTimeOfTodayBeforeRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.SimpleTimeOfTodayBeforeRegex, "gis");
        this.specificTimeOfDayRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.SpecificTimeOfDayRegex, "gis");
        this.specificEndOfRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.SpecificEndOfRegex, "gis");
        this.unspecificEndOfRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.UnspecificEndOfRegex, "gis");
        this.unitRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.UnitRegex, "gis");

        this.nextPrefixRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.NextPrefixRegex, "gis");
        this.previousPrefixRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.PreviousPrefixRegex, "gis");
        this.lastNightTimeRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.LastNightTimeRegex, "gis");

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
        return text.toLowerCase().includes("esta mañana")
            && matchedText.toLocaleLowerCase().includes("mañana");
    }

    getMatchedNowTimex(text: string): { matched: boolean; timex: string; } {
        let trimedText = text.trim().toLowerCase();
        let timex = "";
        if (trimedText.endsWith("ahora") || trimedText.endsWith("mismo") || trimedText.endsWith("momento")) {
            timex = "PRESENT_REF";
        }
        else if (trimedText.endsWith("posible") || trimedText.endsWith("pueda") ||
            trimedText.endsWith("puedas") || trimedText.endsWith("podamos") || trimedText.endsWith("puedan")) {
            timex = "FUTURE_REF";
        }
        else if (trimedText.endsWith("mente")) {
            timex = "PAST_REF";
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

        if (RegExpUtility.getFirstMatchIndex(this.previousPrefixRegex, trimedText).matched ||
            RegExpUtility.getFirstMatchIndex(this.lastNightTimeRegex, trimedText).matched) {
            swift = -1;
        }
        else if (RegExpUtility.getFirstMatchIndex(this.nextPrefixRegex, trimedText).matched) {
            swift = 1;
        }

        return swift;

    }

    getHour(text: string, hour: number): number {
        let trimedText = text.trim().toLowerCase();
        let result = hour;

        // TODO: Replace with a regex
        if ((trimedText.endsWith("mañana") || trimedText.endsWith("madrugada")) && hour >= 12) {
            result -= 12;
        }
        else if (!(trimedText.endsWith("mañana") || trimedText.endsWith("madrugada")) && hour < 12) {
            result += 12;
        }

        return result;
    }
}