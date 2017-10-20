import { CultureInfo, Culture } from "recognizers-text-number";
import { Constants } from "../constants";
import { ChineseNumberWithUnitExtractorConfiguration, ChineseNumberWithUnitParserConfiguration } from "./base";
import { ChineseNumericWithUnit } from "../../resources/chineseNumericWithUnit";

export class ChineseCurrencyExtractorConfiguration extends ChineseNumberWithUnitExtractorConfiguration {
    readonly suffixList: ReadonlyMap<string, string>;
    readonly prefixList: ReadonlyMap<string, string>;
    readonly ambiguousUnitList: ReadonlyArray<string>;
    readonly extractType: string;

    constructor(ci?: CultureInfo) {
        if (!ci) {
            ci = new CultureInfo(Culture.Chinese);
        }

        super(ci);

        this.extractType = Constants.SYS_UNIT_CURRENCY;

        // Reference Source: https:// en.wikipedia.org/wiki/List_of_circulating_currencies
        this.suffixList = ChineseNumericWithUnit.CurrencySuffixList;
        this.prefixList = ChineseNumericWithUnit.CurrencyPrefixList;
        this.ambiguousUnitList = ChineseNumericWithUnit.CurrencyAmbiguousValues;;
    }
}

export class ChineseCurrencyParserConfiguration extends ChineseNumberWithUnitParserConfiguration {
    constructor(ci?: CultureInfo) {
        if(!ci) {
            ci = new CultureInfo(Culture.Chinese);
        }

        super(ci);

        this.BindDictionary(ChineseNumericWithUnit.CurrencySuffixList);
        this.BindDictionary(ChineseNumericWithUnit.CurrencyPrefixList);
    }
}