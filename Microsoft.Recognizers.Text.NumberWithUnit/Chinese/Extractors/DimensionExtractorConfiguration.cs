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

        public override string ExtractType => Constants.SYS_UNIT_DIMENSION;

        public static readonly ImmutableDictionary<string, string> DimensionSuffixList = new Dictionary<string, string>
        {
            // China 公制
            {"Meter", "m|米|	公尺"},
            {"Kilometer", "km|千米|公里"},
            {"Decimeter", "dm|分米|公寸"},
            {"Cntimeter", "cm|釐米|厘米|公分"},
            {"Micrometer", "mm|毫米|公釐"},
            {"Microns", "μm|微米"},
            {"Picometer", "pm|皮米"},
            {"Nanometer", "nm|纳米"},
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
            {"Knot", "kt|海里"},
            {"Light year", "光年"},
            // Spped
            {"Meter per second", "m/s|米每秒"},
            {"Kilometer per hour", "km/h|公里每小时|千米每小时"},
            {"Kilometer per minute", "km/min|公里每分钟|千米每分钟"},
            {"Kilometer per second", "km/s|公里每秒|千米每秒"},
            {"Mile per hour", "mph|英里每小时"},
            {"Foot per second", "ft/s|英尺每小时"},
            {"Foot per minute", "ft/min|英尺每分钟"},
            {"Yard per minute", "yards/min|码每分"},
            {"Yard per second", "yards/s|码每秒"},
            // Area
            {"Square centimetre", "cm2|平方厘米"},
            {"Square decimeter", "dm2|平方分米"},
            {"Square meter", "m2|平方米"},
            {"Square kilometer", "km2|平方公里"},
            {"Acre", "英亩|公亩"},
            {"Mu", "亩|市亩"},
            {"Hectare", "公顷"},
            {"Liter", "l|公升|升"},
            {"Dou", "市斗|斗"},
            {"Dan", "市石|石"},
            {"Pt", "品脱"},
            {"Milliliter", "ml|毫升"},
            {"Cubic meter", "m3|立方米"},
            {"Cubic decimeter", "立方分米"},
            {"Cubic millimeter", "立方毫米"},
            {"Cubic feet", "立方英尺"},
            // Weight
            {"Kilogram", "kg|千克|公斤"},
            {"Jin", "市斤|斤"},
            {"Milligram", "mg|毫克"},
            {"Barrel", "桶"},
            {"Pot", "罐"},
            {"Gallon", "加仑"},
            {"Gram", "g|克"},
            {"Metric ton", "mt"},
            {"Ton", "公吨|吨"},
            {"Pound", "磅"},
            {"Ounce", "oz|盎司"},
            // Information
            {"Bit", "比特|位"},
            {"Byte", "字节"},
            {"Kilobyte", "kb|千字节"},
            {"Megabyte", "mb|兆字节"},
            {"Gigabyte", "gb|十亿字节|千兆字节"},
            {"Terabyte", "tb|万亿字节|兆兆字节"},
            {"Petabyte", "pb|千兆兆|千万亿字节"}
        }.ToImmutableDictionary();
    }
}
