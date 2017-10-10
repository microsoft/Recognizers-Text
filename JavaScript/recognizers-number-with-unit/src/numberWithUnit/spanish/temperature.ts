import { CultureInfo, Culture } from "recognizers-text-number";
import { Constants } from "../constants";
import { SpanishNumberWithUnitExtractorConfiguration, SpanishNumberWithUnitParserConfiguration } from "./base";
import { SpanishNumericWithUnit } from "../../resources/spanishNumericWithUnit";

export class SpanishTemperatureExtractorConfiguration extends SpanishNumberWithUnitExtractorConfiguration {
    readonly suffixList: ReadonlyMap<string, string>;
    readonly prefixList: ReadonlyMap<string, string>;
    readonly ambiguousUnitList: ReadonlyArray<string>;
    readonly extractType: string;

    constructor(ci?: CultureInfo) {
        if (!ci) {
            ci = new CultureInfo(Culture.Spanish);
        }

        super(ci);

        this.extractType = Constants.SYS_UNIT_TEMPERATURE;

        this.suffixList = SpanishNumericWithUnit.TemperatureSuffixList;
        this.prefixList = new Map<string, string>();
        this.ambiguousUnitList = new Array<string>();
    }
}

export class SpanishTemperatureParserConfiguration extends SpanishNumberWithUnitParserConfiguration {
    constructor(ci?: CultureInfo) {
        if(!ci) {
            ci = new CultureInfo(Culture.Spanish);
        }

        super(ci);

        this.BindDictionary(SpanishNumericWithUnit.TemperatureSuffixList);
    }
}