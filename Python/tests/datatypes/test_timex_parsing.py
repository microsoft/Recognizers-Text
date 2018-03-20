
from datatypes_timex_expression import Timex, Constants

def test_timex_parsing_complete_date():
  timex = Timex('2017-05-29')
  assert timex.types == { Constants.TIMEX_TYPES_DEFINITE, Constants.TIMEX_TYPES_DATE }
  assert timex.year == 2017
  assert timex.month == 5
  assert timex.day_of_month == 29
  assert timex.day_of_week == None
  assert timex.week_of_year == None
  assert timex.week_of_month == None
  assert timex.season == None
  assert timex.hour == None
  assert timex.minute == None
  assert timex.second == None
  assert timex.weekend == None
  assert timex.part_of_day == None
  assert timex.years == None
  assert timex.months == None
  assert timex.weeks == None
  assert timex.days == None
  assert timex.hours == None
  assert timex.minutes == None
  assert timex.seconds == None
  assert timex.now == None

def test_timex_parsing_month_and_ady_of_month():
  timex = Timex('XXXX-12-05')
  assert timex.types == { Constants.TIMEX_TYPES_DATE }
  assert timex.year == None
  assert timex.month == 12
  assert timex.day_of_month == 5
  assert timex.day_of_week == None
  assert timex.week_of_year == None
  assert timex.week_of_month == None
  assert timex.season == None
  assert timex.hour == None
  assert timex.minute == None
  assert timex.second == None
  assert timex.weekend == None
  assert timex.part_of_day == None
  assert timex.years == None
  assert timex.months == None
  assert timex.weeks == None
  assert timex.days == None
  assert timex.hours == None
  assert timex.minutes == None
  assert timex.seconds == None
  assert timex.now == None

def test_dataTypes_parsing_day_of_week():
  timex = Timex('XXXX-WXX-3')
  assert timex.types == { Constants.TIMEX_TYPES_DATE }
  assert timex.year == None
  assert timex.month == None
  assert timex.day_of_month == None
  assert timex.day_of_week == 3
  assert timex.week_of_year == None
  assert timex.week_of_month == None
  assert timex.season == None
  assert timex.hour == None
  assert timex.minute == None
  assert timex.second == None
  assert timex.weekend == None
  assert timex.part_of_day == None
  assert timex.years == None
  assert timex.months == None
  assert timex.weeks == None
  assert timex.days == None
  assert timex.hours == None
  assert timex.minutes == None
  assert timex.seconds == None
  assert timex.now == None

def test_datatypes_parsing_hours_minutes_and_seconds():
  timex = Timex('T17:30:05')
  assert timex.types == { Constants.TIMEX_TYPES_TIME }
  assert timex.year == None
  assert timex.month == None
  assert timex.day_of_month == None
  assert timex.day_of_week == None
  assert timex.week_of_year == None
  assert timex.week_of_month == None
  assert timex.season == None
  assert timex.hour == 17
  assert timex.minute == 30
  assert timex.second == 5
  assert timex.weekend == None
  assert timex.part_of_day == None
  assert timex.years == None
  assert timex.months == None
  assert timex.weeks == None
  assert timex.days == None
  assert timex.hours == None
  assert timex.minutes == None
  assert timex.seconds == None
  assert timex.now == None

def test_dataTypes_parsing_hours_and_minutes():
  timex = Timex('T17:30')
  assert timex.types == { Constants.TIMEX_TYPES_TIME }
  assert timex.year == None
  assert timex.month == None
  assert timex.day_of_month == None
  assert timex.day_of_week == None
  assert timex.week_of_year == None
  assert timex.week_of_month == None
  assert timex.season == None
  assert timex.hour == 17
  assert timex.minute == 30
  assert timex.second == 0
  assert timex.weekend == None
  assert timex.part_of_day == None
  assert timex.years == None
  assert timex.months == None
  assert timex.weeks == None
  assert timex.days == None
  assert timex.hours == None
  assert timex.minutes == None
  assert timex.seconds == None
  assert timex.now == None

