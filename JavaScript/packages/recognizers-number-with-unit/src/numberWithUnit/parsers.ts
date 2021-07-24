// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

import { IExtractor, ExtractResult, IParser, ParseResult, StringUtility } from "@microsoft/recognizers-text";
import { CultureInfo, Constants as NumberConstants } from "@microsoft/recognizers-text-number";
import last = require("lodash.last");
import { Constants } from "./constants";
import { DictionaryUtils } from "./utilities";
import { BaseCurrency } from "../resources/baseCurrency";

export class UnitValue {
    public number: string = "";
    public unit: string = "";
}

export class UnitValueIso extends UnitValue {
    public isoCurrency: string = "";
}

export interface INumberWithUnitParserConfiguration {
    readonly unitMap: Map<string, string>;
    readonly cultureInfo: CultureInfo;
    readonly internalNumberParser: IParser;
    readonly internalNumberExtractor: IExtractor;
    readonly connectorToken: string;
    readonly currencyNameToIsoCodeMap: ReadonlyMap<string, string>;
    readonly currencyFractionCodeList: ReadonlyMap<string, string>;
    readonly currencyFractionNumMap: ReadonlyMap<string, number>;
    readonly currencyFractionMapping: ReadonlyMap<string, string>;
    BindDictionary(dictionary: Map<string, string>): void;
}

export abstract class BaseNumberWithUnitParserConfiguration implements INumberWithUnitParserConfiguration {
    unitMap: Map<string, string>;
    cultureInfo: CultureInfo;
    abstract internalNumberParser: IParser;
    abstract internalNumberExtractor: IExtractor;
    abstract connectorToken: string;
    abstract currencyNameToIsoCodeMap: ReadonlyMap<string, string>;
    abstract currencyFractionCodeList: ReadonlyMap<string, string>;
    readonly currencyFractionNumMap: ReadonlyMap<string, number>;
    readonly currencyFractionMapping: ReadonlyMap<string, string>;

    constructor(cultureInfo: CultureInfo) {
        this.cultureInfo = cultureInfo;
        this.unitMap = new Map<string, string>();
        this.currencyFractionNumMap = BaseCurrency.CurrencyFractionalRatios;
        this.currencyFractionMapping = BaseCurrency.CurrencyFractionMapping;
    }

    BindDictionary(dictionary: ReadonlyMap<string, string>): void {
        DictionaryUtils.bindDictionary(dictionary, this.unitMap);
    }
}

export class NumberWithUnitParser implements IParser {
    protected readonly config: INumberWithUnitParserConfiguration;

    constructor(config: INumberWithUnitParserConfiguration) {
        this.config = config;
    }

    parse(extResult: ExtractResult): ParseResult {
        let ret = new ParseResult(extResult);
        let numberResult: ExtractResult;
        let halfResult: ExtractResult;
        if (extResult.data && extResult.data instanceof Array && extResult.data.length === 2) {
            numberResult = extResult.data[0];
            halfResult = extResult.data[1];
        }
        else if (extResult.data && typeof extResult.data === "object") {
            numberResult = extResult.data as ExtractResult;
            halfResult = null;
        }
        else if (extResult.type === Constants.SYS_NUM) {
            ret.value = this.config.internalNumberParser.parse(extResult).value;
            return ret;
        }
        else {
            // if there is no unitResult, means there is just unit
            numberResult = { start: -1, length: 0, text: null, type: null };
            halfResult = null;
        }

        // key contains units
        let key = extResult.text;
        let unitKeyBuild = '';
        let unitKeys = new Array<string>();
        for (let i = 0; i <= key.length; i++) {
            if (i === key.length) {
                if (unitKeyBuild.length !== 0) {
                    this.addIfNotContained(unitKeys, unitKeyBuild.trim());
                }
            }

            // numberResult.start is a relative position
            else if (i === numberResult.start) {
                if (unitKeyBuild.length !== 0) {
                    this.addIfNotContained(unitKeys, unitKeyBuild.trim());
                    unitKeyBuild = '';
                }

                let o = numberResult.start + numberResult.length - 1;
                if (o !== null && !isNaN(o)) {
                    i = o;
                }
            }
            else {
                unitKeyBuild += key[i];
            }
        }

        /* Unit type depends on last unit in suffix.*/
        let lastUnit = last(unitKeys);
        if (halfResult != null) {
            lastUnit = lastUnit.substring(0, lastUnit.length - halfResult.text.length);
        }
        let normalizedLastUnit = lastUnit.toLowerCase();
        if (this.config.connectorToken && this.config.connectorToken.length && normalizedLastUnit.indexOf(this.config.connectorToken) === 0) {
            normalizedLastUnit = normalizedLastUnit.substring(this.config.connectorToken.length).trim();
            lastUnit = lastUnit.substring(this.config.connectorToken.length).trim();
        }

        if (key && key.length && (this.config.unitMap !== null)) {
            let unitValue = null;
            if (this.config.unitMap.has(lastUnit)) {
                unitValue = this.config.unitMap.get(lastUnit);
            }
            else if (this.config.unitMap.has(normalizedLastUnit)) {
                unitValue = this.config.unitMap.get(normalizedLastUnit);
            }

            if (unitValue) {
                let numValue = numberResult.text && numberResult.text.length ? this.config.internalNumberParser.parse(numberResult) : null;
                let resolutionStr = numValue ? numValue.resolutionStr : null;
                if (halfResult != null) {
                    let halfValue = this.config.internalNumberParser.parse(halfResult);
                    resolutionStr += halfValue.resolutionStr.substring(1);
                }
                ret.value = { number: resolutionStr, unit: unitValue } as UnitValue;
                ret.resolutionStr = (`${resolutionStr} ${unitValue}`).trim();
            }
        }

        ret.text = ret.text.toLowerCase();

        return ret;
    }

