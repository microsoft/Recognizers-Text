@ECHO off

REM This is only used for local build. It executes the same build and test scripts than the CI server.

build.ci.cmd && tests.ci.cmd