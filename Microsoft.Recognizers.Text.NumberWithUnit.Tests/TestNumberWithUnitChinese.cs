using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Tests
{
    [TestClass]
    public class TestNumberWithUnitChinese
    {
        private void BasicTest(IModel model, string source, string value)
        {
            var resultStr = model.Parse(source);
            var resultJson = resultStr;
            Assert.AreEqual(1, resultJson.Count);
            Assert.AreEqual(value, resultJson.First().Resolution["value"] + " " + resultJson.First().Resolution["unit"]);
        }

        private void BasicTest(IModel model, string source, string[] values)
        {
            var results = model.Parse(source);
            Assert.AreEqual(values.Length, results.Count);
            var resultsValues = results.Select(x => GetStringValue(x)).ToArray();
            CollectionAssert.AreEqual(values, resultsValues);
        }

        private string GetStringValue(ModelResult source)
        {
            object value, unit;
            source.Resolution.TryGetValue(nameof(value), out value);
            source.Resolution.TryGetValue(nameof(unit), out unit);
            return $"{value} {unit}".Trim();
        }

        [TestMethod]
        public void TestCurrency()
        {
            var model = GetCurrencyModel();

            BasicTest(model,
                "江苏彩民15元中大乐透1600万 奖池36.57亿",
                "15 Chinese yuan");

            BasicTest(model,
                "其中，四川彩友中得1注1000万元基本头奖；",
                "10000000 Chinese yuan");

            BasicTest(model,
                "本期开奖结束后，奖池金额攀升至36.57亿元。",
                "3657000000 Chinese yuan");

            BasicTest(model,
                "１欧元可以",
                "1 Euro");

            BasicTest(model,
                "兑换１．０７５３美元",
                "1.0753 United States dollar");

            BasicTest(model,
                "兑换1.0092瑞士法郎",
                "1.0092 Swiss franc");

            BasicTest(model,
                "2016年由于并购等直接投资，中国资金净流出1200亿美元",
                "120000000000 United States dollar");

            BasicTest(model,
                "宝安科技公司与国际精密的15位股东签署收购协议，以每股1.95港元",
                "1.95 Hong Kong dollar");

            BasicTest(model,
                "央行到期定存单5306亿台币",
                "530600000000 New Taiwan dollar");

            BasicTest(model,
                "东芝重组另需1万亿日元 已向交易银行申请贷款",
                "1000000000000 Japanese yen");

            BasicTest(model,
                "达到445奈拉兑换",
                "445 Nigerian naira");

            BasicTest(model,
                "十五美元",
                "15 United States dollar");

            BasicTest(model,
                "十美元",
                "10 United States dollar");
        }

        [TestMethod]
        public void TestDimension()
        {
            var model = GetDimensionModel();

            BasicTest(model,
                "去年，潜江虾稻产业综合产值突破180亿元，带动就业超10万人，龙虾养殖户户平增收16000元，带动全省养殖小龙虾387万亩。",
                "3870000 Mu");

            BasicTest(model,
                "阳西辣椒火辣上市日均20万公斤远销珠三角",
                "200000 Kilogram");

            BasicTest(model,
                "如今身高168公分",
                "168 Cntimeter");

            BasicTest(model,
                "澳联邦警察与维州警方在墨尔本缴获近一吨冰毒，为澳洲史上最大冰毒走私案。（澳洲联邦警察局 ...",
                "1 Ton");
        }

        [TestMethod][Ignore]
        public void TestTemperature()
        {
            var model = GetTemperatureModel();
            
        }

        [TestMethod][Ignore]
        public void TestAge()
        {
            var model = GetAgeModel();
            
        }


        private static IModel GetCurrencyModel()
        {
            return new CurrencyModel(
                new Dictionary<IExtractor, IParser>
                {
                    {
                        new NumberWithUnitExtractor(new Chinese.Extractors.CurrencyExtractorConfiguration()),
                        new NumberWithUnitParser(new Chinese.Parsers.CurrencyParserConfiguration())
                    },
                    {
                        new NumberWithUnitExtractor(new English.Extractors.CurrencyExtractorConfiguration()),
                        new NumberWithUnitParser(new English.Parsers.CurrencyParserConfiguration())
                    }
                }
                );
        }

        private static IModel GetAgeModel()
        {
            return new AgeModel(
                new Dictionary<IExtractor, IParser>
                {
                    {
                        new NumberWithUnitExtractor(new Chinese.Extractors.AgeExtractorConfiguration()),
                        new NumberWithUnitParser(new Chinese.Parsers.AgeParserConfiguration())
                    },
                    {
                        new NumberWithUnitExtractor(new English.Extractors.AgeExtractorConfiguration()),
                        new NumberWithUnitParser(new English.Parsers.AgeParserConfiguration())
                    }
                }
                );
        }

        private static IModel GetDimensionModel()
        {
            return new DimensionModel(
                new Dictionary<IExtractor, IParser>
                {
                    {
                        new NumberWithUnitExtractor(new Chinese.Extractors.DimensionExtractorConfiguration()),
                        new NumberWithUnitParser(new Chinese.Parsers.DimensionParserConfiguration())
                    },
                    {
                        new NumberWithUnitExtractor(new English.Extractors.DimensionExtractorConfiguration()),
                        new NumberWithUnitParser(new English.Parsers.DimensionParserConfiguration())
                    }
                }
                );
        }

        private static IModel GetTemperatureModel()
        {
            return new TemperatureModel(
                new Dictionary<IExtractor, IParser>
                {
                    {
                        new NumberWithUnitExtractor(new Chinese.Extractors.TemperatureExtractorConfiguration()),
                        new NumberWithUnitParser(new Chinese.Parsers.TemperatureParserConfiguration())
                    },
                    {
                        new NumberWithUnitExtractor(new English.Extractors.TemperatureExtractorConfiguration()),
                        new NumberWithUnitParser(new English.Parsers.TemperatureParserConfiguration())
                    }
                }
                );
        }
    }
}
