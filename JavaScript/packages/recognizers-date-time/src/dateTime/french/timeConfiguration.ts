import { IExtractor } from "@microsoft/recognizers-text";
import { RegExpUtility } from "@microsoft/recognizers-text";
import { ITimeExtractorConfiguration, ITimeParserConfiguration } from "../baseTime";
import { FrenchDateTime } from "../../resources/frenchDateTime";
import { BaseDurationExtractor } from "../baseDuration";
import { FrenchDurationExtractorConfiguration } from "./durationConfiguration";
import { ICommonDateTimeParserConfiguration } from "../parsers";
import { IDateTimeUtilityConfiguration } from "../utilities";
import { IDateTimeExtractor } from "../baseDateTime";

export class FrenchTimeExtractorConfiguration implements ITimeExtractorConfiguration {
    readonly timeRegexList: RegExp[];
    readonly atRegex: RegExp;
    readonly ishRegex: RegExp;

    readonly durationExtractor: IDateTimeExtractor;

    constructor() {
        this.atRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.AtRegex, "gis");
        this.ishRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.IshRegex, "gis");;
        this.timeRegexList = FrenchTimeExtractorConfiguration.getTimeRegexList();

        this.durationExtractor = new BaseDurationExtractor(new FrenchDurationExtractorConfiguration());
    }

    static getTimeRegexList(): RegExp[] {
        return [
            RegExpUtility.getSafeRegExp(FrenchDateTime.TimeRegex1, "gis"),
            RegExpUtility.getSafeRegExp(FrenchDateTime.TimeRegex2, "gis"),
            RegExpUtility.getSafeRegExp(FrenchDateTime.TimeRegex3, "gis"),
            RegExpUtility.getSafeRegExp(FrenchDateTime.TimeRegex4, "gis"),
            RegExpUtility.getSafeRegExp(FrenchDateTime.TimeRegex5, "gis"),
            RegExpUtility.getSafeRegExp(FrenchDateTime.TimeRegex6, "gis"),
            RegExpUtility.getSafeRegExp(FrenchDateTime.TimeRegex7, "gis"),
            RegExpUtility.getSafeRegExp(FrenchDateTime.TimeRegex8, "gis"),
            RegExpUtility.getSafeRegExp(FrenchDateTime.TimeRegex9, "gis"),
            RegExpUtility.getSafeRegExp(FrenchDateTime.TimeRegex10, "gis"),
            RegExpUtility.getSafeRegExp(FrenchDateTime.ConnectNumRegex, "gis")
        ];
    }
}

export class FrenchTimeParserConfiguration implements ITimeParserConfiguration {

    readonly timeTokenPrefix: string;
    readonly atRegex: RegExp;
    readonly timeRegexes: RegExp[];
    readonly lessThanOneHour: RegExp;
    readonly timeSuffix: RegExp;
    readonly numbers: ReadonlyMap<string, number>;
    readonly utilityConfiguration: IDateTimeUtilityConfiguration;

    constructor(config: ICommonDateTimeParserConfiguration) {

        this.timeTokenPrefix = FrenchDateTime.TimeTokenPrefix;
        this.atRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.AtRegex, "gis");
        this.timeRegexes = FrenchTimeExtractorConfiguration.getTimeRegexList();
        this.lessThanOneHour = RegExpUtility.getSafeRegExp(FrenchDateTime.LessThanOneHour, "gis");
        this.timeSuffix = RegExpUtility.getSafeRegExp(FrenchDateTime.TimeSuffix, "gis");

        this.utilityConfiguration = config.utilityConfiguration;
        this.numbers = config.numbers;
    }

    adjustByPrefix(prefix: string, adjust: { hour: number; min: number; hasMin: boolean; }) {
        let deltaMin = 0;
        let trimedPrefix = prefix.trim().toLowerCase();

        // @todo Move hardcoded strings to resource YAML file.
        if (trimedPrefix.endsWith("demie")) {
            deltaMin = 30;
        }
        else if (trimedPrefix.endsWith("un quart") || trimedPrefix.endsWith("quart")) {
            deltaMin = 15;
        }
        else if (trimedPrefix.endsWith("trois quarts")) {
            deltaMin = 45;
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

        if (trimedPrefix.endsWith("à") || trimedPrefix.includes("moins")) {
            deltaMin = -deltaMin;
        }

        adjust.min += deltaMin;
        if (adjust.min < 0) {
            adjust.min += 60;
            adjust.hour -= 1;
        }

        adjust.hasMin = true;
    }

    adjustBySuffix(suffix: string, adjust: { hour: number; min: number; hasMin: boolean; hasAm: boolean; hasPm: boolean; }) {
        let trimedSuffix = suffix.trim().toLowerCase();

        let deltaHour = 0;
        let matches = RegExpUtility.getMatches(this.timeSuffix, trimedSuffix);
        if (matches.length) {
            let match = matches[0];
            if (match.index === 0 && match.length === trimedSuffix.length) {
                let oclockStr = match.groups("heures").value;
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