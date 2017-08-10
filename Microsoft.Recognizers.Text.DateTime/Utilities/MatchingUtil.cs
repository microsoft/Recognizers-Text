using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Recognizers.Text.DateTime
{
    public class MatchingUtil
    {
        public static bool GetAgoLaterIndex(string text, List<string> stringList, out int index)
        {
            index = -1;

            foreach (var matchString in stringList)
            {
                if (text.TrimStart().ToLower().StartsWith(matchString))
                {
                    index = text.ToLower().LastIndexOf(matchString, StringComparison.Ordinal) + matchString.Length;
                    return true;
                }
            }
            return false;
        }

        public static bool GetInIndex(string text, List<string> stringList, out int index)
        {
            index = -1;

            foreach (var matchString in stringList)
            {
                if (text.Trim().ToLower().Split(' ').Last().EndsWith(matchString))
                {
                    index = text.Length - text.ToLower().LastIndexOf(matchString, StringComparison.Ordinal);
                    return true;
                }
            }
            return false;
        }

        public static bool ContainsAgoLaterIndex(string text, List<string> stringList)
        {
            int index = -1;
            return GetAgoLaterIndex(text, stringList, out index);
        }

        public static bool ContainsInIndex(string text, List<string> stringList)
        {
            int index = -1;
            return GetInIndex(text, stringList, out index);
        }
    }
}