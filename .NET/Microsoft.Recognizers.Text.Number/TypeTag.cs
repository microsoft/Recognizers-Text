// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Recognizers.Text.Number
{
    public class TypeTag
    {
        public TypeTag(string name, int priority)
        {
            Name = name;
            Priority = priority;
        }

        public string Name { get; }

        public int Priority { get; }
    }
}
