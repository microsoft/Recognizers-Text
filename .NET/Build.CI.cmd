@ECHO off
SETLOCAL EnableDelayedExpansion

ECHO.
ECHO # Building .NET platform

ECHO.
ECHO # Restoring NuGet dependencies
CALL nuget restore

set configuration=Release
ECHO.
ECHO # Building .NET solution (%configuration%)
CALL msbuild Microsoft.Recognizers.Text.sln /t:Clean,Build /p:Configuration=%configuration%

ECHO.
ECHO # Running .NET Tests
SET testcontainer=
FOR /R %%f IN (*Tests.dll) DO (
	(ECHO "%%f" | FIND /V "\bin\%configuration%" 1>NUL) || (
		SET testcontainer=!testcontainer! "%%f"
	)
)
vstest.console %testcontainer% /Parallel 
IF %ERRORLEVEL% NEQ 0 GOTO TEST_ERROR

ECHO.
ECHO # Running CreateAllPackages.cmd
CALL CreateAllPackages.cmd

EXIT /b 0

:TEST_ERROR
ECHO Test failure(s) found!
EXIT /b 1