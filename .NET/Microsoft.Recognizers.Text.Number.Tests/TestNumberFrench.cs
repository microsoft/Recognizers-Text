using Microsoft.Recognizers.Text.Number.French;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Microsoft.Recognizers.Text.Number.Tests
{
    [TestClass]
    public class TestNumberFrench
    {

        [ClassCleanup]
        public static void ClassCleanup()
        {
            TestWriter.Close("Fra", typeof(NumberModel));
            TestWriter.Close("Fra", typeof(PercentModel));
            TestWriter.Close("Fra", typeof(OrdinalModel));
        }

        private void BasicTest(IModel model, string source, string value)
        {
            var resultStr = model.Parse(source);
            var resultJson = resultStr;
            Assert.AreEqual(1, resultJson.Count);
            Assert.AreEqual(source.Trim(), resultJson[0].Text);
            Assert.AreEqual(value, resultJson[0].Resolution["value"]);
            TestWriter.Write("Fra", model, source, resultStr);
        }

        private void WrappedTest(IModel model, string source, string extractSrc, string value)
        {
            var resultStr = model.Parse(source);
            var resultJson = resultStr;
            Assert.AreEqual(1, resultJson.Count);
            Assert.AreEqual(extractSrc, resultJson[0].Text);
            Assert.AreEqual(value, resultJson[0].Resolution["value"]);
            TestWriter.Write("Fra", model, source, resultStr);
        }

        private void MultiTest(IModel model, string source, int count)
        {
            var resultStr = model.Parse(source);
            var resultJson = resultStr;
            Assert.AreEqual(count, resultJson.Count);
            TestWriter.Write("Fra", model, source, resultStr);
        }
       
        [TestMethod]
        public void TestOrdinalModel()
        {
            var model = GetOrdinalModel();

            BasicTest(model,
                "tierce", "3");

            BasicTest(model, 
                "trois billionieme", "3000000000000");

            MultiTest(model, 
                "un trillion", 0);

            BasicTest(model,
                "11eme", "11");

            BasicTest(model,
                "trois millieme", "3000");

            BasicTest(model,
                "neuf milliardieme", "9000000000");

            BasicTest(model,
                "22e", "22");

            BasicTest(model,
                "30e", "30");

            BasicTest(model,
                "155e", "155");

            BasicTest(model,
                "255eme", "255");

            BasicTest(model,
                "500000eme", "500000");

            BasicTest(model,
                "2eme", "2");

            BasicTest(model,
                "onzieme", "11");

            BasicTest(model,
                "vingtieme", "20");
  
            BasicTest(model, 
                "vingt-cinquieme", "25");

            BasicTest(model,
                "octantieme", "80");

            BasicTest(model,
                "deux centieme", "200");

            BasicTest(model,
                "cent vingt cinquieme", "125");

            BasicTest(model,
                "cent vingt-cinquieme", "125");

            BasicTest(model,
                "soixante-dixieme", "70");

            BasicTest(model,
                "quatre-vingtieme", "80");

            BasicTest(model,
                "septante-huitieme", "78");

            BasicTest(model,
                "nonante-deuxieme", "92");

            BasicTest(model,
                "trois cent vingt seconde", "322");

           // BasicTest(model,
            //    "vingt et un trillions et trois cent vingt secondes", "21000000000322");
        }

        [TestMethod]
        public void TestNumberModel()
        {
            var ci = new FrenchNumberParserConfiguration().CultureInfo;
          
            var model = GetNumberModel();

            WrappedTest(model,
                "192,", "192", "192");

            MultiTest(model,
                "192.168.1.2", 4);

            MultiTest(model,
                "le liquide de 180ml", 0);

            MultiTest(model,
                "le liquide de .25ml", 0);

            MultiTest(model,
                " route de 29km ", 0);

            //MultiTest(model,
            //    " le 4 mai ", 0);

            //MultiTest(model,
            //    "le liquide de 180.25 ml", 0);

            BasicTest(model, "9,2321312", "9,2321312");

            BasicTest(model, " -9,2321312", "-9,2321312");

            BasicTest(model,
                "vingt", "20");

            BasicTest(model,
                "cent", "100");

            BasicTest(model,
                ",08", "0,08");

            BasicTest(model,
                "vingt quatre", "24");

            BasicTest(model,
                "vingt-quatre", "24");

            BasicTest(model,
                "trente trois", "33");

            BasicTest(model,
                "trente-trois", "33");

            BasicTest(model,
                "dix-sept", "17");

            BasicTest(model,
                "dix sept", "17");

            BasicTest(model,
                "nonante", "90");

            BasicTest(model, "trois million", "3000000");

            BasicTest(model, "trois million sept", "3000007");

            BasicTest(model,
                ",23456000", "0,23456");

            BasicTest(model,
                "4,800", "4,8");

            BasicTest(model,
                "cent trois et deux tiers", (103 + (double)2 / 3).ToString(ci));

            BasicTest(model,
                "seize", "16");

            BasicTest(model, 
                "deux tiers", ((double)2 / 3).ToString(ci));

            BasicTest(model,
                "cent six", "106");

            BasicTest(model,
                "cinq cent trente cinq", "535");

            BasicTest(model,
                "soixante et un", "61");

            BasicTest(model,
                " 3 douzaines", "36");

            BasicTest(model,
                "2 douzaine", "24");

            BasicTest(model,
                " trois cent douze", "312");

            BasicTest(model,
                "trois cent vingt quatre", "324");

            BasicTest(model, 
                "cent seize", "116");

            BasicTest(model,
                " 322 cent ", "32200");

            MultiTest(model,
                "1, 234, 567", 3);

            BasicTest(model,
                "9,2321312", "9,2321312");

            BasicTest(model,
              " -9,2321312", "-9,2321312");

            BasicTest(model,
                " -1", "-1");

            BasicTest(model,
                "-4/5", "-0,8");

            BasicTest(model,
                "- 1 4/5", "-1,8");

            BasicTest(model,
                "trois", "3");

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
                "1 billion", "1000000000000"); //trillion is 'billion' in FR

            BasicTest(model,
                " trois ", "3");

            BasicTest(model,
                "un billion", "1000000000000");

            BasicTest(model,
                "un centieme", "0,01"); // a hundredth

            BasicTest(model,
                "1,1^+23", "8,95430243255239");

            BasicTest(model,
                "2,5^-1", "0,4");

            BasicTest(model,
                "-1,1^+23", "-8,95430243255239");

            BasicTest(model,
                "-2,5^-1", "-0,4");

            BasicTest(model,
                "2  1/4", "2,25");

            BasicTest(model,
                "3/4", "0,75");

            BasicTest(model,
                "-1,1^--23", "-8,95430243255239");

            BasicTest(model,
                "-127,32e13", "-1,2732E+15");

            BasicTest(model,
                "12,32e+14", "1,232E+15");

            BasicTest(model,
                "-12e-1", "-1,2");

            BasicTest(model,
                "1e10", "10000000000");

            BasicTest(model,
                "1,1^23", "8,95430243255239");

            BasicTest(model,
                "trois", "3");

            BasicTest(model,
                "cent et trois quarts", "100,75"); // one hundred and three quarters

            BasicTest(model,
                "deux cent deux mille", "202000");

            BasicTest(model,
                "trois cent trente et un", "331");

            BasicTest(model,
                "soixante-dix", "70");

            BasicTest(model,
                "trois quarts", "0,75"); // 3/4 three quarters

            BasicTest(model,
                "vingt et trois cinquiemes", "20,6"); // 20 and three fifths

            BasicTest(model,
                "un huitieme", "0,125"); // 1/8 one eigth

            BasicTest(model,
                "cinq huitieme", "0,625"); // 5/8 five eigths

            BasicTest(model,
                "2,33 k", "2330");

            BasicTest(model,
                "vingt-trois cinquiemes", "4,6"); // twenty-three fifths

            BasicTest(model,
                "un demi", "0,5"); // 1/2 one half

            BasicTest(model,
                "un et un demi", "1,5"); // one and a half 

            BasicTest(model,
                "un et un quatrieme", "1,25"); // one and a quarter

            BasicTest(model,
                "cinq et un quart", "5,25"); // five and one quarter

            BasicTest(model,
                "cinquante-deux", "52");

            BasicTest(model,
                "cinquante  deux", "52");

            BasicTest(model,
                "vingt et trois et trois cinquiemes", "23,6"); // twenty and three and three fifths      

            BasicTest(model,
                "deux  cent  deux  mille", "202000");

            BasicTest(model,
                "deux cent virgule trois", "200,3");

            BasicTest(model,
                "deux cent virgule cinquante deux", "200,52");

            //BasicTest(model, //half dozen fails
            //    " une demis douzaine ", "6");

            // TODO: Integrate better support for tens numbers + 1, etc "21" - "vingt-et-un" 

            //BasicTest(model, // fails
            //    "vingt et un billion", "21000000000000"); // yields 10000000000020 instead

            //BasicTest(model,
            //    "vingt et un billion trois cent", "21000000000300");

            //BasicTest(model,
            //    "vingt et un billions et trois cents", "21000000000300");

            //BasicTest(model, // fails, adds 1,234 and 567
            //    "1.234.567", "1234567");
        }

        [TestMethod]
        public void TestFractionModel()
        {
            var ci = new FrenchNumberParserConfiguration().CultureInfo;
          
            var model = GetNumberModel();

            BasicTest(model,
                "un cinquieme", "0,2");

            BasicTest(model,
                "un demi", "0,5");

            BasicTest(model,
                "un billionieme", "1E-12"); // a trillionth

            BasicTest(model, 
                "cent mille billionieme", "1E-07"); // a hundred thousand trillionths

            BasicTest(model,
                "un cinquieme", "0,2"); // one fifth 

            BasicTest(model,  
                "trois cinquieme", "0,6"); // three fifths

            BasicTest(model, 
                "un vingt cinquieme", "0,04"); // twenty fifths 

            BasicTest(model,
                "trois et un cinquieme", "3,2"); //three and a fifth

            BasicTest(model, 
                "vingt et un cinquieme", "20,2"); // twenty one fifths

            BasicTest(model,
                "une vingt et unieme", ((double)1 / 21).ToString(ci)); // a twenty first

            BasicTest(model,
                "un vingt-cinquieme", ((double)1 / 25).ToString(ci)); // one twenty fifth

            BasicTest(model,
                "un sur trois", ((double)1 / 3).ToString(ci)); // one over three

            BasicTest(model,
                "1 sur vingt-un", ((double)1 / 21).ToString(ci)); // 1 over twenty one 

            BasicTest(model,
                "1 sur cent vingt et un", ((double)1 / 121).ToString(ci)); // 1 over one hundred and twenty one

            BasicTest(model,
                "1 sur trois", ((double)1 / 3).ToString(ci));

            BasicTest(model,
                "1 sur 3", ((double)1 / 3).ToString(ci));

            BasicTest(model,
                "un sur 3", ((double)1 / 3).ToString(ci));

            BasicTest(model,
                "un sur 20", ((double)1 / 20).ToString(ci));

            BasicTest(model,
                "un sur cent", ((double)1 / 100).ToString(ci));

            BasicTest(model,
                "un sur cent vingt cinq", ((double)1 / 125).ToString(ci));
            BasicTest(model,
                "cent trente cinquiemes", ((double)130 / 5).ToString(ci)); // one hundred and thirty fifths **CHECK

            //BasicTest(model,
            //    "cent trent-cinquiemes", ((double)100 / 35).ToString(ci)); // Fails - must make it so including '-' parses denom first

            //BasicTest(model, //fails
            //    "vingt-trois cinquieme", "4.6"); //twenty three fifths

            //BasicTest(model,
            //    "trois vingt-et-unieme", ((double)3 / 21).ToString(ci)); // three twenty firsts ** note NOT 'vingt-premier' 

            //BasicTest(model,
            //    "trois vingt et enieme", ((double)3 / 21).ToString(ci)); // same as above

            //BasicTest(model,
            //    "vingt cinquieme ", "0,8"); // twenty twenty fifths


            // ACT LIKE GOOGLE - ****THESE NEED TO BE LOOKED AT 

            //BasicTest(model,
            //    "cent trente et duex cinquiemes", ((double)132 / 5).ToString(ci)); // one hundred thirty two fifths

            //BasicTest(model,
            //    "cent trente-deux cinquiemes", ((double)132 / 5).ToString(ci));

            //BasicTest(model,
            //    "cent et trente-deux cinquiemes", ((double)132 / 5).ToString(ci));

            //BasicTest(model,
            //    "cent et trente et deux cinquiemes", (130 + (double)2 / 5).ToString(ci)); //one hundred and thirty and two fifths

            //BasicTest(model,
            //    "cent trente cinquiemes", ((double)100 / 35).ToString(ci)); // one hundred 35th's 

            //BasicTest(model,
            //    "un cent cinquieme", ((double)1 / 105).ToString(ci)); // one / one hundred fifth

            //BasicTest(model,
            //    "cent mille cinquiemes", ((double)100 / 1005).ToString(ci)); // one hundred / one thousand fifths 

        }

        [TestMethod]
        public void TestPercentageModel()
        {
            var model = GetPercentageModel();

            BasicTest(model,
                "100%", "100%");

            BasicTest(model,
                " 100% ", "100%");

            BasicTest(model,
                " 100 pourcent", "100%");

            BasicTest(model,
                "120 pourcent", "120%");

            BasicTest(model,
                "10 pourcents", "10%");

            BasicTest(model,
                " 100 pourcentage", "100%");

            BasicTest(model,
                "240 pourcent", "240%");

            BasicTest(model,
                "vingt pourcent", "20%");

            BasicTest(model,
                "trente pourcentage", "30%");

            BasicTest(model, 
                "cent pourcent", "100%");

            BasicTest(model, 
                "cent pourcentages", "100%");

            BasicTest(model,
                "pourcentage de vingt", "20%");

            BasicTest(model, "4,800%", "4,8%");

            BasicTest(model,
                "pourcent des trois cent cinq", "305%");

            BasicTest(model, "9,2321312%", "9,2321312%");

            BasicTest(model,
                "cinq cent trente cinq pourcent", "535%");

            BasicTest(model, "1,1^23 pourcent", "8,95430243255239%");

            //BasicTest(model, // fails
            //    "pourcentage 20", "20%");
        }

        private static IModel GetNumberModel()
        {
            return
                new NumberModel(
                        AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Number, new FrenchNumberParserConfiguration()),
                        new NumberExtractor(NumberMode.PureNumber));
        }

        private static IModel GetOrdinalModel()
        {
            return
                new OrdinalModel(
                        AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Ordinal, new FrenchNumberParserConfiguration()),
                        new OrdinalExtractor());
        }

        private static IModel GetPercentageModel()
        {
            return
                new PercentModel(
                    AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Percentage, new FrenchNumberParserConfiguration()),
                    new PercentageExtractor());

        }
    }
}
