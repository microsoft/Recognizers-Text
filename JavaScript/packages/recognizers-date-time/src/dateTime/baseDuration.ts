// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

import { ExtractResult, RegExpUtility, StringUtility } from "@microsoft/recognizers-text";
import { Constants, TimeTypeConstants } from "./constants";
import { BaseNumberExtractor, BaseNumberParser } from "@microsoft/recognizers-text-number";
import { IDateTimeParser, DateTimeParseResult } from "./parsers";
import { Token, DateTimeResolutionResult } from "./utilities";
import { IDateTimeExtractor } from "./baseDateTime";

export interface IDurationExtractorConfiguration {
    allRegex: RegExp,
    halfRegex: RegExp,
    followedUnit: RegExp,
    numberCombinedWithUnit: RegExp,
    anUnitRegex: RegExp,
    inexactNumberUnitRegex: RegExp,
    suffixAndRegex: RegExp,
    relativeDurationUnitRegex: RegExp,
    moreThanRegex: RegExp,
    lessThanRegex: RegExp,
    cardinalExtractor: BaseNumberExtractor
}

export class BaseDurationExtractor implements IDateTimeExtractor {
    private readonly extractorName = Constants.SYS_DATETIME_DURATION;
    private readonly config: IDurationExtractorConfiguration;

    constructor(config: IDurationExtractorConfiguration) {
        this.config = config;
    }

    extract(source: string, refDate: Date): ExtractResult[] {
        if (!refDate) {
            refDate = new Date();
        }

        let baseTokens = this.numberWithUnit(source);
        let tokens: Token[] = new Array<Token>()
            .concat(baseTokens)
            .concat(this.numberWithUnitAndSuffix(source, baseTokens))
            .concat(this.implicitDuration(source));
        let result = Token.mergeAllTokens(tokens, source, this.extractorName);

        this.resolveMoreThanOrLessThanPrefix(source, result);

        return result;
    }

    // handle cases look like: {more than | less than} {duration}?
    private resolveMoreThanOrLessThanPrefix(text: string, ers: ExtractResult[]) {
        for (let er of ers) {
            let beforeString = text.substr(0, er.start);
            let match = RegExpUtility.getMatches(this.config.moreThanRegex, beforeString);
            if (match && match.length) {
                er.data = TimeTypeConstants.moreThanMod;
            }

            if (!match || match.length === 0) {
                match = RegExpUtility.getMatches(this.config.lessThanRegex, beforeString);
                if (match && match.length) {
                    er.data = TimeTypeConstants.lessThanMod;
                }
            }

            if (match && match.length) {
                er.length += er.start - match[0].index;
                er.start = match[0].index;
                er.text = text.substr(er.start, er.length);
            }
        }
    }

    private numberWithUnit(source: string): Token[] {
        return this.config.cardinalExtractor.extract(source)
            .map(o => {
                let afterString = source.substring(o.start + o.length);
                let match = RegExpUtility.getMatches(this.config.followedUnit, afterString)[0];
                if (match && match.index === 0) {
                    return new Token(o.start | 0, o.start + o.length + match.length);
                }
            }).filter(o => o !== undefined)
            .concat(this.getTokensFromRegex(this.config.numberCombinedWithUnit, source))
            .concat(this.getTokensFromRegex(this.config.anUnitRegex, source))
            .concat(this.getTokensFromRegex(this.config.inexactNumberUnitRegex, source));
    }

    private numberWithUnitAndSuffix(source: string, ers: Token[]): Token[] {
        return ers.map(o => {
            let afterString = source.substring(o.start + o.length);
            let match = RegExpUtility.getMatches(this.config.suffixAndRegex, afterString)[0];
            if (match && match.index === 0) {
                return new Token(o.start | 0, o.start + o.length + match.length);
            }
        });
    }

    private implicitDuration(source: string): Token[] {
        // handle "all day", "all year"
        return this.getTokensFromRegex(this.config.allRegex, source)
            // handle "half day", "half year"
            .concat(this.getTokensFromRegex(this.config.halfRegex, source))
            // handle "next day", "last year"
            .concat(this.getTokensFromRegex(this.config.relativeDurationUnitRegex, source));
    }

    private getTokensFromRegex(regexp: RegExp, source: string): Token[] {
        return RegExpUtility.getMatches(regexp, source)
            .map(o => new Token(o.index, o.index + o.length));
    }
}

