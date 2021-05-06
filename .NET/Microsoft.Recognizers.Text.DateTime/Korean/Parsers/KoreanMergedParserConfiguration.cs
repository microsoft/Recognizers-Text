using System;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.Korean;

using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime.Korean
{
    public class KoreanMergedParserConfiguration : KoreanCommonDateTimeParserConfiguration, ICJKMergedParserConfiguration
    {
        public KoreanMergedParserConfiguration(ICJKCommonDateTimeParserConfiguration config)
            : base(config)
        {
            BeforeRegex = KoreanMergedExtractorConfiguration.BeforeRegex;
            AfterRegex = KoreanMergedExtractorConfiguration.AfterRegex;
            SincePrefixRegex = KoreanMergedExtractorConfiguration.SincePrefixRegex;
            SinceSuffixRegex = KoreanMergedExtractorConfiguration.SinceSuffixRegex;
            EqualRegex = KoreanMergedExtractorConfiguration.EqualRegex;
            UntilRegex = KoreanMergedExtractorConfiguration.UntilRegex;
        }

        public Regex BeforeRegex { get; }

        public Regex AfterRegex { get; }

        public Regex SincePrefixRegex { get; }

        public Regex SinceSuffixRegex { get; }

        public Regex UntilRegex { get; }

        public Regex EqualRegex { get; }
    }
}