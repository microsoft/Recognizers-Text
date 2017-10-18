import { CultureInfo, Culture } from "recognizers-text-number";
import { Constants } from "../constants";
import { ChineseNumberWithUnitExtractorConfiguration, ChineseNumberWithUnitParserConfiguration } from "./base";
import { ChineseNumericWithUnit } from "../../resources/chineseNumericWithUnit";

export class ChineseTemperatureExtractorConfiguration extends ChineseNumberWithUnitExtractorConfiguration {
    readonly suffixList: ReadonlyMap<string, string>;
    readonly prefixList: ReadonlyMap<string, string>;
    readonly ambiguousUnitList: ReadonlyArray<string>;
    readonly extractType: string;

    constructor(ci?: CultureInfo) {
        if (!ci) {
            ci = new CultureInfo(Culture.Chinese);
        }

        super(ci);

        this.extractType = Constants.SYS_UNIT_TEMPERATURE;

        this.suffixList = ChineseNumericWithUnit.TemperatureSuffixList;
        this.prefixList = ChineseNumericWithUnit.TemperaturePrefixList;
        this.ambiguousUnitList = ChineseNumericWithUnit.TemperatureAmbiguousValues;
    }
}

export class ChineseTemperatureParserConfiguration extends ChineseNumberWithUnitParserConfiguration {
    constructor(ci?: CultureInfo) {
        if(!ci) {
            ci = new CultureInfo(Culture.Chinese);
        }

        super(ci);

        this.BindDictionary(ChineseNumericWithUnit.TemperaturePrefixList);
        this.BindDictionary(ChineseNumericWithUnit.TemperatureSuffixList);
    }
}