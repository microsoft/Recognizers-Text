// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.DateTime
{
    public interface IHolidayExtractorConfiguration : IDateTimeOptionsConfiguration
    {
        IEnumerable<Regex> HolidayRegexes { get; }
    }
}