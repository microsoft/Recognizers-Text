import { CultureInfo, Culture } from "recognizers-text-number";
import { Constants } from "../constants";
import { ChineseNumberWithUnitExtractorConfiguration, ChineseNumberWithUnitParserConfiguration } from "./base";
import { ChineseNumericWithUnit } from "../../resources/chineseNumericWithUnit";

export class ChineseAgeExtractorConfiguration extends ChineseNumberWithUnitExtractorConfiguration {
    readonly suffixList: ReadonlyMap<string, string>;
    readonly prefixList: ReadonlyMap<string, string>;
    readonly ambiguousUnitList: ReadonlyArray<string>;
    readonly extractType: string;

    constructor(ci?: CultureInfo) {
        if (!ci) {
            ci = new CultureInfo(Culture.Chinese);
        }

        super(ci);

        this.extractType = Constants.SYS_UNIT_AGE;

        this.suffixList = ChineseNumericWithUnit.AgeSuffixList;
        this.prefixList = new Map<string, string>();
        this.ambiguousUnitList = ChineseNumericWithUnit.AgeAmbiguousValues;
    }
}

export class ChineseAgeParserConfiguration extends ChineseNumberWithUnitParserConfiguration {
    constructor(ci?: CultureInfo) {
        if(!ci) {
            ci = new CultureInfo(Culture.Chinese);
        }

        super(ci);

        this.BindDictionary(ChineseNumericWithUnit.AgeSuffixList);
    }
}