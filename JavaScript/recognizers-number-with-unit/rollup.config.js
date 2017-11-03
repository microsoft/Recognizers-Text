import resolve from 'rollup-plugin-node-resolve';
import commonjs from 'rollup-plugin-commonjs';
import sourceMaps from 'rollup-plugin-sourcemaps';
import pkg from './package.json';
import camelCase from 'lodash.camelcase';
import babel from 'rollup-plugin-babel';
import alias from 'rollup-plugin-alias';
import path from 'path';

export default {
  input: `compiled/${pkg.name}.js`,
  output: [
    { file: pkg.module, format: 'es' },
    { file: pkg.main, name: camelCase(pkg.name), format: 'umd', exports: 'named'  },
    { file: pkg.browser, format: 'iife', name: camelCase(pkg.name), exports: 'named' }
  ],
  exports: 'named',
  sourcemap: true,
  plugins: [
    alias({
      'recognizers-text-number': path.resolve(__dirname, '../recognizers-number/compiled/recognizers-text-number.js')
    }),
    // Allow bundling cjs modules (unlike webpack, rollup doesn't understand cjs)
    commonjs(),
    // Allow node_modules resolution, so you can use 'external' to control
    // which external modules to include in the bundle
    // https://github.com/rollup/rollup-plugin-node-resolve#usage
    resolve(),

    // Resolve source maps to the original source
    sourceMaps(),

    babel({
      exclude: 'node_modules/**'
    })
  ]
};
