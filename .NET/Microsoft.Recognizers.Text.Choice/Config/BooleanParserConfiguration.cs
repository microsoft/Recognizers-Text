// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;

namespace Microsoft.Recognizers.Text.Choice
{
    public class BooleanParserConfiguration : IChoiceParserConfiguration<bool>
    {
        public static IDictionary<string, bool> Resolutions { get; } = new Dictionary<string, bool>
        {
            { Constants.SYS_BOOLEAN_TRUE, true },
            { Constants.SYS_BOOLEAN_FALSE, false },
        };

        IDictionary<string, bool> IChoiceParserConfiguration<bool>.Resolutions => Resolutions;
    }
}
