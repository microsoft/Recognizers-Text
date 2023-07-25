// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

import pkg from './package.json';
import resolve from 'rollup-plugin-node-resolve';
import commonjs from 'rollup-plugin-commonjs';
import sourceMaps from 'rollup-plugin-sourcemaps';
import alias from 'rollup-plugin-alias';
import path from 'path';
const lodash = require("lodash");

export default {
  input: `compiled/recognizers-text-suite.js`,
  output: [
    { file: pkg.module, format: 'es' },
    { file: pkg.main, name: lodash.camelCase(pkg.name), format: 'umd', exports: 'named'  },
    { file: pkg.browser, format: 'iife', name: 'RecognizersText', exports: 'named' }
  ],
  exports: 'named',
  sourcemap: true,
  plugins: [
    alias({
      '@microsoft/recognizers-text': path.resolve(__dirname, '../recognizers-text/compiled/recognizers-text.js'),
      '@microsoft/recognizers-text-choice': path.resolve(__dirname, '../recognizers-choice/compiled/recognizers-text-choice.js'),
      '@microsoft/recognizers-text-number': path.resolve(__dirname, '../recognizers-number/compiled/recognizers-text-number.js'),
      '@microsoft/recognizers-text-number-with-unit': path.resolve(__dirname, '../recognizers-number-with-unit/compiled/recognizers-text-number-with-unit.js'),
      '@microsoft/recognizers-text-date-time': path.resolve(__dirname, '../recognizers-date-time/compiled/recognizers-text-date-time.js'),
      '@microsoft/recognizers-text-sequence': path.resolve(__dirname, '../recognizers-sequence/compiled/recognizers-text-sequence.js'),
    }),
    // Allow bundling cjs modules (unlike webpack, rollup doesn't understand cjs)
    commonjs(),
    // Allow node_modules resolution, so you can use 'external' to control
    // which external modules to include in the bundle
    // https://github.com/rollup/rollup-plugin-node-resolve#usage
    resolve(),

    // Resolve source maps to the original source
    sourceMaps()
  ]
};
