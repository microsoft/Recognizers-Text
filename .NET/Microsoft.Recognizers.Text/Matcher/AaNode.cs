using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Recognizers.Text.Matcher
{
    public class AaNode<T> : Node<T>
    {
        public AaNode()
        {

        }

        public AaNode(T c, int depth)
        {
            Word = c;
            Depth = depth;
        }

        public AaNode(T c, int depth, AaNode<T> parent)
        {
            Word = c;
            Depth = depth;
            Parent = parent;
        }

        public T Word { get; set; }
        public int Depth { get; set; }
        public AaNode<T> Parent { get; set; }

        public AaNode<T> Fail { get; set; } = null;

        public new AaNode<T> this[T c]
        {
            get { return Children != null && Children.ContainsKey(c) ? Children[c] as AaNode<T> : null; }
            set
            {
                if (Children == null)
                {
                    Children = new Dictionary<T, Node<T>>();
                }

                Children[c] = value;
            }
        }

        public new IEnumerator<AaNode<T>> GetEnumerator()
        {
            return Children?.Values.Select(child => child as AaNode<T>).GetEnumerator();
        }

        public override string ToString()
        {
            return Word.ToString();
        }
    }
}
