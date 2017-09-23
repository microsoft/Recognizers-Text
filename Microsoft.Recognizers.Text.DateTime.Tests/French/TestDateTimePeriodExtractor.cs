using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Recognizers.Text.DateTime.French.Tests
{
    [TestClass]
    public class TestDateTimePeriodExtractor
    {
        private readonly IExtractor extractor = new BaseDateTimePeriodExtractor(new FrenchDateTimePeriodExtractorConfiguration());

        public void BasicTest(string text, int start, int length)
        {
            var results = extractor.Extract(text);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(start, results[0].Start);
            Assert.AreEqual(length, results[0].Length);
            Assert.AreEqual(Constants.SYS_DATETIME_DATETIMEPERIOD, results[0].Type);
        }

        [TestMethod]
        public void TestDateTimePeriodExtract()
        {
            // basic match
            BasicTest("Je serai sorti cinq au sept aujourd'hui", 15, 24); // I'll be out five to seven today
            BasicTest("Je serai sorti cinq à sept demain", 15, 18);
            BasicTest("Je serai sorti de cinq au sept dimanche prochain", 15, 33);
            BasicTest("Je serai sorti de 5 au 6pm dimanche prochain", 15, 29);

            BasicTest("Je serai sorti de 4pm à 5pm aujourd'hui", 15, 24);
            BasicTest("Je serai sorti de 4pm aujourd'hui à 5pm demain", 15, 31);
            BasicTest("Je serai sorti de 4pm à 5pm lendemain", 15, 22);
            BasicTest("Je serai sorti de 4pm à 5pm de 2017-6-6", 15, 24);
            BasicTest("Je serai sorti de 4pm à 5pm 5 Mai, 2018", 15, 24);
            BasicTest("Je serai sorti de 4:00 a 5pm 5 Mai, 2018", 15, 25);
            BasicTest("Je serai sorti de 4pm 1 Jan, 2016 a 5pm aujourd'hui", 15, 36);
            //BasicTest("Je serai sorti de 2:00pm, 2016-21-1 a 3:32, 23/04/2016", 15, 39);
            //BasicTest("Je serai sorti aujourd'hui a 4 jusqu'a cette Mer a 5", 15, 37);

            BasicTest("Je serai sorti entre 4pm et 5pm aujourd'hui", 15, 28);

            BasicTest("Je reviendrai ce soir", 14, 7);
            BasicTest("Je reviendrai cette nuit", 14, 10);
            BasicTest("Je reviendrai cette soirée", 14, 12);
            BasicTest("Je reviendrai ce matin", 14, 8);
            BasicTest("Je reviendrai cette d'apres-midi", 14, 18);

            // **TODO: Needs handling for Relative 'Suffix' 
            //- prochain(next) and dernier(last) go AFTER date, aren't prefixes
            //BasicTest("Je reviendrai le nuit prochain", 14, 13);
            //BasicTest("Je reviendrai nuit dernier", 14, 12);
            
            BasicTest("Je reviendrai nuit lendemain", 14, 14);
            // i'll be back this monday afternoon
            BasicTest("Je reviendrai cette Lundi l'apres-midi", 14, 24); 

            BasicTest("Je reviendrai nuit 5 Mai", 14, 10);

            // some Language cases, dernier/prochain are prefix 
            BasicTest("Je reviendrai derniere 3 minute", 14, 17);

            // TODO: switch position with 'SetLastRegex' so X 'past' minute is recognized
            //BasicTest("Je reviendrai les 3 dernière minutes", 18, 18);
            BasicTest("Je reviendrai derniere 3 minute", 14, 17);
            BasicTest("Je reviendrai derniere 3mins", 14, 14);
            BasicTest("Je reviendrai prochain 5 hrs", 14, 14);
            BasicTest("Je reviendrai derniere minute", 14, 15);
            BasicTest("Je reviendrai prochaine heures", 14, 16);
            BasicTest("Je reviendrai derniere quel ques minutes", 14, 26);
            BasicTest("Je reviendrai derniere plusieur minutes", 14, 25);

            BasicTest("Je reviendrai mardi dans le matin", 14, 19);
            BasicTest("Je reviendrai mardi l'apres-midi", 14, 18);
            BasicTest("Je reviendrai mardi l'apres midi", 14, 18);
            BasicTest("Je reviendrai mardi l'après midi", 14, 18);
            BasicTest("Je reviendrai mardi dans la nuit", 14, 18);
            BasicTest("Je reviendrai mardi dans la soiree", 14, 20);
            BasicTest("Je reviendrai mardi dans la soirée", 14, 20);
            BasicTest("Je reviendrai mardi du soir", 14, 13);

            // early/late date time
            BasicTest("rencontrons-nous dans tôt le matin Mardi", 22, 18);
            BasicTest("rencontrons-nous dans le tard matin Mardi", 17, 24);
            BasicTest("rencontrons-nous dans le début d'après-midi Mardi", 17, 32);
            BasicTest("rencontrons-nous dans tot d'après-midi Mardi", 22, 22);
            BasicTest("rencontrons-nous dans le tard d'apres-midi Mardi", 17, 31);
            BasicTest("rencontrons-nous dans tôt le soir mardi", 22, 17);
            BasicTest("rencontrons-nous dans fin de soirée mardi", 22, 19);
            BasicTest("rencontrons-nous dans le tôt le soiree mardi", 17, 27);
            BasicTest("rencontrons-nous dans le tard nuit Mardi", 17, 23);
            BasicTest("rencontrons-nous dans le tôt nuit mardi", 17, 22);
            BasicTest("rencontrons-nous dans fin de nuit mardi", 22, 17);

            BasicTest("rencontrons-nous dans tot le matin Mardi", 22, 18);
            BasicTest("rencontrons-nous dans tard matin Mardi", 22, 16);
            BasicTest("rencontrons-nous dans début d'après-midi Mardi", 22, 24);
            BasicTest("rencontrons-nous dans tard d'apres-midi Mardi", 22, 23);
            BasicTest("rencontrons-nous dans debut soiree mardi", 22, 18);
            BasicTest("rencontrons-nous dans fin de soiree mardi", 22, 19);
            BasicTest("rencontrons-nous dans tot le nuit mardi", 22, 17);
            BasicTest("rencontrons-nous dans debut nuit mardi", 22, 16);
            BasicTest("rencontrons-nous dans tard nuit Mardi", 22, 15);

            BasicTest("rencontrons-nous Mardi matin", 17, 11);
            BasicTest("rencontrons-nous Mardi tot le matin", 17, 18);
            BasicTest("rencontrons-nous Mardi d'apres-midi", 17, 18);
            BasicTest("rencontrons-nous Mardi dans la soiree", 17, 20);
            BasicTest("rencontrons-nous Mardi dans la nuit", 17, 18);
            BasicTest("rencontrons-nous Mardi soiree", 17, 12);
            BasicTest("rencontrons-nous Mardi nuit", 17, 10);
            BasicTest("rencontrons-nous Mardi de soir", 17, 13);
            BasicTest("rencontrons-nous Mardi ce soir", 17, 13);

        }
    }
}