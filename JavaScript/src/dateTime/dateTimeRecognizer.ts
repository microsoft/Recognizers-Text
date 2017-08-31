import { IModel } from "../models";
import { IDateTimeModel, DateTimeModel } from "./models";
import { Recognizer } from "../recognizer";
import { Culture } from "../culture";
import { BaseMergedParser } from "./parsers";
import { BaseMergedExtractor } from "./extractors";
import { EnglishCommonDateTimeParserConfiguration, EnglishMergedParserConfiguration } from "./english/parserConfiguration";
import { EnglishMergedExtractorConfiguration } from "./english/extractorConfiguration";

export default class DateTimeRecognizer extends Recognizer {
    static readonly instance: DateTimeRecognizer = new DateTimeRecognizer();

    private constructor() {
        super();

        // English models
        this.registerModel("DateTimeModel", Culture.English, new DateTimeModel(
            new BaseMergedParser(new EnglishMergedParserConfiguration(new EnglishCommonDateTimeParserConfiguration())),
            new BaseMergedExtractor(new EnglishMergedExtractorConfiguration())
        ));
    }

    getDateTimeModel(culture: string, fallbackToDefaultCulture: boolean = true) : IDateTimeModel {
        return this.getModel("DateTimeModel", culture, fallbackToDefaultCulture);
    }
}