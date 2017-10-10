import { Culture, CultureInfo, IExtractor, NumberMode, IParser, AgnosticNumberParserFactory, AgnosticNumberParserType, EnglishNumberExtractor, EnglishNumberParserConfiguration } from "recognizers-text-number";
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

    constructor(ci: CultureInfo) {
        this.cultureInfo = ci;
        this.unitNumExtractor = new EnglishNumberExtractor();

        this.buildPrefix = EnglishNumericWithUnit.BuildPrefix;
        this.buildSuffix = EnglishNumericWithUnit.BuildSuffix;
        this.connectorToken = '';
    }
}

export class EnglishNumberWithUnitParserConfiguration extends BaseNumberWithUnitParserConfiguration {
    readonly internalNumberParser: IParser;
    readonly internalNumberExtractor: IExtractor;
    readonly connectorToken: string;

    constructor(ci: CultureInfo) {
        super(ci);

        this.internalNumberExtractor = new EnglishNumberExtractor(NumberMode.Default);
        this.internalNumberParser = AgnosticNumberParserFactory.getParser(AgnosticNumberParserType.Number, new EnglishNumberParserConfiguration());
        this.connectorToken = '';
    }
}