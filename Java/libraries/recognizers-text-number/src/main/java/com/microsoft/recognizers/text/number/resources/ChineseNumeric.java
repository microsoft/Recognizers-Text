// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
// ------------------------------------------------------------------------------

package com.microsoft.recognizers.text.number.resources;

import java.util.Arrays;
import java.util.List;
import java.util.Map;

import com.google.common.collect.ImmutableMap;

public class ChineseNumeric {

    public static final String LangMarker = "Chi";

    public static final Boolean CompoundNumberLanguage = true;

    public static final Boolean MultiDecimalSeparatorCulture = false;

    public static final Character DecimalSeparatorChar = '.';

    public static final String FractionMarkerToken = "";

    public static final Character NonDecimalSeparatorChar = ' ';

    public static final String HalfADozenText = "";

    public static final String WordSeparatorToken = "";

    public static final Character ZeroChar = '零';

    public static final Character PairChar = '对';

    public static final ImmutableMap<String, Long> RoundNumberMap = ImmutableMap.<String, Long>builder()
        .put("k", 1000L)
        .put("m", 1000000L)
        .put("g", 1000000000L)
        .put("t", 1000000000000L)
        .build();

    public static final ImmutableMap<Character, Long> RoundNumberMapChar = ImmutableMap.<Character, Long>builder()
        .put('十', 10L)
        .put('百', 100L)
        .put('千', 1000L)
        .put('万', 10000L)
        .put('亿', 100000000L)
        .put('兆', 1000000000000L)
        .put('拾', 10L)
        .put('佰', 100L)
        .put('仟', 1000L)
        .put('萬', 10000L)
        .put('億', 100000000L)
        .build();

    public static final ImmutableMap<Character, Double> ZeroToNineMap = ImmutableMap.<Character, Double>builder()
        .put('零', 0D)
        .put('一', 1D)
        .put('二', 2D)
        .put('三', 3D)
        .put('四', 4D)
        .put('五', 5D)
        .put('六', 6D)
        .put('七', 7D)
        .put('八', 8D)
        .put('九', 9D)
        .put('〇', 0D)
        .put('壹', 1D)
        .put('贰', 2D)
        .put('貳', 2D)
        .put('叁', 3D)
        .put('肆', 4D)
        .put('伍', 5D)
        .put('陆', 6D)
        .put('陸', 6D)
        .put('柒', 7D)
        .put('捌', 8D)
        .put('玖', 9D)
        .put('０', 0D)
        .put('１', 1D)
        .put('２', 2D)
        .put('３', 3D)
        .put('４', 4D)
        .put('５', 5D)
        .put('６', 6D)
        .put('７', 7D)
        .put('８', 8D)
        .put('９', 9D)
        .put('0', 0D)
        .put('1', 1D)
        .put('2', 2D)
        .put('3', 3D)
        .put('4', 4D)
        .put('5', 5D)
        .put('6', 6D)
        .put('7', 7D)
        .put('8', 8D)
        .put('9', 9D)
        .put('半', 0.5D)
        .put('两', 2D)
        .put('兩', 2D)
        .put('俩', 2D)
        .put('倆', 2D)
        .put('仨', 3D)
        .build();

    public static final ImmutableMap<Character, Character> FullToHalfMap = ImmutableMap.<Character, Character>builder()
        .put('０', '0')
        .put('１', '1')
        .put('２', '2')
        .put('３', '3')
        .put('４', '4')
        .put('５', '5')
        .put('６', '6')
        .put('７', '7')
        .put('８', '8')
        .put('９', '9')
        .put('／', '/')
        .put('－', '-')
        .put('，', '\'')
        .put('Ｇ', 'G')
        .put('Ｍ', 'M')
        .put('Ｔ', 'T')
        .put('Ｋ', 'K')
        .put('ｋ', 'k')
        .put('．', '.')
        .build();

    public static final ImmutableMap<Character, Character> TratoSimMap = ImmutableMap.<Character, Character>builder()
        .put('佰', '百')
        .put('點', '点')
        .put('個', '个')
        .put('幾', '几')
        .put('對', '对')
        .put('雙', '双')
        .build();

    public static final ImmutableMap<String, String> UnitMap = ImmutableMap.<String, String>builder()
        .put("萬萬", "億")
        .put("億萬", "兆")
        .put("萬億", "兆")
        .put("万万", "亿")
        .put("万亿", "兆")
        .put("亿万", "兆")
        .put(" ", "")
        .put("多", "")
        .put("余", "")
        .put("几", "")
        .build();

    public static final List<Character> RoundDirectList = Arrays.asList('亿', '兆', '億');

    public static final List<Character> TenChars = Arrays.asList('十', '拾');

    public static final String DigitalNumberRegex = "((?<=(\\d|\\b)){BaseNumbers.MultiplierLookupRegex}(?=\\b))"
            .replace("{BaseNumbers.MultiplierLookupRegex}", BaseNumbers.MultiplierLookupRegex);

    public static final String ZeroToNineFullHalfRegex = "[\\d]";

    public static final String DigitNumRegex = "{ZeroToNineFullHalfRegex}+"
            .replace("{ZeroToNineFullHalfRegex}", ZeroToNineFullHalfRegex);

    public static final String DozenRegex = ".*打$";

    public static final String PercentageRegex = "(?<=(((?<![十百千拾佰仟])[十百千拾佰仟])|([万亿兆萬億]))\\s*分\\s*之).+|.+(?=个\\s*(((?<![十百千拾佰仟])[十百千拾佰仟])|([万亿兆萬億]))\\s*分\\s*点)|.*(?=[％%])";

