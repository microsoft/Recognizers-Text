// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

import { BaseNumberExtractor, RegExpValue, RegExpRegExp, BasePercentageExtractor } from "../extractors";
import { Constants } from "../constants";
import { NumberMode, LongFormatType } from "../models";
import { HebrewNumeric } from "../../resources/HebrewNumeric";
import { BaseNumbers } from "../../resources/baseNumbers";
import { RegExpUtility } from "@microsoft/recognizers-text";

export class HebrewNumberExtractor extends BaseNumberExtractor {
    protected extractType: string = Constants.SYS_NUM;
    protected negativeNumberTermsRegex: RegExp;

    constructor(mode: NumberMode = NumberMode.Default) {
        super();

        this.negativeNumberTermsRegex = RegExpUtility.getSafeRegExp(HebrewNumeric.NegativeNumberTermsRegex + "$", "is");

        let regexes = new Array<RegExpValue>();

        // Add Cardinal
        let cardExtract: HebrewCardinalExtractor | null = null;
        switch (mode) {
            case NumberMode.PureNumber:
                cardExtract = new HebrewCardinalExtractor(HebrewNumeric.PlaceHolderPureNumber);
                break;
            case NumberMode.Currency:
                regexes.push({ regExp: RegExpUtility.getSafeRegExp(BaseNumbers.CurrencyRegex, "gs"), value: "IntegerNum" });
                break;
            case NumberMode.Default:
                break;
        }

        if (cardExtract === null) {
            cardExtract = new HebrewCardinalExtractor();
        }

        cardExtract.regexes.forEach(r => regexes.push(r));

        // Add Fraction
        let fracExtract = new HebrewFractionExtractor(mode);
        fracExtract.regexes.forEach(r => regexes.push(r));

        this.regexes = regexes;

        // Add filter
        let ambiguityFiltersDict = new Array<RegExpRegExp>();

        if (mode != NumberMode.Unit) {
            for (let [ key, value ] of HebrewNumeric.AmbiguityFiltersDict){
                ambiguityFiltersDict.push({ regExpKey: RegExpUtility.getSafeRegExp(key, "gs"), regExpValue: RegExpUtility.getSafeRegExp(value, "gs")})
            }
            
        }

        this.ambiguityFiltersDict = ambiguityFiltersDict;
    }
}

export class HebrewCardinalExtractor extends BaseNumberExtractor {
    protected extractType: string = Constants.SYS_NUM_CARDINAL;

    constructor(placeholder: string = HebrewNumeric.PlaceHolderDefault) {
        super();
        let regexes = new Array<RegExpValue>();

        // Add Integer Regexes
        let intExtract = new HebrewIntegerExtractor(placeholder);
        intExtract.regexes.forEach(r => regexes.push(r));

        // Add Double Regexes
        let doubleExtract = new HebrewDoubleExtractor(placeholder);
        doubleExtract.regexes.forEach(r => regexes.push(r));

        this.regexes = regexes;
    }
}

export class HebrewIntegerExtractor extends BaseNumberExtractor {
    protected extractType: string = Constants.SYS_NUM_INTEGER;

    constructor(placeholder: string = HebrewNumeric.PlaceHolderDefault) {
        super();

        let regexes = new Array<RegExpValue>(
            {
                regExp: RegExpUtility.getSafeRegExp(HebrewNumeric.NumbersWithPlaceHolder(placeholder), "gi"),
                value: "IntegerNum"
            },
            {
                regExp: RegExpUtility.getSafeRegExp(HebrewNumeric.NumbersWithSuffix, "gs"),
                value: "IntegerNum"
            },
            {
                regExp: this.generateLongFormatNumberRegexes(LongFormatType.integerNumComma, placeholder),
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
                regExp: RegExpUtility.getSafeRegExp(HebrewNumeric.RoundNumberIntegerRegexWithLocks, "gis"),
                value: "IntegerNum"
            },
            {
                regExp: RegExpUtility.getSafeRegExp(HebrewNumeric.NumbersWithDozenSuffix, "gis"),
                value: "IntegerNum"
            },
            {
                regExp: RegExpUtility.getSafeRegExp(HebrewNumeric.AllIntRegexWithLocks, "gis"),
                value: "IntegerHeb"
            },
            {
                regExp: RegExpUtility.getSafeRegExp(HebrewNumeric.AllIntRegexWithDozenSuffixLocks, "gis"),
                value: "IntegerHeb"
            }
        );

        this.regexes = regexes;
    }
}

