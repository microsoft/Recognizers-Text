ECHO ==============================.NET BUILD START==============================

@ECHO off
SETLOCAL EnableDelayedExpansion

ECHO.
ECHO # Setting encoding to UTF-8
chcp 65001

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
ECHO # Finding MSBuild

SET MsBuildVersion=Current
ECHO # Trying !MsBuildVersion! for VS2019

if EXIST "%VSInstallDir%\MSBuild\!MsBuildVersion!\Bin\MSBuild.exe" (
	SET MSBuild="%VSInstallDir%\MSBuild\!MsBuildVersion!\Bin\MSBuild.exe" %*
) else (
	ECHO MSBuild !MsBuildVersion! not found!
	ECHO.
	
	SET MsBuildVersion=15.0
	ECHO # Trying !MsBuildVersion! for VS2017

	if EXIST "%VSInstallDir%\MSBuild\!MsBuildVersion!\Bin\MSBuild.exe" (
		SET MSBuild="%VSInstallDir%\MSBuild\!MsBuildVersion!\Bin\MSBuild.exe" %*
	) else (
		ECHO "msbuild.exe" could not be found at "!VSInstallDir!"
		EXIT /B
	)
)

ECHO Found MSBuild !MSBuild!

ECHO.
ECHO # Check for empty and duplicate inputs in Specs
Powershell -ExecutionPolicy Bypass "& {buildtools\checkSpec.ps1; exit $LastExitCode }"
IF %ERRORLEVEL% NEQ 0 (
	ECHO # Failed, including empty or duplicate inputs in Specs
	EXIT /b %ERRORLEVEL%
)

ECHO.
ECHO # Restoring NuGet dependencies
CALL "buildtools\nuget" restore

set configuration=Release
ECHO.
ECHO # Generate resources
CALL !MSBuild! Microsoft.Recognizers.Definitions.Common\Microsoft.Recognizers.Definitions.Common.csproj /t:Clean,Build /p:Configuration=%configuration%

ECHO # Building .NET solution (%configuration%)
CALL !MSBuild! Microsoft.Recognizers.Text.sln /t:Restore,Clean,Build /p:Configuration=%configuration%
IF %ERRORLEVEL% NEQ 0 (
	ECHO # Failed to build .NET Project.
	EXIT /b %ERRORLEVEL%
)

ECHO ============================== .NET BUILD END ==============================
