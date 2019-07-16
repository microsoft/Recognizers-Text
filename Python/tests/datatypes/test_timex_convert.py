from datatypes_timex_expression import TimexConvert, Timex


def test_timex_convert_complete_date():
    assert TimexConvert.convert_timex_to_string(Timex(timex="2017-05-29")) == "29th May 2017"


def test_timex_convert_month_and_day_of_month():
    assert TimexConvert.convert_timex_to_string(Timex(timex="XXXX-01-05")) == "5th January"
    assert TimexConvert.convert_timex_to_string(Timex(timex="XXXX-02-05")) == "5th Februrary"
    assert TimexConvert.convert_timex_to_string(Timex(timex="XXXX-03-05")) == "5th March"
    assert TimexConvert.convert_timex_to_string(Timex(timex="XXXX-04-05")) == "5th April"
    assert TimexConvert.convert_timex_to_string(Timex(timex="XXXX-05-05")) == "5th May"
    assert TimexConvert.convert_timex_to_string(Timex(timex="XXXX-06-05")) == "5th June"
    assert TimexConvert.convert_timex_to_string(Timex(timex="XXXX-07-05")) == "5th July"
    assert TimexConvert.convert_timex_to_string(Timex(timex="XXXX-08-05")) == "5th August"
    assert TimexConvert.convert_timex_to_string(Timex(timex="XXXX-09-05")) == "5th September"
    assert TimexConvert.convert_timex_to_string(Timex(timex="XXXX-10-05")) == "5th October"
    assert TimexConvert.convert_timex_to_string(Timex(timex="XXXX-11-05")) == "5th November"
    assert TimexConvert.convert_timex_to_string(Timex(timex="XXXX-12-05")) == "5th December"


def test_timex_convert_month_and_day_of_month_with_correct_abbreviation():
    assert TimexConvert.convert_timex_to_string(Timex(timex="XXXX-06-01")) == "1st June"
    assert TimexConvert.convert_timex_to_string(Timex(timex="XXXX-06-02")) == "2nd June"
    assert TimexConvert.convert_timex_to_string(Timex(timex="XXXX-06-03")) == "3rd June"
    assert TimexConvert.convert_timex_to_string(Timex(timex="XXXX-06-04")) == "4th June"


def test_timex_convert_day_of_week():
    assert TimexConvert.convert_timex_to_string(Timex(timex="XXXX-WXX-1")) == "Monday"
    assert TimexConvert.convert_timex_to_string(Timex(timex="XXXX-WXX-2")) == "Tuesday"
    assert TimexConvert.convert_timex_to_string(Timex(timex="XXXX-WXX-3")) == "Wednesday"
    assert TimexConvert.convert_timex_to_string(Timex(timex="XXXX-WXX-4")) == "Thursday"
    assert TimexConvert.convert_timex_to_string(Timex(timex="XXXX-WXX-5")) == "Friday"
    assert TimexConvert.convert_timex_to_string(Timex(timex="XXXX-WXX-6")) == "Saturday"
    assert TimexConvert.convert_timex_to_string(Timex(timex="XXXX-WXX-7")) == "Sunday"


def test_timex_convert_time():
    assert TimexConvert.convert_timex_to_string(Timex(timex="T17:30:05")) == "5:30:05PM"
    assert TimexConvert.convert_timex_to_string(Timex(timex="T02:30:30")) == "2:30:30AM"
    assert TimexConvert.convert_timex_to_string(Timex(timex="T00:30:30")) == "12:30:30AM"
    assert TimexConvert.convert_timex_to_string(Timex(timex="T12:30:30")) == "12:30:30PM"


def test_timex_convert_hour_and_minute():
    assert TimexConvert.convert_timex_to_string(Timex(timex="T17:30")) == "5:30PM"
    assert TimexConvert.convert_timex_to_string(Timex(timex="T17:00")) == "5PM"
    assert TimexConvert.convert_timex_to_string(Timex(timex="T01:30")) == "1:30AM"
    assert TimexConvert.convert_timex_to_string(Timex(timex="T01:00")) == "1AM"


def test_timex_convert_now():
    assert TimexConvert.convert_timex_to_string(Timex(timex="PRESENT_REF")) == "now"


def test_timex_convert_full_datetime():
    assert TimexConvert.convert_timex_to_string(Timex(timex="1984-01-03T18:30:45")) == "6:30:45PM 3rd January 1984"
    assert TimexConvert.convert_timex_to_string(Timex(timex="2000-01-01T00")) == "midnight 1st January 2000"
    assert TimexConvert.convert_timex_to_string(Timex(timex="1967-05-29T19:30:00")) == "7:30PM 29th May 1967"


def test_timex_convert_particular_time_on_particular_day_of_week():
    assert TimexConvert.convert_timex_to_string(Timex(timex="XXXX-WXX-3T16")) == "4PM Wednesday"
    assert TimexConvert.convert_timex_to_string(Timex(timex="XXXX-WXX-5T18:30")) == "6:30PM Friday"


