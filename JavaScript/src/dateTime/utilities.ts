import { ExtractResult } from "../number/extractors"

export class Token {
    constructor(start: number, end: number) {
        this.start = start;
        this.end = end;
    }

    start: number;
    end: number;

    get length(): number {
        return this.end - this.start;
    }

    static mergeAllTokens(tokens: Token[], source: string, extractorName: string): Array<ExtractResult> {
        let ret: Array<ExtractResult> = [];
        let mergedTokens: Array<Token> = [];
        tokens = tokens.sort((a, b) => { return a.start < b.start ? -1 : 1});
        tokens.forEach(token => {
            if (token) {
                let bAdd = true;
                for (let index = 0; index < mergedTokens.length && bAdd; index++) {
                    let mergedToken = mergedTokens[index];
                    if (token.start >= mergedToken.start && token.end <= mergedToken.end) {
                        bAdd = false;
                    }
                    if (token.start > mergedToken.start && token.start < mergedToken.end)
                    {
                        bAdd = false;
                    }
                    if (token.start <= mergedToken.start && token.end >= mergedToken.end)
                    {
                        bAdd = false;
                        mergedTokens[index] = token;
                    }
                }
                if (bAdd) {
                    mergedTokens.push(token);
                }
            }
        });
        mergedTokens.forEach(token => {
            ret.push({
                start: token.start,
                length: token.length,
                text: source.substr(token.start, token.length),
                type: extractorName
            });
        });
        return ret;
    }
}

export interface IDateTimeUtilityConfiguration {
    agoStringList: string[]
    laterStringList: string[]
    inStringList: string[]
}

export class AgoLaterUtil {
    static extractorDurationWithBeforeAndAfter(source: string, er: ExtractResult, ret: Token[], config: IDateTimeUtilityConfiguration): Array<Token> {
        let pos = er.start + er.length;
        if (pos <= source.length) {
            let afterString = source.substring(pos);
            let beforeString = source.substring(0, er.start);
            let index = -1;
            let value = MatchingUtil.getAgoLaterIndex(afterString, config.agoStringList);
            if (value.matched) {
                ret.push(new Token(er.start, er.start + er.length + value.index));
            }
            else {
                value = MatchingUtil.getAgoLaterIndex(afterString, config.laterStringList);
                if (value.matched) {
                    ret.push(new Token(er.start, er.start + er.length + value.index));
                }
                else {
                    value = MatchingUtil.getInIndex(beforeString, config.inStringList);
                    if (er.start && er.length && er.start > value.index) {
                        ret.push(new Token(er.start - value.index, er.start + er.length));
                    }
                }
            }
        }
        return ret;
    }
}

export interface MatchedIndex {
    matched: boolean,
    index: number
}

export class MatchingUtil {
    static getAgoLaterIndex(source: string, referenceList: string[]): MatchedIndex {
        let result: MatchedIndex = { matched: false, index: -1 };
        let referencedMatch = referenceList.find(o => source.trim().toLowerCase().startsWith(o));
        if (referencedMatch) {
            result.index = source.toLowerCase().lastIndexOf(referencedMatch) + referencedMatch.length;
            result.matched = true;
        }
        return result;
    }

    static getInIndex(source: string, referenceList: string[]): MatchedIndex {
        let result: MatchedIndex = { matched: false, index: -1 };
        let referencedMatch = referenceList.find(o => source.trim().toLowerCase().split(' ').pop().endsWith(o));
        if (referencedMatch) {
            result.index = source.length - source.toLowerCase().lastIndexOf(referencedMatch);
            result.matched = true;
        }
        return result;
    }
}