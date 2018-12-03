package com.microsoft.recognizers.text.datetime.utilities;

import com.microsoft.recognizers.text.datetime.Constants;
import com.microsoft.recognizers.text.utilities.IntegerUtility;
import com.microsoft.recognizers.text.utilities.Match;
import com.microsoft.recognizers.text.utilities.RegExpUtility;
import com.microsoft.recognizers.text.utilities.StringUtility;

import java.time.Duration;
import java.time.LocalDateTime;
import java.time.temporal.ChronoUnit;
import java.util.ArrayList;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

public class FormatUtil {

    private static final String dateDelimiter = "-";
    private static final String timeDelimiter = ":";

    public static final Pattern HourTimexRegex = Pattern.compile("(?<!P)T(\\d{2})");
    public static final Pattern WeekDayTimexRegex = Pattern.compile("XXXX-WXX-(\\d)");

    public static String luisDate(Integer year, Integer month, Integer day) {
        if (year == -1) {
            if (month == -1) {
                if (day == -1) {
                    return String.join(dateDelimiter, "XXXX", "XX", "XX");
                }

                return String.join(dateDelimiter, "XXXX", "XX", String.format("%02d", day));
            }

            return String.join(dateDelimiter, "XXXX", String.format("%02d", month), String.format("%02d", day));
        }

        return String.join(dateDelimiter, String.format("%04d", year), String.format("%02d", month),
                String.format("%02d", day));
    }

    public static String luisDate(LocalDateTime date) {
        return luisDate(date.getYear(), date.getMonthValue(), date.getDayOfMonth());
    }

    public static String luisDateTime(LocalDateTime time) {
        return luisDate(time) + "T" + luisTime(time.getHour(), time.getMinute(), time.getSecond());
    }

    public static String luisTime(int hour, int min, int second) {
        return String.join(timeDelimiter, String.format("%02d", hour), String.format("%02d", min),
                String.format("%02d", second));
    }

    public static String luisTime(LocalDateTime time) {
        return luisTime(time.getHour(), time.getMinute(), time.getSecond());
    }

    public static String formatDate(LocalDateTime date) {
        return String.join(dateDelimiter, String.format("%04d", date.getYear()), String.format("%02d", date.getMonthValue()), String.format("%02d", date.getDayOfMonth()));
    }
    
    public static String formatTime(LocalDateTime time) {
        return String.join(timeDelimiter, String.format("%02d", time.getHour()), String.format("%02d", time.getMinute()), String.format("%02d", time.getSecond()));
    }

    public static String formatDateTime(LocalDateTime datetime) {
        return String.join(" ", formatDate(datetime), formatTime(datetime));
    }

    public static String shortTime(int hour, int min, int second) {
        if (min < 0 && second < 0) {
            return String.format("T%02d", hour);
        } else if (second < 0) {
            return String.format("T%02d:%02d", hour, min);
        }

        return String.format("T%02d:%02d:%02d", hour, min, second);
    }

    // Only handle TimeSpan which is less than one day
    public static String luisTimeSpan(Duration timeSpan) {
        String result = "PT";

        if (timeSpan.toHours() % 24 > 0) {
            result = String.format("%s%sH", result, timeSpan.toHours() % 24);
        }

        if (timeSpan.toMinutes() % 60 > 0) {
            result = String.format("%s%sM", result, timeSpan.toMinutes() % 60);
        }

        if (timeSpan.get(ChronoUnit.SECONDS) % 60 > 0) {
            result = String.format("%s%sS", result, timeSpan.get(ChronoUnit.SECONDS) % 60);
        }

        if (timeSpan.toMinutes() % 60 > 0) {
            result = String.format("%s%sM", result, timeSpan.toMinutes() % 60);
        }

        if (timeSpan.get(ChronoUnit.SECONDS) % 60 > 0) {
            result = String.format("%s%sS", result, timeSpan.get(ChronoUnit.SECONDS) % 60);
        }

        return timeSpan.toString();
    }

    public static String toPm(String timeStr) {
        boolean hasT = false;
        if (timeStr.startsWith("T")) {
            hasT = true;
            timeStr = timeStr.substring(1);
        }

        String[] splited = timeStr.split(":");
        int hour = Integer.parseInt(splited[0]);
        hour = hour >= Constants.HalfDayHourCount ? hour - Constants.HalfDayHourCount : hour + Constants.HalfDayHourCount;
        splited[0] = String.format("%02d", hour);
        timeStr = String.join(":", splited);

        return hasT ? "T" + timeStr : timeStr;
    }

    public static String allStringToPm(String timeStr) {
        Match[] matches = RegExpUtility.getMatches(HourTimexRegex, timeStr);
        ArrayList<String> splited = new ArrayList<>();

        int lastPos = 0;
        for (Match match : matches) {
            if (lastPos != match.index) {
                splited.add(timeStr.substring(lastPos, match.index));
            }
            splited.add(timeStr.substring(match.index, match.index + match.length));
            lastPos = match.index + match.length;
        }

        if (!StringUtility.isNullOrEmpty(timeStr.substring(lastPos))) {
            splited.add(timeStr.substring(lastPos));
        }

        for (int i = 0; i < splited.size(); i++) {
            if (HourTimexRegex.matcher(splited.get(i)).lookingAt()) {
                splited.set(i, toPm(splited.get(i)));
            }
        }

        // Modify weekDay timex for the cases which cross day boundary
        if (splited.size() >= 4) {
            Matcher weekDayStartMatch = WeekDayTimexRegex.matcher(splited.get(0));
            Matcher weekDayEndMatch = WeekDayTimexRegex.matcher(splited.get(2));
            Matcher hourStartMatch = HourTimexRegex.matcher(splited.get(1));
            Matcher hourEndMatch = HourTimexRegex.matcher(splited.get(3));

            String weekDayStartStr = weekDayStartMatch.find() ? weekDayStartMatch.group(1) : "";
            String weekDayEndStr = weekDayEndMatch.find() ? weekDayEndMatch.group(1) : "";
            String hourStartStr = hourStartMatch.find() ? hourStartMatch.group(1) : "";
            String hourEndStr = hourEndMatch.find() ? hourEndMatch.group(1) : "";

            if (IntegerUtility.canParse(weekDayStartStr) &&
                    IntegerUtility.canParse(weekDayEndStr) &&
                    IntegerUtility.canParse(hourStartStr) &&
                    IntegerUtility.canParse(hourEndStr)) {
                int weekDayStart = Integer.parseInt(weekDayStartStr);
                int weekDayEnd = Integer.parseInt(weekDayEndStr);
                int hourStart = Integer.parseInt(hourStartStr);
                int hourEnd = Integer.parseInt(hourEndStr);

                if (hourEnd < hourStart && weekDayStart == weekDayEnd) {
                    weekDayEnd = weekDayEnd == Constants.WeekDayCount ? 1 : weekDayEnd + 1;
                    splited.set(2, splited.get(2).substring(0, weekDayEndMatch.start(1)) + weekDayEnd);
                }
            }
        }

        return String.join("", splited);
    }
}
