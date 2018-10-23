import { ITimePeriodExtractorConfiguration, ITimePeriodParserConfiguration } from "../baseTimePeriod"
import { BaseTimeExtractor, BaseTimeParser } from "../baseTime";
import { RegExpUtility, IExtractor } from "@microsoft/recognizers-text";
import { EnglishDateTime } from "../../resources/englishDateTime";
import { ICommonDateTimeParserConfiguration } from "../parsers"
import { IDateTimeUtilityConfiguration } from "../utilities"
import { EnglishTimeExtractorConfiguration } from "./timeConfiguration"
import { IDateTimeExtractor } from "../baseDateTime"
import { EnglishIntegerExtractor } from "@microsoft/recognizers-text-number";
import { Constants } from "../constants";

export class EnglishTimePeriodExtractorConfiguration implements ITimePeriodExtractorConfiguration {
    readonly simpleCasesRegex: RegExp[];
    readonly tillRegex: RegExp;
    readonly timeOfDayRegex: RegExp;
    readonly generalEndingRegex: RegExp;
    readonly singleTimeExtractor: IDateTimeExtractor;
    readonly integerExtractor: IExtractor;

    constructor() {
        this.simpleCasesRegex = [
            RegExpUtility.getSafeRegExp(EnglishDateTime.PureNumFromTo, "gis"),
            RegExpUtility.getSafeRegExp(EnglishDateTime.PureNumBetweenAnd, "gis")
        ];
        this.tillRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.TillRegex, "gis");
        this.timeOfDayRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.TimeOfDayRegex, "gis");
        this.generalEndingRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.GeneralEndingRegex, "gis");
        this.singleTimeExtractor = new BaseTimeExtractor(new EnglishTimeExtractorConfiguration());
        this.integerExtractor = new EnglishIntegerExtractor();
    }

    public getFromTokenIndex(source: string): { matched: boolean, index: number } {
        let index = -1;
        if (source.endsWith("from")) {
            index = source.lastIndexOf("from");
            return { matched: true, index: index };
        }
        return { matched: false, index: index };
    }

    public getBetweenTokenIndex(source: string): { matched: boolean, index: number } {
        let index = -1;
        if (source.endsWith("between")) {
            index = source.lastIndexOf("between");
            return { matched: true, index: index };
        }
        return { matched: false, index: index };
    }

    public hasConnectorToken(source: string): boolean {
        return source === "and";
    }
}

export class EnglishTimePeriodParserConfiguration implements ITimePeriodParserConfiguration {
    timeExtractor: IDateTimeExtractor;
    timeParser: BaseTimeParser;
    integerExtractor: IDateTimeExtractor;
    pureNumberFromToRegex: RegExp;
    pureNumberBetweenAndRegex: RegExp;
    timeOfDayRegex: RegExp;
    tillRegex: RegExp;
    numbers: ReadonlyMap<string, number>;
    utilityConfiguration: IDateTimeUtilityConfiguration;
    specificTimeFromToRegex: RegExp;
    specificTimeBetweenAndRegex: RegExp;

    constructor(config: ICommonDateTimeParserConfiguration) {
        this.timeExtractor = config.timeExtractor;
        this.timeParser = config.timeParser;
        this.integerExtractor = config.integerExtractor;
        this.pureNumberFromToRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.PureNumFromTo);
        this.pureNumberBetweenAndRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.PureNumBetweenAnd);
        this.timeOfDayRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.TimeOfDayRegex);
        this.tillRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.TillRegex, "gis");
        this.numbers = config.numbers;
        this.utilityConfiguration = config.utilityConfiguration;
        this.specificTimeFromToRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.SpecificTimeFromTo);
        this.specificTimeBetweenAndRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.SpecificTimeBetweenAnd);
    }

    getMatchedTimexRange(text: string): {
        matched: boolean, timex: string, beginHour: number, endHour: number, endMin: number
    } {
        let trimmedText = text.trim().toLowerCase();
        if (trimmedText.endsWith("s")) {
            trimmedText = trimmedText.substring(0, trimmedText.length - 1);
        }
        let result = {
            matched: false,
            timex: '',
            beginHour: 0,
            endHour: 0,
            endMin: 0
        };
        if (trimmedText.endsWith(Constants.EN_MORNING)) {
            result.timex = EnglishDateTime.TimeOfDayTimex[Constants.EN_MORNING];
            result.beginHour = EnglishDateTime.TimeOfDayBeginHour[Constants.EN_MORNING];
            result.endHour = EnglishDateTime.TimeOfDayEndHour[Constants.EN_MORNING];
        }
        else if (trimmedText.endsWith(Constants.EN_AFTERNOON)) {
            result.timex = EnglishDateTime.TimeOfDayTimex[Constants.EN_AFTERNOON];
            result.beginHour = EnglishDateTime.TimeOfDayBeginHour[Constants.EN_AFTERNOON];
            result.endHour = EnglishDateTime.TimeOfDayEndHour[Constants.EN_AFTERNOON];
        }
        else if (trimmedText.endsWith(Constants.EN_EVENING)) {
            result.timex = EnglishDateTime.TimeOfDayTimex[Constants.EN_EVENING];
            result.beginHour = EnglishDateTime.TimeOfDayBeginHour[Constants.EN_EVENING];
            result.endHour = EnglishDateTime.TimeOfDayEndHour[Constants.EN_EVENING];
        }
        else if (trimmedText === Constants.EN_DAYTIME) {
            result.timex = EnglishDateTime.TimeOfDayTimex[Constants.EN_DAYTIME];
            result.beginHour = EnglishDateTime.TimeOfDayBeginHour[Constants.EN_DAYTIME];
            result.endHour = EnglishDateTime.TimeOfDayEndHour[Constants.EN_DAYTIME];
        }
        else if (trimmedText.endsWith(Constants.EN_NIGHT)) {
            result.timex = EnglishDateTime.TimeOfDayTimex[Constants.EN_NIGHT];
            result.beginHour = EnglishDateTime.TimeOfDayBeginHour[Constants.EN_NIGHT];
            result.endHour = EnglishDateTime.TimeOfDayEndHour[Constants.EN_NIGHT];
            result.endMin = EnglishDateTime.TimeOfDayEndMin[Constants.EN_NIGHT];
        }
        else {
            result.timex = null;
            result.matched = false;
            return result;
        }

        result.matched = true;
        return result;
    }
}
