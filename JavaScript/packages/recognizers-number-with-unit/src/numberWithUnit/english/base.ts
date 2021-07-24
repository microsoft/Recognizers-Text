// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

import { ExtractResult, IExtractor, IParser, RegExpUtility } from "@microsoft/recognizers-text";
import { Culture, CultureInfo, NumberMode, AgnosticNumberParserFactory, AgnosticNumberParserType, EnglishNumberExtractor, EnglishNumberParserConfiguration } from "@microsoft/recognizers-text-number";
import { Constants } from "../constants";
import { INumberWithUnitExtractorConfiguration } from "../extractors";
import { BaseNumberWithUnitParserConfiguration } from "../parsers";
import { EnglishNumericWithUnit } from "../../resources/englishNumericWithUnit";
import { BaseUnits } from "../../resources/baseUnits";

export abstract class EnglishNumberWithUnitExtractorConfiguration implements INumberWithUnitExtractorConfiguration {
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
        this.unitNumExtractor = new EnglishNumberExtractor(NumberMode.Unit);

        this.buildPrefix = EnglishNumericWithUnit.BuildPrefix;
        this.buildSuffix = EnglishNumericWithUnit.BuildSuffix;
        this.connectorToken = '';
        this.compoundUnitConnectorRegex = RegExpUtility.getSafeRegExp(EnglishNumericWithUnit.CompoundUnitConnectorRegex);
        this.nonUnitRegex = RegExpUtility.getSafeRegExp(BaseUnits.PmNonUnitRegex);
    }

    expandHalfSuffix(source: string, result: ExtractResult[], numbers: ExtractResult[]) {
    }
}

export class EnglishNumberWithUnitParserConfiguration extends BaseNumberWithUnitParserConfiguration {
    readonly internalNumberParser: IParser;
    readonly internalNumberExtractor: IExtractor;
    readonly connectorToken: string;
    readonly currencyNameToIsoCodeMap: ReadonlyMap<string, string>;
    readonly currencyFractionCodeList: ReadonlyMap<string, string>;

    constructor(ci: CultureInfo) {
        super(ci);

        this.internalNumberExtractor = new EnglishNumberExtractor(NumberMode.Default);
        this.internalNumberParser = AgnosticNumberParserFactory.getParser(AgnosticNumberParserType.Number, new EnglishNumberParserConfiguration());
        this.connectorToken = '';
        this.currencyNameToIsoCodeMap = EnglishNumericWithUnit.CurrencyNameToIsoCodeMap;
        this.currencyFractionCodeList = EnglishNumericWithUnit.FractionalUnitNameToCodeMap;
    }
}