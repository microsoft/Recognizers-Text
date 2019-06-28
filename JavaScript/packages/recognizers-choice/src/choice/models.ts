import { IModel, ModelResult, IExtractor, IParser, ParseResult } from "@microsoft/recognizers-text";

export abstract class ChoiceModel implements IModel {
    public abstract readonly modelTypeName: string;

    protected readonly extractor: IExtractor;
    protected readonly parser: IParser;

    constructor(parser: IParser, extractor: IExtractor) {
        this.extractor = extractor;
        this.parser = parser;
    }

    parse(source: string): ModelResult[] {
        let parseResults: ParseResult[];

        try {
            let extractResults = this.extractor.extract(source);
            parseResults = extractResults.map(r => this.parser.parse(r));
        }
        catch(err) {
            // Nothing to do. Exceptions in parse should not break users of recognizers.
            // No result.
        }
        finally {
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
    }

    protected abstract getResolution(data: any): any;
}

export class BooleanModel extends ChoiceModel {
    public readonly modelTypeName = 'boolean';

    protected getResolution(sources: any): any {
        let results: any = {
            value: sources.value,
            score: sources.data.score
        }
        if (sources.data.otherMatches) {
            results.otherResults = sources.data.otherMatches.map(o => ({
                text: o.text,
                value: o.value,
                score: o.data.score
            }))
        }
        return results;
    }
}