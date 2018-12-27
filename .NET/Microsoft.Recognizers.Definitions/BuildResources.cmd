@ECHO off
SETLOCAL EnableDelayedExpansion
SET COMMONASSEMBLYPATH=%~dp0..\build\package\net462\Microsoft.Recognizers.Definitions.Common.dll


ECHO.
ECHO # Transform All T4 Templates

ECHO.
FOR /R %%i IN (*.tt) DO (ECHO # Transform %%i to %%~dpni.cs & dotnet tt %%i -o %%~dpni.cs -r %COMMONASSEMBLYPATH%)

EXIT /b 0

:ERROR
ECHO Error found Transforming T4 Templates
EXIT /b 1