package com.microsoft.recognizers.text.number.parsers;

import com.microsoft.recognizers.text.ExtractResult;
import com.microsoft.recognizers.text.ParseResult;
import com.microsoft.recognizers.text.utilities.Match;
import com.microsoft.recognizers.text.utilities.RegExpUtility;
import com.microsoft.recognizers.text.utilities.StringUtility;

import java.util.Locale;
import java.util.Map;
import java.util.regex.Pattern;

public class BaseCJKNumberParser extends BaseNumberParser {

    protected final ICJKNumberParserConfiguration cjkConfig;

    public BaseCJKNumberParser(INumberParserConfiguration config) {
        super(config);
        this.cjkConfig = (ICJKNumberParserConfiguration)config;
    }

    @Override
    public ParseResult parse(ExtractResult extResult) {

        // check if the parser is configured to support specific types
        if (supportedTypes.isPresent() && !supportedTypes.get().stream().anyMatch(t -> extResult.getType().equals(t))) {
            return null;
        }

        String extra = extResult.getData() instanceof String ? (String)extResult.getData() : null;
        ParseResult ret = null;

        ExtractResult getExtResult = new ExtractResult(extResult.getStart(), extResult.getLength(), extResult.getText(), extResult.getType(), extResult.getData());

        if (config.getCultureInfo().cultureCode.equalsIgnoreCase("zh-CN")) {
            getExtResult.setText(replaceTraWithSim(getExtResult.getText()));
        }

        if (extra == null) {
            return null;
        }

        if (extra.contains("Per")) {
            ret = parsePercentage(getExtResult);
        } else if (extra.contains("Num")) {
            getExtResult.setText(normalizeCharWidth(getExtResult.getText()));
            ret = digitNumberParse(getExtResult);
            if (config.getNegativeNumberSignRegex().matcher(getExtResult.getText()).find() && (double)ret.getValue() > 0) {
                ret.setValue(-(double)ret.getValue());
            }

            ret.setResolutionStr(getResolutionString((double)ret.getValue()));
        } else if (extra.contains("Pow")) {
            getExtResult.setText(normalizeCharWidth(getExtResult.getText()));
            ret = powerNumberParse(getExtResult);
            ret.setResolutionStr(getResolutionString((double)ret.getValue()));
        } else if (extra.contains("Frac")) {
            ret = parseFraction(getExtResult);
        } else if (extra.contains("Dou")) {
            ret = parseDouble(getExtResult);
        } else if (extra.contains("Integer")) {
            ret = parseInteger(getExtResult);
        } else if (extra.contains("Ordinal")) {
            ret = parseOrdinal(getExtResult);
        }

        if (ret != null) {
            ret.setText(extResult.getText().toLowerCase(Locale.ROOT));
        }

        return ret;
    }

    // Parse Fraction phrase.
    protected ParseResult parseFraction(ExtractResult extResult) {
        ParseResult result = new ParseResult(extResult.getStart(), extResult.getLength(), extResult.getText(), extResult.getType(), null, null, null);

        String resultText = extResult.getText();
        String[] splitResult = cjkConfig.getFracSplitRegex().split(resultText);
        String intPart = "";
        String demoPart = "";
        String numPart = "";
        
        if (splitResult.length == 3) {
            intPart = splitResult[0];
            demoPart = splitResult[1];
            numPart = splitResult[2];
        } else {
            intPart = String.valueOf(cjkConfig.getZeroChar());
            demoPart = splitResult[0];
            numPart = splitResult[1];
        }

        Pattern digitNumRegex = cjkConfig.getDigitNumRegex();
        Pattern pointRegex = cjkConfig.getPointRegex();

        double intValue = digitNumRegex.matcher(intPart).find() ?
                getDigitValue(intPart, 1.0) :
                getIntValue(intPart);

        double numValue = digitNumRegex.matcher(numPart).find() ?
                getDigitValue(numPart, 1.0) :
                (pointRegex.matcher(numPart).find() ?
                getIntValue(pointRegex.split(numPart)[0]) + getPointValue(pointRegex.split(numPart)[1]) :
                getIntValue(numPart));

        double demoValue = digitNumRegex.matcher(demoPart).find() ?
                getDigitValue(demoPart, 1.0) :
                getIntValue(demoPart);
                

        if (cjkConfig.getNegativeNumberSignRegex().matcher(intPart).find()) {
            result.setValue(intValue - numValue / demoValue);
        } else {
            result.setValue(intValue + numValue / demoValue);
        }

        result.setResolutionStr(getResolutionString((double)result.getValue()));
        return result;
    }

