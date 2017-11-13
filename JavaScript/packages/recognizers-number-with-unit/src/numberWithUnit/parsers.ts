import { CultureInfo, IParser, ParseResult, IExtractor, ExtractResult } from "recognizers-text-number";
import last = require("lodash.last");

export class UnitValue {
    public number: string = "";
    public unit: string = "";
}

export class NumberWithUnitParser implements IParser {
    protected readonly config: INumberWithUnitParserConfiguration;

    constructor(config: INumberWithUnitParserConfiguration) {
        this.config = config;
    }

    parse(extResult: ExtractResult): ParseResult {
        let ret = new ParseResult(extResult);
        let numberResult: ExtractResult;
        if (extResult.data && typeof extResult.data === "object") {
            numberResult = extResult.data as ExtractResult;
        }
        else // if there is no unitResult, means there is just unit
        {
            numberResult = { start: -1, length: 0, text: null, type: null };
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

        let lastUnit = last(unitKeys).toLowerCase();
        if (this.config.connectorToken && this.config.connectorToken.length && lastUnit.indexOf(this.config.connectorToken) === 0) {
            lastUnit = lastUnit.substring(this.config.connectorToken.length).trim();
        }
        if (key && key.length && (this.config.unitMap !== null) && this.config.unitMap.has(lastUnit)) {
            let unitValue = this.config.unitMap.get(lastUnit);
            let numValue = numberResult.text && numberResult.text.length
                ? this.config.internalNumberParser.parse(numberResult)
                : null;

            let resolutionStr = numValue ? numValue.resolutionStr : null;

            ret.value =
                {
                    number: resolutionStr,
                    unit: unitValue
                } as UnitValue;

            ret.resolutionStr = (`${resolutionStr} ${unitValue}`).trim();
        }

        return ret;
    }

    private addIfNotContained(keys: Array<string>, newKey: string): void {
        if (!keys.some(key => key.includes(newKey))) {
            keys.push(newKey);
        }
    }

}

export interface INumberWithUnitParserConfiguration {
    readonly unitMap: Map<string, string>;
    readonly cultureInfo: CultureInfo;
    readonly internalNumberParser: IParser;
    readonly internalNumberExtractor: IExtractor;
    readonly connectorToken: string;
    BindDictionary(dictionary: Map<string, string>): void;
}

export abstract class BaseNumberWithUnitParserConfiguration implements INumberWithUnitParserConfiguration {
    unitMap: Map<string, string>;
    cultureInfo: CultureInfo;
    abstract internalNumberParser: IParser;
    abstract internalNumberExtractor: IExtractor;
    abstract connectorToken: string;

    constructor(cultureInfo: CultureInfo) {
        this.cultureInfo = cultureInfo;
        this.unitMap = new Map<string, string>();
    }

    BindDictionary(dictionary: ReadonlyMap<string, string>): void {
        if (!dictionary) return;
        for (let key of dictionary.keys()) {
            let value = dictionary.get(key);

            if (!key || key.length === 0) {
                continue;
            }

            let values = value.trim().split('|');
            values.forEach(token => {
                if (!token || token.length === 0 || this.unitMap.has(token)) {
                    return;
                }

                this.unitMap.set(token, key);
            });
        }
    }
}