def test_dataTypes_parsing_hours():
  timex = Timex('T17')
  assert timex.types == { Constants.TIMEX_TYPES_TIME }
  assert timex.year == None
  assert timex.month == None
  assert timex.day_of_month == None
  assert timex.day_of_week == None
  assert timex.week_of_year == None
  assert timex.week_of_month == None
  assert timex.season == None
  assert timex.hour == 17
  assert timex.minute == 0
  assert timex.second == 0
  assert timex.weekend == None
  assert timex.part_of_day == None
  assert timex.years == None
  assert timex.months == None
  assert timex.weeks == None
  assert timex.days == None
  assert timex.hours == None
  assert timex.minutes == None
  assert timex.seconds == None
  assert timex.now == None

def test_dataTypes_parsing_now():
  timex = Timex('PRESENT_REF')
  assert timex.types == {
    Constants.TIMEX_TYPES_PRESENT,
    Constants.TIMEX_TYPES_DATE,
    Constants.TIMEX_TYPES_TIME,
    Constants.TIMEX_TYPES_DATETIME }
  assert timex.year == None
  assert timex.month == None
  assert timex.day_of_month == None
  assert timex.day_of_week == None
  assert timex.week_of_year == None
  assert timex.week_of_month == None
  assert timex.season == None
  assert timex.hour == None
  assert timex.minute == None
  assert timex.second == None
  assert timex.weekend == None
  assert timex.part_of_day == None
  assert timex.years == None
  assert timex.months == None
  assert timex.weeks == None
  assert timex.days == None
  assert timex.hours == None
  assert timex.minutes == None
  assert timex.seconds == None
  assert timex.now == True

def test_datatypes_parsing_full_datetime():
  timex = Timex('1984-01-03T18:30:45')
  assert timex.types == {
    Constants.TIMEX_TYPES_DEFINITE,
    Constants.TIMEX_TYPES_DATE,
    Constants.TIMEX_TYPES_TIME,
    Constants.TIMEX_TYPES_DATETIME }
  assert timex.year == 1984
  assert timex.month == 1
  assert timex.day_of_month == 3
  assert timex.day_of_week == None
  assert timex.week_of_year == None
  assert timex.week_of_month == None
  assert timex.season == None
  assert timex.hour == 18
  assert timex.minute == 30
  assert timex.second == 45
  assert timex.weekend == None
  assert timex.part_of_day == None
  assert timex.years == None
  assert timex.months == None
  assert timex.weeks == None
  assert timex.days == None
  assert timex.hours == None
  assert timex.minutes == None
  assert timex.seconds == None
  assert timex.now == None

def test_datatypes_parsing_paricular_time_on_particular_day_of_week():
  timex = Timex('XXXX-WXX-3T16')
  assert timex.types == { 
      Constants.TIMEX_TYPES_TIME,
      Constants.TIMEX_TYPES_DATE,
      Constants.TIMEX_TYPES_DATETIME }
  assert timex.year == None
  assert timex.month == None
  assert timex.day_of_month == None
  assert timex.day_of_week == 3
  assert timex.week_of_year == None
  assert timex.week_of_month == None
  assert timex.season == None
  assert timex.hour == 16
  assert timex.minute == 0
  assert timex.second == 0
  assert timex.weekend == None
  assert timex.part_of_day == None
  assert timex.years == None
  assert timex.months == None
  assert timex.weeks == None
  assert timex.days == None
  assert timex.hours == None
  assert timex.minutes == None
  assert timex.seconds == None
  assert timex.now == None

def test_dataTypes_parsing_year():
  timex = Timex('2016')
  assert timex.types == { Constants.TIMEX_TYPES_DATERANGE }
  assert timex.year == 2016
  assert timex.month == None
  assert timex.day_of_month == None
  assert timex.day_of_week == None
  assert timex.week_of_year == None
  assert timex.week_of_month == None
  assert timex.season == None
  assert timex.hour == None
  assert timex.minute == None
  assert timex.second == None
  assert timex.weekend == None
  assert timex.part_of_day == None
  assert timex.years == None
  assert timex.months == None
  assert timex.weeks == None
  assert timex.days == None
  assert timex.hours == None
  assert timex.minutes == None
  assert timex.seconds == None
  assert timex.now == None

