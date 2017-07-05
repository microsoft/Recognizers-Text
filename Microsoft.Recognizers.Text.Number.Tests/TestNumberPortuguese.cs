using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Recognizers.Text.Number.Portuguese;

namespace Microsoft.Recognizers.Text.Number.Tests
{

    // Based on the Spanish tests

    [TestClass]
    public class TestNumberPortuguese
    {
        private void BasicTest(IModel model, string source, string value, string text = null)
        {
            var result = model.Parse(source);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(text ?? source.Trim(), result[0].Text);
            Assert.AreEqual(value, result[0].Resolution["value"]);
        }

        private void MultiTest(IModel model, string source, int count)
        {
            var result = model.Parse(source);
            Assert.AreEqual(count, result.Count);
        }

        [TestMethod]
        public void TestOrdinalModel()
        {
            var model = GetOrdinalModel();

            BasicTest(model, "setimo", "7");

            BasicTest(model, "quadragesimo setimo", "47");

            BasicTest(model, "tricentésimo quadragésima sétima", "347");

            BasicTest(model, "dois milesimo tricentesimo quadragesimo setimo", "2347");

            BasicTest(model, "cinquenta e dois milesimo tricentesimo quadragesimo setimo", "52347");

            BasicTest(model, "milesimo quingentesimo vigesimo terceiro", "1523");

            BasicTest(model, "vigesimo quinto", "25");

            BasicTest(model, "vigesimo primeiro", "21");

            BasicTest(model, "centesimo vigesimo quinto", "125");

            BasicTest(model, "ducentesimo", "200");

            BasicTest(model, "21o", "21");

            BasicTest(model, "30a", "30");

            BasicTest(model, "2a", "2");

            BasicTest(model, "undecimo", "11");

            BasicTest(model, "décimo primeiro", "11");

            BasicTest(model, "vigesimo", "20");

            BasicTest(model, "centesimo", "100");

            BasicTest(model, "vinte e dois milesimo", "22000");

            BasicTest(model, "quatrocentos e cinquenta e dois milesimo tricentesimo quadragesimo setimo", "452347");

            BasicTest(model, "três milhões quatrocentos e cinquenta e dois milésimo tricentesimo quadragesimo setimo", "3452347");

            BasicTest(model, "tres mil quinhentos e vinte e quatro milhoes seiscentos e noventa e quatro milesimo sexcentesimo septuagesimo terceiro", "3524694673");

            //BasicTest(model, "trilionesimo", "3000000");

            //BasicTest(model, "trilionesimo setimo", "3000007");

            //BasicTest(model, "trilionesimo quadragesimo setimo", "3000047");

        }

