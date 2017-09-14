using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Recognizers.Text.DateTime.Spanish.Tests
{
    [TestClass]
    public class TestTimeExtractor
    {
        private readonly BaseTimeExtractor extractor = new BaseTimeExtractor(new SpanishTimeExtractorConfiguration());

        [ClassCleanup]
        public static void ClassCleanup()
        {
            TestWriter.Close("Spa", typeof(BaseTimeExtractor));
        }


        public void BasicTest(string text, int start, int length, int expected = 1)
        {
            var results = extractor.Extract(text);
            Assert.AreEqual(expected, results.Count);

            if (expected < 1)
            {
                TestWriter.Write("Spa", extractor, text);
                return;
            }

            Assert.AreEqual(start, results[0].Start);
            Assert.AreEqual(length, results[0].Length);
            Assert.AreEqual(Constants.SYS_DATETIME_TIME, results[0].Type);
            TestWriter.Write("Spa", extractor, text, results);
        }

        public void BasicNegativeTest(string text)
        {
            BasicTest(text, -1, -1, 0);
        }

        [TestMethod]
        public void TestTimeExtract()
        {
            BasicTest("Volvere a las 7", 14, 1);
            BasicTest("Volvere a las siete", 14, 5);
            BasicTest("Volvere a las 7pm", 14, 3);
            BasicTest("Volvere a las 7p.m.", 14, 5);
            BasicTest("Volvere a las 7:56pm", 14, 6);
            BasicTest("Volvere a las 7:56:35pm", 14, 9);
            BasicTest("Volvere a las 7:56:35 pm", 14, 10);
            BasicTest("Volvere a las 12:34", 14, 5);
            BasicTest("Volvere a las 12:34:20", 14, 8);
            BasicTest("Volvere a las T12:34:20", 14, 9);
            BasicTest("Volvere a las 00:00", 14, 5);
            BasicTest("Volvere a las 00:00:30", 14, 8);

            BasicTest("Son las 7 en punto", 8, 10);
            BasicTest("Son las siete en punto", 8, 14);
            BasicTest("Son las 8 de la mañana", 8, 14);
            BasicTest("Son las 8 de la tarde", 8, 13);
            BasicTest("Son las 8 de la noche", 8, 13);

            BasicTest("Son las ocho y media", 8, 12);
            BasicTest("Son las 8pm y media", 8, 11);
            BasicTest("Son 30 mins pasadas las ocho", 4, 24);
            BasicTest("Son las ocho y cuarto", 8, 13);
            BasicTest("Son cuarto pasadas las ocho", 4, 23);
            BasicTest("Son cuarto pasadas de las ocho", 4, 26);
            BasicTest("Son 9pm menos cuarto", 4, 16);
            BasicTest("Faltan 3 minutos para las ocho", 7, 23);

            BasicTest("Son siete y media en punto", 4, 22);
            BasicTest("Son siete y media de la tarde", 4, 25);
            BasicTest("Son siete y media de la mañana", 4, 26);
            BasicTest("Son ocho menos veinticinco de la mañana", 4, 35);
            BasicTest("Son ocho menos veinte de la mañana", 4, 30);
            BasicTest("Son ocho menos cuarto de la mañana", 4, 30);
            BasicTest("Son ocho menos diez de la mañana", 4, 28);
            BasicTest("Son ocho menos cinco de la mañana", 4, 29);
            BasicTest("Son 20 min pasadas las ocho de la tarde", 4, 35);

            BasicTest("Volvere por la tarde a las 7", 8, 20);
            BasicTest("Volvere a la tarde a las 7", 8, 18);
            BasicTest("Volvere a la tarde a las 7:00", 8, 21);
            BasicTest("Volvere a la tarde a las 7:00:14", 8, 24);
            BasicTest("Volvere a la tarde a las siete pm", 8, 25);
            BasicTest("Volvere a las siete treinta pm", 14, 16);
            BasicTest("Volvere a las siete treinta y cinco pm", 14, 24);
            BasicTest("Volvere a las once y cinco", 14, 12);

            BasicTest("Volvere tres minutos para las cinco treinta", 8, 35);
            BasicTest("Volvere tres minutos antes de las cinco treinta", 8, 39);
            BasicTest("Volvere a las cinco treinta de la noche", 14, 25);
            BasicTest("Volvere a las cinco treinta en la noche", 14, 25);
            BasicTest("Volvere a las cinco treinta de la madrugada", 14, 29);

            BasicTest("Volvere a la madrugada", 8, 14);
            BasicTest("Volvere a la mañana", 8, 11);
            BasicTest("Volvere al mediodia", 8, 11);
            BasicTest("Volvere al medio dia", 8, 12);
            BasicTest("Volvere a la tarde", 8, 10);
            BasicTest("Volvere al noche", 8, 8);

            BasicTest("Volvere 340pm", 8, 5);
            BasicTest("Volvere 1140 a.m.", 8, 9);
        }

        [TestMethod]
        public void TestDatePeriodExtractNegativeCase()
        {

            var sentence = "no hay ninguna pm string despues de la hora";
            BasicNegativeTest(sentence);

        }

    }
}
