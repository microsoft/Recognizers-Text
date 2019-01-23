ECHO ==============================.NET BUILD START==============================

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
ECHO # Restoring NuGet dependencies
CALL "buildtools\nuget" restore

set configuration=Release
ECHO.
ECHO # Generate resources
CALL "!MsBuildDir!\msbuild" Microsoft.Recognizers.Definitions.Common\Microsoft.Recognizers.Definitions.Common.csproj /t:Clean,Build /p:Configuration=%configuration%

ECHO # Building .NET solution (%configuration%)
CALL !MSBuild! Microsoft.Recognizers.Text.sln /t:Clean,Build /p:Configuration=%configuration%
IF %ERRORLEVEL% NEQ 0 (
	ECHO # Failed to build .NET Project.
	EXIT /b %ERRORLEVEL%
)

ECHO ============================== .NET BUILD END ==============================