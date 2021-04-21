from datatypes_timex_expression import Timex, datetime, TimexRelativeConvert


def test_datatypes_relativeconvert_date_today():
    timex = Timex(timex='2017-09-25')
    today = datetime(2017, 9, 25)

    assert TimexRelativeConvert.convert_timex_to_string_relative(timex, today) == 'today'


def test_datatypes_relativeconvert_date_tomorrow():
    timex = Timex(timex='2017-09-23')
    today = datetime(2017, 9, 22)

    assert TimexRelativeConvert.convert_timex_to_string_relative(timex, today) == 'tomorrow'


def test_datatypes_relativeconvert_date_tomorrow_cross_year_month_boundary():
    timex = Timex(timex='2018-01-01')
    today = datetime(2017, 12, 31)

    assert TimexRelativeConvert.convert_timex_to_string_relative(timex, today) == 'tomorrow'


def test_datatypes_relativeconvert_date_yesterday():
    timex = Timex(timex='2017-09-21')
    today = datetime(2017, 9, 22)

    assert TimexRelativeConvert.convert_timex_to_string_relative(timex, today) == 'yesterday'


def test_datatypes_relativeconvert_date_yesterday_cross_year_month_boundary():
    timex = Timex(timex='2017-12-31')
    today = datetime(2018, 1, 1)

    assert TimexRelativeConvert.convert_timex_to_string_relative(timex, today) == 'yesterday'


def test_datatypes_relativeconvert_date_this_week():
    timex = Timex(timex='2017-10-18')
    today = datetime(2017, 10, 16)

    assert TimexRelativeConvert.convert_timex_to_string_relative(timex, today) == 'this Wednesday'


def test_datatypes_relativeconvert_date_this_week_cross_year_month_boundary():
    timex = Timex(timex='2017-11-03')
    today = datetime(2017, 10, 31)

    assert TimexRelativeConvert.convert_timex_to_string_relative(timex, today) == 'this Friday'


def test_datatypes_relativeconvert_date_next_week():
    timex = Timex(timex='2017-09-27')
    today = datetime(2017, 9, 22)

    assert TimexRelativeConvert.convert_timex_to_string_relative(timex, today) == 'next Wednesday'


def test_datatypes_relativeconvert_date_next_week_cross_year_month_boundary():
    timex = Timex(timex='2018-01-05')
    today = datetime(2017, 12, 28)

    assert TimexRelativeConvert.convert_timex_to_string_relative(timex, today) == 'next Friday'


def test_datatypes_relativeconvert_date_last_week():
    timex = Timex(timex='2017-09-14')
    today = datetime(2017, 9, 22)

    assert TimexRelativeConvert.convert_timex_to_string_relative(timex, today) == 'last Thursday'


def test_datatypes_relativeconvert_date_last_week_cross_year_month_boundary():
    timex = Timex(timex='2017-12-25')
    today = datetime(2018, 1, 4)

    assert TimexRelativeConvert.convert_timex_to_string_relative(timex, today) == 'last Monday'


def test_datatypes_relativeconvert_date_this_week_2():
    timex = Timex(timex='2017-10-25')
    today = datetime(2017, 9, 9)

    assert TimexRelativeConvert.convert_timex_to_string_relative(timex, today) == '25th October 2017'


def test_datatypes_relativeconvert_date_next_week_2():
    timex = Timex(timex='2017-10-04')
    today = datetime(2017, 9, 22)

    assert TimexRelativeConvert.convert_timex_to_string_relative(timex, today) == '4th October 2017'


def test_datatypes_relativeconvert_date_last_week_2():
    timex = Timex(timex='2017-09-07')
    today = datetime(2017, 9, 22)

    assert TimexRelativeConvert.convert_timex_to_string_relative(timex, today) == '7th September 2017'


def test_datatypes_relativeconvert_datetime_today():
    timex = Timex(timex='2017-09-25T16:00:00')
    today = datetime(2017, 9, 25)

    assert TimexRelativeConvert.convert_timex_to_string_relative(timex, today) == 'today 4PM'


def test_datatypes_relativeconvert_datetime_tomorrow():
    timex = Timex(timex='2017-09-23T16:00:00')
    today = datetime(2017, 9, 22)

    assert TimexRelativeConvert.convert_timex_to_string_relative(timex, today) == 'tomorrow 4PM'


def test_datatypes_relativeconvert_datetime_yesterday():
    timex = Timex(timex='2017-09-21T16:00:00')
    today = datetime(2017, 9, 22)

    assert TimexRelativeConvert.convert_timex_to_string_relative(timex, today) == 'yesterday 4PM'


def test_datatypes_relativeconvert_daterange_this_week():
    timex = Timex(timex='2017-W39')
    today = datetime(2017, 9, 25)

    assert TimexRelativeConvert.convert_timex_to_string_relative(timex, today) == 'this week'


def test_datatypes_relativeconvert_daterange_next_week():
    timex = Timex(timex='2017-W40')
    today = datetime(2017, 9, 25)

    assert TimexRelativeConvert.convert_timex_to_string_relative(timex, today) == 'next week'


