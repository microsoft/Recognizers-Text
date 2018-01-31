@ECHO off
SETLOCAL EnableDelayedExpansion

ECHO.
ECHO # Building .NET platform

SET "Vs2017SubDir=\Microsoft Visual Studio\2017\Enterprise"
SET MsBuildSubDir=\MSBuild\15.0\bin
SET VsCommonSubDir=\Common7\IDE\CommonExtensions\Microsoft\TestWindow

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

ECHO # Finding VsTest
SET "VsTestDir=!ProgFilesDir!%Vs2017SubDir%%VsCommonSubDir%"
IF NOT EXIST "%VsTestDir%\vstest.console.exe" (
    ECHO "vstest.console.exe" could not be found at "%VsTestDir%"
    EXIT /B
)

ECHO.
ECHO # Restoring NuGet dependencies
CALL "buildtools\nuget" restore

ECHO.
ECHO # Building .NET solution (release)
CALL "!MsBuildDir!\msbuild" Microsoft.Recognizers.Text.sln /t:Clean,Build /p:Configuration=Release

ECHO.
ECHO # Running .NET Tests
SET testcontainer=
FOR /R %%f IN (*Tests.dll) DO (
	(ECHO "%%f" | FIND /V "\bin\Release" 1>NUL) || (
		SET testcontainer=!testcontainer! "%%f"
	)
)
CALL "!VsTestDir!\vstest.console" /Parallel %testcontainer%

ECHO.
ECHO # Running CreateAllPackages.cmd
CALL CreateAllPackages.cmd
IF %ERRORLEVEL% NEQ 0 (
	ECHO # Failed to create packages.
	EXIT /b -1
)