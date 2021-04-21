from datatypes_timex_expression import datetime, TimexDateHelpers, Constants


def test_datatypes_datehelpers_tomorrow():
    assert TimexDateHelpers.tomorrow(datetime(2016, 12, 31)) == datetime(2017, 1, 1)
    assert TimexDateHelpers.tomorrow(datetime(2017, 1, 1)) == datetime(2017, 1, 2)
    assert TimexDateHelpers.tomorrow(datetime(2017, 2, 28)) == datetime(2017, 3, 1)
    assert TimexDateHelpers.tomorrow(datetime(2016, 2, 28)) == datetime(2016, 2, 29)


def test_datatypes_datehelpers_yesterday():
    assert TimexDateHelpers.yesterday(datetime(2017, 1, 1)) == datetime(2016, 12, 31)
    assert TimexDateHelpers.yesterday(datetime(2017, 1, 2)) == datetime(2017, 1, 1)
    assert TimexDateHelpers.yesterday(datetime(2017, 3, 1)) == datetime(2017, 2, 28)
    assert TimexDateHelpers.yesterday(datetime(2016, 2, 29)) == datetime(2016, 2, 28)


def test_datatypes_datehelpers_datepartequals():
    assert TimexDateHelpers.date_part_equal(datetime(2017, 5, 29), datetime(2017, 5, 29)) is True
    assert TimexDateHelpers.date_part_equal(datetime(2017, 5, 29, 19, 30, 0), datetime(2017, 5, 29)) is False
    assert TimexDateHelpers.date_part_equal(datetime(2017, 5, 29), datetime(2017, 11, 15)) is False


def test_datatypes_datehelpers_isnextweek():
    today = datetime(2017, 9, 25)

    assert TimexDateHelpers.is_next_week(datetime(2017, 10, 4), today) is True
    assert TimexDateHelpers.is_next_week(datetime(2017, 9, 27), today) is False
    assert TimexDateHelpers.is_next_week(today, today) is False


def test_datatypes_datehelpers_islastweek():
    today = datetime(2017, 9, 25)

    assert TimexDateHelpers.is_last_week(datetime(2017, 9, 20), today) is True
    assert TimexDateHelpers.is_last_week(datetime(2017, 9, 4), today) is False
    assert TimexDateHelpers.is_last_week(today, today) is False


def test_datatypes_datehelpers_weekofyear():
    assert TimexDateHelpers.week_of_year(datetime(2017, 1, 1)) == 52
    assert TimexDateHelpers.week_of_year(datetime(2017, 1, 2)) == 1
    assert TimexDateHelpers.week_of_year(datetime(2017, 2, 23)) == 8
    assert TimexDateHelpers.week_of_year(datetime(2017, 3, 15)) == 11
    assert TimexDateHelpers.week_of_year(datetime(2017, 9, 25)) == 39
    assert TimexDateHelpers.week_of_year(datetime(2017, 12, 31)) == 52
    assert TimexDateHelpers.week_of_year(datetime(2018, 1, 1)) == 1
    assert TimexDateHelpers.week_of_year(datetime(2018, 1, 2)) == 1
    assert TimexDateHelpers.week_of_year(datetime(2018, 1, 7)) == 1
    assert TimexDateHelpers.week_of_year(datetime(2018, 1, 8)) == 2
    assert TimexDateHelpers.week_of_year(datetime(2018, 12, 31)) == 1
    assert TimexDateHelpers.week_of_year(datetime(2021, 4, 20)) == 16


def test_datatypes_datehelpers_invariance():
    d = datetime(2017, 8, 25)
    before = d

    TimexDateHelpers.tomorrow(d)
    TimexDateHelpers.yesterday(d)
    TimexDateHelpers.date_part_equal(datetime.now(), d)
    TimexDateHelpers.date_part_equal(d, datetime.now())
    TimexDateHelpers.is_next_week(d, datetime.now())
    TimexDateHelpers.is_next_week(datetime.now(), d)
    TimexDateHelpers.is_last_week(datetime.now(), d)
    TimexDateHelpers.week_of_year(d)

    after = d
    assert after is before


def test_datatypes_datehelpers_dateoflastday_friday_lastweek():
    day = Constants.DAYS['FRIDAY']
    date = datetime(2017, 9, 28)

    assert datetime(2017, 9, 22) == TimexDateHelpers.date_of_last_day(day, date)


def test_datatypes_datehelpers_dateofnextday_wednesday_nextweek():
    day = Constants.DAYS['WEDNESDAY']
    date = datetime(2017, 9, 28)

    assert datetime(2017, 10, 4) == TimexDateHelpers.date_of_next_day(day, date)


def test_datatypes_datehelpers_dateofnextday_today():
    day = Constants.DAYS['THURSDAY']
    date = datetime(2017, 9, 28)

    assert date != TimexDateHelpers.date_of_next_day(day, date)


def test_datatypes_datehelpers_datesmatchingday():
    day = Constants.DAYS['THURSDAY']
    start = datetime(2017, 3, 1)
    end = datetime(2017, 4, 1)
    result = TimexDateHelpers.dates_matching_day(day, start, end)

    assert len(result) == 5
    assert result[0] == datetime(2017, 3, 2)
    assert result[1] == datetime(2017, 3, 9)
    assert result[2] == datetime(2017, 3, 16)
    assert result[3] == datetime(2017, 3, 23)
    assert result[4] == datetime(2017, 3, 30)
