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
	
	static isCrossOverlap(er1: ExtractResult, er2: ExtractResult): boolean {
        let splitedGroupEr1: string[] = er1.text.toString().split(' ');
        let splitedGroupEr2: string[] = er2.text.toString().split(' ');
        let lastWordEr1: string = splitedGroupEr1[0];
        let firstWordEr2: string = splitedGroupEr2[splitedGroupEr2.length - 1];
        return lastWordEr1 === firstWordEr2;
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
