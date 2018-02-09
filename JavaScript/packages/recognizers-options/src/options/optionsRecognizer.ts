import { Recognizer, IModel, Culture } from "@microsoft/recognizers-text";
import { BooleanModel } from "./models";
import { BooleanExtractor } from "./extractors";
import { BooleanParser } from "./parsers";
import { EnglishBooleanExtractorConfiguration } from "./english/boolean";

export enum OptionsOptions {
    None = 0,
}

export default class OptionsRecognizer extends Recognizer<OptionsOptions> {
    private constructor(culture: string, options: OptionsOptions = OptionsOptions.None) {
        super(culture, options);
    }

    protected InitializeConfiguration() {
        // English models
        this.registerModel("BooleanModel", Culture.English, (options) => new BooleanModel(
            new BooleanParser(),
            new BooleanExtractor(new EnglishBooleanExtractorConfiguration())
        ));
    }

    getBooleanModel(): IModel {
        return this.getModel("BooleanModel");
    }
}