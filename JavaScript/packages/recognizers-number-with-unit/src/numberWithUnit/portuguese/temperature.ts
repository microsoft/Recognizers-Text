import { CultureInfo, Culture, RegExpUtility } from "@microsoft/recognizers-text-number";
import { Constants } from "../constants";
import { PortugueseNumberWithUnitExtractorConfiguration, PortugueseNumberWithUnitParserConfiguration } from "./base";
import { PortugueseNumericWithUnit } from "../../resources/portugueseNumericWithUnit";
import { BaseUnits } from "../../resources/baseUnits";

export class PortugueseTemperatureExtractorConfiguration extends PortugueseNumberWithUnitExtractorConfiguration {
    readonly suffixList: ReadonlyMap<string, string>;
    readonly prefixList: ReadonlyMap<string, string>;
    readonly ambiguousUnitList: ReadonlyArray<string>;
    readonly extractType: string;
    readonly ambiguousUnitNumberMultiplierRegex: RegExp;

    constructor(ci?: CultureInfo) {
        if (!ci) {
            ci = new CultureInfo(Culture.Portuguese);
        }

        super(ci);

        this.extractType = Constants.SYS_UNIT_TEMPERATURE;

        this.suffixList = PortugueseNumericWithUnit.TemperatureSuffixList;
        this.prefixList = new Map<string, string>();
        this.ambiguousUnitList = new Array<string>();

        this.ambiguousUnitNumberMultiplierRegex = RegExpUtility.getSafeRegExp(BaseUnits.AmbiguousUnitNumberMultiplierRegex, "gs");
    }
}

export class PortugueseTemperatureParserConfiguration extends PortugueseNumberWithUnitParserConfiguration {
    constructor(ci?: CultureInfo) {
        if(!ci) {
            ci = new CultureInfo(Culture.Portuguese);
        }

        super(ci);

        this.BindDictionary(PortugueseNumericWithUnit.TemperatureSuffixList);
    }
}