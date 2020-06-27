from datatypes_timex_expression import Timex, Constants


def test_timex_parsing_complete_date():
    timex = Timex(timex='2017-05-29')
    assert timex.types == {
        Constants.TIMEX_TYPES_DEFINITE,
        Constants.TIMEX_TYPES_DATE}
    assert timex.year == 2017
    assert timex.month == 5
    assert timex.day_of_month == 29
    assert timex.day_of_week is None
    assert timex.week_of_year is None
    assert timex.week_of_month is None
    assert timex.season is None
    assert timex.hour is None
    assert timex.minute is None
    assert timex.second is None
    assert timex.weekend is False
    assert timex.part_of_day is None
    assert timex.years is None
    assert timex.months is None
    assert timex.weeks is None
    assert timex.days is None
    assert timex.hours is None
    assert timex.minutes is None
    assert timex.seconds is None
    assert timex.now is False


def test_timex_parsing_month_and_ady_of_month():
    timex = Timex(timex='XXXX-12-05')
    assert timex.types == {Constants.TIMEX_TYPES_DATE}
    assert timex.year is None
    assert timex.month == 12
    assert timex.day_of_month == 5
    assert timex.day_of_week is None
    assert timex.week_of_year is None
    assert timex.week_of_month is None
    assert timex.season is None
    assert timex.hour is None
    assert timex.minute is None
    assert timex.second is None
    assert timex.weekend is False
    assert timex.part_of_day is None
    assert timex.years is None
    assert timex.months is None
    assert timex.weeks is None
    assert timex.days is None
    assert timex.hours is None
    assert timex.minutes is None
    assert timex.seconds is None
    assert timex.now is False


def test_datatypes_parsing_day_of_week():
    timex = Timex(timex='XXXX-WXX-3')
    assert timex.types == {Constants.TIMEX_TYPES_DATE}
    assert timex.year is None
    assert timex.month is None
    assert timex.day_of_month is None
    assert timex.day_of_week == 3
    assert timex.week_of_year is None
    assert timex.week_of_month is None
    assert timex.season is None
    assert timex.hour is None
    assert timex.minute is None
    assert timex.second is None
    assert timex.weekend is False
    assert timex.part_of_day is None
    assert timex.years is None
    assert timex.months is None
    assert timex.weeks is None
    assert timex.days is None
    assert timex.hours is None
    assert timex.minutes is None
    assert timex.seconds is None
    assert timex.now is False


def test_datatypes_parsing_hours_minutes_and_seconds():
    timex = Timex(timex='T17:30:05')
    assert timex.types == {Constants.TIMEX_TYPES_TIME}
    assert timex.year is None
    assert timex.month is None
    assert timex.day_of_month is None
    assert timex.day_of_week is None
    assert timex.week_of_year is None
    assert timex.week_of_month is None
    assert timex.season is None
    assert timex.hour == 17
    assert timex.minute == 30
    assert timex.second == 5
    assert timex.weekend is False
    assert timex.part_of_day is None
    assert timex.years is None
    assert timex.months is None
    assert timex.weeks is None
    assert timex.days is None
    assert timex.hours is None
    assert timex.minutes is None
    assert timex.seconds is None
    assert timex.now is False


def test_datatypes_parsing_hours_and_minutes():
    timex = Timex(timex='T17:30')
    assert timex.types == {Constants.TIMEX_TYPES_TIME}
    assert timex.year is None
    assert timex.month is None
    assert timex.day_of_month is None
    assert timex.day_of_week is None
    assert timex.week_of_year is None
    assert timex.week_of_month is None
    assert timex.season is None
    assert timex.hour == 17
    assert timex.minute == 30
    assert timex.second == 0
    assert timex.weekend is False
    assert timex.part_of_day is None
    assert timex.years is None
    assert timex.months is None
    assert timex.weeks is None
    assert timex.days is None
    assert timex.hours is None
    assert timex.minutes is None
    assert timex.seconds is None
    assert timex.now is False


