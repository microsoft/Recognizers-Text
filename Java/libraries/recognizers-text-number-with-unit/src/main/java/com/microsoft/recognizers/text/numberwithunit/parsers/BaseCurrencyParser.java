package com.microsoft.recognizers.text.numberwithunit.parsers;

import com.microsoft.recognizers.text.ExtractResult;
import com.microsoft.recognizers.text.IParser;
import com.microsoft.recognizers.text.ParseResult;
import com.microsoft.recognizers.text.numberwithunit.Constants;
import com.microsoft.recognizers.text.numberwithunit.models.CurrencyUnitValue;
import com.microsoft.recognizers.text.numberwithunit.models.UnitValue;
import com.microsoft.recognizers.text.numberwithunit.utilities.DictionaryUtils;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.Optional;


public class BaseCurrencyParser implements IParser {

    private final BaseNumberWithUnitParserConfiguration config;
    private final NumberWithUnitParser numberWithUnitParser;

    public BaseCurrencyParser(BaseNumberWithUnitParserConfiguration config) {
        this.config = config;
        this.numberWithUnitParser = new NumberWithUnitParser(config);
    }

    @Override
    public ParseResult parse(ExtractResult extResult) {

        ParseResult pr;
        if (extResult.getData() instanceof List) {
            return mergeCompoundUnit(extResult);
        } else {
            pr = numberWithUnitParser.parse(extResult);
            UnitValue value = (UnitValue)pr.getValue();

            String mainUnitIsoCode = null;
            if (value != null && config.getCurrencyNameToIsoCodeMap().containsKey(value.unit)) {
                mainUnitIsoCode = config.getCurrencyNameToIsoCodeMap().get(value.unit);
            }

            if (mainUnitIsoCode == null || mainUnitIsoCode.isEmpty() || mainUnitIsoCode.startsWith(Constants.FAKE_ISO_CODE_PREFIX)) {
                pr.setValue(new UnitValue(value.number, value.unit));
            } else {
                pr.setValue(new CurrencyUnitValue(value.number, value.unit, mainUnitIsoCode));
            }
            return pr;
        }
    }

