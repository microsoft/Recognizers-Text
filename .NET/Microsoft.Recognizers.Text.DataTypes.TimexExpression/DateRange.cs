// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DataTypes.TimexExpression
{
    public class DateRange
    {
        public DateObject Start { get; set; }

        public DateObject End { get; set; }
    }
}
