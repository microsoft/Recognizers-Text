// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

import { CultureInfo, Culture } from "@microsoft/recognizers-text-number";
import { Constants } from "../constants";
import { SpanishNumberWithUnitExtractorConfiguration, SpanishNumberWithUnitParserConfiguration } from "./base";
import { SpanishNumericWithUnit } from "../../resources/spanishNumericWithUnit";

export class SpanishAgeExtractorConfiguration extends SpanishNumberWithUnitExtractorConfiguration {
    readonly suffixList: ReadonlyMap<string, string>;
    readonly prefixList: ReadonlyMap<string, string>;
    readonly ambiguousUnitList: readonly string[];
    readonly extractType: string;

    constructor(ci?: CultureInfo) {
        if (!ci) {
            ci = new CultureInfo(Culture.Spanish);
        }

        super(ci);

        this.extractType = Constants.SYS_UNIT_AGE;

        this.suffixList = SpanishNumericWithUnit.AgeSuffixList;
        this.prefixList = new Map<string, string>();
        this.ambiguousUnitList = SpanishNumericWithUnit.AmbiguousAgeUnitList;
    }
}

export class SpanishAgeParserConfiguration extends SpanishNumberWithUnitParserConfiguration {
    constructor(ci?: CultureInfo) {
        if (!ci) {
            ci = new CultureInfo(Culture.Spanish);
        }

        super(ci);

        this.BindDictionary(SpanishNumericWithUnit.AgeSuffixList);
    }
}