    private addIfNotContained(keys: string[], newKey: string): void {
        if (!keys.some(key => key.includes(newKey))) {
            keys.push(newKey);
        }
    }

}

export class BaseCurrencyParser implements IParser {
    protected readonly config: INumberWithUnitParserConfiguration;
    private readonly numberWithUnitParser: NumberWithUnitParser;

    constructor(config: INumberWithUnitParserConfiguration) {
        this.config = config;
        this.numberWithUnitParser = new NumberWithUnitParser(config);
    }

    public parse(extResult: ExtractResult): ParseResult {
        let result: ParseResult = null;

        if (extResult.data instanceof Array) {
            result = this.mergeCompoundUnit(extResult);
        }
        else {
            result = this.numberWithUnitParser.parse(extResult);
            let value: UnitValue = result.value;
            if (!this.config.currencyNameToIsoCodeMap.has(value.unit) || this.config.currencyNameToIsoCodeMap.get(value.unit).startsWith(Constants.FAKE_ISO_CODE_PREFIX)) {
                result.value = {
                    unit: value.unit,
                    number: value.number
                } as UnitValue;
            }
            else {
                result.value = {
                    unit: value.unit,
                    number: value.number,
                    isoCurrency: this.config.currencyNameToIsoCodeMap.get(value.unit)
                } as UnitValueIso;
            }
        }
        return result;
    }

