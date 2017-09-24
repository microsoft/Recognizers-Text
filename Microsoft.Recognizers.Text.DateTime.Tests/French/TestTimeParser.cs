using Microsoft.VisualStudio.TestTools.UnitTesting;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime.French.Tests
{

    [TestClass]
    public class TestTimeParser
    {
        readonly BaseTimeExtractor extractor = new BaseTimeExtractor(new FrenchTimeExtractorConfiguration());
        readonly IDateTimeParser parser = new TimeParser(new FrenchTimeParserConfiguration(new FrenchCommonDateTimeParserConfiguration()));

        public void BasicTest(string text, DateObject date)
        {
            var er = extractor.Extract(text);
            Assert.AreEqual(1, er.Count);
            var pr = parser.Parse(er[0]);
            Assert.AreEqual(Constants.SYS_DATETIME_TIME, pr.Type);
            Assert.AreEqual(date, ((DateTimeResolutionResult)pr.Value).FutureValue);
        }

        public void BasicTest(string text, string luisValueStr)
        {
            var er = extractor.Extract(text);
            Assert.AreEqual(1, er.Count);
            var pr = parser.Parse(er[0]);
            Assert.AreEqual(Constants.SYS_DATETIME_TIME, pr.Type);
            Assert.AreEqual(luisValueStr, ((DateTimeResolutionResult)pr.Value).Timex);
        }

        [TestMethod]
        public void TestTimeParseWithTwoNumbers()
        {
            var today = DateObject.Today;
            int year = today.Year, month = today.Month, day = today.Day, second = 0;

            BasicTest("régler une alarm pour huit quarante", new DateObject(year, month, day, 8, 40, second));
            BasicTest("régler une alarm pour huit quarante dans le matin", new DateObject(year, month, day, 8, 40, second));
            BasicTest("régler une alarm pour huit quarante du matin", new DateObject(year, month, day, 8, 40, second));
            BasicTest("régler une alarm pour huit quarante ce matin", new DateObject(year, month, day, 8, 40, second));
            BasicTest("régler une alarm pour huit quarante a.m", new DateObject(year, month, day, 8, 40, second));
            BasicTest("régler une alarm pour huit quarante a.m.", new DateObject(year, month, day, 8, 40, second));
            BasicTest("régler une alarm pour huit quarante am", new DateObject(year, month, day, 8, 40, second));

            BasicTest("régler une alarm pour huit quarante pm", new DateObject(year, month, day, 20, 40, second));
            BasicTest("régler une alarm pour huit quarante p.m.", new DateObject(year, month, day, 20, 40, second));
            BasicTest("régler une alarm pour huit quarante p.m", new DateObject(year, month, day, 20, 40, second));

            // TODO: doesn't resolve third number without am/pm suffix
            // below will only recognize dix quarante without am/pm
            BasicTest("régler une alarm pour dix quarante cinq am", new DateObject(year, month, day, 10, 45, second));

            BasicTest("régler une alarm pour quinze quinze p m", new DateObject(year, month, day, 15, 15, second));
            BasicTest("régler une alarm pour quinze trente p m", new DateObject(year, month, day, 15, 30, second));
            BasicTest("régler une alarm pour dix dix", new DateObject(year, month, day, 10, 10, second));
            BasicTest("régler une alarm pour vingt-deux dix", new DateObject(year, month, day, 22, 10, second));
            BasicTest("régler une alarm pour dix cinquante cinq p. m.", new DateObject(year, month, day, 22, 55, second));
        }

        [TestMethod]
        public void TestTimeParse()
        {
            var today = DateObject.Today;
            int year = today.Year, month = today.Month, day = today.Day, min = 0, second = 0;

            BasicTest("Je retournerai a sept trente", new DateObject(year, month, day, 7, 30, second));

            //AMRegex
            BasicTest("Je retournerai a 7ampm", new DateObject(year, month, day, 7, min, second));
            BasicTest("Je retournerai a 7h", new DateObject(year, month, day, 7, min, second));
            BasicTest("Je retournerai a 7", new DateObject(year, month, day, 7, min, second));
            BasicTest("Je retournerai a sept", new DateObject(year, month, day, 7, min, second));
            BasicTest("Je retournerai 7pm", new DateObject(year, month, day, 19, min, second));
            BasicTest("Je retournerai a 19", new DateObject(year, month, day, 19, 0, second));

            //**NOTE: consider removing AM/PM, and +/- 12 hour adjustment from French implementation
            //BasicTest("Je retournerai a 19h", new DateObject(year, month, day, 19, 0, second));

            BasicTest("Je retournerai à 19", new DateObject(year, month, day, 19, 0, second));
            BasicTest("Je retournerai 7:56pm", new DateObject(year, month, day, 19, 56, second));
            BasicTest("Je retournerai 7:56:30pm", new DateObject(year, month, day, 19, 56, 30));
            BasicTest("Je retournerai 7:56:30 pm", new DateObject(year, month, day, 19, 56, 30));
            BasicTest("Je retournerai 12:34", new DateObject(year, month, day, 12, 34, second));
            BasicTest("Je retournerai 22:34", new DateObject(year, month, day, 22, 34, second));
            BasicTest("Je retournerai 12:34:25 ", new DateObject(year, month, day, 12, 34, 25));

            // PMRegex
            BasicTest("C'est 7 heures", new DateObject(year, month, day, 7, min, second));
            BasicTest("C'est sept heures", new DateObject(year, month, day, 7, min, second));
            BasicTest("C'est 8 du matin", new DateObject(year, month, day, 8, min, second));
            BasicTest("C'est 10 de matin", new DateObject(year, month, day, 10, min, second));
            BasicTest("C'est 10 du matinée", new DateObject(year, month, day, 10, min, second));
            BasicTest("C'est 10 du matinee", new DateObject(year, month, day, 10, min, second));
            BasicTest("C'est 11 de matinée", new DateObject(year, month, day, 11, min, second));
            BasicTest("C'est 11 de la matinée", new DateObject(year, month, day, 11, min, second));
            BasicTest("C'est 11 de la matinée", new DateObject(year, month, day, 11, min, second));
            BasicTest("C'est 8 du soir", new DateObject(year, month, day, 20, min, second));
            BasicTest("C'est 8 dans la nuit", new DateObject(year, month, day, 20, min, second));
            BasicTest("C'est 8 de soir", new DateObject(year, month, day, 20, min, second));
            BasicTest("C'est 8 du soir", new DateObject(year, month, day, 20, min, second));
            BasicTest("C'est 8 du soiree", new DateObject(year, month, day, 20, min, second));
            BasicTest("C'est 8 du soirée", new DateObject(year, month, day, 20, min, second));
            BasicTest("C'est 10 de soirée", new DateObject(year, month, day, 22, min, second));
            BasicTest("C'est 10 dans le soirée", new DateObject(year, month, day, 22, min, second));
            BasicTest("C'est 10 dans la soiree", new DateObject(year, month, day, 22, min, second));

            // TODO: Handle '3 in the afternoon', conflicts with 'afternoonRegex'
            //BasicTest("C'est 3 l'après-midi", new DateObject(year, month, day, 15, min, second));

            // TODO: Handle 'it's half past 8' - Il est 8 heures et demie
            // TODO: Handle military time - 24:00, possibly remove AM/PM from French files?
            
            //BasicTest("C'est 8 heures et demie", new DateObject(year, month, day, 8, 30, second));
            //BasicTest("Il est 20 heures et emi", new DateObject(year, month, day, 20, 30, second));
            //BasicTest("It's 30 mins past eight", new DateObject(year, month, day, 8, 30, second));
            //BasicTest("It's a quarter past eight", new DateObject(year, month, day, 8, 15, second));
            //BasicTest("It's quarter past eight", new DateObject(year, month, day, 8, 15, second));
            //BasicTest("It's three quarters past 9pm", new DateObject(year, month, day, 21, 45, second));
            //BasicTest("It's three minutes to eight", new DateObject(year, month, day, 7, 57, second));

            //BasicTest("It's half past seven o'clock", new DateObject(year, month, day, 7, 30, second));
            //BasicTest("It's half past seven afternoon", new DateObject(year, month, day, 19, 30, second));
            //BasicTest("It's half past seven in the morning", new DateObject(year, month, day, 7, 30, second));
            //BasicTest("It's a quarter to 8 in the morning", new DateObject(year, month, day, 7, 45, second));
            //BasicTest("It's 20 min past eight in the evening", new DateObject(year, month, day, 20, 20, second));

            //PMRegex
            BasicTest("Je retournerai apres-midi a 7", new DateObject(year, month, day, 19, 0, second));
            BasicTest("Je retournerai apres midi 7:00", new DateObject(year, month, day, 19, 0, second));
            BasicTest("Je retournerai l'après-midi 7:00:05", new DateObject(year, month, day, 19, 0, 05));
            BasicTest("Je retournerai l'après midi sept", new DateObject(year, month, day, 19, 0, second));
            BasicTest("Je retournerai a 7 ce soir", new DateObject(year, month, day, 19, 0, second));
            BasicTest("Je retournerai a 7:30 du soir", new DateObject(year, month, day, 19, 30, second));
            BasicTest("Je retournerai a 7:30 dans la nuit", new DateObject(year, month, day, 19, 30, second));

            //IshRegex, approximately noon
            BasicTest("C'est peu pres midi", new DateObject(year, month, day, 12, min, second));
            BasicTest("C'est peu près midi", new DateObject(year, month, day, 12, min, second));
            BasicTest("Je retournerai peu pres 11", new DateObject(year, month, day, 11, 0, second));
            BasicTest("Je retournerai peu près 23", new DateObject(year, month, day, 23, 0, second));
            // BasicTest("Je retournerai peu près vingt-trois", new DateObject(year, month, day, 23, 0, second));

            // 24:00 time, how French actually say it
            // **NOTE: There is no 'pm/am' in french
            BasicTest("Je retournerai dix-neuf trente", new DateObject(year, month, day, 19, 30, second));
            BasicTest("Je retournerai vingt-trois vingt", new DateObject(year, month, day, 23, 20, second));

            // resolves to AM 
            // TODO - PMRegex with more complex number, i.e sept trente 'seven thirty'
            //BasicTest("Je retournerai sept trente du soir", new DateObject(year, month, day, 19, 30, second));

            // No AM/PM in french, consider removing      
            BasicTest("Je retournerai 340pm", new DateObject(year, month, day, 15, 40, second));
            BasicTest("Je retournerai 1140 a.m.", new DateObject(year, month, day, 11, 40, second));

            BasicTest("minuit", new DateObject(year, month, day, 0, 0, second));

            BasicTest("milieu du matin", new DateObject(year, month, day, 10, 0, second));
            BasicTest("milieu de matinee", new DateObject(year, month, day, 10, 0, second));
            BasicTest("milieu de matinée", new DateObject(year, month, day, 10, 0, second));
            BasicTest("milieu d'apres-midi", new DateObject(year, month, day, 14, 0, second));
            BasicTest("milieu d'après-midi", new DateObject(year, month, day, 14, 0, second));
            BasicTest("milieu d'après midi", new DateObject(year, month, day, 14, 0, second));
            BasicTest("milieu de midi", new DateObject(year, month, day, 12, 0, second));
            BasicTest("milieu du jour", new DateObject(year, month, day, 12, 0, second));
            BasicTest("apres-midi", new DateObject(year, month, day, 12, 0, second));
            BasicTest("après-midi", new DateObject(year, month, day, 12, 0, second));
            BasicTest("après midi", new DateObject(year, month, day, 12, 0, second));
        }

        [TestMethod]
        public void TestTimeParseLuis()
        {
            BasicTest("Je retournerai à 7", "T07");
            BasicTest("Je retournerai à sept", "T07");
            BasicTest("Je retournerai 7pm", "T19");
            BasicTest("Je retournerai 7:56pm", "T19:56");
            BasicTest("Je retournerai 7:56:30pm", "T19:56:30");
            BasicTest("Je retournerai 7:56:13 pm", "T19:56:13");
            BasicTest("Je retournerai 12:34", "T12:34");
            BasicTest("Je retournerai 12:34:45 ", "T12:34:45");
            // more accurate to french to use 24:00 time
            BasicTest("Je retournerai 22:34:45 ", "T22:34:45");

            BasicTest("C'est 7 heures", "T07");
            BasicTest("C'est sept heures", "T07");
            BasicTest("C'est 8 du matin", "T08");
            BasicTest("C'est 8 dans la nuit", "T20");
            BasicTest("C'est 8 dans la soiree", "T20");
            BasicTest("C'est 8 ce soir", "T20");
            BasicTest("C'est 8 dans l'apres midi", "T20");
            // **TODO - 
            // in french - 'c'est 8 heures et demies' 
            //BasicTest("It's half past eight", "T08:30");
            //BasicTest("It's half past 8pm", "T20:30");
            //BasicTest("It's 30 mins past eight", "T08:30");
            //BasicTest("It's a quarter past eight", "T08:15");
            //BasicTest("It's three quarters past 9pm", "T21:45");
            //BasicTest("It's three minutes to eight", "T07:57");

            //BasicTest("It's half past seven o'clock", "T07:30");
            //BasicTest("It's half past seven afternoon", "T19:30");
            //BasicTest("It's half past seven in the morning", "T07:30");
            //BasicTest("It's a quarter to 8 in the morning", "T07:45");
            //BasicTest("It's 20 min past eight in the evening", "T20:20");

            BasicTest("Je retournerai dans l'apres midi a 7", "T19");
            BasicTest("Je retournerai apres-midi a 7", "T19");
            BasicTest("Je retournerai apres midi 7:00", "T19:00");
            BasicTest("Je retournerai l'après-midi 7:00:25", "T19:00:25");
            BasicTest("Je retournerai l'apres-midi sept", "T19");

            BasicTest("Je retournerai sept trente", "T07:30");
            BasicTest("Je retournerai dix-neuf trente", "T19:30");
            BasicTest("Je retournerai vingt-et-un trente", "T21:30");
            BasicTest("Je retournerai vingt-deux trente", "T22:30");
            
            // parses 'deux trente' - 2:30, i.e you need '-' 
            //BasicTest("Je retournerai vingt deux trente", "T22:30");

            // doesn't parse last number 'cinq', resolves to 19:30
            // BasicTest("Je retournerai dix-neuf trente cinq", "T19:35");           
            
            //TODO - fix
            //BasicTest("Je retournerai trois jusqu'a cinq trente ", "T05:27");

            BasicTest("Je retournerai cinq trente dans la nuit", "T17:30");
            BasicTest("Je retournerai cinq trente ce soir", "T17:30");
            BasicTest("Je retournerai cinq trente du soir", "T17:30");
            BasicTest("Je retournerai cinq trente dans la soiree", "T17:30");
            BasicTest("Je retournerai cinq trente du matin", "T05:30");
            BasicTest("Je retournerai cinq trente du matinee", "T05:30");
            BasicTest("Je retournerai cinq trente dans le matin", "T05:30");

            BasicTest("Je retournerai peu près 12", "T12");
            BasicTest("Je retournerai peu pres 23", "T23");
            BasicTest("Je retournerai peu pres 5", "T05");
            BasicTest("Je retournerai peu pres midi", "T12");

            //TODO: discussion on the definition
            //Default time period definition for now
            //LUIS Time Resolution Spec address: https://microsoft.sharepoint.com/teams/luis_core/_layouts/15/WopiFrame2.aspx?sourcedoc=%7B852DBAAF-911B-4CCC-9401-996505EC9B67%7D&file=LUIS%20Time%20Resolution%20Spec.docx&action=default
            //a.Morning: 08:00:00 - 12:00:00
            //b.Afternoon: 12:00:00 – 16:00:00
            //c.Evening: 16:00:00 – 20:00:00
            //d.Night: 20:00:00 – 23:59:59
            //e.Daytime: 08:00:00 – 16:00:00(morning + afternoon)
            BasicTest("minuit", "T00");
            BasicTest("milieu de matinée", "T10");
            BasicTest("milieu de matinee", "T10");
            BasicTest("milieu du matin", "T10");

            BasicTest("milieu d'après-midi", "T14");
            BasicTest("milieu d'apres midi", "T14");
            BasicTest("milieu d'après midi", "T14");
            BasicTest("milieu de jour", "T12");
            BasicTest("milieu du jour", "T12");
            BasicTest("milieu de midi", "T12");
            BasicTest("apres-midi", "T12");
        }
    }
}