def test_datatypes_parsing_hours():
    timex = Timex(timex='T17')
    assert timex.types == {Constants.TIMEX_TYPES_TIME}
    assert timex.year is None
    assert timex.month is None
    assert timex.day_of_month is None
    assert timex.day_of_week is None
    assert timex.week_of_year is None
    assert timex.week_of_month is None
    assert timex.season is None
    assert timex.hour == 17
    assert timex.minute == 0
    assert timex.second == 0
    assert timex.weekend is False
    assert timex.part_of_day is None
    assert timex.years is None
    assert timex.months is None
    assert timex.weeks is None
    assert timex.days is None
    assert timex.hours is None
    assert timex.minutes is None
    assert timex.seconds is None
    assert timex.now is False


def test_datatypes_parsing_now():
    timex = Timex(timex='PRESENT_REF')
    assert timex.types == {
        Constants.TIMEX_TYPES_PRESENT,
        Constants.TIMEX_TYPES_DATE,
        Constants.TIMEX_TYPES_TIME,
        Constants.TIMEX_TYPES_DATETIME}
    assert timex.year is None
    assert timex.month is None
    assert timex.day_of_month is None
    assert timex.day_of_week is None
    assert timex.week_of_year is None
    assert timex.week_of_month is None
    assert timex.season is None
    assert timex.hour is None
    assert timex.minute is None
    assert timex.second is None
    assert timex.weekend is False
    assert timex.part_of_day is None
    assert timex.years is None
    assert timex.months is None
    assert timex.weeks is None
    assert timex.days is None
    assert timex.hours is None
    assert timex.minutes is None
    assert timex.seconds is None
    assert timex.now is True


def test_datatypes_parsing_full_datetime():
    timex = Timex(timex='1984-01-03T18:30:45')
    assert timex.types == {
        Constants.TIMEX_TYPES_DEFINITE,
        Constants.TIMEX_TYPES_DATE,
        Constants.TIMEX_TYPES_TIME,
        Constants.TIMEX_TYPES_DATETIME}
    assert timex.year == 1984
    assert timex.month == 1
    assert timex.day_of_month == 3
    assert timex.day_of_week is None
    assert timex.week_of_year is None
    assert timex.week_of_month is None
    assert timex.season is None
    assert timex.hour == 18
    assert timex.minute == 30
    assert timex.second == 45
    assert timex.weekend is False
    assert timex.part_of_day is None
    assert timex.years is None
    assert timex.months is None
    assert timex.weeks is None
    assert timex.days is None
    assert timex.hours is None
    assert timex.minutes is None
    assert timex.seconds is None
    assert timex.now is False


def test_datatypes_parsing_paricular_time_on_particular_day_of_week():
    timex = Timex(timex='XXXX-WXX-3T16')
    assert timex.types == {
        Constants.TIMEX_TYPES_TIME,
        Constants.TIMEX_TYPES_DATE,
        Constants.TIMEX_TYPES_DATETIME}
    assert timex.year is None
    assert timex.month is None
    assert timex.day_of_month is None
    assert timex.day_of_week == 3
    assert timex.week_of_year is None
    assert timex.week_of_month is None
    assert timex.season is None
    assert timex.hour == 16
    assert timex.minute == 0
    assert timex.second == 0
    assert timex.weekend is False
    assert timex.part_of_day is None
    assert timex.years is None
    assert timex.months is None
    assert timex.weeks is None
    assert timex.days is None
    assert timex.hours is None
    assert timex.minutes is None
    assert timex.seconds is None
    assert timex.now is False


def test_datatypes_parsing_year():
    timex = Timex(timex='2016')
    assert timex.types == {Constants.TIMEX_TYPES_DATERANGE}
    assert timex.year == 2016
    assert timex.month is None
    assert timex.day_of_month is None
    assert timex.day_of_week is None
    assert timex.week_of_year is None
    assert timex.week_of_month is None
    assert timex.season is None
    assert timex.hour is None
    assert timex.minute is None
    assert timex.second is None
    assert timex.weekend is False
    assert timex.part_of_day is None
    assert timex.years is None
    assert timex.months is None
    assert timex.weeks is None
    assert timex.days is None
    assert timex.hours is None
    assert timex.minutes is None
    assert timex.seconds is None
    assert timex.now is False


