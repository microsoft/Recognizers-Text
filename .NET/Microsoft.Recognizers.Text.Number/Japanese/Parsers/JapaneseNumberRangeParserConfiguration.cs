﻿using Microsoft.Recognizers.Definitions.Japanese;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.Number.Japanese
{
    public class JapaneseNumberRangeParserConfiguration : INumberRangeParserConfiguration
    {
        public CultureInfo CultureInfo { get; private set; }

        public IExtractor NumberExtractor { get; private set; }

        public IExtractor OrdinalExtractor { get; private set; }

        public IParser NumberParser { get; private set; }

        public Regex MoreOrEqual { get; private set; }

        public Regex LessOrEqual { get; private set; }

        public Regex MoreOrEqualSuffix { get; private set; }

        public Regex LessOrEqualSuffix { get; private set; }

        public Regex MoreOrEqualSeparate { get; private set; }

        public Regex LessOrEqualSeparate { get; private set; }

        public JapaneseNumberRangeParserConfiguration() : this(new CultureInfo(Culture.Japanese))
        {
        }

        public JapaneseNumberRangeParserConfiguration(CultureInfo ci)
        {
            CultureInfo = ci;

            NumberExtractor = new NumberExtractor();
            OrdinalExtractor = new OrdinalExtractor();
            NumberParser = new BaseCJKNumberParser(new JapaneseNumberParserConfiguration());
            MoreOrEqual = new Regex(NumbersDefinitions.MoreOrEqual, RegexOptions.Singleline);
            LessOrEqual = new Regex(NumbersDefinitions.LessOrEqual, RegexOptions.Singleline);
            MoreOrEqualSuffix = new Regex(NumbersDefinitions.MoreOrEqualSuffix, RegexOptions.Singleline);
            LessOrEqualSuffix = new Regex(NumbersDefinitions.LessOrEqualSuffix, RegexOptions.Singleline);
            MoreOrEqualSeparate = new Regex(NumbersDefinitions.OneNumberRangeMoreSeparateRegex, RegexOptions.Singleline);
            LessOrEqualSeparate = new Regex(NumbersDefinitions.OneNumberRangeLessSeparateRegex, RegexOptions.Singleline);
        }
    }
}
