import pytest
from runner import get_specs
from recognizers_number.number import NumberRecognizer

MODELFUNCTION = {
    'NumberModel': NumberRecognizer.recognize_number,
    'OrdinalModel': NumberRecognizer.recognize_ordinal,
    'PercentModel': NumberRecognizer.recognize_percentage,
}

def get_results(culture, model, source):
    return MODELFUNCTION[model](source, culture)

@pytest.mark.parametrize('culture, model, source, expected_results', get_specs('Number'))
def test_number_recognizer(culture, model, source, expected_results):
    results = get_results(culture, model, source)
    assert len(results) == len(expected_results)
    for expected, actual in zip(expected_results, results):
        assert expected['TypeName'] == actual.type_name
        assert expected['Text'] == actual.text
        assert expected['Resolution']['value'] == actual.resolution['value']
