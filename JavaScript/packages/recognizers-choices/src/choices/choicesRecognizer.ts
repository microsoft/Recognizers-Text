import { Recognizer, IModel, Culture } from "recognizers-text-base";
import { BooleanModel } from "./models";
import { BooleanExtractor } from "./extractors";
import { BooleanParser } from "./parsers";
import { EnglishChoices } from "../resources/englishChoices";

export default class BooleanRecognizer extends Recognizer {
    static readonly instance: BooleanRecognizer = new BooleanRecognizer();

    private constructor() {
        super();

        // English models
        this.registerModel("BooleanModel", Culture.English, new BooleanModel(
            new BooleanParser(),
            new BooleanExtractor(null)
        ));
    }

    getBooleanModel(culture: string, fallbackToDefaultCulture: boolean = true): IModel {
        return this.getModel("BooleanModel", culture, fallbackToDefaultCulture);
    }
}