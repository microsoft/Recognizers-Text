using System.Collections.Generic;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime.Utilities
{
    public class ModAndDateResult
    {
        public ModAndDateResult(DateObject beginDate, DateObject endDate, string mod, List<DateObject> dateList)
        {
            this.BeginDate = beginDate;
            this.EndDate = endDate;
            this.Mod = mod;
            this.DateList = dateList;
        }

        public ModAndDateResult(DateObject beginDate, DateObject endDate)
        {
            this.BeginDate = beginDate;
            this.EndDate = endDate;
            this.Mod = string.Empty;
            this.DateList = null;
        }

        public DateObject BeginDate { get; set; }

        public DateObject EndDate { get; set; }

        public string Mod { get; set; }

        public List<DateObject> DateList { get; set; }
    }
}
