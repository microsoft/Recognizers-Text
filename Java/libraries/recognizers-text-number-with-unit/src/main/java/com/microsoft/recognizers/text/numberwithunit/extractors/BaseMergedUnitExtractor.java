package com.microsoft.recognizers.text.numberwithunit.extractors;

import com.google.common.collect.Lists;
import com.microsoft.recognizers.text.ExtractResult;
import com.microsoft.recognizers.text.IExtractor;
import com.microsoft.recognizers.text.numberwithunit.Constants;

import java.util.ArrayList;
import java.util.Collections;
import java.util.Comparator;
import java.util.List;
import java.util.regex.Matcher;
import java.util.stream.Collectors;

public class BaseMergedUnitExtractor implements IExtractor {

    private final INumberWithUnitExtractorConfiguration config;

    public BaseMergedUnitExtractor(INumberWithUnitExtractorConfiguration config) {
        this.config = config;
    }

    @Override
    public List<ExtractResult> extract(String source) {
        // Only merge currency's compound units for now.
        if (config.getExtractType().equals(Constants.SYS_UNIT_CURRENCY)) {
            return mergeCompoundUnits(source);
        } else {
            return new NumberWithUnitExtractor(config).extract(source);
        }
    }

    @SuppressWarnings("unchecked")
    private List<ExtractResult> mergeCompoundUnits(String source) {
        List<ExtractResult> ers = new NumberWithUnitExtractor(config).extract(source);
        mergePureNumber(source, ers);

        if (ers.size() == 0) {
            return ers;
        }            

        List<ExtractResult> result = new ArrayList<>();
        int[] groups = new int[ers.size()];
        groups[0] = 0;
        for (int idx = 0; idx < ers.size() - 1; idx++) {
            if (!ers.get(idx).getType().equals(ers.get(idx + 1).getType()) &&
                !ers.get(idx).getType().equals(Constants.SYS_NUM) &&
                !ers.get(idx + 1).getType().equals(Constants.SYS_NUM)) {
                continue;
            }

            if (ers.get(idx).getData() instanceof ExtractResult && !((ExtractResult)ers.get(idx).getData()).getData().toString().startsWith("Integer")) {
                groups[idx + 1] = groups[idx] + 1;
                continue;
            }

            int middleBegin = ers.get(idx).getStart() + ers.get(idx).getLength();
            int middleEnd = ers.get(idx + 1).getStart();

            String middleStr = source.substring(middleBegin, middleEnd).trim().toLowerCase();

            // Separated by whitespace
            if (middleStr.isEmpty()) {
                groups[idx + 1] = groups[idx];
                continue;
            }

            // Separated by connectors
            Matcher match = config.getCompoundUnitConnectorRegex().matcher(middleStr);
            if (match.find() && match.start() == 0 && (match.end() - match.start()) == middleStr.length()) {
                groups[idx + 1] = groups[idx];
            } else {
                groups[idx + 1] = groups[idx] + 1;
            }
        }

        for (int idx = 0; idx < ers.size(); idx++) {
            if (idx == 0 || groups[idx] != groups[idx - 1]) {
                ExtractResult tmpExtractResult = ers.get(idx);
                tmpExtractResult.setData(Lists.newArrayList(
                        new ExtractResult(
                                tmpExtractResult.getStart(),
                                tmpExtractResult.getLength(),
                                tmpExtractResult.getText(),
                                tmpExtractResult.getType(),
                                tmpExtractResult.getData())));

                ers.set(idx, tmpExtractResult);
                result.add(tmpExtractResult);
            }

            // Reduce extract results in same group
            if (idx + 1 < ers.size() && groups[idx + 1] == groups[idx]) {
                int group = groups[idx];

                int periodBegin = result.get(group).getStart();
                int periodEnd = ers.get(idx + 1).getStart() + ers.get(idx + 1).getLength();

                ExtractResult r = result.get(group);

                List<ExtractResult> data = (List<ExtractResult>)r.getData();
                data.add(ers.get(idx + 1));
                r.setLength(periodEnd - periodBegin);
                r.setText(source.substring(periodBegin, periodEnd));
                r.setType(Constants.SYS_UNIT_CURRENCY);
                r.setData(data);

                result.set(group, r);
            }
        }

        for (int idx = 0; idx < result.size(); idx++) {
            if (result.get(idx).getData() instanceof List) {
                List<ExtractResult> innerData = (List<ExtractResult>)result.get(idx).getData();
                if (innerData.size() == 1) {
                    result.set(idx, innerData.get(0));
                }
            }
        }

        result = result.stream().filter(o -> !o.getType().equals(Constants.SYS_NUM))
                .collect(Collectors.toList());

        return result;
    }

    private void mergePureNumber(String source, List<ExtractResult> ers) {

        List<ExtractResult> numErs = config.getUnitNumExtractor().extract(source);
        List<ExtractResult> unitNumbers = new ArrayList<>();
        for (int i = 0, j = 0; i < numErs.size(); i++) {
            boolean hasBehindExtraction = false;
            while (j < ers.size() && ers.get(j).getStart() + ers.get(j).getLength() < numErs.get(i).getStart()) {
                hasBehindExtraction = true;
                j++;
            }

            if (!hasBehindExtraction) {
                continue;
            }

            int middleBegin = ers.get(j - 1).getStart() + ers.get(j - 1).getLength();
            int middleEnd = numErs.get(i).getStart();

            String middleStr = source.substring(middleBegin, middleEnd).trim().toLowerCase();

            // Separated by whitespace
            if (middleStr.isEmpty()) {
                unitNumbers.add(numErs.get(i));
                continue;
            }

            // Separated by connectors
            Matcher match = config.getCompoundUnitConnectorRegex().matcher(middleStr);
            if (match.find()) {
                int start = match.start();
                int end = match.end();
                int length = end - start;

                if (start == 0 && length == middleStr.length()) {
                    unitNumbers.add(numErs.get(i));
                }
            }
        }

        for (ExtractResult extractResult : unitNumbers) {
            boolean overlap = false;
            for (ExtractResult er : ers) {
                if (er.getStart() <= extractResult.getStart() && er.getStart() + er.getLength() >= extractResult.getStart()) {
                    overlap = true;
                }
            }

            if (!overlap) {
                ers.add(extractResult);
            }
        }

        Collections.sort(ers, (Comparator<ExtractResult>)(xo, yo) -> {
            ExtractResult x = (ExtractResult)xo;
            ExtractResult y = (ExtractResult)yo;
            return x.getStart() - y.getStart();
        });
    }
}
