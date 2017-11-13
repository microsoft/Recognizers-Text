// web server to host browser specs
const fs = require('fs');
const path = require('path');
const express = require('express');
const specs = require('../specs');

const app = express();


// retrieve specs
app.get('/specs.js', function (req, res) {
  res.send("var specs = " + JSON.stringify(specs.readAll(), null, '\t'));
});

app.use(express.static(__dirname));
app.use("/scripts", express.static(path.join(__dirname, '../../packages/')));

app.listen(8001, function () {
  console.log('Browser Sample listening on port 8001!');
});


// helpers
function getSpecFilePaths(specsPath) {
  if (!fs.existsSync(specsPath)) {
    throw new Error(`Specs directory not found at ${path.resolve(specsPath)}`);
  }

  return fs
    .readdirSync(specsPath).map(s => path.join(specsPath, s))
    .filter(p => fs.lstatSync(p).isDirectory())
    .map(s1 => fs.readdirSync(s1).map(s2 => path.join(s1, s2)))
    .reduce((a, b) => a.concat(b), [])                      // flatten
    .filter(p => fs.lstatSync(p).isDirectory())
    .map(s1 => fs.readdirSync(s1).map(s2 => path.join(s1, s2)))
    .reduce((a, b) => a.concat(b), [])                      // flatten
    .filter(s => s.indexOf('.json') !== -1);
}

function getSuiteConfig(jsonPath) {
  var parts = jsonPath.split(path.sep).slice(-3);

  return {
    type: parts[0],
    subType: parts[2].split('.json')[0],
    language: parts[1]
  };
}