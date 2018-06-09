using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Recognizers.Text.Matcher
{
    public class AcAutomaton<T> : AbstractMatcher<T>
    {
        protected readonly AaNode<T> root = new AaNode<T>();

        public AcAutomaton()
        {

        }

        public override void Insert(IEnumerable<T> value, string id)
        {
            var node = root;
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
            queue.Enqueue(root);

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

                if (node == root)
                {
                    root.Fail = root;
                    continue;
                }

                var fail = node.Parent.Fail;

                while (fail[node.Word] == null && fail != root)
                {
                    fail = fail.Fail;
                }

                node.Fail = fail[node.Word] ?? root;
                node.Fail = node.Fail == node ? root : node.Fail;
            }
        }

        public override IEnumerable<MatchResult<T>> Find(IEnumerable<T> queryText)
        {
            var node = root;
            var i = 0;

            foreach (var c in queryText)
            {
                while (node[c] == null && node != root)
                {
                    node = node.Fail;
                }

                node = node[c] ?? root;

                for (var t = node; t != root; t = t.Fail)
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
