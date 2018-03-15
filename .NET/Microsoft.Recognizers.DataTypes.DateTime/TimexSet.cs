// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Recognizers.DataTypes.DateTime
{
    public class TimexSet
    {
        public TimexSet(string timex)
        {
            Timex = new Timex(timex);
        }

        public Timex Timex { get; set; }
    }
}