def test_datatypes_relativeconvert_daterange_last_week():
    timex = Timex(timex='2017-W38')
    today = datetime(2017, 9, 25)

    assert TimexRelativeConvert.convert_timex_to_string_relative(timex, today) == 'last week'


def test_datatypes_relativeconvert_daterange_this_week_2():
    timex = Timex(timex='2017-W40')
    today = datetime(2017, 10, 4)

    assert TimexRelativeConvert.convert_timex_to_string_relative(timex, today) == 'this week'


def test_datatypes_relativeconvert_daterange_next_week_2():
    timex = Timex(timex='2017-W41')
    today = datetime(2017, 10, 4)

    assert TimexRelativeConvert.convert_timex_to_string_relative(timex, today) == 'next week'


def test_datatypes_relativeconvert_daterange_last_week_2():
    timex = Timex(timex='2017-W39')
    today = datetime(2017, 10, 4)

    assert TimexRelativeConvert.convert_timex_to_string_relative(timex, today) == 'last week'


def test_datatypes_relativeconvert_daterange_this_weekend():
    timex = Timex(timex='2017-W39-WE')
    today = datetime(2017, 9, 25)

    assert TimexRelativeConvert.convert_timex_to_string_relative(timex, today) == 'this weekend'


def test_datatypes_relativeconvert_daterange_next_weekend():
    timex = Timex(timex='2017-W40-WE')
    today = datetime(2017, 9, 25)

    assert TimexRelativeConvert.convert_timex_to_string_relative(timex, today) == 'next weekend'


def test_datatypes_relativeconvert_daterange_last_weekend():
    timex = Timex(timex='2017-W38-WE')
    today = datetime(2017, 9, 25)

    assert TimexRelativeConvert.convert_timex_to_string_relative(timex, today) == 'last weekend'


def test_datatypes_relativeconvert_daterange_this_month():
    timex = Timex(timex='2017-09')
    today = datetime(2017, 9, 25)

    assert TimexRelativeConvert.convert_timex_to_string_relative(timex, today) == 'this month'


def test_datatypes_relativeconvert_daterange_next_month():
    timex = Timex(timex='2017-10')
    today = datetime(2017, 9, 25)

    assert TimexRelativeConvert.convert_timex_to_string_relative(timex, today) == 'next month'


def test_datatypes_relativeconvert_daterange_last_month():
    timex = Timex(timex='2017-08')
    today = datetime(2017, 9, 25)

    assert TimexRelativeConvert.convert_timex_to_string_relative(timex, today) == 'last month'


def test_datatypes_relativeconvert_daterange_this_year():
    timex = Timex(timex='2017')
    today = datetime(2017, 9, 25)

    assert TimexRelativeConvert.convert_timex_to_string_relative(timex, today) == 'this year'


def test_datatypes_relativeconvert_daterange_next_year():
    timex = Timex(timex='2018')
    today = datetime(2017, 9, 25)

    assert TimexRelativeConvert.convert_timex_to_string_relative(timex, today) == 'next year'


def test_datatypes_relativeconvert_daterange_last_year():
    timex = Timex(timex='2016')
    today = datetime(2017, 9, 25)

    assert TimexRelativeConvert.convert_timex_to_string_relative(timex, today) == 'last year'


def test_datatypes_relativeconvert_season_next_summer():
    timex = Timex(timex='2018-SU')
    today = datetime(2017, 9, 25)

    assert TimexRelativeConvert.convert_timex_to_string_relative(timex, today) == 'next summer'


def test_datatypes_relativeconvert_season_last_summer():
    timex = Timex(timex='2016-SU')
    today = datetime(2017, 9, 25)

    assert TimexRelativeConvert.convert_timex_to_string_relative(timex, today) == 'last summer'


def test_datatypes_relativeconvert_partofday_this_evening():
    timex = Timex(timex='2017-09-25TEV')
    today = datetime(2017, 9, 25)

    assert TimexRelativeConvert.convert_timex_to_string_relative(timex, today) == 'this evening'


def test_datatypes_relativeconvert_partofday_tonight():
    timex = Timex(timex='2017-09-25TNI')
    today = datetime(2017, 9, 25)

    assert TimexRelativeConvert.convert_timex_to_string_relative(timex, today) == 'tonight'


def test_datatypes_relativeconvert_partofday_tomorrow_morning():
    timex = Timex(timex='2017-09-26TMO')
    today = datetime(2017, 9, 25)

    assert TimexRelativeConvert.convert_timex_to_string_relative(timex, today) == 'tomorrow morning'


def test_datatypes_relativeconvert_partofday_yesterday_afternoon():
    timex = Timex(timex='2017-09-24TAF')
    today = datetime(2017, 9, 25)

    assert TimexRelativeConvert.convert_timex_to_string_relative(timex, today) == 'yesterday afternoon'


def test_datatypes_relativeconvert_partofday_next_wednesday_evening():
    timex = Timex(timex='2017-10-04TEV')
    today = datetime(2017, 9, 25)

    assert TimexRelativeConvert.convert_timex_to_string_relative(timex, today) == 'next Wednesday evening'
