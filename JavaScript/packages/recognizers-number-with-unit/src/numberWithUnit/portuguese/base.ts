import { IExtractor, IParser, RegExpUtility } from "@microsoft/recognizers-text";
import { Culture, CultureInfo, NumberMode, AgnosticNumberParserFactory, AgnosticNumberParserType, PortugueseNumberExtractor, PortugueseNumberParserConfiguration } from "@microsoft/recognizers-text-number";
import { Constants } from "../constants";
import { INumberWithUnitExtractorConfiguration } from "../extractors";
import { BaseNumberWithUnitParserConfiguration } from "../parsers";
import { PortugueseNumericWithUnit } from "../../resources/portugueseNumericWithUnit";
import { BaseUnits } from "../../resources/baseUnits";

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
    readonly compoundUnitConnectorRegex: RegExp;
    readonly nonUnitRegex: RegExp;
    readonly ambiguousUnitNumberMultiplierRegex: RegExp;

    constructor(ci: CultureInfo) {
        this.cultureInfo = ci;
        this.unitNumExtractor = new PortugueseNumberExtractor();

        this.buildPrefix = PortugueseNumericWithUnit.BuildPrefix;
        this.buildSuffix = PortugueseNumericWithUnit.BuildSuffix;
        this.connectorToken = PortugueseNumericWithUnit.ConnectorToken;
        this.compoundUnitConnectorRegex = RegExpUtility.getSafeRegExp(PortugueseNumericWithUnit.CompoundUnitConnectorRegex);
        this.nonUnitRegex = RegExpUtility.getSafeRegExp(BaseUnits.PmNonUnitRegex);
    }
}

export class PortugueseNumberWithUnitParserConfiguration extends BaseNumberWithUnitParserConfiguration {
    readonly internalNumberParser: IParser;
    readonly internalNumberExtractor: IExtractor;
    readonly connectorToken: string;
    readonly currencyNameToIsoCodeMap: ReadonlyMap<string, string>;
    readonly currencyFractionCodeList: ReadonlyMap<string, string>;

    constructor(ci: CultureInfo) {
        super(ci);

        this.internalNumberExtractor = new PortugueseNumberExtractor(NumberMode.Default);
        this.internalNumberParser = AgnosticNumberParserFactory.getParser(AgnosticNumberParserType.Number, new PortugueseNumberParserConfiguration());
        this.connectorToken = PortugueseNumericWithUnit.ConnectorToken;
    }
}