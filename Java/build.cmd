@ECHO off

ECHO.
ECHO # Java environment info
CALL mvn -v

ECHO.
ECHO # Building Java platform
mvn package