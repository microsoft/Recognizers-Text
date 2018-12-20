package com.microsoft.recognizers.text.choice.parsers;

import com.microsoft.recognizers.text.ExtractResult;
import com.microsoft.recognizers.text.IParser;
import com.microsoft.recognizers.text.ParseResult;
import com.microsoft.recognizers.text.choice.config.IChoiceParserConfiguration;
import com.microsoft.recognizers.text.choice.extractors.ChoiceExtractDataResult;

import java.util.List;
import java.util.Map;
import java.util.stream.Collectors;

public class ChoiceParser<T> implements IParser {

    private IChoiceParserConfiguration<T> config;

    public ChoiceParser(IChoiceParserConfiguration<T> config) {
        this.config = config;
    }

    public ParseResult parse(ExtractResult extractResult) {

        ParseResult parseResult = new ParseResult(extractResult);
        ChoiceExtractDataResult data = (ChoiceExtractDataResult)extractResult.getData();
        Map<String, Boolean> resolutions = this.config.getResolutions();
        List<OptionsOtherMatchParseResult> matches = data.otherMatches.stream().map(match -> getOptionsOtherMatchResult(match)).collect(Collectors.toList());

        parseResult.setData(new OptionsParseDataResult(data.score, matches));
        parseResult.setValue(resolutions.getOrDefault(parseResult.getType(), false));

        return parseResult;
    }

    private OptionsOtherMatchParseResult getOptionsOtherMatchResult(ExtractResult extractResult) {

        ParseResult parseResult = new ParseResult(extractResult);
        ChoiceExtractDataResult data = (ChoiceExtractDataResult)extractResult.getData();
        Map<String, Boolean> resolutions = this.config.getResolutions();
        OptionsOtherMatchParseResult result = new OptionsOtherMatchParseResult(parseResult.getText(), resolutions.get(parseResult.getType()), data.score);

        return result;
    }
}