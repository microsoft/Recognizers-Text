// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.text.datatypes.timex.expression;

import com.microsoft.recognizers.datatypes.timex.expression.TimexCreator;
import com.microsoft.recognizers.datatypes.timex.expression.TimexProperty;
import com.microsoft.recognizers.datatypes.timex.expression.TimexRangeResolver;

import java.math.BigDecimal;
import java.util.ArrayList;
import java.util.HashSet;
import java.util.List;
import java.util.Set;
import java.util.stream.Collectors;

import org.junit.Assert;
import org.junit.Test;

public class TestTimexRangeResolver {
    @Test
    public void dataTypesRangeResolveDaterangeDefinite() {
        Set<String> candidates = new HashSet<String>() {
            {
                add("2017-09-28");
            }
        };
        TimexProperty timex = new TimexProperty() {
            {
                setYear(2017);
                setMonth(9);
                setDayOfMonth(27);
                setDays(new BigDecimal(2));
            }
        };
        ArrayList<String> constraints = new ArrayList<String>() {
            {
                add(timex.getTimexValue());
            }
        };

        List<TimexProperty> result = TimexRangeResolver.evaluate(candidates, constraints);

        Set<String> r = result.stream().map(t -> {
            return t.getTimexValue();
        }).collect(Collectors.toSet());
        Assert.assertTrue(r.contains("2017-09-28"));
        Assert.assertEquals(1, r.size());
    }

    @Test
    public void dataTypesRangeResolveDateRangeDefiniteConstrainstAsTimex() {
        Set<String> candidates = new HashSet<String>() {
            {
                add("2017-09-28");
            }
        };
        ArrayList<String> constraints = new ArrayList<String>() {
            {
                add("(2017-09-27,2017-09-29,P2D)");
            }
        };

        List<TimexProperty> result = TimexRangeResolver.evaluate(candidates, constraints);

        Set<String> r = result.stream().map(t -> {
            return t.getTimexValue();
        }).collect(Collectors.toSet());
        Assert.assertTrue(r.contains("2017-09-28"));
        Assert.assertEquals(1, r.size());
    }

    @Test
    public void dataTypesRangeResolveDateRangeMonthAndDate() {
        Set<String> candidates = new HashSet<String>() {
            {
                add("XXXX-05-29");
            }
        };
        TimexProperty timex = new TimexProperty() {
            {
                setYear(2006);
                setMonth(1);
                setDayOfMonth(1);
                setYears(new BigDecimal(2));
            }
        };
        ArrayList<String> constraints = new ArrayList<String>() {
            {
                add(timex.getTimexValue());
            }
        };

        List<TimexProperty> result = TimexRangeResolver.evaluate(candidates, constraints);

        Set<String> r = result.stream().map(t -> {
            return t.getTimexValue();
        }).collect(Collectors.toSet());
        Assert.assertTrue(r.contains("2006-05-29"));
        Assert.assertTrue(r.contains("2007-05-29"));
        Assert.assertEquals(2, r.size());
    }

    @Test
    public void dataTypesRangeResolveDateRangeMonthAndDateConditional() {
        Set<String> candidates = new HashSet<String>() {
            {
                add("XXXX-05-29");
            }
        };
        ArrayList<String> constraints = new ArrayList<String>() {
            {
                add("(2006-01-01,2008-06-01,P882D)");
            }
        };

        List<TimexProperty> result = TimexRangeResolver.evaluate(candidates, constraints);

        Set<String> r = result.stream().map(t -> {
            return t.getTimexValue();
        }).collect(Collectors.toSet());
        Assert.assertTrue(r.contains("2006-05-29"));
        Assert.assertTrue(r.contains("2007-05-29"));
        Assert.assertTrue(r.contains("2008-05-29"));
        Assert.assertEquals(3, r.size());
    }

    @Test
    public void dataTypesRangeResolveDateRangeSaturdaysInSeptember() {
        Set<String> candidates = new HashSet<String>() {
            {
                add("XXXX-WXX-6");
            }
        };
        ArrayList<String> constraints = new ArrayList<String>() {
            {
                add("2017-09");
            }
        };

        List<TimexProperty> result = TimexRangeResolver.evaluate(candidates, constraints);

        Set<String> r = result.stream().map(t -> {
            return t.getTimexValue();
        }).collect(Collectors.toSet());
        Assert.assertTrue(r.contains("2017-09-02"));
        Assert.assertTrue(r.contains("2017-09-09"));
        Assert.assertTrue(r.contains("2017-09-16"));
        Assert.assertTrue(r.contains("2017-09-23"));
        Assert.assertTrue(r.contains("2017-09-30"));
        Assert.assertEquals(5, r.size());
    }