def test_datatypes_parsing_summer_of_1999():
    timex = Timex(timex='1999-SU')
    assert timex.types == {Constants.TIMEX_TYPES_DATERANGE}
    assert timex.year == 1999
    assert timex.month is None
    assert timex.day_of_month is None
    assert timex.day_of_week is None
    assert timex.week_of_year is None
    assert timex.week_of_month is None
    assert timex.season == 'SU'
    assert timex.hour is None
    assert timex.minute is None
    assert timex.second is None
    assert timex.weekend is False
    assert timex.part_of_day is None
    assert timex.years is None
    assert timex.months is None
    assert timex.weeks is None
    assert timex.days is None
    assert timex.hours is None
    assert timex.minutes is None
    assert timex.seconds is None
    assert timex.now is False


def test_datatypes_parsing_year_and_week():
    timex = Timex(timex='2017-W37')
    assert timex.types == {Constants.TIMEX_TYPES_DATERANGE}
    assert timex.year == 2017
    assert timex.month is None
    assert timex.day_of_month is None
    assert timex.day_of_week is None
    assert timex.week_of_year == 37
    assert timex.week_of_month is None
    assert timex.season is None
    assert timex.hour is None
    assert timex.minute is None
    assert timex.second is None
    assert timex.weekend is False
    assert timex.part_of_day is None
    assert timex.years is None
    assert timex.months is None
    assert timex.weeks is None
    assert timex.days is None
    assert timex.hours is None
    assert timex.minutes is None
    assert timex.seconds is None
    assert timex.now is False


def test_datatypes_Parsing_SeasonSummer():
    timex = Timex(timex='SU')
    assert timex.types == {Constants.TIMEX_TYPES_DATERANGE}
    assert timex.year is None
    assert timex.month is None
    assert timex.day_of_month is None
    assert timex.day_of_week is None
    assert timex.week_of_year is None
    assert timex.week_of_month is None
    assert timex.season == 'SU'
    assert timex.hour is None
    assert timex.minute is None
    assert timex.second is None
    assert timex.weekend is False
    assert timex.part_of_day is None
    assert timex.years is None
    assert timex.months is None
    assert timex.weeks is None
    assert timex.days is None
    assert timex.hours is None
    assert timex.minutes is None
    assert timex.seconds is None
    assert timex.now is False


def test_datatypes_parsing_season_winter():
    timex = Timex(timex='WI')
    assert timex.types == {Constants.TIMEX_TYPES_DATERANGE}
    assert timex.year is None
    assert timex.month is None
    assert timex.day_of_month is None
    assert timex.day_of_week is None
    assert timex.week_of_year is None
    assert timex.week_of_month is None
    assert timex.season == 'WI'
    assert timex.hour is None
    assert timex.minute is None
    assert timex.second is None
    assert timex.weekend is False
    assert timex.part_of_day is None
    assert timex.years is None
    assert timex.months is None
    assert timex.weeks is None
    assert timex.days is None
    assert timex.hours is None
    assert timex.minutes is None
    assert timex.seconds is None
    assert timex.now is False


def test_datatypes_parsing_year_and_weekend():
    timex = Timex(timex='2017-W37-WE')
    assert timex.types == {Constants.TIMEX_TYPES_DATERANGE}
    assert timex.year == 2017
    assert timex.month is None
    assert timex.day_of_month is None
    assert timex.day_of_week is None
    assert timex.week_of_year == 37
    assert timex.week_of_month is None
    assert timex.season is None
    assert timex.hour is None
    assert timex.minute is None
    assert timex.second is None
    assert timex.weekend is True
    assert timex.part_of_day is None
    assert timex.years is None
    assert timex.months is None
    assert timex.weeks is None
    assert timex.days is None
    assert timex.hours is None
    assert timex.minutes is None
    assert timex.seconds is None
    assert timex.now is False


