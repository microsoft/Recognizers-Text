package com.microsoft.recognizers.text.resources.writters;

public class IntegerWriter implements ICodeWriter {

    private final String name;
    private final int value;

    public IntegerWriter(String name, int value) {

        this.name = name;
        this.value = value;
    }

    @Override
    public String write() {
        return String.format(
                "    public static final int %s = %s;",
                this.name,
                this.value);
    }
}
