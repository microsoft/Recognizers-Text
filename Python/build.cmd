pushd libraries\resource-generator

REM Dependencies
ECHO.
ECHO # Installing Resource Generator Dependencies
CALL pip install -r .\requirements.txt

REM Build Resources
ECHO.
ECHO # Building Resources
CALL python index.py ..\recognizers-number\resource-definitions.json
CALL python index.py ..\recognizers-number-with-unit\resource-definitions.json
CALL python index.py ..\recognizers-date-time\resource-definitions.json

popd
ECHO.
ECHO # Installing recognizers-text
CALL  pip install -e .\libraries\recognizers-text\

ECHO.
ECHO # Installing recognizers-number
CALL  pip install -e .\libraries\recognizers-number\

ECHO.
ECHO # Installing recognizers-number-with-unit
CALL  pip install -e .\libraries\recognizers-number-with-unit\

ECHO.
ECHO # Installing recognizers-date-time
CALL  pip install -e .\libraries\recognizers-date-time\

ECHO.
ECHO # Installing Test Dependencies
CALL pip install -r .\tests\requirements.txt

ECHO.
ECHO # Running tests
CALL pytest --tb=line