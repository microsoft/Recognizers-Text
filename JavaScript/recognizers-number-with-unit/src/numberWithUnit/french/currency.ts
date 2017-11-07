import { CultureInfo, Culture } from "recognizers-text-number";
import { Constants } from "../constants";
import { FrenchNumberWithUnitExtractorConfiguration, FrenchNumberWithUnitParserConfiguration } from "./base";
import { FrenchNumericWithUnit } from "../../resources/frenchNumericWithUnit";

export class FrenchCurrencyExtractorConfiguration extends FrenchNumberWithUnitExtractorConfiguration {
    readonly suffixList: ReadonlyMap<string, string>;
    readonly prefixList: ReadonlyMap<string, string>;
    readonly ambiguousUnitList: ReadonlyArray<string>;
    readonly extractType: string;

    constructor(ci?: CultureInfo) {
        if (!ci) {
            ci = new CultureInfo(Culture.French);
        }

        super(ci);

        this.extractType = Constants.SYS_UNIT_CURRENCY;

        // Reference Source: https:// en.wikipedia.org/wiki/List_of_circulating_currencies
        this.suffixList = FrenchNumericWithUnit.CurrencySuffixList;
        this.prefixList = FrenchNumericWithUnit.CurrencyPrefixList;
        this.ambiguousUnitList = FrenchNumericWithUnit.AmbiguousCurrencyUnitList;
    }
}

export class FrenchCurrencyParserConfiguration extends FrenchNumberWithUnitParserConfiguration {
    constructor(ci?: CultureInfo) {
        if(!ci) {
            ci = new CultureInfo(Culture.French);
        }

        super(ci);

        this.BindDictionary(FrenchNumericWithUnit.CurrencySuffixList);
        this.BindDictionary(FrenchNumericWithUnit.CurrencyPrefixList);
    }
}