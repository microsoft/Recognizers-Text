import { ExtractResult, RegExpUtility, CultureInfo, Culture, BaseNumberExtractor, BaseNumberParser } from "recognizers-text-number";
import { NumberWithUnitExtractor, ChineseNumberWithUnitExtractorConfiguration, NumberWithUnitParser, ChineseNumberWithUnitParserConfiguration, UnitValue } from "recognizers-text-number-with-unit";
import { BaseDateTimeExtractor } from "./baseDateTime";
import { IDurationParserConfiguration, BaseDurationParser } from "../baseDuration";
import { Constants, TimeTypeConstants } from "../constants"
import { ChineseDateTime } from "../../resources/chineseDateTime";
import { IDateTimeParser, DateTimeParseResult } from "../parsers";
import { DateTimeResolutionResult } from "../utilities";

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

class ChineseDurationParserConfiguration implements IDurationParserConfiguration {
    readonly cardinalExtractor: BaseNumberExtractor;
    readonly numberParser: BaseNumberParser;
    readonly followedUnit: RegExp;
    readonly suffixAndRegex: RegExp;
    readonly numberCombinedWithUnit: RegExp;
    readonly anUnitRegex: RegExp;
    readonly allDateUnitRegex: RegExp;
    readonly halfDateUnitRegex: RegExp;
    readonly inExactNumberUnitRegex: RegExp;
    readonly unitMap: ReadonlyMap<string, string>;
    readonly unitValueMap: ReadonlyMap<string, number>;
    readonly doubleNumbers: ReadonlyMap<string, number>;

    constructor() {
        this.unitValueMap = ChineseDateTime.DurationUnitValueMap;
    }
}

class DurationParserConfiguration extends ChineseNumberWithUnitParserConfiguration {
    constructor() {
        super(new CultureInfo(Culture.Chinese));
        this.BindDictionary(ChineseDateTime.DurationSuffixList);
    }
}

export class ChineseDurationParser extends BaseDurationParser {
    private readonly internalParser: NumberWithUnitParser

    constructor() {
        let config = new ChineseDurationParserConfiguration();
        super(config);
        this.internalParser = new NumberWithUnitParser(new DurationParserConfiguration());
    }
    
    parse(extractorResult: ExtractResult, referenceDate?: Date): DateTimeParseResult | null {
        if (!referenceDate) referenceDate = new Date();
        let resultValue;
        if (extractorResult.type === this.parserName) {
            let innerResult = new DateTimeResolutionResult();
            let hasHalfSuffix = extractorResult.text.endsWith('半');

            if (hasHalfSuffix) {
                extractorResult.length--;
                extractorResult.text = extractorResult.text.substr(0, extractorResult.length);
            }

            let parserResult = this.internalParser.parse(extractorResult);
            let unitResult: UnitValue = parserResult.value;
            if (!unitResult) {
                return new DateTimeParseResult();
            }

            let unitStr = unitResult.unit;
            let numberStr = unitResult.number;

            if (hasHalfSuffix) {
                numberStr = (Number.parseFloat(numberStr) + 0.5).toString();
            }

            innerResult.timex = `P${this.isLessThanDay(unitStr) ? 'T' : ''}${numberStr}${unitStr.charAt(0)}`;
            innerResult.futureValue = Number.parseFloat(numberStr) * this.config.unitValueMap.get(unitStr);
            innerResult.pastValue = Number.parseFloat(numberStr) * this.config.unitValueMap.get(unitStr);
            innerResult.futureResolution = new Map<string, string>([[TimeTypeConstants.DURATION, innerResult.futureValue]])
            innerResult.pastResolution = new Map<string, string>([[TimeTypeConstants.DURATION, innerResult.pastValue]])
            innerResult.success = true;
            
            resultValue = innerResult;
        }
        let result = new DateTimeParseResult(extractorResult);
        result.value = resultValue;
        result.timexStr = resultValue ? resultValue.timex : '';
        result.resolutionStr = '';
        
        return result;
    }
}