    // Parse percentage phrase.
    protected ParseResult parsePercentage(ExtractResult extResult) {
        ParseResult result = new ParseResult(extResult.getStart(), extResult.getLength(), extResult.getText(), extResult.getType(), null, null, null);
        Map<Character, Double> zeroToNineMap = cjkConfig.getZeroToNineMap();

        String resultText = extResult.getText();
        long power = 1;

        if (extResult.getData().toString().contains("Spe")) {
            resultText = normalizeCharWidth(resultText);
            resultText = replaceUnit(resultText);

            if (resultText.equals("半額") || resultText.equals("半値") || resultText.equals("半折")) {
                result.setValue(50);
            } else if (resultText.equals("10成") || resultText.equals("10割") || resultText.equals("十割")) {
                result.setValue(100);
            } else {
                Match[] matches = RegExpUtility.getMatches(this.cjkConfig.getSpeGetNumberRegex(), resultText);
                double intNumber;

                if (matches.length == 2) {
                    char intNumberChar = matches[0].value.charAt(0);

                    if (intNumberChar == cjkConfig.getPairChar()) {
                        intNumber = 5;
                    } else if (cjkConfig.getTenChars().contains(intNumberChar)) {
                        intNumber = 10;
                    } else {
                        intNumber = zeroToNineMap.get(intNumberChar);
                    }

                    char pointNumberChar = matches[1].value.charAt(0);
                    double pointNumber;
                    if (pointNumberChar == '半') {
                        pointNumber = 0.5;
                    } else {
                        pointNumber = zeroToNineMap.get(pointNumberChar) * 0.1;
                    }

                    result.setValue((intNumber + pointNumber) * 10);
                } else if (matches.length == 5) {
                    // Deal the Japanese percentage case like "xxx割xxx分xxx厘", get the integer value and convert into result.
                    char intNumberChar = matches[0].value.charAt(0);
                    char pointNumberChar = matches[1].value.charAt(0);
                    char dotNumberChar = matches[3].value.charAt(0);

                    double pointNumber = zeroToNineMap.get(pointNumberChar) * 0.1;
                    double dotNumber = zeroToNineMap.get(dotNumberChar) * 0.01;

                    intNumber = zeroToNineMap.get(intNumberChar);

                    result.setValue((intNumber + pointNumber + dotNumber) * 10);
                } else {
                    char intNumberChar = matches[0].value.charAt(0);

                    if (intNumberChar == cjkConfig.getPairChar()) {
                        intNumber = 5;
                    } else if (cjkConfig.getTenChars().contains(intNumberChar)) {
                        intNumber = 10;
                    } else {
                        intNumber = zeroToNineMap.get(intNumberChar);
                    }

                    result.setValue(intNumber * 10);
                }
            }
        } else if (extResult.getData().toString().contains("Num")) {

            Match[] doubleMatches = RegExpUtility.getMatches(cjkConfig.getPercentageRegex(), resultText);
            String doubleText = doubleMatches[doubleMatches.length - 1].value;

            if (doubleText.contains("k") || doubleText.contains("K") || doubleText.contains("ｋ") ||
                doubleText.contains("Ｋ")) {
                power = 1000;
            }

            if (doubleText.contains("M") || doubleText.contains("Ｍ")) {
                power = 1000000;
            }

            if (doubleText.contains("G") || doubleText.contains("Ｇ")) {
                power = 1000000000;
            }

            if (doubleText.contains("T") || doubleText.contains("Ｔ")) {
                power = 1000000000000L;
            }

            result.setValue(getDigitValue(resultText, power));
        } else {
            Match[] doubleMatches = RegExpUtility.getMatches(cjkConfig.getPercentageRegex(), resultText);
            String doubleText = doubleMatches[doubleMatches.length - 1].value;

            doubleText = replaceUnit(doubleText);

            String[] splitResult = cjkConfig.getPointRegex().split(doubleText);
            if (splitResult[0].equals("")) {
                splitResult[0] = String.valueOf(cjkConfig.getZeroChar());
            }

            double doubleValue = getIntValue(splitResult[0]);
            if (splitResult.length == 2) {
                if (cjkConfig.getNegativeNumberSignRegex().matcher(splitResult[0]).find()) {
                    doubleValue -= getPointValue(splitResult[1]);
                } else {
                    doubleValue += getPointValue(splitResult[1]);
                }
            }

            result.setValue(doubleValue);
        }

        Match[] matches = RegExpUtility.getMatches(this.cjkConfig.getPercentageNumRegex(), resultText);
        if (matches.length > 0) {
            String demoString = matches[0].value;
            String[] splitResult = cjkConfig.getFracSplitRegex().split(demoString);
            String demoPart = splitResult[0];

            Pattern digitNumRegex = cjkConfig.getDigitNumRegex();

            double demoValue = digitNumRegex.matcher(demoPart).find() ?
                    getDigitValue(demoPart, 1.0) :
                    getIntValue(demoPart);
            if (demoValue < 100) {
                result.setValue((double)result.getValue() * (100 / demoValue));
            } else {
                result.setValue((double)result.getValue() / (demoValue / 100));
            }
        }

        if (result.getValue() instanceof Double) {
            result.setResolutionStr(getResolutionString((double)result.getValue()) + "%");
        } else if (result.getValue() instanceof Integer) {
            result.setResolutionStr(getResolutionString((int)result.getValue()) + "%");
        }

        return result;
    }

