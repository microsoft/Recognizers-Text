using System.Collections.Generic;

namespace Microsoft.Recognizers.Text.Matcher
{
    public class MatchResult: MatchResult<string>
    {
        public string Text { get; set; }
    }

    public class MatchResult<T>
    {
        public int Start { get; set; }
        public int Length { get; set; }
        public int End { get { return Start + Length; } }

        public HashSet<string> Values { get; set; }

        public MatchResult()
        {

        }

        public MatchResult(int start, int length)
        {
            Start = start;
            Length = length;
        }
    }
}
