// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Utilities
{
    public static class CommonUtils
    {
        // Expand patterns with 'half' suffix in CJK implementation.
        public static void ExpandHalfSuffix(string source, ref List<ExtractResult> result, IOrderedEnumerable<ExtractResult> numbers, Regex halfUnitRegex)
        {
            if (halfUnitRegex != null && numbers != null)
            {
                var match = new List<ExtractResult>();
                foreach (var number in numbers)
                {
                    if (halfUnitRegex.Matches(number.Text).Count == 1)
                    {
                        match.Add(number);
                    }

                }

                if (match.Count > 0)
                {
                    var res = new List<ExtractResult>();
                    foreach (var er in result)
                    {
                        int start = (int)er.Start;
                        int length = (int)er.Length;
                        var match_suffix = new List<ExtractResult>();
                        foreach (var mr in match)
                        {
                            // Take into account possible whitespaces between result and half unit.
                            var subLength = (int)mr.Start - (start + length) >= 0 ? (int)mr.Start - (start + length) : 0;
                            var midStr = source.Substring(start + length, subLength);
                            if (string.IsNullOrWhiteSpace(midStr) && (int)mr.Start - (start + length) >= 0)
                            {
                                match_suffix.Add(mr);
                            }
                        }

                        if (match_suffix.Count == 1)
                        {
                            var mr = match_suffix[0];
                            var suffixLength = (int)(mr.Start + mr.Length) - (start + length);
                            er.Length += suffixLength;
                            er.Text += source.Substring(start + length, suffixLength);
                            var tmp = new List<ExtractResult>();
                            tmp.Add((ExtractResult)er.Data);
                            tmp.Add(mr);
                            er.Data = tmp;
                        }

                        res.Add(er);
                    }

                    result = res;
                }
            }
        }
    }
}