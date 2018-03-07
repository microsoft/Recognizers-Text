import pytest
import glob
import os
import json


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
    return { "type": parts[2], "sub_type": parts[4], "language": parts[3] }

def get_suite(json_path):
    print(json_path)
    return {"specs": json.load(open(json_path, encoding="utf-8")), "config": get_suite_config(json_path) }

def get_all_specs():
    files = glob.glob("../Specs/**/*.json", recursive=True)
    result = list(map(get_suite, files))
    return result

def get_specs(spec_type):
    ret_specs = []
    filtered_specs = list(filter(lambda x: x["config"]["type"] == spec_type, specs))
    for sp in filtered_specs:
        for spec in sp['specs']:
            if "NotSupportedByDesign" in spec and "python" in spec["NotSupportedByDesign"]:
                continue
            if "NotSupported" in spec and "python" in spec["NotSupported"]:
                continue
            ret_specs.append((sp["config"]["language"], sp["config"]["sub_type"], spec["Input"], spec["Results"]))
    return ret_specs

specs = get_all_specs()

def get_model_function(culture, model):
    pass
    
@pytest.mark.parametrize("culture, model, source, results", get_specs("Number"))
def test_if_it_works(culture, model, source, expected_results):
    func = get_model_function(culture, model)
    result = func(source)
    assert result == expected_results