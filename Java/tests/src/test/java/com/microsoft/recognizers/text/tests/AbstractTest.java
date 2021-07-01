package com.microsoft.recognizers.text.tests;

import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.databind.MapperFeature;
import com.fasterxml.jackson.databind.ObjectMapper;
import com.microsoft.recognizers.text.*;
import com.microsoft.recognizers.text.datetime.parsers.DateTimeParseResult;
import com.microsoft.recognizers.text.tests.helpers.DateTimeParseResultMixIn;
import com.microsoft.recognizers.text.tests.helpers.ExtractResultMixIn;
import com.microsoft.recognizers.text.tests.helpers.ModelResultMixIn;
import org.apache.commons.io.FileUtils;
import org.javatuples.Pair;
import org.junit.*;
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

    // FEFF - UTF-8 byte order mark (EF BB BF) as Unicode char representation.
    private static final String UTF8_BOM = "\uFEFF";

    protected final TestCase currentCase;

    public AbstractTest(TestCase currentCase) {
        this.currentCase = currentCase;
    }

    private static Map<String, Integer> testCounter;
    private static Map<String, Integer> passCounter;
    private static Map<String, Integer> failCounter;
    private static Map<String, Integer> skipCounter;

    @BeforeClass
    public static void before() {
        testCounter = new LinkedHashMap<>();
        passCounter = new LinkedHashMap<>();
        failCounter = new LinkedHashMap<>();
        skipCounter = new LinkedHashMap<>();
    }

    @AfterClass
    public static void after() {

        Map<String, String> counter = new LinkedHashMap<>();

        for (Map.Entry<String, Integer> entry : testCounter.entrySet()) {
            int skipped = skipCounter.getOrDefault(entry.getKey(), 0);
            if (entry.getValue() > skipped) {
                counter.put(entry.getKey(), String.format("%7d", entry.getValue()));
            }
        }

        for (Map.Entry<String, String> entry : counter.entrySet()) {
            Integer passValue = passCounter.getOrDefault(entry.getKey(), 0);
            Integer failValue = failCounter.getOrDefault(entry.getKey(), 0);
            Integer skipValue = skipCounter.getOrDefault(entry.getKey(), 0);
            counter.put(entry.getKey(), String.format("|%s  |%7d  |%7d  |%7d  ", entry.getValue(), passValue, skipValue, failValue));
        }

        print(counter);
    }

    private static void print(Map<String, String> map) {
        System.out.println("|  TOTAL  |  Passed | Skipped |  Failed || Key");
        for (Map.Entry<String, String> entry : map.entrySet()) {
            System.out.println(entry.getValue() + "|| " + entry.getKey());
        }
    }

    private void count(TestCase testCase) {
        String key = testCase.recognizerName + "-" + testCase.language + "-" + testCase.modelName;
        Integer current = testCounter.getOrDefault(key, 0);
        testCounter.put(key, current + 1);
    }

    private void countPass(TestCase testCase) {
        String key = testCase.recognizerName + "-" + testCase.language + "-" + testCase.modelName;
        Integer current = passCounter.getOrDefault(key, 0);
        passCounter.put(key, current + 1);
    }

    private void countSkip(TestCase testCase) {
        String key = testCase.recognizerName + "-" + testCase.language + "-" + testCase.modelName;
        Integer current = skipCounter.getOrDefault(key, 0);
        skipCounter.put(key, current + 1);
    }

    private void countFail(TestCase testCase) {
        String key = testCase.recognizerName + "-" + testCase.language + "-" + testCase.modelName;
        Integer current = failCounter.getOrDefault(key, 0);
        failCounter.put(key, current + 1);
    }

    @Test
    public void test() {

        count(currentCase);

        if (!isJavaSupported(this.currentCase.notSupported)) {
            countSkip(currentCase);
            throw new AssumptionViolatedException("Test case wih input '" + this.currentCase.input + "' not supported.");
        }

        if (this.currentCase.debug) {
            // Add breakpoint here to stop on those TestCases marked with "Debug": true
            System.out.println("Debug Break!");
        }

        try {
            recognizeAndAssert(currentCase);
            countPass(this.currentCase);
        } catch (AssumptionViolatedException ex) {
            countSkip(currentCase);
            throw ex;
        } catch (Throwable err) {
            countFail(currentCase);
            throw err;
        }
    }

    // TODO Override in specific models
    protected abstract List<ModelResult> recognize(TestCase currentCase);

    protected void recognizeAndAssert(TestCase currentCase) {
        List<ModelResult> results = recognize(currentCase);
        assertResults(currentCase, results, Collections.emptyList());
    }

    public void assertResults(TestCase currentCase, List<ModelResult> results, List<String> testResolutionKeys) {
        List<ModelResult> expectedResults = readExpectedResults(ModelResult.class, currentCase.results);
        Assert.assertEquals(getMessage(currentCase, "\"Result Count\""), expectedResults.size(), results.size());

        IntStream.range(0, expectedResults.size())
                .mapToObj(i -> Pair.with(expectedResults.get(i), results.get(i)))
                .forEach(t -> {
                    ModelResult expected = t.getValue0();
                    ModelResult actual = t.getValue1();
                    // Validate common properties of models
                    assertModel(expected, actual);
                    // Validate Resolution keys
                    assertResolutionKeys(expected, actual, currentCase, testResolutionKeys);
                });
    }

    protected void assertModel(ModelResult expected, ModelResult actual){
        Assert.assertEquals(getMessage(currentCase, "typeName"), expected.typeName, actual.typeName);
        Assert.assertEquals(getMessage(currentCase, "text"), expected.text, actual.text);
        if (expected.start != null) {
            Assert.assertEquals(getMessage(currentCase, "start"), expected.start, actual.start);
        }
        if (expected.end != null) {
            Assert.assertEquals(getMessage(currentCase, "end"), expected.end, actual.end);
        }
    }

    public static Collection<TestCase> enumerateTestCases(String recognizerType, String modelName) {
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
                .filter(ts -> ts.modelName.contains(modelName))
                .collect(Collectors.toCollection(ArrayList::new));
    }

    public static TestCase[] parseSpecFile(File f, ObjectMapper mapper) {

        List<String> paths = Arrays.asList(f.toPath().toString().split(Pattern.quote(File.separator)));
        List<String> testInfo = paths.subList(paths.size() - 3, paths.size());

        try {

            // Workaround to consume a possible UTF-8 BOM byte
            // https://stackoverflow.com/questions/4897876/reading-utf-8-bom-marker
            String contents = new String(Files.readAllBytes(f.toPath()));
            String json = StringUtf8Bom(contents);

            TestCase[] tests = mapper.readValue(json, TestCase[].class);
            Arrays.stream(tests).forEach(t -> {
                t.recognizerName = testInfo.get(0);
                t.language = testInfo.get(1);
                t.modelName = testInfo.get(2).split(Pattern.quote("."))[0];
            });

            return tests;

        } catch (IOException ex) {

            System.out.println("Error reading Spec file: " + f.toString() + " | " + ex.getMessage());

            // @TODO: This should cause a test run failure.
            return new TestCase[0];
        }
    }

    public static <T extends ExtractResult> T parseExtractResult(Class<T> extractorResultClass, Object result) {
        // Deserializer
        ObjectMapper mapper = new ObjectMapper();
        mapper.configure(MapperFeature.ACCEPT_CASE_INSENSITIVE_PROPERTIES, true);
        mapper.addMixIn(ExtractResult.class, ExtractResultMixIn.class);

        try {
            String json = mapper.writeValueAsString(result);
            return mapper.readValue(json, extractorResultClass);

        } catch (JsonProcessingException e) {
            e.printStackTrace();
            return null;

        } catch (IOException e) {
            e.printStackTrace();
            return null;
        }
    }

    public static <T extends DateTimeParseResult> T parseDateTimeParseResult(Class<T> dateTimeParseResultClass, Object result) {
        // Deserializer
        ObjectMapper mapper = new ObjectMapper();
        mapper.configure(MapperFeature.ACCEPT_CASE_INSENSITIVE_PROPERTIES, true);
        mapper.addMixIn(DateTimeParseResult.class, DateTimeParseResultMixIn.class);

        try {
            String json = mapper.writeValueAsString(result);
            return mapper.readValue(json, dateTimeParseResultClass);

        } catch (JsonProcessingException e) {
            e.printStackTrace();
            return null;

        } catch (IOException e) {
            e.printStackTrace();
            return null;
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

    public static <T extends ExtractResult> List<T> readExpectedExtractResults(Class<T> extractorResultClass, List<Object> results) {
        return results.stream().map(r -> parseExtractResult(extractorResultClass, r))
                .collect(Collectors.toCollection(ArrayList::new));
    }

    public static <T extends DateTimeParseResult> List<T> readExpectedDateTimeParseResults(Class<T> dateTimeParseResultClass, List<Object> results) {
        return results.stream().map(r -> parseDateTimeParseResult(dateTimeParseResultClass, r))
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

    protected void assertResolutionKeys(ModelResult expected, ModelResult actual, TestCase currentCase, List<String> testResolutionKeys) {
        for (String key : testResolutionKeys) {
            Assert.assertEquals(getMessage(currentCase, key), String.valueOf(expected.resolution.get(key)), String.valueOf(actual.resolution.get(key)));
        }
    }

    private static String StringUtf8Bom(String input) {

        if (input.startsWith(UTF8_BOM)) {
            input = input.substring(1);
        }

        return input;
    }

}
