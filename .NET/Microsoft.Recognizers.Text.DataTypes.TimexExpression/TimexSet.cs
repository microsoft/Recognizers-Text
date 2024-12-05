// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Recognizers.Text.DataTypes.TimexExpression
{
    public class TimexSet
    {
        public TimexSet(string timex)
        {
            Timex = new TimexProperty(timex);
        }

        public TimexSet(TimexProperty timex)
        {
            Timex = timex;
        }

        public TimexProperty Timex { get; set; }

        public override string ToString()
        {
            return TimexConvert.ConvertTimexSetToString(this);
        }
    }
}