    // Parse ordinal phrase.
    protected ParseResult parseOrdinal(ExtractResult extResult) {
        ParseResult result = new ParseResult(extResult.getStart(), extResult.getLength(), extResult.getText(), extResult.getType(), null, null, null);

        String resultText = extResult.getText();
        resultText = resultText.substring(1);

        boolean isDigit = cjkConfig.getDigitNumRegex().matcher(resultText).find();
        boolean isRoundInt = cjkConfig.getRoundNumberIntegerRegex().matcher(resultText).find();

        double newValue = isDigit && !isRoundInt ? getDigitValue(resultText, 1) : getIntValue(resultText);

        result.setValue(newValue);
        result.setResolutionStr(getResolutionString(newValue));

        return result;
    }

    // Parse double phrase
    protected ParseResult parseDouble(ExtractResult extResult) {
        ParseResult result = new ParseResult(extResult.getStart(), extResult.getLength(), extResult.getText(), extResult.getType(), null, null, null);
        String resultText = extResult.getText();

        if (cjkConfig.getDoubleAndRoundRegex().matcher(resultText).find()) {
            resultText = replaceUnit(resultText);
            result.setValue(getDigitValue(
                    resultText.substring(0, resultText.length() - 1),
                    cjkConfig.getRoundNumberMapChar().get(resultText.charAt(resultText.length() - 1))));
        } else {
            resultText = replaceUnit(resultText);
            String[] splitResult = cjkConfig.getPointRegex().split(resultText);

            if (splitResult[0].equals("")) {
                splitResult[0] = String.valueOf(cjkConfig.getZeroChar());
            }

            if (cjkConfig.getNegativeNumberSignRegex().matcher(splitResult[0]).find()) {
                result.setValue(getIntValue(splitResult[0]) - getPointValue(splitResult[1]));
            } else {
                result.setValue(getIntValue(splitResult[0]) + getPointValue(splitResult[1]));
            }
        }

        result.setResolutionStr(getResolutionString((double)result.getValue()));
        return result;
    }

    // Parse integer phrase
    protected ParseResult parseInteger(ExtractResult extResult) {
        double value = getIntValue(extResult.getText());
        return new ParseResult(extResult.getStart(), extResult.getLength(), extResult.getText(), extResult.getType(), extResult.getText(), value, getResolutionString(value));
    }

    // Replace traditional Chinese characters with simpilified Chinese ones.
    private String replaceTraWithSim(String text) {
        if (StringUtility.isNullOrWhiteSpace(text)) {
            return text;
        }

        Map<Character, Character> tratoSimMap = cjkConfig.getTratoSimMap();

        StringBuilder builder = new StringBuilder();
        text.chars().mapToObj(i -> (char)i).forEach(c -> {
            builder.append(tratoSimMap.containsKey(c) ? tratoSimMap.get(c) : c);
        });

        return builder.toString();
    }

    // Replace full digtal numbers with half digtal numbers. "４" and "4" are both legal in Japanese, replace "４" with "4", then deal with "4"
    private String normalizeCharWidth(String text) {
        if (StringUtility.isNullOrWhiteSpace(text)) {
            return text;
        }

        Map<Character, Character> fullToHalfMap = cjkConfig.getFullToHalfMap();
        StringBuilder builder = new StringBuilder();
        text.chars().mapToObj(i -> (char)i).forEach(c -> {
            builder.append(fullToHalfMap.containsKey(c) ? fullToHalfMap.get(c) : c);
        });

        return builder.toString();
    }

    // Parse unit phrase. "万", "億",...
    private String replaceUnit(String resultText) {
        for (Map.Entry<String, String> p : cjkConfig.getUnitMap().entrySet()) {
            resultText = resultText.replace(p.getKey(), p.getValue());
        }

        return resultText;
    }

