import { IExtractor, IParser, RegExpUtility } from "@microsoft/recognizers-text";
import { Culture, CultureInfo, NumberMode, AgnosticNumberParserFactory, AgnosticNumberParserType, EnglishNumberExtractor, EnglishNumberParserConfiguration } from "@microsoft/recognizers-text-number";
import { Constants } from "../constants";
import { INumberWithUnitExtractorConfiguration } from "../extractors";
import { BaseNumberWithUnitParserConfiguration } from "../parsers";
import { EnglishNumericWithUnit } from "../../resources/englishNumericWithUnit";

export abstract class EnglishNumberWithUnitExtractorConfiguration implements INumberWithUnitExtractorConfiguration {
    abstract readonly suffixList: ReadonlyMap<string, string>;
    abstract readonly prefixList: ReadonlyMap<string, string>;
    abstract readonly ambiguousUnitList: ReadonlyArray<string>;
    readonly abstract extractType: string;

    readonly cultureInfo: CultureInfo;
    readonly unitNumExtractor: IExtractor;
    readonly buildPrefix: string;
    readonly buildSuffix: string;
    readonly connectorToken: string;
    readonly compoundUnitConnectorRegex: RegExp;

    constructor(ci: CultureInfo) {
        this.cultureInfo = ci;
        this.unitNumExtractor = new EnglishNumberExtractor();

        this.buildPrefix = EnglishNumericWithUnit.BuildPrefix;
        this.buildSuffix = EnglishNumericWithUnit.BuildSuffix;
        this.connectorToken = '';
        this.compoundUnitConnectorRegex = RegExpUtility.getSafeRegExp(EnglishNumericWithUnit.CompoundUnitConnectorRegex);
    }
}

export class EnglishNumberWithUnitParserConfiguration extends BaseNumberWithUnitParserConfiguration {
    readonly internalNumberParser: IParser;
    readonly internalNumberExtractor: IExtractor;
    readonly connectorToken: string;
    readonly currencyNameToIsoCodeMap: ReadonlyMap<string, string>;
    readonly currencyFractionCodeList: ReadonlyMap<string, string>;

    constructor(ci: CultureInfo) {
        super(ci);

        this.internalNumberExtractor = new EnglishNumberExtractor(NumberMode.Default);
        this.internalNumberParser = AgnosticNumberParserFactory.getParser(AgnosticNumberParserType.Number, new EnglishNumberParserConfiguration());
        this.connectorToken = '';
        this.currencyNameToIsoCodeMap = EnglishNumericWithUnit.CurrencyNameToIsoCodeMap;
        this.currencyFractionCodeList = EnglishNumericWithUnit.FractionalUnitNameToCodeMap;
    }
}