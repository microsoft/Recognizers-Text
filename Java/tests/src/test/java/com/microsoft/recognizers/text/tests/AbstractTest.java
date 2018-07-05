package com.microsoft.recognizers.text.tests;

import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.databind.MapperFeature;
import com.fasterxml.jackson.databind.ObjectMapper;
import com.microsoft.recognizers.text.Culture;
import com.microsoft.recognizers.text.ModelResult;
import com.microsoft.recognizers.text.ResolutionKey;
import com.microsoft.recognizers.text.tests.helpers.ModelResultMixIn;
import org.apache.commons.io.FileUtils;
import org.javatuples.Pair;
import org.junit.Assert;
import org.junit.AssumptionViolatedException;
import org.junit.Test;
import org.junit.runner.RunWith;
import org.junit.runners.Parameterized;

import java.io.File;
import java.io.IOException;
import java.nio.file.Files;
import java.util.*;
import java.util.regex.Pattern;
import java.util.stream.Collectors;
import java.util.stream.IntStream;

@RunWith(Parameterized.class)
public abstract class AbstractTest {

    private static final String SpecsPath = "../../Specs";

    private static final List<String> SupportedCultures = Arrays.asList("English", "Spanish", "Portuguese", "French", "German", "Chinese");

    protected final TestCase currentCase;

    public AbstractTest(TestCase currentCase) {
        this.currentCase = currentCase;
    }

    @Test
    public void test() {
        if (!isJavaSupported(this.currentCase.notSupported)) {
            throw new AssumptionViolatedException("Test case wih input '" + this.currentCase.input + "' not supported.");
        }

        if (this.currentCase.debug) {
            // Add breakpoint here to stop on those TestCases marked with "Debug": true
            System.out.println("Debug Brk!");
        }

        recognizeAndAssert(currentCase);
    }

    // TODO Override in specific models
    protected abstract List<ModelResult> recognize(TestCase currentCase);

    protected void recognizeAndAssert(TestCase currentCase) {
        List<ModelResult> results = recognize(currentCase);
        assertResults(currentCase, results);
    }

    public static void assertResults(TestCase currentCase, List<ModelResult> results) {
        assertResultsWithKeys(currentCase, results, Collections.emptyList());
    }

    public static void assertResultsWithKeys(TestCase currentCase, List<ModelResult> results, List<String> testResolutionKeys) {
        List<ModelResult> expectedResults = readExpectedResults(ModelResult.class, currentCase.results);
        Assert.assertEquals(getMessage(currentCase, "\"Result Count\""), expectedResults.size(), results.size());

        IntStream.range(0, expectedResults.size())
                .mapToObj(i -> Pair.with(expectedResults.get(i), results.get(i)))
                .forEach(t -> {
                    ModelResult expected = t.getValue0();
                    ModelResult actual = t.getValue1();

                    Assert.assertEquals(getMessage(currentCase, "typeName"), expected.typeName, actual.typeName);
                    Assert.assertEquals(getMessage(currentCase, "text"), expected.text, actual.text);

                    Assert.assertEquals(getMessage(currentCase, "resolution.value"), expected.resolution.get(ResolutionKey.Value), actual.resolution.get(ResolutionKey.Value));

                    for (String key : testResolutionKeys) {
                        Assert.assertEquals(getMessage(currentCase, key), expected.resolution.get(key), actual.resolution.get(key));
                    }
                });
    }

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

    public static TestCase[] parseSpecFile(File f, ObjectMapper mapper) {
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

    public static <T extends ModelResult> T parseResult(Class<T> modelResultClass, Object result) {
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

    public static <T extends ModelResult> List<T> readExpectedResults(Class<T> modelResultClass, List<Object> results) {
        return results.stream().map(r -> parseResult(modelResultClass, r))
                .collect(Collectors.toCollection(ArrayList::new));
    }

    public static String getCultureCode(String language) {
        return Arrays.stream(Culture.SupportedCultures)
                .filter(c -> c.cultureName.equalsIgnoreCase(language))
                .findFirst().get().cultureCode;
    }

    public static boolean isJavaSupported(String notSupported) {
        // definition for "not supported" missing, should be supported then
        if (notSupported == null) return true;

        return !Arrays.asList(notSupported.toLowerCase().trim().split("\\s*,\\s*")).contains("java");
    }

    public static String getMessage(TestCase testCase, String propName) {
        return "Does not match " + propName + " on Input: \"" + testCase.input + "\"";
    }

}
