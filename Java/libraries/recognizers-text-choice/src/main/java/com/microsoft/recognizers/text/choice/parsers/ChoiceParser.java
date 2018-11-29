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
        ChoiceExtractDataResult data = (ChoiceExtractDataResult)extractResult.data;
        Map<String, Boolean> resolutions = this.config.getResolutions();
        List<OptionsOtherMatchParseResult> matches = data.otherMatches.stream().map(match -> getOptionsOtherMatchResult(match)).collect(Collectors.toList());

        parseResult = parseResult.withData(new OptionsParseDataResult(data.score, matches));
        parseResult = parseResult.withValue(resolutions.getOrDefault(parseResult.type, false));

        return parseResult;
    }

    private OptionsOtherMatchParseResult getOptionsOtherMatchResult(ExtractResult extractResult) {

        ParseResult parseResult = new ParseResult(extractResult);
        ChoiceExtractDataResult data = (ChoiceExtractDataResult)extractResult.data;
        Map<String, Boolean> resolutions = this.config.getResolutions();
        OptionsOtherMatchParseResult result = new OptionsOtherMatchParseResult(parseResult.text, resolutions.get(parseResult.type), data.score);

        return result;
    }
}