using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Recognizers.Text.DateTime.French.Tests
{

    [TestClass]
    public class TestDatePeriodExtractor
    {

        private readonly BaseDatePeriodExtractor extractor =
            new BaseDatePeriodExtractor(new FrenchDatePeriodExtractorConfiguration());

        public void BasicTest(string text, int start, int length, int expected = 1)
        {
            var results = extractor.Extract(text);
            Assert.AreEqual(expected, results.Count);

            if (expected < 1)
            {
                return;
            }

            Assert.AreEqual(start, results[0].Start);
            Assert.AreEqual(length, results[0].Length);
            Assert.AreEqual(Constants.SYS_DATETIME_DATEPERIOD, results[0].Type);
        }

        public void BasicNegativeTest(string text)
        {
            BasicTest(text, -1, -1, 0);
        }

        private readonly string[] shortMonths = {
            "Jan", "Fev", "Mar", "Avr", "Mai", "Jun", "Jul", "Aout", "Sep", "Oct", "Nov", "Dec"
        };

        private readonly string[] fullMonths = {
            "Janvier", "Fevrier", "Mars", "Avril", "Mai", "Juin", "Juillet", "Aout", "Septembre", "Octobre",
            "Novembre", "Decembre"
        };

        [TestMethod]
        public void TestDatePeriodExtract()
        {

            foreach (var month in shortMonths)
            {
                BasicTest($"Je serai dans {month}", 14, month.Length);
                BasicTest($"je vai cette {month}", 7, 6 + month.Length);
                BasicTest($"J'etais disparue {month} 2001", 17, 5 + month.Length); // I was missing... 
                BasicTest($"J'etais disparue {month}, 2001", 17, 6 + month.Length);
            }

            foreach (var month in fullMonths)
            {
                BasicTest($"Je serai {month}", 9, month.Length);
                BasicTest($"Je vai cette {month}", 7, 6 + month.Length);
                BasicTest($"J'etais disparue {month} 2001", 17, 5 + month.Length);
                BasicTest($"J'etais disparue {month}, 2001", 17, 6 + month.Length);
            }
        }

        [TestMethod]
        public void TestDatePeriodExtractBasicCases()
        {
            BasicTest("Je serai dehors de 4 à 22 ce mois", 16, 17);
 //         BasicTest("Je serai dehors 23-4 cette mois", 16, 23);
            BasicTest("Je serai dehors 3 jusqu'a 12 Sep hahaha", 16, 16);
            BasicTest("Je serai dehors 4 à 23 mois prochain", 16, 20);
            BasicTest("Je serai dehors 4 jusqu'a 23 cette mois", 16, 23);
            BasicTest("Je serai dehors entre 4 et 22 cette mois", 22, 18);
            BasicTest("Je serai dehors entre 3 et 12 de Sept hahaha", 22, 15);
            BasicTest("Je serai dehors entre septembre 4 à septembre 8", 21, 26);
            BasicTest("Je serai dehors entre Novembre 15 jusqu'a 19", 22, 22);
            BasicTest("Je serai dehors entre Novembre 15 avant 19", 22, 20);
//          BasicTest("Je serai dehors entre Nov le 15 au 19", 22, 20); // one space off, 
            BasicTest("Je serai dehors entre 4 à 22 ce mois", 22, 14);
//          BasicTest("Je serai dehors entre 4 à 22 Janvier, 2017", 22, 20); 
            BasicTest("Je serai dehors entre 4-22 Janv, 2017", 22, 15);

            BasicTest("Je serai dehors dans cette semaine", 21, 13);
            BasicTest("Je serai dehors Septembre", 16, 9);
            BasicTest("Je serai dehors cette Septembre", 16, 15);
//          BasicTest("Je serai dehors sept dernier", 16, 12); // OneWordPeriodRegex - to fix
//          BasicTest("Je serai dehors jun prochain", 16, 12);  ** Month + suffix (dernier/prochain) has issue
            BasicTest("Je serai dehors juin 2016", 16, 9);
//          BasicTest("Je serai dehors juin annee prochain", 16, 19);
            BasicTest("Je serai dehors cette weekend", 16, 13);
//            BasicTest("Je serai dehors le 3 semaine de cette mois", 19, 23);
 //           BasicTest("I'll be out the last week of july", 12, 21);
        }

        // **Most Difficult
        [TestMethod]
        public void TestDatePeriodExtractDuration()
        {
//            BasicTest("je serai dehors 3 jours prochain", 16, 16); // I wil be out next 3 days
            BasicTest("Je serai dehors cette 3 mois", 16, 12);
            BasicTest("I'll be out in 3 year", 12, 9);
            BasicTest("I'll be out in 3 years", 12, 10);
            BasicTest("I'll be out in 3 weeks", 12, 10);
            BasicTest("I'll be out in 3 months", 12, 11);
            BasicTest("I'll be out past 3 weeks", 12, 12);
            BasicTest("I'll be out last 3year", 12, 10);
            BasicTest("I'll be out last year", 12, 9);
            BasicTest("I'll be out past month", 12, 10);
            BasicTest("I'll be out previous 3 weeks", 12, 16);

            BasicTest("past few weeks", 0, 14);
            BasicTest("past several days", 0, 17);
        }

        [TestMethod]
        public void TestDatePeriodExtractMErgingTwoTimepoints()
        {
            // test merging two time points
 //           BasicTest("Je serais dehors Oct. 2 au Octobre 22", 17, 20); // one space ahead at 16...
//            BasicTest("Je serais dehors 12 Janvier, 2016 - 22/02/2016", 17, 29); // also one space too early...
//            BasicTest("Je serais dehors 1er Jan jusqu'a 22 Mer, de Jan", 17, 30);
            BasicTest("I'll be out today till tomorrow", 12, 19);
            BasicTest("I'll be out today to October 22", 12, 19);
            BasicTest("I'll be out Oct. 2 until the day after tomorrow", 12, 35);
            BasicTest("I'll be out today until next Sunday", 12, 23);
            BasicTest("I'll be out this Friday until next Sunday", 12, 29);

            BasicTest("I'll be out from Oct. 2 to October 22", 12, 25);
            BasicTest("I'll be out from 2015/08/12 until October 22", 12, 32);
            BasicTest("I'll be out from today till tomorrow", 12, 24);
            BasicTest("I'll be out from this Friday until next Sunday", 12, 34);
            BasicTest("I'll be out between Oct. 2 and October 22", 12, 29);

            BasicTest("I'll be out November 19-20", 12, 14);
            BasicTest("I'll be out November 19 to 20", 12, 17);
            BasicTest("I'll be out November between 19 and 20", 12, 26);

            BasicTest("I'll be out the third quarter of 2016", 12, 25);
            BasicTest("I'll be out the third quarter this year", 12, 27);
            BasicTest("I'll be out 2016 the third quarter", 12, 22);

            BasicTest("I'll be out 2015.3", 12, 6);
            BasicTest("I'll be out 2015-3", 12, 6);
            BasicTest("I'll be out 2015/3", 12, 6);
            BasicTest("I'll be out 3/2015", 12, 6);

            BasicTest("I'll be out the third week of 2027", 12, 22);
            BasicTest("I'll be out the third week next year", 12, 24);
        }

        [TestMethod]
        public void TestDatePeriodExtractSeason()
        {
            BasicTest("Je vais partir cette été", 15, 9); // I will leave this summer
  //          BasicTest("Je vais partir printemps prochain", 15, 18); // I will leave next spring
            BasicTest("Je vais partir le été", 18, 3);
            BasicTest("Je vais partir été", 15, 3);
            BasicTest("Je vais partir été 2016", 15, 8);
            BasicTest("Je vais partir été de 2016", 15, 11);
        }

        [TestMethod]
        public void TestDatePeriodExtractNextAndUpcoming()
        {
            //next and upcoming
            BasicTest("upcoming month holidays", 0, 14);
            BasicTest("next month holidays", 0, 10);
        }

        [TestMethod]
        public void TestDatePeriodExtractWeekOf()
        {
            //test week of and month of
            BasicTest("semaine de 15.septembre", 0, 23);
            BasicTest("mois de septembre.15", 0, 23);
        }

        [TestMethod]
        public void TestDatePeriodExtractOver()
        {
            // over the weekend = this weekend
            BasicTest("Je vais partir dans la weekend", 15, 15);
        }

    }

}