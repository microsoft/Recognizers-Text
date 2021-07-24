// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

import { CultureInfo, Culture, RegExpUtility } from "@microsoft/recognizers-text-number";
import { Constants } from "../constants";
import { EnglishNumberWithUnitExtractorConfiguration, EnglishNumberWithUnitParserConfiguration } from "./base";
import { BaseUnits } from "../../resources/baseUnits";
import { EnglishNumericWithUnit } from "../../resources/englishNumericWithUnit";

export class EnglishTemperatureExtractorConfiguration extends EnglishNumberWithUnitExtractorConfiguration {
    readonly suffixList: ReadonlyMap<string, string>;
    readonly prefixList: ReadonlyMap<string, string>;
    readonly ambiguousUnitList: readonly string[];
    readonly extractType: string;
    readonly ambiguousUnitNumberMultiplierRegex: RegExp;

    constructor(ci?: CultureInfo) {
        if (!ci) {
            ci = new CultureInfo(Culture.English);
        }

        super(ci);

        this.extractType = Constants.SYS_UNIT_TEMPERATURE;

        this.suffixList = EnglishNumericWithUnit.TemperatureSuffixList;
        this.prefixList = new Map<string, string>();
        this.ambiguousUnitList = EnglishNumericWithUnit.AmbiguousTemperatureUnitList;

        this.ambiguousUnitNumberMultiplierRegex = RegExpUtility.getSafeRegExp(BaseUnits.AmbiguousUnitNumberMultiplierRegex, "gs");
    }
}

export class EnglishTemperatureParserConfiguration extends EnglishNumberWithUnitParserConfiguration {
    constructor(ci?: CultureInfo) {
        if (!ci) {
            ci = new CultureInfo(Culture.English);
        }

        super(ci);

        this.BindDictionary(EnglishNumericWithUnit.TemperatureSuffixList);
    }
}