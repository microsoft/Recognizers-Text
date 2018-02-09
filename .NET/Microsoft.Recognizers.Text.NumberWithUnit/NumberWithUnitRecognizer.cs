using System;
using System.Collections.Generic;
using Microsoft.Recognizers.Text.Number;

namespace Microsoft.Recognizers.Text.NumberWithUnit
{
    public class NumberWithUnitRecognizer : Recognizer<NumberWithUnitOptions>
    {
        public NumberWithUnitRecognizer(string culture, NumberWithUnitOptions options = NumberWithUnitOptions.None)
            : base(culture, options)
        {
        }

        public CurrencyModel GetCurrencyModel()
        {
            return GetModel<CurrencyModel>();
        }

        public TemperatureModel GetTemperatureModel()
        {
            return GetModel<TemperatureModel>();
        }

        public DimensionModel GetDimensionModel()
        {
            return GetModel<DimensionModel>();
        }

        public AgeModel GetAgeModel()
        {
            return GetModel<AgeModel>();
        }

        public static List<ModelResult> RecognizeCurrency(string query, string culture, NumberWithUnitOptions options = NumberWithUnitOptions.None)
        {
            return RecognizeByModel(recognizer => recognizer.GetCurrencyModel(), query, culture, options);
        }

        public static List<ModelResult> RecognizeTemperature(string query, string culture, NumberWithUnitOptions options = NumberWithUnitOptions.None)
        {
            return RecognizeByModel(recognizer => recognizer.GetTemperatureModel(), query, culture, options);
        }

        public static List<ModelResult> RecognizeDimension(string query, string culture, NumberWithUnitOptions options = NumberWithUnitOptions.None)
        {
            return RecognizeByModel(recognizer => recognizer.GetDimensionModel(), query, culture, options);
        }

        public static List<ModelResult> RecognizeAge(string query, string culture, NumberWithUnitOptions options = NumberWithUnitOptions.None)
        {
            return RecognizeByModel(recognizer => recognizer.GetAgeModel(), query, culture, options);
        }

        private static List<ModelResult> RecognizeByModel(Func<NumberWithUnitRecognizer, IModel> getModelFunc, string query, string culture, NumberWithUnitOptions options)
        {
            var recognizer = new NumberWithUnitRecognizer(culture, options);
            var model = getModelFunc(recognizer);
            return model.Parse(query);
        }

