import { IModel, ModelResult, Recognizer } from "@microsoft/recognizers-text";
import { Culture } from "@microsoft/recognizers-text-number";
import { IDateTimeModel, DateTimeModel } from "./models";
import { BaseMergedParser, BaseMergedExtractor } from "./baseMerged";
import { EnglishCommonDateTimeParserConfiguration } from "./english/baseConfiguration";
import { EnglishMergedExtractorConfiguration, EnglishMergedParserConfiguration } from "./english/mergedConfiguration";
import { SpanishMergedParserConfiguration, SpanishMergedExtractorConfiguration } from "./spanish/mergedConfiguration";
import { FrenchMergedParserConfiguration, FrenchMergedExtractorConfiguration } from "./french/mergedConfiguration";
import { ChineseMergedExtractor, ChineseMergedParser, ChineseFullMergedParser } from "./chinese/mergedConfiguration";

export enum DateTimeOptions {
    None = 0, SkipFromToMerge = 1, SplitDateAndTime = 2, Calendar = 4 
}

export function recognizeDateTime(query: string, culture: string, options: DateTimeOptions = DateTimeOptions.None, referenceDate: Date = new Date()): Array<ModelResult> {
    return recognizeByModel(recognizer => recognizer.getDateTimeModel(), query, culture, options, referenceDate);
}

function recognizeByModel(getModelFunc: (n: DateTimeRecognizer) => IDateTimeModel, query: string, culture: string, options: DateTimeOptions = DateTimeOptions.None, referenceDate: Date = new Date()): Array<ModelResult> {
    let recognizer = new DateTimeRecognizer(culture, options);
    let model = getModelFunc(recognizer);
    return model.parse(query, referenceDate);
}

export default class DateTimeRecognizer extends Recognizer<DateTimeOptions> {
    constructor(culture: string, options: DateTimeOptions = DateTimeOptions.None) {
        super(culture, options);
    }

    protected InitializeConfiguration() {
        // English models
        this.registerModel("DateTimeModel", Culture.English, (options) => new DateTimeModel(
            new BaseMergedParser(new EnglishMergedParserConfiguration(new EnglishCommonDateTimeParserConfiguration()), this.RecognizerOptions),
            new BaseMergedExtractor(new EnglishMergedExtractorConfiguration(), this.RecognizerOptions)
        ));

        // Spanish models
        this.registerModel("DateTimeModel", Culture.Spanish, (options) => new DateTimeModel(
            new BaseMergedParser(new SpanishMergedParserConfiguration(), this.RecognizerOptions),
            new BaseMergedExtractor(new SpanishMergedExtractorConfiguration(), this.RecognizerOptions)
        ));

        // Chinese models
        this.registerModel("DateTimeModel", Culture.Chinese, (options) => new DateTimeModel(
            new ChineseFullMergedParser(),
            new ChineseMergedExtractor(this.RecognizerOptions)
        ));

        // French models
        this.registerModel("DateTimeModel", Culture.French, (options) => new DateTimeModel(
            new BaseMergedParser(new FrenchMergedParserConfiguration(), this.RecognizerOptions),
            new BaseMergedExtractor(new FrenchMergedExtractorConfiguration(), this.RecognizerOptions)
        ));
    }

    getDateTimeModel(): IDateTimeModel {
        return this.getModel("DateTimeModel");
    }
}