using System;
using System.Collections.Generic;
using Microsoft.Recognizers.Text.Number.Chinese;
using Microsoft.Recognizers.Text.Number.Dutch;
using Microsoft.Recognizers.Text.Number.English;
using Microsoft.Recognizers.Text.Number.French;
using Microsoft.Recognizers.Text.Number.German;
using Microsoft.Recognizers.Text.Number.Japanese;
using Microsoft.Recognizers.Text.Number.Korean;
using Microsoft.Recognizers.Text.Number.Portuguese;
using Microsoft.Recognizers.Text.Number.Spanish;

namespace Microsoft.Recognizers.Text.Number
{
    public class NumberRecognizer : Recognizer<NumberOptions>
    {
        public NumberRecognizer(string targetCulture, NumberOptions options = NumberOptions.None, bool lazyInitialization = false)
            : base(targetCulture, options, lazyInitialization)
        {
        }

        public NumberRecognizer(string targetCulture, int options, bool lazyInitialization = false)
            : this(targetCulture, GetOptions(options), lazyInitialization)
        {
        }

        public NumberRecognizer(NumberOptions options = NumberOptions.None, bool lazyInitialization = true)
            : this(null, options, lazyInitialization)
        {
        }

        public NumberRecognizer(int options, bool lazyInitialization = true)
            : this(null, options, lazyInitialization)
        {
        }

        public static List<ModelResult> RecognizeNumber(string query, string culture, NumberOptions options = NumberOptions.None, bool fallbackToDefaultCulture = true)
        {
            return RecognizeByModel(recognizer => recognizer.GetNumberModel(culture, fallbackToDefaultCulture), query, options);
        }

        public static List<ModelResult> RecognizeOrdinal(string query, string culture, NumberOptions options = NumberOptions.None, bool fallbackToDefaultCulture = true)
        {
            return RecognizeByModel(recognizer => recognizer.GetOrdinalModel(culture, fallbackToDefaultCulture), query, options);
        }

        public static List<ModelResult> RecognizePercentage(string query, string culture, NumberOptions options = NumberOptions.None, bool fallbackToDefaultCulture = true)
        {
            var recognizer = new NumberRecognizer(options);
            var model = recognizer.GetPercentageModel(culture, fallbackToDefaultCulture);
            return model.Parse(query);
        }

        public static List<ModelResult> RecognizeNumberRange(string query, string culture, NumberOptions options = NumberOptions.None, bool fallbackToDefaultCulture = true)
        {
            return RecognizeByModel(recognizer => recognizer.GetNumberRangeModel(culture, fallbackToDefaultCulture), query, options);
        }

        public NumberModel GetNumberModel(string culture = null, bool fallbackToDefaultCulture = true)
        {
            return GetModel<NumberModel>(culture, fallbackToDefaultCulture);
        }

        public OrdinalModel GetOrdinalModel(string culture = null, bool fallbackToDefaultCulture = true)
        {
            return GetModel<OrdinalModel>(culture, fallbackToDefaultCulture);
        }

        public PercentModel GetPercentageModel(string culture = null, bool fallbackToDefaultCulture = true)
        {
            return GetModel<PercentModel>(culture, fallbackToDefaultCulture);
        }

        public NumberRangeModel GetNumberRangeModel(string culture = null, bool fallbackToDefaultCulture = true)
        {
            return GetModel<NumberRangeModel>(culture, fallbackToDefaultCulture);
        }

        protected override void InitializeConfiguration()
        {
            RegisterModel<NumberModel>(
                Culture.English,
                options => new NumberModel(
                    AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Number, new EnglishNumberParserConfiguration(options)),
                    English.MergedNumberExtractor.GetInstance(NumberMode.PureNumber, options)));

            RegisterModel<OrdinalModel>(
                Culture.English,
                options => new OrdinalModel(
                    AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Ordinal, new EnglishNumberParserConfiguration(options)),
                    English.OrdinalExtractor.GetInstance(options)));

