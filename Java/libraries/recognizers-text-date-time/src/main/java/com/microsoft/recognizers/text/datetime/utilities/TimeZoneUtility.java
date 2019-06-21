package com.microsoft.recognizers.text.datetime.utilities;

import com.microsoft.recognizers.text.ExtractResult;
import com.microsoft.recognizers.text.datetime.Constants;
import com.microsoft.recognizers.text.datetime.DateTimeOptions;
import com.microsoft.recognizers.text.utilities.StringUtility;

import java.util.HashMap;
import java.util.List;
import java.util.Map;

public abstract class TimeZoneUtility {
    public static List<ExtractResult> mergeTimeZones(List<ExtractResult> originalErs, List<ExtractResult> timeZoneErs, String text) {

        int index = 0;
        for (ExtractResult er : originalErs.toArray(new ExtractResult[0])) {
            for (ExtractResult timeZoneEr : timeZoneErs) {
                int begin = er.getStart() + er.getLength();
                int end = timeZoneEr.getStart();

                if (begin < end) {
                    String gapText = text.substring(begin, end);

                    if (StringUtility.isNullOrWhiteSpace(gapText)) {
                        int length = timeZoneEr.getStart() + timeZoneEr.getLength() - er.getStart();
                        Map<String, Object> data = new HashMap<>();
                        data.put(Constants.SYS_DATETIME_TIMEZONE, timeZoneEr);

                        originalErs.set(index, new ExtractResult(er.getStart(), length, text.substring(er.getStart(), er.getStart() + length), er.getType(), data));
                    }
                }

                // Make sure timezone info propagates to longer span entity.
                if (er.isOverlap(timeZoneEr)) {
                    Map<String, Object> data = new HashMap<>();
                    data.put(Constants.SYS_DATETIME_TIMEZONE, timeZoneEr);
                    er.setData(data);
                }
            }
            index++;
        }

        return originalErs;
    }

    public static boolean shouldResolveTimeZone(ExtractResult er, DateTimeOptions options) {
        boolean enablePreview = options.match(DateTimeOptions.EnablePreview);
        if (!enablePreview) {
            return enablePreview;
        }

        boolean hasTimeZoneData = false;

        if (er.getData() instanceof Map) {
            Map<String, Object> metadata = (HashMap<String, Object>)er.getData();

            if (metadata.containsKey(Constants.SYS_DATETIME_TIMEZONE)) {
                hasTimeZoneData = true;
            }
        }

        return hasTimeZoneData;
    }
}
