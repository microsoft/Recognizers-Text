import { IHolidayExtractorConfiguration, BaseHolidayParserConfiguration } from "../baseHoliday"
import { RegExpUtility } from "recognizers-text-number";
import { DateUtils } from "../utilities";
import { EnglishDateTime } from "../../resources/englishDateTime";

export class EnglishHolidayExtractorConfiguration implements IHolidayExtractorConfiguration {
    readonly holidayRegexes: RegExp[]
            
    constructor() {
        this.holidayRegexes = [
                    RegExpUtility.getSafeRegExp(EnglishDateTime.HolidayRegex1, "gis"),
                    RegExpUtility.getSafeRegExp(EnglishDateTime.HolidayRegex2, "gis"),
                    RegExpUtility.getSafeRegExp(EnglishDateTime.HolidayRegex3, "gis")
                ];
            }
}
   
export class EnglishHolidayParserConfiguration extends BaseHolidayParserConfiguration {
    constructor() {
        super();
        this.holidayRegexList = [
            RegExpUtility.getSafeRegExp(EnglishDateTime.HolidayRegex1, "gis"),
            RegExpUtility.getSafeRegExp(EnglishDateTime.HolidayRegex2, "gis"),
            RegExpUtility.getSafeRegExp(EnglishDateTime.HolidayRegex3, "gis")
        ];
        this.holidayNames = EnglishDateTime.HolidayNames;
        this.holidayFuncDictionary = this.initHolidayFuncs();
    }

    protected initHolidayFuncs(): ReadonlyMap<string, (year: number) => Date> {
        return new Map<string, (year: number) => Date>(
            [
                ...super.initHolidayFuncs(),
                ["maosbirthday", EnglishHolidayParserConfiguration.MaoBirthday],
                ["yuandan", EnglishHolidayParserConfiguration.NewYear],
                ["teachersday", EnglishHolidayParserConfiguration.TeacherDay],
                ["singleday", EnglishHolidayParserConfiguration.SinglesDay],
                ["allsaintsday", EnglishHolidayParserConfiguration.HalloweenDay],
                ["youthday", EnglishHolidayParserConfiguration.YouthDay],
                ["childrenday", EnglishHolidayParserConfiguration.ChildrenDay],
                ["femaleday", EnglishHolidayParserConfiguration.FemaleDay],
                ["treeplantingday", EnglishHolidayParserConfiguration.TreePlantDay],
                ["arborday", EnglishHolidayParserConfiguration.TreePlantDay],
                ["girlsday", EnglishHolidayParserConfiguration.GirlsDay],
                ["whiteloverday", EnglishHolidayParserConfiguration.WhiteLoverDay],
                ["loverday", EnglishHolidayParserConfiguration.ValentinesDay],
                ["christmas", EnglishHolidayParserConfiguration.ChristmasDay],
                ["xmas", EnglishHolidayParserConfiguration.ChristmasDay],
                ["newyear", EnglishHolidayParserConfiguration.NewYear],
                ["newyearday", EnglishHolidayParserConfiguration.NewYear],
                ["newyearsday", EnglishHolidayParserConfiguration.NewYear],
                ["inaugurationday", EnglishHolidayParserConfiguration.InaugurationDay],
                ["groundhougday", EnglishHolidayParserConfiguration.GroundhogDay],
                ["valentinesday", EnglishHolidayParserConfiguration.ValentinesDay],
                ["stpatrickday", EnglishHolidayParserConfiguration.StPatrickDay],
                ["aprilfools", EnglishHolidayParserConfiguration.FoolDay],
                ["stgeorgeday", EnglishHolidayParserConfiguration.StGeorgeDay],
                ["mayday", EnglishHolidayParserConfiguration.Mayday],
                ["cincodemayoday", EnglishHolidayParserConfiguration.CincoDeMayoday],
                ["baptisteday", EnglishHolidayParserConfiguration.BaptisteDay],
                ["usindependenceday", EnglishHolidayParserConfiguration.UsaIndependenceDay],
                ["independenceday", EnglishHolidayParserConfiguration.UsaIndependenceDay],
                ["bastilleday", EnglishHolidayParserConfiguration.BastilleDay],
                ["halloweenday", EnglishHolidayParserConfiguration.HalloweenDay],
                ["allhallowday", EnglishHolidayParserConfiguration.AllHallowDay],
                ["allsoulsday", EnglishHolidayParserConfiguration.AllSoulsday],
                ["guyfawkesday", EnglishHolidayParserConfiguration.GuyFawkesDay],
                ["veteransday", EnglishHolidayParserConfiguration.Veteransday],
                ["christmaseve", EnglishHolidayParserConfiguration.ChristmasEve],
                ["newyeareve", EnglishHolidayParserConfiguration.NewYearEve],
                ["easterday", EnglishHolidayParserConfiguration.EasterDay]
            ]);
    }

