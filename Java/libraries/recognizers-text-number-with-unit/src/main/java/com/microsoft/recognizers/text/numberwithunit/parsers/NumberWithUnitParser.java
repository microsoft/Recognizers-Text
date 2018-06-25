package com.microsoft.recognizers.text.numberwithunit.parsers;

import com.microsoft.recognizers.text.ExtractResult;
import com.microsoft.recognizers.text.IParser;
import com.microsoft.recognizers.text.ParseResult;
import com.microsoft.recognizers.text.numberwithunit.Constants;
import com.microsoft.recognizers.text.numberwithunit.models.UnitValue;

import java.util.ArrayList;
import java.util.List;
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

        if (extResult.data instanceof ExtractResult) {
            numberResult = (ExtractResult) extResult.data;
        } else if (extResult.type.equals(Constants.SYS_NUM)) {
            return ret.withValue(config.getInternalNumberParser().parse(extResult).value);
        } else // if there is no unitResult, means there is just unit
        {
            numberResult = new ExtractResult(-1, 0, null, null, null);
        }

        // key contains units
        String key = extResult.text;
        StringBuilder unitKeyBuild = new StringBuilder();
        List<String> unitKeys = new ArrayList<>();

        for (int i = 0; i <= key.length(); i++) {
            if (i == key.length()) {
                if (unitKeyBuild.length() != 0) {
                    addIfNotContained(unitKeys, unitKeyBuild.toString().trim());
                }
            } else if (i == numberResult.start) {
                // numberResult.start is a relative position
                if (unitKeyBuild.length() != 0) {
                    addIfNotContained(unitKeys, unitKeyBuild.toString().trim());
                    unitKeyBuild = new StringBuilder();
                }
                i = numberResult.start + numberResult.length - 1;
            } else {
                unitKeyBuild.append(key.charAt(i));
            }
        }

        /* Unit type depends on last unit in suffix.*/
        String lastUnit = unitKeys.get(unitKeys.size() - 1).toLowerCase();

        if (connectorToken != null && !connectorToken.isEmpty() && lastUnit.startsWith(connectorToken)) {
            lastUnit = lastUnit.substring(connectorToken.length()).trim();
        }

        if (key != null && !key.isEmpty() && unitMap != null && unitMap.containsKey(lastUnit)) {
            String unitValue = unitMap.get(lastUnit);

            ParseResult numValue = numberResult.text == null || numberResult.text.isEmpty()
                    ? null
                    : this.config.getInternalNumberParser().parse(numberResult);

            String resolutionStr = numValue != null ? numValue.resolutionStr : null;

            ret = ret
                    .withValue(new UnitValue(resolutionStr, unitValue))
                    .withResolutionStr(String.format("%s %s", resolutionStr != null ? resolutionStr : "", unitValue).trim());
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