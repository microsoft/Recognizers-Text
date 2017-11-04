echo // Building Javacript platform

echo // Building recognizers number module
npm run prebuild-number

echo // Building recognizers number-with-unit module
npm run prebuild-number-with-unit

echo // Building recognizers date-time module
npm run prebuild-date-time

echo // Installing dependencies - npm install
npm i

echo // Building - npm run build
npm run build

echo // Running test - npm run test
npm run test