using System;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions;

namespace Microsoft.Recognizers.Text.Sequence.English
{
    public class PhoneNumberParser : BaseSequenceParser
    {
        private static double scoreUpperLimit = 100;
        private static double scoreLowerLimit = 0;
        private static double baseScore = 30;
        private static double countryCodeAward = 40;
        private static double areaCodeAward = 30;
        private static double formattedAward = 20;
        private static double lengthAward = 10;
        private static double typicalFormatDeductionScore = 40;
        private static double continueDigitDeductionScore = 10;
        private static double tailSameDeductionScore = 10;
        private static double continueFormatIndicatorDeductionScore = 20;
        private static double wrongFormatDeductionScore = 20;
        private static int maxFormatIndicatorNum = 3;
        private static int maxLengthAwardNum = 3;
        private static int tailSameLimit = 2;
        private static int phoneNumberLengthBase = 8;
        private static int pureDigitLengthLimit = 11;

        // @TODO move regexes to base resource files
        private static string completeBracketRegex = @"\(.*\)";
        private static string singleBracketRegex = @"\(|\)";
        private static string tailSameDigitRegex = @"([\d])\1{2,10}$";
        private static string pureDigitRegex = @"^\d*$";
        private static string continueDigitRegex = @"\d{5}\d*";
        private static string digitRegex = @"\d";

        private static readonly Regex CountryCodeRegex = new Regex(BasePhoneNumbers.CountryCodeRegex);
        private static readonly Regex AreaCodeRegex = new Regex(BasePhoneNumbers.AreaCodeIndicatorRegex);
        private static readonly Regex FormatIndicatorRegex = new Regex(BasePhoneNumbers.FormatIndicatorRegex);
        private static readonly Regex NoAreaCodeUSphonenumbeRegex = new Regex(BasePhoneNumbers.NoAreaCodeUSPhoneNumberRegex);

        public static double ScorePhoneNumber(string phoneNumberText)
        {
            double score = baseScore;

            // Country code score or area code score
            score += CountryCodeRegex.IsMatch(phoneNumberText) ?
                                    countryCodeAward : AreaCodeRegex.IsMatch(phoneNumberText) ? areaCodeAward : 0;

            // Formatted score
            if (FormatIndicatorRegex.IsMatch(phoneNumberText))
            {
                var formatMatches = FormatIndicatorRegex.Matches(phoneNumberText);
                int formatIndicatorCount = formatMatches.Count;
                score += Math.Min(formatIndicatorCount, maxFormatIndicatorNum) * formattedAward;
                score -= formatMatches.Cast<Match>().Any(o => o.Value.Length > 1) ? continueFormatIndicatorDeductionScore : 0;
                if (Regex.IsMatch(phoneNumberText, singleBracketRegex) &&
                    !Regex.IsMatch(phoneNumberText, completeBracketRegex))
                {
                    score -= wrongFormatDeductionScore;
                }
            }

            // Length score
            score += Math.Min(Regex.Matches(phoneNumberText, digitRegex).Count - phoneNumberLengthBase, maxLengthAwardNum) * lengthAward;

            // Same tailing digit deduction
            if (Regex.IsMatch(phoneNumberText, tailSameDigitRegex))
            {
                score -= (Regex.Match(phoneNumberText, tailSameDigitRegex).Length - tailSameLimit) * tailSameDeductionScore;
            }

            // Pure digit deduction
            if (Regex.IsMatch(phoneNumberText, pureDigitRegex))
            {
                score -= phoneNumberText.Length > pureDigitLengthLimit ?
                    (phoneNumberText.Length - pureDigitLengthLimit) * lengthAward : 0;
            }

            // Special format deduction
            score -= BasePhoneNumbers.TypicalDeductionRegexList.Any(o => Regex.IsMatch(phoneNumberText, o)) ? typicalFormatDeductionScore : 0;

            // Continue digit deduction
            score -= Math.Max(Regex.Matches(phoneNumberText, continueDigitRegex).Count - 1, 0) * continueDigitDeductionScore;

            // Special award for USphonenumber without area code, i.e. 223-4567 or 223 - 4567
            if (NoAreaCodeUSphonenumbeRegex.IsMatch(phoneNumberText))
            {
                score += lengthAward * 1.5;
            }

            return Math.Max(Math.Min(score, scoreUpperLimit), scoreLowerLimit) / (scoreUpperLimit - scoreLowerLimit);
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
