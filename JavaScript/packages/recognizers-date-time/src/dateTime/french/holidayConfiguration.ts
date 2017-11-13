import { IHolidayExtractorConfiguration, BaseHolidayParserConfiguration } from "../baseHoliday";
import { RegExpUtility } from "recognizers-text-number";
import { DateUtils } from "../utilities";
import { FrenchDateTime } from "../../resources/frenchDateTime";

export class FrenchHolidayExtractorConfiguration implements IHolidayExtractorConfiguration {
    readonly holidayRegexes: RegExp[];

    constructor() {
        this.holidayRegexes = [
            RegExpUtility.getSafeRegExp(FrenchDateTime.HolidayRegex1, "gis"),
            RegExpUtility.getSafeRegExp(FrenchDateTime.HolidayRegex2, "gis"),
            RegExpUtility.getSafeRegExp(FrenchDateTime.HolidayRegex3, "gis"),
            RegExpUtility.getSafeRegExp(FrenchDateTime.HolidayRegex4, "gis")
        ];
    }
}

export class FrenchHolidayParserConfiguration extends BaseHolidayParserConfiguration {

    constructor() {
        super();

        this.holidayRegexList = [
            RegExpUtility.getSafeRegExp(FrenchDateTime.HolidayRegex1, "gis"),
            RegExpUtility.getSafeRegExp(FrenchDateTime.HolidayRegex2, "gis"),
            RegExpUtility.getSafeRegExp(FrenchDateTime.HolidayRegex3, "gis"),
            RegExpUtility.getSafeRegExp(FrenchDateTime.HolidayRegex4, "gis")
        ];

        this.holidayNames = FrenchDateTime.HolidayNames;
        this.holidayFuncDictionary = this.initHolidayFuncs();
    }