    public static final String DoubleAndRoundRegex = "{ZeroToNineFullHalfRegex}+(\\.{ZeroToNineFullHalfRegex}+)?\\s*[多几余]?[万亿萬億]{1,2}"
            .replace("{ZeroToNineFullHalfRegex}", ZeroToNineFullHalfRegex);

    public static final String FracSplitRegex = "又|分\\s*之|分\\s*点";

    public static final String ZeroToNineIntegerRegex = "[一二三四五六七八九零壹贰貳叁肆伍陆陸柒捌玖〇两兩俩倆仨]";

    public static final String DigitNumPlusRegex = "{ZeroToNineIntegerRegex}|{ZeroToNineFullHalfRegex}"
            .replace("{ZeroToNineIntegerRegex}", ZeroToNineIntegerRegex)
            .replace("{ZeroToNineFullHalfRegex}", ZeroToNineFullHalfRegex);

    public static final String HalfUnitRegex = "半";

    public static final String NegativeNumberTermsRegex = "[负負]";

    public static final String NegativeNumberTermsRegexNum = "((?<!(\\d+(\\s*{BaseNumbers.NumberMultiplierRegex})?\\s*)|[-－])[-－])"
            .replace("{BaseNumbers.NumberMultiplierRegex}", BaseNumbers.NumberMultiplierRegex);

    public static final String NegativeNumberSignRegex = "^{NegativeNumberTermsRegex}.*|^{NegativeNumberTermsRegexNum}.*"
            .replace("{NegativeNumberTermsRegex}", NegativeNumberTermsRegex)
            .replace("{NegativeNumberTermsRegexNum}", NegativeNumberTermsRegexNum);

    public static final String SpeGetNumberRegex = "{ZeroToNineFullHalfRegex}|{ZeroToNineIntegerRegex}|[十拾半对對]"
            .replace("{ZeroToNineFullHalfRegex}", ZeroToNineFullHalfRegex)
            .replace("{ZeroToNineIntegerRegex}", ZeroToNineIntegerRegex);

    public static final String PairRegex = ".*[双对雙對]$";

    public static final String KiloUnitNames = "[米克位焦卡赫瓦]|比特|字节|大卡";

    public static final String MegaUnitNames = "[位赫瓦]|比特|字节";

    public static final String RoundNumberIntegerRegex = "(((?<![十百拾佰千仟])[十百拾佰])([万亿萬億]{0,2})|([万亿萬億]{1,2})|(?<![十百拾佰千仟])[千仟]([万亿萬億]{1,2})|(?<![十百拾佰千仟])[千仟](?!{KiloUnitNames})|(兆{1,2})(?!{MegaUnitNames}))"
            .replace("{KiloUnitNames}", KiloUnitNames)
            .replace("{MegaUnitNames}", MegaUnitNames);

    public static final String PercentageNumRegex = "(([十百千拾佰仟])|([万亿兆萬億])){1,3}\\s*分(\\s*之|\\s*点)";

    public static final String AllowListRegex = "([。，、（）“”]|[这那不也还而却更但這還卻]?是|[到以至]|[国國]|周|夜|[点點]|[个個]|倍|票|[项項]|[亩畝]|分|元|角|天|[双雙]|[对對]|加|[减減]|乘|除|[對对]|打|公[里裏]|公[顷頃]|公分|平方|方|米|厘|毫|[条條]|船|[车車]|[辆輛]|群|[页頁]|杯|人|[张張]|次|位|份|批|[届屆]|[级級]|[种種]|套|[笔筆]|根|[块塊]|件|座|步|[颗顆]|棵|[节節]|支|只|名|年|月|日|[号號]|朵|克|[吨噸]|磅|[码碼]|英尺|英寸|升|加[仑侖]|立方|[台臺]|套|[罗羅]|令|卷|[头頭]|箱|包|桶|袋|[块塊]|家|行|期|[层層]|度|面|所|架|把|片|[阵陣]|[间間]|等|[叠疊]|碟|下|起|手|季|部|人|小[时時]|[时時]|秒|[样樣]|章|段|星|州|款|代|维|重|[户戸]|楼|路|篇|句|键|本|生|者|字|郎|道|边|场|口|线|世|岸|金|类|番|组|卦|眼|系|声|更|带|色|战|成|轮|食|首|幡|站|股|井|流|开|刻|洲|回|宮|集|练|週|和|环|甲|处|省|里|海|遍|品|体|王|尾|新|隻|版|阶|板|侧|波|身|则|扫|房|彩|木|军|居|晚|岛|课|式|通|相|区|文|端|味|田|心|胎|班|出|连|单|事|丝|副|岁|旁|幕|些|枚|招|卡|幅|言|街|指|辈|室|堆|作|封|厢|声|城|族|圈|脸|目|排|模|夕|网|市|向|极|驱|科|提|核|村|审|刀|册|例|关|粒|局|山|寸|碗|瞬|联|游|脚|宅|线|格|入|趟|貫|界|社|肢|技|滴|问|笑|院|堂|尺|寨|档|举|盘|门|客|餐|艘|毛|丈|剑|曲|任|叶|团|派|嘴|桥|抹|枝|贯|伏|拳|列|机|盒|队|进制|栋|席|斤|词|击|题|型|宗|柱|钱|拍|剧|旬|命|扇|匹|湖|壶|觉|叉|校|泉|具|串|射|证|大批|球|横|竖|尊|轴|观|审|石|束|弹|株|领|委|栏|炮|鼎|町|帆|斗|缕|桌|针|帧|转|落|足|梯|县|投|试|帮|掌|箭|盏|锅|计|大片|学期|截|顶|屋|介|剑|桂|旗|巷|挥|晃|员|翼|池|围|勺|宿|库|棒|冠|树|缸|伙|签|揽|坨|匙|桩|顿|纸|隅|诺|案|刊|厂|杆|袭|仓|床|担|帖|屏|盏|腔|贴|窍|洞|円|坪|泡|园|馆|湾|拨|枪|职|亭|背|維|[護护戸]|樓|鍵|邊|場|線|類|組|聲|帶|戰|輪|開|練|環|處|裏|體|隻|階|側|則|掃|軍|居|島|課|式|區|連|單|絲|歲|廂|聲|臉|網|極|驅|審|冊|關|聯|遊|腳|線|貫|問|檔|舉|盤|門|劍|曲|任|葉|團|派|嘴|橋|抹|枝|貫|伏|拳|列|機|盒|隊|進制|棟|詞|擊|題|錢|壺|覺|證|大批|球|橫|豎|尊|軸|觀|審|彈|領|委|欄|釘|鬥|縷|針|幀|轉|縣|試|幫|盞|鍋|計|學期|截|頂|介|劍|桂|旗|巷|揮|晃|員|圍|勺|宿|庫|棒|冠|樹|缸|夥|簽|攬|樁|頓|紙|隅|諾|廠|桿|襲|倉|擔|盞|貼|竅|洞|坪|泡|員|館|灣|撥|槍|職|的|\\s|$)";

