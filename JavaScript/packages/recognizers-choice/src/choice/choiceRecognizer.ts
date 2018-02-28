import { Recognizer, IModel, Culture, ModelResult } from "@microsoft/recognizers-text";
import { BooleanModel } from "./models";
import { BooleanExtractor } from "./extractors";
import { BooleanParser } from "./parsers";
import { EnglishBooleanExtractorConfiguration } from "./english/boolean";

export enum ChoiceOptions {
    None = 0,
}

export function recognizeBoolean(query: string, culture: string, options: ChoiceOptions = ChoiceOptions.None,
        fallbackToDefaultCulture: boolean = true): Array<ModelResult> {
    let recognizer = new OptionsRecognizer(culture, options);
    let model = recognizer.getBooleanModel(culture, fallbackToDefaultCulture);
    return model.parse(query);
}

export default class OptionsRecognizer extends Recognizer<ChoiceOptions> {
    constructor(culture: string, options: ChoiceOptions = ChoiceOptions.None, lazyInitialization: boolean = false) {
        super(culture, options, lazyInitialization);
    }

    protected InitializeConfiguration() {
        //#region English
        this.registerModel("BooleanModel", Culture.English, (options) => new BooleanModel(
            new BooleanParser(),
            new BooleanExtractor(new EnglishBooleanExtractorConfiguration())
        ));
        //#endregion
    }

    protected IsValidOptions(options: number): boolean {
        return options >= 0 && options <= ChoiceOptions.None
    }

    getBooleanModel(culture: string = null, fallbackToDefaultCulture: boolean = true): IModel {
        return this.getModel("BooleanModel", culture, fallbackToDefaultCulture);
    }
}