import { Culture, CultureInfo, IExtractor, NumberMode, IParser, AgnosticNumberParserFactory, AgnosticNumberParserType, SpanishNumberExtractor, SpanishNumberParserConfiguration } from "recognizers-text-number";
import { Constants } from "../constants";
import { INumberWithUnitExtractorConfiguration } from "../extractors";
import { BaseNumberWithUnitParserConfiguration } from "../parsers";
import { SpanishNumericWithUnit } from "../../resources/spanishNumericWithUnit";

export abstract class SpanishNumberWithUnitExtractorConfiguration implements INumberWithUnitExtractorConfiguration {
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
        this.unitNumExtractor = new SpanishNumberExtractor();

        this.buildPrefix = SpanishNumericWithUnit.BuildPrefix;
        this.buildSuffix = SpanishNumericWithUnit.BuildSuffix;
        this.connectorToken = SpanishNumericWithUnit.ConnectorToken;
    }
}

export class SpanishNumberWithUnitParserConfiguration extends BaseNumberWithUnitParserConfiguration {
    readonly internalNumberParser: IParser;
    readonly internalNumberExtractor: IExtractor;
    readonly connectorToken: string;

    constructor(ci: CultureInfo) {
        super(ci);

        this.internalNumberExtractor = new SpanishNumberExtractor(NumberMode.Default);
        this.internalNumberParser = AgnosticNumberParserFactory.getParser(AgnosticNumberParserType.Number, new SpanishNumberParserConfiguration());
        this.connectorToken = SpanishNumericWithUnit.ConnectorToken;
    }
}