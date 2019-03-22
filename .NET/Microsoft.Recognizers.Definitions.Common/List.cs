// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;

using YamlDotNet.Serialization;

namespace Microsoft.Recognizers.Definitions.Common
{
    public class List
    {
        [YamlMember(Alias = "types", ApplyNamingConventions = false)]
        public IList<string> Types { get; set; }

        [YamlMember(Alias = "entries", ApplyNamingConventions = false)]
        public IEnumerable<object> Entries { get; set; }
    }
}
