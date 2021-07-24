// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Recognizers.Text.Number
{
    public interface INumberOptionsConfiguration : IConfiguration
    {
        NumberOptions Options { get; }

        NumberMode Mode { get; }

        string Placeholder { get; }
    }
}
