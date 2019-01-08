package com.microsoft.recognizers.text.number.parsers;

import com.microsoft.recognizers.text.ExtractResult;
import com.microsoft.recognizers.text.ParseResult;
import java.util.List;
import org.javatuples.Pair;

public class BasePercentageParser extends BaseNumberParser {
    public BasePercentageParser(INumberParserConfiguration config) {
        super(config);
    }

    @Override
    @SuppressWarnings("unchecked")
    public ParseResult parse(ExtractResult extractResult) {
        String originText = extractResult.getText();
        ParseResult ret = null;

        // replace text & data from extended info
        if (extractResult.getData() instanceof List) {
            List<Pair<String, ExtractResult>> extendedData = (List<Pair<String, ExtractResult>>)extractResult.getData();
            if (extendedData.size() == 2) {
                // for case like "2 out of 5".
                String newText = extendedData.get(0).getValue0() + " " + config.getFractionMarkerToken() + " " + extendedData.get(1).getValue0();
                extractResult.setText(newText);
                extractResult.setData("Frac" + config.getLangMarker());

                ret = super.parse(extractResult);
                ret.setValue((double)ret.getValue() * 100);
            } else if (extendedData.size() == 1) {
                // for case like "one third of".
                extractResult.setText(extendedData.get(0).getValue0());
                extractResult.setData(extendedData.get(0).getValue1().getData());

                ret = super.parse(extractResult);

                if (extractResult.getData().toString().startsWith("Frac")) {
                    ret.setValue((double)ret.getValue() * 100);
                }
            }

            String resolutionStr = config.getCultureInfo() != null ?
                    NumberFormatUtility.format(ret.getValue(), config.getCultureInfo()) + "%" :
                    ret.getValue() + "%";
            ret.setResolutionStr(resolutionStr);
        } else {
            // for case like "one percent" or "1%".
            Pair<String, ExtractResult> extendedData = (Pair<String, ExtractResult>)extractResult.getData();
            extractResult.setText(extendedData.getValue0());
            extractResult.setData(extendedData.getValue1().getData());

            ret = super.parse(extractResult);

            if (ret.getResolutionStr() != null && !ret.getResolutionStr().isEmpty()) {
                if (!ret.getResolutionStr().trim().endsWith("%")) {
                    ret.setResolutionStr(ret.getResolutionStr().trim() + "%");
                }
            }
        }

        ret.setText(originText);
        ret.setData(extractResult.getText());

        return ret;

    }
}
