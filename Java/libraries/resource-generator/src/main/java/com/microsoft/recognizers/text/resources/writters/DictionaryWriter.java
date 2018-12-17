package com.microsoft.recognizers.text.resources.writters;

import com.microsoft.recognizers.text.resources.datatypes.Dictionary;
import com.microsoft.recognizers.text.resources.datatypes.List;

import java.util.ArrayList;

public class DictionaryWriter implements ICodeWriter {

    private final String name;
    private final Dictionary def;

    public DictionaryWriter(String name, Dictionary def) {
        this.name = name;
        this.def = def;
    }

    @Override
    public String write() {

        String keyType = toJavaType(this.def.types[0]);
        String valueType = toJavaType(this.def.types[1]);

        String keyQuote;
        if(keyType.equals("Long") || keyType.equals("Double")) {
            keyQuote = "";
        } else if(keyType.equals("Character")) {
            keyQuote = "'";
        } else {
            keyQuote = "\"";
        }

        String valueQuote1;
        String valueQuote2;
        String prefix;
        boolean hasList = false;
        if (valueType.endsWith("[]")) {
            hasList = true;
            valueQuote1 = "{";
            valueQuote2 = "}";
        } else if(valueType.equals("Integer") || valueType.equals("Long") || valueType.equals("Double")) {
            valueQuote1 = valueQuote2 = "";
        } else if(valueType.equals("Character")) {
            valueQuote1 = valueQuote2 = "'";
        } else {
            valueQuote1 = valueQuote2 = "\"";
        }

        if (hasList) {
            prefix = String.format("new %s", valueType);
        } else {
            prefix = "";
        }

        String[] entries = this.def.entries.entrySet().stream()
                .map(kv -> String.format("\n        .put(%s%s%s, %s%s%s%s)", keyQuote, sanitize(kv.getKey(), keyType), keyQuote, prefix, valueQuote1, getEntryValue(kv.getValue(), valueType), valueQuote2))
                .toArray(size -> new String[size]);

        return String.format(
                "    public static final ImmutableMap<%s, %s> %s = ImmutableMap.<%s, %s>builder()%s\n        .build();",
                keyType,
                valueType,
                this.name,
                keyType,
                valueType,
                String.join("", entries));
    }

    private String getEntryValue(Object value, String valueType) {
        if (value instanceof ArrayList) {
            return String.join(", ", (String[])((ArrayList) value).stream().map(o -> String.format("\"%s\"", sanitize(o.toString(), valueType))).toArray(size -> new String[size]));
        }
        return  sanitize(value.toString(), valueType);
    }

    private String toJavaType(String type) {
        switch (type) {
            case "string":
                return "String";
            case "char":
                return "Character";
            case "long":
                return "Long";
            case "int":
                return "Integer";
            case "double":
                return "Double";
            case "string[]":
                return  "String[]";
            default:
                throw new IllegalArgumentException("Type '" + type + "' is not supported.");
        }
    }
}
