import glob
import os
import json
import re
import pytest
from recognizers_text.culture import Culture


def split_all(path):
    all_parts = []
    while True:
        parts = os.path.split(path)
        if parts[0] == path:  # sentinel for absolute paths
            all_parts.insert(0, parts[0])
            break
        elif parts[1] == path:  # sentinel for relative paths
            all_parts.insert(0, parts[1])
            break
        else:
            path = parts[0]
            all_parts.insert(0, parts[1])
    return all_parts


def get_suite_config(json_path):
    parts = split_all(json_path)
    filename = os.path.splitext(parts[4])[0]
    model, entity, options = ENTITY_PATTERN.search(filename).groups()
    if model == 'Merged' and entity == 'Parser':
        entity = f'{model}{entity}'
    return {'recognizer': parts[2], 'model': model,
            'entity': entity, 'options': options, 'language': parts[3]}


def get_suite(json_path):
    return {'specs': json.load(
        open(json_path, encoding='utf-8-sig')), 'config': get_suite_config(json_path)}


def get_all_specs():
    files = glob.glob('../Specs/**/*.json', recursive=True)
    result = list(map(get_suite, files))
    return result


def get_specs(recognizer, entity):
    ret_specs = []
    filtered_specs = list(filter(
        lambda x: x['config']['recognizer'] == recognizer and x['config']['entity'] == entity and CULTURES.get(
            x['config']['language']), SPECS))
    for sp in filtered_specs:
        for spec in sp['specs']:
            if 'NotSupportedByDesign' in spec and 'python' in spec['NotSupportedByDesign']:
                continue
            not_supported = 'NotSupported' in spec and 'python' in spec['NotSupported']
            message = sp['config']['language'] + ' - ' + \
                recognizer + ' - ' + sp['config']['model'] + entity
            ret_specs.append(pytest.param(
                CULTURES[sp['config']['language']],
                sp['config']['model'],
                sp['config']['options'],
                spec.get('Context'),
                spec['Input'],
                spec['Results'],
                marks=pytest.mark.skipif(not_supported, reason=f'Not supported: {message}')))
    return ret_specs


ENTITY_PATTERN = re.compile('(.*)(Model|Parser|Extractor)(.*)')

CULTURES = {
    'Chinese': Culture.Chinese,
    'Dutch': Culture.Dutch,
    'English': Culture.English,
    'French': Culture.French,
    'Italian': Culture.Italian,
    'Japanese': Culture.Japanese,
    'Korean': Culture.Korean,
    'Portuguese': Culture.Portuguese,
    'Spanish': Culture.Spanish,
    'Turkish': Culture.Turkish,
    # 'German': Culture.German,
}

SPECS = get_all_specs()
