import { IDateTimeExtractor, IDateTimeExtractorConfiguration, IDateTimeParserConfiguration } from "../baseDateTime"
import { BaseDateExtractor, BaseDateParser } from "../baseDate";
import { BaseTimeExtractor, BaseTimeParser } from "../baseTime";
import { BaseNumberExtractor,RegExpUtility, StringUtility, BaseNumberParser } from "recognizers-text-number"
import { BaseDurationExtractor, BaseDurationParser } from "../baseDuration"
import { EnglishDateTime } from "../../resources/englishDateTime";
import { ICommonDateTimeParserConfiguration, IDateTimeParser } from "../parsers"
import { EnglishDateTimeUtilityConfiguration } from "./baseConfiguration"
import { IDateTimeUtilityConfiguration } from "../utilities";
import { EnglishDurationExtractorConfiguration } from "./durationConfiguration"
import { EnglishDateExtractorConfiguration } from "./dateConfiguration"
import { EnglishTimeExtractorConfiguration } from "./timeConfiguration"

export class EnglishDateTimeExtractorConfiguration implements IDateTimeExtractorConfiguration {
    readonly datePointExtractor: IDateTimeExtractor
    readonly timePointExtractor: IDateTimeExtractor
    readonly durationExtractor: IDateTimeExtractor
    readonly suffixRegex: RegExp
    readonly nowRegex: RegExp
    readonly timeOfTodayAfterRegex: RegExp
    readonly simpleTimeOfTodayAfterRegex: RegExp
    readonly nightRegex: RegExp
    readonly timeOfTodayBeforeRegex: RegExp
    readonly simpleTimeOfTodayBeforeRegex: RegExp
    readonly theEndOfRegex: RegExp
    readonly unitRegex: RegExp
    readonly prepositionRegex: RegExp
    readonly utilityConfiguration: IDateTimeUtilityConfiguration

    constructor() {
        this.datePointExtractor = new BaseDateExtractor(new EnglishDateExtractorConfiguration());
        this.timePointExtractor = new BaseTimeExtractor(new EnglishTimeExtractorConfiguration());
        this.durationExtractor = new BaseDurationExtractor(new EnglishDurationExtractorConfiguration());
        this.suffixRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.SuffixRegex, "gis");
        this.nowRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.NowRegex, "gis");
        this.timeOfTodayAfterRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.TimeOfTodayAfterRegex, "gis");
        this.simpleTimeOfTodayAfterRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.SimpleTimeOfTodayAfterRegex, "gis");
        this.nightRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.TimeOfDayRegex, "gis");
        this.timeOfTodayBeforeRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.TimeOfTodayBeforeRegex, "gis");
        this.simpleTimeOfTodayBeforeRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.SimpleTimeOfTodayBeforeRegex, "gis");
        this.theEndOfRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.TheEndOfRegex, "gis");
        this.unitRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.TimeUnitRegex, "gis");
        this.prepositionRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.PrepositionRegex, "gis");
        this.utilityConfiguration = new EnglishDateTimeUtilityConfiguration();
    }
            
    isConnectorToken(source: string): boolean {
        return (StringUtility.isNullOrWhitespace(source)
                    || source === ","
                    || source === "t"
                    || source === "for"
                    || source === "around"
                    || RegExpUtility.getMatches(this.prepositionRegex, source).length > 0);
            }
}
  

export class EnglishDateTimeParserConfiguration implements IDateTimeParserConfiguration {
    tokenBeforeDate: string;
    tokenBeforeTime: string;
    dateExtractor: IDateTimeExtractor;
    timeExtractor: IDateTimeExtractor;
    dateParser: BaseDateParser;
    timeParser: BaseTimeParser;
    cardinalExtractor: BaseNumberExtractor;
    numberParser: BaseNumberParser;
    durationExtractor: IDateTimeExtractor;
    durationParser: IDateTimeParser;
    nowRegex: RegExp;
    amTimeRegex: RegExp;
    pmTimeRegex: RegExp;
    simpleTimeOfTodayAfterRegex: RegExp;
    simpleTimeOfTodayBeforeRegex: RegExp;
    specificTimeOfDayRegex: RegExp;
    theEndOfRegex: RegExp;
    unitRegex: RegExp;
    unitMap: ReadonlyMap<string, string>;
    numbers: ReadonlyMap<string, number>;
    utilityConfiguration: IDateTimeUtilityConfiguration;

    constructor(config: ICommonDateTimeParserConfiguration) {
        this.tokenBeforeDate = EnglishDateTime.TokenBeforeDate;
        this.tokenBeforeTime = EnglishDateTime.TokenBeforeTime;
        this.dateExtractor = config.dateExtractor;
        this.timeExtractor = config.timeExtractor;
        this.dateParser = config.dateParser;
        this.timeParser = config.timeParser;
        this.nowRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.NowRegex);
        this.amTimeRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.AMTimeRegex);
        this.pmTimeRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.PMTimeRegex);
        this.simpleTimeOfTodayAfterRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.SimpleTimeOfTodayAfterRegex);
        this.simpleTimeOfTodayBeforeRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.SimpleTimeOfTodayBeforeRegex);
        this.specificTimeOfDayRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.SpecificTimeOfDayRegex);
        this.theEndOfRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.TheEndOfRegex);
        this.unitRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.TimeUnitRegex);
        this.numbers = config.numbers;
        this.cardinalExtractor = config.cardinalExtractor;
        this.numberParser = config.numberParser;
        this.durationExtractor = config.durationExtractor;
        this.durationParser = config.durationParser;
        this.unitMap = config.unitMap;
        this.utilityConfiguration = config.utilityConfiguration;
    }

    public getHour(text: string, hour: number): number {
        let trimmedText = text.trim().toLowerCase();
        let result = hour;
        if (trimmedText.endsWith("morning") && hour >= 12) {
            result -= 12;
        }
        else if (!trimmedText.endsWith("morning") && hour < 12) {
            result += 12;
        }
        return result;
    }

    public getMatchedNowTimex(text: string): { matched: boolean, timex: string } {
        let trimmedText = text.trim().toLowerCase();
        let timex: string;
        if (trimmedText.endsWith("now")) {
            timex = "PRESENT_REF";
        }
        else if (trimmedText === "recently" || trimmedText === "previously") {
            timex = "PAST_REF";
        }
        else if (trimmedText === "as soon as possible" || trimmedText === "asap") {
            timex = "FUTURE_REF";
        }
        else {
            timex = null;
            return { matched: false, timex: timex };
        }
        return { matched: true, timex: timex };
    }

    public getSwiftDay(text: string): number {
        let trimmedText = text.trim().toLowerCase();
        let swift = 0;
        if (trimmedText.startsWith("next")) {
            swift = 1;
        }
        else if (trimmedText.startsWith("last")) {
            swift = -1;
        }
        return swift;
    }

    public haveAmbiguousToken(text: string, matchedText: string): boolean { return false; }
}
