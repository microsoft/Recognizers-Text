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

export function recognizeDateTime(query: string, culture: string, options: DateTimeOptions = DateTimeOptions.None,
        referenceDate: Date = new Date(), fallbackToDefaultCulture: boolean = true): Array<ModelResult> {
    let recognizer = new DateTimeRecognizer(culture, options);
    let model = recognizer.getDateTimeModel(culture, fallbackToDefaultCulture);
    return model.parse(query, referenceDate);
}

export default class DateTimeRecognizer extends Recognizer<DateTimeOptions> {
    constructor(culture: string, options: DateTimeOptions = DateTimeOptions.None, lazyInitialization: boolean = false) {
        super(culture, options, lazyInitialization);
    }

    protected InitializeConfiguration() {
        //#region English
        this.registerModel("DateTimeModel", Culture.English, (options) => new DateTimeModel(
            new BaseMergedParser(new EnglishMergedParserConfiguration(new EnglishCommonDateTimeParserConfiguration()), this.Options),
            new BaseMergedExtractor(new EnglishMergedExtractorConfiguration(), this.Options)
        ));
        //#endregion

        //#region EnglishOhters
        this.registerModel("DateTimeModel", Culture.EnglishOthers, (options) => new DateTimeModel(
            new BaseMergedParser(new EnglishMergedParserConfiguration(new EnglishCommonDateTimeParserConfiguration(true)), this.Options),
            new BaseMergedExtractor(new EnglishMergedExtractorConfiguration(true), this.Options)
        ));
        //#endregion

        //#region Spanish
        this.registerModel("DateTimeModel", Culture.Spanish, (options) => new DateTimeModel(
            new BaseMergedParser(new SpanishMergedParserConfiguration(), this.Options),
            new BaseMergedExtractor(new SpanishMergedExtractorConfiguration(), this.Options)
        ));
        //#endregion

        //#region Chinese
        this.registerModel("DateTimeModel", Culture.Chinese, (options) => new DateTimeModel(
            new ChineseFullMergedParser(),
            new ChineseMergedExtractor(this.Options)
        ));
        //#endregion

        //#region French
        this.registerModel("DateTimeModel", Culture.French, (options) => new DateTimeModel(
            new BaseMergedParser(new FrenchMergedParserConfiguration(), this.Options),
            new BaseMergedExtractor(new FrenchMergedExtractorConfiguration(), this.Options)
        ));
        //#endregion
    }

    protected IsValidOptions(options: number): boolean {
        return options >= 0 && options <= DateTimeOptions.None + DateTimeOptions.SkipFromToMerge + DateTimeOptions.SplitDateAndTime + DateTimeOptions.Calendar;
    }

    getDateTimeModel(culture: string = null, fallbackToDefaultCulture: boolean = true): IDateTimeModel {
        return this.getModel("DateTimeModel", culture, fallbackToDefaultCulture);
    }
}