    @Test
    public void dataTypesRangeResolveDateRangeSaturdaysInSeptemberExpressedAsRange() {
        Set<String> candidates = new HashSet<String>() {
            {
                add("XXXX-WXX-6");
            }
        };
        ArrayList<String> constraints = new ArrayList<String>() {
            {
                add("(2017-09-01,2017-10-01,P30D)");
            }
        };

        List<TimexProperty> result = TimexRangeResolver.evaluate(candidates, constraints);

        Set<String> r = result.stream().map(t -> {
            return t.getTimexValue();
        }).collect(Collectors.toSet());
        Assert.assertTrue(r.contains("2017-09-02"));
        Assert.assertTrue(r.contains("2017-09-09"));
        Assert.assertTrue(r.contains("2017-09-16"));
        Assert.assertTrue(r.contains("2017-09-23"));
        Assert.assertTrue(r.contains("2017-09-30"));
        Assert.assertEquals(5, r.size());
    }

    @Test
    public void dataTypesRangeResolveDateRangeYear() {
        Set<String> candidates = new HashSet<String>() {
            {
                add("XXXX-05-29");
            }
        };
        ArrayList<String> constraints = new ArrayList<String>() {
            {
                add("2018");
            }
        };

        List<TimexProperty> result = TimexRangeResolver.evaluate(candidates, constraints);

        Set<String> r = result.stream().map(t -> {
            return t.getTimexValue();
        }).collect(Collectors.toSet());
        Assert.assertTrue(r.contains("2018-05-29"));
        Assert.assertEquals(1, r.size());
    }

    @Test
    public void dataTypesRangeResolveDateRangeExpressedAsRange() {
        Set<String> candidates = new HashSet<String>() {
            {
                add("XXXX-05-29");
            }
        };
        ArrayList<String> constraints = new ArrayList<String>() {
            {
                add("(2018-01-01,2019-01-01,P365D)");
            }
        };

        List<TimexProperty> result = TimexRangeResolver.evaluate(candidates, constraints);

        Set<String> r = result.stream().map(t -> {
            return t.getTimexValue();
        }).collect(Collectors.toSet());
        Assert.assertTrue(r.contains("2018-05-29"));
        Assert.assertEquals(1, r.size());
    }

    @Test
    public void dataTypesRangeResolveDateRangeMultipleConstraints() {
        Set<String> candidates = new HashSet<String>() {
            {
                add("XXXX-WXX-3");
            }
        };
        ArrayList<String> constraints = new ArrayList<String>() {
            {
                add("(2017-09-01,2017-09-08,P7D)");
                add("(2017-10-01,2017-10-08,P7D)");
            }
        };

        List<TimexProperty> result = TimexRangeResolver.evaluate(candidates, constraints);

        Set<String> r = result.stream().map(t -> {
            return t.getTimexValue();
        }).collect(Collectors.toSet());
        Assert.assertTrue(r.contains("2017-09-06"));
        Assert.assertTrue(r.contains("2017-10-04"));
        Assert.assertEquals(2, r.size());
    }

    @Test
    public void dataTypesRangeResolveDateRangeMultipleCandidatesWithMultipleConstraints() {
        Set<String> candidates = new HashSet<String>() {
            {
                add("XXXX-WXX-2");
                add("XXXX-WXX-4");
            }
        };
        ArrayList<String> constraints = new ArrayList<String>() {
            {
                add("(2017-09-01,2017-09-08,P7D)");
                add("(2017-10-01,2017-10-08,P7D)");
            }
        };

        List<TimexProperty> result = TimexRangeResolver.evaluate(candidates, constraints);

        Set<String> r = result.stream().map(t -> {
            return t.getTimexValue();
        }).collect(Collectors.toSet());
        Assert.assertTrue(r.contains("2017-09-05"));
        Assert.assertTrue(r.contains("2017-09-07"));
        Assert.assertTrue(r.contains("2017-10-03"));
        Assert.assertTrue(r.contains("2017-10-05"));
        Assert.assertEquals(4, r.size());
    }

