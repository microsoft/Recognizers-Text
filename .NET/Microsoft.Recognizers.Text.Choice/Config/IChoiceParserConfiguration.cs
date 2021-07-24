// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;

namespace Microsoft.Recognizers.Text.Choice
{
    public interface IChoiceParserConfiguration<T>
    {
        IDictionary<string, T> Resolutions { get; }
    }
}
