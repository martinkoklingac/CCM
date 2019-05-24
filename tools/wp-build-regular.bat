@ECHO OFF

setlocal

cd ..
cd CcmClient

set webpack_path=node_modules\.bin

%webpack_path%\webpack --mode development