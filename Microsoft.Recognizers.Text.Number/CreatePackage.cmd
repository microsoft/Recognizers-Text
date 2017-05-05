@echo off
echo *** Building Microsoft.Recognizers.Text.Number
setlocal
setlocal enabledelayedexpansion
setlocal enableextensions
set errorlevel=0
mkdir ..\nuget
erase /s ..\nuget\Microsoft.Recognizers.Text.Number*.nupkg
msbuild /property:Configuration=release Microsoft.Recognizers.Text.Number.csproj
for /f %%v in ('powershell -noprofile "(Get-Command .\bin\release\Microsoft.Recognizers.Text.Number.dll).FileVersionInfo.FileVersion"') do set numberVersion=%%v
..\packages\NuGet.CommandLine.3.5.0\tools\NuGet.exe pack Microsoft.Recognizers.Text.Number.nuspec -symbols -properties version=%numberVersion% -OutputDirectory ..\nuget
echo *** Finished building Microsoft.Recognizers.Text.Number