def test_timex_convert_year():
    assert TimexConvert.convert_timex_to_string(Timex(timex="2016")) == " 2016"


def test_timex_convert_year_season():
    assert TimexConvert.convert_timex_to_string(Timex(timex="1999-SU")) == "summer 1999"


def test_timex_convert_season():
    assert TimexConvert.convert_timex_to_string(Timex(timex="SU")) == "summer "
    assert TimexConvert.convert_timex_to_string(Timex(timex="WI")) == "winter "


def test_timex_convert_month():
    assert TimexConvert.convert_timex_to_string(Timex(timex="XXXX-01")) == "January"
    assert TimexConvert.convert_timex_to_string(Timex(timex="XXXX-05")) == "May"
    assert TimexConvert.convert_timex_to_string(Timex(timex="XXXX-12")) == "December"


def test_timex_convert_month_and_year():
    assert TimexConvert.convert_timex_to_string(Timex(timex="2018-05")) == "May2018"


def test_timex_convert_week_of_month():
    assert TimexConvert.convert_timex_to_string(Timex(timex="XXXX-01-W01")) == "firstweek of January"
    assert TimexConvert.convert_timex_to_string(Timex(timex="XXXX-08-W03")) == "thirdweek of August"


def test_timex_convert_part_of_the_day():
    assert TimexConvert.convert_timex_to_string(Timex(timex="TDT")) == "daytime"
    assert TimexConvert.convert_timex_to_string(Timex(timex="TNI")) == "night"
    assert TimexConvert.convert_timex_to_string(Timex(timex="TMO")) == "morning"
    assert TimexConvert.convert_timex_to_string(Timex(timex="TAF")) == "afternoon"
    assert TimexConvert.convert_timex_to_string(Timex(timex="TEV")) == "evening"


def test_timex_convert_friday_evening():
    assert TimexConvert.convert_timex_to_string(Timex(timex="XXXX-WXX-5TEV")) == "Friday evening"


def test_timex_convert_date_and_part_of_day():
    assert TimexConvert.convert_timex_to_string(Timex(timex="2017-09-07TNI")) == "7th September 2017 night"

# def test_timex_convert_last_5_minutes():

# def test_timex_convert_wednesday_to_saturdays():


def test_timex_convert_years():
    assert TimexConvert.convert_timex_to_string(Timex(timex="P2Y")) == "2 years"
    assert TimexConvert.convert_timex_to_string(Timex(timex="P1Y")) == "1 year"


def test_timex_convert_weeks():
    assert TimexConvert.convert_timex_to_string(Timex(timex="P6W")) == "6 weeks"
    assert TimexConvert.convert_timex_to_string(Timex(timex="P9.5W")) == "9.5 weeks"


def test_timex_convert_days():
    assert TimexConvert.convert_timex_to_string(Timex(timex="P5D")) == "5 days"
    assert TimexConvert.convert_timex_to_string(Timex(timex="P1D")) == "1 day"


def test_timex_convert_hours():
    assert TimexConvert.convert_timex_to_string(Timex(timex="PT5H")) == "5 hours"
    assert TimexConvert.convert_timex_to_string(Timex(timex="PT1H")) == "1 hour"


def test_timex_convert_minutes():
    assert TimexConvert.convert_timex_to_string(Timex(timex="PT30M")) == "30 minutes"
    assert TimexConvert.convert_timex_to_string(Timex(timex="PT1M")) == "1 minute"


def test_timex_convert_seconds():
    assert TimexConvert.convert_timex_to_string(Timex(timex="PT45S")) == "45 seconds"


def test_timex_convert_every_2days():
    assert TimexConvert.convert_timex_to_string(Timex(timex="P2D")) == "2 days"


def test_timex_convert_every_week():
    assert TimexConvert.convert_timex_to_string(Timex(timex="P1W")) == "1 week"


def test_timex_convert_every_October():
    assert TimexConvert.convert_timex_to_string(Timex(timex="XXXX-10")) == "October"


def test_timex_convert_every_Sunday():
    assert TimexConvert.convert_timex_to_string(Timex(timex="XXXX-WXX-7")) == "Sunday"


def test_timex_convert_every_Day():
    assert TimexConvert.convert_timex_to_string(Timex(timex="P1D")) == "1 day"


def test_timex_convert_every_Year():
    assert TimexConvert.convert_timex_to_string(Timex(timex="P1Y")) == "1 year"


def test_timex_convert_every_spring():
    assert TimexConvert.convert_timex_to_string(Timex(timex="SP")) == "spring "


def test_timex_convert_every_winter():
    assert TimexConvert.convert_timex_to_string(Timex(timex="WI")) == "winter "


def test_timex_convert_every_evening():
    assert TimexConvert.convert_timex_to_string(Timex(timex="TEV")) == "evening"
