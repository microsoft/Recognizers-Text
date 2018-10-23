import { Recognizer, IModel, Culture, ModelResult } from "@microsoft/recognizers-text";
import { BooleanModel } from "./models";
import { BooleanExtractor } from "./extractors";
import { BooleanParser } from "./parsers";
import { ChineseBooleanExtractorConfiguration } from "./chinese/boolean";
import { DutchBooleanExtractorConfiguration } from "./dutch/boolean";
import { EnglishBooleanExtractorConfiguration } from "./english/boolean";
import { FrenchBooleanExtractorConfiguration } from "./french/boolean";
import { GermanBooleanExtractorConfiguration } from "./german/boolean";
import { JapaneseBooleanExtractorConfiguration } from "./japanese/boolean";
import { PortugueseBooleanExtractorConfiguration } from "./portuguese/boolean";
import { SpanishBooleanExtractorConfiguration } from "./spanish/boolean";


export enum ChoiceOptions {
    None = 0,
}

export function recognizeBoolean(query: string, culture: string, options: ChoiceOptions = ChoiceOptions.None,
        fallbackToDefaultCulture: boolean = true): Array<ModelResult> {
    let recognizer = new ChoiceRecognizer(culture, options);
    let model = recognizer.getBooleanModel(culture, fallbackToDefaultCulture);
    return model.parse(query);
}

export default class ChoiceRecognizer extends Recognizer<ChoiceOptions> {
    constructor(culture: string, options: ChoiceOptions = ChoiceOptions.None, lazyInitialization: boolean = false) {
        super(culture, options, lazyInitialization);
    }

    protected InitializeConfiguration() {
        //#region Chinese
        this.registerModel("BooleanModel", Culture.Chinese, (options) => new BooleanModel(
            new BooleanParser(),
            new BooleanExtractor(new ChineseBooleanExtractorConfiguration())
        ));
        //#endregion

        //#region Dutch
        this.registerModel("BooleanModel", Culture.Dutch, (options) => new BooleanModel(
            new BooleanParser(),
            new BooleanExtractor(new DutchBooleanExtractorConfiguration())
        ));
        //#endregion

        //#region English
        this.registerModel("BooleanModel", Culture.English, (options) => new BooleanModel(
            new BooleanParser(),
            new BooleanExtractor(new EnglishBooleanExtractorConfiguration())
        ));
        //#endregion

        //#region French
        this.registerModel("BooleanModel", Culture.French, (options) => new BooleanModel(
            new BooleanParser(),
            new BooleanExtractor(new FrenchBooleanExtractorConfiguration())
        ));
        //#endregion

        //#region German
        this.registerModel("BooleanModel", Culture.German, (options) => new BooleanModel(
            new BooleanParser(),
            new BooleanExtractor(new GermanBooleanExtractorConfiguration())
        ));
        //#endregion

        //#region Japanese
        this.registerModel("BooleanModel", Culture.Japanese, (options) => new BooleanModel(
            new BooleanParser(),
            new BooleanExtractor(new JapaneseBooleanExtractorConfiguration())
        ));
        //#endregion

        //#region Portuguese
        this.registerModel("BooleanModel", Culture.Portuguese, (options) => new BooleanModel(
            new BooleanParser(),
            new BooleanExtractor(new PortugueseBooleanExtractorConfiguration())
        ));
        //#endregion

        //#region Spanish
        this.registerModel("BooleanModel", Culture.Spanish, (options) => new BooleanModel(
            new BooleanParser(),
            new BooleanExtractor(new SpanishBooleanExtractorConfiguration())
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