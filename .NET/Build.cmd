@ECHO off
SETLOCAL EnableDelayedExpansion

SET "Vs2017SubDir=\Microsoft Visual Studio\2017\Enterprise"
SET MsBuildSubDir=\MSBuild\15.0\bin
SET VsCommonSubDir=\Common7\IDE

SET "ProgFilesDir=%programfiles(x86)%"

ECHO # Finding MSBuild
SET "MsBuildDir=!ProgFilesDir!%Vs2017SubDir%%MsBuildSubDir%"
REM SET MsBuildDir=%programfiles(x86)%\MSBuild\14.0\Bin
IF NOT EXIST "!MsBuildDir!\msbuild.exe" (
	SET "ProgFilesDir=F:\Program Files (x86)"
	SET "MsBuildDir=!ProgFilesDir!%Vs2017SubDir%%MsBuildSubDir%"
	IF NOT EXIST "!MsBuildDir!\msbuild.exe" (
		ECHO "msbuild.exe" could not be found at "!MsBuildDir!"
		EXIT /B
	)
)

ECHO # Finding MSTest
SET "MsTestDir=!ProgFilesDir!%Vs2017SubDir%%VsCommonSubDir%"
IF NOT EXIST "%MsTestDir%\MSTest.exe" (
    ECHO "mstest.exe" could not be found at "%MsTestDir%"
    EXIT /B
)

ECHO # Restoring NuGet dependencies
CALL "buildtools\nuget" restore

ECHO # Building .NET solution (debug)
CALL "!MsBuildDir!\msbuild" Microsoft.Recognizers.Text.sln /t:Clean,Build /p:Configuration=Debug


ECHO # Running .NET Tests
SET testcontainer=
FOR /R %%f IN (*.Tests.dll) DO (
	(ECHO "%%f" | FIND /V "\bin\Debug" 1>NUL) || (
		SET testcontainer=!testcontainer! /testcontainer:%%f
	)
)
CALL "!MsTestDir!\mstest" %testcontainer%