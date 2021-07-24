// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

import { IModel, ModelResult, IExtractor, ParseResult, QueryProcessor } from "@microsoft/recognizers-text";
import { IDateTimeParser, DateTimeParseResult } from "./parsers";
import { IDateTimeExtractor } from "./baseDateTime";

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

        query = QueryProcessor.preProcess(query);
        let parseDates = new Array<DateTimeParseResult>();

        try {
            let extractResults = this.extractor.extract(query, referenceDate);
            for (let result of extractResults) {
                let parseResult = this.parser.parse(result, referenceDate);
                if (Array.isArray(parseResult.value)) {
                    parseDates.push(...parseResult.value);
                }
                else {
                    parseDates.push(parseResult);
                }
            }
        }
        catch (err) {
            // Nothing to do. Exceptions in parse should not break users of recognizers.
            // No result.
        }
        finally {
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
}