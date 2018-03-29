
porting notes:

1) looks like potential problems in:
    TimexHelpers.timex_date_add
    TimexHelpers.timex_time_add
        - time missing seconds duration
        - minutes overflow increments hour but that overflow should...

2) note to check date_from_timex and time_from_timex - tests?
    (in Python the ternary operator is used - is this correct?)

3) back in javascript need to delete duplicate functions from timexFormat

4) TimexConvert weekend l.66 

5) need way more tests on these helpers (DateTime and Timex)

