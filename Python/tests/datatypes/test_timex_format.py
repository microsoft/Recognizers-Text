from datatypes_timex_expression import Timex


def test_datatypes_format_date():
    assert '2017-09-27' == Timex(year=2017, month=9, day_of_month=27).timex_value()
    assert 'XXXX-WXX-3' == Timex(day_of_week=3).timex_value()
    assert 'XXXX-12-05' == Timex(month=12, day_of_month=5).timex_value()


def test_datatypes_format_time():
    assert 'T17:30:45' == Timex(hour=17, minute=30, second=45).timex_value()
    assert 'T05:06:07' == Timex(hour=5, minute=6, second=7).timex_value()
    assert 'T17:30' == Timex(hour=17, minute=30, second=0).timex_value()
    assert 'T23' == Timex(hour=23, minute=0, second=0).timex_value()


def test_datatypes_format_duration():
    assert 'P50Y' == Timex(years=50).timex_value()
    assert 'P6M' == Timex(months=6).timex_value()
    assert 'P3W' == Timex(weeks=3).timex_value()
    assert 'P5D' == Timex(days=5).timex_value()
    assert 'PT16H' == Timex(hours=16).timex_value()
    assert 'PT32M' == Timex(minutes=32).timex_value()
    assert 'PT20S' == Timex(seconds=20).timex_value()


def test_datatypes_format_present():
    assert Timex(now=True).timex_value() == 'PRESENT_REF'


def test_datatypes_format_datetime():
    assert Timex(hour=4, minute=0, second=0, day_of_week=3).timex_value() == 'XXXX-WXX-3T04'
    assert Timex(year=2017, month=9, day_of_month=27, hour=11, minute=41, second=30).timex_value() == '2017-09-27T11:41:30'


def test_datatypes_format_daterange():

    assert Timex(year=2017).timex_value() == '2017'
    assert Timex(season='SU').timex_value() == 'SU'
    assert Timex(year=2017, season='WI').timex_value() == '2017-WI'
    assert Timex(year=2017, month=9).timex_value() == '2017-09'
    assert Timex(year=2017, week_of_year=37).timex_value() == '2017-W37'
    assert Timex(year=2017, week_of_year=37, weekend=True).timex_value() == '2017-W37-WE'
    assert Timex(month=5).timex_value() == 'XXXX-05'


def test_datatypes_format_timerange():
    assert Timex(part_of_day='EV').timex_value() == 'TEV'


def test_datatypes_format_datetimerange():
    assert Timex(year=2017, month=9, day_of_month=27, part_of_day='EV').timex_value() == '2017-09-27TEV'
