# ------------------------------------------------------------------------------
# <auto-generated>
#     This code was generated by a tool.
#     Changes to this file may cause incorrect behavior and will be lost if
#     the code is regenerated.
# </auto-generated>
# ------------------------------------------------------------------------------

from .base_numbers import BaseNumbers
# pylint: disable=line-too-long
class ChineseNumeric:
    LangMarker = ''
    DecimalSeparatorChar = '.'
    FractionMarkerToken = ''
    NonDecimalSeparatorChar = ' '
    HalfADozenText = ''
    WordSeparatorToken = ''
    RoundNumberMap = dict([('k', 1000),
                           ('m', 1000000),
                           ('g', 1000000000),
                           ('t', 1000000000000)])
    RoundNumberMapChar = dict([('十', 10),
                               ('百', 100),
                               ('千', 1000),
                               ('万', 10000),
                               ('亿', 100000000),
                               ('兆', 1000000000000),
                               ('拾', 10),
                               ('佰', 100),
                               ('仟', 1000),
                               ('萬', 10000),
                               ('億', 100000000)])
    ZeroToNineMap = dict([('零', 0),
                          ('一', 1),
                          ('二', 2),
                          ('三', 3),
                          ('四', 4),
                          ('五', 5),
                          ('六', 6),
                          ('七', 7),
                          ('八', 8),
                          ('九', 9),
                          ('〇', 0),
                          ('壹', 1),
                          ('贰', 2),
                          ('貳', 2),
                          ('叁', 3),
                          ('肆', 4),
                          ('伍', 5),
                          ('陆', 6),
                          ('陸', 6),
                          ('柒', 7),
                          ('捌', 8),
                          ('玖', 9),
                          ('０', 0),
                          ('１', 1),
                          ('２', 2),
                          ('３', 3),
                          ('４', 4),
                          ('５', 5),
                          ('６', 6),
                          ('７', 7),
                          ('８', 8),
                          ('９', 9),
                          ('0', 0),
                          ('1', 1),
                          ('2', 2),
                          ('3', 3),
                          ('4', 4),
                          ('5', 5),
                          ('6', 6),
                          ('7', 7),
                          ('8', 8),
                          ('9', 9),
                          ('半', 0.5),
                          ('两', 2),
                          ('兩', 2),
                          ('俩', 2),
                          ('倆', 2),
                          ('仨', 3)])
    FullToHalfMap = dict([('０', '0'),
                          ('１', '1'),
                          ('２', '2'),
                          ('３', '3'),
                          ('４', '4'),
                          ('５', '5'),
                          ('６', '6'),
                          ('７', '7'),
                          ('８', '8'),
                          ('９', '9'),
                          ('／', '/'),
                          ('－', '-'),
                          ('，', '\''),
                          ('Ｇ', 'G'),
                          ('Ｍ', 'M'),
                          ('Ｔ', 'T'),
                          ('Ｋ', 'K'),
                          ('ｋ', 'k'),
                          ('．', '.')])
    TratoSimMap = dict([('佰', '百'),
                        ('點', '点'),
                        ('個', '个'),
                        ('幾', '几'),
                        ('對', '对'),
                        ('雙', '双')])
    UnitMap = dict([('萬萬', '億'),
                    ('億萬', '兆'),
                    ('萬億', '兆'),
                    ('万万', '亿'),
                    ('万亿', '兆'),
                    ('亿万', '兆'),
                    (' ', ''),
                    ('多', ''),
                    ('余', ''),
                    ('几', '')])
    RoundDirectList = ['万', '萬', '亿', '兆', '億']
    DigitalNumberRegex = f'((?<=(\\d|\\b)){BaseNumbers.MultiplierLookupRegex}(?=\\b))'
    ZeroToNineFullHalfRegex = f'[\\d１２３４５６７８９０]'
    DigitNumRegex = f'{ZeroToNineFullHalfRegex}+'
    DozenRegex = f'.*打$'
    PercentageRegex = f'(?<=百\\s*分\\s*之).+|.+(?=个\\s*百\\s*分\\s*点)|.*(?=[％%])'
    DoubleAndRoundRegex = f'{ZeroToNineFullHalfRegex}+(\\.{ZeroToNineFullHalfRegex}+)?\\s*[多几余]?[万亿萬億]{{1,2}}'
    FracSplitRegex = f'又|分\\s*之'
    ZeroToNineIntegerRegex = f'[一二三四五六七八九零壹贰貳叁肆伍陆陸柒捌玖〇两兩俩倆仨]'
    NegativeNumberTermsRegex = f'[负負]'
    NegativeNumberTermsRegexNum = f'((?<!(\\d+\\s*)|[-－])[-－])'
    NegativeNumberSignRegex = f'^{NegativeNumberTermsRegex}.*|^{NegativeNumberTermsRegexNum}.*'
    SpeGetNumberRegex = f'{ZeroToNineFullHalfRegex}|{ZeroToNineIntegerRegex}|[十拾半对對]'
    PairRegex = '.*[双对雙對]$'
    RoundNumberIntegerRegex = f'(((?<![十百千拾佰仟])[十百千拾佰仟])|([万亿兆萬億]))'
    WhiteListRegex = f'([。，、（）“”]|[到以至]|[国國]|周|夜|[点點]|[个個]|倍|票|[项項]|[亩畝]|分|元|角|天|加|[减減]|乘|除|是|[對对]|打|公[里裏]|公[顷頃]|公分|平方|方|米|厘|毫|[条條]|船|[车車]|[辆輛]|群|[页頁]|杯|人|[张張]|次|位|份|批|[届屆]|[级級]|[种種]|套|[笔筆]|根|[块塊]|件|座|步|[颗顆]|棵|[节節]|支|只|名|年|月|日|[号號]|朵|克|[吨噸]|磅|[码碼]|英尺|英寸|升|加[仑侖]|立方|[台臺]|套|[罗羅]|令|卷|[头頭]|箱|包|桶|袋|[块塊]|家|行|期|[层層]|度|面|所|架|把|片|[阵陣]|[间間]|等|[叠疊]|碟|下|起|手|季|部|人|小[时時]|[时時]|秒|[样樣]|章|段|星|州|款|代|维|重|[户戸]|楼|路|篇|句|键|本|生|者|字|郎|道|边|场|口|线|世|岸|金|类|番|组|卦|眼|系|声|更|带|色|战|成|轮|食|首|幡|站|股|井|流|开|刻|洲|回|宮|集|练|週|和|环|甲|处|省|里|海|遍|品|体|王|尾|新|隻|版|阶|板|侧|波|身|则|扫|房|彩|木|军|居|晚|岛|课|式|通|相|区|文|端|味|田|心|胎|班|出|连|单|事|丝|副|岁|旁|幕|些|枚|招|卡|幅|言|街|指|辈|室|堆|作|封|厢|声|城|族|圈|脸|目|排|模|夕|网|市|向|极|驱|科|提|核|村|审|刀|册|例|关|粒|局|山|寸|碗|瞬|联|游|脚|宅|线|格|入|趟|貫|界|社|肢|技|滴|问|笑|院|堂|尺|寨|档|举|盘|门|客|餐|艘|毛|丈|剑|曲|任|叶|团|派|嘴|桥|抹|枝|贯|伏|拳|列|机|盒|队|进制|栋|席|斤|词|击|题|型|宗|柱|钱|拍|剧|旬|命|扇|匹|湖|壶|觉|叉|校|泉|具|串|射|证|大批|球|横|竖|尊|轴|观|审|石|束|弹|株|领|委|栏|炮|鼎|町|帆|斗|缕|桌|针|帧|转|落|足|梯|县|投|试|帮|掌|箭|盏|锅|计|大片|学期|截|顶|屋|介|剑|桂|旗|巷|挥|晃|员|翼|池|围|勺|宿|库|棒|冠|树|缸|伙|签|揽|坨|匙|桩|顿|纸|隅|诺|案|刊|厂|杆|袭|仓|床|担|帖|屏|盏|腔|贴|窍|洞|円|坪|泡|园|馆|湾|拨|枪|职|亭|背|維|[護护戸]|樓|鍵|邊|場|線|類|組|聲|帶|戰|輪|開|練|環|處|裏|體|隻|階|側|則|掃|軍|居|島|課|式|區|連|單|絲|歲|廂|聲|臉|網|極|驅|審|冊|關|聯|遊|腳|線|貫|問|檔|舉|盤|門|劍|曲|任|葉|團|派|嘴|橋|抹|枝|貫|伏|拳|列|機|盒|隊|進制|棟|詞|擊|題|錢|壺|覺|證|大批|球|橫|豎|尊|軸|觀|審|彈|領|委|欄|釘|鬥|縷|針|幀|轉|縣|試|幫|盞|鍋|計|學期|截|頂|介|劍|桂|旗|巷|揮|晃|員|圍|勺|宿|庫|棒|冠|樹|缸|夥|簽|攬|樁|頓|紙|隅|諾|廠|桿|襲|倉|擔|盞|貼|竅|洞|坪|泡|員|館|灣|撥|槍|職|\\s|$)'
    NotSingleRegex = f'(({ZeroToNineIntegerRegex}|{ZeroToNineFullHalfRegex}|[十拾])\\s*(\\s*[多几幾余]?\\s*{RoundNumberIntegerRegex}){{1,2}}|[十拾]|{RoundNumberIntegerRegex}\\s*({ZeroToNineIntegerRegex}|{ZeroToNineFullHalfRegex}|零))\\s*((({ZeroToNineIntegerRegex}|{ZeroToNineFullHalfRegex})\\s*(\\s*[多几幾余]?\\s*{RoundNumberIntegerRegex}){{1,2}}|零)\\s*)*{ZeroToNineIntegerRegex}?'
    SingleRegex = f'(?<!{ZeroToNineIntegerRegex}){ZeroToNineIntegerRegex}(?={WhiteListRegex})'
    AllIntRegex = f'((({ZeroToNineIntegerRegex}|{ZeroToNineFullHalfRegex}|[十拾])\\s*(\\s*[多几幾余]?\\s*{RoundNumberIntegerRegex}){{1,2}}|[十拾]|{RoundNumberIntegerRegex}\\s*({ZeroToNineIntegerRegex}|{ZeroToNineFullHalfRegex}|零))\\s*((({ZeroToNineIntegerRegex}|{ZeroToNineFullHalfRegex})\\s*(\\s*[多几幾余]?\\s*{RoundNumberIntegerRegex}){{1,2}}|零)\\s*)*{ZeroToNineIntegerRegex}?|{ZeroToNineIntegerRegex})'
    PlaceHolderPureNumber = f'\\b'
    PlaceHolderDefault = f'\\D|\\b'
    NumbersSpecialsChars = f'(({NegativeNumberTermsRegexNum}|{NegativeNumberTermsRegex})\\s*)?{ZeroToNineFullHalfRegex}+'
    NumbersSpecialsCharsWithSuffix = f'{NegativeNumberTermsRegexNum}?{ZeroToNineFullHalfRegex}+\\s*{BaseNumbers.NumberMultiplierRegex}'
    DottedNumbersSpecialsChar = f'{NegativeNumberTermsRegexNum}?{ZeroToNineFullHalfRegex}{{1,3}}([,，]{ZeroToNineFullHalfRegex}{{3}})+'
    NumbersWithHalfDozen = f'半({RoundNumberIntegerRegex}|打)'
    NumbersWithDozen = f'{AllIntRegex}[双雙对對打](?!{AllIntRegex})'
    PointRegexStr = f'[点點\\.．]'
    AllFloatRegex = f'{NegativeNumberTermsRegex}?{AllIntRegex}\\s*{PointRegexStr}\\s*[一二三四五六七八九零壹贰貳叁肆伍陆陸柒捌玖〇](\\s*{ZeroToNineIntegerRegex})*'
    NumbersWithAllowListRegex = f'(?<![百佰]\\s*分\\s*之\\s*({AllIntRegex}[点點]*|{AllFloatRegex})*){NegativeNumberTermsRegex}?({NotSingleRegex}|{SingleRegex})(?!({AllIntRegex}*([点點]{ZeroToNineIntegerRegex}+)*|{AllFloatRegex})*\\s*[个個]\\s*[百佰]\\s*分\\s*[点點])'
    NumbersAggressiveRegex = f'(?<![百佰]\\s*分\\s*之\\s*({AllIntRegex}[点點]*|{AllFloatRegex})*){NegativeNumberTermsRegex}?{AllIntRegex}(?!({AllIntRegex}*([点點]{ZeroToNineIntegerRegex}+)*|{AllFloatRegex})*\\s*[个個]\\s*[百佰]\\s*分\\s*[点點])'
    PointRegex = f'{PointRegexStr}'
    DoubleSpecialsChars = f'(?<!({ZeroToNineFullHalfRegex}+[\\.．]{ZeroToNineFullHalfRegex}*))({NegativeNumberTermsRegexNum}\\s*)?{ZeroToNineFullHalfRegex}+[\\.．]{ZeroToNineFullHalfRegex}+(?!{ZeroToNineFullHalfRegex}*[\\.．]{ZeroToNineFullHalfRegex}+)'
    DoubleSpecialsCharsWithNegatives = f'(?<!({ZeroToNineFullHalfRegex}+|\\.\\.|．．))({NegativeNumberTermsRegexNum}\\s*)?[\\.．]{ZeroToNineFullHalfRegex}+(?!{ZeroToNineFullHalfRegex}*([\\.．]{ZeroToNineFullHalfRegex}+))'
    SimpleDoubleSpecialsChars = f'({NegativeNumberTermsRegexNum}\\s*)?{ZeroToNineFullHalfRegex}{{1,3}}([,，]{ZeroToNineFullHalfRegex}{{3}})+[\\.．]{ZeroToNineFullHalfRegex}+'
    DoubleWithMultiplierRegex = f'({NegativeNumberTermsRegexNum}\\s*)?{ZeroToNineFullHalfRegex}+[\\.．]{ZeroToNineFullHalfRegex}+\\s*{BaseNumbers.NumberMultiplierRegex}'
    DoubleWithThousandsRegex = f'{NegativeNumberTermsRegex}?{ZeroToNineFullHalfRegex}+([\\.．]{ZeroToNineFullHalfRegex}+)?\\s*[多几幾余]?[万亿萬億]{{1,2}}'
    DoubleAllFloatRegex = f'(?<![百佰]\\s*分\\s*之\\s*(({AllIntRegex}[点點]*)|{AllFloatRegex})*){AllFloatRegex}(?!{ZeroToNineIntegerRegex}*\\s*[个個]\\s*[百佰]\\s*分\\s*[点點])'
    DoubleExponentialNotationRegex = f'(?<!{ZeroToNineFullHalfRegex}+[\\.．])({NegativeNumberTermsRegexNum}\\s*)?{ZeroToNineFullHalfRegex}+([\\.．]{ZeroToNineFullHalfRegex}+)?e(([-－+＋]*[1-9１２３４５６７８９]{ZeroToNineFullHalfRegex}*)|[0０](?!{ZeroToNineFullHalfRegex}+))'
    DoubleScientificNotationRegex = f'(?<!{ZeroToNineFullHalfRegex}+[\\.．])({NegativeNumberTermsRegexNum}\\s*)?({ZeroToNineFullHalfRegex}+([\\.．]{ZeroToNineFullHalfRegex}+)?)\\^([-－+＋]*[1-9１２３４５６７８９]{ZeroToNineFullHalfRegex}*)'
    OrdinalRegex = f'第{AllIntRegex}'
    OrdinalNumbersRegex = f'第{ZeroToNineFullHalfRegex}+'
    AllFractionNumber = f'{NegativeNumberTermsRegex}?(({ZeroToNineFullHalfRegex}+|{AllIntRegex})\\s*又\\s*)?{NegativeNumberTermsRegex}?({ZeroToNineFullHalfRegex}+|{AllIntRegex})\\s*分\\s*之\\s*{NegativeNumberTermsRegex}?({ZeroToNineFullHalfRegex}+|{AllIntRegex})'
    FractionNotationSpecialsCharsRegex = f'({NegativeNumberTermsRegexNum}\\s*)?{ZeroToNineFullHalfRegex}+\\s+{ZeroToNineFullHalfRegex}+[/／]{ZeroToNineFullHalfRegex}+'
    FractionNotationRegex = f'({NegativeNumberTermsRegexNum}\\s*)?{ZeroToNineFullHalfRegex}+[/／]{ZeroToNineFullHalfRegex}+'
    PercentagePointRegex = f'(?<!{AllIntRegex})({AllFloatRegex}|{AllIntRegex})\\s*[个個]\\s*[百佰]\\s*分\\s*[点點]'
    SimplePercentageRegex = f'(?<!{ZeroToNineIntegerRegex})[百佰]\\s*分\\s*之\\s*({AllFloatRegex}|{AllIntRegex}|[百佰])(?!{AllIntRegex})'
    NumbersPercentagePointRegex = f'(?<!{ZeroToNineIntegerRegex})[百佰]\\s*分\\s*之\\s*{ZeroToNineFullHalfRegex}+[\\.．]{ZeroToNineFullHalfRegex}+(?!([\\.．]{ZeroToNineFullHalfRegex}+))'
    NumbersPercentageWithSeparatorRegex = f'(?<!{ZeroToNineIntegerRegex})[百佰]\\s*分\\s*之\\s*{ZeroToNineFullHalfRegex}{{1,3}}([,，]{ZeroToNineFullHalfRegex}{{3}})+[\\.．]{ZeroToNineFullHalfRegex}+'
    NumbersPercentageWithMultiplierRegex = f'(?<!{ZeroToNineIntegerRegex})[百佰]\\s*分\\s*之\\s*{ZeroToNineFullHalfRegex}+[\\.．]{ZeroToNineFullHalfRegex}+\\s*{BaseNumbers.NumberMultiplierRegex}'
    FractionPercentagePointRegex = f'(?<!({ZeroToNineFullHalfRegex}+[\\.．])){ZeroToNineFullHalfRegex}+[\\.．]{ZeroToNineFullHalfRegex}+(?!([\\.．]{ZeroToNineFullHalfRegex}+))\\s*[个個]\\s*[百佰]\\s*分\\s*[点點]'
    FractionPercentageWithSeparatorRegex = f'{ZeroToNineFullHalfRegex}{{1,3}}([,，]{ZeroToNineFullHalfRegex}{{3}})+[\\.．]{ZeroToNineFullHalfRegex}+\\s*[个個]\\s*[百佰]\\s*分\\s*[点點]'
    FractionPercentageWithMultiplierRegex = f'{ZeroToNineFullHalfRegex}+[\\.．]{ZeroToNineFullHalfRegex}+\\s*{BaseNumbers.NumberMultiplierRegex}\\s*[个個]\\s*[百佰]\\s*分\\s*[点點]'
    SimpleNumbersPercentageRegex = f'(?<!{ZeroToNineIntegerRegex})[百佰]\\s*分\\s*之\\s*{ZeroToNineFullHalfRegex}+(?!([\\.．]{ZeroToNineFullHalfRegex}+))'
    SimpleNumbersPercentageWithMultiplierRegex = f'(?<!{ZeroToNineIntegerRegex})[百佰]\\s*分\\s*之\\s*{ZeroToNineFullHalfRegex}+\\s*{BaseNumbers.NumberMultiplierRegex}'
    SimpleNumbersPercentagePointRegex = f'(?!{ZeroToNineIntegerRegex})[百佰]\\s*分\\s*之\\s*{ZeroToNineFullHalfRegex}{{1,3}}([,，]{ZeroToNineFullHalfRegex}{{3}})+'
    IntegerPercentageRegex = f'{ZeroToNineFullHalfRegex}+\\s*[个個]\\s*[百佰]\\s*分\\s*[点點]'
    IntegerPercentageWithMultiplierRegex = f'{ZeroToNineFullHalfRegex}+\\s*{BaseNumbers.NumberMultiplierRegex}\\s*[个個]\\s*[百佰]\\s*分\\s*[点點]'
    NumbersFractionPercentageRegex = f'{ZeroToNineFullHalfRegex}{{1,3}}([,，]{ZeroToNineFullHalfRegex}{{3}})+\\s*[个個]\\s*[百佰]\\s*分\\s*[点點]'
    SimpleIntegerPercentageRegex = f'(?<!%|\\d){NegativeNumberTermsRegexNum}?{ZeroToNineFullHalfRegex}+([\\.．]{ZeroToNineFullHalfRegex}+)?(\\s*)[％%](?!\\d)'
    NumbersFoldsPercentageRegex = f'{ZeroToNineFullHalfRegex}(([\\.．]?|\\s*){ZeroToNineFullHalfRegex})?\\s*折'
    FoldsPercentageRegex = f'{ZeroToNineIntegerRegex}(\\s*[点點]?\\s*{ZeroToNineIntegerRegex})?\\s*折'
    SimpleFoldsPercentageRegex = f'{ZeroToNineFullHalfRegex}\\s*成(\\s*(半|{ZeroToNineFullHalfRegex}))?'
    SpecialsPercentageRegex = f'({ZeroToNineIntegerRegex}|[十拾])\\s*成(\\s*(半|{ZeroToNineIntegerRegex}))?'
    NumbersSpecialsPercentageRegex = f'({ZeroToNineFullHalfRegex}[\\.．]{ZeroToNineFullHalfRegex}|[1１][0０])\\s*成'
    SimpleSpecialsPercentageRegex = f'{ZeroToNineIntegerRegex}\\s*[点點]\\s*{ZeroToNineIntegerRegex}\\s*成'
    SpecialsFoldsPercentageRegex = f'半\\s*成|(?<=打)[对對]\\s*折|半\\s*折'
    TillRegex = f'(到|至|--|-|—|——|~|–)'
    MoreRegex = f'(大于|多于|高于|超过|大於|多於|高於|超過|>)'
    LessRegex = f'(小于|少于|低于|小於|少於|低於|不到|不足|<)'
    EqualRegex = f'(等于|等於|=)'
    MoreOrEqual = f'(({MoreRegex}\\s*(或|或者)?\\s*{EqualRegex})|至少|最少|不{LessRegex})'
    MoreOrEqualSuffix = f'(或|或者)\\s*(以上|之上|更[大多高])'
    LessOrEqual = f'(({LessRegex}\\s*(或|或者)?\\s*{EqualRegex})|至多|最多|不{MoreRegex})'
    LessOrEqualSuffix = f'(或|或者)\\s*(以下|之下|更[小少低])'
    OneNumberRangeMoreRegex1 = f'({MoreOrEqual}|{MoreRegex})\\s*(?<number1>((?!([并且而並的同時时]|([,，](?!\\d+))|。)).)+)'
    OneNumberRangeMoreRegex2 = f'比\\s*(?<number1>((?!(([,，](?!\\d+))|。)).)+)\\s*更?[大多高]'
    OneNumberRangeMoreRegex3 = f'(?<number1>((?!(([,，](?!\\d+))|。|[或者])).)+)\\s*(或|或者)?\\s*([多几余幾餘]|以上|之上|更[大多高])(?![万亿萬億]{{1,2}})'
    OneNumberRangeLessRegex1 = f'({LessOrEqual}|{LessRegex})\\s*(?<number2>((?!([并且而並的同時时]|([,，](?!\\d+))|。)).)+)'
    OneNumberRangeLessRegex2 = f'比\\s*(?<number2>((?!(([,，](?!\\d+))|。)).)+)\\s*更?[小少低]'
    OneNumberRangeLessRegex3 = f'(?<number2>((?!(([,，](?!\\d+))|。|[或者])).)+)\\s*(或|或者)?\\s*(以下|之下|更[小少低])'
    OneNumberRangeMoreSeparateRegex = f'^[.]'
    OneNumberRangeLessSeparateRegex = f'^[.]'
    OneNumberRangeEqualRegex = f'{EqualRegex}\\s*(?<number1>((?!(([,，](?!\\d+))|。)).)+)'
    TwoNumberRangeRegex1 = f'((位于|在|位於)|(?=(\\d|\\+|\\-)))\\s*(?<number1>((?!(([,，](?!\\d+))|。)).)+)\\s*(和|与|與|{TillRegex})\\s*(?<number2>((?!(([,，](?!\\d+))|。))[^之])+)\\s*(之)?(间|間)'
    TwoNumberRangeRegex2 = f'({OneNumberRangeMoreRegex1}|{OneNumberRangeMoreRegex2}|{OneNumberRangeMoreRegex3})\\s*(且|并且|而且|並且|((的)?同時)|((的)?同时)|[,，])?\\s*({OneNumberRangeLessRegex1}|{OneNumberRangeLessRegex2}|{OneNumberRangeLessRegex3})'
    TwoNumberRangeRegex3 = f'({OneNumberRangeLessRegex1}|{OneNumberRangeLessRegex2}|{OneNumberRangeLessRegex3})\\s*(且|并且|而且|並且|((的)?同時)|((的)?同时)|[,，])?\\s*({OneNumberRangeMoreRegex1}|{OneNumberRangeMoreRegex2}|{OneNumberRangeMoreRegex3})'
    TwoNumberRangeRegex4 = f'(?<number1>((?!(([,，](?!\\d+))|。)).)+)\\s*{TillRegex}\\s*(?<number2>((?!(([,，](?!\\d+))|。)).)+)'
    AmbiguousFractionConnectorsRegex = f'^[.]'
# pylint: enable=line-too-long
