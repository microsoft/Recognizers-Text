﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.Utilities
{
    public class ConditionalMatch
    {
        public ConditionalMatch(Match match, bool success)
        {
            Match = match;
            Success = success;
        }

        public Match Match { get; }

        public bool Success { get; }

        public int Index
        {
            get
            {
                return Match.Index;
            }
        }

        public int Length
        {
            get
            {
                return Match.Length;
            }
        }

        public string Value
        {
            get
            {
                return Match.Value;
            }
        }

        public GroupCollection Groups
        {
            get
            {
                return Match.Groups;
            }
        }
    }
}
