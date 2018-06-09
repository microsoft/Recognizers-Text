using System.Collections.Generic;

namespace Microsoft.Recognizers.Text.Matcher
{
    public class MatchResult<T>
    {
        public int Start { get; set; }
        public int Length { get; set; }
        public int End { get { return Start + Length; } }

        public T Text { get; set; }

        public HashSet<string> CanonicalValues { get; set; } = new HashSet<string>();

        public MatchResult()
        {

        }

        public MatchResult(int start, int length)
        {
            Start = start;
            Length = length;
        }

        public MatchResult(int start, int length, HashSet<string> ids)
        {
            Start = start;
            Length = length;
            CanonicalValues = ids;
        }
    }
}
