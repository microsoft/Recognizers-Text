using System.Collections.Generic;

namespace Microsoft.Recognizers.Text.Matcher
{
    public interface IMatcher<T>
    {
        // Id could be a canonical value or a unique id
        void Init(IEnumerable<T>[] values, string[] ids);

        IEnumerable<MatchResult<T>> Find(IEnumerable<T> queryText);
    }
}
