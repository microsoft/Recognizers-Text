// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Recognizers.Text.DateTime
{
    public interface IDateTimeOptionsConfiguration : IConfiguration
    {
        DateTimeOptions Options { get; }

        bool DmyDateFormat { get; }

        string LanguageMarker { get; }
    }
}
