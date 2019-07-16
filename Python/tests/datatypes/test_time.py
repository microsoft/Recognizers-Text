from datatypes_timex_expression import Time


def test_datatypes_time_constructor():
    time = Time(23, 45, 32)

    assert time.hour is 23
    assert time.minute is 45
    assert time.second is 32


def test_datatypes_time_gettime():
    time = Time(23, 45, 32)

    assert time.get_time() == 85532000
