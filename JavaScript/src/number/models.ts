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

export enum ArabicType {
    // Reference : https://www.wikiwand.com/en/Decimal_mark

    // Value : 1234567.89
    // 1,234,567
    IntegerNumComma,
    // 1.234.567
    IntegerNumDot,
    // 1 234 567
    IntegerNumBlank,
    // 1'234'567
    IntegerNumQuote,
    // 1,234,567.89
    DoubleNumCommaDot,
    // 1,234,567·89
    DoubleNumCommaCdot,
    // 1 234 567,89
    DoubleNumBlankComma,
    // 1 234 567.89
    DoubleNumBlankDot,
    // 1.234.567,89
    DoubleNumDotComma,
    // 1'234'567,89
    DoubleNumQuoteComma
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