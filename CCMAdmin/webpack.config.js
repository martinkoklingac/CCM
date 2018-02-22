/// <binding BeforeBuild='Run - Production, Run - Development' Clean='Run - Development' />
"use strict";

module.exports = {
    entry: "./wwwroot/js/main.js",
    output: {
        filename: "./wwwroot/js/out.js"
    },
    //devServer: {
    //    contentBase: ".",
    //    host: "localhost",
    //    port: 9000
    //},
    module: {
        loaders: [
            {
                test: /\.js?$/,
                loader: "babel-loader",
                exclude: /node_modules/
            },
            {
                test: /\.vue?$/,
                loader: "vue-loader",
                exclude: /node_modules/
            }
        ]
    },
    resolve: {
        alias: {
            vue: 'vue/dist/vue.js'
        }
    }
};