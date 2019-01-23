ECHO ==============================JAVASCRIPT BUILD START==============================

ECHO.
ECHO # Building Javascript platform

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

IF %ERRORLEVEL% NEQ 0 (
	ECHO # Failed to build JavaScript Project.
	EXIT /b %ERRORLEVEL%
)

ECHO ============================== JAVASCRIPT BUILD END ==============================