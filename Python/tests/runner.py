import pytest
import glob
import os
import json
from recognizers_text.culture import Culture

def splitall(path):
    allparts = []
    while 1:
        parts = os.path.split(path)
        if parts[0] == path:  # sentinel for absolute paths
            allparts.insert(0, parts[0])
            break
        elif parts[1] == path: # sentinel for relative paths
            allparts.insert(0, parts[1])
            break
        else:
            path = parts[0]
            allparts.insert(0, parts[1])
    return allparts

def get_suite_config(json_path):
    parts = splitall(json_path)
    filename = os.path.splitext(parts[4])[0]
    return {'type': parts[2], 'sub_type': filename, 'language': parts[3]}

def get_suite(json_path):
    print(json_path)
    return {'specs': json.load(open(json_path, encoding='utf-8')), 'config': get_suite_config(json_path)}

def get_all_specs():
    files = glob.glob('../Specs/**/*.json', recursive=True)
    result = list(map(get_suite, files))
    return result

def get_specs(spec_type):
    ret_specs = []
    filtered_specs = list(filter(lambda x: x['config']['type'] == spec_type, SPECS))
    for sp in filtered_specs:
        for spec in sp['specs']:
            if 'NotSupportedByDesign' in spec and 'python' in spec['NotSupportedByDesign']:
                continue
            not_suppoted = 'NotSupported' in spec and 'python' in spec['NotSupported']
            ret_specs.append(pytest.param(CULTURES[sp['config']['language']], sp['config']['sub_type'], spec['Input'], spec['Results'], marks=pytest.mark.skipif(not_suppoted, reason='Not supported')))
    return ret_specs

CULTURES = {
    'English': Culture.English,
    'Chinese': Culture.Chinese,
    'Spanish': Culture.Spanish,
    'Portuguese': Culture.Portuguese,
    'French': Culture.French,
    'Japanese': Culture.Japanese
}

SPECS = get_all_specs()
