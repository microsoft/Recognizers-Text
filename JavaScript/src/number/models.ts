import { IModel, ModelResult } from "../models";
import { IExtractor } from "./extractors";
import { IParser, ParseResult } from "./parsers";

export enum NumberMode {
    // Default is for unit and datetime
    Default,
    // Add 67.5 billion & million support.
    Currency,
    // Don't extract number from cases like 16ml
    PureNumber
}

export class LongFormatType {
    // Reference : https://www.wikiwand.com/en/Decimal_mark
    // Value : 1234567.89
    // 1,234,567
    static readonly integerNumComma = new LongFormatType(',', '\0');

    // 1.234.567
    static readonly integerNumDot = new LongFormatType('.', '\0');

    // 1 234 567
    static readonly integerNumBlank = new LongFormatType(' ', '\0');

    // 1'234'567
    static readonly integerNumQuote = new LongFormatType('\'', '\0');

    // 1,234,567.89
    static readonly doubleNumCommaDot = new LongFormatType(',', '.');

    // 1,234,567·89
    static readonly doubleNumCommaCdot = new LongFormatType(',', '·');

    // 1 234 567,89
    static readonly doubleNumBlankComma = new LongFormatType(' ', ',');

    // 1 234 567.89
    static readonly doubleNumBlankDot = new LongFormatType(' ', '.');

    // 1.234.567,89
    static readonly doubleNumDotComma = new LongFormatType('.', ',');

    // 1'234'567,89
    static readonly doubleNumQuoteComma = new LongFormatType('\'', ',');

    readonly thousandsMark: string;
    readonly decimalsMark: string;

    constructor(thousandsMark: string, decimalsMark: string) {
        this.thousandsMark = thousandsMark;
        this.decimalsMark = decimalsMark;
    }
}

export abstract class AbstractNumberModel implements IModel {
    abstract modelTypeName: string;

    protected readonly parser: IParser;
    protected readonly extractor: IExtractor;

    constructor(parser: IParser, extractor: IExtractor) {
        this.extractor = extractor;
        this.parser = parser;
    }

    parse(query: string): ModelResult[] {
        let extractResults = this.extractor.extract(query);
        let parseNums = extractResults.map(r => this.parser.parse(r));

        return parseNums
            .map(o => o as ParseResult)
            .map(o => ({
                start: o.start,
                end: o.start + o.length - 1,
                resolution: { value: o.resolutionStr },
                text: o.text,
                typeName: this.modelTypeName
            }));
    }
}

export class NumberModel extends AbstractNumberModel {
    modelTypeName: string = "number";
}

export class OrdinalModel extends AbstractNumberModel {
    modelTypeName: string = "ordinal";
}

export class PercentModel extends AbstractNumberModel {
    modelTypeName: string = "percentage";
}