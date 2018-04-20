import { ParseResult } from "@microsoft/recognizers-text";
import { INumberParserConfiguration } from "../parsers";
import { CultureInfo, Culture } from "../../culture";
import { ChineseNumeric } from "../../resources/chineseNumeric";
import { RegExpUtility } from "@microsoft/recognizers-text"

export class ChineseNumberParserConfiguration implements INumberParserConfiguration {

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
    readonly negativeNumberSignRegex: RegExp;
    readonly pointRegex: RegExp;
    readonly speGetNumberRegex: RegExp;
    readonly pairRegex: RegExp;

    constructor(ci?: CultureInfo) {
        if (!ci) {
            ci = new CultureInfo(Culture.Chinese);
        }

        this.cultureInfo = ci;

        this.langMarker = ChineseNumeric.LangMarker;
        this.decimalSeparatorChar = ChineseNumeric.DecimalSeparatorChar;
        this.fractionMarkerToken = ChineseNumeric.FractionMarkerToken;
        this.nonDecimalSeparatorChar = ChineseNumeric.NonDecimalSeparatorChar;
        this.halfADozenText = ChineseNumeric.HalfADozenText;
        this.wordSeparatorToken = ChineseNumeric.WordSeparatorToken;

        this.writtenDecimalSeparatorTexts = [];
        this.writtenGroupSeparatorTexts = [];
        this.writtenIntegerSeparatorTexts = [];
        this.writtenFractionSeparatorTexts = [];

        this.cardinalNumberMap = new Map<string, number>();
        this.ordinalNumberMap = new Map<string, number>();
        this.roundNumberMap = ChineseNumeric.RoundNumberMap;
        this.halfADozenRegex = null;
        this.digitalNumberRegex = RegExpUtility.getSafeRegExp(ChineseNumeric.DigitalNumberRegex, "gis");

        this.zeroToNineMap = ChineseNumeric.ZeroToNineMap;
        this.roundNumberMapChar = ChineseNumeric.RoundNumberMapChar;
        this.fullToHalfMap = ChineseNumeric.FullToHalfMap;
        this.tratoSimMap = ChineseNumeric.TratoSimMap;
        this.unitMap = ChineseNumeric.UnitMap;
        this.roundDirectList = ChineseNumeric.RoundDirectList;
        this.digitNumRegex = RegExpUtility.getSafeRegExp(ChineseNumeric.DigitNumRegex, "gis");
        this.dozenRegex = RegExpUtility.getSafeRegExp(ChineseNumeric.DozenRegex, "gis");
        this.percentageRegex = RegExpUtility.getSafeRegExp(ChineseNumeric.PercentageRegex, "gis");
        this.doubleAndRoundRegex = RegExpUtility.getSafeRegExp(ChineseNumeric.DoubleAndRoundRegex, "gis");
        this.fracSplitRegex = RegExpUtility.getSafeRegExp(ChineseNumeric.FracSplitRegex, "gis");
        this.negativeNumberSignRegex = RegExpUtility.getSafeRegExp(ChineseNumeric.NegativeNumberSignRegex, "gis");
        this.pointRegex = RegExpUtility.getSafeRegExp(ChineseNumeric.PointRegex, "gis");
        this.speGetNumberRegex = RegExpUtility.getSafeRegExp(ChineseNumeric.SpeGetNumberRegex, "gis");
        this.pairRegex = RegExpUtility.getSafeRegExp(ChineseNumeric.PairRegex, "gis");
    }

    normalizeTokenSet(tokens: ReadonlyArray<string>, context: ParseResult): ReadonlyArray<string> {
        return tokens;
    }

    resolveCompositeNumber(numberStr: string): number {
        return 0;
    }
}