// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Recognizers.Text.DataTypes.TimexExpression
{
    public static class TimexRelativeConvert
    {
        public static string ConvertTimexToStringRelative(TimexProperty timex, System.DateTime referenceDate)
        {
            return TimexRelativeConvertEnglish.ConvertTimexToStringRelative(timex, referenceDate);
        }
    }
}
