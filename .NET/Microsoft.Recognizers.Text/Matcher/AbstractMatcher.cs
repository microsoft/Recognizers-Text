using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Recognizers.Text.Matcher
{
    public abstract class AbstractMatcher<T> : IMatcher<T>
    {
        public abstract void Init(IEnumerable<T>[] values, string[] ids);

        public abstract IEnumerable<MatchResult<T>> Find(IEnumerable<T> queryText);

        public abstract void Insert(IEnumerable<T> value, string id);

        public bool IsMatch(IEnumerable<T> queryText)
        {
            return Find(queryText).FirstOrDefault() == null;
        }

        protected void BatchInsert(IEnumerable<T>[] values, string[] ids)
        {
            if (values.Length != ids.Length)
            {
                throw new ArgumentException("Lengths of Values and Ids are different.");
            }

            for (int i = 0; i < values.Length; i++)
            {
                Insert(values[i], ids[i]);
            }
        }

        protected void ConvertDictToList(Node<T> node)
        {
            if (node.Values != null)
            {
                node.Values.TrimExcess();
            }

            if (node.Children == null)
            {
                return;
            }

            foreach (var kvp in node.Children)
            {
                ConvertDictToList(kvp.Value);
            }

            // re-malloc dictionary to reduce memory usage
            node.Children = new Dictionary<T, Node<T>>(node.Children);
        }
    }
}
