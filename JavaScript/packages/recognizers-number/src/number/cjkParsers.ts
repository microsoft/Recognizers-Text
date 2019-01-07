import { ExtractResult, IParser, ParseResult } from "@microsoft/recognizers-text";
import { BaseNumberParser, INumberParserConfiguration } from "./parsers";
import { Constants } from "./constants";
import { LongFormatType } from "./models";
import { CultureInfo, Culture } from "../culture";
import { RegExpUtility, StringUtility } from "@microsoft/recognizers-text";
import { BigNumber } from 'bignumber.js/bignumber';
import trimEnd = require("lodash.trimend");
import sortBy = require("lodash.sortby");

export interface ICJKNumberParserConfiguration extends INumberParserConfiguration {
    readonly zeroToNineMap: ReadonlyMap<string, number>;
    readonly roundNumberMapChar: ReadonlyMap<string, number>;
    readonly fullToHalfMap: ReadonlyMap<string, string>;
    readonly tratoSimMap: ReadonlyMap<string, string>;
    readonly unitMap: ReadonlyMap<string, string>;
    readonly roundDirectList: ReadonlyArray<string>;
    readonly digitNumRegex: RegExp;
    readonly dozenRegex: RegExp;
    readonly percentageRegex: RegExp;
    readonly doubleAndRoundRegex: RegExp;
    readonly fracSplitRegex: RegExp;
    readonly pointRegex: RegExp;
    readonly speGetNumberRegex: RegExp;
    readonly pairRegex: RegExp;
    readonly roundNumberIntegerRegex: RegExp;
}

export class BaseCJKNumberParser extends BaseNumberParser {
    readonly config: ICJKNumberParserConfiguration;

    constructor(config: ICJKNumberParserConfiguration) {
        super(config);
        this.config = config;
    }

    private toString(value: any): string {
        return this.config.cultureInfo
            ? this.config.cultureInfo.format(value)
            : value.toString();
    }

    parse(extResult: ExtractResult): ParseResult | null {
        let extra = '';
        let result: ParseResult;
        extra = extResult.data;
        let getExtResult: ExtractResult = {
            start: extResult.start,
            length: extResult.length,
            data: extResult.data,
            text: this.replaceTraditionalWithSimplified(extResult.text),
            type: extResult.type
        }

        if (!extra) {
            return result;
        }

        if (extra.includes("Per")) {
            result = this.perParseCJK(getExtResult);
        } else if (extra.includes("Num")) {
            getExtResult.text = this.replaceFullWithHalf(getExtResult.text);
            result = this.digitNumberParse(getExtResult);
            if(RegExpUtility.isMatch(this.config.negativeNumberSignRegex, getExtResult.text) && result.value > 0){
                result.value = - result.value;
            }
            result.resolutionStr = this.toString(result.value);
        } else if (extra.includes("Pow")) {
            getExtResult.text = this.replaceFullWithHalf(getExtResult.text);
            result = this.powerNumberParse(getExtResult);
            result.resolutionStr = this.toString(result.value);
        } else if (extra.includes("Frac")) {
            result = this.fracParseCJK(getExtResult);
        } else if (extra.includes("Dou")) {
            result = this.douParseCJK(getExtResult);
        } else if (extra.includes("Integer")) {
            result = this.intParseCJK(getExtResult);
        } else if (extra.includes("Ordinal")) {
            result = this.ordParseCJK(getExtResult);
        }

        if (result) {
            result.text = extResult.text.toLowerCase();
        }

        return result;
    }

    private replaceTraditionalWithSimplified(value: string): string {
        if (StringUtility.isNullOrWhitespace(value)) {
            return value;
        }
        if (this.config.tratoSimMap == null) {
            return value;
        }

        let result = '';
        for(let index = 0; index < value.length; index++) {
            result = result.concat(this.config.tratoSimMap.get(value.charAt(index)) || value.charAt(index));
        }
        return result;
    }

