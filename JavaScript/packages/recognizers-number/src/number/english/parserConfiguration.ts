import { ParseResult } from "@microsoft/recognizers-text";
import { INumberParserConfiguration } from "../parsers";
import { CultureInfo, Culture } from "../../culture";
import { EnglishNumeric } from "../../resources/englishNumeric";
import { RegExpUtility } from "@microsoft/recognizers-text"

export class EnglishNumberParserConfiguration implements INumberParserConfiguration {

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

    constructor(ci?: CultureInfo) {
        if (!ci) {
            ci = new CultureInfo(Culture.English);
        }

        this.cultureInfo = ci;

        this.langMarker = EnglishNumeric.LangMarker;
        this.decimalSeparatorChar = EnglishNumeric.DecimalSeparatorChar;
        this.fractionMarkerToken = EnglishNumeric.FractionMarkerToken;
        this.nonDecimalSeparatorChar = EnglishNumeric.NonDecimalSeparatorChar;
        this.halfADozenText = EnglishNumeric.HalfADozenText;
        this.wordSeparatorToken = EnglishNumeric.WordSeparatorToken;

        this.writtenDecimalSeparatorTexts = EnglishNumeric.WrittenDecimalSeparatorTexts;
        this.writtenGroupSeparatorTexts = EnglishNumeric.WrittenGroupSeparatorTexts;
        this.writtenIntegerSeparatorTexts = EnglishNumeric.WrittenIntegerSeparatorTexts;
        this.writtenFractionSeparatorTexts = EnglishNumeric.WrittenFractionSeparatorTexts;

        this.cardinalNumberMap = EnglishNumeric.CardinalNumberMap;
        this.ordinalNumberMap = EnglishNumeric.OrdinalNumberMap;
        this.roundNumberMap = EnglishNumeric.RoundNumberMap;
        this.negativeNumberSignRegex = RegExpUtility.getSafeRegExp(EnglishNumeric.NegativeNumberSignRegex, "is");
        this.halfADozenRegex = RegExpUtility.getSafeRegExp(EnglishNumeric.HalfADozenRegex, "gis");
        this.digitalNumberRegex = RegExpUtility.getSafeRegExp(EnglishNumeric.DigitalNumberRegex, "gis");
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