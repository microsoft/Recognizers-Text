using Microsoft.Recognizers.Text.Number.Chinese;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Recognizers.Text.Number.Tests
{
    [TestClass]
    public class TestNumberChinese
    {
        private void BasicTest(IModel model, string source, string value)
        {
            var resultStr = model.Parse(source);
            var resultJson = resultStr;
            Assert.AreEqual(1, resultJson.Count);
            Assert.AreEqual(source.Trim().Length - 1, resultJson[0].End);
            Assert.AreEqual(0, resultJson[0].Start);
            Assert.AreEqual(value, resultJson[0].Resolution["value"]);
        }

        private void WrappedTest(IModel model, string source, string extractSrc, string value)
        {
            var resultStr = model.Parse(source);
            var resultJson = resultStr;
            Assert.AreEqual(1, resultJson.Count);
            Assert.AreEqual(source.Trim().Length - 1, resultJson[0].End);
            Assert.AreEqual(0, resultJson[0].Start);
            Assert.AreEqual(value, resultJson[0].Resolution["value"]);
        }


        private void MultiTest(IModel model, string source, int count)
        {
            var resultStr = model.Parse(source);
            var resultJson = resultStr;
            Assert.AreEqual(count, resultJson.Count);
        }

        private void MultiOneTest(IModel model, string source, int count, string first)
        {
            var resultStr = model.Parse(source);
            var resultJson = resultStr;
            Assert.AreEqual(count, resultJson.Count);
            Assert.AreEqual(resultJson[0].Resolution["value"], first);
        }

        [TestMethod]
        public void TestNumberModel()
        {
            var model = NumberRecognizer.Instance.GetNumberModel(Culture.Chinese);

            #region Integer numbers

            BasicTest(model,
                "肆佰陸拾",
                "460");

            BasicTest(model,
                "十 余 万",
                "100000");

            BasicTest(model,
                "一百五十二",
                "152");

            BasicTest(model,
                "壹佰伍拾",
                "150");

            BasicTest(model,
                "两千四百五",
                "2450");

            BasicTest(model,
                "负三十二",
                "-32");

            MultiTest(model,
                "万",
                0);

            MultiTest(model,
                "万万",
                0);

            BasicTest(model,
                "二",
                "2");

            BasicTest(model,
                "10万万",
                "1000000000");

            BasicTest(model,
                "4.78万亿",
                "4780000000000");

            BasicTest(model,
                "7 万5 千4 百",
                "75400");

            BasicTest(model,
                "40k",
                "40000");

            BasicTest(model,
                "二十 億",
                "2000000000");

            BasicTest(model,
                "４，５６５",
                "4565");

            BasicTest(model,
                "四百五十",
                "450");

            BasicTest(model,
                "四百五",
                "450");

            BasicTest(model,
                "一千零一",
                "1001");

            BasicTest(model,
                "一百零一万一千",
                "1011000");

            BasicTest(model,
                "一千两百三十四万",
                "12340000");

            BasicTest(model,
                "五亿零六",
                "500000006");

            BasicTest(model,
                "七十八万五佰零六",
                "780506");

            BasicTest(model,
                "肆拾肆",
                "44");

            BasicTest(model,
                "倆千两百贰拾二",
                "2222");

            BasicTest(model,
                "5万4千6百",
                "54600");

            BasicTest(model,
                "5亿1万4千6百",
                "500014600");

            BasicTest(model,
                "-10000",
                "-10000");

            BasicTest(model,
                "一打",
                "12");

            BasicTest(model,
                "拾万零五十六",
                "100056");

            BasicTest(model,
                "十",
                "10");

            BasicTest(model,
                "十五",
                "15");

            BasicTest(model,
                "一百",
                "100");

            BasicTest(model,
                "九百九十九",
                "999");

            BasicTest(model,
                "九百九",
                "990");

            BasicTest(model,
                "二百五十",
                "250");

            BasicTest(model,
                "一千零五十",
                "1050");

            BasicTest(model,
                "一千零五",
                "1005");

            BasicTest(model,
                "一千陆",
                "1600");

            BasicTest(model,
                "一万三",
                "13000");

            BasicTest(model,
                "一万二百五十",
                "10250");

            BasicTest(model,
                "一万零二百五十",
                "10250");

            BasicTest(model,
                "九万四",
                "94000");

            BasicTest(model,
                "九十二万四",
                "924000");

            BasicTest(model,
                "九万零四百",
                "90400");

            BasicTest(model,
                "九百万零四百",
                "9000400");

            BasicTest(model,
                "四千零五万",
                "40050000");

            BasicTest(model,
                "四千万",
                "40000000");

            BasicTest(model,
                "四千万零六千",
                "40006000");

            BasicTest(model,
                "四亿",
                "400000000");

            BasicTest(model,
                "四亿五千六百四十二万零一十五",
                "456420015");

            BasicTest(model,
                "四万五千六百二十二",
                "45622");

            BasicTest(model,
                "五兆六十二亿四万五千二",
                "5006200045200");

            BasicTest(model,
                "五十五兆一十二亿四万五千二",
                "55001200045200");

            BasicTest(model,
                "十万柒仟捌佰伍拾肆",
                "107854");

            BasicTest(model,
                "半百",
                "50");

            BasicTest(model,
                "拾万零五十六",
                "100056");

            BasicTest(model,
                "拾六万零五百零六",
                "160506");

            BasicTest(model,
                "拾亿零六万零五百零六",
                "1000060506");

            BasicTest(model,
                "一百万零四",
                "1000004");

            BasicTest(model,
                "四十万",
                "400000");

            BasicTest(model,
                "四千零四万",
                "40040000");

            BasicTest(model,
                "四十五亿四千零四万",
                "4540040000");

            BasicTest(model,
                "负四十五亿零四百零四万",
                "-4504040000");

            BasicTest(model,
                "负四百零五亿零四百零四万",
                "-40504040000");

            BasicTest(model,
                "四万万",
                "400000000");

            BasicTest(model,
                "五十打",
                "600");

            BasicTest(model,
                "百三",
                "130");

            MultiTest(model,
                "1, 234, 567",
                3);

            MultiTest(model,
                "二百五, 二百五, 还有二百五。",
                3);

            MultiTest(model,
                "1502222, 二百五, 还有二分之。",
                3);

            MultiTest(model,
                "199个",
                1);

            BasicTest(model,
                "1,050,000,000",
                "1050000000");

            BasicTest(model,
                "两千三百五",
                "2350");

            BasicTest(model,
                "一百 五 十 二",
                "152");
            BasicTest(model,
                "两千 四 百 五",
                "2450");

            BasicTest(model,
                "7万 5千 4百",
                "75400");

            BasicTest(model,
                "三千 八百 九十六 万 四千 九百 六十五",
                "38964965");

            BasicTest(model,
                "三千 九百 六十五 ",
                "3965");
            BasicTest(model,
                "五百 余万",
                "5000000");

            BasicTest(model,
                "一千多万",
                "10000000");

            BasicTest(model,
                "十多万",
                "100000");

            BasicTest(model,
                "二十几万",
                "200000");

            BasicTest(model,
                "一百零几万",
                "1000000");

            BasicTest(model,
                "一千五百零几万",
                "15000000");

            BasicTest(model,
                "一百 二 十 几亿",
                "12000000000");

            BasicTest(model,
                "三双",
                "6");

            BasicTest(model,
                "一百对",
                "200");

            BasicTest(model,
                "十双",
                "20");

            BasicTest(model,
                "一百五十对",
                "300");

            MultiOneTest(model,
                "十打苹果", 1, "120");

            MultiTest(model,
                "１９９１年１月１６日晚巴黎时间２０点：法国总统密特朗发表电视讲话，宣布和平解决海湾危机已经让位于战争，法国已经作好一切战斗准备。",
                4);

            BasicTest(model,
                "500余万",
                "5000000");
            BasicTest(model,
                "八千 三百 八十五 万 二千 三百 二十六 点三三",
                "83852326.33");

            BasicTest(model,
                "2^5",
                "32");

            BasicTest(model,
                "2e5",
                "200000");

            MultiOneTest(model,
                "四百多",
                1,
                "400");

            BasicTest(model,
                "两双",
                "4");

            BasicTest(model,
                "一百五     ",
                "150");

            BasicTest(model,
                "十 余 萬",
                "100000");

            BasicTest(model,
                "壹百五十二",
                "152");

            BasicTest(model,
                "壹佰伍拾",
                "150");

            BasicTest(model,
                "兩千四百五",
                "2450");

            BasicTest(model,
                "負三十二",
                "-32");

            MultiTest(model,
                "萬",
                0);

            MultiTest(model,
                "萬萬",
                0);

            BasicTest(model,
                "二",
                "2");

            BasicTest(model,
                "10萬萬",
                "1000000000");

            BasicTest(model,
                "4.78萬億",
                "4780000000000");

            BasicTest(model,
                "7 萬5 千4 百",
                "75400");

            BasicTest(model,
                "40k",
                "40000");

            BasicTest(model,
                "二十 億",
                "2000000000");

            BasicTest(model,
                "４，５６５",
                "4565");

            BasicTest(model,
                "四百五十",
                "450");

            BasicTest(model,
                "四百五",
                "450");

            BasicTest(model,
                "壹千零壹",
                "1001");

            BasicTest(model,
                "壹百零壹萬壹千",
                "1011000");

            BasicTest(model,
                "壹千兩百三十四萬",
                "12340000");

            BasicTest(model,
                "五億零六",
                "500000006");

            BasicTest(model,
                "七十八萬五佰零六",
                "780506");

            BasicTest(model,
                "肆拾肆",
                "44");

            BasicTest(model,
                "兩千兩百貳拾二",
                "2222");

            BasicTest(model,
                "5萬4千6百",
                "54600");

            BasicTest(model,
                "5億1萬4千6百",
                "500014600");

            BasicTest(model,
                "-10000",
                "-10000");

            BasicTest(model,
                "壹打",
                "12");

            BasicTest(model,
                "拾萬零五十六",
                "100056");

            BasicTest(model,
                "十",
                "10");

            BasicTest(model,
                "十五",
                "15");

            BasicTest(model,
                "壹百",
                "100");

            BasicTest(model,
                "九百九十九",
                "999");

            BasicTest(model,
                "九百九",
                "990");

            BasicTest(model,
                "二百五十",
                "250");

            BasicTest(model,
                "壹千零五十",
                "1050");

            BasicTest(model,
                "壹千零五",
                "1005");

            BasicTest(model,
                "壹千六",
                "1600");

            BasicTest(model,
                "壹萬三",
                "13000");

            BasicTest(model,
                "壹萬二百五十",
                "10250");

            BasicTest(model,
                "壹萬零二百五十",
                "10250");

            BasicTest(model,
                "九萬四",
                "94000");

            BasicTest(model,
                "九十二萬四",
                "924000");

            BasicTest(model,
                "九萬零四百",
                "90400");

            BasicTest(model,
                "九百萬零四百",
                "9000400");

            BasicTest(model,
                "四千零五萬",
                "40050000");

            BasicTest(model,
                "四千萬",
                "40000000");

            BasicTest(model,
                "四千萬零六千",
                "40006000");

            BasicTest(model,
                "四億",
                "400000000");

            BasicTest(model,
                "四億五千六百四十二萬零壹十五",
                "456420015");

            BasicTest(model,
                "四萬五千六百二十二",
                "45622");

            BasicTest(model,
                "五兆六十二億四萬五千二",
                "5006200045200");

            BasicTest(model,
                "五十五兆壹十二億四萬五千二",
                "55001200045200");

            BasicTest(model,
                "十萬柒仟捌佰伍拾肆",
                "107854");

            BasicTest(model,
                "半百",
                "50");

            BasicTest(model,
                "拾萬零五十六",
                "100056");

            BasicTest(model,
                "拾六萬零五百零六",
                "160506");

            BasicTest(model,
                "拾億零六萬零五百零六",
                "1000060506");

            BasicTest(model,
                "壹百萬零四",
                "1000004");

            BasicTest(model,
                "四十萬",
                "400000");

            BasicTest(model,
                "四千零四萬",
                "40040000");

            BasicTest(model,
                "四十五億四千零四萬",
                "4540040000");

            BasicTest(model,
                "負四十五億零四百零四萬",
                "-4504040000");

            BasicTest(model,
                "負四百零五億零四百零四萬",
                "-40504040000");

            BasicTest(model,
                "四萬萬",
                "400000000");

            BasicTest(model,
                "五十打",
                "600");

            BasicTest(model,
                "百三",
                "130");

            BasicTest(model,
                "5,236",
                "5236");

            MultiTest(model,
                "1, 234, 567",
                3);

            MultiTest(model,
                "二百五, 二百五, 還有二百五。",
                3);

            MultiTest(model,
                "1502222, 二百五, 還有二分之。",
                3);

            MultiTest(model,
                "199個",
                1);

            BasicTest(model,
                "1,050,000,000",
                "1050000000");

            BasicTest(model,
                "兩千三百五",
                "2350");

            BasicTest(model,
                "壹百 五 十 二",
                "152");
            BasicTest(model,
                "兩千 四 百 五",
                "2450");

            BasicTest(model,
                "7萬 5千 4百",
                "75400");

            BasicTest(model,
                "三千 八百 九十六 萬 四千 九百 六十五",
                "38964965");

            BasicTest(model,
                "三千 九百 六十五 ",
                "3965");
            BasicTest(model,
                "五百 余萬",
                "5000000");

            BasicTest(model,
                "壹千多萬",
                "10000000");

            BasicTest(model,
                "十多萬",
                "100000");

            BasicTest(model,
                "二十幾萬",
                "200000");

            BasicTest(model,
                "壹百零幾萬",
                "1000000");

            BasicTest(model,
                "壹千五百零幾萬",
                "15000000");

            BasicTest(model,
                "壹百 二 十 幾億",
                "12000000000");

            BasicTest(model,
                "三雙",
                "6");

            BasicTest(model,
                "壹百對",
                "200");

            BasicTest(model,
                "十雙",
                "20");

            BasicTest(model,
                "壹百五十對",
                "300");

            MultiOneTest(model,
                "十打蘋果", 1, "120");

            MultiTest(model,
                "１９９１年１月１６日晚巴黎時間２０點：法國總統密特朗發表電視講話，宣布和平解抉海灣危機已經讓位於戰爭，法國已經作好壹切戰鬥準備。",
                4);

            BasicTest(model,
                "500余萬",
                "5000000");
            BasicTest(model,
                "八千 三百 八十五 萬 二千 三百 二十六 點三三",
                "83852326.33");

            BasicTest(model,
                "2^5",
                "32");

            BasicTest(model,
                "2e5",
                "200000");

            MultiOneTest(model,
                "四百多",
                1,
                "400");

            BasicTest(model,
                "兩雙",
                "4");

            BasicTest(model,
                "壹百五     ",
                "150");
            #endregion

            #region Double numbers 

            MultiTest(model,
                "百分之五十五点五七九",
                0);

            MultiTest(model,
                "百分之十万零六百五十一",
                0);

            MultiTest(model,
                "六千零五十一个百分点",
                0);

            MultiTest(model,
                "六百一十一点二五五个百分点",
                0);

            BasicTest(model,
                "负六点六",
                "-6.6");

            BasicTest(model,
                "十五点七",
                "15.7");

            BasicTest(model,
                "１１.92亿",
                "1192000000");

            BasicTest(model,
                "２．２",
                "2.2");

            BasicTest(model,
                "２．２亿",
                "220000000");

            BasicTest(model,
                "1000  万",
                "10000000");

            BasicTest(model,
                "21.2E0",
                "21.2");

            BasicTest(model,
                "2^-1",
                "0.5");

            BasicTest(model,
                "四百 点 五",
                "400.5");

            BasicTest(model,
                "零点五",
                "0.5");

            BasicTest(model,
                "10.233",
                "10.233");

            BasicTest(model,
                "-1e1",
                "-10");

            BasicTest(model,
                "-2.5 M",
                "-2500000");

            BasicTest(model,
                "二点五",
                "2.5");

            BasicTest(model,
                "十点二三三",
                "10.233");

            BasicTest(model,
                "两千万点五五四四",
                "20000000.5544");

            BasicTest(model,
                "两千点零",
                "2000");

            BasicTest(model,
                "两千三点一",
                "2300.1");

            BasicTest(model,
                "10.233",
                "10.233");

            BasicTest(model,
                "10,000.233",
                "10000.233");

            BasicTest(model,
                ".23456000",
                "0.23456");

            BasicTest(model,
                "4.800",
                "4.8");

            BasicTest(model,
                "２.７８９０",
                "2.789");

            BasicTest(model,
                "2.3",
                "2.3");

            BasicTest(model,
                "2.3万",
                "23000");

            BasicTest(model,
                "２.７８９０e－１",
                "0.2789");

            BasicTest(model,
                ".5",
                "0.5");

            BasicTest(model,
                "-.5",
                "-0.5");

            MultiTest(model,
                "1, 234, 567.3",
                3);

            MultiTest(model,
                "１, 2４4, ６６7.123 五点八",
                4);

            MultiTest(model,
                "二百三十三，五百七十七，一千六点五。",
                3);

            MultiTest(model,
                "2222.2222.22222.222",
                4);

            MultiOneTest(model,
                "...9",
                1,
                "9");

            MultiOneTest(model,
                "－－9",
                1,
                "9");

            BasicTest(model,
                "1.1^+23", "8.95430243255239");

            BasicTest(model,
                "2.5^-1", "0.4");

            BasicTest(model,
                "-1.1^+23", "-8.95430243255239");

            BasicTest(model,
                "-2.5^-1", "-0.4");

            BasicTest(model,
                "-1.1^--23", "-8.95430243255239");

            BasicTest(model,
                "-127.32e13", "-1.2732E+15");

            BasicTest(model,
                "12.32e+14", "1.232E+15");

            BasicTest(model,
                "-12e-1", "-1.2");
            #endregion
        }

        [TestMethod]
        public void TestFractionModel()
        {
            var model = NumberRecognizer.Instance.GetNumberModel(Culture.Chinese);

            BasicTest(model,
                "五 分之 一",
                (1.0 / 5).ToString());

            BasicTest(model,
                "三 百 五 又 三 分之 一",
                (350 + 1.0 / 3).ToString());

            BasicTest(model,
                "４ ６／３",
                (4 + (double)6 / 3).ToString());

            BasicTest(model,
                "-3/2",
                "-1.5");

            BasicTest(model,
                "五分之一",
                "0.2");

            BasicTest(model,
                "一百万又五十万分之二十五",
                (1000000 + (double)25 / 500000).ToString());

            BasicTest(model,
                "一百分之二",
                "0.02");

            BasicTest(model,
                "四千二分之三",
                ((double)3 / 4200).ToString());

            BasicTest(model,
                "一百分之2",
                "0.02");

            BasicTest(model,
                "五百分之2333",
                ((double)2333 / 500).ToString());

            BasicTest(model,
                "3又一千分之23",
                (3 + (double)23 / 1000).ToString());

            BasicTest(model,
                "３／５",
                "0.6");

            BasicTest(model,
                "1 ３/5",
                "1.6");

            BasicTest(model,
                "１６/5",
                "3.2");

            BasicTest(model,
                "１６分之5",
                ((double)5 / 16).ToString());

            BasicTest(model,
                "１６分之2225",
                ((double)2225 / 16).ToString());

            BasicTest(model,
                "负一又二分之一",
                "-1.5");

            BasicTest(model,
                "二分之一",
                "0.5");

            BasicTest(model,
                "三百 五又三分之一",
                (350 + (double)1 / 3).ToString());

            BasicTest(model,
                "三百五十又3分之1",
                (350 + (double)1 / 3).ToString());

            BasicTest(model,
                "３分之一百五十七",
                ((double)157 / 3).ToString());

            BasicTest(model,
                "负３分之负一百五十七",
                ((double)157 / 3).ToString());

            MultiTest(model,
                @"一百四十四。一百五十万五千二百四十五,二千零四十五个,三千零五个,和四千万零五十,一百五十四点零,四百亿点零五零,二十五分之一百四十四,十一又十四分之一,1个",
                10);

            MultiTest(model,
                @"1 4/3的美梦,1/2的努力",
                2);

            BasicTest(model,
                "二分 之一",
                "0.5");

            BasicTest(model,
                "三百 五 又 三分  之一",
                (350 + (double)1 / 3).ToString());

            BasicTest(model,
                "三百五十 又 3分  之1",
                (350 + (double)1 / 3).ToString());

            BasicTest(model,
                "３ 分  之 一百五十七",
                ((double)157 / 3).ToString());

            BasicTest(model,
                "负３ 分  之 负一百五十七",
                ((double)157 / 3).ToString());

            BasicTest(model,
                "五 分之 壹",
                (1.0 / 5).ToString());

            BasicTest(model,
                "三 百 五 又 三 分之 壹",
                (350 + 1.0 / 3).ToString());

            BasicTest(model,
                "４ ６／３",
                (4 + (double)6 / 3).ToString());

            BasicTest(model,
                "-3/2",
                "-1.5");

            BasicTest(model,
                "五分之壹",
                "0.2");

            BasicTest(model,
                "壹百萬又五十萬分之二十五",
                (1000000 + (double)25 / 500000).ToString());

            BasicTest(model,
                "壹百分之二",
                "0.02");

            BasicTest(model,
                "四千二分之三",
                ((double)3 / 4200).ToString());

            BasicTest(model,
                "壹百分之2",
                "0.02");

            BasicTest(model,
                "五百分之2333",
                ((double)2333 / 500).ToString());

            BasicTest(model,
                "3又壹千分之23",
                (3 + (double)23 / 1000).ToString());

            BasicTest(model,
                "３／５",
                "0.6");

            BasicTest(model,
                "1 ３/5",
                "1.6");

            BasicTest(model,
                "１６/5",
                "3.2");

            BasicTest(model,
                "１６分之5",
                ((double)5 / 16).ToString());

            BasicTest(model,
                "１６分之2225",
                ((double)2225 / 16).ToString());

            BasicTest(model,
                "負壹又二分之壹",
                "-1.5");

            BasicTest(model,
                "二分之壹",
                "0.5");

            BasicTest(model,
                "三百 五又三分之壹",
                (350 + (double)1 / 3).ToString());

            BasicTest(model,
                "三百五十又3分之1",
                (350 + (double)1 / 3).ToString());

            BasicTest(model,
                "３分之壹百五十七",
                ((double)157 / 3).ToString());

            BasicTest(model,
                "負３分之負壹百五十七",
                ((double)157 / 3).ToString());

            MultiTest(model,
                @"壹百四十四。壹百五十萬五千二百四十五,二千零四十五個,三千零五個,和四千萬零五十,壹百五十四點零,四百億點零五零,二十五分之壹百四十四,十壹又十四分之壹,1個",
                10);

            MultiTest(model,
                @"1 4/3的美夢,1/2的努力",
                2);

            BasicTest(model,
                "二分 之壹",
                "0.5");

            BasicTest(model,
                "三百 五 又 三分  之壹",
                (350 + (double)1 / 3).ToString());

            BasicTest(model,
                "三百五十 又 3分  之1",
                (350 + (double)1 / 3).ToString());

            BasicTest(model,
                "３ 分  之 壹百五十七",
                ((double)157 / 3).ToString());

            BasicTest(model,
                "負３ 分  之 負壹百五十七",
                ((double)157 / 3).ToString());
        }

        [TestMethod]
        public void TestPercentageModel()
        {
            var model = NumberRecognizer.Instance.GetPercentageModel(Culture.Chinese);

            MultiTest(model,
                "打对折", 1);

            MultiOneTest(model,
                "打对折", 1, "50%");

            MultiTest(model,
                "对折", 0);

            BasicTest(model,
                "陆 点 五 折", "65%");

            BasicTest(model,
                "９成", "90%");

            BasicTest(model,
                "七成 六", "76%");

            BasicTest(model,
                "7.２成", "72%");

            BasicTest(model,
                "7 2 折", "72%");

            BasicTest(model,
                "六 点 五 成", "65%");

            BasicTest(model,
                "１０ 成", "100%");

            BasicTest(model,
                "１0 成", "100%");

            BasicTest(model,
                "十 成", "100%");

            BasicTest(model,
                "75折", "75%");

            BasicTest(model,
                "9.9折", "99%");

            BasicTest(model,
                "九 九 折", "99%");

            BasicTest(model,
                "三点一折", "31%");

            BasicTest(model,
                "三成", "30%");

            BasicTest(model,
                "半成", "5%");

            BasicTest(model,
                "半折", "50%");

            BasicTest(model,
                "10成", "100%");

            BasicTest(model,
                "十成", "100%");

            MultiTest(model,
                "十折", 0);

            BasicTest(model,
                "9.5成", "95%");

            BasicTest(model,
                "九成", "90%");

            BasicTest(model,
                "三成半", "35%");

            BasicTest(model,
                "２.５成", "25%");

            BasicTest(model,
                "２成", "20%");

            BasicTest(model,
                "２折", "20%");

            BasicTest(model,
                "两折", "20%");

            BasicTest(model,
                "两成", "20%");

            BasicTest(model,
                "三八折", "38%");

            BasicTest(model,
                "三点一折", "31%");

            MultiTest(model,
                "2成,2.5成,２．１成，２成", 4);

            MultiTest(model,
                "九成,五成,八点五成", 3);

            BasicTest(model,
                "2折", "20%");

            MultiTest(model,
                "2折,2.5折,２．１折，２折", 4);

            MultiTest(model,
                "九折,五五折,八点五折", 3);

            MultiTest(model,
                "五折", 1);

            BasicTest(model,
                "百分之２．４",
                "2.4%");

            MultiTest(model,
                "二三十个百分点",
                0);

            MultiTest(model,
                "百分之二三十",
                0);

            BasicTest(model,
                "百分之２．４",
                "2.4%");

            BasicTest(model,
                "百分之五",
                "5%");

            BasicTest(model,
                "一百五十    个百分点",
                "150%");

            BasicTest(model,
                "六个百分点",
                "6%");

            BasicTest(model,
                "２．４个百分点",
                "2.4%");

            BasicTest(model,
                "-22.2%",
                "-22.2%");

            BasicTest(model,
                "２２％",
                "22%");

            BasicTest(model,
                "－1２２％",
                "-122%");

            BasicTest(model,
                "百分之22",
                "22%");

            BasicTest(model,
                "百分之１２０",
                "120%");

            BasicTest(model,
                "百分之15Ｋ",
                "15000%");

            BasicTest(model,
                "百分之1,111",
                "1111%");

            BasicTest(model,
                "百分之９，９９９",
                "9999%");

            BasicTest(model,
                "12个百分点",
                "12%");

            BasicTest(model,
                "2,123个百分点",
                "2123%");

            BasicTest(model,
                "二十个百分点",
                "20%");

            BasicTest(model,
                "四点 五个百分点",
                "4.5%");

            BasicTest(model,
                "百分之五 十",
                "50%");

            BasicTest(model,
                "百分之一 点五",
                "1.5%");

            BasicTest(model,
                "百分之５６.２",
                "56.2%");

            BasicTest(model,
                "百分之１２",
                "12%");

            BasicTest(model,
                "百分之3,000",
                "3000%");

            BasicTest(model,
                "百分之１，１２３",
                "1123%");

            BasicTest(model,
                "百分之3.2k",
                "3200%");

            BasicTest(model,
                "百分之3.2M",
                "3200000%");

            BasicTest(model,
                "12.56个百分点",
                "12.56%");

            BasicTest(model,
                "０.４个百分点",
                "0.4%");

            BasicTest(model,
                "15,123个百分点",
                "15123%");

            BasicTest(model,
                "１１１，１１１个百分点",
                "111111%");

            BasicTest(model,
                "25%",
                "25%");

            BasicTest(model,
                "12k个百分点",
                "12000%");

            BasicTest(model,
                "１５k个百分点",
                "15000%");

            BasicTest(model,
                "一百五十个百分点",
                "150%");

            BasicTest(model,
                "七十五万个百分点",
                "750000%");

            BasicTest(model,
                "拾万零五十六点叁叁个百分点",
                "100056.33%");

            BasicTest(model,
                "75.2个百分点",
                "75.2%");

            BasicTest(model,
                "75个百分点",
                "75%");

            BasicTest(model,
                "1,075个百分点",
                "1075%");

            BasicTest(model,
                "百分之一百",
                "100%");

            BasicTest(model,
                "百分之百",
                "100%");

            BasicTest(model,
                "百分之一百二十点五",
                "120.5%");

            BasicTest(model,
                "百分之2.4",
                "2.4%");

            BasicTest(model,
                "百分之2",
                "2%");

            BasicTest(model,
                "百分之1,669",
                "1669%");

            BasicTest(model,
                "百分之５２.５",
                "52.5%");

            MultiTest(model,
                "五百分之2.2",
                0);

            MultiTest(model,
                "上升了百分之2.2",
                1);

            BasicTest(model,
                "5％",
                "5%");

            BasicTest(model,
                "５．５％",
                "5.5%");

            BasicTest(model,
                "一 百 五 十 个 百 分 点",
                "150%");

            BasicTest(model,
                "六   个 百 分点",
                "6%");

            BasicTest(model,
                "２．４   个百分 点",
                "2.4%");

            BasicTest(model,
                "百 分之 一百二十点五",
                "120.5%");

            BasicTest(model,
                "百 分 之2.4",
                "2.4%");

            BasicTest(model,
                "百 分之 2",
                "2%");

            BasicTest(model,
                "百  分 之 669",
                "669%");

            BasicTest(model,
                "百 分 之  ５２.５ k",
                "52500%");

            BasicTest(model,
                "百 分 之 一 百二 十点 五",
                "120.5%");

            MultiTest(model,
                "打對折", 1);

            MultiOneTest(model,
                "打對折", 1, "50%");

            MultiTest(model,
                "對折", 0);

            BasicTest(model,
                "陸 點 五 折", "65%");

            BasicTest(model,
                "９成", "90%");

            BasicTest(model,
                "七成 六", "76%");

            BasicTest(model,
                "7.２成", "72%");

            BasicTest(model,
                "7 2 折", "72%");

            BasicTest(model,
                "六 點 五 成", "65%");

            BasicTest(model,
                "１０ 成", "100%");

            BasicTest(model,
                "１0 成", "100%");

            BasicTest(model,
                "十 成", "100%");

            BasicTest(model,
                "75折", "75%");

            BasicTest(model,
                "9.9折", "99%");

            BasicTest(model,
                "九 九 折", "99%");

            BasicTest(model,
                "三點一折", "31%");

            BasicTest(model,
                "三成", "30%");

            BasicTest(model,
                "半成", "5%");

            BasicTest(model,
                "半折", "50%");

            BasicTest(model,
                "10成", "100%");

            BasicTest(model,
                "十成", "100%");

            MultiTest(model,
                "十折", 0);

            BasicTest(model,
                "9.5成", "95%");

            BasicTest(model,
                "九成", "90%");

            BasicTest(model,
                "三成半", "35%");

            BasicTest(model,
                "２.５成", "25%");

            BasicTest(model,
                "２成", "20%");

            BasicTest(model,
                "２折", "20%");

            BasicTest(model,
                "兩折", "20%");

            BasicTest(model,
                "兩成", "20%");

            BasicTest(model,
                "三八折", "38%");

            BasicTest(model,
                "三點一折", "31%");

            MultiTest(model,
                "2成,2.5成,２．１成，２成", 4);

            MultiTest(model,
                "九成,五成,八點五成", 3);

            BasicTest(model,
                "2折", "20%");

            MultiTest(model,
                "2折,2.5折,２．１折，２折", 4);

            MultiTest(model,
                "九折,五五折,八點五折", 3);

            MultiTest(model,
                "五折", 1);

            BasicTest(model,
                "百分之２．４",
                "2.4%");

            MultiTest(model,
                "二三十個百分點",
                0);

            MultiTest(model,
                "百分之二三十",
                0);

            BasicTest(model,
                "百分之２．４",
                "2.4%");

            BasicTest(model,
                "百分之五",
                "5%");

            BasicTest(model,
                "一百五十 個百分點",
                "150%");

            BasicTest(model,
                "六個百分點",
                "6%");

            BasicTest(model,
                "２．４個百分點",
                "2.4%");

            BasicTest(model,
                "-22.2%",
                "-22.2%");

            BasicTest(model,
                "２２％",
                "22%");

            BasicTest(model,
                "－1２２％",
                "-122%");

            BasicTest(model,
                "百分之22",
                "22%");

            BasicTest(model,
                "百分之１２０",
                "120%");

            BasicTest(model,
                "百分之15Ｋ",
                "15000%");

            BasicTest(model,
                "百分之1,111",
                "1111%");

            BasicTest(model,
                "百分之９，９９９",
                "9999%");

            BasicTest(model,
                "12個百分點",
                "12%");

            BasicTest(model,
                "2,123個百分點",
                "2123%");

            BasicTest(model,
                "二十個百分點",
                "20%");

            BasicTest(model,
                "四點 五個百分點",
                "4.5%");

            BasicTest(model,
                "百分之五 十",
                "50%");

            BasicTest(model,
                "百分之一 點五",
                "1.5%");

            BasicTest(model,
                "百分之５６.２",
                "56.2%");

            BasicTest(model,
                "百分之１２",
                "12%");

            BasicTest(model,
                "百分之3,000",
                "3000%");

            BasicTest(model,
                "百分之１，１２３",
                "1123%");

            BasicTest(model,
                "百分之3.2k",
                "3200%");

            BasicTest(model,
                "百分之3.2M",
                "3200000%");

            BasicTest(model,
                "12.56個百分點",
                "12.56%");

            BasicTest(model,
                "０.４個百分點",
                "0.4%");

            BasicTest(model,
                "15,123個百分點",
                "15123%");

            BasicTest(model,
                "１１１，１１１個百分點",
                "111111%");

            BasicTest(model,
                "25%",
                "25%");

            BasicTest(model,
                "12k個百分點",
                "12000%");

            BasicTest(model,
                "１５k個百分點",
                "15000%");

            BasicTest(model,
                "一百五十個百分點",
                "150%");

            BasicTest(model,
                "七十五萬個百分點",
                "750000%");

            BasicTest(model,
                "拾萬零五十六點叁叁個百分點",
                "100056.33%");

            BasicTest(model,
                "75.2個百分點",
                "75.2%");

            BasicTest(model,
                "75個百分點",
                "75%");

            BasicTest(model,
                "1,075個百分點",
                "1075%");

            BasicTest(model,
                "百分之一百",
                "100%");

            BasicTest(model,
                "百分之百",
                "100%");

            BasicTest(model,
                "百分之一百二十點五",
                "120.5%");

            BasicTest(model,
                "百分之2.4",
                "2.4%");

            BasicTest(model,
                "百分之2",
                "2%");

            BasicTest(model,
                "百分之1,669",
                "1669%");

            BasicTest(model,
                "百分之５２.５",
                "52.5%");

            MultiTest(model,
                "五百分之2.2",
                0);

            MultiTest(model,
                "上升了百分之2.2",
                1);

            BasicTest(model,
                "5％",
                "5%");

            BasicTest(model,
                "５．５％",
                "5.5%");

            BasicTest(model,
                "一 百 五 十 個 百 分 點",
                "150%");

            BasicTest(model,
                "六   個 百 分點",
                "6%");

            BasicTest(model,
                "２．４   個百分 點",
                "2.4%");

            BasicTest(model,
                "百 分之 一百二十點五",
                "120.5%");

            BasicTest(model,
                "百 分 之2.4",
                "2.4%");

            BasicTest(model,
                "百 分之 2",
                "2%");

            BasicTest(model,
                "百  分 之 669",
                "669%");

            BasicTest(model,
                "百 分 之  ５２.５ k",
                "52500%");

            BasicTest(model,
                "佰 分 之 一 百二 十點 五",
                "120.5%");
        }

        [TestMethod]
        public void TestOrdinalModel()
        {
            var model = NumberRecognizer.Instance.GetOrdinalModel(Culture.Chinese);

            BasicTest(model,
                "第二百五十",
                "250");

            BasicTest(model,
                "第２５０",
                "250");

            MultiTest(model,
                "第一名第二名第三名第四名",
                4);

            BasicTest(model,
                "第十四",
                "14");

            BasicTest(model,
                "第三",
                "3");
        }
        [TestMethod]
        public void TestCompareModel()
        {
            var model = NumberRecognizer.Instance.GetNumberModel(Culture.Chinese);
            var wmodel = GetWithoutWhiteListNumberModel();

            MultiTest(model,
                "一看",
                0);

            MultiTest(wmodel,
                "一看",
                1);

            MultiTest(model,
                "一美元",
                0);

            MultiTest(wmodel,
                "一美元",
                1);

            MultiTest(model,
                "两美刀",
                0);

            MultiTest(wmodel,
                "两美刀",
                1);

            MultiTest(model,
                "四川",
                0);

            MultiTest(wmodel,
                "四川",
                1);
            MultiTest(model,
                "陆地",
                0);

            MultiTest(wmodel,
                "陆地",
                1);

            MultiTest(model,
                "十",
                1);

            MultiTest(wmodel,
                "十",
                1);

        }

        private static IModel GetWithoutWhiteListNumberModel()
        {
            return
                new NumberModel(
                    AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Number,
                        new ChineseNumberParserConfiguration()), new NumberExtractor(ChineseNumberMode.ExtractAll));
        }
    }
}