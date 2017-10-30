import { IModel, Recognizer, Culture } from "recognizers-text-number";
import { IDateTimeModel, DateTimeModel } from "./models";
import { BaseMergedParser, BaseMergedExtractor } from "./baseMerged";
import { EnglishCommonDateTimeParserConfiguration } from "./english/baseConfiguration";
import { EnglishMergedExtractorConfiguration, EnglishMergedParserConfiguration } from "./english/mergedConfiguration";
import { SpanishMergedParserConfiguration, SpanishMergedExtractorConfiguration } from "./spanish/mergedConfiguration";
import { FrenchMergedParserConfiguration, FrenchMergedExtractorConfiguration } from "./french/mergedConfiguration";
import { ChineseMergedExtractor, ChineseMergedParser, ChineseFullMergedParser } from "./chinese/mergedConfiguration";

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

        // Spanish models
        this.registerModel("DateTimeModel", Culture.Spanish, new DateTimeModel(
            new BaseMergedParser(new SpanishMergedParserConfiguration(), options),
            new BaseMergedExtractor(new SpanishMergedExtractorConfiguration(), options)
        ));

        // Chinese models
        this.registerModel("DateTimeModel", Culture.Chinese, new DateTimeModel(
            new ChineseFullMergedParser(),
            new ChineseMergedExtractor(options)
        ));

        // French models
        this.registerModel("DateTimeModel", Culture.French, new DateTimeModel(
            new BaseMergedParser(new FrenchMergedParserConfiguration(), options),
            new BaseMergedExtractor(new FrenchMergedExtractorConfiguration(), options)
        ));
    }

    getDateTimeModel(culture: string = "", fallbackToDefaultCulture: boolean = true): IDateTimeModel {
        return this.getModel("DateTimeModel", culture, fallbackToDefaultCulture);
    }

    public static getSingleCultureInstance(cultureCode: string, options: DateTimeOptions = DateTimeOptions.None): DateTimeRecognizer {
        return new DateTimeRecognizer(options);
    }
}