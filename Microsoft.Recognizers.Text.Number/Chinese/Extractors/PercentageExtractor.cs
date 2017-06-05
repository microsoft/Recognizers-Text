using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.Number.Chinese
{
    public class PercentageExtractor : BaseNumberExtractor
    {
        internal sealed override ImmutableDictionary<Regex, string> Regexes { get; }
        protected sealed override string ExtractType { get; } = Constants.SYS_NUM_PERCENTAGE;

        public PercentageExtractor()
        {
            var regexes = new Dictionary<Regex, string>()
            {
                //二十个百分点,  四点五个百分点
                {
                    new Regex(
                        $@"(?<!{IntegerExtractor.AllIntRegexChs})({DoubleExtractor.AllFloatRegexChs}|{IntegerExtractor
                            .AllIntRegexChs})\s*[个個]\s*[百佰]\s*分\s*[点點]",
                        RegexOptions.Singleline),
                    "PerChs"
                },
                //百分之五十  百分之一点五
                {
                    new Regex(
                        $@"(?<!{IntegerExtractor.ZeroToNineIntegerRegexChs})[百佰]\s*分\s*之\s*({DoubleExtractor
                            .AllFloatRegexChs}|{IntegerExtractor
                                .AllIntRegexChs}|[百佰])(?!{IntegerExtractor.AllIntRegexChs})",
                        RegexOptions.Singleline),
                    "PerChs"
                },
                //百分之５６.２　百分之１２
                {
                    new Regex(
                        $@"(?<!{IntegerExtractor.ZeroToNineIntegerRegexChs})[百佰]\s*分\s*之\s*{IntegerExtractor
                            .ZeroToNineChsFullHalfRegexChs}+[\.．]{IntegerExtractor
                                .ZeroToNineChsFullHalfRegexChs}+(?!([\.．]{IntegerExtractor
                                    .ZeroToNineChsFullHalfRegexChs}+))",
                        RegexOptions.IgnoreCase | RegexOptions.Singleline),
                    "PerNum"
                },
                //百分之3,000  百分之１，１２３
                {
                    new Regex(
                        $@"(?<!{IntegerExtractor.ZeroToNineIntegerRegexChs})[百佰]\s*分\s*之\s*{IntegerExtractor
                            .ZeroToNineChsFullHalfRegexChs}" + @"{1,3}([,，]" +
                        $@"{IntegerExtractor.ZeroToNineChsFullHalfRegexChs}" + @"{3})+[\.．]" +
                        $@"{IntegerExtractor.ZeroToNineChsFullHalfRegexChs}+",
                        RegexOptions.IgnoreCase | RegexOptions.Singleline),
                    "PerNum"
                },
                //百分之3.2 k 
                {
                    new Regex(
                        $@"(?<!{IntegerExtractor.ZeroToNineIntegerRegexChs})[百佰]\s*分\s*之\s*{IntegerExtractor
                            .ZeroToNineChsFullHalfRegexChs}+[\.．]{IntegerExtractor
                                .ZeroToNineChsFullHalfRegexChs}+\s*(K|k|M|G|T|Ｍ|Ｋ|ｋ|Ｇ|Ｔ)",
                        RegexOptions.Singleline),
                    "PerNum"
                },
                //12.56个百分点  ０.４个百分点
                {
                    new Regex(
                        $@"(?<!({IntegerExtractor.ZeroToNineChsFullHalfRegexChs}+[\.．])){IntegerExtractor
                            .ZeroToNineChsFullHalfRegexChs}+[\.．]{IntegerExtractor.ZeroToNineChsFullHalfRegexChs}+(?!([\.．]{IntegerExtractor
                                .ZeroToNineChsFullHalfRegexChs}+))\s*[个個]\s*[百佰]\s*分\s*[点點]",
                        RegexOptions.IgnoreCase | RegexOptions.Singleline),
                    "PerNum"
                },
                //15,123个百分点  １１１，１１１个百分点
                {
                    new Regex(
                        $@"{IntegerExtractor.ZeroToNineChsFullHalfRegexChs}" + @"{1,3}([,，]" +
                        $@"{IntegerExtractor.ZeroToNineChsFullHalfRegexChs}" + @"{3})+[\.．]" +
                        $@"{IntegerExtractor.ZeroToNineChsFullHalfRegexChs}+\s*[个個]\s*[百佰]\s*分\s*[点點]",
                        RegexOptions.IgnoreCase | RegexOptions.Singleline),
                    "PerNum"
                },
                //12.1k个百分点  １５.1k个百分点
                {
                    new Regex(
                        $@"{IntegerExtractor.ZeroToNineChsFullHalfRegexChs}+[\.．]{IntegerExtractor
                            .ZeroToNineChsFullHalfRegexChs}+\s*(K|k|M|G|T|Ｍ|Ｋ|ｋ|Ｇ|Ｔ)\s*[个個]\s*[百佰]\s*分\s*[点點]",
                        RegexOptions.Singleline),
                    "PerNum"
                },
                //百分之22  百分之１２０
                {
                    new Regex(
                        $@"(?<!{IntegerExtractor.ZeroToNineIntegerRegexChs})[百佰]\s*分\s*之\s*{IntegerExtractor
                            .ZeroToNineChsFullHalfRegexChs}+(?!([\.．]{IntegerExtractor
                                .ZeroToNineChsFullHalfRegexChs}+))",
                        RegexOptions.IgnoreCase | RegexOptions.Singleline),
                    "PerNum"
                },
                //百分之15k 
                {
                    new Regex(
                        $@"(?<!{IntegerExtractor.ZeroToNineIntegerRegexChs})[百佰]\s*分\s*之\s*{IntegerExtractor
                            .ZeroToNineChsFullHalfRegexChs}+\s*(K|k|M|G|T|Ｍ|Ｋ|ｋ|Ｇ|Ｔ)",
                        RegexOptions.Singleline),
                    "PerNum"
                },
                //百分之1,111  百分之９，９９９
                {
                    new Regex(
                        $@"(?!{IntegerExtractor.ZeroToNineIntegerRegexChs})[百佰]\s*分\s*之\s*{IntegerExtractor
                            .ZeroToNineChsFullHalfRegexChs}" + @"{1,3}([,，]" +
                        $@"{IntegerExtractor.ZeroToNineChsFullHalfRegexChs}" + @"{3})+",
                        RegexOptions.IgnoreCase | RegexOptions.Singleline),
                    "PerNum"
                },
                //12个百分点
                {
                    new Regex(
                        $@"{IntegerExtractor.ZeroToNineChsFullHalfRegexChs}+\s*[个個]\s*[百佰]\s*分\s*[点點]",
                        RegexOptions.IgnoreCase | RegexOptions.Singleline),
                    "PerNum"
                },
                //12k个百分点
                {
                    new Regex(
                        $@"{IntegerExtractor.ZeroToNineChsFullHalfRegexChs}+\s*(K|k|M|G|T|Ｍ|Ｋ|ｋ|Ｇ|Ｔ)\s*[个個]\s*[百佰]\s*分\s*[点點]",
                        RegexOptions.Singleline),
                    "PerNum"
                },
                //2,123个百分点
                {
                    new Regex(
                        $@"{IntegerExtractor.ZeroToNineChsFullHalfRegexChs}" + @"{1,3}([,，]" +
                        $@"{IntegerExtractor.ZeroToNineChsFullHalfRegexChs}" + @"{3})+\s*[个個]\s*[百佰]\s*分\s*[点點]",
                        RegexOptions.IgnoreCase | RegexOptions.Singleline),
                    "PerNum"
                },
                {
                    new Regex(
                        $@"{IntegerExtractor.SignSymbolRegexNum}?{IntegerExtractor.ZeroToNineChsFullHalfRegexChs}+([\.．]{IntegerExtractor
                            .ZeroToNineChsFullHalfRegexChs}+)?(\s*)[％%]",
                        RegexOptions.IgnoreCase | RegexOptions.Singleline),
                    "PerNum"
                },
                //2折 ２.５折
                {
                    new Regex(
                        $@"{IntegerExtractor.ZeroToNineChsFullHalfRegexChs}(([\.．]?|\s*){IntegerExtractor
                            .ZeroToNineChsFullHalfRegexChs})?\s*折",
                        RegexOptions.IgnoreCase | RegexOptions.Singleline),
                    "PerSpe"
                },
                //三折 六点五折 七五折
                {
                    new Regex(
                        $@"{IntegerExtractor.ZeroToNineIntegerRegexChs}(\s*[点點]?\s*{IntegerExtractor
                            .ZeroToNineIntegerRegexChs})?\s*折",
                        RegexOptions.IgnoreCase | RegexOptions.Singleline),
                    "PerSpe"
                },
                //5成 6成半 6成4
                {
                    new Regex(
                        $@"{IntegerExtractor.ZeroToNineChsFullHalfRegexChs}\s*成(\s*(半|{IntegerExtractor
                            .ZeroToNineChsFullHalfRegexChs}))?",
                        RegexOptions.IgnoreCase | RegexOptions.Singleline),
                    "PerSpe"
                },
                //七成半 七成五
                {
                    new Regex(
                        $@"({IntegerExtractor.ZeroToNineIntegerRegexChs}|[十拾])\s*成(\s*(半|{IntegerExtractor
                            .ZeroToNineIntegerRegexChs}))?",
                        RegexOptions.IgnoreCase | RegexOptions.Singleline),
                    "PerSpe"
                },
                //2成 ２.５成
                {
                    new Regex(
                        $@"({IntegerExtractor.ZeroToNineChsFullHalfRegexChs}[\.．]{IntegerExtractor
                            .ZeroToNineChsFullHalfRegexChs}|[1１][0０])\s*成",
                        RegexOptions.IgnoreCase | RegexOptions.Singleline),
                    "PerSpe"
                },
                //三成 六点五成
                {
                    new Regex(
                        $@"{IntegerExtractor.ZeroToNineIntegerRegexChs}\s*[点點]\s*{IntegerExtractor
                            .ZeroToNineIntegerRegexChs}\s*成",
                        RegexOptions.IgnoreCase | RegexOptions.Singleline),
                    "PerSpe"
                },
                {
                    new Regex(@"半\s*成|(?<=打)[对對]\s*折|半\s*折",
                        RegexOptions.IgnoreCase | RegexOptions.Singleline),
                    "PerSpe"
                }
            };
            Regexes = regexes.ToImmutableDictionary();
        }
    }
}