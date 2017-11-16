import { IParser, ExtractResult, ParseResult } from "recognizers-text-base";
import { Constants } from "./constants";

export interface IChoicesParserConfiguration<T> {
    resolutions: Map<string, T>;
}

export class ChoicesParser<T> implements IParser {
    private readonly config: IChoicesParserConfiguration<T>;

    constructor(config: IChoicesParserConfiguration<T>) {
        this.config = config;
    }

    parse(extResult: ExtractResult): ParseResult {
        let result = new ParseResult(extResult);
        result.value = this.config.resolutions.get(result.type);
        return result;
    }
}

export class BooleanParser extends ChoicesParser<boolean> {
    constructor() {
        let resolutions = new Map<string, boolean>([
            [Constants.SYS_BOOLEAN_TRUE, true],
            [Constants.SYS_BOOLEAN_FALSE, false]
        ]);
        let config: IChoicesParserConfiguration<boolean> = {
            resolutions: resolutions
        }
        super(config);
    }
}