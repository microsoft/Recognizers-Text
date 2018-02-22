using System.Collections.Generic;

namespace Microsoft.Recognizers.Text.Options
{
    public class BooleanParserConfiguration : IOptionParserConfiguration<bool>
    {
        public static IDictionary<string, bool> Resolutions = new Dictionary<string, bool>
        {
            { Constants.SYS_BOOLEAN_TRUE, true },
            { Constants.SYS_BOOLEAN_FALSE, false }
        };

        IDictionary<string, bool> IOptionParserConfiguration<bool>.Resolutions => Resolutions;
    }
}
