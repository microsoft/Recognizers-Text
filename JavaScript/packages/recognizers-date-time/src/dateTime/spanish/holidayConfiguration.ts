import { IHolidayExtractorConfiguration, BaseHolidayParserConfiguration } from "../baseHoliday";
import { RegExpUtility } from "@microsoft/recognizers-text";
import { DateUtils } from "../utilities";
import { SpanishDateTime } from "../../resources/spanishDateTime";

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
    readonly previousPrefixRegex: RegExp;
    readonly thisPrefixRegex: RegExp;

    constructor() {
        super();

        this.holidayRegexList = [
            RegExpUtility.getSafeRegExp(SpanishDateTime.HolidayRegex1, "gis"),
            RegExpUtility.getSafeRegExp(SpanishDateTime.HolidayRegex2, "gis"),
            RegExpUtility.getSafeRegExp(SpanishDateTime.HolidayRegex3, "gis")
        ];

        this.holidayNames = SpanishDateTime.HolidayNames;
        this.holidayFuncDictionary = this.initHolidayFuncs();
        this.variableHolidaysTimexDictionary = SpanishDateTime.VariableHolidaysTimexDictionary;

        this.nextPrefixRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.NextPrefixRegex);
        this.previousPrefixRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.PreviousPrefixRegex);
        this.thisPrefixRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.ThisPrefixRegex);
    }

    protected initHolidayFuncs(): ReadonlyMap<string, (year: number) => Date> {
        return new Map<string, (year: number) => Date>(
            [
                ...super.initHolidayFuncs(),
                ["padres", SpanishHolidayParserConfiguration.FathersDay],
                ["madres", SpanishHolidayParserConfiguration.MothersDay],
                ["acciondegracias", SpanishHolidayParserConfiguration.ThanksgivingDay],
                ["trabajador", SpanishHolidayParserConfiguration.LabourDay],
                ["delaraza", SpanishHolidayParserConfiguration.ColumbusDay],
                ["memoria", SpanishHolidayParserConfiguration.MemorialDay],
                ["pascuas", SpanishHolidayParserConfiguration.EasterDay],
                ["navidad", SpanishHolidayParserConfiguration.ChristmasDay],
                ["nochebuena", SpanishHolidayParserConfiguration.ChristmasEve],
                ["añonuevo", SpanishHolidayParserConfiguration.NewYear],
                ["nochevieja", SpanishHolidayParserConfiguration.NewYearEve],
                ["yuandan", SpanishHolidayParserConfiguration.NewYear],
                ["maestro", SpanishHolidayParserConfiguration.TeacherDay],
                ["todoslossantos", SpanishHolidayParserConfiguration.HalloweenDay],
                ["niño", SpanishHolidayParserConfiguration.ChildrenDay],
                ["mujer", SpanishHolidayParserConfiguration.FemaleDay]
            ]);
    }

    // All JavaScript dates are zero-based (-1)
    private static NewYear(year: number): Date { return new Date(year, 1 - 1, 1); }
    private static NewYearEve(year: number): Date { return new Date(year, 12 - 1, 31); }
    private static ChristmasDay(year: number): Date { return new Date(year, 12 - 1, 25); }
    private static ChristmasEve(year: number): Date { return new Date(year, 12 - 1, 24); }
    private static FemaleDay(year: number): Date { return new Date(year, 3 - 1, 8); }
    private static ChildrenDay(year: number): Date { return new Date(year, 6 - 1, 1); }
    private static HalloweenDay(year: number): Date { return new Date(year, 10 - 1, 31); }
    private static TeacherDay(year: number): Date { return new Date(year, 9 - 1, 11); }
    private static EasterDay(year: number): Date { return DateUtils.minValue(); }

    getSwiftYear(text: string): number {
        let trimedText = text.trim().toLowerCase();
        let swift = -10;

        if (RegExpUtility.getFirstMatchIndex(this.nextPrefixRegex, trimedText).matched) {
            swift = 1;
        }

        if (RegExpUtility.getFirstMatchIndex(this.previousPrefixRegex, trimedText).matched) {
            swift = -1;
        }
        else if (RegExpUtility.getFirstMatchIndex(this.thisPrefixRegex, trimedText).matched) {
            swift = 0;
        }

        return swift;
    }

    sanitizeHolidayToken(holiday: string): string {
        return holiday.replace(/ /g, "")
            .replace(/á/g, "a")
            .replace(/é/g, "e")
            .replace(/í/g, "i")
            .replace(/ó/g, "o")
            .replace(/ú/g, "u");
    }
}