def test_datatypes_parsing_may():
    timex = Timex(timex='XXXX-05')
    assert timex.types == {Constants.TIMEX_TYPES_DATERANGE}
    assert timex.year is None
    assert timex.month == 5
    assert timex.day_of_month is None
    assert timex.day_of_week is None
    assert timex.week_of_year is None
    assert timex.week_of_month is None
    assert timex.season is None
    assert timex.hour is None
    assert timex.minute is None
    assert timex.second is None
    assert timex.weekend is False
    assert timex.part_of_day is None
    assert timex.years is None
    assert timex.months is None
    assert timex.weeks is None
    assert timex.days is None
    assert timex.hours is None
    assert timex.minutes is None
    assert timex.seconds is None
    assert timex.now is False


def test_datatypes_parsing_july_2020():
    timex = Timex(timex='2020-07')
    assert timex.types == {Constants.TIMEX_TYPES_DATERANGE}
    assert timex.year == 2020
    assert timex.month == 7
    assert timex.day_of_month is None
    assert timex.day_of_week is None
    assert timex.week_of_year is None
    assert timex.week_of_month is None
    assert timex.season is None
    assert timex.hour is None
    assert timex.minute is None
    assert timex.second is None
    assert timex.weekend is False
    assert timex.part_of_day is None
    assert timex.years is None
    assert timex.months is None
    assert timex.weeks is None
    assert timex.days is None
    assert timex.hours is None
    assert timex.minutes is None
    assert timex.seconds is None
    assert timex.now is False


def test_datatypes_parsing_week_of_month():
    timex = Timex(timex='XXXX-01-W01')
    assert timex.types == {Constants.TIMEX_TYPES_DATERANGE}
    assert timex.year is None
    assert timex.month == 1
    assert timex.day_of_month is None
    assert timex.day_of_week is None
    assert timex.week_of_year is None
    assert timex.week_of_month == 1
    assert timex.season is None
    assert timex.hour is None
    assert timex.minute is None
    assert timex.second is None
    assert timex.weekend is False
    assert timex.part_of_day is None
    assert timex.years is None
    assert timex.months is None
    assert timex.weeks is None
    assert timex.days is None
    assert timex.hours is None
    assert timex.minutes is None
    assert timex.seconds is None
    assert timex.now is False


def test_datatypes_parsing_wednesday_to_saturday():
    timex = Timex(timex='(XXXX-WXX-3,XXXX-WXX-6,P3D)')
    assert timex.types == {
        Constants.TIMEX_TYPES_DATE,
        Constants.TIMEX_TYPES_DURATION,
        Constants.TIMEX_TYPES_DATERANGE}
    assert timex.year is None
    assert timex.month is None
    assert timex.day_of_month is None
    assert timex.day_of_week == 3
    assert timex.week_of_year is None
    assert timex.week_of_month is None
    assert timex.season is None
    assert timex.hour is None
    assert timex.minute is None
    assert timex.second is None
    assert timex.weekend is False
    assert timex.part_of_day is None
    assert timex.years is None
    assert timex.months is None
    assert timex.weeks is None
    assert timex.days == 3
    assert timex.hours is None
    assert timex.minutes is None
    assert timex.seconds is None
    assert timex.now is False


def test_datatypes_parsing_jan_1_to_aug_5():
    timex = Timex(timex='(XXXX-01-01,XXXX-08-05,P216D)')
    assert timex.types == {
        Constants.TIMEX_TYPES_DATE,
        Constants.TIMEX_TYPES_DURATION,
        Constants.TIMEX_TYPES_DATERANGE}
    assert timex.year is None
    assert timex.month == 1
    assert timex.day_of_month == 1
    assert timex.day_of_week is None
    assert timex.week_of_year is None
    assert timex.week_of_month is None
    assert timex.season is None
    assert timex.hour is None
    assert timex.minute is None
    assert timex.second is None
    assert timex.weekend is False
    assert timex.part_of_day is None
    assert timex.years is None
    assert timex.months is None
    assert timex.weeks is None
    assert timex.days == 216
    assert timex.hours is None
    assert timex.minutes is None
    assert timex.seconds is None
    assert timex.now is False


