// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

import { CultureInfo, Culture } from "@microsoft/recognizers-text-number";
import { Constants } from "../constants";
import { PortugueseNumberWithUnitExtractorConfiguration, PortugueseNumberWithUnitParserConfiguration } from "./base";
import { PortugueseNumericWithUnit } from "../../resources/portugueseNumericWithUnit";

const dimensionSuffixList = new Map<string, string>([
    ...PortugueseNumericWithUnit.InformationSuffixList,
    ...PortugueseNumericWithUnit.AreaSuffixList,
    ...PortugueseNumericWithUnit.LengthSuffixList,
    ...PortugueseNumericWithUnit.SpeedSuffixList,
    ...PortugueseNumericWithUnit.VolumeSuffixList,
    ...PortugueseNumericWithUnit.WeightSuffixList
]);

export class PortugueseDimensionExtractorConfiguration extends PortugueseNumberWithUnitExtractorConfiguration {

    readonly suffixList: ReadonlyMap<string, string>;
    readonly prefixList: ReadonlyMap<string, string>;
    readonly ambiguousUnitList: readonly string[];
    readonly extractType: string;

    constructor(ci?: CultureInfo) {
        if (!ci) {
            ci = new CultureInfo(Culture.Portuguese);
        }

        super(ci);

        this.extractType = Constants.SYS_UNIT_DIMENSION;

        this.suffixList = dimensionSuffixList;
        this.prefixList = new Map<string, string>();
        this.ambiguousUnitList = PortugueseNumericWithUnit.AmbiguousDimensionUnitList;
    }
}

export class PortugueseDimensionParserConfiguration extends PortugueseNumberWithUnitParserConfiguration {
    constructor(ci?: CultureInfo) {
        if (!ci) {
            ci = new CultureInfo(Culture.Portuguese);
        }

        super(ci);

        this.BindDictionary(dimensionSuffixList);
    }
}