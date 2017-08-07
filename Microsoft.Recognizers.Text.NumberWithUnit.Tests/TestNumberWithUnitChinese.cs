using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
            var model = NumberWithUnitRecognizer.Instance.GetCurrencyModel(Culture.Chinese);

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
            var model = NumberWithUnitRecognizer.Instance.GetDimensionModel(Culture.Chinese);

            BasicTest(model,
                "去年，潜江虾稻产业综合产值突破180亿元，带动就业超10万人，龙虾养殖户户平增收16000元，带动全省养殖小龙虾387万亩。",
                "3870000 Mu");

            BasicTest(model,
                "阳西辣椒火辣上市日均20万公斤远销珠三角",
                "200000 Kilogram");

            BasicTest(model,
                "如今身高168公分",
                "168 Centimeter");

            BasicTest(model,
                "澳联邦警察与维州警方在墨尔本缴获近一吨冰毒，为澳洲史上最大冰毒走私案。（澳洲联邦警察局 ...",
                "1 Ton");
        }

        [TestMethod]
        public void TestTemperature()
        {
            var model = NumberWithUnitRecognizer.Instance.GetTemperatureModel(Culture.Chinese);

            BasicTest(model, "设置恒温器为85度", "85 Degree");

            BasicTest(model, "把温度升高5度", "5 Degree");

            BasicTest(model, "正常的温度是华氏温度98.6度", "98.6 F");
            BasicTest(model, "华氏温度100度", "100 F");
            BasicTest(model, "20摄氏度", "20 C");
            BasicTest(model, "外面的温度是98度", "98 Degree");
            BasicTest(model, "你能把华氏温度51度转换为摄氏度吗", new string[] { "51 F", "C" });
        }

        [TestMethod]
        public void TestAge()
        {
            var model = NumberWithUnitRecognizer.Instance.GetAgeModel(Culture.Chinese);

            BasicTest(model,
                      "当她五岁的时候，她学会了骑自行车",
                      "5 Year");

            BasicTest(model,
                      "我只有29岁！",
                      "29 Year");

            BasicTest(model,
                      "这件事发生在宝宝只有十个月大的时候.",
                      "10 Month");

            BasicTest(model,
                      "十二月初出生的话已经三周大了",
                      "3 Week");

            BasicTest(model,
                      "她出生于1945年5月8号，现在60岁了",
                      "60 Year");

            BasicTest(model,
                      "她已经满七周岁了，可以上小学了",
                      "7 Year");

            BasicTest(model,
                      "90天大的小孩应该去医院做检查",
                      "90 Day");
        }

    }
}
