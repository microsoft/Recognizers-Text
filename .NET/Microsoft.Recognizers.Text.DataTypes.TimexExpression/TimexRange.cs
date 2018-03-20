// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Recognizers.Text.DataTypes.TimexExpression
{
    public class TimexRange
    {
        public TimexProperties Start { get; set; }

        public TimexProperties End { get; set; }

        public TimexProperties Duration { get; set; }
    }
}
