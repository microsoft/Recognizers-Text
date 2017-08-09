import { BaseNumberExtractor, RegExpValue, BasePercentageExtractor } from "../extractors";
import { Constants } from "../constants";
import { NumberMode, ArabicType } from "../models";
import { EnglishNumericResources } from "../../resources/englishNumeric";
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
                cardExtract = new EnglishCardinalExtractor(EnglishNumericResources.PlaceHolderPureNumber);
                break;
            case NumberMode.Currency:
                regexes.push({ regExp: XRegExp(EnglishNumericResources.CurrencyRegex, "gs"), value: "IntegerNum" });
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

    constructor(placeholder: string = EnglishNumericResources.PlaceHolderDefault) {
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

    constructor(placeholder: string = EnglishNumericResources.PlaceHolderDefault) {
        super();

        let regexes = new Array<RegExpValue>(
            {
                regExp: XRegExp(EnglishNumericResources.NumbersWithPlaceHolder(placeholder), "gi"),
                value: "IntegerNum"
            },
            {
                regExp: XRegExp(EnglishNumericResources.NumbersWithSufix, "gs"),
                value: "IntegerNum"
            },
            {
                regExp: this.generateArabicNumberRegex(ArabicType.IntegerNumComma, placeholder),
                value: "IntegerNum"
            },
            {
                regExp: XRegExp(EnglishNumericResources.RoundNumberIntegerRegexWithLocks, "gis"),
                value: "IntegerNum"
            },
            {
                regExp: XRegExp(EnglishNumericResources.NumbersWithDozenSufix, "gis"),
                value: "IntegerNum"
            },
            {
                regExp: XRegExp(EnglishNumericResources.AllIntRegexWithLocks, "gis"),
                value: "IntegerEng"
            },
            {
                regExp: XRegExp(EnglishNumericResources.AllIntRegexWithDozenSufixLocks, "gis"),
                value: "IntegerEng"
            }
        );

        this.regexes = regexes;
    }
}

export class EnglishDoubleExtractor extends BaseNumberExtractor {
    protected extractType: string = Constants.SYS_NUM_DOUBLE;

    constructor(placeholder: string = EnglishNumericResources.PlaceHolderDefault) {
        super();

        let regexes = new Array<RegExpValue>(
            {
                regExp: XRegExp(EnglishNumericResources.DoubleDecimalPointRegex(placeholder), "gis"),
                value: "DoubleNum"
            },
            {
                regExp: XRegExp(EnglishNumericResources.DoubleWithoutIntegralRegex(placeholder), "gis"),
                value: "DoubleNum"
            },
            {
                regExp: this.generateArabicNumberRegex(ArabicType.DoubleNumCommaDot, placeholder),
                value: "DoubleNum"
            },
            {
                regExp: XRegExp(EnglishNumericResources.DoubleWithMultiplierRegex, "gs"),
                value: "DoubleNum"
            },
            {
                regExp: XRegExp(EnglishNumericResources.DoubleWithRoundNumber, "gis"),
                value: "DoubleNum"
            },
            {
                regExp: XRegExp(EnglishNumericResources.DoubleAllFloatRegex, "gis"),
                value: "DoubleEng"
            },
            {
                regExp: XRegExp(EnglishNumericResources.DoubleExponentialNotationRegex, "gis"),
                value: "DoublePow"
            },
            {
                regExp: XRegExp(EnglishNumericResources.DoubleCaretExponentialNotationRegex, "gis"),
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
                regExp: XRegExp(EnglishNumericResources.FractionNotationWithSpacesRegex, "gis"),
                value: "FracNum"
            },
            {
                regExp: XRegExp(EnglishNumericResources.FractionNotationRegex, "gis"),
                value: "FracNum"
            },
            {
                regExp: XRegExp(EnglishNumericResources.FractionNounRegex, "gis"),
                value: "FracEng"
            },
            {
                regExp: XRegExp(EnglishNumericResources.FractionNounWithArticleRegex, "gis"),
                value: "FracEng"
            },
            {
                regExp: XRegExp(EnglishNumericResources.FractionPrepositionRegex, "gis"),
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
                regExp: XRegExp(EnglishNumericResources.OrdinalSuffixRegex, "gis"),
                value: "OrdinalNum"
            },
            {
                regExp: XRegExp(EnglishNumericResources.OrdinalNumericRegex, "gis"),
                value: "OrdinalNum"
            },
            {
                regExp: XRegExp(EnglishNumericResources.OrdinalEnglishRegex, "gis"),
                value: "OrdEng"
            },
            {
                regExp: XRegExp(EnglishNumericResources.OrdinalRoundNumberRegex, "gis"),
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
            EnglishNumericResources.NumberWithSuffixPercentage,
            EnglishNumericResources.NumberWithPrefixPercentage
        ];

        return this.buildRegexes(regexStrs);
    }
}