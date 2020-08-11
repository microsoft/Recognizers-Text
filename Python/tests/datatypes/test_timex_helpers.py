from datatypes_timex_expression import Timex, TimexHelpers, datetime, Time


def test_datatypes_helpers_expanddatetimerange_short():
    timex = Timex(timex='(2017-09-27,2017-09-29,P2D)')
    range = TimexHelpers.expand_datetime_range(timex)

    assert range.start.timex_value() == '2017-09-27'
    assert range.end.timex_value() == '2017-09-29'


def test_datatypes_helpers_expanddatetimerange_long():
    timex = Timex(timex='(2006-01-01,2008-06-01,P882D)')
    range = TimexHelpers.expand_datetime_range(timex)

    assert range.start.timex_value() == '2006-01-01'
    assert range.end.timex_value() == '2008-06-01'


def test_datatypes_gelpers_expanddatetimerange_include_time():
    timex = Timex(timex='(2017-10-10T16:02:04,2017-10-10T16:07:04,PT5M)')
    range = TimexHelpers.expand_datetime_range(timex)

    assert range.start.timex_value() == '2017-10-10T16:02:04'
    assert range.end.timex_value() == '2017-10-10T16:07:04'


def test_datatypes_helpers_expanddatetimerange_month():
    timex = Timex(timex='2017-05')
    range = TimexHelpers.expand_datetime_range(timex)

    assert range.start.timex_value() == '2017-05-01'
    assert range.end.timex_value() == '2017-06-01'


def test_datatypes_helpers_expanddatetimerange_year():
    timex = Timex(timex='1999')
    range = TimexHelpers.expand_datetime_range(timex)

    assert range.start.timex_value() == '1999-01-01'
    assert range.end.timex_value() == '2000-01-01'


def test_datatypes_helpers_expandtimerange():
    timex = Timex(timex='(T14,T16,PT2H)')
    range = TimexHelpers.expand_datetime_range(timex)

    assert range.start.timex_value() == 'T14'
    assert range.end.timex_value() == 'T16'


def test_datatypes_helpers_daterangefromtimex():
    timex = Timex(timex='(2017-09-27,2017-09-29,P2D)')
    range = TimexHelpers.daterange_from_timex(timex)

    assert range.start == datetime(2017, 9, 27).date()
    assert range.end == datetime(2017, 9, 29).date()


def test__datatypes_helpers_datefromtimex():
    timex = Timex(timex='2017-09-27')
    date = TimexHelpers.date_from_timex(timex)

    assert date == datetime(2017, 9, 27).date()


def test_datatypes_helpers_timefromtimex():
    timex = Timex(timex='T00:05:00')
    time = TimexHelpers.time_from_timex(timex)

    assert time.get_time() == Time(0, 5, 0).get_time()