def test_datatypes_parsing_summer_of_1999():
  timex = Timex('1999-SU')
  assert timex.types == { Constants.TIMEX_TYPES_DATERANGE }
  assert timex.year == 1999
  assert timex.month == None
  assert timex.day_of_month == None
  assert timex.day_of_week == None
  assert timex.week_of_year == None
  assert timex.week_of_month == None
  assert timex.season == 'SU'
  assert timex.hour == None
  assert timex.minute == None
  assert timex.second == None
  assert timex.weekend == None
  assert timex.part_of_day == None
  assert timex.years == None
  assert timex.months == None
  assert timex.weeks == None
  assert timex.days == None
  assert timex.hours == None
  assert timex.minutes == None
  assert timex.seconds == None
  assert timex.now == None

def test_datatypes_parsing_year_and_week():
  timex = Timex('2017-W37')
  assert timex.types == { Constants.TIMEX_TYPES_DATERANGE }
  assert timex.year == 2017
  assert timex.month == None
  assert timex.day_of_month == None
  assert timex.day_of_week == None
  assert timex.week_of_year == 37
  assert timex.week_of_month == None
  assert timex.season == None
  assert timex.hour == None
  assert timex.minute == None
  assert timex.second == None
  assert timex.weekend == None
  assert timex.part_of_day == None
  assert timex.years == None
  assert timex.months == None
  assert timex.weeks == None
  assert timex.days == None
  assert timex.hours == None
  assert timex.minutes == None
  assert timex.seconds == None
  assert timex.now == None

def test_datatypes_Parsing_SeasonSummer():
  timex = Timex('SU')
  assert timex.types == { Constants.TIMEX_TYPES_DATERANGE }
  assert timex.year == None
  assert timex.month == None
  assert timex.day_of_month == None
  assert timex.day_of_week == None
  assert timex.week_of_year == None
  assert timex.week_of_month == None
  assert timex.season == 'SU'
  assert timex.hour == None
  assert timex.minute == None
  assert timex.second == None
  assert timex.weekend == None
  assert timex.part_of_day == None
  assert timex.years == None
  assert timex.months == None
  assert timex.weeks == None
  assert timex.days == None
  assert timex.hours == None
  assert timex.minutes == None
  assert timex.seconds == None
  assert timex.now == None

def test_datatypes_parsing_season_winter():
  timex = Timex('WI')
  assert timex.types == { Constants.TIMEX_TYPES_DATERANGE }
  assert timex.year == None
  assert timex.month == None
  assert timex.day_of_month == None
  assert timex.day_of_week == None
  assert timex.week_of_year == None
  assert timex.week_of_month == None
  assert timex.season == 'WI'
  assert timex.hour == None
  assert timex.minute == None
  assert timex.second == None
  assert timex.weekend == None
  assert timex.part_of_day == None
  assert timex.years == None
  assert timex.months == None
  assert timex.weeks == None
  assert timex.days == None
  assert timex.hours == None
  assert timex.minutes == None
  assert timex.seconds == None
  assert timex.now == None

def test_datatypes_parsing_year_and_weekend():
  timex = Timex('2017-W37-WE')
  assert timex.types == { Constants.TIMEX_TYPES_DATERANGE }
  assert timex.year == 2017
  assert timex.month == None
  assert timex.day_of_month == None
  assert timex.day_of_week == None
  assert timex.week_of_year == 37
  assert timex.week_of_month == None
  assert timex.season == None
  assert timex.hour == None
  assert timex.minute == None
  assert timex.second == None
  assert timex.weekend == True
  assert timex.part_of_day == None
  assert timex.years == None
  assert timex.months == None
  assert timex.weeks == None
  assert timex.days == None
  assert timex.hours == None
  assert timex.minutes == None
  assert timex.seconds == None
  assert timex.now == None

