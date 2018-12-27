@ECHO off
SETLOCAL EnableDelayedExpansion

ECHO.
ECHO # Building .NET platform

REM vswhere is an optional component for Visual Studio and also installed with Build Tools. 
REM vswhere will look for Community, Professional, and Enterprise editions of Visual Studio
REM (only works with Visual Studio 2017 Update 2 or newer installed)
SET vswhere="%ProgramFiles(x86)%\Microsoft Visual Studio\Installer\vswhere.exe"

for /f "usebackq tokens=*" %%i in (`!vswhere! -latest -products * -requires Microsoft.Component.MSBuild -property installationPath`) do (
  set VSInstallDir=%%i
)

ECHO.
SET MsBuildVersion=15.0
ECHO # Finding MSBuild !MsBuildVersion!

if EXIST "%VSInstallDir%\MSBuild\!MsBuildVersion!\Bin\MSBuild.exe" (
	SET MSBuild="%VSInstallDir%\MSBuild\15.0\Bin\MSBuild.exe" %*
	ECHO Found MSBuild !MSBuild!
) else (
	ECHO "msbuild.exe" could not be found at "!VSInstallDir!"
	EXIT /B
)

ECHO.
ECHO # Finding VSTest
SET VSTestDir=%VSInstallDir%\Common7\IDE\CommonExtensions\Microsoft\TestWindow

IF NOT EXIST "%VSTestDir%\vstest.console.exe" (
	ECHO "vstest.console.exe" could not be found at "%VSTestDir%"
	EXIT /B
) 

ECHO.
ECHO # Restoring NuGet dependencies
CALL "buildtools\nuget" restore

ECHO.
ECHO # Building .NET solution (release)
CALL !MSBuild! Microsoft.Recognizers.Text.sln /t:Clean,Build /p:Configuration=Release

ECHO.
ECHO # Running .NET Tests
SET testcontainer=
FOR /R %%f IN (*Tests.dll) DO (
	(ECHO "%%f" | FIND /V "\bin\Release" 1>NUL) || (
		SET testcontainer=!testcontainer! "%%f"
	)
)
ECHO "!VsTestDir!\vstest.console"
CALL "!VsTestDir!\vstest.console" /Parallel %testcontainer%

ECHO.
ECHO # Running CreateAllPackages.cmd
CALL CreateAllPackages.cmd
IF %ERRORLEVEL% NEQ 0 (
	ECHO # Failed to create packages.
	EXIT /b -1
)