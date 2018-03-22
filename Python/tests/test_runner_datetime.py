import importlib
import re
import pytest
from runner import get_specs, CULTURES
from recognizers_date_time.date_time import DateTimeRecognizer

MODELFUNCTION = {
    'DateTime': DateTimeRecognizer.recognize_datetime
}

@pytest.mark.parametrize('culture, model, options, context, source, expected_results', get_specs(recognizer='DateTime', entity='Extractor'))
def test_datetime_extractor(culture, model, options, context, source, expected_results):
    language = get_language(culture)
    extractor = create_extractor(language, model)

    result = extractor.extract(source, context)

    assert len(result) == len(expected_results)
    for actual, expected in zip(result, expected_results):
        assert actual.text == expected['Text']
        assert actual.type == expected['Type']

@pytest.mark.parametrize('culture, model, options, context, source, expected_results', get_specs(recognizer='DateTime', entity='Parser'))
def test_datetime_parser(culture, model, options, context, source, expected_results):
    reference_datetime = get_reference_date(context)
    language = get_language(culture)
    extractor = create_extractor(language, model)
    parser = create_parser(language, model)

    extract_results = extractor.extract(source, reference_datetime)
    result = [parser.parse(x, reference_datetime) for x in extract_results]

    assert len(result) == len(expected_results)
    for actual, expected in zip(result, expected_results):
        assert actual.text == expected['Text']
        assert actual.type == expected['Type']
        if actual.value and 'Value' in expected:
            assert actual.value.timex == expected['Value']['Timex']
            #TODO: assert FutureResolution and PastResolution

@pytest.mark.parametrize('culture, model, options, context, source, expected_results', get_specs(recognizer='DateTime', entity='Model'))
def test_datetime_model(culture, model, options, context, source, expected_results):
    reference_datetime = get_reference_date(context)
    option_obj = get_option(options)

    result = get_results(culture, model, source, option_obj, reference_datetime)

    assert_resolution = get_assert_model_resolution(options)
    assert len(result) == len(expected_results)
    for actual, expected in zip(result, expected_results):
        assert actual.text == expected['Text']
        assert actual.type == expected['Type']
        assert len(actual.resolution.values) == len(expected_results['Resolution']['Values'])
        for actual_resilution_value, expected_resoulution_value in zip(actual.resolution.values, expected_results['Resolution']['Values']):
            assert_resolution(actual_resilution_value, expected_resoulution_value)

def get_assert_model_resolution(option):
    assert_resolutions = {
        None: assert_model_resolution_option_none,
        'CalendarMode': assert_model_resolution_option_calendar_mode,
        'ExtendedTypes': assert_model_resolution_option_extended_types,
        'SplitDateAndTime': assert_model_resolution_option_split_date_and_time
    }
    return assert_resolutions.get(option)

def assert_model_resolution_option_none(actual, expected):
    assert actual.timex == expected['timex']
    assert actual.type == expected['type']
    assert actual.value == expected['value']

def assert_model_resolution_option_calendar_mode(actual, expected):
    assert actual.timex == expected['timex']
    assert actual.type == expected['type']
    assert actual.start == expected['start']
    assert actual.end == expected['end']

def assert_model_resolution_option_extended_types(actual, expected):
    assert actual.timex == expected['timex']
    assert actual.type == expected['type']
    assert actual.value == expected['value']

def assert_model_resolution_option_split_date_and_time(actual, expected):
    assert actual.timex == expected['timex']
    assert actual.type == expected['type']
    assert actual.value == expected['value']
    assert actual.mod == expected['Mod']

def create_extractor(language, model):
    extractor = get_class(f'recognizers_date_time.date_time.{language.lower()}.{model.lower()}',
                          f'{language}{model}Extractor')
    if extractor:
        return extractor()

    extractor = get_class(f'recognizers_date_time.date_time.base_{model.lower()}',
                          f'Base{model}Extractor')
    configuration = get_class(f'recognizers_date_time.date_time.{language.lower()}.{model.lower()}_configs',
                              f'{language}{model}ExtractorConfiguration')

    return extractor(configuration())

def create_parser(language, model):
    parser = get_class(f'recognizers_date_time.date_time.{language.lower()}.parsers', f'{language}{model}Parser')
    if not parser:
        parser = get_class(f'recognizers_date_time.date_time.base_{model.lower()}',
                           f'Base{model}Parser')

    configuration = get_class(f'recognizers_date_time.date_time.{language.lower()}.{model.lower()}_configs',
                              f'{language}{model}ParserConfiguration')

    return parser(configuration())

def get_class(module_name, class_name):
    try:
        module = importlib.import_module(module_name)
    except ImportError:
        return None
    return getattr(module, class_name) if hasattr(module, class_name) else None

def get_language(culture):
    return [x for x in CULTURES if CULTURES[x] == culture][0]

def get_reference_date(context):
    return context.get('ReferenceDateTime') if context else None

def get_results(culture, model, source, options, reference):
    return MODELFUNCTION[model](source, culture, options, reference)

def get_option(option):
    if not option:
        option = 'NONE'
    option = re.sub('\\B[A-Z]', r'_\g<0>', option).upper()
    module = importlib.import_module('recognizers_date_time.date_time.date_time_recognizer')
    option_class = getattr(module, 'DateTimeOptions')

    return option_class[option]
