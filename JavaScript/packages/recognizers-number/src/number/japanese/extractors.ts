import { BaseNumberExtractor, RegExpValue, RegExpRegExp, BasePercentageExtractor } from "../extractors";
import { Constants } from "../constants";
import { NumberMode, LongFormatType } from "../models";
import { JapaneseNumeric } from "../../resources/japaneseNumeric";
import { RegExpUtility } from "@microsoft/recognizers-text";

export enum JapaneseNumberExtractorMode {
    // Number extraction with an allow list that filters what numbers to extract.
    Default,
    // Extract all number-related terms aggressively.
    ExtractAll,
}

export class JapaneseNumberExtractor extends BaseNumberExtractor {
    protected extractType: string = Constants.SYS_NUM;

    constructor(mode: JapaneseNumberExtractorMode = JapaneseNumberExtractorMode.Default) {
        super();
        let regexes = new Array<RegExpValue>();

        // Add Cardinal
        let cardExtract = new JapaneseCardinalExtractor(mode);
        cardExtract.regexes.forEach(r => regexes.push(r));

        // Add Fraction
        let fracExtract = new JapaneseFractionExtractor();
        fracExtract.regexes.forEach(r => regexes.push(r));

        this.regexes = regexes;

        // Add filter
        let AmbiguityFiltersDict = new Array<RegExpRegExp>();
        this.AmbiguityFiltersDict = AmbiguityFiltersDict;
    }
}

export class JapaneseCardinalExtractor extends BaseNumberExtractor {
    protected extractType: string = Constants.SYS_NUM_CARDINAL;

    constructor(mode: JapaneseNumberExtractorMode = JapaneseNumberExtractorMode.Default) {
        super();
        let regexes = new Array<RegExpValue>();

        // Add Integer Regexes
        let intExtract = new JapaneseIntegerExtractor(mode);
        intExtract.regexes.forEach(r => regexes.push(r));

        // Add Double Regexes
        let doubleExtract = new JapaneseDoubleExtractor();
        doubleExtract.regexes.forEach(r => regexes.push(r));

        this.regexes = regexes;
    }
}

export class JapaneseIntegerExtractor extends BaseNumberExtractor {
    protected extractType: string = Constants.SYS_NUM_INTEGER;

    constructor(mode: JapaneseNumberExtractorMode = JapaneseNumberExtractorMode.Default) {
        super();

        let regexes = new Array<RegExpValue>(
            { // 123456,  －１２３４５６
                regExp: RegExpUtility.getSafeRegExp(JapaneseNumeric.NumbersSpecialsChars, "gi"),
                value: "IntegerNum"
            },
            { // 15k,  16 G
                regExp: RegExpUtility.getSafeRegExp(JapaneseNumeric.NumbersSpecialsCharsWithSuffix, "gi"),
                value: "IntegerNum"
            },
            { // 1,234,  ２，３３２，１１１
                regExp: RegExpUtility.getSafeRegExp(JapaneseNumeric.DottedNumbersSpecialsChar, "gi"),
                value: "IntegerNum"
            },
            { // 半百  半ダース
                regExp: RegExpUtility.getSafeRegExp(JapaneseNumeric.NumbersWithHalfDozen, "gi"),
                value: "IntegerJpn"
            },
            { // 一ダース  五十ダース
                regExp: RegExpUtility.getSafeRegExp(JapaneseNumeric.NumbersWithDozen, "gi"),
                value: "IntegerJpn"
            }
        );

        switch (mode) {
            case JapaneseNumberExtractorMode.Default:
                regexes.push({ // 一百五十五, 负一亿三百二十二. Uses an allow list to avoid extracting "西九条" from "九"
                    regExp: RegExpUtility.getSafeRegExp(JapaneseNumeric.NumbersWithAllowListRegex, "gi"),
                    value: "IntegerJpn"
                });
                break;

            case JapaneseNumberExtractorMode.ExtractAll:
                regexes.push({ // 一百五十五, 负一亿三百二十二, "西九条" from "九". Uses no allow lists and extracts all potential integers (useful in Units, for example).
                    regExp: RegExpUtility.getSafeRegExp(JapaneseNumeric.NumbersAggressiveRegex, "gi"),
                    value: "IntegerJpn"
                });
                break;
        }

        this.regexes = regexes;
    }
}

export class JapaneseDoubleExtractor extends BaseNumberExtractor {
    protected extractType: string = Constants.SYS_NUM_DOUBLE;

    constructor() {
        super();

        let regexes = new Array<RegExpValue>(
            {
                regExp: RegExpUtility.getSafeRegExp(JapaneseNumeric.DoubleSpecialsChars, "gis"),
                value: "DoubleNum"
            },
            { // (-)2.5, can avoid cases like ip address xx.xx.xx.xx
                regExp: RegExpUtility.getSafeRegExp(JapaneseNumeric.DoubleSpecialsCharsWithNegatives, "gis"),
                value: "DoubleNum"
            },
            { // (-).2
                regExp: RegExpUtility.getSafeRegExp(JapaneseNumeric.SimpleDoubleSpecialsChars, "gis"),
                value: "DoubleNum"
            },
            { // 1.0 K
                regExp: RegExpUtility.getSafeRegExp(JapaneseNumeric.DoubleWithMultiplierRegex, "gis"),
                value: "DoubleNum"
            },
            { // １５.２万
                regExp: RegExpUtility.getSafeRegExp(JapaneseNumeric.DoubleWithThousandsRegex, "gis"),
                value: "DoubleJpn"
            },
            { // 2e6, 21.2e0
                regExp: RegExpUtility.getSafeRegExp(JapaneseNumeric.DoubleExponentialNotationRegex, "gis"),
                value: "DoublePow"
            },
            { // 2^5
                regExp: RegExpUtility.getSafeRegExp(JapaneseNumeric.DoubleScientificNotationRegex, "gis"),
                value: "DoublePow"
            }
        );

        this.regexes = regexes;
    }
}

