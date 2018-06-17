import { IExtractor, IParser, RegExpUtility } from "@microsoft/recognizers-text";
import { Culture, CultureInfo, NumberMode, AgnosticNumberParserFactory, AgnosticNumberParserType, ChineseNumberExtractor, ChineseNumberParserConfiguration, ChineseNumberExtractorMode } from "@microsoft/recognizers-text-number";
import { Constants } from "../constants";
import { INumberWithUnitExtractorConfiguration } from "../extractors";
import { BaseNumberWithUnitParserConfiguration } from "../parsers";
import { ChineseNumericWithUnit } from "../../resources/chineseNumericWithUnit";

export abstract class ChineseNumberWithUnitExtractorConfiguration implements INumberWithUnitExtractorConfiguration {
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
        this.unitNumExtractor = new ChineseNumberExtractor(ChineseNumberExtractorMode.ExtractAll);

        this.buildPrefix = ChineseNumericWithUnit.BuildPrefix;
        this.buildSuffix = ChineseNumericWithUnit.BuildSuffix;
        this.connectorToken = ChineseNumericWithUnit.ConnectorToken;
        this.compoundUnitConnectorRegex = RegExpUtility.getSafeRegExp(ChineseNumericWithUnit.CompoundUnitConnectorRegex);
    }
}

export class ChineseNumberWithUnitParserConfiguration extends BaseNumberWithUnitParserConfiguration {
    readonly internalNumberParser: IParser;
    readonly internalNumberExtractor: IExtractor;
    readonly connectorToken: string;
    readonly currencyNameToIsoCodeMap: ReadonlyMap<string, string>;
    readonly currencyFractionCodeList: ReadonlyMap<string, string>;

    constructor(ci: CultureInfo) {
        super(ci);

        this.internalNumberExtractor = new ChineseNumberExtractor(ChineseNumberExtractorMode.Default);
        this.internalNumberParser = AgnosticNumberParserFactory.getParser(AgnosticNumberParserType.Number, new ChineseNumberParserConfiguration());
        this.connectorToken = '';
        this.currencyNameToIsoCodeMap = ChineseNumericWithUnit.CurrencyNameToIsoCodeMap;
        this.currencyFractionCodeList = ChineseNumericWithUnit.FractionalUnitNameToCodeMap;
    }
}