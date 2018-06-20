import pytest
from runner import get_specs
from recognizers_number_with_unit.number_with_unit.number_with_unit_recognizer import recognize_age, recognize_currency, recognize_dimension, recognize_temperature

MODELFUNCTION = {
    'Age': recognize_age,
    'Currency': recognize_currency,
    'Temperature': recognize_temperature,
    'Dimension': recognize_dimension,
}

@pytest.mark.parametrize('culture, model, options, context, source, expected_results', get_specs(recognizer='NumberWithUnit', entity='Model'))
def test_number_with_unit_recognizer(culture, model, options, context, source, expected_results):
    results = get_results(culture, model, source)
    assert len(results) == len(expected_results)
    for actual, expected in zip(results, expected_results):
        assert actual.text == expected['Text']
        assert actual.type_name == expected['TypeName']
        resoultion_assert(actual, expected, ['value', 'unit', 'isoCurrency'])

def get_results(culture, model, source):
    return MODELFUNCTION[model](source, culture)

def resoultion_assert(actual, expected, props):
    if expected['Resolution'] is None:
        assert actual.resolution is None
    else:
        for prop in props:
            if prop in expected['Resolution']:
                assert actual.resolution[prop] == expected['Resolution'][prop]
