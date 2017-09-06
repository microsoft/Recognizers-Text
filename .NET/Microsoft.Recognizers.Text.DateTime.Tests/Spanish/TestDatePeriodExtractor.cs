using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Recognizers.Text.DateTime.Spanish.Tests
{
    [TestClass]
    public class TestDatePeriodExtractor
    {
        private readonly BaseDatePeriodExtractor extractor = new BaseDatePeriodExtractor(new SpanishDatePeriodExtractorConfiguration());

        public void BasicTest(string text, int start, int length)
        {
            var results = extractor.Extract(text);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(start, results[0].Start);
            Assert.AreEqual(length, results[0].Length);
            Assert.AreEqual(Constants.SYS_DATETIME_DATEPERIOD, results[0].Type);
        }

        [TestMethod]
        public void TestDatePeriodExtract()
        {
            // test basic cases
            BasicTest("Estare afuera desde el 4 hasta el 22 de este mes", 14, 34);
            BasicTest("Estare afuera desde 4 hasta 22 de este mes", 14, 28);
            BasicTest("Estare afuera desde 4-23 del proximo mes", 14, 26);
            BasicTest("Estare afuera desde el 3 hasta el 12 de Sept jajaja", 14, 30);
            BasicTest("Estare afuera 4 hasta 23 del proximo mes", 14, 26);
            BasicTest("Estare afuera desde el 4 hasta el 23 de este mes", 14, 34);
            BasicTest("Estare afuera entre 4 y 22 este mes", 14, 21);
            BasicTest("Estare afuera entre el 3 y el 12 de Set jajaja", 14, 25);
            BasicTest("Estare afuera del 4 al 22 de enero, 2017", 14, 26);
            BasicTest("Estare afuera entre 4-22 enero, 2017", 14, 22);

            BasicTest("Estare afuera esta semana", 14, 11);
            BasicTest("Estare afuera en Septiembre", 14, 13);
            BasicTest("Estare afuera este Septiembre", 14, 15);
            BasicTest("Estare afuera el ultimo sept", 17, 11);
            BasicTest("Estare afuera el proximo junio", 17, 13);
            BasicTest("Estare afuera en junio 2016", 14, 13);
            BasicTest("Estare afuera en junio del proximo año", 14, 24);
            BasicTest("Estare afuera este fin de semana", 14, 18);
            BasicTest("Estare afuera la tercera semana de este mes", 14, 29);
            BasicTest("Estare afuera la ultima semana de julio", 14, 25);

            BasicTest("Estare afuera los proximos 3 dias", 18, 15);
            BasicTest("Estare afuera los proximos 3 meses", 18, 16);
            BasicTest("Estare afuera en 3 años", 14, 9);
            BasicTest("Estuve afuera las pasadas 3 semanas", 18, 17);
            BasicTest("Estuve afuera los ultimos 3años", 18, 13);
            BasicTest("Estuve afuera las anteriores 3 semanas", 18, 20); 

            // test merging two time points
            BasicTest("Estare afuera el 2 de Oct hasta 22 de Octubre", 17, 28);
            BasicTest("Estare afuera el 12 de Enero, 2016 - 22/02/2016", 17, 30);
            BasicTest("Estare afuera el 1ro de Ene hasta Mi, 22 de Ene", 17, 30);
            BasicTest("Estare afuera hoy hasta mañana", 14, 16);
            BasicTest("Estare afuera hoy hasta 22 de Octubre", 14, 23);
            BasicTest("Estare afuera 2 de Oct hasta el dia despues de mañana", 14, 39);
            BasicTest("Estare afuera hoy hasta proximo domingo", 14, 25); 
            BasicTest("Estare afuera este viernes hasta siguiente domingo", 14, 36); 

            BasicTest("Estare afuera desde 2 de Oct hasta 22 de Octubre", 14, 34); 
            BasicTest("Estare afuera desde 2015/08/12 hasta 22 de Octubre", 14, 36); 
            BasicTest("Estare afuera desde hoy hasta mañana", 14, 22); 
            BasicTest("Estare afuera desde este viernes hasta proximo domingo", 14, 40); 
            BasicTest("Estare afuera entre 2 de Oct y 22 de Octubre", 14, 30); 

            BasicTest("Estare afuera 19-20 de Noviembre", 14, 18);
            BasicTest("Estare afuera 19 hasta 20 de Noviembre", 14, 24);
            BasicTest("Estare afuera entre 19 y 20 de Noviembre", 14, 26);

            BasicTest("Estare afuera el tercer cuatrimestre de 2016", 14, 30);
            BasicTest("Estare afuera el tercer cuatrimestre de este año", 14, 34);
            BasicTest("Estare afuera 2016 el tercer cuatrimestre", 14, 27);

            BasicTest("Estare afuera 2015.3", 14, 6);
            BasicTest("Estare afuera 2015-3", 14, 6);
            BasicTest("Estare afuera 2015/3", 14, 6);
            BasicTest("Estare afuera 3/2015", 14, 6);

            BasicTest("Estare afuera la tercer semana de 2027", 14, 24);
            BasicTest("Estare afuera la tercer semana proximo año", 14, 28);

            BasicTest("Estare afuera este verano", 14, 11);
            BasicTest("Estare afuera la siguiente primavera", 17, 19);
            BasicTest("Estare afuera el verano", 14, 9);
            BasicTest("Estare afuera verano", 14, 6);
            BasicTest("Estare afuera verano 2016", 14, 11);
            BasicTest("Estare afuera verano del 2016", 14, 15);

            //TODO: add tests for week of and month of
        }
    }
}
