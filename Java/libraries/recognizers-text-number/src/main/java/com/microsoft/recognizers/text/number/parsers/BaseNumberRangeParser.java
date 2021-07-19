// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.text.number.parsers;

import com.google.common.collect.ImmutableMap;
import com.microsoft.recognizers.text.ExtractResult;
import com.microsoft.recognizers.text.IParser;
import com.microsoft.recognizers.text.ParseResult;
import com.microsoft.recognizers.text.number.NumberRangeConstants;

import java.util.List;
import java.util.regex.Matcher;
import java.util.stream.Collectors;

public class BaseNumberRangeParser implements IParser {

    protected final INumberRangeParserConfiguration config;

    public BaseNumberRangeParser(INumberRangeParserConfiguration config) {
        this.config = config;
    }

    @Override
    public ParseResult parse(ExtractResult extractResult) {

        ParseResult ret = null;

        if (extractResult.getData() != null && !extractResult.getData().toString().isEmpty()) {
            String type = extractResult.getData().toString();
            if (type.contains(NumberRangeConstants.TWONUM)) {
                ret = parseNumberRangeWhichHasTwoNum(extractResult);
            } else {
                ret = parseNumberRangeWhichHasOneNum(extractResult);
            }
        }

        return ret;
    }

    private ParseResult parseNumberRangeWhichHasTwoNum(ExtractResult extractResult) {

        ParseResult result = new ParseResult(extractResult.getStart(), extractResult.getLength(), extractResult.getText(), extractResult.getType(), null, null, null);
        List<ExtractResult> er = config.getNumberExtractor().extract(extractResult.getText());

        // Valid extracted results for this type should have two numbers
        if (er.size() != 2) {
            er = config.getOrdinalExtractor().extract(extractResult.getText());

            if (er.size() != 2) {
                return result;
            }
        }

        List<Double> nums = er.stream().map(r -> {
            Object value = config.getNumberParser().parse(r).getValue();
            return value == null ? 0 : (Double)value;
        }).collect(Collectors.toList());

        double startValue;
        double endValue;
        if (nums.get(0) < nums.get(1)) {
            startValue = nums.get(0);
            endValue = nums.get(1);
        } else {
            startValue = nums.get(1);
            endValue = nums.get(0);
        }

        String startValueStr = config.getCultureInfo() != null ? NumberFormatUtility.format(startValue, config.getCultureInfo()) : String.valueOf(startValue);
        String endValueStr = config.getCultureInfo() != null ? NumberFormatUtility.format(endValue, config.getCultureInfo()) : String.valueOf(endValue);

        char leftBracket;
        char rightBracket;

        String type = (String)extractResult.getData();
        if (type.contains(NumberRangeConstants.TWONUMBETWEEN)) {
            // between 20 and 30: (20,30)
            leftBracket = NumberRangeConstants.LEFT_OPEN;
            rightBracket = NumberRangeConstants.RIGHT_OPEN;
        } else if (type.contains(NumberRangeConstants.TWONUMTILL)) {
            // 20~30: [20,30)
            leftBracket = NumberRangeConstants.LEFT_CLOSED;
            rightBracket = NumberRangeConstants.RIGHT_OPEN;
        } else {
            // check whether it contains string like "more or equal", "less or equal", "at least", etc.
            Matcher match = config.getMoreOrEqual().matcher(extractResult.getText());
            boolean matches = match.find();
            if (!matches) {
                match = config.getMoreOrEqualSuffix().matcher(extractResult.getText());
                matches = match.find();
            }

            if (matches) {
                leftBracket = NumberRangeConstants.LEFT_CLOSED;
            } else {
                leftBracket = NumberRangeConstants.LEFT_OPEN;
            }

            match = config.getLessOrEqual().matcher(extractResult.getText());
            matches = match.find();

            if (!matches) {
                match = config.getLessOrEqualSuffix().matcher(extractResult.getText());
                matches = match.find();
            }

            if (matches) {
                rightBracket = NumberRangeConstants.RIGHT_CLOSED;
            } else {
                rightBracket = NumberRangeConstants.RIGHT_OPEN;
            }
        }

        result.setValue(ImmutableMap.of(
                        "StartValue", startValue,
                        "EndValue", endValue));
        result.setResolutionStr(new StringBuilder()
                        .append(leftBracket)
                        .append(startValueStr)
                        .append(NumberRangeConstants.INTERVAL_SEPARATOR)
                        .append(endValueStr)
                        .append(rightBracket).toString());

        return result;
    }

