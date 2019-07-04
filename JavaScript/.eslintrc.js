module.exports = {
    parser: '@typescript-eslint/parser',  // Specifies the ESLint parser
    plugins: [
        '@typescript-eslint',
        'only-warn'
    ],
    extends: [
        'plugin:@typescript-eslint/recommended',
        'prettier',
    ],
    parserOptions: {
        ecmaVersion: 2018,  // Allows for the parsing of modern ECMAScript features
        sourceType: 'module',  // Allows for the use of imports
        ecmaFeatures: {
            impliedStrict: true
        },
    },
    rules: {
    },
}; 
