import { CultureInfo, Culture } from "recognizers-text-number";
import { Constants } from "../constants";
import { PortugueseNumberWithUnitExtractorConfiguration, PortugueseNumberWithUnitParserConfiguration } from "./base";
import { PortugueseNumericWithUnit } from "../../resources/portugueseNumericWithUnit";

export class PortugueseCurrencyExtractorConfiguration extends PortugueseNumberWithUnitExtractorConfiguration {
    readonly suffixList: ReadonlyMap<string, string>;
    readonly prefixList: ReadonlyMap<string, string>;
    readonly ambiguousUnitList: ReadonlyArray<string>;
    readonly extractType: string;

    constructor(ci?: CultureInfo) {
        if (!ci) {
            ci = new CultureInfo(Culture.Portuguese);
        }

        super(ci);

        this.extractType = Constants.SYS_UNIT_CURRENCY;

        // Reference Source: https:// en.wikipedia.org/wiki/List_of_circulating_currencies
        this.suffixList = PortugueseNumericWithUnit.CurrencySuffixList;
        this.prefixList = PortugueseNumericWithUnit.CurrencyPrefixList;
        this.ambiguousUnitList = PortugueseNumericWithUnit.AmbiguousCurrencyUnitList;
    }
}

export class PortugueseCurrencyParserConfiguration extends PortugueseNumberWithUnitParserConfiguration {
    constructor(ci?: CultureInfo) {
        if(!ci) {
            ci = new CultureInfo(Culture.Portuguese);
        }

        super(ci);

        this.BindDictionary(PortugueseNumericWithUnit.CurrencySuffixList);
        this.BindDictionary(PortugueseNumericWithUnit.CurrencyPrefixList);
    }
}