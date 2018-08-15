@echo off
set MAVEN_OPTS=-Dfile.encoding=utf-8
mvn compile exec:java -pl libraries/resource-generator/
