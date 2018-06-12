import { BaseNumberExtractor, RegExpValue, BasePercentageExtractor } from "../extractors";
import { Constants } from "../constants";
import { LongFormatType } from "../models";
import { ChineseNumeric } from "../../resources/chineseNumeric";
import { RegExpUtility } from "@microsoft/recognizers-text"

export enum ChineseNumberExtractorMode {
    // Number extraction with an allow list that filters what numbers to extract.
    Default,
    // Extract all number-related terms aggressively.
    ExtractAll,
}

export class ChineseNumberExtractor extends BaseNumberExtractor {
    protected extractType: string = Constants.SYS_NUM;

    constructor(mode: ChineseNumberExtractorMode = ChineseNumberExtractorMode.Default) {
        super();
        let regexes = new Array<RegExpValue>();

        // Add Cardinal
        let cardExtract = new ChineseCardinalExtractor(mode);
        cardExtract.regexes.forEach(r => regexes.push(r));

        // Add Fraction
        let fracExtract = new ChineseFractionExtractor();
        fracExtract.regexes.forEach(r => regexes.push(r));

        this.regexes = regexes;
    }
}

export class ChineseCardinalExtractor extends BaseNumberExtractor {
    protected extractType: string = Constants.SYS_NUM_CARDINAL;

    constructor(mode: ChineseNumberExtractorMode = ChineseNumberExtractorMode.Default) {
        super();
        let regexes = new Array<RegExpValue>();

        // Add Integer Regexes
        let intExtract = new ChineseIntegerExtractor(mode);
        intExtract.regexes.forEach(r => regexes.push(r));

        // Add Double Regexes
        let doubleExtract = new ChineseDoubleExtractor();
        doubleExtract.regexes.forEach(r => regexes.push(r));

        this.regexes = regexes;
    }
}

export class ChineseIntegerExtractor extends BaseNumberExtractor {
    protected extractType: string = Constants.SYS_NUM_INTEGER;

    constructor(mode: ChineseNumberExtractorMode = ChineseNumberExtractorMode.Default) {
        super();

        let regexes = new Array<RegExpValue>(
            { // 123456,  －１２３４５６
                regExp: RegExpUtility.getSafeRegExp(ChineseNumeric.NumbersSpecialsChars, "gi"),
                value: "IntegerNum"
            },
            { // 15k,  16 G
                regExp: RegExpUtility.getSafeRegExp(ChineseNumeric.NumbersSpecialsCharsWithSuffix, "gs"),
                value: "IntegerNum"
            },
            { // 1,234,  ２，３３２，１１１
                regExp: RegExpUtility.getSafeRegExp(ChineseNumeric.DottedNumbersSpecialsChar, "gis"),
                value: "IntegerNum"
            },
            { // 半百  半打
                regExp: RegExpUtility.getSafeRegExp(ChineseNumeric.NumbersWithHalfDozen, "gis"),
                value: "IntegerChs"
            },
            { // 一打  五十打
                regExp: RegExpUtility.getSafeRegExp(ChineseNumeric.NumbersWithDozen, "gis"),
                value: "IntegerChs"
            }
        );

        switch (mode) {
            case ChineseNumberExtractorMode.Default:
                regexes.push({ // 一百五十五, 负一亿三百二十二. Uses an allow list to avoid extracting "四" from "四川"
                    regExp: RegExpUtility.getSafeRegExp(ChineseNumeric.NumbersWithAllowListRegex, "gi"),
                    value: "IntegerChs"
                });
                break;

            case ChineseNumberExtractorMode.ExtractAll:
                regexes.push({ // 一百五十五, 负一亿三百二十二, "四" from "四川". Uses no allow lists and extracts all potential integers (useful in Units, for example).
                    regExp: RegExpUtility.getSafeRegExp(ChineseNumeric.NumbersAggressiveRegex, "gi"),
                    value: "IntegerChs"
                });
                break;
        }

        this.regexes = regexes;
    }
}

