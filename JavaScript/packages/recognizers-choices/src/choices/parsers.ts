import { IParser, ExtractResult, ParseResult } from "recognizers-text-base";

export class ChoicesParser<T> implements IParser {
    private readonly resolutions: Map<string, T>;

    constructor(resolutions: Map<string, T>) {
        this.resolutions = resolutions;
    }

    parse(extResult: ExtractResult): ParseResult {
        return null;
    }
}

export class BooleanParser extends ChoicesParser<boolean> {
    private static readonly booleanTrue = 'choices_true';
    private static readonly booleanFalse = 'choices_false';

    constructor() {
        super(new Map<string, boolean>([
            [BooleanParser.booleanTrue, true],
            [BooleanParser.booleanFalse, false]
        ]));
    }
}