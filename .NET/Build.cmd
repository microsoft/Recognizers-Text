@ECHO off
SETLOCAL EnableDelayedExpansion

SET MsBuildDir=%programfiles(x86)%\Microsoft Visual Studio\2017\Enterprise\MSBuild\15.0\bin
REM SET MsBuildDir=%programfiles(x86)%\MSBuild\14.0\Bin
IF NOT EXIST "%MsBuildDir%\msbuild.exe" (
    ECHO "msbuild.exe" could not be found at "%MsBuildDir%"
    EXIT /B
)

SET MsTestDir=%programfiles(x86)%\Microsoft Visual Studio\2017\Enterprise\Common7\IDE
IF NOT EXIST "%MsTestDir%\MSTest.exe" (
    ECHO "mstest.exe" could not be found at "%MsTestDir%"
    EXIT /B
)

ECHO # Restoring NuGet dependencies
CALL "buildtools\nuget" restore

ECHO # Building .NET solution (debug)
CALL "%MsBuildDir%\msbuild" Microsoft.Recognizers.Text.sln /t:Clean,Build /p:Configuration=Debug


ECHO # Running .NET Tests
SET testcontainer=
FOR /R %%f IN (*.Tests.dll) DO (
	(ECHO "%%f" | FIND /V "\bin\Debug" 1>NUL) || (
		SET testcontainer=!testcontainer! /testcontainer:%%f
	)
)
CALL "%MsTestDir%\mstest" %testcontainer%