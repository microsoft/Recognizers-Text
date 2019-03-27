@echo off
echo *** Building Microsoft.Recognizers.Text.DataTypes.TimexExpression
setlocal
setlocal enabledelayedexpansion
setlocal enableextensions

for /f "usebackq tokens=*" %%i in (`..\packages\vswhere.2.2.7\tools\vswhere -latest -products * -requires Microsoft.Component.MSBuild -property installationPath`) do (
  set MSBuildDir=%%i\MSBuild\15.0\Bin\
)

if not exist ..\nuget mkdir ..\nuget
if exist ..\nuget\Microsoft.Recognizers.Text.DataTypes.TimexExpression*.nupkg erase /s ..\nuget\Microsoft.Recognizers.Text.DataTypes.TimexExpression*.nupkg
"%MSBuildDir%\MSBuild\15.0\Bin\MSBuild.exe" /property:Configuration=release Microsoft.Recognizers.Text.DataTypes.TimexExpression.csproj
for /f %%v in ('powershell -noprofile "(Get-Command .\bin\release\netstandard2.0\Microsoft.Recognizers.Text.DataTypes.TimexExpression.dll).FileVersionInfo.FileVersion"') do set numberVersion=%%v
..\buildtools\NuGet.exe pack Microsoft.Recognizers.Text.DataTypes.TimexExpression.nuspec -symbols -properties version=%numberVersion% -OutputDirectory ..\nuget

set error=%errorlevel%
set packageName=Microsoft.Recognizers.Text.DataTypes.TimexExpression
if %error% NEQ 0 (
	echo *** Failed to build %packageName%
	exit /b %error%
) else (
	echo *** Succeeded to build %packageName%
)