    @Test
    public void dataTypesRangeResolveDateRangeMultipleOverlappingConstraints() {
        Set<String> candidates = new HashSet<String>() {
            {
                add("XXXX-WXX-3");
            }
        };
        ArrayList<String> constraints = new ArrayList<String>() {
            {
                add("(2017-09-03,2017-09-07,P4D)");
                add("(2017-09-01,2017-09-08,P7D)");
                add("(2017-09-01,2017-09-16,P15D)");
            }
        };

        List<TimexProperty> result = TimexRangeResolver.evaluate(candidates, constraints);

        Set<String> r = result.stream().map(t -> {
            return t.getTimexValue();
        }).collect(Collectors.toSet());
        Assert.assertTrue(r.contains("2017-09-06"));
        Assert.assertEquals(1, r.size());
    }

    @Test
    public void dataTypesRangeResolveTimeRangeTimeWithinRange() {
        Set<String> candidates = new HashSet<String>() {
            {
                add("T16");
            }
        };
        TimexProperty timex = new TimexProperty() {
            {
                setHour(14);
                setHours(new BigDecimal(4));
            }
        };
        ArrayList<String> constraints = new ArrayList<String>() {
            {
                add(timex.getTimexValue());
            }
        };

        List<TimexProperty> result = TimexRangeResolver.evaluate(candidates, constraints);

        Set<String> r = result.stream().map(t -> {
            return t.getTimexValue();
        }).collect(Collectors.toSet());
        Assert.assertTrue(r.contains("T16"));
        Assert.assertEquals(1, r.size());
    }

    @Test
    public void dataTypesRangeResolveTimeRangeMultipleTimesWithinRange() {
        Set<String> candidates = new HashSet<String>() {
            {
                add("T12");
                add("T16");
                add("T16:30");
                add("T17");
                add("T18");
            }
        };
        TimexProperty timex = new TimexProperty() {
            {
                setHour(14);
                setHours(new BigDecimal(4));
            }
        };
        ArrayList<String> constraints = new ArrayList<String>() {
            {
                add(timex.getTimexValue());
            }
        };
        List<TimexProperty> result = TimexRangeResolver.evaluate(candidates, constraints);

        Set<String> r = result.stream().map(t -> {
            return t.getTimexValue();
        }).collect(Collectors.toSet());
        Assert.assertTrue(r.contains("T16"));
        Assert.assertTrue(r.contains("T16:30"));
        Assert.assertTrue(r.contains("T17"));
        Assert.assertEquals(3, r.size());
    }

    @Test
    public void dataTypesRangeResolveTimeRangeTimeWithOverlappingRanges() {
        TimexProperty timex1 = new TimexProperty() {
            {
                setHour(16);
                setHours(new BigDecimal(4));
            }
        };
        ArrayList<String> constraints = new ArrayList<String>() {
            {
                add(timex1.getTimexValue());
            }
        };

        Set<String> candidatesT19 = new HashSet<String>() {
            {
                add("T19");
            }
        };
        List<TimexProperty> result1 = TimexRangeResolver.evaluate(candidatesT19, constraints);

        Set<String> r1 = new HashSet<String>(result1.stream().map(t -> {
            return t.getTimexValue();
        }).collect(Collectors.toSet()));
        Assert.assertTrue(r1.contains("T19"));
        Assert.assertEquals(1, r1.size());

        TimexProperty timex2 = new TimexProperty() {
            {
                setHour(14);
                setHours(new BigDecimal(4));
            }
        };
        constraints.add(timex2.getTimexValue());

        List<TimexProperty> result2 = TimexRangeResolver.evaluate(candidatesT19, constraints);

        Set<String> r2 = new HashSet<String>(result2.stream().map(t -> {
            return t.getTimexValue();
        }).collect(Collectors.toSet()));
        Assert.assertFalse(!r2.isEmpty());

        Set<String> candidatesT17 = new HashSet<String>() {
            {
                add("T17");
            }
        };

        List<TimexProperty> result3 = TimexRangeResolver.evaluate(candidatesT17, constraints);

        Set<String> r3 = new HashSet<String>(result3.stream().map(t -> {
            return t.getTimexValue();
        }).collect(Collectors.toSet()));
        Assert.assertTrue(r3.contains("T17"));
        Assert.assertEquals(1, r3.size());
    }

