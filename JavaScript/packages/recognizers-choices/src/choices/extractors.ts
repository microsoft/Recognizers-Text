import { IExtractor, ExtractResult } from "recognizers-text-base";

export class ChoicesExtractor implements IExtractor {
    private readonly regexes: Map<RegExp, string>;

    constructor(regexes: Map<RegExp, string>) {
        this.regexes = regexes;
    }

    extract(source: string): Array<ExtractResult> {
        if (!source || source.trim().length === 0) {
            return new Array<ExtractResult>();
        }
    }
}

export class BooleanExtractor extends ChoicesExtractor {
    private static readonly booleanTrue = 'choices_true';
    private static readonly booleanFalse = 'choices_false';

    constructor(trueRegex: RegExp, falseRegex: RegExp) {
        super(new Map<RegExp, string>([
            [trueRegex, BooleanExtractor.booleanTrue],
            [falseRegex, BooleanExtractor.booleanFalse]
        ]));
    }
}