﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.DateTime
{
    public interface IDateExtractor : IDateTimeExtractor
    {
        int GetYearFromText(Match match);
    }
}
