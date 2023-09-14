#  Copyright (c) Microsoft Corporation. All rights reserved.
#  Licensed under the MIT License.

import pytest

from recognizers_number.culture import SUPPORTED_CULTURES
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
        assert_verbose(actual.resolution['value'], expected['Resolution']['value'], spec_info)

        if 'Start' in expected and 'End' in expected:
            assert_verbose(actual.start, expected['Start'], spec_info)
            assert_verbose(actual.end, expected['End'], spec_info)


def get_results(culture, model, source):
    return MODELFUNCTION[model](source, culture)


def assert_verbose(actual, expected, spec_info):
    assert actual == expected, \
        "Actual: {} | Expected: {} | Context: {}".format(actual, expected, spec_info)


def test_test():
    two_thousand = 0
    one = 0

    for culture in SUPPORTED_CULTURES:
        recognize_number("123", culture)

    for x in range(0, 3000):
        res = recognize_number("twee\u00adduizend", "nl-nl")
        temp = res[0].resolution.get('value')
        if temp == '2000':
            two_thousand += 1
        elif temp == '1000' or temp == '2':
            one += 1

    print("Number of times it returned 2000 - " + str(two_thousand))
    print("Number of times it returned 1000 - " + str(one))


def test_test():
    res = recognize_number("4 thousand 3 hundred and 0 are two valid numbers", "en-us")
    # res2 = recognize_number("3 hundred and negative one are two valid numbers.", "en-us")

    print(res)
    # print(res2)
