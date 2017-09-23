using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Recognizers.Text.DateTime.French.Tests
{
    [TestClass]
    public class TestTimePeriodExtractor
    {
        private readonly BaseTimePeriodExtractor extractor = new BaseTimePeriodExtractor(new FrenchTimePeriodExtractorConfiguration());

        public void BasicTest(string text, int start, int length)
        {
            var results = extractor.Extract(text);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(start, results[0].Start);
            Assert.AreEqual(length, results[0].Length);
            Assert.AreEqual(Constants.SYS_DATETIME_TIMEPERIOD, results[0].Type);
        }

        [TestMethod]
        public void TestTimePeriodExtract()
        {
            // basic match
            BasicTest("Je vais dehors 5 au 6 ce soir", 15, 14);
            BasicTest("Je vais dehors 5 au 6 du soir", 15, 14);
            BasicTest("Je vais dehors 5 au 6 dans l'apres midi", 15, 24);
            BasicTest("Je vais dehors 5 au sept dans le matin", 15, 23);
            BasicTest("Je vais dehors de 5 au 6 ce soir", 15, 17);
            BasicTest("Je vais dehors entre 5 au 6 dans la nuit", 15, 25);
            BasicTest("Je vais dehors entre 5 au 6 dans la soiree", 15, 27);
            BasicTest("Je vais dehors entre 5 au 6 dans l'apres midi", 15, 30);

            // merge two time points
            BasicTest("Je vais dehors 4pm au 5pm", 15, 10);
            BasicTest("Je vais dehors 4:00 jusqu'a 5pm", 15, 16);
            BasicTest("Je vais dehors 14:00 jusqu'a 15:00", 15, 19);
            BasicTest("Je vais dehors 18:00 jusqu'a 20:00", 15, 19);
            BasicTest("Je vais dehors de 4:00 et 7 heures", 15, 19);
            BasicTest("Je vais dehors 3pm a sept trente", 15, 17);

            // TODO: add support for 'half past X' - sept heures et demies (half past 7) 
            //BasicTest("Je vais dehors 3pm au sept heures et demies", 15, 18);
            //BasicTest("Je vais dehors de 4pm to half past five", 12, 26);
            //BasicTest("Je vais dehors between 4pm and half past five", 12, 30);

            BasicTest("Je vais dehors 4pm-5pm", 15, 7);
            BasicTest("Je vais dehors 4:00 -5:00", 15, 10);
            BasicTest("Je vais dehors 4:00- 5:00", 15, 10);
            BasicTest("Je vais dehors 4:00 - 5:00", 15, 11);
            BasicTest("Je vais dehors 20 minutes à trois au huit dans la soiree", 15, 41);

            BasicTest("Je vais dehors de 4pm au 5pm", 15, 13);
            BasicTest("Je vais dehors de 14:00 au 15:00", 15, 17);

            BasicTest("Je vais dehors de 4pm au cinq trente", 15, 21);
            BasicTest("Je vais dehors de 3 du matin jusqu'a 5pm", 15, 25);
            BasicTest("Je vais dehors de 3 du matinee jusqu'a cinq dans l'apres-midi", 15, 46);
            
            //BasicTest("Je vais dehors entre 3 du matin et 5pm", 15, 22);

            BasicTest("Rendez-vouz dans le matin", 12, 13);
            BasicTest("Rendez-vouz dans l'apres-midi", 12, 17);
            BasicTest("Rendez-vouz dans le nuit", 12, 12);
            BasicTest("Rendez-vouz dans la soiree", 12, 14);
            BasicTest("Rendez-vouz dans le soirée", 12, 14);

            BasicTest("Allons nous recontrer ce soir", 22, 7); // let's meet tonight
            BasicTest("Allons nous recontrer du matin", 22, 8);
            BasicTest("Rendez-vouz ce soir", 12, 7);

            BasicTest("Allons nous recontrer debut matin", 22, 11);
            BasicTest("Allons nous recontrer fin de matinee", 22, 14); // let's meet end of the morning
            BasicTest("Allons nous recontrer tard matinee", 22, 12);  // lets meet late morning
            BasicTest("Allons nous recontrer tard matin", 22, 10);
            BasicTest("Allons nous recontrer tard matinée", 22, 12);
            BasicTest("Allons nous recontrer tot matin", 22, 9); // lets meet early morning
            BasicTest("Allons nous recontrer tôt soiree", 22, 10); // let's meet early evening
            BasicTest("Allons nous recontrer tôt nuit", 22, 8); // let's meet early night
            BasicTest("Allons nous recontrer tard apres-midi", 22, 15); // lets meet late afternoon
            BasicTest("Allons nous recontrer tot apres midi", 22, 14);
            BasicTest("Allons nous recontrer tard soiree", 22, 11);
            BasicTest("Allons nous recontrer tard nuit", 22, 9);
            BasicTest("Allons nous recontrer debut nuit", 22, 10);
            BasicTest("Allons nous recontrer debut soiree", 22, 12);
        }
    }
}