    private mergeCompoundUnit(compoundResult: ExtractResult): ParseResult {
        let results: ParseResult[] = [];
        let compoundUnit: ParseResult[] = compoundResult.data;

        let count = 0;
        let result: ParseResult = null;
        let numberValue = null;
        let mainUnitValue = '';
        let mainUnitIsoCode = '';
        let fractionUnitsString = '';

        for (let i = 0; i < compoundUnit.length; i++) {
            let extractResult = compoundUnit[i];
            let parseResult = this.numberWithUnitParser.parse(extractResult);
            let parseResultValue: UnitValue = parseResult.value;
            let unitValue = parseResultValue != null ? parseResultValue.unit : null;

            // Process a new group
            if (count === 0) {
                if (extractResult.type !== Constants.SYS_UNIT_CURRENCY) {
                    continue;
                }

                // Initialize a new result
                result = new ParseResult(extractResult);

                mainUnitValue = unitValue;
                if (parseResultValue.number != null) {
                    numberValue = parseFloat(parseResultValue.number);
                }
                result.resolutionStr = parseResult.resolutionStr;

                if (this.config.currencyNameToIsoCodeMap.has(unitValue)) {
                    mainUnitIsoCode = this.config.currencyNameToIsoCodeMap.get(unitValue);
                }

                // If the main unit can't be recognized, finish process this group.
                if (StringUtility.isNullOrEmpty(mainUnitIsoCode)) {
                    result.value = {
                        number: numberValue.toString(),
                        unit: mainUnitValue
                    } as UnitValue;

                    results.push(result);
                    result = null;
                    continue;
                }

                if (this.config.currencyFractionMapping.has(mainUnitIsoCode)) {
                    fractionUnitsString = this.config.currencyFractionMapping.get(mainUnitIsoCode);
                }
            }
            else {
                // Match pure number as fraction unit.
                if (extractResult.type === NumberConstants.SYS_NUM) {
                    numberValue += parseResult.value * (1.0 / 100);
                    result.resolutionStr += ' ' + parseResult.resolutionStr;
                    result.length = parseResult.start + parseResult.length - result.start;
                    count++;
                    continue;
                }

                let fractionUnitCode: string;
                let fractionNumValue: number;

                if (this.config.currencyFractionCodeList.has(unitValue)) {
                    fractionUnitCode = this.config.currencyFractionCodeList.get(unitValue);
                }

                if (this.config.currencyFractionNumMap.has(unitValue)) {
                    fractionNumValue = this.config.currencyFractionNumMap.get(unitValue);
                }

                if (fractionUnitCode && fractionNumValue !== 0 && this.checkUnitsStringContains(fractionUnitCode, fractionUnitsString)) {
                    numberValue += parseFloat(parseResultValue.number) * (1.0 / fractionNumValue);
                    result.resolutionStr += ' ' + parseResult.resolutionStr;
                    result.length = parseResult.start + parseResult.length - result.start;
                }
                else {
                    // If the fraction unit doesn't match the main unit, finish process this group.
                    if (result !== null) {
                        result = this.createCurrencyResult(result, mainUnitIsoCode, numberValue, mainUnitValue);
                        results.push(result);
                        result = null;
                    }

                    count = 0;
                    i -= 1;
                    numberValue = null;
                    continue;
                }
            }

            count++;
        }

        if (result !== null) {
            result = this.createCurrencyResult(result, mainUnitIsoCode, numberValue, mainUnitValue);
            results.push(result);
        }

        this.resolveText(results, compoundResult.text, compoundResult.start);

        return { value: results } as ParseResult;
    }

    private checkUnitsStringContains(fractionUnitCode: string, fractionUnitsString: string): boolean {
        let unitsMap = new Map<string, string>();
        DictionaryUtils.bindUnitsString(unitsMap, '', fractionUnitsString);

        return unitsMap.has(fractionUnitCode);
    }

    private resolveText(prs: ParseResult[], source: string, bias: number): void {
        prs.forEach(parseResult => {
            if (parseResult.start !== null && parseResult.length !== null) {
                parseResult.text = source.substr(parseResult.start - bias, parseResult.length);
            }
        });
    }

    private createCurrencyResult(result: ParseResult, mainUnitIsoCode: string, numberValue: number, mainUnitValue: string): ParseResult{
        if (StringUtility.isNullOrEmpty(mainUnitIsoCode) || mainUnitIsoCode.startsWith(Constants.FAKE_ISO_CODE_PREFIX)) {
            result.value = {
                number: numberValue ? numberValue.toString() : 'null',
                unit: mainUnitValue
            } as UnitValue;
        }
        else {
            result.value = {
                number: numberValue ? numberValue.toString() : 'null',
                unit: mainUnitValue,
                isoCurrency: mainUnitIsoCode
            } as UnitValueIso;
        }

        return result;
    }
}

export class BaseMergedUnitParser implements IParser {
    protected readonly config: INumberWithUnitParserConfiguration;
    private readonly numberWithUnitParser: NumberWithUnitParser;
    private readonly currencyParser: BaseCurrencyParser;

    constructor(config: INumberWithUnitParserConfiguration) {
        this.config = config;
        this.numberWithUnitParser = new NumberWithUnitParser(config);
        this.currencyParser = new BaseCurrencyParser(config);
    }

    parse(extResult: ExtractResult): ParseResult {
        let result: ParseResult;

        if (extResult.type === Constants.SYS_UNIT_CURRENCY) {
            result = this.currencyParser.parse(extResult);
        }
        else {
            result = this.numberWithUnitParser.parse(extResult);
        }

        return result;
    }
}