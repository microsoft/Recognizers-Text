package com.microsoft.recognizers.text.number;

public enum NumberMode {
    //Default is for unit and datetime
    Default,
    //Add 67.5 billion & million support.
    Currency,
    //Don't extract number from cases like 16ml
    PureNumber
}