    private double getDigitValue(String intStr, double power) {
        boolean isNegative = false;

        if (cjkConfig.getNegativeNumberSignRegex().matcher(intStr).find()) {
            isNegative = true;
            intStr = intStr.substring(1);
        }

        intStr = normalizeCharWidth(intStr);
        double intValue = getDigitalValue(intStr, power);
        if (isNegative) {
            intValue = -intValue;
        }

        return intValue;
    }

    private double getIntValue(String intStr) {
        Map<Character, Long> roundNumberMapChar = cjkConfig.getRoundNumberMapChar();

        intStr = replaceUnit(intStr);
        double intValue = 0;
        double partValue = 0;
        double beforeValue = 1;

        boolean isRoundBefore = false;
        long roundBefore = -1;
        long roundDefault = 1;
        boolean isNegative = false;
        boolean hasPreviousDigits = false;

        boolean isDozen = false;
        boolean isPair = false;

        if (cjkConfig.getDozenRegex().matcher(intStr).find()) {
            isDozen = true;
            if (cjkConfig.getCultureInfo().cultureCode.equalsIgnoreCase("zh-CN")) {
                intStr = intStr.substring(0, intStr.length() - 1);
            } else if (cjkConfig.getCultureInfo().cultureCode.equalsIgnoreCase("ja-JP")) {
                intStr = intStr.substring(0, intStr.length() - 3);
            }

        } else if (cjkConfig.getPairRegex().matcher(intStr).find()) {
            isPair = true;
            intStr = intStr.substring(0, intStr.length() - 1);
        }

        if (cjkConfig.getNegativeNumberSignRegex().matcher(intStr).find()) {
            isNegative = true;
            intStr = intStr.substring(1);
        }

        for (int i = 0; i < intStr.length(); i++) {
            if (roundNumberMapChar.containsKey(intStr.charAt(i))) {

                Long roundRecent = roundNumberMapChar.get(intStr.charAt(i));
                if (roundBefore != -1 && roundRecent > roundBefore) {
                    if (isRoundBefore) {
                        intValue += partValue * roundRecent;
                        isRoundBefore = false;
                    } else {
                        partValue += beforeValue * roundDefault;
                        intValue += partValue * roundRecent;
                    }
                    roundBefore = -1;
                    partValue = 0;
                } else {
                    isRoundBefore = true;
                    partValue += beforeValue * roundRecent;
                    roundBefore = roundRecent;

                    if (i == intStr.length() - 1 || cjkConfig.getRoundDirectList().contains(intStr.charAt(i))) {
                        intValue += partValue;
                        partValue = 0;
                    }
                }

                roundDefault = roundRecent / 10;
            } else if (cjkConfig.getZeroToNineMap().containsKey(intStr.charAt(i))) {
                if (i != intStr.length() - 1) {
                    boolean isNotRoundNext = cjkConfig.getTenChars().contains(intStr.charAt(i + 1)) || !roundNumberMapChar.containsKey(intStr.charAt(i + 1));
                    if (intStr.charAt(i) == cjkConfig.getZeroChar() && isNotRoundNext) {
                        beforeValue = 1;
                        roundDefault = 1;
                    } else {
                        double currentDigit = cjkConfig.getZeroToNineMap().get(intStr.charAt(i));
                        if (hasPreviousDigits) {
                            beforeValue = beforeValue * 10 + currentDigit;
                        } else {
                            beforeValue = currentDigit;
                        }
                        isRoundBefore = false;
                    }
                } else {
                    if (Character.isDigit(intStr.charAt(i))) {
                        roundDefault = 1;
                    }
                    double currentDigit = cjkConfig.getZeroToNineMap().get(intStr.charAt(i));
                    if (hasPreviousDigits) {
                        beforeValue = beforeValue * 10 + currentDigit;
                    } else {
                        beforeValue = currentDigit;
                    }
                    partValue += beforeValue * roundDefault;
                    intValue += partValue;
                    partValue = 0;
                }
            }
            hasPreviousDigits = Character.isDigit(intStr.charAt(i));
        }

        if (isNegative) {
            intValue = -intValue;
        }

        if (isDozen) {
            intValue = intValue * 12;
        } else if (isPair) {
            intValue = intValue * 2;
        }

        return intValue;
    }

    private double getPointValue(String pointStr) {
        double pointValue = 0;
        double scale = 0.1;

        Map<Character, Double> zeroToNineMap = cjkConfig.getZeroToNineMap();

        for (int i : pointStr.chars().toArray()) {
            char c = (char)i;
            pointValue += scale * zeroToNineMap.get(c);
            scale *= 0.1;
        }

        return pointValue;
    }

    private String getResolutionString(double value) {
        return config.getCultureInfo() != null ?
                NumberFormatUtility.format(value, config.getCultureInfo()) :
                String.valueOf(value);
    }
}
