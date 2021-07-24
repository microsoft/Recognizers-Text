// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Recognizers.Text.Number
{
    public static class RegexTagGenerator
    {
        private static int priority;

        public static TypeTag GenerateRegexTag(string extractorType, string suffix)
        {
            return new TypeTag($"{extractorType}{suffix}", ++priority);
        }
    }
}
