## Use this container to download, and build recognizers source project
## in Nodejs without having to install the prerequisite software. This 
## container doesn't depend on the underlying host's copy of the repository. 
## 
## Docker Image Name:      recognizers-text
## Docker Container Name:  recognizers-text-browser
##
## Once the container is running, use the URL on the host
## (http://localhost:8000/) to query the recognizers at ./index.html.
##
## The container user and password are both "docker"
## Log on interactively to use other Nodejs samples


## Step 1: BUILD & RUN: 
#> docker build -t recognizers-text . && docker run -it -p 0.0.0.0:8000:8000 --name recognizers-text-browser recognizers-text

## Step 2: Wait until you see: 
#> Browser Sample listening on port 8000!

## Step 3: Open browser to http://localhost:8000/

FROM node:latest

#user is docker with password docker, has access to sudo
RUN useradd -m docker && \
    echo "docker:docker" | chpasswd && \
    adduser docker sudo 

WORKDIR /usr/src/

RUN git clone https://github.com/Microsoft/Recognizers-Text && \
  echo "***Clone complete" && \
  cd Recognizers-Text/JavaScript && \
  npm install && \
  echo "***Install complete" && \
  npm run-script build && \
  echo "***Build complete"
  
WORKDIR /usr/src/Recognizers-Text/JavaScript/samples/browser

CMD ["npm", "start"]
