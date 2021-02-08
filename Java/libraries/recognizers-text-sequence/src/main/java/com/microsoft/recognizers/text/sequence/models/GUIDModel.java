// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.text.sequence.models;

import com.microsoft.recognizers.text.ExtractResult;
import com.microsoft.recognizers.text.IExtractor;
import com.microsoft.recognizers.text.IParser;
import com.microsoft.recognizers.text.ModelResult;
import com.microsoft.recognizers.text.ParseResult;
import com.microsoft.recognizers.text.ResolutionKey;
import com.microsoft.recognizers.text.sequence.Constants;
import com.microsoft.recognizers.text.utilities.QueryProcessor;

import java.util.ArrayList;
import java.util.List;
import java.util.TreeMap;
import java.util.stream.Collectors;

public class GUIDModel extends AbstractSequenceModel {
    public GUIDModel(IParser parser, IExtractor extractor) {
        super(parser, extractor);
        this.modelTypeName = Constants.MODEL_GUID;
    }

    @Override
    public List<ModelResult> parse(String query) {
        List<ParseResult> parsedSequences = new ArrayList<ParseResult>();

        // Preprocess the query
        query = QueryProcessor.preprocess(query);

        try {
            List<ExtractResult> extractResults = extractor.extract(query);

            for (ExtractResult result : extractResults) {
                parsedSequences.add(this.parser.parse(result));
            }
        } catch (Exception ex) {
            // Nothing to do. Exceptions in parse should not break users of recognizers.
            // No result.
        }

        return parsedSequences.stream().map(o -> {
            return new ModelResult(o.getText(), o.getStart(), o.getStart() + o.getLength() - 1, modelTypeName,
                    new TreeMap<String, Object>() {
                        {
                            put(ResolutionKey.Value, o.getResolutionStr());
                            put(ResolutionKey.Score, o.getValue());
                        }
                    });
        }).collect(Collectors.toList());
    }
}
