using System;
using System.Collections.Generic;

namespace Microsoft.Recognizers.Text.NumberWithUnit
{
    public class StringComparer : IComparer<string>
    {
        public int Compare(string stringA, string stringB)
        {
            if (string.IsNullOrEmpty(stringA) && string.IsNullOrEmpty(stringB))
            {
                return 0;
            }
            else
            {
                if (string.IsNullOrEmpty(stringA))
                {
                    return 1;
                }

                if (string.IsNullOrEmpty(stringB))
                {
                    return -1;
                }
            }

            int compareResult = stringB.Length.CompareTo(stringA.Length);

            if (compareResult != 0)
            {
                return compareResult;
            }
            else
            {
                return string.Compare(stringA, stringB, StringComparison.Ordinal);
            }
        }
    }
}