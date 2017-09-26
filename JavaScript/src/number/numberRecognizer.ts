import { IModel } from "../models";
import { Recognizer } from "../recognizer";
import { Culture } from "../culture";
import { NumberMode, NumberModel, OrdinalModel, PercentModel } from "./models";
import { AgnosticNumberParserType, AgnosticNumberParserFactory } from "./parsers";
import { EnglishNumberParserConfiguration } from "./english/parserConfiguration";
import { SpanishNumberParserConfiguration } from "./spanish/parserConfiguration";
import { EnglishNumberExtractor, EnglishOrdinalExtractor, EnglishPercentageExtractor } from "./english/extractors";
import { SpanishNumberExtractor, SpanishOrdinalExtractor, SpanishPercentageExtractor } from "./spanish/extractors";

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

        // Spanish models
        this.registerModel("NumberModel", Culture.Spanish, new NumberModel(
            AgnosticNumberParserFactory.getParser(AgnosticNumberParserType.Number, new SpanishNumberParserConfiguration()),
            new SpanishNumberExtractor(NumberMode.PureNumber)));
        this.registerModel("OrdinalModel", Culture.Spanish, new OrdinalModel(
            AgnosticNumberParserFactory.getParser(AgnosticNumberParserType.Ordinal, new SpanishNumberParserConfiguration()),
            new SpanishOrdinalExtractor()));
        this.registerModel("PercentModel", Culture.Spanish, new PercentModel(
            AgnosticNumberParserFactory.getParser(AgnosticNumberParserType.Percentage, new SpanishNumberParserConfiguration()),
            new SpanishPercentageExtractor()));
                
        // TODO: register Chinese models        
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