    protected initHolidayFuncs(): ReadonlyMap<string, (year: number) => Date> {
        return new Map<string, (year: number) => Date>(
            [
                ...super.initHolidayFuncs(),
                ["maosbirthday", FrenchHolidayParserConfiguration.MaoBirthday],
                ["yuandan", FrenchHolidayParserConfiguration.NewYear],
                ["teachersday", FrenchHolidayParserConfiguration.TeacherDay],
                ["singleday", FrenchHolidayParserConfiguration.SinglesDay],
                ["allsaintsday", FrenchHolidayParserConfiguration.HalloweenDay],
                ["youthday", FrenchHolidayParserConfiguration.YouthDay],
                ["childrenday", FrenchHolidayParserConfiguration.ChildrenDay],
                ["femaleday", FrenchHolidayParserConfiguration.FemaleDay],
                ["treeplantingday", FrenchHolidayParserConfiguration.TreePlantDay],
                ["arborday", FrenchHolidayParserConfiguration.TreePlantDay],
                ["girlsday", FrenchHolidayParserConfiguration.GirlsDay],
                ["whiteloverday", FrenchHolidayParserConfiguration.WhiteLoverDay],
                ["loverday", FrenchHolidayParserConfiguration.ValentinesDay],
                ["christmas", FrenchHolidayParserConfiguration.ChristmasDay],
                ["xmas", FrenchHolidayParserConfiguration.ChristmasDay],
                ["newyear", FrenchHolidayParserConfiguration.NewYear],
                ["newyearday", FrenchHolidayParserConfiguration.NewYear],
                ["newyearsday", FrenchHolidayParserConfiguration.NewYear],
                ["inaugurationday", FrenchHolidayParserConfiguration.InaugurationDay],
                ["groundhougday", FrenchHolidayParserConfiguration.GroundhogDay],
                ["valentinesday", FrenchHolidayParserConfiguration.ValentinesDay],
                ["stpatrickday", FrenchHolidayParserConfiguration.StPatrickDay],
                ["aprilfools", FrenchHolidayParserConfiguration.FoolDay],
                ["stgeorgeday", FrenchHolidayParserConfiguration.StGeorgeDay],
                ["mayday", FrenchHolidayParserConfiguration.Mayday],
                ["cincodemayoday", FrenchHolidayParserConfiguration.CincoDeMayoday],
                ["baptisteday", FrenchHolidayParserConfiguration.BaptisteDay],
                ["usindependenceday", FrenchHolidayParserConfiguration.UsaIndependenceDay],
                ["independenceday", FrenchHolidayParserConfiguration.UsaIndependenceDay],
                ["bastilleday", FrenchHolidayParserConfiguration.BastilleDay],
                ["halloweenday", FrenchHolidayParserConfiguration.HalloweenDay],
                ["allhallowday", FrenchHolidayParserConfiguration.AllHallowDay],
                ["allsoulsday", FrenchHolidayParserConfiguration.AllSoulsday],
                ["guyfawkesday", FrenchHolidayParserConfiguration.GuyFawkesDay],
                ["veteransday", FrenchHolidayParserConfiguration.Veteransday],
                ["christmaseve", FrenchHolidayParserConfiguration.ChristmasEve],
                ["newyeareve", FrenchHolidayParserConfiguration.NewYearEve],
                ["fathersday", FrenchHolidayParserConfiguration.FathersDay ],
                ["mothersday", FrenchHolidayParserConfiguration.MothersDay],
                ["labourday", FrenchHolidayParserConfiguration.LabourDay ]
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
    private static EasterDay(year: number): Date { return DateUtils.minValue(); }

    private static ValentinesDay(year: number): Date { return new Date(year, 2, 14);}
    private static WhiteLoverDay(year: number): Date { return new Date(year, 3, 14);}
    private static FoolDay(year: number): Date { return new Date(year, 4, 1);}
    private static GirlsDay(year: number): Date { return new Date(year, 3, 7);}
    private static TreePlantDay(year: number): Date { return new Date(year, 3, 12);}
    private static YouthDay(year: number): Date { return new Date(year, 5, 4);}
    private static TeacherDay(year: number): Date { return new Date(year, 9, 10);}
    private static SinglesDay(year: number): Date { return new Date(year, 11, 11);}
    private static MaoBirthday(year: number): Date { return new Date(year, 12, 26);}
    private static InaugurationDay(year: number): Date { return new Date(year, 1, 20);}
    private static GroundhogDay(year: number): Date { return new Date(year, 2, 2);}
    private static StPatrickDay(year: number): Date { return new Date(year, 3, 17);}
    private static StGeorgeDay(year: number): Date { return new Date(year, 4, 23);}
    private static Mayday(year: number): Date { return new Date(year, 5, 1);}
    private static CincoDeMayoday(year: number): Date { return new Date(year, 5, 5);}
    private static BaptisteDay(year: number): Date { return new Date(year, 6, 24);}
    private static UsaIndependenceDay(year: number): Date { return new Date(year, 7, 4);}
    private static BastilleDay(year: number): Date { return new Date(year, 7, 14);}
    private static AllHallowDay(year: number): Date { return new Date(year, 11, 1);}
    private static AllSoulsday(year: number): Date { return new Date(year, 11, 2);}
    private static GuyFawkesDay(year: number): Date { return new Date(year, 11, 5);}
    private static Veteransday(year: number): Date { return new Date(year, 11, 11);}
    protected static FathersDay(year: number): Date { return new Date(year, 6, 17);}
    protected static MothersDay(year: number): Date { return new Date(year, 5, 27);}
    protected static LabourDay(year: number): Date { return new Date(year, 5, 1);}

    getSwiftYear(text: string): number {
        let trimedText = text.trim().toLowerCase();
        let swift = -10;

        if (trimedText.endsWith("prochain")) { // next - 'l'annee prochain')
            swift = 1;
        }
        else if (trimedText.endsWith("dernier")) { // last - 'l'annee dernier'
            swift = -1;
        }
        else if (trimedText.startsWith("cette")) { // this - 'cette annees'
            swift = 0;
        }

        return swift;
    }

    sanitizeHolidayToken(holiday: string): string {
        return holiday.replace(/ /g, "")
            .replace(/'/g, "");
    }
}