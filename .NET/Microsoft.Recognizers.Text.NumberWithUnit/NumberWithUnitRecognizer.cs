﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

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

        protected override void InitializeConfiguration()
        {
            RegisterModel<CurrencyModel>(
                Culture.Chinese,
                (options) => new CurrencyModel(new Dictionary<IExtractor, IParser>
                {
                    {
                        new BaseMergedUnitExtractor(new Chinese.CurrencyExtractorConfiguration()),
                        new BaseMergedUnitParser(new Chinese.CurrencyParserConfiguration())
                    },
                    {
                        new NumberWithUnitExtractor(new English.CurrencyExtractorConfiguration()),
                        new NumberWithUnitParser(new English.CurrencyParserConfiguration())
                    },
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
                    },
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
                    },
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
                    },
                }));

            RegisterModel<CurrencyModel>(
                Culture.Dutch,
                (options) => new CurrencyModel(new Dictionary<IExtractor, IParser>
                {
                    {
                        new BaseMergedUnitExtractor(new Dutch.CurrencyExtractorConfiguration()),
                        new BaseMergedUnitParser(new Dutch.CurrencyParserConfiguration())
                    },
                }));

            RegisterModel<TemperatureModel>(
                Culture.Dutch,
                (options) => new TemperatureModel(new Dictionary<IExtractor, IParser>
                {
                    {
                        new NumberWithUnitExtractor(new Dutch.TemperatureExtractorConfiguration()),
                        new NumberWithUnitParser(new Dutch.TemperatureParserConfiguration())
                    },
                }));

            RegisterModel<DimensionModel>(
                Culture.Dutch,
                (options) => new DimensionModel(new Dictionary<IExtractor, IParser>
                {
                    {
                        new NumberWithUnitExtractor(new Dutch.DimensionExtractorConfiguration()),
                        new NumberWithUnitParser(new Dutch.DimensionParserConfiguration())
                    },
                }));

            RegisterModel<AgeModel>(
                Culture.Dutch,
                (options) => new AgeModel(new Dictionary<IExtractor, IParser>
                {
                    {
                        new NumberWithUnitExtractor(new Dutch.AgeExtractorConfiguration()),
                        new NumberWithUnitParser(new Dutch.AgeParserConfiguration())
                    },
                }));

            RegisterModel<CurrencyModel>(
                Culture.English,
                (options) => new CurrencyModel(new Dictionary<IExtractor, IParser>
                {
                    {
                        new BaseMergedUnitExtractor(new English.CurrencyExtractorConfiguration()),
                        new BaseMergedUnitParser(new English.CurrencyParserConfiguration())
                    },
                }));

            RegisterModel<TemperatureModel>(
                Culture.English,
                (options) => new TemperatureModel(new Dictionary<IExtractor, IParser>
                {
                    {
                        new NumberWithUnitExtractor(new English.TemperatureExtractorConfiguration()),
                        new NumberWithUnitParser(new English.TemperatureParserConfiguration())
                    },
                }));

            RegisterModel<DimensionModel>(
                Culture.English,
                (options) => new DimensionModel(new Dictionary<IExtractor, IParser>
                {
                    {
                        new NumberWithUnitExtractor(new English.DimensionExtractorConfiguration()),
                        new NumberWithUnitParser(new English.DimensionParserConfiguration())
                    },
                }));

            RegisterModel<AgeModel>(
                Culture.English,
                (options) => new AgeModel(new Dictionary<IExtractor, IParser>
                {
                    {
                        new NumberWithUnitExtractor(new English.AgeExtractorConfiguration()),
                        new NumberWithUnitParser(new English.AgeParserConfiguration())
                    },
                }));

            RegisterModel<CurrencyModel>(
                Culture.French,
                (options) => new CurrencyModel(new Dictionary<IExtractor, IParser>
                {
                    {
                        new BaseMergedUnitExtractor(new French.CurrencyExtractorConfiguration()),
                        new BaseMergedUnitParser(new French.CurrencyParserConfiguration())
                    },
                }));

            RegisterModel<TemperatureModel>(
                Culture.French,
                (options) => new TemperatureModel(new Dictionary<IExtractor, IParser>
                {
                    {
                        new NumberWithUnitExtractor(new French.TemperatureExtractorConfiguration()),
                        new NumberWithUnitParser(new French.TemperatureParserConfiguration())
                    },
                }));

            RegisterModel<DimensionModel>(
                Culture.French,
                (options) => new DimensionModel(new Dictionary<IExtractor, IParser>
                {
                    {
                        new NumberWithUnitExtractor(new French.DimensionExtractorConfiguration()),
                        new NumberWithUnitParser(new French.DimensionParserConfiguration())
                    },
                }));

            RegisterModel<AgeModel>(
                Culture.French,
                (options) => new AgeModel(new Dictionary<IExtractor, IParser>
                {
                    {
                        new NumberWithUnitExtractor(new French.AgeExtractorConfiguration()),
                        new NumberWithUnitParser(new French.AgeParserConfiguration())
                    },
                }));

            RegisterModel<CurrencyModel>(
                Culture.German,
                (options) => new CurrencyModel(new Dictionary<IExtractor, IParser>
                 {
                    {
                        new BaseMergedUnitExtractor(new German.CurrencyExtractorConfiguration()),
                        new BaseMergedUnitParser(new German.CurrencyParserConfiguration())
                    },
                 }));

            RegisterModel<TemperatureModel>(
                Culture.German,
                (options) => new TemperatureModel(new Dictionary<IExtractor, IParser>
                 {
                    {
                        new NumberWithUnitExtractor(new German.TemperatureExtractorConfiguration()),
                        new NumberWithUnitParser(new German.TemperatureParserConfiguration())
                    },
                 }));

            RegisterModel<DimensionModel>(
                Culture.German,
                (options) => new DimensionModel(new Dictionary<IExtractor, IParser>
                 {
                    {
                        new NumberWithUnitExtractor(new German.DimensionExtractorConfiguration()),
                        new NumberWithUnitParser(new German.DimensionParserConfiguration())
                    },
                 }));

            RegisterModel<AgeModel>(
                Culture.German,
                (options) => new AgeModel(new Dictionary<IExtractor, IParser>
                {
                    {
                        new NumberWithUnitExtractor(new German.AgeExtractorConfiguration()),
                        new NumberWithUnitParser(new German.AgeParserConfiguration())
                    },
                }));

            RegisterModel<AgeModel>(
                Culture.Hindi,
                (options) => new AgeModel(new Dictionary<IExtractor, IParser>
                {
                    {
                        new NumberWithUnitExtractor(new Hindi.AgeExtractorConfiguration()),
                        new NumberWithUnitParser(new Hindi.AgeParserConfiguration())
                    },
                }));

            RegisterModel<TemperatureModel>(
               Culture.Hindi,
               (options) => new TemperatureModel(new Dictionary<IExtractor, IParser>
               {
                    {
                        new NumberWithUnitExtractor(new Hindi.TemperatureExtractorConfiguration()),
                        new NumberWithUnitParser(new Hindi.TemperatureParserConfiguration())
                    },
               }));

            RegisterModel<DimensionModel>(
                Culture.Hindi,
                (options) => new DimensionModel(new Dictionary<IExtractor, IParser>
                {
                    {
                        new NumberWithUnitExtractor(new Hindi.DimensionExtractorConfiguration()),
                        new NumberWithUnitParser(new Hindi.DimensionParserConfiguration())
                    },
                }));

            RegisterModel<CurrencyModel>(
                Culture.Hindi,
                (options) => new CurrencyModel(new Dictionary<IExtractor, IParser>
                {
                    {
                        new BaseMergedUnitExtractor(new Hindi.CurrencyExtractorConfiguration()),
                        new BaseMergedUnitParser(new Hindi.CurrencyParserConfiguration())
                    },
                }));

            RegisterModel<CurrencyModel>(
                Culture.Italian,
                (options) => new CurrencyModel(new Dictionary<IExtractor, IParser>
                {
                    {
                        new BaseMergedUnitExtractor(new Italian.CurrencyExtractorConfiguration()),
                        new BaseMergedUnitParser(new Italian.CurrencyParserConfiguration())
                    },
                }));

            RegisterModel<TemperatureModel>(
                Culture.Italian,
                (options) => new TemperatureModel(new Dictionary<IExtractor, IParser>
                {
                    {
                        new NumberWithUnitExtractor(new Italian.TemperatureExtractorConfiguration()),
                        new NumberWithUnitParser(new Italian.TemperatureParserConfiguration())
                    },
                }));

            RegisterModel<DimensionModel>(
                Culture.Italian,
                (options) => new DimensionModel(new Dictionary<IExtractor, IParser>
                {
                    {
                        new NumberWithUnitExtractor(new Italian.DimensionExtractorConfiguration()),
                        new NumberWithUnitParser(new Italian.DimensionParserConfiguration())
                    },
                }));

            RegisterModel<AgeModel>(
                Culture.Italian,
                (options) => new AgeModel(new Dictionary<IExtractor, IParser>
                {
                    {
                        new NumberWithUnitExtractor(new Italian.AgeExtractorConfiguration()),
                        new NumberWithUnitParser(new Italian.AgeParserConfiguration())
                    },
                }));

            RegisterModel<CurrencyModel>(
                Culture.Japanese,
                (options) => new CurrencyModel(new Dictionary<IExtractor, IParser>
                {
                    {
                        new BaseMergedUnitExtractor(new Japanese.CurrencyExtractorConfiguration()),
                        new BaseMergedUnitParser(new Japanese.CurrencyParserConfiguration())
                    }, /*
                    {
                        new NumberWithUnitExtractor(new English.CurrencyExtractorConfiguration()),
                        new NumberWithUnitParser(new English.CurrencyParserConfiguration())
                    }*/
                }));

            RegisterModel<AgeModel>(
                Culture.Japanese,
                (options) => new AgeModel(new Dictionary<IExtractor, IParser>
                {
                    {
                        new NumberWithUnitExtractor(new Japanese.AgeExtractorConfiguration()),
                        new NumberWithUnitParser(new Japanese.AgeParserConfiguration())
                    },
                    {
                        new NumberWithUnitExtractor(new English.AgeExtractorConfiguration()),
                        new NumberWithUnitParser(new English.AgeParserConfiguration())
                    },
                }));

            RegisterModel<CurrencyModel>(
                Culture.Korean,
                (options) => new CurrencyModel(new Dictionary<IExtractor, IParser>
                {
                    {
                        new BaseMergedUnitExtractor(new Korean.CurrencyExtractorConfiguration()),
                        new BaseMergedUnitParser(new Korean.CurrencyParserConfiguration())
                    },
                    {
                        new NumberWithUnitExtractor(new English.CurrencyExtractorConfiguration()),
                        new NumberWithUnitParser(new English.CurrencyParserConfiguration())
                    },
                }));

            RegisterModel<TemperatureModel>(
                Culture.Korean,
                (options) => new TemperatureModel(new Dictionary<IExtractor, IParser>
                {
                    {
                        new NumberWithUnitExtractor(new Korean.TemperatureExtractorConfiguration()),
                        new NumberWithUnitParser(new Korean.TemperatureParserConfiguration())
                    },
                    {
                        new NumberWithUnitExtractor(new English.TemperatureExtractorConfiguration()),
                        new NumberWithUnitParser(new English.TemperatureParserConfiguration())
                    },
                }));

            RegisterModel<DimensionModel>(
                Culture.Korean,
                (options) => new DimensionModel(new Dictionary<IExtractor, IParser>
                {
                    {
                        new NumberWithUnitExtractor(new Korean.DimensionExtractorConfiguration()),
                        new NumberWithUnitParser(new Korean.DimensionParserConfiguration())
                    },
                    {
                        new NumberWithUnitExtractor(new English.DimensionExtractorConfiguration()),
                        new NumberWithUnitParser(new English.DimensionParserConfiguration())
                    },
                }));

            RegisterModel<AgeModel>(
                Culture.Korean,
                (options) => new AgeModel(new Dictionary<IExtractor, IParser>
                {
                    {
                        new NumberWithUnitExtractor(new Korean.AgeExtractorConfiguration()),
                        new NumberWithUnitParser(new Korean.AgeParserConfiguration())
                    },
                    {
                        new NumberWithUnitExtractor(new English.AgeExtractorConfiguration()),
                        new NumberWithUnitParser(new English.AgeParserConfiguration())
                    },
                }));

            RegisterModel<CurrencyModel>(
                Culture.Portuguese,
                (options) => new CurrencyModel(new Dictionary<IExtractor, IParser>
                {
                    {
                        new BaseMergedUnitExtractor(new Portuguese.CurrencyExtractorConfiguration()),
                        new BaseMergedUnitParser(new Portuguese.CurrencyParserConfiguration())
                    },
                }));

            RegisterModel<TemperatureModel>(
                Culture.Portuguese,
                (options) => new TemperatureModel(new Dictionary<IExtractor, IParser>
                {
                    {
                        new NumberWithUnitExtractor(new Portuguese.TemperatureExtractorConfiguration()),
                        new NumberWithUnitParser(new Portuguese.TemperatureParserConfiguration())
                    },
                }));

            RegisterModel<DimensionModel>(
                Culture.Portuguese,
                (options) => new DimensionModel(new Dictionary<IExtractor, IParser>
                {
                    {
                        new NumberWithUnitExtractor(new Portuguese.DimensionExtractorConfiguration()),
                        new NumberWithUnitParser(new Portuguese.DimensionParserConfiguration())
                    },
                }));

            RegisterModel<AgeModel>(
                Culture.Portuguese,
                (options) => new AgeModel(new Dictionary<IExtractor, IParser>
                {
                    {
                        new NumberWithUnitExtractor(new Portuguese.AgeExtractorConfiguration()),
                        new NumberWithUnitParser(new Portuguese.AgeParserConfiguration())
                    },
                }));

            RegisterModel<CurrencyModel>(
                Culture.Spanish,
                (options) => new CurrencyModel(new Dictionary<IExtractor, IParser>
                {
                    {
                        new BaseMergedUnitExtractor(new Spanish.CurrencyExtractorConfiguration()),
                        new BaseMergedUnitParser(new Spanish.CurrencyParserConfiguration())
                    },
                }));

            RegisterModel<TemperatureModel>(
                Culture.Spanish,
                (options) => new TemperatureModel(new Dictionary<IExtractor, IParser>
                {
                    {
                        new NumberWithUnitExtractor(new Spanish.TemperatureExtractorConfiguration()),
                        new NumberWithUnitParser(new Spanish.TemperatureParserConfiguration())
                    },
                }));

            RegisterModel<DimensionModel>(
                Culture.Spanish,
                (options) => new DimensionModel(new Dictionary<IExtractor, IParser>
                {
                    {
                        new NumberWithUnitExtractor(new Spanish.DimensionExtractorConfiguration()),
                        new NumberWithUnitParser(new Spanish.DimensionParserConfiguration())
                    },
                }));

            RegisterModel<AgeModel>(
                Culture.Spanish,
                (options) => new AgeModel(new Dictionary<IExtractor, IParser>
                {
                    {
                        new NumberWithUnitExtractor(new Spanish.AgeExtractorConfiguration()),
                        new NumberWithUnitParser(new Spanish.AgeParserConfiguration())
                    },
                }));

            RegisterModel<CurrencyModel>(
                Culture.SpanishMexican,
                (options) => new CurrencyModel(new Dictionary<IExtractor, IParser>
                {
                    {
                        new BaseMergedUnitExtractor(new Spanish.CurrencyExtractorConfiguration()),
                        new BaseMergedUnitParser(new Spanish.CurrencyParserConfiguration())
                    },
                }));

            RegisterModel<TemperatureModel>(
                Culture.SpanishMexican,
                (options) => new TemperatureModel(new Dictionary<IExtractor, IParser>
                {
                    {
                        new NumberWithUnitExtractor(new Spanish.TemperatureExtractorConfiguration()),
                        new NumberWithUnitParser(new Spanish.TemperatureParserConfiguration())
                    },
                }));

            RegisterModel<DimensionModel>(
                Culture.SpanishMexican,
                (options) => new DimensionModel(new Dictionary<IExtractor, IParser>
                {
                    {
                        new NumberWithUnitExtractor(new Spanish.DimensionExtractorConfiguration()),
                        new NumberWithUnitParser(new Spanish.DimensionParserConfiguration())
                    },
                }));

            RegisterModel<AgeModel>(
                Culture.SpanishMexican,
                (options) => new AgeModel(new Dictionary<IExtractor, IParser>
                {
                    {
                        new NumberWithUnitExtractor(new Spanish.AgeExtractorConfiguration()),
                        new NumberWithUnitParser(new Spanish.AgeParserConfiguration())
                    },
                }));

            RegisterModel<AgeModel>(
                Culture.Swedish,
                (options) => new AgeModel(new Dictionary<IExtractor, IParser>
                {
                    {
                        new NumberWithUnitExtractor(new Swedish.AgeExtractorConfiguration()),
                        new NumberWithUnitParser(new Swedish.AgeParserConfiguration())
                    },
                }));

            RegisterModel<TemperatureModel>(
                Culture.Swedish,
                (options) => new TemperatureModel(new Dictionary<IExtractor, IParser>
                {
                    {
                        new NumberWithUnitExtractor(new Swedish.TemperatureExtractorConfiguration()),
                        new NumberWithUnitParser(new Swedish.TemperatureParserConfiguration())
                    },
                }));

            RegisterModel<DimensionModel>(
                Culture.Swedish,
                (options) => new DimensionModel(new Dictionary<IExtractor, IParser>
                {
                    {
                        new NumberWithUnitExtractor(new Swedish.DimensionExtractorConfiguration()),
                        new NumberWithUnitParser(new Swedish.DimensionParserConfiguration())
                    },
                }));

            RegisterModel<CurrencyModel>(
                Culture.Swedish,
                (options) => new CurrencyModel(new Dictionary<IExtractor, IParser>
                {
                    {
                        new BaseMergedUnitExtractor(new Swedish.CurrencyExtractorConfiguration()),
                        new BaseMergedUnitParser(new Swedish.CurrencyParserConfiguration())
                    },
                }));

            RegisterModel<AgeModel>(
                Culture.Turkish,
                (options) => new AgeModel(new Dictionary<IExtractor, IParser>
                {
                    {
                        new NumberWithUnitExtractor(new Turkish.AgeExtractorConfiguration()),
                        new NumberWithUnitParser(new Turkish.AgeParserConfiguration())
                    },
                }));

            RegisterModel<TemperatureModel>(
                Culture.Turkish,
                (options) => new TemperatureModel(new Dictionary<IExtractor, IParser>
                {
                    {
                        new NumberWithUnitExtractor(new Turkish.TemperatureExtractorConfiguration()),
                        new NumberWithUnitParser(new Turkish.TemperatureParserConfiguration())
                    },
                }));

            RegisterModel<DimensionModel>(
                Culture.Turkish,
                (options) => new DimensionModel(new Dictionary<IExtractor, IParser>
                {
                    {
                        new NumberWithUnitExtractor(new Turkish.DimensionExtractorConfiguration()),
                        new NumberWithUnitParser(new Turkish.DimensionParserConfiguration())
                    },
                }));

            RegisterModel<CurrencyModel>(
                Culture.Turkish,
                (options) => new CurrencyModel(new Dictionary<IExtractor, IParser>
                {
                    {
                        new BaseMergedUnitExtractor(new Turkish.CurrencyExtractorConfiguration()),
                        new BaseMergedUnitParser(new Turkish.CurrencyParserConfiguration())
                    },
                }));
        }

        private static List<ModelResult> RecognizeByModel(Func<NumberWithUnitRecognizer, IModel> getModelFunc, string query, NumberWithUnitOptions options)
        {
            var recognizer = new NumberWithUnitRecognizer(options);
            var model = getModelFunc(recognizer);
            return model.Parse(query);
        }
    }
}