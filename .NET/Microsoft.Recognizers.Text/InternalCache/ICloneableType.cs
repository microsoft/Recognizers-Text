// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Recognizers.Text.InternalCache
{
    public interface ICloneableType<T>
    {
        T Clone();
    }
}