    @Test
    public void dataTypesRangeResolveMultipleTimesWithOverlappingRanges() {
        TimexProperty timex1 = new TimexProperty() {
            {
                setHour(16);
                setHours(new BigDecimal(4));
            }
        };
        ArrayList<String> constraints = new ArrayList<String>() {
            {
                add(timex1.getTimexValue());
            }
        };

        Set<String> candidatesT191930 = new HashSet<String>() {
            {
                add("T19");
                add("T19:30");
            }
        };
        List<TimexProperty> result1 = TimexRangeResolver.evaluate(candidatesT191930, constraints);

        Set<String> r1 = new HashSet<String>(result1.stream().map(t -> {
            return t.getTimexValue();
        }).collect(Collectors.toSet()));
        Assert.assertTrue(r1.contains("T19"));
        Assert.assertTrue(r1.contains("T19:30"));
        Assert.assertEquals(2, r1.size());

        TimexProperty timex2 = new TimexProperty() {
            {
                setHour(14);
                setHours(new BigDecimal(4));
            }
        };
        constraints.add(timex2.getTimexValue());

        List<TimexProperty> result2 = TimexRangeResolver.evaluate(candidatesT191930, constraints);

        Set<String> r2 = new HashSet<String>(result2.stream().map(t -> {
            return t.getTimexValue();
        }).collect(Collectors.toSet()));
        Assert.assertFalse(!r2.isEmpty());

        Set<String> candidatesT17173019 = new HashSet<String>() {
            {
                add("T17");
                add("T17:30");
                add("T19");
            }
        };

        List<TimexProperty> result3 = TimexRangeResolver.evaluate(candidatesT17173019, constraints);

        Set<String> r3 = new HashSet<String>(result3.stream().map(t -> {
            return t.getTimexValue();
        }).collect(Collectors.toSet()));
        Assert.assertTrue(r3.contains("T17"));
        Assert.assertTrue(r3.contains("T17:30"));
        Assert.assertEquals(2, r3.size());
    }

    @Test
    public void dataTypesRangeResolveFilterDuplicate() {
        TimexProperty timex = new TimexProperty() {
            {
                setHour(16);
                setHours(new BigDecimal(4));
            }
        };
        ArrayList<String> constraints = new ArrayList<String>() {
            {
                add(timex.getTimexValue());
            }
        };
        Set<String> candidates = new HashSet<String>() {
            {
                add("T16");
                add("T16");
                add("T16");
            }
        };

        List<TimexProperty> result = TimexRangeResolver.evaluate(candidates, constraints);

        Set<String> r = result.stream().map(t -> {
            return t.getTimexValue();
        }).collect(Collectors.toSet());
        Assert.assertTrue(r.contains("T16"));
        Assert.assertEquals(1, r.size());
    }

    @Test
    public void dataTypesRangeResolveCarryThroughTimeDefinite() {
        TimexProperty timex = new TimexProperty() {
            {
                setYear(2017);
                setMonth(9);
                setDayOfMonth(27);
                setDays(new BigDecimal(2));
            }
        };
        ArrayList<String> constraints = new ArrayList<String>() {
            {
                add(timex.getTimexValue());
            }
        };
        Set<String> candidates = new HashSet<String>() {
            {
                add("2017-09-28T18:30:01");
            }
        };
        List<TimexProperty> result = TimexRangeResolver.evaluate(candidates, constraints);

        Set<String> r = result.stream().map(t -> {
            return t.getTimexValue();
        }).collect(Collectors.toSet());
        Assert.assertTrue(r.contains("2017-09-28T18:30:01"));
        Assert.assertEquals(1, r.size());
    }

    @Test
    public void dataTypesRangeResolveCarryThroughTimeDefiniteConstrainstExpressedAsTimex() {
        ArrayList<String> constraints = new ArrayList<String>() {
            {
                add("(2017-09-27,2017-09-29,P2D)");
            }
        };

        Set<String> candidates = new HashSet<String>() {
            {
                add("2017-09-28T18:30:01");
            }
        };
        List<TimexProperty> result = TimexRangeResolver.evaluate(candidates, constraints);

        Set<String> r = result.stream().map(t -> {
            return t.getTimexValue();
        }).collect(Collectors.toSet());
        Assert.assertTrue(r.contains("2017-09-28T18:30:01"));
        Assert.assertEquals(1, r.size());
    }

