using System;
using System.Collections.Generic;
using Microsoft.Recognizers.Text.Number;

namespace Microsoft.Recognizers.Text.NumberWithUnit
{
    public class NumberWithUnitRecognizer : BaseNumberRecognizer
    {
        static NumberWithUnitRecognizer()
        {
            ModelInstances = new Dictionary<string, Dictionary<Type, IModel>>(StringComparer.InvariantCultureIgnoreCase)
            {
                {
                    Culture.English, new Dictionary<Type, IModel>
                    {
                        [typeof (CurrencyModel)] = new CurrencyModel(
                            new Dictionary<IExtractor, IParser>
                            {
                                {
                                    new NumberWithUnitExtractor(new English.CurrencyExtractorConfiguration()),
                                    new NumberWithUnitParser(new English.CurrencyParserConfiguration())
                                }
                            }
                            ),
                        [typeof (TemperatureModel)] = new TemperatureModel(
                            new Dictionary<IExtractor, IParser>
                            {
                                {
                                    new NumberWithUnitExtractor(new English.TemperatureExtractorConfiguration()),
                                    new NumberWithUnitParser(new English.TemperatureParserConfiguration())
                                }
                            }
                            ),
                        [typeof (DimensionModel)] = new DimensionModel(
                            new Dictionary<IExtractor, IParser>
                            {
                                {
                                    new NumberWithUnitExtractor(new English.DimensionExtractorConfiguration()),
                                    new NumberWithUnitParser(new English.DimensionParserConfiguration())
                                }
                            }
                            ),
                        [typeof (AgeModel)] = new AgeModel(
                            new Dictionary<IExtractor, IParser>
                            {
                                {
                                    new NumberWithUnitExtractor(new English.AgeExtractorConfiguration()),
                                    new NumberWithUnitParser(new English.AgeParserConfiguration())
                                }
                            }
                            ),
                    }
                },
                {
                    Culture.Chinese, new Dictionary<Type, IModel>
                    {
                        [typeof (CurrencyModel)] = new CurrencyModel(
                            new Dictionary<IExtractor, IParser>
                            {
                                {
                                    new NumberWithUnitExtractor(new Chinese.CurrencyExtractorConfiguration()),
                                    new NumberWithUnitParser(new Chinese.CurrencyParserConfiguration())
                                },
                                {
                                    new NumberWithUnitExtractor(new English.CurrencyExtractorConfiguration()),
                                    new NumberWithUnitParser(new English.CurrencyParserConfiguration())
                                }
                            }
                            ),
                        [typeof (TemperatureModel)] = new TemperatureModel(
                            new Dictionary<IExtractor, IParser>
                            {
                                {
                                    new NumberWithUnitExtractor(new Chinese.TemperatureExtractorConfiguration()),
                                    new NumberWithUnitParser(new Chinese.TemperatureParserConfiguration())
                                },
                                {
                                    new NumberWithUnitExtractor(new English.TemperatureExtractorConfiguration()),
                                    new NumberWithUnitParser(new English.TemperatureParserConfiguration())
                                }
                            }
                            ),
                        [typeof (DimensionModel)] = new DimensionModel(
                            new Dictionary<IExtractor, IParser>
                            {
                                {
                                    new NumberWithUnitExtractor(new Chinese.DimensionExtractorConfiguration()),
                                    new NumberWithUnitParser(new Chinese.DimensionParserConfiguration())
                                },
                                {
                                    new NumberWithUnitExtractor(new English.DimensionExtractorConfiguration()),
                                    new NumberWithUnitParser(new English.DimensionParserConfiguration())
                                }
                            }
                            ),
                        [typeof (AgeModel)] = new AgeModel(
                            new Dictionary<IExtractor, IParser>
                            {
                                {
                                    new NumberWithUnitExtractor(new Chinese.AgeExtractorConfiguration()),
                                    new NumberWithUnitParser(new Chinese.AgeParserConfiguration())
                                },
                                {
                                    new NumberWithUnitExtractor(new English.AgeExtractorConfiguration()),
                                    new NumberWithUnitParser(new English.AgeParserConfiguration())
                                }
                            }
                            ),
                    }
                },
                {
                    Culture.Spanish, new Dictionary<Type, IModel>
                    {
                        [typeof (CurrencyModel)] = new CurrencyModel(
                            new Dictionary<IExtractor, IParser>
                            {
                                {
                                    new NumberWithUnitExtractor(new Spanish.CurrencyExtractorConfiguration()),
                                    new NumberWithUnitParser(new Spanish.CurrencyParserConfiguration())
                                }
                            }
                            ),
                        [typeof (TemperatureModel)] = new TemperatureModel(
                            new Dictionary<IExtractor, IParser>
                            {
                                {
                                    new NumberWithUnitExtractor(new Spanish.TemperatureExtractorConfiguration()),
                                    new NumberWithUnitParser(new Spanish.TemperatureParserConfiguration())
                                }
                            }
                            ),
                        [typeof (DimensionModel)] = new DimensionModel(
                            new Dictionary<IExtractor, IParser>
                            {
                                {
                                    new NumberWithUnitExtractor(new Spanish.DimensionExtractorConfiguration()),
                                    new NumberWithUnitParser(new Spanish.DimensionParserConfiguration())
                                }
                            }
                            ),
                        [typeof (AgeModel)] = new AgeModel(
                            new Dictionary<IExtractor, IParser>
                            {
                                {
                                    new NumberWithUnitExtractor(new Spanish.AgeExtractorConfiguration()),
                                    new NumberWithUnitParser(new Spanish.AgeParserConfiguration())
                                }
                            }
                            ),
                    }
                },
                {
                    Culture.Portuguese, new Dictionary<Type, IModel>
                    {
                        [typeof (CurrencyModel)] = new CurrencyModel(
                            new Dictionary<IExtractor, IParser>
                            {
                                {
                                    new NumberWithUnitExtractor(new Portuguese.CurrencyExtractorConfiguration()),
                                    new NumberWithUnitParser(new Portuguese.CurrencyParserConfiguration())
                                }
                            }
                            ),
                        [typeof (TemperatureModel)] = new TemperatureModel(
                            new Dictionary<IExtractor, IParser>
                            {
                                {
                                    new NumberWithUnitExtractor(new Portuguese.TemperatureExtractorConfiguration()),
                                    new NumberWithUnitParser(new Portuguese.TemperatureParserConfiguration())
                                }
                            }
                            ),
                        [typeof (DimensionModel)] = new DimensionModel(
                            new Dictionary<IExtractor, IParser>
                            {
                                {
                                    new NumberWithUnitExtractor(new Portuguese.DimensionExtractorConfiguration()),
                                    new NumberWithUnitParser(new Portuguese.DimensionParserConfiguration())
                                }
                            }
                            ),
                        [typeof (AgeModel)] = new AgeModel(
                            new Dictionary<IExtractor, IParser>
                            {
                                {
                                    new NumberWithUnitExtractor(new Portuguese.AgeExtractorConfiguration()),
                                    new NumberWithUnitParser(new Portuguese.AgeParserConfiguration())
                                }
                            }
                            ),
                    }
                }
            };
        }

        public static IModel GetCurrencyModel(string culture, bool fallbackToDefaultCulture = true)
        {
            return GetModel<CurrencyModel>(culture, fallbackToDefaultCulture);
        }

        public static IModel GetTemperatureModel(string culture, bool fallbackToDefaultCulture = true)
        {
            return GetModel<TemperatureModel>(culture, fallbackToDefaultCulture);
        }

        public static IModel GetDimensionModel(string culture, bool fallbackToDefaultCulture = true)
        {
            return GetModel<DimensionModel>(culture, fallbackToDefaultCulture);
        }

        public static IModel GetAgeModel(string culture, bool fallbackToDefaultCulture = true)
        {
            return GetModel<AgeModel>(culture, fallbackToDefaultCulture);
        }
    }
}
