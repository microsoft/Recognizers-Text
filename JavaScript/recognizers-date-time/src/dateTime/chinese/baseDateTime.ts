import { IExtractor, ExtractResult, StringUtility, Match, RegExpUtility } from "recognizers-text-number";
import { Constants, TimeTypeConstants } from "../constants"
import { Token } from "../utilities";

export interface DateTimeExtra<T> {
    dataType: T;
    namedEntity(key: string): { value: string, index: number, length: number, captures: string[] }
}

export class TimeResult {
    hour: number;
    minute: number;
    second: number;
    lowBound: number;

    constructor(hour: number, minute: number, second: number, lowBound?: number) {
        this.hour = hour;
        this.minute = minute;
        this.second = second;
        this.lowBound = lowBound ? lowBound : -1;
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
                                ? { dataType: matchSource.get(srcMatch), namedEntity: (key: string) => srcMatch.groups(key) } as DateTimeExtra<T>
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

export class TimeResolutionUtils {
    static addDescription(lowBoundMap: ReadonlyMap<string, number>, timeResult: TimeResult, description: string) {
        if (lowBoundMap.has(description) && timeResult.hour < lowBoundMap.get(description)) {
            timeResult.hour += 12;
            timeResult.lowBound = lowBoundMap.get(description);
        } else {
            timeResult.lowBound = 0;
        }
    }

    static matchToValue(onlyDigitMatch: RegExp, numbersMap: ReadonlyMap<string, number>, source: string): number {
        if (StringUtility.isNullOrEmpty(source)) {
            return -1;
        }

        if (RegExpUtility.isMatch(onlyDigitMatch, source)) {
            return Number.parseInt(source);
        }

        if (source.length === 1) {
            return numbersMap.get(source);
        }

        let value = 1;
        for (let index = 0; index < source.length; index++) {
            let char = source.charAt(index);
            if (char === '十') {
                value *= 10;
            } else if (index === 0) {
                value *= numbersMap.get(char);
            } else {
                value += numbersMap.get(char);
            }
        }

        return value;
    }
}