    private replaceFullWithHalf(value: string): string {
        if (StringUtility.isNullOrWhitespace(value)) {
            return value;
        }

        let result = '';
        for(let index = 0; index < value.length; index++) {
            result = result.concat(this.config.fullToHalfMap.get(value.charAt(index)) || value.charAt(index));
        }
        return result;
    }

    private replaceUnit(value: string): string {
        if (StringUtility.isNullOrEmpty(value)) return value;
        let result = value;
        this.config.unitMap.forEach((value: string, key: string) => {
            result = result.replace(new RegExp(key, 'g'), value);
        });
        return result;
    }

    private perParseCJK(extResult: ExtractResult): ParseResult {
        let result = new ParseResult(extResult);

        let resultText = extResult.text;
        let power = 1;

        if (extResult.data.includes("Spe")) {
            resultText = this.replaceFullWithHalf(resultText);
            resultText = this.replaceUnit(resultText);

            if (resultText === "半額" || resultText === "半折" || resultText === "半折") {
                result.value = 50;
            } else if (resultText === "10成" || resultText === "10割" || resultText === "十割") {
                result.value = 100;
            } else {
                let matches = RegExpUtility.getMatches(this.config.speGetNumberRegex, resultText);
                let intNumber: number;

                if (matches.length === 2) {
                    let intNumberChar = matches[0].value.charAt(0);

                    if (intNumberChar === '対' || intNumberChar === "对") {
                        intNumber = 5;
                    } else if (intNumberChar === "十" || intNumberChar === "拾") {
                        intNumber = 10;
                    } else {
                        intNumber = this.config.zeroToNineMap.get(intNumberChar);
                    }

                    let pointNumberChar = matches[1].value.charAt(0);
                    let pointNumber: number;

                    if (pointNumberChar === "半") {
                        pointNumber = 0.5;
                    } else {
                        pointNumber = this.config.zeroToNineMap.get(pointNumberChar) * 0.1;
                    }

                    result.value = (intNumber + pointNumber) * 10;
                } else if (matches.length === 5) {
                    // Deal the Japanese percentage case like "xxx割xxx分xxx厘", get the integer value and convert into result.
                    let intNumberChar = matches[0].value.charAt(0);
                    let pointNumberChar = matches[1].value.charAt(0);
                    let dotNumberChar = matches[3].value.charAt(0);

                    let pointNumber = this.config.zeroToNineMap.get(pointNumberChar) * 0.1;
                    let dotNumber = this.config.zeroToNineMap.get(dotNumberChar) * 0.01;

                    intNumber = this.config.zeroToNineMap.get(intNumberChar);

                    result.value = (intNumber + pointNumber + dotNumber) * 10;
                } else {
                    let intNumberChar = matches[0].value.charAt(0);

                    if (intNumberChar === '対' || intNumberChar === "对") {
                        intNumber = 5;
                    } else if (intNumberChar === "十" || intNumberChar === "拾") {
                        intNumber = 10;
                    } else {
                        intNumber = this.config.zeroToNineMap.get(intNumberChar);
                    }
                    result.value = intNumber * 10;
                }
            }
        } else if (extResult.data.includes("Num")) {
            let doubleMatch = RegExpUtility.getMatches(this.config.percentageRegex, resultText).pop();
            let doubleText = doubleMatch.value;

            if (doubleText.includes("k") || doubleText.includes("K") || doubleText.includes("ｋ") || doubleText.includes("Ｋ")) {
                power = 1000;
            }

            if (doubleText.includes("M") || doubleText.includes("Ｍ")) {
                power = 1000000;
            }

            if (doubleText.includes("G") || doubleText.includes("Ｇ")) {
                power = 1000000000;
            }

            if (doubleText.includes("T") || doubleText.includes("Ｔ")) {
                power = 1000000000000;
            }

            result.value = this.getDigitValueCJK(resultText, power);
        } else {
            let doubleMatch = RegExpUtility.getMatches(this.config.percentageRegex, resultText).pop();
            let doubleText = this.replaceUnit(doubleMatch.value);

            let splitResult = RegExpUtility.split(this.config.pointRegex, doubleText);
            if (splitResult[0] === "") {
                splitResult[0] = "零"
            }

            let doubleValue = this.getIntValueCJK(splitResult[0]);
            if (splitResult.length === 2) {
                if (RegExpUtility.isMatch(this.config.negativeNumberSignRegex, splitResult[0])) {
                    doubleValue -= this.getPointValueCJK(splitResult[1]);
                } else {
                    doubleValue += this.getPointValueCJK(splitResult[1]);
                }
            }

            result.value = doubleValue;
        }

        result.resolutionStr = this.toString(result.value) + "%";
        return result;
    }

