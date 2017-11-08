using System;
using System.Collections.Generic;

using Microsoft.Recognizers.Text.Number.Chinese;
using Microsoft.Recognizers.Text.Number.English;
using Microsoft.Recognizers.Text.Number.French;
using Microsoft.Recognizers.Text.Number.Portuguese;
using Microsoft.Recognizers.Text.Number.Spanish;

namespace Microsoft.Recognizers.Text.Number
{
    public class NumberRecognizer : Recognizer
    {
        public static readonly NumberRecognizer Instance = new NumberRecognizer();

        private NumberRecognizer()
        {
            RegisterModel(Culture.English, new Dictionary<Type, IModel>
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

            RegisterModel(Culture.Chinese, new Dictionary<Type, IModel>
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

            RegisterModel(Culture.Spanish, new Dictionary<Type, IModel>
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

            RegisterModel(Culture.Portuguese, new Dictionary<Type, IModel>
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

            RegisterModel(Culture.French, new Dictionary<Type, IModel>
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
        }

        public IModel GetNumberModel(string culture, bool fallbackToDefaultCulture = true)
        {
            return GetModel<NumberModel>(culture, fallbackToDefaultCulture);
        }

        public IModel GetOrdinalModel(string culture, bool fallbackToDefaultCulture = true)
        {
            return GetModel<OrdinalModel>(culture, fallbackToDefaultCulture);
        }

        public IModel GetPercentageModel(string culture, bool fallbackToDefaultCulture = true)
        {
            return GetModel<PercentModel>(culture, fallbackToDefaultCulture);
        }
    }
}
