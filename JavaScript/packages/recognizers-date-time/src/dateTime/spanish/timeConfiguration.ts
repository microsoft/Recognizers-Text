import { RegExpUtility, IExtractor } from "recognizers-text-number";
import { ITimeExtractorConfiguration, ITimeParserConfiguration } from "../baseTime";
import { SpanishDateTime } from "../../resources/spanishDateTime";
import { BaseDurationExtractor } from "../baseDuration";
import { SpanishDurationExtractorConfiguration } from "./durationConfiguration";
import { ICommonDateTimeParserConfiguration } from "../parsers";
import { IDateTimeUtilityConfiguration } from "../utilities";
import { IDateTimeExtractor } from "../baseDateTime";

export class SpanishTimeExtractorConfiguration implements ITimeExtractorConfiguration {
    readonly timeRegexList: RegExp[];
    readonly atRegex: RegExp;
    readonly ishRegex: RegExp;

    readonly durationExtractor: IDateTimeExtractor;

    constructor() {
        this.atRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.AtRegex, "gis");
        this.ishRegex = null;
        this.timeRegexList = SpanishTimeExtractorConfiguration.getTimeRegexList();

        this.durationExtractor = new BaseDurationExtractor(new SpanishDurationExtractorConfiguration());
    }

    static getTimeRegexList(): RegExp[] {
        return [
            RegExpUtility.getSafeRegExp(SpanishDateTime.TimeRegex1, "gis"),
            RegExpUtility.getSafeRegExp(SpanishDateTime.TimeRegex2, "gis"),
            RegExpUtility.getSafeRegExp(SpanishDateTime.TimeRegex3, "gis"),
            RegExpUtility.getSafeRegExp(SpanishDateTime.TimeRegex4, "gis"),
            RegExpUtility.getSafeRegExp(SpanishDateTime.TimeRegex5, "gis"),
            RegExpUtility.getSafeRegExp(SpanishDateTime.TimeRegex6, "gis"),
            RegExpUtility.getSafeRegExp(SpanishDateTime.TimeRegex7, "gis"),
            RegExpUtility.getSafeRegExp(SpanishDateTime.TimeRegex8, "gis"),
            RegExpUtility.getSafeRegExp(SpanishDateTime.TimeRegex9, "gis"),
            RegExpUtility.getSafeRegExp(SpanishDateTime.TimeRegex10, "gis"),
            RegExpUtility.getSafeRegExp(SpanishDateTime.TimeRegex11, "gis"),
            RegExpUtility.getSafeRegExp(SpanishDateTime.ConnectNumRegex, "gis")
        ]
    }
}

export class SpanishTimeParserConfiguration implements ITimeParserConfiguration {

    readonly timeTokenPrefix: string;
    readonly atRegex: RegExp;
    readonly timeRegexes: RegExp[];
    readonly lessThanOneHour: RegExp;
    readonly timeSuffix: RegExp;
    readonly numbers: ReadonlyMap<string, number>;
    readonly utilityConfiguration: IDateTimeUtilityConfiguration;

    constructor(config: ICommonDateTimeParserConfiguration) {

        this.timeTokenPrefix = SpanishDateTime.TimeTokenPrefix;
        this.atRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.AtRegex, "gis");
        this.timeRegexes = SpanishTimeExtractorConfiguration.getTimeRegexList();
        this.lessThanOneHour = RegExpUtility.getSafeRegExp(SpanishDateTime.LessThanOneHour, "gis");
        this.timeSuffix = RegExpUtility.getSafeRegExp(SpanishDateTime.TimeSuffix, "gis");

        this.utilityConfiguration = config.utilityConfiguration;
        this.numbers = config.numbers;
    }

    adjustByPrefix(prefix: string, adjust: { hour: number; min: number; hasMin: boolean; }) {
        let deltaMin = 0;
        let trimedPrefix = prefix.trim().toLowerCase();

        if (trimedPrefix.startsWith("cuarto") || trimedPrefix.startsWith("y cuarto")) {
            deltaMin = 15;
        }
        else if (trimedPrefix.startsWith("menos cuarto")) {
            deltaMin = -15;
        }
        else if (trimedPrefix.startsWith("media") || trimedPrefix.startsWith("y media")) {
            deltaMin = 30;
        }
        else {
            let matches = RegExpUtility.getMatches(this.lessThanOneHour, trimedPrefix);
            if (matches.length) {
                let match = matches[0];
                let minStr = match.groups("deltamin").value;
                if (minStr) {
                    deltaMin = parseInt(minStr, 10);
                }
                else {
                    minStr = match.groups("deltaminnum").value.toLowerCase();
                    if (this.numbers.has(minStr)) {
                        deltaMin = this.numbers.get(minStr);
                    }
                }
            }
        }

        if (trimedPrefix.endsWith("pasadas") || trimedPrefix.endsWith("pasados") ||
            trimedPrefix.endsWith("pasadas las") || trimedPrefix.endsWith("pasados las") ||
            trimedPrefix.endsWith("pasadas de las") || trimedPrefix.endsWith("pasados de las")) {
            // deltaMin it's positive
        }
        else if (trimedPrefix.endsWith("para la") || trimedPrefix.endsWith("para las") ||
            trimedPrefix.endsWith("antes de la") || trimedPrefix.endsWith("antes de las")) {
            deltaMin = -deltaMin;
        }

        adjust.min += deltaMin;
        if (adjust.min < 0) {
            adjust.min += 60;
            adjust.hour -= 1;
        }

        adjust.hasMin = adjust.hasMin || adjust.min !== 0;
    }

    adjustBySuffix(suffix: string, adjust: { hour: number; min: number; hasMin: boolean; hasAm: boolean; hasPm: boolean; }) {
        let trimedSuffix = suffix.trim().toLowerCase();
        this.adjustByPrefix(trimedSuffix, adjust);

        let deltaHour = 0;
        let matches = RegExpUtility.getMatches(this.timeSuffix, trimedSuffix);
        if (matches.length) {
            let match = matches[0];
            if (match.index === 0 && match.length === trimedSuffix.length) {
                let oclockStr = match.groups("oclock").value;
                if (!oclockStr) {
                    let amStr = match.groups("am").value;
                    if (amStr) {
                        if (adjust.hour >= 12) {
                            deltaHour = -12;
                        }

                        adjust.hasAm = true;
                    }

                    let pmStr = match.groups("pm").value;
                    if (pmStr) {
                        if (adjust.hour < 12) {
                            deltaHour = 12;
                        }

                        adjust.hasPm = true;
                    }
                }
            }
        }

        adjust.hour = (adjust.hour + deltaHour) % 24;
    }
}