def test_datatypes_parsing_jan_1_to_aug_5_year_2015():
    timex = Timex(timex='(2015-01-01,2015-08-05,P216D)')
    assert timex.types == {
        Constants.TIMEX_TYPES_DEFINITE,
        Constants.TIMEX_TYPES_DATE,
        Constants.TIMEX_TYPES_DURATION,
        Constants.TIMEX_TYPES_DATERANGE}
    assert timex.year == 2015
    assert timex.month == 1
    assert timex.day_of_month == 1
    assert timex.day_of_week is None
    assert timex.week_of_year is None
    assert timex.week_of_month is None
    assert timex.season is None
    assert timex.hour is None
    assert timex.minute is None
    assert timex.second is None
    assert timex.weekend is False
    assert timex.part_of_day is None
    assert timex.years is None
    assert timex.months is None
    assert timex.weeks is None
    assert timex.days == 216
    assert timex.hours is None
    assert timex.minutes is None
    assert timex.seconds is None
    assert timex.now is False


def test_datatypes_parsing_dayTime():
    timex = Timex(timex='TDT')
    assert timex.types == {Constants.TIMEX_TYPES_TIMERANGE}
    assert timex.year is None
    assert timex.month is None
    assert timex.day_of_month is None
    assert timex.day_of_week is None
    assert timex.week_of_year is None
    assert timex.week_of_month is None
    assert timex.season is None
    assert timex.hour is None
    assert timex.minute is None
    assert timex.second is None
    assert timex.weekend is False
    assert timex.part_of_day == 'DT'
    assert timex.years is None
    assert timex.months is None
    assert timex.weeks is None
    assert timex.days is None
    assert timex.hours is None
    assert timex.minutes is None
    assert timex.seconds is None
    assert timex.now is False


def test_datatypes_parsing_nighttime():
    timex = Timex(timex='TNI')
    assert timex.types == {Constants.TIMEX_TYPES_TIMERANGE}
    assert timex.year is None
    assert timex.month is None
    assert timex.day_of_month is None
    assert timex.day_of_week is None
    assert timex.week_of_year is None
    assert timex.week_of_month is None
    assert timex.season is None
    assert timex.hour is None
    assert timex.minute is None
    assert timex.second is None
    assert timex.weekend is False
    assert timex.part_of_day == 'NI'
    assert timex.years is None
    assert timex.months is None
    assert timex.weeks is None
    assert timex.days is None
    assert timex.hours is None
    assert timex.minutes is None
    assert timex.seconds is None
    assert timex.now is False


def test_datatypes_parsing_morning():
    timex = Timex(timex='TMO')
    assert timex.types == {Constants.TIMEX_TYPES_TIMERANGE}
    assert timex.year is None
    assert timex.month is None
    assert timex.day_of_month is None
    assert timex.day_of_week is None
    assert timex.week_of_year is None
    assert timex.week_of_month is None
    assert timex.season is None
    assert timex.hour is None
    assert timex.minute is None
    assert timex.second is None
    assert timex.weekend is False
    assert timex.part_of_day == 'MO'
    assert timex.years is None
    assert timex.months is None
    assert timex.weeks is None
    assert timex.days is None
    assert timex.hours is None
    assert timex.minutes is None
    assert timex.seconds is None
    assert timex.now is False


def test_datatypes_parsing_afternoon():
    timex = Timex(timex='TAF')
    assert timex.types == {Constants.TIMEX_TYPES_TIMERANGE}
    assert timex.year is None
    assert timex.month is None
    assert timex.day_of_month is None
    assert timex.day_of_week is None
    assert timex.week_of_year is None
    assert timex.week_of_month is None
    assert timex.season is None
    assert timex.hour is None
    assert timex.minute is None
    assert timex.second is None
    assert timex.weekend is False
    assert timex.part_of_day == 'AF'
    assert timex.years is None
    assert timex.months is None
    assert timex.weeks is None
    assert timex.days is None
    assert timex.hours is None
    assert timex.minutes is None
    assert timex.seconds is None
    assert timex.now is False


