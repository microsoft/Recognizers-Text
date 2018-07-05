import { IModel, ModelResult, Recognizer, IExtractor, IParser } from "@microsoft/recognizers-text";
import { Culture } from "@microsoft/recognizers-text-number";
import { CurrencyModel, TemperatureModel, DimensionModel, AgeModel } from "./models";
import { NumberWithUnitExtractor, BaseMergedUnitExtractor } from "./extractors";
import { NumberWithUnitParser, BaseMergedUnitParser } from "./parsers";
import { EnglishCurrencyExtractorConfiguration, EnglishCurrencyParserConfiguration } from "./english/currency";
import { EnglishTemperatureExtractorConfiguration, EnglishTemperatureParserConfiguration } from "./english/temperature";
import { EnglishDimensionExtractorConfiguration, EnglishDimensionParserConfiguration } from "./english/dimension";
import { EnglishAgeExtractorConfiguration, EnglishAgeParserConfiguration } from "./english/age";
import { SpanishCurrencyExtractorConfiguration, SpanishCurrencyParserConfiguration } from "./spanish/currency";
import { SpanishTemperatureExtractorConfiguration, SpanishTemperatureParserConfiguration } from "./spanish/temperature";
import { SpanishDimensionExtractorConfiguration, SpanishDimensionParserConfiguration } from "./spanish/dimension";
import { SpanishAgeExtractorConfiguration, SpanishAgeParserConfiguration } from "./spanish/age";
import { PortugueseCurrencyExtractorConfiguration, PortugueseCurrencyParserConfiguration } from "./portuguese/currency";
import { PortugueseTemperatureExtractorConfiguration, PortugueseTemperatureParserConfiguration } from "./portuguese/temperature";
import { PortugueseDimensionExtractorConfiguration, PortugueseDimensionParserConfiguration } from "./portuguese/dimension";
import { PortugueseAgeExtractorConfiguration, PortugueseAgeParserConfiguration } from "./portuguese/age";
import { ChineseCurrencyExtractorConfiguration, ChineseCurrencyParserConfiguration } from "./chinese/currency";
import { ChineseTemperatureExtractorConfiguration, ChineseTemperatureParserConfiguration } from "./chinese/temperature";
import { ChineseDimensionExtractorConfiguration, ChineseDimensionParserConfiguration } from "./chinese/dimension";
import { ChineseAgeExtractorConfiguration, ChineseAgeParserConfiguration } from "./chinese/age";
import { FrenchCurrencyExtractorConfiguration, FrenchCurrencyParserConfiguration } from "./french/currency";
import { FrenchTemperatureExtractorConfiguration, FrenchTemperatureParserConfiguration } from "./french/temperature";
import { FrenchDimensionExtractorConfiguration, FrenchDimensionParserConfiguration } from "./french/dimension";
import { FrenchAgeExtractorConfiguration, FrenchAgeParserConfiguration } from "./french/age";

export enum NumberWithUnitOptions {
    None = 0,
}

export function recognizeCurrency(query: string, culture: string, options: NumberWithUnitOptions = NumberWithUnitOptions.None, fallbackToDefaultCulture: boolean = true): Array<ModelResult> {
    return recognizeByModel(recognizer => recognizer.getCurrencyModel(culture, fallbackToDefaultCulture), query, culture, options);
}

export function recognizeTemperature(query: string, culture: string, options: NumberWithUnitOptions = NumberWithUnitOptions.None, fallbackToDefaultCulture: boolean = true): Array<ModelResult> {
    return recognizeByModel(recognizer => recognizer.getTemperatureModel(culture, fallbackToDefaultCulture), query, culture, options);
}

export function recognizeDimension(query: string, culture: string, options: NumberWithUnitOptions = NumberWithUnitOptions.None, fallbackToDefaultCulture: boolean = true): Array<ModelResult> {
    return recognizeByModel(recognizer => recognizer.getDimensionModel(culture, fallbackToDefaultCulture), query, culture, options);
}

export function recognizeAge(query: string, culture: string, options: NumberWithUnitOptions = NumberWithUnitOptions.None, fallbackToDefaultCulture: boolean = true): Array<ModelResult> {
    return recognizeByModel(recognizer => recognizer.getAgeModel(culture, fallbackToDefaultCulture), query, culture, options);
}

