import pytest
from runner import get_specs
from recognizers_number_with_unit.number_with_unit import NumberWithUnitRecognizer

MODELFUNCTION = {
    'AgeModel': NumberWithUnitRecognizer.recognize_age,
    'CurrencyModel': NumberWithUnitRecognizer.recognize_currency,
    'TemperatureModel': NumberWithUnitRecognizer.recognize_temperature,
    'DimensionModel': NumberWithUnitRecognizer.recognize_dimension,
}

def get_results(culture, model, source):
    return MODELFUNCTION[model](source, culture)

@pytest.mark.parametrize('culture, model, source, expected_results', get_specs('NumberWithUnit'))
def test_number_with_unit_recognizer(culture, model, source, expected_results):
    results = get_results(culture, model, source)
    assert len(results) == len(expected_results)
    for expected, actual in zip(expected_results, results):
        assert expected['Text'] == actual.text
        assert expected['TypeName'] == actual.type_name
        if expected['Resolution'] is None:
            assert actual.resolution is None
        else:
            expected_resolution = expected['Resolution']
            assert expected_resolution['value'] == actual.resolution.value
            assert expected_resolution['unit'] == actual.resolution.unit
