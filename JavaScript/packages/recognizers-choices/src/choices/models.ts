import { IModel, ModelResult, IExtractor, IParser, ParseResult } from "recognizers-text-base";

export abstract class ChoicesModel implements IModel {
    public abstract readonly modelTypeName: string;

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
                resolution: this.getResolution(o),
                text: o.text,
                typeName: this.modelTypeName
            }));
    }

    protected abstract getResolution(data: any): any;
}

export class BooleanModel extends ChoicesModel {
    public readonly modelTypeName = 'boolean';

    protected getResolution(result: any): any {
        return {
            value: result.value,
            score: result.data.score
        }
    }
}