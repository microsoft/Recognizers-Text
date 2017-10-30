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

REM Prebuild each sub-module referenced on main module
ECHO.
ECHO # Building recognizers number module
CALL npm run prebuild-number

ECHO.
ECHO # Building recognizers number-with-unit module
CALL npm run prebuild-number-with-unit

ECHO.
ECHO # Building recognizers date-time module
CALL npm run prebuild-date-time

REM Build main module
ECHO.
ECHO # Installing dependencies - npm install
CALL npm i

ECHO.
ECHO # Building - npm run build
CALL npm run build

ECHO.
ECHO # Running test - npm run test
CALL npm run test