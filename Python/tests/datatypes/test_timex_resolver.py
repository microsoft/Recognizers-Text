from datatypes_timex_expression import Timex, datetime, TimexResolver


def test_datatypes_resolver_date_definite():
    today = datetime(2017, 9, 26, 15, 30, 0)
    resolution = TimexResolver.resolve(["2017-09-28"], today)

    assert len(resolution.values) == 1
    assert resolution.values[0].timex == "2017-09-28"
    assert resolution.values[0].type == "date"
    assert resolution.values[0].value == "2017-09-28"
    assert resolution.values[0].start is None
    assert resolution.values[0].end is None


def test_datatypes_resolver_date_saturday():
    today = datetime(2017, 9, 26, 15, 30, 0)
    resolution = TimexResolver.resolve(["XXXX-WXX-6"], today)

    assert len(resolution.values) == 2
    assert resolution.values[0].timex == "XXXX-WXX-6"
    assert resolution.values[0].type == "date"
    assert resolution.values[0].value == "2017-09-23"
    assert resolution.values[0].start is None
    assert resolution.values[0].end is None

    assert resolution.values[1].timex == "XXXX-WXX-6"
    assert resolution.values[1].type == "date"
    assert resolution.values[1].value == "2017-09-30"
    assert resolution.values[1].start is None
    assert resolution.values[1].end is None


def test_datatypes_resolver_date_sunday():
    today = datetime(2019, 4, 23, 15, 30, 0)
    resolution = TimexResolver.resolve(["XXXX-WXX-7"], today)

    assert len(resolution.values) == 2
    assert resolution.values[0].timex == "XXXX-WXX-7"
    assert resolution.values[0].type == "date"
    assert resolution.values[0].value == "2019-04-21"
    assert resolution.values[0].start is None
    assert resolution.values[0].end is None

    assert resolution.values[1].timex == "XXXX-WXX-7"
    assert resolution.values[1].type == "date"
    assert resolution.values[1].value == "2019-04-28"
    assert resolution.values[1].start is None
    assert resolution.values[1].end is None


def test_datatypes_resolver_date_wednesday_4():
    today = datetime(2017, 9, 28, 15, 30, 0)
    resolution = TimexResolver.resolve(["XXXX-WXX-3T04", "XXXX-WXX-3T16"], today)

    assert len(resolution.values) == 4
    assert resolution.values[0].timex == "XXXX-WXX-3T04"
    assert resolution.values[0].type == "datetime"
    assert resolution.values[0].value == "2017-09-27 04:00:00"
    assert resolution.values[0].start is None
    assert resolution.values[0].end is None

    assert resolution.values[1].timex == "XXXX-WXX-3T04"
    assert resolution.values[1].type == "datetime"
    assert resolution.values[1].value == "2017-10-04 04:00:00"
    assert resolution.values[1].start is None
    assert resolution.values[1].end is None

    assert resolution.values[2].timex == "XXXX-WXX-3T16"
    assert resolution.values[2].type == "datetime"
    assert resolution.values[2].value == "2017-09-27 16:00:00"
    assert resolution.values[2].start is None
    assert resolution.values[2].end is None

    assert resolution.values[3].timex == "XXXX-WXX-3T16"
    assert resolution.values[3].type == "datetime"
    assert resolution.values[3].value == "2017-10-04 16:00:00"
    assert resolution.values[3].start is None
    assert resolution.values[3].end is None


def test_datatypes_resolver_datetime_wednesday_4_am():
    today = datetime(2017, 9, 7)
    resolution = TimexResolver.resolve(["2017-10-11T04"], today)

    assert len(resolution.values) == 1
    assert resolution.values[0].timex == "2017-10-11T04"
    assert resolution.values[0].type == "datetime"
    assert resolution.values[0].value == "2017-10-11 04:00:00"
    assert resolution.values[0].start is None
    assert resolution.values[0].end is None


def test_datatypes_resolver_duration_2years():
    today = datetime(2017, 9, 7)
    resolution = TimexResolver.resolve(["P2Y"], today)

    assert len(resolution.values) == 1
    assert resolution.values[0].timex == "P2Y"
    assert resolution.values[0].type == "duration"
    assert resolution.values[0].value == "63072000"
    assert resolution.values[0].start is None
    assert resolution.values[0].end is None


def test_datatypes_resolver_duration_6month():
    today = datetime(2017, 9, 7)
    resolution = TimexResolver.resolve(["P6M"], today)

    assert len(resolution.values) == 1
    assert resolution.values[0].timex == "P6M"
    assert resolution.values[0].type == "duration"
    assert resolution.values[0].value == "15552000"
    assert resolution.values[0].start is None
    assert resolution.values[0].end is None


