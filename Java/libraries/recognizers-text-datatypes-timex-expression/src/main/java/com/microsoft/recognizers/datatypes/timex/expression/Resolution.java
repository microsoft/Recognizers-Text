// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.datatypes.timex.expression;

import java.util.ArrayList;
import java.util.List;

public class Resolution {
    private List<Entry> values;

    public List<Entry> getValues() {
        return this.values;
    }

    public Resolution() {
        this.values = new ArrayList<Entry>();
    }

    public static class Entry {
        private String timex;

        private String type;

        private String value;

        private String start;

        private String end;

        public String getTimex() {
            return timex;
        }

        public void setTimex(String withTimex) {
            this.timex = withTimex;
        }

        public String getType() {
            return type;
        }

        public void setType(String withType) {
            this.type = withType;
        }

        public String getValue() {
            return value;
        }

        public void setValue(String withValue) {
            this.value = withValue;
        }

        public String getStart() {
            return start;
        }

        public void setStart(String withStart) {
            this.start = withStart;
        }

        public String getEnd() {
            return end;
        }

        public void setEnd(String withEnd) {
            this.end = withEnd;
        }
    }
}
