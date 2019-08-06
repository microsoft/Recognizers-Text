import { IModel, ModelResult } from "@microsoft/recognizers-text";
import { Recognizer } from "@microsoft/recognizers-text";
import { Culture } from "../culture";
import { NumberMode, NumberModel, OrdinalModel, PercentModel } from "./models";
import { AgnosticNumberParserType, AgnosticNumberParserFactory } from "./agnosticNumberParser";
import { EnglishNumberParserConfiguration } from "./english/parserConfiguration";
import { SpanishNumberParserConfiguration } from "./spanish/parserConfiguration";
import { PortugueseNumberParserConfiguration } from "./portuguese/parserConfiguration";
import { FrenchNumberParserConfiguration } from "./french/parserConfiguration";
import { ChineseNumberParserConfiguration } from "./chinese/parserConfiguration";
import { JapaneseNumberParserConfiguration } from "./japanese/parserConfiguration";
import { EnglishNumberExtractor, EnglishOrdinalExtractor, EnglishPercentageExtractor } from "./english/extractors";
import { SpanishNumberExtractor, SpanishOrdinalExtractor, SpanishPercentageExtractor } from "./spanish/extractors";
import { PortugueseNumberExtractor, PortugueseOrdinalExtractor, PortuguesePercentageExtractor } from "./portuguese/extractors";
import { FrenchNumberExtractor, FrenchOrdinalExtractor, FrenchPercentageExtractor } from "./french/extractors";
import { ChineseNumberExtractor, ChineseOrdinalExtractor, ChinesePercentageExtractor } from "./chinese/extractors";
import { JapaneseNumberExtractor, JapaneseOrdinalExtractor, JapanesePercentageExtractor } from "./japanese/extractors";

export enum NumberOptions {
    None = 0,
}

export function recognizeNumber(query: string, culture: string, options: NumberOptions = NumberOptions.None, fallbackToDefaultCulture: boolean = true): ModelResult[] {
    return recognizeByModel(recognizer => recognizer.getNumberModel(culture, fallbackToDefaultCulture), query, culture, options);
}

export function recognizeOrdinal(query: string, culture: string, options: NumberOptions = NumberOptions.None, fallbackToDefaultCulture: boolean = true): ModelResult[] {
    return recognizeByModel(recognizer => recognizer.getOrdinalModel(culture, fallbackToDefaultCulture), query, culture, options);
}

export function recognizePercentage(query: string, culture: string, options: NumberOptions = NumberOptions.None, fallbackToDefaultCulture: boolean = true): ModelResult[] {
    return recognizeByModel(recognizer => recognizer.getPercentageModel(culture, fallbackToDefaultCulture), query, culture, options);
}

function recognizeByModel(getModelFunc: (n: NumberRecognizer) => IModel, query: string, culture: string, options: NumberOptions): ModelResult[] {
    let recognizer = new NumberRecognizer(culture, options);
    let model = getModelFunc(recognizer);
    return model.parse(query);
}

export default class NumberRecognizer extends Recognizer<NumberOptions> {
    constructor(culture, options: NumberOptions = NumberOptions.None, lazyInitialization: boolean = false) {
        super(culture, options, lazyInitialization);
    }

