using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.Number.Chinese
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
        @"(。|，|、|（|）|“｜”｜[国國]|周|夜|[点點]|[个個]|倍|票|[项項]|[亩畝]|分|元|角|天|加|[减減]|乘|除|是|[對对]|打|公[里裏]|" +
        @"公[顷頃]|公分|平方|方|米|厘|毫|[条條]|船|[车車]|[辆輛]|群|[页頁]|杯|人|[张張]|次|位|份|批|[届屆]|[级級]|[种種]|套|" +
        @"[笔筆]|根|[块塊]|件|座|步|[颗顆]|棵|[节節]|支|只|名|年|月|日|[号號]|朵|克|[吨噸]|磅|[码碼]|英尺|英寸|升|加[仑侖]|" +
        @"立方|[台臺]|套|[罗羅]|令|卷|[头頭]|箱|包|桶|袋|[块塊]|家|行|期|[层層]|度|面|所|架|把|片|[阵陣]|[间間]|等|[叠疊]|碟|" +
        @"下|起|手|季|部|人|小[时時]|[时時]|秒|[样樣]|章|段|星|州|款|代|维|重|[户戸]|楼|路|篇|句|键|本|生|者|字|郎|道|边|场|口|" +
        @"线|世|岸|金|类|番|组|卦|眼|系|声|更|带|色|战|成|轮|食|首|幡|站|股|井|流|开|刻|洲|回|宮|集|练|週|和|环|甲|处|省|里|海|遍|" +
        @"品|体|王|尾|新|隻|版|阶|板|侧|波|身|则|扫|房|彩|木|军|居|晚|岛|课|式|通|相|区|文|端|味|田|心|胎|班|出|连|单|事|丝|副|岁|旁|幕|些|枚|" +
        @"招|卡|幅|言|街|指|辈|室|堆|作|封|厢|声|城|族|圈|脸|目|排|模|夕|网|市|向|极|驱|科|提|核|村|审|刀|册|例|关|粒|局|山|寸|碗|瞬|联|游|脚|" +
        @"宅|线|格|入|趟|貫|界|社|肢|技|滴|问|笑|院|堂|尺|寨|档|举|盘|门|客|餐|艘|毛|丈|剑|曲|任|叶|团|派|嘴|桥|抹|枝|贯|伏|拳|列|机|盒|队|进制|" +
        @"栋|席|斤|词|击|题|型|宗|柱|钱|拍|剧|旬|命|扇|匹|湖|壶|觉|叉|校|泉|具|串|射|证|大批|球|横|竖|尊|轴|观|审|石|束|弹|株|领|委|栏|炮|鼎|町|帆|" +
        @"斗|缕|桌|针|帧|转|落|足|梯|县|投|试|帮|掌|箭|盏|锅|计|大片|学期|截|顶|屋|介|剑|桂|旗|巷|挥|晃|员|翼|池|围|勺|宿|库|棒|冠|树|缸|伙|签|揽|" +
        @"坨|匙|桩|顿|纸|隅|诺|案|刊|厂|杆|袭|仓|床|担|帖|屏|盏|腔|贴|窍|洞|円|坪|泡|园|馆|湾|拨|枪|职|亭|背|維|[護护戸]|" +
        @"樓|鍵|邊|場|線|類|組|聲|帶|戰|輪|開|練|環|處|裏|體|隻|階|側|則|掃|軍|居|島|課|式|區|連|單|絲|歲|廂|聲|臉|網|極|驅|審|" +
        @"冊|關|聯|遊|腳|線|貫|問|檔|舉|盤|門|劍|曲|任|葉|團|派|嘴|橋|抹|枝|貫|伏|拳|列|機|盒|隊|進制|棟|詞|擊|題|錢|壺|覺|證|大批|" +
        @"球|橫|豎|尊|軸|觀|審|彈|領|委|欄|釘|鬥|縷|針|幀|轉|縣|試|幫|盞|鍋|計|學期|截|頂|介|劍|桂|旗|巷|揮|晃|員|圍|勺|宿|庫|棒|冠|樹|缸|" +
        @"夥|簽|攬|樁|頓|紙|隅|諾|廠|桿|襲|倉|擔|盞|貼|竅|洞|坪|泡|員|館|灣|撥|槍|職|\s|$)";

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
                
                {
                    // 123456,  －１２３４５６
                    new Regex(
                        $@"({SignSymbolRegexNum}\s*)?{ZeroToNineChsFullHalfRegexChs}+",
                        RegexOptions.IgnoreCase | RegexOptions.Singleline)
                    , "IntegerNum"
                },
                {
                    //15k,  16 G
                    new Regex($@"{SignSymbolRegexNum}?{ZeroToNineChsFullHalfRegexChs}+\s*(K|k|M|G|T|Ｍ|Ｋ|ｋ|Ｇ|Ｔ)",
                        RegexOptions.Singleline)
                    , "IntegerNum"
                },
                {
                    //1,234,  ２，３３２，１１１
                    new Regex(
                        $@"{SignSymbolRegexNum}?{ZeroToNineChsFullHalfRegexChs}" + @"{1,3}([,，]" +
                        $@"{ZeroToNineChsFullHalfRegexChs}" + @"{3})+",
                        RegexOptions.IgnoreCase | RegexOptions.Singleline),
                    "IntegerNum"
                },
                {
                    //半百  半打
                    new Regex($@"半({RoundNumberIntegerRegexChs}|打)", RegexOptions.Singleline)
                    , "IntegerChs"
                },
                {
                    //一打  五十打
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