export class ChineseDoubleExtractor extends BaseNumberExtractor {
    protected extractType: string = Constants.SYS_NUM_DOUBLE;

    constructor() {
        super();

        let regexes = new Array<RegExpValue>(
            {
                regExp: RegExpUtility.getSafeRegExp(ChineseNumeric.DoubleSpecialsChars, "gis"),
                value: "DoubleNum"
            },
            { // (-)2.5, can avoid cases like ip address xx.xx.xx.xx
                regExp: RegExpUtility.getSafeRegExp(ChineseNumeric.DoubleSpecialsCharsWithNegatives, "gis"),
                value: "DoubleNum"
            },
            { // (-).2 
                regExp: RegExpUtility.getSafeRegExp(ChineseNumeric.SimpleDoubleSpecialsChars, "gis"),
                value: "DoubleNum"
            },
            { // 1.0 K
                regExp: RegExpUtility.getSafeRegExp(ChineseNumeric.DoubleWithMultiplierRegex, "gi"),
                value: "DoubleNum"
            },
            { // １５.２万
                regExp: RegExpUtility.getSafeRegExp(ChineseNumeric.DoubleWithThousandsRegex, "gi"),
                value: "DoubleChs"
            },
            { // 四十五点三三
                regExp: RegExpUtility.getSafeRegExp(ChineseNumeric.DoubleAllFloatRegex, "gi"),
                value: "DoubleChs"
            },
            { // 2e6, 21.2e0
                regExp: RegExpUtility.getSafeRegExp(ChineseNumeric.DoubleExponentialNotationRegex, "gis"),
                value: "DoublePow"
            },
            { // 2^5
                regExp: RegExpUtility.getSafeRegExp(ChineseNumeric.DoubleScientificNotationRegex, "gis"),
                value: "DoublePow"
            }
        );

        this.regexes = regexes;
    }
}

export class ChineseFractionExtractor extends BaseNumberExtractor {

    protected extractType: string = Constants.SYS_NUM_FRACTION;

    constructor() {
        super();

        let regexes = new Array<RegExpValue>(
            { // -4 5/2, ４ ６／３
                regExp: RegExpUtility.getSafeRegExp(ChineseNumeric.FractionNotationSpecialsCharsRegex, "gis"),
                value: "FracNum"
            },
            { // 8/3 
                regExp: RegExpUtility.getSafeRegExp(ChineseNumeric.FractionNotationRegex, "gis"),
                value: "FracNum"
            },
            { // 四分之六十五
                regExp: RegExpUtility.getSafeRegExp(ChineseNumeric.AllFractionNumber, "gi"),
                value: "FracChs"
            }
        );

        this.regexes = regexes;
    }
}

export class ChineseOrdinalExtractor extends BaseNumberExtractor {
    protected extractType: string = Constants.SYS_NUM_ORDINAL;

    constructor() {
        super();
        let regexes = new Array<RegExpValue>(
            { // 第一百五十四
                regExp: RegExpUtility.getSafeRegExp(ChineseNumeric.OrdinalRegex, "gi"),
                value: "OrdinalChs"
            },
            { // 第２５６５,  第1234
                regExp: RegExpUtility.getSafeRegExp(ChineseNumeric.OrdinalNumbersRegex, "gi"),
                value: "OrdinalChs"
            }
        );

        this.regexes = regexes;
    }
}

export class ChinesePercentageExtractor extends BaseNumberExtractor {
    protected extractType: string = Constants.SYS_NUM_PERCENTAGE;

