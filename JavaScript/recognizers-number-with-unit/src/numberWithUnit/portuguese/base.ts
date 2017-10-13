import { Culture, CultureInfo, IExtractor, NumberMode, IParser, AgnosticNumberParserFactory, AgnosticNumberParserType, PortugueseNumberExtractor, PortugueseNumberParserConfiguration } from "recognizers-text-number";
import { Constants } from "../constants";
import { INumberWithUnitExtractorConfiguration } from "../extractors";
import { BaseNumberWithUnitParserConfiguration } from "../parsers";
import { PortugueseNumericWithUnit } from "../../resources/portugueseNumericWithUnit";

export abstract class PortugueseNumberWithUnitExtractorConfiguration implements INumberWithUnitExtractorConfiguration {
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
        this.unitNumExtractor = new PortugueseNumberExtractor();

        this.buildPrefix = PortugueseNumericWithUnit.BuildPrefix;
        this.buildSuffix = PortugueseNumericWithUnit.BuildSuffix;
        this.connectorToken = PortugueseNumericWithUnit.ConnectorToken;
    }
}

export class PortugueseNumberWithUnitParserConfiguration extends BaseNumberWithUnitParserConfiguration {
    readonly internalNumberParser: IParser;
    readonly internalNumberExtractor: IExtractor;
    readonly connectorToken: string;

    constructor(ci: CultureInfo) {
        super(ci);

        this.internalNumberExtractor = new PortugueseNumberExtractor(NumberMode.Default);
        this.internalNumberParser = AgnosticNumberParserFactory.getParser(AgnosticNumberParserType.Number, new PortugueseNumberParserConfiguration());
        this.connectorToken = PortugueseNumericWithUnit.ConnectorToken;
    }
}