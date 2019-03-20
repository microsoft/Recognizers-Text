@echo off
echo *** Building Microsoft.Recognizers.Text.Number
setlocal
setlocal enabledelayedexpansion
setlocal enableextensions

for /f "usebackq tokens=*" %%i in (`..\packages\vswhere.2.2.7\tools\vswhere -latest -products * -requires Microsoft.Component.MSBuild -property installationPath`) do (
  set MSBuildDir=%%i\MSBuild\15.0\Bin\
)

if not exist ..\nuget mkdir ..\nuget
if exist ..\nuget\Microsoft.Recognizers.Text.Number*.nupkg erase /s ..\nuget\Microsoft.Recognizers.Text.Number*.nupkg
"%MSBuildDir%\MSBuild\15.0\Bin\MSBuild.exe" /property:Configuration=release Microsoft.Recognizers.Text.Number.csproj
for /f %%v in ('powershell -noprofile "(Get-Command .\bin\release\netstandard2.0\Microsoft.Recognizers.Text.dll).FileVersionInfo.FileVersion"') do set basic=%%v
for /f %%v in ('powershell -noprofile "(Get-Command .\bin\release\netstandard2.0\Microsoft.Recognizers.Text.Number.dll).FileVersionInfo.FileVersion"') do set numberVersion=%%v
..\buildtools\NuGet.exe pack Microsoft.Recognizers.Text.Number.nuspec -symbols -properties version=%numberVersion%;basic=%basic% -OutputDirectory ..\nuget

set error=%errorlevel%
set packageName=Microsoft.Recognizers.Text.Number
if %error% NEQ 0 (
	echo *** Failed to build %packageName%
	exit /b %error%
) else (
	echo *** Succeeded to build %packageName%
)