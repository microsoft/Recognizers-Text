package com.microsoft.recognizers.text.tests;

import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.databind.MapperFeature;
import com.fasterxml.jackson.databind.ObjectMapper;
import com.microsoft.recognizers.text.ModelResult;
import com.microsoft.recognizers.text.tests.helpers.ModelResultMixIn;
import org.apache.commons.io.FileUtils;
import org.junit.AssumptionViolatedException;
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

    private static final List<String> SupportedCultures = Arrays.asList("English", "Spanish", "Portuguese", "French", "German");

    protected final TestCase currentCase;

    public AbstractTest(TestCase currentCase) {
        this.currentCase = currentCase;
    }

    @Test
    public void testPreValidate() {
        if (!isJavaSupported(this.currentCase.notSupported)) {
            throw new AssumptionViolatedException("Test case wih input '" + this.currentCase.input + "' not supported.");
        }

        if (this.currentCase.debug) {
            // Add breakpoint here to stop on those TestCases marked with "Debug": true
        }

        test();
    }

    abstract void test();

    public static Collection<TestCase> enumerateTestCases(String recognizerType) {

        String recognizerTypePath = String.format(File.separator + recognizerType + File.separator);

        // Deserializer
        ObjectMapper mapper = new ObjectMapper();
        mapper.configure(MapperFeature.ACCEPT_CASE_INSENSITIVE_PROPERTIES, true);

        // Map json to TestCases
        return FileUtils.listFiles(new File(SpecsPath), new String[]{"json"}, true)
                .stream().filter(f -> f.getPath().contains(recognizerTypePath))
                .map(f -> parseSpecFile(f, mapper))
                .flatMap(ts -> Arrays.stream(ts))
                // Ignore tests with NotSupportedByDesign = Java
                .filter(ts -> isJavaSupported(ts.notSupportedByDesign))
                // Filter supported languages only
                .filter(ts -> SupportedCultures.contains(ts.language))
                .collect(Collectors.toCollection(ArrayList::new));
    }

    private static TestCase[] parseSpecFile(File f, ObjectMapper mapper) {
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

    protected <T extends ModelResult> List<T> readExpectedResults(Class<T> modelResultClass, List<Object> results) {
        return results.stream().map(r -> this.parseResult(modelResultClass, r))
                .collect(Collectors.toCollection(ArrayList::new));
    }

    private <T extends ModelResult> T parseResult(Class<T> modelResultClass, Object result) {
        // Deserializer
        ObjectMapper mapper = new ObjectMapper();
        mapper.configure(MapperFeature.ACCEPT_CASE_INSENSITIVE_PROPERTIES, true);
        mapper.addMixIn(ModelResult.class, ModelResultMixIn.class);

        try {
            String json = mapper.writeValueAsString(result);
            return mapper.readValue(json, modelResultClass);

        } catch (JsonProcessingException e) {
            e.printStackTrace();
            return null;

        } catch (IOException e) {
            e.printStackTrace();
            return null;

        }
    }

    private static boolean isJavaSupported(String notSupported) {
        // definition for "not supported" missing, should be supported then
        if (notSupported == null) return true;

        return !Arrays.asList(notSupported.toLowerCase().split(Pattern.quote(","))).contains("java");
    }
}
