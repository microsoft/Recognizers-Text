import { IModel, Recognizer, Culture, IExtractor, IParser } from "recognizers-text-number";
import { CurrencyModel, TemperatureModel, DimensionModel, AgeModel } from "./models";
import { NumberWithUnitExtractor } from "./extractors";
import { NumberWithUnitParser } from "./parsers";
import { EnglishCurrencyExtractorConfiguration, EnglishCurrencyParserConfiguration } from "./english/currency";
import { EnglishTemperatureExtractorConfiguration, EnglishTemperatureParserConfiguration } from "./english/temperature";
import { EnglishDimensionExtractorConfiguration, EnglishDimensionParserConfiguration } from "./english/dimension";
import { EnglishAgeExtractorConfiguration, EnglishAgeParserConfiguration } from "./english/age";
import { SpanishCurrencyExtractorConfiguration, SpanishCurrencyParserConfiguration } from "./spanish/currency";
import { SpanishTemperatureExtractorConfiguration, SpanishTemperatureParserConfiguration } from "./spanish/temperature";
import { SpanishDimensionExtractorConfiguration, SpanishDimensionParserConfiguration } from "./spanish/dimension";
import { SpanishAgeExtractorConfiguration, SpanishAgeParserConfiguration } from "./spanish/age";

export default class NumberWithUnitRecognizer extends Recognizer {
    static readonly instance: NumberWithUnitRecognizer = new NumberWithUnitRecognizer();

    private constructor() {
        super();

        // English models
        this.registerModel("CurrencyModel", Culture.English, new CurrencyModel(new Map<IExtractor, IParser>([
            [new NumberWithUnitExtractor(new EnglishCurrencyExtractorConfiguration()), new NumberWithUnitParser(new EnglishCurrencyParserConfiguration())]
        ])));
        this.registerModel("TemperatureModel", Culture.English, new TemperatureModel(new Map<IExtractor, IParser>([
            [new NumberWithUnitExtractor(new EnglishTemperatureExtractorConfiguration()), new NumberWithUnitParser(new EnglishTemperatureParserConfiguration())]
        ])));
        this.registerModel("DimensionModel", Culture.English, new DimensionModel(new Map<IExtractor, IParser>([
            [new NumberWithUnitExtractor(new EnglishDimensionExtractorConfiguration()), new NumberWithUnitParser(new EnglishDimensionParserConfiguration())]
        ])));
        this.registerModel("AgeModel", Culture.English, new AgeModel(new Map<IExtractor, IParser>([
            [new NumberWithUnitExtractor(new EnglishAgeExtractorConfiguration()), new NumberWithUnitParser(new EnglishAgeParserConfiguration())]
        ])));

        // TODO: Register Spanish models
        this.registerModel("CurrencyModel", Culture.Spanish, new CurrencyModel(new Map<IExtractor, IParser>([
            [new NumberWithUnitExtractor(new SpanishCurrencyExtractorConfiguration()), new NumberWithUnitParser(new SpanishCurrencyParserConfiguration())]
        ])));
        this.registerModel("TemperatureModel", Culture.Spanish, new TemperatureModel(new Map<IExtractor, IParser>([
            [new NumberWithUnitExtractor(new SpanishTemperatureExtractorConfiguration()), new NumberWithUnitParser(new SpanishTemperatureParserConfiguration())]
        ])));
        this.registerModel("DimensionModel", Culture.Spanish, new DimensionModel(new Map<IExtractor, IParser>([
            [new NumberWithUnitExtractor(new SpanishDimensionExtractorConfiguration()), new NumberWithUnitParser(new SpanishDimensionParserConfiguration())]
        ])));
        this.registerModel("AgeModel", Culture.Spanish, new AgeModel(new Map<IExtractor, IParser>([
            [new NumberWithUnitExtractor(new SpanishAgeExtractorConfiguration()), new NumberWithUnitParser(new SpanishAgeParserConfiguration())]
        ])));

        // TODO: register Chinese models
        // TODO: Register Portuguese models
        // TODO: Register French models
    }

    getCurrencyModel(culture: string, fallbackToDefaultCulture: boolean = true): IModel {
        return this.getModel("CurrencyModel", culture, fallbackToDefaultCulture);
    }

    getTemperatureModel(culture: string, fallbackToDefaultCulture: boolean = true): IModel {
        return this.getModel("TemperatureModel", culture, fallbackToDefaultCulture);
    }

    getDimensionModel(culture: string, fallbackToDefaultCulture: boolean = true): IModel {
        return this.getModel("DimensionModel", culture, fallbackToDefaultCulture);
    }

    getAgeModel(culture: string, fallbackToDefaultCulture: boolean = true): IModel {
        return this.getModel("AgeModel", culture, fallbackToDefaultCulture);
    }
}