    private ParseResult parseNumberRangeWhichHasOneNum(ExtractResult extractResult) {

        ParseResult result = new ParseResult(extractResult.getStart(), extractResult.getLength(), extractResult.getText(), extractResult.getType(), null, null, null);

        List<ExtractResult> er = config.getNumberExtractor().extract(extractResult.getText());

        // Valid extracted results for this type should have one number
        if (er.size() != 1) {
            er = config.getOrdinalExtractor().extract(extractResult.getText());

            if (er.size() != 1) {
                return result;
            }
        }

        List<Double> nums = er.stream().map(r -> {
            Object value = config.getNumberParser().parse(r).getValue();
            return value == null ? 0 : (Double)value;
        }).collect(Collectors.toList());

        char leftBracket;
        char rightBracket;
        String startValueStr = "";
        String endValueStr = "";

        String type = (String)extractResult.getData();
        if (type.contains(NumberRangeConstants.MORE)) {
            rightBracket = NumberRangeConstants.RIGHT_OPEN;

            Matcher match = config.getMoreOrEqual().matcher(extractResult.getText());
            boolean matches = match.find();

            if (!matches) {
                match = config.getMoreOrEqualSuffix().matcher(extractResult.getText());
                matches = match.find();
            }

            if (!matches) {
                match = config.getMoreOrEqualSeparate().matcher(extractResult.getText());
                matches = match.find();
            }

            if (matches) {
                leftBracket = NumberRangeConstants.LEFT_CLOSED;
            } else {
                leftBracket = NumberRangeConstants.LEFT_OPEN;
            }

            startValueStr = config.getCultureInfo() != null ? NumberFormatUtility.format(nums.get(0), config.getCultureInfo()) : nums.get(0).toString();

            result.setValue(ImmutableMap.of("StartValue", nums.get(0)));
        } else if (type.contains(NumberRangeConstants.LESS)) {
            leftBracket = NumberRangeConstants.LEFT_OPEN;

            Matcher match = config.getLessOrEqual().matcher(extractResult.getText());
            boolean matches = match.find();

            if (!matches) {
                match = config.getLessOrEqualSuffix().matcher(extractResult.getText());
                matches = match.find();
            }

            if (!matches) {
                match = config.getLessOrEqualSeparate().matcher(extractResult.getText());
                matches = match.find();
            }

            if (matches) {
                rightBracket = NumberRangeConstants.RIGHT_CLOSED;
            } else {
                rightBracket = NumberRangeConstants.RIGHT_OPEN;
            }

            endValueStr = config.getCultureInfo() != null ? NumberFormatUtility.format(nums.get(0), config.getCultureInfo()) : nums.get(0).toString();

            result.setValue(ImmutableMap.of("EndValue", nums.get(0)));
        } else {
            leftBracket = NumberRangeConstants.LEFT_CLOSED;
            rightBracket = NumberRangeConstants.RIGHT_CLOSED;

            startValueStr = config.getCultureInfo() != null ? NumberFormatUtility.format(nums.get(0), config.getCultureInfo()) : nums.get(0).toString();
            endValueStr = startValueStr;

            result.setValue(ImmutableMap.of(
                    "StartValue", nums.get(0),
                    "EndValue", nums.get(0)
            ));
        }

        result.setResolutionStr(new StringBuilder()
                .append(leftBracket)
                .append(startValueStr)
                .append(NumberRangeConstants.INTERVAL_SEPARATOR)
                .append(endValueStr)
                .append(rightBracket)
                .toString());

        return result;
    }
}
