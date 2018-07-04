using System;
using System.Collections.Generic;
using Microsoft.Recognizers.Text.Number.Chinese;
using Microsoft.Recognizers.Text.Number.English;
using Microsoft.Recognizers.Text.Number.French;
using Microsoft.Recognizers.Text.Number.German;
using Microsoft.Recognizers.Text.Number.Japanese;
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

        private static List<ModelResult> RecognizeByModel(Func<NumberRecognizer, IModel> getModelFunc, string query, NumberOptions options)
        {
            var recognizer = new NumberRecognizer(options);
            var model = getModelFunc(recognizer);
            return model.Parse(query);
        }

        protected override void InitializeConfiguration()
        {
            #region English

            RegisterModel<NumberModel>(
                Culture.English,
                options => new NumberModel(
                    AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Number,
                        new EnglishNumberParserConfiguration(options)),
                    English.MergedNumberExtractor.GetInstance(NumberMode.PureNumber, options)));
            RegisterModel<OrdinalModel>(
                Culture.English,
                options => new OrdinalModel(
                    AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Ordinal,
                        new EnglishNumberParserConfiguration(options)),
                    English.OrdinalExtractor.GetInstance()));
            RegisterModel<PercentModel>(
                Culture.English,
                options => new PercentModel(
                    AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Percentage,
                        new EnglishNumberParserConfiguration(options)),
                    new English.PercentageExtractor(options)));
            RegisterModel<NumberRangeModel>(
                Culture.English,
                options => new NumberRangeModel(
                    new BaseNumberRangeParser(new EnglishNumberRangeParserConfiguration()),
                    new English.NumberRangeExtractor()));
            #endregion

            #region Chinese
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
            #endregion

            #region Spanish
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
            #endregion

            #region Portuguese
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
            #endregion

            #region French
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
            #endregion

            #region German
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
            #endregion

            #region Japanese
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
            RegisterModel<NumberRangeModel>(
                Culture.Japanese,
                (options) => new NumberRangeModel(
                            new BaseNumberRangeParser(new JapaneseNumberRangeParserConfiguration()),
                            new Japanese.NumberRangeExtractor()));
            #endregion
        }
    }
}