    public static final String ContinuouslyNumberRegex = "({DigitNumPlusRegex}|{RoundNumberIntegerRegex})"
            .replace("{DigitNumPlusRegex}", DigitNumPlusRegex)
            .replace("{RoundNumberIntegerRegex}", RoundNumberIntegerRegex);

    public static final String SingleLiangRegex = "(?<!{ContinuouslyNumberRegex})两(?!{ContinuouslyNumberRegex})"
            .replace("{ContinuouslyNumberRegex}", ContinuouslyNumberRegex);

    public static final String NotSingleRegex = "((({DigitNumPlusRegex}|[十拾])\\s*(\\s*[多几幾余]?\\s*{RoundNumberIntegerRegex}){1,2}|[十拾]|{RoundNumberIntegerRegex}\\s*{ZeroToNineIntegerRegex})\\s*|{ZeroToNineFullHalfRegex})(((({DigitNumPlusRegex})\\s*(\\s*[多几幾余]?\\s*{RoundNumberIntegerRegex}){1,2}|零)\\s*)|({ZeroToNineFullHalfRegex}))*({DigitNumPlusRegex})?(?<!两)"
            .replace("{DigitNumPlusRegex}", DigitNumPlusRegex)
            .replace("{ZeroToNineIntegerRegex}", ZeroToNineIntegerRegex)
            .replace("{ZeroToNineFullHalfRegex}", ZeroToNineFullHalfRegex)
            .replace("{RoundNumberIntegerRegex}", RoundNumberIntegerRegex);

    public static final String SingleRegex = "((?<!{ZeroToNineIntegerRegex}){ZeroToNineIntegerRegex}(?<!两)|{SingleLiangRegex})(?={AllowListRegex})"
            .replace("{ZeroToNineIntegerRegex}", ZeroToNineIntegerRegex)
            .replace("{AllowListRegex}", AllowListRegex)
            .replace("{SingleLiangRegex}", SingleLiangRegex);

    public static final String AllIntRegex = "((((({DigitNumPlusRegex}|[十拾])\\s*(\\s*[多几幾余]?\\s*{RoundNumberIntegerRegex}){1,2}|[十拾])\\s*)(((({DigitNumPlusRegex})\\s*(\\s*[多几幾余]?\\s*{RoundNumberIntegerRegex}){1,2}|零)\\s*)|({ZeroToNineFullHalfRegex}))*({DigitNumPlusRegex})?|({DigitNumPlusRegex}))(?<!两)|{SingleLiangRegex})"
            .replace("{SingleLiangRegex}", SingleLiangRegex)
            .replace("{DigitNumPlusRegex}", DigitNumPlusRegex)
            .replace("{ZeroToNineIntegerRegex}", ZeroToNineIntegerRegex)
            .replace("{ZeroToNineFullHalfRegex}", ZeroToNineFullHalfRegex)
            .replace("{RoundNumberIntegerRegex}", RoundNumberIntegerRegex);

    public static final String PlaceHolderPureNumber = "\\b";

    public static final String PlaceHolderDefault = "\\D|\\b";

    public static final String NumbersSpecialsChars = "(({NegativeNumberTermsRegexNum}|{NegativeNumberTermsRegex})\\s*)?{ZeroToNineFullHalfRegex}+"
            .replace("{NegativeNumberTermsRegexNum}", NegativeNumberTermsRegexNum)
            .replace("{ZeroToNineFullHalfRegex}", ZeroToNineFullHalfRegex)
            .replace("{NegativeNumberTermsRegex}", NegativeNumberTermsRegex);

    public static final String NumbersSpecialsCharsWithSuffix = "{NegativeNumberTermsRegexNum}?{ZeroToNineFullHalfRegex}+\\s*{BaseNumbers.NumberMultiplierRegex}"
            .replace("{NegativeNumberTermsRegexNum}", NegativeNumberTermsRegexNum)
            .replace("{ZeroToNineFullHalfRegex}", ZeroToNineFullHalfRegex)
            .replace("{BaseNumbers.NumberMultiplierRegex}", BaseNumbers.NumberMultiplierRegex);

