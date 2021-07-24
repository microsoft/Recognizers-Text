﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;

namespace Microsoft.Recognizers.Text
{
    public class ModelResult
    {
        public string Text { get; set; }

        public int Start { get; set; } = Constants.InvalidIndex;

        public int End { get; set; } = Constants.InvalidIndex;

        public string TypeName { get; set; }

        // Resolution field
        public SortedDictionary<string, object> Resolution { get; set; }
    }
}