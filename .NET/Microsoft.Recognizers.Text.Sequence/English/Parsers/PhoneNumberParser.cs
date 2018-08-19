using System;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions;

namespace Microsoft.Recognizers.Text.Sequence.English
{
    public class PhoneNumberParser : BaseSequenceParser
    {
        private static double scoreUpperBound = 1;
        private static double scoreLowerBound = 0;
        private static double baseScore = 0.3;
        private static double countryCodeAward = 0.4;
        private static double areaCodeAward = 0.2;
        private static double formattedAward = 0.1;
        private static double lengthAward = 0.05;
        private static int maxFormatIndicatorNum = 4;
        private static int maxLengthAwardNum = 4;
        private static int phoneNumberLengthBase = 8;

        public PhoneNumberParser()
        {

        }
        
        double ScorePhoneNumber(string phoneNumberText)
        {
            double score = baseScore;

            Regex countryCodeRegex = new Regex(BasePhoneNumbers.CountryCodeRegex);
            Regex areaCodeRegex = new Regex(BasePhoneNumbers.AreaCodeIndicatorRegex);
            Regex formatIndicatorRegex = new Regex(BasePhoneNumbers.FormatIndicatorRegex);

            // Country code score or area code score 
            score += countryCodeRegex.IsMatch(phoneNumberText) ?
                                    countryCodeAward : areaCodeRegex.IsMatch(phoneNumberText) ? areaCodeAward : 0;
            
            // Formatted score
            if (formatIndicatorRegex.IsMatch(phoneNumberText))
            {
                int formatIndicatorCount = formatIndicatorRegex.Matches(phoneNumberText).Count;
                score += Math.Min(formatIndicatorCount, maxFormatIndicatorNum) * formattedAward;
            }
            
            // Length score
            score += Math.Min((phoneNumberText.Length - phoneNumberLengthBase), maxLengthAwardNum) * lengthAward;

            return Math.Max(Math.Min(score, scoreUpperBound), scoreLowerBound) / scoreUpperBound;
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
                Value = ScorePhoneNumber(extResult.Text),
            };
            return result;
        }
    }
}
