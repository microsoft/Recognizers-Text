import { CultureInfo, Culture } from "recognizers-text-number";
import { Constants } from "../constants";
import { EnglishNumberWithUnitExtractorConfiguration, EnglishNumberWithUnitParserConfiguration } from "./base";
import { EnglishNumericWithUnit } from "../../resources/englishNumericWithUnit";

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

        this.suffixList = EnglishNumericWithUnit.AgeSuffixList;
        this.prefixList = new Map<string, string>();
        this.ambiguousUnitList = new Array<string>();
    }
}

export class EnglishAgeParserConfiguration extends EnglishNumberWithUnitParserConfiguration {
    constructor(ci?: CultureInfo) {
        if(!ci) {
            ci = new CultureInfo(Culture.English);
        }

        super(ci);

        this.BindDictionary(EnglishNumericWithUnit.AgeSuffixList);
    }
}