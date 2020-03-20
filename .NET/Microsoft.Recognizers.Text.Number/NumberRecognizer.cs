using System;
using System.Collections.Generic;
using Microsoft.Recognizers.Text.Number.Chinese;
using Microsoft.Recognizers.Text.Number.Dutch;
using Microsoft.Recognizers.Text.Number.English;
using Microsoft.Recognizers.Text.Number.French;
using Microsoft.Recognizers.Text.Number.German;
using Microsoft.Recognizers.Text.Number.Hindi;
using Microsoft.Recognizers.Text.Number.Italian;
using Microsoft.Recognizers.Text.Number.Japanese;
using Microsoft.Recognizers.Text.Number.Korean;
using Microsoft.Recognizers.Text.Number.Portuguese;
using Microsoft.Recognizers.Text.Number.Spanish;
using Microsoft.Recognizers.Text.Number.Swedish;
using Microsoft.Recognizers.Text.Number.Turkish;

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
            return RecognizeByModel(recognizer => recognizer.GetPercentageModel(culture, fallbackToDefaultCulture), query, options);
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
                    AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Number, new EnglishNumberParserConfiguration(
                                                              new BaseNumberOptionsConfiguration(Culture.English, options))),
                    English.MergedNumberExtractor.GetInstance(new BaseNumberOptionsConfiguration(Culture.English, options, NumberMode.PureNumber))));

            RegisterModel<OrdinalModel>(
                Culture.English,
                options => new OrdinalModel(
                    AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Ordinal, new EnglishNumberParserConfiguration(
                                                              new BaseNumberOptionsConfiguration(Culture.English, options))),
                    English.OrdinalExtractor.GetInstance(new BaseNumberOptionsConfiguration(Culture.English, options))));

            RegisterModel<PercentModel>(
                Culture.English,
                options => new PercentModel(
                    AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Percentage, new EnglishNumberParserConfiguration(
                                                              new BaseNumberOptionsConfiguration(Culture.English, options))),
                    new English.PercentageExtractor(new BaseNumberOptionsConfiguration(Culture.English, options))));

            RegisterModel<NumberRangeModel>(
                Culture.English,
                options => new NumberRangeModel(
                    new BaseNumberRangeParser(new EnglishNumberRangeParserConfiguration(
                                                  new BaseNumberOptionsConfiguration(Culture.English, options))),
                    new English.NumberRangeExtractor(new BaseNumberOptionsConfiguration(Culture.English, options))));

            RegisterModel<NumberModel>(
                Culture.Chinese,
                (options) => new NumberModel(
                    AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Number, new ChineseNumberParserConfiguration(
                                                              new BaseNumberOptionsConfiguration(Culture.Chinese, options))),
                    new Chinese.NumberExtractor(new BaseNumberOptionsConfiguration(Culture.Chinese, options))));

            RegisterModel<OrdinalModel>(
                Culture.Chinese,
                (options) => new OrdinalModel(
                    AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Ordinal, new ChineseNumberParserConfiguration(
                                                              new BaseNumberOptionsConfiguration(Culture.Chinese, options))),
                    new Chinese.OrdinalExtractor(new BaseNumberOptionsConfiguration(Culture.Chinese, options))));

            RegisterModel<PercentModel>(
                Culture.Chinese,
                (options) => new PercentModel(
                    AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Percentage, new ChineseNumberParserConfiguration(
                                                              new BaseNumberOptionsConfiguration(Culture.Chinese, options))),
                    new Chinese.PercentageExtractor(new BaseNumberOptionsConfiguration(Culture.Chinese, options))));

            RegisterModel<NumberRangeModel>(
                Culture.Chinese,
                (options) => new NumberRangeModel(
                            new BaseNumberRangeParser(new ChineseNumberRangeParserConfiguration(
                                                          new BaseNumberOptionsConfiguration(Culture.Chinese, options))),
                            new Chinese.NumberRangeExtractor(new BaseNumberOptionsConfiguration(Culture.Chinese, options))));

            RegisterModel<NumberModel>(
                Culture.Spanish,
                (options) => new NumberModel(
                    AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Number, new SpanishNumberParserConfiguration(
                                                              new BaseNumberOptionsConfiguration(Culture.Spanish, options))),
                    Spanish.NumberExtractor.GetInstance(new BaseNumberOptionsConfiguration(Culture.Spanish, options, NumberMode.PureNumber))));

            RegisterModel<OrdinalModel>(
                Culture.Spanish,
                (options) => new OrdinalModel(
                    AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Ordinal, new SpanishNumberParserConfiguration(
                                                              new BaseNumberOptionsConfiguration(Culture.Spanish, options))),
                    Spanish.OrdinalExtractor.GetInstance(new BaseNumberOptionsConfiguration(Culture.Spanish, options))));

            RegisterModel<PercentModel>(
                Culture.Spanish,
                (options) => new PercentModel(
                    AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Percentage, new SpanishNumberParserConfiguration(
                                                              new BaseNumberOptionsConfiguration(Culture.Spanish, options))),
                    new Spanish.PercentageExtractor(new BaseNumberOptionsConfiguration(Culture.Spanish, options))));

            RegisterModel<NumberRangeModel>(
                Culture.Spanish,
                (options) => new NumberRangeModel(
                    new BaseNumberRangeParser(new SpanishNumberRangeParserConfiguration(
                                                  new BaseNumberOptionsConfiguration(Culture.Spanish, options))),
                    new Spanish.NumberRangeExtractor(new BaseNumberOptionsConfiguration(Culture.Spanish, options))));

            RegisterModel<NumberModel>(
                Culture.Portuguese,
                (options) => new NumberModel(
                    AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Number, new PortugueseNumberParserConfiguration(
                                                              new BaseNumberOptionsConfiguration(Culture.Portuguese, options))),
                    Portuguese.NumberExtractor.GetInstance(new BaseNumberOptionsConfiguration(Culture.Portuguese, options, NumberMode.PureNumber))));

            RegisterModel<OrdinalModel>(
                Culture.Portuguese,
                (options) => new OrdinalModel(
                    AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Ordinal, new PortugueseNumberParserConfiguration(
                                                              new BaseNumberOptionsConfiguration(Culture.Portuguese, options))),
                    Portuguese.OrdinalExtractor.GetInstance(new BaseNumberOptionsConfiguration(Culture.Portuguese, options))));

            RegisterModel<PercentModel>(
                Culture.Portuguese,
                (options) => new PercentModel(
                    AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Percentage, new PortugueseNumberParserConfiguration(
                                                              new BaseNumberOptionsConfiguration(Culture.Portuguese, options))),
                    new Portuguese.PercentageExtractor(new BaseNumberOptionsConfiguration(Culture.Portuguese, options))));

            RegisterModel<NumberModel>(
                Culture.French,
                (options) => new NumberModel(
                    AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Number, new FrenchNumberParserConfiguration(
                                                              new BaseNumberOptionsConfiguration(Culture.French, options))),
                    French.NumberExtractor.GetInstance(NumberMode.PureNumber, options)));

            RegisterModel<OrdinalModel>(
                Culture.French,
                (options) => new OrdinalModel(
                    AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Ordinal, new FrenchNumberParserConfiguration(
                                                              new BaseNumberOptionsConfiguration(Culture.French, options))),
                    French.OrdinalExtractor.GetInstance()));

            RegisterModel<PercentModel>(
                Culture.French,
                (options) => new PercentModel(
                    AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Percentage, new FrenchNumberParserConfiguration(
                                                              new BaseNumberOptionsConfiguration(Culture.French, options))),
                    new French.PercentageExtractor()));

            RegisterModel<NumberModel>(
                Culture.German,
                (options) => new NumberModel(
                    AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Number, new GermanNumberParserConfiguration(
                                                              new BaseNumberOptionsConfiguration(Culture.German, options))),
                    German.NumberExtractor.GetInstance(NumberMode.PureNumber)));

            RegisterModel<OrdinalModel>(
                Culture.German,
                (options) => new OrdinalModel(
                    AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Ordinal, new GermanNumberParserConfiguration(
                                                              new BaseNumberOptionsConfiguration(Culture.German, options))),
                    German.OrdinalExtractor.GetInstance()));

            RegisterModel<PercentModel>(
                Culture.German,
                (options) => new PercentModel(
                    AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Percentage, new GermanNumberParserConfiguration(
                                                              new BaseNumberOptionsConfiguration(Culture.German, options))),
                    new German.PercentageExtractor()));

            RegisterModel<NumberModel>(
               Culture.Italian,
               (options) => new NumberModel(
                   AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Number, new ItalianNumberParserConfiguration(
                                                             new BaseNumberOptionsConfiguration(Culture.Italian, options))),
                   Italian.NumberExtractor.GetInstance(NumberMode.PureNumber, options)));

            RegisterModel<OrdinalModel>(
                Culture.Italian,
                (options) => new OrdinalModel(
                    AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Ordinal, new ItalianNumberParserConfiguration(
                                                              new BaseNumberOptionsConfiguration(Culture.Italian, options))),
                    Italian.OrdinalExtractor.GetInstance()));

            RegisterModel<PercentModel>(
                Culture.Italian,
                (options) => new PercentModel(
                    AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Percentage, new ItalianNumberParserConfiguration(
                                                              new BaseNumberOptionsConfiguration(Culture.Italian, options))),
                    new Italian.PercentageExtractor()));

            RegisterModel<NumberRangeModel>(
                Culture.Italian,
                (options) => new NumberRangeModel(
                    new BaseNumberRangeParser(new ItalianNumberRangeParserConfiguration(
                                                  new BaseNumberOptionsConfiguration(Culture.Italian, options))),
                    new Italian.NumberRangeExtractor(new BaseNumberOptionsConfiguration(Culture.Italian, options))));

            RegisterModel<NumberModel>(
                Culture.Japanese,
                (options) => new NumberModel(
                    AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Number, new JapaneseNumberParserConfiguration(
                                                              new BaseNumberOptionsConfiguration(Culture.Japanese, options))),
                    new Japanese.NumberExtractor()));

            RegisterModel<OrdinalModel>(
                Culture.Japanese,
                (options) => new OrdinalModel(
                    AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Ordinal, new JapaneseNumberParserConfiguration(
                                                              new BaseNumberOptionsConfiguration(Culture.Japanese, options))),
                    new Japanese.OrdinalExtractor()));

            RegisterModel<PercentModel>(
                Culture.Japanese,
                (options) => new PercentModel(
                    AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Percentage, new JapaneseNumberParserConfiguration(
                                                              new BaseNumberOptionsConfiguration(Culture.Japanese, options))),
                    new Japanese.PercentageExtractor()));

            /*
            RegisterModel<NumberRangeModel>(
                Culture.Japanese,
                (options) => new NumberRangeModel(
                    new BaseNumberRangeParser(new JapaneseNumberRangeParserConfiguration(
                                                  new BaseNumberOptionsConfiguration(Culture.Japanese, options))),
                    new Japanese.NumberRangeExtractor(options)));
            */

            RegisterModel<NumberModel>(
                Culture.Korean,
                (options) => new NumberModel(
                    AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Number, new KoreanNumberParserConfiguration(
                                                              new BaseNumberOptionsConfiguration(Culture.Korean, options))),
                    new Korean.NumberExtractor()));

            RegisterModel<NumberModel>(
                Culture.Dutch,
                (options) => new NumberModel(
                    AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Number, new DutchNumberParserConfiguration(
                                                              new BaseNumberOptionsConfiguration(Culture.Dutch, options))),
                    Dutch.NumberExtractor.GetInstance(new BaseNumberOptionsConfiguration(Culture.Dutch, options, NumberMode.PureNumber))));

            RegisterModel<OrdinalModel>(
                Culture.Dutch,
                (options) => new OrdinalModel(
                    AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Ordinal, new DutchNumberParserConfiguration(
                                                              new BaseNumberOptionsConfiguration(Culture.Dutch, options))),
                    Dutch.OrdinalExtractor.GetInstance(new BaseNumberOptionsConfiguration(Culture.Dutch, options))));

            RegisterModel<PercentModel>(
                Culture.Dutch,
                (options) => new PercentModel(
                    AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Percentage, new DutchNumberParserConfiguration(
                                                              new BaseNumberOptionsConfiguration(Culture.Dutch, options))),
                    new Dutch.PercentageExtractor(new BaseNumberOptionsConfiguration(Culture.Dutch, options))));

            // When registering NumberRangeModel, enable TestNumber_Dutch -> NumberRangeModel tests
            /*
            RegisterModel<NumberRangeModel>(
                Culture.Dutch,
                (options) => new NumberRangeModel(
                    new BaseNumberRangeParser(new DutchNumberRangeParserConfiguration()),
                    new Dutch.NumberRangeExtractor(options)));
            */

            RegisterModel<NumberModel>(
               Culture.Turkish,
               (options) => new NumberModel(
                   AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Number, new TurkishNumberParserConfiguration(
                                                             new BaseNumberOptionsConfiguration(Culture.Turkish, options))),
                   Turkish.NumberExtractor.GetInstance(NumberMode.PureNumber)));

            RegisterModel<OrdinalModel>(
                Culture.Turkish,
                (options) => new OrdinalModel(
                    AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Ordinal, new TurkishNumberParserConfiguration(
                                                              new BaseNumberOptionsConfiguration(Culture.Turkish, options))),
                    Turkish.OrdinalExtractor.GetInstance()));

            RegisterModel<PercentModel>(
                Culture.Turkish,
                (options) => new PercentModel(
                    AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Percentage, new TurkishNumberParserConfiguration(
                                                              new BaseNumberOptionsConfiguration(Culture.Turkish, options))),
                    new Turkish.PercentageExtractor(options)));

            RegisterModel<NumberRangeModel>(
               Culture.Turkish,
               options => new NumberRangeModel(
                   new BaseNumberRangeParser(new TurkishNumberRangeParserConfiguration(
                                                 new BaseNumberOptionsConfiguration(Culture.Turkish, options))),
                   new Turkish.NumberRangeExtractor(new BaseNumberOptionsConfiguration(Culture.Turkish, options))));

            RegisterModel<NumberModel>(
                Culture.Hindi,
                options => new NumberModel(
                    AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Number, new HindiNumberParserConfiguration(
                                                              new BaseNumberOptionsConfiguration(Culture.Hindi, options))),
                    Hindi.MergedNumberExtractor.GetInstance(NumberMode.PureNumber, options)));

            RegisterModel<OrdinalModel>(
                Culture.Hindi,
                options => new OrdinalModel(
                    AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Ordinal, new HindiNumberParserConfiguration(
                                                              new BaseNumberOptionsConfiguration(Culture.Hindi, options))),
                    Hindi.OrdinalExtractor.GetInstance(options)));

            RegisterModel<PercentModel>(
                Culture.Hindi,
                options => new PercentModel(
                    AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Percentage, new HindiNumberParserConfiguration(
                                                              new BaseNumberOptionsConfiguration(Culture.Hindi, options))),
                    new Hindi.PercentageExtractor(options)));

            // @TODO Uncomment once all the tests pass
            /*

            RegisterModel<NumberRangeModel>(
                Culture.Hindi,
                options => new NumberRangeModel(
                    new BaseNumberRangeParser(new HindiNumberRangeParserConfiguration(
                                                  new BaseNumberOptionsConfiguration(Culture.Hindi, options))),
                    new Hindi.NumberRangeExtractor(new BaseNumberOptionsConfiguration(Culture.Hindi, options))));*/

            RegisterModel<NumberModel>(
                Culture.Swedish,
                (options) => new NumberModel(
                    AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Number, new SwedishNumberParserConfiguration(
                                                              new BaseNumberOptionsConfiguration(Culture.Swedish, options))),
                    Swedish.NumberExtractor.GetInstance(NumberMode.PureNumber, options)));

            RegisterModel<OrdinalModel>(
                Culture.Swedish,
                (options) => new OrdinalModel(
                    AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Ordinal, new SwedishNumberParserConfiguration(
                                                              new BaseNumberOptionsConfiguration(Culture.Swedish, options))),
                    Swedish.OrdinalExtractor.GetInstance()));

            RegisterModel<PercentModel>(
                Culture.Swedish,
                (options) => new PercentModel(
                    AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Percentage, new SwedishNumberParserConfiguration(
                                                              new BaseNumberOptionsConfiguration(Culture.Swedish, options))),
                    new Swedish.PercentageExtractor(options)));
        }

        private static List<ModelResult> RecognizeByModel(Func<NumberRecognizer, IModel> getModelFunc, string query, NumberOptions options)
        {
            var recognizer = new NumberRecognizer(options);
            var model = getModelFunc(recognizer);
            return model.Parse(query);
        }
    }
}