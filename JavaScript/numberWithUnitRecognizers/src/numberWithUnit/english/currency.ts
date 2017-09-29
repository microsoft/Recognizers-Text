import { CultureInfo, Culture } from "recognizers-text-number";
import { Constants } from "../constants";
import { EnglishNumberWithUnitExtractorConfiguration, EnglishNumberWithUnitParserConfiguration } from "./base";
import { EnglishNumericWithUnit } from "../../resources/englishNumericWithUnit";

export class EnglishCurrencyExtractorConfiguration extends EnglishNumberWithUnitExtractorConfiguration {
    readonly suffixList: ReadonlyMap<string, string>;
    readonly prefixList: ReadonlyMap<string, string>;
    readonly ambiguousUnitList: ReadonlyArray<string>;
    readonly extractType: string;

    constructor(ci?: CultureInfo) {
        if (!ci) {
            ci = new CultureInfo(Culture.English);
        }

        super(ci);

        this.extractType = Constants.SYS_UNIT_CURRENCY;

        // Reference Source: https:// en.wikipedia.org/wiki/List_of_circulating_currencies
        this.suffixList = EnglishNumericWithUnit.CurrencySuffixList;
        this.prefixList = EnglishNumericWithUnit.CurrencyPrefixList;
        this.ambiguousUnitList = EnglishNumericWithUnit.AmbiguousCurrencyUnitList;
    }
}

export class EnglishCurrencyParserConfiguration extends EnglishNumberWithUnitParserConfiguration {
    constructor(ci?: CultureInfo) {
        if(!ci) {
            ci = new CultureInfo(Culture.English);
        }

        super(ci);

        this.BindDictionary(EnglishNumericWithUnit.CurrencySuffixList);
        this.BindDictionary(EnglishNumericWithUnit.CurrencyPrefixList);
    }
}