export class JapaneseFractionExtractor extends BaseNumberExtractor {
    protected extractType: string = Constants.SYS_NUM_FRACTION;

    constructor() {
        super();

        let regexes = new Array<RegExpValue>(
            { // -4 5/2,  ４ ６／３
                regExp: RegExpUtility.getSafeRegExp(JapaneseNumeric.FractionNotationSpecialsCharsRegex, "gis"),
                value: "FracNum"
            },
            { // 8/3 
                regExp: RegExpUtility.getSafeRegExp(JapaneseNumeric.FractionNotationRegex, "gis"),
                value: "FracNum"
            },
            { // 五分の二 七分の三
                regExp: RegExpUtility.getSafeRegExp(JapaneseNumeric.AllFractionNumber, "gis"),
                value: "FracJpn"
            }
        );

        this.regexes = regexes;
    }
}

export class JapaneseOrdinalExtractor extends BaseNumberExtractor {
    protected extractType: string = Constants.SYS_NUM_ORDINAL;

    constructor() {
        super();
        let regexes = new Array<RegExpValue>(
            { // だい一百五十四
                regExp: RegExpUtility.getSafeRegExp(JapaneseNumeric.OrdinalRegex, "gi"),
                value: "OrdinalJpn"
            },
            { // だい２５６５
                regExp: RegExpUtility.getSafeRegExp(JapaneseNumeric.OrdinalNumbersRegex, "gi"),
                value: "OrdinalJpn"
            },
            { // 2折 ２.５折
                regExp: RegExpUtility.getSafeRegExp(JapaneseNumeric.NumbersFoldsPercentageRegex, "gi"),
                value: "OrdinalJpn"
            }
        );

        this.regexes = regexes;
    }
}

export class JapanesePercentageExtractor extends BaseNumberExtractor {
    extractType: string = Constants.SYS_NUM_PERCENTAGE;

    constructor() {
        super();
        let regexes = new Array<RegExpValue>(
            { // 百パーセント 十五パーセント
                regExp: RegExpUtility.getSafeRegExp(JapaneseNumeric.SimplePercentageRegex, "gi"),
                value: "PerJpn"
            },
            { // 19パーセント　１パーセント
                regExp: RegExpUtility.getSafeRegExp(JapaneseNumeric.NumbersPercentagePointRegex, "gis"),
                value: "PerNum"
            },
            { // 3,000パーセント  １，１２３パーセント
                regExp: RegExpUtility.getSafeRegExp(JapaneseNumeric.NumbersPercentageWithSeparatorRegex, "gis"),
                value: "PerNum"
            },
            { // 3.2 k パーセント
                regExp: RegExpUtility.getSafeRegExp(JapaneseNumeric.NumbersPercentageWithMultiplierRegex, "gi"),
                value: "PerNum"
            },
            { // 15kパーセント
                regExp: RegExpUtility.getSafeRegExp(JapaneseNumeric.SimpleNumbersPercentageWithMultiplierRegex, "gi"),
                value: "PerNum"
            },
            {
                regExp: RegExpUtility.getSafeRegExp(JapaneseNumeric.SimpleIntegerPercentageRegex, "gis"),
                value: "PerNum"
            },
            { // 2割引 ２.５割引
                regExp: RegExpUtility.getSafeRegExp(JapaneseNumeric.NumbersFoldsPercentageRegex, "gis"),
                value: "PerSpe"
            },
            { // 三割引 六点五折 七五折
                regExp: RegExpUtility.getSafeRegExp(JapaneseNumeric.FoldsPercentageRegex, "gis"),
                value: "PerSpe"
            },
            { // 5割 7割半
                regExp: RegExpUtility.getSafeRegExp(JapaneseNumeric.SimpleFoldsPercentageRegex, "gis"),
                value: "PerSpe"
            },
            { // 七割半
                regExp: RegExpUtility.getSafeRegExp(JapaneseNumeric.SpecialsPercentageRegex, "gis"),
                value: "PerSpe"
            },
            { // 2割 ２.５割
                regExp: RegExpUtility.getSafeRegExp(JapaneseNumeric.NumbersSpecialsPercentageRegex, "gis"),
                value: "PerSpe"
            },
            { // 三割
                regExp: RegExpUtility.getSafeRegExp(JapaneseNumeric.SimpleSpecialsPercentageRegex, "gis"),
                value: "PerSpe"
            },
            {
                regExp: RegExpUtility.getSafeRegExp(JapaneseNumeric.SpecialsFoldsPercentageRegex, "gis"),
                value: "PerSpe"
            },
        );

        this.regexes = regexes;
    }
}