def test_datatypes_parsing_may():
  timex = Timex('XXXX-05')
  assert timex.types == { Constants.TIMEX_TYPES_DATERANGE }
  assert timex.year == None
  assert timex.month == 5
  assert timex.day_of_month == None
  assert timex.day_of_week == None
  assert timex.week_of_year == None
  assert timex.week_of_month == None
  assert timex.season == None
  assert timex.hour == None
  assert timex.minute == None
  assert timex.second == None
  assert timex.weekend == None
  assert timex.part_of_day == None
  assert timex.years == None
  assert timex.months == None
  assert timex.weeks == None
  assert timex.days == None
  assert timex.hours == None
  assert timex.minutes == None
  assert timex.seconds == None
  assert timex.now == None

def test_datatypes_parsing_july_2020():
  timex = Timex('2020-07')
  assert timex.types == { Constants.TIMEX_TYPES_DATERANGE }
  assert timex.year == 2020
  assert timex.month == 7
  assert timex.day_of_month == None
  assert timex.day_of_week == None
  assert timex.week_of_year == None
  assert timex.week_of_month == None
  assert timex.season == None
  assert timex.hour == None
  assert timex.minute == None
  assert timex.second == None
  assert timex.weekend == None
  assert timex.part_of_day == None
  assert timex.years == None
  assert timex.months == None
  assert timex.weeks == None
  assert timex.days == None
  assert timex.hours == None
  assert timex.minutes == None
  assert timex.seconds == None
  assert timex.now == None

def test_datatypes_parsing_week_of_month():
  timex = Timex('XXXX-01-W01')
  assert timex.types == { Constants.TIMEX_TYPES_DATERANGE }
  assert timex.year == None
  assert timex.month == 1
  assert timex.day_of_month == None
  assert timex.day_of_week == None
  assert timex.week_of_year == None
  assert timex.week_of_month == 1
  assert timex.season == None
  assert timex.hour == None
  assert timex.minute == None
  assert timex.second == None
  assert timex.weekend == None
  assert timex.part_of_day == None
  assert timex.years == None
  assert timex.months == None
  assert timex.weeks == None
  assert timex.days == None
  assert timex.hours == None
  assert timex.minutes == None
  assert timex.seconds == None
  assert timex.now == None

def test_datatypes_parsing_wednesday_to_saturday():
  timex = Timex('(XXXX-WXX-3,XXXX-WXX-6,P3D)')
  assert timex.types == {
    Constants.TIMEX_TYPES_DATE,
    Constants.TIMEX_TYPES_DURATION,
    Constants.TIMEX_TYPES_DATERANGE }
  assert timex.year == None
  assert timex.month == None
  assert timex.day_of_month == None
  assert timex.day_of_week == 3
  assert timex.week_of_year == None
  assert timex.week_of_month == None
  assert timex.season == None
  assert timex.hour == None
  assert timex.minute == None
  assert timex.second == None
  assert timex.weekend == None
  assert timex.part_of_day == None
  assert timex.years == None
  assert timex.months == None
  assert timex.weeks == None
  assert timex.days == 3
  assert timex.hours == None
  assert timex.minutes == None
  assert timex.seconds == None
  assert timex.now == None

def test_datatypes_parsing_jan_1_to_aug_5():
  timex = Timex('(XXXX-01-01,XXXX-08-05,P216D)')
  assert timex.types == {
    Constants.TIMEX_TYPES_DATE,
    Constants.TIMEX_TYPES_DURATION,
    Constants.TIMEX_TYPES_DATERANGE }
  assert timex.year == None
  assert timex.month == 1
  assert timex.day_of_month == 1
  assert timex.day_of_week == None
  assert timex.week_of_year == None
  assert timex.week_of_month == None
  assert timex.season == None
  assert timex.hour == None
  assert timex.minute == None
  assert timex.second == None
  assert timex.weekend == None
  assert timex.part_of_day == None
  assert timex.years == None
  assert timex.months == None
  assert timex.weeks == None
  assert timex.days == 216
  assert timex.hours == None
  assert timex.minutes == None
  assert timex.seconds == None
  assert timex.now == None

