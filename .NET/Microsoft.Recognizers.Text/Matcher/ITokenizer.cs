// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;

namespace Microsoft.Recognizers.Text.Matcher
{
    public interface ITokenizer
    {
        List<Token> Tokenize(string input);
    }
}