export class HebrewDoubleExtractor extends BaseNumberExtractor {
    protected extractType: string = Constants.SYS_NUM_DOUBLE;

    constructor(placeholder: string = HebrewNumeric.PlaceHolderDefault) {
        super();

        let regexes = new Array<RegExpValue>(
            {
                regExp: RegExpUtility.getSafeRegExp(HebrewNumeric.DoubleDecimalPointRegex(placeholder), "gis"),
                value: "DoubleNum"
            },
            {
                regExp: RegExpUtility.getSafeRegExp(HebrewNumeric.DoubleWithoutIntegralRegex(placeholder), "gis"),
                value: "DoubleNum"
            },
            {
                regExp: this.generateLongFormatNumberRegexes(LongFormatType.doubleNumCommaDot, placeholder),
                value: "DoubleNum"
            },
            {
                regExp: this.generateLongFormatNumberRegexes(LongFormatType.doubleNumNoBreakSpaceDot, placeholder),
                value: "DoubleNum"
            },
            {
                regExp: RegExpUtility.getSafeRegExp(HebrewNumeric.DoubleWithMultiplierRegex, "gs"),
                value: "DoubleNum"
            },
            {
                regExp: RegExpUtility.getSafeRegExp(HebrewNumeric.DoubleWithRoundNumber, "gis"),
                value: "DoubleNum"
            },
            {
                regExp: RegExpUtility.getSafeRegExp(HebrewNumeric.DoubleAllFloatRegex, "gis"),
                value: "DoubleHeb"
            },
            {
                regExp: RegExpUtility.getSafeRegExp(HebrewNumeric.DoubleExponentialNotationRegex, "gis"),
                value: "DoublePow"
            },
            {
                regExp: RegExpUtility.getSafeRegExp(HebrewNumeric.DoubleCaretExponentialNotationRegex, "gis"),
                value: "DoublePow"
            }
        );

        this.regexes = regexes;
    }
}

export class HebrewFractionExtractor extends BaseNumberExtractor {

    protected extractType: string = Constants.SYS_NUM_FRACTION;

    constructor(mode: NumberMode = NumberMode.Default) {
        super();

        let regexes = new Array<RegExpValue>(
            {
                regExp: RegExpUtility.getSafeRegExp(HebrewNumeric.FractionNotationWithSpacesRegex, "gis"),
                value: "FracNum"
            },
            {
                regExp: RegExpUtility.getSafeRegExp(HebrewNumeric.FractionNotationRegex, "gis"),
                value: "FracNum"
            },
            {
                regExp: RegExpUtility.getSafeRegExp(HebrewNumeric.FractionNounRegex, "gis"),
                value: "FracHeb"
            },
            {
                regExp: RegExpUtility.getSafeRegExp(HebrewNumeric.FractionNounWithArticleRegex, "gis"),
                value: "FracHeb"
            }
        );

        // Not add FractionPrepositionRegex when the mode is Unit to avoid wrong recognize cases like "$1000 over 3"
        if (mode != NumberMode.Unit) {
            regexes.push({
                regExp: RegExpUtility.getSafeRegExp(HebrewNumeric.FractionPrepositionRegex, "gis"),
                value: "FracHeb"
                });
        };

        this.regexes = regexes;
    }
}

export class HebrewOrdinalExtractor extends BaseNumberExtractor {
    protected extractType: string = Constants.SYS_NUM_ORDINAL;

    constructor() {
        super();
        let regexes = new Array<RegExpValue>(
            {
                regExp: RegExpUtility.getSafeRegExp(HebrewNumeric.OrdinalNumericRegex, "gis"),
                value: "OrdinalNum"
            },
            {
                regExp: RegExpUtility.getSafeRegExp(HebrewNumeric.OrdinalEnglishRegex, "gis"),
                value: "OrdHeb"
            },
            {
                regExp: RegExpUtility.getSafeRegExp(HebrewNumeric.OrdinalRoundNumberRegex, "gis"),
                value: "OrdHeb"
            }
        );

        this.regexes = regexes;
    }
}

export class HebrewPercentageExtractor extends BasePercentageExtractor {
    constructor() {
        super(new HebrewNumberExtractor());
    }

    protected initRegexes(): RegExp[] {
        let regexStrs = [
            HebrewNumeric.NumberWithSuffixPercentage,
            HebrewNumeric.NumberWithPrefixPercentage
        ];

        return this.buildRegexes(regexStrs);
    }
}