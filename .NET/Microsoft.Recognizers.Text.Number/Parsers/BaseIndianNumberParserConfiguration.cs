using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.Number
{
    public class BaseIndianNumberParserConfiguration : BaseNumberParserConfiguration, IIndianNumberParserConfiguration
    {

        public ImmutableDictionary<char, long> ZeroToNineMap { get; set; }

        public ImmutableDictionary<string, double> DecimalUnitsMap { get; set; }

        public Regex FractionPrepositionInverseRegex { get; set; }

        public Regex AdditionTermsRegex { get; set; }

        // Used to parse regional Hindi cases like डेढ/सवा/ढाई
        // they are Indian language specific cases and holds various meaning when prefixed with Number units.
        public virtual double ResolveUnitCompositeNumber(string numberStr)
        {
            if (this.DecimalUnitsMap.ContainsKey(numberStr))
            {
                return this.DecimalUnitsMap[numberStr];
            }

            return 0;
        }
    }
}
