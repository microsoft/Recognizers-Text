import { INumberParserConfiguration, ParseResult } from "../parsers";
import { CultureInfo, Culture } from "../../culture";
import { SpanishNumeric } from "../../resources/spanishNumeric";
import { RegExpUtility } from "../../utilities"

export class SpanishNumberParserConfiguration implements INumberParserConfiguration {

    readonly cardinalNumberMap: ReadonlyMap<string, number>;
    readonly ordinalNumberMap: ReadonlyMap<string, number>;
    readonly roundNumberMap: ReadonlyMap<string, number>;
    readonly cultureInfo: CultureInfo;
    readonly digitalNumberRegex: RegExp;
    readonly fractionMarkerToken: string;
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

        let ordinalNumberMap = new Map<string, number>(SpanishNumeric.SimpleOrdinalNumberMap);

        SpanishNumeric.PrefixCardinalDictionary.forEach((prefixValue: number, prefixKey: string) => {
            SpanishNumeric.SufixOrdinalDictionary.forEach((suffixValue: number, suffixKey: string) => {
                if (!ordinalNumberMap.has(prefixKey + suffixKey)) {
                    ordinalNumberMap.set(prefixKey + suffixKey, prefixValue * suffixValue);
                }
            });
        });

        this.cardinalNumberMap = SpanishNumeric.CardinalNumberMap;
        this.ordinalNumberMap = ordinalNumberMap;
        this.roundNumberMap = SpanishNumeric.RoundNumberMap;
        this.halfADozenRegex = RegExpUtility.getSafeRegExp(SpanishNumeric.HalfADozenRegex);
        this.digitalNumberRegex = RegExpUtility.getSafeRegExp(SpanishNumeric.DigitalNumberRegex);
    }

    normalizeTokenSet(tokens: ReadonlyArray<string>, context: ParseResult): ReadonlyArray<string> {
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