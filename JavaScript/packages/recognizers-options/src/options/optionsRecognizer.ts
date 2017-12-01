import { Recognizer, IModel, Culture } from "recognizers-text";
import { BooleanModel } from "./models";
import { BooleanExtractor } from "./extractors";
import { BooleanParser } from "./parsers";
import { EnglishBooleanExtractorConfiguration } from "./english/boolean";

export default class OptionsRecognizer extends Recognizer {
    static readonly instance: OptionsRecognizer = new OptionsRecognizer();

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