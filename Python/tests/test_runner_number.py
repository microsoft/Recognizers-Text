import pytest
from runner import get_specs
from recognizers_number.number import NumberRecognizer

MODELFUNCTION = {
    'Number': NumberRecognizer.recognize_number,
    'Ordinal': NumberRecognizer.recognize_ordinal,
    'Percent': NumberRecognizer.recognize_percentage,
}

@pytest.mark.parametrize('culture, model, options, context, source, expected_results', get_specs(recognizer='Number', entity='Model'))
def test_number_recognizer(culture, model, options, context, source, expected_results):
    results = get_results(culture, model, source)
    assert len(results) == len(expected_results)
    for actual, expected in zip(results, expected_results):
        assert actual.type_name == expected['TypeName']
        assert actual.text == expected['Text']
        assert actual.resolution['value'] == expected['Resolution']['value']

def get_results(culture, model, source):
    return MODELFUNCTION[model](source, culture)
