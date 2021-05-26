using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions;
using Microsoft.Recognizers.Definitions.Korean;
using Microsoft.Recognizers.Definitions.Utilities;
using Microsoft.Recognizers.Text.Number;
using Microsoft.Recognizers.Text.Number.Config;
using Microsoft.Recognizers.Text.Number.Korean;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Korean
{
    public abstract class KoreanNumberWithUnitExtractorConfiguration : INumberWithUnitExtractorConfiguration
    {

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        private static readonly Regex CompoundUnitConnRegex =
            new Regex(NumbersWithUnitDefinitions.CompoundUnitConnectorRegex, RegexFlags);

        private static readonly Regex NonUnitsRegex =
            new Regex(BaseUnits.PmNonUnitRegex, RegexFlags);

        private static readonly Regex HalfUnitRegex = new Regex(NumbersWithUnitDefinitions.HalfUnitRegex, RegexFlags);

        protected KoreanNumberWithUnitExtractorConfiguration(CultureInfo ci)
        {
            this.CultureInfo = ci;

            var numConfig = new BaseNumberOptionsConfiguration(ci.Name, NumberOptions.None);

            this.UnitNumExtractor = new NumberExtractor(numConfig, CJKNumberExtractorMode.ExtractAll);

            this.BuildPrefix = NumbersWithUnitDefinitions.BuildPrefix;
            this.BuildSuffix = NumbersWithUnitDefinitions.BuildSuffix;
            this.ConnectorToken = NumbersWithUnitDefinitions.ConnectorToken;

            AmbiguityFiltersDict = DefinitionLoader.LoadAmbiguityFilters(NumbersWithUnitDefinitions.AmbiguityFiltersDict);
        }

        public Regex CompoundUnitConnectorRegex => CompoundUnitConnRegex;

        public Regex NonUnitRegex => NonUnitsRegex;

        public virtual Regex AmbiguousUnitNumberMultiplierRegex => null;

        public abstract string ExtractType { get; }

        public CultureInfo CultureInfo { get; }

        public IExtractor UnitNumExtractor { get; }

        public string BuildPrefix { get; }

        public string BuildSuffix { get; }

        public string ConnectorToken { get; }

        public IExtractor IntegerExtractor { get; }

        public Dictionary<Regex, Regex> AmbiguityFiltersDict { get; } = null;

        public abstract ImmutableDictionary<string, string> SuffixList { get; }

        public abstract ImmutableDictionary<string, string> PrefixList { get; }

        public abstract ImmutableList<string> AmbiguousUnitList { get; }

        public void ExpandHalfSuffix(string source, ref List<ExtractResult> result, IOrderedEnumerable<ExtractResult> numbers)
        {
            // Expand Korean phrase to the `half` patterns when it follows closely origin phrase.
            if (HalfUnitRegex != null && numbers != null)
            {
                var match = new List<ExtractResult>();
                foreach (var number in numbers)
                {
                    if (HalfUnitRegex.Matches(number.Text).Count == 1)
                    {
                        match.Add(number);
                    }

                }

                if (match.Count > 0)
                {
                    var res = new List<ExtractResult>();
                    foreach (var er in result)
                    {
                        int start = (int)er.Start;
                        int length = (int)er.Length;
                        var match_suffix = new List<ExtractResult>();
                        foreach (var mr in match)
                        {
                            // Take into account possible whitespaces between result and half unit.
                            var subLength = (int)mr.Start - (start + length) >= 0 ? (int)mr.Start - (start + length) : 0;
                            var midStr = source.Substring(start + length, subLength);
                            if (string.IsNullOrWhiteSpace(midStr) && (int)mr.Start - (start + length) >= 0)
                            {
                                match_suffix.Add(mr);
                            }
                        }

                        if (match_suffix.Count == 1)
                        {
                            var mr = match_suffix[0];
                            var suffixLength = (int)(mr.Start + mr.Length) - (start + length);
                            er.Length += suffixLength;
                            er.Text += source.Substring(start + length, suffixLength);
                            var tmp = new List<ExtractResult>();
                            tmp.Add((ExtractResult)er.Data);
                            tmp.Add(mr);
                            er.Data = tmp;
                        }

                        res.Add(er);
                    }

                    result = res;
                }
            }
        }
    }
}
