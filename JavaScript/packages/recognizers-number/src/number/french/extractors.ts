import { BaseNumberExtractor, RegExpValue, RegExpRegExp, BasePercentageExtractor } from "../extractors";
import { Constants } from "../constants";
import { NumberMode, LongFormatType } from "../models";
import { FrenchNumeric } from "../../resources/frenchNumeric";
import { BaseNumbers } from "../../resources/baseNumbers";
import { RegExpUtility } from "@microsoft/recognizers-text";

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
                regexes.push({ regExp: RegExpUtility.getSafeRegExp(BaseNumbers.CurrencyRegex, "gs"), value: "IntegerNum" });
                break;
            case NumberMode.Default:
                break;
        }

        if (cardExtract === null) {
            cardExtract = new FrenchCardinalExtractor();
        }

        cardExtract.regexes.forEach(r => regexes.push(r));

        // Add Fraction
        let fracExtract = new FrenchFractionExtractor(mode);
        fracExtract.regexes.forEach(r => regexes.push(r));

        this.regexes = regexes;

        // Add filter
        let ambiguityFiltersDict = new Array<RegExpRegExp>();

        if (mode != NumberMode.Unit) {            
            for (let [ key, value ] of FrenchNumeric.AmbiguityFiltersDict){
                ambiguityFiltersDict.push({ regExpKey: RegExpUtility.getSafeRegExp(key, "gs"), regExpValue: RegExpUtility.getSafeRegExp(value, "gs")})
            }
            
        }

        this.ambiguityFiltersDict = ambiguityFiltersDict;
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
                regExp: this.generateLongFormatNumberRegexes(LongFormatType.integerNumBlank, placeholder),
                value: "IntegerNum"
            },
            {
                regExp: this.generateLongFormatNumberRegexes(LongFormatType.integerNumNoBreakSpace, placeholder),
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
                value: "Integer" + FrenchNumeric.LangMarker
            },
            {
                regExp: RegExpUtility.getSafeRegExp(FrenchNumeric.AllIntRegexWithDozenSuffixLocks),
                value: "Integer" + FrenchNumeric.LangMarker
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
                value: "Double" + FrenchNumeric.LangMarker
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
            },
            {
                regExp: this.generateLongFormatNumberRegexes(LongFormatType.doubleNumNoBreakSpaceComma, placeholder),
                value: "DoubleNum"
            }
        );

        this.regexes = regexes;
    }
}

export class FrenchFractionExtractor extends BaseNumberExtractor {

    protected extractType: string = Constants.SYS_NUM_FRACTION;

    constructor(mode: NumberMode = NumberMode.Default) {
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
                value: "Frac" + FrenchNumeric.LangMarker
            },
            {
                regExp: RegExpUtility.getSafeRegExp(FrenchNumeric.FractionNounWithArticleRegex),
                value: "Frac" + FrenchNumeric.LangMarker
            }
        );

        // Not add FractionPrepositionRegex when the mode is Unit to avoid wrong recognize cases like "$1000 over 3"
        if (mode != NumberMode.Unit) {
            regexes.push({
                regExp: RegExpUtility.getSafeRegExp(FrenchNumeric.FractionPrepositionRegex),
                value: "Frac" + FrenchNumeric.LangMarker
                });
        };

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
                value: "Ord" + FrenchNumeric.LangMarker
            }
        );

        this.regexes = regexes;
    }
}

export class FrenchPercentageExtractor extends BasePercentageExtractor {
    constructor() {
        super(new FrenchNumberExtractor());
    }

    protected initRegexes(): RegExp[] {
        let regexStrs = [
            FrenchNumeric.NumberWithSuffixPercentage,
            FrenchNumeric.NumberWithPrefixPercentage
        ];

        return this.buildRegexes(regexStrs);
    }
}