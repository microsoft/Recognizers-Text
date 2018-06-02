package com.microsoft.recognizers.text.tests;

import com.fasterxml.jackson.databind.MapperFeature;
import com.fasterxml.jackson.databind.ObjectMapper;
import com.microsoft.recognizers.text.tests.TestCase;
import org.apache.commons.io.FileUtils;
import org.junit.Test;
import org.junit.runner.RunWith;
import org.junit.runners.Parameterized;

import java.io.File;
import java.io.IOException;
import java.nio.file.Files;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.Collection;
import java.util.List;
import java.util.regex.Pattern;
import java.util.stream.Collectors;

@RunWith(Parameterized.class)
public abstract class AbstractTest {

    private static final String SpecsPath = "../../Specs";

    protected final TestCase currentCase;

    public AbstractTest(TestCase currentCase) {
        this.currentCase = currentCase;
    }

    @Test
    public void Recognize() {
        if(this.currentCase.debug) {
            // Add breakpoint here to stop on those TestCases marked with "debug": true
        }
    }

    public static Collection<TestCase> testCases(String recognizerType) {

        String recognizerTypePath = String.format(File.separator + recognizerType + File.separator);

        // Deserializer
        ObjectMapper mapper = new ObjectMapper();
        mapper.configure(MapperFeature.ACCEPT_CASE_INSENSITIVE_PROPERTIES, true);

        // Map json to TestCases
        return FileUtils.listFiles(new File(SpecsPath), new String[]{"json"}, true)
                .stream().filter(f -> f.getPath().contains(recognizerTypePath))
                .map(f -> ReadTestSpec(f, mapper))
                .flatMap(ts -> Arrays.stream(ts))
                .collect(Collectors.toCollection(ArrayList::new));
    }

    private static TestCase[] ReadTestSpec(File f, ObjectMapper mapper) {
        List<String> paths = Arrays.asList(f.toPath().toString().split(Pattern.quote(File.separator)));
        List<String> testInfo = paths.subList(paths.size() - 3, paths.size());

        try {
            String json = new String(Files.readAllBytes(f.toPath()));
            TestCase[] tests = mapper.readValue(json, TestCase[].class);
            Arrays.stream(tests).forEach(t -> {
                t.recognizerName = testInfo.get(0);
                t.language = testInfo.get(1);
                t.modelName = testInfo.get(2).split(Pattern.quote("."))[0];
            });
            return tests;
        } catch (IOException ex) {
            System.out.println("Error reading Spec file: " + f.toString() + " | " + ex.getMessage());
            return new TestCase[0];
        }
    }
}
