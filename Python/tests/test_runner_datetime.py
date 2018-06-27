import importlib
import re
import datetime
import pytest
from runner import get_specs, CULTURES
from recognizers_date_time import recognize_datetime

MODELFUNCTION = {
    'DateTime': recognize_datetime
}

@pytest.mark.parametrize('culture, model, options, context, source, expected_results', get_specs(recognizer='DateTime', entity='Extractor'))
def test_datetime_extractor(culture, model, options, context, source, expected_results):
    reference_datetime = get_reference_date(context)
    language = get_language(culture)
    extractor = create_extractor(language, model, options)

    result = extractor.extract(source, reference_datetime)

    assert len(result) == len(expected_results)
    for actual, expected in zip(result, expected_results):
        simple_extractor_assert(actual, expected, 'text', 'Text')
        simple_extractor_assert(actual, expected, 'type', 'Type')
        simple_extractor_assert(actual, expected, 'start', 'Start')
        simple_extractor_assert(actual, expected, 'length', 'Length')

@pytest.mark.parametrize('culture, model, options, context, source, expected_results', get_specs(recognizer='DateTime', entity='Parser'))
def test_datetime_parser(culture, model, options, context, source, expected_results):
    reference_datetime = get_reference_date(context)
    language = get_language(culture)
    extractor = create_extractor(language, model, options)
    parser = create_parser(language, model, options)

    extract_results = extractor.extract(source, reference_datetime)
    result = [parser.parse(x, reference_datetime) for x in extract_results]
    assert len(result) == len(expected_results)
    for actual, expected in zip(result, expected_results):
        simple_parser_assert(actual, expected, 'text', 'Text')
        simple_parser_assert(actual, expected, 'type', 'Type')
        if 'Value' in expected:
            assert actual.value
        if actual.value and 'Value' in expected:
            simple_parser_assert(actual.value, expected['Value'], 'timex', 'Timex')
            simple_parser_assert(actual.value, expected['Value'], 'mod', 'Mod')
            simple_parser_assert(actual.value, expected['Value'], 'future_resolution', 'FutureResolution')
            simple_parser_assert(actual.value, expected['Value'], 'past_resolution', 'PastResolution')

@pytest.mark.parametrize('culture, model, options, context, source, expected_results', get_specs(recognizer='DateTime', entity='MergedParser'))
def test_datetime_mergedparser(culture, model, options, context, source, expected_results):
    reference_datetime = get_reference_date(context)
    language = get_language(culture)
    extractor = create_extractor(language, model, options)
    parser = create_parser(language, model, options)

    extract_results = extractor.extract(source, reference_datetime)
    result = [parser.parse(x, reference_datetime) for x in extract_results]
    assert len(result) == len(expected_results)
    for actual, expected in zip(result, expected_results):
        simple_extractor_assert(actual, expected, 'text', 'Text')
        simple_extractor_assert(actual, expected, 'type', 'Type')
        simple_extractor_assert(actual, expected, 'start', 'Start')
        simple_extractor_assert(actual, expected, 'length', 'Length')
        if 'Value' in expected:
            assert actual.value
        if actual.value and 'Value' in expected:
            if 'values' in expected['Value']:
                assert isinstance(actual.value['values'], list)
                for actual_values, expected_values in zip(actual.value['values'], expected['Value']['values']):
                    for key in expected_values.keys():
                        assert actual_values[key] == expected_values[key]

@pytest.mark.parametrize('culture, model, options, context, source, expected_results', get_specs(recognizer='DateTime', entity='Model'))
def test_datetime_model(culture, model, options, context, source, expected_results):
    reference_datetime = get_reference_date(context)
    option_obj = get_option(options)

    result = get_results(culture, model, source, option_obj, reference_datetime)

    assert len(result) == len(expected_results)
    for actual, expected in zip(result, expected_results):
        simple_parser_assert(actual, expected, 'text', 'Text')
        simple_parser_assert(actual, expected, 'type_name', 'TypeName')
        simple_parser_assert(actual, expected, 'parent_text', 'ParentText')
        simple_parser_assert(actual, expected, 'start', 'Start')
        simple_parser_assert(actual, expected, 'end', 'End')
        assert len(actual.resolution['values']) == len(expected['Resolution']['values'])
        for actual_resilution_value, expected_resoulution_value in zip(actual.resolution['values'], expected['Resolution']['values']):
            assert_model_resolution(actual_resilution_value, expected_resoulution_value)

def single_assert(actual, expected, prop):
    if expected.get(prop):
        assert actual[prop] == expected[prop]
    else:
        assert actual.get(prop) is None

def assert_model_resolution(actual, expected):
    single_assert(actual, expected, 'timex')
    single_assert(actual, expected, 'type')
    single_assert(actual, expected, 'value')
    single_assert(actual, expected, 'start')
    single_assert(actual, expected, 'end')
    single_assert(actual, expected, 'Mod')

def simple_extractor_assert(actual, expected, prop, resolution):
    if resolution in expected:
        assert getattr(actual, prop) == expected[resolution]

def simple_parser_assert(actual, expected, prop, resolution):
    if resolution in expected:
        assert getattr(actual, prop) == expected[resolution]

def create_extractor(language, model, options):
    extractor = get_class('recognizers_date_time', f'{language}{model}Extractor')
    if extractor:
        return extractor()

    extractor = get_class(f'recognizers_date_time.date_time.{language.lower()}.{model.lower()}',
                          f'{language}{model}Extractor')
    if extractor:
        return extractor()

    extractor = get_class(f'recognizers_date_time.date_time.base_{model.lower()}',
                          f'Base{model}Extractor')
    configuration = get_class(f'recognizers_date_time.date_time.{language.lower()}.{model.lower()}_extractor_config',
                              f'{language}{model}ExtractorConfiguration')

    if model == 'Merged':
        option = get_option(options)
        return extractor(configuration(), option)
    
    return extractor(configuration())

def create_parser(language, model, options):
    parser = get_class(f'recognizers_date_time.date_time.{language.lower()}.{model.lower()}_parser', f'{language}{model}Parser')
    if parser:
        return parser()

    parser = get_class(f'recognizers_date_time.date_time.{language.lower()}.parsers', f'{language}{model}Parser')
    if not parser:
        parser = get_class(f'recognizers_date_time.date_time.base_{model.lower()}',
                           f'Base{model}Parser')

    configuration_class = get_class(f'recognizers_date_time.date_time.{language.lower()}.{model.lower()}_parser_config',
                                    f'{language}{model}ParserConfiguration')

    language_configuration = get_class(f'recognizers_date_time.date_time.{language.lower()}.common_configs',
                                       f'{language}CommonDateTimeParserConfiguration')

    configuration = configuration_class(language_configuration()) if language_configuration else configuration_class()

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
    return datetime.datetime.strptime(reference_datetime[0:19], '%Y-%m-%dT%H:%M:%S') if reference_datetime and not isinstance(reference_datetime, datetime.datetime) else None

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
