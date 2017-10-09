using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Recognizers.Text.DateTime.French.Tests
{
    [TestClass]
    public class TestDateTimeExtractor
    {
        private readonly BaseDateTimeExtractor extractor = new BaseDateTimeExtractor(new FrenchDateTimeExtractorConfiguration());

        public void BasicTest(string text, int start, int length)
        {
            var results = extractor.Extract(text);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(start, results[0].Start);
            Assert.AreEqual(length, results[0].Length);
            Assert.AreEqual(Constants.SYS_DATETIME_DATETIME, results[0].Type);
        }

        [TestMethod]
        public void TestDateTimeExtract()
        {
            //Note: French don't typically use am/pm, 24:00 is standard, consider removing am/pm

            BasicTest("Je reviendrai maintenant", 14, 10);
            BasicTest("Je reviendrai dès que possible", 14, 16); // i'll be back as soon as possible
            BasicTest("Je reviendrai dqp", 14, 3);
            BasicTest("Je reviendrai en ce moment", 17, 9);

            BasicTest("Je reviendrai 21/04/2016, 8:00pm", 14, 18);
            BasicTest("Je reviendrai 21/04/2016, 8:00:13pm", 14, 21);
            
            BasicTest("Je reviendrai 14 Octobre 8:00am", 14, 17);
            BasicTest("Je reviendrai 14 Octobre 8:00:00am", 14, 20);
            BasicTest("Je reviendrai 14 Octobre, 8:00am", 14, 18);
            BasicTest("Je reviendrai 14 Octobre, 8:00:01am", 14, 21);
            BasicTest("Je reviendrai 14 Octobre, 15:00", 14, 17);
            BasicTest("Je reviendrai 15:00, 14 Octobre", 14, 17);
            BasicTest("Je reviendrai 15:00, 14 Oct", 14, 13);
            BasicTest("Je reviendrai 15:00, 14 Oct.", 14, 13);
            BasicTest("Je vais demain 8:00", 8, 11);
            BasicTest("Je vais demain vers 8:00am", 8, 18); 
            BasicTest("Je vais demain pour 8:00am", 8, 18);
            BasicTest("Je vais demain 8:00:05am", 8, 16);

            BasicTest("Je reviendrai 8pm dimanche prochain", 14, 21);
            BasicTest("Je reviendrai 8pm aujourd'hui", 14, 15);
            //BasicTest("Je reviendrai une quart a sept lendemain", 14, 26);
            BasicTest("Je reviendrai 19:00, 2016-12-22", 14, 17);
            BasicTest("Je reviendrai sept heures lendemain", 14, 21);


            BasicTest("je reviendrai 14 Octobre 8:00, 14 Octobre", 14, 15);
            BasicTest("je reviendrai 7, cette matin", 14, 14);

            BasicTest("Je reviendrai 8pm dans la nuit, Lundi", 14, 23);
            BasicTest("je reviendrai 8pm dans la soiree, 1er Jan", 14, 27);
            BasicTest("Je reviendrai 8pm dans la soirée, 1 Jan", 14, 25);
            BasicTest("Je reviendrai 10 ce soir", 14, 10);

            //BasicTest("Je reviendrai 8pm dans la soirée", 14, 18); // unsure why this doesn't work but above does
            //BasicTest("Je reviendrai 20 dans la soiree", 14, 20);

            BasicTest("Je reviendrai 8 ce soir", 14, 9);
            BasicTest("Je reviendrai ce soir vers 7", 14, 14);

            BasicTest("Je reviendrai cette matin a 7", 14, 15);
            BasicTest("Je reviendrai cette matinee a 7", 14, 17);  
            BasicTest("Je reviendrai cette matinée a sept", 14, 20);
            BasicTest("Je reviendrai cette matin à 7", 14, 15);
            BasicTest("Je reviendrai ce matin 7:00", 14, 10);
            BasicTest("Je reviendrai cette nuit a 7", 14, 14);
            BasicTest("Je reviendrai ce soir à 7", 14, 11);

            BasicTest("Je reviendrai fin de dimanche", 14, 15);
            BasicTest("Je reviendrai 2016-12-16T12:23:59", 14, 19);
            BasicTest("Je reviendrai dans 5 heures", 14, 13); //dateregexlist --> IEnumerable<Regex> IDateExtractorConfiguration.DateRegexList => DateRegexList

            //BasicTest("Je reviendrai cette vendredi a 3 et quart ", 14, 27);
            //BasicTest("Je reviendrai 5 Mai, 2016, 20 min apres huit dans la nuit", 14, 43);

            //BasicTest("Je reviendrai 8pm a 15", 14, 8);
            //BasicTest("Je reviendrai sept en 15", 14, 10);

            //BasicTest("Je reviendrai sur 15 a 8:00", 15, 9);
            //BasicTest("Je reviendrai sur 15 à 8:00:30", 18, 12);
            //BasicTest("Je reviendrai sur 15, 8pm", 18, 7);
            //BasicTest("Je reviendrai 23 Oct a 7", 14, 13);
            //BasicTest("Je reviendrai a 5 a 4 a.m.", 16, 10);
            //BasicTest("pour 2 personnes ce soir a 9:30 pm", 17, 17);
            //BasicTest("pour 2 personnes ce soir à 9:30:31 pm", 17, 20);

            //BasicTest("Je reviendrai landemain matin à 7", 14, 19);
            //BasicTest("Je reviendrai 7:00 en Dimanche l'apres-midi", 14, 29);
            //BasicTest("Je reviendrai vingt minutes apres cinq demain matin", 14, 37);

            //BasicTest("je reviendrai le fin de la journee", 14, 20); // timeofdayregex
            //BasicTest("Je reviendrai la fin de landemain", 14, 19);
        }
    }
}