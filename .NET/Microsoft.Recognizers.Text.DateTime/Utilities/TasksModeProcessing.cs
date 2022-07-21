using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.Recognizers.Text.DataTypes.TimexExpression;
using Microsoft.Recognizers.Text.Utilities;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime
{
    public static class TasksModeProcessing
    {
        public const string ParserTypeName = "datetimeV2";

        public static readonly string DateMinString = DateTimeFormatUtil.FormatDate(DateObject.MinValue);

        public static Dictionary<string, string> TasksModeGenerateResolutionSetParser(Dictionary<string, string> resolutionDic, string mod, string timex)
        {
            var res = new Dictionary<string, string>();

            TasksModeAddAltSingleDateTimeToResolution(resolutionDic, TimeTypeConstants.DATETIMEALT, mod, res);
            if (timex.StartsWith("P") && res.Count > 0)
            {
                var extracted = new Dictionary<string, string>();
                TimexRegex.Extract("period", timex, extracted);
                res.Add("intervalSize", extracted.TryGetValue("amount", out var intervalSize) ? intervalSize : string.Empty);
                res.Add("intervalType", extracted.TryGetValue("dateUnit", out var intervalType) ? intervalType : string.Empty);
            }
            else if (timex.StartsWith("XXXX-") && res.Count > 0)
            {
                var extracted = new Dictionary<string, string>();
                TimexRegex.Extract("period", timex, extracted);
                res.Add("intervalSize", extracted.TryGetValue("amount", out var intervalSize) ? intervalSize : "1");
                res.Add("intervalType", extracted.TryGetValue("dateUnit", out var intervalType) ? intervalType : "W");
            }
            else if (timex.StartsWith("T") && res.Count > 0)
            {
                res.Add("intervalSize", "1");
                res.Add("intervalType", "D");
            }

            return res;
        }

        internal static void TasksModeAddAltSingleDateTimeToResolution(Dictionary<string, string> resolutionDic, string type, string mod,
                                                            Dictionary<string, string> res)
        {
            if (resolutionDic.ContainsKey(TimeTypeConstants.DATE))
            {
                res.Add("setTypename", TimeTypeConstants.DATE);
                MergedParserUtil.AddSingleDateTimeToResolution(resolutionDic, TimeTypeConstants.DATE, mod, res);
            }
            else if (resolutionDic.ContainsKey(TimeTypeConstants.DATETIME))
            {
                res.Add("setTypename", Constants.SYS_DATETIME_DATETIME);
                MergedParserUtil.AddSingleDateTimeToResolution(resolutionDic, TimeTypeConstants.DATETIME, mod, res);
            }
            else if (resolutionDic.ContainsKey(TimeTypeConstants.TIME))
            {
                MergedParserUtil.AddSingleDateTimeToResolution(resolutionDic, TimeTypeConstants.TIME, mod, res);
            }
        }
    }
}
