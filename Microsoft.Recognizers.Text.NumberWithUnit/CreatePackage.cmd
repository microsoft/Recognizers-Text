@echo off
echo *** Building Microsoft.Recognizers.Text.NumberWithUnit
setlocal
setlocal enabledelayedexpansion
setlocal enableextensions
set errorlevel=0
mkdir ..\nuget
erase /s ..\nuget\Microsoft.Recognizers.Text.NumberWithUnit*nupkg
msbuild /property:Configuration=release Microsoft.Recognizers.Text.NumberWithUnit.csproj
for /f %%v in ('powershell -noprofile "(Get-Command .\bin\release\Microsoft.Recognizers.Text.Number.dll).FileVersionInfo.FileVersion"') do set number=%%v
for /f %%v in ('powershell -noprofile "(Get-Command .\bin\release\Microsoft.Recognizers.Text.NumberWithUnit.dll).FileVersionInfo.FileVersion"') do set version=%%v
..\packages\NuGet.CommandLine.3.5.0\tools\NuGet.exe pack Microsoft.Recognizers.Text.NumberWithUnit.nuspec -symbols -properties version=%version%;number=%number% -OutputDirectory ..\nuget
echo *** Finished building Microsoft.Recognizers.Text.NumberWithUnit
