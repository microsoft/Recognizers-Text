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

REM Dependencies
ECHO.
ECHO # Installing dependencies
CALL npm i

REM Build Packages
ECHO.
ECHO # Building - npm run build
CALL npm run build

REM Tests
ECHO.
ECHO # Running tests - npm run test
CALL npm run test