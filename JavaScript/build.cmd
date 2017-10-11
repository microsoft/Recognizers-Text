@ECHO off

ECHO.
ECHO # Building Javacript platform

REM Check Node/NPM installation
WHERE /q node
IF ERRORLEVEL 1 (
    ECHO Node.js executable not found. Please install it from https://nodejs.org/
    EXIT /B
)
WHERE /q npm
IF ERRORLEVEL 1 (
    ECHO NPM executable not found. Please install it from https://nodejs.org/
    EXIT /B
)

IF NOT EXIST "node_modules" (
	ECHO # Installing dependencies - npm install
	CALL npm i
)

IF NOT EXIST "recognizers-number/node_modules" (
    ECHO # Building recognizers number module
    CALL npm run prebuild-number
)

IF NOT EXIST "recognizers-number-with-unit/node_modules" (
    ECHO # Building recognizers number-with-unit module
    CALL npm run prebuild-number-with-unit
)

IF NOT EXIST "recognizers-date-time/node_modules" (
    ECHO # Building recognizers date-time module
    CALL npm run prebuild-date-time
)

ECHO.
ECHO # Building - npm run build
CALL npm run build

ECHO.
ECHO # Running test - npm run test
CALL npm run test