    public static final String DottedNumbersSpecialsChar = "{NegativeNumberTermsRegexNum}?{ZeroToNineFullHalfRegex}{1,3}([,，]{ZeroToNineFullHalfRegex}{3})+"
            .replace("{NegativeNumberTermsRegexNum}", NegativeNumberTermsRegexNum)
            .replace("{ZeroToNineFullHalfRegex}", ZeroToNineFullHalfRegex);

    public static final String NumbersWithHalfDozen = "半({RoundNumberIntegerRegex}|打)"
            .replace("{RoundNumberIntegerRegex}", RoundNumberIntegerRegex);

    public static final String NumbersWithDozen = "{AllIntRegex}[打](?!{AllIntRegex})"
            .replace("{AllIntRegex}", AllIntRegex);

    public static final String PointRegexStr = "[点點\\.．]";

    public static final String AllFloatRegex = "{NegativeNumberTermsRegex}?{AllIntRegex}\\s*{PointRegexStr}\\s*[一二三四五六七八九零壹贰貳叁肆伍陆陸柒捌玖〇](\\s*{ZeroToNineIntegerRegex})*"
            .replace("{NegativeNumberTermsRegex}", NegativeNumberTermsRegex)
            .replace("{AllIntRegex}", AllIntRegex)
            .replace("{PointRegexStr}", PointRegexStr)
            .replace("{ZeroToNineIntegerRegex}", ZeroToNineIntegerRegex);

    public static final String NumbersWithAllowListRegex = "(?<![百佰]\\s*分\\s*之\\s*({AllIntRegex}[点點]*|{AllFloatRegex})*){NegativeNumberTermsRegex}?({NotSingleRegex}|{SingleRegex})(?!({AllIntRegex}*([点點]{ZeroToNineIntegerRegex}+)*|{AllFloatRegex})*\\s*[个個]\\s*[百佰]\\s*分\\s*[点點])"
            .replace("{AllIntRegex}", AllIntRegex)
            .replace("{AllFloatRegex}", AllFloatRegex)
            .replace("{NegativeNumberTermsRegex}", NegativeNumberTermsRegex)
            .replace("{NotSingleRegex}", NotSingleRegex)
            .replace("{SingleRegex}", SingleRegex)
            .replace("{ZeroToNineIntegerRegex}", ZeroToNineIntegerRegex);

    public static final String NumbersAggressiveRegex = "(?<![百佰]\\s*分\\s*之\\s*({AllIntRegex}[点點]*|{AllFloatRegex})*){NegativeNumberTermsRegex}?{AllIntRegex}(?!({AllIntRegex}*([点點]{ZeroToNineIntegerRegex}+)*|{AllFloatRegex})*\\s*[个個]\\s*[百佰]\\s*分\\s*[点點])"
            .replace("{AllIntRegex}", AllIntRegex)
            .replace("{AllFloatRegex}", AllFloatRegex)
            .replace("{NegativeNumberTermsRegex}", NegativeNumberTermsRegex)
            .replace("{ZeroToNineIntegerRegex}", ZeroToNineIntegerRegex);

    public static final String PointRegex = "{PointRegexStr}"
            .replace("{PointRegexStr}", PointRegexStr);

    public static final String DoubleSpecialsChars = "(?<!({ZeroToNineFullHalfRegex}+[\\.．]{ZeroToNineFullHalfRegex}*))({NegativeNumberTermsRegexNum}\\s*)?{ZeroToNineFullHalfRegex}+[\\.．]{ZeroToNineFullHalfRegex}+(?!{ZeroToNineFullHalfRegex}*[\\.．]{ZeroToNineFullHalfRegex}+)"
            .replace("{ZeroToNineFullHalfRegex}", ZeroToNineFullHalfRegex)
            .replace("{NegativeNumberTermsRegexNum}", NegativeNumberTermsRegexNum);

    public static final String DoubleSpecialsCharsWithNegatives = "(?<!({ZeroToNineFullHalfRegex}+|\\.\\.|．．))({NegativeNumberTermsRegexNum}\\s*)?[\\.．]{ZeroToNineFullHalfRegex}+(?!{ZeroToNineFullHalfRegex}*([\\.．]{ZeroToNineFullHalfRegex}+))"
            .replace("{ZeroToNineFullHalfRegex}", ZeroToNineFullHalfRegex)
            .replace("{NegativeNumberTermsRegexNum}", NegativeNumberTermsRegexNum);

    public static final String SimpleDoubleSpecialsChars = "({NegativeNumberTermsRegexNum}\\s*)?{ZeroToNineFullHalfRegex}{1,3}([,，]{ZeroToNineFullHalfRegex}{3})+[\\.．]{ZeroToNineFullHalfRegex}+"
            .replace("{NegativeNumberTermsRegexNum}", NegativeNumberTermsRegexNum)
            .replace("{ZeroToNineFullHalfRegex}", ZeroToNineFullHalfRegex);

    public static final String DoubleWithMultiplierRegex = "({NegativeNumberTermsRegexNum}\\s*)?{ZeroToNineFullHalfRegex}+[\\.．]{ZeroToNineFullHalfRegex}+\\s*{BaseNumbers.NumberMultiplierRegex}"
            .replace("{NegativeNumberTermsRegexNum}", NegativeNumberTermsRegexNum)
            .replace("{ZeroToNineFullHalfRegex}", ZeroToNineFullHalfRegex)
            .replace("{BaseNumbers.NumberMultiplierRegex}", BaseNumbers.NumberMultiplierRegex);

