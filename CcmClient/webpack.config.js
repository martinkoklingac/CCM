/// <binding ProjectOpened='Watch - Development' />

//production "webpack -p --env.production"

const webpack = require('webpack');
const path = require('path');
const glob = require('glob');
const CleanWebpackPlugin = require('clean-webpack-plugin');
const ExtractTextPlugin = require('extract-text-webpack-plugin');

//the plug in in now required in the vue version 15.x
const { VueLoaderPlugin } = require('vue-loader')

module.exports = env => {

    const isProduction = env && env.production ? true : false;
    console.log('Production: ', isProduction);

    const extractCSS = new ExtractTextPlugin('/[name]/[name].css');

    return {

        //this goes through each file in the ppp/entrypoints and patientportalentry points and builds up the entry config so we don't have to have a long list of files.
        entry: glob.sync('./ClientApp/*/entrypoints/**.js')
            .reduce(function (obj, el) {

                var parentDirectory = path.parse(el).dir.replace('./ClientApp/', '').replace('entrypoints', '').replace('/', '');

                //console.log(parentDirectory);
                obj[parentDirectory + '_' + path.parse(el).name] = el;

                return obj;
            }, {}),

        //this is the long hand version of whats above
        //entry: {
        //    'ppp_Home': "./ClientApp/ppp/entrypoints/home.entry.js",
        //    'ppp_Main': "./ClientApp/ppp/entrypoints/main.entry.js"
        //},

        //this declares where the entry points above get saved to on webpack build
        output: {
            publicPath: "/js/",
            path: path.join(__dirname, '/wwwroot/build'),
            filename: '[name]/[name].build.js'
        },

        //this goes through all the imports and if it is used more the 'minChunks' amount of times then it will be put into the shared bundle. This way it doesn't get outputted in the entry point (reducing the page script size)
        //ie: import a common service that is used on 10 pages. The 10 pages would have this service in it's script. Instead, the service will be put into the shared bundle.
        //optimization: {
        //    splitChunks: {
        //        cacheGroups: {
        //            commons: {
        //                name: 'Shared',
        //                chunks: "initial",
        //                minChunks: 2,
        //                maxInitialRequests: 5, // The default limit is too small to showcase the effect
        //                minSize: 0 // This is example is too small to create commons chunks
        //            },
        //        }
        //    }
        //},

        //anytime you see 'import Vue' then grab the esm which only contains the runtime. The regular vue file contains the compiler and run time which increases the payload
        resolve: {
            alias: {
                'vue$': 'vue/dist/vue.esm.js',
            }
        },

        //output source maps in dev so we can debug javascript
        devtool: isProduction ? 'none' : 'eval',

        plugins: [

            //needed to load .vue componenets
            new VueLoaderPlugin(),

            //cleans the build folder so we dont have legacy scripts that have been removed from the solution. 
            new CleanWebpackPlugin([path.join(__dirname, '/wwwroot/build/ClientApp/**')]),

            //outputs the css to a text file so we can manually add it a page. This way the browser caches the file
            extractCSS,

            //holds a reference to the vendor bundle. This way if we import from bootstrap or vue then it won't be included in the entry point file
            new webpack.DllReferencePlugin({
                context: __dirname,
                manifest: require('./wwwroot/BuildFramework/vendor-manifest.json')
            })

        ],
        module: {

            rules: [
                //.json file loaders (ie localization)
                {
                    test: /\.json$/,
                    loader: 'json-loader',
                    type: 'javascript/auto'
                },
                //sass to css conversion
                {
                    test: /\.(s*)css(\?|$)/,
                    use: extractCSS.extract({ use: !isProduction ? 'css-loader!sass-loader' : 'css-loader!sass-loader?minimize' })
                },
                //compile es6 down to es5 for older browser
                {
                    test: /\.js$/,
                    exclude: /(node_modules|bower_components)/,
                    use: {
                        loader: 'babel-loader',
                        options: {
                            presets: ['babel-preset-env']
                        }
                    }
                },
                //loader for vue
                {
                    test: /\.vue$/,
                    loader: 'vue-loader'
                }
            ]
        }
    };
};
