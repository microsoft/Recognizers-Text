using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Recognizers.Text.Matcher
{
    public class TrieTree<T> : AbstractMatcher<T>
    {
        public TrieTree()
        {
        }

        protected Node<T> Root { get; private set; } = new Node<T>();

        public override void Insert(IEnumerable<T> value, string id)
        {
            var node = Root;

            foreach (var item in value)
            {
                var child = node[item];

                if (child == null)
                {
                    child = node[item] = new Node<T>();
                }

                node = child;
            }

            node.AddValue(id);
        }

        public override void Init(IEnumerable<T>[] values, string[] ids)
        {
            BatchInsert(values, ids);
            ConvertDictToList(Root);
        }

        public override IEnumerable<MatchResult<T>> Find(IEnumerable<T> queryText)
        {
            var queryArray = queryText.ToArray();
            for (var i = 0; i < queryArray.Length; i++)
            {
                var node = Root;
                for (var j = i; j <= queryArray.Length; j++)
                {
                    if (node.End)
                    {
                        yield return new MatchResult<T>(i, j - i, node.Values);
                    }

                    if (j == queryArray.Length)
                    {
                        break;
                    }

                    var text = queryArray[j];
                    if (node[text] == null)
                    {
                        break;
                    }

                    node = node[text];
                }
            }
        }
    }
}
