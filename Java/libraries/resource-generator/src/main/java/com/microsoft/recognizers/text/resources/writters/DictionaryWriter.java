package com.microsoft.recognizers.text.resources.writters;

import com.microsoft.recognizers.text.resources.datatypes.Dictionary;

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
        if (valueType.endsWith("[]")) {
            valueQuote1 = "[";
            valueQuote2 = "]";
        } else if(valueType.equals("Long") || valueType.equals("Double")) {
            valueQuote1 = valueQuote2 = "";
        } else if(valueType.equals("Character")) {
            valueQuote1 = valueQuote2 = "'";
        } else {
            valueQuote1 = valueQuote2 = "\"";
        }

        String[] entries = this.def.entries.entrySet().stream()
                .map(kv -> String.format("\n        .put(%s%s%s, %s%s%s)", keyQuote, kv.getKey(), keyQuote, valueQuote1, sanitize(kv.getValue().toString(), valueType), valueQuote2))
                .toArray(size -> new String[size]);

        return String.format(
                "    public static final Map<%s, %s> %s = ImmutableMap.<%s, %s>builder()%s\n        .build();",
                keyType,
                valueType,
                this.name,
                keyType,
                valueType,
                String.join("", entries));
    }

    private String toJavaType(String type) {
        switch (type) {
            case "string":
                return "String";
            case "char":
                return "Character";
            case "long":
            case "int":
                return "Long";
            case "double":
                return "Double";
            default:
                throw new IllegalArgumentException("Type '" + type + "' is not supported.");
        }
    }
}
