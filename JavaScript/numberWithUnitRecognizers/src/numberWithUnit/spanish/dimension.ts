import { CultureInfo, Culture } from "recognizers-text-number";
import { Constants } from "../constants";
import { SpanishNumberWithUnitExtractorConfiguration, SpanishNumberWithUnitParserConfiguration } from "./base";
import { SpanishNumericWithUnit } from "../../resources/spanishNumericWithUnit";

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

        this.suffixList = SpanishNumericWithUnit.DimensionSuffixList;
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

        this.BindDictionary(SpanishNumericWithUnit.DimensionSuffixList);
    }
}