function recognizeByModel(getModelFunc: (n: NumberWithUnitRecognizer) => IModel, query: string, culture: string, options: NumberWithUnitOptions): Array<ModelResult> {
    let recognizer = new NumberWithUnitRecognizer(culture, options);
    let model = getModelFunc(recognizer);
    return model.parse(query);
}

export default class NumberWithUnitRecognizer extends Recognizer<NumberWithUnitOptions> {
    constructor(culture: string, options: NumberWithUnitOptions = NumberWithUnitOptions.None, lazyInitialization: boolean = false) {
        super(culture, options, lazyInitialization);
    }

    protected InitializeConfiguration() {
        //#region English
        this.registerModel("CurrencyModel", Culture.English, (options) => new CurrencyModel(new Map<IExtractor, IParser>([
            [new BaseMergedUnitExtractor(new EnglishCurrencyExtractorConfiguration()), new BaseMergedUnitParser(new EnglishCurrencyParserConfiguration())]
        ])));
        this.registerModel("TemperatureModel", Culture.English, (options) => new TemperatureModel(new Map<IExtractor, IParser>([
            [new NumberWithUnitExtractor(new EnglishTemperatureExtractorConfiguration()), new NumberWithUnitParser(new EnglishTemperatureParserConfiguration())]
        ])));
        this.registerModel("DimensionModel", Culture.English,(options) =>  new DimensionModel(new Map<IExtractor, IParser>([
            [new NumberWithUnitExtractor(new EnglishDimensionExtractorConfiguration()), new NumberWithUnitParser(new EnglishDimensionParserConfiguration())]
        ])));
        this.registerModel("AgeModel", Culture.English, (options) => new AgeModel(new Map<IExtractor, IParser>([
            [new NumberWithUnitExtractor(new EnglishAgeExtractorConfiguration()), new NumberWithUnitParser(new EnglishAgeParserConfiguration())]
        ])));
        //#endregion

        //#region Spanish
        this.registerModel("CurrencyModel", Culture.Spanish, (options) => new CurrencyModel(new Map<IExtractor, IParser>([
            [new NumberWithUnitExtractor(new SpanishCurrencyExtractorConfiguration()), new NumberWithUnitParser(new SpanishCurrencyParserConfiguration())]
        ])));
        this.registerModel("TemperatureModel", Culture.Spanish, (options) => new TemperatureModel(new Map<IExtractor, IParser>([
            [new NumberWithUnitExtractor(new SpanishTemperatureExtractorConfiguration()), new NumberWithUnitParser(new SpanishTemperatureParserConfiguration())]
        ])));
        this.registerModel("DimensionModel", Culture.Spanish, (options) => new DimensionModel(new Map<IExtractor, IParser>([
            [new NumberWithUnitExtractor(new SpanishDimensionExtractorConfiguration()), new NumberWithUnitParser(new SpanishDimensionParserConfiguration())]
        ])));
        this.registerModel("AgeModel", Culture.Spanish, (options) => new AgeModel(new Map<IExtractor, IParser>([
            [new NumberWithUnitExtractor(new SpanishAgeExtractorConfiguration()), new NumberWithUnitParser(new SpanishAgeParserConfiguration())]
        ])));
        //#endregion

        //#region Portuguese
        this.registerModel("CurrencyModel", Culture.Portuguese, (options) => new CurrencyModel(new Map<IExtractor, IParser>([
            [new NumberWithUnitExtractor(new PortugueseCurrencyExtractorConfiguration()), new NumberWithUnitParser(new PortugueseCurrencyParserConfiguration())]
        ])));
        this.registerModel("TemperatureModel", Culture.Portuguese, (options) => new TemperatureModel(new Map<IExtractor, IParser>([
            [new NumberWithUnitExtractor(new PortugueseTemperatureExtractorConfiguration()), new NumberWithUnitParser(new PortugueseTemperatureParserConfiguration())]
        ])));
        this.registerModel("DimensionModel", Culture.Portuguese, (options) => new DimensionModel(new Map<IExtractor, IParser>([
            [new NumberWithUnitExtractor(new PortugueseDimensionExtractorConfiguration()), new NumberWithUnitParser(new PortugueseDimensionParserConfiguration())]
        ])));
        this.registerModel("AgeModel", Culture.Portuguese, (options) => new AgeModel(new Map<IExtractor, IParser>([
            [new NumberWithUnitExtractor(new PortugueseAgeExtractorConfiguration()), new NumberWithUnitParser(new PortugueseAgeParserConfiguration())]
        ])));
        //#endregion

        //#region Chinese
        this.registerModel("CurrencyModel", Culture.Chinese, (options) => new CurrencyModel(new Map<IExtractor, IParser>([
            [new BaseMergedUnitExtractor(new ChineseCurrencyExtractorConfiguration()), new BaseMergedUnitParser(new ChineseCurrencyParserConfiguration())],
            [new NumberWithUnitExtractor(new EnglishCurrencyExtractorConfiguration()), new NumberWithUnitParser(new EnglishCurrencyParserConfiguration())]
        ])));
        this.registerModel("TemperatureModel", Culture.Chinese, (options) => new TemperatureModel(new Map<IExtractor, IParser>([
            [new NumberWithUnitExtractor(new ChineseTemperatureExtractorConfiguration()), new NumberWithUnitParser(new ChineseTemperatureParserConfiguration())],
            [new NumberWithUnitExtractor(new EnglishTemperatureExtractorConfiguration()), new NumberWithUnitParser(new EnglishTemperatureParserConfiguration())]
        ])));
        this.registerModel("DimensionModel", Culture.Chinese, (options) => new DimensionModel(new Map<IExtractor, IParser>([
            [new NumberWithUnitExtractor(new ChineseDimensionExtractorConfiguration()), new NumberWithUnitParser(new ChineseDimensionParserConfiguration())],
            [new NumberWithUnitExtractor(new EnglishDimensionExtractorConfiguration()), new NumberWithUnitParser(new EnglishDimensionParserConfiguration())]
        ])));
        this.registerModel("AgeModel", Culture.Chinese, (options) => new AgeModel(new Map<IExtractor, IParser>([
            [new NumberWithUnitExtractor(new ChineseAgeExtractorConfiguration()), new NumberWithUnitParser(new ChineseAgeParserConfiguration())],
            [new NumberWithUnitExtractor(new EnglishAgeExtractorConfiguration()), new NumberWithUnitParser(new EnglishAgeParserConfiguration())]
        ])));
        //#endregion

        //#region French
        this.registerModel("CurrencyModel", Culture.French, (options) => new CurrencyModel(new Map<IExtractor, IParser>([
            [new NumberWithUnitExtractor(new FrenchCurrencyExtractorConfiguration()), new NumberWithUnitParser(new FrenchCurrencyParserConfiguration())]
        ])));
        this.registerModel("TemperatureModel", Culture.French, (options) => new TemperatureModel(new Map<IExtractor, IParser>([
            [new NumberWithUnitExtractor(new FrenchTemperatureExtractorConfiguration()), new NumberWithUnitParser(new FrenchTemperatureParserConfiguration())]
        ])));
        this.registerModel("DimensionModel", Culture.French, (options) => new DimensionModel(new Map<IExtractor, IParser>([
            [new NumberWithUnitExtractor(new FrenchDimensionExtractorConfiguration()), new NumberWithUnitParser(new FrenchDimensionParserConfiguration())]
        ])));
        this.registerModel("AgeModel", Culture.French, (options) => new AgeModel(new Map<IExtractor, IParser>([
            [new NumberWithUnitExtractor(new FrenchAgeExtractorConfiguration()), new NumberWithUnitParser(new FrenchAgeParserConfiguration())]
        ])));
        //#endregion
    }

    protected IsValidOptions(options: number): boolean {
        return options >= 0 && options <= NumberWithUnitOptions.None
    }

    getCurrencyModel(culture: string = null, fallbackToDefaultCulture: boolean = true): IModel {
        return this.getModel("CurrencyModel", culture, fallbackToDefaultCulture);
    }

    getTemperatureModel(culture: string = null, fallbackToDefaultCulture: boolean = true): IModel {
        return this.getModel("TemperatureModel", culture, fallbackToDefaultCulture);
    }

    getDimensionModel(culture: string = null, fallbackToDefaultCulture: boolean = true): IModel {
        return this.getModel("DimensionModel", culture, fallbackToDefaultCulture);
    }

    getAgeModel(culture: string = null, fallbackToDefaultCulture: boolean = true): IModel {
        return this.getModel("AgeModel", culture, fallbackToDefaultCulture);
    }
}