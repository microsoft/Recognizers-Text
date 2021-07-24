// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

import { ExtractResult, IExtractor, IParser, RegExpUtility } from "@microsoft/recognizers-text";
import { Culture, CultureInfo, NumberMode, AgnosticNumberParserFactory, AgnosticNumberParserType, SpanishNumberExtractor, SpanishNumberParserConfiguration } from "@microsoft/recognizers-text-number";
import { Constants } from "../constants";
import { INumberWithUnitExtractorConfiguration } from "../extractors";
import { BaseNumberWithUnitParserConfiguration } from "../parsers";
import { SpanishNumericWithUnit } from "../../resources/spanishNumericWithUnit";
import { BaseUnits } from "../../resources/baseUnits";

export abstract class SpanishNumberWithUnitExtractorConfiguration implements INumberWithUnitExtractorConfiguration {
    abstract readonly suffixList: ReadonlyMap<string, string>;
    abstract readonly prefixList: ReadonlyMap<string, string>;
    abstract readonly ambiguousUnitList: readonly string[];
    readonly abstract extractType: string;

    readonly cultureInfo: CultureInfo;
    readonly unitNumExtractor: IExtractor;
    readonly buildPrefix: string;
    readonly buildSuffix: string;
    readonly connectorToken: string;
    readonly compoundUnitConnectorRegex: RegExp;
    readonly nonUnitRegex: RegExp;
    readonly ambiguousUnitNumberMultiplierRegex: RegExp;

    constructor(ci: CultureInfo) {
        this.cultureInfo = ci;
        this.unitNumExtractor = new SpanishNumberExtractor(NumberMode.Unit);

        this.buildPrefix = SpanishNumericWithUnit.BuildPrefix;
        this.buildSuffix = SpanishNumericWithUnit.BuildSuffix;
        this.connectorToken = SpanishNumericWithUnit.ConnectorToken;
        this.compoundUnitConnectorRegex = RegExpUtility.getSafeRegExp(SpanishNumericWithUnit.CompoundUnitConnectorRegex);
        this.nonUnitRegex = RegExpUtility.getSafeRegExp(BaseUnits.PmNonUnitRegex);
    }

    expandHalfSuffix(source: string, result: ExtractResult[], numbers: ExtractResult[]) {
    }
}

export class SpanishNumberWithUnitParserConfiguration extends BaseNumberWithUnitParserConfiguration {
    readonly internalNumberParser: IParser;
    readonly internalNumberExtractor: IExtractor;
    readonly connectorToken: string;
    readonly currencyNameToIsoCodeMap: ReadonlyMap<string, string>;
    readonly currencyFractionCodeList: ReadonlyMap<string, string>;

    constructor(ci: CultureInfo) {
        super(ci);

        this.internalNumberExtractor = new SpanishNumberExtractor(NumberMode.Default);
        this.internalNumberParser = AgnosticNumberParserFactory.getParser(AgnosticNumberParserType.Number, new SpanishNumberParserConfiguration());
        this.connectorToken = SpanishNumericWithUnit.ConnectorToken;
    }
}