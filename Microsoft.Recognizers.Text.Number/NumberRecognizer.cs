using System;
using System.Collections.Generic;
using Microsoft.Recognizers.Text.Number.Chinese;
using Microsoft.Recognizers.Text.Number.English;
using Microsoft.Recognizers.Text.Number.Spanish;

namespace Microsoft.Recognizers.Text.Number
{
    public class NumberRecognizer : BaseNumberRecognizer
    {
        static NumberRecognizer()
        {
            ModelInstances = new Dictionary<string, Dictionary<Type, IModel>>(StringComparer.InvariantCultureIgnoreCase)
            {
                {
                    Culture.English, new Dictionary<Type, IModel>
                    {
                        [typeof (NumberModel)] = new NumberModel(
                            AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Number, new EnglishNumberParserConfiguration()),
                            new English.NumberExtractor(NumberMode.PureNumber)),
                        [typeof (OrdinalModel)] = new OrdinalModel(
                            AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Ordinal, new EnglishNumberParserConfiguration()),
                            new English.OrdinalExtractor()),
                        [typeof (PercentModel)] = new PercentModel(
                            AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Percentage, new EnglishNumberParserConfiguration()),
                            new English.PercentageExtractor())
                    }
                },
                {
                    Culture.Chinese, new Dictionary<Type, IModel>
                    {
                        [typeof (NumberModel)] = new NumberModel(
                            AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Number, new ChineseNumberParserConfiguration()),
                            new Chinese.NumberExtractor()),
                        [typeof (OrdinalModel)] = new OrdinalModel(
                            AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Ordinal, new ChineseNumberParserConfiguration()),
                            new Chinese.OrdinalExtractor()),
                        [typeof (PercentModel)] = new PercentModel(
                            AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Percentage, new ChineseNumberParserConfiguration()),
                            new Chinese.PercentageExtractor())
                    }
                },
                {
                    Culture.Spanish, new Dictionary<Type, IModel>
                    {
                        [typeof (NumberModel)] = new NumberModel(
                            AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Number, new SpanishNumberParserConfiguration()),
                            new Spanish.NumberExtractor(NumberMode.PureNumber)),
                        [typeof (OrdinalModel)] = new OrdinalModel(
                            AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Ordinal, new SpanishNumberParserConfiguration()),
                            new Spanish.OrdinalExtractor()),
                        [typeof (PercentModel)] = new PercentModel(
                            AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Percentage, new SpanishNumberParserConfiguration()),
                            new Spanish.PercentageExtractor())
                    }
                }
            };
        }

        public static IModel GetNumberModel(string culture)
        {
            return GetModel<NumberModel>(culture);
        }

        public static IModel GetOrdinalModel(string culture)
        {
            return GetModel<OrdinalModel>(culture);
        }

        public static IModel GetPercentageModel(string culture)
        {
            return GetModel<PercentModel>(culture);
        }
    }
}
