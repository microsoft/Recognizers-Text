using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.Korean;
using Microsoft.Recognizers.Text.Number;
using Microsoft.Recognizers.Text.Number.Korean;
using Microsoft.Recognizers.Text.Utilities;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime.Korean
{
    public class KoreanDateTimePeriodExtractorConfiguration : BaseDateTimeOptionsConfiguration,
        ICJKDateTimePeriodExtractorConfiguration
    {

        public static readonly Regex TillRegex = new Regex(DateTimeDefinitions.DateTimePeriodTillRegex, RegexFlags);

        public static readonly Regex PrepositionRegex = new Regex(DateTimeDefinitions.DateTimePeriodPrepositionRegex, RegexFlags);

        public static readonly Regex ZhijianRegex = new Regex(DateTimeDefinitions.ZhijianRegex, RegexFlags);

        public static readonly Regex TimeOfDayRegex = new Regex(DateTimeDefinitions.TimeOfDayRegex, RegexFlags);

        public static readonly Regex SpecificTimeOfDayRegex = new Regex(DateTimeDefinitions.SpecificTimeOfDayRegex, RegexFlags);

        public static readonly Regex UnitRegex = new Regex(DateTimeDefinitions.DateTimePeriodUnitRegex, RegexFlags);

        public static readonly Regex FollowedUnit = new Regex(DateTimeDefinitions.DateTimePeriodFollowedUnit, RegexFlags);

        public static readonly Regex PastRegex = new Regex(DateTimeDefinitions.PastRegex, RegexFlags);

        public static readonly Regex FutureRegex = new Regex(DateTimeDefinitions.FutureRegex, RegexFlags);

        public static readonly Regex HourRegex = new Regex(DateTimeDefinitions.HourRegex, RegexFlags);
        public static readonly Regex HourNumRegex = new Regex(DateTimeDefinitions.HourNumRegex, RegexFlags);
        public static readonly Regex ThisRegex = new Regex(DateTimeDefinitions.DateTimePeriodThisRegex, RegexFlags);
        public static readonly Regex LastRegex = new Regex(DateTimeDefinitions.DateTimePeriodLastRegex, RegexFlags);
        public static readonly Regex NextRegex = new Regex(DateTimeDefinitions.DateTimePeriodNextRegex, RegexFlags);
        public static readonly Regex NumberCombinedWithUnit = new Regex(DateTimeDefinitions.DateTimePeriodNumberCombinedWithUnit, RegexFlags);

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        public KoreanDateTimePeriodExtractorConfiguration(IDateTimeOptionsConfiguration config)
            : base(config)
        {
            var numOptions = NumberOptions.None;
            if ((config.Options & DateTimeOptions.NoProtoCache) != 0)
            {
                numOptions = NumberOptions.NoProtoCache;
            }

            var numConfig = new BaseNumberOptionsConfiguration(config.Culture, numOptions);

            CardinalExtractor = new CardinalExtractor(numConfig);

            SingleDateExtractor = new BaseCJKDateExtractor(new KoreanDateExtractorConfiguration(this));
            SingleTimeExtractor = new BaseCJKTimeExtractor(new KoreanTimeExtractorConfiguration(this));
            SingleDateTimeExtractor = new BaseCJKDateTimeExtractor(new KoreanDateTimeExtractorConfiguration(this));
            DurationExtractor = new BaseCJKDurationExtractor(new KoreanDurationExtractorConfiguration(this));
            TimePeriodExtractor = new BaseCJKTimePeriodExtractor(new KoreanTimePeriodExtractorConfiguration(this));
        }

        public IExtractor CardinalExtractor { get; }

        public IDateTimeExtractor SingleDateExtractor { get; }

        public IDateTimeExtractor SingleTimeExtractor { get; }

        public IDateTimeExtractor SingleDateTimeExtractor { get; }

        public IDateTimeExtractor DurationExtractor { get; }

        public IDateTimeExtractor TimePeriodExtractor { get; }

        Regex ICJKDateTimePeriodExtractorConfiguration.PrepositionRegex => PrepositionRegex;

        Regex ICJKDateTimePeriodExtractorConfiguration.TillRegex => TillRegex;

        Regex ICJKDateTimePeriodExtractorConfiguration.SpecificTimeOfDayRegex => SpecificTimeOfDayRegex;

        Regex ICJKDateTimePeriodExtractorConfiguration.TimeOfDayRegex => TimeOfDayRegex;

        Regex ICJKDateTimePeriodExtractorConfiguration.FollowedUnit => FollowedUnit;

        Regex ICJKDateTimePeriodExtractorConfiguration.UnitRegex => UnitRegex;

        Regex ICJKDateTimePeriodExtractorConfiguration.PastRegex => PastRegex;

        Regex ICJKDateTimePeriodExtractorConfiguration.FutureRegex => FutureRegex;

        public bool GetFromTokenIndex(string text, out int index)
        {
            index = -1;

            // @TODO move hardcoded values to resources file
            if (text.Trim().EndsWith("从", StringComparison.Ordinal))
            {
                index = text.LastIndexOf("从", StringComparison.Ordinal);
                return true;
            }

            return false;
        }

        public bool GetBetweenTokenIndex(string text, out int index)
        {
            index = -1;
            var match = ZhijianRegex.Match(text);
            if (match.Success)
            {
                index = match.Length;
                return true;
            }

            return false;
        }

        public bool HasConnectorToken(string text)
        {
            // @TODO move hardcoded values to resources file
            return text.Equals("和", StringComparison.Ordinal) ||
                    text.Equals("与", StringComparison.Ordinal) ||
                    text.Equals("到", StringComparison.Ordinal);
        }
    }
}