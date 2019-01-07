package com.microsoft.recognizers.text.datetime;

public enum DateTimeOptions {
    None(0),
    SkipFromToMerge(1),
    SplitDateAndTime(2),
    CalendarMode(4),
    ExtendedTypes(8),
    EnablePreview(8388608),
    ExperimentalMode(4194304),
    ComplexCalendar(8 + 4 + 8388608);

    private final int value;

    DateTimeOptions(int value) {
        this.value = value;
    }

    public int getValue() {
        return value;
    }

    public boolean match(DateTimeOptions option) {
        return (this.value & option.value) == option.value;
    }
}