    // All JavaScript dates are zero-based (-1)
    private static NewYear(year: number): Date { return new Date(year, 1 - 1, 1); }
    private static NewYearEve(year: number): Date { return new Date(year, 12 - 1, 31); }
    private static ChristmasDay(year: number): Date { return new Date(year, 12 - 1, 25); }
    private static ChristmasEve(year: number): Date { return new Date(year, 12 - 1, 24); }
    private static ValentinesDay(year: number): Date { return new Date(year, 2 - 1, 14); }
    private static WhiteLoverDay(year: number): Date { return new Date(year, 3 - 1, 14); }
    private static FoolDay(year: number): Date { return new Date(year, 4 - 1, 1); }
    private static GirlsDay(year: number): Date { return new Date(year, 3 - 1, 7); }
    private static TreePlantDay(year: number): Date { return new Date(year, 3 - 1, 12); }
    private static FemaleDay(year: number): Date { return new Date(year, 3 - 1, 8); }
    private static ChildrenDay(year: number): Date { return new Date(year, 6 - 1, 1); }
    private static YouthDay(year: number): Date { return new Date(year, 5 - 1, 4); }
    private static TeacherDay(year: number): Date { return new Date(year, 9 - 1, 10); }
    private static SinglesDay(year: number): Date { return new Date(year, 11 - 1, 11); }
    private static MaoBirthday(year: number): Date { return new Date(year, 12 - 1, 26); }
    private static InaugurationDay(year: number): Date { return new Date(year, 1 - 1, 20); }
    private static GroundhogDay(year: number): Date { return new Date(year, 2 - 1, 2); }
    private static StPatrickDay(year: number): Date { return new Date(year, 3 - 1, 17); }
    private static StGeorgeDay(year: number): Date { return new Date(year, 4 - 1, 23); }
    private static Mayday(year: number): Date { return new Date(year, 5 - 1, 1); }
    private static CincoDeMayoday(year: number): Date { return new Date(year, 5 - 1, 5); }
    private static BaptisteDay(year: number): Date { return new Date(year, 6 - 1, 24); }
    private static UsaIndependenceDay(year: number): Date { return new Date(year, 7 - 1, 4); }
    private static BastilleDay(year: number): Date { return new Date(year, 7 - 1, 14); }
    private static HalloweenDay(year: number): Date { return new Date(year, 10 - 1, 31); }
    private static AllHallowDay(year: number): Date { return new Date(year, 11 - 1, 1); }
    private static AllSoulsday(year: number): Date { return new Date(year, 11 - 1, 2); }
    private static GuyFawkesDay(year: number): Date { return new Date(year, 11 - 1, 5); }
    private static Veteransday(year: number): Date { return new Date(year, 11 - 1, 11); }
    private static EasterDay(year: number): Date { return DateUtils.minValue(); }

    public getSwiftYear(text: string): number {
        let trimmedText = text.trim().toLowerCase();
        let swift = -10;
        if (trimmedText.startsWith("next")) {
            swift = 1;
        }
        else if (trimmedText.startsWith("last")) {
            swift = -1;
        }
        else if (trimmedText.startsWith("this")) {
            swift = 0;
        }
        return swift;
    }

    public sanitizeHolidayToken(holiday: string): string {
        return holiday.replace(/[ ']/g, "");
    }
}