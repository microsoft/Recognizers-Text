cd libraries/resource-generator

echo // Installing Resource Generator Dependencies
pip install -r ./requirements.txt

echo // Building Resources
python index.py ../recognizers-number/resource-definitions.json
python index.py ../recognizers-number-with-unit/resource-definitions.json
python index.py ../recognizers-date-time/resource-definitions.json
python index.py ../recognizers-sequence/resource-definitions.json

cd ../..
echo // Installing recognizers-text
pip install -e ./libraries/recognizers-text/

echo // Installing recognizers-number
pip install -e ./libraries/recognizers-number/

echo // Installing recognizers-number-with-unit
pip install -e ./libraries/recognizers-number-with-unit/

echo // Installing recognizers-date-time
pip install -e ./libraries/recognizers-date-time/

echo // Installing recognizers-sequence
pip install -e ./libraries/recognizers-sequence/

echo // Installing recognizers-suite
pip install -e ./libraries/recognizers-suite/

echo // Installing Test Dependencies
pip install -r ./tests/requirements.txt

echo // Running tests
pytest --tb=line
