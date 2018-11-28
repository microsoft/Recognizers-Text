package com.microsoft.recognizers.text.datetime.utilities;

import com.microsoft.recognizers.text.utilities.Match;

import java.util.Optional;

public class ConditionalMatch {

    private final Optional<Match> match;
    private final boolean success;

    public ConditionalMatch(Optional<Match> match, boolean success) {
        this.match = match;
        this.success = success;
    }

    public Optional<Match> getMatch() {

        return match;
    }

    public boolean getSuccess() {

        return success;
    }

}
