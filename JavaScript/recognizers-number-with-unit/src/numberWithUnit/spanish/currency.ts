import { CultureInfo, Culture } from "recognizers-text-number";
import { Constants } from "../constants";
import { SpanishNumberWithUnitExtractorConfiguration, SpanishNumberWithUnitParserConfiguration } from "./base";
import { SpanishNumericWithUnit } from "../../resources/spanishNumericWithUnit";

export class SpanishCurrencyExtractorConfiguration extends SpanishNumberWithUnitExtractorConfiguration {
    readonly suffixList: ReadonlyMap<string, string>;
    readonly prefixList: ReadonlyMap<string, string>;
    readonly ambiguousUnitList: ReadonlyArray<string>;
    readonly extractType: string;

    constructor(ci?: CultureInfo) {
        if (!ci) {
            ci = new CultureInfo(Culture.Spanish);
        }

        super(ci);

        this.extractType = Constants.SYS_UNIT_CURRENCY;

        // Reference Source: https:// en.wikipedia.org/wiki/List_of_circulating_currencies
        this.suffixList = SpanishNumericWithUnit.CurrencySuffixList;
        this.prefixList = SpanishNumericWithUnit.CurrencyPrefixList;
        this.ambiguousUnitList = SpanishNumericWithUnit.AmbiguousCurrencyUnitList;
    }
}

export class SpanishCurrencyParserConfiguration extends SpanishNumberWithUnitParserConfiguration {
    constructor(ci?: CultureInfo) {
        if(!ci) {
            ci = new CultureInfo(Culture.Spanish);
        }

        super(ci);

        this.BindDictionary(SpanishNumericWithUnit.CurrencySuffixList);
        this.BindDictionary(SpanishNumericWithUnit.CurrencyPrefixList);
    }
}