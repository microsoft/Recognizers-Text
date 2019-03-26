// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;

namespace Microsoft.Recognizers.Text.DataTypes.TimexExpression
{
    public class Resolution
    {
        public Resolution()
        {
            Values = new List<Entry>();
        }

        public List<Entry> Values { get; private set; }

        public class Entry
        {
            public string Timex { get; set; }

            public string Type { get; set; }

            public string Value { get; set; }

            public string Start { get; set; }

            public string End { get; set; }
        }
    }
}
