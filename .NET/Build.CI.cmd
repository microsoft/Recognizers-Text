@ECHO off
SETLOCAL EnableDelayedExpansion

ECHO.
ECHO # Building .NET platform

ECHO.
ECHO # Restoring NuGet dependencies
CALL nuget restore

ECHO.
ECHO # Building .NET solution (debug)
CALL msbuild Microsoft.Recognizers.Text.sln /t:Clean,Build /p:Configuration=Debug

ECHO.
ECHO # Running .NET Tests
SET testcontainer=
FOR /R %%f IN (*Tests.dll) DO (
	(ECHO "%%f" | FIND /V "\bin\Debug" 1>NUL) || (
		SET testcontainer=!testcontainer! "%%f"
	)
)
CALL vstest.console %testcontainer%