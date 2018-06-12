using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Recognizers.Text.Matcher;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Recognizers.Text.DataDrivenTests.Matcher
{
    [TestClass]
    public class StringMatcherTest
    {
        [TestMethod]
        public void SimpleTestStringMatcher()
        {
            var values = new List<string>() { "China", "Beijing", "City" };
            var stringMatcher = new StringMatcher();
            stringMatcher.Init(values);

            foreach (var value in values)
            {
                Assert.AreEqual(value, stringMatcher.Find(value).Single().Text);
            }
        }

        [TestMethod]
        public void SimpleTestWithIdsStringMatcher()
        {
            var values = new List<string>() { "China", "Beijing", "City" };
            var Ids = new List<string>() { "1", "2", "3" };
            var stringMatcher = new StringMatcher();
            stringMatcher.Init(values, Ids.ToArray());

            for (var i = 0; i < values.Count; i++)
            {
                var value = values[i];
                var match = stringMatcher.Find(value).Single();
                Assert.AreEqual(value, match.Text);
                Assert.AreEqual(Ids[i], match.CanonicalValues.First());
            }
        }

        [TestMethod]
        public void TestStringMatcher()
        {
            var utc8Value = "UTC+08:00";
            var utc8Words = new List<string>()
            {
                "beijing time", "chongqing time", "hong kong time", "urumqi time"
            };

            var utc2Value = "UTC+02:00";
            var utc2Words = new List<string>()
            {
                  "cairo time", "beirut time", "gaza time", "amman time"
            };

            var valueDictionary = new Dictionary<string, List<string>>()
            {
                {
                    utc8Value, utc8Words
                },
                {
                    utc2Value, utc2Words
                },
            };

            var stringMatcher = new StringMatcher();
            stringMatcher.Init(valueDictionary);

            foreach (var value in utc8Words)
            {
                var sentence = $"please change {value}, thanks";
                var matches = stringMatcher.Find(sentence);
                Assert.AreEqual(value, matches.Single().Text);
                Assert.AreEqual(utc8Value, matches.Single().CanonicalValues.First());
                Assert.AreEqual(14, matches.Single().Start);
            }

            foreach (var value in utc2Words)
            {
                var sentence = $"please change {value}, thanks";
                var matches = stringMatcher.Find(sentence);
                Assert.AreEqual(value, matches.Single().Text);
                Assert.AreEqual(utc2Value, matches.Single().CanonicalValues.First());
                Assert.AreEqual(14, matches.Single().Start);
            }
        }
    }
}
