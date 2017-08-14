import { IModel } from "../models";
import { IDateTimeModel, DateTimeModel } from "./models";
import { Recognizer } from "../recognizer";
import { Culture } from "../culture";

export default class DateTimeRecognizer extends Recognizer {
    static readonly instance: DateTimeRecognizer = new DateTimeRecognizer();

    private constructor() {
        super();

        // English models
        // TODO replace for real extractors and parsers
        this.registerModel("DateTimeModel", Culture.English, new DateTimeModel(null, null));
    }

    getDateTimeModel(culture: string, fallbackToDefaultCulture: boolean = true) : IDateTimeModel {
        return this.getModel("DateTimeModel", culture, fallbackToDefaultCulture);
    }
}