    @Test
    public void dataTypesRangeResolveCarryThroughTimeMonthAndDate() {
        TimexProperty timex = new TimexProperty() {
            {
                setYear(2006);
                setMonth(1);
                setDayOfMonth(1);
                setYears(new BigDecimal(2));
            }
        };
        ArrayList<String> constraints = new ArrayList<String>() {
            {
                add(timex.getTimexValue());
            }
        };
        Set<String> candidates = new HashSet<String>() {
            {
                add("XXXX-05-29T19:30");
            }
        };
        List<TimexProperty> result = TimexRangeResolver.evaluate(candidates, constraints);

        Set<String> r = result.stream().map(t -> {
            return t.getTimexValue();
        }).collect(Collectors.toSet());
        Assert.assertTrue(r.contains("2006-05-29T19:30"));
        Assert.assertTrue(r.contains("2007-05-29T19:30"));
        Assert.assertEquals(2, r.size());
    }

    @Test
    public void dataTypesRangeResolveCarryThroughTimeMonthAndDateConditional() {
        ArrayList<String> constraints = new ArrayList<String>() {
            {
                add("(2006-01-01,2008-06-01,P882D)");
            }
        };

        Set<String> candidates = new HashSet<String>() {
            {
                add("XXXX-05-29T19:30");
            }
        };
        List<TimexProperty> result = TimexRangeResolver.evaluate(candidates, constraints);

        Set<String> r = result.stream().map(t -> {
            return t.getTimexValue();
        }).collect(Collectors.toSet());
        Assert.assertTrue(r.contains("2006-05-29T19:30"));
        Assert.assertTrue(r.contains("2007-05-29T19:30"));
        Assert.assertTrue(r.contains("2008-05-29T19:30"));
        Assert.assertEquals(3, r.size());
    }

    @Test
    public void dataTypesRangeResolveCarryThroughTimeSaturdaysInSeptember() {
        ArrayList<String> constraints = new ArrayList<String>() {
            {
                add("(2017-09-01,2017-10-01,P30D)");
            }
        };

        Set<String> candidates = new HashSet<String>() {
            {
                add("XXXX-WXX-6T01:00:00");
            }
        };
        List<TimexProperty> result = TimexRangeResolver.evaluate(candidates, constraints);

        Set<String> r = result.stream().map(t -> {
            return t.getTimexValue();
        }).collect(Collectors.toSet());
        Assert.assertTrue(r.contains("2017-09-02T01"));
        Assert.assertTrue(r.contains("2017-09-09T01"));
        Assert.assertTrue(r.contains("2017-09-16T01"));
        Assert.assertTrue(r.contains("2017-09-23T01"));
        Assert.assertTrue(r.contains("2017-09-30T01"));
        Assert.assertEquals(5, r.size());
    }

    @Test
    public void dataTypesRangeResolveCarryThroughTimeMultipleConstraints() {
        ArrayList<String> constraints = new ArrayList<String>() {
            {
                add("(2017-09-01,2017-09-08,P7D)");
                add("(2017-10-01,2017-10-08,P7D)");
            }
        };

        Set<String> candidates = new HashSet<String>() {
            {
                add("XXXX-WXX-3T01:02");
            }
        };
        List<TimexProperty> result = TimexRangeResolver.evaluate(candidates, constraints);

        Set<String> r = result.stream().map(t -> {
            return t.getTimexValue();
        }).collect(Collectors.toSet());
        Assert.assertTrue(r.contains("2017-09-06T01:02"));
        Assert.assertTrue(r.contains("2017-10-04T01:02"));
        Assert.assertEquals(2, r.size());
    }

