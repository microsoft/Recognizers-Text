git rev-parse HEAD > commitId.txt

set /p commitId=<commitId.txt

set username=%1
set token=%2
set owner=%3
set repo=%4
set status=%5
set target_url=%6
set description=%7

curl -u %username%:%token% https://api.github.com/repos/%owner%/%repo%/statuses/%commitId% -d '{
  "state": "%state%",
  "target_url": "%target_url%",
  "description": "%description%",
  "context": "continuous-integration/vso"
}'