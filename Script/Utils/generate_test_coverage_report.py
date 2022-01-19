import csv
import json
from itertools import groupby
from pathlib import Path


OUTPUT_FILE = 'python_test_coverage_report.csv'


def main():
    test_coverage = []
    language_set = set()

    for path in Path('../../Specs').rglob('*.json'):
        entity, lang = path.parts[-3:-1]
        language_set.add(lang)

        with path.open() as f:
            try:
                test_cases = json.loads(f.read())
            except json.JSONDecodeError as e:
                continue

            total = len(test_cases)
            supported = 0
            for test_case in test_cases:
                not_supported = test_case.get('NotSupportedByDesign', '')
                not_supported += test_case.get('NotSupported', '')
                if 'python' not in not_supported:
                    supported += 1
            test_coverage.append({
                'entity': entity,
                'lang': lang,
                'model': path.stem,
                'percent': round(supported / total * 100) if total else None,
            })

    languages = sorted(language_set)
    sort_key = lambda x: (x['entity'], x['model'], x['lang'])
    test_coverage = sorted(test_coverage, key=sort_key)

    with open(OUTPUT_FILE, 'w', newline='') as f:
        writer = csv.writer(f)
        writer.writerow([''] + languages)
        for entity, entity_group in groupby(test_coverage, key=lambda x: x['entity']):
            writer.writerow([entity])
            for model, model_group in groupby(entity_group, key=lambda x: x['model']):
                lang_coverage = {lang: {'percent': None} for lang in languages}
                for lang in model_group:
                    if lang['percent'] is not None:
                        lang_coverage[lang['lang']] = lang
                lang_coverage_list = [lang_coverage[lang] for lang in languages]
                writer.writerow([model] + [lang['percent'] if lang else '' for lang in lang_coverage_list])


if __name__ == '__main__':
    main()
