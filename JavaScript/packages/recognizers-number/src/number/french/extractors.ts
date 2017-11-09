import { BaseNumberExtractor, RegExpValue, BasePercentageExtractor } from "../extractors";
import { Constants } from "../constants";
import { NumberMode, LongFormatType } from "../models";
import { FrenchNumeric } from "../../resources/frenchNumeric";
import { RegExpUtility } from "../../utilities"

export class FrenchNumberExtractor extends BaseNumberExtractor {
    protected extractType: string = Constants.SYS_NUM;

    constructor(mode: NumberMode = NumberMode.Default) {
        super();
        let regexes = new Array<RegExpValue>();

        // Add Cardinal
        let cardExtract: FrenchCardinalExtractor | null = null;
        switch (mode) {
            case NumberMode.PureNumber:
                cardExtract = new FrenchCardinalExtractor(FrenchNumeric.PlaceHolderPureNumber);
                break;
            case NumberMode.Currency:
                regexes.push({ regExp: RegExpUtility.getSafeRegExp(FrenchNumeric.CurrencyRegex, "gs"), value: "IntegerNum" });
                break;
            case NumberMode.Default:
                break;
        }

        if (cardExtract === null) {
            cardExtract = new FrenchCardinalExtractor();
        }

        cardExtract.regexes.forEach(r => regexes.push(r));

        // Add Fraction
        let fracExtract = new FrenchFractionExtractor();
        fracExtract.regexes.forEach(r => regexes.push(r));

        this.regexes = regexes;
    }
}

export class FrenchCardinalExtractor extends BaseNumberExtractor {
    protected extractType: string = Constants.SYS_NUM_CARDINAL;

    constructor(placeholder: string = FrenchNumeric.PlaceHolderDefault) {
        super();
        let regexes = new Array<RegExpValue>();

        // Add Integer Regexes
        let intExtract = new FrenchIntegerExtractor(placeholder);
        intExtract.regexes.forEach(r => regexes.push(r));

        // Add Double Regexes
        let doubleExtract = new FrenchDoubleExtractor(placeholder);
        doubleExtract.regexes.forEach(r => regexes.push(r));

        this.regexes = regexes;
    }
}

export class FrenchIntegerExtractor extends BaseNumberExtractor {
    protected extractType: string = Constants.SYS_NUM_INTEGER;

    constructor(placeholder: string = FrenchNumeric.PlaceHolderDefault) {
        super();

        let regexes = new Array<RegExpValue>(
            {
                regExp: RegExpUtility.getSafeRegExp(FrenchNumeric.NumbersWithPlaceHolder(placeholder), "gi"),
                value: "IntegerNum"
            },
            {
                regExp: RegExpUtility.getSafeRegExp(FrenchNumeric.NumbersWithSuffix, "gs"),
                value: "IntegerNum"
            },
            {
                regExp: this.generateLongFormatNumberRegexes(LongFormatType.integerNumDot, placeholder),
                value: "IntegerNum"
            },
            {
                regExp: RegExpUtility.getSafeRegExp(FrenchNumeric.RoundNumberIntegerRegexWithLocks),
                value: "IntegerNum"
            },
            {
                regExp: RegExpUtility.getSafeRegExp(FrenchNumeric.NumbersWithDozenSuffix),
                value: "IntegerNum"
            },
            {
                regExp: RegExpUtility.getSafeRegExp(FrenchNumeric.AllIntRegexWithLocks),
                value: "IntegerFr"
            },
            {
                regExp: RegExpUtility.getSafeRegExp(FrenchNumeric.AllIntRegexWithDozenSuffixLocks),
                value: "IntegerFr"
            }
        );

        this.regexes = regexes;
    }
}

export class FrenchDoubleExtractor extends BaseNumberExtractor {
    protected extractType: string = Constants.SYS_NUM_DOUBLE;

    constructor(placeholder: string = FrenchNumeric.PlaceHolderDefault) {
        super();

        let regexes = new Array<RegExpValue>(
            {
                regExp: RegExpUtility.getSafeRegExp(FrenchNumeric.DoubleDecimalPointRegex(placeholder)),
                value: "DoubleNum"
            },
            {
                regExp: RegExpUtility.getSafeRegExp(FrenchNumeric.DoubleWithoutIntegralRegex(placeholder)),
                value: "DoubleNum"
            },
            {
                regExp: RegExpUtility.getSafeRegExp(FrenchNumeric.DoubleWithMultiplierRegex, "gs"),
                value: "DoubleNum"
            },
            {
                regExp: RegExpUtility.getSafeRegExp(FrenchNumeric.DoubleWithRoundNumber),
                value: "DoubleNum"
            },
            {
                regExp: RegExpUtility.getSafeRegExp(FrenchNumeric.DoubleAllFloatRegex),
                value: "DoubleFr"
            },
            {
                regExp: RegExpUtility.getSafeRegExp(FrenchNumeric.DoubleExponentialNotationRegex),
                value: "DoublePow"
            },
            {
                regExp: RegExpUtility.getSafeRegExp(FrenchNumeric.DoubleCaretExponentialNotationRegex),
                value: "DoublePow"
            },
            {
                regExp: this.generateLongFormatNumberRegexes(LongFormatType.doubleNumDotComma, placeholder),
                value: "DoubleNum"
            }
        );

        this.regexes = regexes;
    }
}

export class FrenchFractionExtractor extends BaseNumberExtractor {

    protected extractType: string = Constants.SYS_NUM_FRACTION;

    constructor() {
        super();

        let regexes = new Array<RegExpValue>(
            {
                regExp: RegExpUtility.getSafeRegExp(FrenchNumeric.FractionNotationRegex),
                value: "FracNum"
            },
            {
                regExp: RegExpUtility.getSafeRegExp(FrenchNumeric.FractionNotationWithSpacesRegex),
                value: "FracNum"
            },
            {
                regExp: RegExpUtility.getSafeRegExp(FrenchNumeric.FractionNounRegex),
                value: "FracFr"
            },
            {
                regExp: RegExpUtility.getSafeRegExp(FrenchNumeric.FractionNounWithArticleRegex),
                value: "FracFr"
            },
            {
                regExp: RegExpUtility.getSafeRegExp(FrenchNumeric.FractionPrepositionRegex),
                value: "FracFr"
            }
        );

        this.regexes = regexes;
    }
}

export class FrenchOrdinalExtractor extends BaseNumberExtractor {
    protected extractType: string = Constants.SYS_NUM_ORDINAL;

    constructor() {
        super();
        let regexes = new Array<RegExpValue>(
            {
                regExp: RegExpUtility.getSafeRegExp(FrenchNumeric.OrdinalSuffixRegex),
                value: "OrdinalNum"
            },
            {
                regExp: RegExpUtility.getSafeRegExp(FrenchNumeric.OrdinalFrenchRegex),
                value: "OrdFr"
            }
        );

        this.regexes = regexes;
    }
}

export class FrenchPercentageExtractor extends BasePercentageExtractor {
    constructor() {
        super(new FrenchNumberExtractor())
    }

    protected initRegexes(): Array<RegExp> {
        let regexStrs = [
            FrenchNumeric.NumberWithSuffixPercentage,
            FrenchNumeric.NumberWithPrefixPercentage
        ];

        return this.buildRegexes(regexStrs);
    }
}