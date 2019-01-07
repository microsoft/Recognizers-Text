package com.microsoft.recognizers.text.numberwithunit.parsers;

import com.microsoft.recognizers.text.ExtractResult;
import com.microsoft.recognizers.text.IParser;
import com.microsoft.recognizers.text.ParseResult;
import com.microsoft.recognizers.text.numberwithunit.Constants;
import com.microsoft.recognizers.text.numberwithunit.models.UnitValue;

import java.util.ArrayList;
import java.util.List;
import java.util.Locale;
import java.util.Map;

public class NumberWithUnitParser implements IParser {

    protected final INumberWithUnitParserConfiguration config;

    public NumberWithUnitParser(INumberWithUnitParserConfiguration config) {
        this.config = config;
    }

    @Override
    public ParseResult parse(ExtractResult extResult) {

        Map<String, String> unitMap = this.config.getUnitMap();
        String connectorToken = this.config.getConnectorToken();
        ParseResult ret = new ParseResult(extResult);
        ExtractResult numberResult;

        if (extResult.getData() instanceof ExtractResult) {
            numberResult = (ExtractResult)extResult.getData();
        } else if (extResult.getType().equals(Constants.SYS_NUM)) {
            ret.setValue(config.getInternalNumberParser().parse(extResult).getValue());
            return ret;
        } else {
            // if there is no unitResult, means there is just unit
            numberResult = new ExtractResult(-1, 0, null, null, null);
        }

        // key contains units
        String key = extResult.getText();
        StringBuilder unitKeyBuild = new StringBuilder();
        List<String> unitKeys = new ArrayList<>();

        for (int i = 0; i <= key.length(); i++) {

            if (i == key.length()) {
                if (unitKeyBuild.length() != 0) {
                    addIfNotContained(unitKeys, unitKeyBuild.toString().trim());
                }
            } else if (i == numberResult.getStart()) {
                // numberResult.start is a relative position
                if (unitKeyBuild.length() != 0) {
                    addIfNotContained(unitKeys, unitKeyBuild.toString().trim());
                    unitKeyBuild = new StringBuilder();
                }
                i = numberResult.getStart() + numberResult.getLength() - 1;
            } else {
                unitKeyBuild.append(key.charAt(i));
            }
        }

        /* Unit type depends on last unit in suffix.*/
        String lastUnit = unitKeys.get(unitKeys.size() - 1);
        String normalizedLastUnit = lastUnit.toLowerCase();

        if (connectorToken != null && !connectorToken.isEmpty() && normalizedLastUnit.startsWith(connectorToken)) {
            normalizedLastUnit = normalizedLastUnit.substring(connectorToken.length()).trim();
            lastUnit = lastUnit.substring(connectorToken.length()).trim();
        }

        if (key != null && !key.isEmpty() && unitMap != null) {

            String unitValue = null;

            if (unitMap.containsKey(lastUnit)) {
                unitValue = unitMap.get(lastUnit);
            } else if (unitMap.containsKey(normalizedLastUnit)) {
                unitValue = unitMap.get(normalizedLastUnit);
            }

            if (unitValue != null) {

                ParseResult numValue = numberResult.getText() == null || numberResult.getText().isEmpty() ?
                        null :
                        this.config.getInternalNumberParser().parse(numberResult);

                String resolutionStr = numValue != null ? numValue.getResolutionStr() : null;

                ret.setValue(new UnitValue(resolutionStr, unitValue));
                ret.setResolutionStr(String.format("%s %s", resolutionStr != null ? resolutionStr : "", unitValue).trim());
            }
        }

        if (ret != null) {
            ret.setText(ret.getText().toLowerCase(Locale.ROOT));
        }

        return ret;
    }

    private void addIfNotContained(List<String> unitKeys, String unit) {

        boolean add = true;
        for (String unitKey : unitKeys) {
            if (unitKey.contains(unit)) {
                add = false;
                break;
            }
        }

        if (add) {
            unitKeys.add(unit);
        }
    }

}