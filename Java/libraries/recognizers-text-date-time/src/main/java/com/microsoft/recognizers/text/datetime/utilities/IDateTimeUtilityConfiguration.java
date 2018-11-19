package com.microsoft.recognizers.text.datetime.utilities;

import java.util.regex.Pattern;

public interface IDateTimeUtilityConfiguration {

    Pattern getAgoRegex();

    Pattern getLaterRegex();

    Pattern getInConnectorRegex();

    Pattern getWithinNextPrefixRegex();

    Pattern getRangeUnitRegex();

    Pattern getTimeUnitRegex();

    Pattern getDateUnitRegex();

    Pattern getAmDescRegex();

    Pattern getPmDescRegex();

    Pattern getAmPmDescRegex();

    Pattern getCommonDatePrefixRegex();
}
