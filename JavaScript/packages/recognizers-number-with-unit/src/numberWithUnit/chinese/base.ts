// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

import { ExtractResult, IExtractor, IParser, RegExpUtility } from "@microsoft/recognizers-text";
import { Culture, CultureInfo, NumberMode, AgnosticNumberParserFactory, AgnosticNumberParserType, ChineseNumberExtractor, ChineseNumberParserConfiguration, ChineseNumberExtractorMode } from "@microsoft/recognizers-text-number";
import { Constants } from "../constants";
import { INumberWithUnitExtractorConfiguration } from "../extractors";
import { BaseNumberWithUnitParserConfiguration } from "../parsers";
import { ChineseNumericWithUnit } from "../../resources/chineseNumericWithUnit";
import { BaseUnits } from "../../resources/baseUnits";

export abstract class ChineseNumberWithUnitExtractorConfiguration implements INumberWithUnitExtractorConfiguration {
    private readonly halfUnitRegex = RegExpUtility.getSafeRegExp(ChineseNumericWithUnit.HalfUnitRegex);

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
        this.unitNumExtractor = new ChineseNumberExtractor(ChineseNumberExtractorMode.ExtractAll);

        this.buildPrefix = ChineseNumericWithUnit.BuildPrefix;
        this.buildSuffix = ChineseNumericWithUnit.BuildSuffix;
        this.connectorToken = ChineseNumericWithUnit.ConnectorToken;
        this.compoundUnitConnectorRegex = RegExpUtility.getSafeRegExp(ChineseNumericWithUnit.CompoundUnitConnectorRegex);
        this.nonUnitRegex = RegExpUtility.getSafeRegExp(BaseUnits.PmNonUnitRegex);
    }

    expandHalfSuffix(source: string, result: ExtractResult[], numbers: ExtractResult[]) {
        // expand Chinese phrase to the `half` patterns when it follows closely origin phrase.
        if (this.halfUnitRegex != null){
            let match = new Array<ExtractResult>();
            for (let number of numbers) {
                if (RegExpUtility.getMatches(this.halfUnitRegex, number.text).length > 0){
                    match.push(number);
                }
            }
            if (match.length > 0){
                let res = new Array<ExtractResult>();
                for (let er of result){
                    let start = er.start;
                    let length = er.length;
                    let matchSuffix = new Array<ExtractResult>();
                    for (let mr of match){
                        if (mr.start == start + length){
                            matchSuffix.push(mr);
                        }
                    }
                    if (matchSuffix.length === 1){
                        let mr = matchSuffix[0];
                        er.length += mr.length;
                        er.text += mr.text;
                        let tmp = new Array<ExtractResult>();
                        tmp.push(er.data);
                        tmp.push(mr);
                        er.data = tmp;
                    }
                    res.push(er);
                }
                result = res;
            }
        }
    }
}

export class ChineseNumberWithUnitParserConfiguration extends BaseNumberWithUnitParserConfiguration {
    readonly internalNumberParser: IParser;
    readonly internalNumberExtractor: IExtractor;
    readonly connectorToken: string;
    readonly currencyNameToIsoCodeMap: ReadonlyMap<string, string>;
    readonly currencyFractionCodeList: ReadonlyMap<string, string>;

    constructor(ci: CultureInfo) {
        super(ci);

        this.internalNumberExtractor = new ChineseNumberExtractor(ChineseNumberExtractorMode.Default);
        this.internalNumberParser = AgnosticNumberParserFactory.getParser(AgnosticNumberParserType.Number, new ChineseNumberParserConfiguration());
        this.connectorToken = '';
        this.currencyNameToIsoCodeMap = ChineseNumericWithUnit.CurrencyNameToIsoCodeMap;
        this.currencyFractionCodeList = ChineseNumericWithUnit.FractionalUnitNameToCodeMap;
    }
}