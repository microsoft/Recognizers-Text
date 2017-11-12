import { INumberParserConfiguration, ParseResult } from "../parsers";
import { CultureInfo, Culture } from "../../culture";
import { FrenchNumeric } from "../../resources/frenchNumeric";
import { RegExpUtility } from "../../utilities"

export class FrenchNumberParserConfiguration implements INumberParserConfiguration {

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
            ci = new CultureInfo(Culture.French);
        }

        this.cultureInfo = ci;

        this.langMarker = FrenchNumeric.LangMarker;
        this.decimalSeparatorChar = FrenchNumeric.DecimalSeparatorChar;
        this.fractionMarkerToken = FrenchNumeric.FractionMarkerToken;
        this.nonDecimalSeparatorChar = FrenchNumeric.NonDecimalSeparatorChar;
        this.halfADozenText = FrenchNumeric.HalfADozenText;
        this.wordSeparatorToken = FrenchNumeric.WordSeparatorToken;

        this.writtenDecimalSeparatorTexts = FrenchNumeric.WrittenDecimalSeparatorTexts;
        this.writtenGroupSeparatorTexts = FrenchNumeric.WrittenGroupSeparatorTexts;
        this.writtenIntegerSeparatorTexts = FrenchNumeric.WrittenIntegerSeparatorTexts;
        this.writtenFractionSeparatorTexts = FrenchNumeric.WrittenFractionSeparatorTexts;

        this.cardinalNumberMap = FrenchNumeric.CardinalNumberMap;
        this.ordinalNumberMap = FrenchNumeric.OrdinalNumberMap;
        this.roundNumberMap = FrenchNumeric.RoundNumberMap;
        this.halfADozenRegex = RegExpUtility.getSafeRegExp(FrenchNumeric.HalfADozenRegex);
        this.digitalNumberRegex = RegExpUtility.getSafeRegExp(FrenchNumeric.DigitalNumberRegex);
    }

    normalizeTokenSet(tokens: ReadonlyArray<string>, context: ParseResult): ReadonlyArray<string> {
        return tokens;
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