package com.microsoft.recognizers.text.choice.parsers;

import com.microsoft.recognizers.text.choice.config.BooleanParserConfiguration;

public class BooleanParser extends ChoiceParser<Boolean> {
    public BooleanParser() {
        super(new BooleanParserConfiguration());
    }
}