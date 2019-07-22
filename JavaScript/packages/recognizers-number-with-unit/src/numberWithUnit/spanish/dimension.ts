import { CultureInfo, Culture } from "@microsoft/recognizers-text-number";
import { Constants } from "../constants";
import { SpanishNumberWithUnitExtractorConfiguration, SpanishNumberWithUnitParserConfiguration } from "./base";
import { SpanishNumericWithUnit } from "../../resources/spanishNumericWithUnit";

const dimensionSuffixList = new Map<string, string>([
    ...SpanishNumericWithUnit.InformationSuffixList,
    ...SpanishNumericWithUnit.AreaSuffixList,
    ...SpanishNumericWithUnit.LengthSuffixList,
    ...SpanishNumericWithUnit.SpeedSuffixList,
    ...SpanishNumericWithUnit.VolumeSuffixList,
    ...SpanishNumericWithUnit.WeightSuffixList
]);


export class SpanishDimensionExtractorConfiguration extends SpanishNumberWithUnitExtractorConfiguration {

    readonly suffixList: ReadonlyMap<string, string>;
    readonly prefixList: ReadonlyMap<string, string>;
    readonly ambiguousUnitList: ReadonlyArray<string>;
    readonly extractType: string;

    constructor(ci?: CultureInfo) {
        if (!ci) {
            ci = new CultureInfo(Culture.Spanish);
        }

        super(ci);

        this.extractType = Constants.SYS_UNIT_DIMENSION;

        this.suffixList = dimensionSuffixList;
        this.prefixList = new Map<string, string>();
        this.ambiguousUnitList = SpanishNumericWithUnit.AmbiguousDimensionUnitList
    }
}

export class SpanishDimensionParserConfiguration extends SpanishNumberWithUnitParserConfiguration {
    constructor(ci?: CultureInfo) {
        if (!ci) {
            ci = new CultureInfo(Culture.Spanish);
        }

        super(ci);

        this.BindDictionary(dimensionSuffixList);
    }
}