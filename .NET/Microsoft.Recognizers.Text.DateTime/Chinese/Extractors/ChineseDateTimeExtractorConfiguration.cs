﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.Chinese;
using Microsoft.Recognizers.Definitions.Utilities;
using Microsoft.Recognizers.Text.Utilities;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime.Chinese
{
    public class ChineseDateTimeExtractorConfiguration : BaseDateTimeOptionsConfiguration, ICJKDateTimeExtractorConfiguration
    {
        public static readonly string ExtractorName = Constants.SYS_DATETIME_DATETIME; // "DateTime";

        public static readonly Regex PrepositionRegex = new Regex(DateTimeDefinitions.PrepositionRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex NowRegex = new Regex(DateTimeDefinitions.NowRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex NightRegex = new Regex(DateTimeDefinitions.NightRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex TimeOfSpecialDayRegex = new Regex(DateTimeDefinitions.TimeOfSpecialDayRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex TimeOfDayRegex = new Regex(DateTimeDefinitions.TimeOfDayRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex BeforeRegex = new Regex(DateTimeDefinitions.BeforeRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex AfterRegex = new Regex(DateTimeDefinitions.AfterRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex DurationRelativeDurationUnitRegex = new Regex(DateTimeDefinitions.DurationRelativeDurationUnitRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex AgoLaterRegex = new Regex(DateTimeDefinitions.AgoLaterRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex DateTimePeriodUnitRegex = new Regex(DateTimeDefinitions.DateTimePeriodUnitRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex ConnectorRegex = new Regex(DateTimeDefinitions.ConnectorRegex, RegexFlags, RegexTimeOut);

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        public ChineseDateTimeExtractorConfiguration(IDateTimeOptionsConfiguration config)
            : base(config)
        {

            DatePointExtractor = new BaseCJKDateExtractor(new ChineseDateExtractorConfiguration(this));
            TimePointExtractor = new BaseCJKTimeExtractor(new ChineseTimeExtractorConfiguration(this));
            DurationExtractor = new BaseCJKDurationExtractor(new ChineseDurationExtractorConfiguration(this));
        }

        public IDateTimeExtractor DatePointExtractor { get; }

        public IDateTimeExtractor TimePointExtractor { get; }

        public IDateTimeExtractor DurationExtractor { get; }

        Regex ICJKDateTimeExtractorConfiguration.NowRegex => NowRegex;

        Regex ICJKDateTimeExtractorConfiguration.PrepositionRegex => PrepositionRegex;

        Regex ICJKDateTimeExtractorConfiguration.NightRegex => NightRegex;

        Regex ICJKDateTimeExtractorConfiguration.TimeOfSpecialDayRegex => TimeOfSpecialDayRegex;

        Regex ICJKDateTimeExtractorConfiguration.TimeOfDayRegex => TimeOfDayRegex;

        Regex ICJKDateTimeExtractorConfiguration.BeforeRegex => BeforeRegex;

        Regex ICJKDateTimeExtractorConfiguration.AfterRegex => AfterRegex;

        Regex ICJKDateTimeExtractorConfiguration.ConnectorRegex => ConnectorRegex;

        public Dictionary<Regex, Regex> AmbiguityDateTimeFiltersDict => DefinitionLoader.LoadAmbiguityFilters(DateTimeDefinitions.AmbiguityDateTimeFiltersDict);

    }
}