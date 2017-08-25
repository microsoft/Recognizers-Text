import {
    BaseDateExtractor,
    BaseTimeExtractor,
    BaseDateTimeExtractor,
    BaseDurationExtractor,
    BaseDatePeriodExtractor,
    BaseTimePeriodExtractor,
    BaseDateTimePeriodExtractor
} from "../extractors";
import {
    EnglishTimeExtractorConfiguration,
    EnglishDateExtractorConfiguration,
    EnglishDateTimeExtractorConfiguration,
    EnglishDurationExtractorConfiguration,
    EnglishDatePeriodExtractorConfiguration,
    EnglishTimePeriodExtractorConfiguration,
    EnglishDateTimePeriodExtractorConfiguration
} from "./extractorConfiguration";
import {
    BaseDateParserConfiguration,
    ITimeParserConfiguration,
    ICommonDateTimeParserConfiguration,
    IDateParserConfiguration,
    BaseTimeParser
} from "../parsers";
import {
    EnglishCardinalExtractor,
    EnglishIntegerExtractor,
    EnglishOrdinalExtractor
} from "../../number/english/extractors";
import { BaseNumberParser, IParser } from "../../number/parsers";
import { IExtractor } from "../../number/extractors";
import { EnglishNumberParserConfiguration } from "../../number/english/parserConfiguration";
import { CultureInfo, Culture } from "../../culture";
import { EnglishNumeric } from "../../resources/englishNumeric";
import { EnglishDateTime } from "../../resources/englishDateTime"
import { BaseDateTime } from "../../resources/baseDateTime"
import { RegExpUtility } from "../../utilities";
import { FormatUtil, DateTimeResolutionResult, IDateTimeUtilityConfiguration } from "../utilities"
import * as XRegExp from 'xregexp';

export class EnglishCommonDateTimeParserConfiguration extends BaseDateParserConfiguration {
    constructor() {
        super();
        this.utilityConfiguration = {
            agoRegex: RegExpUtility.getSafeRegExp(EnglishDateTime.AgoRegex, "gis"),
            laterRegex: RegExpUtility.getSafeRegExp(EnglishDateTime.LaterRegex, "gis"),
            inConnectorRegex: RegExpUtility.getSafeRegExp(EnglishDateTime.InConnectorRegex, "gis"),
        };
        this.unitMap = EnglishDateTime.UnitMap;
        this.unitValueMap = EnglishDateTime.UnitValueMap;
        this.seasonMap = EnglishDateTime.SeasonMap;
        this.cardinalMap = EnglishDateTime.CardinalMap;
        this.dayOfWeek = EnglishDateTime.DayOfWeek;
        this.monthOfYear = EnglishDateTime.MonthOfYear;
        this.numbers = EnglishDateTime.Numbers;
        this.doubleNumbers = EnglishDateTime.DoubleNumbers;
        this.cardinalExtractor = new EnglishCardinalExtractor();
        this.integerExtractor = new EnglishIntegerExtractor();
        this.ordinalExtractor = new EnglishOrdinalExtractor();
        this.numberParser = new BaseNumberParser(new EnglishNumberParserConfiguration());
        this.dateExtractor = new BaseDateExtractor(new EnglishDateExtractorConfiguration());
        this.timeExtractor = new BaseTimeExtractor(new EnglishTimeExtractorConfiguration());
        this.dateTimeExtractor = new BaseDateTimeExtractor(new EnglishDateTimeExtractorConfiguration());
        this.durationExtractor = new BaseDurationExtractor(new EnglishDurationExtractorConfiguration());
        this.datePeriodExtractor = new BaseDatePeriodExtractor(new EnglishDatePeriodExtractorConfiguration());
        this.timePeriodExtractor = new BaseTimePeriodExtractor(new EnglishTimePeriodExtractorConfiguration());
        this.dateTimePeriodExtractor = new BaseDateTimePeriodExtractor(new EnglishDateTimePeriodExtractorConfiguration());
        // this.dateParser = new BaseDateParser(new EnglishDateParserConfiguration(this));
        // this.timeParser = new TimeParser(new EnglishTimeParserConfiguration(this));
        // this.dateTimeParser = new BaseDateTimeParser(new EnglishDateTimeParserConfiguration(this));
        // this.durationParser = new BaseDurationParser(new EnglishDurationParserConfiguration(this));
        // this.datePeriodParser = new BaseDatePeriodParser(new EnglishDatePeriodParserConfiguration(this));
        // this.timePeriodParser = new BaseTimePeriodParser(new EnglishTimePeriodParserConfiguration(this));
        // this.dateTimePeriodParser = new BaseDateTimePeriodParser(new EnglishDateTimePeriodParserConfiguration(this));
        this.dayOfMonth = new Map<string, number>([...BaseDateTime.DayOfMonthDictionary, ...EnglishDateTime.DayOfMonth]);
    }
}

