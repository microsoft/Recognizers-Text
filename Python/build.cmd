pushd libraries\resource-generator

REM Dependencies
ECHO.
ECHO # Installing Resource Generator Dependencies

CALL pip install -r .\requirements.txt

REM Build Resources
ECHO.
ECHO # Building Resources
CALL python index.py ..\recognizers-number\resource-definitions.json

popd