def test_datatypes_parsing_jan_1_to_aug_5_year_2015():
  timex = Timex('(2015-01-01,2015-08-05,P216D)')
  assert timex.types == {
    Constants.TIMEX_TYPES_DEFINITE,
    Constants.TIMEX_TYPES_DATE,
    Constants.TIMEX_TYPES_DURATION,
    Constants.TIMEX_TYPES_DATERANGE }
  assert timex.year == 2015
  assert timex.month == 1
  assert timex.day_of_month == 1
  assert timex.day_of_week == None
  assert timex.week_of_year == None
  assert timex.week_of_month == None
  assert timex.season == None
  assert timex.hour == None
  assert timex.minute == None
  assert timex.second == None
  assert timex.weekend == None
  assert timex.part_of_day == None
  assert timex.years == None
  assert timex.months == None
  assert timex.weeks == None
  assert timex.days == 216
  assert timex.hours == None
  assert timex.minutes == None
  assert timex.seconds == None
  assert timex.now == None

def test_datatypes_parsing_dayTime():
  timex = Timex('TDT')
  assert timex.types == { Constants.TIMEX_TYPES_TIMERANGE }
  assert timex.year == None
  assert timex.month == None
  assert timex.day_of_month == None
  assert timex.day_of_week == None
  assert timex.week_of_year == None
  assert timex.week_of_month == None
  assert timex.season == None
  assert timex.hour == None
  assert timex.minute == None
  assert timex.second == None
  assert timex.weekend == None
  assert timex.part_of_day == 'DT'
  assert timex.years == None
  assert timex.months == None
  assert timex.weeks == None
  assert timex.days == None
  assert timex.hours == None
  assert timex.minutes == None
  assert timex.seconds == None
  assert timex.now == None

def test_datatypes_parsing_nighttime():
  timex = Timex('TNI')
  assert timex.types == { Constants.TIMEX_TYPES_TIMERANGE }
  assert timex.year == None
  assert timex.month == None
  assert timex.day_of_month == None
  assert timex.day_of_week == None
  assert timex.week_of_year == None
  assert timex.week_of_month == None
  assert timex.season == None
  assert timex.hour == None
  assert timex.minute == None
  assert timex.second == None
  assert timex.weekend == None
  assert timex.part_of_day == 'NI'
  assert timex.years == None
  assert timex.months == None
  assert timex.weeks == None
  assert timex.days == None
  assert timex.hours == None
  assert timex.minutes == None
  assert timex.seconds == None
  assert timex.now == None

def test_datatypes_parsing_morning():
  timex = Timex('TMO')
  assert timex.types == { Constants.TIMEX_TYPES_TIMERANGE }
  assert timex.year == None
  assert timex.month == None
  assert timex.day_of_month == None
  assert timex.day_of_week == None
  assert timex.week_of_year == None
  assert timex.week_of_month == None
  assert timex.season == None
  assert timex.hour == None
  assert timex.minute == None
  assert timex.second == None
  assert timex.weekend == None
  assert timex.part_of_day == 'MO'
  assert timex.years == None
  assert timex.months == None
  assert timex.weeks == None
  assert timex.days == None
  assert timex.hours == None
  assert timex.minutes == None
  assert timex.seconds == None
  assert timex.now == None

def test_datatypes_parsing_afternoon():
  timex = Timex('TAF')
  assert timex.types == { Constants.TIMEX_TYPES_TIMERANGE }
  assert timex.year == None
  assert timex.month == None
  assert timex.day_of_month == None
  assert timex.day_of_week == None
  assert timex.week_of_year == None
  assert timex.week_of_month == None
  assert timex.season == None
  assert timex.hour == None
  assert timex.minute == None
  assert timex.second == None
  assert timex.weekend == None
  assert timex.part_of_day == 'AF'
  assert timex.years == None
  assert timex.months == None
  assert timex.weeks == None
  assert timex.days == None
  assert timex.hours == None
  assert timex.minutes == None
  assert timex.seconds == None
  assert timex.now == None

