import { Recognizer, IModel, Culture, ModelResult } from "@microsoft/recognizers-text";
import { BooleanModel } from "./models";
import { BooleanExtractor } from "./extractors";
import { BooleanParser } from "./parsers";
import { EnglishBooleanExtractorConfiguration } from "./english/boolean";

export enum ChoiceOptions {
    None = 0,
}

export function recognizeBoolean(query: string, culture: string, options: ChoiceOptions = ChoiceOptions.None): Array<ModelResult> {
    return recognizeByModel(recognizer => recognizer.getBooleanModel(), query, culture, options);
}

function recognizeByModel(getModelFunc: (n: OptionsRecognizer) => IModel, query: string, culture: string, options: ChoiceOptions): Array<ModelResult> {
    let recognizer = new OptionsRecognizer(culture, options);
    let model = getModelFunc(recognizer);
    return model.parse(query);
}

export default class OptionsRecognizer extends Recognizer<ChoiceOptions> {
    constructor(culture: string, options: ChoiceOptions = ChoiceOptions.None) {
        super(culture, options);
    }

    protected InitializeConfiguration() {
        //#region English
        this.registerModel("BooleanModel", Culture.English, (options) => new BooleanModel(
            new BooleanParser(),
            new BooleanExtractor(new EnglishBooleanExtractorConfiguration())
        ));
        //#endregion
    }

    getBooleanModel(): IModel {
        return this.getModel("BooleanModel");
    }
}