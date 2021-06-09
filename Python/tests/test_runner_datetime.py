import importlib
import datetime
import pytest
from runner import get_specs, CULTURES
from recognizers_date_time import recognize_datetime

MODELFUNCTION = {
    'DateTime': recognize_datetime
}


@pytest.mark.parametrize(
    'culture, model, options, context, source, expected_results',
    get_specs(recognizer='DateTime', entity='Extractor'))
def test_datetime_extractor(
        culture,
        model,
        options,
        context,
        source,
        expected_results):

    reference_datetime = get_reference_date(context)
    language = get_language(culture)
    extractor = create_extractor(language, model, options)

    result = extractor.extract(source, reference_datetime)

    spec_info = type(extractor).__name__ + " : " + source

    assert len(result) == len(expected_results)
    for actual, expected in zip(result, expected_results):
        simple_extractor_assert(spec_info, actual, expected, 'text', 'Text', True)
        simple_extractor_assert(spec_info, actual, expected, 'type', 'Type')
        simple_extractor_assert(spec_info, actual, expected, 'start', 'Start')
        simple_extractor_assert(spec_info, actual, expected, 'length', 'Length')


@pytest.mark.parametrize(
    'culture, model, options, context, source, expected_results',
    get_specs(recognizer='DateTime', entity='Parser'))
def test_datetime_parser(
        culture,
        model,
        options,
        context,
        source,
        expected_results):

    reference_datetime = get_reference_date(context)
    language = get_language(culture)
    extractor = create_extractor(language, model, options)
    parser = create_parser(language, model, options)

    spec_info = type(parser).__name__ + " : " + source

    extract_results = extractor.extract(source, reference_datetime)
    result = [parser.parse(x, reference_datetime) for x in extract_results]
    assert len(result) == len(expected_results)

    for actual, expected in zip(result, expected_results):

        simple_parser_assert(spec_info, actual, expected, 'text', 'Text', True)
        simple_parser_assert(spec_info, actual, expected, 'type', 'Type')

        if 'Value' in expected:
            assert_verbose(actual.value is None, False, spec_info)

        if actual.value and 'Value' in expected:
            simple_parser_assert(spec_info, actual.value, expected['Value'], 'timex', 'Timex')
            simple_parser_assert(spec_info, actual.value, expected['Value'], 'mod', 'Mod')
            simple_parser_assert(spec_info, actual.value, expected['Value'], 'future_resolution', 'FutureResolution')
            simple_parser_assert(spec_info, actual.value, expected['Value'], 'past_resolution', 'PastResolution')


@pytest.mark.parametrize(
    'culture, model, options, context, source, expected_results',
    get_specs(recognizer='DateTime', entity='MergedParser'))
def test_datetime_mergedparser(
        culture,
        model,
        options,
        context,
        source,
        expected_results):

    reference_datetime = get_reference_date(context)
    language = get_language(culture)
    extractor = create_extractor(language, model, options)
    parser = create_parser(language, model, options)

    extract_results = extractor.extract(source, reference_datetime)
    result = [parser.parse(x, reference_datetime) for x in extract_results]

    spec_info = type(parser).__name__ + " : " + source

    assert len(result) == len(expected_results)

    for actual, expected in zip(result, expected_results):

        simple_extractor_assert(spec_info, actual, expected, 'text', 'Text')
        simple_extractor_assert(spec_info, actual, expected, 'type', 'Type')
        simple_extractor_assert(spec_info, actual, expected, 'start', 'Start')
        simple_extractor_assert(spec_info, actual, expected, 'length', 'Length')

        if 'Value' in expected:
            assert_verbose(actual.value is None, False, spec_info)

        if actual.value and 'Value' in expected:
            if 'values' in expected['Value']:
                assert isinstance(actual.value['values'], list)

                assert len(expected['Value']) == len(actual.value)

                for actual_values, expected_values in zip(actual.value['values'], expected['Value']['values']):
                    for key in expected_values.keys():
                        assert actual_values[key] == expected_values[key]


@pytest.mark.parametrize(
    'culture, model, options, context, source, expected_results',
    get_specs(recognizer='DateTime', entity='Model'))
def test_datetime_model(
        culture,
        model,
        options,
        context,
        source,
        expected_results):

    reference_datetime = get_reference_date(context)
    option_obj = get_option(options)

    result = get_results(
        culture,
        model,
        source,
        option_obj,
        reference_datetime)

    spec_info = model + "Model : " + source

    assert_verbose(len(result), len(expected_results), spec_info)

    for actual, expected in zip(result, expected_results):

        simple_parser_assert(spec_info, actual, expected, 'text', 'Text')
        simple_parser_assert(spec_info, actual, expected, 'type_name', 'TypeName')
        simple_parser_assert(spec_info, actual, expected, 'parent_text', 'ParentText')
        simple_parser_assert(spec_info, actual, expected, 'start', 'Start')
        simple_parser_assert(spec_info, actual, expected, 'end', 'End')

        # Avoid TypError if Actual is None
        assert_verbose(actual is None, False, spec_info)
        assert_verbose(actual.resolution is None, False, spec_info)
        assert_verbose(len(actual.resolution['values']), len(expected['Resolution']['values']), spec_info)

        for actual_resolution_value in actual.resolution['values']:
            assert_model_resolution(actual_resolution_value, expected['Resolution']['values'], spec_info)


