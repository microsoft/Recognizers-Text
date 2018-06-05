package com.microsoft.recognizers.text.number.parsers;

import com.microsoft.recognizers.text.ExtractResult;
import com.microsoft.recognizers.text.IParser;
import com.microsoft.recognizers.text.ParseResult;
import org.omg.CORBA.SystemException;

import java.util.*;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

public class BaseNumberParser implements IParser {

    protected final INumberParserConfiguration config;

    protected final Pattern textNumberRegex;

    protected final Pattern longFormatRegex;

    protected final Set<String> roundNumberSet;

    protected List<String> supportedTypes = Collections.emptyList();

    public void setSupportedTypes(List<String> types) {
        this.supportedTypes = types;
    }

    public BaseNumberParser(INumberParserConfiguration config) {
        this.config = config;

        String singleIntFrac = config.getWordSeparatorToken() + "| -|"
                + getKeyRegex(config.getCardinalNumberMap().keySet()) + "|"
                + getKeyRegex(config.getOrdinalNumberMap().keySet());

        // Necessary for the german language because bigger numbers are not separated by whitespaces or special characters like in other languages
        if (config.getCultureInfo().cultureCode.equalsIgnoreCase("de-DE")) {
            this.textNumberRegex = Pattern.compile("(" + singleIntFrac + ")", Pattern.CASE_INSENSITIVE);
        } else {
            this.textNumberRegex = Pattern.compile("(?<=\\b)(" + singleIntFrac + ")(?=\\b)", Pattern.CASE_INSENSITIVE);
        }

        this.longFormatRegex = Pattern.compile("\\d+", Pattern.CASE_INSENSITIVE);

        this.roundNumberSet = new HashSet<>(config.getRoundNumberMap().keySet());
    }

    @Override
    public ParseResult parse(ExtractResult extractResult) {
        // check if the parser is configured to support specific types
        if (!this.supportedTypes.contains(extractResult.type)) {
            return null;
        }

        String extra;
        ParseResult ret = null;

        if (extractResult.data instanceof String) {
            extra = (String) extractResult.data;
        } else {
            if (this.longFormatRegex.matcher(extractResult.text).matches()) {
                extra = "Num";
            } else {
                extra = config.getLangMarker();
            }
        }

        // Resolve symbol prefix
        boolean isNegative = false;
        Matcher matchNegative = config.getNegativeNumberSignRegex().matcher(extractResult.text);
        if (matchNegative.matches()) {
            isNegative = true;
            extractResult = new ExtractResult(
                    extractResult.start,
                    extractResult.length,
                    extractResult.text.substring(matchNegative.group(1).length()),
                    extractResult.type,
                    extractResult.data);
        }

        if (extra.contains("Num")) {
            ret = digitNumberParse(extractResult);
        } else if (extra.contains("Frac" + config.getLangMarker())) {
            // Frac is a special number, parse via another method
            ret = fracLikeNumberParse(extractResult);
        } else if (extra.contains(config.getLangMarker())) {
            ret = textNumberParse(extractResult);
        } else if (extra.contains("Pow")) {
            ret = powerNumberParse(extractResult);
        }

        if (ret != null && ret.value != null) {
            if (isNegative) {
                // Recover to the original extracted Text
                ret = new ParseResult(
                        ret.start,
                        ret.length,
                        matchNegative.group(1) + extractResult.text,
                        ret.type,
                        ret.data,
                        -(double) ret.value,
                        ret.resolutionStr);
            }
        }

        return ret;
    }

    private ParseResult digitNumberParse(ExtractResult extractResult) {
        return null;
    }

    private ParseResult fracLikeNumberParse(ExtractResult extractResult) {
        return null;
    }

    private ParseResult textNumberParse(ExtractResult extractResult) {
        return null;
    }

    private ParseResult powerNumberParse(ExtractResult extractResult) {
        return null;
    }

    protected String getKeyRegex(Set<String> keyCollection) {
        ArrayList<String> keys = new ArrayList<>(keyCollection);
        Collections.sort(keys, Collections.reverseOrder());

        return String.join("|", keys);
    }
}
