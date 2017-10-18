import { INumberParserConfiguration, ParseResult } from "../parsers";
import { CultureInfo, Culture } from "../../culture";
import { PortugueseNumeric } from "../../resources/portugueseNumeric";
import { RegExpUtility } from "../../utilities"

export class PortugueseNumberParserConfiguration implements INumberParserConfiguration {

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
            ci = new CultureInfo(Culture.Portuguese);
        }

        this.cultureInfo = ci;

        this.langMarker = PortugueseNumeric.LangMarker;
        this.decimalSeparatorChar = PortugueseNumeric.DecimalSeparatorChar;
        this.fractionMarkerToken = PortugueseNumeric.FractionMarkerToken;
        this.nonDecimalSeparatorChar = PortugueseNumeric.NonDecimalSeparatorChar;
        this.halfADozenText = PortugueseNumeric.HalfADozenText;
        this.wordSeparatorToken = PortugueseNumeric.WordSeparatorToken;

        this.writtenDecimalSeparatorTexts = PortugueseNumeric.WrittenDecimalSeparatorTexts;
        this.writtenGroupSeparatorTexts = PortugueseNumeric.WrittenGroupSeparatorTexts;
        this.writtenIntegerSeparatorTexts = PortugueseNumeric.WrittenIntegerSeparatorTexts;
        this.writtenFractionSeparatorTexts = PortugueseNumeric.WrittenFractionSeparatorTexts;

        let ordinalNumberMap = new Map<string, number>(PortugueseNumeric.OrdinalNumberMap);

        PortugueseNumeric.PrefixCardinalMap.forEach((prefixValue: number, prefixKey: string) => {
            PortugueseNumeric.SuffixOrdinalMap.forEach((suffixValue: number, suffixKey: string) => {
                if (!ordinalNumberMap.has(prefixKey + suffixKey)) {
                    ordinalNumberMap.set(prefixKey + suffixKey, prefixValue * suffixValue);
                }
            });
        });

        this.cardinalNumberMap = PortugueseNumeric.CardinalNumberMap;
        this.ordinalNumberMap = ordinalNumberMap;
        this.roundNumberMap = PortugueseNumeric.RoundNumberMap;
        this.halfADozenRegex = RegExpUtility.getSafeRegExp(PortugueseNumeric.HalfADozenRegex);
        this.digitalNumberRegex = RegExpUtility.getSafeRegExp(PortugueseNumeric.DigitalNumberRegex);
    }

    normalizeTokenSet(tokens: ReadonlyArray<string>, context: ParseResult): ReadonlyArray<string> {
        let result = new Array<string>();

        tokens.forEach((token: string) => {
            let tempWord = token.replace(/^s+/, '').replace(/s+$/, '');
            if (this.ordinalNumberMap.has(tempWord)) {
                result.push(tempWord);
                return;
            }

            // ends with 'avo' or 'ava'
            if (PortugueseNumeric.WrittenFractionSuffix.some(suffix => tempWord.endsWith(suffix))) {
                let origTempWord = tempWord;
                let newLength = origTempWord.length;
                tempWord = origTempWord.substring(0, newLength - 3);
                if (!tempWord) {
                    return;
                }
                else if (this.cardinalNumberMap.has(tempWord)) {
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