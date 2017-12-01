export interface IExtractor {
    extract(input: string): Array<ExtractResult>
}

export class ExtractResult {
    start: number;
    length: number;
    text: string;
    type: string;
    data?: any;

    static isOverlap(erA: ExtractResult, erB: ExtractResult): boolean {
        return !( erA.start >= erB.start + erB.length ) && !( erB.start >= erA.start + erA.length );
    }

    static isCover(er1: ExtractResult, er2: ExtractResult): boolean {
        return ((er2.start < er1.start) && ((er2.start + er2.length) >= (er1.start + er1.length)))
        || ((er2.start <= er1.start) && ((er2.start + er2.length) > (er1.start + er1.length)));
    }

    static getFromText(source: string): ExtractResult {
        return {
            start: 0,
            length: source.length,
            text: source,
            type: 'custom'
        }
    }
}
