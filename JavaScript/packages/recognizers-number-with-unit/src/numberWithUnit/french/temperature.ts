import { CultureInfo, Culture, RegExpUtility } from "@microsoft/recognizers-text-number";
import { Constants } from "../constants";
import { FrenchNumberWithUnitExtractorConfiguration, FrenchNumberWithUnitParserConfiguration } from "./base";
import { FrenchNumericWithUnit } from "../../resources/frenchNumericWithUnit";
import { BaseUnits } from "../../resources/baseUnits";

export class FrenchTemperatureExtractorConfiguration extends FrenchNumberWithUnitExtractorConfiguration {
    readonly suffixList: ReadonlyMap<string, string>;
    readonly prefixList: ReadonlyMap<string, string>;
    readonly ambiguousUnitList: ReadonlyArray<string>;
    readonly extractType: string;
    readonly ambiguousUnitNumberMultiplierRegex: RegExp;

    constructor(ci?: CultureInfo) {
        if (!ci) {
            ci = new CultureInfo(Culture.French);
        }

        super(ci);

        this.extractType = Constants.SYS_UNIT_TEMPERATURE;

        this.suffixList = FrenchNumericWithUnit.TemperatureSuffixList;
        this.prefixList = new Map<string, string>();
        this.ambiguousUnitList = new Array<string>();

        this.ambiguousUnitNumberMultiplierRegex = RegExpUtility.getSafeRegExp(BaseUnits.AmbiguousUnitNumberMultiplierRegex, "gs");
    }
}

export class FrenchTemperatureParserConfiguration extends FrenchNumberWithUnitParserConfiguration {
    constructor(ci?: CultureInfo) {
        if(!ci) {
            ci = new CultureInfo(Culture.French);
        }

        super(ci);

        this.connectorToken = null;
        this.BindDictionary(FrenchNumericWithUnit.TemperatureSuffixList);
    }

    readonly connectorToken: string;
}