    @Test
    public void dataTypesRangeResolveCombinedDaterangeAndTimeRangeNextWeekAndAnyTime() {
        TimexProperty timex1 = new TimexProperty() {
            {
                setYear(2017);
                setMonth(10);
                setDayOfMonth(5);
                setDays(new BigDecimal(7));
            }
        };
        TimexProperty timex2 = new TimexProperty() {
            {
                setHour(0);
                setMinute(0);
                setSecond(0);
                setHours(new BigDecimal(24));
            }
        };
        ArrayList<String> constraints = new ArrayList<String>() {
            {
                add(timex1.getTimexValue());
                add(timex2.getTimexValue());
            }
        };

        Set<String> candidates = new HashSet<String>() {
            {
                add("XXXX-WXX-3T04");
                add("XXXX-WXX-3T16");
            }
        };
        List<TimexProperty> result = TimexRangeResolver.evaluate(candidates, constraints);

        Set<String> r = result.stream().map(t -> {
            return t.getTimexValue();
        }).collect(Collectors.toSet());
        Assert.assertTrue(r.contains("2017-10-11T04"));
        Assert.assertTrue(r.contains("2017-10-11T16"));
        Assert.assertEquals(2, r.size());
    }

    @Test
    public void dataTypesRangeResolveDaterangeAndTimeRangeNextWeekAndBusinessHours() {
        TimexProperty timex1 = new TimexProperty() {
            {
                setYear(2017);
                setMonth(10);
                setDayOfMonth(5);
                setDays(new BigDecimal(7));
            }
        };
        TimexProperty timex2 = new TimexProperty() {
            {
                setHour(12);
                setMinute(0);
                setSecond(0);
                setHours(new BigDecimal(8));
            }
        };
        ArrayList<String> constraints = new ArrayList<String>() {
            {
                add(timex1.getTimexValue());
                add(timex2.getTimexValue());
            }
        };

        Set<String> candidates = new HashSet<String>() {
            {
                add("XXXX-WXX-3T04");
                add("XXXX-WXX-3T16");
            }
        };
        List<TimexProperty> result = TimexRangeResolver.evaluate(candidates, constraints);

        Set<String> r = result.stream().map(t -> {
            return t.getTimexValue();
        }).collect(Collectors.toSet());
        Assert.assertTrue(r.contains("2017-10-11T16"));
        Assert.assertEquals(1, r.size());
    }

    @Test
    public void dataTypesRangeResolveAddingTimesAddSpecificTimeToDate() {
        ArrayList<String> constraints = new ArrayList<String>() {
            {
                add("2017");
                add("T19:30:00");
            }
        };

        Set<String> candidates = new HashSet<String>() {
            {
                add("XXXX-05-29");
            }
        };
        List<TimexProperty> result = TimexRangeResolver.evaluate(candidates, constraints);

        Set<String> r = result.stream().map(t -> {
            return t.getTimexValue();
        }).collect(Collectors.toSet());
        Assert.assertTrue(r.contains("2017-05-29T19:30"));
        Assert.assertEquals(1, r.size());
    }

    @Test
    public void dataTypesRangeResolveAddingTimesAddSpecificTimeToDate2() {
        ArrayList<String> constraints = new ArrayList<String>() {
            {
                add("2017");
                add("T19:30:00");
                add("T20:01:01");
            }
        };

        Set<String> candidates = new HashSet<String>() {
            {
                add("XXXX-05-29");
            }
        };
        List<TimexProperty> result = TimexRangeResolver.evaluate(candidates, constraints);

        Set<String> r = result.stream().map(t -> {
            return t.getTimexValue();
        }).collect(Collectors.toSet());
        Assert.assertTrue(r.contains("2017-05-29T19:30"));
        Assert.assertTrue(r.contains("2017-05-29T20:01:01"));
        Assert.assertEquals(2, r.size());
    }

    @Test
    public void dataTypesRangeResolveDurationSpecificDatetime() {
        ArrayList<String> constraints = new ArrayList<String>() {
            {
                add("2017-12-05T19:30:00");
            }
        };

        Set<String> candidates = new HashSet<String>() {
            {
                add("PT5M");
            }
        };
        List<TimexProperty> result = TimexRangeResolver.evaluate(candidates, constraints);

        Set<String> r = result.stream().map(t -> {
            return t.getTimexValue();
        }).collect(Collectors.toSet());
        Assert.assertTrue(r.contains("2017-12-05T19:35"));
        Assert.assertEquals(1, r.size());
    }

    @Test
    public void dataTypesRangeResolveDurationSpecificTime() {
        ArrayList<String> constraints = new ArrayList<String>() {
            {
                add("T19:30:00");
            }
        };

        Set<String> candidates = new HashSet<String>() {
            {
                add("PT5M");
            }
        };
        List<TimexProperty> result = TimexRangeResolver.evaluate(candidates, constraints);

        Set<String> r = result.stream().map(t -> {
            return t.getTimexValue();
        }).collect(Collectors.toSet());
        Assert.assertTrue(r.contains("T19:35"));
        Assert.assertEquals(1, r.size());
    }

