using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.DateTime
{
    public abstract class AbstractYearExtractor : IDateExtractor
    {

        protected AbstractYearExtractor(IDateExtractorConfiguration config)
        {
            this.Config = config;
        }

        protected IDateExtractorConfiguration Config { get; private set; }

        public abstract List<ExtractResult> Extract(string input);

        public abstract List<ExtractResult> Extract(string input, System.DateTime reference);

        public int GetYearFromText(Match match)
        {
            int year = Constants.InvalidYear;

            var yearStr = match.Groups["year"].Value;
            var writtenYearStr = match.Groups["fullyear"].Value;

            if (!string.IsNullOrEmpty(yearStr) && !yearStr.Equals(writtenYearStr, StringComparison.Ordinal))
            {
                year = int.Parse(yearStr, CultureInfo.InvariantCulture);
                if (year < 100 && year >= Constants.MinTwoDigitYearPastNum)
                {
                    year += Constants.BASE_YEAR_PAST_CENTURY;
                }
                else if (year >= 0 && year < Constants.MaxTwoDigitYearFutureNum)
                {
                    year += Constants.BASE_YEAR_CURRENT_CENTURY;
                }
            }
            else
            {
                var firstTwoYearNumStr = match.Groups["firsttwoyearnum"].Value;
                if (!string.IsNullOrEmpty(firstTwoYearNumStr))
                {
                    var er = new ExtractResult
                    {
                        Text = firstTwoYearNumStr,
                        Start = match.Groups["firsttwoyearnum"].Index,
                        Length = match.Groups["firsttwoyearnum"].Length,
                    };

                    var firstTwoYearNum = Convert.ToInt32((double)(this.Config.NumberParser.Parse(er).Value ?? 0));

                    var lastTwoYearNum = 0;
                    var lastTwoYearNumStr = match.Groups["lasttwoyearnum"].Value;
                    if (!string.IsNullOrEmpty(lastTwoYearNumStr))
                    {
                        er.Text = lastTwoYearNumStr;
                        er.Start = match.Groups["lasttwoyearnum"].Index;
                        er.Length = match.Groups["lasttwoyearnum"].Length;

                        lastTwoYearNum = Convert.ToInt32((double)(this.Config.NumberParser.Parse(er).Value ?? 0));
                    }

                    // Exclude pure number like "nineteen", "twenty four"
                    if ((firstTwoYearNum < 100 && lastTwoYearNum == 0) ||
                        (firstTwoYearNum < 100 && firstTwoYearNum % 10 == 0 && lastTwoYearNumStr.Trim().Split(' ').Length == 1))
                    {
                        year = Constants.InvalidYear;
                        return year;
                    }

                    if (firstTwoYearNum >= 100)
                    {
                        year = firstTwoYearNum + lastTwoYearNum;
                    }
                    else
                    {
                        year = (firstTwoYearNum * 100) + lastTwoYearNum;
                    }
                }
                else
                {

                    if (!string.IsNullOrEmpty(writtenYearStr))
                    {
                        var er = new ExtractResult
                        {
                            Text = writtenYearStr,
                            Start = match.Groups["fullyear"].Index,
                            Length = match.Groups["fullyear"].Length,
                        };

                        year = Convert.ToInt32((double)(this.Config.NumberParser.Parse(er).Value ?? 0));

                        if (year < 100 && year >= Constants.MinTwoDigitYearPastNum)
                        {
                            year += Constants.BASE_YEAR_PAST_CENTURY;
                        }
                        else if (year >= 0 && year < Constants.MaxTwoDigitYearFutureNum)
                        {
                            year += Constants.BASE_YEAR_CURRENT_CENTURY;
                        }
                    }
                }
            }

            return year;
        }
    }
}