export interface IDurationParserConfiguration {
    cardinalExtractor: BaseNumberExtractor
    numberParser: BaseNumberParser
    followedUnit: RegExp
    suffixAndRegex: RegExp
    numberCombinedWithUnit: RegExp
    anUnitRegex: RegExp
    allDateUnitRegex: RegExp
    halfDateUnitRegex: RegExp
    inexactNumberUnitRegex: RegExp
    unitMap: ReadonlyMap<string, string>
    unitValueMap: ReadonlyMap<string, number>
    doubleNumbers: ReadonlyMap<string, number>
}

export class BaseDurationParser implements IDateTimeParser {
    protected readonly parserName = Constants.SYS_DATETIME_DURATION;
    protected readonly config: IDurationParserConfiguration;

    constructor(config: IDurationParserConfiguration) {
        this.config = config;
    }

    parse(extractorResult: ExtractResult, referenceDate?: Date): DateTimeParseResult | null {
        if (!referenceDate) {
            referenceDate = new Date();
        }
        let resultValue;
        if (extractorResult.type === this.parserName) {
            let source = extractorResult.text.toLowerCase();
            let innerResult = this.parseNumberWithUnit(source, referenceDate);
            if (!innerResult.success) {
                innerResult = this.parseImplicitDuration(source, referenceDate);
            }
            if (innerResult.success) {
                innerResult.futureResolution = {};
                innerResult.futureResolution[TimeTypeConstants.DURATION] = innerResult.futureValue.toString();
                innerResult.pastResolution = {};
                innerResult.pastResolution[TimeTypeConstants.DURATION] = innerResult.pastValue.toString();
                resultValue = innerResult;
            }
        }

        let value = resultValue as DateTimeResolutionResult;

        if (value && extractorResult.data) {
            if (extractorResult.data === TimeTypeConstants.moreThanMod ||
                extractorResult.data === TimeTypeConstants.lessThanMod) {
                value.mod = extractorResult.data;
            }
        }

        let result = new DateTimeParseResult(extractorResult);
        result.value = resultValue;
        result.timexStr = resultValue ? resultValue.timex : '';
        result.resolutionStr = '';

        return result;
    }

    private parseNumberWithUnit(source: string, referenceDate: Date): DateTimeResolutionResult {
        let trimmedSource = source.trim();
        let result = this.parseNumberSpaceUnit(trimmedSource);
        if (!result.success) {
            result = this.parseNumberCombinedUnit(trimmedSource);
        }
        if (!result.success) {
            result = this.parseAnUnit(trimmedSource);
        }
        if (!result.success) {
            result = this.parseInexactNumberUnit(trimmedSource);
        }
        return result;
    }

    private parseImplicitDuration(source: string, referenceDate: Date): DateTimeResolutionResult {
        let trimmedSource = source.trim();
        // handle "all day" "all year"
        let result = this.getResultFromRegex(this.config.allDateUnitRegex, trimmedSource, 1);
        // handle "half day", "half year"
        if (!result.success) {
            result = this.getResultFromRegex(this.config.halfDateUnitRegex, trimmedSource, 0.5);
        }
        // handle single duration unit, it is filtered in the extraction that there is a relative word in advance
        if (!result.success) {
            result = this.getResultFromRegex(this.config.followedUnit, trimmedSource, 1);
        }
        return result;
    }

    private getResultFromRegex(regex: RegExp, source: string, num: number): DateTimeResolutionResult {
        let result = new DateTimeResolutionResult();
        let match = RegExpUtility.getMatches(regex, source).pop();
        if (!match) {
            return result;
        }

        let sourceUnit = match.groups('unit').value;
        if (!this.config.unitMap.has(sourceUnit)) {
            return result;
        }

        let unitStr = this.config.unitMap.get(sourceUnit);
        result.timex = `P${this.isLessThanDay(unitStr) ? 'T' : ''}${num}${unitStr[0]}`;
        result.futureValue = num * this.config.unitValueMap.get(sourceUnit);
        result.pastValue = result.futureValue;
        result.success = true;
        return result;
    }