    @SuppressWarnings("unchecked")
    private ParseResult mergeCompoundUnit(ExtractResult compoundResult) {
        List<ParseResult> results = new ArrayList<>();
        List<ExtractResult> compoundUnit = (List<ExtractResult>)compoundResult.getData();

        int count = 0;
        ParseResult result = null;
        // Make the default numberValue a constant to check if there is no Value.
        double numberValue = Double.MIN_VALUE;
        String mainUnitValue = "";
        String mainUnitIsoCode = "";
        String fractionUnitsString = "";

        for (int idx = 0; idx < compoundUnit.size(); idx++) {
            ExtractResult extractResult = compoundUnit.get(idx);
            ParseResult parseResult = numberWithUnitParser.parse(extractResult);
            Optional<UnitValue> parseResultValue = Optional.ofNullable(parseResult.getValue() instanceof UnitValue ? (UnitValue)parseResult.getValue() : null);
            String unitValue = parseResultValue.isPresent() ? parseResultValue.get().unit : "";

            // Process a new group
            if (count == 0) {
                if (!extractResult.getType().equals(Constants.SYS_UNIT_CURRENCY)) {
                    continue;
                }

                // Initialize a new result
                result = new ParseResult(extractResult.getStart(), extractResult.getLength(), extractResult.getText(), extractResult.getType(), null, null, null);

                mainUnitValue = unitValue;
                if (parseResultValue.isPresent() && parseResultValue.get().number != null) {
                    numberValue = Double.parseDouble(parseResultValue.get().number);
                }

                result.setResolutionStr(parseResult.getResolutionStr());
                mainUnitIsoCode = config.getCurrencyNameToIsoCodeMap().containsKey(unitValue) ? config.getCurrencyNameToIsoCodeMap().get(unitValue) : mainUnitIsoCode;

                // If the main unit can't be recognized, finish process this group.
                if (mainUnitIsoCode == null || mainUnitIsoCode.isEmpty()) {
                    result.setValue(new UnitValue(getNumberValue(numberValue), mainUnitValue));
                    results.add(result);
                    result = null;
                    continue;
                }

                fractionUnitsString = config.getCurrencyFractionMapping().containsKey(mainUnitIsoCode) ?
                        config.getCurrencyFractionMapping().get(mainUnitIsoCode) :
                        fractionUnitsString;
            } else {

                // Match pure number as fraction unit.
                if (extractResult.getType().equals(Constants.SYS_NUM)) {
                    numberValue += (double)parseResult.getValue() * (1.0 / 100);
                    result.setLength(parseResult.getStart() + parseResult.getLength() - result.getStart());
                    result.setResolutionStr(result.getResolutionStr() + " " + parseResult.getResolutionStr());
                    count++;
                    continue;
                }

                String fractionUnitCode = config.getCurrencyFractionCodeList().containsKey(unitValue) ?
                        config.getCurrencyFractionCodeList().get(unitValue) :
                        null;

                String unit = parseResultValue.isPresent() ? parseResultValue.get().unit : null;
                Optional<Long> fractionNumValue = Optional.ofNullable(config.getCurrencyFractionNumMap().containsKey(unit) ?
                        config.getCurrencyFractionNumMap().get(unit) :
                        null);

                if (fractionUnitCode != null && !fractionUnitCode.isEmpty() && fractionNumValue.isPresent() && fractionNumValue.get() != 0 &&
                    checkUnitsStringContains(fractionUnitCode, fractionUnitsString)) {
                    numberValue += Double.parseDouble(parseResultValue.get().number) * (1.0 / fractionNumValue.get());
                    result.setLength(parseResult.getStart() + parseResult.getLength() - result.getStart());
                    result.setResolutionStr(result.getResolutionStr() + " " + parseResult.getResolutionStr());
                } else {
                    // If the fraction unit doesn't match the main unit, finish process this group.
                    if (result != null) {
                        result = createCurrencyResult(result, mainUnitIsoCode, numberValue, mainUnitValue);
                        results.add(result);
                        result = null;
                    }

                    count = 0;
                    idx -= 1;
                    numberValue = Double.MIN_VALUE;
                    continue;
                }
            }

            count++;
        }

        if (result != null) {
            result = createCurrencyResult(result, mainUnitIsoCode, numberValue, mainUnitValue);
            results.add(result);
        }

        resolveText(results, compoundResult.getText(), compoundResult.getStart());

        return new ParseResult(null, null, null, null, null, results, null);
    }

    private boolean checkUnitsStringContains(String fractionUnitCode, String fractionUnitsString) {
        Map<String, String> unitsMap = new HashMap<String, String>();
        DictionaryUtils.bindUnitsString(unitsMap, "", fractionUnitsString);
        return unitsMap.containsKey(fractionUnitCode);
    }

    private void resolveText(List<ParseResult> prs, String source, int bias) {
        for (ParseResult parseResult : prs) {
            if (parseResult.getStart() != null && parseResult.getLength() != null) {
                int start = parseResult.getStart() - bias;
                parseResult.setText(source.substring(start, start + parseResult.getLength()));
                prs.set(prs.indexOf(parseResult), parseResult);
            }
        }
    }

    private String getNumberValue(double numberValue) {
        if (numberValue == Double.MIN_VALUE) {
            return null;
        } else {
            java.text.NumberFormat numberFormat = java.text.NumberFormat.getInstance();
            numberFormat.setMinimumFractionDigits(0);
            numberFormat.setGroupingUsed(false);
            return numberFormat.format(numberValue);
        }
    }

    private ParseResult createCurrencyResult(ParseResult result, String mainUnitIsoCode, Double numberValue, String mainUnitValue) {
        if (mainUnitIsoCode == null || mainUnitIsoCode.isEmpty() ||
                mainUnitIsoCode.startsWith(Constants.FAKE_ISO_CODE_PREFIX)) {
            result.setValue(new UnitValue(getNumberValue(numberValue), mainUnitValue));
        } else {
            result.setValue(new CurrencyUnitValue(getNumberValue(numberValue), mainUnitValue, mainUnitIsoCode));
        }
        return result;
    }
}
