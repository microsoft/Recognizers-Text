import { CultureInfo, Culture } from "@microsoft/recognizers-text-number";
import { Constants } from "../constants";
import { JapaneseNumberWithUnitExtractorConfiguration, JapaneseNumberWithUnitParserConfiguration } from "./base";
import { JapaneseNumericWithUnit } from "../../resources/japaneseNumericWithUnit";

export class JapaneseCurrencyExtractorConfiguration extends JapaneseNumberWithUnitExtractorConfiguration {
    readonly suffixList: ReadonlyMap<string, string>;
    readonly prefixList: ReadonlyMap<string, string>;
    readonly ambiguousUnitList: readonly string[];
    readonly extractType: string;

    constructor(ci?: CultureInfo) {
        if (!ci) {
            ci = new CultureInfo(Culture.Japanese);
        }

        super(ci);

        this.extractType = Constants.SYS_UNIT_CURRENCY;

        // Reference Source: https:// en.wikipedia.org/wiki/List_of_circulating_currencies
        this.suffixList = JapaneseNumericWithUnit.CurrencySuffixList;
        this.prefixList = JapaneseNumericWithUnit.CurrencyPrefixList;
        this.ambiguousUnitList = JapaneseNumericWithUnit.CurrencyAmbiguousValues;;
    }
}

export class JapaneseCurrencyParserConfiguration extends JapaneseNumberWithUnitParserConfiguration {
    constructor(ci?: CultureInfo) {
        if (!ci) {
            ci = new CultureInfo(Culture.Japanese);
        }

        super(ci);

        this.BindDictionary(JapaneseNumericWithUnit.CurrencySuffixList);
        this.BindDictionary(JapaneseNumericWithUnit.CurrencyPrefixList);
    }
}