def test_datatypes_parsing_evening():
    timex = Timex(timex='TEV')
    assert timex.types == {Constants.TIMEX_TYPES_TIMERANGE}
    assert timex.year is None
    assert timex.month is None
    assert timex.day_of_month is None
    assert timex.day_of_week is None
    assert timex.week_of_year is None
    assert timex.week_of_month is None
    assert timex.season is None
    assert timex.hour is None
    assert timex.minute is None
    assert timex.second is None
    assert timex.weekend is False
    assert timex.part_of_day == 'EV'
    assert timex.years is None
    assert timex.months is None
    assert timex.weeks is None
    assert timex.days is None
    assert timex.hours is None
    assert timex.minutes is None
    assert timex.seconds is None
    assert timex.now is False


def test_datatypes_parsing_timerange_430pm_to_445pm():
    timex = Timex(timex='(T16:30,T16:45,PT15M)')
    assert timex.types == {
        Constants.TIMEX_TYPES_TIME,
        Constants.TIMEX_TYPES_DURATION,
        Constants.TIMEX_TYPES_TIMERANGE}
    assert timex.year is None
    assert timex.month is None
    assert timex.day_of_month is None
    assert timex.day_of_week is None
    assert timex.week_of_year is None
    assert timex.week_of_month is None
    assert timex.season is None
    assert timex.hour == 16
    assert timex.minute == 30
    assert timex.second == 0
    assert timex.weekend is False
    assert timex.part_of_day is None
    assert timex.years is None
    assert timex.months is None
    assert timex.weeks is None
    assert timex.days is None
    assert timex.hours is None
    assert timex.minutes == 15
    assert timex.seconds is None
    assert timex.now is False


def test_datatypes_parsing_datetimerange():
    timex = Timex(timex='XXXX-WXX-5TEV')
    assert timex.types == {
        Constants.TIMEX_TYPES_DATE,
        Constants.TIMEX_TYPES_TIMERANGE,
        Constants.TIMEX_TYPES_DATETIMERANGE}
    assert timex.year is None
    assert timex.month is None
    assert timex.day_of_month is None
    assert timex.day_of_week == 5
    assert timex.week_of_year is None
    assert timex.week_of_month is None
    assert timex.season is None
    assert timex.hour is None
    assert timex.minute is None
    assert timex.second is None
    assert timex.weekend is False
    assert timex.part_of_day == 'EV'
    assert timex.years is None
    assert timex.months is None
    assert timex.weeks is None
    assert timex.days is None
    assert timex.hours is None
    assert timex.minutes is None
    assert timex.seconds is None
    assert timex.now is False


def test_datatypes_parsing_lastnight():
    timex = Timex(timex='2017-09-07TNI')
    assert timex.types == {
        Constants.TIMEX_TYPES_DEFINITE,
        Constants.TIMEX_TYPES_DATE,
        Constants.TIMEX_TYPES_TIMERANGE,
        Constants.TIMEX_TYPES_DATETIMERANGE}
    assert timex.year == 2017
    assert timex.month == 9
    assert timex.day_of_month == 7
    assert timex.day_of_week is None
    assert timex.week_of_year is None
    assert timex.week_of_month is None
    assert timex.season is None
    assert timex.hour is None
    assert timex.minute is None
    assert timex.second is None
    assert timex.weekend is False
    assert timex.part_of_day == 'NI'
    assert timex.years is None
    assert timex.months is None
    assert timex.weeks is None
    assert timex.days is None
    assert timex.hours is None
    assert timex.minutes is None
    assert timex.seconds is None
    assert timex.now is False


