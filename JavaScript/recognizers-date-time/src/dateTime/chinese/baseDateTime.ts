import { IExtractor, ExtractResult, StringUtility, Match, RegExpUtility } from "recognizers-text-number";
import { Constants, TimeTypeConstants } from "../constants"
import { Token } from "../utilities";

export class DateTimeExtra<T> {
    dataType: T;
    namedEntity: any;

    constructor(dataType: T, namedEntity) {
        this.dataType = dataType;
        this.namedEntity = namedEntity;
    }
}

export abstract class BaseDateTimeExtractor<T> implements IExtractor {
    protected abstract readonly extractorName: string;
    private readonly regexesDictionary: Map<RegExp, T>;

    constructor(regexesDictionary: Map<RegExp, T>) {
        this.regexesDictionary = regexesDictionary;
    }
    
    extract(source: string): Array<ExtractResult> {
        let results = new Array<ExtractResult>();
        if (StringUtility.isNullOrEmpty(source)) {
            return results;
        }

        let matchSource = new Map<Match, T>();
        let matched = new Array<boolean>(source.length);
        for (let i = 0; i < source.length; i++) {
            matched[i] = false;
        }

        let collections: Array<{ matches: Match[], value: T}> = [];
        this.regexesDictionary.forEach((value, regex) => {
            let matches = RegExpUtility.getMatches(regex, source);
            if (matches.length > 0) {
                collections.push({ matches: matches, value: value});
            }
        });

        collections.forEach(collection => {
            collection.matches.forEach(m => {
                for (let j = 0; j < m.length; j++) {
                    matched[m.index + j] = true;
                }

                // Keep Source Data for extra information
                matchSource.set(m, collection.value);
            });
        });

        let last = -1;
        for (let i = 0; i < source.length; i++) {
            if (matched[i]) {
                if (i + 1 === source.length || !matched[i + 1]) {
                    let start = last + 1;
                    let length = i - last;
                    let substr = source.substring(start, start + length).trim();
                    let srcMatch = Array.from(matchSource.keys()).find(m => m.index === start && m.length === length);
                    if (srcMatch) {
                        results.push({
                            start: start,
                            length: length,
                            text: substr,
                            type: this.extractorName,
                            data: matchSource.has(srcMatch)
                                ? new DateTimeExtra<T>(matchSource.get(srcMatch), srcMatch.groups)
                                : null
                        });
                    }
                }
            }
            else {
                last = i;
            }
        }

        return results;
    }
}