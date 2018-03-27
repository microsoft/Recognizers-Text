package com.microsoft.recognizers.text;

import org.javatuples.Pair;
import org.junit.Assert;
import org.junit.Test;

public class ModelFactoryTest {
    @Test
    public void NoExistingModelConfigurationThrows() {
        try {
            ModelFactory<TestOptionsEnum> factory = new ModelFactory<>();
            factory.getModel(TestModel.class, "xx-xx", false, TestOptionsEnum.None);
        } catch(IllegalArgumentException e) {
            Assert.assertEquals("Could not find Model with the specified configuration: xx-xx, com.microsoft.recognizers.text.TestModel", e.getMessage());
        }
    }

    @Test
    public void ProperModelConfigurationReturnsModel() {
        ModelFactory<TestOptionsEnum> factory = new ModelFactory<>();
        factory.put(new Pair<>("xx-xx", TestModel.class), testOptionsEnum -> new TestModel());

        IModel model = factory.getModel(TestModel.class, "xx-xx", false, TestOptionsEnum.None);
        Assert.assertNotNull(model);
    }

    @Test
    public void FactoryReturnsCachedModel() {
        ModelFactory<TestOptionsEnum> factory = new ModelFactory<>();
        factory.put(new Pair<>("xx-xx", TestModel.class), testOptionsEnum -> new TestModel());

        IModel first = factory.getModel(TestModel.class, "xx-xx", false, TestOptionsEnum.None);
        IModel second = factory.getModel(TestModel.class, "xx-xx", false, TestOptionsEnum.None);

        Assert.assertEquals(first, second);
    }

    @Test
    public void DifferentOptionsShouldReturnDifferentModelInstances() {
        ModelFactory<TestOptionsEnum> factory = new ModelFactory<>();
        factory.put(new Pair<>("xx-xx", TestModel.class), testOptionsEnum -> new TestModel());

        IModel first = factory.getModel(TestModel.class, "xx-xx", false, TestOptionsEnum.First);
        IModel second = factory.getModel(TestModel.class, "xx-xx", false, TestOptionsEnum.Second);

        Assert.assertNotEquals(first, second);
    }
}