export class EnglishTimeParserConfiguration implements ITimeParserConfiguration {
    readonly timeTokenPrefix: string;
    readonly atRegex: RegExp
    readonly timeRegexes: RegExp[];
    readonly numbers: ReadonlyMap<string, number>;

    constructor(config: ICommonDateTimeParserConfiguration) {
        this.timeTokenPrefix = EnglishDateTime.TimeTokenPrefix;
        this.atRegex = EnglishTimeExtractorConfiguration.AtRegex;
        this.timeRegexes = EnglishTimeExtractorConfiguration.TimeRegexList;
        this.numbers = config.numbers;
    }

    public adjustByPrefix(prefix: string, adjust: { hour: number, min: number, hasMin: boolean }) {
        let deltaMin = 0;
        let trimedPrefix = prefix.trim().toLowerCase();

        if (trimedPrefix.startsWith("half")) {
            deltaMin = 30;
        }
        else if (trimedPrefix.startsWith("a quarter") || trimedPrefix.startsWith("quarter")) {
            deltaMin = 15;
        }
        else if (trimedPrefix.startsWith("three quarter")) {
            deltaMin = 45;
        }
        else {
            let match = RegExpUtility.getMatches(EnglishTimeExtractorConfiguration.LessThanOneHour, trimedPrefix);
            let minStr = match[0].groups["deltamin"];
            if (minStr) {
                deltaMin = parseInt(minStr);
            }
            else {
                minStr = match[0].groups["deltaminnum"].toLowerCase();
                deltaMin = this.numbers.get(minStr);
            }
        }

        if (trimedPrefix.endsWith("to")) {
            deltaMin = -deltaMin;
        }

        adjust.min += deltaMin;
        if (adjust.min < 0) {
            adjust.min += 60;
            adjust.hour -= 1;
        }
        adjust.hasMin = true;
    }

    public adjustBySuffix(suffix: string, adjust: { hour: number, min: number, hasMin: boolean, hasAm: boolean, hasPm: boolean }) {
        let trimedSuffix = suffix.trim().toLowerCase();
        let deltaHour = 0;
        let matches = RegExpUtility.getMatches(EnglishTimeExtractorConfiguration.TimeSuffix, trimedSuffix);
        if (matches.length > 0 && matches[0].index == 0 && matches[0].length == trimedSuffix.length) {
            let oclockStr = matches[0].groups["oclock"];
            if (!(oclockStr)) {
                let amStr = matches[0].groups["am"];
                if (amStr) {
                    if (adjust.hour >= 12) {
                        deltaHour = -12;
                    }
                    adjust.hasAm = true;
                }

                let pmStr = matches[0].groups["pm"];
                if (pmStr) {
                    if (adjust.hour < 12) {
                        deltaHour = 12;
                    }
                    adjust.hasPm = true;
                }
            }
        }

        adjust.hour = (adjust.hour + deltaHour) % 24;
    }
}

export class EnglishTimeParser extends BaseTimeParser {
    constructor(configuration: ITimeParserConfiguration) {
        super(configuration);
    }

    internalParse(text: string, referenceTime: Date): DateTimeResolutionResult {
        let innerResult = super.internalParse(text, referenceTime);
        if (!innerResult.success) {
            innerResult = this.parseIsh(text, referenceTime);
        }
        return innerResult;
    }

    // parse "noonish", "11-ish"
    private parseIsh(text: string, referenceTime: Date): DateTimeResolutionResult {
        let ret = new DateTimeResolutionResult();
        let trimedText = text.toLowerCase().trim();

        var matches = RegExpUtility.getMatches(EnglishTimeExtractorConfiguration.IshRegex, trimedText);
        if (matches.length > 0 && matches[0].length == trimedText.length) {
            var hourStr = matches[0].groups["hour"];
            var hour = 12;
            if (hourStr) {
                hour = parseInt(hourStr);
            }

            ret.timex = "T" + FormatUtil.toString(hour, 2);
            ret.futureValue =
                ret.pastValue =
                new Date(referenceTime.getFullYear(), referenceTime.getMonth(), referenceTime.getDay(), hour, 0, 0);
            ret.success = true;
            return ret;
        }

        return ret;
    }
}

