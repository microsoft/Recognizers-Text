import { IExtractor, IParser, RegExpUtility } from "@microsoft/recognizers-text";
import { Culture, CultureInfo, NumberMode, AgnosticNumberParserFactory, AgnosticNumberParserType, JapaneseNumberExtractor, JapaneseNumberParserConfiguration, JapaneseNumberExtractorMode } from "@microsoft/recognizers-text-number";
import { Constants } from "../constants";
import { INumberWithUnitExtractorConfiguration } from "../extractors";
import { BaseNumberWithUnitParserConfiguration } from "../parsers";
import { JapaneseNumericWithUnit } from "../../resources/japaneseNumericWithUnit";
import { BaseUnits } from "../../resources/baseUnits";

export abstract class JapaneseNumberWithUnitExtractorConfiguration implements INumberWithUnitExtractorConfiguration {
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
        this.unitNumExtractor = new JapaneseNumberExtractor(JapaneseNumberExtractorMode.ExtractAll);

        this.buildPrefix = JapaneseNumericWithUnit.BuildPrefix;
        this.buildSuffix = JapaneseNumericWithUnit.BuildSuffix;
        this.connectorToken = JapaneseNumericWithUnit.ConnectorToken;
        this.compoundUnitConnectorRegex = RegExpUtility.getSafeRegExp(JapaneseNumericWithUnit.CompoundUnitConnectorRegex);
        this.nonUnitRegex = RegExpUtility.getSafeRegExp(BaseUnits.PmNonUnitRegex);
    }
}

export class JapaneseNumberWithUnitParserConfiguration extends BaseNumberWithUnitParserConfiguration {
    readonly internalNumberParser: IParser;
    readonly internalNumberExtractor: IExtractor;
    readonly connectorToken: string;
    readonly currencyNameToIsoCodeMap: ReadonlyMap<string, string>;
    readonly currencyFractionCodeList: ReadonlyMap<string, string>;

    constructor(ci: CultureInfo) {
        super(ci);

        this.internalNumberExtractor = new JapaneseNumberExtractor(JapaneseNumberExtractorMode.Default);
        this.internalNumberParser = AgnosticNumberParserFactory.getParser(AgnosticNumberParserType.Number, new JapaneseNumberParserConfiguration());
        this.connectorToken = '';
        this.currencyNameToIsoCodeMap = JapaneseNumericWithUnit.CurrencyNameToIsoCodeMap;
        this.currencyFractionCodeList = JapaneseNumericWithUnit.FractionalUnitNameToCodeMap;
    }
}