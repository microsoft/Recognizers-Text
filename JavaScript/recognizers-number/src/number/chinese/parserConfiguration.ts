import { INumberParserConfiguration, ParseResult } from "../parsers";
import { CultureInfo, Culture } from "../../culture";
import { ChineseNumeric } from "../../resources/chineseNumeric";
import { RegExpUtility } from "../../utilities"

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
    
    readonly zeroToNineMapChs: ReadonlyMap<string, number>;
    readonly roundNumberMapChs: ReadonlyMap<string, number>;
    readonly fullToHalfMapChs: ReadonlyMap<string, string>;
    readonly tratoSimMapChs: ReadonlyMap<string, string>;
    readonly unitMapChs: ReadonlyMap<string, string>;
    readonly roundDirectListChs: ReadonlyArray<string>;
    readonly digitNumRegex: RegExp;
    readonly dozenRegex: RegExp;
    readonly percentageRegex: RegExp;
    readonly doubleAndRoundChsRegex: RegExp;
    readonly fracSplitRegex: RegExp;
    readonly symbolRegex: RegExp;
    readonly pointRegexChs: RegExp;
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

        this.zeroToNineMapChs = ChineseNumeric.ZeroToNineMapChs;
        this.roundNumberMapChs = ChineseNumeric.RoundNumberMapChs;
        this.fullToHalfMapChs = ChineseNumeric.FullToHalfMapChs;
        this.tratoSimMapChs = ChineseNumeric.TratoSimMapChs;
        this.unitMapChs = ChineseNumeric.UnitMapChs;
        this.roundDirectListChs = ChineseNumeric.RoundDirectListChs;
        this.digitNumRegex = RegExpUtility.getSafeRegExp(ChineseNumeric.DigitNumRegex, "gis");
        this.dozenRegex = RegExpUtility.getSafeRegExp(ChineseNumeric.DozenRegex, "gis");
        this.percentageRegex = RegExpUtility.getSafeRegExp(ChineseNumeric.PercentageRegex, "gis");
        this.doubleAndRoundChsRegex = RegExpUtility.getSafeRegExp(ChineseNumeric.DoubleAndRoundChsRegex, "gis");
        this.fracSplitRegex = RegExpUtility.getSafeRegExp(ChineseNumeric.FracSplitRegex, "gis");
        this.symbolRegex = RegExpUtility.getSafeRegExp(ChineseNumeric.SymbolRegex, "gis");
        this.pointRegexChs = RegExpUtility.getSafeRegExp(ChineseNumeric.PointRegexChs, "gis");
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