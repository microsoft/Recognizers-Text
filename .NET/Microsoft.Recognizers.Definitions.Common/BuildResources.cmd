@ECHO off
SETLOCAL EnableDelayedExpansion

SET COMMONASSEMBLYPATH=%1
IF [%1] == [] SET COMMONASSEMBLYPATH=%~dp0bin\Debug\net462\Microsoft.Recognizers.Definitions.Common.dll

IF NOT EXIST "%COMMONASSEMBLYPATH%" (
	ECHO "%COMMONASSEMBLYPATH%" could not be found, build the Microsoft.Recognizers.Definitions solution first.
	EXIT /B
) 

ECHO.
ECHO # Transform All T4 Templates

ECHO.
FOR /R %%i IN (*.tt) DO (ECHO # Transform %%i to %%~dpni.cs & dotnet tt "%%i" -o "%%~dpni.cs" -r "%COMMONASSEMBLYPATH%")

EXIT /b 0

:ERROR
ECHO Error found Transforming T4 Templates
EXIT /b 1