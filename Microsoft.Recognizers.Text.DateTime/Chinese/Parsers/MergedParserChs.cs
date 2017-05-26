using System.Text.RegularExpressions;
using Microsoft.Recognizers.Text.DateTime.Parsers;
using DateObject = System.DateTime;
using Microsoft.Recognizers.Text.DateTime.Utilities;

namespace Microsoft.Recognizers.Text.DateTime.Chinese.Parsers
{
    public class MergedParserChs : BaseMergedParser
    {
        private static readonly Regex BeforeRegex = new Regex(@"(前|之前)$",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        private static readonly Regex AfterRegex = new Regex(@"(后|之后)$", RegexOptions.IgnoreCase | RegexOptions.Singleline);
        
        public MergedParserChs(IMergedParserConfiguration configuration) : base(configuration)
        {
        }

        public ParseResult Parse(ExtractResult er)
        {
            return Parse(er, null);
        }

        public ParseResult Parse(ExtractResult er, DateObject? refTime)
        {
            var referenceTime = refTime ?? DateObject.Now;
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
                pr = this.config.DateParser.Parse(er, referenceTime);
                if (pr.Value == null)
                {
                    //pr = this.config.HolidayParser.Parse(er, referenceTime);
                }
            }
            else if (er.Type.Equals(Constants.SYS_DATETIME_TIME))
            {
                pr = this.config.TimeParser.Parse(er, referenceTime);
            }
            else if (er.Type.Equals(Constants.SYS_DATETIME_DATETIME))
            {
                pr = this.config.DateTimeParser.Parse(er, referenceTime);
            }
            else if (er.Type.Equals(Constants.SYS_DATETIME_DATEPERIOD))
            {
                pr = this.config.DatePeriodParser.Parse(er, referenceTime);
            }
            else if (er.Type.Equals(Constants.SYS_DATETIME_TIMEPERIOD))
            {
                pr = this.config.TimePeriodParser.Parse(er, referenceTime);
            }
            else if (er.Type.Equals(Constants.SYS_DATETIME_DATETIMEPERIOD))
            {
                pr = this.config.DateTimePeriodParser.Parse(er, referenceTime);
            }
            else if (er.Type.Equals(Constants.SYS_DATETIME_DURATION))
            {
                pr = this.config.DurationParser.Parse(er, referenceTime);
            }
            else if (er.Type.Equals(Constants.SYS_DATETIME_SET))
            {
                pr = this.config.SetParser.Parse(er, referenceTime);
            }
            else
            {
                return null;
            }

            // pop, restore the MOD string
            if (hasBefore)
            {
                var val = (DTParseResult) pr.Value;
                if (val != null)
                {
                    val.mod = TimeTypeConstants.beforeMod;
                }
                pr.Value = val;
            }

            if (hasAfter)
            {
                var val = (DTParseResult) pr.Value;
                if (val != null)
                {
                    val.mod = TimeTypeConstants.afterMod;
                }
                pr.Value = val;
            }

            pr.Value = DateTimeResolution(pr, hasBefore, hasAfter);

            //change the type at last for the after or before mode
            pr.Type = string.Format("{0}.{1}", ParserTypeName, DetermineDateTimeType(er.Type, hasBefore, hasAfter));

            return pr;
        }
    }
}