import { IModel, ModelResult, IExtractor, ParseResult, FormatUtility } from "recognizers-text-number";
import { IDateTimeExtractor } from "./baseDateTime"
import { IDateTimeParser, DateTimeParseResult } from "./parsers";

export class DateTimeModelResult extends ModelResult {
    timexStr: string
}

export interface IDateTimeModel extends IModel {
    parse(query: string, referenceDate?: Date): ModelResult[]
}

export class DateTimeModel implements IDateTimeModel {
    modelTypeName: string = "datetime";

    protected readonly extractor: IDateTimeExtractor;
    protected readonly parser: IDateTimeParser;

    constructor(parser: IDateTimeParser, extractor: IDateTimeExtractor) {
        this.extractor = extractor;
        this.parser = parser;
    }

    parse(query: string, referenceDate: Date = new Date()): ModelResult[] {
        query = FormatUtility.preProcess(query);

        let extractResults = this.extractor.extract(query, referenceDate);
        let parseDates = new Array<DateTimeParseResult>();
        for (let result of extractResults) {
            let parseResult = this.parser.parse(result, referenceDate);
            if (Array.isArray(parseResult.value)) {
                parseDates.push(...parseResult.value);
            } else { parseDates.push(parseResult); }
        }

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