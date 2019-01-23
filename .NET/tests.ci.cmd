ECHO ==============================.NET TESTS START==============================

@ECHO off
SETLOCAL EnableDelayedExpansion

SET vswhere="%ProgramFiles(x86)%\Microsoft Visual Studio\Installer\vswhere.exe"

for /f "usebackq tokens=*" %%i in (`!vswhere! -latest -products * -requires Microsoft.Component.MSBuild -property installationPath`) do (
  set VSInstallDir=%%i
)

set configuration=Release

ECHO.
ECHO # Finding VSTest
SET VSTestDir=%VSInstallDir%\Common7\IDE\CommonExtensions\Microsoft\TestWindow

IF NOT EXIST "%VSTestDir%\vstest.console.exe" (
	ECHO "vstest.console.exe" could not be found at "%VSTestDir%"
	EXIT /B
) 

ECHO # Running .NET Tests
SET testcontainer=
FOR /R %%f IN (*Tests.dll) DO (
	(ECHO "%%f" | FIND /V "\bin\%configuration%" 1>NUL) || (
		SET testcontainer=!testcontainer! "%%f"
	)
)
ECHO "!VsTestDir!\vstest.console"
CALL "!VsTestDir!\vstest.console" /Parallel %testcontainer%
IF %ERRORLEVEL% NEQ 0 GOTO TEST_ERROR

ECHO.
ECHO # Running CreateAllPackages.cmd
CALL CreateAllPackages.cmd
IF %ERRORLEVEL% NEQ 0 (
	ECHO # Failed to create packages.
	EXIT /b -1
)

EXIT /b 0

:TEST_ERROR
ECHO .NET Test failure(s) found!
EXIT /b 1

ECHO ============================== .NET TESTS END ==============================