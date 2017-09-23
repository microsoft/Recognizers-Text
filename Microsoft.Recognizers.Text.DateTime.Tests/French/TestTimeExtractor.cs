using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Recognizers.Text.DateTime.French.Tests
{
    [TestClass]
    public class TestTimeExtractor
    {
        private readonly BaseTimeExtractor extractor = new BaseTimeExtractor(new FrenchTimeExtractorConfiguration());

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
            Assert.AreEqual(Constants.SYS_DATETIME_TIME, results[0].Type);
        }

        public void BasicNegativeTest(string text)
        {
            BasicTest(text, -1, -1, 0);
        }

        [TestMethod]
        public void TestTimeExtract()
        {
            BasicTest("je retournerai à 7", 17, 1);
            BasicTest("je retournerai a sept", 17, 4);
            BasicTest("je retournerai 7pm", 15, 3);
            BasicTest("je retournerai 7p.m.", 15, 5);
            BasicTest("je retournerai 7:56pm", 15, 6);
            BasicTest("Je retournerai 15:00", 15, 5);
            BasicTest("je retournerai 7:56:35pm", 15, 9);
            BasicTest("je retournerai 7:56:35 pm", 15, 10);
            BasicTest("je retournerai 12:34", 15, 5);
            BasicTest("je retournerai 12:34:20", 15, 8);
            BasicTest("je retournerai T12:34:20", 15, 9);
            BasicTest("je retournerai 00:00", 15, 5);
            BasicTest("je retournerai 00:00:30", 15, 8);

            BasicTest("C'est 7 heures", 6, 8);
            BasicTest("C'est sept heure", 6, 10);
            BasicTest("C'est 8 dans le matin", 6, 15);
            BasicTest("C'est 8 dans la nuit", 6, 14);

            // BasicTest("C'est 20 heures du soir", 6, 17); // better way to say half past 8pm
            // BasicTest("C'est 20h30", 6, 5); // add another time extractor... for H/M

            // TODO: These REGEX needs to be re-done for these...
            // eng) it is quarter past 4 pm 
            // fr) C'est seize heures quinze - use 'quinze' rather than 'et quart' past noon

            //BasicTest("C'est half past 8pm", 5, 13);
            //BasicTest("C'est 30 mins past eight", 5, 18);
            //BasicTest("C'est a quarter past eight", 5, 20);
            //BasicTest("C'est quarter past eight", 5, 18);
            //BasicTest("C'est three quarters past 9pm", 5, 23);
            //BasicTest("C'est three minutes to eight", 5, 22);

            //BasicTest("C'est half past seven o'clock", 5, 23);
            //BasicTest("C'est half past seven afternoon", 5, 25);
            //BasicTest("C'est half past seven in the morning", 5, 30);
            //BasicTest("C'est a quarter to 8 in the morning", 5, 29);
            //BasicTest("C'est 20 min past eight in the evening", 5, 32);


            //BasicTest("je retournerai a 7 l'apres-midi", 17, 15);
            //BasicTest("je retournerai l'apres midi a 7", 15, 16);

            BasicTest("je retournerai apres-midi 7:00", 15, 15);
            BasicTest("je retournerai apres-midi 7:00:14", 15, 18);
            BasicTest("je retournerai apres-midi sept", 15, 15);

            //BasicTest("je retournerai sept heures du soir", 15, 19);   // need to get ' heures + du soir' 'in the evening' 
            BasicTest("Je retournerai sept trente cinq du soir", 15, 24); // this one works - need to make sure it registers pm
            BasicTest("Je retournerai a onze cinq", 17, 9);
            //BasicTest("Je retournerai trois mins a cinq trente", 15, 24);
            BasicTest("Je retournerai cinq trente dans la nuit", 15, 24);

            //BasicTest("je retournerai peu pres a midi", 15, 15);
            BasicTest("je retournerai peu pres a 5", 15, 12);
            BasicTest("je retournerai peu près à 11 ", 15, 13); // i'll return close to 11 

            //BasicTest("je retournerai 1540", 15, 4);  //fix to recognize military time
            BasicTest("je retournerai 1140 a.m.", 15, 9);
            //BasicTest("je retournerai 1140 du matin", 15, 13); // needs to recognize du matin

            BasicTest("minuit", 0, 6);

            BasicTest("milieu de matin", 0, 15);
            BasicTest("milieu de matinée", 0, 17);

            BasicTest("milieu d'après-midi", 0, 19);
            BasicTest("milieu d'après midi", 0, 19);

            BasicTest("milieu du jour", 0, 14);
            BasicTest("milieu de midi", 0, 14);

            BasicTest("après midi", 0, 10);
        }

        [TestMethod]
        public void TestTimeDescExtract()
        {
            BasicTest("je retournerai 7pm", 15, 3);
            BasicTest("je retournerai 7 du soir", 15, 9);
            BasicTest("je retournerai 7 dans la nuit", 15, 14);
            BasicTest("je retournerai 7 p m", 15, 5);
            BasicTest("je retournerai 7 p. m", 15, 6);
            BasicTest("je retournerai 7 p. m.", 15, 7);
            BasicTest("je retournerai 7p.m.", 15, 5);
            BasicTest("je retournerai 7 p.m.", 15, 6);
            BasicTest("je retournerai 7:56 a m", 15, 8);
            BasicTest("je retournerai 7:56:35 a. m", 15, 12);
            BasicTest("je retournerai 7:56:35 am", 15, 10);
            BasicTest("je retournerai 7:56:35 a. m.", 15, 13);

            BasicTest("je retournerai sept trente ce matin", 15, 20);
            BasicTest("je retournerai sept trente du matin", 15, 20);
            BasicTest("je retournerai sept trente de soir", 15, 19);
            BasicTest("je retournerai sept trente dans la soiree", 15, 26);
            // BasicTest("je retournerai sept trente dans l'apres midi", 15, 29); //recognizes 2, PMregex
            BasicTest("je retournerai sept trente p. m", 15, 16);
            BasicTest("je retournerai sept trente p. m.", 15, 17);

            BasicTest("je retournerai 340pm", 15, 5);
            BasicTest("je retournerai 340 pm", 15, 6);
            BasicTest("je retournerai 1140 a.m.", 15, 9);
            BasicTest("je retournerai 1140 a m", 15, 8);

            // BasicTest("je retournerai 2030", 15, 4); // TODO: Add Military time
        }

        [TestMethod]
        public void TestDatePeriodExtractNegativeCase()
        {

            var sentence = "quel emails ont obtenu p en tant sujet ";
            BasicNegativeTest(sentence);

            sentence = "quel emails ont recu une response?";
            BasicNegativeTest(sentence);

        }
    }
}