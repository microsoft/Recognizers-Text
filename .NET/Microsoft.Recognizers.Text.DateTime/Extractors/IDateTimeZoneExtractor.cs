// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;

namespace Microsoft.Recognizers.Text.DateTime
{
    public interface IDateTimeZoneExtractor : IDateTimeExtractor
    {
        List<ExtractResult> RemoveAmbiguousTimezone(List<ExtractResult> extractResult);
    }
}