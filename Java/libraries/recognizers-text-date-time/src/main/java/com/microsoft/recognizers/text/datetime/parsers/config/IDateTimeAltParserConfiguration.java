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
