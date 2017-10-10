import { CultureInfo, Culture } from "recognizers-text-number";
import { Constants } from "../constants";
import { EnglishNumberWithUnitExtractorConfiguration, EnglishNumberWithUnitParserConfiguration } from "./base";
import { EnglishNumericWithUnit } from "../../resources/englishNumericWithUnit";

const dimensionSuffixList = new Map<string, string>([
    ...EnglishNumericWithUnit.InformationSuffixList,
    ...EnglishNumericWithUnit.AreaSuffixList,
    ...EnglishNumericWithUnit.LenghtSuffixList,
    ...EnglishNumericWithUnit.SpeedSuffixList,
    ...EnglishNumericWithUnit.VolumeSuffixList,
    ...EnglishNumericWithUnit.WeightSuffixList
]);

export class EnglishDimensionExtractorConfiguration extends EnglishNumberWithUnitExtractorConfiguration {

    readonly suffixList: ReadonlyMap<string, string>;
    readonly prefixList: ReadonlyMap<string, string>;
    readonly ambiguousUnitList: ReadonlyArray<string>;
    readonly extractType: string;

    constructor(ci?: CultureInfo) {
        if (!ci) {
            ci = new CultureInfo(Culture.English);
        }

        super(ci);

        this.extractType = Constants.SYS_UNIT_DIMENSION;

        this.suffixList = dimensionSuffixList;
        this.prefixList = new Map<string, string>();
        this.ambiguousUnitList = EnglishNumericWithUnit.AmbiguousDimensionUnitList
    }
}

export class EnglishDimensionParserConfiguration extends EnglishNumberWithUnitParserConfiguration {
    constructor(ci?: CultureInfo) {
        if (!ci) {
            ci = new CultureInfo(Culture.English);
        }

        super(ci);

        this.BindDictionary(dimensionSuffixList);
    }
}