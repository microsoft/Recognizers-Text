﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;

namespace Microsoft.Recognizers.Text.Sequence
{
    [Flags]
    public enum SequenceOptions
    {
        /// <summary>
        /// None
        /// </summary>
        None = 0,

        /// <summary>
        /// Relaxed. Likely match, don't perform extra validation.
        /// </summary>
        Relaxed = 1,
    }
}
