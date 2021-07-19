// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Recognizers.Text.Sequence
{
    public class BaseSequenceConfiguration : ISequenceConfiguration
    {
        public BaseSequenceConfiguration(SequenceOptions options = SequenceOptions.None)
        {
            Options = options;
        }

        public SequenceOptions Options { get; }

    }
}
