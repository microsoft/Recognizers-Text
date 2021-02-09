import { IExtractor, ExtractResult, RegExpUtility, Match, StringUtility } from "@microsoft/recognizers-text";
import { Constants } from "./constants";
import GraphemeSplitter = require("grapheme-splitter");
const splitter = new GraphemeSplitter();

export interface IChoiceExtractorConfiguration {
    regexesMap: Map<RegExp, string>;
    tokenRegex: RegExp;
    allowPartialMatch: boolean;
    maxDistance: number;
    onlyTopMatch: boolean;
    emojiSkinToneRegex: RegExp;
}

export class ChoiceExtractor implements IExtractor {
    private readonly config: IChoiceExtractorConfiguration;
    protected extractType: string;

    constructor(config: IChoiceExtractorConfiguration) {
        this.config = config;
    }

    extract(source: string): ExtractResult[] {
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

        if (partialResults.length === 0) {
            return results;
        }

        partialResults = partialResults.sort((a, b) => a.start - b.start);

        if (this.config.onlyTopMatch) {
            let topResult = partialResults.reduce((top, value) => top = top.data.score < value.data.score ? value : top, partialResults[0]);
            topResult.data.otherMatches = partialResults.filter(r => r !== topResult);
            results.push(topResult);
        }
        else {
            results = partialResults;
        }

        return results;
    }

    matchValue(source: string[], match: string[], startPos: number): number {
        let matched = 0;
        let emojiSkinToneMatch = 0;
        let totalDeviation = 0;
        match.forEach(matchToken => {
            let pos = source.indexOf(matchToken, startPos);
            if (pos >= 0) {
                let distance = matched > 0 ? pos - startPos : 0;
                if (distance <= this.config.maxDistance) {
                    matched++;
                    totalDeviation += distance;
                    startPos = pos + 1;
                    emojiSkinToneMatch += RegExpUtility.getMatches(this.config.emojiSkinToneRegex, matchToken).length;
                }
            }
        });

        let score = 0.0;
        let emojiSkinToneLen = RegExpUtility.getMatches(this.config.emojiSkinToneRegex, source.join()).length;
        if (matched > 0 && (matched === (match.length) || this.config.allowPartialMatch)) {
            let completeness = matched / match.length;
            let accuracy = completeness * (matched / (matched + totalDeviation));
            let initialScore = accuracy * ((matched + emojiSkinToneMatch) / (source.length + emojiSkinToneLen));
            score = 0.4 + (0.6 * initialScore);
        }
        return score;
    }

    private tokenize(source: string): string[] {
        let tokens: string[] = [];
        let chars = splitter.splitGraphemes(source);
        let token = '';
        chars.forEach(c => {
            let codePoint = c.codePointAt(0) || c.charAt(0);
            if (codePoint > 0xFFFF) {
                // Character is in a Supplementary Unicode Plane. This is where emoji live so
                // we're going to just break each character in this range out as its own token.
                tokens.push(c);
                if (!StringUtility.isNullOrWhitespace(token)) {
                    tokens.push(token);
                    token = '';
                }
            }
            else if (!(this.config.tokenRegex.test(c) || StringUtility.isWhitespace(c))) {
                token = token.concat(c);
            }
            else if (!StringUtility.isNullOrWhitespace(token)) {
                tokens.push(token);
                token = '';
            }
        });

        if (!StringUtility.isNullOrWhitespace(token)) {
            tokens.push(token);
            token = '';
        }
        return tokens;
    }
}

export interface IBooleanExtractorConfiguration {
    regexTrue: RegExp;
    regexFalse: RegExp;
    tokenRegex: RegExp;
    onlyTopMatch: boolean;
    emojiSkinToneRegex: RegExp;
}

export class BooleanExtractor extends ChoiceExtractor {
    private static readonly booleanTrue = Constants.SYS_BOOLEAN_TRUE;
    private static readonly booleanFalse = Constants.SYS_BOOLEAN_FALSE;

    constructor(config: IBooleanExtractorConfiguration) {
        let regexesMap = new Map<RegExp, string>()
            .set(config.regexTrue, Constants.SYS_BOOLEAN_TRUE)
            .set(config.regexFalse, Constants.SYS_BOOLEAN_FALSE);

        let optionsConfig: IChoiceExtractorConfiguration = {
            regexesMap: regexesMap,
            tokenRegex: config.tokenRegex,
            allowPartialMatch: false,
            maxDistance: 2,
            onlyTopMatch: config.onlyTopMatch,
            emojiSkinToneRegex: config.emojiSkinToneRegex
        };
        super(optionsConfig);
        this.extractType = Constants.SYS_BOOLEAN;
    }
}