        [TestMethod]
        public void TestNumberModel()
        {
            var ci = new PortugueseNumberParserConfiguration().CultureInfo;

            var model = GetNumberModel();

            #region Integer numbers

            BasicTest(model, " 123456789101231", "123456789101231");

            BasicTest(model, "-123456789101231", "-123456789101231");

            BasicTest(model, " -123456789101231", "-123456789101231");

            BasicTest(model, " -1", "-1");

            BasicTest(model, "1.234.567", "1234567");

            BasicTest(model, "sete", "7");

            BasicTest(model, "dezessete", "17");

            BasicTest(model, "dezassete", "17");

            BasicTest(model, "mil", "1000");

            BasicTest(model, "cem", "100");

            BasicTest(model, "mil e cem", "1100");

            BasicTest(model, "mil cento e dez", "1110");

            BasicTest(model, "mil e duzentos", "1200");

            BasicTest(model, "mil duzentos e vinte", "1220");

            BasicTest(model, "vinte e sete", "27");

            BasicTest(model, "quarenta e sete", "47");

            BasicTest(model, "trezentos e quarenta e sete", "347");

            BasicTest(model, "dois mil trezentos e quarenta e sete", "2347");

            BasicTest(model, "duas mil trezentas e quarenta e sete", "2347");

            BasicTest(model, "cinquenta e dois mil trezentos e quarenta e sete", "52347");

            BasicTest(model, "quatrocentos e cinquenta e dois mil trezentos e quarenta e sete", "452347");

            BasicTest(model, "mil quinhentos e vinte e três", "1523");

            BasicTest(model, "mil quinhentos e vinte e tres", "1523");

            BasicTest(model, "dois bilhoes", "2000000000");

            BasicTest(model, "treze milhoes quatrocentos e cinquenta e dois mil", "13452000"); 

            BasicTest(model, "dois bilhões", "2000000000");

            BasicTest(model, "três milhões", "3000000");

            BasicTest(model, "tres milhoes e sete", "3000007");

            BasicTest(model, "tres milhoes e quarenta e sete", "3000047");

            BasicTest(model, "tres milhoes trezentos e quarenta e sete", "3000347");

            BasicTest(model, "tres milhoes dois mil trezentos e quarenta e sete", "3002347");

            BasicTest(model, "tres milhoes cinquenta e dois mil trezentos e quarenta e sete", "3052347");

            BasicTest(model, "tres milhoes quatrocentos e cinquenta e dois mil trezentos e quarenta e sete", "3452347");

            BasicTest(model, "treze milhoes quatrocentos e cinquenta e dois mil trezentos e quarenta e sete", "13452347");

            BasicTest(model, "quinhentos e treze milhoes quatrocentos e cinquenta e dois mil trezentos e quarenta e sete", "513452347");

            BasicTest(model, "quinhentos e treze milhoes quatrocentos e cinquenta e dois mil trezentos e quarenta", "513452340");

            BasicTest(model, "quinhentos e treze milhoes quatrocentos e cinquenta e dois mil trezentos", "513452300");

            BasicTest(model, "quinhentos e treze milhoes quatrocentos e cinquenta e dois mil", "513452000");

            BasicTest(model, "quinhentos e treze milhoes quatrocentos e cinquenta mil", "513450000");

            BasicTest(model, "quinhentos e treze milhoes e quatrocentos mil", "513400000");

            BasicTest(model, "quinhentos e treze milhoes", "513000000");

            BasicTest(model, "quinhentos milhoes", "500000000");
           
            BasicTest(model, "tres trilhoes quatrocentos e cinquenta e cinco bilhoes duzentos e vinte e oito milhoes e quinhentos e cinquenta e seis mil", "3455228556000");

            BasicTest(model, "tres trilhoes quatrocentos e cinquenta e cinco bilhoes duzentos e vinte e oito milhoes", "3455228000000");

            BasicTest(model, "tres trilhoes quatrocentos e cinquenta e cinco bilhoes", "3455000000000");

            BasicTest(model, "três trilhões", "3000000000000");

            // numbers within sentences
            BasicTest(model, "me dê mil", "1000", "mil");

            BasicTest(model, "tirasse um fino", "1", "um");

            BasicTest(model, "compre só uma vaca", "1", "uma");

            BasicTest(model, "vá e compre duzentas vacas", "200", "duzentas");

            BasicTest(model, "só tenho mil e cem reais", "1100", "mil e cem");

            BasicTest(model, "so tenho sete mil duzentos e trinta e cinco euros", "7235", "sete mil duzentos e trinta e cinco");

            BasicTest(model, "creio que vou gastar algo em torno de treze milhoes quatrocentos e cinquenta e dois mil trezentos e quarenta e sete escudos no projeto", "13452347", "treze milhoes quatrocentos e cinquenta e dois mil trezentos e quarenta e sete");

            BasicTest(model, "meia duzia", "6");

            BasicTest(model, "meia dúzia", "6");

            BasicTest(model, "três", "3");

            BasicTest(model, "tres duzias", "36");

            BasicTest(model, "uma dúzia", "12");

            BasicTest(model, "quinze dúzias", "180");

            BasicTest(model, "uma dezena", "10");

            //BasicTest(model, " 3 duzias", "36");

            //BasicTest(model, "3 dúzias", "36");

            //BasicTest(model, "duas dezenas", "20");

            //BasicTest(model, "3 dezenas", "30");

            #endregion

            #region Double numbers

            BasicTest(model, " 101231,2353", "101231,2353");

            BasicTest(model, "-101231,4323", "-101231,4323");

            BasicTest(model, " -89101231,5127", "-89101231,5127");

            BasicTest(model, " -1,1234567", "-1,1234567");

            BasicTest(model, "1.234.567,51274", "1234567,51274");

            BasicTest(model, "192,", "192", "192");

            BasicTest(model, ",23456000", "0,23456");

            BasicTest(model, "4,800", "4,8");

            BasicTest(model, ",08", "0,08");

            BasicTest(model, "9,2321312", "9,2321312");

            BasicTest(model, " -9,2321312", "-9,2321312");

            BasicTest(model, "1e10", "10000000000");

            BasicTest(model, "1,1^23", "8,95430243255239");

            BasicTest(model, "sete vírgula cinco", "7,5");

            BasicTest(model, "quarenta e sete vírgula vinte e oito", "47,28");

            BasicTest(model, "dois mil trezentos e quarenta e sete virgula mil quinhentos e setenta e oito", "2347,1578");

            BasicTest(model, "cinquenta e dois mil trezentos e quarenta e sete virgula duzentos", "52347,2");

            BasicTest(model, "quatrocentos e cinquenta e dois mil trezentos e quarenta e sete virgula vinte e dois", "452347,22");

            BasicTest(model, "1,1^+23", "8,95430243255239");

            BasicTest(model, "2,5^-1", "0,4");

            BasicTest(model, "-2500^-1", "-0,0004");

            BasicTest(model, "-1,1^+23", "-8,95430243255239");

            BasicTest(model, "-2,5^-1", "-0,4");

            BasicTest(model, "-1,1^--23", "-8,95430243255239");

            BasicTest(model, "-127,32e13", "-1,2732E+15");

            BasicTest(model, "12,32e+14", "1,232E+15");

            BasicTest(model, "-12e-1", "-1,2");

            // What should be the behaviour in this case? The parser works correctly, and the text ignores "hum", as it should.
            BasicTest(model, "hum mil e três", "1003", "mil e três");

            //BasicTest(model, "sete e meio", "7,5");

            // People say these, but they are not formally correct.
            //BasicTest(model, "trezentos e quarenta e sete ponto quinhentos e doze", "347,512");
            //BasicTest(model, "duzentos ponto setenta e um", "200,71");
            //BasicTest(model, "quarenta e sete e vinte e oito", "47,28");

            #endregion

            #region Translated numbers from English

            BasicTest(model, "192.", "192", "192");

            // '.' is group separator in spanish - so not understood as IP
            MultiTest(model, "192.168.1.2", 3);

            MultiTest(model, " subimos ao 4o piso ", 0);

            BasicTest(model, ",08", "0,08");

            BasicTest(model, "um décimo", "0,1");

            BasicTest(model, "um milésimo", "0,001");

            BasicTest(model, "um  centesimo", "0,01");

            MultiTest(model, "um", 1);

            MultiTest(model, "uma", 1);

            BasicTest(model, ",23456000", "0,23456");

            BasicTest(model, "4,800", "4,8");

            BasicTest(model, "dezesseis", "16");

            BasicTest(model, "cento e dezesseis", "116");

            BasicTest(model, "cento e seis", "106");

            BasicTest(model, "cento e sessenta e um", "161");

            BasicTest(model, "um milionésimo", "1E-06");

            BasicTest(model, " meia   duzia ", "6");

            BasicTest(model, "uma duzia", "12");

            BasicTest(model, " tres duzias ", "36");

            BasicTest(model, "1.234.567", "1234567");

            MultiTest(model, "1. 234. 567", 3);

            BasicTest(model, "9,2321312", "9,2321312");

            BasicTest(model, " -9,2321312", "-9,2321312");

            BasicTest(model, " -1", "-1");

            BasicTest(model, "-4/5", "-0,8");

            BasicTest(model, "- 1 4/5", "-1,8");

            BasicTest(model, "tres", "3");

            BasicTest(model, " 123456789101231", "123456789101231");

            BasicTest(model, "-123456789101231", "-123456789101231");

            BasicTest(model, " -123456789101231", "-123456789101231");

            BasicTest(model, "1", "1");

            BasicTest(model, "10k", "10000");

            BasicTest(model, "10G", "10000000000");

            BasicTest(model, "- 10  k", "-10000");

            BasicTest(model, "2 milhoes", "2000000");

            BasicTest(model, " tres ", "3");

            BasicTest(model, "cinquenta   e   dois", "52");

            BasicTest(model, "trezentos  e trinta  e   um", "331");

            BasicTest(model, "duzentos e dois mil", "202000");

            BasicTest(model, "dois mil e duzentos", "2200");

            BasicTest(model, " 2,33 k", "2330");

            BasicTest(model, " duzentos virgula zero tres", "200,03");

            BasicTest(model, "1e10", "10000000000");

            BasicTest(model, "1,1^23", "8,95430243255239");

            BasicTest(model, " 322 milhoes ", "322000000");

            BasicTest(model, "setenta", "70");

            BasicTest(model, "cinquenta e dois", "52");

            BasicTest(model, "2  1/4", "2,25");

            BasicTest(model, "3/4", "0,75");

            BasicTest(model, "um oitavo", "0,125");

            BasicTest(model, "cinco oitavos", "0,625");

            BasicTest(model, "tres quartos", "0,75");

            // Ambiguous in PT, assuming the first result.
            BasicTest(model, "vinte e tres quintos", "4,6");
            //BasicTest(model, "vinte e tres quintos", "20,6");

            BasicTest(model, "um centesimo", "0,01");

            //BasicTest(model, "um e um quarto", "1,25");

            //BasicTest(model, "cinco e um quarto", "5,25");

            //BasicTest(model, "cem e tres quartos", "100,75");

            //BasicTest(model, "dois tercos", ((double)2 / 3).ToString(ci));

            //BasicTest(model, "dois terços", ((double)2 / 3).ToString(ci));

            //BasicTest(model, " 2 tercos", ((double)2 / 3).ToString(ci));

            //BasicTest(model, "cento e tres e dois terços", (103 + (double)2 / 3).ToString(ci));

            //BasicTest(model, "vinte e tres e tres quintos", "23,6");

            //BasicTest(model, "um milhao dois mil duzentos e tres quintos", "200440,6");

            //BasicTest(model, "1 quatrilhao", "1000000000000");

            //BasicTest(model, "vinte e um quadrilhões", "21000000000000");

            //BasicTest(model, "vinte e um quatrilhoes e trezentos", "21000000000300");

            //BasicTest(model, "meio", "0,5");

            //// BasicTest(model, "meia", "0,5"); ???

            //BasicTest(model, "um e meio", "1,5");

            #endregion
        }

