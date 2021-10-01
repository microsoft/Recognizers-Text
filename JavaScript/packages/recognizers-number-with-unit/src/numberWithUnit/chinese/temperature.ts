// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

import { CultureInfo, Culture, RegExpUtility } from "@microsoft/recognizers-text-number";
import { Constants } from "../constants";
import { ChineseNumberWithUnitExtractorConfiguration, ChineseNumberWithUnitParserConfiguration } from "./base";
import { BaseUnits } from "../../resources/baseUnits";
import { ChineseNumericWithUnit } from "../../resources/chineseNumericWithUnit";

export class ChineseTemperatureExtractorConfiguration extends ChineseNumberWithUnitExtractorConfiguration {
    readonly suffixList: ReadonlyMap<string, string>;
    readonly prefixList: ReadonlyMap<string, string>;
    readonly ambiguousUnitList: readonly string[];
    readonly extractType: string;
    readonly ambiguousUnitNumberMultiplierRegex: RegExp;

    constructor(ci?: CultureInfo) {
        if (!ci) {
            ci = new CultureInfo(Culture.Chinese);
        }

        super(ci);

        this.extractType = Constants.SYS_UNIT_TEMPERATURE;

        this.suffixList = ChineseNumericWithUnit.TemperatureSuffixList;
        this.prefixList = ChineseNumericWithUnit.TemperaturePrefixList;
        this.ambiguousUnitList = ChineseNumericWithUnit.TemperatureAmbiguousValues;

        this.ambiguousUnitNumberMultiplierRegex = RegExpUtility.getSafeRegExp(BaseUnits.AmbiguousUnitNumberMultiplierRegex, "gs");
    }
}

export class ChineseTemperatureParserConfiguration extends ChineseNumberWithUnitParserConfiguration {
    constructor(ci?: CultureInfo) {
        if (!ci) {
            ci = new CultureInfo(Culture.Chinese);
        }

        super(ci);

        this.BindDictionary(ChineseNumericWithUnit.TemperaturePrefixList);
        this.BindDictionary(ChineseNumericWithUnit.TemperatureSuffixList);
    }
}