def test_datatypes_parsing_evening():
  timex = Timex('TEV')
  assert timex.types == { Constants.TIMEX_TYPES_TIMERANGE }
  assert timex.year == None
  assert timex.month == None
  assert timex.day_of_month == None
  assert timex.day_of_week == None
  assert timex.week_of_year == None
  assert timex.week_of_month == None
  assert timex.season == None
  assert timex.hour == None
  assert timex.minute == None
  assert timex.second == None
  assert timex.weekend == None
  assert timex.part_of_day == 'EV'
  assert timex.years == None
  assert timex.months == None
  assert timex.weeks == None
  assert timex.days == None
  assert timex.hours == None
  assert timex.minutes == None
  assert timex.seconds == None
  assert timex.now == None

def test_datatypes_parsing_timerange_430pm_to_445pm():
  timex = Timex('(T16:30,T16:45,PT15M)')
  assert timex.types == {
    Constants.TIMEX_TYPES_TIME,
    Constants.TIMEX_TYPES_DURATION,
    Constants.TIMEX_TYPES_TIMERANGE }
  assert timex.year == None
  assert timex.month == None
  assert timex.day_of_month == None
  assert timex.day_of_week == None
  assert timex.week_of_year == None
  assert timex.week_of_month == None
  assert timex.season == None
  assert timex.hour == 16
  assert timex.minute == 30
  assert timex.second == 0
  assert timex.weekend == None
  assert timex.part_of_day == None
  assert timex.years == None
  assert timex.months == None
  assert timex.weeks == None
  assert timex.days == None
  assert timex.hours == None
  assert timex.minutes == 15
  assert timex.seconds == None
  assert timex.now == None

def test_datatypes_parsing_datetimerange():
  timex = Timex('XXXX-WXX-5TEV')
  assert timex.types == {
    Constants.TIMEX_TYPES_DATE,
    Constants.TIMEX_TYPES_TIMERANGE,
    Constants.TIMEX_TYPES_DATETIMERANGE }
  assert timex.year == None
  assert timex.month == None
  assert timex.day_of_month == None
  assert timex.day_of_week == 5
  assert timex.week_of_year == None
  assert timex.week_of_month == None
  assert timex.season == None
  assert timex.hour == None
  assert timex.minute == None
  assert timex.second == None
  assert timex.weekend == None
  assert timex.part_of_day == 'EV'
  assert timex.years == None
  assert timex.months == None
  assert timex.weeks == None
  assert timex.days == None
  assert timex.hours == None
  assert timex.minutes == None
  assert timex.seconds == None
  assert timex.now == None

def test_datatypes_parsing_lastnight():
  timex = Timex('2017-09-07TNI')
  assert timex.types == {
    Constants.TIMEX_TYPES_DEFINITE,
    Constants.TIMEX_TYPES_DATE,
    Constants.TIMEX_TYPES_TIMERANGE,
    Constants.TIMEX_TYPES_DATETIMERANGE }
  assert timex.year == 2017
  assert timex.month == 9
  assert timex.day_of_month == 7
  assert timex.day_of_week == None
  assert timex.week_of_year == None
  assert timex.week_of_month == None
  assert timex.season == None
  assert timex.hour == None
  assert timex.minute == None
  assert timex.second == None
  assert timex.weekend == None
  assert timex.part_of_day == 'NI'
  assert timex.years == None
  assert timex.months == None
  assert timex.weeks == None
  assert timex.days == None
  assert timex.hours == None
  assert timex.minutes == None
  assert timex.seconds == None
  assert timex.now == None

