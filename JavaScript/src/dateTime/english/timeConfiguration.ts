import { ITimeExtractorConfiguration, ITimeParserConfiguration } from "../baseTime"
import { RegExpUtility } from "../../utilities";
import { EnglishDateTime } from "../../resources/englishDateTime";
import { ICommonDateTimeParserConfiguration } from "../parsers"

export class EnglishTimeExtractorConfiguration implements ITimeExtractorConfiguration {
    public static timeRegexList: RegExp[] = [
        // (three min past)? seven|7|(senven thirty) pm
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

    constructor(config: ICommonDateTimeParserConfiguration) {
        this.timeTokenPrefix = EnglishDateTime.TimeTokenPrefix;
        this.atRegex = EnglishTimeExtractorConfiguration.atRegex;
        this.timeRegexes = EnglishTimeExtractorConfiguration.timeRegexList;
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
            let match = RegExpUtility.getMatches(EnglishTimeExtractorConfiguration.lessThanOneHour, trimedPrefix);
            let minStr = match[0].groups("deltamin").value;
            if (minStr) {
                deltaMin = Number.parseInt(minStr, 10);
            }
            else {
                minStr = match[0].groups("deltaminnum").value.toLowerCase();
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
        let matches = RegExpUtility.getMatches(EnglishTimeExtractorConfiguration.timeSuffix, trimedSuffix);
        if (matches.length > 0 && matches[0].index === 0 && matches[0].length === trimedSuffix.length) {
            let oclockStr = matches[0].groups("oclock").value;
            if (!oclockStr) {
                let amStr = matches[0].groups("am").value;
                if (amStr) {
                    if (adjust.hour >= 12) {
                        deltaHour = -12;
                    }
                    adjust.hasAm = true;
                }

                let pmStr = matches[0].groups("pm").value;
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