    public static final String DoubleWithThousandsRegex = "{NegativeNumberTermsRegex}?(({ZeroToNineFullHalfRegex}+)|({ZeroToNineFullHalfRegex}{1,3}(,{ZeroToNineFullHalfRegex}{3})+))([\\.．]{ZeroToNineFullHalfRegex}+)?\\s*[多几幾余]?[万亿萬億]{1,2}"
            .replace("{NegativeNumberTermsRegex}", NegativeNumberTermsRegex)
            .replace("{ZeroToNineFullHalfRegex}", ZeroToNineFullHalfRegex);

    public static final String DoubleAllFloatRegex = "(?<![百佰]\\s*分\\s*之\\s*(({AllIntRegex}[点點]*)|{AllFloatRegex})*){AllFloatRegex}(?!{ZeroToNineIntegerRegex}*\\s*[个個]\\s*[百佰]\\s*分\\s*[点點])"
            .replace("{AllIntRegex}", AllIntRegex)
            .replace("{AllFloatRegex}", AllFloatRegex)
            .replace("{ZeroToNineIntegerRegex}", ZeroToNineIntegerRegex);

    public static final String DoubleExponentialNotationRegex = "(?<!{ZeroToNineFullHalfRegex}+[\\.．])({NegativeNumberTermsRegexNum}\\s*)?{ZeroToNineFullHalfRegex}+([\\.．]{ZeroToNineFullHalfRegex}+)?e(([-－+＋]*[1-9]{ZeroToNineFullHalfRegex}*)|0(?!{ZeroToNineFullHalfRegex}+))"
            .replace("{ZeroToNineFullHalfRegex}", ZeroToNineFullHalfRegex)
            .replace("{NegativeNumberTermsRegexNum}", NegativeNumberTermsRegexNum);

    public static final String DoubleScientificNotationRegex = "(?<!{ZeroToNineFullHalfRegex}+[\\.．])({NegativeNumberTermsRegexNum}\\s*)?({ZeroToNineFullHalfRegex}+([\\.．]{ZeroToNineFullHalfRegex}+)?)\\^([-－+＋]*[1-9]{ZeroToNineFullHalfRegex}*)"
            .replace("{ZeroToNineFullHalfRegex}", ZeroToNineFullHalfRegex)
            .replace("{NegativeNumberTermsRegexNum}", NegativeNumberTermsRegexNum);

    public static final String OrdinalRegex = "第{AllIntRegex}"
            .replace("{AllIntRegex}", AllIntRegex);

    public static final String OrdinalNumbersRegex = "第{ZeroToNineFullHalfRegex}+"
            .replace("{ZeroToNineFullHalfRegex}", ZeroToNineFullHalfRegex);

    public static final String AllFractionNumber = "{NegativeNumberTermsRegex}?(({ZeroToNineFullHalfRegex}+|{AllIntRegex})\\s*又\\s*)?{NegativeNumberTermsRegex}?({ZeroToNineFullHalfRegex}+|{AllIntRegex})\\s*分\\s*之\\s*{NegativeNumberTermsRegex}?({ZeroToNineFullHalfRegex}+|{AllIntRegex})({PointRegexStr}{AllIntRegex}*)?"
            .replace("{NegativeNumberTermsRegex}", NegativeNumberTermsRegex)
            .replace("{ZeroToNineFullHalfRegex}", ZeroToNineFullHalfRegex)
            .replace("{AllIntRegex}", AllIntRegex)
            .replace("{PointRegexStr}", PointRegexStr);

    public static final String FractionNotationSpecialsCharsRegex = "({NegativeNumberTermsRegexNum}\\s*)?{ZeroToNineFullHalfRegex}+\\s+{ZeroToNineFullHalfRegex}+[/／]{ZeroToNineFullHalfRegex}+"
            .replace("{NegativeNumberTermsRegexNum}", NegativeNumberTermsRegexNum)
            .replace("{ZeroToNineFullHalfRegex}", ZeroToNineFullHalfRegex);

    public static final String FractionNotationRegex = "({NegativeNumberTermsRegexNum}\\s*)?{ZeroToNineFullHalfRegex}+[/／]{ZeroToNineFullHalfRegex}+"
            .replace("{NegativeNumberTermsRegexNum}", NegativeNumberTermsRegexNum)
            .replace("{ZeroToNineFullHalfRegex}", ZeroToNineFullHalfRegex);

    public static final String PercentagePointRegex = "(?<!{AllIntRegex})({AllFloatRegex}|{AllIntRegex})\\s*[个個]\\s*{RoundNumberIntegerRegex}\\s*分\\s*[点點]"
            .replace("{AllIntRegex}", AllIntRegex)
            .replace("{AllFloatRegex}", AllFloatRegex)
            .replace("{RoundNumberIntegerRegex}", RoundNumberIntegerRegex);

    public static final String SimplePercentageRegex = "(?<!{ZeroToNineIntegerRegex}){RoundNumberIntegerRegex}{1,3}\\s*分\\s*之\\s*({AllFloatRegex}|{AllIntRegex}|{RoundNumberIntegerRegex})(?!{AllIntRegex})"
            .replace("{ZeroToNineIntegerRegex}", ZeroToNineIntegerRegex)
            .replace("{AllFloatRegex}", AllFloatRegex)
            .replace("{AllIntRegex}", AllIntRegex)
            .replace("{RoundNumberIntegerRegex}", RoundNumberIntegerRegex);