        protected override void InitializeConfiguration()
        {
            RegisterModel<CurrencyModel>(
                Culture.English,
                (options) => new CurrencyModel(new Dictionary<IExtractor, IParser>
                {
                    {
                        new NumberWithUnitExtractor(new English.CurrencyExtractorConfiguration()),
                        new NumberWithUnitParser(new English.CurrencyParserConfiguration())
                    }
                }));
            RegisterModel<TemperatureModel>(
                Culture.English,
                (options) => new TemperatureModel(new Dictionary<IExtractor, IParser>
                {
                    {
                        new NumberWithUnitExtractor(new English.TemperatureExtractorConfiguration()),
                        new NumberWithUnitParser(new English.TemperatureParserConfiguration())
                    }
                }));
            RegisterModel<DimensionModel>(
                Culture.English,
                (options) => new DimensionModel(new Dictionary<IExtractor, IParser>
                {
                    {
                        new NumberWithUnitExtractor(new English.DimensionExtractorConfiguration()),
                        new NumberWithUnitParser(new English.DimensionParserConfiguration())
                    }
                }));
            RegisterModel<AgeModel>(
                Culture.English,
                (options) => new AgeModel(new Dictionary<IExtractor, IParser>
                {
                    {
                        new NumberWithUnitExtractor(new English.AgeExtractorConfiguration()),
                        new NumberWithUnitParser(new English.AgeParserConfiguration())
                    }
                }));

            RegisterModel<CurrencyModel>(
                Culture.Chinese,
                (options) => new CurrencyModel(new Dictionary<IExtractor, IParser>
                {
                    {
                        new NumberWithUnitExtractor(new Chinese.CurrencyExtractorConfiguration()),
                        new NumberWithUnitParser(new Chinese.CurrencyParserConfiguration())
                    },
                    {
                        new NumberWithUnitExtractor(new English.CurrencyExtractorConfiguration()),
                        new NumberWithUnitParser(new English.CurrencyParserConfiguration())
                    }
                }));
            RegisterModel<TemperatureModel>(
                Culture.Chinese,
                (options) => new TemperatureModel(new Dictionary<IExtractor, IParser>
                {
                    {
                        new NumberWithUnitExtractor(new Chinese.TemperatureExtractorConfiguration()),
                        new NumberWithUnitParser(new Chinese.TemperatureParserConfiguration())
                    },
                    {
                        new NumberWithUnitExtractor(new English.TemperatureExtractorConfiguration()),
                        new NumberWithUnitParser(new English.TemperatureParserConfiguration())
                    }
                }));
            RegisterModel<DimensionModel>(
                Culture.Chinese,
                (options) => new DimensionModel(new Dictionary<IExtractor, IParser>
                {
                    {
                        new NumberWithUnitExtractor(new Chinese.DimensionExtractorConfiguration()),
                        new NumberWithUnitParser(new Chinese.DimensionParserConfiguration())
                    },
                    {
                        new NumberWithUnitExtractor(new English.DimensionExtractorConfiguration()),
                        new NumberWithUnitParser(new English.DimensionParserConfiguration())
                    }
                }));
            RegisterModel<AgeModel>(
                Culture.Chinese,
                (options) => new AgeModel(new Dictionary<IExtractor, IParser>
                {
                    {
                        new NumberWithUnitExtractor(new Chinese.AgeExtractorConfiguration()),
                        new NumberWithUnitParser(new Chinese.AgeParserConfiguration())
                    },
                    {
                        new NumberWithUnitExtractor(new English.AgeExtractorConfiguration()),
                        new NumberWithUnitParser(new English.AgeParserConfiguration())
                    }
                }));

            RegisterModel<CurrencyModel>(
                Culture.Spanish,
                (options) => new CurrencyModel(new Dictionary<IExtractor, IParser>
                {
                    {
                        new NumberWithUnitExtractor(new Spanish.CurrencyExtractorConfiguration()),
                        new NumberWithUnitParser(new Spanish.CurrencyParserConfiguration())
                    }
                }));
            RegisterModel<TemperatureModel>(
                Culture.Spanish,
                (options) => new TemperatureModel(new Dictionary<IExtractor, IParser>
                {
                    {
                        new NumberWithUnitExtractor(new Spanish.TemperatureExtractorConfiguration()),
                        new NumberWithUnitParser(new Spanish.TemperatureParserConfiguration())
                    }
                }));
            RegisterModel<DimensionModel>(
                Culture.Spanish,
                (options) => new DimensionModel(new Dictionary<IExtractor, IParser>
                {
                    {
                        new NumberWithUnitExtractor(new Spanish.DimensionExtractorConfiguration()),
                        new NumberWithUnitParser(new Spanish.DimensionParserConfiguration())
                    }
                }));
            RegisterModel<AgeModel>(
                Culture.Spanish,
                (options) => new AgeModel(new Dictionary<IExtractor, IParser>
                {
                    {
                        new NumberWithUnitExtractor(new Spanish.AgeExtractorConfiguration()),
                        new NumberWithUnitParser(new Spanish.AgeParserConfiguration())
                    }
                }));

            RegisterModel<CurrencyModel>(
                Culture.Portuguese,
                (options) => new CurrencyModel(new Dictionary<IExtractor, IParser>
                {
                    {
                        new NumberWithUnitExtractor(new Portuguese.CurrencyExtractorConfiguration()),
                        new NumberWithUnitParser(new Portuguese.CurrencyParserConfiguration())
                    }
                }));
            RegisterModel<TemperatureModel>(
                Culture.Portuguese,
                (options) => new TemperatureModel(new Dictionary<IExtractor, IParser>
                {
                    {
                        new NumberWithUnitExtractor(new Portuguese.TemperatureExtractorConfiguration()),
                        new NumberWithUnitParser(new Portuguese.TemperatureParserConfiguration())
                    }
                }));
            RegisterModel<DimensionModel>(
                Culture.Portuguese,
                (options) => new DimensionModel(new Dictionary<IExtractor, IParser>
                {
                    {
                        new NumberWithUnitExtractor(new Portuguese.DimensionExtractorConfiguration()),
                        new NumberWithUnitParser(new Portuguese.DimensionParserConfiguration())
                    }
                }));
            RegisterModel<AgeModel>(
                Culture.Portuguese,
                (options) => new AgeModel(new Dictionary<IExtractor, IParser>
                {
                    {
                        new NumberWithUnitExtractor(new Portuguese.AgeExtractorConfiguration()),
                        new NumberWithUnitParser(new Portuguese.AgeParserConfiguration())
                    }
                }));

            RegisterModel<CurrencyModel>(
                Culture.French,
                (options) => new CurrencyModel(new Dictionary<IExtractor, IParser>
                {
                    {
                        new NumberWithUnitExtractor(new French.CurrencyExtractorConfiguration()),
                        new NumberWithUnitParser(new French.CurrencyParserConfiguration())
                    }
                }));
            RegisterModel<TemperatureModel>(
                Culture.French,
                (options) => new TemperatureModel(new Dictionary<IExtractor, IParser>
                {
                    {
                        new NumberWithUnitExtractor(new French.TemperatureExtractorConfiguration()),
                        new NumberWithUnitParser(new French.TemperatureParserConfiguration())
                    }
                }));
            RegisterModel<DimensionModel>(
                Culture.French,
                (options) => new DimensionModel(new Dictionary<IExtractor, IParser>
                {
                    {
                        new NumberWithUnitExtractor(new French.DimensionExtractorConfiguration()),
                        new NumberWithUnitParser(new French.DimensionParserConfiguration())
                    }
                }));
            RegisterModel<AgeModel>(
                Culture.French,
                (options) => new AgeModel(new Dictionary<IExtractor, IParser>
                {
                    {
                        new NumberWithUnitExtractor(new French.AgeExtractorConfiguration()),
                        new NumberWithUnitParser(new French.AgeParserConfiguration())
                    }
                }));


            RegisterModel<CurrencyModel>(
                Culture.German,
                (options) => new CurrencyModel(
                             new Dictionary<IExtractor, IParser>
                             {
                                {
                                    new NumberWithUnitExtractor(new German.CurrencyExtractorConfiguration()),
                                    new NumberWithUnitParser(new German.CurrencyParserConfiguration())
                                }
                             }));
            RegisterModel<TemperatureModel>(
                Culture.German,
                (options) => new TemperatureModel(
                             new Dictionary<IExtractor, IParser>
                             {
                                {
                                    new NumberWithUnitExtractor(new German.TemperatureExtractorConfiguration()),
                                    new NumberWithUnitParser(new German.TemperatureParserConfiguration())
                                }
                             }));
            RegisterModel<DimensionModel>(
                Culture.German,
                (options) => new DimensionModel(
                             new Dictionary<IExtractor, IParser>
                             {
                                {
                                    new NumberWithUnitExtractor(new German.DimensionExtractorConfiguration()),
                                    new NumberWithUnitParser(new German.DimensionParserConfiguration())
                                }
                             }));
            RegisterModel<AgeModel>(
                Culture.German,
                (options) => new AgeModel(
                            new Dictionary<IExtractor, IParser>
                            {
                                {
                                    new NumberWithUnitExtractor(new German.AgeExtractorConfiguration()),
                                    new NumberWithUnitParser(new German.AgeParserConfiguration())
                                }
                            }));
        }
    }
}
