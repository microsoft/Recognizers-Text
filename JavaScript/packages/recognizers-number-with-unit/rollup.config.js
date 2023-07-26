// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

import resolve from 'rollup-plugin-node-resolve';
import commonjs from 'rollup-plugin-commonjs';
import sourceMaps from 'rollup-plugin-sourcemaps';
import pkg from './package.json';
import alias from 'rollup-plugin-alias';
import path from 'path';
const lodash = require("lodash");

export default {
  input: `compiled/recognizers-text-number-with-unit.js`,
  output: [
    { file: pkg.module, format: 'es' },
    { file: pkg.main, name: lodash.camelCase(pkg.name), format: 'umd', exports: 'named'  },
    { file: pkg.browser, format: 'iife', name: lodash.camelCase(pkg.name), exports: 'named' }
  ],
  exports: 'named',
  sourcemap: true,
  plugins: [
    alias({
      '@microsoft/recognizers-text': path.resolve(__dirname, '../recognizers-text/compiled/recognizers-text.js'),
      '@microsoft/recognizers-text-number': path.resolve(__dirname, '../recognizers-number/compiled/recognizers-text-number.js')
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
