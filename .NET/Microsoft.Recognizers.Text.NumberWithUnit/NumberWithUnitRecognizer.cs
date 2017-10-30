using System;
using System.Collections.Generic;

namespace Microsoft.Recognizers.Text.NumberWithUnit
{
    public class NumberWithUnitRecognizer : Recognizer 
    {
        public static readonly NumberWithUnitRecognizer Instance = new NumberWithUnitRecognizer();

        private NumberWithUnitRecognizer()
        {
            RegisterModel(Culture.English, new Dictionary<Type, IModel>
            {
                [typeof(CurrencyModel)] = new CurrencyModel(
                            new Dictionary<IExtractor, IParser>
                            {
                                {
                                    new NumberWithUnitExtractor(new English.CurrencyExtractorConfiguration()),
                                    new NumberWithUnitParser(new English.CurrencyParserConfiguration())
                                }
                            }
                            ),
                [typeof(TemperatureModel)] = new TemperatureModel(
                            new Dictionary<IExtractor, IParser>
                            {
                                {
                                    new NumberWithUnitExtractor(new English.TemperatureExtractorConfiguration()),
                                    new NumberWithUnitParser(new English.TemperatureParserConfiguration())
                                }
                            }
                            ),
                [typeof(DimensionModel)] = new DimensionModel(
                            new Dictionary<IExtractor, IParser>
                            {
                                {
                                    new NumberWithUnitExtractor(new English.DimensionExtractorConfiguration()),
                                    new NumberWithUnitParser(new English.DimensionParserConfiguration())
                                }
                            }
                            ),
                [typeof(AgeModel)] = new AgeModel(
                            new Dictionary<IExtractor, IParser>
                            {
                                {
                                    new NumberWithUnitExtractor(new English.AgeExtractorConfiguration()),
                                    new NumberWithUnitParser(new English.AgeParserConfiguration())
                                }
                            }
                            ),
            });

            RegisterModel(Culture.Chinese, new Dictionary<Type, IModel>
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
            });

            RegisterModel(Culture.Spanish, new Dictionary<Type, IModel>
            {
                [typeof(CurrencyModel)] = new CurrencyModel(
                            new Dictionary<IExtractor, IParser>
                            {
                                {
                                    new NumberWithUnitExtractor(new Spanish.CurrencyExtractorConfiguration()),
                                    new NumberWithUnitParser(new Spanish.CurrencyParserConfiguration())
                                }
                            }
                            ),
                [typeof(TemperatureModel)] = new TemperatureModel(
                            new Dictionary<IExtractor, IParser>
                            {
                                {
                                    new NumberWithUnitExtractor(new Spanish.TemperatureExtractorConfiguration()),
                                    new NumberWithUnitParser(new Spanish.TemperatureParserConfiguration())
                                }
                            }
                            ),
                [typeof(DimensionModel)] = new DimensionModel(
                            new Dictionary<IExtractor, IParser>
                            {
                                {
                                    new NumberWithUnitExtractor(new Spanish.DimensionExtractorConfiguration()),
                                    new NumberWithUnitParser(new Spanish.DimensionParserConfiguration())
                                }
                            }
                            ),
                [typeof(AgeModel)] = new AgeModel(
                            new Dictionary<IExtractor, IParser>
                            {
                                {
                                    new NumberWithUnitExtractor(new Spanish.AgeExtractorConfiguration()),
                                    new NumberWithUnitParser(new Spanish.AgeParserConfiguration())
                                }
                            }
                            ),
            });

            RegisterModel(Culture.Portuguese, new Dictionary<Type, IModel>
            {
                [typeof(CurrencyModel)] = new CurrencyModel(
                            new Dictionary<IExtractor, IParser>
                            {
                                {
                                    new NumberWithUnitExtractor(new Portuguese.CurrencyExtractorConfiguration()),
                                    new NumberWithUnitParser(new Portuguese.CurrencyParserConfiguration())
                                }
                            }
                            ),
                [typeof(TemperatureModel)] = new TemperatureModel(
                            new Dictionary<IExtractor, IParser>
                            {
                                {
                                    new NumberWithUnitExtractor(new Portuguese.TemperatureExtractorConfiguration()),
                                    new NumberWithUnitParser(new Portuguese.TemperatureParserConfiguration())
                                }
                            }
                            ),
                [typeof(DimensionModel)] = new DimensionModel(
                            new Dictionary<IExtractor, IParser>
                            {
                                {
                                    new NumberWithUnitExtractor(new Portuguese.DimensionExtractorConfiguration()),
                                    new NumberWithUnitParser(new Portuguese.DimensionParserConfiguration())
                                }
                            }
                            ),
                [typeof(AgeModel)] = new AgeModel(
                            new Dictionary<IExtractor, IParser>
                            {
                                {
                                    new NumberWithUnitExtractor(new Portuguese.AgeExtractorConfiguration()),
                                    new NumberWithUnitParser(new Portuguese.AgeParserConfiguration())
                                }
                            }
                            ),
            });

            RegisterModel(Culture.French, new Dictionary<Type, IModel>
            {
                [typeof(CurrencyModel)] = new CurrencyModel(
                            new Dictionary<IExtractor, IParser>
                            {
                                {
                                    new NumberWithUnitExtractor(new French.CurrencyExtractorConfiguration()),
                                    new NumberWithUnitParser(new French.CurrencyParserConfiguration())
                                }
                            }
                            ),
                [typeof(TemperatureModel)] = new TemperatureModel(
                new Dictionary<IExtractor, IParser>
                            {           
                                {
                                    new NumberWithUnitExtractor(new French.TemperatureExtractorConfiguration()),
                                    new NumberWithUnitParser(new French.TemperatureParserConfiguration())
                                }
                            }
                            ),
                [typeof(DimensionModel)] = new DimensionModel(
                new Dictionary<IExtractor, IParser>
                            {
                                {
                                    new NumberWithUnitExtractor(new French.DimensionExtractorConfiguration()),
                                    new NumberWithUnitParser(new French.DimensionParserConfiguration())
                                }
                            }
                            ),
                [typeof(AgeModel)] = new AgeModel(
                new Dictionary<IExtractor, IParser>
                            {
                                {
                                    new NumberWithUnitExtractor(new French.AgeExtractorConfiguration()),
                                    new NumberWithUnitParser(new French.AgeParserConfiguration())
                                }
                            }
                            ),
            });
        }

        public IModel GetCurrencyModel(string culture, bool fallbackToDefaultCulture = true)
        {
            return GetModel<CurrencyModel>(culture, fallbackToDefaultCulture);
        }

        public IModel GetTemperatureModel(string culture, bool fallbackToDefaultCulture = true)
        {
            return GetModel<TemperatureModel>(culture, fallbackToDefaultCulture);
        }

        public IModel GetDimensionModel(string culture, bool fallbackToDefaultCulture = true)
        {
            return GetModel<DimensionModel>(culture, fallbackToDefaultCulture);
        }

        public IModel GetAgeModel(string culture, bool fallbackToDefaultCulture = true)
        {
            return GetModel<AgeModel>(culture, fallbackToDefaultCulture);
        }
    }
}