export class EnglishDateParserConfiguration implements IDateParserConfiguration {
    readonly ordinalExtractor: IExtractor
    readonly integerExtractor: IExtractor
    readonly cardinalExtractor: IExtractor
    readonly durationExtractor: IExtractor
    readonly numberParser: IParser
    readonly monthOfYear: ReadonlyMap<string, number>
    readonly dayOfMonth: ReadonlyMap<string, number>
    readonly dayOfWeek: ReadonlyMap<string, number>
    readonly unitMap: ReadonlyMap<string, string>
    readonly cardinalMap: ReadonlyMap<string, number>
    readonly dateRegex: RegExp[]
    readonly onRegex: RegExp
    readonly specialDayRegex: RegExp
    readonly nextRegex: RegExp
    readonly unitRegex: RegExp
    readonly monthRegex: RegExp
    readonly strictWeekDay: RegExp
    readonly lastRegex: RegExp
    readonly thisRegex: RegExp
    readonly weekDayOfMonthRegex: RegExp
    readonly utilityConfiguration: IDateTimeUtilityConfiguration
    readonly dateTokenPrefix: string

    constructor(config: ICommonDateTimeParserConfiguration) {
        this.ordinalExtractor = config.ordinalExtractor;
        this.integerExtractor = config.integerExtractor;
        this.cardinalExtractor = config.cardinalExtractor;
        this.durationExtractor = config.durationExtractor;
        this.numberParser = config.numberParser;
        this.monthOfYear = config.monthOfYear;
        this.dayOfMonth = config.dayOfMonth;
        this.dayOfWeek = config.dayOfWeek;
        this.unitMap = config.unitMap;
        this.cardinalMap = config.cardinalMap;
        this.dateRegex = [
            RegExpUtility.getSafeRegExp(EnglishDateTime.DateExtractor1, "gis"),
            RegExpUtility.getSafeRegExp(EnglishDateTime.DateExtractor2, "gis"),
            RegExpUtility.getSafeRegExp(EnglishDateTime.DateExtractor3, "gis"),
            RegExpUtility.getSafeRegExp(EnglishDateTime.DateExtractor4, "gis"),
            RegExpUtility.getSafeRegExp(EnglishDateTime.DateExtractor5, "gis"),
            RegExpUtility.getSafeRegExp(EnglishDateTime.DateExtractor6, "gis"),
            RegExpUtility.getSafeRegExp(EnglishDateTime.DateExtractor7, "gis"),
            RegExpUtility.getSafeRegExp(EnglishDateTime.DateExtractor8, "gis"),
            RegExpUtility.getSafeRegExp(EnglishDateTime.DateExtractor9, "gis"),
            RegExpUtility.getSafeRegExp(EnglishDateTime.DateExtractorA, "gis"),
        ];
        this.onRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.OnRegex, "gis");
        this.specialDayRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.SpecialDayRegex, "gis");
        this.nextRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.NextRegex, "gis");
        this.unitRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.UnitRegex, "gis");
        this.monthRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.MonthRegex, "gis");
        this.strictWeekDay = RegExpUtility.getSafeRegExp(EnglishDateTime.StrictWeekDay, "gis");
        this.lastRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.LastRegex, "gis");
        this.thisRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.ThisRegex, "gis");
        this.weekDayOfMonthRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.WeekDayOfMonthRegex, "gis");
        this.utilityConfiguration = config.utilityConfiguration;
        this.dateTokenPrefix = EnglishDateTime.DateTokenPrefix;
    }

    getSwiftDay(source: string):number {
        let trimedText = source.trim().toLowerCase();
        let swift = 0;
        if (trimedText === "today" || trimedText === "the day") {
            swift = 0;
        } else if (trimedText === "tomorrow" || trimedText === "tmr" ||
                    trimedText === "next day" || trimedText === "the next day") {
            swift = 1;
        } else if (trimedText === "yesterday") {
            swift = -1;
        } else if (trimedText.endsWith("day after tomorrow") ||
                    trimedText.endsWith("day after tmr")) {
            swift = 2;
        } else if (trimedText.endsWith("day before yesterday")) {
            swift = -2;
        } else if (trimedText.endsWith("last day")) {
            swift = -1;
        }
        return swift;
    }

    getSwiftMonth(source: string): number {
        let trimedText = source.trim().toLowerCase();
        let swift = 0;
        if (trimedText.startsWith("next") || trimedText.startsWith("upcoming")) {
            swift = 1;
        } else if (trimedText.startsWith("last")) {
            swift = -1;
        }
        return swift;
    }

    isCardinalLast(source: string): boolean {
        let trimedText = source.trim().toLowerCase();
        return trimedText === "last";
    }
}