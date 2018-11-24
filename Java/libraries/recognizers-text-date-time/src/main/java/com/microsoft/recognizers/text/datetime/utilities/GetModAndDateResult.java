package com.microsoft.recognizers.text.datetime.utilities;

import java.time.LocalDateTime;

public class GetModAndDateResult {
    public final LocalDateTime beginDate;
    public final LocalDateTime endDate;
    public final String mod;

    public GetModAndDateResult(LocalDateTime beginDate, LocalDateTime endDate, String mod) {
        this.beginDate = beginDate;
        this.endDate = endDate;
        this.mod = mod;
    }

    public GetModAndDateResult() {
        this(null, null, "");
    }
}