    protected InitializeConfiguration() {
        // #region English
        this.registerModel("NumberModel", Culture.English, (options) => new NumberModel(
            AgnosticNumberParserFactory.getParser(AgnosticNumberParserType.Number, new EnglishNumberParserConfiguration()),
            new EnglishNumberExtractor(NumberMode.PureNumber)));
        this.registerModel("OrdinalModel", Culture.English, (options) => new OrdinalModel(
            AgnosticNumberParserFactory.getParser(AgnosticNumberParserType.Ordinal, new EnglishNumberParserConfiguration()),
            new EnglishOrdinalExtractor()));
        this.registerModel("PercentModel", Culture.English, (options) => new PercentModel(
            AgnosticNumberParserFactory.getParser(AgnosticNumberParserType.Percentage, new EnglishNumberParserConfiguration()),
            new EnglishPercentageExtractor()));
        // #endregion

        // #region Spanish
        this.registerModel("NumberModel", Culture.Spanish, (options) => new NumberModel(
            AgnosticNumberParserFactory.getParser(AgnosticNumberParserType.Number, new SpanishNumberParserConfiguration()),
            new SpanishNumberExtractor(NumberMode.PureNumber)));
        this.registerModel("OrdinalModel", Culture.Spanish, (options) => new OrdinalModel(
            AgnosticNumberParserFactory.getParser(AgnosticNumberParserType.Ordinal, new SpanishNumberParserConfiguration()),
            new SpanishOrdinalExtractor()));
        this.registerModel("PercentModel", Culture.Spanish, (options) => new PercentModel(
            AgnosticNumberParserFactory.getParser(AgnosticNumberParserType.Percentage, new SpanishNumberParserConfiguration()),
            new SpanishPercentageExtractor()));
        // #endregion

        // #region Portuguese
        this.registerModel("NumberModel", Culture.Portuguese, (options) => new NumberModel(
            AgnosticNumberParserFactory.getParser(AgnosticNumberParserType.Number, new PortugueseNumberParserConfiguration()),
            new PortugueseNumberExtractor(NumberMode.PureNumber)));
        this.registerModel("OrdinalModel", Culture.Portuguese, (options) => new OrdinalModel(
            AgnosticNumberParserFactory.getParser(AgnosticNumberParserType.Ordinal, new PortugueseNumberParserConfiguration()),
            new PortugueseOrdinalExtractor()));
        this.registerModel("PercentModel", Culture.Portuguese, (options) => new PercentModel(
            AgnosticNumberParserFactory.getParser(AgnosticNumberParserType.Percentage, new PortugueseNumberParserConfiguration()),
            new PortuguesePercentageExtractor()));
        // #endregion

        // #region Chinese
        this.registerModel("NumberModel", Culture.Chinese, (options) => new NumberModel(
            AgnosticNumberParserFactory.getParser(AgnosticNumberParserType.Number, new ChineseNumberParserConfiguration()),
            new ChineseNumberExtractor()));
        this.registerModel("OrdinalModel", Culture.Chinese, (options) => new OrdinalModel(
            AgnosticNumberParserFactory.getParser(AgnosticNumberParserType.Ordinal, new ChineseNumberParserConfiguration()),
            new ChineseOrdinalExtractor()));
        this.registerModel("PercentModel", Culture.Chinese, (options) => new PercentModel(
            AgnosticNumberParserFactory.getParser(AgnosticNumberParserType.Percentage, new ChineseNumberParserConfiguration()),
            new ChinesePercentageExtractor()));
        // #endregion

        // #region Japanese
        this.registerModel("NumberModel", Culture.Japanese, (options) => new NumberModel(
            AgnosticNumberParserFactory.getParser(AgnosticNumberParserType.Number, new JapaneseNumberParserConfiguration()),
            new JapaneseNumberExtractor()));
        this.registerModel("OrdinalModel", Culture.Japanese, (options) => new OrdinalModel(
            AgnosticNumberParserFactory.getParser(AgnosticNumberParserType.Ordinal, new JapaneseNumberParserConfiguration()),
            new JapaneseOrdinalExtractor()));
        this.registerModel("PercentModel", Culture.Japanese, (options) => new PercentModel(
            AgnosticNumberParserFactory.getParser(AgnosticNumberParserType.Percentage, new JapaneseNumberParserConfiguration()),
            new JapanesePercentageExtractor()));
        // #endregion

        // #region French
        this.registerModel("NumberModel", Culture.French, (options) => new NumberModel(
            AgnosticNumberParserFactory.getParser(AgnosticNumberParserType.Number, new FrenchNumberParserConfiguration()),
            new FrenchNumberExtractor(NumberMode.PureNumber)));
        this.registerModel("OrdinalModel", Culture.French, (options) => new OrdinalModel(
            AgnosticNumberParserFactory.getParser(AgnosticNumberParserType.Ordinal, new FrenchNumberParserConfiguration()),
            new FrenchOrdinalExtractor()));
        this.registerModel("PercentModel", Culture.French, (options) => new PercentModel(
            AgnosticNumberParserFactory.getParser(AgnosticNumberParserType.Percentage, new FrenchNumberParserConfiguration()),
            new FrenchPercentageExtractor()));
        // #endregion
    }

    protected IsValidOptions(options: number): boolean {
        return options >= 0 && options <= NumberOptions.None;
    }

    getNumberModel(culture: string = null, fallbackToDefaultCulture: boolean = true): IModel {
        return this.getModel("NumberModel", culture, fallbackToDefaultCulture);
    }

    getOrdinalModel(culture: string = null, fallbackToDefaultCulture: boolean = true): IModel {
        return this.getModel("OrdinalModel", culture, fallbackToDefaultCulture);
    }

    getPercentageModel(culture: string = null, fallbackToDefaultCulture: boolean = true): IModel {
        return this.getModel("PercentModel", culture, fallbackToDefaultCulture);
    }
}