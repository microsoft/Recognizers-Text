import { CultureInfo, Culture } from "@microsoft/recognizers-text-number";
import { Constants } from "../constants";
import { JapaneseNumberWithUnitExtractorConfiguration, JapaneseNumberWithUnitParserConfiguration } from "./base";
import { JapaneseNumericWithUnit } from "../../resources/japaneseNumericWithUnit";

export class JapaneseAgeExtractorConfiguration extends JapaneseNumberWithUnitExtractorConfiguration {
    readonly suffixList: ReadonlyMap<string, string>;
    readonly prefixList: ReadonlyMap<string, string>;
    readonly ambiguousUnitList: readonly string[];
    readonly extractType: string;

    constructor(ci?: CultureInfo) {
        if (!ci) {
            ci = new CultureInfo(Culture.Japanese);
        }

        super(ci);

        this.extractType = Constants.SYS_UNIT_AGE;

        this.suffixList = JapaneseNumericWithUnit.AgeSuffixList;
        this.prefixList = new Map<string, string>();
        this.ambiguousUnitList = JapaneseNumericWithUnit.AgeAmbiguousValues;
    }
}

export class JapaneseAgeParserConfiguration extends JapaneseNumberWithUnitParserConfiguration {
    constructor(ci?: CultureInfo) {
        if (!ci) {
            ci = new CultureInfo(Culture.Japanese);
        }

        super(ci);

        this.BindDictionary(JapaneseNumericWithUnit.AgeSuffixList);
    }
}