    private parseNumberSpaceUnit(source: string): DateTimeResolutionResult {
        let result = new DateTimeResolutionResult();
        let suffixStr = source;
        let ers = this.config.cardinalExtractor.extract(source);
        if (ers && ers.length === 1) {
            let er = ers[0];
            let sourceUnit = '';
            let pr = this.config.numberParser.parse(er);
            let noNumStr = source.substr(er.start + er.length).trim().toLowerCase();
            let match = RegExpUtility.getMatches(this.config.followedUnit, noNumStr).pop();
            if (match) {
                sourceUnit = match.groups('unit').value;
                suffixStr = match.groups('suffix').value;
            }
            if (this.config.unitMap.has(sourceUnit)) {
                let num = Number.parseFloat(pr.value) + this.parseNumberWithUnitAndSuffix(suffixStr);
                let unitStr = this.config.unitMap.get(sourceUnit);

                result.timex = `P${this.isLessThanDay(unitStr) ? 'T' : ''}${num}${unitStr[0]}`;
                result.futureValue = num * this.config.unitValueMap.get(sourceUnit);
                result.pastValue = result.futureValue;
                result.success = true;
                return result;
            }
        }
        return result;
    }

    private parseNumberWithUnitAndSuffix(source: string): number {
        let match = RegExpUtility.getMatches(this.config.suffixAndRegex, source).pop();
        if (match) {
            let numStr = match.groups('suffix_num').value;
            if (this.config.doubleNumbers.has(numStr)) {
                return this.config.doubleNumbers.get(numStr);
            }
        }
        return 0;
    }

    private parseNumberCombinedUnit(source: string): DateTimeResolutionResult {
        let result = new DateTimeResolutionResult();
        let match = RegExpUtility.getMatches(this.config.numberCombinedWithUnit, source).pop();
        if (!match) {
            return result;
        }
        let num = Number.parseFloat(match.groups('num').value) + this.parseNumberWithUnitAndSuffix(source);

        let sourceUnit = match.groups('unit').value;
        if (this.config.unitMap.has(sourceUnit)) {
            let unitStr = this.config.unitMap.get(sourceUnit);
            if (num > 1000 && (unitStr === 'Y' || unitStr === 'MON' || unitStr === 'W')) {
                return result;
            }

            result.timex = `P${this.isLessThanDay(unitStr) ? 'T' : ''}${num}${unitStr[0]}`;
            result.futureValue = num * this.config.unitValueMap.get(sourceUnit);
            result.pastValue = result.futureValue;
            result.success = true;
            return result;
        }
        return result;
    }

    private parseAnUnit(source: string): DateTimeResolutionResult {
        let result = new DateTimeResolutionResult();
        let match = RegExpUtility.getMatches(this.config.anUnitRegex, source).pop();
        if (!match) {
            match = RegExpUtility.getMatches(this.config.halfDateUnitRegex, source).pop();
        }
        if (!match) {
            return result;
        }
        let num = StringUtility.isNullOrEmpty(match.groups('half').value) ? 1 : 0.5;
        num += this.parseNumberWithUnitAndSuffix(source);

        let sourceUnit = match.groups('unit').value;
        if (this.config.unitMap.has(sourceUnit)) {
            let unitStr = this.config.unitMap.get(sourceUnit);

            result.timex = `P${this.isLessThanDay(unitStr) ? 'T' : ''}${num}${unitStr[0]}`;
            result.futureValue = num * this.config.unitValueMap.get(sourceUnit);
            result.pastValue = result.futureValue;
            result.success = true;
            return result;
        }
        return result;
    }

    private parseInexactNumberUnit(source: string): DateTimeResolutionResult {
        let result = new DateTimeResolutionResult();
        let match = RegExpUtility.getMatches(this.config.inexactNumberUnitRegex, source).pop();
        if (!match) {
            return result;
        }

        let num: number;
        if (match.groups('NumTwoTerm').value) {
            num = 2;
        }
        else {
            // set the inexact number "few", "some" to 3 for now
            num = 3;
        }

        let sourceUnit = match.groups('unit').value;
        if (this.config.unitMap.has(sourceUnit)) {
            let unitStr = this.config.unitMap.get(sourceUnit);
            if (num > 1000 && (unitStr === 'Y' || unitStr === 'MON' || unitStr === 'W')) {
                return result;
            }

            result.timex = `P${this.isLessThanDay(unitStr) ? 'T' : ''}${num}${unitStr[0]}`;
            result.futureValue = num * this.config.unitValueMap.get(sourceUnit);
            result.pastValue = result.futureValue;
            result.success = true;
            return result;
        }
        return result;
    }

    protected isLessThanDay(source: string): boolean {
        return (source === 'S') || (source === 'M') || (source === 'H');
    }
}
