import { IHolidayExtractorConfiguration, BaseHolidayParserConfiguration } from "../baseHoliday";
import { RegExpUtility } from "../../utilities";
import { SpanishDateTime } from "../../resources/spanishDateTime";
import { StringUtilities } from "./utilities";

export class SpanishHolidayExtractorConfiguration implements IHolidayExtractorConfiguration {
    readonly holidayRegexes: RegExp[];

    constructor() {
        this.holidayRegexes = [
            RegExpUtility.getSafeRegExp(SpanishDateTime.HolidayRegex1, "gis"),
            RegExpUtility.getSafeRegExp(SpanishDateTime.HolidayRegex2, "gis"),
            RegExpUtility.getSafeRegExp(SpanishDateTime.HolidayRegex3, "gis")
        ];
    }
}

export class SpanishHolidayParserConfiguration extends BaseHolidayParserConfiguration {

    readonly nextPrefixRegex: RegExp;
    readonly pastPrefixRegex: RegExp;
    readonly thisPrefixRegex: RegExp;

    constructor() {
        super();

        this.holidayRegexList = [
            RegExpUtility.getSafeRegExp(SpanishDateTime.HolidayRegex1, "gis"),
            RegExpUtility.getSafeRegExp(SpanishDateTime.HolidayRegex2, "gis"),
            RegExpUtility.getSafeRegExp(SpanishDateTime.HolidayRegex3, "gis")
        ];

        this.holidayNames = SpanishDateTime.HolidayNames;

        this.nextPrefixRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.NextPrefixRegex);
        this.pastPrefixRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.PastPrefixRegex);
        this.thisPrefixRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.ThisPrefixRegex);
    }

    getSwiftYear(text: string): number {
        let trimedText = text.trim().toLowerCase();
        let swift = -10;

        if (RegExpUtility.getFirstMatchIndex(this.nextPrefixRegex, trimedText).matched) {
            swift = 1;
        }

        if (RegExpUtility.getFirstMatchIndex(this.pastPrefixRegex, trimedText).matched) {
            swift = -1;
        }
        else if (RegExpUtility.getFirstMatchIndex(this.thisPrefixRegex, trimedText).matched) {
            swift = 0;
        }

        return swift;
    }

    sanitizeHolidayToken(holiday: string): string {
        return StringUtilities.normalize(holiday)
            .replace(/ /g, "");
    }
}