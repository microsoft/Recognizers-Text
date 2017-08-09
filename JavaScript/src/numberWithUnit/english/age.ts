import { CultureInfo, Culture } from "../../culture";
import { Constants } from "../constants";
import { EnglishNumberWithUnitExtractorConfiguration, EnglishNumberWithUnitParserConfiguration } from "./base";

export class EnglishAgeExtractorConfiguration extends EnglishNumberWithUnitExtractorConfiguration {
    readonly suffixList: ReadonlyMap<string, string>;
    readonly prefixList: ReadonlyMap<string, string>;
    readonly ambiguousUnitList: ReadonlyArray<string>;
    readonly extractType: string;

    constructor(ci?: CultureInfo) {
        if (!ci) {
            ci = new CultureInfo(Culture.English);
        }

        super(ci);

        this.extractType = Constants.SYS_UNIT_AGE;

        this.suffixList = EnglishAgeExtractorConfiguration.ageSuffixList();
        this.prefixList = new Map<string, string>();
        this.ambiguousUnitList = new Array<string>();
    }

    static ageSuffixList(): ReadonlyMap<string, string> {
        return new Map<string, string>([
            ["Year", "years old|year old|year-old|years-old|-year-old|-years-old|years of age|year of age"],
            ["Month", "months old|month old|month-old|months-old|-month-old|-months-old|month of age|months of age"],
            ["Week", "weeks old|week old|week-old|weeks-old|-week-old|-weeks-old|week of age|weeks of age"],
            ["Day", "days old|day old|day-old|days-old|-day-old|-days-old|day of age|days of age"]
        ]);
    }
}

export class EnglishAgeParserConfiguration extends EnglishNumberWithUnitParserConfiguration {
    constructor(ci?: CultureInfo) {
        if(!ci) {
            ci = new CultureInfo(Culture.English);
        }

        super(ci);

        this.BindDictionary(EnglishAgeExtractorConfiguration.ageSuffixList());
    }
}