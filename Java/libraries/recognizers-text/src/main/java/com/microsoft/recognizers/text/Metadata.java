package com.microsoft.recognizers.text;

public class Metadata {
    // For cases like "from 2014 to 2018", the period end "2018" could be inclusive or exclusive
    // For extraction, we only mark this flag to avoid future duplicate judgment, whether to include the period end or not is not determined in the extraction step
    private boolean possiblyIncludePeriodEnd = false;

    // For cases like "2015年以前" (usually regards as "before 2015" in English), "5天以前"
    // (usually regards as "5 days ago" in English) in Chinese, we need to decide whether this is a "Date with Mode" or "Duration with Before and After".
    // We use this flag to avoid duplicate judgment both in the Extraction step and Parse step.
    // Currently, this flag is only used in Chinese DateTime as other languages don't have this ambiguity cases.
    private boolean isDurationWithBeforeAndAfter = false;

    private boolean isHoliday = false;

    public boolean getIsHoliday() {
        return isHoliday;
    }

    public void setIsHoliday(boolean isHoliday) {
        this.isHoliday = isHoliday;
    }

    private boolean hasMod = false;

    public boolean getHasMod() {
        return hasMod;
    }

    public void setHasMod(boolean hasMod) {
        this.hasMod = hasMod;
    }

    public boolean getIsPossiblyIncludePeriodEnd() {
        return possiblyIncludePeriodEnd;
    }

    public void setPossiblyIncludePeriodEnd(boolean possiblyIncludePeriodEnd) {
        this.possiblyIncludePeriodEnd = possiblyIncludePeriodEnd;
    }

    public boolean getIsDurationWithBeforeAndAfter() {
        return isDurationWithBeforeAndAfter;
    }

    public void setDurationWithBeforeAndAfter(boolean durationWithBeforeAndAfter) {
        isDurationWithBeforeAndAfter = durationWithBeforeAndAfter;
    }
}