    @Test
    public void dataTypesRangeResolveDurationNoConstraints() {
        ArrayList<String> constraints = new ArrayList<String>();

        Set<String> candidates = new HashSet<String>() {
            {
                add("PT5M");
            }
        };
        List<TimexProperty> result = TimexRangeResolver.evaluate(candidates, constraints);

        Set<String> r = result.stream().map(t -> {
            return t.getTimexValue();
        }).collect(Collectors.toSet());
        Assert.assertFalse(!r.isEmpty());
    }

    @Test
    public void dataTypesRangeResolveDurationNoTimeComponent() {
        TimexProperty timex = new TimexProperty() {
            {
                setYear(2017);
                setMonth(10);
                setDayOfMonth(5);
                setDays(new BigDecimal(7));
            }
        };
        ArrayList<String> constraints = new ArrayList<String>() {
            {
                add(timex.getTimexValue());
            }
        };

        Set<String> candidates = new HashSet<String>() {
            {
                add("PT5M");
            }
        };
        List<TimexProperty> result = TimexRangeResolver.evaluate(candidates, constraints);

        Set<String> r = result.stream().map(t -> {
            return t.getTimexValue();
        }).collect(Collectors.toSet());
        Assert.assertFalse(!r.isEmpty());
    }

    @Test
    public void dataTypesRangeResolveDateRanges() {
        ArrayList<String> constraints = new ArrayList<String>() {
            {
                add("(2018-06-04,2018-06-11,P7D)"); // e.g. this week
                add("(2018-06-11,2018-06-18,P7D)"); // e.g. next week
                add(TimexCreator.EVENING);

            }
        };

        Set<String> candidates = new HashSet<String>() {
            {
                add("XXXX-WXX-7");
            }
        };

        List<TimexProperty> result = TimexRangeResolver.evaluate(candidates, constraints);

        Set<String> r = result.stream().map(t -> {
            return t.getTimexValue();
        }).collect(Collectors.toSet());
        Assert.assertTrue(r.contains("2018-06-10T16"));
        Assert.assertTrue(r.contains("2018-06-17T16"));
        Assert.assertEquals(2, r.size());
    }

    @Test
    public void dataTypesRangeResolveDateRangesNoTimeConstraint() {
        Set<String> candidates = new HashSet<String>() {
            {
                add("XXXX-WXX-7TEV");
            }
        };
        ArrayList<String> constraints = new ArrayList<String>() {
            {
                add("(2018-06-04,2018-06-11,P7D)"); // e.g. this week
                add("(2018-06-11,2018-06-18,P7D)"); // e.g. next week
            }
        };

        List<TimexProperty> result = TimexRangeResolver.evaluate(candidates, constraints);

        Set<String> r = result.stream().map(t -> {
            return t.getTimexValue();
        }).collect(Collectors.toSet());
        Assert.assertTrue(r.contains("2018-06-10TEV"));
        Assert.assertTrue(r.contains("2018-06-17TEV"));
        Assert.assertEquals(2, r.size());
    }

    @Test
    public void dataTypesRangeResolveDateRangesOverlappingConstraint1() {
        Set<String> candidates = new HashSet<String>() {
            {
                add("XXXX-WXX-7TEV");
            }
        };
        ArrayList<String> constraints = new ArrayList<String>() {
            {
                add("(2018-06-04,2018-06-11,P7D)"); // e.g. this week
                add("(2018-06-11,2018-06-18,P7D)"); // e.g. next week
                add("(T18,T22,PT4H)");

            }
        };

        List<TimexProperty> result = TimexRangeResolver.evaluate(candidates, constraints);

        Set<String> r = result.stream().map(t -> {
            return t.getTimexValue();
        }).collect(Collectors.toSet());
        Assert.assertTrue(r.contains("2018-06-10T18"));
        Assert.assertTrue(r.contains("2018-06-17T18"));
        Assert.assertEquals(2, r.size());
    }