def get_props(results, prop):
    list_result = []
    for result in results:
        list_result.append(result.get(prop))

    return list_result


def single_assert(actual, expected, prop, spec_info):
    if expected.get(prop):
        assert actual[prop] == expected[prop], \
            "Actual: {} | Expected: {} | Context: {}".format(actual, expected, spec_info)
    else:
        assert actual.get(prop) is None, \
            "Actual: 'None' | Expected: {} | Context: {}".format(actual, expected, spec_info)


def assert_verbose(actual, expected, spec_info):
    assert actual == expected, \
        "Actual: {} | Expected: {} | Context: {}".format(actual, expected, spec_info)


def assert_prop(actual, expected, prop, spec_info):
    actual_val = actual.get(prop)
    expected_val = get_props(expected, prop)
    assert actual_val in expected_val, \
        "Actual: {} | Expected: {} | Prop: {} | Context: {}".format(actual, expected, prop, spec_info)


def assert_model_resolution(actual, expected, spec_info):
    assert_prop(actual, expected, 'timex', spec_info)
    assert_prop(actual, expected, 'type', spec_info)
    assert_prop(actual, expected, 'value', spec_info)
    assert_prop(actual, expected, 'start', spec_info)
    assert_prop(actual, expected, 'end', spec_info)
    assert_prop(actual, expected, 'Mod', spec_info)


def simple_extractor_assert(spec_info, actual, expected, prop, resolution, ignore_result_case=False):
    if resolution in expected:
        expected_normalize = expected[resolution] if not ignore_result_case else expected[resolution].lower()
        actual_normalize = getattr(actual, prop) if not ignore_result_case else getattr(actual, prop).lower()
        assert_verbose(actual_normalize, expected_normalize, spec_info)


def simple_parser_assert(spec_info, actual, expected, prop, resolution, ignore_result_case=False):
    if resolution in expected:
        expected_normalize = expected[resolution] if not ignore_result_case else expected[resolution].lower()
        actual_normalize = getattr(actual, prop) if not ignore_result_case else getattr(actual, prop).lower()
        assert_verbose(actual_normalize, expected_normalize, spec_info)


def create_extractor(language, model, options):
    extractor = get_class(
        'recognizers_date_time',
        f'{language}{model}Extractor')

    if extractor:
        return extractor()

    extractor = get_class(
        f'recognizers_date_time.date_time.{language.lower()}.{model.lower()}',
        f'{language}{model}Extractor')
    if extractor:
        return extractor()

    extractor = get_class(
        f'recognizers_date_time.date_time.base_{model.lower()}',
        f'Base{model}Extractor')
    configuration = get_class(
        f'recognizers_date_time.date_time.{language.lower()}.{model.lower()}_extractor_config',
        f'{language}{model}ExtractorConfiguration')

    if model == 'Merged':
        option = get_option(options)
        return extractor(configuration(), option)

    return extractor(configuration())


def create_parser(language, model, options):
    parser = get_class(
        f'recognizers_date_time.date_time.{language.lower()}.{model.lower()}_parser',
        f'{language}{model}Parser')

    if parser:
        return parser()

    parser = get_class(
        f'recognizers_date_time.date_time.{language.lower()}.parsers',
        f'{language}{model}Parser')

    if not parser:
        parser = get_class(
            f'recognizers_date_time.date_time.base_{model.lower()}',
            f'Base{model}Parser')
        if model == 'TimeZone':
            return parser()

    configuration_class = get_class(
        f'recognizers_date_time.date_time.{language.lower()}.{model.lower()}_parser_config',
        f'{language}{model}ParserConfiguration')

    language_configuration = get_class(
        f'recognizers_date_time.date_time.{language.lower()}.common_configs',
        f'{language}CommonDateTimeParserConfiguration')

    configuration = configuration_class(
        language_configuration()) if language_configuration else configuration_class()

    if model == 'Merged':
        option = get_option(options)
        return parser(configuration, option)

    return parser(configuration)


def get_class(module_name, class_name):
    try:
        module = importlib.import_module(module_name)
    except ImportError:
        return None
    return getattr(module, class_name) if hasattr(module, class_name) else None


def get_language(culture):
    return [x for x in CULTURES if CULTURES[x] == culture][0]


def get_reference_date(context):
    reference_datetime = context.get('ReferenceDateTime') if context else None
    return datetime.datetime.strptime(reference_datetime[0:19],
                                      '%Y-%m-%dT%H:%M:%S') if reference_datetime and not isinstance(reference_datetime,
                                                                                                    datetime.datetime) else None


def get_results(culture, model, source, options, reference):
    return MODELFUNCTION[model](source, culture, options, reference)


def get_option(option):

    if not option:
        option = 'None'

    module = importlib.import_module('recognizers_date_time.date_time.utilities')

    option_class = getattr(module, 'DateTimeOptions')

    if option in ['CalendarMode']:
        return option_class['CALENDAR']
    elif option in ['SkipFromTo']:
        return option_class['SKIP_FROM_TO_MERGE']
    elif option in ['SplitDateAndTime']:
        return option_class['SPLIT_DATE_AND_TIME']

    return option_class['NONE']
