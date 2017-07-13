using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime.Chinese.Tests
{
    [TestClass]
    public class TestChineseModel
    {
        public void BasicTest(DateTimeModel model, DateObject baseDateTime, string text, string expectedType, string expectedString, string expectedTimex)
        {
            var results = model.Parse(text, baseDateTime);
            Assert.AreEqual(1, results.Count);
            var result = results.First();
            Assert.AreEqual(expectedString, result.Text);

            var values = result.Resolution["values"] as IEnumerable<Dictionary<string, string>>;
            Assert.AreEqual(expectedType, values.First()["type"]);
            Assert.AreEqual(expectedTimex, values.First()["timex"]);
        }

        public void BasicTest(DateTimeModel model, DateObject baseDateTime, string text, string expectedType, string expectedString, string expectedTimex, string expectedFuture, string expectedPast)
        {
            var results = model.Parse(text, baseDateTime);
            Assert.AreEqual(1, results.Count);
            var result = results.First();
            Assert.AreEqual(expectedString, result.Text);

            var values = result.Resolution["values"] as IEnumerable<Dictionary<string, string>>;

            Assert.AreEqual(expectedType, values.First()["type"]);
            Assert.AreEqual(expectedTimex, values.First()["timex"]);

            Assert.AreEqual(expectedFuture ?? values.Last()["value"], values.Last()["value"]);

            Assert.AreEqual(expectedPast ?? values.First()["value"], values.First()["value"]);
        }

        [TestMethod]
        public void TestDateTime_Date()
        {
            var model = DateTimeRecognizer.Instance.GetDateTimeModel(Culture.Chinese);
            var reference = new DateObject(2017, 3, 22);

            BasicTest(model, reference,
                "2010/01/29",
                Constants.SYS_DATETIME_DATE, "2010/01/29", "2010-01-29");

            BasicTest(model, reference,
                "农历2015年十月初一",
                Constants.SYS_DATETIME_DATE, "农历2015年十月初一", "2015-10-01");

            BasicTest(model, reference,
                "正月三十",
                Constants.SYS_DATETIME_DATE, "正月三十", "XXXX-01-30");

            BasicTest(model, reference,
                "10月12号，星期一",
                Constants.SYS_DATETIME_DATE, "10月12号,星期一", "XXXX-10-12");

            BasicTest(model, reference,
                "最近",
                Constants.SYS_DATETIME_DATE, "最近", "2017-03-22");

            BasicTest(model, reference,
                "12号",
                Constants.SYS_DATETIME_DATE, "12号", "XXXX-XX-12");

            BasicTest(model, reference,
                "二零零四年八月十五",
                Constants.SYS_DATETIME_DATE, "二零零四年八月十五", "2004-08-15");

            BasicTest(model, reference,
                "本月十日",
                Constants.SYS_DATETIME_DATE, "本月十日", "XXXX-03-10");
        }

        [TestMethod]
        public void TestDateTime_DatePeriod()
        {
            var model = DateTimeRecognizer.Instance.GetDateTimeModel(Culture.Chinese);
            var reference = new DateObject(2017, 3, 22);

            BasicTest(model, reference,
                "时间从一月十日到十二日",
                Constants.SYS_DATETIME_DATEPERIOD, "从一月十日到十二日", "(XXXX-01-10,XXXX-01-12,P2D)");

            BasicTest(model, reference,
                "时间从2016年一月十日到十二日",
                Constants.SYS_DATETIME_DATEPERIOD, "从2016年一月十日到十二日", "(2016-01-10,2016-01-12,P2D)");

            BasicTest(model, reference,
                "从一月十日到20日",
                Constants.SYS_DATETIME_DATEPERIOD, "从一月十日到20日", "(XXXX-01-10,XXXX-01-20,P10D)");

            BasicTest(model, reference,
                "明年",
                Constants.SYS_DATETIME_DATEPERIOD, "明年", "2018");

            BasicTest(model, reference,
                "十月的第一周",
                Constants.SYS_DATETIME_DATEPERIOD, "十月的第一周", "XXXX-10-W01");

            BasicTest(model, reference,
                "前1周",
                Constants.SYS_DATETIME_DATEPERIOD, "前1周", "(2017-03-15,2017-03-22,P1W)");

            BasicTest(model, reference,
                "后1年",
                Constants.SYS_DATETIME_DATEPERIOD, "后1年", "(2017-03-23,2018-03-23,P1Y)");

            BasicTest(model, reference,
                "今年夏天",
                Constants.SYS_DATETIME_DATEPERIOD, "今年夏天", "2017-SU");
        }

        [TestMethod]
        public void TestDateTime_DateTime()
        {
            var model = DateTimeRecognizer.Instance.GetDateTimeModel(Culture.Chinese);
            var reference = new DateObject(2016, 11, 7, 14, 7, 0);

            BasicTest(model, reference,
                "2010.01.29晚上六点",
                Constants.SYS_DATETIME_DATETIME, "2010.01.29晚上六点", "2010-01-29T18");

            BasicTest(model, reference,
                "1987年1月11日八点",
                Constants.SYS_DATETIME_DATETIME, "1987年1月11日八点", "1987-01-11T08");

            BasicTest(model, reference,
                "2015年十月初一早上九点二十",
                Constants.SYS_DATETIME_DATETIME, "2015年十月初一早上九点二十", "2015-10-01T09:20");

            BasicTest(model, reference,
                "1月19号下午5:00",
                Constants.SYS_DATETIME_DATETIME, "1月19号下午5:00", "XXXX-01-19T17:00");

            BasicTest(model, reference,
                "明天下午5:00",
                Constants.SYS_DATETIME_DATETIME, "明天下午5:00", "2016-11-08T17:00");

            BasicTest(model, reference,
                "今晚6点",
                Constants.SYS_DATETIME_DATETIME, "今晚6点", "2016-11-07T18");

            BasicTest(model, reference,
                "今晨5点",
                Constants.SYS_DATETIME_DATETIME, "今晨5点", "2016-11-07T05");

            BasicTest(model, reference,
                "今早8点十五分",
                Constants.SYS_DATETIME_DATETIME, "今早8点十五分", "2016-11-07T08:15");
        }

        [TestMethod]
        public void TestDateTime_DateTimePeriod()
        {
            var model = DateTimeRecognizer.Instance.GetDateTimeModel(Culture.Chinese);
            var reference = new DateObject(2016, 11, 7, 16, 12, 0);

            BasicTest(model, reference,
                "从昨天下午两点到明天四点",
                Constants.SYS_DATETIME_DATETIMEPERIOD, "从昨天下午两点到明天四点", "(2016-11-06T14,2016-11-08T04,PT38H)");

            BasicTest(model, reference,
                "从昨天5:00-6:00",
                Constants.SYS_DATETIME_DATETIMEPERIOD, "从昨天5:00-6:00", "(2016-11-06T05:00,2016-11-06T06:00,PT1H)");

            BasicTest(model, reference,
                "1月15号4点和2月3号9点之间",
                Constants.SYS_DATETIME_DATETIMEPERIOD, "1月15号4点和2月3号9点之间", "(XXXX-01-15T04,XXXX-02-03T09,PT461H)");

            BasicTest(model, reference,
                "昨晚",
                Constants.SYS_DATETIME_DATETIMEPERIOD, "昨晚", "2016-11-06TEV");

            BasicTest(model, reference,
                "明天上午",
                Constants.SYS_DATETIME_DATETIMEPERIOD, "明天上午", "2016-11-08TMO");

            BasicTest(model, reference,
                "上个小时",
                Constants.SYS_DATETIME_DATETIMEPERIOD, "上个小时", "(2016-11-07T15:12:00,2016-11-07T16:12:00,PT1H)");

            BasicTest(model, reference,
                "之后5分钟",
                Constants.SYS_DATETIME_DATETIMEPERIOD, "之后5分钟", "(2016-11-07T16:12:00,2016-11-07T16:17:00,PT5M)");

            BasicTest(model, reference,
                "之前3小时",
                Constants.SYS_DATETIME_DATETIMEPERIOD, "之前3小时", "(2016-11-07T13:12:00,2016-11-07T16:12:00,PT3H)");
        }

        [TestMethod]
        public void TestDateTime_Duration()
        {
            var model = DateTimeRecognizer.Instance.GetDateTimeModel(Culture.Chinese);
            var reference = new DateObject(2017, 3, 22);

            BasicTest(model, reference,
                "两年",
                Constants.SYS_DATETIME_DURATION, "两年", "P2Y");

            BasicTest(model, reference,
                "6 天",
                Constants.SYS_DATETIME_DURATION, "6 天", "P6D");

            BasicTest(model, reference,
                "7 周",
                Constants.SYS_DATETIME_DURATION, "7 周", "P7W");

            BasicTest(model, reference,
                "5 小时",
                Constants.SYS_DATETIME_DURATION, "5 小时", "PT5H");
        }

        [TestMethod]
        public void TestDateTime_Set()
        {
            var model = DateTimeRecognizer.Instance.GetDateTimeModel(Culture.Chinese); ;
            var reference = new DateObject(2017, 3, 22);

            BasicTest(model, reference,
                "事件 每天都发生",
                Constants.SYS_DATETIME_SET, "每天", "P1D");

            BasicTest(model, reference,
                "事件每日都发生",
                Constants.SYS_DATETIME_SET, "每日", "P1D");

            BasicTest(model, reference,
                "事件每周都发生",
                Constants.SYS_DATETIME_SET, "每周", "P1W");

            BasicTest(model, reference,
                "事件每个星期都发生",
                Constants.SYS_DATETIME_SET, "每个星期", "P1W");

            BasicTest(model, reference,
                "事件每个月都发生",
                Constants.SYS_DATETIME_SET, "每个月", "P1M");

            BasicTest(model, reference,
                "事件每年都发生",
                Constants.SYS_DATETIME_SET, "每年", "P1Y");

            BasicTest(model, reference,
                "事件每周一都发生",
                Constants.SYS_DATETIME_SET, "每周一", "XXXX-WXX-1");

            BasicTest(model, reference,
                "事件每周一下午八点都发生",
                Constants.SYS_DATETIME_SET, "每周一下午八点", "XXXX-WXX-1T20");
        }

        [TestMethod]
        public void TestDateTime_Time()
        {
            var model = DateTimeRecognizer.Instance.GetDateTimeModel(Culture.Chinese);
            var reference = new DateObject(2017, 3, 22);

            BasicTest(model, reference,
                "下午5:00",
                Constants.SYS_DATETIME_TIME, "下午5:00", "T17:00");

            BasicTest(model, reference,
                "晚上9:30",
                Constants.SYS_DATETIME_TIME, "晚上9:30", "T21:30");

            BasicTest(model, reference,
                "晚上19:30",
                Constants.SYS_DATETIME_TIME, "晚上19:30", "T19:30");

            BasicTest(model, reference,
                "大约十点",
                Constants.SYS_DATETIME_TIME, "大约十点", "T10");

            BasicTest(model, reference,
                "大约晚上十点",
                Constants.SYS_DATETIME_TIME, "大约晚上十点", "T22");

            BasicTest(model, reference,
                "凌晨2点半",
                Constants.SYS_DATETIME_TIME, "凌晨2点半", "T02:30");

            BasicTest(model, reference,
                "零点",
                Constants.SYS_DATETIME_TIME, "零点", "T00");

            BasicTest(model, reference,
                "零点整",
                Constants.SYS_DATETIME_TIME, "零点整", "T00");
        }

        [TestMethod]
        public void TestDateTime_TimePeriod()
        {
            var model = DateTimeRecognizer.Instance.GetDateTimeModel(Culture.Chinese);
            var reference = new DateObject(2017, 3, 22);

            BasicTest(model, reference,
                "从下午五点一刻到六点",
                Constants.SYS_DATETIME_TIMEPERIOD, "从下午五点一刻到六点", "(T17:15,T18,PT0H45M)");

            BasicTest(model, reference,
                "17:55:23-18:33:02",
                Constants.SYS_DATETIME_TIMEPERIOD, "17:55:23-18:33:02", "(T17:55:23,T18:33:02,PT0H37M39S)");

            BasicTest(model, reference,
                "从17点55分23秒至18点33分02秒",
                Constants.SYS_DATETIME_TIMEPERIOD, "从17点55分23秒至18点33分02秒", "(T17:55:23,T18:33:02,PT0H37M39S)");

            BasicTest(model, reference,
                "早上五到六点",
                Constants.SYS_DATETIME_TIMEPERIOD, "早上五到六点", "(T05,T06,PT1H)");

            BasicTest(model, reference,
                "下午五点到晚上七点半",
                Constants.SYS_DATETIME_TIMEPERIOD, "下午五点到晚上七点半", "(T17,T19:30,PT2H30M)");

            BasicTest(model, reference,
                "下午5:00到凌晨3:00",
                Constants.SYS_DATETIME_TIMEPERIOD, "下午5:00到凌晨3:00", "(T17:00,T03:00,PT10H)");

            BasicTest(model, reference,
                "下午5:00到6:00",
                Constants.SYS_DATETIME_TIMEPERIOD, "下午5:00到6:00", "(T17:00,T18:00,PT1H)");

            BasicTest(model, reference,
                "5:00到6:00",
                Constants.SYS_DATETIME_TIMEPERIOD, "5:00到6:00", "(T05:00,T06:00,PT1H)");
        }
    }
}
