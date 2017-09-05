import { IModel } from "../models";
import { Recognizer } from "../recognizer";
import { Culture } from "../culture";
import { CurrencyModel, TemperatureModel, DimensionModel, AgeModel } from "./models";
import { IExtractor } from "../number/extractors";
import { IParser } from "../number/parsers";
import { NumberWithUnitExtractor } from "./extractors";
import { NumberWithUnitParser } from "./parsers";
import { EnglishCurrencyExtractorConfiguration, EnglishCurrencyParserConfiguration } from "./english/currency";
import { EnglishTemperatureExtractorConfiguration, EnglishTemperatureParserConfiguration } from "./english/temperature";
import { EnglishDimensionExtractorConfiguration, EnglishDimensionParserConfiguration } from "./english/dimension";
import { EnglishAgeExtractorConfiguration, EnglishAgeParserConfiguration } from "./english/age";

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

        // TODO: register Chinese models
        // TODO: Register Spanish models
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