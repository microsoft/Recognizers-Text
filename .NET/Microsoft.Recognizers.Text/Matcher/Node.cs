using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Recognizers.Text.Matcher
{
    public class Node: Node<string>
    {

    }

    public class Node<T>
    {
        public Node()
        {

        }

        public Node(T c, int depth)
        {
            Word = c;
            Depth = depth;
        }

        public Node(T c, int depth, Node<T> parent)
        {
            Word = c;
            Depth = depth;
            Parent = parent;
        }

        public T Word { get; set; }
        public int Depth { get; set; }
        public Node<T> Parent { get; set; }

        // Set Default Children to null to avoid instantiating empty dictionaries
        public Dictionary<T, Node<T>> Children { get; set; } = null;
        public Node<T> Fail { get; set; } = null;
        public HashSet<string> Values { get; set; } = null;
        public bool End { get { return Values != null && Values.Any(); } }

        public Node<T> this[T c]
        {
            get { return Children != null && Children.ContainsKey(c) ? Children[c] : null; }
            set
            {
                if (Children == null)
                {
                    Children = new Dictionary<T, Node<T>>();
                }

                Children[c] = value;
            }
        }

        public void AddValue(string value)
        {
            if (Values == null)
            {
                Values = new HashSet<string>();
            }

            Values.Add(value);
        }

        public IEnumerator<Node<T>> GetEnumerator()
        {
            return Children?.Values.GetEnumerator();
        }

        public override string ToString()
        {
            return Word.ToString();
        }
    }
}
