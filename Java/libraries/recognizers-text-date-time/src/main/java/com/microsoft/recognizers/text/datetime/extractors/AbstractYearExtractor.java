package com.microsoft.recognizers.text.datetime.extractors;

import com.microsoft.recognizers.text.ExtractResult;
import com.microsoft.recognizers.text.datetime.Constants;
import com.microsoft.recognizers.text.datetime.extractors.config.IDateExtractorConfiguration;
import com.microsoft.recognizers.text.utilities.Match;
import com.microsoft.recognizers.text.utilities.MatchGroup;
import com.microsoft.recognizers.text.utilities.StringUtility;

import java.time.LocalDateTime;
import java.util.List;

public abstract class AbstractYearExtractor implements IDateExtractor {

    protected final IDateExtractorConfiguration config;

    public AbstractYearExtractor(IDateExtractorConfiguration config) {
        this.config = config;
    }

    @Override
    public abstract String getExtractorName();

    @Override
    public abstract List<ExtractResult> extract(String input, LocalDateTime reference);

    @Override
    public abstract List<ExtractResult> extract(String input);

    @Override
    public int getYearFromText(Match match) {

        int year = Constants.InvalidYear;

        String yearStr = match.getGroup("year").value;
        String writtenYearStr = match.getGroup("fullyear").value;

        if (!StringUtility.isNullOrEmpty(yearStr) && !yearStr.equals(writtenYearStr)) {

            year = Math.round(Double.valueOf(yearStr).floatValue());

            if (year < 100 && year >= Constants.MinTwoDigitYearPastNum) {
                year += 1900;
            } else if (year >= 0 && year < Constants.MaxTwoDigitYearFutureNum) {
                year += 2000;
            }
        } else {

            MatchGroup firstTwoYear = match.getGroup("firsttwoyearnum");

            if (!StringUtility.isNullOrEmpty(firstTwoYear.value)) {
                ExtractResult er = new ExtractResult();
                er.setStart(firstTwoYear.index);
                er.setLength(firstTwoYear.length);
                er.setText(firstTwoYear.value);

                int firstTwoYearNum = Math.round(Double.valueOf((double)config.getNumberParser().parse(er).getValue()).floatValue());

                int lastTwoYearNum = 0;

                MatchGroup lastTwoYear = match.getGroup("lasttwoyearnum");

                if (!StringUtility.isNullOrEmpty(lastTwoYear.value)) {
                    er = new ExtractResult();
                    er.setStart(lastTwoYear.index);
                    er.setLength(lastTwoYear.length);
                    er.setText(lastTwoYear.value);

                    lastTwoYearNum = Math.round(Double.valueOf((double)config.getNumberParser().parse(er).getValue()).floatValue());
                }

                // Exclude pure number like "nineteen", "twenty four"
                if (firstTwoYearNum < 100 && lastTwoYearNum == 0 || firstTwoYearNum < 100 && firstTwoYearNum % 10 == 0 && lastTwoYear.value.trim().split(" ").length == 1) {
                    year = Constants.InvalidYear;
                    return year;
                }

                if (firstTwoYearNum >= 100) {
                    year = firstTwoYearNum + lastTwoYearNum;
                } else {
                    year = firstTwoYearNum * 100 + lastTwoYearNum;
                }

            } else {

                if (!StringUtility.isNullOrEmpty(writtenYearStr)) {

                    MatchGroup writtenYear = match.getGroup("fullyear");

                    ExtractResult er = new ExtractResult();
                    er.setStart(writtenYear.index);
                    er.setLength(writtenYear.length);
                    er.setText(writtenYear.value);

                    year = Math.round(Double.valueOf((double)config.getNumberParser().parse(er).getValue()).floatValue());

                    if (year < 100 && year >= Constants.MinTwoDigitYearPastNum) {
                        year += 1900;
                    } else if (year >= 0 && year < Constants.MaxTwoDigitYearFutureNum) {
                        year += 2000;
                    }
                }
            }
        }
        
        return year;
    }
}
