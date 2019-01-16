using System.Linq;

namespace Microsoft.Recognizers.Text.DateTime
{
    public enum DatePeriodTimexType
    {
        ByDay,
        ByWeek,
        ByMonth,
        ByYear
    }

    public enum PeriodType
    {
        /// <summary>
        /// Represents a ShortTime.
        /// </summary>
        ShortTime,

        /// <summary>
        /// Represents a FullTime.
        /// </summary>
        FullTime,
    }

    public enum TimeType
    {
        /// <summary>
        /// 十二点二十三分五十八秒,12点23分53秒
        /// </summary>
        CountryTime,

        /// <summary>
        /// 差五分十二点
        /// </summary>
        LessTime,

        /// <summary>
        /// 大约早上10:00
        /// </summary>
        DigitTime,
    }
}
