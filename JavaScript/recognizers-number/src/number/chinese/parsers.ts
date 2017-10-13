import { BaseNumberParser, ParseResult, IParser } from "../parsers";
import { ChineseNumberParserConfiguration } from "./parserConfiguration";
import { Constants } from "../constants";
import { LongFormatType } from "../models";
import { ChineseNumeric } from "../../resources/chineseNumeric";
import { ExtractResult } from "../extractors";
import { CultureInfo, Culture } from "../../culture";
import { RegExpUtility, StringUtility } from "../../utilities";
import { BigNumber } from 'bignumber.js';
import trimEnd = require("lodash.trimend");
import sortBy = require("lodash.sortby");

export class ChineseNumberParser extends BaseNumberParser {
    readonly config: ChineseNumberParserConfiguration;

    constructor(config: ChineseNumberParserConfiguration) {
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
        let simplifiedExtResult: ExtractResult = {
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
            result = this.perParseChs(simplifiedExtResult);
        } else if (extra.includes("Num")) {
            simplifiedExtResult.text = this.replaceFullWithHalf(simplifiedExtResult.text);
            result = this.digitNumberParse(simplifiedExtResult);
            result.resolutionStr = this.toString(result.value);
        } else if (extra.includes("Pow")) {
            simplifiedExtResult.text = this.replaceFullWithHalf(simplifiedExtResult.text);
            result = this.powerNumberParse(simplifiedExtResult);
            result.resolutionStr = this.toString(result.value);
        } else if (extra.includes("Frac")) {
            result = this.fracParseChs(simplifiedExtResult);
        } else if (extra.includes("Dou")) {
            result = this.douParseChs(simplifiedExtResult);
        } else if (extra.includes("Integer")) {
            result = this.intParseChs(simplifiedExtResult);
        } else if (extra.includes("Ordinal")) {
            result = this.ordParseChs(simplifiedExtResult);
        }

        if (result) {
            result.text = extResult.text;
        }

        return result;
    }

    private replaceTraditionalWithSimplified(value: string): string {
        if (StringUtility.isNullOrWhitespace(value)) {
            return value;
        }

        let result = '';
        for(let index = 0; index < value.length; index++) {
            result = result.concat(this.config.tratoSimMapChs.get(value.charAt(index)) || value.charAt(index));
        }
        return result;
    }

    private replaceFullWithHalf(value: string): string {
        if (StringUtility.isNullOrWhitespace(value)) {
            return value;
        }

        let result = '';
        for(let index = 0; index < value.length; index++) {
            result = result.concat(this.config.fullToHalfMapChs.get(value.charAt(index)) || value.charAt(index));
        }
        return result;
    }

    private replaceUnit(value: string): string {
        if (StringUtility.isNullOrEmpty(value)) return value;
        let result = value;
        this.config.unitMapChs.forEach((value, key) => {
            result = result.replace(key, value);
        });
        return result;
    }

