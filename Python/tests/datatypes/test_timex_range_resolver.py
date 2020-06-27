from datatypes_timex_expression import Timex, TimexRangeResolver, TimexCreator


def test_datatypes_rangeresolve_daterange_definite():
    eval_constraints_and_candidates(['2017-09-28'], [Timex(year=2017, month=9, day_of_month=27, days=2).timex_value()], ['2017-09-28'])


def test_datatypes_rangeresolve_daterange_definite_constraints_as_timex():
    candidates = ['2017-09-28']
    constraints = ['(2017-09-27,2017-09-29,P2d)']
    eval_constraints_and_candidates(candidates, constraints, ['2017-09-28'])


def test_datatypes_rangeresolve_daterange_month_and_date():
    candidates = ["XXXX-05-29"]
    constraints = [Timex(year=2006, month=1, day_of_month=1, years=2).timex_value()]
    eval_constraints_and_candidates(candidates, constraints, ['2006-05-29', '2007-05-29'])


def test_datatypes_rangeresolve_daterange_saturdays_in_september():
    candidates = ['XXXX-WXX-6']
    constraints = ['2017-09']
    eval_constraints_and_candidates(candidates, constraints, ['2017-09-02', '2017-09-09', '2017-09-16', '2017-09-23', '2017-09-30'])


def test_datatypes_rangeresolve_daterange_saturdays_in_september_expressed_as_range():
    candidates = ['XXXX-WXX-6']
    constraints = ['(2017-09-01,2017-10-01,P30D)']
    eval_constraints_and_candidates(candidates, constraints, ['2017-09-02', '2017-09-09', '2017-09-16', '2017-09-23', '2017-09-30'])


def test_datatypes_rangeresolve_daterange_year():
    candidates = ['XXXX-05-29']
    constraints = ['2018']
    eval_constraints_and_candidates(candidates, constraints, ['2018-05-29'])


def test_datatypes_rangeresolve_daterange_expressed_as_range():
    candidates = ['XXXX-05-29']
    constraints = ['(2018-01-01,2019-01-01,P365D)']
    eval_constraints_and_candidates(candidates, constraints, ['2018-05-29'])


def test_datatypes_rangeresolve_daterange__multiple_constraints():
    candidates = ['XXXX-WXX-3']
    constraints = ['(2017-09-01,2017-09-08,P7D)', '(2017-10-01,2017-10-08,P7D)']
    eval_constraints_and_candidates(candidates, constraints, ['2017-09-06', '2017-10-04'])


def test_datatypes_rangeresolve_daterange__multiple_candidates_with_multiple_constraints():
    candidates = ['XXXX-WXX-2', 'XXXX-WXX-4']
    constraints = ["(2017-09-01,2017-09-08,P7D)",
                   "(2017-10-01,2017-10-08,P7D)"]
    eval_constraints_and_candidates(candidates, constraints, ["2017-09-05", "2017-09-07", "2017-10-03", "2017-10-05"])


def test_datatypes_rangeresolve_daterange_multiple_overlapping_constraints():
    candidates = ["XXXX-WXX-3"]
    constraints = ["(2017-09-03,2017-09-07,P4D)",
                   "(2017-09-01,2017-09-08,P7D)",
                   "(2017-09-01,2017-09-16,P15D)"]
    eval_constraints_and_candidates(candidates, constraints, ['2017-09-06'])


def test_datatypes_rangeresolve_timerange_time_within_range():
    candidates = ['T16']
    constraints = [Timex(hour=14, hours=4).timex_value()]
    eval_constraints_and_candidates(candidates, constraints, ['T16'])


def test_datatypes_rangeresolve_timerange_multiple_times_within_range():
    candidates = ["T12", "T16", "T16:30", "T17", "T18"]
    constraints = [Timex(hour=14, minute=0, second=0, hours=4).timex_value()]
    eval_constraints_and_candidates(candidates, constraints, ['T16', 'T16:30', 'T17'])


def test_DataTypes_RangeResolve_timerange_time_with_overlapping_ranges():
    candidates = ['T19']
    constraints = [Timex(hour=16, minute=0, second=0, hours=4).timex_value()]
    eval_constraints_and_candidates(candidates, constraints, ['T19'])

    constraints.append(Timex(hour=14, minute=0, second=0, hours=4).timex_value())

    eval_constraints_and_candidates(candidates, constraints, [])

    candidates = ['T17']
    eval_constraints_and_candidates(candidates, constraints, ['T17'])


