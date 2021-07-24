// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

import { RegExpUtility, ExtractResult } from "@microsoft/recognizers-text";
import { CultureInfo, Culture, BaseNumberExtractor, BaseNumberParser } from "@microsoft/recognizers-text-number";
import { ChineseNumberWithUnitExtractorConfiguration, NumberWithUnitExtractor, NumberWithUnitParser, ChineseNumberWithUnitParserConfiguration, UnitValue } from "@microsoft/recognizers-text-number-with-unit";
import { BaseDateTimeExtractor } from "./baseDateTime";
import { IDurationParserConfiguration, BaseDurationParser } from "../baseDuration";
import { Constants, TimeTypeConstants } from "../constants";
import { ChineseDateTime } from "../../resources/chineseDateTime";
import { IDateTimeParser, DateTimeParseResult } from "../parsers";
import { DateTimeResolutionResult, StringMap } from "../utilities";

export enum DurationType {
    WithNumber
}

class DurationExtractorConfiguration extends ChineseNumberWithUnitExtractorConfiguration {
    readonly suffixList: ReadonlyMap<string, string>;
    readonly prefixList: ReadonlyMap<string, string>;
    readonly ambiguousUnitList: readonly string[];
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

    extract(source: string, refDate: Date): ExtractResult[] {
        if (!refDate) {
            refDate = new Date();
        }
        let referenceDate = refDate;

        let results = new Array<ExtractResult>();
        this.extractor.extract(source).forEach(result => {
            // filter
            if (RegExpUtility.isMatch(this.yearRegex, result.text)) {
                return;
            }

            // match suffix
            let suffix = source.substr(result.start + result.length);

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
    readonly inexactNumberUnitRegex: RegExp;
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
        if (!referenceDate) {
            referenceDate = new Date();
        }
        let resultValue;
        if (extractorResult.type === this.parserName) {
            let innerResult = new DateTimeResolutionResult();

            let parserResult = this.internalParser.parse(extractorResult);
            let unitResult: UnitValue = parserResult.value;
            if (!unitResult) {
                return new DateTimeParseResult();
            }

            let unitStr = unitResult.unit;
            let numberStr = unitResult.number;

            innerResult.timex = `P${this.isLessThanDay(unitStr) ? 'T' : ''}${numberStr}${unitStr.charAt(0)}`;
            innerResult.futureValue = Number.parseFloat(numberStr) * this.config.unitValueMap.get(unitStr);
            innerResult.pastValue = Number.parseFloat(numberStr) * this.config.unitValueMap.get(unitStr);
            innerResult.futureResolution = {};
            innerResult.futureResolution[TimeTypeConstants.DURATION] = innerResult.futureValue.toString();
            innerResult.pastResolution = {};
            innerResult.pastResolution[TimeTypeConstants.DURATION] = innerResult.pastValue.toString();
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