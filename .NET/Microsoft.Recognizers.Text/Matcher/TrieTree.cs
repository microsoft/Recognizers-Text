using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Recognizers.Text.Matcher
{
    public class TrieTree<T> : AbstractMatcher<T>
    {
        protected readonly Node<T> root = new Node<T>();

        public TrieTree()
        {

        }

        public override void Insert(IEnumerable<T> value, string id)
        {
            var node = root;

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
        }

        public override IEnumerable<MatchResult<T>> Find(IEnumerable<T> queryText)
        {
            var queryArr = queryText.ToArray();
            for (var i = 0; i < queryArr.Length; i++)
            {
                var node = root;
                for (var j = i; j <= queryArr.Length; j++)
                {
                    if (node.End)
                    {
                        yield return new MatchResult<T>(i, j - i, node.Values);
                    }

                    if (j == queryArr.Length)
                    {
                        break;
                    }

                    var text = queryArr[j];
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
