import { Recognizer, IModel, Culture } from "recognizers-text-base";
import { BooleanModel } from "./models";
import { BooleanExtractor } from "./extractors";
import { BooleanParser } from "./parsers";
import { EnglishBooleanExtractorConfiguration } from "./english/boolean";

export default class ChoicesRecognizer extends Recognizer {
    static readonly instance: ChoicesRecognizer = new ChoicesRecognizer();

    private constructor() {
        super();

        // English models
        this.registerModel("BooleanModel", Culture.English, new BooleanModel(
            new BooleanParser(),
            new BooleanExtractor(new EnglishBooleanExtractorConfiguration())
        ));
    }

    getBooleanModel(culture: string, fallbackToDefaultCulture: boolean = true): IModel {
        return this.getModel("BooleanModel", culture, fallbackToDefaultCulture);
    }
}