def test_datatypes_resolver_duration_3weeks():
    resolution = TimexResolver.resolve(["P3W"])

    assert len(resolution.values) == 1
    assert resolution.values[0].timex == "P3W"
    assert resolution.values[0].type == "duration"
    assert resolution.values[0].value == "1814400"
    assert resolution.values[0].start is None
    assert resolution.values[0].end is None


def test_datatypes_resolver_duration_5days():
    resolution = TimexResolver.resolve(["P5D"])

    assert len(resolution.values) == 1
    assert resolution.values[0].timex == "P5D"
    assert resolution.values[0].type == "duration"
    assert resolution.values[0].value == "432000"
    assert resolution.values[0].start is None
    assert resolution.values[0].end is None


def test_datatypes_resolver_duration_8hours():
    resolution = TimexResolver.resolve(["PT8H"])

    assert len(resolution.values) == 1
    assert resolution.values[0].timex == "PT8H"
    assert resolution.values[0].type == "duration"
    assert resolution.values[0].value == "28800"
    assert resolution.values[0].start is None
    assert resolution.values[0].end is None


def test_datatypes_resolver_duration_15minutes():
    resolution = TimexResolver.resolve(["PT15M"])

    assert len(resolution.values) == 1
    assert resolution.values[0].timex == "PT15M"
    assert resolution.values[0].type == "duration"
    assert resolution.values[0].value == "900"
    assert resolution.values[0].start is None
    assert resolution.values[0].end is None


def test_datatypes_resolver_duration_10seconds():
    resolution = TimexResolver.resolve(["PT10S"])

    assert len(resolution.values) == 1
    assert resolution.values[0].timex == "PT10S"
    assert resolution.values[0].type == "duration"
    assert resolution.values[0].value == "10"
    assert resolution.values[0].start is None
    assert resolution.values[0].end is None


def test_datatypes_resolver_dateRange_september():
    today = datetime(2017, 9, 28)
    resolution = TimexResolver.resolve(["XXXX-09"], today)

    assert len(resolution.values) == 2
    assert resolution.values[0].timex == "XXXX-09"
    assert resolution.values[0].type == "daterange"
    assert resolution.values[0].start == "2016-09-01"
    assert resolution.values[0].end == "2016-10-01"
    assert resolution.values[0].value is None

    assert resolution.values[1].timex == "XXXX-09"
    assert resolution.values[1].type == "daterange"
    assert resolution.values[1].start == "2017-09-01"
    assert resolution.values[1].end == "2017-10-01"
    assert resolution.values[1].value is None


def test_datatypes_resolver_dateRange_winter():
    resolution = TimexResolver.resolve(["WI"])

    assert len(resolution.values) == 1
    assert resolution.values[0].timex == "WI"
    assert resolution.values[0].type == "daterange"
    assert resolution.values[0].value == "not resolved"
    assert resolution.values[0].start is None
    assert resolution.values[0].end is None


def test_datatypes_resolver_dateRange_last_week():
    today = datetime(2017, 4, 30)
    resolution = TimexResolver.resolve(["2019-W17"], today)

    assert len(resolution.values) == 1
    assert resolution.values[0].timex == "2019-W17"
    assert resolution.values[0].type == "daterange"
    assert resolution.values[0].start == "2019-04-22"
    assert resolution.values[0].end == "2019-04-29"


def test_datatypes_resolver_dateRange_week_of_year():
    today = datetime(2017, 4, 30)
    resolution = TimexResolver.resolve(["2021-W16"], today)

    assert len(resolution.values) == 1
    assert resolution.values[0].timex == "2021-W16"
    assert resolution.values[0].type == "daterange"
    assert resolution.values[0].start == "2021-04-19"
    assert resolution.values[0].end == "2021-04-26"


def test_datatypes_resolver_dateRange_last_month():
    today = datetime(2017, 4, 30)
    resolution = TimexResolver.resolve(["2019-03"], today)

    assert len(resolution.values) == 1
    assert resolution.values[0].timex == "2019-03"
    assert resolution.values[0].type == "daterange"
    assert resolution.values[0].start == "2019-03-01"
    assert resolution.values[0].end == "2019-04-01"


def test_datatypes_resolver_dateRange_last_year():
    today = datetime(2017, 4, 30)
    resolution = TimexResolver.resolve(["2018"], today)

    assert len(resolution.values) == 1
    assert resolution.values[0].timex == "2018"
    assert resolution.values[0].type == "daterange"
    assert resolution.values[0].start == "2018-01-01"
    assert resolution.values[0].end == "2019-01-01"


def test_datatypes_resolver_dateRange_last_three_weeks():
    today = datetime(2017, 4, 30)
    resolution = TimexResolver.resolve(["(2019-04-10,2019-05-01,P3W)"], today)

    assert len(resolution.values) == 1
    assert resolution.values[0].timex == "(2019-04-10,2019-05-01,P3W)"
    assert resolution.values[0].type == "daterange"
    assert resolution.values[0].start == "2019-04-10"
    assert resolution.values[0].end == "2019-05-01"


