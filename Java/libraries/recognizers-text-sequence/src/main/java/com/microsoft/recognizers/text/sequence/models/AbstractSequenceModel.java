// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.text.sequence.models;

import com.microsoft.recognizers.text.ExtractResult;
import com.microsoft.recognizers.text.IExtractor;
import com.microsoft.recognizers.text.IModel;
import com.microsoft.recognizers.text.IParser;
import com.microsoft.recognizers.text.ModelResult;
import com.microsoft.recognizers.text.ParseResult;
import com.microsoft.recognizers.text.ResolutionKey;

import java.util.ArrayList;
import java.util.List;
import java.util.TreeMap;
import java.util.stream.Collectors;

public abstract class AbstractSequenceModel implements IModel {
    protected String modelTypeName;
    protected IExtractor extractor;
    protected IParser parser;

    protected AbstractSequenceModel(IParser withParser, IExtractor withExtractor) {
        this.parser = withParser;
        this.extractor = withExtractor;
    }

    public List<ModelResult> parse(String query) {
        List<ParseResult> parsedSequences = new ArrayList<ParseResult>();

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
                        }
                    });
        }).collect(Collectors.toList());
    }

    public String getModelTypeName() {
        return modelTypeName;
    }

    public IExtractor getExtractor() {
        return extractor;
    }

    public IParser getParser() {
        return parser;
    }
}
