// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

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
