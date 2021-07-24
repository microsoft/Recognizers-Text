// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Recognizers.Text.Sequence.English
{
    public class EmailParser : BaseSequenceParser
    {

        private BaseSequenceConfiguration config;

        public EmailParser(BaseSequenceConfiguration config)
        {
            this.config = config;
        }
    }
}
