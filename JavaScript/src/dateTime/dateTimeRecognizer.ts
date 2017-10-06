import { IModel } from "../models";
import { IDateTimeModel, DateTimeModel } from "./models";
import { Recognizer } from "../recognizer";
import { Culture } from "../culture";
import { BaseMergedParser, BaseMergedExtractor } from "./baseMerged";
import { EnglishCommonDateTimeParserConfiguration } from "./english/baseConfiguration";
import { EnglishMergedExtractorConfiguration, EnglishMergedParserConfiguration } from "./english/mergedConfiguration";

export enum DateTimeOptions {
    None = 0, SkipFromToMerge = 1, SplitDateAndTime = 2
}

export default class DateTimeRecognizer extends Recognizer {
    static readonly instance: DateTimeRecognizer = new DateTimeRecognizer(DateTimeOptions.None);

    private constructor(options: DateTimeOptions) {
        super();

        // English models
        this.registerModel("DateTimeModel", Culture.English, new DateTimeModel(
            new BaseMergedParser(new EnglishMergedParserConfiguration(new EnglishCommonDateTimeParserConfiguration()), options),
            new BaseMergedExtractor(new EnglishMergedExtractorConfiguration(), options)
        ));
    }

    getDateTimeModel(culture: string = "", fallbackToDefaultCulture: boolean = true): IDateTimeModel {
        return this.getModel("DateTimeModel", culture, fallbackToDefaultCulture);
    }

    public static getSingleCultureInstance(cultureCode: string, options: DateTimeOptions = DateTimeOptions.None): DateTimeRecognizer {
        return new DateTimeRecognizer(options);
    }
}