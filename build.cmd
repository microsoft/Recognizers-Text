@ECHO off

PUSHD .NET
CALL build.cmd
POPD

PUSHD JavaScript
CALL build.cmd
POPD