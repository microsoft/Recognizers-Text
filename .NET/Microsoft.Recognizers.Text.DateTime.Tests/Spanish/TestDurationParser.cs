using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Recognizers.Text.DateTime.Spanish.Tests
{
    [TestClass]
    public class TestDurationParser
    {
        readonly BaseDurationExtractor extractor = new BaseDurationExtractor(new SpanishDurationExtractorConfiguration());
        readonly IDateTimeParser parser = new BaseDurationParser(new SpanishDurationParserConfiguration(new SpanishCommonDateTimeParserConfiguration()));

        [ClassCleanup]
        public static void ClassCleanup()
        {
            TestWriter.Close("Spa", typeof(BaseDurationParser));
        }


        public void BasicTest(string text, double value, string luisValue)
        {
            var er = extractor.Extract(text);
            Assert.AreEqual(1, er.Count);
            var pr = parser.Parse(er[0], new System.DateTime(2016, 11, 7));
            Assert.AreEqual(Constants.SYS_DATETIME_DURATION, pr.Type);
            Assert.AreEqual(value, ((DateTimeResolutionResult) pr.Value).FutureValue);
            Assert.AreEqual(luisValue, ((DateTimeResolutionResult) pr.Value).Timex);
            TestWriter.Write("Spa", parser, text, pr);
        }

        [TestMethod]
        public void TestDurationParse()
        {
            BasicTest("Me voy por 3h", 10800, "PT3H");
            BasicTest("Me voy por 3días", 259200, "P3D");
            BasicTest("Me voy por 3,5 años", 110376000, "P3.5Y");

            BasicTest("Me voy por 3 h", 10800, "PT3H");
            BasicTest("Me voy por 3 horas", 10800, "PT3H");
            BasicTest("Me voy por 3 hrs", 10800, "PT3H");
            BasicTest("Me voy por 3 hr", 10800, "PT3H");
            BasicTest("Me voy por 3dias", 259200, "P3D");
            BasicTest("Me voy por 3 dias", 259200, "P3D");
            BasicTest("Me voy por 3 meses", 7776000, "P3M");
            BasicTest("Me voy por 3 minutos", 180, "PT3M");
            BasicTest("Me voy por 3 min", 180, "PT3M");
            BasicTest("Me voy por 3,5 segundos ", 3.5, "PT3.5S");
            BasicTest("Me voy por 123,45 seg", 123.45, "PT123.45S");
            BasicTest("Me voy por dos semanas", 1209600, "P2W");
            BasicTest("Me voy 20 minutos", 1200, "PT20M");
            BasicTest("Me voy por veinticuatro horas", 86400, "PT24H");

            BasicTest("Me voy todo el dia", 86400, "P1D");
            BasicTest("Me voy toda la semana", 604800, "P1W");
            BasicTest("Me voy por todo el mes", 2592000, "P1M");
            BasicTest("Me voy por todo el año", 31536000, "P1Y");

            BasicTest("Me voy por una hora", 3600, "PT1H");
            BasicTest("Me voy por un hora", 3600, "PT1H");
        }
    }
}