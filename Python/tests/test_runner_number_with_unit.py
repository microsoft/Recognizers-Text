import pytest
from runner import get_specs
from recognizers_number_with_unit.number_with_unit import NumberWithUnitRecognizer

MODELFUNCTION = {
    'Age': NumberWithUnitRecognizer.recognize_age,
    'Currency': NumberWithUnitRecognizer.recognize_currency,
    'Temperature': NumberWithUnitRecognizer.recognize_temperature,
    'Dimension': NumberWithUnitRecognizer.recognize_dimension,
}

@pytest.mark.parametrize('culture, model, options, context, source, expected_results', get_specs(recognizer='NumberWithUnit', entity='Model'))
def test_number_with_unit_recognizer(culture, model, options, context, source, expected_results):
    results = get_results(culture, model, source)
    assert len(results) == len(expected_results)
    for actual, expected in zip(results, expected_results):
        assert actual.text == expected['Text']
        assert actual.type_name == expected['TypeName']
        if expected['Resolution'] is None:
            assert actual.resolution is None
        else:
            expected_resolution = expected['Resolution']
            assert actual.resolution.value == expected_resolution['value']
            assert actual.resolution.unit == expected_resolution['unit']

def get_results(culture, model, source):
    return MODELFUNCTION[model](source, culture)
