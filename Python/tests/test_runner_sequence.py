import pytest
from runner import get_specs
from recognizers_sequence.sequence.sequence_recognizer import recognize_phone_number, recognize_email

MODEL_FUNCTION = {
    'PhoneNumber': recognize_phone_number,
    'Email':recognize_email,
}


@pytest.mark.parametrize('culture, model, options, context, source, expected_results',
                         get_specs(recognizer='Sequence', entity='Model'))
def test_sequence_recognizer(culture, model, options, context, source, expected_results):
    results = get_results(culture, model, source)
    assert len(results) == len(expected_results)
    for actual, expected in zip(results, expected_results):
        assert actual.type_name == expected['TypeName']
        assert actual.text == expected['Text']
        assert actual.resolution['value'] == expected['Resolution']['value']    
        resolution_assert(actual, expected, ['value', 'score'])

def get_results(culture, model, source):
    return MODEL_FUNCTION[model](source, culture)

def resolution_assert(actual, expected, props):
    for prop in props:
        if prop in expected['Resolution']:
            assert actual.resolution[prop] == expected['Resolution'][prop]
 
