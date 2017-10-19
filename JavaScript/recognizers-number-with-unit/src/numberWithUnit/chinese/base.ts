import { Culture, CultureInfo, IExtractor, NumberMode, IParser, AgnosticNumberParserFactory, AgnosticNumberParserType, ChineseNumberExtractor, ChineseNumberParserConfiguration, ChineseNumberMode } from "recognizers-text-number";
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

    constructor(ci: CultureInfo) {
        this.cultureInfo = ci;
        this.unitNumExtractor = new ChineseNumberExtractor(ChineseNumberMode.ExtractAll);

        this.buildPrefix = ChineseNumericWithUnit.BuildPrefix;
        this.buildSuffix = ChineseNumericWithUnit.BuildSuffix;
        this.connectorToken = ChineseNumericWithUnit.ConnectorToken;
    }
}

export class ChineseNumberWithUnitParserConfiguration extends BaseNumberWithUnitParserConfiguration {
    readonly internalNumberParser: IParser;
    readonly internalNumberExtractor: IExtractor;
    readonly connectorToken: string;

    constructor(ci: CultureInfo) {
        super(ci);

        this.internalNumberExtractor = new ChineseNumberExtractor(ChineseNumberMode.Default);
        this.internalNumberParser = AgnosticNumberParserFactory.getParser(AgnosticNumberParserType.Number, new ChineseNumberParserConfiguration());
        this.connectorToken = '';
    }
}