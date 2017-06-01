@echo off
pushd Microsoft.Recognizers.Text.Number
call CreatePackage.cmd
popd
pushd Microsoft.Recognizers.Text.NumberWithUnit
call CreatePackage.cmd
popd
pushd Microsoft.Recognizers.Text.DateTime
call CreatePackage.cmd
popd
