using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Recognizers.Text.Matcher
{
    public class AcAutomaton<T>
    {
        protected readonly Node<T> root = new Node<T>();

        public AcAutomaton()
        {

        }

        public AcAutomaton(IEnumerable<T>[] values)
        {
            BatchInsert(values, values.Select(value => value.ToString()).ToArray());
            Build();
        }
        
        // Id could be a canonical value or a unique id
        public AcAutomaton(IEnumerable<T>[] values, string[] ids)
        {
            BatchInsert(values, ids);
            Build();
        }

        public void Insert(IEnumerable<T> value, string id)
        {
            var node = root;
            for (int i = 0; i < value.Count(); i++)
            {
                var item = value.ElementAt(i);
                var child = node[item];

                if (child == null)
                {
                    child = node[item] = new Node<T>(item, i, node);
                }

                node = child;
            }

            node.AddValue(id);
        }

        public void BatchInsert(IEnumerable<T>[] values, string[] ids)
        {
            if (values.Length != ids.Length || values.Length == 0)
            {
                return;
            }

            for (int i = 0; i < values.Length; i++)
            {
                Insert(values[i], ids[i]);
            }
        }

        public virtual void Build()
        {
            var queue = new Queue<Node<T>>();
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

        public virtual List<MatchResult<T>> Find(IEnumerable<T> queryText)
        {
            var node = root;
            var rets = new List<MatchResult<T>>();
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
                        rets.Add(new MatchResult<T>(i - t.Depth, t.Depth + 1)
                        {
                            Values = t.Values
                        });
                    }
                }

                i++;
            }

            return rets;
        }
    }
}
