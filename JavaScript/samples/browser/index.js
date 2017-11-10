const express = require('express')
const app = express()

app.use(express.static('./'))
app.use("/scripts", express.static('../../packages/recognizers-text/dist/'))

app.listen(8000, function () {
  console.log('Browser Sample listening on port 8000!')
})