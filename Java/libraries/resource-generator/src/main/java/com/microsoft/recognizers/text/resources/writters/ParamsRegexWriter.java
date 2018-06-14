package com.microsoft.recognizers.text.resources.writters;

import com.microsoft.recognizers.text.resources.datatypes.ParamsRegex;

import java.util.Arrays;

public class ParamsRegexWriter implements ICodeWriter {

    private final String name;
    private ParamsRegex params;

    public ParamsRegexWriter(String name, ParamsRegex params) {
        this.name = name;
        this.params = params;
    }

    @Override
    public String write() {
        String parameters = String.join(", ", Arrays.stream(this.params.params).map(p -> "String " + p).toArray(size -> new String[size]));
        String replace = String.join("", Arrays.stream(this.params.params).map(p -> "\n\t\t\t.replace(\"{" + p + "}\", " + p + ")").toArray(size -> new String[size]));

        String template = String.join(
                "\n    ",
                "    public static String %s(%s) {",
                "    return \"%s\"%s;",
                "}");
        return String.format(template, this.name, parameters, sanitize(this.params.def), replace);
    }
}
