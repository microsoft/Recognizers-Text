@echo off
echo *** Building Microsoft.Recognizers.Suite
setlocal
setlocal enabledelayedexpansion
setlocal enableextensions

if not exist ..\dist mkdir ..\dist
if exist ..\dist\Microsoft.Recognizers.Suite*.whl erase /s ..\dist\Microsoft.Recognizers.Suite*.whl
python setup.py bdist_wheel -d ..\dist\

set error=%errorlevel%
set packageName=Microsoft.Recognizers.Suite
if %error% NEQ 0 (
	echo *** Failed to build %packageName%
	exit /b %error%
) else (
	echo *** Succeeded to build %packageName%
)