        [TestMethod]
        public void TestPercentageModel()
        {
            var ci = new PortugueseNumberParserConfiguration().CultureInfo;

            var model = GetPercentageModel();

            #region Integer percentages

            BasicTest(model, "100%", "100%");

            BasicTest(model, " 100% ", "100%");

            BasicTest(model, " 100 por cento", "100%");

            BasicTest(model, " cem por cento", "100%");

            BasicTest(model, "243 por cento", "243%");

            BasicTest(model, "vinte por cento", "20%");

            BasicTest(model, "trinta e cinco por cento", "35%");

            BasicTest(model, "quinhentos e trinta e cinco por cento", "535%");

            BasicTest(model, "10 por cento", "10%");

            BasicTest(model, "dez por cento", "10%");

            BasicTest(model, "tres milhoes cinquenta e dois mil trezentos e quarenta e sete por cento", "3052347%");

            // percentages within sentences
            BasicTest(model, "algo como uns 11%", "11%", "11%");

            BasicTest(model, "claro, somente uns 15 por cento", "15%", "15 por cento");

            BasicTest(model, "sim, nada mais que vinte e cinco por cento", "25%", "vinte e cinco por cento");

            BasicTest(model, "derrame cem por cento do liquido", "100%", "cem por cento");

            BasicTest(model, "um percentual de 25%", "25%", "25%");

            BasicTest(model, "uma percentagem de trinta e seis por cento do total", "36%", "trinta e seis por cento");

            BasicTest(model, "um percentual de oitenta e quatro por cento", "84%", "oitenta e quatro por cento");

            #endregion

            #region Double percentages

            BasicTest(model, " 101231,2353%", "101231,2353%");

            BasicTest(model, "-101231,4323%", "-101231,4323%");

            BasicTest(model, " -89101231,5127%", "-89101231,5127%");

            BasicTest(model, " - 89101231,5127%", "-89101231,5127%");

            BasicTest(model, ",23456000%", "0,23456%");

            BasicTest(model, "4,800%", "4,8%");

            BasicTest(model, "4,8 por cento", "4,8%");

            BasicTest(model, " -89101231,5127 por cento", "-89101231,5127%");

            BasicTest(model, "-89101231,5127 por cento", "-89101231,5127%");

            BasicTest(model, " - 89101231,5127 por cento", "-89101231,5127%");

            BasicTest(model, " -1,1234567 por cento", "-1,1234567%");

            BasicTest(model, "1.234.567,51274 por cento", "1234567,51274%");

            BasicTest(model, ",08 por cento", "0,08%");

            BasicTest(model, "9,2321312%", "9,2321312%");

            BasicTest(model, " -9,2321312 por cento", "-9,2321312%");

            BasicTest(model, "1e10%", "10000000000%");

            BasicTest(model, "1,1^23 por cento", "8,95430243255239%");

//            BasicTest(model, "sete ponto 5 por cento", "7,5%");

//            BasicTest(model, "sete virgula 5 por cento", "7,5%");

            BasicTest(model, "quarenta e sete virgula vinte e oito por cento", "47,28%");

            #endregion

            #region Fraction percentages

            BasicTest(model, "tres quintos por cento", ((double)3 / 5).ToString(ci) + "%");

            BasicTest(model, "dois virgula cinco por cento", "2,5%");

            BasicTest(model, "um quinto por cento", "0,2%");

            BasicTest(model, "um bilionesimo por cento", "1E-09%");

            BasicTest(model, "um vinte e um avos por cento", ((double)1 / 21).ToString(ci) + "%");

            BasicTest(model, "cento e trinta e tres vinte e um avos por cento", ((double)133 / 21).ToString(ci) + "%");

            // Ambiguous in Portuguese
            // BasicTest(model, "cento e trinta e tres vinte e um avos por cento", (130 + (double)3 / 21).ToString(ci) + "%");

            BasicTest(model, "vinte e dois trinta avos por cento", ((double)22 / 30).ToString(ci) + "%");

            BasicTest(model, "tres dois milesimos por cento", ((double)3 / 2000).ToString(ci) + "%");

            // act like Google
            BasicTest(model, "cento e trinta quintos por cento", ((double)130 / 5).ToString(ci) + "%");

            BasicTest(model, "cento e trinta e dois quintos por cento", ((double)132 / 5).ToString(ci) + "%");

            BasicTest(model, "um sobre tres por cento", ((double)1 / 3).ToString(ci) + "%");

            BasicTest(model, "1 sobre 3 por cento", ((double)1 / 3).ToString(ci) + "%");

            BasicTest(model, "3/4%", ((double)3 / 4).ToString(ci) + "%");

            BasicTest(model, "2/3%", ((double)2 / 3).ToString(ci) + "%");

//            BasicTest(model, "sete e meio por cento", "7,5%");

//            BasicTest(model, "cem trinta e cinco avos por cento", ((double)100 / 35).ToString(ci) + "%");

            #endregion
        }

