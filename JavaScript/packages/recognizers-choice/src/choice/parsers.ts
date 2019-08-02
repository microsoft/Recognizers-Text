import { IParser, ExtractResult, ParseResult } from "@microsoft/recognizers-text";
import { Constants } from "./constants";

export interface IChoiceParserConfiguration<T> {
    resolutions: Map<string, T>;
}

export class ChoiceParser<T> implements IParser {
    private readonly config: IChoiceParserConfiguration<T>;

    constructor(config: IChoiceParserConfiguration<T>) {
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

export class BooleanParser extends ChoiceParser<boolean> {
    constructor() {
        let resolutions = new Map<string, boolean>([
            [Constants.SYS_BOOLEAN_TRUE, true],
            [Constants.SYS_BOOLEAN_FALSE, false]
        ]);
        let config: IChoiceParserConfiguration<boolean> = {
            resolutions: resolutions
        };
        super(config);
    }
}