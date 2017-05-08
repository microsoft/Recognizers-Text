using Microsoft.Recognizers.Text.Number.Extractors;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.Number.Chinese.Extractors
{
    public class IntegerExtractor : BaseNumberExtractor
    {
        internal sealed override ImmutableDictionary<Regex, string> Regexes { get; }

        protected sealed override string ExtractType { get; } = Constants.SYS_NUM_INTEGER;

        public const string RoundNumberIntegerRegexChs = @"[十百千万亿兆拾佰仟萬億]";

        public const string ZeroToNineIntegerRegexChs = @"[一二三四五六七八九零壹贰貳叁肆伍陆陸柒捌玖〇两兩俩倆仨]";

        public const string ZeroToNineChsFullHalfRegexChs = @"[\d１２３４５６７８９０]";

        public const string SignSymbolRegexChs = @"[负負]";

        public const string SignSymbolRegexNum = @"(?<![-－])[-－]";

        public const string WhiteListRegex =
            @"(。|，|、|[国國]|周|夜|[点點]|[个個]|倍|票|[项項]|[亩畝]|分|元|角|天|加|[减減]|乘|除|是|[對对]|打|公[里裏]|" +
            @"公[顷頃]|公分|平方|方|米|厘|毫|[条條]|船|[车車]|[辆輛]|群|[页頁]|杯|人|[张張]|次|位|份|批|[届屆]|[级級]|[种種]|套|" +
            @"[笔筆]|根|[块塊]|件|座|步|[颗顆]|棵|[节節]|支|只|名|年|月|日|[号號]|朵|克|[吨噸]|磅|[码碼]|英尺|英寸|升|加[仑侖]|" +
            @"立方|[台臺]|套|[罗羅]|令|卷|[头頭]|箱|包|桶|袋|[块塊]|家|行|期|[层層]|度|面|所|架|把|片|[阵陣]|[间間]|等|[叠疊]|碟|\s|$)";

        public static string NotSingleRegexChs
            =>
                $@"(({ZeroToNineIntegerRegexChs}|{ZeroToNineChsFullHalfRegexChs}|[十拾])\s*(\s*[多几幾余]?\s*{RoundNumberIntegerRegexChs})"
                +
                @"{1,2}"
                +
                $@"|[十拾]|{RoundNumberIntegerRegexChs}\s*({ZeroToNineIntegerRegexChs}|{ZeroToNineChsFullHalfRegexChs}|零))\s*((({ZeroToNineIntegerRegexChs}|{ZeroToNineChsFullHalfRegexChs})\s*(\s*[多几幾余]?\s*{RoundNumberIntegerRegexChs})"
                +
                @"{1,2}" + $@"|零)\s*)*{ZeroToNineIntegerRegexChs}?";

        public static string SingleRegexChs
            => $@"(?<!{ZeroToNineIntegerRegexChs}){ZeroToNineIntegerRegexChs}(?={WhiteListRegex})";

        public static string AllIntRegexChs
            =>
                $@"((({ZeroToNineIntegerRegexChs}|{ZeroToNineChsFullHalfRegexChs}|[十拾])\s*(\s*[多几幾余]?\s*{RoundNumberIntegerRegexChs})"
                +
                @"{1,2}"
                +
                $@"|[十拾]|{RoundNumberIntegerRegexChs}\s*({ZeroToNineIntegerRegexChs}|{ZeroToNineChsFullHalfRegexChs}|零))\s*((({ZeroToNineIntegerRegexChs}|{ZeroToNineChsFullHalfRegexChs})\s*(\s*[多几幾余]?\s*{RoundNumberIntegerRegexChs})"
                +
                @"{1,2}" + $@"|零)\s*)*{ZeroToNineIntegerRegexChs}?|{ZeroToNineIntegerRegexChs})";

        public IntegerExtractor(ChineseNumberMode mode = ChineseNumberMode.Default)
        {
            var regexes = new Dictionary<Regex, string>()
            {
                // 123456,  －１２３４５６
                {
                    new Regex(
                        $@"({SignSymbolRegexNum}\s*)?{ZeroToNineChsFullHalfRegexChs}+",
                        RegexOptions.IgnoreCase | RegexOptions.Singleline)
                    , "IntegerNum"
                },
                //15k,  16 G
                {
                    new Regex($@"{SignSymbolRegexNum}?{ZeroToNineChsFullHalfRegexChs}+\s*(K|k|M|G|T|Ｍ|Ｋ|ｋ|Ｇ|Ｔ)",
                        RegexOptions.Singleline)
                    , "IntegerNum"
                },
                //1,234,  ２，３３２，１１１
                {
                    new Regex(
                        $@"{SignSymbolRegexNum}?{ZeroToNineChsFullHalfRegexChs}" + @"{1,3}([,，]" +
                        $@"{ZeroToNineChsFullHalfRegexChs}" + @"{3})+",
                        RegexOptions.IgnoreCase | RegexOptions.Singleline),
                    "IntegerNum"
                },
                //半百  半打
                {
                    new Regex($@"半({RoundNumberIntegerRegexChs}|打)", RegexOptions.Singleline)
                    , "IntegerChs"
                },
                //一打  五十打
                {
                    new Regex($@"{AllIntRegexChs}[双雙对對打](?!{AllIntRegexChs})",
                        RegexOptions.Singleline)
                    , "IntegerChs"
                }
            };
            switch (mode)
            {
                case ChineseNumberMode.Default:
                    regexes.Add(
                        //一百五十五,  负一亿三百二十二, avoid 五十五点五个百分点
                        new Regex(
                            $@"(?<![百佰]\s*分\s*之\s*({AllIntRegexChs}[点點]*|{DoubleExtractor.AllFloatRegexChs})*){SignSymbolRegexChs}?({NotSingleRegexChs}|{SingleRegexChs})(?!({AllIntRegexChs}*([点點]{ZeroToNineIntegerRegexChs}+)*|{DoubleExtractor
                                .AllFloatRegexChs})*\s*[个個]\s*[百佰]\s*分\s*[点點])",
                            RegexOptions.Singleline),
                        "IntegerChs");
                    break;
                case ChineseNumberMode.ExtractAll:
                    regexes.Add(
                        //一百五十五,  负一亿三百二十二, avoid 五十五点五个百分点
                        new Regex(
                            $@"(?<![百佰]\s*分\s*之\s*({AllIntRegexChs}[点點]*|{DoubleExtractor.AllFloatRegexChs})*){SignSymbolRegexChs}?{AllIntRegexChs}(?!({AllIntRegexChs}*([点點]{ZeroToNineIntegerRegexChs}+)*|{DoubleExtractor
                                .AllFloatRegexChs})*\s*[个個]\s*[百佰]\s*分\s*[点點])",
                            RegexOptions.Singleline),
                        "IntegerChs");
                    break;
            }
            Regexes = regexes.ToImmutableDictionary();
        }
    }
}