    constructor() {
        super();
        let regexes = new Array<RegExpValue>(
            { // 二十个百分点,  四点五个百分点
                regExp: RegExpUtility.getSafeRegExp(ChineseNumeric.PercentagePointRegex, "gi"),
                value: "PerChs"
            },
            { // 百分之五十  百分之一点五
                regExp: RegExpUtility.getSafeRegExp(ChineseNumeric.SimplePercentageRegex, "gi"),
                value: "PerChs"
            },
            { // 百分之５６.２　百分之１２
                regExp: RegExpUtility.getSafeRegExp(ChineseNumeric.NumbersPercentagePointRegex, "gis"),
                value: "PerNum"
            },
            { // 百分之3,000  百分之１，１２３
                regExp: RegExpUtility.getSafeRegExp(ChineseNumeric.NumbersPercentageWithSeparatorRegex, "gis"),
                value: "PerNum"
            },
            { // 百分之3.2 k 
                regExp: RegExpUtility.getSafeRegExp(ChineseNumeric.NumbersPercentageWithMultiplierRegex, "gi"),
                value: "PerNum"
            },
            { // 12.56个百分点  ０.４个百分点
                regExp: RegExpUtility.getSafeRegExp(ChineseNumeric.FractionPercentagePointRegex, "gis"),
                value: "PerNum"
            },
            { // 15,123个百分点  １１１，１１１个百分点
                regExp: RegExpUtility.getSafeRegExp(ChineseNumeric.FractionPercentageWithSeparatorRegex, "gis"),
                value: "PerNum"
            },
            { // 12.1k个百分点  １５.1k个百分点
                regExp: RegExpUtility.getSafeRegExp(ChineseNumeric.FractionPercentageWithMultiplierRegex, "gi"),
                value: "PerNum"
            },
            { // 百分之22  百分之１２０
                regExp: RegExpUtility.getSafeRegExp(ChineseNumeric.SimpleNumbersPercentageRegex, "gis"),
                value: "PerNum"
            },
            { // 百分之15k 
                regExp: RegExpUtility.getSafeRegExp(ChineseNumeric.SimpleNumbersPercentageWithMultiplierRegex, "gi"),
                value: "PerNum"
            },
            { // 百分之1,111  百分之９，９９９
                regExp: RegExpUtility.getSafeRegExp(ChineseNumeric.SimpleNumbersPercentagePointRegex, "gis"),
                value: "PerNum"
            },
            { // 12个百分点
                regExp: RegExpUtility.getSafeRegExp(ChineseNumeric.IntegerPercentageRegex, "gis"),
                value: "PerNum"
            },
            { // 12k个百分点
                regExp: RegExpUtility.getSafeRegExp(ChineseNumeric.IntegerPercentageWithMultiplierRegex, "gi"),
                value: "PerNum"
            },
            { // 2,123个百分点
                regExp: RegExpUtility.getSafeRegExp(ChineseNumeric.NumbersFractionPercentageRegex, "gis"),
                value: "PerNum"
            },
            {
                regExp: RegExpUtility.getSafeRegExp(ChineseNumeric.SimpleIntegerPercentageRegex, "gis"),
                value: "PerNum"
            },
            { // 2折 ２.５折
                regExp: RegExpUtility.getSafeRegExp(ChineseNumeric.NumbersFoldsPercentageRegex, "gis"),
                value: "PerSpe"
            },
            { // 三折 六点五折 七五折
                regExp: RegExpUtility.getSafeRegExp(ChineseNumeric.FoldsPercentageRegex, "gis"),
                value: "PerSpe"
            },
            { // 5成 6成半 6成4
                regExp: RegExpUtility.getSafeRegExp(ChineseNumeric.SimpleFoldsPercentageRegex, "gis"),
                value: "PerSpe"
            },
            { // 七成半 七成五
                regExp: RegExpUtility.getSafeRegExp(ChineseNumeric.SpecialsPercentageRegex, "gis"),
                value: "PerSpe"
            },
            { // 2成 ２.５成
                regExp: RegExpUtility.getSafeRegExp(ChineseNumeric.NumbersSpecialsPercentageRegex, "gis"),
                value: "PerSpe"
            },
            { // 三成 六点五成
                regExp: RegExpUtility.getSafeRegExp(ChineseNumeric.SimpleSpecialsPercentageRegex, "gis"),
                value: "PerSpe"
            },
            {
                regExp: RegExpUtility.getSafeRegExp(ChineseNumeric.SpecialsFoldsPercentageRegex, "gis"),
                value: "PerSpe"
            }
        );

        this.regexes = regexes;
    }
}