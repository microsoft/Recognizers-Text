// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Recognizers.DataTypes.DateTime
{
    public static class TimexRelativeConvert
    {
        public static string ConvertTimexToStringRelative(Timex timex, System.DateTime referenceDate)
        {
            return TimexRelativeConvertEn.ConvertTimexToStringRelative(timex, referenceDate);
        }
    }
}
