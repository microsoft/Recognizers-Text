using System;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.Chinese;

using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime.Chinese
{
    public class ChineseMergedParserConfiguration : ChineseCommonDateTimeParserConfiguration, ICJKMergedParserConfiguration
    {
        public ChineseMergedParserConfiguration(ICJKCommonDateTimeParserConfiguration config)
            : base(config)
        {
            BeforeRegex = ChineseMergedExtractorConfiguration.BeforeRegex;
            AfterRegex = ChineseMergedExtractorConfiguration.AfterRegex;
            SincePrefixRegex = ChineseMergedExtractorConfiguration.SincePrefixRegex;
            SinceSuffixRegex = ChineseMergedExtractorConfiguration.SinceSuffixRegex;
            EqualRegex = ChineseMergedExtractorConfiguration.EqualRegex;
            UntilRegex = ChineseMergedExtractorConfiguration.UntilRegex;
        }

        public Regex BeforeRegex { get; }

        public Regex AfterRegex { get; }

        public Regex SincePrefixRegex { get; }

        public Regex SinceSuffixRegex { get; }

        public Regex UntilRegex { get; }

        public Regex EqualRegex { get; }
    }
}