    public static final String NumbersPercentagePointRegex = "(?<!{ZeroToNineIntegerRegex}){RoundNumberIntegerRegex}{1,3}\\s*分\\s*之\\s*{ZeroToNineFullHalfRegex}+[\\.．]{ZeroToNineFullHalfRegex}+(?!([\\.．]{ZeroToNineFullHalfRegex}+))"
            .replace("{ZeroToNineIntegerRegex}", ZeroToNineIntegerRegex)
            .replace("{ZeroToNineFullHalfRegex}", ZeroToNineFullHalfRegex)
            .replace("{RoundNumberIntegerRegex}", RoundNumberIntegerRegex);

    public static final String NumbersPercentageWithSeparatorRegex = "(?<!{ZeroToNineIntegerRegex}){RoundNumberIntegerRegex}{1,3}\\s*分\\s*之\\s*{ZeroToNineFullHalfRegex}{1,3}([,，]{ZeroToNineFullHalfRegex}{3})+[\\.．]{ZeroToNineFullHalfRegex}+"
            .replace("{ZeroToNineIntegerRegex}", ZeroToNineIntegerRegex)
            .replace("{ZeroToNineFullHalfRegex}", ZeroToNineFullHalfRegex)
            .replace("{RoundNumberIntegerRegex}", RoundNumberIntegerRegex);

    public static final String NumbersPercentageWithMultiplierRegex = "(?<!{ZeroToNineIntegerRegex}){RoundNumberIntegerRegex}{1,3}\\s*分\\s*之\\s*{ZeroToNineFullHalfRegex}+[\\.．]{ZeroToNineFullHalfRegex}+\\s*{BaseNumbers.NumberMultiplierRegex}"
            .replace("{ZeroToNineIntegerRegex}", ZeroToNineIntegerRegex)
            .replace("{ZeroToNineFullHalfRegex}", ZeroToNineFullHalfRegex)
            .replace("{BaseNumbers.NumberMultiplierRegex}", BaseNumbers.NumberMultiplierRegex)
            .replace("{RoundNumberIntegerRegex}", RoundNumberIntegerRegex);

    public static final String FractionPercentagePointRegex = "(?<!({ZeroToNineFullHalfRegex}+[\\.．])){ZeroToNineFullHalfRegex}+[\\.．]{ZeroToNineFullHalfRegex}+(?!([\\.．]{ZeroToNineFullHalfRegex}+))\\s*[个個]\\s*[百佰]\\s*分\\s*[点點]"
            .replace("{ZeroToNineFullHalfRegex}", ZeroToNineFullHalfRegex);

    public static final String FractionPercentageWithSeparatorRegex = "{ZeroToNineFullHalfRegex}{1,3}([,，]{ZeroToNineFullHalfRegex}{3})+[\\.．]{ZeroToNineFullHalfRegex}+\\s*[个個]\\s*{RoundNumberIntegerRegex}{1,3}\\s*分\\s*[点點]"
            .replace("{ZeroToNineFullHalfRegex}", ZeroToNineFullHalfRegex)
            .replace("{RoundNumberIntegerRegex}", RoundNumberIntegerRegex);

    public static final String FractionPercentageWithMultiplierRegex = "{ZeroToNineFullHalfRegex}+[\\.．]{ZeroToNineFullHalfRegex}+\\s*{BaseNumbers.NumberMultiplierRegex}\\s*[个個]\\s*{RoundNumberIntegerRegex}{1,3}\\s*分\\s*[点點]"
            .replace("{ZeroToNineFullHalfRegex}", ZeroToNineFullHalfRegex)
            .replace("{BaseNumbers.NumberMultiplierRegex}", BaseNumbers.NumberMultiplierRegex)
            .replace("{RoundNumberIntegerRegex}", RoundNumberIntegerRegex);

    public static final String SimpleNumbersPercentageRegex = "(?<!{ZeroToNineIntegerRegex}){RoundNumberIntegerRegex}{1,3}\\s*分\\s*之\\s*{ZeroToNineFullHalfRegex}+(?!([\\.．]{ZeroToNineFullHalfRegex}+))"
            .replace("{ZeroToNineIntegerRegex}", ZeroToNineIntegerRegex)
            .replace("{ZeroToNineFullHalfRegex}", ZeroToNineFullHalfRegex)
            .replace("{RoundNumberIntegerRegex}", RoundNumberIntegerRegex);

    public static final String SimpleNumbersPercentageWithMultiplierRegex = "(?<!{ZeroToNineIntegerRegex}){RoundNumberIntegerRegex}{1,3}\\s*分\\s*之\\s*{ZeroToNineFullHalfRegex}+\\s*{BaseNumbers.NumberMultiplierRegex}"
            .replace("{ZeroToNineIntegerRegex}", ZeroToNineIntegerRegex)
            .replace("{ZeroToNineFullHalfRegex}", ZeroToNineFullHalfRegex)
            .replace("{BaseNumbers.NumberMultiplierRegex}", BaseNumbers.NumberMultiplierRegex)
            .replace("{RoundNumberIntegerRegex}", RoundNumberIntegerRegex);

    public static final String SimpleNumbersPercentagePointRegex = "(?!{ZeroToNineIntegerRegex}){RoundNumberIntegerRegex}{1,3}\\s*分\\s*之\\s*{ZeroToNineFullHalfRegex}{1,3}([,，]{ZeroToNineFullHalfRegex}{3})+"
            .replace("{ZeroToNineIntegerRegex}", ZeroToNineIntegerRegex)
            .replace("{ZeroToNineFullHalfRegex}", ZeroToNineFullHalfRegex)
            .replace("{RoundNumberIntegerRegex}", RoundNumberIntegerRegex);

