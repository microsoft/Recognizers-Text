using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Recognizers.Text.Choice;
using Microsoft.Recognizers.Text.DateTime;
using Microsoft.Recognizers.Text.DateTime.English;
using Microsoft.Recognizers.Text.DateTime.French;
using Microsoft.Recognizers.Text.DateTime.German;
using Microsoft.Recognizers.Text.DateTime.Italian;
using Microsoft.Recognizers.Text.DateTime.Portuguese;
using Microsoft.Recognizers.Text.DateTime.Spanish;
using Microsoft.Recognizers.Text.Number;
using Microsoft.Recognizers.Text.NumberWithUnit;
using Microsoft.Recognizers.Text.Sequence;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DataDrivenTests
{
    public enum Models
    {
        /// <summary>
        /// Number model
        /// </summary>
        Number,

        /// <summary>
        /// Number percebt mode
        /// </summary>
        NumberPercentMode,

        /// <summary>
        /// Number experimental mode
        /// </summary>
        NumberExperimentalMode,

        /// <summary>
        /// Ordinal model
        /// </summary>
        Ordinal,

        /// <summary>
        /// OrdinalEnablePreview model
        /// </summary>
        OrdinalEnablePreview,

        /// <summary>
        /// Percent model
        /// </summary>
        Percent,

        /// <summary>
        /// Percent mode
        /// </summary>
        PercentPercentMode,

        /// <summary>
        /// Number range model
        /// </summary>
        NumberRange,

        /// <summary>
        /// Number range experimental mode
        /// </summary>
        NumberRangeExperimentalMode,

        /// <summary>
        /// Custom number model
        /// </summary>
        CustomNumber,

        /// <summary>
        /// Age model
        /// </summary>
        Age,

        /// <summary>
        /// Currency model
        /// </summary>
        Currency,

        /// <summary>
        /// Dimension model
        /// </summary>
        Dimension,

        /// <summary>
        /// Temperature model
        /// </summary>
        Temperature,

        /// <summary>
        /// Date time model
        /// </summary>
        DateTime,

        /// <summary>
        /// Date time split date and time model
        /// </summary>
        DateTimeSplitDateAndTime,

        /// <summary>
        /// Date time calendar mode
        /// </summary>
        DateTimeCalendarMode,

        /// <summary>
        /// Date time extended types model
        /// </summary>
        DateTimeExtendedTypes,

        /// <summary>
        /// Date time complex calendar model
        /// </summary>
        DateTimeComplexCalendar,

        /// <summary>
        /// Dat time experimental mode
        /// </summary>
        DateTimeExperimentalMode,

        /// <summary>
        /// Phone number model
        /// </summary>
        PhoneNumber,

        /// <summary>
        /// IP Address model
        /// </summary>
        IpAddress,

        /// <summary>
        /// Mention model
        /// </summary>
        Mention,

        /// <summary>
        /// Hashtag model
        /// </summary>
        Hashtag,

        /// <summary>
        /// Email model
        /// </summary>
        Email,

        /// <summary>
        /// URL model
        /// </summary>
        URL,

        /// <summary>
        /// GUID model
        /// </summary>
        GUID,

        /// <summary>
        /// Boolean model
        /// </summary>
        Boolean,
    }

    public enum DateTimeExtractors
    {
        /// <summary>
        /// Date extractor
        /// </summary>
        Date,

        /// <summary>
        /// Time extractor
        /// </summary>
        Time,

        /// <summary>
        /// Date period extractor
        /// </summary>
        DatePeriod,

        /// <summary>
        /// Time period extractor
        /// </summary>
        TimePeriod,

        /// <summary>
        /// Date time extractor
        /// </summary>
        DateTime,

        /// <summary>
        /// Date time period extractor
        /// </summary>
        DateTimePeriod,

        /// <summary>
        /// Duration extractor
        /// </summary>
        Duration,

        /// <summary>
        /// Holiday extractor
        /// </summary>
        Holiday,

        /// <summary>
        /// Time zone extractor
        /// </summary>
        TimeZone,

        /// <summary>
        /// set extractor
        /// </summary>
        Set,

        /// <summary>
        /// merged extractor
        /// </summary>
        Merged,

        /// <summary>
        /// Merged skip from to extractor
        /// </summary>
        MergedSkipFromTo,
    }

    public enum DateTimeParsers
    {
        /// <summary>
        /// Date parser
        /// </summary>
        Date,

        /// <summary>
        /// Time parser
        /// </summary>
        Time,

        /// <summary>
        /// Date period parser
        /// </summary>
        DatePeriod,

        /// <summary>
        /// Time period parser
        /// </summary>
        TimePeriod,

        /// <summary>
        /// Date time parser
        /// </summary>
        DateTime,

        /// <summary>
        /// Date time period parser
        /// </summary>
        DateTimePeriod,

        /// <summary>
        /// Duration parser
        /// </summary>
        Duration,

        /// <summary>
        /// Holiday parser
        /// </summary>
        Holiday,

        /// <summary>
        /// Time zone parser
        /// </summary>
        TimeZone,

        /// <summary>
        /// set parser
        /// </summary>
        Set,

        /// <summary>
        /// merged parser
        /// </summary>
        Merged,
    }

    public class TestResources : Dictionary<string, IList<TestModel>>
    {
    }
}