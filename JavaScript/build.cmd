@ECHO off

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

ECHO # Building - npm run build
CALL npm run build

ECHO # Running test - npm run test
CALL npm run test