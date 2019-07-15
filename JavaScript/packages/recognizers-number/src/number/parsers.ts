import { IParser, ParseResult, ExtractResult } from "@microsoft/recognizers-text";
import { CultureInfo, Culture } from "../culture";
import { Constants } from "./constants";
import trimEnd = require("lodash.trimend");
import sortBy = require("lodash.sortby");
import { RegExpUtility } from "@microsoft/recognizers-text";
import { BigNumber } from 'bignumber.js/bignumber';

// The exponent value(s) at which toString returns exponential notation.
BigNumber.config({ EXPONENTIAL_AT: [-5, 15] });

export interface INumberParserConfiguration {
    readonly cardinalNumberMap: ReadonlyMap<string, number>;
    readonly ordinalNumberMap: ReadonlyMap<string, number>;
    readonly roundNumberMap: ReadonlyMap<string, number>;
    readonly cultureInfo: CultureInfo;
    readonly digitalNumberRegex: RegExp;
    readonly fractionMarkerToken: string;
    readonly negativeNumberSignRegex: RegExp;
    readonly halfADozenRegex: RegExp;
    readonly halfADozenText: string;
    readonly langMarker: string;
    readonly nonDecimalSeparatorChar: string;
    readonly decimalSeparatorChar: string;
    readonly wordSeparatorToken: string;
    readonly writtenDecimalSeparatorTexts: ReadonlyArray<string>;
    readonly writtenGroupSeparatorTexts: ReadonlyArray<string>;
    readonly writtenIntegerSeparatorTexts: ReadonlyArray<string>;
    readonly writtenFractionSeparatorTexts: ReadonlyArray<string>;

    normalizeTokenSet(tokens: ReadonlyArray<string>, context: ParseResult): ReadonlyArray<string>;
    resolveCompositeNumber(numberStr: string): number;
}

export class BaseNumberParser implements IParser {
    protected readonly config: INumberParserConfiguration;
    protected readonly textNumberRegex: RegExp;
    protected readonly arabicNumberRegex: RegExp;
    protected readonly roundNumberSet: Set<string>;

    supportedTypes: ReadonlyArray<string>;

    constructor(config: INumberParserConfiguration) {
        this.config = config;

        let singleIntFrac = `${this.config.wordSeparatorToken}| -|${this.getKeyRegex(this.config.cardinalNumberMap)}|${this.getKeyRegex(this.config.ordinalNumberMap)}`;

        this.textNumberRegex = RegExpUtility.getSafeRegExp(String.raw`(?=\b)(${singleIntFrac})(?=\b)`, "gis");
        this.arabicNumberRegex = RegExpUtility.getSafeRegExp(String.raw`\d+`, "is");
        this.roundNumberSet = new Set<string>();
        this.config.roundNumberMap.forEach((value, key) =>
            this.roundNumberSet.add(key)
        );
    }

    parse(extResult: ExtractResult): ParseResult | null {

        // check if the parser is configured to support specific types
        if (this.supportedTypes && !this.supportedTypes.find(t => t === extResult.type)) {
            return null;
        }

        let ret: ParseResult | null = null;
        let extra = extResult.data as string;
        if (!extra) {
            if (this.arabicNumberRegex.test(extResult.text)) {
                extra = "Num";
            } else {
                extra = this.config.langMarker;
            }
        }
        
        // Resolve symbol prefix
        let isNegative = false;
        let matchNegative = extResult.text.match(this.config.negativeNumberSignRegex);

        if (matchNegative)
        {
            isNegative = true;
            extResult.text = extResult.text.substr(matchNegative[1].length);
        }

        if (extra.includes("Num")) {
            ret = this.digitNumberParse(extResult);
        }
        else if (extra.includes(`Frac${this.config.langMarker}`)) // Frac is a special number, parse via another method
        {
            ret = this.fracLikeNumberParse(extResult);
        }
        else if (extra.includes(this.config.langMarker)) {
            ret = this.textNumberParse(extResult);
        }
        else if (extra.includes("Pow")) {
            ret = this.powerNumberParse(extResult);
        }

        if (ret && ret.value !== null) {
            if (isNegative)
            {
                // Recover to the original extracted Text
                ret.text = matchNegative[1] + extResult.text;
                // Check if ret.value is a BigNumber
                if (typeof ret.value === "number") {
                    ret.value = -ret.value;
                }
                else {
                    ret.value.s = -1;
                }
            }

            ret.resolutionStr = this.config.cultureInfo
                ? this.config.cultureInfo.format(ret.value)
                : ret.value.toString();

            ret.text = ret.text.toLowerCase();
        }

        return ret;
    }

