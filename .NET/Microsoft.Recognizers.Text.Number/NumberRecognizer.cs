using System;
using System.Collections.Generic;

using Microsoft.Recognizers.Text.Number.Chinese;
using Microsoft.Recognizers.Text.Number.English;
using Microsoft.Recognizers.Text.Number.French;
using Microsoft.Recognizers.Text.Number.Portuguese;
using Microsoft.Recognizers.Text.Number.Spanish;
using Microsoft.Recognizers.Text.Number.German;

namespace Microsoft.Recognizers.Text.Number
{
    public class NumberRecognizer : Recognizer
    {
        public static readonly NumberRecognizer Instance = new NumberRecognizer(NumberOptions.None);

        private NumberRecognizer(NumberOptions options)
        {
            RegisterModel(Culture.English, options.ToString(), new Dictionary<Type, IModel>
            {
                [typeof(NumberModel)] = new NumberModel(
                            AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Number, new EnglishNumberParserConfiguration()),
                            English.NumberExtractor.GetInstance(NumberMode.PureNumber)),
                [typeof(OrdinalModel)] = new OrdinalModel(
                            AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Ordinal, new EnglishNumberParserConfiguration()),
                            English.OrdinalExtractor.GetInstance()),
                [typeof(PercentModel)] = new PercentModel(
                            AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Percentage, new EnglishNumberParserConfiguration()),
                            new English.PercentageExtractor())
            });

            RegisterModel(Culture.Chinese, options.ToString(), new Dictionary<Type, IModel>
            {
                [typeof(NumberModel)] = new NumberModel(
                            AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Number, new ChineseNumberParserConfiguration()),
                            new Chinese.NumberExtractor()),
                [typeof(OrdinalModel)] = new OrdinalModel(
                            AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Ordinal, new ChineseNumberParserConfiguration()),
                            new Chinese.OrdinalExtractor()),
                [typeof(PercentModel)] = new PercentModel(
                            AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Percentage, new ChineseNumberParserConfiguration()),
                            new Chinese.PercentageExtractor())
            });

            RegisterModel(Culture.Spanish, options.ToString(), new Dictionary<Type, IModel>
            {
                [typeof(NumberModel)] = new NumberModel(
                            AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Number, new SpanishNumberParserConfiguration()),
                            new Spanish.NumberExtractor(NumberMode.PureNumber)),
                [typeof(OrdinalModel)] = new OrdinalModel(
                            AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Ordinal, new SpanishNumberParserConfiguration()),
                            new Spanish.OrdinalExtractor()),
                [typeof(PercentModel)] = new PercentModel(
                            AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Percentage, new SpanishNumberParserConfiguration()),
                            new Spanish.PercentageExtractor())
            });

            RegisterModel(Culture.Portuguese, options.ToString(), new Dictionary<Type, IModel>
            {
                [typeof(NumberModel)] = new NumberModel(
                            AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Number, new PortugueseNumberParserConfiguration()),
                            new Portuguese.NumberExtractor(NumberMode.PureNumber)),
                [typeof(OrdinalModel)] = new OrdinalModel(
                            AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Ordinal, new PortugueseNumberParserConfiguration()),
                            new Portuguese.OrdinalExtractor()),
                [typeof(PercentModel)] = new PercentModel(
                            AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Percentage, new PortugueseNumberParserConfiguration()),
                            new Portuguese.PercentageExtractor())
            });

            RegisterModel(Culture.French, options.ToString(), new Dictionary<Type, IModel>
            {
                [typeof(NumberModel)] = new NumberModel(
                            AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Number, new FrenchNumberParserConfiguration()),
                            new French.NumberExtractor(NumberMode.PureNumber)),
                [typeof(OrdinalModel)] = new OrdinalModel(
                            AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Ordinal, new FrenchNumberParserConfiguration()),
                            new French.OrdinalExtractor()),
                [typeof(PercentModel)] = new PercentModel(
                            AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Percentage, new FrenchNumberParserConfiguration()),
                            new French.PercentageExtractor())
            });

            RegisterModel(Culture.German, options.ToString(), new Dictionary<Type, IModel>
            {
                [typeof(NumberModel)] = new NumberModel(
                            AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Number, new GermanNumberParserConfiguration()),
                            German.NumberExtractor.GetInstance(NumberMode.PureNumber)),
                [typeof(OrdinalModel)] = new OrdinalModel(
                            AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Ordinal, new GermanNumberParserConfiguration()),
                            German.OrdinalExtractor.GetInstance()),
                [typeof(PercentModel)] = new PercentModel(
                            AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Percentage, new GermanNumberParserConfiguration()),
                            new German.PercentageExtractor())
            });
        }

        public IModel GetNumberModel(string culture, bool fallbackToDefaultCulture = true)
        {
            return GetModel<NumberModel>(culture, fallbackToDefaultCulture, NumberOptions.None.ToString());
        }

        public IModel GetOrdinalModel(string culture, bool fallbackToDefaultCulture = true)
        {
            return GetModel<OrdinalModel>(culture, fallbackToDefaultCulture, NumberOptions.None.ToString());
        }

        public IModel GetPercentageModel(string culture, bool fallbackToDefaultCulture = true)
        {
            return GetModel<PercentModel>(culture, fallbackToDefaultCulture, NumberOptions.None.ToString());
        }
    }
}
