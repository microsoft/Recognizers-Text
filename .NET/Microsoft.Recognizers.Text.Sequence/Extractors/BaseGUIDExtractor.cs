using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions;
using Microsoft.Recognizers.Text.Matcher;

namespace Microsoft.Recognizers.Text.Sequence
{
    public class BaseGUIDExtractor : BaseSequenceExtractor
    {
        internal override ImmutableDictionary<Regex, string> Regexes { get; }

        protected sealed override string ExtractType { get; } = Constants.SYS_GUID;

        public BaseGUIDExtractor()
        {
            var regexes = new Dictionary<Regex, string>
            {
                {
                    new Regex(BaseGUID.GUIDRegex), Constants.GUID_REGEX
                }
            };

            Regexes = regexes.ToImmutableDictionary();
        }
    }
}
