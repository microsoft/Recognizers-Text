import { NumberWithUnitExtractor, INumberWithUnitExtractorConfiguration } from "../extractors";
import { ExtractResult, RegExpUtility } from "@microsoft/recognizers-text";
import { ChineseNumericWithUnit } from "../../resources/chineseNumericWithUnit";

export class ChineseNumberWithUnitExtractor extends NumberWithUnitExtractor {
    private readonly halfUnitRegex = RegExpUtility.getSafeRegExp(ChineseNumericWithUnit.HalfUnitRegex);

    constructor(config: INumberWithUnitExtractorConfiguration) {
        super(config);
    }

    extract(source: string): ExtractResult[] {
        let result = this._extract(source);
        let numbers = this.config.unitNumExtractor.extract(source);

        // expand Chinese phrase to the `half` patterns when it follows closely origin phrase.
        if (this.halfUnitRegex != null){
            let match = new Array<ExtractResult>();
            for (let number of numbers) {
                if (RegExpUtility.getMatches(this.halfUnitRegex, number.text).length > 0){
                    match.push(number);
                }
            }
            if (match.length > 0){
                let res = new Array<ExtractResult>();
                for (let er of result){
                    let start = er.start;
                    let length = er.length;
                    let matchSuffix = new Array<ExtractResult>();
                    for (let mr of match){
                        if (mr.start == start + length){
                            matchSuffix.push(mr);
                        }
                    }
                    if (matchSuffix.length === 1){
                        let mr = matchSuffix[0];
                        er.length += mr.length;
                        er.text += mr.text;
                        let tmp = new Array<ExtractResult>();
                        tmp.push(er.data);
                        tmp.push(mr);
                        er.data = tmp;
                    }
                    res.push(er);
                }
                result = res;
            }
        }
        return result;
    }
}
