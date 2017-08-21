using Microsoft.Recognizers.Text.Number.Spanish;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Recognizers.Text.Number.Tests
{
    [TestClass]
    public class TestNumberSpanish
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
            var model = NumberRecognizer.Instance.GetOrdinalModel(Culture.Spanish);

            BasicTest(model, "tresmillonesimo", "3000000");

            BasicTest(model, "dos mil millonesimo", "2000000000");

            BasicTest(model, "septimo", "7");

            BasicTest(model, "cuadragesimo septimo", "47");

            BasicTest(model, "tricentesimo cuadragesimo septimo", "347");

            BasicTest(model, "dosmilesimo tricentesimo cuadragesimo septimo", "2347");

            BasicTest(model, "cincuenta y dos milesimo tricentesimo cuadragesimo septimo", "52347");

            BasicTest(model, "cuatrocientos cincuenta y dos milesimo tricentesimo cuadragesimo septimo", "452347");

            BasicTest(model, "tresmillonesimo septimo", "3000007");

            BasicTest(model, "tresmillonesimo cuadragesimo septimo", "3000047");

            BasicTest(model, "tresmillonesimo tricentesimo cuadragesimo septimo", "3000347");

            BasicTest(model, "tres millones dos milesimo tricentesimo cuadragesimo septimo", "3002347");

            BasicTest(model, "tres millones cincuenta y dos milesimo tricentesimo cuadragesimo septimo", "3052347");

            BasicTest(model, "tres millones cuatrocientos cincuenta y dos milesimo tricentesimo cuadragesimo septimo", "3452347");

            BasicTest(model, "trece millones cuatrocientos cincuenta y dos milesimo tricentesimo cuadragesimo septimo", "13452347");

            BasicTest(model, "quinientos trece millones cuatrocientos cincuenta y dos milesimo tricentesimo cuadragesimo septimo", "513452347");

            BasicTest(model, "quinientos trece millones cuatrocientos cincuenta y dos milesimo tricentesimo cuadragesimo", "513452340");

            BasicTest(model, "quinientos trece millones cuatrocientos cincuenta y dos milesimo tricentesimo", "513452300");

            BasicTest(model, "quinientos trece millones cuatrocientos cincuenta y dos milesimo", "513452000");

            BasicTest(model, "quinientos trece millones cuatrocientos cincuenta milesimo", "513450000");

            BasicTest(model, "quinientos trece millones cuatrocientos milesimo", "513400000");

            BasicTest(model, "quinientos trece millonesimo", "513000000");

            BasicTest(model, "quinientos diez millonesimo", "510000000");

            BasicTest(model, "quinientosmillonesimo", "500000000");

            BasicTest(model, "milesimo quingentesimo vigesimo tercero", "1523");

            // a little number :p
            BasicTest(model, "tres billones cuatrocientos cincuenta y cinco mil doscientos veintiocho millones quinientos cincuenta y seis milesimo octingentesimo trigesimo segundo", "3455228556832");

            BasicTest(model, "tres billones cuatrocientos cincuenta y cinco mil doscientos veintiocho millones quinientos cincuenta y seis milesimo", "3455228556000");

            BasicTest(model, "tres billones cuatrocientos cincuenta y cinco mil doscientos veintiocho millonesimo", "3455228000000");

            BasicTest(model, "tres billones cuatrocientos cincuenta y cinco mil millonesimo", "3455000000000");

            BasicTest(model, "tresbillonesimo", "3000000000000");

            BasicTest(model, "vigesimo quinto", "25");

            BasicTest(model, "vigesimo primero", "21");

            BasicTest(model, "centesimo vigesimo quinto", "125");

            BasicTest(model, "ducentesimo", "200");

            BasicTest(model, "tres mil quinientos veinticuatro millones seiscientos noventa y cuatro milesimo sexcentesimo septuagesimo tercero", "3524694673");

            BasicTest(model, "tres mil quinientos veinticuatro millones seiscientos noventa y cuatro milesimo sexcentesimo septuagesimo", "3524694670");

            BasicTest(model, "tres mil quinientos veinticuatro millones seiscientos noventa y cuatro milesimo sexcentesimo", "3524694600");

            BasicTest(model, "tres mil quinientos veinticuatro millones seiscientos milesimo", "3524600000");

            BasicTest(model, "tres mil millonesimo", "3000000000");

            BasicTest(model, "tres mil millonesimo tercero", "3000000003");

            BasicTest(model, "tres mil millonesimo septuagesimo tercero", "3000000073");

            BasicTest(model, "tres mil millonesimo sexcentesimo septuagesimo tercero", "3000000673");

            BasicTest(model, "tres mil millones cuatro milesimo sexcentesimo septuagesimo tercero", "3000004673");

            BasicTest(model, "tres mil veinticuatro millones seiscientos noventa y cuatro milesimo sexcentesimo septuagesimo tercero", "3024694673");

            BasicTest(model, "11mo", "11");

            BasicTest(model, "11vo", "11");

            BasicTest(model, "12vo", "12");

            BasicTest(model, "111ro", "111");

            BasicTest(model, "111ro", "111");

            BasicTest(model, "21ro", "21");

            BasicTest(model, "30ma", "30");

            BasicTest(model, "2da", "2");

            BasicTest(model, "undecimo", "11");

            BasicTest(model, "veintidosmilesimo", "22000");

            BasicTest(model, "cincuenta y cinco billones quinientos cincuenta y cinco mil quinientos cincuenta y cinco millones quinientos cincuenta y cinco milesimo quingentesimo quincuagesimo quinto", "55555555555555");

            BasicTest(model, "vigesimo", "20");

            BasicTest(model, "centesimo", "100");

            BasicTest(model, "tres billonesimo", "3000000000000");

            BasicTest(model, "tres billonesima", "3000000000000");

            BasicTest(model, "cien billonesimo", "100000000000000");
        }

        [TestMethod]
        public void TestNumberModel()
        {
            var ci = new SpanishNumberParserConfiguration().CultureInfo;

            var model = NumberRecognizer.Instance.GetNumberModel(Culture.Spanish);

            #region Integer numbers

            BasicTest(model, "2 mil millones", "2000000000");

            BasicTest(model, " 123456789101231", "123456789101231");

            BasicTest(model, "-123456789101231", "-123456789101231");

            BasicTest(model, " -123456789101231", "-123456789101231");

            BasicTest(model, " -1", "-1");

            BasicTest(model, "1.234.567", "1234567");

            BasicTest(model, "3 docenas", "36");

            BasicTest(model, "dos mil millones", "2000000000");

            BasicTest(model, "una docena", "12");

            BasicTest(model, "quince docenas", "180");

            BasicTest(model, "dos mil y cuatro docenas", "2048");

            BasicTest(model, "siete", "7");

            BasicTest(model, "cuarenta y siete", "47");

            BasicTest(model, "trescientos cuarenta y siete", "347");

            BasicTest(model, "dos mil trescientos cuarenta y siete", "2347");

            BasicTest(model, "cincuenta y dos mil trescientos cuarenta y siete", "52347");

            BasicTest(model, "cuatrocientos cincuenta y dos mil trescientos cuarenta y siete", "452347");

            BasicTest(model, "tres millones", "3000000");

            BasicTest(model, "tres millones siete", "3000007");

            BasicTest(model, "tres millones cuarenta y siete", "3000047");

            BasicTest(model, "tres millones trescientos cuarenta y siete", "3000347");

            BasicTest(model, "tres millones dos mil trescientos cuarenta y siete", "3002347");

            BasicTest(model, "tres millones cincuenta y dos mil trescientos cuarenta y siete", "3052347");

            BasicTest(model, "tres millones cuatrocientos cincuenta y dos mil trescientos cuarenta y siete", "3452347");

            BasicTest(model, "trece millones cuatrocientos cincuenta y dos mil trescientos cuarenta y siete", "13452347");

            BasicTest(model, "quinientos trece millones cuatrocientos cincuenta y dos mil trescientos cuarenta y siete", "513452347");

            BasicTest(model, "quinientos trece millones cuatrocientos cincuenta y dos mil trescientos cuarenta", "513452340");

            BasicTest(model, "quinientos trece millones cuatrocientos cincuenta y dos mil trescientos", "513452300");

            BasicTest(model, "quinientos trece millones cuatrocientos cincuenta y dos mil", "513452000");

            BasicTest(model, "quinientos trece millones cuatrocientos cincuenta mil", "513450000");

            BasicTest(model, "quinientos trece millones cuatrocientos mil", "513400000");

            BasicTest(model, "quinientos trece millones", "513000000");

            BasicTest(model, "quinientos diez millones", "510000000");

            BasicTest(model, "quinientos millones", "500000000");

            BasicTest(model, "mil quinientos veintitres", "1523");

            // a little number :p
            BasicTest(model, "tres billones cuatrocientos cincuenta y cinco mil doscientos veintiocho millones quinientos cincuenta y seis mil ochocientos treinta y dos", "3455228556832");

            BasicTest(model, "tres billones cuatrocientos cincuenta y cinco mil doscientos veintiocho millones quinientos cincuenta y seis mil", "3455228556000");

            BasicTest(model, "tres billones cuatrocientos cincuenta y cinco mil doscientos veintiocho millones", "3455228000000");

            BasicTest(model, "tres billones cuatrocientos cincuenta y cinco mil millones", "3455000000000");

            BasicTest(model, "tres billones", "3000000000000");

            // super number :p - supported by extractor but not by parser
            //BasicTest(GetOrdinalModel(), "ciento sesenta y tres septillones quinientos ochenta y dos mil ochocientos setenta y un sextillones ciento dieciocho mil novecientos trece quintillones quinientos ochenta y cinco mil trescientos cuarenta y seis cuatrillones novecientos noventa y siete mil doscientos setenta y tres trillones cuatrocientos treinta y cuatro mil trescientos veinticinco billones quinientos cincuenta y cinco mil ochocientos veintiún millones novecientos cincuenta y tres mil seiscientos setenta y cinco", "163582871118913585346997273434325555821953675");

            // numbers within sentences
            BasicTest(model, "dame un mil", "1000", "un mil");

            BasicTest(model, "tirate un paso", "1", "un");

            BasicTest(model, "voy a comprar solo una vaca", "1", "una");

            BasicTest(model, "voy a comprar doscientas vacas", "200", "doscientas");

            BasicTest(model, "tengo solamente mil cien pesos", "1100", "mil cien");

            BasicTest(model, "tengo solamente siete mil doscientos treinta y cinco pesos", "7235", "siete mil doscientos treinta y cinco");

            BasicTest(model, "no mucho, creo que voy a gastar algo asi como trece millones cuatrocientos cincuenta y dos mil trescientos cuarenta y siete bolivares en todo el proyecto", "13452347", "trece millones cuatrocientos cincuenta y dos mil trescientos cuarenta y siete");

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

            BasicTest(model, "siete con cincuenta", "7,5");

            BasicTest(model, "cuarenta y siete coma veintiocho", "47,28");

            BasicTest(model, "trescientos cuarenta y siete con quinientos doce", "347,512");

            BasicTest(model, "dos mil trescientos cuarenta y siete coma mil quinientos setenta y ocho", "2347,1578");

            BasicTest(model, "cincuenta y dos mil trescientos cuarenta y siete con doscientos", "52347,2");

            BasicTest(model, "cuatrocientos cincuenta y dos mil trescientos cuarenta y siete coma veintidos", "452347,22");

            BasicTest(model, "1,1^+23", "8,95430243255239");

            BasicTest(model, "2,5^-1", "0,4");

            BasicTest(model, "-2500^-1", "-0,0004");

            BasicTest(model, "-1,1^+23", "-8,95430243255239");

            BasicTest(model, "-2,5^-1", "-0,4");

            BasicTest(model, "-1,1^--23", "-8,95430243255239");

            BasicTest(model, "-127,32e13", "-1,2732E+15");

            BasicTest(model, "12,32e+14", "1,232E+15");

            BasicTest(model, "-12e-1", "-1,2");

            #endregion

            #region Translated numbers from english

            BasicTest(model, "192.", "192", "192");

            // '.' is group separator in spanish - so not understood as IP
            MultiTest(model, "192.168.1.2", 3);
            //this will be supported for the NumberWithUnitModel
            MultiTest(model, "son 180,25ml liquidos", 0);

            MultiTest(model, "son 180ml liquidos", 0);

            MultiTest(model, " 29km caminando ", 0);

            MultiTest(model, " subamos al 4to piso ", 0);

            MultiTest(model, "son ,25ml liquidos", 0);

            BasicTest(model, ",08", "0,08");

            MultiTest(model, "uno", 1);

            MultiTest(model, "un", 1);

            BasicTest(model, ",23456000", "0,23456");

            BasicTest(model, "4,800", "4,8");

            BasicTest(model, "ciento tres con dos tercios", (103 + (double)2 / 3).ToString(ci));

            BasicTest(model, "dieciseis", "16");

            BasicTest(model, "dos tercios", ((double)2 / 3).ToString(ci));

            BasicTest(model, "ciento dieciseis", "116");

            BasicTest(model, "ciento seis", "106");

            BasicTest(model, "ciento sesenta y un", "161");

            BasicTest(model, "un billonesimo", "1E-12");

            BasicTest(model, "cien billonesimos", "1E-10");

            BasicTest(model, " media   docena ", "6");

            BasicTest(model, " 3 docenas", "36");

            BasicTest(model, "una docena", "12");

            BasicTest(model, " tres docenas ", "36");

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

            BasicTest(model, "2 millones", "2000000");

            BasicTest(model, "1 billon", "1000000000000");

            BasicTest(model, " tres ", "3");

            BasicTest(model, "un billon", "1000000000000");

            BasicTest(model, "veintiun billones", "21000000000000");

            BasicTest(model, "veintiun billones trescientos", "21000000000300");

            BasicTest(model, "cincuenta   y   dos", "52");

            BasicTest(model, "trescientos   treinta  y   uno", "331");

            BasicTest(model, "doscientos dos mil", "202000");

            BasicTest(model, "dos mil doscientos", "2200");

            BasicTest(model, " 2,33 k", "2330");

            BasicTest(model, " doscientos coma cero tres", "200,03");

            BasicTest(model, " doscientos con setenta y uno", "200,71");

            BasicTest(model, "1e10", "10000000000");

            BasicTest(model, "1,1^23", "8,95430243255239");

            BasicTest(model, " 322 millones ", "322000000");

            BasicTest(model, "setenta", "70");

            BasicTest(model, "cincuenta y dos", "52");

            BasicTest(model, "2  1/4", "2,25");

            BasicTest(model, "3/4", "0,75");

            BasicTest(model, "un octavo", "0,125");

            BasicTest(model, "cinco octavos", "0,625");

            BasicTest(model, "un medio", "0,5");

            BasicTest(model, "tres cuartos", "0,75");

            BasicTest(model, "veinte con tres quintos", "20,6");

            BasicTest(model, "veintitres quintos", "4,6");

            BasicTest(model, "veintitres con tres quintos", "23,6");

            BasicTest(model, "un millon dos mil doscientos tres quintos", "200440,6");

            BasicTest(model, "uno con un medio", "1,5");

            BasicTest(model, "uno con un cuarto", "1,25");

            BasicTest(model, "cinco con un cuarto", "5,25");

            BasicTest(model, "cien con tres cuartos", "100,75");

            BasicTest(model, "un centesimo", "0,01");

            #endregion
        }

        [TestMethod]
        public void TestFractionModel()
        {
            var ci = new SpanishNumberParserConfiguration().CultureInfo;

            var model = NumberRecognizer.Instance.GetNumberModel(Culture.Spanish);

            BasicTest(model, "un quinto", "0,2");

            BasicTest(model, "un billonesimo", "1E-12");

            BasicTest(model, "cien mil billonesima", "1E-07");

            BasicTest(model, "tres quintos", "0,6");

            BasicTest(model, "veinte quintos", "4");

            BasicTest(model, "veintitres quintas", "4,6");

            BasicTest(model, "tres con un quinto", "3,2");

            BasicTest(model, "veintiun quintos", "4,2");

            BasicTest(model, "un veintiunavo", ((double)1 / 21).ToString(ci));

            BasicTest(model, "ciento treinta y tres veintiunavos", ((double)133 / 21).ToString(ci));

            BasicTest(model, "ciento treinta con tres veintiunavos", (130 + (double)3 / 21).ToString(ci));

            BasicTest(model, "veintidos treintavos", ((double)22 / 30).ToString(ci));

            BasicTest(model, "un veinticincoavo", ((double)1 / 25).ToString(ci));

            BasicTest(model, "veinte veinticincoavos", "0,8");

            BasicTest(model, "tres veintiunavos", ((double)3 / 21).ToString(ci));

            BasicTest(model, "tres veinteavos", ((double)3 / 20).ToString(ci));

            BasicTest(model, "tres doscientosavos", ((double)3 / 200).ToString(ci));

            BasicTest(model, "tres dosmilesimos", ((double)3 / 2000).ToString(ci));

            BasicTest(model, "tres veintemilesimos", ((double)3 / 20000).ToString(ci));

            BasicTest(model, "tres doscientosmilesimos", ((double)3 / 200000).ToString(ci));

            BasicTest(model, "tres dosmillonesimos", ((double)3 / 2000000).ToString(ci));

            // act like Google
            BasicTest(model, "ciento treinta quintos", ((double)130 / 5).ToString(ci));

            BasicTest(model, "cien treintaicincoavos", ((double)100 / 35).ToString(ci));

            // this is spanish can be interpreted as 130 + 2/5 or 132 / 5 - in this case go for 2nd option for simplicity
            BasicTest(model, "ciento treinta y dos cincoavos", ((double)132 / 5).ToString(ci));

            // and we go for the first option if the user writes it using 'con'
            BasicTest(model, "ciento treinta con dos cincoavos", (130 + (double)2 / 5).ToString(ci));

            BasicTest(model, "ciento treinta y dos quintos", ((double)132 / 5).ToString(ci));

            BasicTest(model, "ciento treinta con dos quintos", (130 + (double)2 / 5).ToString(ci));

            BasicTest(model, "un cientocincoavos", ((double)1 / 105).ToString(ci));

            BasicTest(model, "cien milcincoavos", ((double)100 / 1005).ToString(ci));

            BasicTest(model, "uno sobre tres", ((double)1 / 3).ToString(ci));

            BasicTest(model, "1 sobre 21", ((double)1 / 21).ToString(ci));

            BasicTest(model, "1 sobre tres", ((double)1 / 3).ToString(ci));

            BasicTest(model, "1 sobre 3", ((double)1 / 3).ToString(ci));

            BasicTest(model, "uno sobre 3", ((double)1 / 3).ToString(ci));

            BasicTest(model, "uno sobre 20", ((double)1 / 20).ToString(ci));

            BasicTest(model, "uno sobre veinte", ((double)1 / 20).ToString(ci));

            BasicTest(model, "uno sobre cien", ((double)1 / 100).ToString(ci));

            BasicTest(model, "1 sobre ciento veintiuno", ((double)1 / 121).ToString(ci));

            BasicTest(model, "uno sobre ciento treinta y cinco", ((double)1 / 135).ToString(ci));

            BasicTest(model, "cinco medios", ((double)5 / 2).ToString(ci));

            BasicTest(model, "tres cuartos", ((double)3 / 4).ToString(ci));

            BasicTest(model, "dos tercios", ((double)2 / 3).ToString(ci));

            BasicTest(model, "ciento treinta y cinco medios", ((double)135 / 2).ToString(ci));

            // not supported should be written as integer + decimal part => once con uno y medio
            //BasicTest(model, "diez con tres medios", (10 + (double)3 / 2).ToString(ci));
            BasicTest(model, "once con uno y medio", (10 + (double)3 / 2).ToString(ci));

            BasicTest(model, "diez con un medio", (10 + (double)1 / 2).ToString(ci));

            BasicTest(model, "diez con un cuarto", (10 + (double)1 / 4).ToString(ci));
        }

        [TestMethod]
        public void TestPercentageModel()
        {
            var ci = new SpanishNumberParserConfiguration().CultureInfo;

            var model = NumberRecognizer.Instance.GetPercentageModel(Culture.Spanish); ;

            #region Integer percentages

            BasicTest(model, "100%", "100%");

            BasicTest(model, " 100% ", "100%");

            BasicTest(model, " 100 por ciento", "100%");

            BasicTest(model, " cien por cien", "100%");

            BasicTest(model, "cien por ciento", "100%");

            BasicTest(model, "243 por ciento", "243%");

            BasicTest(model, "veinte por ciento", "20%");

            BasicTest(model, "treinta y cinco por ciento", "35%");

            BasicTest(model, "quinientos treinta y cinco por ciento", "535%");

            BasicTest(model, "10 por ciento", "10%");

            BasicTest(model, "diez por ciento", "10%");

            BasicTest(model, "tres millones cincuenta y dos mil trescientos cuarenta y siete por ciento", "3052347%");

            BasicTest(model, "tres millones cuatrocientos cincuenta y dos mil trescientos cuarenta y siete por ciento", "3452347%");

            BasicTest(model, "trece millones cuatrocientos cincuenta y dos mil trescientos cuarenta y siete por ciento", "13452347%");

            BasicTest(model, "quinientos trece millones cuatrocientos cincuenta y dos mil trescientos cuarenta y siete por ciento", "513452347%");

            BasicTest(model, "quinientos trece millones cuatrocientos cincuenta y dos mil trescientos cuarenta por ciento", "513452340%");

            BasicTest(model, "quinientos trece millones cuatrocientos cincuenta y dos mil trescientos por ciento", "513452300%");

            BasicTest(model, "quinientos trece millones cuatrocientos cincuenta y dos mil por ciento", "513452000%");

            // a little percentage :p
            BasicTest(model, "tres billones cuatrocientos cincuenta y cinco mil doscientos veintiocho millones quinientos cincuenta y seis mil ochocientos treinta y dos por ciento", "3455228556832%");

            // percentages within sentences
            BasicTest(model, "algo asi como un 11%", "11%", "11%");

            BasicTest(model, "claro, solamente un 15 por ciento", "15%", "15 por ciento");

            BasicTest(model, "si, nada mas un veinticinco por ciento", "25%", "veinticinco por ciento");

            BasicTest(model, "todo, dejame el cien por cien del combustible", "100%", "cien por cien");

            BasicTest(model, "un porcentaje del 25%", "25%", "25%");

            BasicTest(model, "un porcentaje del treinta y seis por ciento del total", "36%", "treinta y seis por ciento");

            BasicTest(model, "un porcentaje del ochenta y cuatro por cien solamente", "84%", "ochenta y cuatro por cien");

            #endregion

            #region Double percentages

            BasicTest(model, " 101231,2353%", "101231,2353%");

            BasicTest(model, "-101231,4323%", "-101231,4323%");

            BasicTest(model, " -89101231,5127 por ciento", "-89101231,5127%");

            BasicTest(model, " -1,1234567 por cien", "-1,1234567%");

            BasicTest(model, "1.234.567,51274 por ciento", "1234567,51274%");

            BasicTest(model, ",23456000%", "0,23456%");

            BasicTest(model, "4,800%", "4,8%");

            BasicTest(model, ",08 por ciento", "0,08%");

            BasicTest(model, "9,2321312%", "9,2321312%");

            BasicTest(model, " -9,2321312 por cien", "-9,2321312%");

            BasicTest(model, "1e10%", "10000000000%");

            BasicTest(model, "1,1^23 por ciento", "8,95430243255239%");

            BasicTest(model, "siete con cincuenta por ciento", "7,5%");

            BasicTest(model, "cuarenta y siete coma veintiocho por ciento", "47,28%");

            BasicTest(model, "trescientos cuarenta y siete con quinientos doce por ciento", "347,512%");

            BasicTest(model, "dos mil trescientos cuarenta y siete coma mil quinientos setenta y ocho por ciento", "2347,1578%");

            BasicTest(model, "cincuenta y dos mil trescientos cuarenta y siete con doscientos por ciento", "52347,2%");

            BasicTest(model, "cuatrocientos cincuenta y dos mil trescientos cuarenta y siete coma veintidos por ciento", "452347,22%");

            #endregion

            #region Fraction percentages

            BasicTest(model, "tres quintos por ciento", ((double)3 / 5).ToString(ci) + "%");

            BasicTest(model, "dos coma cinco por ciento", "2,5%");

            BasicTest(model, "un quinto por ciento", "0,2%");

            BasicTest(model, "un billonesimo por cien", "1E-12%");

            BasicTest(model, "un veintiunavo por ciento", ((double)1 / 21).ToString(ci) + "%");

            BasicTest(model, "ciento treinta y tres veintiunavos por ciento", ((double)133 / 21).ToString(ci) + "%");

            BasicTest(model, "ciento treinta con tres veintiunavos por ciento", (130 + (double)3 / 21).ToString(ci) + "%");

            BasicTest(model, "veintidos treintavos por ciento", ((double)22 / 30).ToString(ci) + "%");

            BasicTest(model, "tres dosmilesimos por ciento", ((double)3 / 2000).ToString(ci) + "%");

            BasicTest(model, "tres veintemilesimos por ciento", ((double)3 / 20000).ToString(ci) + "%");

            // act like Google
            BasicTest(model, "ciento treinta quintos por ciento", ((double)130 / 5).ToString(ci) + "%");

            BasicTest(model, "cien treintaicincoavos por ciento", ((double)100 / 35).ToString(ci) + "%");

            // this is spanish can be interpreted as 130 + 2/5 or 132 / 5 - in this case go for 2nd option for simplicity
            BasicTest(model, "ciento treinta y dos cincoavos por ciento", ((double)132 / 5).ToString(ci) + "%");

            // and we go for the first option if the user writes it using 'con'
            BasicTest(model, "ciento treinta con dos cincoavos por ciento", (130 + (double)2 / 5).ToString(ci) + "%");

            BasicTest(model, "ciento treinta y dos quintos por ciento", ((double)132 / 5).ToString(ci) + "%");

            BasicTest(model, "ciento treinta con dos quintos por ciento", (130 + (double)2 / 5).ToString(ci) + "%");

            BasicTest(model, "uno sobre tres por ciento", ((double)1 / 3).ToString(ci) + "%");

            BasicTest(model, "1 sobre 3 por ciento", ((double)1 / 3).ToString(ci) + "%");

            BasicTest(model, "3/4%", ((double)3 / 4).ToString(ci) + "%");

            BasicTest(model, "2/3%", ((double)2 / 3).ToString(ci) + "%");

            #endregion
        }
    }
}
