package com.microsoft.recognizers.text.number.parsers;

import com.microsoft.recognizers.text.ExtractResult;
import com.microsoft.recognizers.text.ParseResult;
import org.javatuples.Pair;

import java.util.List;

public class BasePercentageParser extends BaseNumberParser {
    public BasePercentageParser(INumberParserConfiguration config) {
        super(config);
    }

    @Override
    public ParseResult parse(ExtractResult extractResult) {
        String originText = extractResult.text;
        ParseResult ret = null;

        // replace text & data from extended info
        if (extractResult.data instanceof List) {
            List<Pair<String, ExtractResult>> extendedData = (List<Pair<String, ExtractResult>>) extractResult.data;
            if (extendedData.size() == 2) {
                // for case like "2 out of 5".
                String newText = extendedData.get(0).getValue0() + " " + config.getFractionMarkerToken() + " " + extendedData.get(1).getValue0();
                extractResult = extractResult
                        .withText(newText)
                        .withData("Frac" + config.getLangMarker());

                ret = super.parse(extractResult);
                ret = ret.withValue((double) ret.value * 100);
            } else if (extendedData.size() == 1) {
                // for case like "one third of".
                extractResult = extractResult
                        .withText(extendedData.get(0).getValue0())
                        .withData(extendedData.get(0).getValue1().data);

                ret = super.parse(extractResult);

                if (extractResult.data.toString().startsWith("Frac")) {
                    ret = ret.withValue((double) ret.value * 100);
                }
            }

            String resolutionStr = config.getCultureInfo() != null
                    ? NumberFormatUtility.format(ret.value, config.getCultureInfo()) + "%"
                    : ret.value + "%";
            ret = ret.withResolutionStr(resolutionStr);
        } else {
            // for case like "one percent" or "1%".
            Pair<String, ExtractResult> extendedData = (Pair<String, ExtractResult>) extractResult.data;
            extractResult = extractResult
                    .withText(extendedData.getValue0())
                    .withData(extendedData.getValue1().data);

            ret = super.parse(extractResult);

            if (ret.resolutionStr != null && !ret.resolutionStr.isEmpty()) {
                if (!ret.resolutionStr.trim().endsWith("%")) {
                    ret = ret.withResolutionStr(ret.resolutionStr.trim() + "%");
                }
            }
        }

        ret = ret
                .withText(originText)
                .withData(extractResult.text);

        return ret;

    }
}