def test_datatypes_resolver_dateRange_last_ten_years():
    today = datetime(2021, 1, 1)
    resolution = TimexResolver.resolve(["(2011-01-01,2021-01-01,P10Y)"], today)
    assert len(resolution.values) == 1
    assert resolution.values[0].timex == "(2011-01-01,2021-01-01,P10Y)"
    assert resolution.values[0].type == "daterange"
    assert resolution.values[0].start == "2011-01-01"
    assert resolution.values[0].end == "2021-01-01"


def test_datatypes_resolver_timeRange_4am_to_8pm():
    today = datetime.now()
    resolution = TimexResolver.resolve(["(T04,T20,PT16H)"], today)

    assert len(resolution.values) == 1
    assert resolution.values[0].timex == "(T04,T20,PT16H)"
    assert resolution.values[0].type == "timerange"
    assert resolution.values[0].start == "04:00:00"
    assert resolution.values[0].end == "20:00:00"
    assert resolution.values[0].value is None


def test_datatypes_resolver_timeRange_morning():
    today = datetime.now()
    resolution = TimexResolver.resolve(["TMO"], today)

    assert len(resolution.values) == 1
    assert resolution.values[0].timex == "TMO"
    assert resolution.values[0].type == "timerange"
    assert resolution.values[0].start == "08:00:00"
    assert resolution.values[0].end == "12:00:00"
    assert resolution.values[0].value is None


def test_datatypes_resolver_timeRange_afternoon():
    today = datetime.now()
    resolution = TimexResolver.resolve(["TAF"], today)

    assert len(resolution.values) == 1
    assert resolution.values[0].timex == "TAF"
    assert resolution.values[0].type == "timerange"
    assert resolution.values[0].start == "12:00:00"
    assert resolution.values[0].end == "16:00:00"
    assert resolution.values[0].value is None


def test_datatypes_resolver_timeRange_evening():
    today = datetime.now()
    resolution = TimexResolver.resolve(["TEV"], today)

    assert len(resolution.values) == 1
    assert resolution.values[0].timex == "TEV"
    assert resolution.values[0].type == "timerange"
    assert resolution.values[0].start == "16:00:00"
    assert resolution.values[0].end == "20:00:00"
    assert resolution.values[0].value is None


def test_datatypes_resolver_dateTimeRange_this_morning():
    today = datetime.now()
    resolution = TimexResolver.resolve(["2017-10-07TMO"], today)

    assert len(resolution.values) == 1
    assert resolution.values[0].timex == "2017-10-07TMO"
    assert resolution.values[0].type == "datetimerange"
    assert resolution.values[0].start == "2017-10-07 08:00:00"
    assert resolution.values[0].end == "2017-10-07 12:00:00"
    assert resolution.values[0].value is None


def test_datatypes_resolver_dateTimeRange_tonight():
    today = datetime.now()
    resolution = TimexResolver.resolve(["2018-03-18TNI"], today)

    assert len(resolution.values) == 1
    assert resolution.values[0].timex == "2018-03-18TNI"
    assert resolution.values[0].type == "datetimerange"
    assert resolution.values[0].start == "2018-03-18 20:00:00"
    assert resolution.values[0].end == "2018-03-18 24:00:00"
    assert resolution.values[0].value is None


def test_datatypes_resolver_dateTimeRange_next_monday_4am_to_next_thursday_3pm():
    today = datetime.now()
    resolution = TimexResolver.resolve(["(2017-10-09T04,2017-10-12T15,PT83H)"], today)

    assert len(resolution.values) == 1
    assert resolution.values[0].timex == "(2017-10-09T04,2017-10-12T15,PT83H)"
    assert resolution.values[0].type == "datetimerange"
    assert resolution.values[0].start == "2017-10-09 04:00:00"
    assert resolution.values[0].end == "2017-10-12 15:00:00"
    assert resolution.values[0].value is None


def test_datatypes_resolver_time_4am():
    resolution = TimexResolver.resolve(["T04"])

    assert len(resolution.values) == 1
    assert resolution.values[0].timex == "T04"
    assert resolution.values[0].type == "time"
    assert resolution.values[0].value == "04:00:00"
    assert resolution.values[0].start is None
    assert resolution.values[0].end is None


def test_datatypes_resolver_time_4_oclock():
    resolution = TimexResolver.resolve(["T04", "T16"])

    assert len(resolution.values) == 2
    assert resolution.values[0].timex == "T04"
    assert resolution.values[0].type == "time"
    assert resolution.values[0].value == "04:00:00"
    assert resolution.values[0].start is None
    assert resolution.values[0].end is None
    assert resolution.values[1].timex == "T16"
    assert resolution.values[1].type == "time"
    assert resolution.values[1].value == "16:00:00"
    assert resolution.values[1].start is None
    assert resolution.values[1].end is None