def test_DataTypes_RangeResolve_multiple_times_with_overlapping_range():
    candidates = ["T19", "T19:30"]
    constraints = [Timex(hour=16, minute=0, second=0, hours=4).timex_value()]
    eval_constraints_and_candidates(candidates, constraints, ["T19", "T19:30"])

    constraints.append(Timex(hour=14, minute=0, second=0, hours=4).timex_value())

    eval_constraints_and_candidates(candidates, constraints, [])

    candidates = ["T17", "T17:30", "T19"]
    eval_constraints_and_candidates(candidates, constraints, ["T17", "T17:30"])


def test_DataTypes_RangeResolve_filter_duplicate():
    candidates = ["T16", "T16", "T16"]
    constraints = [Timex(hour=16, minute=0, second=0, hours=4).timex_value()]
    eval_constraints_and_candidates(candidates, constraints, ['T16'])


def test_DataTypes_RangeResolve_carry_through_time_definite():
    candidates = ['2017-09-28T18:30:01']
    constraints = [Timex(year=2017, month=9, day_of_month=27, days=2).timex_value()]
    eval_constraints_and_candidates(candidates, constraints, ['2017-09-28T18:30:01'])


def test_DataTypes_RangeResolve_carry_through_time_definite_constrainst_expressed_as_timex():
    candidates = ['2017-09-28T18:30:01']
    constraints = ['(2017-09-27,2017-09-29,P2D)']
    eval_constraints_and_candidates(candidates, constraints, ['2017-09-28T18:30:01'])


def test_DataTypes_RangeResolve_carry_through_time_month_and_date():
    candidates = ['XXXX-05-29T19:30']
    constraints = [Timex(year=2006, month=1, day_of_month=1, years=2).timex_value()]
    eval_constraints_and_candidates(candidates, constraints, ['2006-05-29T19:30', '2007-05-29T19:30'])


def test_DataTypes_RangeResolve_carry_through_time_month_and_date_conditiona():
    candidates = ['XXXX-05-29T19:30']
    constraints = ['(2006-01-01,2008-06-01,P882D)']
    eval_constraints_and_candidates(candidates, constraints, ['2006-05-29T19:30', '2007-05-29T19:30', '2008-05-29T19:30'])


def test_DataTypes_RangeResolve_carry_through_time_Saturdays_in_September():
    candidates = ['XXXX-WXX-6T01:00:00']
    constraints = ['(2017-09-01,2017-10-01,P30D)']
    eval_constraints_and_candidates(candidates, constraints, ['2017-09-02T01', '2017-09-09T01', '2017-09-16T01', '2017-09-23T01', '2017-09-30T01'])


def test_DataTypes_RangeResolve__carry_through_time_multiple_constraints():
    candidates = ['XXXX-WXX-3T01:02']
    constraints = ['(2017-09-01,2017-09-08,P7D)', '(2017-10-01,2017-10-08,P7D)']
    eval_constraints_and_candidates(candidates, constraints, ['2017-09-06T01:02', '2017-10-04T01:02'])


def test_DataTypes_RangeResolve_combined_daterange_and_timerange_next_week_and_any_time():
    candidates = ['XXXX-WXX-3T04', 'XXXX-WXX-3T16']
    constraints = [Timex(year=2017, month=10, day_of_month=5, days=7).timex_value(),
                   Timex(hour=0, minute=0, second=0, hours=24).timex_value()]
    eval_constraints_and_candidates(candidates, constraints, ['2017-10-11T04', '2017-10-11T16'])


def test_DataTypes_RangeResolve_combined_daterange_and_timerange_next_week_and_business_hours():
    candidates = ['XXXX-WXX-3T04', 'XXXX-WXX-3T16']
    constraints = [Timex(year=2017, month=10, day_of_month=5, days=7).timex_value(),
                   Timex(hour=12, minute=0, second=0, hours=8).timex_value()]
    eval_constraints_and_candidates(candidates, constraints, ['2017-10-11T16'])