    private fracParseCJK(extResult: ExtractResult): ParseResult {
        let result = new ParseResult(extResult);

        let resultText = extResult.text;
        let splitResult = RegExpUtility.split(this.config.fracSplitRegex, resultText);
        let intPart = "";
        let demoPart = "";
        let numPart = "";
        if (splitResult.length === 3) {
            intPart = splitResult[0] || "";
            demoPart = splitResult[1] || "";
            numPart = splitResult[2] || "";
        } else {
            intPart = "零";
            demoPart = splitResult[0] || "";
            numPart = splitResult[1] || "";
        }

        let intValue = this.isDigitCJK(intPart)
            ? this.getDigitValueCJK(intPart, 1.0)
            : this.getIntValueCJK(intPart);

        let numValue = this.isDigitCJK(numPart)
            ? this.getDigitValueCJK(numPart, 1.0)
            : this.getIntValueCJK(numPart);

        let demoValue = this.isDigitCJK(demoPart)
            ? this.getDigitValueCJK(demoPart, 1.0)
            : this.getIntValueCJK(demoPart);

        if (RegExpUtility.isMatch(this.config.negativeNumberSignRegex, intPart)) {
            result.value = intValue - numValue / demoValue;
        } else {
            result.value = intValue + numValue / demoValue;
        }

        result.resolutionStr = this.toString(result.value);
        return result;
    }

    private douParseCJK(extResult: ExtractResult): ParseResult {
        let result = new ParseResult(extResult);
        
        let resultText = extResult.text;

        if (RegExpUtility.isMatch(this.config.doubleAndRoundRegex, resultText)) {
            resultText = this.replaceUnit(resultText);
            let power = this.config.roundNumberMapChar.get(resultText.charAt(resultText.length - 1));
            result.value = this.getDigitValueCJK(resultText.substr(0, resultText.length - 1), power);
        } else {
            resultText = this.replaceUnit(resultText);
            let splitResult = RegExpUtility.split(this.config.pointRegex, resultText);

            if (splitResult[0] === "") {
                splitResult[0] = "零";
            }

            if (RegExpUtility.isMatch(this.config.negativeNumberSignRegex, splitResult[0])) {
                result.value = this.getIntValueCJK(splitResult[0]) - this.getPointValueCJK(splitResult[1]);
            } else {
                result.value = this.getIntValueCJK(splitResult[0]) + this.getPointValueCJK(splitResult[1]);
            }
        }

        result.resolutionStr = this.toString(result.value);        
        return result;
    }

    private intParseCJK(extResult: ExtractResult): ParseResult {
        let result = new ParseResult(extResult);
        
        result.value = this.getIntValueCJK(extResult.text);
        result.resolutionStr = this.toString(result.value);        
        return result;
    }

    private ordParseCJK(extResult: ExtractResult): ParseResult {
        let result = new ParseResult(extResult);
        
        let resultText = extResult.text.substr(1);

        if (RegExpUtility.isMatch(this.config.digitNumRegex, resultText) && !RegExpUtility.isMatch(this.config.roundNumberIntegerRegex, resultText)) {
            result.value = this.getDigitValueCJK(resultText, 1);
        } else {
            result.value = this.getIntValueCJK(resultText);
        }

        result.resolutionStr = this.toString(result.value);        
        return result;
    }

