package com.microsoft.recognizers.text.number.parsers;

import com.microsoft.recognizers.text.CultureInfo;
import com.microsoft.recognizers.text.ExtractResult;
import com.microsoft.recognizers.text.IParser;
import com.microsoft.recognizers.text.ParseResult;
import com.microsoft.recognizers.text.number.Constants;
import com.microsoft.recognizers.text.utilities.QueryProcessor;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.Collections;
import java.util.HashSet;
import java.util.List;
import java.util.Locale;
import java.util.Optional;
import java.util.Set;
import java.util.Stack;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

public class BaseNumberParser implements IParser {

    protected final INumberParserConfiguration config;

    protected final Pattern textNumberRegex;

    protected final Pattern longFormatRegex;

    protected final Set<String> roundNumberSet;

    protected Optional<List<String>> supportedTypes = Optional.empty();

    public void setSupportedTypes(List<String> types) {
        this.supportedTypes = Optional.of(types);
    }

    public BaseNumberParser(INumberParserConfiguration config) {
        this.config = config;

        String singleIntFrac = config.getWordSeparatorToken() + "| -|"
                + getKeyRegex(config.getCardinalNumberMap().keySet()) + "|"
                + getKeyRegex(config.getOrdinalNumberMap().keySet());

        // Necessary for the german language because bigger numbers are not separated by whitespaces or special characters like in other languages
        if (config.getCultureInfo().cultureCode.equalsIgnoreCase("de-DE")) {
            this.textNumberRegex = Pattern.compile("(" + singleIntFrac + ")", Pattern.CASE_INSENSITIVE | Pattern.UNICODE_CHARACTER_CLASS);
        } else {
            this.textNumberRegex = Pattern.compile("(?<=\\b)(" + singleIntFrac + ")(?=\\b)", Pattern.CASE_INSENSITIVE | Pattern.UNICODE_CHARACTER_CLASS);
        }

        this.longFormatRegex = Pattern.compile("\\d+", Pattern.CASE_INSENSITIVE | Pattern.UNICODE_CHARACTER_CLASS);

        this.roundNumberSet = new HashSet<>(config.getRoundNumberMap().keySet());
    }

