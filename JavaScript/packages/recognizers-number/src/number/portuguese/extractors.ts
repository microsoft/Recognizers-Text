// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

import { BaseNumberExtractor, RegExpValue, RegExpRegExp, BasePercentageExtractor } from "../extractors";
import { Constants } from "../constants";
import { NumberMode, LongFormatType } from "../models";
import { PortugueseNumeric } from "../../resources/portugueseNumeric";
import { BaseNumbers } from "../../resources/baseNumbers";
import { RegExpUtility } from "@microsoft/recognizers-text";

export class PortugueseNumberExtractor extends BaseNumberExtractor {
    protected extractType: string = Constants.SYS_NUM;

    constructor(mode: NumberMode = NumberMode.Default) {
        super();
        let regexes = new Array<RegExpValue>();

        // Add Cardinal
        let cardExtract: PortugueseCardinalExtractor | null = null;
        switch (mode) {
            case NumberMode.PureNumber:
                cardExtract = new PortugueseCardinalExtractor(PortugueseNumeric.PlaceHolderPureNumber);
                break;
            case NumberMode.Currency:
                regexes.push({ regExp: RegExpUtility.getSafeRegExp(BaseNumbers.CurrencyRegex, "gs"), value: "IntegerNum" });
                break;
            case NumberMode.Default:
                break;
        }

        if (cardExtract === null) {
            cardExtract = new PortugueseCardinalExtractor();
        }

        cardExtract.regexes.forEach(r => regexes.push(r));

        // Add Fraction
        let fracExtract = new PortugueseFractionExtractor(mode);
        fracExtract.regexes.forEach(r => regexes.push(r));

        this.regexes = regexes;

        // Add filter
        let ambiguityFiltersDict = new Array<RegExpRegExp>();

        if (mode != NumberMode.Unit) {            
            for (let [ key, value ] of PortugueseNumeric.AmbiguityFiltersDict){
                ambiguityFiltersDict.push({ regExpKey: RegExpUtility.getSafeRegExp(key, "gs"), regExpValue: RegExpUtility.getSafeRegExp(value, "gs")})
            }
        }

        this.ambiguityFiltersDict = ambiguityFiltersDict;
    }
}

export class PortugueseCardinalExtractor extends BaseNumberExtractor {
    protected extractType: string = Constants.SYS_NUM_CARDINAL;

    constructor(placeholder: string = PortugueseNumeric.PlaceHolderDefault) {
        super();
        let regexes = new Array<RegExpValue>();

        // Add Integer Regexes
        let intExtract = new PortugueseIntegerExtractor(placeholder);
        intExtract.regexes.forEach(r => regexes.push(r));

        // Add Double Regexes
        let doubleExtract = new PortugueseDoubleExtractor(placeholder);
        doubleExtract.regexes.forEach(r => regexes.push(r));

        this.regexes = regexes;
    }
}

export class PortugueseIntegerExtractor extends BaseNumberExtractor {
    protected extractType: string = Constants.SYS_NUM_INTEGER;

    constructor(placeholder: string = PortugueseNumeric.PlaceHolderDefault) {
        super();

        let regexes = new Array<RegExpValue>(
            {
                regExp: RegExpUtility.getSafeRegExp(PortugueseNumeric.NumbersWithPlaceHolder(placeholder), "gi"),
                value: "IntegerNum"
            },
            {
                regExp: RegExpUtility.getSafeRegExp(PortugueseNumeric.NumbersWithSuffix, "gs"),
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
                regExp: RegExpUtility.getSafeRegExp(PortugueseNumeric.RoundNumberIntegerRegexWithLocks),
                value: "IntegerNum"
            },
            {
                regExp: RegExpUtility.getSafeRegExp(PortugueseNumeric.NumbersWithDozen2Suffix),
                value: "IntegerNum"
            },
            {
                regExp: RegExpUtility.getSafeRegExp(PortugueseNumeric.NumbersWithDozenSuffix),
                value: "IntegerNum"
            },
            {
                regExp: RegExpUtility.getSafeRegExp(PortugueseNumeric.AllIntRegexWithLocks),
                value: "IntegerPor"
            },
            {
                regExp: RegExpUtility.getSafeRegExp(PortugueseNumeric.AllIntRegexWithDozenSuffixLocks),
                value: "IntegerPor"
            }
        );

        this.regexes = regexes;
    }
}

