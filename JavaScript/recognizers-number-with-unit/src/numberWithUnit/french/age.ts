import { CultureInfo, Culture } from "recognizers-text-number";
import { Constants } from "../constants";
import { FrenchNumberWithUnitExtractorConfiguration, FrenchNumberWithUnitParserConfiguration } from "./base";
import { FrenchNumericWithUnit } from "../../resources/frenchNumericWithUnit";

export class FrenchAgeExtractorConfiguration extends FrenchNumberWithUnitExtractorConfiguration {
    readonly suffixList: ReadonlyMap<string, string>;
    readonly prefixList: ReadonlyMap<string, string>;
    readonly ambiguousUnitList: ReadonlyArray<string>;
    readonly extractType: string;

    constructor(ci?: CultureInfo) {
        if (!ci) {
            ci = new CultureInfo(Culture.French);
        }

        super(ci);

        this.extractType = Constants.SYS_UNIT_AGE;

        this.suffixList = FrenchNumericWithUnit.AgeSuffixList;
        this.prefixList = new Map<string, string>();
        this.ambiguousUnitList = new Array<string>();
    }
}

export class FrenchAgeParserConfiguration extends FrenchNumberWithUnitParserConfiguration {
    constructor(ci?: CultureInfo) {
        if(!ci) {
            ci = new CultureInfo(Culture.French);
        }

        super(ci);

        this.BindDictionary(FrenchNumericWithUnit.AgeSuffixList);
    }
}