import { ExtractResult } from "./extractors";

export interface IParser {
    parse(extResult: ExtractResult): ParseResult | null
}

export class ParseResult extends ExtractResult {
    constructor(er?: ExtractResult) {
        super();
        if (er) {
            this.length = er.length;
            this.start = er.start;
            this.data = er.data;
            this.text = er.text;
            this.type = er.type;
        }
        this.resolutionStr = "";
    }

    // Value is for resolution.
    // e.g. 1000 for "one thousand".
    // The resolutions are different for different parsers.
    // Therefore, we use object here.
    value?: any;

    // Output the value in string format.
    // It is used in some parsers.
    resolutionStr?: string;
}