using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Arabic;

namespace Microsoft.Recognizers.Text.Number.Arabic
{
    public sealed class PercentageExtractor : BasePercentageExtractor
    {
        public PercentageExtractor(BaseNumberOptionsConfiguration config)
            : base(NumberExtractor.GetInstance(config))
        {
            Options = config.Options;
            Regexes = InitRegexes();
        }

        protected override NumberOptions Options { get; }

        protected override ImmutableHashSet<Regex> InitRegexes()
        {
            HashSet<string> regexStrings = new HashSet<string>
            {
                NumbersDefinitions.NumberWithSuffixPercentage,
                NumbersDefinitions.NumberWithPrefixPercentage,
            };

            if ((Options & NumberOptions.PercentageMode) != 0)
            {
                regexStrings.Add(NumbersDefinitions.FractionNumberWithSuffixPercentage);
                regexStrings.Add(NumbersDefinitions.NumberWithPrepositionPercentage);
            }

            return BuildRegexes(regexStrings);
        }
    }
}