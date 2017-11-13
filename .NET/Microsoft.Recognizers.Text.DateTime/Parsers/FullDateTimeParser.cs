using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Text.Number;

using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime
{
    public class FullDateTimeParser : IDateTimeParser
    {
        private readonly Regex beforeRegex;

        private readonly Regex afterRegex;

        private readonly IFullDateTimeParserConfiguration config;

        public const string ParserTypeName = "datetimeV2";

        public FullDateTimeParser(IFullDateTimeParserConfiguration configuration, DateTimeOptions options)
        {
            config = configuration;
            beforeRegex = new Regex(config.Before, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            afterRegex = afterRegex = new Regex(config.After, RegexOptions.IgnoreCase | RegexOptions.Singleline);
        }
        
        public ParseResult Parse(ExtractResult extResult)
        {
            return Parse(extResult, DateObject.Now);
        }

        public DateTimeParseResult Parse(ExtractResult er, DateObject referenceTime)
        {
            DateTimeParseResult pr = null;

            // push, save teh MOD string
            bool hasBefore = false, hasAfter = false;
            var modStr = string.Empty;
            if (beforeRegex.IsMatch(er.Text))
            {
                hasBefore = true;
                var match = beforeRegex.Match(er.Text);
                er.Start += match.Length;
                er.Length -= match.Length;
                er.Text = er.Text.Substring(match.Length);
                modStr = match.Value;
            }
            else if (afterRegex.IsMatch(er.Text))
            {
                hasAfter = true;
                var match = afterRegex.Match(er.Text);
                er.Start += match.Length;
                er.Length -= match.Length;
                er.Text = er.Text.Substring(match.Length);
                modStr = match.Value;
            }

            if (er.Type.Equals(Constants.SYS_DATETIME_DATE))
            {
                pr = config.DateParser.Parse(er, referenceTime);
                if (pr.Value == null)
                {
                    pr = config.HolidayParser.Parse(er, referenceTime);
                }
            }
            else if (er.Type.Equals(Constants.SYS_DATETIME_TIME))
            {
                pr = config.TimeParser.Parse(er, referenceTime);
            }
            else if (er.Type.Equals(Constants.SYS_DATETIME_DATETIME))
            {
                pr = config.DateTimeParser.Parse(er, referenceTime);
            }
            else if (er.Type.Equals(Constants.SYS_DATETIME_DATEPERIOD))
            {
                pr = config.DatePeriodParser.Parse(er, referenceTime);
            }
            else if (er.Type.Equals(Constants.SYS_DATETIME_TIMEPERIOD))
            {
                pr = config.TimePeriodParser.Parse(er, referenceTime);
            }
            else if (er.Type.Equals(Constants.SYS_DATETIME_DATETIMEPERIOD))
            {
                pr = config.DateTimePeriodParser.Parse(er, referenceTime);
            }
            else if (er.Type.Equals(Constants.SYS_DATETIME_DURATION))
            {
                pr = config.DurationParser.Parse(er, referenceTime);
            }
            else if (er.Type.Equals(Constants.SYS_DATETIME_SET))
            {
                pr = config.GetParser.Parse(er, referenceTime);
            }
            else
            {
                return null;
            }

            // pop, restore the MOD string
            if (hasBefore)
            {
                pr.Length += modStr.Length;
                pr.Start -= modStr.Length;
                pr.Text = modStr + pr.Text;
                var val = (DateTimeResolutionResult)pr.Value;
                val.Mod = TimeTypeConstants.beforeMod;
                pr.Value = val;
            }

            if (hasAfter)
            {
                pr.Length += modStr.Length;
                pr.Start -= modStr.Length;
                pr.Text = modStr + pr.Text;
                var val = (DateTimeResolutionResult)pr.Value;
                val.Mod = TimeTypeConstants.afterMod;
                pr.Value = val;
            }

            pr.Value = DateTimeResolution(pr, hasBefore, hasAfter);

            //change the type at last for the after or before mode
            pr.Type = $"{ParserTypeName}.{DetermineDateTimeType(er.Type, hasBefore, hasAfter)}";

            return pr;
        }

        public string DetermineDateTimeType(string type, bool hasBefore, bool hasAfter)
        {
            if (hasBefore || hasAfter)
            {
                if (type.Equals(Constants.SYS_DATETIME_DATE))
                {
                    return Constants.SYS_DATETIME_DATEPERIOD;
                }

                if (type.Equals(Constants.SYS_DATETIME_TIME))
                {
                    return Constants.SYS_DATETIME_TIMEPERIOD;
                }

                if (type.Equals(Constants.SYS_DATETIME_DATETIME))
                {
                    return Constants.SYS_DATETIME_DATETIMEPERIOD;
                }
            }

            return type;
        }


        public SortedDictionary<string, object> DateTimeResolution(DateTimeParseResult slot, bool hasBefore, bool hasAfter)
        {
            var resolutions = new List<Dictionary<string, string>>();
            var res = new Dictionary<string, object>();

            var type = slot.Type;
            var typeOutput = DetermineDateTimeType(slot.Type, hasBefore, hasAfter);
            var timex = slot.TimexStr;

            var val = (DateTimeResolutionResult)slot.Value;
            if (val == null)
            {
                return null;
            }

            var isLunar = val.IsLunar;
            var mod = val.Mod;
            var comment = val.Comment;

            if (!string.IsNullOrEmpty(timex))
            {
                res.Add("timex", timex);
            }

            if (!string.IsNullOrEmpty(comment))
            {
                res.Add("Comment", comment);
            }

            if (!string.IsNullOrEmpty(mod))
            {
                res.Add("Mod", mod);
            }

            if (!string.IsNullOrEmpty(type))
            {
                res.Add("type", typeOutput);
            }

            var pastResolutionStr = ((DateTimeResolutionResult)slot.Value).PastResolution;
            var futureResolutionStr = ((DateTimeResolutionResult)slot.Value).FutureResolution;

            var resolutionPast = GenerateResolution(type, pastResolutionStr, mod);
            var resolutionFuture = GenerateResolution(type, futureResolutionStr, mod);

            // if past and future are same, keep only one
            if (resolutionFuture.OrderBy(t => t.Key).Select(t => t.Value).SequenceEqual(resolutionPast.OrderBy(t => t.Key).Select(t => t.Value)))
            {
                if (resolutionPast.Count > 0)
                {
                    res.Add("resolve", resolutionPast);
                }
            }
            else
            {
                if (resolutionPast.Count > 0)
                {
                    res.Add("resolveToPast", resolutionPast);
                }

                if (resolutionFuture.Count > 0)
                {
                    res.Add("resolveToFuture", resolutionFuture);
                }
            }

            // if ampm, double our resolution accordingly
            if (!string.IsNullOrEmpty(comment) && comment.Equals("ampm"))
            {
                if (res.ContainsKey("resolve"))
                {
                    ResolveAmpm(res, "resolve");
                }
                else
                {
                    ResolveAmpm(res, "resolveToPast");
                    ResolveAmpm(res, "resolveToFuture");
                }
            }

            if (isLunar)
            {
                res.Add("isLunar", isLunar);
            }

            foreach (var p in res)
            {
                if (p.Value is Dictionary<string, string>)
                {
                    var value = new Dictionary<string, string>();

                    if (!string.IsNullOrEmpty(timex))
                    {
                        value.Add("timex", timex);
                    }

                    if (!string.IsNullOrEmpty(type))
                    {
                        value.Add("type", typeOutput);
                    }

                    foreach (var q in (Dictionary<string, string>)p.Value)
                    {
                        if (value.ContainsKey(q.Key))
                        {
                            value[q.Key] = q.Value;
                        }
                        else
                        {
                            value.Add(q.Key, q.Value);
                        }
                    }

                    resolutions.Add(value);
                }
            }

            if (resolutionPast.Count == 0 && resolutionFuture.Count == 0)
            {
                var notResolved = new Dictionary<string, string> {
                    {
                        "timex", timex
                    }, {
                        "type", typeOutput
                    }, {
                        "value", "not resolved"
                    }
                };

                resolutions.Add(notResolved);
            }

            return new SortedDictionary<string, object> { { "values", resolutions } };
        }

        internal void ResolveAmpm(Dictionary<string, object> resolutionDic,
            string keyName)
        {
            if (resolutionDic.ContainsKey(keyName))
            {
                var resolution = (Dictionary<string, string>)resolutionDic[keyName];
                if (!resolutionDic.ContainsKey("timex"))
                {
                    return;
                }

                var timex = (string)resolutionDic["timex"];
                resolutionDic.Remove(keyName);

                resolutionDic.Add(keyName + "Am", resolution);

                var resolutionPm = new Dictionary<string, string>();
                switch ((string)resolutionDic["type"])
                {
                    case Constants.SYS_DATETIME_TIME:
                        resolutionPm[TimeTypeConstants.VALUE] = FormatUtil.ToPm(resolution[TimeTypeConstants.VALUE]);
                        resolutionPm["timex"] = FormatUtil.ToPm(timex);
                        break;
                    case Constants.SYS_DATETIME_DATETIME:
                        var splited = resolution[TimeTypeConstants.VALUE].Split(' ');
                        resolutionPm[TimeTypeConstants.VALUE] = splited[0] + " " + FormatUtil.ToPm(splited[1]);
                        resolutionPm["timex"] = FormatUtil.AllStringToPm(timex);
                        break;
                    case Constants.SYS_DATETIME_TIMEPERIOD:
                        if (resolution.ContainsKey(TimeTypeConstants.START))
                        {
                            resolutionPm[TimeTypeConstants.START] = FormatUtil.ToPm(resolution[TimeTypeConstants.START]);
                        }

                        if (resolution.ContainsKey(TimeTypeConstants.END))
                        {
                            resolutionPm[TimeTypeConstants.END] = FormatUtil.ToPm(resolution[TimeTypeConstants.END]);
                        }

                        resolutionPm["timex"] = FormatUtil.AllStringToPm(timex);
                        break;
                    case Constants.SYS_DATETIME_DATETIMEPERIOD:
                        splited = resolution[TimeTypeConstants.START].Split(' ');
                        if (resolution.ContainsKey(TimeTypeConstants.START))
                        {
                            resolutionPm[TimeTypeConstants.START] = splited[0] + " " + FormatUtil.ToPm(splited[1]);
                        }

                        splited = resolution[TimeTypeConstants.END].Split(' ');

                        if (resolution.ContainsKey(TimeTypeConstants.END))
                        {
                            resolutionPm[TimeTypeConstants.END] = splited[0] + " " + FormatUtil.ToPm(splited[1]);
                        }

                        resolutionPm["timex"] = FormatUtil.AllStringToPm(timex);
                        break;
                }

                resolutionDic.Add(keyName + "Pm", resolutionPm);
            }
        }

        internal Dictionary<string, string> GenerateResolution(string type, Dictionary<string, string> resolutionDic, string mod)
        {
            var res = new Dictionary<string, string>();

            if (type.Equals(Constants.SYS_DATETIME_DATETIME))
            {
                AddSingleDateTimeToResolution(resolutionDic, TimeTypeConstants.DATETIME, mod, res);
            }
            else if (type.Equals(Constants.SYS_DATETIME_TIME))
            {
                AddSingleDateTimeToResolution(resolutionDic, TimeTypeConstants.TIME, mod, res);
            }
            else if (type.Equals(Constants.SYS_DATETIME_DATE))
            {
                AddSingleDateTimeToResolution(resolutionDic, TimeTypeConstants.DATE, mod, res);
            }
            else if (type.Equals(Constants.SYS_DATETIME_DURATION))
            {
                if (resolutionDic.ContainsKey(TimeTypeConstants.DURATION))
                {
                    res.Add(TimeTypeConstants.VALUE, resolutionDic[TimeTypeConstants.DURATION]);
                }
            }
            else if (type.Equals(Constants.SYS_DATETIME_TIMEPERIOD))
            {
                AddPeriodToResolution(resolutionDic, TimeTypeConstants.START_TIME, TimeTypeConstants.END_TIME, mod, res);
            }
            else if (type.Equals(Constants.SYS_DATETIME_DATEPERIOD))
            {
                AddPeriodToResolution(resolutionDic, TimeTypeConstants.START_DATE, TimeTypeConstants.END_DATE, mod, res);
            }
            else if (type.Equals(Constants.SYS_DATETIME_DATETIMEPERIOD))
            {
                AddPeriodToResolution(resolutionDic, TimeTypeConstants.START_DATETIME, TimeTypeConstants.END_DATETIME, mod, res);
            }

            return res;
        }

        public void AddSingleDateTimeToResolution(Dictionary<string, string> resolutionDic,
            string type,
            string mod,
            Dictionary<string, string> res)
        {
            if (resolutionDic.ContainsKey(type))
            {
                if (!string.IsNullOrEmpty(mod))
                {
                    if (mod.Equals(TimeTypeConstants.beforeMod))
                    {
                        res.Add(TimeTypeConstants.END, resolutionDic[type]);
                        return;
                    }

                    if (mod.Equals(TimeTypeConstants.afterMod))
                    {
                        res.Add(TimeTypeConstants.START, resolutionDic[type]);
                        return;
                    }
                }

                res.Add(TimeTypeConstants.VALUE, resolutionDic[type]);
            }
        }

        public void AddPeriodToResolution(Dictionary<string, string> resolutionDic, string startType, string endType, string mod,
            Dictionary<string, string> res)
        {
            var start = "";
            var end = "";

            if (resolutionDic.ContainsKey(startType))
            {
                start = resolutionDic[startType];
            }

            if (resolutionDic.ContainsKey(endType))
            {
                end = resolutionDic[endType];
            }

            if (!string.IsNullOrEmpty(mod))
            {
                //For before mode, the start of the period should be the end the new period, no start 
                if (mod.Equals(TimeTypeConstants.beforeMod))
                {
                    res.Add(TimeTypeConstants.END, start);
                    return;
                }

                //For after mode, the end of the period should be the start the new period, no end 
                if (mod.Equals(TimeTypeConstants.afterMod))
                {
                    res.Add(TimeTypeConstants.START, end);
                    return;
                }
            }

            if (!string.IsNullOrEmpty(start) && !string.IsNullOrEmpty(end))
            {
                res.Add(TimeTypeConstants.START, start);
                res.Add(TimeTypeConstants.END, end);
            }
        }
    }
}