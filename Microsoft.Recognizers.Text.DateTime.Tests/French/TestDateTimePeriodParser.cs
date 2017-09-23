using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime.French.Tests
{
    [TestClass]
    public class TestDateTimePeriodParser
    {
        readonly IExtractor extractor;
        readonly IDateTimeParser parser;

        readonly DateObject referenceTime;

        public TestDateTimePeriodParser()
        {
            referenceTime = new DateObject(2016, 11, 7, 16, 12, 0);
            extractor = new BaseDateTimePeriodExtractor(new FrenchDateTimePeriodExtractorConfiguration());
            parser = new BaseDateTimePeriodParser(new FrenchDateTimePeriodParserConfiguration(new FrenchCommonDateTimeParserConfiguration()));
        }

        public void BasicTestFuture(string text, DateObject beginDate, DateObject endDate)
        {
            var er = extractor.Extract(text);
            Assert.AreEqual(1, er.Count);
            var pr = parser.Parse(er[0], referenceTime);
            Assert.AreEqual(Constants.SYS_DATETIME_DATETIMEPERIOD, pr.Type);
            Assert.AreEqual(beginDate,
                ((Tuple<DateObject, DateObject>)((DateTimeResolutionResult)pr.Value).FutureValue).Item1);
            Assert.AreEqual(endDate,
                ((Tuple<DateObject, DateObject>)((DateTimeResolutionResult)pr.Value).FutureValue).Item2);
        }

        public void BasicTest(string text, string luisValueStr)
        {
            var er = extractor.Extract(text);
            Assert.AreEqual(1, er.Count);
            var pr = parser.Parse(er[0], referenceTime);
            Assert.AreEqual(Constants.SYS_DATETIME_DATETIMEPERIOD, pr.Type);
            Assert.AreEqual(luisValueStr, ((DateTimeResolutionResult)pr.Value).Timex);
        }

        [TestMethod]
        public void TestDateTimePeriodParse()
        {
            int year = 2016, month = 11, day = 7, min = 0, second = 0;

            // basic match
            BasicTestFuture("Je serai sorti cinq au sept aujourd'hui",
                new DateObject(year, month, day, 5, min, second),
                new DateObject(year, month, day, 7, min, second));
            BasicTestFuture("Je serai sorti from 5 à 6 de 22/4/2016",
                new DateObject(2016, 4, 22, 5, min, second),
                new DateObject(2016, 4, 22, 6, min, second));
            BasicTestFuture("Je serai sorti de 5 au 6 de Avril 22",
                new DateObject(year + 1, 4, 22, 5, min, second),
                new DateObject(year + 1, 4, 22, 6, min, second));
            BasicTestFuture("Je serai sorti de 5 a 6pm de Avril 22",
                new DateObject(year + 1, 4, 22, 17, min, second),
                new DateObject(year + 1, 4, 22, 18, min, second));
            BasicTestFuture("Je serai sorti de 5 au 6 de 1 Jan",
                new DateObject(year + 1, 1, 1, 5, min, second),
                new DateObject(year + 1, 1, 1, 6, min, second));

            // '1er' was working... 
            //BasicTestFuture("Je serai sorti de 5 au 6 de 1er Jan",
            //    new DateObject(year + 1, 1, 1, 5, min, second),
            //    new DateObject(year + 1, 1, 1, 6, min, second));

            // merge two time points
            BasicTestFuture("Je serai sorti 3pm a 4pm lendemain",
                new DateObject(year, month, 8, 15, min, second),
                new DateObject(year, month, 8, 16, min, second));

            BasicTestFuture("Je serai sorti 3:00 à 4:00 demain",
                new DateObject(year, month, 8, 3, min, second),
                new DateObject(year, month, 8, 4, min, second));

            // 'i'll be out to half past 7...
            //BasicTestFuture("Je serai sorti sept et demies heures a 4pm demain",
            //    new DateObject(year, month, 8, 7, 30, second),
            //    new DateObject(year, month, 8, 16, min, second));

            BasicTestFuture("Je serai sorti de 4pm aujourd'hui à 5pm demain",
                new DateObject(year, month, day, 16, min, second),
                new DateObject(year, month, 8, 17, min, second));

            //BasicTestFuture("Je serai sorti de 2:00pm, 2016-21-2 a 3:32, 23/04/2016",
            //    new DateObject(2016, 2, 21, 14, min, second),
            //    new DateObject(2016, 4, 23, 3, 32, second));

            BasicTestFuture("Je serai sorti entre 4pm et 5pm aujourd'hui",
                new DateObject(year, month, day, 16, min, second),
                new DateObject(year, month, day, 17, min, second));

            BasicTestFuture("Je serai sorti de 4pm 1 Jan, 2016 a 5pm aujourd'hui",
                new DateObject(2016, 1, 1, 16, min, second),
                new DateObject(year, month, day, 17, min, second));

            BasicTestFuture("Je reviendrai cette nuit",
                new DateObject(year, month, day, 20, min, second),
                new DateObject(year, month, day, 23, 59, 59));

            BasicTestFuture("Je reviendrai ce soir",
                new DateObject(year, month, day, 16, min, second),
                new DateObject(year, month, day, 20, 00, 00));

            BasicTestFuture("Je reviendai cette soiree",
                new DateObject(year, month, day, 16, min, second),
                new DateObject(year, month, day, 20, 00, 00));

            BasicTestFuture("Je reviendrai cette de la soiree",
                new DateObject(year, month, day, 16, min, second),
                new DateObject(year, month, day, 20, min, second));

            BasicTestFuture("Je reviendrai cette matin",
                new DateObject(year, month, day, 8, min, second),
                new DateObject(year, month, day, 12, min, second));

            BasicTestFuture("Je reviendrai ce l'apres-midi",
                new DateObject(year, month, day, 12, min, second),
                new DateObject(year, month, day, 16, min, second));

            BasicTestFuture("Je reviendrai le prochaine soiree",
                new DateObject(year, month, day + 1, 16, min, second),
                new DateObject(year, month, day + 1, 20, 00, 00));

            BasicTestFuture("Je reviendrai la derniere nuit",
                new DateObject(year, month, day - 1, 20, min, second),
                new DateObject(year, month, day - 1, 23, 59, 59));

            BasicTestFuture("Je reviens demain soir",
                new DateObject(year, month, day + 1, 16, min, second),
                new DateObject(year, month, day + 1, 20, 00, 00));

            //BasicTestFuture("Je reviendrai lundi prochain semaine",
            //    new DateObject(year, month, 14, 12, min, second),
            //    new DateObject(year, month, 14, 16, min, second));


            // needs to support '3 derniere minutes'
            BasicTestFuture("Je reviendrai dernier 3 minutes",
                new DateObject(year, month, day, 16, 9, second),
                new DateObject(year, month, day, 16, 12, second));

            BasicTestFuture("Je reviendrai dernier 3mins",
                new DateObject(year, month, day, 16, 9, second),
                new DateObject(year, month, day, 16, 12, second));

            BasicTestFuture("Je reviendrai prochaine 5 heures",
                new DateObject(year, month, day, 16, 12, second),
                new DateObject(year, month, day, 21, 12, second));

            BasicTestFuture("Je reviendrai derniere minute",
                new DateObject(year, month, day, 16, 11, second),
                new DateObject(year, month, day, 16, 12, second));

            BasicTestFuture("Je reviendrai prochain heure",
                new DateObject(year, month, day, 16, 12, second),
                new DateObject(year, month, day, 17, 12, second));

            //needs grammar fix
            BasicTestFuture("Je reviendrai prochain quelques heures",
                new DateObject(year, month, day, 16, 12, second),
                new DateObject(year, month, day, 19, 12, second));

            BasicTestFuture("Je reviendrai mardi matinee",
                new DateObject(year, month, day + 1, 8, 0, 0),
                new DateObject(year, month, day + 1, 12, 0, 0));

            BasicTestFuture("Je reviendrai mardi l'apres-midi",
               new DateObject(year, month, day + 1, 12, 0, 0),
               new DateObject(year, month, day + 1, 16, 0, 0));

            BasicTestFuture("Je reviendrai mardi soiree",
               new DateObject(year, month, day + 1, 16, 0, 0),
               new DateObject(year, month, day + 1, 20, 0, 0));

            // late/early
            BasicTestFuture("rencontrons-nous dans tôt le matin Mardi",
                new DateObject(year, month, day + 1, 8, 0, 0),
                new DateObject(year, month, day + 1, 10, 0, 0));
            BasicTestFuture("rencontrons-nous dans le début matin Mardi",
                new DateObject(year, month, day + 1, 8, 0, 0),
                new DateObject(year, month, day + 1, 10, 0, 0));
            BasicTestFuture("rencontrons-nous dans le tard matin Mardi",
                new DateObject(year, month, day + 1, 10, 0, 0),
                new DateObject(year, month, day + 1, 12, 0, 0));

            //only resolves at 12:00, not 2:00 if you use 'tot d'apres-midi mardi'
            BasicTestFuture("rencontrons-nous dans mardi tôt d'après-midi",
                new DateObject(year, month, day + 1, 12, 0, 0),
                new DateObject(year, month, day + 1, 14, 0, 0));

            BasicTestFuture("Allons nous recontrer mardi tard apres-midi",
                new DateObject(year, month, day + 1, 14, 0, 0),
                new DateObject(year, month, day + 1, 16, 0, 0));

            //BasicTestFuture("rencontrons-nous dans fin de soiree mardi",
            //    new DateObject(year, month, day + 1, 16, 0, 0),
            //    new DateObject(year, month, day + 1, 18, 0, 0));

            //BasicTestFuture("rencontrons-nous dans fin de soirée mardi",
            //    new DateObject(year, month, day + 1, 18, 0, 0),
            //    new DateObject(year, month, day + 1, 20, 0, 0));

            BasicTestFuture("rencontrons-nous dans tot le nuit mardi",
                new DateObject(year, month, day + 1, 20, 0, 0),
                new DateObject(year, month, day + 1, 22, 0, 0));

            BasicTestFuture("rencontrons-nous dans tard nuit Mardi",
                new DateObject(year, month, day + 1, 22, 0, 0),
                new DateObject(year, month, day + 1, 23, 59, 59));

            BasicTestFuture("rencontrons-nous dans le tôt nuit mardi",
                new DateObject(year, month, day + 1, 20, 0, 0),
                new DateObject(year, month, day + 1, 22, 0, 0));

            BasicTestFuture("rencontrons-nous dans fin de nuit mardi",
                new DateObject(year, month, day + 1, 22, 0, 0),
                new DateObject(year, month, day + 1, 23, 59, 59));

            BasicTestFuture("rencontrons-nous Mardi tot le matin",
                new DateObject(year, month, day + 1, 8, 0, 0),
                new DateObject(year, month, day + 1, 10, 0, 0));

            BasicTestFuture("rencontrons-nous Mardi tard matin",
                new DateObject(year, month, day + 1, 10, 0, 0),
                new DateObject(year, month, day + 1, 12, 0, 0));

            //BasicTestFuture("rencontrons-nous dans tot d'après-midi Mardi",
            //    new DateObject(year, month, day + 1, 12, 0, 0),
            //    new DateObject(year, month, day + 1, 14, 0, 0));

            //BasicTestFuture("rencontrons-nous dans le tard d'apres-midi Mardi",
            //    new DateObject(year, month, day + 1, 14, 0, 0),
            //    new DateObject(year, month, day + 1, 16, 0, 0));

            BasicTestFuture("rencontrons-nous dans tôt le soir mardi",
                new DateObject(year, month, day + 1, 16, 0, 0),
                new DateObject(year, month, day + 1, 18, 0, 0));

            BasicTestFuture("rencontrons-nous le tard soir mardi",
                new DateObject(year, month, day + 1, 18, 0, 0),
                new DateObject(year, month, day + 1, 20, 0, 0));

            BasicTestFuture("rencontrons-nous dans le tôt nuit mardi",
                new DateObject(year, month, day + 1, 20, 0, 0),
                new DateObject(year, month, day + 1, 22, 0, 0));

            BasicTestFuture("rencontrons-nous dans fin de nuit mardi",
                new DateObject(year, month, day + 1, 22, 0, 0),
                new DateObject(year, month, day + 1, 23, 59, 59));
            BasicTestFuture("rencontrons-nous dans le tôt nuit mardi",
                new DateObject(year, month, day + 1, 20, 0, 0),
                new DateObject(year, month, day + 1, 22, 0, 0));
            BasicTestFuture("rencontrons-nous dans le tard nuit Mardi",
                new DateObject(year, month, day + 1, 22, 0, 0),
                new DateObject(year, month, day + 1, 23, 59, 59));

        }

        [TestMethod]
        public void TestDateTimePeriodParseLuis()
        {
            // basic match
            BasicTest("Je serai sorti cinq au sept aujourd'hui", "(2016-11-07T05,2016-11-07T07,PT2H)");
            BasicTest("Je serai sorti from 5 à 6 de 22/4/2016", "(2016-04-22T05,2016-04-22T06,PT1H)");
            BasicTest("Je serai sorti de 5 au 6 de Avril 22", "(XXXX-04-22T05,XXXX-04-22T06,PT1H)");
            BasicTest("Je serai sorti de 5 au 6 de 1 Jan", "(XXXX-01-01T05,XXXX-01-01T06,PT1H)");

            // merge two time points
            BasicTest("Je serai sorti 3pm a 4pm lendemain", "(2016-11-08T15,2016-11-08T16,PT1H)");
            BasicTest("Je serai sorti 15 à 16 demain", "(2016-11-08T15,2016-11-08T16,PT1H)"); // more accurate to use 15, 16 instead of 3:00, 4:00
            //BasicTest("I'll be out I'll be out from 2:00pm, 2016-2-21 to 3:32, 04/23/2016",
            //    "(2016-02-21T14:00,2016-04-23T03:32,PT1478H)");

            BasicTest("Je reviendrai cette nuit", "2016-11-07TNI");
            BasicTest("Je reviendrai ce nuit", "2016-11-07TNI");
            BasicTest("Je reviendrai ce soir", "2016-11-07TEV");
            BasicTest("Je reviendrai cette matin", "2016-11-07TMO");
            BasicTest("Je reviendrai ce matin", "2016-11-07TMO");
            BasicTest("Je reviendri ce l'apres-midi", "2016-11-07TAF");

            // grammar: prochaine should be suffix
            BasicTest("Je reviendrai prochaine nuit", "2016-11-08TNI");
            // grammar: derniere should be suffix
            BasicTest("Je reviendrai derniere nuit", "2016-11-06TNI");

            // i'll go back tomorrow night
            BasicTest("Je reviendrai nuit lendemain", "2016-11-08TNI");

            BasicTest("Je reviendrai lundi prochain d'apres-midi", "2016-11-14TAF");

            // grammar: should fix to '3 derniere minute' or add case 
            BasicTest("Je reviendrai derniere 3 minute", "(2016-11-07T16:09:00,2016-11-07T16:12:00,PT3M)");
            BasicTest("Je reviendrai derniere 3mins", "(2016-11-07T16:09:00,2016-11-07T16:12:00,PT3M)");
            BasicTest("Je reviendrai dernier 3 minutes", "(2016-11-07T16:09:00,2016-11-07T16:12:00,PT3M)");

            BasicTest("Je reviendrai prochain 5 hrs", "(2016-11-07T16:12:00,2016-11-07T21:12:00,PT5H)");
            BasicTest("Je reviendrai dernière minute", "(2016-11-07T16:11:00,2016-11-07T16:12:00,PT1M)");
            BasicTest("Je reviendrai prochaine heures", "(2016-11-07T16:12:00,2016-11-07T17:12:00,PT1H)");

            BasicTest("Je reviendrai mardi dans le matin", "XXXX-WXX-2TMO");

            // early/late date time
            BasicTest("rencontrons-nous dans tôt le matin Mardi", "XXXX-WXX-2TMO");
            BasicTest("rencontrons-nous dans le tard matin Mardi", "XXXX-WXX-2TMO");

            // Resolves to proper time, but XXXX-WXX-2T12,XXXX-WXX-2T12,PTOH which is a range...
            //BasicTest("rencontrons-nous dans le début d'après-midi Mardi", "XXXX-WXX-2TAF");

            //TODO: investigate d'apres midi in REGEX's
            //BasicTest("rencontrons-nous d'apres-midi Mardi", "XXXX-WXX-2TAF");

            // works for short slang
            BasicTest("rencontrons-nous Mardi d'apres-midi", "XXXX-WXX-2TAF");
            BasicTest("rencontrons-nous Mardi matin", "XXXX-WXX-2TMO");
            BasicTest("rencontrons-nous Mardi soiree", "XXXX-WXX-2TEV");
            BasicTest("rencontrons-nous Mardi nuit", "XXXX-WXX-2TNI");
            BasicTest("rencontrons-nous nuit Mardi", "XXXX-WXX-2TNI");
            BasicTest("rencontrons-nous de la nuit Mardi", "XXXX-WXX-2TNI");
            BasicTest("rencontrons-nous de la soiree Mardi", "XXXX-WXX-2TEV");
            BasicTest("rencontrons-nous de la matin mardi", "XXXX-WXX-2TMO");

            BasicTest("rencontrons-nous tot de le soiree mardi", "XXXX-WXX-2TEV");
            BasicTest("rencontrons-nous fin de la soiree mardi", "XXXX-WXX-2TEV");
            BasicTest("rencontrons-nous fin du soiree mardi", "XXXX-WXX-2TEV");
            BasicTest("rencontrons-nous tot du soiree mardi", "XXXX-WXX-2TEV");
            BasicTest("rencontrons-nous Mardi tot d'apres-midi", "XXXX-WXX-2TAF");
            BasicTest("rencontrons-nous Mardi tard d'apres-midi", "XXXX-WXX-2TAF");

            BasicTest("rencontrons-nous dans tot le nuit mardi", "XXXX-WXX-2TNI");
            BasicTest("rencontrons-nous dans tard nuit Mardi", "XXXX-WXX-2TNI");
            BasicTest("rencontrons-nous dans tot le nuit mardi", "XXXX-WXX-2TNI");
            BasicTest("rencontrons-nous Mardi dans la nuit", "XXXX-WXX-2TNI");

            // doesn't work for these cases
            //BasicTest("rencontrons-nous Mardi dans la soiree", "XXXX-WXX-2TNI");
            //BasicTest("rencontrons-nous Mardi du soiree", "XXXX-WXX-2TEV");

            BasicTest("rencontrons-nous dans le tard nuit Mardi", "XXXX-WXX-2TNI");

            BasicTest("rencontrons-nous Mardi tot le matin", "XXXX-WXX-2TMO");
            BasicTest("rencontrons-nous Mardi tard matin", "XXXX-WXX-2TMO");
            BasicTest("rencontrons-nous Mardi d'apres-midi", "XXXX-WXX-2TAF");
            BasicTest("rencontrons-nous Mardi tard d'apres-midi", "XXXX-WXX-2TAF");
            BasicTest("rencontrons-nous Mardi tot d'apres-midi", "XXXX-WXX-2TAF");
            BasicTest("rencontrons-nous Mardi soiree", "XXXX-WXX-2TEV");
            BasicTest("rencontrons-nous Mardi soir", "XXXX-WXX-2TEV");
            BasicTest("rencontrons-nous Mardi dans la nuit", "XXXX-WXX-2TNI");
            BasicTest("rencontrons-nous Mardi fin de nuit", "XXXX-WXX-2TNI");
            BasicTest("rencontrons-nous Mardi tard nuit", "XXXX-WXX-2TNI");
        }
    }
}