using System.Text.RegularExpressions;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime.Chinese
{
    public class MergedParserChs : BaseMergedParser
    {
        private static readonly Regex BeforeRegex = new Regex(@"(前|之前)$",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        private static readonly Regex AfterRegex = new Regex(@"(后|之后)$", RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public MergedParserChs(IMergedParserConfiguration configuration) : base(configuration) { }

        public new ParseResult Parse(ExtractResult er)
        {
            return Parse(er, DateObject.Now);
        }

        public new ParseResult Parse(ExtractResult er, DateObject refTime)
        {
            var referenceTime = refTime;
            DateTimeParseResult pr = null;

            // push, save teh MOD string
            bool hasBefore = false, hasAfter = false;
            var modStr = string.Empty;
            if (BeforeRegex.IsMatch(er.Text))
            {
                hasBefore = true;
            }
            else if (AfterRegex.IsMatch(er.Text))
            {
                hasAfter = true;
            }

            if (er.Type.Equals(Constants.SYS_DATETIME_DATE))
            {
                pr = this.Config.DateParser.Parse(er, referenceTime);
                if (pr.Value == null)
                {
                    //pr = this.config.HolidayParser.Parse(er, referenceTime);
                }
            }
            else if (er.Type.Equals(Constants.SYS_DATETIME_TIME))
            {
                pr = this.Config.TimeParser.Parse(er, referenceTime);
            }
            else if (er.Type.Equals(Constants.SYS_DATETIME_DATETIME))
            {
                pr = this.Config.DateTimeParser.Parse(er, referenceTime);
            }
            else if (er.Type.Equals(Constants.SYS_DATETIME_DATEPERIOD))
            {
                pr = this.Config.DatePeriodParser.Parse(er, referenceTime);
            }
            else if (er.Type.Equals(Constants.SYS_DATETIME_TIMEPERIOD))
            {
                pr = this.Config.TimePeriodParser.Parse(er, referenceTime);
            }
            else if (er.Type.Equals(Constants.SYS_DATETIME_DATETIMEPERIOD))
            {
                pr = this.Config.DateTimePeriodParser.Parse(er, referenceTime);
            }
            else if (er.Type.Equals(Constants.SYS_DATETIME_DURATION))
            {
                pr = this.Config.DurationParser.Parse(er, referenceTime);
            }
            else if (er.Type.Equals(Constants.SYS_DATETIME_SET))
            {
                pr = this.Config.GetParser.Parse(er, referenceTime);
            }
            else
            {
                return null;
            }

            // pop, restore the MOD string
            if (hasBefore)
            {
                var val = (DateTimeResolutionResult)pr.Value;
                if (val != null)
                {
                    val.Mod = TimeTypeConstants.beforeMod;
                }
                pr.Value = val;
            }

            if (hasAfter)
            {
                var val = (DateTimeResolutionResult)pr.Value;
                if (val != null)
                {
                    val.Mod = TimeTypeConstants.afterMod;
                }
                pr.Value = val;
            }

            pr.Value = DateTimeResolution(pr, hasBefore, hasAfter);

            //change the type at last for the after or before mode
            pr.Type = $"{ParserTypeName}.{DetermineDateTimeType(er.Type, hasBefore, hasAfter)}";

            return pr;
        }
    }
}