export class PortugueseDoubleExtractor extends BaseNumberExtractor {
    protected extractType: string = Constants.SYS_NUM_DOUBLE;

    constructor(placeholder: string = PortugueseNumeric.PlaceHolderDefault) {
        super();

        let regexes = new Array<RegExpValue>(
            {
                regExp: RegExpUtility.getSafeRegExp(PortugueseNumeric.DoubleDecimalPointRegex(placeholder)),
                value: "DoubleNum"
            },
            {
                regExp: RegExpUtility.getSafeRegExp(PortugueseNumeric.DoubleWithoutIntegralRegex(placeholder)),
                value: "DoubleNum"
            },
            {
                regExp: RegExpUtility.getSafeRegExp(PortugueseNumeric.DoubleWithMultiplierRegex, "gs"),
                value: "DoubleNum"
            },
            {
                regExp: RegExpUtility.getSafeRegExp(PortugueseNumeric.DoubleWithRoundNumber),
                value: "DoubleNum"
            },
            {
                regExp: RegExpUtility.getSafeRegExp(PortugueseNumeric.DoubleAllFloatRegex),
                value: "DoublePor"
            },
            {
                regExp: RegExpUtility.getSafeRegExp(PortugueseNumeric.DoubleExponentialNotationRegex),
                value: "DoublePow"
            },
            {
                regExp: RegExpUtility.getSafeRegExp(PortugueseNumeric.DoubleCaretExponentialNotationRegex),
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

export class PortugueseFractionExtractor extends BaseNumberExtractor {

    protected extractType: string = Constants.SYS_NUM_FRACTION;

    constructor(mode: NumberMode = NumberMode.Default) {
        super();

        let regexes = new Array<RegExpValue>(
            {
                regExp: RegExpUtility.getSafeRegExp(PortugueseNumeric.FractionNotationRegex),
                value: "FracNum"
            },
            {
                regExp: RegExpUtility.getSafeRegExp(PortugueseNumeric.FractionNotationWithSpacesRegex),
                value: "FracNum"
            },
            {
                regExp: RegExpUtility.getSafeRegExp(PortugueseNumeric.FractionNounRegex),
                value: "FracPor"
            },
            {
                regExp: RegExpUtility.getSafeRegExp(PortugueseNumeric.FractionNounWithArticleRegex),
                value: "FracPor"
            }
        );

        // Not add FractionPrepositionRegex when the mode is Unit to avoid wrong recognize cases like "$1000 over 3"
        if (mode != NumberMode.Unit) {
            regexes.push({
                regExp: RegExpUtility.getSafeRegExp(PortugueseNumeric.FractionPrepositionRegex),
                value: "FracPor"
                });
        };

        this.regexes = regexes;
    }
}

export class PortugueseOrdinalExtractor extends BaseNumberExtractor {
    protected extractType: string = Constants.SYS_NUM_ORDINAL;

    constructor() {
        super();
        let regexes = new Array<RegExpValue>(
            {
                regExp: RegExpUtility.getSafeRegExp(PortugueseNumeric.OrdinalSuffixRegex),
                value: "OrdinalNum"
            },
            {
                regExp: RegExpUtility.getSafeRegExp(PortugueseNumeric.OrdinalEnglishRegex),
                value: "OrdinalPor"
            }
        );

        this.regexes = regexes;
    }
}

export class PortuguesePercentageExtractor extends BasePercentageExtractor {
    constructor() {
        super(new PortugueseNumberExtractor());
    }

    protected initRegexes(): RegExp[] {
        let regexStrs = [
            PortugueseNumeric.NumberWithSuffixPercentage
        ];

        return this.buildRegexes(regexStrs);
    }
}