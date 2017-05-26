using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Text.NumberWithUnit;
using Microsoft.Recognizers.Text.NumberWithUnit.Chinese.Extractors;

namespace Microsoft.Recognizers.Text.DateTime.Chinese.Extractors
{
    public class DurationExtractorChs : BaseDateTimeExtractor<DurationType>
    {
        internal override ImmutableDictionary<Regex, DurationType> Regexes { get; }
        protected sealed override string ExtractType { get; } = Constants.SYS_DATETIME_DURATION; // "Duration";

        private static readonly IExtractor InternalExtractor = new NumberWithUnitExtractor(new DurationExtractorConfiguration());

        private static readonly Regex YearRegex = new Regex(@"((19\d{2}|20\d{2})|两千)年",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        // extract by number with unit
        public override List<ExtractResult> Extract(string source)
        {
            //Use Unit to extract 
            var retList = InternalExtractor.Extract(source);
            var res = new List<ExtractResult>();
            foreach (var ret in retList)
            {
                var match = YearRegex.Match(ret.Text);
                if (match.Success)
                {
                    continue;
                }
                res.Add(ret);
            }
            return res;
        }

        internal class DurationExtractorConfiguration : ChineseNumberWithUnitExtractorConfiguration
        {
            public DurationExtractorConfiguration() : base(new CultureInfo("zh-CN")) { }

            public override ImmutableDictionary<string, string> SuffixList => DurationSuffixList;

            public override ImmutableDictionary<string, string> PrefixList => null;

            public override string ExtractType => Constants.SYS_DATETIME_DURATION;

            public static readonly ImmutableDictionary<string, string> DurationSuffixList = new Dictionary<string, string>
            {
                {"M", "分钟"},
                {"S", "秒钟|秒"},
                {"H", "个小时|小时"},
                {"D", "天"},
                {"W", "星期|个星期|周"},
                {"Mon", "个月"},
                {"Y", "年"}
            }.ToImmutableDictionary();

            public override ImmutableList<string> AmbiguousUnitList => AmbiguousUnits;
            public static readonly ImmutableList<string> AmbiguousUnits = new List<string>()
            {
                "分钟",
                "秒钟",
                "秒",
                "个小时",
                "小时",
                "天",
                "星期",
                "个星期",
                "周",
                "个月",
                "年",
            }.ToImmutableList();
        }
    }

    public enum DurationType
    {
        WithNumber
    }
}