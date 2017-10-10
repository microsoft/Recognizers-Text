import { BaseNumberExtractor, RegExpValue, BasePercentageExtractor } from "../extractors";
import { Constants } from "../constants";
import { NumberMode, LongFormatType } from "../models";
import { EnglishNumeric } from "../../resources/englishNumeric";
import { RegExpUtility } from "../../utilities"

export class EnglishNumberExtractor extends BaseNumberExtractor {
    protected extractType: string = Constants.SYS_NUM;

    constructor(mode: NumberMode = NumberMode.Default) {
        super();
        let regexes = new Array<RegExpValue>();

        // Add Cardinal
        let cardExtract: EnglishCardinalExtractor | null = null;
        switch (mode) {
            case NumberMode.PureNumber:
                cardExtract = new EnglishCardinalExtractor(EnglishNumeric.PlaceHolderPureNumber);
                break;
            case NumberMode.Currency:
                regexes.push({ regExp: RegExpUtility.getSafeRegExp(EnglishNumeric.CurrencyRegex, "gs"), value: "IntegerNum" });
                break;
            case NumberMode.Default:
                break;
        }

        if (cardExtract === null) {
            cardExtract = new EnglishCardinalExtractor();
        }

        cardExtract.regexes.forEach(r => regexes.push(r));

        // Add Fraction
        let fracExtract = new EnglishFractionExtractor();
        fracExtract.regexes.forEach(r => regexes.push(r));

        this.regexes = regexes;
    }
}

export class EnglishCardinalExtractor extends BaseNumberExtractor {
    protected extractType: string = Constants.SYS_NUM_CARDINAL;

    constructor(placeholder: string = EnglishNumeric.PlaceHolderDefault) {
        super();
        let regexes = new Array<RegExpValue>();

        // Add Integer Regexes
        let intExtract = new EnglishIntegerExtractor(placeholder);
        intExtract.regexes.forEach(r => regexes.push(r));

        // Add Double Regexes
        let doubleExtract = new EnglishDoubleExtractor(placeholder);
        doubleExtract.regexes.forEach(r => regexes.push(r));

        this.regexes = regexes;
    }
}

export class EnglishIntegerExtractor extends BaseNumberExtractor {
    protected extractType: string = Constants.SYS_NUM_INTEGER;

    constructor(placeholder: string = EnglishNumeric.PlaceHolderDefault) {
        super();

        let regexes = new Array<RegExpValue>(
            {
                regExp: RegExpUtility.getSafeRegExp(EnglishNumeric.NumbersWithPlaceHolder(placeholder), "gi"),
                value: "IntegerNum"
            },
            {
                regExp: RegExpUtility.getSafeRegExp(EnglishNumeric.NumbersWithSuffix, "gs"),
                value: "IntegerNum"
            },
            {
                regExp: this.generateLongFormatNumberRegexes(LongFormatType.integerNumComma, placeholder),
                value: "IntegerNum"
            },
            {
                regExp: RegExpUtility.getSafeRegExp(EnglishNumeric.RoundNumberIntegerRegexWithLocks, "gis"),
                value: "IntegerNum"
            },
            {
                regExp: RegExpUtility.getSafeRegExp(EnglishNumeric.NumbersWithDozenSuffix, "gis"),
                value: "IntegerNum"
            },
            {
                regExp: RegExpUtility.getSafeRegExp(EnglishNumeric.AllIntRegexWithLocks, "gis"),
                value: "IntegerEng"
            },
            {
                regExp: RegExpUtility.getSafeRegExp(EnglishNumeric.AllIntRegexWithDozenSuffixLocks, "gis"),
                value: "IntegerEng"
            }
        );

        this.regexes = regexes;
    }
}

export class EnglishDoubleExtractor extends BaseNumberExtractor {
    protected extractType: string = Constants.SYS_NUM_DOUBLE;