            RegisterModel<PercentModel>(
                Culture.English,
                options => new PercentModel(
                    AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Percentage, new EnglishNumberParserConfiguration(options)),
                    new English.PercentageExtractor(options)));

            RegisterModel<NumberRangeModel>(
                Culture.English,
                options => new NumberRangeModel(
                    new BaseNumberRangeParser(new EnglishNumberRangeParserConfiguration()),
                    new English.NumberRangeExtractor(options)));

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
                            new Chinese.NumberRangeExtractor(options)));

            RegisterModel<NumberModel>(
                Culture.Spanish,
                (options) => new NumberModel(
                    AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Number, new SpanishNumberParserConfiguration()),
                    Spanish.NumberExtractor.GetInstance(NumberMode.PureNumber, options)));

            RegisterModel<OrdinalModel>(
                Culture.Spanish,
                (options) => new OrdinalModel(
                    AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Ordinal, new SpanishNumberParserConfiguration()),
                    Spanish.OrdinalExtractor.GetInstance()));

            RegisterModel<PercentModel>(
                Culture.Spanish,
                (options) => new PercentModel(
                    AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Percentage, new SpanishNumberParserConfiguration()),
                    new Spanish.PercentageExtractor()));

            RegisterModel<NumberRangeModel>(
                Culture.Spanish,
                (options) => new NumberRangeModel(
                    new BaseNumberRangeParser(new SpanishNumberRangeParserConfiguration()),
                    new Spanish.NumberRangeExtractor(options)));

            RegisterModel<NumberModel>(
                Culture.Portuguese,
                (options) => new NumberModel(
                    AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Number, new PortugueseNumberParserConfiguration()),
                    Portuguese.NumberExtractor.GetInstance(NumberMode.PureNumber, options)));

            RegisterModel<OrdinalModel>(
                Culture.Portuguese,
                (options) => new OrdinalModel(
                    AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Ordinal, new PortugueseNumberParserConfiguration()),
                    Portuguese.OrdinalExtractor.GetInstance()));

            RegisterModel<PercentModel>(
                Culture.Portuguese,
                (options) => new PercentModel(
                    AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Percentage, new PortugueseNumberParserConfiguration()),
                    new Portuguese.PercentageExtractor()));

            RegisterModel<NumberModel>(
                Culture.French,
                (options) => new NumberModel(
                    AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Number, new FrenchNumberParserConfiguration()),
                    French.NumberExtractor.GetInstance(NumberMode.PureNumber, options)));

            RegisterModel<OrdinalModel>(
                Culture.French,
                (options) => new OrdinalModel(
                    AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Ordinal, new FrenchNumberParserConfiguration()),
                    French.OrdinalExtractor.GetInstance()));

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

            /*
            RegisterModel<NumberModel>(
               Culture.Italian,
               (options) => new NumberModel(
                   AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Number, new ItalianNumberParserConfiguration()),
                   Italian.NumberExtractor.GetInstance(NumberMode.PureNumber, options)));

            RegisterModel<OrdinalModel>(
                Culture.Italian,
                (options) => new OrdinalModel(
                    AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Ordinal, new ItalianNumberParserConfiguration()),
                    Italian.OrdinalExtractor.GetInstance()));

            RegisterModel<PercentModel>(
                Culture.Italian,
                (options) => new PercentModel(
                    AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Percentage, new ItalianNumberParserConfiguration()),
                    new Italian.PercentageExtractor()));
            */

            RegisterModel<NumberModel>(
                Culture.Japanese,
                (options) => new NumberModel(
                    AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Number, new JapaneseNumberParserConfiguration()),
                    new Japanese.NumberExtractor()));

            RegisterModel<OrdinalModel>(
                Culture.Japanese,
                (options) => new OrdinalModel(
                    AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Ordinal, new JapaneseNumberParserConfiguration()),
                    new Japanese.OrdinalExtractor()));

            RegisterModel<PercentModel>(
                Culture.Japanese,
                (options) => new PercentModel(
                    AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Percentage, new JapaneseNumberParserConfiguration()),
                    new Japanese.PercentageExtractor()));

            /*
            RegisterModel<NumberRangeModel>(
                Culture.Japanese,
                (options) => new NumberRangeModel(
                    new BaseNumberRangeParser(new JapaneseNumberRangeParserConfiguration()),
                    new Japanese.NumberRangeExtractor(options)));
            */

            RegisterModel<NumberModel>(
                Culture.Korean,
                (options) => new NumberModel(
                    AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Number, new KoreanNumberParserConfiguration()),
                    new Korean.NumberExtractor()));

            RegisterModel<NumberModel>(
                Culture.Dutch,
                (options) => new NumberModel(
                    AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Number, new DutchNumberParserConfiguration()),
                    Dutch.NumberExtractor.GetInstance(NumberMode.PureNumber)));

            RegisterModel<OrdinalModel>(
                Culture.Dutch,
                (options) => new OrdinalModel(
                    AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Ordinal, new DutchNumberParserConfiguration()),
                    Dutch.OrdinalExtractor.GetInstance()));

            RegisterModel<PercentModel>(
                Culture.Dutch,
                (options) => new PercentModel(
                    AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Percentage, new DutchNumberParserConfiguration()),
                    new Dutch.PercentageExtractor(options)));

            // When registering NumberRangeModel, enable TestNumber_Dutch -> NumberRangeModel tests
            /*
            RegisterModel<NumberRangeModel>(
                Culture.Dutch,
                (options) => new NumberRangeModel(
                    new BaseNumberRangeParser(new DutchNumberRangeParserConfiguration()),
                    new Dutch.NumberRangeExtractor(options)));
            */
        }

        private static List<ModelResult> RecognizeByModel(Func<NumberRecognizer, IModel> getModelFunc, string query, NumberOptions options)
        {
            var recognizer = new NumberRecognizer(options);
            var model = getModelFunc(recognizer);
            return model.Parse(query);
        }
    }
}