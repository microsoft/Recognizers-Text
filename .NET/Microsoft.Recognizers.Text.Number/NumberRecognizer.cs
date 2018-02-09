using System;
using System.Collections.Generic;
using Microsoft.Recognizers.Text.Number.Chinese;
using Microsoft.Recognizers.Text.Number.English;
using Microsoft.Recognizers.Text.Number.French;
using Microsoft.Recognizers.Text.Number.German;
using Microsoft.Recognizers.Text.Number.Portuguese;
using Microsoft.Recognizers.Text.Number.Spanish;

namespace Microsoft.Recognizers.Text.Number
{
    public class NumberRecognizer : Recognizer<NumberOptions>
    {
        public NumberRecognizer(string culture, NumberOptions options = NumberOptions.None)
            : base(culture, options)
        {
        }

        public NumberModel GetNumberModel()
        {
            return GetModel<NumberModel>();
        }

        public OrdinalModel GetOrdinalModel()
        {
            return GetModel<OrdinalModel>();
        }

        public PercentModel GetPercentageModel()
        {
            return GetModel<PercentModel>();
        }

        public NumberRangeModel GetNumberRangeModel()
        {
            return GetModel<NumberRangeModel>();
        }

        public static List<ModelResult> RecognizeNumber(string query, string culture, NumberOptions options = NumberOptions.None)
        {
            return RecognizeByModel(recognizer => recognizer.GetNumberModel(), query, culture, options);
        }

        public static List<ModelResult> RecognizeOrdinal(string query, string culture, NumberOptions options = NumberOptions.None)
        {
            return RecognizeByModel(recognizer => recognizer.GetOrdinalModel(), query, culture, options);
        }

        public static List<ModelResult> RecognizePercentage(string query, string culture, NumberOptions options = NumberOptions.None)
        {
            return RecognizeByModel(recognizer => recognizer.GetPercentageModel(), query, culture, options);
        }

        public static List<ModelResult> RecognizeNumberRange(string query, string culture, NumberOptions options = NumberOptions.None)
        {
            return RecognizeByModel(recognizer => recognizer.GetNumberRangeModel(), query, culture, options);
        }

        private static List<ModelResult> RecognizeByModel(Func<NumberRecognizer, IModel> getModelFunc,  string query, string culture, NumberOptions options)
        {
            var recognizer = new NumberRecognizer(culture, options);
            var model = getModelFunc(recognizer);
            return model.Parse(query);
        }

        protected override void InitializeConfiguration()
        {
            RegisterModel<NumberModel>(
                Culture.English,
                (options) => new NumberModel(
                    AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Number, new EnglishNumberParserConfiguration()),
                    English.NumberExtractor.GetInstance(NumberMode.PureNumber)));
            RegisterModel<OrdinalModel>(
                Culture.English,
                (options) => new OrdinalModel(
                    AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Ordinal, new EnglishNumberParserConfiguration()),
                    English.OrdinalExtractor.GetInstance()));
            RegisterModel<PercentModel>(
                Culture.English,
                (options) => new PercentModel(
                    AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Percentage, new EnglishNumberParserConfiguration()),
                    new English.PercentageExtractor()));
            RegisterModel<NumberRangeModel>(
                Culture.English,
                (options) => new NumberRangeModel(
                            new BaseNumberRangeParser(new EnglishNumberRangeParserConfiguration()),
                            new English.NumberRangeExtractor()));

            RegisterModel<NumberModel>(
                Culture.Chinese,
                (options) => new NumberModel(
                    AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Number, new ChineseNumberParserConfiguration()),
                    new Chinese.NumberExtractor()));
            RegisterModel<OrdinalModel>(
                Culture.Chinese,
                (options) => new OrdinalModel(
                    AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Ordinal, new ChineseNumberParserConfiguration()),
                    new Chinese.OrdinalExtractor()));
            RegisterModel<PercentModel>(
                Culture.Chinese,
                (options) => new PercentModel(
                    AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Percentage, new ChineseNumberParserConfiguration()),
                    new Chinese.PercentageExtractor()));
            RegisterModel<NumberRangeModel>(
                Culture.Chinese,
                (options) => new NumberRangeModel(
                            new BaseNumberRangeParser(new ChineseNumberRangeParserConfiguration()),
                            new Chinese.NumberRangeExtractor()));

            RegisterModel<NumberModel>(
                Culture.Spanish,
                (options) => new NumberModel(
                    AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Number, new SpanishNumberParserConfiguration()),
                    new Spanish.NumberExtractor(NumberMode.PureNumber)));
            RegisterModel<OrdinalModel>(
                Culture.Spanish,
                (options) => new OrdinalModel(
                    AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Ordinal, new SpanishNumberParserConfiguration()),
                    new Spanish.OrdinalExtractor()));
            RegisterModel<PercentModel>(
                Culture.Spanish,
                (options) => new PercentModel(
                    AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Percentage, new SpanishNumberParserConfiguration()),
                    new Spanish.PercentageExtractor()));

            RegisterModel<NumberModel>(
                Culture.Portuguese,
                (options) => new NumberModel(
                    AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Number, new PortugueseNumberParserConfiguration()),
                    new Portuguese.NumberExtractor(NumberMode.PureNumber)));
            RegisterModel<OrdinalModel>(
                Culture.Portuguese,
                (options) => new OrdinalModel(
                    AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Ordinal, new PortugueseNumberParserConfiguration()),
                    new Portuguese.OrdinalExtractor()));
            RegisterModel<PercentModel>(
                Culture.Portuguese,
                (options) => new PercentModel(
                    AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Percentage, new PortugueseNumberParserConfiguration()),
                    new Portuguese.PercentageExtractor()));

            RegisterModel<NumberModel>(
                Culture.French,
                (options) => new NumberModel(
                    AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Number, new FrenchNumberParserConfiguration()),
                    new French.NumberExtractor(NumberMode.PureNumber)));
            RegisterModel<OrdinalModel>(
                Culture.French,
                (options) => new OrdinalModel(
                    AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Ordinal, new FrenchNumberParserConfiguration()),
                    new French.OrdinalExtractor()));
            RegisterModel<PercentModel>(
                Culture.French,
                (options) => new PercentModel(
                    AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Percentage, new FrenchNumberParserConfiguration()),
                    new French.PercentageExtractor()));

            RegisterModel<NumberModel>(
                Culture.German,
                (options) => new NumberModel(
                    AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Number, new GermanNumberParserConfiguration()),
                    German.NumberExtractor.GetInstance(NumberMode.PureNumber)));
            RegisterModel<OrdinalModel>(
                Culture.German,
                (options) => new OrdinalModel(
                    AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Ordinal, new GermanNumberParserConfiguration()),
                    German.OrdinalExtractor.GetInstance()));
            RegisterModel<PercentModel>(
                Culture.German,
                (options) => new PercentModel(
                    AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Percentage, new GermanNumberParserConfiguration()),
                    new German.PercentageExtractor()));
        }
    }
}
