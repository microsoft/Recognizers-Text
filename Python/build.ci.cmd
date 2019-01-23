ECHO ==============================PYTHON BUILD/TEST START==============================

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
CALL python index.py ..\recognizers-sequence\resource-definitions.json

popd

pip install -e .\libraries\recognizers-text\

pip install -e .\libraries\recognizers-number\

pip install -e .\libraries\recognizers-number-with-unit\

pip install -e .\libraries\recognizers-date-time\

pip install -e .\libraries\recognizers-sequence\

pip install -e .\libraries\recognizers-suite\

pip install -r .\tests\requirements.txt

pytest --tb=line

IF %ERRORLEVEL% == 1 (
	ECHO Python Test failure/s found!
	EXIT /b %ERRORLEVEL%
) ELSE (
	IF %ERRORLEVEL% NEQ 0 (
		ECHO # Failed to build Python Project.
		EXIT /b %ERRORLEVEL%
	)
)

ECHO ============================== PYTHON BUILD/TEST END ==============================