def test_datatypes_parsing_last_5_minutes():
    timex = Timex(timex='(2017-09-08T21:19:29,2017-09-08T21:24:29,PT5M)')
    assert timex.types == {
        Constants.TIMEX_TYPES_DATE,
        Constants.TIMEX_TYPES_TIMERANGE,
        Constants.TIMEX_TYPES_DATETIMERANGE,
        Constants.TIMEX_TYPES_TIME,
        Constants.TIMEX_TYPES_DATETIME,
        Constants.TIMEX_TYPES_DURATION,
        Constants.TIMEX_TYPES_DATERANGE,
        Constants.TIMEX_TYPES_DEFINITE}
    assert timex.year == 2017
    assert timex.month == 9
    assert timex.day_of_month == 8
    assert timex.day_of_week is None
    assert timex.week_of_year is None
    assert timex.week_of_month is None
    assert timex.season is None
    assert timex.hour == 21
    assert timex.minute == 19
    assert timex.second == 29
    assert timex.weekend is False
    assert timex.part_of_day is None
    assert timex.years is None
    assert timex.months is None
    assert timex.weeks is None
    assert timex.days is None
    assert timex.hours is None
    assert timex.minutes == 5
    assert timex.seconds is None
    assert timex.now is False


def test_datatypes_parsing_wed_4PM_to_sat_3PM():
    timex = Timex(timex='(XXXX-WXX-3T16,XXXX-WXX-6T15,PT71H)')
    assert timex.types == {
        Constants.TIMEX_TYPES_DATE,
        Constants.TIMEX_TYPES_TIMERANGE,
        Constants.TIMEX_TYPES_DATETIMERANGE,
        Constants.TIMEX_TYPES_TIME,
        Constants.TIMEX_TYPES_DATETIME,
        Constants.TIMEX_TYPES_DURATION,
        Constants.TIMEX_TYPES_DATERANGE}
    assert timex.year is None
    assert timex.month is None
    assert timex.day_of_month is None
    assert timex.day_of_week == 3
    assert timex.week_of_year is None
    assert timex.week_of_month is None
    assert timex.season is None
    assert timex.hour == 16
    assert timex.minute == 0
    assert timex.second == 0
    assert timex.weekend is False
    assert timex.part_of_day is None
    assert timex.years is None
    assert timex.months is None
    assert timex.weeks is None
    assert timex.days is None
    assert timex.hours == 71
    assert timex.minutes is None
    assert timex.seconds is None
    assert timex.now is False


def test_datatypes_parsing_duration_years():
    timex = Timex(timex='P2Y')
#   assert timex.types == { Constants.TIMEX_TYPES_DURATION }
    assert timex.year is None
    assert timex.month is None
    assert timex.day_of_month is None
    assert timex.day_of_week is None
    assert timex.week_of_year is None
    assert timex.week_of_month is None
    assert timex.season is None
    assert timex.hour is None
    assert timex.minute is None
    assert timex.second is None
    assert timex.weekend is False
    assert timex.part_of_day is None
    assert timex.years == 2
    assert timex.months is None
    assert timex.weeks is None
    assert timex.days is None
    assert timex.hours is None
    assert timex.minutes is None
    assert timex.seconds is None
    assert timex.now is False


def test_datatypes_parsing_duration_months():
    timex = Timex(timex='P4M')
    assert timex.types == {Constants.TIMEX_TYPES_DURATION}
    assert timex.year is None
    assert timex.month is None
    assert timex.day_of_month is None
    assert timex.day_of_week is None
    assert timex.week_of_year is None
    assert timex.week_of_month is None
    assert timex.season is None
    assert timex.hour is None
    assert timex.minute is None
    assert timex.second is None
    assert timex.weekend is False
    assert timex.part_of_day is None
    assert timex.years is None
    assert timex.months == 4
    assert timex.weeks is None
    assert timex.days is None
    assert timex.hours is None
    assert timex.minutes is None
    assert timex.seconds is None
    assert timex.now is False


def test_datatypes_parsing_duration_weeks():
    timex = Timex(timex='P6W')
    assert timex.types == {Constants.TIMEX_TYPES_DURATION}
    assert timex.year is None
    assert timex.month is None
    assert timex.day_of_month is None
    assert timex.day_of_week is None
    assert timex.week_of_year is None
    assert timex.week_of_month is None
    assert timex.season is None
    assert timex.hour is None
    assert timex.minute is None
    assert timex.second is None
    assert timex.weekend is False
    assert timex.part_of_day is None
    assert timex.years is None
    assert timex.months is None
    assert timex.weeks == 6
    assert timex.days is None
    assert timex.hours is None
    assert timex.minutes is None
    assert timex.seconds is None
    assert timex.now is False


