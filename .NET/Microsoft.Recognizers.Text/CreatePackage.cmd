@echo off
echo *** Building Microsoft.Recognizers.Text
setlocal
setlocal enabledelayedexpansion
setlocal enableextensions
rem set errorlevel=0

for /f "usebackq tokens=*" %%i in (`..\packages\vswhere.2.2.7\tools\vswhere -latest -products * -requires Microsoft.Component.MSBuild -property installationPath`) do (
  set MSBuildDir=%%i\MSBuild\15.0\Bin\
)

mkdir ..\nuget
erase /s ..\nuget\Microsoft.Recognizers.Text*.nupkg
"%MSBuildDir%\MSBuild\15.0\Bin\MSBuild.exe" /property:Configuration=release Microsoft.Recognizers.Text.csproj
for /f %%v in ('powershell -noprofile "(Get-Command .\bin\release\Microsoft.Recognizers.Text.dll).FileVersionInfo.FileVersion"') do set basicVersion=%%v
..\packages\NuGet.CommandLine.4.3.0\tools\NuGet.exe pack Microsoft.Recognizers.Text.nuspec -symbols -properties version=%basicVersion% -OutputDirectory ..\nuget
echo *** Finished building Microsoft.Recognizers.Text