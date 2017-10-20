import { ExtractResult, RegExpUtility, CultureInfo, Culture } from "recognizers-text-number";
import { NumberWithUnitExtractor, ChineseNumberWithUnitExtractorConfiguration } from "recognizers-text-number-with-unit";
import { BaseDateTimeExtractor } from "./baseDateTime";
import { Constants, TimeTypeConstants } from "../constants"
import { ChineseDateTime } from "../../resources/chineseDateTime";

export enum DurationType {
    WithNumber
}

class DurationExtractorConfiguration extends ChineseNumberWithUnitExtractorConfiguration {
    readonly suffixList: ReadonlyMap<string, string>;
    readonly prefixList: ReadonlyMap<string, string>;
    readonly ambiguousUnitList: ReadonlyArray<string>;
    readonly extractType: string;

    constructor() {
        super(new CultureInfo(Culture.Chinese));
        
        this.extractType = Constants.SYS_DATETIME_DURATION;
        this.suffixList = ChineseDateTime.DurationSuffixList;
        this.prefixList = new Map<string, string>();
        this.ambiguousUnitList = ChineseDateTime.DurationAmbiguousUnits;
    }
}

export class ChineseDurationExtractor extends BaseDateTimeExtractor<DurationType> {
    protected extractorName = Constants.SYS_DATETIME_DURATION; // "Duration";
    private readonly extractor: NumberWithUnitExtractor;
    private readonly yearRegex: RegExp;
    private readonly halfSuffixRegex: RegExp;

    constructor() {
        super(null);
        this.extractor = new NumberWithUnitExtractor(new DurationExtractorConfiguration());
        this.yearRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.DurationYearRegex);
        this.halfSuffixRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.DurationHalfSuffixRegex);
    }

    extract(source: string): Array<ExtractResult> {
        let results = new Array<ExtractResult>();
        this.extractor.extract(source).forEach(result => {
            // filter
            if (RegExpUtility.isMatch(this.yearRegex, result.text)) {
                return;
            }

            // match suffix
            let suffix = source.substr(result.start + result.length);
            let suffixMatch = RegExpUtility.getMatches(this.halfSuffixRegex, suffix).pop();
            if (suffixMatch && suffixMatch.index === 0) {
                result.text = result.text + suffixMatch.value;
                result.length += suffixMatch.length;
            }

            results.push(result);
        });
        
        return results;
    }
}