    constructor(placeholder: string = EnglishNumeric.PlaceHolderDefault) {
        super();

        let regexes = new Array<RegExpValue>(
            {
                regExp: RegExpUtility.getSafeRegExp(EnglishNumeric.DoubleDecimalPointRegex(placeholder), "gis"),
                value: "DoubleNum"
            },
            {
                regExp: RegExpUtility.getSafeRegExp(EnglishNumeric.DoubleWithoutIntegralRegex(placeholder), "gis"),
                value: "DoubleNum"
            },
            {
                regExp: this.generateLongFormatNumberRegexes(LongFormatType.doubleNumCommaDot, placeholder),
                value: "DoubleNum"
            },
            {
                regExp: RegExpUtility.getSafeRegExp(EnglishNumeric.DoubleWithMultiplierRegex, "gs"),
                value: "DoubleNum"
            },
            {
                regExp: RegExpUtility.getSafeRegExp(EnglishNumeric.DoubleWithRoundNumber, "gis"),
                value: "DoubleNum"
            },
            {
                regExp: RegExpUtility.getSafeRegExp(EnglishNumeric.DoubleAllFloatRegex, "gis"),
                value: "DoubleEng"
            },
            {
                regExp: RegExpUtility.getSafeRegExp(EnglishNumeric.DoubleExponentialNotationRegex, "gis"),
                value: "DoublePow"
            },
            {
                regExp: RegExpUtility.getSafeRegExp(EnglishNumeric.DoubleCaretExponentialNotationRegex, "gis"),
                value: "DoublePow"
            }
        );

        this.regexes = regexes;
    }
}

export class EnglishFractionExtractor extends BaseNumberExtractor {

    protected extractType: string = Constants.SYS_NUM_FRACTION;

    constructor() {
        super();

        let regexes = new Array<RegExpValue>(
            {
                regExp: RegExpUtility.getSafeRegExp(EnglishNumeric.FractionNotationWithSpacesRegex, "gis"),
                value: "FracNum"
            },
            {
                regExp: RegExpUtility.getSafeRegExp(EnglishNumeric.FractionNotationRegex, "gis"),
                value: "FracNum"
            },
            {
                regExp: RegExpUtility.getSafeRegExp(EnglishNumeric.FractionNounRegex, "gis"),
                value: "FracEng"
            },
            {
                regExp: RegExpUtility.getSafeRegExp(EnglishNumeric.FractionNounWithArticleRegex, "gis"),
                value: "FracEng"
            },
            {
                regExp: RegExpUtility.getSafeRegExp(EnglishNumeric.FractionPrepositionRegex, "gis"),
                value: "FracEng"
            }
        );

        this.regexes = regexes;
    }
}

export class EnglishOrdinalExtractor extends BaseNumberExtractor {
    protected extractType: string = Constants.SYS_NUM_ORDINAL;

    constructor() {
        super();
        let regexes = new Array<RegExpValue>(
            {
                regExp: RegExpUtility.getSafeRegExp(EnglishNumeric.OrdinalSuffixRegex, "gis"),
                value: "OrdinalNum"
            },
            {
                regExp: RegExpUtility.getSafeRegExp(EnglishNumeric.OrdinalNumericRegex, "gis"),
                value: "OrdinalNum"
            },
            {
                regExp: RegExpUtility.getSafeRegExp(EnglishNumeric.OrdinalEnglishRegex, "gis"),
                value: "OrdEng"
            },
            {
                regExp: RegExpUtility.getSafeRegExp(EnglishNumeric.OrdinalRoundNumberRegex, "gis"),
                value: "OrdEng"
            }
        );

        this.regexes = regexes;
    }
}

export class EnglishPercentageExtractor extends BasePercentageExtractor {
    constructor() {
        super(new EnglishNumberExtractor())
    }

    protected initRegexes(): Array<RegExp> {
        let regexStrs = [
            EnglishNumeric.NumberWithSuffixPercentage,
            EnglishNumeric.NumberWithPrefixPercentage
        ];

        return this.buildRegexes(regexStrs);
    }
}