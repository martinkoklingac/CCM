//webpack --config webpack.config.vendor.js --mode development
//webpack --config webpack.config.vendor.js --env.production=true -p --mode production

const webpack = require('webpack');
const CleanWebpackPlugin = require('clean-webpack-plugin');
const path = require('path');
const ExtractTextPlugin = require('extract-text-webpack-plugin');

module.exports = env => {

    const isProduction = env && env.production ? true : false;
    console.log('Production: ', isProduction);

    const extractCSS = new ExtractTextPlugin('../wwwroot/BuildFramework/vendor.css');

    return {

        //contain all the vendor files
        entry: {
            Vendor: [
                'jquery',
                //'jquery-validation',
                //'jquery-validation-unobtrusive',
                'bootstrap',
                'bootstrap/dist/css/bootstrap.css',
                //'@fortawesome/fontawesome',
                //'@fortawesome/fontawesome-free-regular',
                //'@fortawesome/fontawesome-free-solid',
                //'select2',
                //'select2/dist/css/select2.css',
                'vue/dist/vue.esm.js',
                //'datatables.net/js/jquery.dataTables.js',
                //'datatables.net-bs4/js/dataTables.bootstrap4.js',
                //'datatables.net-bs4/css/dataTables.bootstrap4.css',
                //'moment',
                //'./ClientApp/SharedServices/linq.javascript.service.js',
                //'./LocalizationResources/localization.shared.resource.json'
            ]
            // .concat(isProduction ? './wwwroot/css/site.css' : [])
        },

        //where does the vendor bundle get built
        output: {
            filename: '../wwwroot/buildframework/vendor.bundle.js', //webpack4 screws up the path...so i have to do "../"
            library: 'vendor_lib',
        },
        module: {
            rules: [
                //.json file loaders (ie localization)
                {
                    test: /\.json$/,
                    loader: 'json-loader',
                    type: 'javascript/auto'
                },
                //css for bootstrap (or any css or sass) import
                {
                    test: /\.(s*)css(\?|$)/,
                    use: extractCSS.extract({ use: !isProduction ? 'css-loader!sass-loader' : 'css-loader!sass-loader?minimize' })
                },

                //font awesome. This way it gets outputted a vendor file
                {
                    test: /.(ttf|otf|eot|svg|woff(2)?)(\?[a-z0-9]+)?$/,
                    use: [{
                        loader: 'file-loader',
                        options: {
                            name: '[name].[ext]',
                            outputPath: '../wwwroot/buildframework/',    // where the fonts will go
                            publicPath: '/buildframework/Fonts/',
                            useRelativePath: true
                        }
                    }]
                },

                //incase we load any images through javascript / vue
                {
                    test: /\.(png|jpg|gif)$/,
                    use: [{
                        loader: 'url-loader'
                    }]
                },

                //allow jquery to be on the global scope so we don't need to import it on every screen. 
                //anywhere a import see's import '$' then it will automatically be brought in
                {
                    test: require.resolve('jquery'),
                    use: [{
                        loader: 'expose-loader',
                        options: 'jQuery'
                    },
                    {
                        loader: 'expose-loader',
                        options: '$'
                    }]
                }
            ]
        },
        plugins: [

            extractCSS,

            //this just cleans up the output folder so we don't have left over items that shouldn't be there.
            new CleanWebpackPlugin([path.join(__dirname, '/wwwroot/buildframework/*.*')]), //removes all files in this directory. Or you can do '/wwwroot/Build/**.js' if you want to delete just the js files

            new webpack.DllPlugin({
                name: 'vendor_lib',
                path: './wwwroot/buildframework/vendor-manifest.json',
            }),

            new webpack.ProvidePlugin({ $: 'jquery', jQuery: 'jquery' }),

        ]
    }
};