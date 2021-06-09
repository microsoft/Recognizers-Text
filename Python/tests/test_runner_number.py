import pytest
from runner import get_specs
from recognizers_number.number.number_recognizer import recognize_number, recognize_ordinal, recognize_percentage

MODELFUNCTION = {
    'Number': recognize_number,
    'Ordinal': recognize_ordinal,
    'Percent': recognize_percentage,
}


@pytest.mark.parametrize('culture, model, options, context, source, expected_results', get_specs(
    recognizer='Number', entity='Model'))
def test_number_recognizer(culture, model, options,
                           context, source, expected_results):

    spec_info = model + "Model : " + source

    results = get_results(culture, model, source)

    assert len(results) == len(expected_results)

    for actual, expected in zip(results, expected_results):

        assert_verbose(actual.type_name, expected['TypeName'], spec_info)
        assert_verbose(actual.text, expected['Text'], spec_info)
        assert_verbose(actual.start, expected['Start'], spec_info)
        assert_verbose(actual.end, expected['End'], spec_info)
        assert_verbose(actual.resolution['value'], expected['Resolution']['value'], spec_info)


def get_results(culture, model, source):
    return MODELFUNCTION[model](source, culture)


def assert_verbose(actual, expected, spec_info):
    assert actual == expected, \
        "Actual: {} | Expected: {} | Context: {}".format(actual, expected, spec_info)