    public static final String IntegerPercentageRegex = "{ZeroToNineFullHalfRegex}+\\s*[个個]\\s*{RoundNumberIntegerRegex}{1,3}\\s*分\\s*[点點]"
            .replace("{ZeroToNineFullHalfRegex}", ZeroToNineFullHalfRegex)
            .replace("{RoundNumberIntegerRegex}", RoundNumberIntegerRegex);

    public static final String IntegerPercentageWithMultiplierRegex = "{ZeroToNineFullHalfRegex}+\\s*{BaseNumbers.NumberMultiplierRegex}\\s*[个個]\\s*{RoundNumberIntegerRegex}{1,3}\\s*分\\s*[点點]"
            .replace("{ZeroToNineFullHalfRegex}", ZeroToNineFullHalfRegex)
            .replace("{BaseNumbers.NumberMultiplierRegex}", BaseNumbers.NumberMultiplierRegex)
            .replace("{RoundNumberIntegerRegex}", RoundNumberIntegerRegex);

    public static final String NumbersFractionPercentageRegex = "{ZeroToNineFullHalfRegex}{1,3}([,，]{ZeroToNineFullHalfRegex}{3})+\\s*[个個]\\s*{RoundNumberIntegerRegex}{1,3}\\s*分\\s*[点點]"
            .replace("{ZeroToNineFullHalfRegex}", ZeroToNineFullHalfRegex)
            .replace("{RoundNumberIntegerRegex}", RoundNumberIntegerRegex);

    public static final String SimpleIntegerPercentageRegex = "(?<!%|\\d){NegativeNumberTermsRegexNum}?{ZeroToNineFullHalfRegex}+([\\.．]{ZeroToNineFullHalfRegex}+)?(\\s*)[％%](?!\\d)"
            .replace("{NegativeNumberTermsRegexNum}", NegativeNumberTermsRegexNum)
            .replace("{ZeroToNineFullHalfRegex}", ZeroToNineFullHalfRegex);

    public static final String NumbersFoldsPercentageRegex = "{ZeroToNineFullHalfRegex}(([\\.．]?|\\s*){ZeroToNineFullHalfRegex})?\\s*折"
            .replace("{ZeroToNineFullHalfRegex}", ZeroToNineFullHalfRegex);

    public static final String FoldsPercentageRegex = "{ZeroToNineIntegerRegex}(\\s*[点點]?\\s*{ZeroToNineIntegerRegex})?\\s*折"
            .replace("{ZeroToNineIntegerRegex}", ZeroToNineIntegerRegex);

    public static final String SimpleFoldsPercentageRegex = "{ZeroToNineFullHalfRegex}\\s*成(\\s*(半|{ZeroToNineFullHalfRegex}))?"
            .replace("{ZeroToNineFullHalfRegex}", ZeroToNineFullHalfRegex);

    public static final String SpecialsPercentageRegex = "({ZeroToNineIntegerRegex}|[十拾])\\s*成(\\s*(半|{ZeroToNineIntegerRegex}))?"
            .replace("{ZeroToNineIntegerRegex}", ZeroToNineIntegerRegex);

    public static final String NumbersSpecialsPercentageRegex = "({ZeroToNineFullHalfRegex}[\\.．]{ZeroToNineFullHalfRegex}|10)\\s*成"
            .replace("{ZeroToNineFullHalfRegex}", ZeroToNineFullHalfRegex);

    public static final String SimpleSpecialsPercentageRegex = "{ZeroToNineIntegerRegex}\\s*[点點]\\s*{ZeroToNineIntegerRegex}\\s*成"
            .replace("{ZeroToNineIntegerRegex}", ZeroToNineIntegerRegex);

    public static final String SpecialsFoldsPercentageRegex = "半\\s*成|(?<=打)[对對]\\s*折|半\\s*折";

    public static final String SpeicalCharBeforeNumber = "(有|是|为)";

    public static final String TillRegex = "(到|至|--|-|—|——|~|–)";

    public static final String MoreRegex = "((大于|多于|高于|超过|大於|多於|高於|超過|超过)了?|过|>)";

    public static final String LessRegex = "(小于|少于|低于|小於|少於|低於|不到|不足|<)";

    public static final String EqualRegex = "(等于|等於|=)";

    public static final String MoreOrEqual = "(({MoreRegex}\\s*(或|或者)?\\s*{EqualRegex})|(至少|最少){SpeicalCharBeforeNumber}?|不{LessRegex}|≥)"
            .replace("{MoreRegex}", MoreRegex)
            .replace("{EqualRegex}", EqualRegex)
            .replace("{LessRegex}", LessRegex)
            .replace("{SpeicalCharBeforeNumber}", SpeicalCharBeforeNumber);

    public static final String MoreOrEqualSuffix = "(或|或者)\\s*(次?以上|之上|更[大多高])";

    public static final String LessOrEqual = "(({LessRegex}\\s*(或|或者)?\\s*{EqualRegex})|(至多|最多){SpeicalCharBeforeNumber}?|不{MoreRegex}|≤)"
            .replace("{LessRegex}", LessRegex)
            .replace("{EqualRegex}", EqualRegex)
            .replace("{MoreRegex}", MoreRegex)
            .replace("{SpeicalCharBeforeNumber}", SpeicalCharBeforeNumber);

