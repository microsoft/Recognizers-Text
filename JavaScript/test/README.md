# JavaScript Tests

In order to verify the correct behavior of the JavaScript Recognizers, the same [Specs suite](../../Specs) is shared between platforms.

The JavaScript test runner is implemented as a custom test reader for the specs and the execution is then derived to the proper Test Runner, based on the environment: [AVA](https://github.com/avajs/ava) for Node and [Mocha.js](https://mochajs.org) on the Browser.

## Running the Specs in Node

Specs can be run for Node by invoking AVA (if installed globally) or executing the `test` NPM task:

If AVA is installed globally:
- From the [JavaScript](../) directory, execute `ava`

Or use NPM instead:
- From the [JavaScript](../) directory, execute `npm run test`

> NOTE: Both require to have previously built the packages ([build.cmd](../build.cmd))

## Running the Specs in a Browser

All mayor browsers are supported and IE11 is also supported by [adding the core-js polyfill](../samples/browser/index.html#L9-L13) to your page's scripts.

Running the specs on the browser is also supported. The Test Runner can be ran in the Browser using Mocha.js instead of AVA. In order to do so, the following commands need to be executed:

- From the [JavaScript](../) directory, execute `npm run browser-test`

Then open a browser and navigate to [http://localhost:8001/](http://localhost:8001/).