using Microsoft.Recognizers.Text.Number.English;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Recognizers.Text.Number.Tests
{
    [TestClass]
    public class TestNumberEnglish
    {
        private void BasicTest(IModel model, string source, string value)
        {
            var resultStr = model.Parse(source);
            var resultJson = resultStr;
            Assert.AreEqual(1, resultJson.Count);
            Assert.AreEqual(source.Trim(), resultJson[0].Text);
            Assert.AreEqual(value, resultJson[0].Resolution["value"]);
        }

        private void WrappedTest(IModel model, string source, string extractSrc, string value)
        {
            var resultStr = model.Parse(source);
            var resultJson = resultStr;
            Assert.AreEqual(1, resultJson.Count);
            Assert.AreEqual(extractSrc, resultJson[0].Text);
            Assert.AreEqual(value, resultJson[0].Resolution["value"]);
        }


        private void MultiTest(IModel model, string source, int count)
        {
            var resultStr = model.Parse(source);
            var resultJson = resultStr;
            Assert.AreEqual(count, resultJson.Count);
        }


        [TestMethod]
        public void TestOrdinalModel()
        {
            var model = NumberRecognizer.Instance.GetOrdinalModel(Culture.English);

            BasicTest(model,
                "three trillionth", "3000000000000");

            MultiTest(model,
                "a trillionth", 0);

            BasicTest(model,
                "a hundred trillionth", "100000000000000");

            BasicTest(model,
                "11th", "11");

            BasicTest(model,
                "21st", "21");

            BasicTest(model,
                "30th", "30");

            BasicTest(model,
                "2nd", "2");

            BasicTest(model,
                "eleventh", "11");

            BasicTest(model,
                "twentieth", "20");

            BasicTest(model,
                "twenty-fifth", "25");

            BasicTest(model,
                "twenty-first", "21");

            BasicTest(model,
                "one hundred twenty fifth", "125");

            BasicTest(model,
                "one hundred twenty-fifth", "125");

            BasicTest(model,
                "trillionth", "1000000000000");

            BasicTest(model,
                "twenty-one trillion and three hundred twenty second", "21000000000322");

            BasicTest(model,
                "two hundredth", "200");
        }

        [TestMethod]
        public void TestNumberModel()
        {
            var model = NumberRecognizer.Instance.GetNumberModel(Culture.English);

            WrappedTest(model,
                "192.", "192", "192");

            MultiTest(model,
                "192.168.1.2", 4);

            MultiTest(model,
                "the 180.25ml liquid", 0);

            MultiTest(model,
                "the 180ml liquid", 0);

            MultiTest(model,
                " 29km Road ", 0);

            MultiTest(model,
                " the May 4th ", 0);

            MultiTest(model,
                "the .25ml liquid", 0);

            BasicTest(model,
                ".08", "0.08");

            MultiTest(model,
                "an", 0);

            MultiTest(model,
                "a", 0);

            BasicTest(model,
                ".23456000", "0.23456");

            BasicTest(model,
                "4.800", "4.8");

            BasicTest(model,
                "one hundred and three and two thirds", (103 + (double)2 / 3).ToString());

            BasicTest(model,
                "sixteen", "16");

            BasicTest(model,
                "two thirds", ((double)2 / 3).ToString());


            BasicTest(model,
                "one hundred and sixteen", "116");

            BasicTest(model,
                "one hundred and six", "106");

            BasicTest(model,
                "one hundred and sixty-one", "161");

            BasicTest(model,
                "a trillionth", "1E-12");

            BasicTest(model,
                "a hundred trillionths", "1E-10");

            BasicTest(model,
                " half a  dozen", "6");

            BasicTest(model,
                " 3 dozens", "36");

            BasicTest(model,
                "a dozen", "12");

            BasicTest(model,
                " three dozens ", "36");

            BasicTest(model,
                " three hundred and two dozens", "324");

            BasicTest(model,
                "1,234,567", "1234567");

            MultiTest(model,
                "1, 234, 567", 3);

            BasicTest(model,
                "9.2321312", "9.2321312");

            BasicTest(model,
                " -9.2321312", "-9.2321312");

            BasicTest(model,
                " -1", "-1");

            BasicTest(model,
                "-4/5", "-0.8");

            BasicTest(model,
                "- 1 4/5", "-1.8");

            BasicTest(model,
                "three", "3");

            BasicTest(model,
                " 123456789101231", "123456789101231");

            BasicTest(model,
                "-123456789101231", "-123456789101231");

            BasicTest(model,
                " -123456789101231", "-123456789101231");

            BasicTest(model,
                "1", "1");

            BasicTest(model,
                "10k", "10000");

            BasicTest(model,
                "10G", "10000000000");

            BasicTest(model,
                "- 10  k", "-10000");

            BasicTest(model,
                "2 million", "2000000");

            BasicTest(model,
                "1 trillion", "1000000000000");

            BasicTest(model,
                " three ", "3");

            BasicTest(model,
                "one trillion", "1000000000000");

            BasicTest(model,
                "twenty-one trillion", "21000000000000");

            BasicTest(model,
                "twenty-one trillion three hundred", "21000000000300");

            BasicTest(model,
                "twenty-one trillion and three hundred", "21000000000300");

            BasicTest(model,
                "fifty - two", "52");

            BasicTest(model,
                "fifty   two", "52");

            BasicTest(model,
                "Three hundred  and  thirty one", "331");

            BasicTest(model,
                "two hundred and two thousand", "202000");

            BasicTest(model,
                "two  thousand  and  two hundred", "2200");

            BasicTest(model,
                " 2.33 k", "2330");

            BasicTest(model,
                " two hundred point zero three", "200.03");

            BasicTest(model,
                " two hundred point seventy-one", "200.71");

            BasicTest(model,
                "1e10", "10000000000");

            BasicTest(model,
                "1.1^23", "8.95430243255239");

            BasicTest(model,
                " 322 hundred ", "32200");

            BasicTest(model,
                "three", "3");

            BasicTest(model,
                "seventy", "70");

            BasicTest(model,
                "fifty-two", "52");

            BasicTest(model,
                "2  1/4", "2.25");

            BasicTest(model,
                "3/4", "0.75");

            BasicTest(model,
                "one eighth", "0.125");

            BasicTest(model,
                "five eighths", "0.625");

            BasicTest(model,
                "a half", "0.5");

            BasicTest(model,
                "three quarters", "0.75");

            BasicTest(model,
                "twenty and three fifths", "20.6");

            BasicTest(model,
                "twenty-three fifths", "4.6");

            BasicTest(model,
                "twenty and three and three fifths", "23.6");

            BasicTest(model,
                "one million two thousand two hundred three fifths", "200440.6");

            BasicTest(model,
                "one and a half", "1.5");

            BasicTest(model,
                "one and a fourth", "1.25");

            BasicTest(model,
                "five and a quarter", "5.25");

            BasicTest(model,
                "one hundred and three quarters", "100.75");

            BasicTest(model,
                "a hundredth", "0.01");

            BasicTest(model,
                "1.1^+23", "8.95430243255239");

            BasicTest(model,
                "2.5^-1", "0.4");

            BasicTest(model,
                "-2500^-1", "-0.0004");

            BasicTest(model,
                "-1.1^+23", "-8.95430243255239");

            BasicTest(model,
                "-2.5^-1", "-0.4");

            BasicTest(model,
                "-1.1^--23", "-8.95430243255239");

            BasicTest(model,
                "-127.32e13", "-1.2732E+15");

            BasicTest(model,
                "12.32e+14", "1.232E+15");

            BasicTest(model,
                "-12e-1", "-1.2");

            BasicTest(model,
                "1.2b", "1200000000");
        }

        [TestMethod]
        public void TestFractionModel()
        {
            var model = NumberRecognizer.Instance.GetNumberModel(Culture.English);

            BasicTest(model,
                "a fifth", "0.2");

            BasicTest(model,
                "a trillionth", "1E-12");

            BasicTest(model,
                "a hundred thousand trillionths", "1E-07");

            BasicTest(model,
                "one fifth", "0.2");

            BasicTest(model,
                "three fifths", "0.6");

            BasicTest(model,
                "twenty fifths", "4");

            BasicTest(model,
                "twenty-three fifths", "4.6");

            BasicTest(model,
                "three and a fifth", "3.2");

            BasicTest(model,
                "twenty one fifths", "4.2");

            BasicTest(model,
                "a twenty-first", ((double)1 / 21).ToString());

            BasicTest(model,
                "one twenty-fifth", ((double)1 / 25).ToString());

            BasicTest(model,
                "three twenty-firsts", ((double)3 / 21).ToString());

            BasicTest(model,
                "three twenty firsts", ((double)3 / 21).ToString());

            BasicTest(model,
                "twenty twenty fifths", "0.8");

            // act like Google
            BasicTest(model,
                "one hundred and thirty fifths", ((double)130 / 5).ToString());

            BasicTest(model,
                "one hundred thirty fifths", ((double)100 / 35).ToString());

            BasicTest(model,
                "one hundred thirty two fifths", ((double)132 / 5).ToString());

            BasicTest(model,
                "one hundred thirty-two fifths", ((double)132 / 5).ToString());

            BasicTest(model,
                "one hundred and thirty-two fifths", ((double)132 / 5).ToString());

            BasicTest(model,
                "one hundred and thirty and two fifths", (130 + (double)2 / 5).ToString());

            BasicTest(model,
                "one hundred thirty-fifths", ((double)100 / 35).ToString());

            BasicTest(model,
                "one one hundred fifth", ((double)1 / 105).ToString());

            BasicTest(model,
                "one one hundred and fifth", ((double)1 / 105).ToString());

            BasicTest(model,
                "one hundred one thousand fifths", ((double)100 / 1005).ToString());

            BasicTest(model,
                "one over three", ((double)1 / 3).ToString());

            BasicTest(model,
                "1 over twenty-one", ((double)1 / 21).ToString());

            BasicTest(model,
                "1 over one hundred and twenty one", ((double)1 / 121).ToString());

            BasicTest(model,
                "1 over three", ((double)1 / 3).ToString());

            BasicTest(model,
                "1 over 3", ((double)1 / 3).ToString());

            BasicTest(model,
                "one over 3", ((double)1 / 3).ToString());

            BasicTest(model,
                "one over 20", ((double)1 / 20).ToString());

            BasicTest(model,
                "one over twenty", ((double)1 / 20).ToString());

            BasicTest(model,
                "one over one hundred", ((double)1 / 100).ToString());

            BasicTest(model,
                "one over one hundred and twenty five", ((double)1 / 125).ToString());

            BasicTest(model,
                "ninety - five hundred fifths", ((double)9500 / 5).ToString());
        }

        [TestMethod]
        public void TestPercentageModel()
        {
            var model = NumberRecognizer.Instance.GetPercentageModel(Culture.English);

            BasicTest(model,
                "100%", "100%");

            BasicTest(model,
                " 100% ", "100%");

            BasicTest(model,
                " 100 percent", "100%");

            BasicTest(model,
                " 100 percentage", "100%");

            BasicTest(model,
                "240 percent", "240%");

            BasicTest(model,
                "twenty percent", "20%");

            BasicTest(model,
                "thirty percentage", "30%");

            BasicTest(model,
                "one hundred percent", "100%");

            BasicTest(model,
                "one hundred percents", "100%");

            BasicTest(model,
                "percents of twenty", "20%");

            BasicTest(model,
                "percent of 10", "10%");

            BasicTest(model,
                "per cent of twenty-two", "22%");

            BasicTest(model,
                "per cent of 210", "210%");

            BasicTest(model,
                "10 percent", "10%");
        }
    }
}