@ECHO off

ECHO.
ECHO # Java environment info
CALL mvn -v

ECHO # Generate resources
set MAVEN_OPTS=-Dfile.encoding=utf-8
mvn compile exec:java -pl libraries/resource-generator/

ECHO.
ECHO # Building Java platform
mvn package