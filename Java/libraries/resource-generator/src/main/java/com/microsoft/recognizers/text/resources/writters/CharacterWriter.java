package com.microsoft.recognizers.text.resources.writters;

public class CharacterWriter implements ICodeWriter {

    private final String name;
    private final char value;

    public CharacterWriter(String name, char value) {
        this.name = name;
        this.value = value;
    }

    @Override
    public String write() {
        return String.format(
                "    public static final Character %s = \'%s\';",
                this.name,
                sanitize(String.valueOf(this.value)));
    }
}
