@echo off
echo *** Building Microsoft.Recognizers.DataTypes.DateTime
setlocal
setlocal enabledelayedexpansion
setlocal enableextensions

for /f "usebackq tokens=*" %%i in (`..\packages\vswhere.2.2.7\tools\vswhere -latest -products * -requires Microsoft.Component.MSBuild -property installationPath`) do (
  set MSBuildDir=%%i\MSBuild\15.0\Bin\
)

if not exist ..\nuget mkdir ..\nuget
if exist ..\nuget\Microsoft.Recognizers.DataTypes.DateTime*.nupkg erase /s ..\nuget\Microsoft.Recognizers.DataTypes.DateTime*.nupkg
"%MSBuildDir%\MSBuild\15.0\Bin\MSBuild.exe" /property:Configuration=release Microsoft.Recognizers.DataTypes.DateTime.csproj
for /f %%v in ('powershell -noprofile "(Get-Command .\bin\release\netstandard2.0\Microsoft.Recognizers.DataTypes.DateTime.dll).FileVersionInfo.FileVersion"') do set numberVersion=%%v
..\packages\NuGet.CommandLine.4.3.0\tools\NuGet.exe pack Microsoft.Recognizers.DataTypes.DateTime.nuspec -symbols -properties version=%numberVersion% -OutputDirectory ..\nuget

set error=%errorlevel%
set packageName=Microsoft.Recognizers.DataTypes.DateTime
if %error% NEQ 0 (
	echo *** Failed to build %packageName%
	exit /b %error%
) else (
	echo *** Succeeded to build %packageName%
)