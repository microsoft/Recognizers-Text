import { IParser, ExtractResult, ParseResult } from "recognizers-text";
import { Constants } from "./constants";

export interface IOptionsParserConfiguration<T> {
    resolutions: Map<string, T>;
}

export class OptionsParser<T> implements IParser {
    private readonly config: IOptionsParserConfiguration<T>;

    constructor(config: IOptionsParserConfiguration<T>) {
        this.config = config;
    }

    parse(extResult: ExtractResult): ParseResult {
        let result = new ParseResult(extResult);
        result.value = this.config.resolutions.get(result.type);
        if (result.data.otherMatches) {
            result.data.otherMatches = result.data.otherMatches.map(m => {
                let r = new ParseResult(m);
                r.value = this.config.resolutions.get(r.type);
                return r;
            });
        }
        return result;
    }
}

export class BooleanParser extends OptionsParser<boolean> {
    constructor() {
        let resolutions = new Map<string, boolean>([
            [Constants.SYS_BOOLEAN_TRUE, true],
            [Constants.SYS_BOOLEAN_FALSE, false]
        ]);
        let config: IOptionsParserConfiguration<boolean> = {
            resolutions: resolutions
        }
        super(config);
    }
}