// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using YamlDotNet.Serialization;

namespace Microsoft.Recognizers.Definitions.Common
{
    public class SimpleRegex
    {
        [YamlMember(Alias = "def", ApplyNamingConventions = false)]
        public string Definition { get; set; }

        [YamlMember(Alias = "def_js", ApplyNamingConventions = false)]
        public string DefinitionJS { get; set; }
    }
}
