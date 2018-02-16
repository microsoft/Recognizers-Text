# BUILD: docker build -t recognizers-text .
# RUN: docker run -d -p 0.0.0.0:8000:8000 --name recognizers-text-browser 

FROM node:latest

EXPOSE 8000

VOLUME .:/usr/src/

#user is docker with password docker, has access to sudo
RUN useradd -m docker && \
    echo "docker:docker" | chpasswd && \
    adduser docker sudo 

WORKDIR /usr/src/

RUN npm install -g ts-node && \
    npm install -g typescript && \
  git clone https://github.com/Microsoft/Recognizers-Text && \
  cd Recognizers-Text\Javascript && \
  npm run-script build && \
  npm run build-resources && \
  cd samples\browser
  
CMD ["npm", "start"]