    public static final String LessOrEqualSuffix = "(或|或者)\\s*(以下|之下|更[小少低])";

    public static final String OneNumberRangeMoreRegex1 = "({MoreOrEqual}|{MoreRegex})\\s*(?<number1>((?!([并且而並的同時时]|([,，](?!\\d+))|。)).)+)"
            .replace("{MoreOrEqual}", MoreOrEqual)
            .replace("{MoreRegex}", MoreRegex);

    public static final String OneNumberRangeMoreRegex2 = "比\\s*(?<number1>((?!(([,，](?!\\d+))|。)).)+)\\s*更?[大多高]";

    public static final String OneNumberRangeMoreRegex3 = "(?<number1>((?!(([,，](?!\\d+))|。|[或者])).)+)\\s*(或|或者)?\\s*([多几余幾餘]|次?以上|之上|更[大多高])([万亿萬億]{0,2})";

    public static final String OneNumberRangeLessRegex1 = "({LessOrEqual}|{LessRegex})\\s*(?<number2>((?!([并且而並的同時时]|([,，](?!\\d+))|。)).)+)"
            .replace("{LessOrEqual}", LessOrEqual)
            .replace("{LessRegex}", LessRegex);

    public static final String OneNumberRangeLessRegex2 = "比\\s*(?<number2>((?!(([,，](?!\\d+))|。)).)+)\\s*更?[小少低]";

    public static final String OneNumberRangeLessRegex3 = "(?<number2>((?!(([,，](?!\\d+))|。|[或者])).)+)\\s*(或|或者)?\\s*(以下|之下|更[小少低])";

    public static final String OneNumberRangeMoreSeparateRegex = "^[.]";

    public static final String OneNumberRangeLessSeparateRegex = "^[.]";

    public static final String OneNumberRangeEqualRegex = "{EqualRegex}\\s*(?<number1>((?!(([,，](?!\\d+))|。)).)+)"
            .replace("{EqualRegex}", EqualRegex);

    public static final String TwoNumberRangeRegex1 = "((位于|在|位於)|(?=(\\d|\\+|\\-)))\\s*(?<number1>((?!(([,，](?!\\d+))|。)).)+)\\s*(和|与|與|{TillRegex})\\s*(?<number2>((?!(([,，](?!\\d+))|。))[^之])+)\\s*(之)?(间|間)"
            .replace("{TillRegex}", TillRegex);

    public static final String TwoNumberRangeRegex2 = "({OneNumberRangeMoreRegex1}|{OneNumberRangeMoreRegex2}|{OneNumberRangeMoreRegex3})\\s*(且|(并|並)且?|而且|((的)?同時)|((的)?同时)|[,，])?\\s*({OneNumberRangeLessRegex1}|{OneNumberRangeLessRegex2}|{OneNumberRangeLessRegex3})"
            .replace("{OneNumberRangeMoreRegex1}", OneNumberRangeMoreRegex1)
            .replace("{OneNumberRangeMoreRegex2}", OneNumberRangeMoreRegex2)
            .replace("{OneNumberRangeMoreRegex3}", OneNumberRangeMoreRegex3)
            .replace("{OneNumberRangeLessRegex1}", OneNumberRangeLessRegex1)
            .replace("{OneNumberRangeLessRegex2}", OneNumberRangeLessRegex2)
            .replace("{OneNumberRangeLessRegex3}", OneNumberRangeLessRegex3);

    public static final String TwoNumberRangeRegex3 = "({OneNumberRangeLessRegex1}|{OneNumberRangeLessRegex2}|{OneNumberRangeLessRegex3})\\s*(且|(并|並)且?|而且|((的)?同時)|((的)?同时)|[,，])?\\s*({OneNumberRangeMoreRegex1}|{OneNumberRangeMoreRegex2}|{OneNumberRangeMoreRegex3})"
            .replace("{OneNumberRangeMoreRegex1}", OneNumberRangeMoreRegex1)
            .replace("{OneNumberRangeMoreRegex2}", OneNumberRangeMoreRegex2)
            .replace("{OneNumberRangeMoreRegex3}", OneNumberRangeMoreRegex3)
            .replace("{OneNumberRangeLessRegex1}", OneNumberRangeLessRegex1)
            .replace("{OneNumberRangeLessRegex2}", OneNumberRangeLessRegex2)
            .replace("{OneNumberRangeLessRegex3}", OneNumberRangeLessRegex3);

    public static final String TwoNumberRangeRegex4 = "(?<number1>((?!(([,，](?!\\d+))|。)).)+)\\s*{TillRegex}\\s*(?<number2>((?!(([,，](?!\\d+))|。)).)+)"
            .replace("{TillRegex}", TillRegex);

    public static final ImmutableMap<String, String> AmbiguityFiltersDict = ImmutableMap.<String, String>builder()
        .put("十", "十足")
        .put("伍", "队伍")
        .put("肆", "放肆|肆意|肆无忌惮")
        .put("陆", "大陆|陆地|登陆|海陆")
        .put("拾", "拾取|拾起|收拾|拾到|朝花夕拾")
        .build();

    public static final String AmbiguousFractionConnectorsRegex = "^[.]";

    public static final ImmutableMap<String, String> RelativeReferenceOffsetMap = ImmutableMap.<String, String>builder()
        .put("", "")
        .build();

    public static final ImmutableMap<String, String> RelativeReferenceRelativeToMap = ImmutableMap.<String, String>builder()
        .put("", "")
        .build();
}