def test_DataTypes_RangeResolve_adding_times_add_specific_time_to_date():
    constraints = ['2017', 'T19:30:00']
    candidates = ['XXXX-05-29']
    eval_constraints_and_candidates(candidates, constraints, ['2017-05-29T19:30'])


def test_DataTypes_RangeResolve_adding_times_add_specific_time_to_date_2():
    constraints = ['2017', 'T19:30:00', 'T20:01:01']
    candidates = ['XXXX-05-29']
    eval_constraints_and_candidates(candidates, constraints, ['2017-05-29T19:30', '2017-05-29T20:01:01'])


def test_DataTypes_RangeResolve_duration_specific_datetime():
    constraints = ['2017-12-05T19:30:00']
    candidates = ['PT5M']
    eval_constraints_and_candidates(candidates, constraints, ['2017-12-05T19:35'])


def test_DataTypes_RangeResolve_duration_specific_time():
    constraints = ['T19:30:00']
    candidates = ['PT5M']
    eval_constraints_and_candidates(candidates, constraints, ['T19:35'])


def test_DataTypes_RangeResolve_duration_no_constraints():
    constraints = []
    candidates = ['PT5M']
    eval_constraints_and_candidates(candidates, constraints, [])


def test_DataTypes_RangeResolve_duration_no_time_component():
    constraints = [Timex(year=2017, month=10, day_of_month=5, days=7).timex_value()]
    candidates = ['PT5M']
    eval_constraints_and_candidates(candidates, constraints, [])


def test_DataTypes_RangeResolve_dateranges():
    candidates = ['XXXX-WXX-7']
    constraints = ['(2018-06-04,2018-06-11,P7D)', '(2018-06-11,2018-06-18,P7D)', TimexCreator.EVENING]
    eval_constraints_and_candidates(candidates, constraints, ['2018-06-10T16', '2018-06-17T16'])


def test_DataTypes_RangeResolve_dateranges_no_time_constraint():
    candidates = ['XXXX-WXX-7TEV']
    constraints = ['(2018-06-04,2018-06-11,P7D)', '(2018-06-11,2018-06-18,P7D)']
    eval_constraints_and_candidates(candidates, constraints, ['2018-06-10TEV', '2018-06-17TEV'])


def test_DataTypes_RangeResolve_dateranges_overlapping_constraint_1():
    candidates = ['XXXX-WXX-7TEV']
    constraints = ['(2018-06-04,2018-06-11,P7D)',
                   '(2018-06-11,2018-06-18,P7D)',
                   '(T18,T22,PT4H)']
    eval_constraints_and_candidates(candidates, constraints, ['2018-06-10T18',
                                                              '2018-06-17T18'])


def test_DataTypes_RangeResolve_dateranges_overlapping_constraint_2():
    candidates = ['XXXX-WXX-7TEV']
    constraints = ['(2018-06-04,2018-06-11,P7D)',
                   '(2018-06-11,2018-06-18,P7D)',
                   '(T15,T19,PT4H)']
    eval_constraints_and_candidates(candidates, constraints, ['2018-06-10T16',
                                                              '2018-06-17T16'])


def test_DataTypes_RangeResolve_dateranges_non_overlapping_constraint():
    candidates = ['XXXX-WXX-7TEV']
    constraints = ['(2018-06-04,2018-06-11,P7D)',
                   '(2018-06-11,2018-06-18,P7D)',
                   '(T18,T22,PT4H)']
    eval_constraints_and_candidates(candidates, constraints, ['2018-06-10T18',
                                                              '2018-06-17T18'])


def test_DataTypes_RangeResolve_dateranges_non_overlapping_constraint():
    candidates = ['XXXX-WXX-7TEV']
    constraints = ['(2018-06-04,2018-06-11,P7D)',
                   '(2018-06-11,2018-06-18,P7D)',
                   TimexCreator.EVENING]
    eval_constraints_and_candidates(candidates, constraints, ['2018-06-10T16',
                                                              '2018-06-17T16'])


def eval_constraints_and_candidates(candidates, constraints, results):
    result = TimexRangeResolver.evaluate(candidates, constraints)
    map_results_to_timex_value = list(map(lambda t: t.timex_value(), result))

    if not results:
        assert map_results_to_timex_value == []
    for r in results:
        assert r in map_results_to_timex_value

    assert len(map_results_to_timex_value) == len(results)