    private getDigitValueCJK(value: string, power: number): number {
        let isNegative = false;
        let resultStr = value;
        if (RegExpUtility.isMatch(this.config.negativeNumberSignRegex, resultStr)) {
            isNegative = true;
            resultStr = resultStr.substr(1);
        }

        resultStr = this.replaceFullWithHalf(resultStr);
        let result = this.getDigitalValue(resultStr, power);
        if (isNegative) {
            result = - result;
        }

        return result;
    }

    private getIntValueCJK(value: string): number {
        let resultStr = value;

        let isDozen = false;
        let isPair = false;
        if (RegExpUtility.isMatch(this.config.dozenRegex, resultStr)) {
            isDozen = true;
            if (this.config.cultureInfo.code.toLowerCase() === Culture.Chinese) {
                resultStr = resultStr.substr(0, resultStr.length - 1);
            } else if (this.config.cultureInfo.code.toLowerCase() === Culture.Japanese) {
                resultStr = resultStr.substr(0, resultStr.length - 3);
            }
        } else if (RegExpUtility.isMatch(this.config.pairRegex, resultStr)) {
            isPair = true;
            resultStr = resultStr.substr(0, resultStr.length - 1);
        }

        resultStr = this.replaceUnit(resultStr);
        let intValue = 0;
        let partValue = 0;
        let beforeValue = 0;
        let isRoundBefore = false;
        let roundBefore = -1;
        let roundDefault = 1;
        let isNegative = false;
        let hasNumber = false;

        if (RegExpUtility.isMatch(this.config.negativeNumberSignRegex, resultStr)) {
            isNegative = true;
            resultStr = resultStr.substr(1);
        }

        for(let index = 0; index < resultStr.length; index++) {
            let currentChar = resultStr.charAt(index);
            if (this.config.roundNumberMapChar.has(currentChar)) {
                let roundRecent = this.config.roundNumberMapChar.get(currentChar);
                if (!hasNumber) {
                    beforeValue = 1;
                }
                if (roundBefore !== -1 && roundRecent > roundBefore) {
                    if (isRoundBefore) {
                        intValue += partValue * roundRecent;
                        isRoundBefore = false;
                    } else {
                        partValue += beforeValue * roundDefault;
                        intValue += partValue * roundRecent;
                    }

                    roundBefore = -1;
                    partValue = 0;
                } else {
                    isRoundBefore = true;
                    partValue += beforeValue * roundRecent;
                    roundBefore = roundRecent;

                    if ((index === resultStr.length - 1) || this.config.roundDirectList.some(o => o === currentChar)) {
                        intValue += partValue;
                        partValue = 0;
                    }
                }

                hasNumber = false;
                beforeValue = 0;
                roundDefault = roundRecent / 10;
            } else if (this.config.zeroToNineMap.has(currentChar)) {
                hasNumber = true;
                if (index !== resultStr.length - 1) {
                    if ((currentChar === "零") && !this.config.roundNumberMapChar.has(resultStr.charAt(index + 1))) {
                        roundDefault = 1;
                    } else {
                        beforeValue = beforeValue * 10 + this.config.zeroToNineMap.get(currentChar);
                        isRoundBefore = false;
                    }
                } else {
                    if (index === resultStr.length - 1 && this.config.cultureInfo.code.toLowerCase() === Culture.Japanese) {
                        roundDefault = 1;
                    }
                    partValue += beforeValue * 10;
                    partValue += this.config.zeroToNineMap.get(currentChar) * roundDefault;
                    intValue += partValue;
                    partValue = 0;
                }
            }
        }
 
        if (isNegative) {
            intValue = - intValue;
        }

        if (isDozen) {
            intValue = intValue * 12;
        }

        if (isPair) {
            intValue = intValue * 2;
        }

        return intValue;
    }

    private getPointValueCJK(value: string): number {
        let result = 0;
        let scale = 0.1;
        for(let index = 0; index < value.length; index++) {
            result += scale * this.config.zeroToNineMap.get(value.charAt(index));
            scale *= 0.1;
        }

        return result;
    }

    private isDigitCJK(value: string): boolean {
        return !StringUtility.isNullOrEmpty(value) 
            && RegExpUtility.isMatch(this.config.digitNumRegex, value);
    }
}