package com.microsoft.recognizers.text.numberwithunit.parsers;

import com.microsoft.recognizers.text.ExtractResult;
import com.microsoft.recognizers.text.IParser;
import com.microsoft.recognizers.text.ParseResult;
import com.microsoft.recognizers.text.numberwithunit.Constants;
import com.microsoft.recognizers.text.numberwithunit.models.CurrencyUnitValue;
import com.microsoft.recognizers.text.numberwithunit.models.UnitValue;
import com.microsoft.recognizers.text.numberwithunit.utilities.DictionaryUtils;

import java.util.*;

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
        if (extResult.data instanceof List) {
            return mergeCompoundUnit(extResult);
        } else {
            pr = numberWithUnitParser.parse(extResult);
            UnitValue value = (UnitValue) pr.value;

            String mainUnitIsoCode = null;
            if (value != null && config.getCurrencyNameToIsoCodeMap().containsKey(value.unit)) {
                mainUnitIsoCode = config.getCurrencyNameToIsoCodeMap().get(value.unit);
            }

            if (mainUnitIsoCode == null || mainUnitIsoCode.isEmpty() || mainUnitIsoCode.startsWith(Constants.FAKE_ISO_CODE_PREFIX)) {
                return pr.withValue(new UnitValue(value.number, value.unit));
            } else {
                return pr.withValue(new CurrencyUnitValue(value.number, value.unit, mainUnitIsoCode));
            }
        }
    }

    private ParseResult mergeCompoundUnit(ExtractResult compoundResult) {
        List<ParseResult> results = new ArrayList<>();
        List<ExtractResult> compoundUnit = (List<ExtractResult>) compoundResult.data;

        int count = 0;
        ParseResult result = null;
        double numberValue = 0.0;
        String mainUnitValue = "";
        String mainUnitIsoCode = "";
        String fractionUnitsString = "";

        for (int idx = 0; idx < compoundUnit.size(); idx++) {
            ExtractResult extractResult = compoundUnit.get(idx);
            ParseResult parseResult = numberWithUnitParser.parse(extractResult);
            Optional<UnitValue> parseResultValue = Optional.ofNullable(parseResult.value instanceof UnitValue ? (UnitValue) parseResult.value : null);
            String unitValue = parseResultValue.isPresent() ? parseResultValue.get().unit : "";

            // Process a new group
            if (count == 0) {
                if (!extractResult.type.equals(Constants.SYS_UNIT_CURRENCY)) {
                    continue;
                }

                // Initialize a new result
                result = new ParseResult(extractResult.start, extractResult.length, extractResult.text, extractResult.type, null, null, null);

                mainUnitValue = unitValue;
                numberValue = parseResultValue.isPresent() ? Double.parseDouble(parseResultValue.get().number) : 0;
                result = result.withResolutionStr(parseResult.resolutionStr);
                mainUnitIsoCode = config.getCurrencyNameToIsoCodeMap().containsKey(unitValue) ? config.getCurrencyNameToIsoCodeMap().get(unitValue) : mainUnitIsoCode;

                // If the main unit can't be recognized, finish process this group.
                if (mainUnitIsoCode == null || mainUnitIsoCode.isEmpty()) {
                    result = result.withValue(new UnitValue(String.valueOf(numberValue), mainUnitValue));
                    results.add(result);
                    result = null;
                    continue;
                }

                fractionUnitsString = config.getCurrencyFractionMapping().containsKey(mainUnitIsoCode)
                        ? config.getCurrencyFractionMapping().get(mainUnitIsoCode)
                        : fractionUnitsString;
            } else {

                // Match pure number as fraction unit.
                if (extractResult.type.equals(Constants.SYS_NUM)) {
                    numberValue += (double) parseResult.value * (1.0 / 100);
                    result = result
                            .withResolutionStr(result.resolutionStr + " " + parseResult.resolutionStr)
                            .withLength(parseResult.start + parseResult.length - result.start);
                    count++;
                    continue;
                }

                String fractionUnitCode = config.getCurrencyFractionCodeList().containsKey(unitValue)
                        ? config.getCurrencyFractionCodeList().get(unitValue)
                        : null;

                String unit = parseResultValue.isPresent() ? parseResultValue.get().unit : null;
                Optional<Long> fractionNumValue = Optional.ofNullable(config.getCurrencyFractionNumMap().containsKey(unit)
                        ? config.getCurrencyFractionNumMap().get(unit)
                        : null);

                if (fractionUnitCode != null && !fractionUnitCode.isEmpty() && fractionNumValue.isPresent() && fractionNumValue.get() != 0 &&
                        checkUnitsStringContains(fractionUnitCode, fractionUnitsString)) {
                    numberValue += Double.parseDouble(parseResultValue.get().number) * (1.0 / fractionNumValue.get());
                    result = result
                            .withResolutionStr(result.resolutionStr + " " + parseResult.resolutionStr)
                            .withLength(parseResult.start + parseResult.length - result.start);
                } else {
                    // If the fraction unit doesn't match the main unit, finish process this group.
                    if (result != null) {
                        if (mainUnitIsoCode == null || mainUnitIsoCode.isEmpty() ||
                                mainUnitIsoCode.startsWith(Constants.FAKE_ISO_CODE_PREFIX)) {
                            result = result.withValue(new UnitValue(String.valueOf(numberValue), mainUnitValue));
                        } else {
                            result = result.withValue(new CurrencyUnitValue(String.valueOf(numberValue), mainUnitValue, mainUnitIsoCode));
                        }

                        results.add(result);
                        result = null;
                    }

                    count = 0;
                    idx -= 1;
                    continue;
                }
            }

            count++;
        }

        if (result != null) {
            if (mainUnitIsoCode == null || mainUnitIsoCode.isEmpty() ||
                    mainUnitIsoCode.startsWith(Constants.FAKE_ISO_CODE_PREFIX)) {
                result = result.withValue(new UnitValue(String.valueOf(numberValue), mainUnitValue));
            } else {
                result = result.withValue(new CurrencyUnitValue(String.valueOf(numberValue), mainUnitValue, mainUnitIsoCode));
            }

            results.add(result);
        }

        resolveText(results, compoundResult.text, compoundResult.start);

        return new ParseResult(null, null, null, null, null, results, null);
    }

    private boolean checkUnitsStringContains(String fractionUnitCode, String fractionUnitsString) {
        Map<String, String> unitsMap = new HashMap<>();
        DictionaryUtils.bindUnitsString(unitsMap, "", fractionUnitsString);
        return unitsMap.containsKey(fractionUnitCode);
    }

    private void resolveText(List<ParseResult> prs, String source, int bias) {
        for (ParseResult parseResult : prs) {
            if (parseResult.start != null && parseResult.length != null) {
                int start = parseResult.start - bias;
                prs.set(
                        prs.indexOf(parseResult),
                        parseResult.withText(source.substring(start, start + parseResult.length)));
            }
        }
    }
}
