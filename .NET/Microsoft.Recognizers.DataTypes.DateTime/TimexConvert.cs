// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Recognizers.DataTypes.DateTime
{
    public static class TimexConvert
    {
        public static string ConvertTimexToString(Timex timex)
        {
            return TimexConvertEn.ConvertTimexToString(timex);
        }

        public static string ConvertTimexSetToString(TimexSet timexSet)
        {
            return TimexConvertEn.ConvertTimexSetToString(timexSet);
        }
    }
}
