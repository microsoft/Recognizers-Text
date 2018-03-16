import pytest
from runner import *
from recognizers_number_with_unit.number_with_unit import NumberWithUnitRecognizer

modelFunction = {
    'AgeModel': NumberWithUnitRecognizer.recognize_age,
    'CurrencyModel': NumberWithUnitRecognizer.recognize_currency,
    'TemperatureModel': NumberWithUnitRecognizer.recognize_temperature,
    'DimensionModel': NumberWithUnitRecognizer.recognize_dimension,
}

def get_results(culture, model, source):
    return modelFunction[model](source, culture)

@pytest.mark.parametrize("culture, model, source, expected_results", get_specs("NumberWithUnit"))
def test_number_with_unit_recognizer(culture, model, source, expected_results):
    results = get_results(culture, model, source)
    assert len(results) == len(expected_results)
    for expected, actual in zip(expected_results, results):
        assert expected["Text"] == actual.text
        assert expected["TypeName"] == actual.type_name
        assert expected["Resolution"] is None == actual.resolution is None
        if actual.resolution:
            assert expected["Resolution"]["value"] == actual.resolution["value"]
            assert expected["Resolution"]["unit"] == actual.resolution["unit"]