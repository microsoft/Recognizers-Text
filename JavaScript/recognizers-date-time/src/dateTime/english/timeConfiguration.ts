import { ITimeExtractorConfiguration, ITimeParserConfiguration } from "../baseTime"
import { RegExpUtility } from "recognizers-text-number";
import { EnglishDateTime } from "../../resources/englishDateTime";
import { ICommonDateTimeParserConfiguration } from "../parsers"
import { IDateTimeUtilityConfiguration } from "../utilities";

export class EnglishTimeExtractorConfiguration implements ITimeExtractorConfiguration {
    public static timeRegexList: RegExp[] = [
        // (three min past)? seven|7|(seven thirty) pm
        RegExpUtility.getSafeRegExp(EnglishDateTime.TimeRegex1, "gis"),
        // (three min past)? 3:00(:00)? (pm)?
        RegExpUtility.getSafeRegExp(EnglishDateTime.TimeRegex2, "gis"),
        // (three min past)? 3.00 (pm)?
        RegExpUtility.getSafeRegExp(EnglishDateTime.TimeRegex3, "gis"),
        // (three min past) (five thirty|seven|7|7:00(:00)?) (pm)? (in the night)
        RegExpUtility.getSafeRegExp(EnglishDateTime.TimeRegex4, "gis"),
        // (three min past) (five thirty|seven|7|7:00(:00)?) (pm)?
        RegExpUtility.getSafeRegExp(EnglishDateTime.TimeRegex5, "gis"),
        // (five thirty|seven|7|7:00(:00)?) (pm)? (in the night)
        RegExpUtility.getSafeRegExp(EnglishDateTime.TimeRegex6, "gis"),
        // (in the night) at (five thirty|seven|7|7:00(:00)?) (pm)?
        RegExpUtility.getSafeRegExp(EnglishDateTime.TimeRegex7, "gis"),
        // (in the night) (five thirty|seven|7|7:00(:00)?) (pm)?
        RegExpUtility.getSafeRegExp(EnglishDateTime.TimeRegex8, "gis"),
        RegExpUtility.getSafeRegExp(EnglishDateTime.TimeRegex9, "gis"),
        // 340pm
        RegExpUtility.getSafeRegExp(EnglishDateTime.ConnectNumRegex, "gis")
    ];
    public static atRegex: RegExp = RegExpUtility.getSafeRegExp(EnglishDateTime.AtRegex, "gis");
    public static lessThanOneHour: RegExp = RegExpUtility.getSafeRegExp(EnglishDateTime.LessThanOneHour, "gis");
    public static timeSuffix: RegExp = RegExpUtility.getSafeRegExp(EnglishDateTime.TimeSuffix, "gis");
    public static timeSuffixFull: RegExp = RegExpUtility.getSafeRegExp(EnglishDateTime.TimeSuffixFull, "gis");
    public static ishRegex: RegExp = RegExpUtility.getSafeRegExp(EnglishDateTime.IshRegex, "gis");

    readonly timeRegexList: RegExp[];
    readonly atRegex: RegExp;
    readonly ishRegex: RegExp;

    constructor() {
        this.timeRegexList = EnglishTimeExtractorConfiguration.timeRegexList;
        this.atRegex = EnglishTimeExtractorConfiguration.atRegex;
        this.ishRegex = EnglishTimeExtractorConfiguration.ishRegex;
    }
}

export class EnglishTimeParserConfiguration implements ITimeParserConfiguration {
    readonly timeTokenPrefix: string;
    readonly atRegex: RegExp
    readonly timeRegexes: RegExp[];
    readonly numbers: ReadonlyMap<string, number>;
    readonly lunchRegex: RegExp;
    readonly timeSuffixFull: RegExp;
    readonly nightRegex: RegExp;
    readonly utilityConfiguration: IDateTimeUtilityConfiguration;

    constructor(config: ICommonDateTimeParserConfiguration) {
        this.timeTokenPrefix = EnglishDateTime.TimeTokenPrefix;
        this.atRegex = EnglishTimeExtractorConfiguration.atRegex;
        this.timeRegexes = EnglishTimeExtractorConfiguration.timeRegexList;
        this.numbers = config.numbers;
        this.lunchRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.LunchRegex);
        this.timeSuffixFull = RegExpUtility.getSafeRegExp(EnglishDateTime.TimeSuffixFull);
        this.nightRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.NightRegex);
        this.utilityConfiguration = config.utilityConfiguration;
    }

    public adjustByPrefix(prefix: string, adjust: { hour: number, min: number, hasMin: boolean }) {
        let deltaMin = 0;
        let trimmedPrefix = prefix.trim().toLowerCase();

        if (trimmedPrefix.startsWith("half")) {
            deltaMin = 30;
        }
        else if (trimmedPrefix.startsWith("a quarter") || trimmedPrefix.startsWith("quarter")) {
            deltaMin = 15;
        }
        else if (trimmedPrefix.startsWith("three quarter")) {
            deltaMin = 45;
        }
        else {
            let match = RegExpUtility.getMatches(EnglishTimeExtractorConfiguration.lessThanOneHour, trimmedPrefix);
            let minStr = match[0].groups("deltamin").value;
            if (minStr) {
                deltaMin = Number.parseInt(minStr, 10);
            }
            else {
                minStr = match[0].groups("deltaminnum").value.toLowerCase();
                deltaMin = this.numbers.get(minStr);
            }
        }

        if (trimmedPrefix.endsWith("to")) {
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
        let trimmedSuffix = suffix.trim().toLowerCase();
        let deltaHour = 0;
        let matches = RegExpUtility.getMatches(EnglishTimeExtractorConfiguration.timeSuffixFull, trimmedSuffix);
        if (matches.length > 0 && matches[0].index === 0 && matches[0].length === trimmedSuffix.length) {
            let oclockStr = matches[0].groups("oclock").value;
            if (!oclockStr) {
                let amStr = matches[0].groups("am").value;
                if (amStr) {
                    if (adjust.hour >= 12) {
                        deltaHour = -12;
                    } else {
                        adjust.hasAm = true;
                    }
                }

                let pmStr = matches[0].groups("pm").value;
                if (pmStr) {
                    if (adjust.hour < 12) {
                        deltaHour = 12;
                    }

                    if (RegExpUtility.getMatches(this.lunchRegex, pmStr).length > 0) {
                        // for hour>=10, <12
                        if (adjust.hour >= 10 && adjust.hour <= 12) {
                            deltaHour = 0;
                            if (adjust.hour === 12) {
                                adjust.hasPm = true;
                            } else {
                                adjust.hasAm = true;
                            }
                        } else {
                            adjust.hasPm = true;
                        }
                    } else if (RegExpUtility.getMatches(this.nightRegex, pmStr).length > 0) {
                        // for hour <=3 or == 12, we treat it as am, for example 1 in the night (midnight) == 1am
                        if (adjust.hour <= 3 || adjust.hour === 12) {
                            if (adjust.hour === 12) {
                                adjust.hour = 0;
                            }
                            deltaHour = 0;
                            adjust.hasAm = true;
                        } else {
                            adjust.hasPm = true;
                        }
                    } else {
                        adjust.hasPm = true;
                    }
                }
            }
        }

        adjust.hour = (adjust.hour + deltaHour) % 24;
    }
}
