package com.microsoft.recognizers.text.resources.writters;

public class CharacterWritter  implements ICodeWriter {

    private final String name;
    private final char value;

    public CharacterWritter(String name, char value) {
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
