import { IModel, ModelResult, IExtractor, IParser, ParseResult, QueryProcessor } from "@microsoft/recognizers-text";

export abstract class AbstractSequenceModel implements IModel {
    public abstract readonly modelTypeName: string;

    protected readonly extractor: IExtractor;
    protected readonly parser: IParser;

    constructor(parser: IParser, extractor: IExtractor) {
        this.extractor = extractor;
        this.parser = parser;
    }

    parse(query: string): ModelResult[] {
        let parseResults = new Array<ParseResult>();

        try {
            let extractResults = this.extractor.extract(query);
            parseResults = extractResults.map(r => this.parser.parse(r));
        }
        catch (err) {
            // Nothing to do. Exceptions in result process should not affect other extracted entities.
            // No result.
        }
        finally {
            return parseResults
                .map(o => o as ParseResult)
                .map(o => ({
                    start: o.start,
                    end: o.start + o.length - 1,
                    resolution: { "value": o.resolutionStr },
                    text: o.text,
                    typeName: this.modelTypeName
                }));
        }
    }
}

export class PhoneNumberModel extends AbstractSequenceModel {
    public modelTypeName: string = "phonenumber";

    parse(query: string): ModelResult[] {
        let parseResults = new Array<ParseResult>();
        query = QueryProcessor.preProcess(query);

        try {
            let extractResults = this.extractor.extract(query);
            parseResults = extractResults.map(r => this.parser.parse(r));
        }
        catch (err) {
            // Nothing to do. Exceptions in result process should not affect other extracted entities.
            // No result.
        }
        finally {
            return parseResults
                .map(o => o as ParseResult)
                .map(o => ({
                    start: o.start,
                    end: o.start + o.length - 1,
                    resolution: {
                        "value": o.resolutionStr,
                        "score": o.value.toString()
                    },
                    text: o.text,
                    typeName: this.modelTypeName
                }));
        }
    }
}

export class IpAddressModel extends AbstractSequenceModel {
    public modelTypeName: string = "ip";

    parse(query: string): ModelResult[] {
        let parseResults = new Array<ParseResult>();

        try {
            let extractResults = this.extractor.extract(query);
            parseResults = extractResults.map(r => this.parser.parse(r));
        }
        catch (err) {
            // Nothing to do. Exceptions in result process should not affect other extracted entities.
            // No result.
        }
        finally {
            return parseResults
                .map(o => o as ParseResult)
                .map(o => ({
                    start: o.start,
                    end: o.start + o.length - 1,
                    resolution: {
                        "value": o.resolutionStr,
                        "type": o.data
                    },
                    text: o.text,
                    typeName: this.modelTypeName
                }));
        }
    }
}

export class MentionModel extends AbstractSequenceModel {
    public modelTypeName: string = "mention";
}

export class HashtagModel extends AbstractSequenceModel {
    public modelTypeName: string = "hashtag";
}

export class EmailModel extends AbstractSequenceModel {
    public modelTypeName: string = "email";
}

export class URLModel extends AbstractSequenceModel {
    public modelTypeName: string = "url";
}

export class GUIDModel extends AbstractSequenceModel {
    public modelTypeName: string = "guid";

    parse(query: string): ModelResult[] {
        let parseResults = new Array<ParseResult>();
        query = QueryProcessor.preProcess(query);

        try {
            let extractResults = this.extractor.extract(query);
            parseResults = extractResults.map(r => this.parser.parse(r));
        }
        catch (err) {
            // Nothing to do. in result process should not affect other extracted entities.
            // No result.
        }
        finally {
            return parseResults
                .map(o => o as ParseResult)
                .map(o => ({
                    start: o.start,
                    end: o.start + o.length - 1,
                    resolution: {
                        "value": o.resolutionStr,
                        "score": o.value.toString()
                    },
                    text: o.text,
                    typeName: this.modelTypeName
                }));
        }
    }
}