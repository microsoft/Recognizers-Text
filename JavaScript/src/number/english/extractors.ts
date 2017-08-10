import { BaseNumberExtractor, RegExpValue, BasePercentageExtractor } from "../extractors";
import { Constants } from "../constants";
import { NumberMode, LongFormatType } from "../models";
import { EnglishNumeric } from "../../resources/englishNumeric";
import * as XRegExp from "xregexp";

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
                regexes.push({ regExp: XRegExp(EnglishNumeric.CurrencyRegex, "gs"), value: "IntegerNum" });
                break;
            case NumberMode.Default:
                break;
        }

        if (cardExtract == null) {
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
                regExp: XRegExp(EnglishNumeric.NumbersWithPlaceHolder(placeholder), "gi"),
                value: "IntegerNum"
            },
            {
                regExp: XRegExp(EnglishNumeric.NumbersWithSuffix, "gs"),
                value: "IntegerNum"
            },
            {
                regExp: this.generateLongFormatNumberRegexes(LongFormatType.integerNumComma, placeholder),
                value: "IntegerNum"
            },
            {
                regExp: XRegExp(EnglishNumeric.RoundNumberIntegerRegexWithLocks, "gis"),
                value: "IntegerNum"
            },
            {
                regExp: XRegExp(EnglishNumeric.NumbersWithDozenSuffix, "gis"),
                value: "IntegerNum"
            },
            {
                regExp: XRegExp(EnglishNumeric.AllIntRegexWithLocks, "gis"),
                value: "IntegerEng"
            },
            {
                regExp: XRegExp(EnglishNumeric.AllIntRegexWithDozenSuffixLocks, "gis"),
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
                regExp: XRegExp(EnglishNumeric.DoubleDecimalPointRegex(placeholder), "gis"),
                value: "DoubleNum"
            },
            {
                regExp: XRegExp(EnglishNumeric.DoubleWithoutIntegralRegex(placeholder), "gis"),
                value: "DoubleNum"
            },
            {
                regExp: this.generateLongFormatNumberRegexes(LongFormatType.doubleNumCommaDot, placeholder),
                value: "DoubleNum"
            },
            {
                regExp: XRegExp(EnglishNumeric.DoubleWithMultiplierRegex, "gs"),
                value: "DoubleNum"
            },
            {
                regExp: XRegExp(EnglishNumeric.DoubleWithRoundNumber, "gis"),
                value: "DoubleNum"
            },
            {
                regExp: XRegExp(EnglishNumeric.DoubleAllFloatRegex, "gis"),
                value: "DoubleEng"
            },
            {
                regExp: XRegExp(EnglishNumeric.DoubleExponentialNotationRegex, "gis"),
                value: "DoublePow"
            },
            {
                regExp: XRegExp(EnglishNumeric.DoubleCaretExponentialNotationRegex, "gis"),
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
                regExp: XRegExp(EnglishNumeric.FractionNotationWithSpacesRegex, "gis"),
                value: "FracNum"
            },
            {
                regExp: XRegExp(EnglishNumeric.FractionNotationRegex, "gis"),
                value: "FracNum"
            },
            {
                regExp: XRegExp(EnglishNumeric.FractionNounRegex, "gis"),
                value: "FracEng"
            },
            {
                regExp: XRegExp(EnglishNumeric.FractionNounWithArticleRegex, "gis"),
                value: "FracEng"
            },
            {
                regExp: XRegExp(EnglishNumeric.FractionPrepositionRegex, "gis"),
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
                regExp: XRegExp(EnglishNumeric.OrdinalSuffixRegex, "gis"),
                value: "OrdinalNum"
            },
            {
                regExp: XRegExp(EnglishNumeric.OrdinalNumericRegex, "gis"),
                value: "OrdinalNum"
            },
            {
                regExp: XRegExp(EnglishNumeric.OrdinalEnglishRegex, "gis"),
                value: "OrdEng"
            },
            {
                regExp: XRegExp(EnglishNumeric.OrdinalRoundNumberRegex, "gis"),
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