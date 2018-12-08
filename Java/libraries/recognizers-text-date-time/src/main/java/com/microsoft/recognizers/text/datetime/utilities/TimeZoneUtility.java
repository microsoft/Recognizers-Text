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
                int begin = er.start + er.length;
                int end = timeZoneEr.start;

                if (begin < end) {
                    String gapText = text.substring(begin, end);

                    if (StringUtility.isNullOrWhiteSpace(gapText)) {
                        int length = timeZoneEr.start + timeZoneEr.length - er.start;
                        Map<String, Object> data = new HashMap<>();
                        data.put(Constants.SYS_DATETIME_TIMEZONE, timeZoneEr);

                        originalErs.set(index, new ExtractResult(er.start, length, text.substring(er.start, er.start + length), er.type, data));
                    }
                }
            }
            index++;
        }

        return originalErs;
    }

    public static boolean shouldResolveTimeZone(ExtractResult er, DateTimeOptions options) {
        boolean enablePreview = options.match(DateTimeOptions.EnablePreview);
        boolean hasTimeZoneData = false;

        if (er.data instanceof Map) {
            Map<String, Object> metadata = (HashMap<String, Object>)er.data;

            if (metadata.containsKey(Constants.SYS_DATETIME_TIMEZONE)) {
                hasTimeZoneData = true;
            }
        }

        return enablePreview && hasTimeZoneData;
    }
}
