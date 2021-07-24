// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Recognizers.Text.Matcher
{
    public class Node<T>
    {
        public bool End => Values != null && Values.Any();

        public HashSet<string> Values { get; set; } = null;

        // Set Default Children to null to avoid instantiating empty dictionaries
        public Dictionary<T, Node<T>> Children { get; set; } = null;

        public Node<T> this[T c]
        {
            get
            {
                return Children != null && Children.ContainsKey(c) ? Children[c] : null;
            }

            set
            {
                Children ??= new Dictionary<T, Node<T>>();

                Children[c] = value;
            }
        }

        public IEnumerator<Node<T>> GetEnumerator()
        {
            return Children?.Values.GetEnumerator();
        }

        public void AddValue(string value)
        {
            Values ??= new HashSet<string>();

            Values.Add(value);
        }
    }
}