    @Test
    public void dataTypesRangeResolveDateRangesOverlappingConstraint2() {
        Set<String> candidates = new HashSet<String>() {
            {
                add("XXXX-WXX-7TEV");
            }
        };
        ArrayList<String> constraints = new ArrayList<String>() {
            {
                add("(2018-06-04,2018-06-11,P7D)"); // e.g. this week
                add("(2018-06-11,2018-06-18,P7D)"); // e.g. next week
                add("(T15,T19,PT4H)");

            }
        };

        List<TimexProperty> result = TimexRangeResolver.evaluate(candidates, constraints);

        Set<String> r = result.stream().map(t -> {
            return t.getTimexValue();
        }).collect(Collectors.toSet());
        Assert.assertTrue(r.contains("2018-06-10T16"));
        Assert.assertTrue(r.contains("2018-06-17T16"));
        Assert.assertEquals(2, r.size());
    }

    @Test
    public void dataTypesRangeResolveDateRangesNonOverlappingConstraint() {
        Set<String> candidates = new HashSet<String>() {
            {
                add("XXXX-WXX-7TEV");
            }
        };
        ArrayList<String> constraints = new ArrayList<String>() {
            {
                add("(2018-06-04,2018-06-11,P7D)"); // e.g. this week
                add("(2018-06-11,2018-06-18,P7D)"); // e.g. next week
                add(TimexCreator.MORNING);

            }
        };

        List<TimexProperty> result = TimexRangeResolver.evaluate(candidates, constraints);

        Assert.assertFalse(!result.isEmpty());
    }

    @Test
    public void dataTypesRangeResolveDateRangesSundayEvening() {
        Set<String> candidates = new HashSet<String>() {
            {
                add("XXXX-WXX-7TEV");
            }
        };
        ArrayList<String> constraints = new ArrayList<String>() {
            {
                add("(2018-06-04,2018-06-11,P7D)"); // e.g. this week
                add("(2018-06-11,2018-06-18,P7D)"); // e.g. next week
                add(TimexCreator.EVENING);

            }
        };

        List<TimexProperty> result = TimexRangeResolver.evaluate(candidates, constraints);

        Set<String> r = result.stream().map(t -> {
            return t.getTimexValue();
        }).collect(Collectors.toSet());
        Assert.assertTrue(r.contains("2018-06-10T16"));
        Assert.assertTrue(r.contains("2018-06-17T16"));
        Assert.assertEquals(2, r.size());
    }

    @Test
    public void dataTypesRangeResolveTime() {
        Set<String> candidates = new HashSet<String>() {
            {
                add("T09");
            }
        };
        ArrayList<String> constraints = new ArrayList<String>() {
            {
                add("(2020-01-01,2020-01-02,P1D)");
            }
        };
        List<TimexProperty> resolutions = TimexRangeResolver.evaluate(candidates, constraints);
        Assert.assertEquals(1, resolutions.size());
    }

    @Test
    public void dataTypesRangeResolveTimeWithDateRangeConstraint() {
        Set<String> candidates = new HashSet<String>() {
            {
                add("T09");
            }
        };
        ArrayList<String> constraints = new ArrayList<String>() {
            {
                add("P3D");
            }
        };
        List<TimexProperty> resolutions = TimexRangeResolver.evaluate(candidates, constraints);
        Assert.assertEquals(1, resolutions.size());
    }

    @Test
    public void dataTypesRangeResolveTimeWithDateTimeRangeConstraint() {
        Set<String> candidates = new HashSet<String>() {
            {
                add("T09");
            }
        };
        ArrayList<String> constraints = new ArrayList<String>() {
            {
                add("(2020-01-01T00:00:00,2020-01-02T00:00:00,PT24H)");
            }
        };
        List<TimexProperty> resolutions = TimexRangeResolver.evaluate(candidates, constraints);

        Assert.assertEquals(1, resolutions.size());
    }

    @Test
    public void dataTypesRangeResolveDateTimeWithDateRangeConstraint() {
        Set<String> candidates = new HashSet<String>() {
            {
                add("2020-01-01T09");
                add("2020-01-02T09");
            }
        };
        ArrayList<String> constraints = new ArrayList<String>() {
            {
                add("(2020-01-01,2020-01-02,P1D)");
            }
        };
        List<TimexProperty> resolutions = TimexRangeResolver.evaluate(candidates, constraints);
        Assert.assertEquals(1, resolutions.size());
        Assert.assertEquals(1, (int)resolutions.stream().findFirst().get().getMonth());
    }
}
