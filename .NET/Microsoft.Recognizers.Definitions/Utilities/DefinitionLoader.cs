﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Definitions.Utilities
{
    public static class DefinitionLoader
    {
        public static Dictionary<Regex, Regex> LoadAmbiguityFilters(Dictionary<string, string> filters)
        {
            var ambiguityFiltersDict = new Dictionary<Regex, Regex>();

            if (filters != null)
            {
                foreach (var item in filters)
                {
                    if (!"null".Equals(item.Key, StringComparison.Ordinal))
                    {
                        ambiguityFiltersDict.Add(new Regex(item.Key, RegexOptions.Singleline), new Regex(item.Value, RegexOptions.Singleline));
                    }
                }
            }

            return ambiguityFiltersDict;
        }
    }
}