def test_datatypes_parsing_last_5_minutes():
  timex = Timex('(2017-09-08T21:19:29,2017-09-08T21:24:29,PT5M)')
  assert timex.types == {
    Constants.TIMEX_TYPES_DATE,
    Constants.TIMEX_TYPES_TIMERANGE,
    Constants.TIMEX_TYPES_DATETIMERANGE,
    Constants.TIMEX_TYPES_TIME,
    Constants.TIMEX_TYPES_DATETIME,
    Constants.TIMEX_TYPES_DURATION,
    Constants.TIMEX_TYPES_DATERANGE,
    Constants.TIMEX_TYPES_DEFINITE }
  assert timex.year == 2017
  assert timex.month == 9
  assert timex.day_of_month == 8
  assert timex.day_of_week == None
  assert timex.week_of_year == None
  assert timex.week_of_month == None
  assert timex.season == None
  assert timex.hour == 21
  assert timex.minute == 19
  assert timex.second == 29
  assert timex.weekend == None
  assert timex.part_of_day == None
  assert timex.years == None
  assert timex.months == None
  assert timex.weeks == None
  assert timex.days == None
  assert timex.hours == None
  assert timex.minutes == 5
  assert timex.seconds == None
  assert timex.now == None

def test_datatypes_parsing_wed_4PM_to_sat_3PM():
  timex = Timex('(XXXX-WXX-3T16,XXXX-WXX-6T15,PT71H)')
  assert timex.types == {
    Constants.TIMEX_TYPES_DATE,
    Constants.TIMEX_TYPES_TIMERANGE,
    Constants.TIMEX_TYPES_DATETIMERANGE,
    Constants.TIMEX_TYPES_TIME,
    Constants.TIMEX_TYPES_DATETIME,
    Constants.TIMEX_TYPES_DURATION,
    Constants.TIMEX_TYPES_DATERANGE }
  assert timex.year == None
  assert timex.month == None
  assert timex.day_of_month == None
  assert timex.day_of_week == 3
  assert timex.week_of_year == None
  assert timex.week_of_month == None
  assert timex.season == None
  assert timex.hour == 16
  assert timex.minute == 0
  assert timex.second == 0
  assert timex.weekend == None
  assert timex.part_of_day == None
  assert timex.years == None
  assert timex.months == None
  assert timex.weeks == None
  assert timex.days == None
  assert timex.hours == 71
  assert timex.minutes == None
  assert timex.seconds == None
  assert timex.now == None

def test_datatypes_parsing_duration_years():
  timex = Timex('P2Y')
#   assert timex.types == { Constants.TIMEX_TYPES_DURATION }
  assert timex.year == None
  assert timex.month == None
  assert timex.day_of_month == None
  assert timex.day_of_week == None
  assert timex.week_of_year == None
  assert timex.week_of_month == None
  assert timex.season == None
  assert timex.hour == None
  assert timex.minute == None
  assert timex.second == None
  assert timex.weekend == None
  assert timex.part_of_day == None
  assert timex.years == 2
  assert timex.months == None
  assert timex.weeks == None
  assert timex.days == None
  assert timex.hours == None
  assert timex.minutes == None
  assert timex.seconds == None
  assert timex.now == None

def test_datatypes_parsing_duration_months():
  timex = Timex('P4M')
  assert timex.types == { Constants.TIMEX_TYPES_DURATION }
  assert timex.year == None
  assert timex.month == None
  assert timex.day_of_month == None
  assert timex.day_of_week == None
  assert timex.week_of_year == None
  assert timex.week_of_month == None
  assert timex.season == None
  assert timex.hour == None
  assert timex.minute == None
  assert timex.second == None
  assert timex.weekend == None
  assert timex.part_of_day == None
  assert timex.years == None
  assert timex.months == 4
  assert timex.weeks == None
  assert timex.days == None
  assert timex.hours == None
  assert timex.minutes == None
  assert timex.seconds == None
  assert timex.now == None



def test_datatypes_parsing_duration_weeks():
  timex = Timex('P6W')
  assert timex.types == { Constants.TIMEX_TYPES_DURATION }
  assert timex.year == None
  assert timex.month == None
  assert timex.day_of_month == None
  assert timex.day_of_week == None
  assert timex.week_of_year == None
  assert timex.week_of_month == None
  assert timex.season == None
  assert timex.hour == None
  assert timex.minute == None
  assert timex.second == None
  assert timex.weekend == None
  assert timex.part_of_day == None
  assert timex.years == None
  assert timex.months == None
  assert timex.weeks == 6
  assert timex.days == None
  assert timex.hours == None
  assert timex.minutes == None
  assert timex.seconds == None
  assert timex.now == None

