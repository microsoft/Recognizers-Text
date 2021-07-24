﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime
{
    public interface IHolidayParserConfiguration : IDateTimeOptionsConfiguration
    {
        IImmutableDictionary<string, string> VariableHolidaysTimexDictionary { get; }

        IImmutableDictionary<string, Func<int, DateObject>> HolidayFuncDictionary { get; }

        IImmutableDictionary<string, IEnumerable<string>> HolidayNames { get; }

        IEnumerable<Regex> HolidayRegexList { get; }

        int GetSwiftYear(string text);

        string SanitizeHolidayToken(string holiday);
    }
}