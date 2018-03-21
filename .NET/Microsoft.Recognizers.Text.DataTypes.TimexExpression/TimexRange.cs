// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Recognizers.Text.DataTypes.TimexExpression
{
    public class TimexRange
    {
        public TimexProperty Start { get; set; }

        public TimexProperty End { get; set; }

        public TimexProperty Duration { get; set; }
    }
}
