// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime
{
    public interface IDateTimeExtractor : IExtractor
    {
        List<ExtractResult> Extract(string input, DateObject reference);
    }
}
