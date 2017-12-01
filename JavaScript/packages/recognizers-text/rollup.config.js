import resolve from 'rollup-plugin-node-resolve';
import commonjs from 'rollup-plugin-commonjs';
import sourceMaps from 'rollup-plugin-sourcemaps';
import pkg from './package.json';
import camelCase from 'lodash.camelcase';

export default {
  input: `compiled/recognizers-text.js`,
  output: [
    { file: pkg.module, format: 'es' },
    { file: pkg.main, name: camelCase(pkg.name), format: 'umd', exports: 'named'  },
    { file: pkg.browser, format: 'iife', name: camelCase(pkg.name), exports: 'named' }
  ],
  exports: 'named',
  sourcemap: true,
  plugins: [
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
