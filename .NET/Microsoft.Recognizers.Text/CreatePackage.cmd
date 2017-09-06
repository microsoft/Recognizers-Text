@echo off
echo *** Building Microsoft.Recognizers.Text
setlocal
setlocal enabledelayedexpansion
setlocal enableextensions
set errorlevel=0
mkdir ..\nuget
erase /s ..\nuget\Microsoft.Recognizers.Text*.nupkg
msbuild /property:Configuration=release Microsoft.Recognizers.Text.csproj
for /f %%v in ('powershell -noprofile "(Get-Command .\bin\release\Microsoft.Recognizers.Text.dll).FileVersionInfo.FileVersion"') do set basicVersion=%%v
..\packages\NuGet.CommandLine.3.5.0\tools\NuGet.exe pack Microsoft.Recognizers.Text.nuspec -symbols -properties version=%basicVersion% -OutputDirectory ..\nuget
echo *** Finished building Microsoft.Recognizers.Text