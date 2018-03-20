// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Recognizers.Text.DataTypes.TimexExpression
{
    public static class TimexConvert
    {
        public static string ConvertTimexToString(TimexProperties timex)
        {
            return TimexConvertEnglish.ConvertTimexToString(timex);
        }

        public static string ConvertTimexSetToString(TimexSet timexSet)
        {
            return TimexConvertEnglish.ConvertTimexSetToString(timexSet);
        }
    }
}
