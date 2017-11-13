import { CultureInfo, Culture } from "recognizers-text-number";
import { Constants } from "../constants";
import { FrenchNumberWithUnitExtractorConfiguration, FrenchNumberWithUnitParserConfiguration } from "./base";
import { FrenchNumericWithUnit } from "../../resources/frenchNumericWithUnit";

const dimensionSuffixList = new Map<string, string>([
    ...FrenchNumericWithUnit.InformationSuffixList,
    ...FrenchNumericWithUnit.AreaSuffixList,
    ...FrenchNumericWithUnit.LengthSuffixList,
    ...FrenchNumericWithUnit.SpeedSuffixList,
    ...FrenchNumericWithUnit.VolumeSuffixList,
    ...FrenchNumericWithUnit.WeightSuffixList
]);

export class FrenchDimensionExtractorConfiguration extends FrenchNumberWithUnitExtractorConfiguration {

    readonly suffixList: ReadonlyMap<string, string>;
    readonly prefixList: ReadonlyMap<string, string>;
    readonly ambiguousUnitList: ReadonlyArray<string>;
    readonly extractType: string;

    constructor(ci?: CultureInfo) {
        if (!ci) {
            ci = new CultureInfo(Culture.French);
        }

        super(ci);

        this.extractType = Constants.SYS_UNIT_DIMENSION;

        this.suffixList = dimensionSuffixList;
        this.prefixList = new Map<string, string>();
        this.ambiguousUnitList = FrenchNumericWithUnit.AmbiguousDimensionUnitList
    }
}

export class FrenchDimensionParserConfiguration extends FrenchNumberWithUnitParserConfiguration {
    constructor(ci?: CultureInfo) {
        if (!ci) {
            ci = new CultureInfo(Culture.French);
        }

        super(ci);

        this.BindDictionary(dimensionSuffixList);
    }
}