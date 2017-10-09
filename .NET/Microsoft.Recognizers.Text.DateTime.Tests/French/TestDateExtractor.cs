using Microsoft.VisualStudio.TestTools.UnitTesting;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime.French.Tests
{

    [TestClass]
    public class TestDateExtractor
    {
        private readonly BaseDateExtractor extractor = new BaseDateExtractor(new FrenchDateExtractorConfiguration());

        public void BasicTest(string text, int start, int length)
        {
            var results = extractor.Extract(text);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(start, results[0].Start);
            Assert.AreEqual(length, results[0].Length);
            Assert.AreEqual(Constants.SYS_DATETIME_DATE, results[0].Type);
        }

        public void BasicTest(string text, string expectedOutput)
        {
            var results = extractor.Extract(text);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(expectedOutput, results[0].Text);
            TestWriter.Write(TestCulture.English, extractor, text, results);
        }

        public void BasicTestTwoOutputs(string text, string expectedOutput1, string expectedOutput2)
        {
            var results = extractor.Extract(text);
            Assert.AreEqual(2, results.Count);
            Assert.AreEqual(expectedOutput1, results[0].Text);
            Assert.AreEqual(expectedOutput2, results[1].Text);
            TestWriter.Write(TestCulture.French, extractor, text, results);
        }

        public void BasicTestNone(string text)
        {
            var results = extractor.Extract(text);
            Assert.AreEqual(0, results.Count);
            TestWriter.Write(TestCulture.French, extractor, text);
        }

        // use to generate the test cases sentences inside TestDateExtractWeekDayAndDayOfMonth function
        // return a day of current week which the parameter refer to
        public string CalculateWeekOfDay(int dayOfMonth)
        {
            var weekDay = "None";
            if (dayOfMonth >= 1 && dayOfMonth <= 31)
            {
                var referenceTime = DateObject.Now;
                var date = referenceTime.SafeCreateFromValue(referenceTime.Year, referenceTime.Month, dayOfMonth);
                weekDay = date.DayOfWeek.ToString();
            }

            return weekDay;
        }

        [TestMethod]
        public void TestDateExtract()
        { // in - dans - je reviens dans deux jours
            BasicTest("Je reviens en 15", 14, 2); // I'll go back in 15
            BasicTest("Je reviendrai 22 avril", 14, 8); // I'll go back april 22
            BasicTest("Je reviens Jan-1", 11, 5); // I'll go back Jan-1
            BasicTest("Je reviens Jan/1", 11, 5); // I'll go back Jan/1
            BasicTest("Je reviendrai le 2 Octobre", 17, 9); // I'll go back october 2
            BasicTest("Je reviendrai le 12 janvier 2016", 17, 15); // I'll go back January 12, 2016
            BasicTest("J'y reviendrai 12 janvier 2016", 15, 15); // I'll go back january 12 of 2016 ** consider deleting, redundant
            BasicTest("Je reviendrai le lundi 12 janvier 2016", 17, 21); // I'll go back Monday January 12th, 2016
            BasicTest("Je reviens 22/02/2016", 11, 10); // I'll go back 02/22/2016
            BasicTest("Je reviens 21/04/2016", 11, 10); // I'll go back 21/04/2016
            BasicTest("Je reviens 21/04/16", 11, 8); // I'll go back 21/04/16
            BasicTest("Je reviens 9-18-15", 11, 7); // I'll go back 9-18-15
            BasicTest("Je reviens sur 4.22", 15, 4); // I'll go back on 4.22
            BasicTest("Je reviens sur 4-22", 15, 4); // I'll return at 4-22s
            BasicTest("Je reviens sur 4.22", 15, 4); // I'll return at 4.22
            BasicTest("Je reviens sur    4/22", 18, 4);
            BasicTest("Je reviens sur 22/04", 15, 5);
            BasicTest("Je reviens sur       4/22", 21, 4);
            BasicTest("Je reviens sur 22/04", 15, 5);
            BasicTest("Je reviens sur 2015/08/12", 15, 10);
            BasicTest("Je reviens sur 11/12,2016", 15, 10);
            BasicTest("Je reviens 11/12,16", 11, 8);
            BasicTest("Je reviens 1er Jan", 11, 7);
            BasicTest("Je reviens 1-Jan", 11, 5);
            BasicTest("Je reviens 28-Nov", 11, 6);
            BasicTest("Je reviens Mer 22 Janvier", 11, 14);

            BasicTest("Je reviens 1er Janv", 11, 8);  

//          BasicTest("Je reviens Mai vingt-et-un", 11, 15);
//          BasicTest("Je reviens Mai vingt-et-un", 13, 14);
//          BasicTest("Je reviens Deuxieme aout", 11, 13);
//          BasicTest("I'll go back twenty second of June", 13, 21);

            BasicTest("Je retournerai Vendredi", 15, 8);
            BasicTest("Je vais revenier vendredi", 17, 8); // I'll come back Friday
            BasicTest("Je reviens le vendredi", 14, 8);
            BasicTest("Je reviens les vendredis", 15, 9);
            BasicTest("Je reviendrai aujourd'hui", 14, 11); // I'll return today
            BasicTest("Je vais y retourner demain", 20, 6);  // I'll go back there tomorrow
            BasicTest("J'ai alle hier ", 10, 4);      // I went back yesterday
            BasicTest("Je vais revenir avant-hier", 16, 10); // I'll go back to the day before yesterday
            BasicTest("Je vai retourner lendemain", 17, 9); // I'll go back the next day
            BasicTest("Je vai retourner apres-demain", 17, 12); // I will go back the day after tomorrow 
            BasicTest("Je reviens le lendemain", 14, 9);
            BasicTest("je vai retourner cette vendredi", 17, 14); // I will return this friday
            BasicTest("J'y reviendrai cette dimanche", 15, 14); // I will come back this Sunday
            BasicTest("Je suis retournee dimanche dernier", 18, 16);  // I went back last Sunday
            BasicTest("Je retournerai vendredi cette semaine", 15, 22); // I'll go back this week friday
            BasicTest("Je retournerai dimanche la semaine prochain", 15, 28); // I'll go back next week Sunday
            BasicTest("Je retournerai dimanche la semaine dernier", 15, 27); // I'll go back Sunday last week 
            BasicTest("Je retournerai 15 Juin 2016", 15, 12);
            BasicTest("un baseball le 10 mai", 15, 6);            

            BasicTest("Je retournerai le premier vendredi de juillet", 15, 30); // I'll go back to the first friday of july
            BasicTest("Je retournerai le premier vendredi cette mois", 15, 30); // I'll go back the first friday in this month
            BasicTest("Je retournerai le premier samedi de Dec", 15, 24);

            BasicTest("deux semaines plus tard, chris est partie", 0, 23); // Two weeks later, chris left

            BasicTest("Je retounerai vendredi prochain", 14, 17); // i'll go back next week on friday, or i'll go back on friday next week

            BasicTest("lundi dernier", 0, 13);
        }

        
        [TestMethod]
        public void TestDateExtractAgoLater()
        {
            //           BasicTest("Je suis retourne il y a deux mois", 17, 16); // I went back two months ago
            //           BasicTest("Je reviens deux jours plus tard", 11, 20); // I'll go back two days later

            // TODO: support for 'AgoRegex' as suffix 
            // Note: should be 'il y a un mois' but current BaseDateExtractor/Parser doesn't accomodate 'AgoRegex' as 'Suffix'
            // i.e - "who did I email a month ago", vs "Qui ai-je envoye il y a un mois" 

            BasicTest("Qui ai-je envoye un mois il y a", 17, 14); // who did I email a month ago
            // should be 
            // BasicTest("Qui ai-je envoye il y a un mois", 17, 14); 
        }

        [TestMethod]
        public void TestDateExtractDayOfWeek()
        {
            BasicTest("Je reviendrai Mardi", 14, 5);
            BasicTest("Je reviendrai mar. connes nouvelles.", 14, 3);
            BasicTest("Je reviendrai Mar", 14, 3);
            BasicTest("Je reviendrai Vendredi", 14, 8);
            BasicTest("Je reviendrai le vendredi", 17, 8);
            BasicTest("Je reviendrai aujourd'hui", 14, 11);      // i'll be back today
            BasicTest("Je reviendrai lendemain", 14, 9);        // i'll be back tomorrow
            BasicTest("Je reviendrai hier", 14, 4);             // i'll be back yesterday
            BasicTest("Je reviendrai avant hier", 14, 10);       // i'll be back after tomorrow
            BasicTest("Je reviendrai apres demain", 14, 12);     // i'll be back the day after tomorrow
            BasicTest("Je reviendrai le jour suivant", 14, 15); // i'll be back the next day
            BasicTest("Je reviendrai cette vendredi", 14, 14);
            BasicTest("Je reviendrai dimanche prochain", 14, 17);   // i'll be back next sunday
            BasicTest("Je reviendrai dimanche derniere", 14, 17);   // i'll be back last sunday
            BasicTest("Je reviendrai dernier jour", 14, 12);
            // BasicTest("Je reviendrai le jour", 14, 7);
            BasicTest("Je reviendrai vendredi cette semaine", 14, 22);
            BasicTest("Je reviendrai 15 Juin 2016", 14, 12);

            //BasicTest("Je reviendrai dimanche prochaine semaine", 14, 26);
            //BasicTest("Je reviendrai dimanche semaine prochaine", 14, 26);
            //BasicTest("Je reviendrai dimanche semaine dernier", 14, 24);
            //BasicTest("a baseball le onzième mai", 14, 11);
        }

        [TestMethod]
        public void TestDateExtractOdNumRelativeMonth()
        {           
            BasicTest("Je reviendrai le 20e mois prochain", 17, 17);
            BasicTest("Je reviendrai le 20e cette mois", 17, 14);
            //BasicTest("Je reviendrai le 31e mois derniere", 17, 17);
        }

        [TestMethod]
        public void TestDateExtractForThe()
        {
            BasicTest("Je suis retourne pour le 27", 17, 10);
            BasicTest("Je suis retourne pour le 27e", 17, 11);
            BasicTest("Je suis retourne pour le 27.", 17, 10);
            BasicTest("Je suis retourne pour le 27!", 17, 10);
            BasicTest("Je suis retourne pour le 27 .", 17, 10);
            BasicTest("Je suis retourne pour le seconde", 17, 15);
            BasicTest("Je suis retourne pour le vingt deux", 17, 18);

            // Note: doesn't recognize "et un"
            //BasicTest("Je suis retourne pour le trente et un", 17, 19);
        }

        [TestMethod]
        public void TestDateExtractOn()
        {
            BasicTest("Je suis retourne pour le 27e", 17, 11);
            BasicTest("Je suis retourne pour le seconde!", 17, 15);
            BasicTest("Je suis retourne pour le vingt deux?", 17, 18);
        }

        [TestMethod]
        public void TestDateExtractRelativeDayOfWeek()
        {
            BasicTest("Je reviendrai seconde Dimanche", 14, 16);
            BasicTest("Je reviendrai 1er Dimanche", 14, 12);
            BasicTest("Je reviendrai troisieme Mardi", 14, 15);
            BasicTest("Je reviendrai cinquieme Dimanche", 14, 18);
        }

        [TestMethod]
        public void TestDateExtractRelativeDayOfWeekSingle()
        {
            // For ordinary number>5, only the DayOfWeek should be extracted
            BasicTest("Je reviendrai sixieme Dimanche", 22, 8);
            BasicTest("Je reviendrai dixieme Lundi", 22, 5);
        }


        [TestMethod]
        public void TestDateExtractWeekDayAndDayOfMonthMerge()
        {
            //Need to calculate the DayOfWeek by the date
            //Example: What do I have on Wednesday the second?

            // Note: Works but dateobject returned from CalculateWeekOfDay in english
            //BasicTest("une réunion pour " + CalculateWeekOfDay(27) + " le 27e avec Joe Smith",
            //    CalculateWeekOfDay(27) + " le 27e");
        }
    }
}