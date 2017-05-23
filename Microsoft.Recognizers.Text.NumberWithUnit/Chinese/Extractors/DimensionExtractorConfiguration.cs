using Microsoft.Recognizers.Text.Number.Utilities;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Chinese.Extractors
{
    public class DimensionExtractorConfiguration : ChineseNumberWithUnitExtractorConfiguration
    {
        public DimensionExtractorConfiguration() : this(new CultureInfo(Culture.Chinese)) { }

        public DimensionExtractorConfiguration(CultureInfo ci) : base(ci) { }

        public override ImmutableDictionary<string, string> SuffixList => DimensionSuffixList;

        public override ImmutableDictionary<string, string> PrefixList => null;

        public override ImmutableList<string> AmbiguousUnitList => ambiguousUnitList;

        public override string ExtractType => Constants.SYS_UNIT_DIMENSION;

        public static readonly ImmutableDictionary<string, string> DimensionSuffixList = new Dictionary<string, string>
        {
            // China 公制
            {"Meter", "米|	公尺"},
            {"Kilometer", "千米|公里"},
            {"Decimeter", "分米|公寸"},
            {"Cntimeter", "釐米|厘米|公分"},
            {"Micrometer", "毫米|公釐"},
            {"Microns", "微米"},
            {"Picometer", "皮米"},
            {"Nanometer", "纳米"},
            // China 市制
            {"Li", "里|市里"},
            {"Zhang", "丈"},
            {"Chi", "市尺|尺"},
            {"Cun", "市寸|寸"},
            {"Fen", "市分|分"},
            {"Hao", "毫"},
            {"Mile", "英里"},
            {"Inch", "英寸"},
            {"Foot", "呎|英尺"},
            {"Yard", "码"},
            {"Knot", "海里"},
            {"Light year", "光年"},
            // Spped
            {"Meter per second", "米每秒|米/秒"},
            {"Kilometer per hour", "公里每小时|千米每小时|公里/小时|千米/小时"},
            {"Kilometer per minute", "公里每分钟|千米每分钟|公里/分钟|千米/分钟"},
            {"Kilometer per second", "公里每秒|千米每秒|公里/秒|千米/秒"},
            {"Mile per hour", "英里每小时|英里/小时"},
            {"Foot per second", "英尺每小时|英尺/小时"},
            {"Foot per minute", "英尺每分钟|英尺/分钟"},
            {"Yard per minute", "码每分|码/分"},
            {"Yard per second", "码每秒|码/秒"},
            // Area
            {"Square centimetre", "平方厘米"},
            {"Square decimeter", "平方分米"},
            {"Square meter", "平方米"},
            {"Square kilometer", "平方公里"},
            {"Acre", "英亩|公亩"},
            {"Mu", "亩|市亩"},
            {"Hectare", "公顷"},
            {"Liter", "公升|升"},
            {"Dou", "市斗|斗"},
            {"Dan", "市石|石"},
            {"Pt", "品脱"},
            {"Milliliter", "毫升"},
            {"Cubic meter", "立方米"},
            {"Cubic decimeter", "立方分米"},
            {"Cubic millimeter", "立方毫米"},
            {"Cubic feet", "立方英尺"},
            // Weight
            {"Kilogram", "千克|公斤"},
            {"Jin", "市斤|斤"},
            {"Milligram", "毫克"},
            {"Barrel", "桶"},
            {"Pot", "罐"},
            {"Gallon", "加仑"},
            {"Gram", "克"},
            {"Ton", "公吨|吨"},
            {"Pound", "磅"},
            {"Ounce", "盎司"},
            // Information
            {"Bit", "比特|位"},
            {"Byte", "字节"},
            {"Kilobyte", "千字节"},
            {"Megabyte", "兆字节"},
            {"Gigabyte", "十亿字节|千兆字节"},
            {"Terabyte", "万亿字节|兆兆字节"},
            {"Petabyte", "千兆兆|千万亿字节"}
        }.ToImmutableDictionary();

        private static readonly ImmutableList<string> ambiguousUnitList = new List<string>
        {
            "丈",
            "位",
            "克",
            "分",
            "升",
            "寸",
            "尺",
            "斗",
            "斤",
            "桶",
            "毫",
            "石",
            "码",
            "磅",
            "米",
            "罐",
            "里",
        }.ToImmutableList();
    }
}
