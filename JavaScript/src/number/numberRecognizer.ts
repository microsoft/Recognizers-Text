import { IModel } from "../models";
import { Recognizer } from "../recognizer";
import { Culture } from "../culture";
import { NumberMode, NumberModel, OrdinalModel, PercentModel } from "./models";
import { AgnosticNumberParserType, AgnosticNumberParserFactory } from "./parsers";
import { EnglishNumberParserConfiguration } from "./english/parserConfiguration";
import { EnglishNumberExtractor, EnglishOrdinalExtractor, EnglishPercentageExtractor } from "./english/extractors";

export default class NumberRecognizer extends Recognizer {
    static readonly instance: NumberRecognizer = new NumberRecognizer();

    private constructor() {
        super();

        // English models
        this.registerModel("NumberModel", Culture.English, new NumberModel(
            AgnosticNumberParserFactory.getParser(AgnosticNumberParserType.Number, new EnglishNumberParserConfiguration()),
            new EnglishNumberExtractor(NumberMode.PureNumber)));
        this.registerModel("OrdinalModel", Culture.English, new OrdinalModel(
            AgnosticNumberParserFactory.getParser(AgnosticNumberParserType.Ordinal, new EnglishNumberParserConfiguration()),
            new EnglishOrdinalExtractor()));
        this.registerModel("PercentModel", Culture.English, new PercentModel(
            AgnosticNumberParserFactory.getParser(AgnosticNumberParserType.Percentage, new EnglishNumberParserConfiguration()),
            new EnglishPercentageExtractor()));

        // TODO: register Chinese models
        // TODO: Register Spanish models
        // TODO: Register Portuguese models
        // TODO: Register French models
    }

    getNumberModel(culture: string, fallbackToDefaultCulture: boolean = true): IModel {
        return this.getModel("NumberModel", culture, fallbackToDefaultCulture);
    }

    getOrdinalModel(culture: string, fallbackToDefaultCulture: boolean = true): IModel {
        return this.getModel("OrdinalModel", culture, fallbackToDefaultCulture);
    }

    getPercentageModel(culture: string, fallbackToDefaultCulture: boolean = true): IModel {
        return this.getModel("PercentModel", culture, fallbackToDefaultCulture);
    }
}