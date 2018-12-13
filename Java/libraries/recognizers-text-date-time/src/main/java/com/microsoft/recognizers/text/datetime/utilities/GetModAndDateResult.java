package com.microsoft.recognizers.text.datetime.utilities;

import java.time.LocalDateTime;
import java.util.List;

public class GetModAndDateResult {
    public final LocalDateTime beginDate;
    public final LocalDateTime endDate;
    public final String mod;
    public final List<LocalDateTime> dateList;

    public GetModAndDateResult(LocalDateTime beginDate, LocalDateTime endDate, String mod, List<LocalDateTime> dateList) {
        this.beginDate = beginDate;
        this.endDate = endDate;
        this.mod = mod;
        this.dateList = dateList;
    }

    public GetModAndDateResult() {
        this(null, null, "", null);
    }
}
