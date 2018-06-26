package com.microsoft.recognizers.text.numberwithunit;

public class Constants {
    public static final String SYS_UNIT_DIMENSION = "builtin.unit.dimension";
    public static final String SYS_UNIT = "builtin.unit";
    public static final String SYS_UNIT_AGE = "builtin.unit.age";
    public static final String SYS_UNIT_AREA = "builtin.unit.area";
    public static final String SYS_UNIT_CURRENCY = "builtin.unit.currency";
    public static final String SYS_UNIT_LENGTH = "builtin.unit.length";
    public static final String SYS_UNIT_SPEED = "builtin.unit.speed";
    public static final String SYS_UNIT_TEMPERATURE = "builtin.unit.temperature";
    public static final String SYS_UNIT_VOLUME = "builtin.unit.volume";
    public static final String SYS_UNIT_WEIGHT = "builtin.unit.weight";
    public static final String SYS_NUM = "builtin.num";

    // For currencies without ISO codes, we use internal values prefixed by '_'. 
    // These values should never be present in parse output.
    public static final String FAKE_ISO_CODE_PREFIX = "_";
}
