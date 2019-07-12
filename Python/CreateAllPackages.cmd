rem Install wheel package
call pip install wheel

rem Go through Sub-packages
pushd .\libraries\recognizers-text\
call CreatePackage.cmd
popd
pushd .\libraries\datatypes-timex-expression\
call CreatePackage.cmd
popd
pushd .\libraries\recognizers-number\
call CreatePackage.cmd
popd
pushd .\libraries\recognizers-number-with-unit\
call CreatePackage.cmd
popd
pushd .\libraries\recognizers-date-time\
call CreatePackage.cmd
popd
pushd .\libraries\recognizers-sequence\
call CreatePackage.cmd
popd
pushd .\libraries\recognizers-suite\
call CreatePackage.cmd
popd
rem Exit .Python dir
popd

GOTO END

:NOSPECDIR
echo "Please specify a directory where the package directories containing .cmd scripts should be found. Abort."

GOTO BADEND

:NODIR
echo "Dir %dir% not found. Abort."

rem exit 1
GOTO BADEND

:BADEND
exit /b 1

GOTO END

:END