        [TestMethod]
        public void TestFractionModel()
        {
            var ci = new PortugueseNumberParserConfiguration().CultureInfo;

            var model = GetNumberModel();

            BasicTest(model, "um quinto", "0,2");

            BasicTest(model, "um bilionesimo", "1E-09");

            BasicTest(model, "cem mil bilionesimos", "0,0001");

            BasicTest(model, "tres quintos", "0,6");

            BasicTest(model, "vinte quintos", "4");

            BasicTest(model, "vinte e tres quintos", "4,6");

            BasicTest(model, "dois sobre onze", ((double)2 / 11).ToString(ci));

            BasicTest(model, "um vinte e um avos", ((double)1 / 21).ToString(ci));

            BasicTest(model, "dois onze avos", ((double)2 / 11).ToString(ci));

            BasicTest(model, "cento e trinta e tres vinte e um avos", ((double)133 / 21).ToString(ci));

            BasicTest(model, "vinte e dois trinta avos", ((double)22 / 30).ToString(ci));

            BasicTest(model, "um vinte e cinco avos", ((double)1 / 25).ToString(ci));

            BasicTest(model, "um sobre vinte e cinco", ((double)1 / 25).ToString(ci));

            BasicTest(model, "vinte vinte e cinco avos", "0,8");

            BasicTest(model, "tres vinte e um avos", ((double)3 / 21).ToString(ci));

            BasicTest(model, "tres vinte avos", ((double)3 / 20).ToString(ci));

            BasicTest(model, "tres duzentos avos", ((double)3 / 200).ToString(ci));

            BasicTest(model, "tres dois milesimos", ((double)3 / 2000).ToString(ci));

            BasicTest(model, "tres vinte milesimos", ((double)3 / 20000).ToString(ci));

//            BasicTest(model, "tres duzentos milesimos", ((double)3 / 200000).ToString(ci));

            // act like Google
            BasicTest(model, "cento e trinta quintos", ((double)130 / 5).ToString(ci));

//            BasicTest(model, "cem trinta e cinco avos", ((double)100 / 35).ToString(ci));

            //@HERE
//            BasicTest(model, "tres e um quinto", "3,2");

            BasicTest(model, "vinte e um quintos", "4,2");

            BasicTest(model, "cento e trinta e dois cinco avos", ((double)132 / 5).ToString(ci));

            BasicTest(model, "cento e trinta e dois quintos", ((double)132 / 5).ToString(ci));

//            BasicTest(model, "um cento e cinco avos", ((double)1 / 105).ToString(ci));

//            BasicTest(model, "cem mil e cinco avos", ((double)100 / 1005).ToString(ci));

            BasicTest(model, "um sobre tres", ((double)1 / 3).ToString(ci));

            BasicTest(model, "1 sobre 21", ((double)1 / 21).ToString(ci));

            BasicTest(model, "1 sobre tres", ((double)1 / 3).ToString(ci));

            BasicTest(model, "1 sobre 3", ((double)1 / 3).ToString(ci));

            BasicTest(model, "um sobre 3", ((double)1 / 3).ToString(ci));

            BasicTest(model, "1/3", ((double)1 / 3).ToString(ci));

            BasicTest(model, "um sobre 20", ((double)1 / 20).ToString(ci));

            BasicTest(model, "um sobre vinte", ((double)1 / 20).ToString(ci));

            BasicTest(model, "um sobre cem", ((double)1 / 100).ToString(ci));

            BasicTest(model, "1 sobre cento e vinte e um", ((double)1 / 121).ToString(ci));

            BasicTest(model, "um sobre cento e trinta e cinco", ((double)1 / 135).ToString(ci));

//            BasicTest(model, "cinco meios", ((double)5 / 2).ToString(ci));

            BasicTest(model, "tres quartos", ((double)3 / 4).ToString(ci));

//            BasicTest(model, "dois tercos", ((double)2 / 3).ToString(ci));

//            BasicTest(model, "cento e trinta e cinco meios", ((double)135 / 2).ToString(ci));

//            BasicTest(model, "dez e um meio", (10 + (double)1 / 2).ToString(ci));

//            BasicTest(model, "dez e um quarto", (10 + (double)1 / 4).ToString(ci));

            // Should we also support a common informal format?
            //BasicTest(model, "um por vinte e cinco", ((double)1 / 25).ToString(ci));
        }

        private static IModel GetNumberModel()
        {
            return
                new NumberModel(
                    AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Number, new PortugueseNumberParserConfiguration()),
                    new NumberExtractor(NumberMode.PureNumber));
        }

        private static IModel GetOrdinalModel()
        {
            return
                new OrdinalModel(
                    AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Ordinal, new PortugueseNumberParserConfiguration()),
                    new OrdinalExtractor());
        }

        private static IModel GetPercentageModel()
        {
            return
                new PercentModel(
                    AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Percentage, new PortugueseNumberParserConfiguration()),
                    new PercentageExtractor());
        }
    }
}
