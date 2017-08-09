import { INumberParserConfiguration, ParseResult } from "../parsers";
import { CultureInfo, Culture } from "../../culture";
import { EnglishNumericResources } from "../../resources/englishNumeric";
import * as XRegExp from 'xregexp';

export class EnglishNumberParserConfiguration implements INumberParserConfiguration {

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
            ci = new CultureInfo(Culture.English);
        }

        this.cultureInfo = ci;

        this.langMarker = EnglishNumericResources.LangMarker;
        this.decimalSeparatorChar = EnglishNumericResources.DecimalSeparatorChar;
        this.fractionMarkerToken = EnglishNumericResources.FractionMarkerToken;
        this.nonDecimalSeparatorChar = EnglishNumericResources.NonDecimalSeparatorChar;
        this.halfADozenText = EnglishNumericResources.HalfADozenText;
        this.wordSeparatorToken = EnglishNumericResources.WordSeparatorToken;

        this.writtenDecimalSeparatorTexts = EnglishNumericResources.WrittenDecimalSeparatorTexts;
        this.writtenGroupSeparatorTexts = EnglishNumericResources.WrittenGroupSeparatorTexts;
        this.writtenIntegerSeparatorTexts = EnglishNumericResources.WrittenIntegerSeparatorTexts;
        this.writtenFractionSeparatorTexts = EnglishNumericResources.WrittenFractionSeparatorTexts;

        this.cardinalNumberMap = EnglishNumericResources.CardinalNumberMap;
        this.ordinalNumberMap = EnglishNumericResources.OrdinalNumberMap;
        this.roundNumberMap = EnglishNumericResources.RoundNumberMap;
        this.halfADozenRegex = XRegExp(EnglishNumericResources.HalfADozenRegex, "gis");
        this.digitalNumberRegex = XRegExp(EnglishNumericResources.DigitalNumberRegex, "gis");
    }

    normalizeTokenSet(tokens: ReadonlyArray<string>, context: ParseResult): ReadonlyArray<string> {
        let fracWords = new Array<string>();
        let tokenList = Array.from(tokens);
        let tokenLen = tokenList.length;
        for (let i = 0; i < tokenLen; i++) {
            if ((i < tokenLen - 2) && tokenList[i + 1] === "-") {
                fracWords.push(tokenList[i] + tokenList[i + 1] + tokenList[i + 2]);
                i += 2;
            }
            else {
                fracWords.push(tokenList[i]);
            }
        }
        return fracWords;
    }

    resolveCompositeNumber(numberStr: string): number {
        if (numberStr.includes("-")) {
            let numbers = numberStr.split('-');
            let ret = 0;
            numbers.forEach(num => {
                if (this.ordinalNumberMap.has(num)) {
                    ret += this.ordinalNumberMap.get(num) as number;
                }
                else if (this.cardinalNumberMap.has(num)) {
                    ret += this.cardinalNumberMap.get(num) as number;
                }
            });

            return ret;
        }

        if (this.ordinalNumberMap.has(numberStr)) {
            return this.ordinalNumberMap.get(numberStr) as number;
        }

        if (this.cardinalNumberMap.has(numberStr)) {
            return this.cardinalNumberMap.get(numberStr) as number;
        }

        return 0;
    }
}