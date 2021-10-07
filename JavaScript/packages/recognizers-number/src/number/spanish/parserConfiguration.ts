// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

import { ParseResult } from "@microsoft/recognizers-text";
import { INumberParserConfiguration } from "../parsers";
import { CultureInfo, Culture } from "../../culture";
import { SpanishNumeric } from "../../resources/spanishNumeric";
import { RegExpUtility } from "@microsoft/recognizers-text";

export class SpanishNumberParserConfiguration implements INumberParserConfiguration {

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
    readonly writtenDecimalSeparatorTexts: readonly string[];
    readonly writtenGroupSeparatorTexts: readonly string[];
    readonly writtenIntegerSeparatorTexts: readonly string[];
    readonly writtenFractionSeparatorTexts: readonly string[];

    constructor(ci?: CultureInfo) {
        if (!ci) {
            ci = new CultureInfo(Culture.Spanish);
        }

        this.cultureInfo = ci;

        this.langMarker = SpanishNumeric.LangMarker;
        this.decimalSeparatorChar = SpanishNumeric.DecimalSeparatorChar;
        this.fractionMarkerToken = SpanishNumeric.FractionMarkerToken;
        this.nonDecimalSeparatorChar = SpanishNumeric.NonDecimalSeparatorChar;
        this.halfADozenText = SpanishNumeric.HalfADozenText;
        this.wordSeparatorToken = SpanishNumeric.WordSeparatorToken;

        this.writtenDecimalSeparatorTexts = SpanishNumeric.WrittenDecimalSeparatorTexts;
        this.writtenGroupSeparatorTexts = SpanishNumeric.WrittenGroupSeparatorTexts;
        this.writtenIntegerSeparatorTexts = SpanishNumeric.WrittenIntegerSeparatorTexts;
        this.writtenFractionSeparatorTexts = SpanishNumeric.WrittenFractionSeparatorTexts;

        let ordinalNumberMap = new Map<string, number>(SpanishNumeric.OrdinalNumberMap);

        SpanishNumeric.PrefixCardinalMap.forEach((prefixValue: number, prefixKey: string) => {
            SpanishNumeric.SuffixOrdinalMap.forEach((suffixValue: number, suffixKey: string) => {
                if (!ordinalNumberMap.has(prefixKey + suffixKey)) {
                    ordinalNumberMap.set(prefixKey + suffixKey, prefixValue * suffixValue);
                }
            });
        });

        this.cardinalNumberMap = SpanishNumeric.CardinalNumberMap;
        this.ordinalNumberMap = ordinalNumberMap;
        this.roundNumberMap = SpanishNumeric.RoundNumberMap;
        this.negativeNumberSignRegex = RegExpUtility.getSafeRegExp(SpanishNumeric.NegativeNumberSignRegex, "is");
        this.halfADozenRegex = RegExpUtility.getSafeRegExp(SpanishNumeric.HalfADozenRegex);
        this.digitalNumberRegex = RegExpUtility.getSafeRegExp(SpanishNumeric.DigitalNumberRegex);
    }

    normalizeTokenSet(tokens: readonly string[], context: ParseResult): readonly string[] {
        let result = new Array<string>();

        tokens.forEach((token: string) => {
            let tempWord = token.replace(/^s+/, '').replace(/s+$/, '');
            if (this.ordinalNumberMap.has(tempWord)) {
                result.push(tempWord);
                return;
            }

            if (tempWord.endsWith("avo") || tempWord.endsWith("ava")) {
                let origTempWord = tempWord;
                let newLength = origTempWord.length;
                tempWord = origTempWord.substring(0, newLength - 3);
                if (this.cardinalNumberMap.has(tempWord)) {
                    result.push(tempWord);
                    return;
                }
                else {
                    tempWord = origTempWord.substring(0, newLength - 2);
                    if (this.cardinalNumberMap.has(tempWord)) {
                        result.push(tempWord);
                        return;
                    }
                }
            }

            result.push(token);
        });

        return result;
    }

    resolveCompositeNumber(numberStr: string): number {
        if (this.ordinalNumberMap.has(numberStr)) {
            return this.ordinalNumberMap.get(numberStr);
        }

        if (this.cardinalNumberMap.has(numberStr)) {
            return this.cardinalNumberMap.get(numberStr);
        }

        let value = 0;
        let finalValue = 0;
        let strBuilder = "";
        let lastGoodChar = 0;
        for (let i = 0; i < numberStr.length; i++) {
            strBuilder = strBuilder.concat(numberStr[i]);
            if (this.cardinalNumberMap.has(strBuilder) && this.cardinalNumberMap.get(strBuilder) > value) {
                lastGoodChar = i;
                value = this.cardinalNumberMap.get(strBuilder);
            }
            if ((i + 1) === numberStr.length) {
                finalValue += value;
                strBuilder = "";
                i = lastGoodChar++;
                value = 0;
            }
        }
        return finalValue;
    }
}