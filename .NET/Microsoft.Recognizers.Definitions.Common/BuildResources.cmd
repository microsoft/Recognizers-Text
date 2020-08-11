@ECHO off
SETLOCAL EnableDelayedExpansion

ECHO.
ECHO # Building Resources

SET COMMONASSEMBLYPATH=%1
IF [%1] == [] SET COMMONASSEMBLYPATH=%~dp0bin\Release\net462\Microsoft.Recognizers.Definitions.Common.dll

IF NOT EXIST "%COMMONASSEMBLYPATH%" (
	ECHO "%COMMONASSEMBLYPATH%" could not be found, build the Microsoft.Recognizers.Definitions solution first.
	EXIT /B
) 

ECHO.
ECHO # Transform All T4 Templates

ECHO.
FOR /R %%i IN (*.tt) DO ( CALL :COMPARER %%i %%~dpni)

EXIT /b 0

:COMPARER
REM Workaround to issue with exitCode/ERRORLEVEL in previous FOR loop
FOR /f "delims=" %%a in ('Powershell -ExecutionPolicy Bypass -Command "& {"..\buildtools\tsComparer.ps1" "%2.tt"}"') DO SET "RET=%%a"

IF %RET% NEQ 0 (
	ECHO # Transform %1 to %2.cs & dotnet tt "%1" -o "%2.cs" -r "%COMMONASSEMBLYPATH%"
) ELSE (
	ECHO # No need to re-generate %1
)
GOTO :EOF

:ERROR
ECHO Error found Transforming T4 Templates
EXIT /b 1

:EOF