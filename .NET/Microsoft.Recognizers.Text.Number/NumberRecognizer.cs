using System;
using System.Collections.Generic;
using Microsoft.Recognizers.Text.Number.Arabic;
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
                Culture.Arabic,
                (options) => new NumberModel(
                    AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Number, new ArabicNumberParserConfiguration(
                                                              new BaseNumberOptionsConfiguration(Culture.Arabic, options))),
                    Arabic.NumberExtractor.GetInstance(new BaseNumberOptionsConfiguration(Culture.Arabic, options, NumberMode.PureNumber))));

            RegisterModel<OrdinalModel>(
                Culture.Arabic,
                (options) => new OrdinalModel(
                    AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Ordinal, new ArabicNumberParserConfiguration(
                                                              new BaseNumberOptionsConfiguration(Culture.Arabic, options))),
                    Arabic.OrdinalExtractor.GetInstance(new BaseNumberOptionsConfiguration(Culture.Arabic, options))));

            RegisterModel<PercentModel>(
                Culture.Arabic,
                (options) => new PercentModel(
                    AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Percentage, new ArabicNumberParserConfiguration(
                                                              new BaseNumberOptionsConfiguration(Culture.Arabic, options))),
                    new Arabic.PercentageExtractor(new BaseNumberOptionsConfiguration(Culture.Arabic, options))));

            RegisterModel<NumberRangeModel>(
                Culture.Arabic,
                (options) => new NumberRangeModel(
                    new BaseNumberRangeParser(new ArabicNumberRangeParserConfiguration(
                                                  new BaseNumberOptionsConfiguration(Culture.Arabic, options))),
                    new Arabic.NumberRangeExtractor(new BaseNumberOptionsConfiguration(Culture.Arabic, options))));

            RegisterModel<NumberModel>(
                Culture.English,
                (options) => new NumberModel(
                    AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Number, new EnglishNumberParserConfiguration(
                                                              new BaseNumberOptionsConfiguration(Culture.English, options))),
                    English.MergedNumberExtractor.GetInstance(new BaseNumberOptionsConfiguration(Culture.English, options, NumberMode.PureNumber))));

            RegisterModel<OrdinalModel>(
                Culture.English,
                (options) => new OrdinalModel(
                    AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Ordinal, new EnglishNumberParserConfiguration(
                                                              new BaseNumberOptionsConfiguration(Culture.English, options))),
                    English.OrdinalExtractor.GetInstance(new BaseNumberOptionsConfiguration(Culture.English, options))));

            RegisterModel<PercentModel>(
                Culture.English,
                (options) => new PercentModel(
                    AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Percentage, new EnglishNumberParserConfiguration(
                                                              new BaseNumberOptionsConfiguration(Culture.English, options))),
                    new English.PercentageExtractor(new BaseNumberOptionsConfiguration(Culture.English, options))));

            RegisterModel<NumberRangeModel>(
                Culture.English,
                (options) => new NumberRangeModel(
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
                Culture.SpanishMexican,
                (options) => new NumberModel(
                    AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Number, new SpanishNumberParserConfiguration(
                                                              new BaseNumberOptionsConfiguration(Culture.SpanishMexican, options))),
                    Spanish.NumberExtractor.GetInstance(new BaseNumberOptionsConfiguration(Culture.SpanishMexican, options, NumberMode.PureNumber))));

            RegisterModel<OrdinalModel>(
                Culture.SpanishMexican,
                (options) => new OrdinalModel(
                    AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Ordinal, new SpanishNumberParserConfiguration(
                                                              new BaseNumberOptionsConfiguration(Culture.SpanishMexican, options))),
                    Spanish.OrdinalExtractor.GetInstance(new BaseNumberOptionsConfiguration(Culture.SpanishMexican, options))));

            RegisterModel<PercentModel>(
                Culture.SpanishMexican,
                (options) => new PercentModel(
                    AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Percentage, new SpanishNumberParserConfiguration(
                                                              new BaseNumberOptionsConfiguration(Culture.SpanishMexican, options))),
                    new Spanish.PercentageExtractor(new BaseNumberOptionsConfiguration(Culture.SpanishMexican, options))));

            RegisterModel<NumberRangeModel>(
                Culture.SpanishMexican,
                (options) => new NumberRangeModel(
                    new BaseNumberRangeParser(new SpanishNumberRangeParserConfiguration(
                                                  new BaseNumberOptionsConfiguration(Culture.SpanishMexican, options))),
                    new Spanish.NumberRangeExtractor(new BaseNumberOptionsConfiguration(Culture.SpanishMexican, options))));

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

            /*
            RegisterModel<NumberRangeModel>(
                Culture.Portuguese,
                (options) => new NumberRangeModel(
                    new BaseNumberRangeParser(new PortugueseRangeParserConfiguration(
                                                  new BaseNumberOptionsConfiguration(Culture.Portuguese, options))),
                    new Portuguese.NumberRangeExtractor(new BaseNumberOptionsConfiguration(Culture.Portuguese, options))));
            */

            RegisterModel<NumberModel>(
                Culture.French,
                (options) => new NumberModel(
                    AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Number, new FrenchNumberParserConfiguration(
                                                              new BaseNumberOptionsConfiguration(Culture.French, options))),
                    French.NumberExtractor.GetInstance(new BaseNumberOptionsConfiguration(Culture.French, options, NumberMode.PureNumber))));

            RegisterModel<OrdinalModel>(
                Culture.French,
                (options) => new OrdinalModel(
                    AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Ordinal, new FrenchNumberParserConfiguration(
                                                              new BaseNumberOptionsConfiguration(Culture.French, options))),
                    French.OrdinalExtractor.GetInstance(new BaseNumberOptionsConfiguration(Culture.French, options))));

            RegisterModel<PercentModel>(
                Culture.French,
                (options) => new PercentModel(
                    AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Percentage, new FrenchNumberParserConfiguration(
                                                              new BaseNumberOptionsConfiguration(Culture.French, options))),
                    new French.PercentageExtractor(new BaseNumberOptionsConfiguration(Culture.French, options))));

            RegisterModel<NumberRangeModel>(
                Culture.French,
                (options) => new NumberRangeModel(
                    new BaseNumberRangeParser(new FrenchNumberRangeParserConfiguration(
                                                  new BaseNumberOptionsConfiguration(Culture.French, options))),
                    new French.NumberRangeExtractor(new BaseNumberOptionsConfiguration(Culture.French, options))));

            RegisterModel<NumberModel>(
                Culture.German,
                (options) => new NumberModel(
                    AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Number, new GermanNumberParserConfiguration(
                                                              new BaseNumberOptionsConfiguration(Culture.German, options))),
                    German.NumberExtractor.GetInstance(new BaseNumberOptionsConfiguration(Culture.German, options, NumberMode.PureNumber))));

            RegisterModel<OrdinalModel>(
                Culture.German,
                (options) => new OrdinalModel(
                    AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Ordinal, new GermanNumberParserConfiguration(
                                                              new BaseNumberOptionsConfiguration(Culture.German, options))),
                    German.OrdinalExtractor.GetInstance(new BaseNumberOptionsConfiguration(Culture.German, options))));

            RegisterModel<PercentModel>(
                Culture.German,
                (options) => new PercentModel(
                    AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Percentage, new GermanNumberParserConfiguration(
                                                              new BaseNumberOptionsConfiguration(Culture.German, options))),
                    new German.PercentageExtractor(new BaseNumberOptionsConfiguration(Culture.German, options))));

            RegisterModel<NumberRangeModel>(
                Culture.German,
                (options) => new NumberRangeModel(
                    new BaseNumberRangeParser(new GermanNumberRangeParserConfiguration(
                                                  new BaseNumberOptionsConfiguration(Culture.German, options))),
                    new German.NumberRangeExtractor(new BaseNumberOptionsConfiguration(Culture.German, options))));

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

            RegisterModel<NumberRangeModel>(
                Culture.Dutch,
                (options) => new NumberRangeModel(
                    new BaseNumberRangeParser(new DutchNumberRangeParserConfiguration(
                                                    new BaseNumberOptionsConfiguration(Culture.Dutch, options))),
                    new Dutch.NumberRangeExtractor(new BaseNumberOptionsConfiguration(Culture.Dutch, options))));

            RegisterModel<NumberModel>(
                Culture.Japanese,
                (options) => new NumberModel(
                    AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Number, new JapaneseNumberParserConfiguration(
                                                              new BaseNumberOptionsConfiguration(Culture.Japanese, options))),
                    new Japanese.NumberExtractor(new BaseNumberOptionsConfiguration(Culture.Japanese, options))));

            RegisterModel<OrdinalModel>(
                Culture.Japanese,
                (options) => new OrdinalModel(
                    AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Ordinal, new JapaneseNumberParserConfiguration(
                                                              new BaseNumberOptionsConfiguration(Culture.Japanese, options))),
                    new Japanese.OrdinalExtractor(new BaseNumberOptionsConfiguration(Culture.Japanese, options))));

            RegisterModel<PercentModel>(
                Culture.Japanese,
                (options) => new PercentModel(
                    AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Percentage, new JapaneseNumberParserConfiguration(
                                                              new BaseNumberOptionsConfiguration(Culture.Japanese, options))),
                    new Japanese.PercentageExtractor(new BaseNumberOptionsConfiguration(Culture.Japanese, options))));

            RegisterModel<NumberRangeModel>(
                Culture.Japanese,
                (options) => new NumberRangeModel(
                    new BaseNumberRangeParser(new JapaneseNumberRangeParserConfiguration(
                                                  new BaseNumberOptionsConfiguration(Culture.Japanese, options))),
                    new Japanese.NumberRangeExtractor(new BaseNumberOptionsConfiguration(Culture.Japanese, options))));

            RegisterModel<NumberModel>(
                Culture.Korean,
                (options) => new NumberModel(
                    AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Number, new KoreanNumberParserConfiguration(
                                                              new BaseNumberOptionsConfiguration(Culture.Korean, options))),
                    new Korean.NumberExtractor(new BaseNumberOptionsConfiguration(Culture.Korean, options))));

            RegisterModel<OrdinalModel>(
                Culture.Korean,
                (options) => new OrdinalModel(
                    AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Ordinal, new KoreanNumberParserConfiguration(
                                                              new BaseNumberOptionsConfiguration(Culture.Korean, options))),
                    new Korean.OrdinalExtractor(new BaseNumberOptionsConfiguration(Culture.Korean, options))));

            RegisterModel<PercentModel>(
                Culture.Korean,
                (options) => new PercentModel(
                     AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Percentage, new KoreanNumberParserConfiguration(
                                                               new BaseNumberOptionsConfiguration(Culture.Korean, options))),
                     new Korean.PercentageExtractor(new BaseNumberOptionsConfiguration(Culture.Korean, options))));

            RegisterModel<NumberRangeModel>(
                Culture.Korean,
                (options) => new NumberRangeModel(
                    new BaseNumberRangeParser(new KoreanNumberRangeParserConfiguration(
                                                        new BaseNumberOptionsConfiguration(Culture.Korean, options))),
                    new Korean.NumberRangeExtractor(new BaseNumberOptionsConfiguration(Culture.Korean, options))));

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

            RegisterModel<NumberRangeModel>(
                Culture.Hindi,
                options => new NumberRangeModel(
                    new BaseNumberRangeParser(new HindiNumberRangeParserConfiguration(
                                                  new BaseNumberOptionsConfiguration(Culture.Hindi, options))),
                    new Hindi.NumberRangeExtractor(new BaseNumberOptionsConfiguration(Culture.Hindi, options))));

            RegisterModel<NumberModel>(
                Culture.Swedish,
                (options) => new NumberModel(
                    AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Number, new SwedishNumberParserConfiguration(
                                                              new BaseNumberOptionsConfiguration(Culture.Swedish, options))),
                    Swedish.NumberExtractor.GetInstance(new BaseNumberOptionsConfiguration(Culture.Swedish, options, NumberMode.PureNumber))));

            RegisterModel<OrdinalModel>(
                Culture.Swedish,
                (options) => new OrdinalModel(
                    AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Ordinal, new SwedishNumberParserConfiguration(
                                                              new BaseNumberOptionsConfiguration(Culture.Swedish, options))),
                    Swedish.OrdinalExtractor.GetInstance(new BaseNumberOptionsConfiguration(Culture.Swedish, options))));

            RegisterModel<PercentModel>(
                Culture.Swedish,
                (options) => new PercentModel(
                    AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Percentage, new SwedishNumberParserConfiguration(
                                                              new BaseNumberOptionsConfiguration(Culture.Swedish, options))),
                    new Swedish.PercentageExtractor(new BaseNumberOptionsConfiguration(Culture.Swedish, options))));

            // RegisterModel<NumberRangeModel>(
            //    Culture.Swedish,
            //    (options) => new NumberRangeModel(
            //        new BaseNumberRangeParser(new SwedishNumberRangeParserConfiguration(
            //                                        new BaseNumberOptionsConfiguration(Culture.Swedish, options))),
            //        new Swedish.NumberRangeExtractor(new BaseNumberOptionsConfiguration(Culture.Swedish, options))));
        }

        private static List<ModelResult> RecognizeByModel(Func<NumberRecognizer, IModel> getModelFunc, string query, NumberOptions options)
        {
            var recognizer = new NumberRecognizer(options);
            var model = getModelFunc(recognizer);
            return model.Parse(query);
        }
    }
}