    protected getKeyRegex(regexMap: ReadonlyMap<string, number>): string {
        let keys = new Array<string>();
        regexMap.forEach((value, key) => keys.push(key));
        let sortKeys = sortBy(keys, key => key.length).reverse();
        return sortKeys.join('|');
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

    protected isDigit(c: string): boolean {
        return c >= '0' && c <= '9';
    }

    protected fracLikeNumberParse(extResult: ExtractResult): ParseResult {

        let result =
            {
                start: extResult.start,
                length: extResult.length,
                text: extResult.text,
                type: extResult.type
            } as ParseResult;

        let resultText = extResult.text.toLowerCase();
        if (resultText.includes(this.config.fractionMarkerToken)) {
            let overIndex = resultText.indexOf(this.config.fractionMarkerToken);
            let smallPart = resultText.substring(0, overIndex).trim();
            let bigPart = resultText.substring(overIndex + this.config.fractionMarkerToken.length, resultText.length).trim();

            let smallValue = this.isDigit(smallPart[0])
                ? this.getDigitalValue(smallPart, 1)
                : this.getIntValue(this.getMatches(smallPart));

            let bigValue = this.isDigit(bigPart[0])
                ? this.getDigitalValue(bigPart, 1)
                : this.getIntValue(this.getMatches(bigPart));

            result.value = smallValue / bigValue;
        }
        else {
            let words = resultText.split(" ").filter(s => s && s.length);
            let fracWords = Array.from(this.config.normalizeTokenSet(words, result));

            // Split fraction with integer
            let splitIndex = fracWords.length - 1;
            let currentValue = this.config.resolveCompositeNumber(fracWords[splitIndex]);
            let roundValue = 1;
            
            if (fracWords.length == 1){
                result.value = 1 / this.getIntValue(fracWords);
                return result
            }
            
            for (splitIndex = fracWords.length - 2; splitIndex >= 0; splitIndex--) {

                if (this.config.writtenFractionSeparatorTexts.indexOf(fracWords[splitIndex]) > -1 ||
                    this.config.writtenIntegerSeparatorTexts.indexOf(fracWords[splitIndex]) > -1) {
                    continue;
                }

                let previousValue = currentValue;
                currentValue = this.config.resolveCompositeNumber(fracWords[splitIndex]);

                let smHundreds = 100;

                // previous : hundred
                // current : one
                if ((previousValue >= smHundreds && previousValue > currentValue)
                    || (previousValue < smHundreds && this.isComposable(currentValue, previousValue))) {
                    if (previousValue < smHundreds && currentValue >= roundValue) {
                        roundValue = currentValue;
                    }
                    else if (previousValue < smHundreds && currentValue < roundValue) {
                        splitIndex++;
                        break;
                    }
                    // current is the first word
                    if (splitIndex === 0) {
                        // scan, skip the first word
                        splitIndex = 1;
                        while (splitIndex <= fracWords.length - 2) {
                            // e.g. one hundred thousand
                            // frac[i+1] % 100 && frac[i] % 100 = 0
                            if (this.config.resolveCompositeNumber(fracWords[splitIndex]) >= smHundreds
                                && !(this.config.writtenFractionSeparatorTexts.indexOf(fracWords[splitIndex + 1]) > -1)
                                && this.config.resolveCompositeNumber(fracWords[splitIndex + 1]) < smHundreds) {
                                splitIndex++;
                                break;
                            }
                            splitIndex++;
                        }
                        break;
                    }
                    continue;
                }
                splitIndex++;
                break;
            }

            let fracPart = new Array<string>();
            for (let i = splitIndex; i < fracWords.length; i++) {
                if (fracWords[i].indexOf("-") > -1) {
                    let split = fracWords[i].split('-');
                    fracPart.push(split[0]);
                    fracPart.push("-");
                    fracPart.push(split[1]);
                }
                else {
                    fracPart.push(fracWords[i]);
                }
            }

            fracWords.splice(splitIndex, fracWords.length - splitIndex);

            // denomi = denominator
            let denomiValue = this.getIntValue(fracPart);
            // Split mixed number with fraction
            let numerValue = 0;
            let intValue = 0;

            let mixedIndex = fracWords.length;
            for (let i = fracWords.length - 1; i >= 0; i--) {
                if (i < fracWords.length - 1 && this.config.writtenFractionSeparatorTexts.indexOf(fracWords[i]) > -1) {
                    let numerStr = fracWords.slice(i + 1, fracWords.length).join(" ");
                    numerValue = this.getIntValue(this.getMatches(numerStr));
                    mixedIndex = i + 1;
                    break;
                }
            }

            let intStr = fracWords.slice(0, mixedIndex).join(" ");
            intValue = this.getIntValue(this.getMatches(intStr));

            // Find mixed number
            if (mixedIndex !== fracWords.length && numerValue < denomiValue) {
                // intValue + numerValue / denomiValue
                result.value = new BigNumber(intValue).plus(new BigNumber(numerValue).dividedBy(denomiValue));
            }
            else {
                // (intValue + numerValue) / denomiValue
                result.value = new BigNumber(intValue + numerValue).dividedBy(denomiValue)
            }
        }

        return result;
    }

    protected textNumberParse(extResult: ExtractResult): ParseResult {
        let result =
            {
                start: extResult.start,
                length: extResult.length,
                text: extResult.text,
                type: extResult.type
            } as ParseResult;

        let handle = extResult.text.toLowerCase();

        handle = handle.replace(this.config.halfADozenRegex, this.config.halfADozenText)

        let numGroup = this.splitMulti(handle, Array.from(this.config.writtenDecimalSeparatorTexts)).filter(s => s && s.length > 0);

        let intPart = numGroup[0];

        let matchStrs = (intPart && intPart.match(this.textNumberRegex))
            ? intPart.match(this.textNumberRegex).map(s => s.toLowerCase())
            : new Array<string>();

        // Get the value recursively
        let intPartRet = this.getIntValue(matchStrs);

        let pointPartRet = 0;
        if (numGroup.length === 2) {
            let pointPart = numGroup[1];
            let matchStrs = pointPart.match(this.textNumberRegex).map(s => s.toLowerCase());
            pointPartRet += this.getPointValue(matchStrs);
        }

        result.value = intPartRet + pointPartRet;

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
        let isNegative = false;
        let tmp = new BigNumber(0);
        for (let i = 0; i < handle.length; i++) {
            let ch = handle[i];
            if (ch === '^' || ch === 'E') {
                if (isNegative) {
                    calStack.push(tmp.negated());
                }
                else {
                    calStack.push(tmp);
                }
                tmp = new BigNumber(0);
                scale = new BigNumber(10);
                dot = false;
                isNegative = false;
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
                isNegative = !isNegative;
            }
            else if (ch === '+') {
                continue;
            }

            if (i === handle.length - 1) {
                if (isNegative) {
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

    private splitMulti(str: string, tokens: Array<string>): Array<string> {
        let tempChar = tokens[0]; // We can use the first token as a temporary join character
        for (let i = 0; i < tokens.length; i++) {
            str = str.split(tokens[i]).join(tempChar);
        }
        return str.split(tempChar);
    }

    private getMatches(input: string): Array<string> {
        let matches = input.match(this.textNumberRegex);
        return (matches || []).map(match => {
            return match.toLowerCase();
        });
    }

    // Test if big and combine with small.
    // e.g. "hundred" can combine with "thirty" but "twenty" can't combine with "thirty".
    private isComposable(big: number, small: number): boolean {
        let baseNumber = small > 10 ? 100 : 10;

        if (big % baseNumber === 0 && big / baseNumber >= 1) {
            return true;
        }

        return false;
    }

    private getIntValue(matchStrs: Array<string>): number {
        let isEnd = new Array<boolean>(matchStrs.length);
        for (let i = 0; i < isEnd.length; i++) {
            isEnd[i] = false;
        }

        let tempValue = 0;
        let endFlag = 1;

        // Scan from end to start, find the end word
        for (let i = matchStrs.length - 1; i >= 0; i--) {
            if (this.roundNumberSet.has(matchStrs[i])) {
                // if false,then continue
                // You will meet hundred first, then thousand.
                if (endFlag > this.config.roundNumberMap.get(matchStrs[i])) {
                    continue;
                }
                isEnd[i] = true;
                endFlag = this.config.roundNumberMap.get(matchStrs[i]);
            }
        }

        if (endFlag === 1) {
            let tempStack = new Array<number>();
            let oldSym = "";
            matchStrs.forEach(matchStr => {
                let isCardinal = this.config.cardinalNumberMap.has(matchStr);
                let isOrdinal = this.config.ordinalNumberMap.has(matchStr);
                if (isCardinal || isOrdinal) {
                    let matchValue = isCardinal
                        ? this.config.cardinalNumberMap.get(matchStr)
                        : this.config.ordinalNumberMap.get(matchStr);

                    // This is just for ordinal now. Not for fraction ever.
                    if (isOrdinal) {
                        let fracPart = this.config.ordinalNumberMap.get(matchStr);
                        if (tempStack.length > 0) {
                            let intPart = tempStack.pop();
                            // if intPart >= fracPart, it means it is an ordinal number
                            // it begins with an integer, ends with an ordinal
                            // e.g. ninety-ninth
                            if (intPart >= fracPart) {
                                tempStack.push(intPart + fracPart);
                            }
                            // another case of the type is ordinal
                            // e.g. three hundredth
                            else {
                                while (tempStack.length > 0) {
                                    intPart = intPart + tempStack.pop();
                                }
                                tempStack.push(intPart * fracPart);
                            }
                        }
                        else {
                            tempStack.push(fracPart);
                        }
                    }
                    else if (this.config.cardinalNumberMap.has(matchStr)) {
                        if (oldSym === "-") {
                            let sum = tempStack.pop() + matchValue;
                            tempStack.push(sum);
                        }
                        else if (oldSym === this.config.writtenIntegerSeparatorTexts[0] || tempStack.length < 2) {
                            tempStack.push(matchValue);
                        }
                        else if (tempStack.length >= 2) {
                            let sum = tempStack.pop() + matchValue;
                            sum = tempStack.pop() + sum;
                            tempStack.push(sum);
                        }
                    }
                }
                else {
                    let complexValue = this.config.resolveCompositeNumber(matchStr);
                    if (complexValue !== 0) {
                        tempStack.push(complexValue);
                    }
                }
                oldSym = matchStr;
            });

            tempStack.forEach(stackValue => {
                tempValue += stackValue;
            });
        }
        else {
            let lastIndex = 0;
            let mulValue = 1;
            let partValue = 1;
            for (let i = 0; i < isEnd.length; i++) {
                if (isEnd[i]) {
                    mulValue = this.config.roundNumberMap.get(matchStrs[i]);
                    partValue = 1;

                    if (i !== 0) {
                        partValue = this.getIntValue(matchStrs.slice(lastIndex, i));
                    }

                    tempValue += mulValue * partValue;
                    lastIndex = i + 1;
                }
            }

            // Calculate the part like "thirty-one"
            mulValue = 1;
            if (lastIndex !== isEnd.length) {
                partValue = this.getIntValue(matchStrs.slice(lastIndex, isEnd.length));
                tempValue += mulValue * partValue;
            }
        }

        return tempValue;
    }

    private getPointValue(matchStrs: Array<string>): number {
        let ret = 0;
        let firstMatch = matchStrs[0];

        if (this.config.cardinalNumberMap.has(firstMatch) && this.config.cardinalNumberMap.get(firstMatch) >= 10) {
            let prefix = "0.";
            let tempInt = this.getIntValue(matchStrs);
            let all = prefix + tempInt;
            ret = parseFloat(all);
        }
        else {
            let scale = new BigNumber(0.1);
            for (let i = 0; i < matchStrs.length; i++) {
                ret += scale.times(this.config.cardinalNumberMap.get(matchStrs[i])).toNumber();
                // scale *= 0.1;
                scale = scale.times(0.1);
            }
        }

        return ret;
    }

    private skipNonDecimalSeparator(ch: string, distance: number, culture: CultureInfo) {
        var decimalLength = 3;

        // Special cases for multi-language countries where decimal separators can be used interchangeably. Mostly informally.
        // Ex: South Africa, Namibia; Puerto Rico in ES; or in Canada for EN and FR.
        // "me pidio $5.00 prestados" and "me pidio $5,00 prestados" -> currency $5
        var cultureRegex = RegExpUtility.getSafeRegExp(String.raw`^(en|es|fr)(-)?\b`, "is");

        return (ch == this.config.nonDecimalSeparatorChar && !(distance <= decimalLength && (cultureRegex.exec(culture.code) !== null)) );
    }

    protected getDigitalValue(digitsStr: string, power: number): number {
        let tmp = new BigNumber(0);
        let scale = new BigNumber(10);
        let decimalSeparator = false;
        var strLength = digitsStr.length;
        let isNegative = false;
        let isFrac = digitsStr.includes('/');

        let calStack = new Array<BigNumber>();

        for (let i = 0; i < digitsStr.length; i++) {
            
            let ch = digitsStr[i];
            var skippableNonDecimal = this.skipNonDecimalSeparator(ch, strLength - i, this.config.cultureInfo);

            if (!isFrac && (ch === ' ' || skippableNonDecimal)) {
                continue;
            }

            if (ch === ' ' || ch === '/') {
                calStack.push(tmp);
                tmp = new BigNumber(0);
            }
            else if (ch >= '0' && ch <= '9') {
                if (decimalSeparator) {
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
            else if (ch === this.config.decimalSeparatorChar || (!skippableNonDecimal && ch == this.config.nonDecimalSeparatorChar)) {
                decimalSeparator = true;
                scale = new BigNumber(0.1);
            }
            else if (ch === '-') {
                isNegative = true;
            }
        }
        calStack.push(tmp);

        // if the number is a fraction.
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

        if (isNegative) {
            return calResult.negated().toNumber();
        }


        return calResult.toNumber();
    }
}

export class BasePercentageParser extends BaseNumberParser {
    parse(extResult: ExtractResult): ParseResult | null {

        let originText = extResult.text;

        // do replace text & data from extended info
        if (extResult.data && extResult.data instanceof Array) {
            extResult.text = extResult.data[0];
            extResult.data = extResult.data[1].data;
        }

        let ret = super.parse(extResult) as ParseResult;

        if (ret.resolutionStr && ret.resolutionStr.length > 0) {
            if (!ret.resolutionStr.trim().endsWith("%")) {
                ret.resolutionStr = ret.resolutionStr.trim() + "%";
            }
        }

        ret.data = extResult.text;
        ret.text = originText;

        return ret;
    }
}