def test_datatypes_parsing_duration_weeks_floating_point():
    timex = Timex(timex='P2.5W')
    assert timex.types == {Constants.TIMEX_TYPES_DURATION}
    assert timex.year is None
    assert timex.month is None
    assert timex.day_of_month is None
    assert timex.day_of_week is None
    assert timex.week_of_year is None
    assert timex.week_of_month is None
    assert timex.season is None
    assert timex.hour is None
    assert timex.minute is None
    assert timex.second is None
    assert timex.weekend is False
    assert timex.part_of_day is None
    assert timex.years is None
    assert timex.months is None
    assert timex.weeks == 2.5
    assert timex.days is None
    assert timex.hours is None
    assert timex.minutes is None
    assert timex.seconds is None
    assert timex.now is False


def test_datatypes_parsing_duration_days():
    timex = Timex(timex='P1D')
    assert timex.types == {Constants.TIMEX_TYPES_DURATION}
    assert timex.year is None
    assert timex.month is None
    assert timex.day_of_month is None
    assert timex.day_of_week is None
    assert timex.week_of_year is None
    assert timex.week_of_month is None
    assert timex.season is None
    assert timex.hour is None
    assert timex.minute is None
    assert timex.second is None
    assert timex.weekend is False
    assert timex.part_of_day is None
    assert timex.years is None
    assert timex.months is None
    assert timex.weeks is None
    assert timex.days == 1
    assert timex.hours is None
    assert timex.minutes is None
    assert timex.seconds is None
    assert timex.now is False


def test_datatypes_parsing_duration_hours():
    timex = Timex(timex='PT5H')
    assert timex.types == {Constants.TIMEX_TYPES_DURATION}
    assert timex.year is None
    assert timex.month is None
    assert timex.day_of_month is None
    assert timex.day_of_week is None
    assert timex.week_of_year is None
    assert timex.week_of_month is None
    assert timex.season is None
    assert timex.hour is None
    assert timex.minute is None
    assert timex.second is None
    assert timex.weekend is False
    assert timex.part_of_day is None
    assert timex.years is None
    assert timex.months is None
    assert timex.weeks is None
    assert timex.days is None
    assert timex.hours == 5
    assert timex.minutes is None
    assert timex.seconds is None
    assert timex.now is False


def test_datatypes_Parsing_DurationMinutes():
    timex = Timex(timex='PT30M')
    assert timex.types == {Constants.TIMEX_TYPES_DURATION}
    assert timex.year is None
    assert timex.month is None
    assert timex.day_of_month is None
    assert timex.day_of_week is None
    assert timex.week_of_year is None
    assert timex.week_of_month is None
    assert timex.season is None
    assert timex.hour is None
    assert timex.minute is None
    assert timex.second is None
    assert timex.weekend is False
    assert timex.part_of_day is None
    assert timex.years is None
    assert timex.months is None
    assert timex.weeks is None
    assert timex.days is None
    assert timex.hours is None
    assert timex.minutes == 30
    assert timex.seconds is None
    assert timex.now is False


def test_datatypes_Parsing_DurationSeconds():
    timex = Timex(timex='PT45S')
    assert timex.types == {Constants.TIMEX_TYPES_DURATION}
    assert timex.year is None
    assert timex.month is None
    assert timex.day_of_month is None
    assert timex.day_of_week is None
    assert timex.week_of_year is None
    assert timex.week_of_month is None
    assert timex.season is None
    assert timex.hour is None
    assert timex.minute is None
    assert timex.second is None
    assert timex.weekend is False
    assert timex.part_of_day is None
    assert timex.years is None
    assert timex.months is None
    assert timex.weeks is None
    assert timex.days is None
    assert timex.hours is None
    assert timex.minutes is None
    assert timex.seconds == 45
    assert timex.now is False
