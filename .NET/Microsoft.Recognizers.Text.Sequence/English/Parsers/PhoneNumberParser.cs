using System;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.Sequence.English
{
    public class PhoneNumberParser : BaseSequenceParser
    { 
        public PhoneNumberParser()
        {

        }
        
        double ScorePhoneNumber(string phoneNumberText)
        {
            double baseScore = 0.3;
            double scoreIncrement = 0;

            string countryCodeFormat = @"^(\(\s?(\+\s?|00)\d{1,3}\s?\)|(\+\s?|00)\d{1,3})";
            string areaCodeFormat = @"\(";
            string separatorChar = @"[\s-/]";

            //Country code score or area code score 
            scoreIncrement += Regex.IsMatch(phoneNumberText, countryCodeFormat) ?
                                    0.4 : Regex.IsMatch(phoneNumberText, areaCodeFormat) ? 0.2 : 0;
 
            //Formatted score
            int separatorCount = Regex.Matches(phoneNumberText, separatorChar).Count;
            scoreIncrement += Math.Min(separatorCount, 4) * 0.1;
            
            //Length score
            scoreIncrement += Math.Min((phoneNumberText.Length - 8), 4) * 0.05;
           
            return Math.Max(Math.Min(baseScore + scoreIncrement, 1), 0);
        }
        
        public override ParseResult Parse(ExtractResult extResult)
        {
            var result = new ParseResult
            {
                Start = extResult.Start,
                Length = extResult.Length,
                Text = extResult.Text,
                Type = extResult.Type,
                ResolutionStr = extResult.Text,
                Score = ScorePhoneNumber(extResult.Text),
            };
            return result;
        }
    }
}
