echo // Installing recognizers-text
pip install -e ./libraries/recognizers-text/

echo // Installing recognizers-number
pip install -e ./libraries/recognizers-number/

echo // Installing recognizers-number-with-unit
pip install -e ./libraries/recognizers-number-with-unit/

echo // Installing datatypes-timex-expression
pip install -e ./libraries/datatypes-timex-expression/

echo // Installing recognizers-date-time
pip install -e ./libraries/recognizers-date-time/

echo // Installing recognizers-sequence
pip install -e ./libraries/recognizers-sequence/

echo // Installing recognizers-choice
pip install -e ./libraries/recognizers-choice/

echo // Installing recognizers-suite
pip install -e ./libraries/recognizers-suite/

echo // Installing Test Dependencies
pip install -r ./tests/requirements.txt

echo // Running tests
pytest --tb=line
