using System.Collections.Generic;
using System.Collections.Immutable;

namespace Microsoft.Recognizers.Text.DateTime.Parsers
{
    public abstract class BaseDateParserConfiguration : ICommonDateTimeParserConfiguration
    {
        private static readonly Dictionary<string, int> DayOfMonthDictionary = new Dictionary<string, int>
            {
                {"01", 1},
                {"02", 2},
                {"03", 3},
                {"04", 4},
                {"05", 5},
                {"06", 6},
                {"07", 7},
                {"08", 8},
                {"09", 9},
                {"1", 1},
                {"2", 2},
                {"3", 3},
                {"4", 4},
                {"5", 5},
                {"6", 6},
                {"7", 7},
                {"8", 8},
                {"9", 9},
                {"10", 10},
                {"11", 11},
                {"12", 12},
                {"13", 13},
                {"14", 14},
                {"15", 15},
                {"16", 16},
                {"17", 17},
                {"18", 18},
                {"19", 19},
                {"20", 20},
                {"21", 21},
                {"22", 22},
                {"23", 23},
                {"24", 24},
                {"25", 25},
                {"26", 26},
                {"27", 27},
                {"28", 28},
                {"29", 29},
                {"30", 30},
                {"31", 31}
            };

        public virtual IExtractor CardinalExtractor { get; protected set; }

        public virtual IExtractor IntegerExtractor { get; protected set; }

        public virtual IExtractor OrdinalExtractor { get; protected set; }

        public virtual IParser NumberParser { get; protected set; }

        public virtual IExtractor DateExtractor { get; protected set; }

        public virtual IExtractor TimeExtractor { get; protected set; }

        public virtual IExtractor DateTimeExtractor { get; protected set; }

        public virtual IExtractor DurationExtractor { get; protected set; }

        public virtual IDateTimeParser DateParser { get; protected set; }

        public virtual IDateTimeParser TimeParser { get; protected set; }

        public virtual IDateTimeParser DateTimeParser { get; protected set; }

        public virtual IDateTimeParser DurationParser { get; protected set; }

        public virtual IImmutableDictionary<string, int> MonthOfYear { get; protected set; }

        public virtual IImmutableDictionary<string, int> Numbers { get; protected set; }

        public virtual IImmutableDictionary<string, long> UnitValueMap { get; protected set; }

        public virtual IImmutableDictionary<string, string> SeasonMap { get; protected set; }

        public virtual IImmutableDictionary<string, string> UnitMap { get; protected set; }

        public virtual IImmutableDictionary<string, int> CardinalMap { get; protected set; }

        public virtual IImmutableDictionary<string, int> DayOfWeek { get; protected set; }

        public virtual IImmutableDictionary<string, int> DayOfMonth
        {
            get { return DayOfMonthDictionary.ToImmutableDictionary(); }
        }
    }
}
