using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Recognizers.Text.Matcher
{
    public class AcAutomaton<T> : AbstractMatcher<T>
    {
        public AcAutomaton()
        {
        }

        protected AaNode<T> Root { get; private set; } = new AaNode<T>();

        public override void Insert(IEnumerable<T> value, string id)
        {
            var node = Root;
            int i = 0;
            foreach (var item in value)
            {
                var child = node[item];

                if (child == null)
                {
                    child = node[item] = new AaNode<T>(item, i, node);
                }

                node = child;
                i++;
            }

            node.AddValue(id);
        }

        public override void Init(IEnumerable<T>[] values, string[] ids)
        {
            BatchInsert(values, ids);
            var queue = new Queue<AaNode<T>>();
            queue.Enqueue(Root);

            while (queue.Any())
            {
                var node = queue.Dequeue();

                if (node.Children != null)
                {
                    foreach (var child in node)
                    {
                        queue.Enqueue(child);
                    }
                }

                if (node == Root)
                {
                    Root.Fail = Root;
                    continue;
                }

                var fail = node.Parent.Fail;

                while (fail[node.Word] == null && fail != Root)
                {
                    fail = fail.Fail;
                }

                node.Fail = fail[node.Word] ?? Root;
                node.Fail = node.Fail == node ? Root : node.Fail;
            }

            ConvertDictToList(Root);
        }

        public override IEnumerable<MatchResult<T>> Find(IEnumerable<T> queryText)
        {
            var node = Root;
            var i = 0;

            foreach (var c in queryText)
            {
                while (node[c] == null && node != Root)
                {
                    node = node.Fail;
                }

                node = node[c] ?? Root;

                for (var t = node; t != Root; t = t.Fail)
                {
                    if (t.End)
                    {
                        yield return new MatchResult<T>(i - t.Depth, t.Depth + 1, t.Values);
                    }
                }

                i++;
            }
        }
    }
}
