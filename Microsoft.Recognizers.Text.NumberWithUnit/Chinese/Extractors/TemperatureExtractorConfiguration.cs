using Microsoft.Recognizers.Text.Number.Utilities;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Chinese.Extractors
{
    public class TemperatureExtractorConfiguration : ChineseNumberWithUnitExtractorConfiguration
    {
        public TemperatureExtractorConfiguration() : this(new CultureInfo(Culture.Chinese)) { }

        public TemperatureExtractorConfiguration(CultureInfo ci) : base(ci) { }

        public override ImmutableDictionary<string, string> SuffixList => TemperatureSuffixList;

        public override ImmutableDictionary<string, string> PrefixList => TemperaturePrefixList;
        public override ImmutableList<string> AmbiguousUnitList => ambiguousUnitList;

        public override string ExtractType => Constants.SYS_UNIT_TEMPERATURE;

        public static readonly ImmutableDictionary<string, string> TemperatureSuffixList = new Dictionary<string, string>
        {
            {"F", "华氏温度|华氏度"},
            {"K", "k|开尔文温度|开氏度|凯氏度"},
            {"R", "兰氏温度"},
            {"C", "摄氏温度|摄氏度"},
            {"Degree", "度"}
        }.ToImmutableDictionary();

        public static readonly ImmutableDictionary<string, string> TemperaturePrefixList = new Dictionary<string, string>
        {
            //华氏十三度
            {"F", "华氏"},
            {"K", "开氏"},
            {"R", "兰氏"},
            {"C", "摄氏"}
        }.ToImmutableDictionary();

        private static readonly ImmutableList<string> ambiguousUnitList = new List<string>
        {
            "度",
        }.ToImmutableList();
    }
}
