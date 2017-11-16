import { IExtractor, ExtractResult, RegExpUtility, Match, StringUtility } from "recognizers-text-base";

export interface IChoiceExtractorConfiguration {
    regexesMap: Map<RegExp, string>;
    tokenRegex: RegExp;
    allowPartialMatch: boolean;
    maxDistance: number;
    onlyTopMatch: boolean;
}

export class ChoicesExtractor implements IExtractor {
    private readonly config: IChoiceExtractorConfiguration;

    constructor(config: IChoiceExtractorConfiguration) {
        this.config = config;
    }

    extract(source: string): Array<ExtractResult> {
        let results = new Array<ExtractResult>();
        let trimmedSource = source.toLowerCase();

        if (StringUtility.isNullOrWhitespace(source)) {
            return results;
        }

        let allMatches = new Array<Match>();
        let partialResults = new Array<ExtractResult>();
        let sourceTokens = this.tokenize(trimmedSource);

        this.config.regexesMap.forEach((typeExtracted, regex) => {
            RegExpUtility.getMatches(regex, trimmedSource).forEach(match => {
                let matchTokens = this.tokenize(match.value);
                let topScore = sourceTokens
                    .map((sToken, index) => this.matchValue(sourceTokens, matchTokens, index))
                    .reduce((top, value) => top = Math.max(top, value), 0.0);
                if (topScore > 0.0) {
                    let start = match.index;
                    let length = match.length;
                    let text = source.substr(start, length).trim();
                    partialResults.push({
                        start: start,
                        length: length,
                        text: text,
                        type: typeExtracted,
                        data: {
                            source: source,
                            score: topScore
                        }
                    });
                }
            });
        });

        if (this.config.onlyTopMatch) {
            let topResult = partialResults.reduce((top, value) => top = top.data.score < value.data.score ? value : top, partialResults[0]);
            topResult.data.otherMatches = partialResults.filter(r => r !== topResult);
            results.push(topResult);
        } else {
            results = partialResults;
        }

        return results;
    }

    matchValue(source: string[], match: string[], startPos: number): number {
        let matched = 0;
        let totalDeviation = 0;
        match.forEach(matchToken => {
            let pos = source.indexOf(matchToken, startPos);
            if (pos >= 0) {
                let distance = matched > 0 ? pos - startPos : 0;
                if (distance <= this.config.maxDistance) {
                    matched++;
                    totalDeviation += distance;
                    startPos = pos + 1;
                }
            }
        });

        let score = 0.0;
        if (matched > 0 && (matched === match.length || this.config.allowPartialMatch)) {
            let completeness = matched / match.length;
            let accuracy = completeness * (matched / (matched + totalDeviation));
            let initialScore = accuracy * (matched / source.length);
            score = 0.4 + (0.6 * initialScore);
        }
        return score;
    }

    private tokenize(source: string): string[] {
        return RegExpUtility.split(this.config.tokenRegex, source).filter(s => !StringUtility.isNullOrWhitespace(s));
    }
}

export interface IBooleanExtractorConfiguration {
    regexTrue: RegExp;
    regexFalse: RegExp;
    tokenRegex: RegExp;
}

export class BooleanExtractor extends ChoicesExtractor {
    private static readonly booleanTrue = 'choices_true';
    private static readonly booleanFalse = 'choices_false';

    constructor(config: IBooleanExtractorConfiguration) {
        let regexesMap = new Map<RegExp, string>()
            .set(config.regexTrue, BooleanExtractor.booleanTrue)
            .set(config.regexFalse, BooleanExtractor.booleanFalse)

        let choicesConfig: IChoiceExtractorConfiguration = {
            regexesMap: regexesMap,
            tokenRegex: config.tokenRegex,
            allowPartialMatch: false,
            maxDistance: 2,
            onlyTopMatch: true
        }
        super(choicesConfig);
    }
}