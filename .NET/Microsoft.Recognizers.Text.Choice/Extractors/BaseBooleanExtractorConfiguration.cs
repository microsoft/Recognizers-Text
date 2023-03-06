// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.Choice.Extractors
{
    public abstract class BaseBooleanExtractorConfiguration : IBooleanExtractorConfiguration
    {
        public BaseBooleanExtractorConfiguration(
            string trueRegex,
            string falseRegex,
            string tokenRegex,
            RegexOptions options,
            bool allowPartialMatch,
            int maxDistance,
            bool onlyTopMatch)
        {
            TrueRegex = new Regex(trueRegex, options, RegexTimeOut);
            FalseRegex = new Regex(falseRegex, options, RegexTimeOut);
            TokenRegex = new Regex(tokenRegex, options, RegexTimeOut);
            MapRegexes = new Dictionary<Regex, string>()
            {
                { TrueRegex, Constants.SYS_BOOLEAN_TRUE },
                { FalseRegex, Constants.SYS_BOOLEAN_FALSE },
            };
            AllowPartialMatch = allowPartialMatch;
            MaxDistance = maxDistance;
            OnlyTopMatch = onlyTopMatch;
        }

        public static TimeSpan RegexTimeOut => ChoiceRecognizer.GetTimeout(MethodBase.GetCurrentMethod().DeclaringType);

        public Regex TrueRegex { get; set; }

        public Regex FalseRegex { get; set; }

        public IDictionary<Regex, string> MapRegexes { get; set; }

        public Regex TokenRegex { get; set; }

        public bool AllowPartialMatch { get; set; }

        public int MaxDistance { get; set; }

        public bool OnlyTopMatch { get; set; }
    }
}