    @Override
    public ParseResult parse(ExtractResult extractResult) {
        // check if the parser is configured to support specific types
        if (supportedTypes.isPresent() && !this.supportedTypes.get().contains(extractResult.getType())) {
            return null;
        }

        String extra;
        ParseResult ret = null;

        if (extractResult.getData() instanceof String) {
            extra = (String)extractResult.getData();
        } else {
            if (this.longFormatRegex.matcher(extractResult.getText()).find()) {
                extra = "Num";
            } else {
                extra = config.getLangMarker();
            }
        }

        // Resolve symbol prefix
        boolean isNegative = false;
        Matcher matchNegative = config.getNegativeNumberSignRegex().matcher(extractResult.getText());
        if (matchNegative.find()) {
            isNegative = true;
            extractResult = new ExtractResult(
                    extractResult.getStart(),
                    extractResult.getLength(),
                    extractResult.getText().substring(matchNegative.group(1).length()),
                    extractResult.getType(),
                    extractResult.getData(),
                    extractResult.getMetadata());
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

        if (ret != null && ret.getValue() != null) {
            if (isNegative) {
                // Recover to the original extracted Text
                ret = new ParseResult(
                        ret.getStart(),
                        ret.getLength(),
                        matchNegative.group(1) + extractResult.getText(),
                        ret.getType(),
                        ret.getData(),
                        -(double)ret.getValue(),
                        ret.getResolutionStr());
            }

            String resolutionStr = config.getCultureInfo() != null ? NumberFormatUtility.format(ret.getValue(), config.getCultureInfo()) : ret.getValue().toString();
            ret.setResolutionStr(resolutionStr);
        }

        if (ret != null) {
            ret.setText(ret.getText().toLowerCase(Locale.ROOT));
        }

        return ret;
    }

    /**
     * Precondition: ExtResult must have arabic numerals.
     *
     * @param extractResult input arabic number
     * @return
     */
    protected ParseResult digitNumberParse(ExtractResult extractResult) {

        ParseResult result = new ParseResult(extractResult.getStart(), extractResult.getLength(), extractResult.getText(),
                extractResult.getType(),null,null,null);

        //[1] 24
        //[2] 12 32/33
        //[3] 1,000,000
        //[4] 234.567
        //[5] 44/55
        //[6] 2 hundred
        //dot occurred.
        double power = 1;
        String handle = extractResult.getText().toLowerCase();
        Matcher match = config.getDigitalNumberRegex().matcher(handle);
        int startIndex = 0;
        while (match.find()) {
            int tmpIndex = -1;
            String matched = match.group();
            double rep = config.getRoundNumberMap().get(matched);

            // \\s+ for filter the spaces.
            power *= rep;

            while ((tmpIndex = handle.indexOf(matched.toLowerCase(), startIndex)) >= 0) {
                String front = QueryProcessor.trimEnd(handle.substring(0, tmpIndex));
                startIndex = front.length();
                handle = front + handle.substring(tmpIndex + matched.length());
            }
        }

        // Scale used in the calculate of double
        result.setValue(getDigitalValue(handle, power));

        return result;
    }

    private ParseResult fracLikeNumberParse(ExtractResult extractResult) {

        ParseResult result = new ParseResult(extractResult.getStart(), extractResult.getLength(), extractResult.getText(), extractResult.getType(), null, null, null);
        String resultText = extractResult.getText().toLowerCase();

        Matcher match = config.getFractionPrepositionRegex().matcher(resultText);
        if (match.find()) {

            String numerator = match.group("numerator");
            String denominator = match.group("denominator");

            double smallValue = Character.isDigit(numerator.charAt(0)) ?
                    getDigitalValue(numerator, 1) :
                    getIntValue(getMatches(numerator));

            double bigValue = Character.isDigit(denominator.charAt(0)) ?
                    getDigitalValue(denominator, 1) :
                    getIntValue(getMatches(denominator));

            result.setValue(smallValue / bigValue);
        } else {
            List<String> fracWords = config.normalizeTokenSet(Arrays.asList(resultText.split(" ")), result);

            // Split fraction with integer
            int splitIndex = fracWords.size() - 1;
            long currentValue = config.resolveCompositeNumber(fracWords.get(splitIndex));
            long roundValue = 1;

            // For case like "half"
            if (fracWords.size() == 1) {
                result.setValue(1 / getIntValue(fracWords));
                return result;
            }

            for (splitIndex = fracWords.size() - 2; splitIndex >= 0; splitIndex--) {

                String fracWord = fracWords.get(splitIndex);
                if (config.getWrittenFractionSeparatorTexts().contains(fracWord) ||
                    config.getWrittenIntegerSeparatorTexts().contains(fracWord)) {
                    continue;
                }

                long previousValue = currentValue;
                currentValue = config.resolveCompositeNumber(fracWord);

                int smHundreds = 100;

                // previous : hundred
                // current : one
                if ((previousValue >= smHundreds && previousValue > currentValue) ||
                        (previousValue < smHundreds && isComposable(currentValue, previousValue))) {
                    if (previousValue < smHundreds && currentValue >= roundValue) {
                        roundValue = currentValue;
                    } else if (previousValue < smHundreds && currentValue < roundValue) {
                        splitIndex++;
                        break;
                    }

                    // current is the first word
                    if (splitIndex == 0) {
                        // scan, skip the first word
                        splitIndex = 1;
                        while (splitIndex <= fracWords.size() - 2) {
                            // e.g. one hundred thousand
                            // frac[i+1] % 100 && frac[i] % 100 = 0
                            if (config.resolveCompositeNumber(fracWords.get(splitIndex)) >= smHundreds &&
                                    !config.getWrittenFractionSeparatorTexts().contains(fracWords.get(splitIndex + 1)) &&
                                    config.resolveCompositeNumber(fracWords.get(splitIndex + 1)) < smHundreds) {
                                splitIndex++;
                                break;
                            }
                            splitIndex++;
                        }
                        break;
                    }
                    continue;
                }
                splitIndex++;
                break;
            }

            if (splitIndex < 0) {
                splitIndex = 0;
            }

            List<String> fracPart = new ArrayList<String>();
            for (int i = splitIndex; i < fracWords.size(); i++) {
                if (fracWords.get(i).contains("-")) {
                    String[] split = fracWords.get(i).split(Pattern.quote("-"));
                    fracPart.add(split[0]);
                    fracPart.add("-");
                    fracPart.add(split[1]);
                } else {
                    fracPart.add(fracWords.get(i));
                }
            }

            fracWords.subList(splitIndex, fracWords.size()).clear();

            // denomi = denominator
            double denomiValue = getIntValue(fracPart);
            // Split mixed number with fraction
            double numerValue = 0;
            double intValue = 0;

            int mixedIndex = fracWords.size();
            for (int i = fracWords.size() - 1; i >= 0; i--) {
                if (i < fracWords.size() - 1 && config.getWrittenFractionSeparatorTexts().contains(fracWords.get(i))) {
                    String numerStr = String.join(" ", fracWords.subList(i + 1, fracWords.size()));
                    numerValue = getIntValue(getMatches(numerStr));
                    mixedIndex = i + 1;
                    break;
                }
            }

            String intStr = String.join(" ", fracWords.subList(0, mixedIndex));
            intValue = getIntValue(getMatches(intStr));

            // Find mixed number
            if (mixedIndex != fracWords.size() && numerValue < denomiValue) {
                result.setValue(intValue + numerValue / denomiValue);
            } else {
                result.setValue((intValue + numerValue) / denomiValue);
            }
        }

        return result;
    }

    private ParseResult textNumberParse(ExtractResult extractResult) {

        ParseResult result = new ParseResult(extractResult.getStart(), extractResult.getLength(), extractResult.getText(), extractResult.getType(), null, null, null);
        String handle = extractResult.getText().toLowerCase();

        //region Special case for "dozen"
        handle = config.getHalfADozenRegex().matcher(handle).replaceAll(config.getHalfADozenText());
        //endregion

        List<String> numGroup = QueryProcessor.split(handle, config.getWrittenDecimalSeparatorTexts());

        //region IntegerPart
        String intPart = numGroup.get(0);

        //Store all match str.
        List<String> matchStrs = new ArrayList<>();
        Matcher smatch = textNumberRegex.matcher(intPart);
        while (smatch.find()) {
            String matchStr = smatch.group().toLowerCase();
            matchStrs.add(matchStr);
        }

        // Get the value recursively
        double intPartRet = getIntValue(matchStrs);
        //endregion

        //region DecimalPart
        double pointPartRet = 0;
        if (numGroup.size() == 2) {
            String pointPart = numGroup.get(1);
            smatch = textNumberRegex.matcher(pointPart);
            matchStrs.clear();
            while (smatch.find()) {
                String matchStr = smatch.group().toLowerCase();
                matchStrs.add(matchStr);
            }
            pointPartRet += getPointValue(matchStrs);
        }
        //endregion

        result.setValue(intPartRet + pointPartRet);

        return result;
    }

    protected ParseResult powerNumberParse(ExtractResult extractResult) {

        ParseResult result = new ParseResult(extractResult.getStart(), extractResult.getLength(), extractResult.getText(), extractResult.getType(), null, null, null);

        String handle = extractResult.getText().toUpperCase();
        boolean isE = !extractResult.getText().contains("^");

        //[1] 1e10
        //[2] 1.1^-23
        Stack<Double> calStack = new Stack<>();

        double scale = 10;
        boolean dot = false;
        boolean isNegative = false;
        double tmp = 0;
        for (int i = 0; i < handle.length(); i++) {
            char ch = handle.charAt(i);
            if (ch == '^' || ch == 'E') {
                if (isNegative) {
                    calStack.add(-tmp);
                } else {
                    calStack.add(tmp);
                }
                tmp = 0;
                scale = 10;
                dot = false;
                isNegative = false;
            } else if (ch >= '0' && ch <= '9') {
                if (dot) {
                    tmp = tmp + scale * (ch - '0');
                    scale *= 0.1;
                } else {
                    tmp = tmp * scale + (ch - '0');
                }
            } else if (ch == config.getDecimalSeparatorChar()) {
                dot = true;
                scale = 0.1;
            } else if (ch == '-') {
                isNegative = !isNegative;
            } else if (ch == '+') {
                continue;
            }

            if (i == handle.length() - 1) {
                if (isNegative) {
                    calStack.add(-tmp);
                } else {
                    calStack.add(tmp);
                }
            }
        }

        double ret;
        if (isE) {
            ret = calStack.remove(0) * Math.pow(10, calStack.remove(0));
        } else {
            ret = Math.pow(calStack.remove(0), calStack.remove(0));
        }


        result.setValue(ret);
        result.setResolutionStr(NumberFormatUtility.format(ret, config.getCultureInfo()));

        return result;
    }

    protected String getKeyRegex(Set<String> keyCollection) {
        ArrayList<String> keys = new ArrayList<>(keyCollection);
        Collections.sort(keys, Collections.reverseOrder());

        return String.join("|", keys);
    }

    private boolean skipNonDecimalSeparator(char ch, int distance, CultureInfo culture) {
        int decimalLength = 3;

        // @TODO: Add this to project level configuration file to be kept in sync
        // Special cases for multi-language countries where decimal separators can be used interchangeably. Mostly informally.
        // Ex: South Africa, Namibia; Puerto Rico in ES; or in Canada for EN and FR.
        // "me pidio $5.00 prestados" and "me pidio $5,00 prestados" -> currency $5
        Pattern cultureRegex = Pattern.compile("^(en|es|fr)(-)?\\b", Pattern.CASE_INSENSITIVE | Pattern.UNICODE_CHARACTER_CLASS);

        return (ch == config.getNonDecimalSeparatorChar() && !(distance <= decimalLength && cultureRegex.matcher(culture.cultureCode).find()));
    }

    protected double getDigitalValue(String digitsStr, double power) {
        double temp = 0;
        double scale = 10;
        boolean decimalSeparator = false;
        int strLength = digitsStr.length();
        boolean isNegative = false;
        boolean isFrac = digitsStr.contains("/");

        Stack<Double> calStack = new Stack<>();

        for (int i = 0; i < digitsStr.length(); i++) {

            char ch = digitsStr.charAt(i);
            boolean skippableNonDecimal = skipNonDecimalSeparator(ch, strLength - i, config.getCultureInfo());

            if (!isFrac && (ch == ' ' || ch == Constants.NO_BREAK_SPACE || skippableNonDecimal)) {
                continue;
            }

            if (ch == ' ' || ch == '/') {
                calStack.push(temp);
                temp = 0;
            } else if (ch >= '0' && ch <= '9') {
                if (decimalSeparator) {
                    temp = temp + scale * (ch - '0');
                    scale *= 0.1;
                } else {
                    temp = temp * scale + (ch - '0');
                }
            } else if (ch == config.getDecimalSeparatorChar() || (!skippableNonDecimal && ch == config.getNonDecimalSeparatorChar())) {
                decimalSeparator = true;
                scale = 0.1;
            } else if (ch == '-') {
                isNegative = true;
            }
        }
        calStack.push(temp);

        // is the number is a fraction.
        double calResult = 0;
        if (isFrac) {
            double deno = calStack.pop();
            double mole = calStack.pop();
            calResult += mole / deno;
        }

        while (!calStack.empty()) {
            calResult += calStack.pop();
        }
        calResult *= power;

        if (isNegative) {
            return -calResult;
        }

        return calResult;
    }


    protected Double getIntValue(List<String> matchStrs) {
        boolean[] isEnd = new boolean[matchStrs.size()];
        Arrays.fill(isEnd, false);

        double tempValue = 0;
        long endFlag = 1;

        //Scan from end to start, find the end word
        for (int i = matchStrs.size() - 1; i >= 0; i--) {
            if (roundNumberSet.contains(matchStrs.get(i))) {
                //if false,then continue
                //You will meet hundred first, then thousand.
                if (endFlag > config.getRoundNumberMap().get(matchStrs.get(i))) {
                    continue;
                }

                isEnd[i] = true;
                endFlag = config.getRoundNumberMap().get(matchStrs.get(i));
            }
        }

        if (endFlag == 1) {
            Stack<Double> tempStack = new Stack<>();
            String oldSym = "";
            for (String matchStr : matchStrs) {
                boolean isCardinal = config.getCardinalNumberMap().containsKey(matchStr);
                boolean isOrdinal = config.getOrdinalNumberMap().containsKey(matchStr);

                if (isCardinal || isOrdinal) {
                    double matchValue = isCardinal ?
                            config.getCardinalNumberMap().get(matchStr) :
                            config.getOrdinalNumberMap().get(matchStr);

                    //This is just for ordinal now. Not for fraction ever.
                    if (isOrdinal) {
                        double fracPart = config.getOrdinalNumberMap().get(matchStr);
                        if (!tempStack.empty()) {
                            double intPart = tempStack.pop();

                            // if intPart >= fracPart, it means it is an ordinal number
                            // it begins with an integer, ends with an ordinal
                            // e.g. ninety-ninth
                            if (intPart >= fracPart) {
                                tempStack.push(intPart + fracPart);
                            } else {
                                // another case of the type is ordinal
                                // e.g. three hundredth
                                while (!tempStack.empty()) {
                                    intPart = intPart + tempStack.pop();
                                }
                                tempStack.push(intPart * fracPart);
                            }
                        } else {
                            tempStack.push(fracPart);
                        }
                    } else if (config.getCardinalNumberMap().containsKey(matchStr)) {
                        if (oldSym.equalsIgnoreCase("-")) {
                            double sum = tempStack.pop() + matchValue;
                            tempStack.push(sum);
                        } else if (oldSym.equalsIgnoreCase(config.getWrittenIntegerSeparatorTexts().get(0)) || tempStack.size() < 2) {
                            tempStack.push(matchValue);
                        } else if (tempStack.size() >= 2) {
                            double sum = tempStack.pop() + matchValue;
                            sum = tempStack.pop() + sum;
                            tempStack.push(sum);
                        }
                    }
                } else {
                    long complexValue = config.resolveCompositeNumber(matchStr);
                    if (complexValue != 0) {
                        tempStack.push((double)complexValue);
                    }
                }
                oldSym = matchStr;
            }

            for (double stackValue : tempStack) {
                tempValue += stackValue;
            }
        } else {
            int lastIndex = 0;
            double mulValue = 1;
            double partValue = 1;
            for (int i = 0; i < isEnd.length; i++) {
                if (isEnd[i]) {
                    mulValue = config.getRoundNumberMap().get(matchStrs.get(i));
                    partValue = 1;

                    if (i != 0) {
                        partValue = getIntValue(matchStrs.subList(lastIndex, i));

                    }

                    tempValue += mulValue * partValue;
                    lastIndex = i + 1;
                }
            }

            //Calculate the part like "thirty-one"
            mulValue = 1;
            if (lastIndex != isEnd.length) {
                partValue = getIntValue(matchStrs.subList(lastIndex, isEnd.length));
                tempValue += mulValue * partValue;
            }
        }

        return tempValue;
    }

    private double getPointValue(List<String> matchStrs) {
        double ret = 0;
        String firstMatch = matchStrs.get(0);

        if (config.getCardinalNumberMap().containsKey(firstMatch) && config.getCardinalNumberMap().get(firstMatch) >= 10) {
            String prefix = "0.";
            int tempInt = getIntValue(matchStrs).intValue();
            String all = prefix + tempInt;
            ret = Double.parseDouble(all);
        } else {
            double scale = 0.1;
            for (String matchStr : matchStrs) {
                ret += config.getCardinalNumberMap().get(matchStr) * scale;
                scale *= 0.1;
            }
        }

        return ret;
    }

    private List<String> getMatches(String input) {
        Matcher smatch = textNumberRegex.matcher(input);
        List<String> matchStrs = new ArrayList<String>();

        //Store all match str.
        while (smatch.find()) {
            String matchStr = smatch.group();
            matchStrs.add(matchStr);
        }

        return matchStrs;
    }

    //Test if big and combine with small.
    //e.g. "hundred" can combine with "thirty" but "twenty" can't combine with "thirty".
    private boolean isComposable(long big, long small) {
        int baseNumber = small > 10 ? 100 : 10;
        return big % baseNumber == 0 && big / baseNumber >= 1;
    }
}
