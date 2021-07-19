// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Recognizers.Text.Choice
{
    public class BooleanParser : ChoiceParser<bool>
    {
        public BooleanParser()
               : base(new BooleanParserConfiguration())
        {
        }
    }
}