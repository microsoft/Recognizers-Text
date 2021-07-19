// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

import { IDateTimeExtractor, IDateTimeExtractorConfiguration, IDateTimeParserConfiguration } from "../baseDateTime";
import { BaseDateExtractor, BaseDateParser } from "../baseDate";
import { BaseTimeExtractor, BaseTimeParser } from "../baseTime";
import { RegExpUtility, StringUtility } from "@microsoft/recognizers-text";
import { BaseNumberExtractor, BaseNumberParser } from "@microsoft/recognizers-text-number";
import { BaseDurationExtractor, BaseDurationParser } from "../baseDuration";
import { EnglishDateTime } from "../../resources/englishDateTime";
import { ICommonDateTimeParserConfiguration, IDateTimeParser } from "../parsers";
import { EnglishDateTimeUtilityConfiguration } from "./baseConfiguration";
import { IDateTimeUtilityConfiguration } from "../utilities";
import { EnglishDurationExtractorConfiguration } from "./durationConfiguration";
import { EnglishDateExtractorConfiguration } from "./dateConfiguration";
import { EnglishTimeExtractorConfiguration } from "./timeConfiguration";

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
    readonly specificEndOfRegex: RegExp
    readonly unspecificEndOfRegex: RegExp
    readonly unitRegex: RegExp
    readonly prepositionRegex: RegExp
    readonly connectorRegex: RegExp
    readonly utilityConfiguration: IDateTimeUtilityConfiguration

    constructor(dmyDateFormat: boolean) {
        this.datePointExtractor = new BaseDateExtractor(new EnglishDateExtractorConfiguration(dmyDateFormat));
        this.timePointExtractor = new BaseTimeExtractor(new EnglishTimeExtractorConfiguration());
        this.durationExtractor = new BaseDurationExtractor(new EnglishDurationExtractorConfiguration());
        this.suffixRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.SuffixRegex);
        this.nowRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.NowRegex);
        this.timeOfTodayAfterRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.TimeOfTodayAfterRegex);
        this.simpleTimeOfTodayAfterRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.SimpleTimeOfTodayAfterRegex);
        this.nightRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.TimeOfDayRegex);
        this.timeOfTodayBeforeRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.TimeOfTodayBeforeRegex);
        this.simpleTimeOfTodayBeforeRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.SimpleTimeOfTodayBeforeRegex);
        this.specificEndOfRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.SpecificEndOfRegex);
        this.unspecificEndOfRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.UnspecificEndOfRegex);
        this.unitRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.TimeUnitRegex);
        this.prepositionRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.PrepositionRegex);
        this.connectorRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.ConnectorRegex);
        this.utilityConfiguration = new EnglishDateTimeUtilityConfiguration();
    }

    isConnectorToken(source: string): boolean {
        return (StringUtility.isNullOrWhitespace(source)
            || RegExpUtility.getMatches(this.connectorRegex, source).length > 0
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
    specificEndOfRegex: RegExp;
    unspecificEndOfRegex: RegExp;
    unitRegex: RegExp;
    unitMap: ReadonlyMap<string, string>;
    numbers: ReadonlyMap<string, number>;
    utilityConfiguration: IDateTimeUtilityConfiguration;
    nowTimeRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.NowTimeRegex);
    recentlyTimeRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.RecentlyTimeRegex);
    asapTimeRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.AsapTimeRegex);

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
        this.specificEndOfRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.SpecificEndOfRegex);
        this.unspecificEndOfRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.UnspecificEndOfRegex);
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
        else if (!trimmedText.endsWith("morning") && hour < 12 && !(trimmedText.endsWith("night") && hour < 6)) {
            result += 12;
        }
        return result;
    }

    public getMatchedNowTimex(text: string): { matched: boolean, timex: string } {
        let trimmedText = text.trim().toLowerCase();
        let timex: string;
        if (RegExpUtility.isMatch(this.nowTimeRegex, trimmedText)) {
            timex = "PRESENT_REF";
        }
        else if (RegExpUtility.isMatch(this.recentlyTimeRegex, trimmedText)) {
            timex = "PAST_REF";
        }
        else if (RegExpUtility.isMatch(this.asapTimeRegex, trimmedText)) {
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

    public haveAmbiguousToken(text: string, matchedText: string): boolean {
        return false;
    }
}
