// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.text.datetime.parsers.config;

import com.microsoft.recognizers.text.datetime.parsers.IDateTimeParser;

public interface IDateTimeAltParserConfiguration {
    IDateTimeParser getDateTimeParser();
    
    IDateTimeParser getDateParser();
    
    IDateTimeParser getTimeParser();
    
    IDateTimeParser getDateTimePeriodParser();
    
    IDateTimeParser getTimePeriodParser();
    
    IDateTimeParser getDatePeriodParser();
}
