// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

import { BaseNumberExtractor, RegExpValue, RegExpRegExp, BasePercentageExtractor } from "../extractors";
import { Constants } from "../constants";
import { NumberMode, LongFormatType } from "../models";
import { SpanishNumeric } from "../../resources/spanishNumeric";
import { BaseNumbers } from "../../resources/baseNumbers";
import { RegExpUtility } from "@microsoft/recognizers-text";

export class SpanishNumberExtractor extends BaseNumberExtractor {
    protected extractType: string = Constants.SYS_NUM;

    constructor(mode: NumberMode = NumberMode.Default) {
        super();
        let regexes = new Array<RegExpValue>();

        // Add Cardinal
        let cardExtract: SpanishCardinalExtractor | null = null;
        switch (mode) {
            case NumberMode.PureNumber:
                cardExtract = new SpanishCardinalExtractor(SpanishNumeric.PlaceHolderPureNumber);
                break;
            case NumberMode.Currency:
                regexes.push({ regExp: RegExpUtility.getSafeRegExp(BaseNumbers.CurrencyRegex, "gs"), value: "IntegerNum" });
                break;
            case NumberMode.Default:
                break;
        }

        if (cardExtract === null) {
            cardExtract = new SpanishCardinalExtractor();
        }

        cardExtract.regexes.forEach(r => regexes.push(r));

        // Add Fraction
        let fracExtract = new SpanishFractionExtractor(mode);
        fracExtract.regexes.forEach(r => regexes.push(r));

        this.regexes = regexes;

        // Add filter
        let ambiguityFiltersDict = new Array<RegExpRegExp>();

        if (mode != NumberMode.Unit) {            
            for (let [ key, value ] of SpanishNumeric.AmbiguityFiltersDict){
                ambiguityFiltersDict.push({ regExpKey: RegExpUtility.getSafeRegExp(key, "gs"), regExpValue: RegExpUtility.getSafeRegExp(value, "gs")})
            }
        }

        this.ambiguityFiltersDict = ambiguityFiltersDict;
    }
}

export class SpanishCardinalExtractor extends BaseNumberExtractor {
    protected extractType: string = Constants.SYS_NUM_CARDINAL;

    constructor(placeholder: string = SpanishNumeric.PlaceHolderDefault) {
        super();
        let regexes = new Array<RegExpValue>();

        // Add Integer Regexes
        let intExtract = new SpanishIntegerExtractor(placeholder);
        intExtract.regexes.forEach(r => regexes.push(r));

        // Add Double Regexes
        let doubleExtract = new SpanishDoubleExtractor(placeholder);
        doubleExtract.regexes.forEach(r => regexes.push(r));

        this.regexes = regexes;
    }
}

export class SpanishIntegerExtractor extends BaseNumberExtractor {
    protected extractType: string = Constants.SYS_NUM_INTEGER;

    constructor(placeholder: string = SpanishNumeric.PlaceHolderDefault) {
        super();

        let regexes = new Array<RegExpValue>(
            {
                regExp: RegExpUtility.getSafeRegExp(SpanishNumeric.NumbersWithPlaceHolder(placeholder), "gi"),
                value: "IntegerNum"
            },
            {
                regExp: RegExpUtility.getSafeRegExp(SpanishNumeric.NumbersWithSuffix, "gs"),
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
                regExp: RegExpUtility.getSafeRegExp(SpanishNumeric.RoundNumberIntegerRegexWithLocks),
                value: "IntegerNum"
            },
            {
                regExp: RegExpUtility.getSafeRegExp(SpanishNumeric.NumbersWithDozenSuffix),
                value: "IntegerNum"
            },
            {
                regExp: RegExpUtility.getSafeRegExp(SpanishNumeric.AllIntRegexWithLocks),
                value: "IntegerSpa"
            },
            {
                regExp: RegExpUtility.getSafeRegExp(SpanishNumeric.AllIntRegexWithDozenSuffixLocks),
                value: "IntegerSpa"
            }
        );

        this.regexes = regexes;
    }
}

export class SpanishDoubleExtractor extends BaseNumberExtractor {
    protected extractType: string = Constants.SYS_NUM_DOUBLE;

    constructor(placeholder: string = SpanishNumeric.PlaceHolderDefault) {
        super();

        let regexes = new Array<RegExpValue>(
            {
                regExp: RegExpUtility.getSafeRegExp(SpanishNumeric.DoubleDecimalPointRegex(placeholder)),
                value: "DoubleNum"
            },
            {
                regExp: RegExpUtility.getSafeRegExp(SpanishNumeric.DoubleWithoutIntegralRegex(placeholder)),
                value: "DoubleNum"
            },
            {
                regExp: RegExpUtility.getSafeRegExp(SpanishNumeric.DoubleWithMultiplierRegex, "gs"),
                value: "DoubleNum"
            },
            {
                regExp: RegExpUtility.getSafeRegExp(SpanishNumeric.DoubleWithRoundNumber),
                value: "DoubleNum"
            },
            {
                regExp: RegExpUtility.getSafeRegExp(SpanishNumeric.DoubleAllFloatRegex),
                value: "DoubleSpa"
            },
            {
                regExp: RegExpUtility.getSafeRegExp(SpanishNumeric.DoubleExponentialNotationRegex),
                value: "DoublePow"
            },
            {
                regExp: RegExpUtility.getSafeRegExp(SpanishNumeric.DoubleCaretExponentialNotationRegex),
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

export class SpanishFractionExtractor extends BaseNumberExtractor {

    protected extractType: string = Constants.SYS_NUM_FRACTION;

    constructor(mode: NumberMode = NumberMode.Default) {
        super();

        let regexes = new Array<RegExpValue>(
            {
                regExp: RegExpUtility.getSafeRegExp(SpanishNumeric.FractionNotationRegex),
                value: "FracNum"
            },
            {
                regExp: RegExpUtility.getSafeRegExp(SpanishNumeric.FractionNotationWithSpacesRegex),
                value: "FracNum"
            },
            {
                regExp: RegExpUtility.getSafeRegExp(SpanishNumeric.FractionNounRegex),
                value: "FracSpa"
            },
            {
                regExp: RegExpUtility.getSafeRegExp(SpanishNumeric.FractionNounWithArticleRegex),
                value: "FracSpa"
            }
        );

        // Not add FractionPrepositionRegex when the mode is Unit to avoid wrong recognize cases like "$1000 over 3"
        if (mode != NumberMode.Unit) {
            regexes.push({
                regExp: RegExpUtility.getSafeRegExp(SpanishNumeric.FractionPrepositionRegex),
                value: "FracSpa"
                });
        };

        this.regexes = regexes;
    }
}

export class SpanishOrdinalExtractor extends BaseNumberExtractor {
    protected extractType: string = Constants.SYS_NUM_ORDINAL;

    constructor() {
        super();
        let regexes = new Array<RegExpValue>(
            {
                regExp: RegExpUtility.getSafeRegExp(SpanishNumeric.OrdinalSuffixRegex),
                value: "OrdinalNum"
            },
            {
                regExp: RegExpUtility.getSafeRegExp(SpanishNumeric.OrdinalNounRegex),
                value: "OrdSpa"
            }
        );

        this.regexes = regexes;
    }
}

export class SpanishPercentageExtractor extends BasePercentageExtractor {
    constructor() {
        super(new SpanishNumberExtractor());
    }

    protected initRegexes(): RegExp[] {
        let regexStrs = [
            SpanishNumeric.NumberWithPrefixPercentage
        ];

        return this.buildRegexes(regexStrs);
    }
}