using System;
using System.Collections.Generic;

namespace Microsoft.Recognizers.Text.NumberWithUnit
{
    public class NumberWithUnitRecognizer : Recognizer<NumberWithUnitOptions>
    {
        public NumberWithUnitRecognizer(string targetCulture, NumberWithUnitOptions options = NumberWithUnitOptions.None, bool lazyInitialization = false)
            : base(targetCulture, options, lazyInitialization)
        {
        }

        public NumberWithUnitRecognizer(string targetCulture, int options, bool lazyInitialization = false)
            : this(targetCulture, GetOptions(options), lazyInitialization)
        {
        }

        public NumberWithUnitRecognizer(NumberWithUnitOptions options = NumberWithUnitOptions.None, bool lazyInitialization = true)
            : this(null, options, lazyInitialization)
        {
        }

        public NumberWithUnitRecognizer(int options, bool lazyInitialization = true)
            : this(null, GetOptions(options), lazyInitialization)
        {
        }

        public CurrencyModel GetCurrencyModel(string culture = null, bool fallbackToDefaultCulture = true)
        {
            return GetModel<CurrencyModel>(culture, fallbackToDefaultCulture);
        }

        public TemperatureModel GetTemperatureModel(string culture = null, bool fallbackToDefaultCulture = true)
        {
            return GetModel<TemperatureModel>(culture, fallbackToDefaultCulture);
        }

        public DimensionModel GetDimensionModel(string culture = null, bool fallbackToDefaultCulture = true)
        {
            return GetModel<DimensionModel>(culture, fallbackToDefaultCulture);
        }

        public AgeModel GetAgeModel(string culture = null, bool fallbackToDefaultCulture = true)
        {
            return GetModel<AgeModel>(culture, fallbackToDefaultCulture);
        }

        public static List<ModelResult> RecognizeCurrency(string query, string culture, NumberWithUnitOptions options = NumberWithUnitOptions.None, bool fallbackToDefaultCulture = true)
        {
            return RecognizeByModel(recognizer => recognizer.GetCurrencyModel(culture, fallbackToDefaultCulture), query, options);
        }

        public static List<ModelResult> RecognizeTemperature(string query, string culture, NumberWithUnitOptions options = NumberWithUnitOptions.None, bool fallbackToDefaultCulture = true)
        {
            return RecognizeByModel(recognizer => recognizer.GetTemperatureModel(culture, fallbackToDefaultCulture), query, options);
        }

        public static List<ModelResult> RecognizeDimension(string query, string culture, NumberWithUnitOptions options = NumberWithUnitOptions.None, bool fallbackToDefaultCulture = true)
        {
            return RecognizeByModel(recognizer => recognizer.GetDimensionModel(culture, fallbackToDefaultCulture), query, options);
        }

        public static List<ModelResult> RecognizeAge(string query, string culture, NumberWithUnitOptions options = NumberWithUnitOptions.None, bool fallbackToDefaultCulture = true)
        {
            return RecognizeByModel(recognizer => recognizer.GetAgeModel(culture, fallbackToDefaultCulture), query, options);
        }

        private static List<ModelResult> RecognizeByModel(Func<NumberWithUnitRecognizer, IModel> getModelFunc, string query, NumberWithUnitOptions options)
        {
            var recognizer = new NumberWithUnitRecognizer(options);
            var model = getModelFunc(recognizer);
            return model.Parse(query);
        }

        protected override void InitializeConfiguration()
        {
            #region English
            RegisterModel<CurrencyModel>(
                Culture.English,
                (options) => new CurrencyModel(new Dictionary<IExtractor, IParser>
                {
                    {
                        new BaseMergedUnitExtractor(new English.CurrencyExtractorConfiguration()),
                        new BaseMergedUnitParser(new English.CurrencyParserConfiguration())
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
            #endregion

            #region Chinese
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
            #endregion

            #region Spanish
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
            #endregion

            #region Portuguese
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
            #endregion

            #region French
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
            #endregion

            #region German
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
            #endregion
        }
    }
}