    private perParseChs(extResult: ExtractResult): ParseResult {
        let result = new ParseResult(extResult);

        let resultText = extResult.text;
        let power = 1;

        if (extResult.data.includes("Spe")) {
            resultText = this.replaceFullWithHalf(resultText);
            resultText = this.replaceUnit(resultText);

            if (resultText === "半折") {
                result.value = 50;
            } else if (resultText === "10成") {
                result.value = 100;
            } else {
                let matches = RegExpUtility.getMatches(this.config.speGetNumberRegex, resultText);
                let intNumber: number;

                if (matches.length === 2) {
                    let intNumberChar = matches[0].value.charAt(0);

                    if (intNumberChar === "对") {
                        intNumber = 5;
                    } else if (intNumberChar === "十" || intNumberChar === "拾") {
                        intNumber = 10;
                    } else {
                        intNumber = this.config.zeroToNineMapChs.get(intNumberChar);
                    }

                    let pointNumberChar = matches[1].value.charAt(0);
                    let pointNumber: number;

                    if (pointNumberChar === "半") {
                        pointNumber = 0.5;
                    } else {
                        pointNumber = this.config.zeroToNineMapChs.get(pointNumberChar) * 0.1;
                    }

                    result.value = (intNumber + pointNumber) * 10;
                } else {
                    let intNumberChar = matches[0].value.charAt(0);

                    if (intNumberChar === "对") {
                        intNumber = 5;
                    } else if (intNumberChar === "十" || intNumberChar === "拾") {
                        intNumber = 10;
                    } else {
                        intNumber = this.config.zeroToNineMapChs.get(intNumberChar);
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

            result.value = this.getDigitValueChs(resultText, power);
        } else {
            let doubleMatch = RegExpUtility.getMatches(this.config.percentageRegex, resultText).pop();
            let doubleText = this.replaceUnit(doubleMatch.value);

            let splitResult = RegExpUtility.split(this.config.pointRegexChs, doubleText);
            if (splitResult[0] === "") {
                splitResult[0] = "零"
            }

            let doubleValue = this.getIntValueChs(splitResult[0]);
            if (splitResult.length === 2) {
                if (RegExpUtility.isMatch(this.config.symbolRegex, splitResult[0])) {
                    doubleValue -= this.getPointValueChs(splitResult[1]);
                } else {
                    doubleValue += this.getPointValueChs(splitResult[1]);
                }
            }

            result.value = doubleValue;
        }

        result.resolutionStr = this.toString(result.value) + "%";
        return result;
    }

    private fracParseChs(extResult: ExtractResult): ParseResult {
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

        let intValue = this.isDigitChs(intPart)
            ? this.getDigitValueChs(intPart, 1.0)
            : this.getIntValueChs(intPart);

        let numValue = this.isDigitChs(numPart)
            ? this.getDigitValueChs(numPart, 1.0)
            : this.getIntValueChs(numPart);

        let demoValue = this.isDigitChs(demoPart)
            ? this.getDigitValueChs(demoPart, 1.0)
            : this.getIntValueChs(demoPart);

        if (RegExpUtility.isMatch(this.config.symbolRegex, intPart)) {
            result.value = intValue - numValue / demoValue;
        } else {
            result.value = intValue + numValue / demoValue;
        }

        result.resolutionStr = this.toString(result.value);
        return result;
    }

    private douParseChs(extResult: ExtractResult): ParseResult {
        let result = new ParseResult(extResult);
        
        let resultText = extResult.text;

        if (RegExpUtility.isMatch(this.config.doubleAndRoundChsRegex, resultText)) {
            resultText = this.replaceUnit(resultText);
            let power = this.config.roundNumberMapChs.get(resultText.charAt(resultText.length - 1));
            result.value = this.getDigitValueChs(resultText.substr(0, resultText.length - 1), power);
        } else {
            resultText = this.replaceUnit(resultText);
            let splitResult = RegExpUtility.split(this.config.pointRegexChs, resultText);

            if (splitResult[0] === "") {
                splitResult[0] = "零";
            }

            if (RegExpUtility.getMatches(this.config.symbolRegex, splitResult[0])) {
                result.value = this.getIntValueChs(splitResult[0]) - this.getPointValueChs(splitResult[1]);
            } else {
                result.value = this.getIntValueChs(splitResult[0]) + this.getPointValueChs(splitResult[1]);
            }
        }

        result.resolutionStr = this.toString(result.value);        
        return result;
    }

    private intParseChs(extResult: ExtractResult): ParseResult {
        let result = new ParseResult(extResult);
        
        result.value = this.getIntValueChs(extResult.text);
        result.resolutionStr = this.toString(result.value);        
        return result;
    }

    private ordParseChs(extResult: ExtractResult): ParseResult {
        let result = new ParseResult(extResult);
        
        let resultText = extResult.text.substr(1);

        if (RegExpUtility.isMatch(this.config.digitNumRegex, resultText)) {
            result.value = this.getDigitValueChs(resultText, 1);
        } else {
            result.value = this.getIntValueChs(resultText);
        }

        result.resolutionStr = this.toString(result.value);        
        return result;
    }

    private getDigitValueChs(value: string, power: number): number {
        let isLessZero = false;
        let resultStr = value;
        if (RegExpUtility.isMatch(this.config.symbolRegex, resultStr)) {
            isLessZero = true;
            resultStr = resultStr.substr(1);
        }

        resultStr = this.replaceFullWithHalf(resultStr);
        let result = this.getDigitalValue(resultStr, power);
        if (isLessZero) {
            result = - result;
        }

        return result;
    }

    private getIntValueChs(value: string): number {
        let resultStr = value;

        let isDozen = false;
        let isPair = false;
        if (RegExpUtility.isMatch(this.config.dozenRegex, resultStr)) {
            isDozen = true;
            resultStr = resultStr.substr(0, resultStr.length - 1);
        } else if (RegExpUtility.isMatch(this.config.pairRegex, resultStr)) {
            isPair = true;
            resultStr = resultStr.substr(0, resultStr.length - 1);
        }

        resultStr = this.replaceUnit(resultStr);
        let intValue = 0;
        let partValue = 0;
        let beforeValue = 1;
        let isRoundBefore = false;
        let roundBefore = -1;
        let roundDefault = 1;
        let isLessZero = false;

        if (RegExpUtility.isMatch(this.config.symbolRegex, resultStr)) {
            isLessZero = true;
            resultStr = resultStr.substr(1);
        }

        for(let index = 0; index < resultStr.length; index++) {
            let resultChar = resultStr.charAt(index);
            if (this.config.roundNumberMapChs.has(resultChar)) {
                let roundRecent = this.config.roundNumberMapChs.get(resultChar);
                if (roundBefore !== -1 && roundRecent > roundBefore) {
                    if (roundBefore) {
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

                    if ((index === resultStr.length - 1) || this.config.roundDirectListChs.some(o => o === resultChar)) {
                        intValue += partValue;
                        partValue = 0;
                    }
                }

                roundDefault = roundRecent / 10;
            } else if (this.config.zeroToNineMapChs.has(resultChar)) {
                if (index !== resultStr.length - 1) {
                    if ((resultChar === "零") && !this.config.roundNumberMapChs.has(resultStr.charAt(index + 1))) {
                        beforeValue = 1;
                        roundDefault = 1;
                    } else {
                        beforeValue = this.config.zeroToNineMapChs.get(resultChar);
                        isRoundBefore = false;
                    }
                } else {
                    partValue += this.config.zeroToNineMapChs.get(resultChar) * roundDefault;
                    intValue += partValue;
                    partValue = 0;
                }
            }
        }

        if (isLessZero) {
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

    private getPointValueChs(value: string): number {
        let result = 0;
        let scale = 0.1;
        for(let index = 0; index < value.length; index++) {
            result += scale * this.config.zeroToNineMapChs.get(value.charAt(index));
            scale *= 0.1;
        }

        return result;
    }

    private isDigitChs(value: string): boolean {
        return !StringUtility.isNullOrEmpty(value) 
            && RegExpUtility.isMatch(this.config.digitNumRegex, value);
    }

    protected getDigitalValue(digitStr: string, power: number): number {
        let tmp = new BigNumber(0);
        let scale = new BigNumber(10);
        let dot = false;
        let isLessZero = false;
        let isFrac = false;

        let calStack = new Array<BigNumber>();

        for (let i = 0; i < digitStr.length; i++) {
            let ch = digitStr[i];
            if (ch === '/') {
                isFrac = true;
            }

            if (ch === ' ' || ch === '/') {
                calStack.push(tmp);
                tmp = new BigNumber(0);
            }
            else if (ch >= '0' && ch <= '9') {
                if (dot) {
                    // tmp = tmp + scale * (ch.charCodeAt(0) - 48);
                    // scale *= 0.1;
                    tmp = tmp.plus(scale.times(ch.charCodeAt(0) - 48));
                    scale = scale.times(0.1);
                }
                else {
                    // tmp = tmp * scale + (ch.charCodeAt(0) - 48);
                    tmp = tmp.times(scale).plus(ch.charCodeAt(0) - 48);
                }
            }
            else if (ch === this.config.decimalSeparatorChar) {
                dot = true;
                scale = new BigNumber(0.1);
            }
            else if (ch === '-') {
                isLessZero = true;
            }
        }
        calStack.push(tmp);

        // is the number is a fraction.
        let calResult = new BigNumber(0);
        if (isFrac) {
            let deno = calStack.pop();
            let mole = calStack.pop();
            // calResult += mole / deno;
            calResult = calResult.plus(mole.dividedBy(deno));
        }

        while (calStack.length > 0) {
            calResult = calResult.plus(calStack.pop());
        }

        // calResult *= power;
        calResult = calResult.times(power);

        if (isLessZero) {
            return calResult.negated().toNumber();
        }


        return calResult.toNumber();
    }

    
    protected digitNumberParse(extResult: ExtractResult): ParseResult {
        let result: ParseResult = {
            start: extResult.start,
            length: extResult.length,
            text: extResult.text,
            type: extResult.type
        };

        // [1] 24
        // [2] 12 32/33
        // [3] 1,000,000
        // [4] 234.567
        // [5] 44/55
        // [6] 2 hundred
        // dot occured.

        let power: number = 1;
        let tmpIndex: number = -1;
        let startIndex: number = 0;
        let handle = extResult.text.toLowerCase();

        let matches = RegExpUtility.getMatches(this.config.digitalNumberRegex, handle);
        if (matches) {
            matches.forEach(match => {
                // HACK: Matching regex may be buggy, may include a digit before the unit
                match.value = match.value.replace(/\d/g, '');
                match.length = match.value.length;

                let rep: number = this.config.roundNumberMap.get(match.value) as number;
                // \\s+ for filter the spaces.
                power *= rep;

                // tslint:disable-next-line:no-conditional-assignment
                while ((tmpIndex = handle.indexOf(match.value, startIndex)) >= 0) {
                    let front = trimEnd(handle.substring(0, tmpIndex));
                    startIndex = front.length;
                    handle = front + handle.substring(tmpIndex + match.length);
                }
            });
        }

        // scale used in the calculate of double
        result.value = this.getDigitalValue(handle, power);

        return result;
    }

    
    protected powerNumberParse(extResult: ExtractResult): ParseResult {
        let result =
            {
                start: extResult.start,
                length: extResult.length,
                text: extResult.text,
                type: extResult.type
            } as ParseResult;

        let handle = extResult.text.toUpperCase();
        let isE = !extResult.text.includes("^");

        // [1] 1e10
        // [2] 1.1^-23
        let calStack = new Array<BigNumber>();

        let scale = new BigNumber(10);
        let dot = false;
        let isLessZero = false;
        let tmp = new BigNumber(0);
        for (let i = 0; i < handle.length; i++) {
            let ch = handle[i];
            if (ch === '^' || ch === 'E') {
                if (isLessZero) {
                    calStack.push(tmp.negated());
                }
                else {
                    calStack.push(tmp);
                }
                tmp = new BigNumber(0);
                scale = new BigNumber(10);
                dot = false;
                isLessZero = false;
            }
            else if (ch.charCodeAt(0) - 48 >= 0 && ch.charCodeAt(0) - 48 <= 9) {
                if (dot) {
                    // tmp = tmp + scale * (ch.charCodeAt(0) - 48);
                    // scale *= 0.1;
                    tmp = tmp.plus(scale.times(ch.charCodeAt(0) - 48));
                    scale = scale.times(0.1);
                }
                else {
                    // tmp = tmp * scale + (ch.charCodeAt(0) - 48);
                    tmp = tmp.times(scale).plus(ch.charCodeAt(0) - 48);
                }
            }
            else if (ch === this.config.decimalSeparatorChar) {
                dot = true;
                scale = new BigNumber(0.1);
            }
            else if (ch === '-') {
                isLessZero = !isLessZero;
            }
            else if (ch === '+') {
                continue;
            }

            if (i === handle.length - 1) {
                if (isLessZero) {
                    calStack.push(tmp.negated());
                }
                else {
                    calStack.push(tmp);
                }
            }
        }

        let ret = 0;
        if (isE) {
            // ret = calStack.shift() * Math.pow(10, calStack.shift());
            ret = calStack.shift().times(Math.pow(10, calStack.shift().toNumber())).toNumber();
        }
        else {
            ret = Math.pow(calStack.shift().toNumber(), calStack.shift().toNumber());
        }

        result.value = ret;
        result.resolutionStr = ret.toString(); // @TODO Possible Culture bug.

        return result;
    }
}