import { ParseResult } from "@microsoft/recognizers-text";
import { ICJKNumberParserConfiguration } from "../cjkParsers";
import { CultureInfo, Culture } from "../../culture";
import { JapaneseNumeric } from "../../resources/japaneseNumeric";
import { RegExpUtility } from "@microsoft/recognizers-text"

export class JapaneseNumberParserConfiguration implements ICJKNumberParserConfiguration {

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
    readonly roundNumberIntegerRegex: RegExp;

    // readonly NumberOptions Options { get; }
    // readonly Regex FractionPrepositionRegex { get; }
    // readonly string NonDecimalSeparatorText 

    constructor(ci?: CultureInfo) {
        if (!ci) {
            ci = new CultureInfo(Culture.Japanese);
        }

        this.cultureInfo = ci;

        this.langMarker = JapaneseNumeric.LangMarker;
        this.decimalSeparatorChar = JapaneseNumeric.DecimalSeparatorChar;
        this.fractionMarkerToken = JapaneseNumeric.FractionMarkerToken;
        this.nonDecimalSeparatorChar = JapaneseNumeric.NonDecimalSeparatorChar;
        this.halfADozenText = JapaneseNumeric.HalfADozenText;
        this.wordSeparatorToken = JapaneseNumeric.WordSeparatorToken;

        this.writtenDecimalSeparatorTexts = [];
        this.writtenGroupSeparatorTexts = [];
        this.writtenIntegerSeparatorTexts = [];
        this.writtenFractionSeparatorTexts = [];

        this.cardinalNumberMap = new Map<string, number>();
        this.ordinalNumberMap = new Map<string, number>();
        this.roundNumberMap = JapaneseNumeric.RoundNumberMap;
        this.halfADozenRegex = null;
        this.digitalNumberRegex = RegExpUtility.getSafeRegExp(JapaneseNumeric.DigitalNumberRegex, "gis");

        this.zeroToNineMap = JapaneseNumeric.ZeroToNineMap;
        this.roundNumberMapChar = JapaneseNumeric.RoundNumberMapChar;
        this.fullToHalfMap = JapaneseNumeric.FullToHalfMap;
        this.tratoSimMap = null;
        this.unitMap = JapaneseNumeric.UnitMap;
        this.roundDirectList = JapaneseNumeric.RoundDirectList;
        this.digitNumRegex = RegExpUtility.getSafeRegExp(JapaneseNumeric.DigitNumRegex, "gis");
        this.dozenRegex = RegExpUtility.getSafeRegExp(JapaneseNumeric.DozenRegex, "gis");
        this.percentageRegex = RegExpUtility.getSafeRegExp(JapaneseNumeric.PercentageRegex, "gis");
        this.doubleAndRoundRegex = RegExpUtility.getSafeRegExp(JapaneseNumeric.DoubleAndRoundRegex, "gis");
        this.fracSplitRegex = RegExpUtility.getSafeRegExp(JapaneseNumeric.FracSplitRegex, "gis");
        this.negativeNumberSignRegex = RegExpUtility.getSafeRegExp(JapaneseNumeric.NegativeNumberSignRegex, "is");
        this.pointRegex = RegExpUtility.getSafeRegExp(JapaneseNumeric.PointRegex, "gis");
        this.speGetNumberRegex = RegExpUtility.getSafeRegExp(JapaneseNumeric.SpeGetNumberRegex, "gis");
        this.pairRegex = RegExpUtility.getSafeRegExp(JapaneseNumeric.PairRegex, "gis");
        this.roundNumberIntegerRegex = RegExpUtility.getSafeRegExp(JapaneseNumeric.RoundNumberIntegerRegex, "gis");

    }
    normalizeTokenSet(tokens: ReadonlyArray<string>, context: ParseResult): ReadonlyArray<string> {
        return tokens;
    }

    resolveCompositeNumber(numberStr: string): number {
        return 0;
    }
}