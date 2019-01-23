namespace Microsoft.Recognizers.Text.DateTime
{
    public enum DatePeriodTimexType
    {
        /// <summary>
        /// Represents a day Period
        /// </summary>
        ByDay,

        /// <summary>
        /// Represents a week Period
        /// </summary>
        ByWeek,

        /// <summary>
        /// Represents a month Period
        /// </summary>
        ByMonth,

        /// <summary>
        /// Represents a year Period
        /// </summary>
        ByYear,
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
        CjkTime,

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
