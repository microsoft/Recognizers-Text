// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Text.RegularExpressions;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime.Utilities
{
    public static class TasksModeSetHandler
    {
        public static DateTimeResolutionResult TasksModeResolveSet(ref DateTimeResolutionResult result, string innerTimex, DateTimeParseResult pr = null)
        {
            result.Timex = innerTimex;

            result.FutureValue = result.PastValue = "Set: " + innerTimex;

            if (pr != null)
            {
                DateTimeResolutionResult value = (DateTimeResolutionResult)pr.Value;
                if (value.FutureValue != null)
                {
                    if (pr.TimexStr.EndsWith("WE"))
                    {
                        result.FutureValue = ((Tuple<DateObject, DateObject>)value.FutureValue).Item1;
                        result.PastValue = ((Tuple<DateObject, DateObject>)value.PastValue).Item1;
                    }
                }
            }

            result.Success = true;

            return result;
        }
    }
}
