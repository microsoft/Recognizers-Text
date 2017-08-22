import { IModel, ModelResult } from "../models";
import { IExtractor } from "../number/extractors";
import { ParseResult } from "../number/parsers";
import { IDateTimeParser, DateTimeParseResult } from "./parsers";
import { FormatUtility } from "../utilities";

export class DateTimeModelResult extends ModelResult {
    timexStr: string
}

export interface IDateTimeModel extends IModel {
    parse(query: string, referenceDate?: Date): ModelResult[]
}

export class DateTimeModel implements IDateTimeModel {
    modelTypeName: string = "datetime";

    protected readonly extractor: IExtractor;
    protected readonly parser: IDateTimeParser;

    constructor(parser: IDateTimeParser, extractor: IExtractor) {
        this.extractor = extractor;
        this.parser = parser;
    }

    parse(query: string, referenceDate: Date = new Date()): ModelResult[] {
        query = FormatUtility.preProcess(query);

        let extractResults = this.extractor.extract(query);
        let parseDates = extractResults
            .map(r => this.parser.parseWithReferenceTime(r, referenceDate));

        return parseDates
            .map(o => ({
                start: o.start,
                end: o.start + o.length - 1,
                resolution: o.value, // TODO: convert to proper resolution
                text: o.text,
                typeName: o.type
            }));
    }
}