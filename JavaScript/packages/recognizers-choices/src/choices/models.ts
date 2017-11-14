import { IModel, ModelResult, IExtractor, IParser, ParseResult } from "recognizers-text-base";

export class BooleanModel implements IModel {
    public readonly modelTypeName = 'boolean';
     
    protected readonly extractor: IExtractor;
    protected readonly parser: IParser;

    constructor(parser: IParser, extractor: IExtractor) {
        this.extractor = extractor;
        this.parser = parser;
    }

    parse(source: string): ModelResult[] {
        let extractResults = this.extractor.extract(source);
        let parseResults = extractResults.map(r => this.parser.parse(r));

        return parseResults
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