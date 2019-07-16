from datatypes_timex_expression import Timex, datetime, Time


def test_datatypes_timex_fromdate():
    assert Timex.from_date(datetime(2017, 12, 5)).timex_value() == '2017-12-05'


def test_datatypes_timex_fromdatetime():
    assert Timex.from_date_time(datetime(2017, 12, 5, 23, 57, 35)).timex_value() == '2017-12-05T23:57:35'


def test_datatypes_timex_roundtrip_date():
    roundtrip('2017-09-27')
    roundtrip('XXXX-WXX-3')
    roundtrip('XXXX-12-05')


def test_datatypes_timex_roundtrip_time():
    roundtrip('T17:30:45')
    roundtrip('T05:06:07')
    roundtrip('T17:30')
    roundtrip('T23')


def test_datatypes_timex_roundtrip_duration():
    roundtrip('P50Y')
    roundtrip('P6M')
    roundtrip('P3W')
    roundtrip('P5D')
    roundtrip('PT16H')
    roundtrip('PT32M')
    roundtrip('PT20S')


def test_datatypes_timex_roundtrip_now():
    roundtrip('PRESENT_REF')


def test_datatypes_timex_roundtrip_datetime():
    roundtrip('2017')
    roundtrip('SU')
    roundtrip('2017-WI')
    roundtrip('2017-09')
    roundtrip('2017-W37')
    roundtrip('2017-W37-WE')
    roundtrip('XXXX-05')


def test_datatypes_timex_roundtrip_daterange_start_end_duration():
    roundtrip('(XXXX-WXX-3,XXXX-WXX-6,P3D)')
    roundtrip('(XXXX-01-01,XXXX-08-05,P216D)')
    roundtrip('(2017-01-01,2017-08-05,P216D)')
    roundtrip('(2016-01-01,2016-08-05,P217D)')


def test_datatypes_timex_roundtrip_timerange():
    roundtrip('TEV')


def test_datatypes_timex_roundtrip_timerange_start_end_duration():
    roundtrip('(T16,T19,PT3H)')


def test_datatypes_timex_roundtrip_datetimerange():
    roundtrip('2017-09-27TEV')


def test_datatypes_timex_roundtrip_datetimerange_start_end_duration():
    roundtrip('(2017-09-08T21:19:29,2017-09-08T21:24:29,PT5M)')
    roundtrip('(XXXX-WXX-3T16,XXXX-WXX-6T15,PT71H)')


def test_datatypes_timex_tostring():
    assert Timex('XXXX-05-05').to_string() == '5th May'


def test_datatypes_timex_tonaturallanguage():
    today = datetime(2017, 10, 16)
    Timex('2017-10-17').to_natural_language(today)
    assert 'tomorrow' == Timex('2017-10-17').to_natural_language(today)


def test_datatypes_timex_fromtime():
    assert Timex.from_time(Time(23, 59, 30)).timex_value() == 'T23:59:30'


def roundtrip(timex):
    assert timex == Timex(timex).timex_value()