def test_datatypes_parsing_duration_weeks_floating_point():
  timex = Timex('P2.5W')
  assert timex.types == { Constants.TIMEX_TYPES_DURATION }
  assert timex.year == None
  assert timex.month == None
  assert timex.day_of_month == None
  assert timex.day_of_week == None
  assert timex.week_of_year == None
  assert timex.week_of_month == None
  assert timex.season == None
  assert timex.hour == None
  assert timex.minute == None
  assert timex.second == None
  assert timex.weekend == None
  assert timex.part_of_day == None
  assert timex.years == None
  assert timex.months == None
  assert timex.weeks == 2.5
  assert timex.days == None
  assert timex.hours == None
  assert timex.minutes == None
  assert timex.seconds == None
  assert timex.now == None

def test_datatypes_parsing_duration_days():
  timex = Timex('P1D')
  assert timex.types == { Constants.TIMEX_TYPES_DURATION }
  assert timex.year == None
  assert timex.month == None
  assert timex.day_of_month == None
  assert timex.day_of_week == None
  assert timex.week_of_year == None
  assert timex.week_of_month == None
  assert timex.season == None
  assert timex.hour == None
  assert timex.minute == None
  assert timex.second == None
  assert timex.weekend == None
  assert timex.part_of_day == None
  assert timex.years == None
  assert timex.months == None
  assert timex.weeks == None
  assert timex.days == 1
  assert timex.hours == None
  assert timex.minutes == None
  assert timex.seconds == None
  assert timex.now == None

def test_datatypes_parsing_duration_hours():
  timex = Timex('PT5H')
  assert timex.types == { Constants.TIMEX_TYPES_DURATION }
  assert timex.year == None
  assert timex.month == None
  assert timex.day_of_month == None
  assert timex.day_of_week == None
  assert timex.week_of_year == None
  assert timex.week_of_month == None
  assert timex.season == None
  assert timex.hour == None
  assert timex.minute == None
  assert timex.second == None
  assert timex.weekend == None
  assert timex.part_of_day == None
  assert timex.years == None
  assert timex.months == None
  assert timex.weeks == None
  assert timex.days == None
  assert timex.hours == 5
  assert timex.minutes == None
  assert timex.seconds == None
  assert timex.now == None

def test_datatypes_Parsing_DurationMinutes():
  timex = Timex('PT30M')
  assert timex.types == { Constants.TIMEX_TYPES_DURATION }
  assert timex.year == None
  assert timex.month == None
  assert timex.day_of_month == None
  assert timex.day_of_week == None
  assert timex.week_of_year == None
  assert timex.week_of_month == None
  assert timex.season == None
  assert timex.hour == None
  assert timex.minute == None
  assert timex.second == None
  assert timex.weekend == None
  assert timex.part_of_day == None
  assert timex.years == None
  assert timex.months == None
  assert timex.weeks == None
  assert timex.days == None
  assert timex.hours == None
  assert timex.minutes == 30
  assert timex.seconds == None
  assert timex.now == None

def test_datatypes_Parsing_DurationSeconds():
  timex = Timex('PT45S')
  assert timex.types == { Constants.TIMEX_TYPES_DURATION }
  assert timex.year == None
  assert timex.month == None
  assert timex.day_of_month == None
  assert timex.day_of_week == None
  assert timex.week_of_year == None
  assert timex.week_of_month == None
  assert timex.season == None
  assert timex.hour == None
  assert timex.minute == None
  assert timex.second == None
  assert timex.weekend == None
  assert timex.part_of_day == None
  assert timex.years == None
  assert timex.months == None
  assert timex.weeks == None
  assert timex.days == None
  assert timex.hours == None
  assert timex.minutes == None
  assert timex.seconds == 45
  assert timex.now == None

