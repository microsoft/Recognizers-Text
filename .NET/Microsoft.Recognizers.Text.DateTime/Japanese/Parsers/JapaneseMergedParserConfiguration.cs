using System;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.Japanese;

using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime.Japanese
{
    public class JapaneseMergedParserConfiguration : JapaneseCommonDateTimeParserConfiguration, ICJKMergedParserConfiguration
    {
        public JapaneseMergedParserConfiguration(ICJKCommonDateTimeParserConfiguration config)
            : base(config)
        {
            BeforeRegex = JapaneseMergedExtractorConfiguration.BeforeRegex;
            AfterRegex = JapaneseMergedExtractorConfiguration.AfterRegex;
            SincePrefixRegex = JapaneseMergedExtractorConfiguration.SincePrefixRegex;
            SinceSuffixRegex = JapaneseMergedExtractorConfiguration.SinceSuffixRegex;
            EqualRegex = JapaneseMergedExtractorConfiguration.EqualRegex;
            UntilRegex = JapaneseMergedExtractorConfiguration.UntilRegex;
        }

        public Regex BeforeRegex { get; }

        public Regex AfterRegex { get; }

        public Regex SincePrefixRegex { get; }

        public Regex SinceSuffixRegex { get; }

        public Regex UntilRegex { get; }

        public Regex EqualRegex { get; }
    }
}