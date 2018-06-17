import { IExtractor, IParser, RegExpUtility } from "@microsoft/recognizers-text";
import { Culture, CultureInfo, NumberMode, AgnosticNumberParserFactory, AgnosticNumberParserType, FrenchNumberExtractor, FrenchNumberParserConfiguration } from "@microsoft/recognizers-text-number";
import { Constants } from "../constants";
import { INumberWithUnitExtractorConfiguration } from "../extractors";
import { BaseNumberWithUnitParserConfiguration } from "../parsers";
import { FrenchNumericWithUnit } from "../../resources/frenchNumericWithUnit";

export abstract class FrenchNumberWithUnitExtractorConfiguration implements INumberWithUnitExtractorConfiguration {
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
        this.unitNumExtractor = new FrenchNumberExtractor();

        this.buildPrefix = FrenchNumericWithUnit.BuildPrefix;
        this.buildSuffix = FrenchNumericWithUnit.BuildSuffix;
        this.connectorToken = FrenchNumericWithUnit.ConnectorToken;
        this.compoundUnitConnectorRegex = RegExpUtility.getSafeRegExp(FrenchNumericWithUnit.CompoundUnitConnectorRegex);
    }
}

export class FrenchNumberWithUnitParserConfiguration extends BaseNumberWithUnitParserConfiguration {
    readonly internalNumberParser: IParser;
    readonly internalNumberExtractor: IExtractor;
    readonly connectorToken: string;
    readonly currencyNameToIsoCodeMap: ReadonlyMap<string, string>;
    readonly currencyFractionCodeList: ReadonlyMap<string, string>;

    constructor(ci: CultureInfo) {
        super(ci);

        this.internalNumberExtractor = new FrenchNumberExtractor(NumberMode.Default);
        this.internalNumberParser = AgnosticNumberParserFactory.getParser(AgnosticNumberParserType.Number, new FrenchNumberParserConfiguration());
        this.connectorToken = FrenchNumericWithUnit.ConnectorToken;
    }
}