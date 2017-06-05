@echo off
echo *** Building Microsoft.Recognizers.Text.DateTime
setlocal
setlocal enabledelayedexpansion
setlocal enableextensions
set errorlevel=0
mkdir ..\nuget
erase /s ..\nuget\Microsoft.Recognizers.Text.DateTime*nupkg
msbuild /property:Configuration=release Microsoft.Recognizers.Text.DateTime.csproj
for /f %%v in ('powershell -noprofile "(Get-Command .\bin\release\Microsoft.Recognizers.Text.Number.dll).FileVersionInfo.FileVersion"') do set number=%%v
for /f %%v in ('powershell -noprofile "(Get-Command .\bin\release\Microsoft.Recognizers.Text.NumberWithUnit.dll).FileVersionInfo.FileVersion"') do set numberWithUnit=%%v
for /f %%v in ('powershell -noprofile "(Get-Command .\bin\release\Microsoft.Recognizers.Text.DateTime.dll).FileVersionInfo.FileVersion"') do set version=%%v
..\packages\NuGet.CommandLine.3.5.0\tools\NuGet.exe pack Microsoft.Recognizers.Text.DateTime.nuspec -symbols -properties version=%version%;number=%number%;numberWithUnit=%numberWithUnit% -OutputDirectory ..\nuget
echo *** Finished building Microsoft.Recognizers.Text.DateTime
